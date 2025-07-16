using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsExpiryItemDAL : clsBaseExpiryItemDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsExpiryItemDAL()
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

        public override IValueObject GetExpiryItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredItemListBizActionVO objBizAction = valueObject as clsGetExpiredItemListBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetExpiryItemList");

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizAction.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizAction.SupplierID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, objBizAction.objExpiredItemsVO.ItemName);
                dbServer.AddInParameter(command, "BatchCode", DbType.String, objBizAction.objExpiredItemsVO.BatchCode);
                dbServer.AddInParameter(command, "ReceivedNo", DbType.String, objBizAction.objExpiredItemsVO.ReceivedNo);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "IsFromDOS", DbType.Boolean, objBizAction.IsFromDOS); // Set true from ScrapSale.xaml.cs
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizAction.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ExpiredItemList == null)
                        objBizAction.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO objItem = new clsExpiredItemReturnDetailVO();

                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.BatchExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        objItem.ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItem.ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objItem.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));   //AvailableStockUOM
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItem.StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        objItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItem.ItemTax = Convert.ToDouble(Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxPercentage"])));
                        objItem.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objItem.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objItem.VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ItemVATPercentage"]));
                        objItem.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objItem.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));
                        objItem.ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"]));
                        objItem.ReceivedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedUnitID"]));
                        objItem.ReceivedDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailID"]));
                        objItem.ReceivedDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailUnitID"]));
                        objItem.MainRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"]));
                        objItem.MainMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));
                        objItem.PurchaseRate = objItem.MainRate * objItem.BaseConversionFactor;
                        objItem.MRP = objItem.MainMRP * objItem.BaseConversionFactor;
                        objItem.PendingQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        objItem.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objItem.ReceivedDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReceivedDate"]));


                        //objItem.SelectedUOM = new MasterListItem { objItem.TransactionUOMID, objItem.ReceivedQtyUOM };

                        //objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        //objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        //objItem.Conversion = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        //objItem.ReturnQty = objItem.AvailableStock;
                        //objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        //objItem.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        //objItem.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUOM"]));
                        //objItem.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        //objItem.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));

                        objBizAction.ExpiredItemList.Add(objItem);
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

        public override IValueObject AddExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpiryItemReturnBizActionVO BizActionObj = valueObject as clsAddExpiryItemReturnBizActionVO;

            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsExpiredItemReturnVO objExpiryReturn = BizActionObj.objExpiryItem;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemExpiryReturn");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objExpiryReturn.LinkServer);
                if (objExpiryReturn.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objExpiryReturn.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objExpiryReturn.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objExpiryReturn.SupplierID);
                dbServer.AddInParameter(command, "ExpiryReturnNo", DbType.String, objExpiryReturn.ExpiryReturnNo);
                //dbServer.AddParameter(command, "ExpiryReturnNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, objExpiryReturn.ExpiryReturnNo);
                dbServer.AddInParameter(command, "GateEntryNo", DbType.String, objExpiryReturn.GateEntryNo);
                dbServer.AddInParameter(command, "Date", DbType.DateTime, objExpiryReturn.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objExpiryReturn.Time);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objExpiryReturn.TotalAmount);
                dbServer.AddInParameter(command, "TotalVATAmount", DbType.Double, objExpiryReturn.TotalVATAmount);
                dbServer.AddInParameter(command, "TotalTaxAmount", DbType.Double, objExpiryReturn.TotalTaxAmount);
                dbServer.AddInParameter(command, "TotalOctriAmount", DbType.Double, objExpiryReturn.TotalOctriAmount);
                dbServer.AddInParameter(command, "OtherDeducution", DbType.Double, objExpiryReturn.OtherDeducution);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objExpiryReturn.NetAmount);
                dbServer.AddInParameter(command, "Remark", DbType.String, objExpiryReturn.Remark);


                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objExpiryReturn.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                //if (objExpiryReturn.AddedOn != null) objExpiryReturn.AddedOn = objExpiryReturn.AddedOn.Trim();
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                //if (objExpiryReturn.AddedWindowsLoginName != null) objExpiryReturn.AddedWindowsLoginName = objExpiryReturn.AddedWindowsLoginName.Trim();
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "EditForApprove", DbType.Boolean, objExpiryReturn.EditForApprove);  // parameter used to maintain logs for table T_ItemExpiryReturn into T_ItemExpiryReturnHistory

                dbServer.AddParameter(command, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objExpiryReturn.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                //dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objExpiryReturn.ID);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                if (objExpiryReturn.ID == 0)
                {
                    BizActionObj.objExpiryItem.ExpiryReturnNo = Convert.ToString(dbServer.GetParameterValue(command, "Number"));
                }
                else
                {
                    BizActionObj.objExpiryItem.ExpiryReturnNo = objExpiryReturn.ExpiryReturnNo;
                }

                BizActionObj.objExpiryItem.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                //BizActionObj.objExpiryItem.ExpiryReturnNo = (string)dbServer.GetParameterValue(command, "ExpiryReturnNo");

                foreach (var item in BizActionObj.objExpiryItemDetailList)
                {
                    item.ItemExpiryReturnID = BizActionObj.objExpiryItem.ID;
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemExpiryReturnDetails");
                    command1.Connection = con;

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objExpiryReturn.LinkServer);
                    if (objExpiryReturn.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objExpiryReturn.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId); //item.UnitID
                    dbServer.AddInParameter(command1, "ItemExpiryReturnID", DbType.Int64, item.ItemExpiryReturnID);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "BatchID", DbType.Int64, item.BatchID);
                    dbServer.AddInParameter(command1, "BatchExpiryDate", DbType.DateTime, item.BatchExpiryDate);
                    dbServer.AddInParameter(command1, "Conversion", DbType.Double, item.Conversion);
                    dbServer.AddInParameter(command1, "ReturnQty", DbType.Double, item.ReturnQty);
                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Double, item.PurchaseRate);
                    dbServer.AddInParameter(command1, "TotalPurchaseRate", DbType.Double, item.TotalPurchaseRate);
                    dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);
                    dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.MRPAmount);
                    dbServer.AddInParameter(command1, "ReturnRate", DbType.Double, item.ReturnRate);

                    dbServer.AddInParameter(command1, "DiscountPercentage", DbType.Double, item.DiscountPercentage);
                    dbServer.AddInParameter(command1, "DiscountAmount", DbType.Double, item.DiscountAmount);
                    dbServer.AddInParameter(command1, "OctroiAmount", DbType.Double, item.OctroiAmount);
                    dbServer.AddInParameter(command1, "VATPercentage", DbType.Double, item.VATPercentage);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
                    dbServer.AddInParameter(command1, "TotalTaxAmount", DbType.Double, item.TotalTaxAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);


                    //By Anjali........................................
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Single, item.ReturnQty * item.BaseConversionFactor);                    // Base Quantity            // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);               // Transaction UOM      // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);                            // Base  UOM            // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.StockUOMID);                            // SUOM UOM                     // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);                    // Stocking ConversionFactor     // For Conversion Factor
                    //dbServer.AddInParameter(command1, "StockingQuantity", DbType.Single, item.BaseQuantity * item.ConversionFactor);  // StockingQuantity // For Conversion Factor


                    //.................................................

                    //Begin parameter used to maintain logs for table T_ItemExpiryReturnDetails into T_ItemExpiryReturnDetailsHistory
                    dbServer.AddInParameter(command1, "EditForApprove", DbType.Boolean, objExpiryReturn.EditForApprove);
                    dbServer.AddInParameter(command1, "DirectApprove", DbType.Boolean, objExpiryReturn.DirectApprove);
                    //End



                    #region For Quarantine Items (Expired, DOS)

                    // Use For Vat/Tax Calculations

                    dbServer.AddInParameter(command1, "othertaxApplicableon", DbType.Int16, item.OtherGRNItemTaxApplicationOn);
                    dbServer.AddInParameter(command1, "otherTaxType", DbType.Int64, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command1, "VatApplicableon", DbType.Int64, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command1, "Vattype", DbType.Int64, item.GRNItemVatType);

                    #endregion

                    dbServer.AddInParameter(command1, "TAXPercentage", DbType.Double, item.ItemTax);
                    dbServer.AddInParameter(command1, "TaxAmount", DbType.Double, item.TaxAmount);
                    dbServer.AddInParameter(command1, "ReceivedID", DbType.Int64, item.ReceivedID);
                    dbServer.AddInParameter(command1, "ReceivedUnitID", DbType.Int64, item.ReceivedUnitID);
                    dbServer.AddInParameter(command1, "ReceivedDetailID", DbType.Int64, item.ReceivedDetailID);
                    dbServer.AddInParameter(command1, "ReceivedDetailUnitID", DbType.Int64, item.ReceivedDetailUnitID);

                    //dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    //dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);

                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command1, "ID");

                    #region Commented

                    // Deduct Item Quantity From Item Stock
                    //long ID=0;
                    ////clsAddItemStockBizActionVO BizActionObj1 = valueObject as clsAddItemStockBizActionVO;
                    //try
                    //{                        

                    //    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddItemStock");

                    //    dbServer.AddInParameter(command2, "LinkServer", DbType.String, objExpiryReturn.LinkServer);
                    //    if (objExpiryReturn.LinkServer != null)
                    //        dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objExpiryReturn.LinkServer.Replace(@"\", "_"));

                    //    dbServer.AddInParameter(command2, "Date", DbType.DateTime, objExpiryReturn.Date);
                    //    dbServer.AddInParameter(command2, "Time", DbType.DateTime, objExpiryReturn.Time);
                    //    dbServer.AddInParameter(command2, "StoreID", DbType.Int64, objExpiryReturn.StoreID);
                    //    dbServer.AddInParameter(command2, "DepartmentID", DbType.Int64, 0);
                    //    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                    //    dbServer.AddInParameter(command2, "BatchID", DbType.Int64, item.BatchID);
                    //    dbServer.AddInParameter(command2, "TransactionTypeID", DbType.Int16, (Int16)InventoryTransactionType.ExpiryItemReturn);
                    //    dbServer.AddInParameter(command2, "TransactionID", DbType.Int64, item.ItemExpiryReturnID);
                    //    dbServer.AddInParameter(command2, "OperationType", DbType.Int16, (Int16)InventoryStockOperationType.Substration);
                    //    dbServer.AddInParameter(command2, "TransactionQuantity", DbType.Double, item.ReturnQty);
                    //    dbServer.AddInParameter(command2, "Remarks", DbType.String, InventoryTransactionType.ExpiryItemReturn.ToString());

                    //    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    //    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    //    dbServer.AddInParameter(command2, "Status", DbType.Boolean, objExpiryReturn.Status);
                    //    dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    //    //if (objExpiryReturn.AddedOn != null) objExpiryReturn.AddedOn = objExpiryReturn.AddedOn.Trim();
                    //    dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    //    dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    //    //if (objExpiryReturn.AddedWindowsLoginName != null) objExpiryReturn.AddedWindowsLoginName = objExpiryReturn.AddedWindowsLoginName.Trim();
                    //    dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    //    //dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, ID);
                    //    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                    //    dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);

                    //    intStatus = dbServer.ExecuteNonQuery(command2);
                    //    int SS = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    //    ID = (long)dbServer.GetParameterValue(command2, "ID");


                    //}
                    //catch (Exception ex)
                    //{
                    //    //throw;

                    //}
                    //finally
                    //{

                    //}

                    #endregion

                    #region Commented by Ashish Z. for Approve dated 030615
                    //objExpiryReturn.StockDetails.BatchID = Convert.ToInt64(item.BatchID);
                    //objExpiryReturn.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    //objExpiryReturn.StockDetails.ItemID = Convert.ToInt64(item.ItemID);
                    //objExpiryReturn.StockDetails.TransactionTypeID = InventoryTransactionType.ExpiryItemReturn;
                    //objExpiryReturn.StockDetails.TransactionID = Convert.ToInt64(item.ItemExpiryReturnID);
                    //objExpiryReturn.StockDetails.TransactionQuantity = Convert.ToDouble(item.ReturnQty);// * item.ConversionFactor);
                    //objExpiryReturn.StockDetails.Date = Convert.ToDateTime(objExpiryReturn.AddedDateTime);
                    //objExpiryReturn.StockDetails.Time = System.DateTime.Now;
                    //objExpiryReturn.StockDetails.StoreID = objExpiryReturn.StoreID;

                    //clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    //clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    //obj.Details = objExpiryReturn.StockDetails;


                    ////By Anjali...............
                    //obj.Details.InputTransactionQuantity = Convert.ToSingle(item.ReturnQty);  // InputTransactionQuantity // For Conversion Factor   
                    //obj.Details.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                    //obj.Details.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                    //obj.Details.TransactionQuantity = item.BaseQuantity;         // Base Quantity            // For Conversion Factor
                    //obj.Details.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                    //obj.Details.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                    //obj.Details.StockingQuantity = item.ReturnQty * item.ConversionFactor;  // StockingQuantity // For Conversion Factor
                    //obj.Details.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor     

                    ////..............................


                    //if (objExpiryReturn.EditForApprove == true)  //Apply Stock effect if Expiry Item Return is approve
                    //{
                    //    obj.Details.ID = 0;

                    //    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                    //    if (obj.SuccessStatus == -1)
                    //    {
                    //        throw new Exception();
                    //    }

                    //    objExpiryReturn.StockDetails.ID = obj.Details.ID;
                    //}
                    #endregion
                    if (item.ReceivedDetailID != null && item.ReceivedDetailID > 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command2.Connection = con;

                        dbServer.AddInParameter(command2, "ReceiveItemDetailsID", DbType.Int64, item.ReceivedDetailID);
                        dbServer.AddInParameter(command2, "UnitID", DbType.Int64, item.ReceivedDetailUnitID);
                        dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.ReturnQty * item.BaseConversionFactor);//(item.ReturnQty));
                        int status2 = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }

                if (objExpiryReturn.EditForApprove == true)
                {
                    clsBaseExpiryItemDAL objBaseDAL = clsBaseExpiryItemDAL.GetInstance();
                    clsUpdateExpiryForApproveRejectVO obj = new clsUpdateExpiryForApproveRejectVO();

                    obj.objExpiryItem = objExpiryReturn;

                    obj = (clsUpdateExpiryForApproveRejectVO)objBaseDAL.UpdateExpiryForApproveOrReject(obj, UserVo, con, trans);

                    if (obj.SuccessStatus == -1)
                        throw new Exception("Error");

                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.objExpiryItem = null;
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


        public override IValueObject ApproveExpiryItemReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddExpiryItemReturnBizActionVO BizActionObj = valueObject as clsAddExpiryItemReturnBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateExpiryItemReturnForApprove");
                command.Connection = con;
                dbServer.AddInParameter(command, "ItemExpiryReturnID", DbType.Int64, BizActionObj.objExpiryItem.ID);
                dbServer.AddInParameter(command, "ItemExpiryReturnUnitID", DbType.Int64, BizActionObj.objExpiryItem.UnitId);
                dbServer.AddInParameter(command, "ApproveOrRejectedBy", DbType.Int64, UserVo.ID);
                dbServer.AddParameter(command, "ReasultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ReasultStatus"));

                if (BizActionObj.SuccessStatus == -10)
                {
                    throw new Exception();
                }

                List<clsExpiredItemReturnDetailVO> objDetailsList = BizActionObj.objExpiryItemDetailList;
                if (objDetailsList.Count > 0)
                {
                    foreach (var item in objDetailsList.ToList())
                    {
                        item.StockDetails.PurchaseRate = item.PurchaseRate;
                        item.StockDetails.BatchID = Convert.ToInt64(item.BatchID);
                        item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        item.StockDetails.ItemID = item.ItemID;
                        item.StockDetails.TransactionTypeID = InventoryTransactionType.ExpiryItemReturn;
                        item.StockDetails.TransactionID = Convert.ToInt64(item.ItemExpiryReturnID);
                        item.StockDetails.TransactionQuantity = item.BaseQuantity;
                        item.StockDetails.Date = DateTime.Now;
                        item.StockDetails.Time = DateTime.Now;
                        item.StockDetails.StoreID = item.StoreId;
                        item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReturnQty);
                        item.StockDetails.BaseUOMID = item.BaseUOMID;
                        item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                        item.StockDetails.SUOMID = item.StockUOMID;
                        item.StockDetails.ConversionFactor = item.ConversionFactor;
                        item.StockDetails.StockingQuantity = item.ReturnQty * item.ConversionFactor;
                        item.StockDetails.SelectedUOM.ID = item.TransactionUOMID;
                        item.StockDetails.ExpiryDate = item.BatchExpiryDate;

                        clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                        obj.Details = item.StockDetails;
                        obj.Details.ID = 0;
                        obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                        if (obj.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        item.StockDetails.ID = obj.Details.ID;
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                //BizActionObj.SuccessStatus = -1;
                //BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;

        }

        public override IValueObject GetExpiryItemReturnForDashBoard(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiryItemForDashBoardBizActionVO objBizAction = valueObject as clsGetExpiryItemForDashBoardBizActionVO;

            DbCommand command;
            DbDataReader reader = null;
            //
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetExpiryItemListForDashBoard");

                if (objBizAction.IsOrderBy != null)
                {
                    dbServer.AddInParameter(command, "OrderBy", DbType.Int64, objBizAction.IsOrderBy);
                }

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objBizAction.UnitId);
                //     dbServer.AddInParameter(command, "Date", DbType.DateTime, objBizAction.Date);
                dbServer.AddInParameter(command, "IsPaging", DbType.Boolean, objBizAction.IsPaging);
                dbServer.AddInParameter(command, "StartIndex", DbType.Int64, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "NoOfRecordShow", DbType.Int64, objBizAction.NoOfRecordShow);
                //      dbServer.AddInParameter(command, "Day", DbType.Int64, objBizAction.Day);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizAction.StoreID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, objBizAction.UserID);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);

                //Added by Sayali 21th Aug 2018
                if (objBizAction.ExpiryFromDate != null)
                    dbServer.AddInParameter(command,"ExpiryFromDate",DbType.DateTime,objBizAction.ExpiryFromDate);
                if (objBizAction.ExpiryToDate != null)
                {
                    if (objBizAction.ExpiryFromDate != null)
                    {
                        objBizAction.ExpiryToDate = objBizAction.ExpiryToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ExpiryToDate", DbType.DateTime, objBizAction.ExpiryToDate);
                }
                //

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ExpiredItemList == null)
                        objBizAction.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO objItem = new clsExpiredItemReturnDetailVO();

                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItem.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.BatchExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItem.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objItem.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        //By Anjali............................
                        objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItem.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        objItem.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        objItem.BaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItem.StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItem.StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));  // Transaction UOM of T_ItemStock Table

                        objItem.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));

                        #region Add properties by AniketK on 26Oct2018 to show PO ,GRN , Supplier details on expiry dashboard

                        objItem.PONowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["PONowithDate"]));
                        objItem.GRNNowithDate = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNowithDate"]));
                        objItem.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objItem.BaseCPDashBoard = Convert.ToDouble(DALHelper.HandleDBNull(reader["BaseCP"]));
                        objItem.BaseMRPDashBoard = Convert.ToDouble(DALHelper.HandleDBNull(reader["BaseMRP"]));


                        #endregion

                        #region  Added by Prashant Channe on 07Dec2018, to show TotalCP and TotalMRP details on expiry dashboard

                        objItem.TotalCP = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalCP"]));
                        objItem.TotalMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalMRP"]));

                        #endregion

                        #region For Quarantine Items (Expired, DOS)

                        objItem.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objItem.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objItem.OtherGRNItemTaxPer = Convert.ToSingle(DALHelper.HandleDBNull(reader["othertax"]));
                        objItem.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objItem.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));
                        objItem.GRNItemVATPer = Convert.ToSingle(DALHelper.HandleDBNull(reader["VatPer"]));

                        #endregion

                        objItem.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));

                        

                        //.....................................

                        objBizAction.ExpiredItemList.Add(objItem);
                    }
                }
                reader.NextResult();
                objBizAction.TotalRow = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));

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
        /// <summary>
        /// Fetches expired item list & fills front panel grid
        /// </summary>
        /// <param name="valueObject">clsGetExpiredListBizActionVO</param>
        /// <param name="UserVo"></param>
        /// <returns>clsGetExpiredListBizActionVO object</returns>
        public override IValueObject GetExpiryList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredListBizActionVO objBizAction = valueObject as clsGetExpiredListBizActionVO;
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetExpiryList");
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizAction.SupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizAction.StoreId);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizAction.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizAction.ToDate);
                dbServer.AddInParameter(command, "IsPaging", DbType.Boolean, objBizAction.IsPagingEnabled);
                dbServer.AddInParameter(command, "StartIndex", DbType.Int64, objBizAction.StartIndex);
                dbServer.AddInParameter(command, "NoOfRecordShow", DbType.Int64, objBizAction.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, UserVo.ID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ExpiredList == null)
                        objBizAction.ExpiredList = new List<clsExpiredItemReturnVO>();
                    while (reader.Read())
                    {
                        clsExpiredItemReturnVO objItem = new clsExpiredItemReturnVO();
                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.ExpiryReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["ExpiryReturnNo"]));
                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItem.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objItem.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));

                        objItem.NetAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["NetAmount"]));
                        //(DALHelper.HandleDate(reader["NetAmount"])));
                        objItem.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        #region Approve Flow Variables

                        objItem.Approve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Approved"]));
                        objItem.Reject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Rejected"]));
                        objItem.ApproveOrRejectByName = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejectByName"]));

                        #endregion

                        objBizAction.ExpiredList.Add(objItem);
                    }

                }
                reader.NextResult();
                objBizAction.TotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));




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
            return objBizAction;
        }

        /// <summary>
        /// Fetches expired item list & fills front panel grid
        /// </summary>
        /// <param name="valueObject">clsGetExpiredListForExpiredReturnBizActionVO</param>
        /// <param name="UserVo"></param>
        /// <returns></returns>
        public override IValueObject GetExpiryListForExpiredReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredListForExpiredReturnBizActionVO objBizAction = valueObject as clsGetExpiredListForExpiredReturnBizActionVO;
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetExpiryListForExpiredReturn");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.ConsumptionID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizAction.ItemList == null)
                        objBizAction.ItemList = new List<clsExpiredItemReturnDetailVO>();
                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO objItem = new clsExpiredItemReturnDetailVO();

                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItem.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItem.VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItem.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objItem.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objItem.ItemTax = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercentage"]));
                        objItem.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objItem.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objItem.ReturnQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnQty"]));
                        objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        objItem.ItemExpiryReturnID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiryReturnID"]));
                        objItem.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItem.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItem.StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItem.BatchExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["BatchExpiryDate"]));

                        //objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        //objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        //objItem.ReturnQty = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["ReturnQty"]));
                        //objItem.PurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        //objItem.ReturnRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["ReturnRate"]));

                        //objItem.VATPercentage = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["VATPercentage"]));
                        //objItem.VATAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["VATAmount"]));
                        //objItem.TaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TaxAmount"]));
                        //objItem.TotalTaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalTaxAmount"]));

                        //objItem.Amount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalAmount"]));
                        //objItem.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        //objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        //objItem.TotalPurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalPurchaseRate"]));

                        objBizAction.ItemList.Add(objItem);
                    }

                }
                reader.NextResult();
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
            return objBizAction;
        }


        public override IValueObject GetExpiredReturnDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetExpiredReturnDetailsBizActionVO objBizAction = valueObject as clsGetExpiredReturnDetailsBizActionVO;
            DbCommand command;
            DbDataReader reader = null;
            try
            {
                command = dbServer.GetStoredProcCommand("CIMS_GetExpiredReturnDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizAction.ExpiredID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizAction.UnitID);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (objBizAction.ExpiredMainList == null)
                        objBizAction.ExpiredMainList = new clsExpiredItemReturnVO();

                    while (reader.Read())
                    {
                        clsExpiredItemReturnVO objItem = new clsExpiredItemReturnVO();
                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItem.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItem.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));
                        objItem.ExpiryReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["ExpiryReturnNo"]));

                        objItem.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItem.Time = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Time"]));

                        objItem.TotalAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItem.TotalVATAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalVATAmount"]));
                        objItem.TotalTaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalTaxAmount"]));
                        objItem.TotalOctriAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalOctriAmount"]));
                        objItem.OtherDeducution = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["OtherDeducution"]));
                        objItem.NetAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["NetAmount"]));

                        objItem.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        objItem.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));

                        objItem.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        objItem.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Supplier"]));

                        objBizAction.ExpiredMainList = objItem;
                    }

                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    if (objBizAction.ExpiredItemList == null)
                        objBizAction.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();

                    while (reader.Read())
                    {
                        clsExpiredItemReturnDetailVO objItem = new clsExpiredItemReturnDetailVO();

                        objItem.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItem.ItemExpiryReturnID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiryReturnID"]));
                        objItem.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objItem.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objItem.BatchExpiryDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["BatchExpiryDate"]));
                        objItem.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));

                        //objItem.Conversion = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["Conversion"]));
                        objItem.ReturnQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnQty"]));
                        objItem.SelectedUOM.Description = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        objItem.VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItem.GRNItemVatApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objItem.GRNItemVatType = Convert.ToInt16(DALHelper.HandleDBNull(reader["Vattype"]));
                        objItem.ItemTax = Convert.ToDouble(DALHelper.HandleDBNull(reader["TAXPercentage"]));
                        objItem.OtherGRNItemTaxApplicationOn = Convert.ToInt16(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objItem.OtherGRNItemTaxType = Convert.ToInt16(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objItem.PurchaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objItem.MRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        objItem.DiscountPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPercentage"]));
                        objItem.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objItem.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItem.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objItem.StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));                       // SUOM UOM                     // For Conversion Factor
                        objItem.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItem.OctroiAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["OctroiAmount"]));
                        objItem.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItem.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItem.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItem.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        objItem.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        objItem.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        objItem.ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objItem.ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));
                        objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItem.SelectedUOM = new MasterListItem(objItem.TransactionUOMID, objItem.SelectedUOM.Description);
                        objItem.PendingQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"]));
                        objItem.MainRate = Convert.ToSingle(objItem.PurchaseRate) / objItem.BaseConversionFactor;
                        objItem.MainMRP = Convert.ToSingle(objItem.MRP) / objItem.BaseConversionFactor;
                        //objItem.PurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        //objItem.ReturnRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["ReturnRate"]));
                        //objItem.Amount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalAmount"]));
                        //objItem.DiscountAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["DiscountAmount"]));
                        //objItem.VATAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["VATAmount"]));
                        //objItem.TaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TaxAmount"]));
                        //objItem.TotalTaxAmount = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalTaxAmount"]));
                        //objItem.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        //objItem.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        //objItem.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));         // Transaction UOM      // For Conversion Factor
                        //objItem.TotalPurchaseRate = Convert.ToDouble((decimal)DALHelper.HandleDBNull(reader["TotalPurchaseRate"]));
                        //objItem.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        //#region For Quarantine Items (Expired, DOS)
                        //#endregion

                        objBizAction.ExpiredItemList.Add(objItem);
                    }

                }

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
            return objBizAction;
        }

        public override IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsUpdateExpiryForApproveRejectVO BizActionObj = valueObject as clsUpdateExpiryForApproveRejectVO;

            if (BizActionObj.objExpiryItem.ID > 0)
                BizActionObj = ApproveRejectExpiry(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;
        }

        public override IValueObject UpdateExpiryForApproveOrReject(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateExpiryForApproveRejectVO BizActionObj = valueObject as clsUpdateExpiryForApproveRejectVO;

            if (BizActionObj.objExpiryItem.ID > 0)
                BizActionObj = ApproveRejectExpiry(BizActionObj, UserVo, null, null);

            return valueObject;
        }

        private clsUpdateExpiryForApproveRejectVO ApproveRejectExpiry(clsUpdateExpiryForApproveRejectVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                if (myConnection != null)
                    con = myConnection;
                else
                    con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                if (myTransaction != null)
                    trans = myTransaction;
                else
                    trans = con.BeginTransaction();

                clsExpiredItemReturnVO objDetailsVO = BizActionObj.objExpiryItem;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdaterExpiryApproveRejectStatus");
                command.Connection = con;

                dbServer.AddInParameter(command, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);

                dbServer.AddInParameter(command, "IsApprove", DbType.Boolean, objDetailsVO.Approve);
                dbServer.AddInParameter(command, "IsReject", DbType.Boolean, objDetailsVO.Reject);

                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);


                if (myConnection == null) trans.Commit();

                BizActionObj.SuccessStatus = 0;
            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                if (myConnection == null) trans.Rollback();
            }
            finally
            {
                if (myConnection == null) con.Close();
            }

            return BizActionObj;

        }

    }
}
