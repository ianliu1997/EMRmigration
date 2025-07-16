using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    class clsReturnItemDAL : clsBaseReturnItemDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsReturnItemDAL()
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


        public override IValueObject GetReturnList(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetReturnListBizActionVO objBizAction = valueObject as clsGetReturnListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetReturnList");

                dbServer.AddInParameter(command, "ReturnNumberSrc", DbType.String, objBizAction.ReturnNumberSrc);

                if (DALHelper.IsValidDateRangeDB(objBizAction.ReturnFromDate) == true)
                    //dbServer.AddInParameter(command, "ReturnFromDate", DbType.DateTime, objBizAction.ReturnFromDate.Value.ToString("MM/dd/yyyy"));
                    dbServer.AddInParameter(command, "ReturnFromDate", DbType.DateTime, objBizAction.ReturnFromDate.Value);
                else
                    dbServer.AddInParameter(command, "ReturnFromDate", DbType.DateTime, null);

                if (DALHelper.IsValidDateRangeDB(objBizAction.ReturnToDate) == true)
                    //dbServer.AddInParameter(command, "ReturnToDate", DbType.DateTime, objBizAction.ReturnToDate.Value.ToString("MM/dd/yyyy"));
                    dbServer.AddInParameter(command, "ReturnToDate", DbType.DateTime, objBizAction.ReturnToDate.Value);
                else
                    dbServer.AddInParameter(command, "ReturnToDate", DbType.DateTime, null);

                dbServer.AddInParameter(command, "ReturnFromStoreId", DbType.Int64, objBizAction.ReturnFromStoreId);
                dbServer.AddInParameter(command, "ReturnToStoreId", DbType.Int64, objBizAction.ReturnToStoreId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizAction.InputSortExpression);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserId);

                
                if (objBizAction.transactionType == InventoryTransactionType.ReceivedItemAgainstReturn)
                    dbServer.AddInParameter(command, "transactionType", DbType.Boolean, true);


                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsReturnListVO item = new clsReturnListVO();

                        item.ReturnDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReturnDate"]));
                        item.ReturnFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnFromStoreId"]));
                        item.ReturnFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnFromStoreName"]));
                        item.ReturnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]));
                        item.ReturnNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnNumber"]));
                        item.ReturnToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnToStoreId"]));
                        item.ReturnToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnToStoreName"]));

                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        item.ReturnByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnByName"]));

                        objBizAction.ReturnList.Add(item);

                    }
                }
                reader.NextResult();
                objBizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

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
            return objBizAction;

        }


        public override IValueObject GetItemListByReturnId(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByReturnIdBizActionVO objBizAction = valueObject as clsGetItemListByReturnIdBizActionVO;


            if (objBizAction.ItemList == null)
                objBizAction.ItemList = new List<clsReturnItemDetailsVO>();


            if (objBizAction.ItemListAgainstReturn == null)
                objBizAction.ItemListAgainstReturn = new List<clsReceivedItemAgainstReturnDetailsVO>();

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetItemListByReturnId");

                dbServer.AddInParameter(command, "ReturnId", DbType.Int64, objBizAction.ReturnId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsReturnItemDetailsVO>();
                    while (reader.Read())
                    {
                        clsReturnItemDetailsVO objItem = new clsReturnItemDetailsVO();
                        //objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));

                        objItem.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"]));
                        objItem.ReturnQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturndQuantity"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));//PendingQuantity
                        objItem.ItemTotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem.ReturnedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"]));
                        //By Anjali..........................................
                        objItem.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        // objItem.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        //..........................................................

                        objBizAction.ItemList.Add(objItem);


                        clsReceivedItemAgainstReturnDetailsVO objItem1 = new clsReceivedItemAgainstReturnDetailsVO();
                        //objItem1.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem1.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem1.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));

                        objItem1.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem1.ReturnQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturndQuantity"]));
                        objItem1.ReceivedQty = 0; //Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReturndQuantity"]));
                        objItem1.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem1.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem1.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem1.ItemTotalAmount =  Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem1.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem1.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem1.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem1.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem1.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem1.ReturnItemDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReturnId"]));
                        //objItem1.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem1.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        //***Added by Ashish Z. for C.F.
                        objItem1.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        objItem1.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItem1.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objItem1.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objItem1.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));  // TransactionUOMID for Return Qty.
                        objItem1.ReturnUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"]));
                        objItem1.ConversionFactor = Convert.ToSingle(DALHelper.HandleBoolDBNull(reader["StockCF"]));
                        objItem1.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleBoolDBNull(reader["BaseCF"]));
                        objItem1.SelectedUOM = new MasterListItem { ID = objItem1.UOMID, Description = objItem1.ReturnUOM };

                        //***
                        objBizAction.ItemListAgainstReturn.Add(objItem1);

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
            return objBizAction;

        }


        public override IValueObject AddReturnItemToStore(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddReturnItemToStoreBizActionVO BizActionObj = valueObject as clsAddReturnItemToStoreBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsReturnItemVO objReturnItem = BizActionObj.ReturnItemDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddReturnItemToStore");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objReturnItem.LinkServer);
                if (objReturnItem.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objReturnItem.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "IssueID", DbType.Int64, objReturnItem.IssueID);

                if (DALHelper.IsValidDateRangeDB(objReturnItem.ReturnDate) == true)
                    dbServer.AddInParameter(command, "ReturnDate", DbType.DateTime, objReturnItem.ReturnDate);
                else
                    dbServer.AddInParameter(command, "ReturnDate", DbType.DateTime, null);


                dbServer.AddInParameter(command, "ReturnFromStoreId", DbType.Int64, objReturnItem.ReturnFromStoreId);
                //dbServer.AddInParameter(command, "ReturnID", DbType.Int64, objReturnItem.ReturnID);
                //dbServer.AddInParameter(command, "ReturnNumber", DbType.String, objReturnItem.ReturnNumber);
                dbServer.AddParameter(command, "ReturnNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                dbServer.AddInParameter(command, "ReturnToStoreId", DbType.Int64, objReturnItem.ReturnToStoreId);

                dbServer.AddInParameter(command, "Remark", DbType.String, objReturnItem.Remark);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objReturnItem.TotalAmount);
                dbServer.AddInParameter(command, "TotalItems", DbType.Decimal, objReturnItem.TotalItems);
                dbServer.AddInParameter(command, "TotalVATAmount", DbType.Decimal, objReturnItem.TotalVATAmount);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                //dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                //dbServer.AddInParameter(command, "Status", DbType.Boolean, objReturnItem.Status);
                dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                dbServer.AddInParameter(command, "ReceivedID", DbType.Int64, objReturnItem.ReceivedID);
                dbServer.AddInParameter(command, "IssueUnitID", DbType.Int64, objReturnItem.IssueUnitID);

                dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objReturnItem.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objReturnItem.PatientUnitID);
                dbServer.AddInParameter(command, "IsAgainstPatient", DbType.Boolean, objReturnItem.IsAgainstPatient);

                dbServer.AddInParameter(command, "IsIndent", DbType.Int32, objReturnItem.IsIndent);

                if (objReturnItem.IsIndent == 1)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                }
                else if (objReturnItem.IsIndent == 0)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (objReturnItem.IsIndent == 2)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (objReturnItem.IsIndent == 3)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (objReturnItem.IsIndent == 4)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }

                else if (objReturnItem.IsIndent == 6) //***//
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }

                //................................................................................
                dbServer.AddParameter(command, "ReturnID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objReturnItem.ReturnID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                //int intStatus = dbServer.ExecuteNonQuery(command, trans);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ReturnItemDetails.ReturnNumber = (string)dbServer.GetParameterValue(command, "ReturnNumber");
                BizActionObj.ReturnItemDetails.ReturnID = (long?)dbServer.GetParameterValue(command, "ID");

                foreach (var item in BizActionObj.ReturnItemDetails.ReturnItemDetailsList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddReturnItemListToStore");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objReturnItem.LinkServer);
                    if (objReturnItem.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objReturnItem.LinkServer.Replace(@"\", "_"));

                    //dbServer.AddInParameter(command1, "AvailableStock", DbType.Decimal, item.AvailableStock);
                    //dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, item.BalanceQty);
                    //dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);


                    if (DALHelper.IsValidDateRangeDB(item.ExpiryDate) == true)
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    else
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, null);


                    dbServer.AddInParameter(command1, "IssueQty", DbType.Decimal, item.IssueQty);
                    dbServer.AddInParameter(command1, "ReturnQty", DbType.Decimal, item.ReturnQty);
                    #region Commented By Pallavi
                    //if (item.IssueQty!=null)
                    //  dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, (item.IssueQty-item.ReturnQty));
                    //else
                    //    dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal,item.ReturnQty);
                    #endregion
                    dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, item.BaseQuantity);//item.ReturnQty);

                    //***Added by Ashish Z.
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Double, item.ConversionFactor);  // StockCF
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.StockingQuantity);

                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Int64, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "IssuedUOMID", DbType.Int64, item.UOMID);
                    //***

                    //dbServer.AddInParameter(command1, "ItemCode", DbType.String, item.ItemCode);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);
                    //dbServer.AddInParameter(command1, "ItemName", DbType.String, item.ItemName);
                    dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);
                    dbServer.AddInParameter(command1, "ItemVATAmount", DbType.Decimal, item.ItemVATAmount);
                    dbServer.AddInParameter(command1, "ItemVATPercentage", DbType.Decimal, item.ItemVATPercentage);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP);
                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Decimal, item.PurchaseRate);
                    //dbServer.AddInParameter(command1, "NetPayBeforeVATAmount", DbType.Decimal, item.NetPayBeforeVATAmount);
                    //dbServer.AddInParameter(command1, "TotalForVAT)", DbType.Decimal, item.TotalForVAT);
                    //dbServer.AddInParameter(command1, "UnverifiedStock", DbType.Decimal, item.UnverifiedStock);
                    //dbServer.AddInParameter(command1, "VATInclusive", DbType.Decimal, item.VATInclusive);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "ReturnFromStoreID", DbType.Int64, objReturnItem.ReturnFromStoreId);
                    dbServer.AddInParameter(command1, "ReturnToStoreID", DbType.Int64, objReturnItem.ReturnToStoreId);
                    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ItemReturn);
                    //dbServer.AddInParameter(command1, "TransactionID", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                    //By Anjali......................................................................
                    dbServer.AddInParameter(command1, "IsIndent", DbType.Int32, item.IsIndent);

                    if (item.IsIndent == 1)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                    }
                    else if (item.IsIndent == 0)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                    }
                    else if (item.IsIndent == 2)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                    }
                    else if (item.IsIndent == 3)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                    }
                    else if (item.IsIndent == 4)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                    }

                    else if (objReturnItem.IsIndent == 6) //***//
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                    }

                    //dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, item.IsIndent);

                    //if (item.IsIndent == true)
                    //{
                    //    dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                    //}


                    //................................................................

                    dbServer.AddInParameter(command1, "ReturnId", DbType.Int64, BizActionObj.ReturnItemDetails.ReturnID);

                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                    //int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                    if (item.ReceivedItemDetailsID != null && item.ReceivedItemDetailsID != 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command2.Connection = con;

                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                        //dbServer.AddInParameter(command2, "BalanceQty", DbType.Int64, (item.IssueQty - item.ReceivedQty));
                        dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.BaseQuantity);//(item.ReturnQty));
                        dbServer.AddInParameter(command2, "ReceiveItemDetailsID", DbType.Int64, item.ReceivedItemDetailsID);
                        int status2 = dbServer.ExecuteNonQuery(command2, trans);
                    }


                    //item.BatchId = 0;
                    objReturnItem.StockDetails.BatchID = Convert.ToInt64(item.BatchId);
                    objReturnItem.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    objReturnItem.StockDetails.ItemID = Convert.ToInt64(item.ItemId);
                    objReturnItem.StockDetails.TransactionTypeID = InventoryTransactionType.ItemReturn;
                    objReturnItem.StockDetails.TransactionID = Convert.ToInt64(objReturnItem.ReturnID);

                    if (item.IsIndent == 0 && objReturnItem.IsIssue)//item.IsIndent == false && objReturnItem.IsIssue //By Anjali..................................
                    {
                        objReturnItem.StockDetails.TransactionQuantity = item.BaseQuantity; //Convert.ToDouble(item.ReturnQty * Convert.ToDecimal(item.ConversionFactor));// * item.ConversionFactor);
                    }
                    else
                    {
                        objReturnItem.StockDetails.TransactionQuantity = item.BaseQuantity; // Convert.ToDouble(item.ReturnQty);
                    }
                    objReturnItem.StockDetails.Date = DateTime.Now;
                    objReturnItem.StockDetails.Time = System.DateTime.Now;
                    objReturnItem.StockDetails.StoreID = objReturnItem.ReturnFromStoreId;

                    objReturnItem.StockDetails.StockingQuantity = item.StockingQuantity;
                    objReturnItem.StockDetails.BaseUOMID = item.BaseUOMID;
                    objReturnItem.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    objReturnItem.StockDetails.SUOMID = item.SUOMID;
                    objReturnItem.StockDetails.ConversionFactor = Convert.ToDouble(item.ConversionFactor);   // StockCF
                    objReturnItem.StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;   //TransactionUOMID

                    objReturnItem.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReturnQty);
                    objReturnItem.StockDetails.ExpiryDate = item.ExpiryDate;

                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    obj.Details = objReturnItem.StockDetails;
                    obj.Details.ID = 0;
                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, userVO, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    objReturnItem.StockDetails.ID = obj.Details.ID;

                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.ReturnItemDetails = null;
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
