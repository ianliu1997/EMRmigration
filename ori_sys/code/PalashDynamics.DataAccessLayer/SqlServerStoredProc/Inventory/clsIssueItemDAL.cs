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
    class clsIssueItemDAL : clsBaseIssueItemDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsIssueItemDAL()
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


        public override IValueObject GetItemListByIndentIdForIsueItem(IValueObject valueObject, clsUserVO userVO)
        {

            GetItemListByIndentIdForIssueItemBizActionVO objBizAction = valueObject as GetItemListByIndentIdForIssueItemBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                if (objBizAction.TransactionType == InventoryTransactionType.Issue)
                {
                    valueObject = GetBatchListForIndentItemIdForIsueItem(valueObject, userVO);
                    return valueObject;

                }
                command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdForIssueItem");

                dbServer.AddInParameter(command, "IndentId", DbType.Int64, objBizAction.IndentID);
                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.String, objBizAction.IssueFromStoreId);
                // dbServer.AddInParameter(command, "IssueToStoreId", DbType.String, objBizAction.IssueToStoreId);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                //dbServer.AddInParameter(command, "IndnetUnitID", DbType.Int64, objBizAction.IndentUnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.IssueItemDetailsList == null)
                        objBizAction.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();

                    while (reader.Read())
                    {
                        clsIssueItemDetailsVO objItem = new clsIssueItemDetailsVO();
                        objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItem.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem.ExpiryDate = (DateTime?)Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));

                        objItem.IssueQty = Convert.ToDecimal("0");
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem.ItemTotalAmount =  Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem.NetPayBeforeVATAmount = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["NetPayBeforeVATAmount"]));
                        objItem.TotalForVAT = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["ToDecimal"]));
                        objItem.UnverifiedStock = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["UnverifiedStock"]));
                        objItem.VATInclusive = 0;//Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATInclusive"]));
                        objItem.AvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        if (objItem.AvailableStock <= 0)
                            continue;
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        long Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        // objItem.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.IndentDetailsID = Id;
                        objItem.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        objItem.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        objItem.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));

                        objBizAction.IssueItemDetailsList.Add(objItem);
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
                if (reader != null && reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return objBizAction;

        }


        public IValueObject GetBatchListForIndentItemIdForIsueItem(IValueObject valueObject, clsUserVO userVO)
        {

            GetItemListByIndentIdForIssueItemBizActionVO objBizAction = valueObject as GetItemListByIndentIdForIssueItemBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                command = dbServer.GetStoredProcCommand("CIMS_GetBatchListForIndentItemIdForIssueItem");

                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objBizAction.ItemID);
                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.String, objBizAction.IssueFromStoreId);
                // dbServer.AddInParameter(command, "IssueToStoreId", DbType.String, objBizAction.IssueToStoreId);

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                //dbServer.AddInParameter(command, "IndnetUnitID", DbType.Int64, objBizAction.IndentUnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.IssueItemDetailsList == null)
                        objBizAction.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();

                    while (reader.Read())
                    {
                        clsIssueItemDetailsVO objItem = new clsIssueItemDetailsVO();
                        //  objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItem.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        //objItem.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));

                        //objItem.IssueQty = Convert.ToDecimal("0");
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem.ItemTotalAmount =  Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        objItem.NetPayBeforeVATAmount = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["NetPayBeforeVATAmount"]));
                        objItem.TotalForVAT = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["ToDecimal"]));
                        objItem.UnverifiedStock = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["UnverifiedStock"]));
                        objItem.VATInclusive = 0;//Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATInclusive"]));
                        objItem.AvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        //if (objItem.AvailableStock <= 0)
                        //    continue;
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        // objItem.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        //long Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        // objItem.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objItem.IndentDetailsID = Id;
                        //objItem.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        //objItem.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        //objItem.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));

                        objBizAction.IssueItemDetailsList.Add(objItem);
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
        public override IValueObject AddIssueItemToStore(IValueObject valueObject, clsUserVO userVO)
        {

            clsAddIssueItemToStoreBizActionVO BizActionObj = valueObject as clsAddIssueItemToStoreBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsIssueItemVO objIssueItem = BizActionObj.IssueItemDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddIssueItemToStore");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objIssueItem.LinkServer);
                if (objIssueItem.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objIssueItem.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objIssueItem.IndentID);
                if (DALHelper.HandleDBNull(objIssueItem.IndentUnitID) == null)
                    objIssueItem.IndentUnitID = 0;
                dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, objIssueItem.IndentUnitID);

                if (DALHelper.IsValidDateRangeDB(objIssueItem.IssueDate) == true)
                    dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, objIssueItem.IssueDate);
                else
                    dbServer.AddInParameter(command, "IssueDate", DbType.DateTime, null);

                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.Int64, objIssueItem.IssueFromStoreId);
                dbServer.AddParameter(command, "IssueNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "IssueToStoreId", DbType.Int64, objIssueItem.IssueToStoreId);
                dbServer.AddInParameter(command, "IssuedByID", DbType.Int64, objIssueItem.ReceivedById);
                dbServer.AddInParameter(command, "Remark", DbType.String, objIssueItem.Remark);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objIssueItem.TotalAmount);
                dbServer.AddInParameter(command, "TotalItems", DbType.Decimal, objIssueItem.TotalItems);
                dbServer.AddInParameter(command, "TotalVATAmount", DbType.Decimal, objIssueItem.TotalVATAmount);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                dbServer.AddInParameter(command, "TotalTaxAmount", DbType.Decimal, objIssueItem.TotalTaxAmount);

                //By Anjali...........................................................

                dbServer.AddInParameter(command, "IsIndent", DbType.Int32, objIssueItem.IsIndent);

                if (objIssueItem.IsIndent == 1)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);

                }
                else if (objIssueItem.IsIndent == 0)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                }
                else if (objIssueItem.IsIndent == 2)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                }
                else if (objIssueItem.IsIndent == 3)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                }
                else if (objIssueItem.IsIndent == 4)
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                }
                else if (objIssueItem.IsIndent == 6) //***//
                {
                    dbServer.AddInParameter(command, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                }

                dbServer.AddInParameter(command, "IsApproved", DbType.Boolean, objIssueItem.IsApprovedDirect); //***//

                dbServer.AddInParameter(command, "ReasonForIssue", DbType.Int64, objIssueItem.ReasonForIssue);
                dbServer.AddInParameter(command, "IsFromStoreQuarantine", DbType.Boolean, objIssueItem.IsFromStoreQuarantine);
                dbServer.AddInParameter(command, "IsToStoreQuarantine", DbType.Boolean, objIssueItem.IsToStoreQuarantine);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objIssueItem.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objIssueItem.PatientUnitID);
                dbServer.AddInParameter(command, "IsPatientIndent", DbType.Boolean, objIssueItem.IsAgainstPatient);

                dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "IssueID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objIssueItem.IssueID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.IssueItemDetails.IssueID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                BizActionObj.IssueItemDetails.IssueNumber = (string)dbServer.GetParameterValue(command, "IssueNumber");


                int indentStatus = 0;
                decimal? pendingIndentQuantity;
                decimal? pendingIndentQuantityForIndentStatus;
                decimal? TotalIndentQuantity = 0;
                decimal? TotalIsssuedQty = 0;

                foreach (var item in BizActionObj.IssueItemDetails.IssueItemDetailsList)
                {
                    TotalIndentQuantity = TotalIndentQuantity + item.BalanceQty;
                    TotalIsssuedQty = TotalIsssuedQty + item.IssueQty;
                }
                pendingIndentQuantityForIndentStatus = TotalIndentQuantity - TotalIsssuedQty;
                foreach (var item in BizActionObj.IssueItemDetails.IssueItemDetailsList)
                {

                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddIssueItemListToStore");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objIssueItem.LinkServer);
                    if (objIssueItem.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objIssueItem.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "BatchId", DbType.Int64, item.BatchId);
                    dbServer.AddInParameter(command1, "ConversionFactor", DbType.Single, item.ConversionFactor);
                    if (DALHelper.IsValidDateRangeDB(item.ExpiryDate) == true)
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    else
                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, null);
                    dbServer.AddInParameter(command1, "IndentQty", DbType.Decimal, item.IndentQty);
                    dbServer.AddInParameter(command1, "IssueQty", DbType.Decimal, item.IssueQty);
                    dbServer.AddInParameter(command1, "ItemId", DbType.Int64, item.ItemId);

                    //dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount); //dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount); 
                    dbServer.AddInParameter(command1, "ItemVATAmount", DbType.Decimal, item.ItemVATAmount);
                    dbServer.AddInParameter(command1, "ItemVATPercentage", DbType.Decimal, item.ItemVATPercentage);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MainMRP); //item.MRP
                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Decimal, item.MainRate);  //item.PurchaseRate

                    //***Added by Ashish Z.
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);  // StockCF
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.StockingQuantity);

                    dbServer.AddInParameter(command1, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "IndentUOMID", DbType.Int64, item.UOMID);
                    //***	
                    dbServer.AddInParameter(command1, "BalanceQty", DbType.Decimal, item.BaseQuantity);
                    // dbServer.AddInParameter(command, "IssueFromStoreId", DbType.Int64, objIssueItem.IssueFromStoreId);

                    dbServer.AddInParameter(command1, "IndentId", DbType.Int64, objIssueItem.IndentID);
                    dbServer.AddInParameter(command1, "IndentUnitID", DbType.Int64, objIssueItem.IndentUnitID); //***//

                    dbServer.AddInParameter(command1, "GRNID", DbType.Int64, item.GRNID);
                    dbServer.AddInParameter(command1, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                    dbServer.AddInParameter(command1, "GRNDetailID", DbType.Int64, item.GRNDetailID);
                    dbServer.AddInParameter(command1, "GRNDetailUnitID", DbType.Int64, item.GRNDetailUnitID);

                    #region For Quarantine Items (Expired, DOS)

                    // Use For Vat/Tax Calculations

                    dbServer.AddInParameter(command1, "othertaxApplicableon", DbType.Int16, item.OtherGRNItemTaxApplicationOn);
                    dbServer.AddInParameter(command1, "otherTaxType", DbType.Int64, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command1, "VatApplicableon", DbType.Int64, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command1, "Vattype", DbType.Int64, item.GRNItemVatType);

                    dbServer.AddInParameter(command1, "InclusiveOfTax", DbType.Boolean, item.InclusiveOfTax);

                    #endregion

                    dbServer.AddInParameter(command1, "ItemTaxAmount", DbType.Decimal, item.ItemTaxAmount);
                    dbServer.AddInParameter(command1, "ItemTaxPercentage", DbType.Decimal, item.ItemTaxPercentage);
                    dbServer.AddInParameter(command1, "MRPAmount", DbType.Decimal, item.MRPAmt);
                    dbServer.AddInParameter(command1, "PurchaseRateAmount", DbType.Decimal, item.PurchaseRateAmt);

                    //By Anjali.............................................
                    //dbServer.AddInParameter(command1, "IsIndent", DbType.Boolean, item.IsIndent);                   
                    dbServer.AddInParameter(command1, "IsIndent", DbType.Int32, item.IsIndent);
                    if (item.IsIndent == 1)//item.IsIndent == true
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Indent);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);  // without Calculating of Tax and Vat

                    }
                    else if (item.IsIndent == 0)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.PurchaseRequisition);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount); // without Calculating of Tax and Vat
                    }
                    else if (item.IsIndent == 2)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineExpired);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount); // with Calculating of Tax and Vat
                    }
                    else if (objIssueItem.IsIndent == 3)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineDOS);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount);  // with Calculating of Tax and Vat
                    }
                    else if (objIssueItem.IsIndent == 4)
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.QuarantineGRNReturn);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.NetAmount);  // with Calculating of Tax and Vat
                    }

                    else if (objIssueItem.IsIndent == 6) //***//
                    {
                        dbServer.AddInParameter(command1, "InventoryIndentType", DbType.String, InventoryIndentType.Direct);
                        dbServer.AddInParameter(command1, "ItemTotalAmount", DbType.Decimal, item.ItemTotalAmount);  // with Calculating of Tax and Vat
                    }

                    dbServer.AddInParameter(command1, "ReasonForIssue", DbType.Int64, item.ReasonForIssue);
                    //.............................................................................


                    dbServer.AddInParameter(command1, "UnitId", DbType.Decimal, userVO.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "IssueFromStoreID", DbType.Int64, objIssueItem.IssueFromStoreId);
                    dbServer.AddInParameter(command1, "IssueToStoreID", DbType.Int64, objIssueItem.IssueToStoreId);
                    dbServer.AddInParameter(command1, "DepartmentID", DbType.Int64, 0);
                    dbServer.AddInParameter(command1, "TransactionTypeID", DbType.Int32, InventoryTransactionType.Issue);
                    dbServer.AddInParameter(command, "LoginUserId", DbType.Int64, userVO.ID);
                    dbServer.AddInParameter(command, "MachineName", DbType.String, userVO.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                    dbServer.AddInParameter(command1, "IssueId", DbType.Int64, BizActionObj.IssueItemDetails.IssueID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                    if (BizActionObj.SuccessStatus == -10)
                    {
                        throw new Exception();
                    }

                    //added by pallavi
                    if (item.IndentQty != null)
                    {
                        pendingIndentQuantity = item.IssueQty * Convert.ToDecimal(item.BaseConversionFactor);//pendingIndentQuantity = item.IssueQty;
                        if (item.IsIndent == 0)  //for PR
                        {
                            indentStatus = 0;
                        }
                        else if (item.IsIndent == 1)  //for Indent
                        {
                            indentStatus = 1;
                        }
                        //if (pendingIndentQuantityForIndentStatus == 0)
                        //    indentStatus = 4;
                        //else
                        //{

                        //    indentStatus = 3;
                        //}

                        UpdateIndentStatus(indentStatus, objIssueItem.IndentID, item.IndentUnitID, pendingIndentQuantity, item.IndentDetailsID);
                    }
                    else if (item.GRNQty != null && item.IsIndent == 4)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateGRNQSPendingQty");
                        command2.Connection = con;
                        dbServer.AddInParameter(command2, "GRNID", DbType.Int64, item.GRNID);
                        dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                        dbServer.AddInParameter(command2, "GRNDetailID", DbType.Int64, item.GRNDetailID);
                        dbServer.AddInParameter(command2, "GRNDetailUnitID", DbType.Int64, item.GRNDetailUnitID);
                        dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.BaseQuantity);
                        dbServer.AddInParameter(command2, "IsFreeItem", DbType.Boolean, item.IsGRNFreeItem);
                        int status2 = dbServer.ExecuteNonQuery(command2, trans);
                    }

                    item.StockDetails.BatchID = item.BatchId;
                    item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    item.StockDetails.ItemID = item.ItemId;
                    item.StockDetails.TransactionTypeID = InventoryTransactionType.Issue;
                    item.StockDetails.TransactionID = BizActionObj.IssueItemDetails.IssueID;
                    //if (item.IsIndent == 0)//item.IsIndent == false
                    //{
                    //    decimal _temp = Convert.ToDecimal(item.ConversionFactor);
                    //    item.StockDetails.TransactionQuantity = Convert.ToDouble(item.IssueQty * _temp);
                    //}
                    //else
                    //{
                    //    item.StockDetails.TransactionQuantity = item.BaseQuantity;// Convert.ToDouble(item.IssueQty);

                    //}
                    item.StockDetails.TransactionQuantity = item.BaseQuantity;
                    item.StockDetails.Date = objIssueItem.IssueDate;
                    item.StockDetails.Time = System.DateTime.Now;
                    item.StockDetails.StoreID = objIssueItem.IssueFromStoreId;

                    item.StockDetails.StockingQuantity = item.StockingQuantity;
                    item.StockDetails.BaseUOMID = item.BaseUOMID;
                    item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    item.StockDetails.SUOMID = item.SUOMID;
                    item.StockDetails.ConversionFactor = Convert.ToDouble(item.ConversionFactor);   // StockCF
                    item.StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;   //TransactionUOMID
                    item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.IssueQty);
                    item.StockDetails.ExpiryDate = item.ExpiryDate;

                    //added by Ashish Z. for Non Batch Item on 11Apr16
                    item.StockDetails.IsFromOpeningBalance = true;   // 
                    item.StockDetails.MRP = Convert.ToDouble(item.MainMRP);  //item.MRP
                    item.StockDetails.PurchaseRate = Convert.ToDouble(item.MainRate);  //item.PurchaseRate
                    //End

                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    obj.Details = item.StockDetails;
                    obj.Details.ID = 0;
                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, userVO, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    item.StockDetails.ID = obj.Details.ID;
                }


                //UPDATE INDENT STATUS
                //DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateIndentStatus");
                //dbServer.AddInParameter(command3, "IndentId", DbType.Int64, objIssueItem.IndentID);
                //dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
                ////UPDATE INDENT STATUS


                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(userVO.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                if (BizActionObj.SuccessStatus != -10)
                    BizActionObj.SuccessStatus = -1;
                BizActionObj.IssueItemDetails = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;
        }

        //Added By Pallavi
        public void UpdateIndentStatus(int indentStatus, long? indentId, long? UnitID, decimal? balanceQty, long IndentDetailsID)
        {

            try
            {

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateIndentStatus");

                dbServer.AddInParameter(command, "IndentStatus", DbType.Int32, indentStatus);
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, indentId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UnitID);
                dbServer.AddInParameter(command, "BalanceQty", DbType.Decimal, balanceQty);
                dbServer.AddInParameter(command, "IndentDetailsID", DbType.Int64, IndentDetailsID);

                int intStatus1 = dbServer.ExecuteNonQuery(command);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }

        }


        public override IValueObject GetItemListByIndentIdSrch(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByIndentIdSrchBizActionVO objBizAction = valueObject as clsGetItemListByIndentIdSrchBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                if (objBizAction.TransactionType == InventoryTransactionType.Issue)
                {
                    valueObject = GetItemListByIndentIdSrch1(valueObject, userVO);
                    return valueObject;

                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch");
                    //command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch2");


                    dbServer.AddInParameter(command, "IndentId", DbType.Int64, objBizAction.IndentId);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                    reader = (DbDataReader)dbServer.ExecuteReader(command);
                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            clsItemListByIndentId item = new clsItemListByIndentId();

                            item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                            item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                            item.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                            item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                            item.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                            item.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                            item.PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                            objBizAction.ItemList.Add(item);

                        }





                    }
                    reader.NextResult();
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (reader != null && reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;

        }

        private IValueObject GetItemListByIndentIdSrch1(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByIndentIdSrchBizActionVO objBizAction = valueObject as clsGetItemListByIndentIdSrchBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {

                //command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch");
                command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIndentIdSrch2");


                dbServer.AddInParameter(command, "IndentId", DbType.Int64, objBizAction.IndentId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);
                dbServer.AddInParameter(command, "FromPO", DbType.Boolean, objBizAction.IssueIndentFlag);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsItemListByIndentId item = new clsItemListByIndentId();

                        item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"])); // RequiredQuantity  (User input qty)
                        item.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        item.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        item.PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        item.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.IssuePendingQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuePendingQuantity"]));

                        item.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUOM"]));
                        item.StockConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockCF"])); //StockCF

                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        item.BaseConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));

                        //if (item.IssueIndentFlag == false)
                        if (objBizAction.IssueIndentFlag == false)
                        {
                            if (item.IssuePendingQuantity <= 0)
                                continue;
                        }



                        item.IssueQty = Convert.ToDecimal("0");
                        item.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        item.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        item.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        item.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        item.VATPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));

                        //Added By MMBABU
                        item.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        item.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));
                        item.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));

                        //By Anjali....................

                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //item.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        //................................
                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));

                        item.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        item.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        item.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        item.ItemTaxPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"]));
                        item.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                        item.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));

                        objBizAction.ItemList.Add(item);



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

        public override IValueObject GetIndentListBySupplier(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIndentListBySupplierBizActionVO objBizAction = valueObject as clsGetIndentListBySupplierBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetIndentListBySupplier");

                dbServer.AddInParameter(command, "FromIndentStoreId", DbType.String, objBizAction.FromIndentStoreId);
                dbServer.AddInParameter(command, "ToIndentStoreId", DbType.String, objBizAction.ToIndentStoreId);

                if (objBizAction.FromDate != null && (int)(objBizAction.FromDate.ToOADate()) != 0)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);

                if (objBizAction.ToDate != null && (int)(objBizAction.ToDate.ToOADate()) != 0)
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "IndentNumber", DbType.String, objBizAction.SIndentNumber);
                dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.SItemName);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizAction.SSupplierID);
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizAction.Freezed);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsItemListByIndentId item = new clsItemListByIndentId();

                        item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        item.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        //item.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        if (Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                            item.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"]));
                        else
                            item.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));

                        item.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        item.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        item.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        item.PUM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                        item.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        item.IssuePendingQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuePendingQuantity"]));
                        item.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        //if (item.IssueIndentFlag == false)
                        if (objBizAction.IssueIndentFlag == false)
                        {
                            if (item.IssuePendingQuantity <= 0)
                                continue;
                        }

                        //item.ConversionFactor = Convert.ToInt64(Convert.ToSingle(reader["ConversionFactor"]));
                        item.IssueQty = Convert.ToDecimal("0");

                        item.IndentDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailsID"]));
                        item.IndentDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailsUnitID"]));
                        item.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        item.FromStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["FromStoreID"]));
                        item.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        item.VATPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"]));
                        item.ItemVatPer = (decimal)DALHelper.HandleDBNull(reader["othertax"]);
                        item.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                        item.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                        item.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                        item.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));

                        //Added By MMBABU
                        item.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        item.Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));
                        item.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
                        item.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        item.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));

                        //Added By CDS
                        //UOM     BaseUOMID    SUOM  StockCF  StockingQuantity  BaseCF TransUOM BaseUOM

                        item.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"]));
                        item.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOM"]));

                        item.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        item.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        item.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUOM"]));
                        //item.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        item.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        item.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        item.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        item.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        item.PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        //item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                        //item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                        item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));

                        item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        item.POApprItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POApprItemQty"]));
                        item.POPendingItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POPendingItemQty"]));
                        //Added By Bhushanp For GST 21062017
                        item.SGSTPercent = (decimal)DALHelper.HandleDBNull(reader["SGSTTax"]);
                        item.CGSTPercent = (decimal)DALHelper.HandleDBNull(reader["CGSTTax"]);
                        item.IGSTPercent = (decimal)DALHelper.HandleDBNull(reader["IGSTTax"]);
                        item.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        item.SGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                        item.SGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        item.CGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                        item.CGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        item.IGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                        item.IGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));

                        //------------------------------------------
                        //END
                        item.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));  // added on 09042018 for Total Batch AvailableStock
                        objBizAction.ItemList.Add(item);



                    }



                }
                reader.NextResult();
                objBizAction.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");


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
        public override IValueObject GetIssueList(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetIssueListBizActionVO objBizAction = valueObject as clsGetIssueListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                if (objBizAction.IndentCriteria == 4)
                {
                    objBizAction.IndentCriteria = 4;
                    command = dbServer.GetStoredProcCommand("CIMS_GetIssueListForReturn");
                    dbServer.AddInParameter(command, "IsFromReceiveIssue", DbType.Boolean, objBizAction.IsFromReceiveIssue);
                    dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBizAction.PatientID);
                    dbServer.AddInParameter(command, "PatientunitID", DbType.Int64, objBizAction.PatientunitID);
                    dbServer.AddInParameter(command, "IsAgainstPatient", DbType.Boolean, objBizAction.IsAgainstPatient);

                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetIssueList");
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);
                    dbServer.AddInParameter(command, "intIsIndent", DbType.Int32, objBizAction.intIsIndent);

                    if (objBizAction.MRNo != null && objBizAction.MRNo.Length != 0)
                        dbServer.AddInParameter(command, "MrNo", DbType.String, "%" + objBizAction.MRNo + "%");
                    

                    if (objBizAction.PatientName != null && objBizAction.PatientName.Length != 0)
                        dbServer.AddInParameter(command, "PatientName", DbType.String, objBizAction.PatientName + "%");

                }

                dbServer.AddInParameter(command, "IndentCriteria", DbType.Int32, objBizAction.IndentCriteria);

                dbServer.AddInParameter(command, "IssueNumber", DbType.String, objBizAction.IssueDetailsVO.IssueNumber);

                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueFromDate) == true)
                    //dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, objBizAction.IssueFromDate.Value.ToString("yyyy-MM-dd"));
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, objBizAction.IssueFromDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, null);


                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueToDate) == true)
                    //dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, objBizAction.IssueToDate.Value.ToString("yyyy-MM-dd"));
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, objBizAction.IssueToDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, null);


                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.Int64, objBizAction.IssueFromStoreId);
                dbServer.AddInParameter(command, "IssueToStoreId", DbType.Int64, objBizAction.IssueToStoreId);
                if (objBizAction.flagReceiveIssue == true)
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, 0);
                else
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);   //userVO.UserLoginInfo.UnitId
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizAction.InputSortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO();

                        item.IssueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]));
                        item.IssueFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]));
                        item.IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"]));
                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.IssueToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]));
                        item.IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"]));
                        item.ReceivedById = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]));
                        item.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        // Added By Rohit As Per the Discussion With Nilesh Ingale Sir 
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));

                        //By Anjali...........................................
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //item.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        item.IsFromStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]));
                        item.IsToStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"]));
                        //..................................................................................

                        // End

                        if (objBizAction.IndentCriteria == 4)
                        {
                            item.IsAgainstPatient = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsPatientIndent"]));
                            item.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                            item.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));                          
                        }

                        //***//------                   
                       
                            item.MRNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRNO"]));
                            item.PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"]));                        
                        //---------------

                            if (objBizAction.IndentCriteria != 4) //***//
                            {
                                item.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApprovedDirect"]));                               
                            }
                        objBizAction.IssueList.Add(item);

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
            return objBizAction;

        }

        public override IValueObject GetItemListByIssueId(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByIssueIdBizActionVO objBizAction = valueObject as clsGetItemListByIssueIdBizActionVO;

            DbCommand command;
            DbCommand command1;
            DbCommand command2;
            DbDataReader reader = null;
            //
            try
            {
                if (objBizAction.TransactionType == InventoryTransactionType.ReceiveItemReturn)
                {

                    valueObject = GetReceivedAgainstIssueItemList(valueObject, userVO);
                    return valueObject;

                }

                if (objBizAction.flagReceivedIssue == false)
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIssueId");
                else
                    command = dbServer.GetStoredProcCommand("CIMS_GetItemListByIssueIdForReceive");

                dbServer.AddInParameter(command, "IssueId", DbType.Int64, objBizAction.IssueId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);



                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsIssueItemDetailsVO>();

                    if (objBizAction.ReturnItemList == null)
                        objBizAction.ReturnItemList = new List<clsReturnItemDetailsVO>();


                    if (objBizAction.ReceivedItemList == null)
                        objBizAction.ReceivedItemList = new List<clsReceivedItemDetailsVO>();

                    decimal BalQty = 0;
                    decimal BalQty1 = 0;

                    while (reader.Read())
                    {
                        clsIssueItemDetailsVO objItem = new clsIssueItemDetailsVO();

                        objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItem.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItem.ExpiryDate = (DateTime?)(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));


                        objItem.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));

                        //objItem.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ItemTotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        objItem.ItemTotalCost = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItem.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItem.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        if(objBizAction.flagReceivedIssue == false) //***//
                        {
                            objItem.MRPAmt = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRPAmount"]));
                        }
                        objItem.NetPayBeforeVATAmount = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["NetPayBeforeVATAmount"]));
                        objItem.TotalForVAT = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["ToDecimal"]));
                        objItem.UnverifiedStock = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["UnverifiedStock"]));
                        objItem.VATInclusive = 0;//Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATInclusive"]));
                        //objItem.AvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.BaseConversionFactor = Convert.ToSingle((DALHelper.HandleDBNull(reader["BaseCF"])));

                        objItem.IndentUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IndentUOM"]));
                        objItem.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));

                        objItem.ReasonForIssue = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReasonForIssue"]));
                        objItem.ReasonIssueDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]));

                        objItem.Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objItem.Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objItem.Bin = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objItem.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));

                        objBizAction.ItemList.Add(objItem);


                        clsReturnItemDetailsVO objItemReturn = new clsReturnItemDetailsVO();
                        //objItemReturn.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItemReturn.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemReturn.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        //objItemReturn.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItemReturn.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        //objItemReturn.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        objItemReturn.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"]));
                        objItemReturn.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItemReturn.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemReturn.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemReturn.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objItemReturn.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        if (objBizAction.flagReceivedIssue == true)
                            objItemReturn.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        //By Anjali.................................
                        objItemReturn.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        // objItemReturn.IsIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIndent"]));

                        objItemReturn.ReasonForIssue = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReasonForIssue"]));
                        objItemReturn.ReasonIssueDescription = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]));
                        //................................................

                        if (objBizAction.flagReceivedIssue == true)
                        {
                            command2 = dbServer.GetStoredProcCommand("CIMS_GetReceiveQuantityAgainstIssue");
                            dbServer.AddInParameter(command2, "IssueId", DbType.Int64, objBizAction.IssueId);
                            dbServer.AddInParameter(command2, "ToStoreID", DbType.Int64, Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"])));
                            dbServer.AddInParameter(command2, "ItemID", DbType.Int64, Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])));

                            dbServer.AddOutParameter(command2, "ReceivedQuantity", DbType.Decimal, int.MaxValue);
                            int intStatus1 = dbServer.ExecuteNonQuery(command2);
                            if (DALHelper.HandleDBNull(dbServer.GetParameterValue(command2, "ReceivedQuantity")) == null)
                                objItemReturn.BalanceQty = 0;
                            else
                                objItemReturn.BalanceQty = (decimal)dbServer.GetParameterValue(command2, "ReceivedQuantity");
                        }



                        objItemReturn.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objBizAction.ReturnItemList.Add(objItemReturn);


                        clsReceivedItemDetailsVO objItemReceived = new clsReceivedItemDetailsVO();
                        objItemReceived.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItemReceived.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemReceived.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        //objItemReceived.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objItemReceived.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        //objItemReceived.IndentQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["RequiredQuantity"]));
                        objItemReceived.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssuedQuantity"]));
                        objItemReceived.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        objItemReceived.IssueQtyBaseCF = Convert.ToSingle((DALHelper.HandleDBNull(reader["BaseCF"])));
                        objItemReceived.BaseConversionFactor = Convert.ToSingle((DALHelper.HandleDBNull(reader["BaseCF"])));
                        objItemReceived.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));

                        objItemReceived.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));
                        objItemReceived.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));

                        objItemReceived.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItemReceived.ConversionFactor = Convert.ToSingle((DALHelper.HandleDBNull(reader["StockCF"])));
                        objItemReceived.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));

                        objItemReceived.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItemReceived.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemReceived.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem.ItemTotalAmount =  Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTotalAmount"]));
                        //objItem.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATAmount"]));
                        objItemReceived.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATPercentage"]));
                        objItemReceived.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BatchMRP"]));
                        // objItemReceived.NetPayBeforeVATAmount = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["NetPayBeforeVATAmount"]));
                        //objItemReceived.TotalForVAT = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["ToDecimal"]));
                        //objItemReceived.UnverifiedStock = 0;// Convert.ToDecimal(DALHelper.HandleDBNull(reader["UnverifiedStock"]));
                        //objItemReceived.VATInclusive = 0;//Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATInclusive"]));
                        //objItem.AvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItemReceived.IssueDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueID"]));

                        /*
                        command2 = dbServer.GetStoredProcCommand("GetTransitBalance");
                        dbServer.AddInParameter(command2, "IssueId", DbType.Int64, objBizAction.IssueId);
                        dbServer.AddInParameter(command2, "ItemId", DbType.Int64, objItemReceived.ItemId);
                        dbServer.AddInParameter(command2, "IssueQuantity", DbType.Decimal, objItemReceived.IssueQty);
                        dbServer.AddOutParameter(command2, "TransitQty", DbType.Decimal, 0);
                        dbServer.AddOutParameter(command2, "RecvQty", DbType.Decimal, 0);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        //BalQty = GetTransitBalance(objItemReceived.ItemId, objBizAction.IssueId, objItemReceived.IssueQty);
                        if (DALHelper.HandleDBNull(dbServer.GetParameterValue(command2, "TransitQty")) == null)
                            objItemReceived.BalanceQty = 0;
                        else
                        objItemReceived.BalanceQty = (decimal)dbServer.GetParameterValue(command2, "TransitQty");

                        if (DALHelper.HandleDBNull(dbServer.GetParameterValue(command2, "RecvQty")) == null)
                            objItemReceived.ReceivedQty = 0;
                        else
                          objItemReceived.ReceivedQty = (decimal)dbServer.GetParameterValue(command2, "RecvQty");
                        */

                        //objItemReceived.ConversionFactor = Convert.ToInt64((DALHelper.HandleDBNull(reader["ConversionFactor"])));

                        objItemReceived.BalanceQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PendingQuantity"]));


                        //By Anjali..........................
                        //objItemReceived.IsIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsIndent"]));
                        objItemReceived.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));

                        objItemReceived.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));

                        //............................................................................

                        //objItemReceived.BalanceQty = BalQty;
                        objItemReceived.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItemReceived.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));
                        objItemReceived.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                        objItemReceived.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                        objItemReceived.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                        objItemReceived.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));

                        objItemReceived.ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonIssueDescription"]));

                        if (objBizAction.flagReceivedIssue == false)
                        {
                            #region For Quarantine Items (Expired, DOS)

                            objItemReceived.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                            objItemReceived.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
                            objItemReceived.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                            objItemReceived.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));

                            objItemReceived.ItemTaxPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxPercentage"]));
                            objItemReceived.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));

                            objItemReceived.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));

                            #endregion
                        }

                        objBizAction.ReceivedItemList.Add(objItemReceived);

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

                if (reader != null && reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }


        private IValueObject GetReceivedAgainstIssueItemList(IValueObject valueObject, clsUserVO userVO)
        {

            clsGetItemListByIssueIdBizActionVO objBizAction = valueObject as clsGetItemListByIssueIdBizActionVO;

            DbCommand command;
            DbCommand command1;
            DbCommand command2;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_ReceivedItemAgainstIssueDetails");
                dbServer.AddInParameter(command, "IssueId", DbType.Int64, objBizAction.IssueId);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);

                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objBizAction.PatientID);
                dbServer.AddInParameter(command, "PatientunitID", DbType.Int64, objBizAction.PatientunitID);
                dbServer.AddInParameter(command, "IsAgainstPatient", DbType.Boolean, objBizAction.IsAgainstPatient);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ReturnItemList == null)
                        objBizAction.ReturnItemList = new List<clsReturnItemDetailsVO>();
                    decimal BalQty = 0;
                    decimal BalQty1 = 0;

                    while (reader.Read())
                    {
                        clsReturnItemDetailsVO objItemReturn = new clsReturnItemDetailsVO();
                        objItemReturn.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemReturn.BatchId = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItemReturn.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItemReturn.IssueQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IssueQty"]));
                        BalQty1 = Convert.ToDecimal(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        objItemReturn.BalanceQty = BalQty1;
                        objItemReturn.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        objItemReturn.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItemReturn.ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemReturn.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemReturn.ItemVATPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemVATPercentage"]));
                        objItemReturn.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        objItemReturn.ToStoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ToStoreID"]));
                        objItemReturn.ReceivedItemDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemReturn.ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"]));
                        objItemReturn.PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItemReturn.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItemReturn.AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        objItemReturn.ConversionFactor = Convert.ToInt64(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        //By Anjali.......................................................
                        objItemReturn.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        objItemReturn.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        objItemReturn.IssuedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["IssuedUOM"]));

                        objItemReturn.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItemReturn.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));

                        objItemReturn.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItemReturn.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));  // TransactionUOM
                        objItemReturn.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItemReturn.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItemReturn.ReceivedQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItemReturn.ReceivedUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        objItemReturn.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));  // Pending Qty UOM


                        objItemReturn.TotalPatientIndentReceiveQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalPatientIndentReceiveQty"]));
                        objItemReturn.TotalPatientIndentConsumptionQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalPatientIndentConsumptionQty"]));

                        objBizAction.ReturnItemList.Add(objItemReturn);
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


        private decimal GetTransitBalance(long? itemId, long? issueId, decimal? issueQuantity)
        {
            DbConnection con = dbServer.CreateConnection();
            DbCommand command;
            DbDataReader reader = null;
            try
            {

                decimal qty = 0;
                con.Open();

                command = dbServer.GetStoredProcCommand("GetTransitBalance");
                dbServer.AddInParameter(command, "IssueId", DbType.Int64, issueId);
                dbServer.AddInParameter(command, "ItemId", DbType.Int64, itemId);
                dbServer.AddInParameter(command, "IssueQuantity", DbType.Int64, issueQuantity);
                //dbServer.AddOutParameter(command, "TransitQty", DbType.Decimal,int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                while (reader.Read())
                {
                    qty = Convert.ToDecimal(DALHelper.HandleDBNull(reader[0]));
                }
                reader.Close();
                return qty;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private decimal GetTransitReturnBalance(long? itemId, long? issueId)
        {
            DbConnection con = dbServer.CreateConnection();
            DbCommand command;
            DbDataReader reader = null;
            try
            {

                decimal qty = 0;
                con.Open();

                command = dbServer.GetStoredProcCommand("GetReturnTransitBalance");
                dbServer.AddInParameter(command, "IssueId", DbType.Int64, issueId);
                dbServer.AddInParameter(command, "ItemId", DbType.Int64, itemId);
                //dbServer.AddOutParameter(command, "TransitQty", DbType.Decimal,int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                while (reader.Read())
                {
                    qty = Convert.ToDecimal(DALHelper.HandleDBNull(reader[0]));
                }
                reader.Close();
                return qty;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public override IValueObject GetIssueListQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIssueListBizActionVO objBizAction = valueObject as clsGetIssueListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                if (objBizAction.IndentCriteria == 4)
                {
                    objBizAction.IndentCriteria = 4;
                    //command = dbServer.GetStoredProcCommand("CIMS_GetIssueListForReturn")
                    command = dbServer.GetStoredProcCommand("CIMS_GetIssueListForReturnQS");   // Use to get already saved Issued Items to Quarantine Stores Only  
                    dbServer.AddInParameter(command, "IsFromReceiveIssue", DbType.Boolean, objBizAction.IsFromReceiveIssue);
                    dbServer.AddInParameter(command, "IsQSOnly", DbType.Boolean, objBizAction.IsQSOnly); // Use to get already saved Issued Items to Quarantine Stores Only
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_GetIssueList");
                    dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);
                }

                dbServer.AddInParameter(command, "IndentCriteria", DbType.Int32, objBizAction.IndentCriteria);

                dbServer.AddInParameter(command, "IssueNumber", DbType.String, objBizAction.IssueDetailsVO.IssueNumber);

                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueFromDate) == true)
                    //dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, objBizAction.IssueFromDate.Value.ToString("yyyy-MM-dd"));
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, objBizAction.IssueFromDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, null);


                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueToDate) == true)
                    //dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, objBizAction.IssueToDate.Value.ToString("yyyy-MM-dd"));
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, objBizAction.IssueToDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, null);


                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.Int64, objBizAction.IssueFromStoreId);
                dbServer.AddInParameter(command, "IssueToStoreId", DbType.Int64, objBizAction.IssueToStoreId);
                if (objBizAction.flagReceiveIssue == true)
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, 0);
                else
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizAction.InputSortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO();

                        item.IssueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]));
                        item.IssueFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]));
                        item.IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"]));
                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.IssueToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]));
                        item.IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"]));
                        item.ReceivedById = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]));
                        item.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        // Added By Rohit As Per the Discussion With Nilesh Ingale Sir 
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));

                        //By Anjali...........................................
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        //item.IsIndent = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsIndent"]));
                        item.IsFromStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]));
                        item.IsToStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"]));
                        //..................................................................................

                        // End

                        if (objBizAction.IndentCriteria == 4)
                            item.ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForIssueName"]));  //use to show whether its a issue against Damaged,Obsolete,Scrap,Expired (DOSE)

                        objBizAction.IssueList.Add(item);

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
            return objBizAction;
        }

        public override IValueObject GetGRNToQSIssueList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetIssueListBizActionVO objBizAction = valueObject as clsGetIssueListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetGRNToQSIssueList");   // Use to get already saved Issued Items to Quarantine Stores Only  
                dbServer.AddInParameter(command, "IsForGRNQS", DbType.Boolean, objBizAction.IsForGRNQS); // Use to set on ReceiveGRNToQS form while getting Records for Issue.
                dbServer.AddInParameter(command, "IndentCriteria", DbType.Int32, objBizAction.IndentCriteria);
                dbServer.AddInParameter(command, "IssueNumber", DbType.String, objBizAction.IssueDetailsVO.IssueNumber);
                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueFromDate) == true)
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, objBizAction.IssueFromDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueFromDate", DbType.DateTime, null);

                if (DALHelper.IsValidDateRangeDB(objBizAction.IssueToDate) == true)
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, objBizAction.IssueToDate.Value);
                else
                    dbServer.AddInParameter(command, "IssueToDate", DbType.DateTime, null);


                dbServer.AddInParameter(command, "IssueFromStoreId", DbType.Int64, objBizAction.IssueFromStoreId);
                dbServer.AddInParameter(command, "IssueToStoreId", DbType.Int64, objBizAction.IssueToStoreId);
                if (objBizAction.flagReceiveIssue == true)
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, 0);
                else
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizAction.InputSortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        clsIssueListVO item = new clsIssueListVO();

                        item.IssueDate = Convert.ToDateTime(DALHelper.HandleDate(reader["IssueDate"]));
                        item.IssueFromStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueFromStoreId"]));
                        item.IssueFromStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueFromStoreName"]));
                        item.IssueId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueId"]));
                        item.IssueNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IssueNumber"]));
                        item.IssueToStoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssueToStoreId"]));
                        item.IssueToStoreName = Convert.ToString(DALHelper.HandleDBNull(reader["IssueToStoreName"]));
                        item.ReceivedById = Convert.ToInt64(DALHelper.HandleDBNull(reader["IssuedByID"]));
                        item.ReceivedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedByName"]));
                        item.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        item.TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"]));

                        item.IndentId = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentId"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        item.IssueUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        item.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        item.IsIndent = Convert.ToInt32(DALHelper.HandleDBNull(reader["IsIndent"]));
                        item.IsFromStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromStoreQuarantine"]));
                        item.IsToStoreQuarantine = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsToStoreQuarantine"]));
                        item.ReasonForIssueName = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForIssueName"]));  //use to show whether its a issue against Damaged,Obsolete,Scrap,Expired (DOSE)

                        objBizAction.IssueList.Add(item);

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
            return objBizAction;
        }



    }
}
