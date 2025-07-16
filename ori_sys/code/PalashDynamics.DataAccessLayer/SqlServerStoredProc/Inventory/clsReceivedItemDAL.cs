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

    class clsReceivedItemDAL : clsBaseReceivedItemDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsReceivedItemDAL()
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


        public override IValueObject GetReceivedList(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetReceivedListBizActionVO objBizAction = valueObject as clsGetReceivedListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetReceivedList");

                if (objBizAction.MRNo != null && objBizAction.MRNo.Length != 0)
                    dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + objBizAction.MRNo + "%");
                //else
                //    dbServer.AddInParameter(command, "MrNo", DbType.String, objBizAction.MRNo + "%");

                if (objBizAction.PatientName != null && objBizAction.PatientName.Length != 0)
                    dbServer.AddInParameter(command, "PatientName", DbType.String, objBizAction.PatientName + "%");

                dbServer.AddInParameter(command, "UserID", DbType.String, objBizAction.UserId);
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
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitId);//userVO.UserLoginInfo.UnitId);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO();

                        item.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        item.ReceivedFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]));
                        item.ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"]));
                        item.ReceivedId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]));
                        item.ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        item.ReceivedToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]));
                        item.ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"]));

                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //***//------
                        item.MRNO = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                        item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));
                        //---------------

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


        public override IValueObject GetItemListByIssueReceivedId(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByIssueReceivedIdBizActionVO objBizAction = valueObject as clsGetItemListByIssueReceivedIdBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetItemListByReceivedId");

                dbServer.AddInParameter(command, "ReceivedId", DbType.Int64, objBizAction.ReceivedId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsReceivedItemDetailsVO>();
                    while (reader.Read())
                    {
                        clsReceivedItemDetailsVO objItem = new clsReceivedItemDetailsVO();

                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));

                        objItem.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"]));
                        objItem.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));
                        objItem.ReceivedQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItem.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ItemTotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        objItem.ReceiveItemTotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem.MRPAmt = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRPAmount"])); //***//
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objItem.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objItem.Bin = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItem.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNo"]));
                        objItem.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objItem.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])); //***//
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


        public override IValueObject AddReceivedItem(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddReceivedItemBizActionVO BizActionObj = valueObject as clsAddReceivedItemBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsReceivedItemVO objReceivedItem = BizActionObj.ReceivedItemDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddReceivedItem");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objReceivedItem.LinkServer);
                if (objReceivedItem.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objReceivedItem.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "IssueID", DbType.Int64, objReceivedItem.IssueID);

                dbServer.AddInParameter(command, "IssueUnitID", DbType.Int64, objReceivedItem.IssueUnitID);

                if (DALHelper.IsValidDateRangeDB(objReceivedItem.ReceivedDate) == true)
                    dbServer.AddInParameter(command, "ReceivedDate", DbType.DateTime, objReceivedItem.ReceivedDate);
                else
                    dbServer.AddInParameter(command, "ReceivedDate", DbType.DateTime, null);

                dbServer.AddInParameter(command, "ReceivedFromStoreId", DbType.Int64, objReceivedItem.ReceivedFromStoreId);
                dbServer.AddParameter(command, "ReceivedNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "ReceivedToStoreId", DbType.Int64, objReceivedItem.ReceivedToStoreId);
                dbServer.AddInParameter(command, "Remark", DbType.String, objReceivedItem.Remark);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objReceivedItem.TotalAmount);
                dbServer.AddInParameter(command, "TotalItems", DbType.Decimal, objReceivedItem.TotalItems);
                dbServer.AddInParameter(command, "TotalTAXAmount", DbType.Decimal, objReceivedItem.TotalTaxAmount);
                dbServer.AddInParameter(command, "TotalVATAmount", DbType.Decimal, objReceivedItem.TotalVATAmount);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ReceivedById", DbType.Int64, objReceivedItem.ReceivedById);
                dbServer.AddInParameter(command, "IsIndent", DbType.Int32, objReceivedItem.IsIndent);

                dbServer.AddInParameter(command, "IsPatientIndent", DbType.Boolean, objReceivedItem.IsAgainstPatient);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objReceivedItem.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objReceivedItem.PatientUnitID);

                if (objReceivedItem.IsIndent == 1)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                }
                else if (objReceivedItem.IsIndent == 0)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (objReceivedItem.IsIndent == 2)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (objReceivedItem.IsIndent == 3)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (objReceivedItem.IsIndent == 4)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }

                else if (objReceivedItem.IsIndent == 6) //***//
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ReceivedItemDetails.ReceivedID = (long?)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ReceivedItemDetails.ReceivedNumber = (string)dbServer.GetParameterValue(command, "ReceivedNumber");

                foreach (var item in BizActionObj.ReceivedItemDetails.ReceivedItemDetailsList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddReceivedItemList");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objReceivedItem.LinkServer);
                    if (objReceivedItem.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objReceivedItem.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);
                    dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);

                    if (DALHelper.IsValidDateRangeDB(item.ExpiryDate) == true)
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    else
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, null);

                    dbServer.AddInParameter(command1, "IssueQty", DbType.Decimal, item.IssueQty);

                    //***Added by Ashish Z.
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);  // StockCF
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.StockingQuantity);
                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Int64, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "IssuedUOMID", DbType.Int64, item.UOMID);
                    //***
                    dbServer.AddInParameter(command1, "ReceivedQty", DbType.Decimal, item.ReceivedQty);
                    dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MainMRP);  // Base MRP
                    dbServer.AddInParameter(command1, "MRPAmount", DbType.Decimal, item.MRPAmt); // Total MRP
                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Decimal, item.MainRate); // Base CP
                    dbServer.AddInParameter(command1, "PurchaseRateAmount", DbType.Decimal, item.PurchaseRateAmt); // Total CP
                    dbServer.AddInParameter(command1, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ReceivedFromStoreID", DbType.Int64, objReceivedItem.ReceivedFromStoreId);
                    dbServer.AddInParameter(command1, "ReceivedToStoreID", DbType.Int64, objReceivedItem.ReceivedToStoreId);
                    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "TransactionTypeID", DbType.Int32, InventoryTransactionType.ReceivedItemAgainstIssue);
                    dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command1, "IsIndent", DbType.Int32, item.IsIndent);
                    dbServer.AddInParameter(command1, "GRNID", DbType.Int64, item.GRNID);
                    dbServer.AddInParameter(command1, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                    dbServer.AddInParameter(command1, "GRNDetailID", DbType.Int64, item.GRNDetailID);
                    dbServer.AddInParameter(command1, "GRNDetailUnitID", DbType.Int64, item.GRNDetailUnitID);

                    #region For Quarantine Items (Expired, DOS)
                    // Use For Vat/Tax Calculations
                    dbServer.AddInParameter(command1, "ItemTaxPercentage", DbType.Decimal, item.ItemTaxPercentage);
                    dbServer.AddInParameter(command1, "ItemTaxAmount", DbType.Decimal, item.ItemTaxAmount);
                    dbServer.AddInParameter(command1, "ItemVATPercentage", DbType.Decimal, item.ItemVATPercentage);
                    dbServer.AddInParameter(command1, "ItemVATAmount", DbType.Decimal, item.ItemVATAmount);
                    dbServer.AddInParameter(command1, "othertaxApplicableon", DbType.Int16, item.OtherGRNItemTaxApplicationOn);
                    dbServer.AddInParameter(command1, "otherTaxType", DbType.Int64, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command1, "VatApplicableon", DbType.Int64, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command1, "Vattype", DbType.Int64, item.GRNItemVatType);
                    dbServer.AddInParameter(command1, "InclusiveOfTax", DbType.Boolean, item.InclusiveOfTax);
                    #endregion

                    if (item.IsIndent == 1)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);

                    }
                    else if (item.IsIndent == 0)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);
                    }
                    else if (item.IsIndent == 2)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount);
                    }
                    else if (objReceivedItem.IsIndent == 3)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount);
                    }
                    else if (objReceivedItem.IsIndent == 4)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount);
                    }

                    else if (objReceivedItem.IsIndent == 6) //***//
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);
                    }

                    dbServer.AddInParameter(command1, "IssueID", DbType.Int64, objReceivedItem.IssueID);
                    dbServer.AddInParameter(command1, "IssueUnitID", DbType.Int64, objReceivedItem.IssueUnitID); //***//

                    dbServer.AddInParameter(command1, "ReceivedId", DbType.Int64, BizActionObj.ReceivedItemDetails.ReceivedID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");



                    //Added By Ashish Z. on Dated 30012017
                    if (BizActionObj.SuccessStatus == 10) 
                    {
                        BizActionObj.ItemName = item.ItemName;
                        throw new Exception();
                    }
                    //End 

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateIssue");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, BizActionObj.ReceivedItemDetails.IssueUnitID);
                    dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, (item.ReceivedQty * Convert.ToDecimal(item.BaseConversionFactor)));
                    dbServer.AddInParameter(command2, "IssueDetailsID", DbType.Int64, item.IssueDetailsID);
                    int status2 = dbServer.ExecuteNonQuery(command2, trans);

                    objReceivedItem.StockDetails.BatchID = 0;
                    objReceivedItem.StockDetails.BatchCode = item.BatchCode;
                    objReceivedItem.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    objReceivedItem.StockDetails.ItemID = Convert.ToInt64(item.ItemId);
                    objReceivedItem.StockDetails.TransactionTypeID = InventoryTransactionType.ReceivedItemAgainstIssue;
                    objReceivedItem.StockDetails.TransactionID = Convert.ToInt64(BizActionObj.ReceivedItemDetails.ReceivedID);

                    if (item.IsIndent == 1)//item.IsIndent == true   //By Anjali......................
                    {
                        objReceivedItem.StockDetails.TransactionQuantity = item.BaseQuantity;//Convert.ToDouble(item.ReceivedQty);
                    }
                    else
                    {
                        objReceivedItem.StockDetails.TransactionQuantity = item.BaseQuantity; //Convert.ToDouble(item.ReceivedQty * item.ConversionFactor);
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

                    //added by Ashish Z. for Non Batch Item on 15Apr16
                    objReceivedItem.StockDetails.IsFromOpeningBalance = true;   // 
                    objReceivedItem.StockDetails.MRP = Convert.ToDouble(item.MainMRP);
                    objReceivedItem.StockDetails.PurchaseRate = Convert.ToDouble(item.MainRate);
                    //End

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

                if (objReceivedItem.IsIndent == 0 || objReceivedItem.IsIndent == 1)  // Updation against Indent and PR only
                {
                    DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateIndentStatusFromReceiveIssue");
                    command3.Connection = con;

                    dbServer.AddInParameter(command3, "ReceivedID", DbType.Int64, BizActionObj.ReceivedItemDetails.ReceivedID);
                    dbServer.AddInParameter(command3, "ReceivedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);

                    int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                if (BizActionObj.SuccessStatus != 10)
                    BizActionObj.SuccessStatus = -1;
                BizActionObj.ReceivedItemDetails = null;
            }
            finally
            {
                con.Close();
                //trans = null;
            }

            return BizActionObj;
        }

        public override IValueObject GetReceivedListQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReceivedListBizActionVO objBizAction = valueObject as clsGetReceivedListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                //command = dbServer.GetStoredProcCommand("CIMS_GetReceivedList");
                command = dbServer.GetStoredProcCommand("CIMS_GetReceivedListQS");   //Use to get already saved Received Items at Quarantine Stores Only

                dbServer.AddInParameter(command, "UserID", DbType.String, objBizAction.UserId);
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
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsQSOnly", DbType.Boolean, objBizAction.IsQSOnly); //Use to get already saved Received Items at Quarantine Stores Only

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);




                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO();

                        item.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        item.ReceivedFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]));
                        item.ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"]));
                        item.ReceivedId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]));
                        item.ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        item.ReceivedToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]));
                        item.ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"]));

                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));



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


        public override IValueObject GetReceivedListForGRNToQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetReceivedListBizActionVO objBizAction = valueObject as clsGetReceivedListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetReceivedListForGRNToQS");   //Use to get already saved Received Items at Quarantine Stores Only

                dbServer.AddInParameter(command, "UserID", DbType.String, objBizAction.UserId);
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
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsForReceiveGRNToQS", DbType.Boolean, objBizAction.IsForReceiveGRNToQS); //set on ReceiveGRNToQS form for FrontPannel List.

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsReceivedListVO item = new clsReceivedListVO();

                        item.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));
                        item.ReceivedFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedFromStoreId"]));
                        item.ReceivedFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedFromStoreName"]));
                        item.ReceivedId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedId"]));
                        item.ReceivedNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        item.ReceivedToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedToStoreId"]));
                        item.ReceivedToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedToStoreName"]));

                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
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
    }
}
