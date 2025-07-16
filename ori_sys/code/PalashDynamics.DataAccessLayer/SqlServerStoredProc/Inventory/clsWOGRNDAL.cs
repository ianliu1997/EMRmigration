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

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    class clsWOGRNDAL : clsBaseWOGRNDAL
    {
         //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        #endregion

        private clsWOGRNDAL()
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

        public override IValueObject WOAdd(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddWOGRNBizActionVO BizActionObj = valueObject as clsAddWOGRNBizActionVO;

            //if (BizActionObj.Details.ID == 0)
            BizActionObj = AddDetails(BizActionObj, UserVo);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }
        //Commentted By Somnath
        //private clsAddGRNBizActionVO AddDetails(clsAddGRNBizActionVO BizActionObj, clsUserVO UserVo)
        //{
        //    DbConnection con = dbServer.CreateConnection();
        //    DbTransaction trans = null;
        //    string strPONO1 = String.Empty;
        //    try
        //    {
        //        con.Open();
        //        trans = con.BeginTransaction();

        //        clsGRNVO objDetailsVO = BizActionObj.Details;

        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddGRN");
        //        command.Connection = con;

        //        dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //        if (objDetailsVO.LinkServer != null)
        //            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
        //        dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
        //        dbServer.AddInParameter(command, "GRNNO", DbType.String, objDetailsVO.GRNNO);
        //        dbServer.AddInParameter(command, "GRNType", DbType.Int16, (int)objDetailsVO.GRNType);
        //        dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
        //        dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
        //        dbServer.AddInParameter(command, "InvoiceNo", DbType.String, objDetailsVO.InvoiceNo);
        //        dbServer.AddInParameter(command, "InvoiceDate", DbType.DateTime, objDetailsVO.InvoiceDate);
        //        dbServer.AddInParameter(command, "DeliveryChallanNo", DbType.String, objDetailsVO.DeliveryChallanNo);
        //        dbServer.AddInParameter(command, "GatePassNo", DbType.String, objDetailsVO.GatePassNo);
        //        dbServer.AddInParameter(command, "POID", DbType.Int64, objDetailsVO.POID);
        //        dbServer.AddInParameter(command, "PaymentModeID", DbType.Int16, (int)objDetailsVO.PaymentModeID);
        //        dbServer.AddInParameter(command, "ReceivedByID", DbType.Int64, objDetailsVO.ReceivedByID);
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
        //        dbServer.AddInParameter(command, "TotalCDiscount", DbType.Double, objDetailsVO.TotalCDiscount);
        //        dbServer.AddInParameter(command, "TotalSchDiscount", DbType.Double, objDetailsVO.TotalSchDiscount);
        //        dbServer.AddInParameter(command, "TotalVAT", DbType.Double, objDetailsVO.TotalVAT);
        //        dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
        //        dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
        //        dbServer.AddInParameter(command, "Other", DbType.Double, objDetailsVO.Other);
        //        dbServer.AddInParameter(command, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

        //        if ((bool)BizActionObj.IsFileAttached)
        //        {
        //            dbServer.AddInParameter(command, "FileData", DbType.Binary, BizActionObj.File);
        //            dbServer.AddInParameter(command, "FileName", DbType.String, BizActionObj.FileName);
        //            dbServer.AddInParameter(command, "IsFileAttached", DbType.String, BizActionObj.IsFileAttached);
        //        }

        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //        dbServer.AddInParameter(command, "Freezed", DbType.Boolean, BizActionObj.Details.Freezed);
        //        dbServer.AddParameter(command, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        int intStatus = dbServer.ExecuteNonQuery(command, trans);
        //        //int intStatus = dbServer.ExecuteNonQuery(command);
        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        if (objDetailsVO.ID == 0)
        //        {
        //            BizActionObj.Details.GRNNO = (string)dbServer.GetParameterValue(command, "Number");
        //        }
        //        else
        //        {
        //            BizActionObj.Details.GRNNO = objDetailsVO.GRNNO;
        //        }
        //        BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");


        //        BizActionObj.Details.UnitId = UserVo.UserLoginInfo.UnitId;

        //        if (BizActionObj.IsEditMode == true)
        //        {
        //            clsBaseGRNDAL objGRN = clsBaseGRNDAL.GetInstance();
        //            IValueObject objTest = objGRN.DeleteGRNItems(BizActionObj, UserVo);
        //        }

        //        bool isItemAdd = false;

        //        foreach (var item in objDetailsVO.Items)
        //        {
        //            item.GRNID = BizActionObj.Details.ID;
        //            item.GRNUnitID = BizActionObj.Details.UnitId;
        //            item.StockDetails.PurchaseRate = item.Rate;
        //            item.StockDetails.MRP = item.MRP;

        //            bool checkItem = false;

        //            if (objDetailsVO.Freezed == true)
        //            {
        //                //Not required
        //                    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_CheckExistingBatch");
        //                    dbServer.AddInParameter(command5, "BatchCode", DbType.String, item.BatchCode);
        //                    dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item.ItemID);
        //                    dbServer.AddInParameter(command5, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                    dbServer.AddInParameter(command5, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
        //                    dbServer.AddOutParameter(command5, "Exist", DbType.Boolean, 1);
        //                    dbServer.AddOutParameter(command5, "BatchID", DbType.Int64, 0);
        //                    dbServer.AddInParameter(command5, "BarCode", DbType.String, item.BarCode);
        //                    dbServer.AddInParameter(command5, "Quantity", DbType.Single,Convert.ToSingle(item.TotalQuantity));
        //                    int intCheck = dbServer.ExecuteNonQuery(command5, trans);
        //                    checkItem = (bool)dbServer.GetParameterValue(command5, "Exist");
        //                    item.BatchID = (long)dbServer.GetParameterValue(command5, "BatchID");
        //                    //Not required 
        //                //
        //                    if (checkItem == false && objDetailsVO.Freezed == true)
        //                    {
        //                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemBatch");
        //                        command1.Connection = con;
        //                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //                        if (objDetailsVO.LinkServer != null)
        //                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

        //                        dbServer.AddInParameter(command1, "Date", DbType.DateTime, objDetailsVO.Date);
        //                        dbServer.AddInParameter(command1, "Time", DbType.DateTime, objDetailsVO.Time);

        //                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
        //                        dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
        //                        dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
        //                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.TotalQuantity);
        //                        dbServer.AddInParameter(command1, "ConversionFactor", DbType.Double, item.ConversionFactor);
        //                        dbServer.AddInParameter(command1, "PurchaseRate", DbType.Double, (item.StockDetails.PurchaseRate / item.ConversionFactor));
        //                        dbServer.AddInParameter(command1, "MRP", DbType.Double, (item.StockDetails.MRP / item.ConversionFactor));


                           
        //                        //dbServer.AddInParameter(command1, "PurchaseRate", DbType.Double, (item.StockDetails.PurchaseRate));
        //                        //dbServer.AddInParameter(command1, "MRP", DbType.Double, (item.StockDetails.MRP ));
        //                        dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
        //                        dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
        //                        dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);

        //                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
        //                        dbServer.AddInParameter(command1, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
        //                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //                        dbServer.AddInParameter(command1, "BarCode", DbType.String, item.BarCode);
        //                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0); // item.ID);

        //                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);

        //                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
        //                        //int intStatus1 = dbServer.ExecuteNonQuery(command1);
        //                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

        //                        item.BatchID = (long)dbServer.GetParameterValue(command1, "ID");
        //                    }
                        
        //            }

        //            DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddGRNItems");
        //            command2.Connection = con;
        //            dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //            if (objDetailsVO.LinkServer != null)
        //                dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
        //            dbServer.AddInParameter(command2, "POID", DbType.Int64, item.POID);
        //            dbServer.AddInParameter(command2, "POUnitID", DbType.Int64, item.POUnitID);

        //            dbServer.AddInParameter(command2, "GRNID", DbType.Int64, item.GRNID);
        //            dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, item.GRNUnitID);
        //            dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
        //            dbServer.AddInParameter(command2, "BatchID", DbType.Int64, item.BatchID);
        //            dbServer.AddInParameter(command2, "BatchCode", DbType.String, item.BatchCode);
        //            dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
        //            //dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
        //            dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity * item.ConversionFactor);
                    
        //            dbServer.AddInParameter(command2, "FreeQuantity", DbType.Double, item.FreeQuantity * item.ConversionFactor);
        //            dbServer.AddInParameter(command2, "CoversionFactor", DbType.Double, item.ConversionFactor);
        //            //dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate / item.ConversionFactor));
        //            //dbServer.AddInParameter(command2, "MRP", DbType.Double, (item.MRP / item.ConversionFactor));

        //            dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate));
        //            dbServer.AddInParameter(command2, "MRP", DbType.Double, (item.MRP));


        //            dbServer.AddInParameter(command2, "POQty", DbType.Double, item.POQuantity);
        //            dbServer.AddInParameter(command2, "PoPendingQty", DbType.Double, item.PendingQuantity);
        //            dbServer.AddInParameter(command2, "ActualPendingQty", DbType.Double, item.POPendingQuantity);


        //            dbServer.AddInParameter(command2, "Amount", DbType.Double, item.Amount);
        //            dbServer.AddInParameter(command2, "VATPercent", DbType.Double, item.VATPercent);
        //            dbServer.AddInParameter(command2, "VATAmount", DbType.Double, item.VATAmount);
        //            dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, item.CDiscountPercent);
        //            dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, item.CDiscountAmount);
        //            dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, item.SchDiscountPercent);
        //            dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, item.SchDiscountAmount);
        //            dbServer.AddInParameter(command2, "ItemTax", DbType.Double, item.ItemTax);
        //            dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
        //            dbServer.AddInParameter(command2, "Remarks", DbType.String, item.Remarks);

        //            dbServer.AddInParameter(command2, "Status", DbType.Boolean, objDetailsVO.Status);
        //            dbServer.AddInParameter(command2, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
        //            dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //            dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
        //            dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //            dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //            dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //            //For assigning supplier through GRN
        //            dbServer.AddInParameter(command2, "AssignSupplier", DbType.Boolean, item.AssignSupplier);
        //            dbServer.AddInParameter(command2, "SupplierId", DbType.Int64, objDetailsVO.SupplierID);

        //            dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
        //            dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

        //            int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
        //            //int intStatus2 = dbServer.ExecuteNonQuery(command2);
        //            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
        //            item.ID = (long)dbServer.GetParameterValue(command2, "ID");

        //            //For Updating itemMaster

        //            DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateItemMasterFromGRN");
        //            command3.Connection = con;
        //            dbServer.AddInParameter(command3, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //            if (objDetailsVO.LinkServer != null)
        //                dbServer.AddInParameter(command3, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
        //            dbServer.AddInParameter(command3, "Date", DbType.DateTime, objDetailsVO.Date);
        //            dbServer.AddInParameter(command3, "Time", DbType.DateTime, objDetailsVO.Time);
        //            dbServer.AddInParameter(command3, "ItemId", DbType.Int64, item.ItemID);
        //            //dbServer.AddInParameter(command3, "PurchaseRate", DbType.Decimal, Convert.ToDecimal(item.Rate)/Convert.ToDecimal(item.ConversionFactor));
        //            //dbServer.AddInParameter(command3, "MRP", DbType.Decimal, Convert.ToDecimal(item.MRP) / Convert.ToDecimal(item.ConversionFactor));

        //            dbServer.AddInParameter(command3, "PurchaseRate", DbType.Decimal, Convert.ToDecimal(item.Rate) );
        //            dbServer.AddInParameter(command3, "MRP", DbType.Decimal, Convert.ToDecimal(item.MRP) );


        //            dbServer.AddInParameter(command3, "VatPer", DbType.Decimal, Convert.ToDecimal(item.VATPercent));
        //            dbServer.AddInParameter(command3, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //            dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, int.MaxValue);
        //            dbServer.AddInParameter(command3, "ConversionFactor", DbType.String, Convert.ToString(item.ConversionFactor));
        //            int intStatus3 = dbServer.ExecuteNonQuery(command3, trans);
        //            // int intStatus3 = dbServer.ExecuteNonQuery(command3);


        //            foreach (var itemPOGRN in objDetailsVO.ItemsPOGRN)
        //            {
        //                //if (itemPOGRN.ItemID == item.ItemID && (itemPOGRN.BatchCode == item.BatchCode))
        //                if (itemPOGRN.ItemID == item.ItemID)
        //                {                            
        //                        DbCommand comand7 = dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
        //                        comand7.Connection = con;
        //                        dbServer.AddInParameter(comand7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(comand7, "GRNID", DbType.Int64, objDetailsVO.ID);
        //                        dbServer.AddInParameter(comand7, "GRNUnitID", DbType.Int64, objDetailsVO.UnitId);

        //                        dbServer.AddInParameter(comand7, "GRNDetailID", DbType.Int64, item.ID);
        //                        dbServer.AddInParameter(comand7, "GRNDetailUnitID", DbType.Int64, objDetailsVO.UnitId);

        //                        dbServer.AddInParameter(comand7, "POID", DbType.Int64, itemPOGRN.POID);
        //                        dbServer.AddInParameter(comand7, "POUnitID", DbType.Int64, itemPOGRN.POUnitID);

        //                        dbServer.AddInParameter(comand7, "PODetailsID", DbType.Int64, itemPOGRN.PoItemsID);
        //                        dbServer.AddInParameter(comand7, "PODetailsUnitID", DbType.Int64, itemPOGRN.POUnitID);



        //                        dbServer.AddInParameter(comand7, "ItemID", DbType.Int64, itemPOGRN.ItemID);

        //                        dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, itemPOGRN.Quantity);
        //                        dbServer.AddInParameter(comand7, "PendingQty", DbType.Decimal, itemPOGRN.POPendingQuantity);

        //                        dbServer.AddInParameter(comand7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(comand7, "AddedBy", DbType.Int64, UserVo.ID);
        //                        dbServer.AddInParameter(comand7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                        dbServer.AddInParameter(comand7, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                        dbServer.AddInParameter(comand7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                        dbServer.AddParameter(comand7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
        //                        int intStatus7 = dbServer.ExecuteNonQuery(comand7, trans);

        //                    //    isItemAdd = true;
        //                    //}
        //                    //else
        //                    //{
        //                    //    isItemAdd = false;
        //                    //}

        //                }

        //            }

        //            #region MMBABU

        //            //Only For Against PO
        //            if (!BizActionObj.IsDraft && objDetailsVO.Freezed)
        //            {
                    
        //                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdatePOFromGRN");
        //                command4.Connection = con;
        //                dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.Quantity);
        //                dbServer.AddInParameter(command4, "POItemsID", DbType.Int64, item.ItemID);   //item.PoItemsID
        //                dbServer.AddInParameter(command4, "PendingQty", DbType.Decimal, item.POPendingQuantity);
        //                dbServer.AddInParameter(command4, "IndentID", DbType.Decimal, item.IndentID);
        //                dbServer.AddInParameter(command4, "IndentUnitID", DbType.Decimal, item.IndentUnitID);

        //                dbServer.AddInParameter(command4, "POID", DbType.Int64, item.POID);   //item.PoItemsID
        //                dbServer.AddInParameter(command4, "POUnitID", DbType.Int64, item.POUnitID);

        //                dbServer.AddInParameter(command4, "GRNID", DbType.Int64, item.GRNID);   //item.PoItemsID
        //                dbServer.AddInParameter(command4, "GRNUnitID", DbType.Int64, item.GRNUnitID);

        //                int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
        //                //int intStatus4 = dbServer.ExecuteNonQuery(command4);
        //                //}
        //            }
        //            #endregion

        //            item.StockDetails.BatchID = item.BatchID;
        //            item.StockDetails.OperationType = InventoryStockOperationType.Addition;
        //            item.StockDetails.ItemID = item.ItemID;
        //            item.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
        //            item.StockDetails.TransactionID = item.GRNID;
        //            //item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity)
        //            item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity) * item.ConversionFactor;
        //            item.StockDetails.BatchCode = item.BatchCode;
        //            item.StockDetails.Date = objDetailsVO.Date;
        //            item.StockDetails.Time = objDetailsVO.Time;
        //            item.StockDetails.StoreID = objDetailsVO.StoreID;

        //            if (item.BatchID>0 && objDetailsVO.Freezed == true)
        //            {
        //                clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //                clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

        //                obj.Details = item.StockDetails;
        //                obj.Details.ID = 0;
        //                if (!BizActionObj.IsDraft)
        //                {
        //                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

        //                    if (obj.SuccessStatus == -1)
        //                        throw new Exception("Error");

        //                    item.StockDetails.ID = obj.Details.ID;
        //                }
        //                else
        //                {
        //                    item.StockDetails.ID = 0;
        //                }
        //            }
        //            #region "Rohit"
        //            // Not Required 
        //            if ((strPONO1 != null) && (item.PONO !=null) && (!strPONO1.Contains(item.PONO)))
        //            {
        //                strPONO1 = String.Format((strPONO1 + "," + item.PONO).Trim());
        //                DbCommand comand = dbServer.GetStoredProcCommand("CIMS_AddPOItemsForGRN");
        //                command.Connection = con;
        //                dbServer.AddInParameter(comand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(comand, "GRNID", DbType.Int64, objDetailsVO.ID);
        //                dbServer.AddInParameter(comand, "GRNUnitID", DbType.Int64, objDetailsVO.UnitId);
        //                dbServer.AddInParameter(comand, "GRNNO", DbType.String, objDetailsVO.GRNNO);
        //                dbServer.AddInParameter(comand, "GRNDate", DbType.DateTime, objDetailsVO.Date);
        //                dbServer.AddInParameter(comand, "BatchCode", DbType.String, item.BatchCode);
        //                dbServer.AddInParameter(comand, "POID", DbType.Int64, item.POID);
        //                dbServer.AddInParameter(comand, "POUnitID", DbType.Int64, item.POUnitID);
        //                dbServer.AddInParameter(comand, "PONO", DbType.String, item.PONO);
        //                dbServer.AddInParameter(comand, "PODate", DbType.DateTime, item.PODate);
        //                dbServer.AddInParameter(comand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(comand, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(comand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(comand, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                dbServer.AddInParameter(comand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddParameter(comand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
        //                int intStatus6 = dbServer.ExecuteNonQuery(comand, trans);
        //            }
        //            #endregion


        //        }


        //        trans.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        trans.Rollback();
        //        logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
        //        BizActionObj.SuccessStatus = -1;
        //        BizActionObj.Details = null;

        //    }
        //    finally
        //    {
        //        con.Close();
        //        trans = null;
        //    }
        //    return BizActionObj;
        //}

        //End

        private clsAddWOGRNBizActionVO AddDetails(clsAddWOGRNBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            string strPONO1 = String.Empty;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsWOGRNVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddWOGRN");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
                dbServer.AddInParameter(command, "GRNNO", DbType.String, objDetailsVO.GRNNO);
                dbServer.AddInParameter(command, "GRNType", DbType.Int16, (int)objDetailsVO.GRNType);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
                dbServer.AddInParameter(command, "InvoiceNo", DbType.String, objDetailsVO.InvoiceNo);
                dbServer.AddInParameter(command, "InvoiceDate", DbType.DateTime, objDetailsVO.InvoiceDate);
                dbServer.AddInParameter(command, "DeliveryChallanNo", DbType.String, objDetailsVO.DeliveryChallanNo);
                dbServer.AddInParameter(command, "GatePassNo", DbType.String, objDetailsVO.GatePassNo);
                dbServer.AddInParameter(command, "WOID", DbType.Int64, objDetailsVO.WOID);
                dbServer.AddInParameter(command, "PaymentModeID", DbType.Int16, (int)objDetailsVO.PaymentModeID);
                dbServer.AddInParameter(command, "ReceivedByID", DbType.Int64, objDetailsVO.ReceivedByID);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                dbServer.AddInParameter(command, "TotalCDiscount", DbType.Double, objDetailsVO.TotalCDiscount);
                dbServer.AddInParameter(command, "TotalSchDiscount", DbType.Double, objDetailsVO.TotalSchDiscount);
                dbServer.AddInParameter(command, "TotalVAT", DbType.Double, objDetailsVO.TotalVAT);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                dbServer.AddInParameter(command, "Other", DbType.Double, objDetailsVO.Other);
                dbServer.AddInParameter(command, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                if ((bool)BizActionObj.IsFileAttached)
                {
                    dbServer.AddInParameter(command, "FileData", DbType.Binary, BizActionObj.File);
                    dbServer.AddInParameter(command, "FileName", DbType.String, BizActionObj.FileName);
                    dbServer.AddInParameter(command, "IsFileAttached", DbType.String, BizActionObj.IsFileAttached);
                }

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, BizActionObj.Details.Freezed);
                dbServer.AddParameter(command, "Number", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                //int intStatus = dbServer.ExecuteNonQuery(command);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                if (objDetailsVO.ID == 0)
                {
                    BizActionObj.Details.GRNNO = (string)dbServer.GetParameterValue(command, "Number");
                }
                else
                {
                    BizActionObj.Details.GRNNO = objDetailsVO.GRNNO;
                }
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");


                BizActionObj.Details.UnitId = UserVo.UserLoginInfo.UnitId;

                if (BizActionObj.IsEditMode == true)
                {
                    clsBaseWOGRNDAL objGRN = clsBaseWOGRNDAL.GetInstance();
                    IValueObject objTest = objGRN.WODeleteGRNItems(BizActionObj, UserVo);
                }

                bool isItemAdd = false;

                foreach (var item in objDetailsVO.Items)
                {
                    item.GRNID = BizActionObj.Details.ID;
                    item.GRNUnitID = BizActionObj.Details.UnitId;
                    item.StockDetails.PurchaseRate = item.Rate;
                    item.StockDetails.MRP = item.MRP;

                    bool checkItem = false;

                    //if (objDetailsVO.Freezed == true) 
                    //{

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRN");
                        command2.Connection = con;
                        dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                        if (objDetailsVO.LinkServer != null)
                            dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                        dbServer.AddInParameter(command2, "POID", DbType.Int64, item.WOID);
                        dbServer.AddInParameter(command2, "POUnitID", DbType.Int64, item.WOUnitID);

                        dbServer.AddInParameter(command2, "GRNID", DbType.Int64, item.GRNID);
                        dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                        dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                       // dbServer.AddInParameter(command2, "BatchID", DbType.Int64, item.BatchID);
                        dbServer.AddParameter(command2, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.BatchID);
                        dbServer.AddInParameter(command2, "BatchCode", DbType.String, item.BatchCode);
                        dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                        //dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
                        dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity * item.ConversionFactor);

                        dbServer.AddInParameter(command2, "FreeQuantity", DbType.Double, item.FreeQuantity * item.ConversionFactor);
                        dbServer.AddInParameter(command2, "CoversionFactor", DbType.Double, item.ConversionFactor);
                        //dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate / item.ConversionFactor));
                        //dbServer.AddInParameter(command2, "MRP", DbType.Double, (item.MRP / item.ConversionFactor));

                        dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate));
                        dbServer.AddInParameter(command2, "MRP", DbType.Double, (item.MRP));


                        dbServer.AddInParameter(command2, "POQty", DbType.Double, item.WOQuantity);
                        dbServer.AddInParameter(command2, "PoPendingQty", DbType.Double, item.PendingQuantity);
                        dbServer.AddInParameter(command2, "ActualPendingQty", DbType.Double, item.WOPendingQuantity);


                        dbServer.AddInParameter(command2, "Amount", DbType.Double, item.Amount);
                        dbServer.AddInParameter(command2, "VATPercent", DbType.Double, item.VATPercent);
                        dbServer.AddInParameter(command2, "VATAmount", DbType.Double, item.VATAmount);
                        dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, item.CDiscountPercent);
                        dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, item.CDiscountAmount);
                        dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, item.SchDiscountPercent);
                        dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, item.SchDiscountAmount);
                        dbServer.AddInParameter(command2, "ItemTax", DbType.Double, item.ItemTax);
                        dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
                        dbServer.AddInParameter(command2, "Remarks", DbType.String, item.Remarks);

                        dbServer.AddInParameter(command2, "Status", DbType.Boolean, objDetailsVO.Status);
                        dbServer.AddInParameter(command2, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
                        dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                        dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                        dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                        dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                        dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                        //For assigning supplier through GRN
                        dbServer.AddInParameter(command2, "AssignSupplier", DbType.Boolean, item.AssignSupplier);
                        dbServer.AddInParameter(command2, "SupplierId", DbType.Int64, objDetailsVO.SupplierID);

                        //Added By Somnath
                        dbServer.AddInParameter(command2, "BarCode", DbType.String, item.BarCode);
                        dbServer.AddInParameter(command2, "Date", DbType.DateTime, objDetailsVO.Date);
                        dbServer.AddInParameter(command2, "Time", DbType.DateTime, objDetailsVO.Time);
                        //End

                        dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                        dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                        //int intStatus2 = dbServer.ExecuteNonQuery(command2);
                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                        item.ID = (long)dbServer.GetParameterValue(command2, "ID");
                        item.BatchID = (long)dbServer.GetParameterValue(command2, "BatchID");

                        if(objDetailsVO.ItemsWOGRN.Count > 0)
                        {
                            foreach (var itemPOGRN in objDetailsVO.ItemsWOGRN)
                            {
                                //if (itemPOGRN.ItemID == item.ItemID && (itemPOGRN.BatchCode == item.BatchCode))
                                if (itemPOGRN.ItemID == item.ItemID)
                                {
                                    DbCommand comand7 = dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
                                    comand7.Connection = con;
                                    dbServer.AddInParameter(comand7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(comand7, "GRNID", DbType.Int64, objDetailsVO.ID);
                                    dbServer.AddInParameter(comand7, "GRNUnitID", DbType.Int64, objDetailsVO.UnitId);

                                    dbServer.AddInParameter(comand7, "GRNDetailID", DbType.Int64, item.ID);
                                    dbServer.AddInParameter(comand7, "GRNDetailUnitID", DbType.Int64, objDetailsVO.UnitId);

                                    dbServer.AddInParameter(comand7, "POID", DbType.Int64, itemPOGRN.WOID);
                                    dbServer.AddInParameter(comand7, "POUnitID", DbType.Int64, itemPOGRN.WOUnitID);

                                    dbServer.AddInParameter(comand7, "PODetailsID", DbType.Int64, itemPOGRN.WoItemsID);
                                    dbServer.AddInParameter(comand7, "PODetailsUnitID", DbType.Int64, itemPOGRN.WOUnitID);



                                    dbServer.AddInParameter(comand7, "ItemID", DbType.Int64, itemPOGRN.ItemID);

                                    dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, itemPOGRN.Quantity);
                                    dbServer.AddInParameter(comand7, "PendingQty", DbType.Decimal, itemPOGRN.WOPendingQuantity);

                                    dbServer.AddInParameter(comand7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                    dbServer.AddInParameter(comand7, "AddedBy", DbType.Int64, UserVo.ID);
                                    dbServer.AddInParameter(comand7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                    dbServer.AddInParameter(comand7, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                                    dbServer.AddInParameter(comand7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                                    dbServer.AddParameter(comand7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                                    int intStatus7 = dbServer.ExecuteNonQuery(comand7, trans);

                                    //    isItemAdd = true;
                                    //}
                                    //else
                                    //{
                                    //    isItemAdd = false;
                                    //}

                                }

                            }
                        }
                        

                    #region MMBABU

                    //Only For Against PO
                    if (!BizActionObj.IsDraft && objDetailsVO.Freezed)
                    {
                        if(item.WOID > 0)
                        {
                            DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdateWOFromGRN");
                            command4.Connection = con;
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.Quantity);
                            dbServer.AddInParameter(command4, "WOItemsID", DbType.Int64, item.ItemID);   //item.PoItemsID
                            dbServer.AddInParameter(command4, "PendingQty", DbType.Decimal, item.WOPendingQuantity);
                            dbServer.AddInParameter(command4, "IndentID", DbType.Decimal, item.IndentID);
                            dbServer.AddInParameter(command4, "IndentUnitID", DbType.Decimal, item.IndentUnitID);

                            dbServer.AddInParameter(command4, "WOID", DbType.Int64, item.WOID);   //item.PoItemsID
                            dbServer.AddInParameter(command4, "WOUnitID", DbType.Int64, item.WOUnitID);

                            dbServer.AddInParameter(command4, "GRNID", DbType.Int64, item.GRNID);   //item.PoItemsID
                            dbServer.AddInParameter(command4, "GRNUnitID", DbType.Int64, item.GRNUnitID);

                            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                        }
                        //int intStatus4 = dbServer.ExecuteNonQuery(command4);
                        //}
                    }
                    #endregion

                    item.StockDetails.BatchID = item.BatchID;
                    item.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    item.StockDetails.ItemID = item.ItemID;
                    item.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
                    item.StockDetails.TransactionID = item.GRNID;
                    //item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity)
                    item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity) * item.ConversionFactor;
                    item.StockDetails.BatchCode = item.BatchCode;
                    item.StockDetails.Date = objDetailsVO.Date;
                    item.StockDetails.Time = objDetailsVO.Time;
                    item.StockDetails.StoreID = objDetailsVO.StoreID;

                    if (objDetailsVO.Freezed == true)
                    {
                        clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                        obj.Details = item.StockDetails;
                        obj.Details.ID = 0;
                        if (!BizActionObj.IsDraft)
                        {
                            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                            if (obj.SuccessStatus == -1)
                                throw new Exception("Error");

                            item.StockDetails.ID = obj.Details.ID;
                        }
                        else
                        {
                            item.StockDetails.ID = 0;
                        }
                    }
             

                }


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Details = null;

            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }

        public override IValueObject WOGetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();

            clsGetWOGRNListBizActionVO BizActionObj = valueObject as clsGetWOGRNListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWOGRNListForSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SupplierID);
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);

                if (BizActionObj.ToDate != null)
                {


                    BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);


                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);

                }
                if (BizActionObj.GRNNo != "")
                dbServer.AddInParameter(command, "GRNNo", DbType.String, BizActionObj.GRNNo);
                else
                    dbServer.AddInParameter(command, "GRNNo", DbType.String, null);
                dbServer.AddInParameter(command, "GrnReturnSearch", DbType.Boolean, BizActionObj.GrnReturnSearch);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddInParameter(command, "IsFreezed", DbType.Boolean, BizActionObj.Freezed);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsWOGRNVO>();
                    while (reader.Read())
                    {
                        clsWOGRNVO objVO = new clsWOGRNVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                        objVO.GRNNO = (string)DALHelper.HandleDBNull(reader["GRNNO"]);
                        objVO.GRNType = (InventoryGRNType)((Int16)DALHelper.HandleDBNull(reader["GRNType"]));
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                        objVO.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                        objVO.PaymentModeID = (MaterPayModeList)Convert.ToInt16(DALHelper.HandleDBNull(reader["PaymentModeID"]));

                        objVO.Freezed = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Freezed"]));
                        objVO.GatePassNo = Convert.ToString(DALHelper.HandleDBNull(reader["GatePassNo"]));
                        objVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        objVO.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        objVO.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        objVO.InvoiceDate = (DateTime?)(DALHelper.HandleDate(reader["InvoiceDate"]));

                        #region Added by MMBABU
                        //objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONo"]));
                        //objVO.PODate = (DateTime?)(DALHelper.HandleDate(reader["PODate"]));
                        objVO.WONowithDate = (string)DALHelper.HandleDBNull(reader["WO No - Date"]);
                       // objVO.IndentNowithDate = (string)DALHelper.HandleDBNull(reader["Indent No. - Date"]);
                        #endregion

                        objVO.FileName = (string)DALHelper.HandleDBNull(reader["FileName"]);
                        objVO.File = (byte[])DALHelper.HandleDBNull(reader["FileData"]);
                        objVO.IsFileAttached = (bool)DALHelper.HandleDBNull(reader["IsFileAttached"]);
                        objVO.FileAttached = objVO.IsFileAttached == true ? "Visible" : "Collapsed";
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
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

        public override IValueObject WOGetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsListBizActionVO BizActionObj = valueObject as clsGetWOGRNDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "Freezed", DbType.Int64, BizActionObj.Freezed);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsWOGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO objVO = new clsWOGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]));
                        objVO.Quantity = (((double)DALHelper.HandleDBNull(reader["Quantity"])) / objVO.ConversionFactor);
                        objVO.FreeQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / objVO.ConversionFactor);
                        objVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));

                        // Added By Rohit 
                        //if(!BizActionObj.Freezed)
                       objVO.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objVO.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WOIQuantity"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));

                        objVO.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        objVO.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        objVO.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        objVO.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        // End
                        //objVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]) * objVO.ConversionFactor;
                        //objVO.Amount = (double)DALHelper.HandleDBNull(reader["Amount"]);
                        objVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        //objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])) * objVO.ConversionFactor;
                        objVO.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));

                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVO.VATPercent > 0)) objVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (!(objVO.CDiscountPercent > 0)) objVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        objVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (!(objVO.SchDiscountPercent > 0)) objVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        //objVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        BizActionObj.List.Add(objVO);

                    }

                }

                reader.NextResult();

                if (BizActionObj.WOGRNList == null)
                    BizActionObj.WOGRNList = new List<clsWOGRNDetailsVO>();

                while (reader.Read())
                {
                    clsWOGRNDetailsVO objVO2 = new clsWOGRNDetailsVO();

                    objVO2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    objVO2.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    objVO2.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                    objVO2.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                    objVO2.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                    objVO2.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));

                    objVO2.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                    objVO2.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"]));

                    objVO2.WODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"]));
                    objVO2.WoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"]));
                    objVO2.WODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailUnitID"]));

                    objVO2.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));

                    objVO2.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                    objVO2.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));

                    objVO2.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));
                    //objVO2.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                    

                    BizActionObj.WOGRNList.Add(objVO2);

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;

        }

        public override IValueObject WOGetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsListByIDBizActionVO BizActionObj = valueObject as clsGetWOGRNDetailsListByIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsByID");
                DbDataReader reader;

               
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId); 

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsWOGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO objVO = new clsWOGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["coversionFactor"]));
                        objVO.Quantity = (((double)DALHelper.HandleDBNull(reader["Quantity"])) / objVO.ConversionFactor);
                        objVO.FreeQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / objVO.ConversionFactor);
                        objVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POQty"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WoPendingQty"]));
                        objVO.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualPendingQty"]));

                        objVO.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        objVO.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        objVO.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        objVO.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));                        
                        objVO.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));

                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVO.VATPercent > 0)) objVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (!(objVO.CDiscountPercent > 0)) objVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        objVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (!(objVO.SchDiscountPercent > 0)) objVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);                       
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);
                        BizActionObj.List.Add(objVO);

                    }

                }

                reader.NextResult();

                if (BizActionObj.WOGRNList == null)
                    BizActionObj.WOGRNList = new List<clsWOGRNDetailsVO>();

                while (reader.Read())
                {
                    clsWOGRNDetailsVO objVO2 = new clsWOGRNDetailsVO();

                    objVO2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    objVO2.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    objVO2.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                    objVO2.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                    objVO2.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                    objVO2.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));

                    objVO2.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                    objVO2.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"]));

                    objVO2.WODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"]));
                    objVO2.WoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailID"]));
                    objVO2.WODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WODetailUnitID"]));

                    objVO2.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));

                    objVO2.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                    objVO2.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));

                    objVO2.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));                  


                    BizActionObj.WOGRNList.Add(objVO2);

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;
            
        }

        public override IValueObject WOGetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetWOGRNDetailsForGRNReturnListBizActionVO BizActionObj = valueObject as clsGetWOGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWOGRNItemsForWOGRNReturnList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "Freezed", DbType.Int64, BizActionObj.Freezed);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsWOGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsWOGRNDetailsVO objVO = new clsWOGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]));
                        objVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]))/objVO.ConversionFactor;
                        objVO.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"]))/objVO.ConversionFactor;
                        objVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.WOPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objVO.WOQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["WOIQuantity"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));

                        objVO.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        objVO.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOrderUnitId"]));
                        objVO.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["WOrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["WorkUOM"]));
                        objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);

                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));

                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVO.VATPercent > 0)) objVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (!(objVO.CDiscountPercent > 0)) objVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        objVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (!(objVO.SchDiscountPercent > 0)) objVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        //objVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));

                        BizActionObj.List.Add(objVO);

                    }

                }


                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

            }
            return valueObject;

        }
        public override IValueObject WODeleteGRNItems(IValueObject valueObject, clsUserVO UserVo)
        {
             DbConnection con = dbServer.CreateConnection();
            clsAddWOGRNBizActionVO objBizActionVO = null;
            clsWOGRNVO objGRNDetailVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                //con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_DeleteGRNItemDetails");
                objBizActionVO = valueObject as clsAddWOGRNBizActionVO;
                objGRNDetailVO = objBizActionVO.Details;
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, objGRNDetailVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objGRNDetailVO.UnitId);
                dbServer.AddInParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus1 = dbServer.ExecuteNonQuery(command);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {

                //trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.Details = null;
            }

            return objBizActionVO;
        }

        public override IValueObject WOUpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            clsAddWOGRNBizActionVO BizActionObj = valueObject as clsAddWOGRNBizActionVO;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsWOGRNDetailsVO GRNItemDetails = BizActionObj.GRNItemDetails;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateGRNForBarcode");
                command.Connection = con;

                dbServer.AddInParameter(command, "Id", DbType.Int64, GRNItemDetails.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, GRNItemDetails.UnitId);

                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

                trans.Commit();


            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.GRNItemDetails = null;

            }
            finally
            {

                con.Close();
                trans = null;
            }

            return BizActionObj;
        }

    }
}
