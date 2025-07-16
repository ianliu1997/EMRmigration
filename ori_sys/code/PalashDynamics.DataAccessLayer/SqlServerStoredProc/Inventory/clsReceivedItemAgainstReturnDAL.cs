using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using System.Reflection;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{

    class clsReceivedItemAgainstReturnDAL : clsBaseReceivedItemAgainstReturnDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsReceivedItemAgainstReturnDAL()
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


        public override IValueObject GetReceivedListAgainstReturn(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetReceivedListAgainstReturnBizActionVO objBizAction = valueObject as clsGetReceivedListAgainstReturnBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetReceivedListAgainstReturn");

                dbServer.AddInParameter(command, "ReceivedNumberSrc", DbType.String, objBizAction.ReceivedNumberSrc);

                if (DALHelper.IsValidDateRangeDB(objBizAction.ReceivedFromDate) == true)
                    //dbServer.AddInParameter(command, "ReceivedFromDate", DbType.DateTime, objBizAction.ReceivedFromDate.Value.ToString("MM/dd/yyyy"));
                    dbServer.AddInParameter(command, "ReceivedFromDate", DbType.DateTime, objBizAction.ReceivedFromDate.Value);
                else
                    dbServer.AddInParameter(command, "ReceivedFromDate", DbType.DateTime, null);

                if (DALHelper.IsValidDateRangeDB(objBizAction.ReceivedToDate) == true)
                    //dbServer.AddInParameter(command, "ReceivedToDate", DbType.DateTime, objBizAction.ReceivedToDate.Value.ToString("MM/dd/yyyy"));
                    dbServer.AddInParameter(command, "ReceivedToDate", DbType.DateTime, objBizAction.ReceivedToDate.Value);
                else
                    dbServer.AddInParameter(command, "ReceivedToDate", DbType.DateTime, null);

                dbServer.AddInParameter(command, "ReceivedFromStoreId", DbType.Int64, objBizAction.ReceivedFromStoreId);
                dbServer.AddInParameter(command, "ReceivedToStoreId", DbType.Int64, objBizAction.ReceivedToStoreId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizAction.InputSortExpression);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, userVO.ID);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsReceivedAgainstReturnListVO item = new clsReceivedAgainstReturnListVO();

                        item.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        item.ReceivedFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]));
                        item.ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"]));
                        item.ReceivedId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]));
                        item.ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        item.ReceivedToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]));
                        item.ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"]));

                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.ReturnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]));
                        item.ReturnNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.ReturnUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnUnitId"]));

                        objBizAction.ReceivedList.Add(item);

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


        public override IValueObject GetItemListByReturnReceivedId(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByReturnReceivedIdBizActionVO objBizAction = valueObject as clsGetItemListByReturnReceivedIdBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetItemListByReceivedAgainstReturnId");

                dbServer.AddInParameter(command, "ReceivedId", DbType.Int64, objBizAction.ReceivedId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "ReceivedUnitId", DbType.Int64, objBizAction.ReceivedUnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsReceivedItemAgainstReturnDetailsVO>();
                    while (reader.Read())
                    {
                        clsReceivedItemAgainstReturnDetailsVO objItem = new clsReceivedItemAgainstReturnDetailsVO();
                        //objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));

                        objItem.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.ReturnQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturnQty"]));
                        objItem.ReturnUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedQtyUOM"]));
                        objItem.ReceivedQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItem.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedQtyUOM"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ItemTotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        //objItem.ReceiveDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]));

                        objBizAction.ItemList.Add(objItem);
                    }
                }
                reader.NextResult();

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


        public override IValueObject AddReceivedItemAgainstReturn(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddReceivedItemAgainstReturnBizActionVO BizActionObj = valueObject as clsAddReceivedItemAgainstReturnBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsReceivedItemAgainstReturnVO objReceivedItem = BizActionObj.ReceivedItemAgainstReturnDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddReceivedItemAgainstReturn");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objReceivedItem.LinkServer);
                if (objReceivedItem.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objReceivedItem.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "ReturnID", DbType.Int64, objReceivedItem.ReturnID);

                if (DALHelper.IsValidDateRangeDB(objReceivedItem.ReceivedDate) == true)
                    dbServer.AddInParameter(command, "ReceivedDate", DbType.DateTime, objReceivedItem.ReceivedDate);
                else
                    dbServer.AddInParameter(command, "ReceivedDate", DbType.DateTime, null);

                dbServer.AddInParameter(command, "ReceivedByID", DbType.Int64, objReceivedItem.ReceivedByID);
                dbServer.AddInParameter(command, "ReceivedFromStoreId", DbType.Int64, objReceivedItem.ReceivedFromStoreId);
                //dbServer.AddInParameter(command, "ReceivedID", DbType.Int64, objReceivedItem.ReceivedID);
                //dbServer.AddInParameter(command, "ReceivedNumber", DbType.String, objReceivedItem.ReceivedNumber);
                dbServer.AddParameter(command, "ReceivedNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                dbServer.AddInParameter(command, "ReceivedToStoreId", DbType.Int64, objReceivedItem.ReceivedToStoreId);
                dbServer.AddInParameter(command, "ReturnUnitId", DbType.Int64, objReceivedItem.ReturnUnitId);

                dbServer.AddInParameter(command, "Remark", DbType.String, objReceivedItem.Remark);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objReceivedItem.TotalAmount);
                dbServer.AddInParameter(command, "TotalItems", DbType.Decimal, objReceivedItem.TotalItems);
                dbServer.AddInParameter(command, "TotalVATAmount", DbType.Decimal, objReceivedItem.TotalVATAmount);

                dbServer.AddInParameter(command, "IsIndent", DbType.Boolean, objReceivedItem.IsIndent);

                if (objReceivedItem.IsIndent == true)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                }
                else
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }



                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objReceivedItem.Status);
                dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);

                dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ReceivedID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objReceivedItem.ReceivedID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);

                //int intStatus = dbServer.ExecuteNonQuery(command, trans);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ReceivedItemAgainstReturnDetails.ReceivedID = (long?)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ReceivedItemAgainstReturnDetails.ReceivedNumber = (string)dbServer.GetParameterValue(command, "ReceivedNumber");
                foreach (var item in BizActionObj.ReceivedItemAgainstReturnDetails.ReceivedItemAgainstReturnDetailsList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddReceivedItemAgainstReturnList");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objReceivedItem.LinkServer);
                    if (objReceivedItem.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objReceivedItem.LinkServer.Replace(@"\", "_"));

                    //dbServer.AddInParameter(command1, "AvailableStock", DbType.Decimal, item.AvailableStock);
                    //dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, item.BalanceQty);
                    //dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);


                    if (DALHelper.IsValidDateRangeDB(item.ExpiryDate) == true)
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    else
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, null);

                    dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "ReturnQty", DbType.Decimal, item.ReturnQty);
                    dbServer.AddInParameter(command1, "ReceivedQty", DbType.Decimal, item.ReceivedQty);
                    //dbServer.AddInParameter(command1, "ItemCode", DbType.String, item.ItemCode);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);
                    //dbServer.AddInParameter(command1, "ItemName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);
                    dbServer.AddInParameter(command1, "ItemVATAmount", DbType.Decimal, item.ItemVATAmount);
                    dbServer.AddInParameter(command1, "ItemVATPercentage", DbType.Decimal, item.ItemVATPercentage);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP);
                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Decimal, item.PurchaseRate);
                    dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, item.IsIndent);


                    //***Added by Ashish Z.
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Double, item.ConversionFactor);  // StockCF
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.StockingQuantity);

                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Int64, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "ReturnUOMID", DbType.Int64, item.UOMID);   // TransactionUOMID for Return Qty.
                    //***

                    if (objReceivedItem.IsIndent == true)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                    }
                    else
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                    }
                    //dbServer.AddInParameter(command1, "NetPayBeforeVATAmount", DbType.Decimal, item.NetPayBeforeVATAmount);
                    //dbServer.AddInParameter(command1, "TotalForVAT)", DbType.Decimal, item.TotalForVAT);
                    //dbServer.AddInParameter(command1, "UnverifiedStock", DbType.Decimal, item.UnverifiedStock);
                    //dbServer.AddInParameter(command1, "VATInclusive", DbType.Decimal, item.VATInclusive);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "ReceivedFromStoreID", DbType.Int64, objReceivedItem.ReceivedFromStoreId);
                    dbServer.AddInParameter(command1, "ReceivedToStoreID", DbType.Int64, objReceivedItem.ReceivedToStoreId);
                    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ReceivedItemAgainstReturn);
                    //dbServer.AddInParameter(command1, "TransactionID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                    dbServer.AddInParameter(command1, "ReceivedId", DbType.Int64, BizActionObj.ReceivedItemAgainstReturnDetails.ReceivedID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int32, 0);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateReturn");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, objReceivedItem.ReturnUnitId);
                    dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.BaseQuantity); //item.ReceivedQty);
                    dbServer.AddInParameter(command2, "ReturnDetailsID", DbType.Int64, item.ReturnItemDetailsID);
                    //int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);

                    objReceivedItem.StockDetails.BatchID = 0;
                    objReceivedItem.StockDetails.BatchCode = item.BatchCode;
                    objReceivedItem.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    objReceivedItem.StockDetails.ItemID = Convert.ToInt64(item.ItemId);
                    objReceivedItem.StockDetails.TransactionTypeID = InventoryTransactionType.ReceivedItemAgainstReturn;
                    objReceivedItem.StockDetails.TransactionID = Convert.ToInt64(BizActionObj.ReceivedItemAgainstReturnDetails.ReceivedID);

                    if (item.IsIndent == false)
                    {
                        objReceivedItem.StockDetails.TransactionQuantity = item.BaseQuantity; //Convert.ToDouble(item.ReceivedQty * Convert.ToDecimal(item.ConversionFactor));
                    }
                    else
                    {
                        objReceivedItem.StockDetails.TransactionQuantity = item.BaseQuantity; //Convert.ToDouble(item.ReceivedQty);
                    }
                    objReceivedItem.StockDetails.Date = Convert.ToDateTime(objReceivedItem.ReceivedDate);
                    objReceivedItem.StockDetails.Time = System.DateTime.Now;
                    objReceivedItem.StockDetails.StoreID = objReceivedItem.ReceivedFromStoreId;

                    objReceivedItem.StockDetails.StockingQuantity = item.StockingQuantity;
                    objReceivedItem.StockDetails.BaseUOMID = item.BaseUOMID;
                    objReceivedItem.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    objReceivedItem.StockDetails.SUOMID = item.SUOMID;
                    objReceivedItem.StockDetails.ConversionFactor = Convert.ToDouble(item.ConversionFactor);   // StockCF
                    objReceivedItem.StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;   //TransactionUOMID

                    objReceivedItem.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReceivedQty);
                    objReceivedItem.StockDetails.ExpiryDate = item.ExpiryDate;

                    ////added by Ashish Z. for Non Batch Item on 15Apr16
                    //objReceivedItem.StockDetails.IsFromOpeningBalance = true;   // 
                    //objReceivedItem.StockDetails.MRP = Convert.ToDouble(item.MRP);
                    //objReceivedItem.StockDetails.PurchaseRate = Convert.ToDouble(item.PurchaseRate);
                    ////End


                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    obj.Details = objReceivedItem.StockDetails;
                    obj.Details.ID = 0;
                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, userVO, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    objReceivedItem.StockDetails.ID = obj.Details.ID;

                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                // trans.Rollback();
                logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.ReceivedItemAgainstReturnDetails = null;
            }
            finally
            {
                con.Close();
                // trans = null;
            }

            return BizActionObj;
        }

    }
}

