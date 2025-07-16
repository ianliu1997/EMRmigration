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
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    public class clsGRNDAL : clsBaseGRNDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;

        //By rohinee for audit trail dated 29/9/16
        bool IsAuditTrail = false;

        #endregion

        private clsGRNDAL()
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

                // By Rohinee For Enable/Disable Audit Trail Dated 26/9/16
                IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail"));

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNBizActionVO BizActionObj = valueObject as clsAddGRNBizActionVO;

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

        private clsAddGRNBizActionVO AddDetails(clsAddGRNBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            string strPONO1 = String.Empty;
            Int32 ResultStatus = 0;  // Added by Ashish Z. on Dated 19102016 for Concurency of PO Quantity
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsGRNVO objDetailsVO = BizActionObj.Details;


                if (objDetailsVO.DirectApprove == false)
                {
                    DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddGRN");
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
                    dbServer.AddInParameter(command, "POID", DbType.Int64, objDetailsVO.POID);
                    dbServer.AddInParameter(command, "PaymentModeID", DbType.Int16, (int)objDetailsVO.PaymentModeID);
                    dbServer.AddInParameter(command, "ReceivedByID", DbType.Int64, objDetailsVO.ReceivedByID);
                    dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                    dbServer.AddInParameter(command, "TotalCDiscount", DbType.Double, objDetailsVO.TotalCDiscount);
                    dbServer.AddInParameter(command, "TotalSchDiscount", DbType.Double, objDetailsVO.TotalSchDiscount);
                    dbServer.AddInParameter(command, "TotalVAT", DbType.Double, objDetailsVO.TotalVAT);
                    dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                    //Addde By Bhushanp For GST 24062017
                    dbServer.AddInParameter(command, "TotalSGST", DbType.Double, objDetailsVO.TotalSGST);
                    dbServer.AddInParameter(command, "TotalCGST", DbType.Double, objDetailsVO.TotalCGST);
                    dbServer.AddInParameter(command, "TotalIGST", DbType.Double, objDetailsVO.TotalIGST);

                    // Added By CDS 
                    dbServer.AddInParameter(command, "PrevNetAmount", DbType.Double, objDetailsVO.PrevNetAmount);
                    dbServer.AddInParameter(command, "OtherCharges", DbType.Double, objDetailsVO.OtherCharges);
                    dbServer.AddInParameter(command, "GRNDiscount", DbType.Double, objDetailsVO.GRNDiscount);
                    dbServer.AddInParameter(command, "GRNRoundOff", DbType.Double, objDetailsVO.GRNRoundOff);
                    dbServer.AddInParameter(command, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                    //

                    //dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                    dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.GRNRoundOff);

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
                }
                bool isItemAdd = false;
                #region GRN Main Items

                foreach (var item in objDetailsVO.Items)
                {
                    item.GRNID = BizActionObj.Details.ID;
                    item.GRNUnitID = BizActionObj.Details.UnitId;
                    item.StockDetails.PurchaseRate = item.Rate;
                    item.StockDetails.MRP = item.MRP;

                    bool checkItem = false;
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRN");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command2, "POID", DbType.Int64, item.POID);
                    dbServer.AddInParameter(command2, "POUnitID", DbType.Int64, item.POUnitID);
                    dbServer.AddInParameter(command2, "GRNID", DbType.Int64, item.GRNID);
                    dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddParameter(command2, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.BatchID);
                    dbServer.AddInParameter(command2, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity * item.BaseConversionFactor);                    // Base Quantity            // For Conversion Factor
                    dbServer.AddInParameter(command2, "FreeQuantity", DbType.Double, item.FreeQuantity);
                    dbServer.AddInParameter(command2, "CoversionFactor", DbType.Double, item.BaseConversionFactor);    // Base Conversion Factor   // For Conversion Factor
                    dbServer.AddInParameter(command2, "InputTransactionQuantity", DbType.Double, item.Quantity);     // InputTransactionQuantity // For Conversion Factor                    
                    # region For Conversion Factor
                    dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Double, item.SelectedUOM.ID);               // Transaction UOM      // For Conversion Factor
                    dbServer.AddInParameter(command2, "BaseUMID", DbType.Double, item.BaseUOMID);                            // Base  UOM            // For Conversion Factor
                    dbServer.AddInParameter(command2, "StockUOMID", DbType.Double, item.SUOMID);                            // SUOM UOM                     // For Conversion Factor
                    dbServer.AddInParameter(command2, "StockCF", DbType.Double, item.ConversionFactor);                    // Stocking ConversionFactor     // For Conversion Factor
                    dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, item.Quantity * item.ConversionFactor);  // StockingQuantity // For Conversion Factor
                    # endregion
                    dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate));
                    dbServer.AddInParameter(command2, "MRP", DbType.Double, (item.MRP));
                    dbServer.AddInParameter(command2, "POQty", DbType.Double, item.POQuantity);
                    dbServer.AddInParameter(command2, "PoPendingQty", DbType.Double, item.PendingQuantity);
                    dbServer.AddInParameter(command2, "ActualPendingQty", DbType.Double, item.POPendingQuantity);
                    dbServer.AddInParameter(command2, "Amount", DbType.Double, item.Amount);
                    dbServer.AddInParameter(command2, "VATPercent", DbType.Double, item.VATPercent);
                    dbServer.AddInParameter(command2, "VATAmount", DbType.Double, item.VATAmount);
                    //Addde By Bhushanp For GST 24062017
                    dbServer.AddInParameter(command2, "SGSTPercent", DbType.Double, item.SGSTPercent);
                    dbServer.AddInParameter(command2, "SGSTAmount", DbType.Double, item.SGSTAmount);
                    dbServer.AddInParameter(command2, "CGSTPercent", DbType.Double, item.CGSTPercent);
                    dbServer.AddInParameter(command2, "CGSTAmount", DbType.Double, item.CGSTAmount);
                    dbServer.AddInParameter(command2, "IGSTPercent", DbType.Double, item.IGSTPercent);
                    dbServer.AddInParameter(command2, "IGSTAmount", DbType.Double, item.IGSTAmount);

                    dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, item.CDiscountPercent);
                    dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, item.CDiscountAmount);
                    dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, item.SchDiscountPercent);
                    dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, item.SchDiscountAmount);
                    //Added By CDS For Vat   
                    dbServer.AddInParameter(command2, "ItemTax", DbType.Double, item.ItemTax);
                    dbServer.AddInParameter(command2, "ItemTaxAmount", DbType.Double, item.TaxAmount);
                    dbServer.AddInParameter(command2, "Vattype", DbType.Int32, item.GRNItemVatType);
                    dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int32, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command2, "otherTaxType", DbType.Int32, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int32, item.OtherGRNItemTaxApplicationOn);
                    //END Vat
                    //Added By Bhushan For GST
                    dbServer.AddInParameter(command2, "SGSTTaxType", DbType.Int32, item.GRNSGSTVatType);
                    dbServer.AddInParameter(command2, "SGSTApplicableOn", DbType.Int32, item.GRNSGSTVatApplicationOn);
                    dbServer.AddInParameter(command2, "CGSTTaxType", DbType.Int32, item.GRNCGSTVatType);
                    dbServer.AddInParameter(command2, "CGSTApplicableOn", DbType.Int32, item.GRNCGSTVatApplicationOn);
                    dbServer.AddInParameter(command2, "IGSTTaxType", DbType.Int32, item.GRNIGSTVatType);
                    dbServer.AddInParameter(command2, "IGSTApplicableOn", DbType.Int32, item.GRNIGSTVatApplicationOn);

                    dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
                    dbServer.AddInParameter(command2, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddInParameter(command2, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command2, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
                    dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    // Added By CDS 
                    dbServer.AddInParameter(command2, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                    dbServer.AddInParameter(command2, "DirectApprove", DbType.Boolean, BizActionObj.Details.DirectApprove);
                    //END
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
                    dbServer.AddInParameter(command2, "GRNQtyOnHand", DbType.Double, item.BaseAvailableQuantity); // Added by Ashish Z. on Dated 19Apr16
                    dbServer.AddInParameter(command2, "SrNo", DbType.Int64, item.SrNo);  // Use to link SrNo of Main Item with Free Item
                    //dbServer.AddInParameter(command2, "IsFromAddItem", DbType.Boolean, item.IsFromAddItem);  // added on 06012017

                    dbServer.AddInParameter(command2, "AvgCost", DbType.Decimal, item.AvgCost);
                    dbServer.AddInParameter(command2, "AvgCostAmount", DbType.Double, item.AvgCostAmount);


                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                    dbServer.AddParameter(command2, "GRNDetailID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.GRNDetailID);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command2, "GRNDetailID");
                    item.BatchID = (long)dbServer.GetParameterValue(command2, "BatchID");

                    if (objDetailsVO.ItemsPOGRN.Count > 0 && objDetailsVO.DirectApprove == false)
                    {
                        int ResutStatusFlag = 0;
                        foreach (var itemPOGRN in objDetailsVO.ItemsPOGRN)
                        {
                            StringBuilder sbPOIDList = new StringBuilder();
                            StringBuilder sbPODetailIDList = new StringBuilder();

                            if (itemPOGRN.ItemID == item.ItemID)
                            {
                                foreach (var item1 in objDetailsVO.ItemsPOGRN)
                                {
                                    if (item1.ItemID == itemPOGRN.ItemID)
                                    {
                                        sbPOIDList.Append("," + Convert.ToString(item1.POID));
                                        sbPODetailIDList.Append("," + Convert.ToString(item1.PoItemsID));
                                    }
                                }
                                sbPOIDList.Replace(",", "", 0, 1);
                                sbPODetailIDList.Replace(",", "", 0, 1);

                                DbCommand comand7 = dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
                                comand7.Connection = con;
                                dbServer.AddInParameter(comand7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(comand7, "GRNID", DbType.Int64, objDetailsVO.ID);
                                dbServer.AddInParameter(comand7, "GRNUnitID", DbType.Int64, objDetailsVO.UnitId);
                                dbServer.AddInParameter(comand7, "GRNDetailID", DbType.Int64, item.ID);
                                dbServer.AddInParameter(comand7, "TempResult", DbType.Int64, ResutStatusFlag);
                                dbServer.AddInParameter(comand7, "GRNDetailUnitID", DbType.Int64, objDetailsVO.UnitId);
                                dbServer.AddInParameter(comand7, "POID", DbType.Int64, itemPOGRN.POID);
                                dbServer.AddInParameter(comand7, "POUnitID", DbType.Int64, itemPOGRN.POUnitID);
                                dbServer.AddInParameter(comand7, "PODetailsID", DbType.Int64, itemPOGRN.PoItemsID);
                                dbServer.AddInParameter(comand7, "PODetailsUnitID", DbType.Int64, itemPOGRN.POUnitID);
                                dbServer.AddInParameter(comand7, "ItemID", DbType.Int64, itemPOGRN.ItemID);
                                dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, itemPOGRN.Quantity);
                                //if (item.IsFromAddItem)
                                //    dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, item.Quantity * Convert.ToSingle(item.BaseConversionFactor));
                                //else
                                //    dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, itemPOGRN.Quantity);
                                dbServer.AddInParameter(comand7, "PendingQty", DbType.Decimal, itemPOGRN.POPendingQuantity);
                                dbServer.AddInParameter(comand7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                                dbServer.AddInParameter(comand7, "AddedBy", DbType.Int64, UserVo.ID);
                                dbServer.AddInParameter(comand7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                                dbServer.AddInParameter(comand7, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                                dbServer.AddInParameter(comand7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                                dbServer.AddInParameter(comand7, "BaseQuantity", DbType.Decimal, item.Quantity * Convert.ToSingle(item.BaseConversionFactor)); // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                dbServer.AddInParameter(comand7, "IsForApprove", DbType.Boolean, objDetailsVO.EditForApprove);// Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                dbServer.AddInParameter(comand7, "POIDList", DbType.String, Convert.ToString(sbPOIDList));// item.POIDList);// Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                dbServer.AddInParameter(comand7, "PODetailsIDList", DbType.String, Convert.ToString(sbPODetailIDList)); //item.PODetailsIDList);// Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                //dbServer.AddInParameter(comand7, "IsFromAddItem", DbType.Boolean, itemPOGRN.IsFromAddItem);

                                dbServer.AddParameter(comand7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                                dbServer.AddOutParameter(comand7, "ResultStatus", DbType.Int32, 0);

                                int intStatus7 = dbServer.ExecuteNonQuery(comand7, trans);

                                // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                ResultStatus = Convert.ToInt32(dbServer.GetParameterValue(comand7, "ResultStatus"));
                                if (ResultStatus == 0)
                                {
                                    ResutStatusFlag = 1;
                                }
                                if (ResultStatus == 1)
                                {
                                    BizActionObj.ItemName = item.ItemName;
                                    throw new Exception();
                                }
                                //End

                            }

                        }
                    }


                    #region MMBABU

                    //Only For Against PO
                    if (!BizActionObj.IsDraft && objDetailsVO.Freezed && objDetailsVO.EditForApprove)
                    {
                        if (item.POID > 0)
                        {
                            DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdatePOFromGRN");
                            command4.Connection = con;
                            dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.Quantity * item.BaseConversionFactor);
                            //dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.BaseQuantity);
                            dbServer.AddInParameter(command4, "POItemsID", DbType.Int64, item.ItemID);   //item.PoItemsID
                            dbServer.AddInParameter(command4, "PendingQty", DbType.Decimal, item.POPendingQuantity);
                            dbServer.AddInParameter(command4, "IndentID", DbType.Decimal, item.IndentID);
                            dbServer.AddInParameter(command4, "IndentUnitID", DbType.Decimal, item.IndentUnitID);

                            dbServer.AddInParameter(command4, "POID", DbType.Int64, item.POID);   //item.PoItemsID
                            dbServer.AddInParameter(command4, "POUnitID", DbType.Int64, item.POUnitID);

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
                    //item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity) * item.ConversionFactor; // Prev Commented By CDS                  
                    item.StockDetails.BatchCode = item.BatchCode;
                    item.StockDetails.Date = objDetailsVO.Date;
                    item.StockDetails.Time = objDetailsVO.Time;
                    item.StockDetails.StoreID = objDetailsVO.StoreID;

                    // Added By CDS
                    item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.Quantity);  // InputTransactionQuantity // For Conversion Factor   

                    item.StockDetails.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                    item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                    //item.StockDetails.TransactionQuantity = item.BaseQuantity;         // Base Quantity            // For Conversion Factor
                    item.StockDetails.TransactionQuantity = item.Quantity * Convert.ToSingle(item.BaseConversionFactor);         // Base Quantity            // For Conversion Factor

                    item.StockDetails.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                    item.StockDetails.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                    item.StockDetails.StockingQuantity = item.Quantity * item.ConversionFactor;  // StockingQuantity // For Conversion Factor

                    item.StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor 
                    item.StockDetails.ExpiryDate = item.ExpiryDate;
                    //END

                    item.StockDetails.IsFromOpeningBalance = true;   // 
                    item.StockDetails.MRP = (item.MRP / item.BaseConversionFactor);
                    item.StockDetails.PurchaseRate = (item.Rate / item.BaseConversionFactor);


                    if (objDetailsVO.Freezed == true && objDetailsVO.EditForApprove == true)
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

                if (objDetailsVO.ItemsDeletedMain != null && objDetailsVO.ItemsDeletedMain.Count > 0)
                {
                    foreach (clsGRNDetailsVO itemMainDelete in objDetailsVO.ItemsDeletedMain)
                    {
                        DbCommand command9 = dbServer.GetStoredProcCommand("CIMS_DeleteGRNMainDetails");
                        command9.Connection = con;

                        dbServer.AddInParameter(command9, "ID", DbType.Int64, itemMainDelete.ID);
                        dbServer.AddInParameter(command9, "UnitId", DbType.Int64, itemMainDelete.UnitId);
                        dbServer.AddInParameter(command9, "GRNID", DbType.Int64, itemMainDelete.GRNID);
                        dbServer.AddInParameter(command9, "GRNUnitID", DbType.Int64, itemMainDelete.UnitId);
                        int intStatus2 = dbServer.ExecuteNonQuery(command9, trans);
                    }
                }

                #endregion

                #region For Free Items

                foreach (clsGRNDetailsVO itemMain in objDetailsVO.Items)  //For Loop Main Item
                {

                    foreach (clsGRNDetailsFreeVO itemFree in objDetailsVO.ItemsFree)   //For Loop Free Item
                    {
                        //if (itemMain.ItemID == itemFree.MainItemID && itemMain.BatchCode == itemFree.MainBatchCode && itemMain.ExpiryDate == itemFree.MainExpiryDate && itemMain.MRP == itemFree.MainItemMRP && itemMain.CostRate == itemFree.MainItemCostRate)
                        if (itemMain.SrNo == itemFree.MainSrNo && itemMain.ItemID == itemFree.MainItemID)
                        {

                            itemFree.GRNID = BizActionObj.Details.ID;
                            itemFree.GRNUnitID = BizActionObj.Details.UnitId;
                            itemFree.StockDetails.PurchaseRate = itemFree.Rate;
                            itemFree.StockDetails.MRP = itemFree.MRP;

                            bool checkItem = false;

                            //if (objDetailsVO.Freezed == true) 
                            //{

                            DbCommand command8 = dbServer.GetStoredProcCommand("CIMS_AddUpdateItemBatchByGRNForFree");   //CIMS_AddUpdateItemBatchByGRN
                            command8.Connection = con;
                            dbServer.AddInParameter(command8, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                            if (objDetailsVO.LinkServer != null)
                                dbServer.AddInParameter(command8, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                            dbServer.AddInParameter(command8, "POID", DbType.Int64, itemFree.POID);
                            dbServer.AddInParameter(command8, "POUnitID", DbType.Int64, itemFree.POUnitID);
                            dbServer.AddInParameter(command8, "GRNID", DbType.Int64, itemFree.GRNID);
                            dbServer.AddInParameter(command8, "GRNUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command8, "ItemID", DbType.Int64, itemFree.FreeItemID);
                            // dbServer.AddInParameter(command2, "BatchID", DbType.Int64, item.BatchID);
                            dbServer.AddParameter(command8, "BatchID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, itemFree.FreeBatchID);
                            dbServer.AddInParameter(command8, "BatchCode", DbType.String, itemFree.FreeBatchCode);
                            dbServer.AddInParameter(command8, "ExpiryDate", DbType.DateTime, itemFree.FreeExpiryDate);
                            //dbServer.AddInParameter(command8, "Quantity", DbType.Double, item.Quantity);
                            //dbServer.AddInParameter(command8, "Quantity", DbType.Double, item.Quantity * item.ConversionFactor);
                            //dbServer.AddInParameter(command8, "FreeQuantity", DbType.Double, item.FreeQuantity * item.ConversionFactor);
                            //dbServer.AddInParameter(command8, "CoversionFactor", DbType.Double, item.ConversionFactor);

                            //Added By CDS
                            //dbServer.AddInParameter(command8, "Quantity", DbType.Double, item.BaseQuantity);                    // Base Quantity            // For Conversion Factor
                            dbServer.AddInParameter(command8, "Quantity", DbType.Double, itemFree.Quantity * itemFree.BaseConversionFactor);                    // Base Quantity            // For Conversion Factor
                            dbServer.AddInParameter(command8, "FreeQuantity", DbType.Double, itemFree.FreeQuantity);
                            dbServer.AddInParameter(command8, "CoversionFactor", DbType.Double, itemFree.BaseConversionFactor);    // Base Conversion Factor   // For Conversion Factor
                            //dbServer.AddInParameter(command8, "BaseQuantity", DbType.Double, item.BaseQuantity);            // Base Quantity            // For Conversion Factor                    
                            dbServer.AddInParameter(command8, "InputTransactionQuantity", DbType.Double, itemFree.Quantity);     // InputTransactionQuantity // For Conversion Factor                    
                            //END

                            # region For Conversion Factor
                            dbServer.AddInParameter(command8, "TransactionUOMID", DbType.Double, itemFree.SelectedUOM.ID);               // Transaction UOM      // For Conversion Factor
                            dbServer.AddInParameter(command8, "BaseUMID", DbType.Double, itemFree.BaseUOMID);                            // Base  UOM            // For Conversion Factor

                            dbServer.AddInParameter(command8, "StockUOMID", DbType.Double, itemFree.SUOMID);                            // SUOM UOM                     // For Conversion Factor
                            dbServer.AddInParameter(command8, "StockCF", DbType.Double, itemFree.ConversionFactor);                    // Stocking ConversionFactor     // For Conversion Factor
                            dbServer.AddInParameter(command8, "StockingQuantity", DbType.Double, itemFree.Quantity * itemFree.ConversionFactor);  // StockingQuantity // For Conversion Factor

                            //dbServer.AddInParameter(command8, "StockingQuantity", DbType.Double, item.SingleQuantity * item.ConversionFactor);  // StockingQuantity // For Conversion Factor
                            //dbServer.AddInParameter(command8, "CoversionFactor", DbType.Double, item.BaseQuantity); 
                            //dbServer.AddInParameter(command8, "BaseQuantity", DbType.Double, item.BaseQuantity); 
                            # endregion

                            //dbServer.AddInParameter(command8, "Rate", DbType.Double, (item.Rate / item.ConversionFactor));
                            //dbServer.AddInParameter(command8, "MRP", DbType.Double, (item.MRP / item.ConversionFactor));

                            dbServer.AddInParameter(command8, "Rate", DbType.Double, (itemFree.Rate));
                            dbServer.AddInParameter(command8, "MRP", DbType.Double, (itemFree.MRP));


                            dbServer.AddInParameter(command8, "POQty", DbType.Double, itemFree.POQuantity);
                            dbServer.AddInParameter(command8, "PoPendingQty", DbType.Double, itemFree.PendingQuantity);
                            dbServer.AddInParameter(command8, "ActualPendingQty", DbType.Double, itemFree.POPendingQuantity);


                            dbServer.AddInParameter(command8, "Amount", DbType.Double, itemFree.Amount);
                            dbServer.AddInParameter(command8, "VATPercent", DbType.Double, itemFree.VATPercent);
                            dbServer.AddInParameter(command8, "VATAmount", DbType.Double, itemFree.VATAmount);

                            //Addde By Bhushanp For GST 24062017
                            dbServer.AddInParameter(command8, "SGSTPercent", DbType.Double, itemFree.SGSTPercent);
                            dbServer.AddInParameter(command8, "SGSTAmount", DbType.Double, itemFree.SGSTAmount);
                            dbServer.AddInParameter(command8, "CGSTPercent", DbType.Double, itemFree.CGSTPercent);
                            dbServer.AddInParameter(command8, "CGSTAmount", DbType.Double, itemFree.CGSTAmount);
                            dbServer.AddInParameter(command8, "IGSTPercent", DbType.Double, itemFree.IGSTPercent);
                            dbServer.AddInParameter(command8, "IGSTAmount", DbType.Double, itemFree.IGSTAmount);


                            dbServer.AddInParameter(command8, "CDiscountPercent", DbType.Double, itemFree.CDiscountPercent);
                            dbServer.AddInParameter(command8, "CDiscountAmount", DbType.Double, itemFree.CDiscountAmount);
                            dbServer.AddInParameter(command8, "SchDiscountPercent", DbType.Double, itemFree.SchDiscountPercent);
                            dbServer.AddInParameter(command8, "SchDiscountAmount", DbType.Double, itemFree.SchDiscountAmount);
                            //dbServer.AddInParameter(command8, "ItemTax", DbType.Double, item.ItemTax);

                            //Added By CDS For Vat   
                            dbServer.AddInParameter(command8, "ItemTax", DbType.Double, itemFree.ItemTax);
                            dbServer.AddInParameter(command8, "ItemTaxAmount", DbType.Double, itemFree.TaxAmount);
                            dbServer.AddInParameter(command8, "Vattype", DbType.Int32, itemFree.GRNItemVatType);
                            dbServer.AddInParameter(command8, "VatApplicableon", DbType.Int32, itemFree.GRNItemVatApplicationOn);
                            dbServer.AddInParameter(command8, "otherTaxType", DbType.Int32, itemFree.OtherGRNItemTaxType);
                            dbServer.AddInParameter(command8, "othertaxApplicableon", DbType.Int32, itemFree.OtherGRNItemTaxApplicationOn);
                            //END Vat
                            //Added By Bhushanp For GST For Free Item
                            dbServer.AddInParameter(command8, "SGSTTaxType", DbType.Int32, itemFree.GRNSGSTVatType);
                            dbServer.AddInParameter(command8, "SGSTApplicableOn", DbType.Int32, itemFree.GRNSGSTVatApplicationOn);
                            dbServer.AddInParameter(command8, "CGSTTaxType", DbType.Int32, itemFree.GRNCGSTVatType);
                            dbServer.AddInParameter(command8, "CGSTApplicableOn", DbType.Int32, itemFree.GRNCGSTVatApplicationOn);
                            dbServer.AddInParameter(command8, "IGSTTaxType", DbType.Int32, itemFree.GRNIGSTVatType);
                            dbServer.AddInParameter(command8, "IGSTApplicableOn", DbType.Int32, itemFree.GRNIGSTVatApplicationOn);

                            ///
                            dbServer.AddInParameter(command8, "NetAmount", DbType.Double, itemFree.NetAmount);
                            dbServer.AddInParameter(command8, "Remarks", DbType.String, itemFree.Remarks);

                            dbServer.AddInParameter(command8, "Status", DbType.Boolean, objDetailsVO.Status);
                            dbServer.AddInParameter(command8, "IsConsignment", DbType.Boolean, objDetailsVO.IsConsignment);
                            dbServer.AddInParameter(command8, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command8, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                            // Added By CDS 
                            dbServer.AddInParameter(command8, "EditForApprove", DbType.Boolean, BizActionObj.Details.EditForApprove);
                            dbServer.AddInParameter(command8, "DirectApprove", DbType.Boolean, BizActionObj.Details.DirectApprove);
                            //END

                            dbServer.AddInParameter(command8, "AddedBy", DbType.Int64, UserVo.ID);
                            dbServer.AddInParameter(command8, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            dbServer.AddInParameter(command8, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                            dbServer.AddInParameter(command8, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                            //For assigning supplier through GRN
                            dbServer.AddInParameter(command8, "AssignSupplier", DbType.Boolean, itemFree.AssignSupplier);
                            dbServer.AddInParameter(command8, "SupplierId", DbType.Int64, objDetailsVO.SupplierID);

                            //Added By Somnath
                            dbServer.AddInParameter(command8, "BarCode", DbType.String, itemFree.BarCode);
                            dbServer.AddInParameter(command8, "Date", DbType.DateTime, objDetailsVO.Date);
                            dbServer.AddInParameter(command8, "Time", DbType.DateTime, objDetailsVO.Time);
                            //End

                            dbServer.AddInParameter(command8, "GRNQtyOnHand", DbType.Double, itemFree.BaseAvailableQuantity); // Added by Ashish Z. on Dated 19Apr16


                            // For Main Items 
                            dbServer.AddInParameter(command8, "MainGRNDetailId", DbType.Int64, itemMain.ID);
                            dbServer.AddInParameter(command8, "MainGRNDetailUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            dbServer.AddInParameter(command8, "MainItemID", DbType.Int64, itemMain.ItemID);
                            dbServer.AddInParameter(command8, "MainBatchCode", DbType.String, itemMain.BatchCode);
                            dbServer.AddInParameter(command8, "MainExpiryDate", DbType.DateTime, itemMain.ExpiryDate);
                            dbServer.AddInParameter(command8, "MainMRP", DbType.Double, (itemMain.MRP));
                            dbServer.AddInParameter(command8, "MainRate", DbType.Double, (itemMain.Rate));
                            dbServer.AddInParameter(command8, "MainBatchID", DbType.Int64, itemMain.BatchID);

                            dbServer.AddInParameter(command8, "MainSrNo", DbType.Int64, itemFree.MainSrNo);  // Use to link SrNo of Main Item with Free Item
                            dbServer.AddInParameter(command8, "AvgCost", DbType.Decimal, itemMain.AvgCost);

                            dbServer.AddParameter(command8, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                            //dbServer.AddParameter(command2, "GRNDetailID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                            dbServer.AddParameter(command8, "GRNDetailID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, itemFree.FreeGRNDetailID);  //itemFree.GRNDetailID

                            dbServer.AddOutParameter(command8, "ResultStatus", DbType.Int32, int.MaxValue);

                            int intStatus2 = dbServer.ExecuteNonQuery(command8, trans);
                            //int intStatus2 = dbServer.ExecuteNonQuery(command2);
                            BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command8, "ResultStatus");
                            //item.ID = (long)dbServer.GetParameterValue(command2, "ID");

                            itemFree.ID = (long)dbServer.GetParameterValue(command8, "GRNDetailID");
                            itemFree.FreeBatchID = (long)dbServer.GetParameterValue(command8, "BatchID");

                            #region Commented 1

                            //if (objDetailsVO.ItemsPOGRN.Count > 0 && objDetailsVO.DirectApprove == false)
                            //{
                            //    foreach (var itemPOGRN in objDetailsVO.ItemsPOGRN)
                            //    {
                            //        //if (itemPOGRN.ItemID == item.ItemID && (itemPOGRN.BatchCode == item.BatchCode))
                            //        if (itemPOGRN.ItemID == item.ItemID)
                            //        {
                            //            DbCommand comand7 = dbServer.GetStoredProcCommand("CIMS_AddPOGRNItemsLink");
                            //            comand7.Connection = con;
                            //            dbServer.AddInParameter(comand7, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //            dbServer.AddInParameter(comand7, "GRNID", DbType.Int64, objDetailsVO.ID);
                            //            dbServer.AddInParameter(comand7, "GRNUnitID", DbType.Int64, objDetailsVO.UnitId);

                            //            dbServer.AddInParameter(comand7, "GRNDetailID", DbType.Int64, item.ID);
                            //            dbServer.AddInParameter(comand7, "GRNDetailUnitID", DbType.Int64, objDetailsVO.UnitId);

                            //            dbServer.AddInParameter(comand7, "POID", DbType.Int64, itemPOGRN.POID);
                            //            dbServer.AddInParameter(comand7, "POUnitID", DbType.Int64, itemPOGRN.POUnitID);

                            //            dbServer.AddInParameter(comand7, "PODetailsID", DbType.Int64, itemPOGRN.PoItemsID);
                            //            dbServer.AddInParameter(comand7, "PODetailsUnitID", DbType.Int64, itemPOGRN.POUnitID);



                            //            dbServer.AddInParameter(comand7, "ItemID", DbType.Int64, itemPOGRN.ItemID);

                            //            dbServer.AddInParameter(comand7, "Quantity", DbType.Decimal, itemPOGRN.Quantity);
                            //            dbServer.AddInParameter(comand7, "PendingQty", DbType.Decimal, itemPOGRN.POPendingQuantity);

                            //            dbServer.AddInParameter(comand7, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //            dbServer.AddInParameter(comand7, "AddedBy", DbType.Int64, UserVo.ID);
                            //            dbServer.AddInParameter(comand7, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                            //            dbServer.AddInParameter(comand7, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                            //            dbServer.AddInParameter(comand7, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                            //            dbServer.AddParameter(comand7, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                            //            int intStatus7 = dbServer.ExecuteNonQuery(comand7, trans);

                            //            //    isItemAdd = true;
                            //            //}
                            //            //else
                            //            //{
                            //            //    isItemAdd = false;
                            //            //}

                            //        }

                            //    }
                            //}

                            #endregion


                            #region MMBABU

                            #region Commented 2

                            ////Only For Against PO
                            //if (!BizActionObj.IsDraft && objDetailsVO.Freezed && objDetailsVO.EditForApprove)
                            //{
                            //    if (item.POID > 0)
                            //    {
                            //        DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_UpdatePOFromGRN");
                            //        command4.Connection = con;
                            //        dbServer.AddInParameter(command4, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                            //        dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.Quantity * item.BaseConversionFactor);
                            //        //dbServer.AddInParameter(command4, "BalanceQty", DbType.Decimal, item.BaseQuantity);
                            //        dbServer.AddInParameter(command4, "POItemsID", DbType.Int64, item.ItemID);   //item.PoItemsID
                            //        dbServer.AddInParameter(command4, "PendingQty", DbType.Decimal, item.POPendingQuantity);
                            //        dbServer.AddInParameter(command4, "IndentID", DbType.Decimal, item.IndentID);
                            //        dbServer.AddInParameter(command4, "IndentUnitID", DbType.Decimal, item.IndentUnitID);

                            //        dbServer.AddInParameter(command4, "POID", DbType.Int64, item.POID);   //item.PoItemsID
                            //        dbServer.AddInParameter(command4, "POUnitID", DbType.Int64, item.POUnitID);

                            //        dbServer.AddInParameter(command4, "GRNID", DbType.Int64, item.GRNID);   //item.PoItemsID
                            //        dbServer.AddInParameter(command4, "GRNUnitID", DbType.Int64, item.GRNUnitID);

                            //        int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                            //    }
                            //    //int intStatus4 = dbServer.ExecuteNonQuery(command4);
                            //    //}
                            //}

                            #endregion

                            #endregion



                            itemFree.StockDetails.BatchID = itemFree.FreeBatchID;
                            itemFree.StockDetails.OperationType = InventoryStockOperationType.Addition;
                            itemFree.StockDetails.ItemID = itemFree.FreeItemID;
                            itemFree.StockDetails.TransactionTypeID = InventoryTransactionType.GoodsReceivedNote;
                            itemFree.StockDetails.TransactionID = itemFree.GRNID;
                            //item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity)
                            //item.StockDetails.TransactionQuantity = (item.Quantity + item.FreeQuantity) * item.ConversionFactor; // Prev Commented By CDS                  
                            itemFree.StockDetails.BatchCode = itemFree.FreeBatchCode;
                            itemFree.StockDetails.Date = objDetailsVO.Date;
                            itemFree.StockDetails.Time = objDetailsVO.Time;
                            itemFree.StockDetails.StoreID = objDetailsVO.StoreID;

                            // Added By CDS
                            itemFree.StockDetails.InputTransactionQuantity = Convert.ToSingle(itemFree.Quantity);  // InputTransactionQuantity // For Conversion Factor   

                            itemFree.StockDetails.BaseUOMID = itemFree.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                            itemFree.StockDetails.BaseConversionFactor = itemFree.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                            //item.StockDetails.TransactionQuantity = item.BaseQuantity;         // Base Quantity            // For Conversion Factor
                            itemFree.StockDetails.TransactionQuantity = itemFree.Quantity * Convert.ToSingle(itemFree.BaseConversionFactor);         // Base Quantity            // For Conversion Factor

                            itemFree.StockDetails.SUOMID = itemFree.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                            itemFree.StockDetails.ConversionFactor = itemFree.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                            itemFree.StockDetails.StockingQuantity = itemFree.Quantity * itemFree.ConversionFactor;  // StockingQuantity // For Conversion Factor

                            itemFree.StockDetails.SelectedUOM.ID = itemFree.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor 
                            itemFree.StockDetails.ExpiryDate = itemFree.FreeExpiryDate;
                            //END

                            itemFree.StockDetails.IsFromOpeningBalance = true;   // 
                            itemFree.StockDetails.MRP = (itemFree.MRP / itemFree.BaseConversionFactor);
                            itemFree.StockDetails.PurchaseRate = (itemFree.Rate / itemFree.BaseConversionFactor);


                            if (objDetailsVO.Freezed == true && objDetailsVO.EditForApprove == true)
                            {
                                clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                                clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                                obj.Details = itemFree.StockDetails;
                                obj.Details.ID = 0;
                                if (!BizActionObj.IsDraft)
                                {
                                    obj = (clsAddItemStockBizActionVO)objBaseDAL.AddFree(obj, UserVo, con, trans);

                                    if (obj.SuccessStatus == -1)
                                        throw new Exception("Error");

                                    itemFree.StockDetails.ID = obj.Details.ID;
                                }
                                else
                                {
                                    itemFree.StockDetails.ID = 0;
                                }
                            }

                        } // if

                    }  //For Loop Free Item

                }  //For Loop Main Item

                if (objDetailsVO.ItemsDeletedFree != null && objDetailsVO.ItemsDeletedFree.Count > 0)
                {
                    foreach (clsGRNDetailsFreeVO itemMainDelete in objDetailsVO.ItemsDeletedFree)
                    {
                        DbCommand command10 = dbServer.GetStoredProcCommand("CIMS_DeleteGRNFreeDetails");
                        command10.Connection = con;

                        dbServer.AddInParameter(command10, "ID", DbType.Int64, itemMainDelete.ID);
                        dbServer.AddInParameter(command10, "UnitId", DbType.Int64, itemMainDelete.UnitId);
                        dbServer.AddInParameter(command10, "GRNID", DbType.Int64, itemMainDelete.GRNID);
                        dbServer.AddInParameter(command10, "GRNUnitID", DbType.Int64, itemMainDelete.UnitId);
                        int intStatus21 = dbServer.ExecuteNonQuery(command10, trans);
                    }
                }

                #endregion  // For Free Items


                // Added By CDS For Approve GRN..........

                if (objDetailsVO.Freezed == true && objDetailsVO.EditForApprove == true)
                {
                    clsBaseGRNDAL objBaseDAL = clsBaseGRNDAL.GetInstance();
                    clsUpdateGRNForApprovalVO obj = new clsUpdateGRNForApprovalVO();

                    obj.Details = objDetailsVO;
                    //obj.Details.ID = 0;
                    if (!BizActionObj.IsDraft)
                    {
                        obj = (clsUpdateGRNForApprovalVO)objBaseDAL.UpdateGRNForApproval(obj, UserVo, con, trans);

                        if (obj.SuccessStatus == -1)
                            throw new Exception("Error");
                    }
                }
                // END


                // Added By CDS For Update Indent Status through  GRN..........
                //  if (objDetailsVO.StoreID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IndentStoreID)
                if ((int)objDetailsVO.GRNType == 2 && objDetailsVO.Freezed == true && objDetailsVO.EditForApprove == true)
                {

                    StringBuilder sbPOIDList = new StringBuilder();
                    StringBuilder sbPOUnitIDList = new StringBuilder();

                    foreach (var item in objDetailsVO.Items)
                    {
                        if (item.POID > 0)
                        {
                            sbPOIDList.Append("," + Convert.ToString(item.POID));
                            sbPOUnitIDList.Append("," + Convert.ToString(item.POUnitID));
                        }
                    }

                    sbPOIDList.Replace(",", "", 0, 1);
                    sbPOUnitIDList.Replace(",", "", 0, 1);

                    DbCommand comand8 = dbServer.GetStoredProcCommand("CIMS_ChangeIndentStatusFromGRNAtSameIndentingSore");
                    comand8.Connection = con;

                    dbServer.AddInParameter(comand8, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                    dbServer.AddInParameter(comand8, "ipPOIDList", DbType.String, Convert.ToString(sbPOIDList));
                    dbServer.AddInParameter(comand8, "ipPOUnitIDList", DbType.String, Convert.ToString(sbPOUnitIDList));

                    //dbServer.AddOutParameter(comand8, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus = dbServer.ExecuteNonQuery(comand8, trans);


                    //BizAction.SuccessStatus = (int)dbServer.GetParameterValue(comand8, "ResultStatus");
                    //foreach (clsServiceMasterVO item in BizActionObj.ipServiceList)
                    //{
                    //    sbServiceList.Append("," + Convert.ToString(item.ID));
                    //    sbTariffList.Append("," + Convert.ToString(item.TariffID));
                    //    sbPackageList.Append("," + Convert.ToString(item.PackageID));
                    //}
                    //sbServiceList.Replace(",", "", 0, 1);
                    //sbTariffList.Replace(",", "", 0, 1);
                    //sbPackageList.Replace(",", "", 0, 1);
                    //dbServer.AddInParameter(command, "ipServiceList", DbType.String, Convert.ToString(sbServiceList));
                    //dbServer.AddInParameter(command, "ipTariffList", DbType.String, Convert.ToString(sbTariffList));
                    //dbServer.AddInParameter(command, "ipPackageList", DbType.String, Convert.ToString(sbPackageList));
                }
                //END

                trans.Commit();

                // By Rohinee for activity log==============================================
                if (IsAuditTrail == true)
                {
                    if (BizActionObj.LogInfoList != null)
                    {
                        if (objDetailsVO.Items.Count > 0)  //for item return id
                        {
                            LogInfo LogInformation = new LogInfo();
                            LogInformation.guid = new Guid();
                            LogInformation.UserId = UserVo.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 140 : Get Output Parameters From T_GRN " //+ Convert.ToString(lineNumber)
                                                   + "Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                                    + " , GRNNO :" + Convert.ToString(BizActionObj.Details.GRNNO)
                                                    + " , GRN ID : " + Convert.ToString(BizActionObj.Details.ID)
                                                    + " , GRN UnitId : " + Convert.ToString(BizActionObj.Details.UnitId);
                            BizActionObj.LogInfoList.Add(LogInformation);
                        }
                        if (BizActionObj.LogInfoList.Count > 0 && IsAuditTrail == true)
                        {
                            SetLogInfo(BizActionObj.LogInfoList, UserVo.ID);
                            BizActionObj.LogInfoList.Clear();
                        }
                    }
                }
                //=================================================================================

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                if (ResultStatus == 1)
                {
                    BizActionObj.SuccessStatus = -2;
                }
                else
                {
                    BizActionObj.SuccessStatus = -1;
                }
                //BizActionObj.SuccessStatus = -1;
                BizActionObj.Details = null;

            }
            finally
            {
                con.Close();
                trans = null;
            }
            return BizActionObj;
        }
        //by Rohinee Dated 29/9/16 For Audit Trail=================================================================================
        private void SetLogInfo(List<LogInfo> objLogList, long userID)
        {
            try
            {
                if (objLogList != null && objLogList.Count > 0)
                {
                    foreach (LogInfo itemLog in objLogList)
                    {
                        logManager.LogInfo(itemLog.guid, userID, itemLog.TimeStamp, itemLog.ClassName, itemLog.MethodName, itemLog.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                logManager.LogError(userID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(),
                    MethodBase.GetCurrentMethod().ToString(), ex.ToString());
            }
        }
        //=========================================================================================================================
        #region Added By CDS For Approve And Reject GRN

        public override IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateGRNForApprovalVO BizActionObj = valueObject as clsUpdateGRNForApprovalVO;

            if (BizActionObj.Details.ID > 0)
                BizActionObj = ApproveGRN(BizActionObj, UserVo, null, null);

            return valueObject;
        }

        public override IValueObject UpdateGRNForApproval(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsUpdateGRNForApprovalVO BizActionObj = valueObject as clsUpdateGRNForApprovalVO;

            if (BizActionObj.Details.ID > 0)
                BizActionObj = ApproveGRN(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;
        }

        private clsUpdateGRNForApprovalVO ApproveGRN(clsUpdateGRNForApprovalVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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

                clsGRNVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdaterGRNApprovalStatus");
                command.Connection = con;

                dbServer.AddInParameter(command, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                dbServer.AddInParameter(command, "ApprovedOrRejectedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
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


        public override IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo)
        {
            clsUpdateGRNForRejectionVO BizActionObj = valueObject as clsUpdateGRNForRejectionVO;

            if (BizActionObj.Details.ID > 0)
                BizActionObj = RejectGRN(BizActionObj, UserVo, null, null);

            return valueObject;
        }

        public override IValueObject UpdateGRNForRejection(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {

            clsUpdateGRNForRejectionVO BizActionObj = valueObject as clsUpdateGRNForRejectionVO;

            if (BizActionObj.Details.ID > 0)
                BizActionObj = RejectGRN(BizActionObj, UserVo, myConnection, myTransaction);

            return valueObject;
        }

        private clsUpdateGRNForRejectionVO RejectGRN(clsUpdateGRNForRejectionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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


                clsGRNVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdaterGRNRejectedStatus");
                command.Connection = con;

                dbServer.AddInParameter(command, "ApprovedOrRejectedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                dbServer.AddInParameter(command, "ApprovedOrRejectedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
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

        #endregion

        public override IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();

            clsGetGRNListBizActionVO BizActionObj = valueObject as clsGetGRNListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNListForSearch");
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
                        BizActionObj.List = new List<clsGRNVO>();
                    while (reader.Read())
                    {
                        clsGRNVO objVO = new clsGRNVO();

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
                        objVO.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        objVO.InvoiceNo = Convert.ToString(DALHelper.HandleDBNull(reader["InvoiceNo"]));
                        objVO.InvoiceDate = (DateTime?)(DALHelper.HandleDate(reader["InvoiceDate"]));

                        #region Added by MMBABU
                        //objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONo"]));
                        //objVO.PODate = (DateTime?)(DALHelper.HandleDate(reader["PODate"]));
                        objVO.PONowithDate = (string)DALHelper.HandleDBNull(reader["PO No - Date"]);
                        objVO.IndentNowithDate = (string)DALHelper.HandleDBNull(reader["Indent No. - Date"]);
                        #endregion

                        objVO.FileName = (string)DALHelper.HandleDBNull(reader["FileName"]);
                        objVO.File = (byte[])DALHelper.HandleDBNull(reader["FileData"]);
                        objVO.IsFileAttached = (bool)DALHelper.HandleDBNull(reader["IsFileAttached"]);
                        objVO.FileAttached = objVO.IsFileAttached == true ? "Visible" : "Collapsed";
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);

                        // Added By CDS
                        objVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objVO.Approve = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Approved"]));
                        objVO.Reject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Rejected"]));
                        //
                        objVO.ApprovedOrRejectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedOrRejectedBy"]));
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

        public override IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsListBizActionVO BizActionObj = valueObject as clsGetGRNDetailsListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemsList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "Freezed", DbType.Int64, BizActionObj.Freezed);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNDetailsVO objVO = new clsGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
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
                        objVO.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objVO.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));

                        objVO.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        objVO.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        objVO.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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
                        //     objVO.IsReadOnly = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));

                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVO.VATPercent > 0)) objVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (!(objVO.CDiscountPercent > 0)) objVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        objVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (!(objVO.SchDiscountPercent > 0)) objVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        //objVO.NetAmount = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);
                        //Added By CDS For Vat
                        objVO.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        objVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        //END

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);

                        //Added By CDS
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])) > 0)
                        {
                            objVO.Quantity = Convert.ToDouble(Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                            objVO.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        }
                        else
                        {
                            objVO.Quantity = ((double)DALHelper.HandleDBNull(reader["Quantity"]));
                            objVO.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])));
                        }

                        objVO.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        objVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));

                        objVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        //objVO.TotalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        objVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        //END
                        objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));

                        objVO.FPNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));

                        objVO.AvgCostForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCost"]));
                        objVO.AvgCostAmountForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCostAmount"]));

                        BizActionObj.List.Add(objVO);

                    }

                }

                reader.NextResult();
                if (reader.HasRows)
                {
                    if (BizActionObj.FreeItemsList == null)
                    {
                        BizActionObj.FreeItemsList = new List<clsGRNDetailsFreeVO>();
                    }
                    while (reader.Read())
                    {
                        clsGRNDetailsFreeVO objFreeVO = new clsGRNDetailsFreeVO();
                        objFreeVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objFreeVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objFreeVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objFreeVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objFreeVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objFreeVO.FreeItemID = (long)DALHelper.HandleDBNull(reader["FreeItemID"]);
                        objFreeVO.FreeBatchID = (long)DALHelper.HandleDBNull(reader["FreeBatchID"]);
                        objFreeVO.FreeBatchCode = (string)DALHelper.HandleDBNull(reader["FreeBatchCode"]);
                        objFreeVO.FreeExpiryDate = (DateTime?)DALHelper.HandleDate(reader["FreeExpiryDate"]);
                        objFreeVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]));
                        objFreeVO.Quantity = (((double)DALHelper.HandleDBNull(reader["FreeQuantity"])) / objFreeVO.ConversionFactor);
                        objFreeVO.FreeQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / objFreeVO.ConversionFactor);
                        objFreeVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        //objFreeVO.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        //objFreeVO.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                        //objFreeVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        //objFreeVO.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        //objFreeVO.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        //objFreeVO.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        objFreeVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objFreeVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        objFreeVO.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        objFreeVO.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objFreeVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objFreeVO.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        objFreeVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        objFreeVO.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));
                        objFreeVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objFreeVO.VATPercent > 0)) objFreeVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        //objFreeVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        //if (!(objFreeVO.CDiscountPercent > 0)) objFreeVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        //objFreeVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        //if (!(objFreeVO.SchDiscountPercent > 0)) objFreeVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        objFreeVO.FreeItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objFreeVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);
                        objFreeVO.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        objFreeVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objFreeVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objFreeVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objFreeVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objFreeVO.FreeItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        // objFreeVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        objFreeVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objFreeVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));

                        //Added By CDS
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])) > 0)
                        {
                            objFreeVO.Quantity = Convert.ToDouble(Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                            objFreeVO.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        }
                        else
                        {
                            objFreeVO.Quantity = ((double)DALHelper.HandleDBNull(reader["FreeQuantity"]));
                            objFreeVO.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])));
                        }

                        objFreeVO.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objFreeVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        objFreeVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objFreeVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objFreeVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objFreeVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objFreeVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objFreeVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        objFreeVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objFreeVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objFreeVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));

                        objFreeVO.FPNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        //objFreeVO.AvgCostForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCost"]));
                        //objFreeVO.AvgCostAmountForFront = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCostAmount"]));

                        BizActionObj.FreeItemsList.Add(objFreeVO);
                    }

                }





                //if (BizActionObj.POGRNList == null)
                //    BizActionObj.POGRNList = new List<clsGRNDetailsVO>();

                //while (reader.Read())
                //{
                //    clsGRNDetailsVO objVO2 = new clsGRNDetailsVO();

                //    objVO2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                //    objVO2.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                //    objVO2.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                //    objVO2.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                //    objVO2.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                //    objVO2.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));

                //    objVO2.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                //    objVO2.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POUnitID"]));

                //    objVO2.PODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"]));
                //    objVO2.PoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"]));
                //    objVO2.PODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailUnitID"]));

                //    objVO2.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));

                //    objVO2.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                //    objVO2.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));

                //    objVO2.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));
                //    //objVO2.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));


                //    BizActionObj.POGRNList.Add(objVO2);

                //}

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

        public override IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsListByIDBizActionVO BizActionObj = valueObject as clsGetGRNDetailsListByIDBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemsByID");
                DbDataReader reader;


                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, BizActionObj.UnitId);

                #region For Main Items

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNDetailsVO objVO = new clsGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        //OLD
                        //objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["coversionFactor"]));

                        objVO.Quantity = (((double)DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        objVO.FreeQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        //Added BY CDS
                        objVO.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        objVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));

                        objVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        //objVO.TotalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));

                        objVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        //Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        objVO.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])); //Added By Ashish Z. on dated 29122016

                        objVO.MainRate = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        objVO.MainMRP = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));

                        //END
                        objVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        objVO.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POQty"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PoPendingQty"])); //OLD Value
                        objVO.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualPendingQty"]));  //OLD Value
                        //New Value
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));
                        objVO.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));

                        objVO.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        objVO.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        objVO.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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

                        //Added By CDS For Vat
                        objVO.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        objVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        //END

                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        objVO.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVO.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        objVO.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);

                        // Added By CDS
                        objVO.PrevNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PrevNetAmount"]));
                        objVO.OtherCharges = Convert.ToDouble(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        objVO.GRNDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["GRNDiscount"]));
                        //objVO.AbatedMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["AbatedMRP"]));

                        objVO.AbatedMRP = Convert.ToDouble((Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])) / (Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercent"])) + 100)) * 100);
                        objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        objVO.GRNApprItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNApprovedItemQty"]));
                        objVO.GRNPendItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNPendingItemQty"]));

                        // to check pending quantity validation at the time of GRN Item Qyantity view & Edit.
                        objVO.GRNDetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));

                        objVO.SrNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["SrNo"]));  //Use to link SrNo of Main Item with Free Item
                        // END
                        //objVO.IsFromAddItem = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFromAddItem"])); //added on Dated 06012017 
                        //Added By Bhushanp FOR GST 26062017
                        objVO.SGSTPercent = (double)DALHelper.HandleDBNull(reader["SGSTPercent"]);
                        if (!(objVO.SGSTPercent > 0)) objVO.SGSTAmount = (double)DALHelper.HandleDBNull(reader["SGSTAmount"]);
                        objVO.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        objVO.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));

                        objVO.CGSTPercent = (double)DALHelper.HandleDBNull(reader["CGSTPercent"]);
                        if (!(objVO.CGSTPercent > 0)) objVO.CGSTAmount = (double)DALHelper.HandleDBNull(reader["CGSTAmount"]);
                        objVO.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        objVO.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));

                        objVO.IGSTPercent = (double)DALHelper.HandleDBNull(reader["IGSTPercent"]);
                        if (!(objVO.IGSTPercent > 0)) objVO.IGSTAmount = (double)DALHelper.HandleDBNull(reader["IGSTAmount"]);
                        objVO.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        objVO.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));

                        objVO.AvgCost = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCost"]));
                        objVO.AvgCostAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvgCostAmount"]));
                        objVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        BizActionObj.List.Add(objVO);

                    }

                }

                #endregion

                reader.NextResult();

                #region For POGRN Details

                if (BizActionObj.POGRNList == null)
                    BizActionObj.POGRNList = new List<clsGRNDetailsVO>();

                while (reader.Read())
                {
                    clsGRNDetailsVO objVO2 = new clsGRNDetailsVO();

                    objVO2.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                    objVO2.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                    objVO2.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                    objVO2.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                    objVO2.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                    objVO2.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));
                    objVO2.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                    objVO2.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POUnitID"]));
                    objVO2.PODetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"]));
                    objVO2.PoItemsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailID"]));
                    objVO2.PODetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PODetailUnitID"]));
                    objVO2.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                    objVO2.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                    objVO2.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));
                    objVO2.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));

                    objVO2.POFinalPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQty"]));  /// Added By ashish Z. on Dated 29122016
                    //objVO2.IsFromAddItem = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFromAddItem"])); //added on Dated 06012017 
                    BizActionObj.POGRNList.Add(objVO2);

                }

                #endregion

                #region For Free Items

                reader.NextResult();

                if (reader.HasRows)
                {
                    if (BizActionObj.ListFree == null)
                        BizActionObj.ListFree = new List<clsGRNDetailsFreeVO>();

                    while (reader.Read())
                    {
                        clsGRNDetailsFreeVO objVOFree = new clsGRNDetailsFreeVO();

                        objVOFree.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVOFree.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVOFree.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVOFree.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                        objVOFree.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));
                        objVOFree.FreeItemID = (long)DALHelper.HandleDBNull(reader["FreeItemID"]);
                        objVOFree.FreeBatchID = (long)DALHelper.HandleDBNull(reader["FreeBatchID"]);
                        objVOFree.FreeBatchCode = (string)DALHelper.HandleDBNull(reader["FreeBatchCode"]);
                        objVOFree.FreeExpiryDate = (DateTime?)DALHelper.HandleDate(reader["FreeExpiryDate"]);

                        objVOFree.FreeGRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));

                        //OLD
                        //objVOFree.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["coversionFactor"]));

                        objVOFree.Quantity = (((double)DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        //objVOFree.FreeQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        ////Added BY CDS
                        objVOFree.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVOFree.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        objVOFree.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));

                        objVOFree.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVOFree.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVOFree.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        //objVOFree.TotalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));

                        objVOFree.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objVOFree.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));
                        //objVOFree.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objVOFree.BaseQuantity = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));

                        objVOFree.MainRate = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        objVOFree.MainMRP = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeMRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"])));

                        //END
                        objVOFree.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVOFree.AvailableStockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["AvailableStockUOM"]));
                        //objVOFree.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POQty"]));
                        //objVOFree.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PoPendingQty"])); //OLD Value
                        //objVOFree.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ActualPendingQty"]));  //OLD Value
                        ////New Value
                        //objVOFree.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));
                        //objVOFree.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POPendingQuantity"]));

                        //objVOFree.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        //objVOFree.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        //objVOFree.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));

                        objVOFree.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVOFree.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        objVOFree.UnitMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        objVOFree.UnitRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVOFree.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["FreeBarCode"]));
                        objVOFree.Rate = (double)DALHelper.HandleDBNull(reader["Rate"]);
                        objVOFree.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"]));
                        objVOFree.IsBatchRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]));

                        objVOFree.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVOFree.VATPercent > 0)) objVOFree.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);

                        //objVOFree.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        //if (!(objVOFree.CDiscountPercent > 0)) objVOFree.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        //objVOFree.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        //if (!(objVOFree.SchDiscountPercent > 0)) objVOFree.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);

                        objVOFree.FreeItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVOFree.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);

                        //Added By CDS For Vat
                        objVOFree.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        objVOFree.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVOFree.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVOFree.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVOFree.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        //END

                        objVOFree.FreeItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        //objVOFree.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        objVOFree.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        objVOFree.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        //objVOFree.IsConsignment = (bool)DALHelper.HandleBoolDBNull(reader["IsConsignment"]);

                        // Added By CDS
                        objVOFree.PrevNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PrevNetAmount"]));
                        objVOFree.OtherCharges = Convert.ToDouble(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        objVOFree.GRNDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["GRNDiscount"]));
                        //objVOFree.AbatedMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["AbatedMRP"]));

                        objVOFree.AbatedMRP = Convert.ToDouble((Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeMRP"])) / (Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercent"])) + 100)) * 100);
                        objVOFree.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        objVOFree.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        objVOFree.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));

                        //objVOFree.GRNApprItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNApprovedItemQty"]));
                        //objVOFree.GRNPendItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNPendingItemQty"]));

                        // to check pending quantity validation at the time of GRN Item Qyantity view & Edit.
                        objVOFree.GRNDetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["FreeQuantity"]));
                        //Added By Bhushanp FOR GST 26062017
                        objVOFree.SGSTPercent = (double)DALHelper.HandleDBNull(reader["SGSTPercent"]);
                        if (!(objVOFree.SGSTPercent > 0)) objVOFree.SGSTAmount = (double)DALHelper.HandleDBNull(reader["SGSTAmount"]);
                        objVOFree.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        objVOFree.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));


                        objVOFree.CGSTPercent = (double)DALHelper.HandleDBNull(reader["CGSTPercent"]);
                        if (!(objVOFree.CGSTPercent > 0)) objVOFree.CGSTAmount = (double)DALHelper.HandleDBNull(reader["CGSTAmount"]);
                        objVOFree.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        objVOFree.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));

                        objVOFree.IGSTPercent = (double)DALHelper.HandleDBNull(reader["IGSTPercent"]);
                        if (!(objVOFree.IGSTPercent > 0)) objVOFree.IGSTAmount = (double)DALHelper.HandleDBNull(reader["IGSTAmount"]);
                        objVOFree.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        objVOFree.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));

                        #region Main Item Details

                        objVOFree.MainItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainItemID"]));
                        objVOFree.MainItemName = Convert.ToString(DALHelper.HandleDBNull(reader["MainItemName"]));
                        objVOFree.MainItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["MainItemCode"]));
                        objVOFree.MainBatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainBatchID"]));
                        objVOFree.MainBatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["MainBatchCode"]));
                        objVOFree.MainExpiryDate = (DateTime?)DALHelper.HandleDate(reader["MainExpiryDate"]);
                        objVOFree.MainItemMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MainItemMRP"]));
                        objVOFree.MainItemCostRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["MainItemCostRate"]));

                        objVOFree.MainSrNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["MainSrNo"]));  //Use to link SrNo of Main Item with Free Item

                        #endregion

                        // END
                        BizActionObj.ListFree.Add(objVOFree);

                    }

                #endregion

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

        public override IValueObject GetGRNForGRNReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO BizActionObj = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForGRNReturnList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ID", DbType.Int64, BizActionObj.ID);
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, BizActionObj.GRNID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitId);
                dbServer.AddInParameter(command, "Freezed", DbType.Int64, BizActionObj.Freezed);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNDetailsVO objVO = new clsGRNDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.BatchID = (long)DALHelper.HandleDBNull(reader["BatchID"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["conversionFactor"]));
                        objVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / objVO.ConversionFactor;
                        objVO.FreeQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["FreeQuantity"])) / objVO.ConversionFactor;
                        objVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.POPendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objVO.POQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["POIQuantity"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));

                        objVO.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        objVO.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POrderUnitId"]));
                        objVO.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["POrderDate"]));
                        objVO.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
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
                        objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));

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

        public override IValueObject DeleteGRNItems(IValueObject valueObject, clsUserVO UserVo)
        {
            //DbConnection con = dbServer.CreateConnection();
            clsAddGRNBizActionVO objBizActionVO = null;
            clsGRNVO objGRNDetailVO = null;
            //DbTransaction trans = null;
            DbCommand command;
            try
            {
                //con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_DeleteGRNItemDetails");
                objBizActionVO = valueObject as clsAddGRNBizActionVO;
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

        public override IValueObject UpdateGRNForBarcode(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            clsAddGRNBizActionVO BizActionObj = valueObject as clsAddGRNBizActionVO;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsGRNDetailsVO GRNItemDetails = BizActionObj.GRNItemDetails;

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

        public override IValueObject GetGRNItemsForQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO BizActionObj = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForQS");
                DbDataReader reader;

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, BizActionObj.StoreId);
                dbServer.AddInParameter(command, "SupplierId", DbType.Int64, BizActionObj.SupplierId);
                dbServer.AddInParameter(command, "GRNNo", DbType.String, BizActionObj.GRNNo);
                dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNDetailsVO objVO = new clsGRNDetailsVO();

                        objVO.GRNDate = Convert.ToDateTime(DALHelper.HandleDate(reader["GRNDate"]));
                        objVO.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));
                        objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objVO.Quantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToDouble(DALHelper.HandleDBNull(reader["CoversionFactor"])));  // Divide by Base Conversion Factor

                        objVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objVO.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                        objVO.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                        objVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                        objVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.SupplierID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierID"]));

                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));

                        objVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["GRNQtyUOM"]));

                        objVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        objVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["CoversionFactor"]));

                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        if (!(objVO.VATPercent > 0)) objVO.VATAmount = (double)DALHelper.HandleDBNull(reader["VATAmount"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        if (!(objVO.CDiscountPercent > 0)) objVO.CDiscountAmount = (double)DALHelper.HandleDBNull(reader["CDiscountAmount"]);
                        objVO.SchDiscountPercent = (double)DALHelper.HandleDBNull(reader["SchDiscountPercent"]);
                        if (!(objVO.SchDiscountPercent > 0)) objVO.SchDiscountAmount = (double)DALHelper.HandleDBNull(reader["SchDiscountAmount"]);
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);
                        objVO.TaxAmount = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));
                        objVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objVO.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"]));
                        objVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["QSPendingQty"]));  //GRN QSPending Quantity
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
            return valueObject;

        }

        public override IValueObject GetGRNItemsForGRNReturnQSSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO BizActionObj = valueObject as clsGetGRNDetailsForGRNReturnListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemsForGRNReturnQSSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "StoreId", DbType.Int64, BizActionObj.StoreId);
                dbServer.AddInParameter(command, "SupplierId", DbType.Int64, BizActionObj.SupplierId);
                dbServer.AddInParameter(command, "GRNNo", DbType.String, BizActionObj.GRNNo);
                dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.ItemName);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNDetailsVO objVO = new clsGRNDetailsVO();
                        objVO.Quantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])));               //  Received Qty 
                        objVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objVO.PendingQuantity = (Convert.ToDouble(DALHelper.HandleDBNull(reader["BalanceQty"])));      // Received Balance Qty not PO..
                        objVO.Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.VATPercent = (double)DALHelper.HandleDBNull(reader["VATPercent"]);
                        objVO.CDiscountPercent = (double)DALHelper.HandleDBNull(reader["CDiscountPercent"]);
                        objVO.SchDiscountPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SchDiscountPercent"]));
                        objVO.ItemTax = (double)DALHelper.HandleDBNull(reader["ItemTax"]);
                        objVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objVO.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objVO.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        objVO.GRNDate = Convert.ToDateTime(DALHelper.HandleDate(reader["GRNDate"]));
                        objVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        objVO.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));  // Received Number not PO No...
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                        objVO.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                        objVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));

                        objVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                        objVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));
                        objVO.ReceivedID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedID"]));
                        objVO.ReceivedUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedUnitID"]));
                        objVO.ReceivedDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ReceivedDetailID"]));

                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["QSStoreID"]));
                        objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["QSStoreName"]));

                        objVO.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"]));

                        //***//
                        objVO.SGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        if (!(objVO.SGSTPercent > 0)) objVO.SGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        objVO.GRNSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        objVO.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));

                        objVO.CGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        if (!(objVO.CGSTPercent > 0)) objVO.CGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        objVO.GRNCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        objVO.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));

                        objVO.IGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        if (!(objVO.IGSTPercent > 0)) objVO.IGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        objVO.GRNIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        objVO.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                        //***//


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
            return valueObject;
        }

        //Added by Ashish Z. for getting GRNInvoice on Dated 25012017
        public override IValueObject GetGRNInvoiceFile(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNListBizActionVO BizActionObj = valueObject as clsGetGRNListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNInvoiceFile");
                DbDataReader reader;

                dbServer.AddInParameter(command, "GRNID", DbType.String, BizActionObj.GRNVO.ID);
                dbServer.AddInParameter(command, "GRNUnitId", DbType.Int64, BizActionObj.GRNVO.UnitId);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsGRNVO grnVO = new clsGRNVO();
                        grnVO.FileName = Convert.ToString(DALHelper.HandleDBNull(reader["FileName"]));
                        grnVO.File = (byte[])DALHelper.HandleDBNull(reader["FileData"]);
                        grnVO.IsFileAttached = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFileAttached"]));
                        BizActionObj.GRNVO = grnVO;
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
            return BizActionObj;
        }
        //End

    }
}
