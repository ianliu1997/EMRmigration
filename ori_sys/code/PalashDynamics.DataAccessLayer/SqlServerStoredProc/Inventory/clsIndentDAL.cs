using System;
using System.Collections.Generic;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory.Indent;
using System.Data.SqlClient;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsIndentDAL : clsBaseIndentDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIndentDAL()
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


        public override IValueObject AddIndent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddIndentBizActionVO BizActionObj = valueObject as clsAddIndentBizActionVO;


            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsIndentMasterVO objIndent = BizActionObj.objIndent;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIndent");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objIndent.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objIndent.Time);
                dbServer.AddInParameter(command, "IndentNumber", DbType.String, objIndent.IndentNumber);
                dbServer.AddInParameter(command, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                dbServer.AddInParameter(command, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                dbServer.AddInParameter(command, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                dbServer.AddInParameter(command, "DueDate", DbType.DateTime, objIndent.DueDate);
                dbServer.AddInParameter(command, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);

                dbServer.AddInParameter(command, "Remark", DbType.String, objIndent.Remark);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                dbServer.AddInParameter(command, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                dbServer.AddInParameter(command, "IsApproved", DbType.Int32, objIndent.IsApproved);

                dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objIndent.IsIndent);

                if (BizActionObj.IsConvertToPR == true)
                {
                    dbServer.AddInParameter(command, "ConvertToPRID", DbType.Int64, objIndent.ConvertToPRID);
                    dbServer.AddInParameter(command, "IsConvertToPR", DbType.Boolean, BizActionObj.IsConvertToPR);

                    dbServer.AddInParameter(command, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                    dbServer.AddInParameter(command, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                    dbServer.AddInParameter(command, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                }
                else
                {
                    dbServer.AddInParameter(command, "IsAuthorized", DbType.Boolean, 0);  //objIndent.IsAuthorized
                    dbServer.AddInParameter(command, "AuthorizedByID", DbType.Int64, 0);   //objIndent.AuthorizedByID
                    dbServer.AddInParameter(command, "AuthorizationDate", DbType.DateTime, null); //objIndent.AuthorizationDate
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objIndent.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //if (objIndent.AddedOn != null) objIndent.AddedOn = objIndent.AddedOn.Trim();
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //if (objIndent.AddedWindowsLoginName != null) objIndent.AddedWindowsLoginName = objIndent.AddedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objIndent.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objIndent.PatientUnitID);
                dbServer.AddInParameter(command, "IsAgainstPatient", DbType.Boolean, objIndent.IsAgainstPatient);

                //  dbServer.AddParameter(command, "IndentNumber", DbType.String,ParameterDirection.InputOutput,null,DataRowVersion.Default,"");
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objIndent.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.objIndent.ID = (long?)dbServer.GetParameterValue(command, "ID");
                BizActionObj.objIndent.IndentNumber = (string)dbServer.GetParameterValue(command, "Number");

                foreach (var item in BizActionObj.objIndentDetailList)
                {
                    item.IndentID = BizActionObj.objIndent.ID;
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddIndentDetail");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objIndent.LinkServer);
                    if (objIndent.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "IndentID", DbType.Int64, item.IndentID);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);

                    //***Conversion Factor
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.SingleQuantity);    //TransactionQuantity
                    dbServer.AddInParameter(command1, "UOM", DbType.Int64, item.SelectedUOM.ID);     //TransactionUOMID
                    dbServer.AddInParameter(command1, "SUOM", DbType.Int64, item.SUOMID);   // StockUOMID
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.StockCF);
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, item.StockingQuantity);

                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "ConversionFactor", DbType.Single, item.ConversionFactor);  // BaseCF
                    dbServer.AddInParameter(command1, "RequiredQuantity", DbType.Double, item.RequiredQuantity); // BaseQuantity
                    //***

                    dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.RequiredQuantity);

                    dbServer.AddInParameter(command1, "PurchaseOrderQuantity", DbType.Double, item.PurchaseOrderQuantity);
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "IndentUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                    dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                    dbServer.AddInParameter(command1, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                    dbServer.AddInParameter(command1, "IsForwarded", DbType.Boolean, 1);
                    dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, 1);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objIndent = null;
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

        public override IValueObject UpdateIndent(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentBizActionVO BizActionObj = valueObject as clsUpdateIndentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsIndentMasterVO objIndent = BizActionObj.objIndent;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIndent");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objIndent.Date);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objIndent.UnitID);

                dbServer.AddInParameter(command, "Time", DbType.DateTime, objIndent.Time);
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objIndent.ID);
                dbServer.AddInParameter(command, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                dbServer.AddInParameter(command, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                dbServer.AddInParameter(command, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                dbServer.AddInParameter(command, "DueDate", DbType.DateTime, objIndent.DueDate);

                dbServer.AddInParameter(command, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);
                dbServer.AddInParameter(command, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                dbServer.AddInParameter(command, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID); //UserVo.ID
                dbServer.AddInParameter(command, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);

                dbServer.AddInParameter(command, "Remark", DbType.String, objIndent.Remark);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                dbServer.AddInParameter(command, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                dbServer.AddInParameter(command, "IsApproved", DbType.Int32, objIndent.IsApproved);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objIndent.Status);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                //if (objIndent.AddedOn != null) objIndent.AddedOn = objIndent.AddedOn.Trim();
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //if (objIndent.AddedWindowsLoginName != null) objIndent.AddedWindowsLoginName = objIndent.AddedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (objIndent.IsChangeAndApprove == true)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteIndentDetails");
                    command2.Connection = con;

                    dbServer.AddInParameter(command2, "IndentID", DbType.Int64, objIndent.ID);
                    //dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                    dbServer.AddInParameter(command2, "IsForChangeAndApprove", DbType.Boolean, 0);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                }

                if (objIndent.IsForwarded == false && objIndent.IsApproved == false)
                {
                    foreach (var item in BizActionObj.objIndent.IndentDetailsList)
                    {
                        item.IndentID = BizActionObj.objIndent.ID;
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddIndentDetail");
                        command1.Connection = con;

                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objIndent.LinkServer);
                        if (objIndent.LinkServer != null)
                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                        dbServer.AddInParameter(command1, "IndentID", DbType.Int64, item.IndentID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);

                        //***Conversion Factor
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.SingleQuantity);  // TransactionQuantity
                        dbServer.AddInParameter(command1, "UOM", DbType.Int64, item.SelectedUOM.ID);    //TransactionUOMID
                        dbServer.AddInParameter(command1, "SUOM", DbType.Int64, item.SUOMID); // StockUOMID
                        dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.StockCF);
                        dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, item.StockingQuantity);

                        dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                        dbServer.AddInParameter(command1, "ConversionFactor", DbType.Single, item.ConversionFactor); // BaseCF
                        dbServer.AddInParameter(command1, "RequiredQuantity", DbType.Double, item.RequiredQuantity); // BaseQuantity
                        //***
                        dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.RequiredQuantity);
                        dbServer.AddInParameter(command1, "PurchaseOrderQuantity", DbType.Double, item.PurchaseOrderQuantity);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objIndent.UnitID);
                        dbServer.AddInParameter(command1, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                        //By Anjali.........................................................................
                        //dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                        dbServer.AddInParameter(command1, "IsIndent", DbType.Int32, objIndent.IsIndent);
                        dbServer.AddInParameter(command1, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                        //...................................................................................
                        dbServer.AddInParameter(command1, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                        if (objIndent.IsChangeAndApprove == true)
                        {
                            dbServer.AddInParameter(command1, "IsChangeAndApprove", DbType.Boolean, objIndent.IsChangeAndApprove);
                            if (objIndent.IsIndent == 1)//objIndent.IsIndent == true      //By Anjali...................................
                                dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, 1);
                        }
                        else
                        {
                            dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, objIndent.IsApproved);
                        }
                        //  dbServer.AddInParameter(command1, "IsModify", DbType.Boolean, objIndent.IsModify);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objIndent = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;
        }

        public override IValueObject UpdateIndentForChangeAndApprove(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentBizActionVO BizActionObj = valueObject as clsUpdateIndentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsIndentMasterVO objIndent = BizActionObj.objIndent;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIndent");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objIndent.LinkServer);
                if (objIndent.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objIndent.Date);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objIndent.UnitID);

                dbServer.AddInParameter(command, "Time", DbType.DateTime, objIndent.Time);
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objIndent.ID);
                dbServer.AddInParameter(command, "TransactionMovementID", DbType.Int64, objIndent.TransactionMovementID);
                dbServer.AddInParameter(command, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                dbServer.AddInParameter(command, "ToStoreID", DbType.Int64, objIndent.ToStoreID);
                dbServer.AddInParameter(command, "DueDate", DbType.DateTime, objIndent.DueDate);

                dbServer.AddInParameter(command, "IndentCreatedByID", DbType.Int64, objIndent.IndentCreatedByID);
                dbServer.AddInParameter(command, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                dbServer.AddInParameter(command, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);  //UserVo.ID
                dbServer.AddInParameter(command, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);

                dbServer.AddInParameter(command, "Remark", DbType.String, objIndent.Remark);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, objIndent.IndentStatus);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objIndent.IsFreezed);
                dbServer.AddInParameter(command, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                if (objIndent.IsChangeAndApprove == true)
                {
                    dbServer.AddInParameter(command, "IsChangeAndApprove", DbType.Boolean, objIndent.IsChangeAndApprove);
                }
                dbServer.AddInParameter(command, "IsApproved", DbType.Int32, objIndent.IsApproved);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objIndent.Status);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                //if (objIndent.AddedOn != null) objIndent.AddedOn = objIndent.AddedOn.Trim();
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //if (objIndent.AddedWindowsLoginName != null) objIndent.AddedWindowsLoginName = objIndent.AddedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objIndent.IsIndent);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (objIndent.IsForwarded == false) //&& objIndent.IsApproved == false
                {
                    foreach (var item in BizActionObj.objIndent.IndentDetailsList)
                    {
                        item.IndentID = BizActionObj.objIndent.ID;
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_UpdateIndentDetail");
                        command1.Connection = con;

                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objIndent.LinkServer);
                        if (objIndent.LinkServer != null)
                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objIndent.LinkServer.Replace(@"\", "_"));

                        dbServer.AddInParameter(command1, "IndentID", DbType.Int64, item.IndentID);
                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);

                        //***Conversion Factor
                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.SingleQuantity); //TransactionQuantity
                        dbServer.AddInParameter(command1, "UOM", DbType.Int64, item.SelectedUOM.ID); //TransactionUOMID
                        dbServer.AddInParameter(command1, "SUOM", DbType.Int64, item.SUOMID); //StockUOMID
                        dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.StockCF);
                        dbServer.AddInParameter(command1, "ConversionFactor", DbType.Single, item.ConversionFactor);
                        dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, item.StockingQuantity);

                        dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                        dbServer.AddInParameter(command1, "RequiredQuantity", DbType.Double, item.RequiredQuantity); // BaseQuantity
                        //***

                        //dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.SingleQuantity);
                        //dbServer.AddInParameter(command1, "UOM", DbType.Int64, item.SelectedUOM.ID);  //item.UOM
                        //dbServer.AddInParameter(command1, "SUOM", DbType.Int64, item.SUOMID);
                        //dbServer.AddInParameter(command1, "ConversionFactor", DbType.Int64, item.ConversionFactor);

                        dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.RequiredQuantity);
                        dbServer.AddInParameter(command1, "PurchaseOrderQuantity", DbType.Double, item.PurchaseOrderQuantity);
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objIndent.UnitID);
                        dbServer.AddInParameter(command1, "IndentUnitID", DbType.Int64, objIndent.UnitID);
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, objIndent.InventoryIndentType);
                        dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, objIndent.IsIndent);
                        dbServer.AddInParameter(command1, "FromStoreID", DbType.Int64, objIndent.FromStoreID);
                        dbServer.AddInParameter(command1, "IsForwarded", DbType.Boolean, objIndent.IsForwarded);
                        //if (objIndent.IsChangeAndApprove == true)
                        //{
                        //    dbServer.AddInParameter(command1, "IsChangeAndApprove", DbType.Boolean, objIndent.IsChangeAndApprove);
                        //    if (objIndent.IsIndent == true)
                        //        dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, 1);
                        //}
                        //else
                        //{
                        //    dbServer.AddInParameter(command1, "IsApproved", DbType.Boolean, objIndent.IsApproved);
                        //}
                        //  dbServer.AddInParameter(command1, "IsModify", DbType.Boolean, objIndent.IsModify);
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    }
                }

                if (BizActionObj.objIndent.DeletedIndentDetailsList.Count > 0)
                {
                    foreach (var item in BizActionObj.objIndent.DeletedIndentDetailsList)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteIndentDetails");
                        command2.Connection = con;

                        dbServer.AddInParameter(command2, "IndentID", DbType.Int64, item.IndentID);
                        dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, item.IndentUnitID);
                        dbServer.AddInParameter(command2, "IsForChangeAndApprove", DbType.Boolean, 1);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objIndent = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;
        }



        public override IValueObject UpdateIndentOnlyForFreeze(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateIndentOnlyForFreezeBizActionVO BizActionObj = valueObject as clsUpdateIndentOnlyForFreezeBizActionVO;


            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIndentOnlyForFreeze");

                dbServer.AddInParameter(command, "IndentID", DbType.Int64, BizActionObj.IndentID);

                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
            }
            catch (Exception ex)
            {
                throw;
            }
            return valueObject;

        }


        public override IValueObject ApproveDirect(IValueObject valueObject, clsUserVO UserVo) //***//
        {
            clsUpdateIndentBizActionVO BizActionObj = valueObject as clsUpdateIndentBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsIndentMasterVO objIndent = BizActionObj.objIndent;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_ApproveDirect");
                command.Connection = con;

                dbServer.AddInParameter(command, "IssueId", DbType.Int64, objIndent.IssueId);
                dbServer.AddInParameter(command, "IssueUnitID", DbType.Int64, objIndent.IssueUnitID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objIndent.UnitID);
                dbServer.AddInParameter(command, "IsAuthorized", DbType.Boolean, objIndent.IsAuthorized);
                dbServer.AddInParameter(command, "AuthorizedByID", DbType.Int64, objIndent.AuthorizedByID);
                dbServer.AddInParameter(command, "AuthorizationDate", DbType.DateTime, objIndent.AuthorizationDate);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objIndent.IsApproved);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;
        }


        public override IValueObject GetIndentList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIndenListBizActionVO objBizAction = valueObject as clsGetIndenListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetStoreIndentList");

                if (objBizAction.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                if (objBizAction.ToDate != null)
                {
                    if (objBizAction.FromDate != null)
                    {
                        //if (objBizAction.FromDate.Equals(objBizAction.ToDate))
                        objBizAction.ToDate = objBizAction.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                }
                //dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                //dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "FromStoreID", DbType.Int64, objBizAction.FromStoreID);
                dbServer.AddInParameter(command, "ToStoreID", DbType.Int64, objBizAction.ToStoreID);

                dbServer.AddInParameter(command, "IndentNO", DbType.String, objBizAction.IndentNO);

                if (objBizAction.MRNo != null && objBizAction.MRNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + objBizAction.MRNo + "%");
                //else
                //    dbServer.AddInParameter(command, "MrNo", DbType.String, objBizAction.MRNo + "%");

                if (objBizAction.PatientName != null && objBizAction.PatientName.Length != 0)
                    dbServer.AddInParameter(command, "PatientName", DbType.String, objBizAction.PatientName + "%");

                dbServer.AddInParameter(command, "CheckStatusType", DbType.Boolean, objBizAction.CheckStatusType);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, objBizAction.IndentStatus);
                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objBizAction.isApproved);
                dbServer.AddInParameter(command, "IsForwarded", DbType.Boolean, objBizAction.isFrowrded);
                dbServer.AddInParameter(command, "IsCancelled", DbType.Boolean, objBizAction.isCancelled);
                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, objBizAction.IsFreezed);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);
                //By Anjali..................................... 
                dbServer.AddInParameter(command, "isIndent", DbType.Int32, objBizAction.isIndent);
                // dbServer.AddInParameter(command, "isIndent", DbType.Boolean, objBizAction.isIndent);
                //..................................................

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.IndentList == null)
                        objBizAction.IndentList = new List<clsIndentMasterVO>();
                    while (reader.Read())
                    {
                        clsIndentMasterVO objItem = new clsIndentMasterVO();
                        //objItem.AuthorizedByName = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorizedByName"]));
                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objItem.DueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]));
                        objItem.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]));
                        objItem.FromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["FromStoreName"]));
                        objItem.ToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ToStoreName"]));
                        objItem.IndentCreatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentCreatedByID"]));
                        //objItem.IndentCreatedByName = Convert.ToString(DALHelper.HandleDBNull(reader["IndentCreatedByName"]));
                        objItem.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        objItem.IndentStatus = (InventoryIndentStatus)Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"]));
                        objItem.IsFreezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreezed"]));
                        objItem.IsForwarded = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForwarded"]));
                        objItem.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));

                        objItem.IsAuthorized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]));
                        objItem.AuthorizedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]));
                        objItem.AuthorizationDate = (DALHelper.HandleDate(reader["AuthorizationDate"]));

                        objItem.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objItem.Time = DALHelper.HandleDate(reader["Time"]);
                        objItem.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        objItem.TransactionMovementID = Convert.ToInt32(DALHelper.HandleDBNull(reader["TransactionMovementID"]));
                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItem.IndentUnitID = objItem.UnitID;
                        objItem.IndentType = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IndentType"]));
                        objItem.IsConvertToPR = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConvertToPR"]));
                        //By Anjali...........................................................................
                        objItem.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //objItem.IsIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //................................................................................

                        objItem.InventoryIndentType = (InventoryIndentType)Enum.Parse(typeof(InventoryIndentType), Convert.ToString(DALHelper.HandleDBNull(reader["InventoryIndentType"])));

                        #region Added by MMBABU
                        if (string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["GRN No - Date"]))))
                            objItem.GRNNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["IndentGRN No - Date"]));
                        else
                            objItem.GRNNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["GRN No - Date"]));

                        if (string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["PO No - Date"]))))
                            objItem.PONowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["IndentPO No - Date"]));
                        else
                            objItem.PONowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["PO No - Date"]));

                        if (string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["Issue No - Date"]))))
                            objItem.IssueNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["IndentIssue No - Date"]));
                        else
                            objItem.IssueNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["Issue No - Date"]));

                        if (string.IsNullOrEmpty(Convert.ToString(DALHelper.HandleDBNull(reader["Indent No - Date"]))))
                            objItem.PRNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["PR No - Date"]));
                        else
                            objItem.PRNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["Indent No - Date"]));
                        #endregion


                        objItem.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                        objItem.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objItem.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objItem.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        objItem.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNo"]));
                        objBizAction.IndentList.Add(objItem);
                    }
                }
                reader.NextResult();
                objBizAction.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;

        }

        public override IValueObject GetIndentDetailList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIndentDetailListBizActionVO objBizAction = valueObject as clsGetIndentDetailListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetStoreIndentDetailList");

                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.IndentID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.IndentDetailList == null)
                        objBizAction.IndentDetailList = new List<clsIndentDetailVO>();
                    while (reader.Read())
                    {
                        clsIndentDetailVO objItem = new clsIndentDetailVO();

                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItem.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));

                        objItem.SingleQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));  //TransactionQuantity
                        objItem.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"])); // TransactionUOMID
                        objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objItem.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUOM"])); // StockUOMID
                        objItem.StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"]));  // BaseCF
                        objItem.StockingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        objItem.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUom"]));

                        objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        //objItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"])); // BaseCF
                        //objItem.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["RequiredQuantity"]));  //  BaseQuantity
                        objItem.RequiredQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        objItem.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUom"]));

                        objItem.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.PurchaseOrderQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseOrderQuantity"]));
                        objItem.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        objItem.PurchaseRate = (double)Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.IsItemBlock = Convert.ToBoolean(reader["IsItemBlock"]);

                        objItem.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));
                        objItem.PendingQtyFP = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantityFP"]));  //for only Indent Front Pannel on 12042018

                        objBizAction.IndentDetailList.Add(objItem);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetIndentListByStoreId(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetIndenListByStoreIdBizActionVO objBizAction = valueObject as clsGetIndenListByStoreIdBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetIndentListByStoreId_1");

                dbServer.AddInParameter(command, "FromIndentStoreId", DbType.String, objBizAction.FromIndentStoreId);
                dbServer.AddInParameter(command, "ToIndentStoreId", DbType.String, objBizAction.ToIndentStoreId);

                if (objBizAction.FromDate != null && (int)(objBizAction.FromDate.ToOADate()) != 0)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);

                if (objBizAction.ToDate != null && (int)(objBizAction.ToDate.ToOADate()) != 0)
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "IndentNumber", DbType.String, objBizAction.IndentNumber);
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizAction.Freezed);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "FromPO", DbType.Boolean, objBizAction.FromPO);
                //By Anjali.............................................................
                // dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objBizAction.IsIndent);
                dbServer.AddInParameter(command, "IsIndent", DbType.Int32, objBizAction.IsIndent);
                //......................................................................
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.IndentList == null)
                        objBizAction.IndentList = new List<clsIndentMasterVO>();
                    while (reader.Read())
                    {
                        clsIndentMasterVO objItem = new clsIndentMasterVO();
                        objItem.AuthorizationDate = Convert.ToDateTime(DALHelper.HandleDate(reader["AuthorizationDate"]));
                        objItem.AuthorizedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]));
                        objItem.AuthorizedByName = Convert.ToString(DALHelper.HandleDBNull(reader["AuthorizedByName"]));
                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objItem.DueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]));
                        objItem.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.IndentCreatedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentCreatedByID"]));
                        objItem.IndentCreatedByName = Convert.ToString(DALHelper.HandleDBNull(reader["IndentCreatedByName"]));
                        objItem.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        objItem.IndentStatus = (InventoryIndentStatus)Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"]));
                        objItem.IsAuthorized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]));
                        objItem.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objItem.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        objItem.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        objItem.TransactionMovementID = Convert.ToInt32(DALHelper.HandleDBNull(reader["TransactionMovementID"]));
                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objItem.UnitName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                        //By Anjali.....................................................................
                        objItem.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //objItem.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        //......................................................................................
                        objItem.IndentUnitID = objItem.UnitID;

                        objItem.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                        objItem.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objItem.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));

                        //***//------
                        objItem.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        objItem.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        //---------------

                        objBizAction.IndentList.Add(objItem);
                    }
                }
                reader.NextResult();
                objBizAction.TotalRowCount = Convert.ToInt32(DALHelper.HandleDBNull(dbServer.GetParameterValue(command, "TotalRows")));
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }


        public override IValueObject GetIndentListForDashBoard(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetIndentListForInventorDashBoardBizActionVO objBizAction = valueObject as clsGetIndentListForInventorDashBoardBizActionVO;

            DbCommand command;
            DbDataReader reader = null;

            try
            {
                //command = dbServer.GetStoredProcCommand("CIMS_GetStoreIndentListForDashBoard");

                command = dbServer.GetStoredProcCommand("CIMS_GetStoreIndentListForDashBoard_New_01");

                //By Umesh
                if (objBizAction.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                if (objBizAction.ToDate != null)
                {
                    if (objBizAction.FromDate != null)
                    {
                        objBizAction.ToDate = objBizAction.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                }
                dbServer.AddInParameter(command, "FromStoreID", DbType.Int64, objBizAction.FromStoreID);
                dbServer.AddInParameter(command, "ToStoreID", DbType.Int64, objBizAction.ToStoreID);
                dbServer.AddInParameter(command, "IndentNO", DbType.String, objBizAction.IndentNO);
                // END


                dbServer.AddInParameter(command, "Date", DbType.DateTime, objBizAction.Date);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int64, objBizAction.IndentStatus);
                dbServer.AddInParameter(command, "IsPaging", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "NoOfRecordShow", DbType.Int64, objBizAction.NoOfRecords);
                dbServer.AddInParameter(command, "StartIndex", DbType.Int64, objBizAction.StartRowIndex);
                dbServer.AddOutParameter(command, "TotalRow", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);
                //By Anjali..........................................
                dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objBizAction.IsIndent);
                //...................................................
                if (objBizAction.IsOrderBy != null)
                {
                    dbServer.AddInParameter(command, "OrderBy", DbType.Int64, objBizAction.IsOrderBy);
                }

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                //objBizAction.TotalRow = (int)dbServer.GetParameterValue(command, "TotalRow");

                if (reader.HasRows)
                {
                    if (objBizAction.IndentList == null)
                        objBizAction.IndentList = new List<clsIndentMasterVO>();
                    while (reader.Read())
                    {
                        clsIndentMasterVO objItem = new clsIndentMasterVO();
                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));

                        objItem.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        objItem.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        objItem.DueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["DueDate"]));
                        objItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.IndentStatus = (InventoryIndentStatus)Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentStatus"]));
                        objItem.FromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["FromStore"]));
                        objItem.ToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ToStore"]));
                        objItem.AddedOn = Convert.ToString(DALHelper.HandleDBNull(reader["LoginName"]));
                        objItem.IsAuthorized = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAuthorized"]));
                        objItem.AuthorizedByID = Convert.ToInt64(DALHelper.HandleDBNull(reader["AuthorizedByID"]));
                        objItem.AuthorizedByName = Convert.ToString(DALHelper.HandleDBNull(reader["UserName"]));
                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        if (objBizAction.IsIndent == false)
                        {
                            objItem.PRBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["PRBaseItemQty"]));
                            objItem.POBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["POBaseItemQty"]));
                            objItem.GRNBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["GRNBaseItemQty"]));
                        }
                        else if (objBizAction.IsIndent == true)
                        {
                            objItem.IndentBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["IndentBaseItemQty"]));
                            objItem.IssueBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["IssueBaseItemQty"]));
                            objItem.ReceivedBaseItemQty = Convert.ToInt32(DALHelper.HandleDBNull(reader["ReceivedBaseItemQty"]));

                        }


                        objBizAction.IndentList.Add(objItem);
                    }
                }
                reader.NextResult();
                objBizAction.TotalRow = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRow"));

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;

        }
        public override IValueObject CloseIndentFromDashBoard(IValueObject valueObject, clsUserVO userVO)
        {
            clsCloseIndentFromDashboard objBizAction = valueObject as clsCloseIndentFromDashboard;
            try
            {

                if (objBizAction.isBulkIndentClosed == false)
                {
                     DbCommand command = dbServer.GetStoredProcCommand("CIMS_CloseIndentFromDashBoard");

                    dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.IndentID);
                    dbServer.AddInParameter(command, "Remark", DbType.String, objBizAction.Remarks);
                    dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, InventoryIndentStatus.ForceFullyCompleted);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                else if (objBizAction.isBulkIndentClosed == true)
                {

                    for (int i = 0; i < objBizAction.BulkCloseIndetList.Count; i++)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_CloseIndentFromDashBoard");

                        dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.BulkCloseIndetList[i].ID);
                        dbServer.AddInParameter(command, "Remark", DbType.String, objBizAction.Remarks);
                        dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, InventoryIndentStatus.BulkClose);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.BulkCloseIndetList[i].UnitID);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        command.Cancel();
                    }

                    
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return objBizAction;

        }
        public override IValueObject UpdateIndentRemarkandCancelIndent(IValueObject valueObject, clsUserVO userVO)
        {
            clsUpdateRemarkForIndentCancellationBizActionVO objBizAction = valueObject as clsUpdateRemarkForIndentCancellationBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans;
            con.Open();
            trans = con.BeginTransaction();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_CancelIndentAndUpdateRemark");
                command.Connection = con;
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.IndentMaster.ID);
                dbServer.AddInParameter(command, "CancellationRemark", DbType.String, objBizAction.CancellationRemark);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, objBizAction.IndentMaster.UnitID);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return objBizAction;

        }


        public override IValueObject UpdateIndentRemarkandRejectIndent(IValueObject valueObject, clsUserVO userVO)
        {
            clsUpdateRemarkForIndentCancellationBizActionVO objBizAction = valueObject as clsUpdateRemarkForIndentCancellationBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans;
            con.Open();
            trans = con.BeginTransaction();
            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_RejectIndentAndUpdateRemark");
                command.Connection = con;
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.IndentMaster.ID);
                dbServer.AddInParameter(command, "CancellationRemark", DbType.String, objBizAction.CancellationRemark);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, userVO.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, objBizAction.IndentMaster.UnitID);
                dbServer.AddInParameter(command, "IndentStatus", DbType.Int64, objBizAction.IndentMaster.IndentStatus);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command);
                objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }

            return objBizAction;

        }

        public override IValueObject ClosePurchaseRequisitionFromDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCloseIndentFromDashboard objBizAction = valueObject as clsCloseIndentFromDashboard;
            try
            {

                if (objBizAction.isBulkPRClosed == false)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_ClosePurchaseRequisitionFromDashBoard");

                    dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.IndentID);
                    dbServer.AddInParameter(command, "Remark", DbType.String, objBizAction.Remarks);
                    dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, InventoryIndentStatus.ForceFullyCompleted);
                    dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                    dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus = dbServer.ExecuteNonQuery(command);
                    objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                }
                else if (objBizAction.isBulkPRClosed == true)
                {

                    for (int i = 0; i < objBizAction.BulkCloseIndetList.Count; i++)
                    {
                        DbCommand command = dbServer.GetStoredProcCommand("CIMS_ClosePurchaseRequisitionFromDashBoard");

                        dbServer.AddInParameter(command, "IndentID", DbType.Int64, objBizAction.BulkCloseIndetList[i].ID);
                        dbServer.AddInParameter(command, "Remark", DbType.String, objBizAction.Remarks);
                        dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, InventoryIndentStatus.BulkClose);
                        dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                        dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                        dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.BulkCloseIndetList[i].UnitID);

                        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus = dbServer.ExecuteNonQuery(command);
                        objBizAction.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                        command.Cancel();
                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return objBizAction;
        }


        


    }
}
