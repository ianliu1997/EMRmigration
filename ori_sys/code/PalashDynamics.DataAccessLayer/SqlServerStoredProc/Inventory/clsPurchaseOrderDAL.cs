using System;
using System.Collections.Generic;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using System.Windows;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Log;
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    public class clsPurchaseOrderDAL : clsBasePurchaseOrderDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        bool IsAuditTrail = false;
        #endregion

        private clsPurchaseOrderDAL()
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
                IsAuditTrail = Convert.ToBoolean(HMSConfigurationManager.GetValueFromApplicationConfig("IsAuditTrail")); // By Umesh For Enable/Disable Audit Trail
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public override IValueObject AddPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            System.Guid objGUID = new Guid();
            if (IsAuditTrail)
                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Add Purchase Order Start");

            DbConnection con = dbServer.CreateConnection();
            clsAddPurchaseOrderBizActionVO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            Int32 ResultStatus = 0;  // Added by Ashish Z. on Dated 19102016 for Concurency of PR Quantity
            try
            {
                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "try Start ");

                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsAddPurchaseOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;

                #region CIMS_AddPurchaseOrder, CIMS_UpdatePurchaseOrder
                if (objBizActionVO.IsEditMode == true)
                {
                    if (IsAuditTrail)
                        SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_UpdatePurchaseOrder Start");

                    command = dbServer.GetStoredProcCommand("CIMS_UpdatePurchaseOrder");
                    command.Connection = con;
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objPurchaseOrderVO.UpdatedBy);
                    if (objPurchaseOrderVO.UpdatedOn != null) objPurchaseOrderVO.UpdatedOn = objPurchaseOrderVO.UpdatedOn.Trim();
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objPurchaseOrderVO.UpdatedOn);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPurchaseOrderVO.UpdatedDateTime);
                    if (objPurchaseOrderVO.UpdatedWindowsLoginName != null) objPurchaseOrderVO.UpdatedWindowsLoginName = objPurchaseOrderVO.UpdatedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objPurchaseOrderVO.UpdatedWindowsLoginName);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objPurchaseOrderVO.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objPurchaseOrderVO.UpdatedUnitId);

                    if (IsAuditTrail)
                        SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_UpdatePurchaseOrder End ");
                }
                else
                {
                    if (IsAuditTrail)
                        SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrder Start");

                    command = dbServer.GetStoredProcCommand("CIMS_AddPurchaseOrder");
                    command.Connection = con;

                    dbServer.AddInParameter(command, "AddedBy", DbType.Int64, objPurchaseOrderVO.AddedBy);
                    if (objPurchaseOrderVO.AddedOn != null) objPurchaseOrderVO.AddedOn = objPurchaseOrderVO.AddedOn.Trim();
                    dbServer.AddInParameter(command, "AddedOn", DbType.String, objPurchaseOrderVO.AddedOn);
                    dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objPurchaseOrderVO.AddedDateTime);
                    if (objPurchaseOrderVO.AddedWindowsLoginName != null) objPurchaseOrderVO.AddedWindowsLoginName = objPurchaseOrderVO.AddedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, objPurchaseOrderVO.AddedWindowsLoginName);
                    dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                    dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, objPurchaseOrderVO.CreatedUnitId);
                    dbServer.AddInParameter(command, "Status", DbType.Boolean, objPurchaseOrderVO.Status);
                    dbServer.AddInParameter(command, "PONO", DbType.String, objPurchaseOrderVO.PONO);

                    if (IsAuditTrail)
                        SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrder End");
                }

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPurchaseOrderVO.LinkServer);
                if (objPurchaseOrderVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPurchaseOrderVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objPurchaseOrderVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objPurchaseOrderVO.Time);

                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objPurchaseOrderVO.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objPurchaseOrderVO.SupplierID);
                dbServer.AddInParameter(command, "IndentID", DbType.Int64, objPurchaseOrderVO.IndentID);
                dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, objPurchaseOrderVO.IndentUnitID);
                dbServer.AddInParameter(command, "EnquiryID", DbType.Int64, objPurchaseOrderVO.EnquiryID);
                dbServer.AddInParameter(command, "DeliveryDuration", DbType.String, objPurchaseOrderVO.DeliveryDuration);
                dbServer.AddInParameter(command, "DeliveryDays", DbType.Int64, objPurchaseOrderVO.DeliveryDays);
                dbServer.AddInParameter(command, "PaymentMode", DbType.Int16, (Int16)objPurchaseOrderVO.PaymentModeID);
                dbServer.AddInParameter(command, "PaymentTerm", DbType.Int64, objPurchaseOrderVO.PaymentTerms);
                dbServer.AddInParameter(command, "Guarantee_Warrantee", DbType.String, objPurchaseOrderVO.Guarantee_Warrantee);
                dbServer.AddInParameter(command, "Schedule", DbType.Int64, objPurchaseOrderVO.Schedule);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objPurchaseOrderVO.Remarks);
                dbServer.AddInParameter(command, "TotalDiscount", DbType.Decimal, objPurchaseOrderVO.TotalDiscount);
                dbServer.AddInParameter(command, "TotalVAT", DbType.Decimal, objPurchaseOrderVO.TotalVAT);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objPurchaseOrderVO.TotalAmount);
                ////Added By Bhushanp 22062017
                dbServer.AddInParameter(command, "TotalSGST", DbType.Decimal, objPurchaseOrderVO.TotalSGST);
                dbServer.AddInParameter(command, "TotalCGST", DbType.Decimal, objPurchaseOrderVO.TotalCGST);
                dbServer.AddInParameter(command, "TotalIGST", DbType.Decimal, objPurchaseOrderVO.TotalIGST);

                // Added By CDS 
                dbServer.AddInParameter(command, "PrevTotalNetAmount", DbType.Decimal, objPurchaseOrderVO.PrevTotalNet);
                dbServer.AddInParameter(command, "OtherCharges", DbType.Decimal, objPurchaseOrderVO.OtherCharges);
                dbServer.AddInParameter(command, "PODiscount", DbType.Decimal, objPurchaseOrderVO.PODiscount);
                //
                dbServer.AddInParameter(command, "TotalNetAmount", DbType.Decimal, objPurchaseOrderVO.TotalNet);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPurchaseOrderVO.UnitId);

                dbServer.AddInParameter(command, "Direct", DbType.Boolean, objPurchaseOrderVO.Direct);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "PurchaseOrderNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");

                int intStatus = dbServer.ExecuteNonQuery(command, trans);

                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Status intStatus = " + Convert.ToString(intStatus));

                if (objBizActionVO.IsEditMode == false)
                {
                    objBizActionVO.PurchaseOrder.ID = (long)dbServer.GetParameterValue(command, "ID");
                }

                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Purchase Order ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.ID) + " Unit ID - " + Convert.ToString(objPurchaseOrderVO.UnitId));

                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                objBizActionVO.PurchaseOrder.PONO = (string)dbServer.GetParameterValue(command, "PurchaseOrderNumber");

                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.IsEditMode : " + Convert.ToString(objBizActionVO.IsEditMode));

                //if (objPurchaseOrderVO.EditForApprove == false)
                //{
                if (IsAuditTrail && objBizActionVO.LogInfoList != null)   // By Umesh For Audit Trail
                {
                    LogInfo LogInformation = new LogInfo();
                    LogInformation.guid = new Guid();
                    LogInformation.UserId = (UserVo.ID);
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 39 : After Saving PO " + "\r\n"
                                            + "User Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                            + "PO ID : " + Convert.ToString(objBizActionVO.PurchaseOrder.ID) + " "
                                            + "PONO : " + Convert.ToString(objBizActionVO.PurchaseOrder.PONO) + " "
                                            + "\r\n";
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    objBizActionVO.LogInfoList.Add(LogInformation);
                }

                if (objBizActionVO.IsEditMode == true)
                {
                    //Begin:Commented on 17Jan2019 by Prashant Channe, as no transaction maintained 
                    //clsBasePurchaseOrderDAL obj = clsBasePurchaseOrderDAL.GetInstance();
                    //IValueObject objtest = obj.DeletePurchaseOrderItems(objBizActionVO, UserVo);
                    //End:Commented on 17Jan2019 by Prashant Channe, as no transaction maintained 

                    DbCommand command7 = dbServer.GetStoredProcCommand("CIMS_DeletePurchaseOrderDetails");
                    //if (IsAuditTrail)
                    //    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_DeletePurchaseOrderDetails Start");

                    command7.Connection = con;
                    command7.Parameters.Clear();

                    //command7 = dbServer.GetStoredProcCommand("CIMS_DeletePurchaseOrderDetails");
                    //objBizActionVO = valueObject as clsAddPurchaseOrderBizActionVO;
                    //objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                    dbServer.AddInParameter(command7, "POID", DbType.Int64, objPurchaseOrderVO.ID);
                    dbServer.AddInParameter(command7, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                    dbServer.AddInParameter(command7, "ResultStatus", DbType.Int64, 0);
                    int intStatus7 = dbServer.ExecuteNonQuery(command7,trans);
                    //objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command7, "ResultStatus");

                }
                //}
                #endregion //CIMS_AddPurchaseOrder, CIMS_UpdatePurchaseOrder

                #region CIMS_AddPurchaseOrderDetails
                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddPurchaseOrderDetails");
                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPurchaseOrderDetails Start ");

                command1.Connection = con;
                foreach (var it in objBizActionVO.PurchaseOrder.OrgItems)
                {
                    if (IsAuditTrail)
                        SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "it.ItemID : -  " + Convert.ToString(it.ItemID));

                    foreach (var item in objBizActionVO.PurchaseOrder.Items)
                    {
                        if (IsAuditTrail)
                            SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.ItemID : -  " + Convert.ToString(item.ItemID) + " item.CheckInserted : - " + Convert.ToString(item.CheckInserted));

                        if (item.ItemID == it.ItemID && item.CheckInserted == false)
                        {
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 1");

                            item.CheckInserted = true;

                            command1.Parameters.Clear();
                            item.POID = objBizActionVO.PurchaseOrder.ID;

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.POI : -  " + Convert.ToString(item.POID));

                            dbServer.AddInParameter(command1, "LinkServer", DbType.String, objPurchaseOrderVO.LinkServer);
                            if (objPurchaseOrderVO.LinkServer != null)
                                dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objPurchaseOrderVO.LinkServer.Replace(@"\", "_"));
                            dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                            dbServer.AddInParameter(command1, "Date", DbType.DateTime, objPurchaseOrderVO.Date);
                            dbServer.AddInParameter(command1, "Time", DbType.DateTime, objPurchaseOrderVO.Time);
                            dbServer.AddInParameter(command1, "POID", DbType.Int64, item.POID);
                            dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.SinleLineItem : -  " + Convert.ToString(item.SinleLineItem));

                            if (item.SinleLineItem == true)    //InCase Of Single Line Item
                            {
                                dbServer.AddInParameter(command1, "Quantity", DbType.Decimal, item.Quantity);  // Base Quantity i.e All ready Converted On Form Level with Conversion Factor 
                                dbServer.AddInParameter(command1, "Amount", DbType.Decimal, item.Amount / Convert.ToDecimal(item.PurchaseToBaseCF));
                                dbServer.AddInParameter(command1, "DiscountPercent", DbType.Decimal, item.DiscountPercent);
                                dbServer.AddInParameter(command1, "DiscountAmount", DbType.Decimal, item.DiscountAmount / Convert.ToDecimal(item.PurchaseToBaseCF));
                                dbServer.AddInParameter(command1, "VatPercent", DbType.Decimal, item.VATPercent);
                                dbServer.AddInParameter(command1, "VATAmount", DbType.Decimal, item.VATAmount / Convert.ToDecimal(item.PurchaseToBaseCF));

                                dbServer.AddInParameter(command1, "Itemtax", DbType.Decimal, item.ItemVATPercent);
                                dbServer.AddInParameter(command1, "ItemTaxAmount", DbType.Decimal, item.ItemVATAmount / Convert.ToDecimal(item.PurchaseToBaseCF));
                                //Added By Bhushanp For GST 22062017
                                dbServer.AddInParameter(command1, "SGSTPercent", DbType.Decimal, item.SGSTPercent);
                                dbServer.AddInParameter(command1, "SGSTAmount", DbType.Decimal, item.SGSTAmount);
                                dbServer.AddInParameter(command1, "CGSTPercent", DbType.Decimal, item.CGSTPercent);
                                dbServer.AddInParameter(command1, "CGSTAmount", DbType.Decimal, item.CGSTAmount);
                                dbServer.AddInParameter(command1, "IGSTPercent", DbType.Decimal, item.IGSTPercent);
                                dbServer.AddInParameter(command1, "IGSTAmount", DbType.Decimal, item.IGSTAmount);

                                dbServer.AddInParameter(command1, "NetAmount", DbType.Decimal, item.NetAmount / Convert.ToDecimal(item.PurchaseToBaseCF));
                            }
                            else
                            {
                                dbServer.AddInParameter(command1, "Quantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                                dbServer.AddInParameter(command1, "Amount", DbType.Decimal, item.Amount);// Base Quantity    // For Conversion Factor
                                dbServer.AddInParameter(command1, "DiscountPercent", DbType.Decimal, item.DiscountPercent);
                                dbServer.AddInParameter(command1, "DiscountAmount", DbType.Decimal, item.DiscountAmount);
                                dbServer.AddInParameter(command1, "VatPercent", DbType.Decimal, item.VATPercent);
                                dbServer.AddInParameter(command1, "VATAmount", DbType.Decimal, item.VATAmount);

                                dbServer.AddInParameter(command1, "Itemtax", DbType.Decimal, item.ItemVATPercent);
                                dbServer.AddInParameter(command1, "ItemTaxAmount", DbType.Decimal, item.ItemVATAmount);
                                //Added By Bhushanp For GST 22062017
                                dbServer.AddInParameter(command1, "SGSTPercent", DbType.Decimal, item.SGSTPercent);
                                dbServer.AddInParameter(command1, "SGSTAmount", DbType.Decimal, item.SGSTAmount);
                                dbServer.AddInParameter(command1, "CGSTPercent", DbType.Decimal, item.CGSTPercent);
                                dbServer.AddInParameter(command1, "CGSTAmount", DbType.Decimal, item.CGSTAmount);
                                dbServer.AddInParameter(command1, "IGSTPercent", DbType.Decimal, item.IGSTPercent);
                                dbServer.AddInParameter(command1, "IGSTAmount", DbType.Decimal, item.IGSTAmount);

                                dbServer.AddInParameter(command1, "NetAmount", DbType.Decimal, item.NetAmount);
                            }
                            dbServer.AddInParameter(command1, "Rate", DbType.Decimal, item.Rate);  // commented by Ashish Z. on dated 11082016
                            dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP); // commented by Ashish Z. on dated 11082016

                            if (item.SinleLineItem == true)   //InCase Of Single Line Item
                            {

                                dbServer.AddInParameter(command1, "InputTransactionQuantity", DbType.Double, item.Quantity / Convert.ToDecimal(item.PurchaseToBaseCF));
                                if (IsAuditTrail)
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "InputTransactionQuantity : -  " + Convert.ToString(item.Quantity / Convert.ToDecimal(item.PurchaseToBaseCF)));

                            }
                            else
                            {
                                dbServer.AddInParameter(command1, "InputTransactionQuantity", DbType.Double, item.Quantity);   // InputTransactionQuantity // For Conversion Factor
                                if (IsAuditTrail)
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "InputTransactionQuantity : -  " + Convert.ToString(item.Quantity));

                            }
                            # region For Conversion Factor
                            dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Double, item.SelectedUOM.ID);        // Transaction UOM      // For Conversion Factor
                            dbServer.AddInParameter(command1, "BaseUMID", DbType.Double, item.BaseUOMID);                     // Base  UOM            // For Conversion Factor
                            if (item.SinleLineItem == true)   //InCase Of Single Line Item
                            {
                                dbServer.AddInParameter(command1, "BaseCF", DbType.Double, item.PurchaseToBaseCF);
                                dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.Quantity);              // Base Quantity    // For Conversion Factor                             
                                dbServer.AddInParameter(command1, "StockCF", DbType.Double, item.StockingToBaseCF);           // Stocking ConversionFactor     // For Conversion Factor
                                dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, Convert.ToSingle(item.Quantity / Convert.ToDecimal(item.PurchaseToBaseCF)) * item.StockingToBaseCF);  // StockingQuantity // For Conversion Factor                    
                            }
                            else
                            {
                                dbServer.AddInParameter(command1, "BaseCF", DbType.Double, item.BaseConversionFactor);
                                dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));   // Base Quantity    // For Conversion Factor                             
                                dbServer.AddInParameter(command1, "StockCF", DbType.Double, item.ConversionFactor);                                               // Stocking ConversionFactor     // For Conversion Factor
                                dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, Convert.ToSingle(item.Quantity) * item.ConversionFactor);   // StockingQuantity // For Conversion Factor                    
                            }
                            dbServer.AddInParameter(command1, "StockUOMID", DbType.Double, item.SUOMID);                   // SUOM UOM                     // For Conversion Factor

                            //dbServer.AddInParameter(command1, "StockingQuantity", DbType.Double, Convert.ToSingle(item.BaseQuantity) * item.ConversionFactor);  // StockingQuantity // For Conversion Factor                    
                            # endregion
                            //Added By CDS
                            dbServer.AddInParameter(command1, "Taxtype", DbType.Int32, item.POItemVatType);
                            dbServer.AddInParameter(command1, "VatApplicableon", DbType.Int32, item.POItemVatApplicationOn);
                            dbServer.AddInParameter(command1, "otherTaxType", DbType.Int32, item.POItemOtherTaxType);
                            dbServer.AddInParameter(command1, "othertaxApplicableon", DbType.Int32, item.POItemOtherTaxApplicationOn);
                            //Added By Bhushanp For GST Calculation
                            dbServer.AddInParameter(command1, "SGSTType", DbType.Int32, item.POSGSTVatType);
                            dbServer.AddInParameter(command1, "SGSTApplicableOn", DbType.Int32, item.POSGSTVatApplicationOn);
                            dbServer.AddInParameter(command1, "CGSTType", DbType.Int32, item.POCGSTVatType);
                            dbServer.AddInParameter(command1, "CGSTApplicableOn", DbType.Int32, item.POCGSTVatApplicationOn);
                            dbServer.AddInParameter(command1, "IGSTType", DbType.Int32, item.POIGSTVatType);
                            dbServer.AddInParameter(command1, "IGSTApplicableOn", DbType.Int32, item.POIGSTVatApplicationOn);

                            dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Specification);
                            //Added By Umesh
                            dbServer.AddInParameter(command1, "EditForApprove", DbType.Boolean, objPurchaseOrderVO.EditForApprove);
                            dbServer.AddInParameter(command1, "PONO", DbType.String, objPurchaseOrderVO.PONO);   //End
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.ConditionFound : -  " + Convert.ToString(item.ConditionFound));

                            dbServer.AddInParameter(command1, "RateContractID", DbType.Int64, item.RateContractID);
                            dbServer.AddInParameter(command1, "RateContractUnitID", DbType.Int64, item.RateContractUnitID);
                            dbServer.AddInParameter(command1, "RateContractCondition", DbType.String, item.RateContractCondition);

                            #region Commented for Rate Contract 16042018
                            //if (item.ConditionFound)
                            //{
                            //    dbServer.AddInParameter(command1, "RateContractID", DbType.Int64, item.RateContractID);
                            //    dbServer.AddInParameter(command1, "RateContractUnitID", DbType.Int64, item.RateContractID);
                            //    dbServer.AddInParameter(command1, "RateContractCondition", DbType.String, item.RateContractCondition);
                            //}
                            //else
                            //{
                            //    dbServer.AddInParameter(command1, "RateContractID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "RateContractUnitID", DbType.Int64, 0);
                            //    dbServer.AddInParameter(command1, "RateContractCondition", DbType.String, string.Empty);
                            //}
                            #endregion

                            if (item.SelectedCurrency != null)
                            {
                                dbServer.AddInParameter(command1, "CurrencyID", DbType.Int64, item.SelectedCurrency.ID);
                            }
                            else
                            {
                                dbServer.AddInParameter(command1, "CurrencyID", DbType.Int64, 0);

                            }
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.SelectedCurrency : -  " + Convert.ToString(item.SelectedCurrency));

                            dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                            dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.PoDetailsID);
                            int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "intStatus1 : -  " + Convert.ToString(intStatus1));

                            objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.SuccessStatus : -  " + Convert.ToString(objBizActionVO.SuccessStatus));

                            long PoDetailsID = (long)dbServer.GetParameterValue(command1, "ID");

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "PoDetailsID : -  " + Convert.ToString(PoDetailsID));

                            it.PoDetailsID = PoDetailsID;
                            item.PoDetailsID = PoDetailsID;

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF End 1");
                            break;
                        }
                        else if (item.ItemID == it.ItemID && item.CheckInserted == true)
                        {
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "else IF Start 1");

                            it.PoDetailsID = item.PoDetailsID;

                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "item.PoDetailsID : -  " + Convert.ToString(item.PoDetailsID));
                            if (IsAuditTrail)
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "else IF End 1");
                        }
                    }


                    //if (objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0 && objPurchaseOrderVO.EditForApprove == true)
                    if (objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0)
                    {
                        if (IsAuditTrail)
                        {
                            SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 2");
                            SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "objBizActionVO.POIndentList.Count : -  " + Convert.ToString(objBizActionVO.POIndentList.Count));
                        }

                        foreach (var item2 in objBizActionVO.POIndentList)
                        {
                            if (IsAuditTrail)
                            {
                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(),
                                    "item2.ItemID : -  " + Convert.ToString(item2.ItemID) +
                                    " /item2.IndentID : -  " + Convert.ToString(item2.IndentID) +
                                    " /item2.IndentUnitID : -  " + Convert.ToString(item2.IndentUnitID) +
                                    " /item2.IndentDetailID : -  " + Convert.ToString(item2.IndentDetailID) +
                                    " /item2.IndentDetailUnitID : -  " + Convert.ToString(item2.IndentDetailUnitID)
                                    );

                                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(),
                                   "it.ItemID : -  " + Convert.ToString(it.ItemID) +
                                   " /it.IndentID : -  " + Convert.ToString(it.IndentID) +
                                   " /it.IndentUnitID : -  " + Convert.ToString(it.IndentUnitID) +
                                   " /it.IndentDetailID : -  " + Convert.ToString(it.IndentDetailID) +
                                   " /it.IndentDetailUnitID : -  " + Convert.ToString(it.IndentDetailUnitID)
                                   );
                            }
                            #region CIMS_AddPOIndentdetails
                            if (item2.ItemID == it.ItemID && item2.IndentID == it.IndentID && item2.IndentUnitID == it.IndentUnitID && item2.IndentDetailID == it.IndentDetailID
                                && item2.IndentDetailUnitID == it.IndentDetailUnitID)  //if (item2.ItemID == item.ItemID)
                            {
                                //try
                                //{
                                if (IsAuditTrail)
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF Start 3");

                                it.POID = objBizActionVO.PurchaseOrder.ID;
                                DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_AddPOIndentdetails");
                                if (IsAuditTrail)
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "CIMS_AddPOIndentdetails Start");

                                command4.Connection = con;
                                command4.Parameters.Clear();
                                it.POID = objBizActionVO.PurchaseOrder.ID;
                                dbServer.AddInParameter(command4, "POID", DbType.Int64, it.POID);
                                dbServer.AddInParameter(command4, "PoUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                                dbServer.AddInParameter(command4, "PODetailsID", DbType.Int64, it.PoDetailsID);
                                dbServer.AddInParameter(command4, "PODetailsUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                                dbServer.AddInParameter(command4, "IndentID", DbType.Int64, item2.IndentID);
                                dbServer.AddInParameter(command4, "IndentUnitId", DbType.Int64, item2.IndentUnitID);
                                dbServer.AddInParameter(command4, "IndentDetailID", DbType.Int64, item2.IndentDetailID);
                                dbServer.AddInParameter(command4, "IndentDetailUnitID", DbType.Int64, item2.IndentDetailUnitID);
                                dbServer.AddInParameter(command4, "ItemID", DbType.Int64, item2.ItemID);
                                dbServer.AddInParameter(command4, "Quantity", DbType.Decimal, it.Quantity * Convert.ToDecimal(it.BaseConversionFactor));
                                dbServer.AddInParameter(command4, "PendingQuantity", DbType.Decimal, 0);
                                dbServer.AddInParameter(command4, "UnitId", DbType.Decimal, objPurchaseOrderVO.UnitId);
                                //Added By CDS
                                # region For Conversion Factor
                                dbServer.AddInParameter(command4, "Rate", DbType.Decimal, item2.Rate);
                                dbServer.AddInParameter(command4, "MRP", DbType.Decimal, item2.MRP);
                                dbServer.AddInParameter(command4, "InputTransactionQuantity", DbType.Double, it.Quantity);   // InputTransactionQuantity // For Conversion Factor
                                if (IsAuditTrail)
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "it.SinleLineItem : -  " + Convert.ToString(it.SinleLineItem));

                                if (it.SinleLineItem == true)
                                {
                                    dbServer.AddInParameter(command4, "TransactionUOMID", DbType.Double, item2.SelectedUOM.ID);        // Transaction UOM      // For Conversion Factor
                                    dbServer.AddInParameter(command4, "BaseUMID", DbType.Double, item2.BaseUOMID);                    // Base  UOM            // For Conversion Factor
                                    dbServer.AddInParameter(command4, "BaseCF", DbType.Double, item2.BaseConversionFactor);
                                    dbServer.AddInParameter(command4, "BaseQuantity", DbType.Decimal, it.Quantity * Convert.ToDecimal(it.BaseConversionFactor));              // Base Quantity    // For Conversion Factor                                 
                                    dbServer.AddInParameter(command4, "StockUOMID", DbType.Double, item2.SUOMID);                   // SUOM UOM                     // For Conversion Factor
                                    dbServer.AddInParameter(command4, "StockCF", DbType.Double, item2.ConversionFactor);           // Stocking ConversionFactor     // For Conversion Factor
                                    dbServer.AddInParameter(command4, "StockingQuantity", DbType.Double, Convert.ToSingle(it.Quantity) * it.ConversionFactor);  // StockingQuantity // For Conversion Factor                                                    
                                }
                                else
                                {
                                    dbServer.AddInParameter(command4, "TransactionUOMID", DbType.Double, it.SelectedUOM.ID);        // Transaction UOM      // For Conversion Factor
                                    dbServer.AddInParameter(command4, "BaseUMID", DbType.Double, it.BaseUOMID);                    // Base  UOM            // For Conversion Factor
                                    dbServer.AddInParameter(command4, "BaseCF", DbType.Double, it.BaseConversionFactor);
                                    dbServer.AddInParameter(command4, "BaseQuantity", DbType.Decimal, it.Quantity * Convert.ToDecimal(it.BaseConversionFactor));              // Base Quantity    // For Conversion Factor                                 
                                    dbServer.AddInParameter(command4, "StockUOMID", DbType.Double, it.SUOMID);                   // SUOM UOM                     // For Conversion Factor
                                    dbServer.AddInParameter(command4, "StockCF", DbType.Double, it.ConversionFactor);           // Stocking ConversionFactor     // For Conversion Factor
                                    dbServer.AddInParameter(command4, "StockingQuantity", DbType.Double, Convert.ToSingle(it.Quantity) * it.ConversionFactor);  // StockingQuantity // For Conversion Factor                                                    
                                }
                                # endregion
                                //
                                //Add By Umesh
                                dbServer.AddOutParameter(command4, "ID", DbType.Int64, 0);
                                dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0);

                                int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);

                                if (IsAuditTrail)
                                {
                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "intStatus4 : -  " + Convert.ToString(intStatus4));

                                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "IF End 3");
                                }
                                // Added by Ashish Z. for Concurrency between two users.. on Dated 21102016
                                ResultStatus = 0;
                                ResultStatus = Convert.ToInt32(dbServer.GetParameterValue(command4, "ResultStatus"));

                                if (ResultStatus == 1)
                                {
                                    objBizActionVO.ItemCode = item2.ItemCode;
                                    throw new Exception();
                                }
                                //End
                            }
                            #endregion CIMS_AddPOIndentdetails
                        }
                    }

                    #region commented Code
                    /// Update Pending Quantity Against PR 
                    if (objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0 && objPurchaseOrderVO.EditForApprove == true)
                    {

                        //foreach (var item2 in objBizActionVO.POIndentList)
                        //{
                        //    if (item2.ItemID == item.ItemID)
                        //    {
                        //        item.POID = objBizActionVO.PurchaseOrder.ID;
                        //        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_UpdatePOPendingQuantity");
                        //        command5.Connection = con;
                        //        command5.Parameters.Clear();
                        //        item.POID = objBizActionVO.PurchaseOrder.ID;
                        //        dbServer.AddInParameter(command5, "POID", DbType.Int64, item.POID);
                        //        dbServer.AddInParameter(command5, "PoUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                        //        dbServer.AddInParameter(command5, "PODetailsID", DbType.Int64, PoDetailsID);
                        //        dbServer.AddInParameter(command5, "PODetailsUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                        //        dbServer.AddInParameter(command5, "IndentID", DbType.Int64, item2.IndentID);
                        //        dbServer.AddInParameter(command5, "IndentUnitId", DbType.Int64, item2.IndentUnitID);
                        //        dbServer.AddInParameter(command5, "IndentDetailID", DbType.Int64, item2.IndentDetailID);
                        //        dbServer.AddInParameter(command5, "IndentDetailUnitID", DbType.Int64, item2.IndentDetailUnitID);
                        //        dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item2.ItemID);
                        //        dbServer.AddInParameter(command5, "Quantity", DbType.Decimal, 0);
                        //        dbServer.AddInParameter(command5, "BalanceQty", DbType.Decimal,item.BaseQuantity);
                        //        dbServer.AddInParameter(command5, "UnitId", DbType.Decimal, objPurchaseOrderVO.UnitId);                                


                        //        dbServer.AddOutParameter(command5, "ID", DbType.Int64, 0);
                        //        int intStatus4 = dbServer.ExecuteNonQuery(command5, trans);
                        //    }

                        //}


                        //foreach (var item2 in objBizActionVO.POIndentList)
                        //{
                        //    DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                        //    command5.Connection = con;
                        //    //dbServer.AddInParameter(command2, "IndentID", DbType.Int64, item.IndentID);
                        //    //dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, item.IndentUnitID);
                        //    dbServer.AddInParameter(command5, "POID", DbType.Int64, item2.POID);
                        //    dbServer.AddInParameter(command5, "POUnitID", DbType.Decimal, item2.POUnitID);
                        //    //dbServer.AddInParameter(command5, "BalanceQty", DbType.Decimal, item2.BaseQuantity);
                        //    dbServer.AddInParameter(command5, "BalanceQty", DbType.Decimal, item2.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                        //    dbServer.AddInParameter(command5, "ItemID", DbType.Int64, item2.ItemID);
                        //    int intStatus4 = dbServer.ExecuteNonQuery(command5, trans);
                        //}


                    }
                    /// END
                    #endregion commented Code
                }
                #endregion //CIMS_AddPurchaseOrderDetails

                #region CIMS_AddPOTermsAndCondition

                if (objBizActionVO.POTerms != null)
                {
                    foreach (clsPurchaseOrderTerms Terms in objBizActionVO.POTerms)
                    {

                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddPOTermsAndCondition");

                        command5.Connection = con;
                        command5.Parameters.Clear();
                        dbServer.AddInParameter(command5, "POID", DbType.Int64, objBizActionVO.PurchaseOrder.ID);
                        dbServer.AddInParameter(command5, "POUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                        dbServer.AddInParameter(command5, "UnitID", DbType.Decimal, objPurchaseOrderVO.UnitId);
                        dbServer.AddInParameter(command5, "TermsAndConditionID", DbType.Int64, Terms.TermsAndConditionID);
                        dbServer.AddInParameter(command5, "Status", DbType.Boolean, Terms.Status);
                        dbServer.AddOutParameter(command5, "ID", DbType.Int64, 0);
                        int intStatus5 = dbServer.ExecuteNonQuery(command5, trans);

                    }
                }

                #endregion //CIMS_AddPOTermsAndCondition

                trans.Commit();
                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Add Purchase Order Comit Completed");
                if (IsAuditTrail && objBizActionVO.LogInfoList != null)   // By Umesh for activity log
                {
                    if (objBizActionVO.LogInfoList.Count > 0 && IsAuditTrail == true)
                    {
                        SetLogInfo(objBizActionVO.LogInfoList, UserVo.ID);
                        objBizActionVO.LogInfoList.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                if (ResultStatus == 1)
                {
                    objBizActionVO.SuccessStatus = -2;
                }
                else
                {
                    objBizActionVO.SuccessStatus = -1;
                }
                objBizActionVO.PurchaseOrder = null;
                //// Added by rohit
                //MessageBox.Show("Error occurred :" + ex.Message);
            }
            finally
            {
                if (IsAuditTrail)
                    SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Finally Block");
                con.Close();
                trans = null;
                con = null;
            }
            if (IsAuditTrail)
                SetLogInfo(objGUID, UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), "Return");
            return objBizActionVO;
        }


        private void SetLogInfo(Guid ActivityId, long UserId, DateTime TimeStamp, string ClassName, string MethodName, string Message)
        {
            try
            {
                logManager.LogInfo(ActivityId, UserId, TimeStamp, ClassName, MethodName, Message);
            }
            catch (Exception ex)
            {
                logManager.LogError(UserId, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
            }
        }

        public override IValueObject GetPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();


                objBizActionVO = valueObject as clsGetPurchaseOrderBizActionVO;
                //objBizActionVO.PurchaseOrder = valueObject;
                if (objBizActionVO.flagPOFromGRN == true)
                {
                    valueObject = GetPurchaseOrderGromGRN(valueObject, UserVo);
                    return valueObject;
                }

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrder");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
                dbServer.AddInParameter(command, "CancelPO", DbType.Boolean, objBizActionVO.CancelPO);
                dbServer.AddInParameter(command, "UnAPProvePO", DbType.Boolean, objBizActionVO.UnApprovePo);
                dbServer.AddInParameter(command, "ApprovePO", DbType.Boolean, objBizActionVO.ApprovePo);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizActionVO.SearchStoreID);
                if (objBizActionVO.searchFromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizActionVO.searchFromDate);
                if (objBizActionVO.searchToDate != null)
                {
                    //if (objBizActionVO.searchFromDate != null)
                    //{
                    //    if (objBizActionVO.searchFromDate.Equals(objBizActionVO.searchToDate))
                    //        objBizActionVO.searchToDate = objBizActionVO.searchToDate.Value.Date.AddDays(1);
                    //}

                    objBizActionVO.searchToDate = objBizActionVO.searchToDate.Value.Date.AddDays(1);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizActionVO.searchToDate);


                }
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizActionVO.Freezed);
                dbServer.AddInParameter(command, "PONO", DbType.String, objBizActionVO.PONO);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizActionVO.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                bool poStatus;
                bool potype;

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO obj = new clsPurchaseOrderVO();
                        poStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        //if (poStatus)
                        //{
                        obj.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        obj.PONO = (string)DALHelper.HandleDBNull(reader["PONO"]);
                        obj.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        obj.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                        obj.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                        obj.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                        obj.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                        obj.EnquiryID = (long)DALHelper.HandleDBNull(reader["EnquiryID"]);
                        obj.DeliveryDuration = (long)DALHelper.HandleDBNull(reader["DeliveryDuration"]);
                        obj.PaymentMode = (long)DALHelper.HandleDBNull(reader["PaymentMode"]);
                        obj.PaymentTerms = (long)DALHelper.HandleDBNull(reader["PaymentTerm"]);
                        obj.Guarantee_Warrantee = (string)DALHelper.HandleDBNull(reader["Guarantee_Warrantee"]);
                        obj.Schedule = (long)DALHelper.HandleDBNull(reader["Schedule"]);
                        obj.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        obj.TotalAmount = (decimal)DALHelper.HandleDBNull(reader["TotalAmount"]);
                        obj.TotalDiscount = (decimal)DALHelper.HandleDBNull(reader["TotalDiscount"]);
                        obj.TotalVAT = (decimal)DALHelper.HandleDBNull(reader["TotalVat"]);
                        obj.TotalNet = (decimal)DALHelper.HandleDBNull(reader["TotalNetAmount"]);
                        obj.Freezed = (Boolean)DALHelper.HandleDBNull(reader["Freezed"]);
                        obj.IndentNumber = (string)DALHelper.HandleDBNull(reader["IndentNumber"]);
                        obj.DeliveryDays = (Int64)DALHelper.HandleIntegerNull(reader["DeliveryDays"]);
                        potype = (bool)DALHelper.HandleDBNull(reader["POType"]);
                        obj.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                        obj.IsApproveded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsApproved"]));
                        obj.ApprovedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedBy"]));
                        obj.IsCancelded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsCanceld"]));
                        if (potype == false)
                            obj.Type = "Mannual";
                        else if (potype == true)
                            obj.Type = "Auto";

                        #region Added by MMBABU

                        obj.GRNNowithDate = (string)DALHelper.HandleDBNull(reader["GRN No - Date"]);

                        if (obj.IndentNumber != "")
                            obj.IndentNowithDate = (string)DALHelper.HandleDBNull(reader["Indent No - Date"]);

                        #endregion
                        obj.Direct = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Direct"]));

                        obj.POApproveLvlID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApproveLvlID"]));
                        obj.ApprovedLvl1Details = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedLvl1Details"]));
                        obj.ApprovedLvl2Details = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedLvl2Details"]));
                        //Added By Bhushanp For GST 22062017
                        obj.TotalSGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalSGST"]));
                        obj.TotalCGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalCGST"]));
                        obj.TotalIGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalIGST"]));
                        objBizActionVO.PurchaseOrderList.Add(obj);
                        //}
                    }

                }

                reader.NextResult();
                objBizActionVO.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }

            }

            return objBizActionVO;
        }

        public IValueObject GetPurchaseOrderGromGRN(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();


                objBizActionVO = valueObject as clsGetPurchaseOrderBizActionVO;
                //objBizActionVO.PurchaseOrder = valueObject;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderFromGRN");

                dbServer.AddInParameter(command, "FromGRN", DbType.Boolean, objBizActionVO.flagPOFromGRN);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizActionVO.SearchStoreID);
                dbServer.AddInParameter(command, "PONO", DbType.String, objBizActionVO.PONO);
                dbServer.AddInParameter(command, "DeliverydStoreID", DbType.Int64, objBizActionVO.SearchDeliveryStoreID);
                if (objBizActionVO.searchFromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizActionVO.searchFromDate);
                if (objBizActionVO.searchToDate != null)
                {
                    if (objBizActionVO.searchFromDate != null)
                    {
                        if (objBizActionVO.searchFromDate.Equals(objBizActionVO.searchToDate))
                            objBizActionVO.searchToDate = objBizActionVO.searchToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizActionVO.searchToDate);


                }
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, objBizActionVO.Freezed);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizActionVO.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);

                bool poStatus;

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO obj = new clsPurchaseOrderVO();
                        poStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        if (poStatus)
                        {
                            obj.PONO = (string)DALHelper.HandleDBNull(reader["PONO"]);
                            obj.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            obj.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                            obj.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                            obj.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                            obj.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                            obj.IndentID = (long)DALHelper.HandleDBNull(reader["IndentID"]);
                            obj.EnquiryID = (long)DALHelper.HandleDBNull(reader["EnquiryID"]);
                            obj.DeliveryDuration = (long)DALHelper.HandleDBNull(reader["DeliveryDuration"]);
                            obj.PaymentMode = (long)DALHelper.HandleDBNull(reader["PaymentMode"]);
                            obj.PaymentTerms = (long)DALHelper.HandleDBNull(reader["PaymentTerm"]);
                            obj.Guarantee_Warrantee = (string)DALHelper.HandleDBNull(reader["Guarantee_Warrantee"]);
                            obj.Schedule = (long)DALHelper.HandleDBNull(reader["Schedule"]);
                            obj.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                            obj.TotalAmount = (decimal)DALHelper.HandleDBNull(reader["TotalAmount"]);
                            obj.TotalDiscount = (decimal)DALHelper.HandleDBNull(reader["TotalDiscount"]);
                            obj.TotalVAT = (decimal)DALHelper.HandleDBNull(reader["TotalVat"]);
                            obj.TotalNet = (decimal)DALHelper.HandleDBNull(reader["TotalNetAmount"]);
                            obj.Freezed = (Boolean)DALHelper.HandleDBNull(reader["Freezed"]);
                            obj.IsApproveded = (Boolean)DALHelper.HandleDBNull(reader["IsApproved"]);
                            //obj.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                            //Added By Bhushanp For 24062017
                            obj.TotalSGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalSGST"]));
                            obj.TotalCGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalCGST"]));
                            obj.TotalIGST = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalIGST"]));
                            objBizActionVO.PurchaseOrderList.Add(obj);
                        }
                    }
                }
                reader.NextResult();
                objBizActionVO.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }

            }

            return objBizActionVO;
        }

        public override IValueObject GetPurchaseOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetPurchaseOrderDetailsBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderDetails_1");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.SearchID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
                dbServer.AddInParameter(command, "FilterPendingQuantity", DbType.Boolean, objBizActionVO.FilterPendingQuantity);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderDetailVO obj = new clsPurchaseOrderDetailVO();
                        obj.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                        obj.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        obj.PUM = (string)DALHelper.HandleDBNull(reader["PUM"]);
                        //obj.Quantity = (decimal)DALHelper.HandleDBNull(reader["Quantity"]);  // OLD Commented By CDS
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        {
                            //decimal Qty=Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"]));
                            //decimal Bcf=Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                            //decimal Bs=
                            obj.Quantity = Convert.ToDecimal(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])));
                        }
                        else
                            obj.Quantity = (decimal)DALHelper.HandleDBNull(reader["Quantity"]);

                        obj.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.Rate = Convert.ToDecimal(Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) * Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        //    obj.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        obj.CostRate = obj.Quantity * obj.Rate;
                        obj.Amount = (decimal)DALHelper.HandleDBNull(reader["Amount"]);
                        obj.DiscountPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.DiscountAmount = (decimal)DALHelper.HandleDBNull(reader["DiscountAmount"]);
                        obj.VATPercent = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                        obj.VATAmount = (decimal)DALHelper.HandleDBNull(reader["VATAmount"]);
                        //Added By Bhushanp For GST 22062017
                        obj.SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        obj.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        obj.CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        obj.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        obj.IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        obj.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        // Added By CDS 
                        obj.ItemVATPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Itemtax"]));
                        obj.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));

                        obj.POItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Taxtype"]));
                        obj.POItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        obj.POItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        obj.POItemOtherTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        //Added By Bhushanp For GST 22062017
                        obj.POSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtype"]));
                        obj.POSGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        obj.POCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtype"]));
                        obj.POCGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        obj.POIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtype"]));
                        obj.POIGSTVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        obj.HSNCode = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        //
                        obj.PrevTotalNet = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PrevTotalNetAmount"]));
                        obj.OtherCharges = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        obj.PODiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PODiscount"]));
                        obj.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        obj.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));

                        obj.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        obj.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        obj.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        obj.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        obj.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        // objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        //objVO.TotalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));                   
                        //obj.BaseCF = Convert.ToString(DALHelper.HandleDBNull(reader["BaseCF"]));
                        //obj.BaseMRP = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])));
                        obj.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["STUOM"]));
                        obj.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        obj.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        //obj.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                            obj.BaseMRP = Convert.ToSingle(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        else
                            obj.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));

                        //obj.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                            obj.BaseRate = Convert.ToSingle(Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        else
                            obj.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));

                        obj.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        // END 
                        obj.Specification = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        obj.DiscPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.VATPer = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                        obj.BatchesRequired = (bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);
                        obj.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        obj.PendingQuantity = (decimal)DALHelper.HandleDBNull(reader["PendingQuantity"]);
                        obj.PoItemsID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        obj.ItemTax = (double)DALHelper.HandleDBNull(reader["PurchaseTax"]);
                        obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.MRP = Convert.ToDecimal(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) * Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        //    obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        obj.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        obj.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        obj.PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        obj.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                        obj.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        obj.IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"]));
                        obj.IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"]));

                        obj.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        obj.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"]));
                        obj.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        obj.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        //obj.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        obj.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));

                        obj.RateContractCondition = Convert.ToString(DALHelper.HandleDBNull(reader["RCCondition"]));
                        obj.RateContractID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractID"]));
                        obj.RateContractUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractUnitID"]));
                        obj.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        obj.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        obj.SelectedCurrency = new MasterListItem();
                        obj.SelectedCurrency.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CurrencyID"]));

                        obj.PRQTY = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRQTY"]));
                        obj.PRUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PRUOM"]));
                        obj.PRPendingQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRPendingQty"]));
                        obj.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        //PurchaseToBaseCF	StockingToBaseCF
                        obj.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseToBaseCF"]));
                        obj.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingToBaseCF"]));
                        obj.NetAmount = (decimal)DALHelper.HandleDBNull(reader["NetAmount"]);
                        obj.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        obj.POApprItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POApprItemQty"]));
                        obj.POPendingItemQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["POPendingItemQty"]));

                        obj.PODetailsViewTimeQty = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));  // to check pending quantity validation at the time of PO Item Qyantity view & Edit.
                        obj.PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUID"]));

                        obj.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));

                        objBizActionVO.PurchaseOrderList.Add(obj);
                    }




                }
                reader.NextResult();
                if (objBizActionVO.PoIndentList == null)
                    objBizActionVO.PoIndentList = new List<clsPurchaseOrderDetailVO>();
                while (reader.Read())
                {
                    clsPurchaseOrderDetailVO obj = new clsPurchaseOrderDetailVO();
                    obj.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                    obj.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POUnitID"]));
                    obj.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                    obj.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                    obj.IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"]));
                    obj.IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"]));
                    obj.Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"]));
                    obj.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));

                    obj.PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PoDetailsID"]));
                    obj.PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PoDetailsUnitID"]));

                    //Rate	MRP	TransactionUOMID	BaseUMID	StockUOMID	StockCF	StockingQuantity	BaseCF	BaseQuantity	InputTransactionQuantity
                    obj.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                    obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                    obj.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                    obj.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                    obj.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));

                    obj.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                    obj.StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                    obj.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                    obj.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                    //obj.InputTransactionQuantity = Convert.ToInt64(DALHelper.HandleDBNull(reader["InputTransactionQuantity"]));


                    objBizActionVO.PoIndentList.Add(obj);
                }

                reader.NextResult();
                if (objBizActionVO.POTerms == null)
                    objBizActionVO.POTerms = new List<clsPurchaseOrderTerms>();
                while (reader.Read())
                {
                    clsPurchaseOrderTerms obj = new clsPurchaseOrderTerms();
                    obj.TermsAndConditionID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TermsAndConditionID"]));
                    obj.Status = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Status"]));
                    objBizActionVO.POTerms.Add(obj);
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }

            }

            return objBizActionVO;
        }

        #region Added by Ashish Z.
        public override IValueObject GetPurchaseOrderDetailsForGRNAgainstPOSearch(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetPurchaseOrderDetailsBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderDetailsForGRNAgainstPOSearch");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.SearchID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
                dbServer.AddInParameter(command, "FilterPendingQuantity", DbType.Boolean, objBizActionVO.FilterPendingQuantity);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderDetailVO obj = new clsPurchaseOrderDetailVO();
                        obj.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                        obj.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        obj.PUM = (string)DALHelper.HandleDBNull(reader["PUM"]);
                        //obj.Quantity = (decimal)DALHelper.HandleDBNull(reader["Quantity"]);  // OLD Commented By CDS

                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.Quantity = Convert.ToDecimal(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        obj.Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["InputTransactionQuantity"]));

                        obj.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.Rate = Convert.ToDecimal(Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) * Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        //    obj.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);

                        obj.Amount = (decimal)DALHelper.HandleDBNull(reader["Amount"]);
                        obj.DiscountPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.DiscountAmount = (decimal)DALHelper.HandleDBNull(reader["DiscountAmount"]);
                        obj.VATPercent = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                        obj.VATAmount = (decimal)DALHelper.HandleDBNull(reader["VATAmount"]);

                        // Added By CDS 
                        obj.ItemVATPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Itemtax"]));
                        obj.ItemVATAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ItemTaxAmount"]));

                        obj.POItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Taxtype"]));
                        obj.POItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        obj.POItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        obj.POItemOtherTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));

                        obj.PrevTotalNet = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PrevTotalNetAmount"]));
                        obj.OtherCharges = Convert.ToDecimal(DALHelper.HandleDBNull(reader["OtherCharges"]));
                        obj.PODiscount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PODiscount"]));
                        obj.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        obj.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));

                        obj.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        obj.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransUOM"]));
                        obj.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        obj.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        obj.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));

                        // objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        //objVO.TotalQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["StockingQuantity"]));                   
                        //obj.BaseCF = Convert.ToString(DALHelper.HandleDBNull(reader["BaseCF"]));
                        //obj.BaseMRP = Convert.ToSingle(Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])));
                        obj.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["STUOM"]));
                        obj.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUOM"]));
                        obj.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        //obj.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                            obj.BaseMRP = Convert.ToSingle(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        else
                            obj.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));

                        //obj.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));
                        if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                            obj.BaseRate = Convert.ToSingle(Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) / Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        else
                            obj.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));

                        obj.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        // END 
                        obj.Specification = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        obj.DiscPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.VATPer = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                        obj.BatchesRequired = (bool)DALHelper.HandleDBNull(reader["BatchesRequired"]);
                        obj.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        obj.PendingQuantity = (decimal)DALHelper.HandleDBNull(reader["PendingQuantity"]);
                        obj.PoItemsID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        obj.ItemTax = (double)DALHelper.HandleDBNull(reader["PurchaseTax"]);
                        obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.MRP = Convert.ToDecimal(Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])) * Convert.ToDecimal(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        //    obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        obj.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        obj.POID = Convert.ToInt64(DALHelper.HandleDBNull(reader["POID"]));
                        obj.PoDetailsID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.PoDetailsUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));



                        obj.POUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        obj.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"]));
                        obj.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUOM"]));
                        obj.StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        //obj.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        obj.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));

                        obj.RateContractCondition = Convert.ToString(DALHelper.HandleDBNull(reader["RCCondition"]));
                        obj.RateContractID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractID"]));
                        obj.RateContractUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractUnitID"]));
                        obj.ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"]));
                        obj.ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"]));
                        obj.SelectedCurrency = new MasterListItem();
                        obj.SelectedCurrency.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CurrencyID"]));

                        //obj.PRQTY = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRQTY"]));
                        //obj.PRUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PRUOM"]));
                        //obj.PRPendingQty = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PRPendingQty"]));
                        //obj.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                        //obj.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                        //obj.IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"]));
                        //obj.IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"]));
                        //obj.IndentNumber = Convert.ToString(DALHelper.HandleDBNull(reader["IndentNumber"]));
                        //if (Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])) > 0)
                        //    obj.Quantity = Convert.ToDecimal(Convert.ToSingle(DALHelper.HandleDBNull(reader["Quantity"])) / Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])));
                        //else
                        //    obj.Quantity = (decimal)DALHelper.HandleDBNull(reader["Quantity"]);


                        obj.NetAmount = (decimal)DALHelper.HandleDBNull(reader["NetAmount"]);
                        obj.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                        obj.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                        obj.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));

                        obj.GRNApprItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNApprovedItemQty"]));
                        obj.GRNPendItemQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["GRNPendingItemQty"]));

                        obj.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        //Added By Bhushanp For GST 24062017
                        obj.SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercent"]));
                        obj.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        obj.CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercent"]));
                        obj.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        obj.IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercent"]));
                        obj.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));

                        obj.GRNSGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTType"]));
                        obj.GRNSGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));
                        obj.GRNCGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTType"]));
                        obj.GRNCGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));
                        obj.GRNIGSTVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTType"]));
                        obj.GRNIGSTVatApplicableOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                        objBizActionVO.PurchaseOrderList.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }

            }

            return objBizActionVO;
        }



        #endregion


        #region Added by AJ //***//19
        public override IValueObject GetLastTherrPODetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = null;           
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetPurchaseOrderDetailsBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetLastThreePurchaseOrderDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.SearchID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
               
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderDetailVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderDetailVO objPOLastThreePriceList = new clsPurchaseOrderDetailVO();                    
                        objPOLastThreePriceList.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                        objPOLastThreePriceList.BestBaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseRate"]));
                        objPOLastThreePriceList.BestBaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BestBaseMRP"]));
                        objPOLastThreePriceList.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        objPOLastThreePriceList.PODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PoDate"]));                       

                        objBizActionVO.PurchaseOrderList.Add(objPOLastThreePriceList);
                    }

                }     

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }

            }

            return objBizActionVO;
        }
        #endregion


        public override IValueObject GetPendingPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetPendingPurchaseOrderBizActionVO BizActionObj = (clsGetPendingPurchaseOrderBizActionVO)valueObject;
            DbDataReader reader = null;
            try
            {
                //DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetCurrentPurchaseOrderList");
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForDashBoard");
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                //dbServer.AddInParameter(command, "Date", DbType.DateTime, BizActionObj.Date);

                if (BizActionObj.IsOrderBy != null)
                {
                    dbServer.AddInParameter(command, "OrderBy", DbType.Int64, BizActionObj.IsOrderBy);
                }

                //By umesh
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SearchSupplierID);
                //   dbServer.AddInParameter(command, "CancelPO", DbType.Boolean, BizActionObj.CancelPO);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.SearchStoreID);
                if (BizActionObj.searchFromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.searchFromDate);
                if (BizActionObj.searchToDate != null)
                {
                    //if (objBizActionVO.searchFromDate != null)
                    //{
                    //    if (objBizActionVO.searchFromDate.Equals(objBizActionVO.searchToDate))
                    //        objBizActionVO.searchToDate = objBizActionVO.searchToDate.Value.Date.AddDays(1);
                    //}

                    BizActionObj.searchToDate = BizActionObj.searchToDate.AddDays(1);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.searchToDate);
                }
                dbServer.AddInParameter(command, "PONO", DbType.String, BizActionObj.PONO);

                // End

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                dbServer.AddInParameter(command, "Freezed", DbType.Boolean, true);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, BizActionObj.NoOfRecordShow);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, BizActionObj.StartIndex);
                dbServer.AddParameter(command, "TotalRows", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, BizActionObj.OutputTotalRows);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {

                    if (BizActionObj.PurchaseOrderList == null)
                        BizActionObj.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO obj = new clsPurchaseOrderVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        obj.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        obj.PONO = Convert.ToString(DALHelper.HandleDBNull(reader["PONO"]));
                        obj.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        obj.TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        obj.SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"]));
                        obj.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                        obj.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));

                        obj.POAutoCloseDuration = Convert.ToInt32(DALHelper.HandleDBNull(reader["POAutoCloseDuration"]));
                        obj.POAutoCloseDurationDate = (DateTime?)DALHelper.HandleDate(reader["POAutoCloseDate"]);
                        obj.ApprovedByLvl2Date = Convert.ToDateTime(DALHelper.HandleDate(reader["ApprovedLvl2Date"]));
                        obj.POAutoCloseReason = Convert.ToString(DALHelper.HandleDBNull(reader["POAutoCloseReason"]));
                        obj.IsAutoClose = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAutoClose"]));

                        BizActionObj.PurchaseOrderList.Add(obj);
                    }
                }
                reader.NextResult();
                BizActionObj.OutputTotalRows = Convert.ToInt32(dbServer.GetParameterValue(command, "TotalRows"));


            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (reader != null)
                {
                    if (reader.IsClosed == false)
                    {
                        reader.Close();

                    }
                }
            }
            return BizActionObj;

        }

        public override IValueObject DeletePurchaseOrderItems(IValueObject valueObject, clsUserVO UserVo)
        {
            //DbConnection con = dbServer.CreateConnection();
            clsAddPurchaseOrderBizActionVO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            //DbTransaction trans = null;
            DbCommand command;
            try
            {
                //con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_DeletePurchaseOrderDetails");
                objBizActionVO = valueObject as clsAddPurchaseOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                dbServer.AddInParameter(command, "POID", DbType.Int64, objPurchaseOrderVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                dbServer.AddInParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus1 = dbServer.ExecuteNonQuery(command);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {

                //trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrderList = null;
            }

            return objBizActionVO;
        }

        public override IValueObject FreezPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {

            //DbConnection con = dbServer.CreateConnection();
            clsFreezPurchaseOrderBizActionVO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            //DbTransaction trans = null;
            DbCommand command;
            try
            {
                //con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_FreezPurchaseOrder");
                objBizActionVO = valueObject as clsFreezPurchaseOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPurchaseOrderVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                dbServer.AddInParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus1 = dbServer.ExecuteNonQuery(command);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {

                //trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrderList = null;
                //con.Close();
            }
            finally
            {
                //con.Close();




            }

            return objBizActionVO;

        }
        public override IValueObject CancelPurchaseOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsCancelPurchaseOrderBizActionVO bizActionVO = null;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_CancelPO");
                bizActionVO = valueObject as clsCancelPurchaseOrderBizActionVO;
                dbServer.AddInParameter(command, "POID", DbType.Int64, bizActionVO.PurchaseOrder.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.PurchaseOrder.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, false);
                int intStatus1 = dbServer.ExecuteNonQuery(command);

                return bizActionVO;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public override IValueObject UpdatePurchaseOrderForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsUpdatePurchaseOrderForApproval objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsUpdatePurchaseOrderForApproval;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                foreach (var item in objBizActionVO.PurchaseOrder.ids)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdatePurchaseOrderApprovalStatus");
                    command.Connection = con;

                    dbServer.AddInParameter(command, "POApproveLvlID", DbType.Int64, objBizActionVO.PurchaseOrder.POApproveLvlID);
                    dbServer.AddInParameter(command, "ApprovedBy", DbType.String, objBizActionVO.PurchaseOrder.ApprovedBy);
                    dbServer.AddInParameter(command, "ApprovedByID", DbType.Int64, objBizActionVO.PurchaseOrder.ApprovedByID);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.PurchaseOrder.UnitId);
                    int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                }

                //Only IN Case Direct Approve Then 
                if (objBizActionVO.PurchaseOrder.POApproveLvlID == 2 && objPurchaseOrderVO.Items != null && objPurchaseOrderVO.Items.Count > 0) // added by Ashish Z on 280716 for Direct Approve from PO Approval Form //if (objPurchaseOrderVO.Items != null && objPurchaseOrderVO.Items.Count > 0)
                {
                    foreach (var item in objPurchaseOrderVO.Items)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                        command2.Connection = con;
                        //dbServer.AddInParameter(command2, "IndentID", DbType.Int64, item.IndentID);
                        //dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, item.IndentUnitID);
                        dbServer.AddInParameter(command2, "POID", DbType.Int64, item.POID);
                        dbServer.AddInParameter(command2, "POUnitID", DbType.Decimal, item.POUnitID);
                        dbServer.AddInParameter(command2, "PoDetailsID", DbType.Int64, item.PoDetailsID);
                        dbServer.AddInParameter(command2, "PoDetailsUnitID", DbType.Decimal, item.PoDetailsUnitID);

                        dbServer.AddInParameter(command2, "IndentID", DbType.Decimal, item.IndentID);
                        dbServer.AddInParameter(command2, "IndentUnitID", DbType.Decimal, item.IndentUnitID);
                        dbServer.AddInParameter(command2, "IndentDetailID", DbType.Decimal, item.IndentDetailID);
                        dbServer.AddInParameter(command2, "IndentDetailUnitID", DbType.Decimal, item.IndentDetailUnitID);
                        //dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.Quantity);
                        dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                        dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                        int intStatus4 = dbServer.ExecuteNonQuery(command2, trans);
                    }
                }
                // Only IN Case Change And Approve Then 
                if (objBizActionVO.PurchaseOrder.POApproveLvlID == 2 && objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0)// added by Ashish Z on 280716 for Direct Approve from PO Approval Form //if (objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0)
                {

                    foreach (var item2 in objBizActionVO.POIndentList)
                    {
                        //foreach (var item in objPurchaseOrderVO.Items)
                        //{
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                        command3.Connection = con;
                        //dbServer.AddInParameter(command2, "IndentID", DbType.Int64, item.IndentID);
                        //dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, item.IndentUnitID);
                        dbServer.AddInParameter(command3, "POID", DbType.Int64, item2.POID);
                        //dbServer.AddInParameter(command2, "POUnitID", DbType.Decimal, item2.POUnitID);
                        dbServer.AddInParameter(command3, "POUnitID", DbType.Decimal, objPurchaseOrderVO.UnitId);
                        dbServer.AddInParameter(command3, "PoDetailsID", DbType.Int64, item2.PoDetailsID);
                        dbServer.AddInParameter(command3, "PoDetailsUnitID", DbType.Decimal, item2.PoDetailsUnitID);

                        dbServer.AddInParameter(command3, "IndentID", DbType.Decimal, item2.IndentID);
                        dbServer.AddInParameter(command3, "IndentUnitID", DbType.Decimal, item2.IndentUnitID);
                        dbServer.AddInParameter(command3, "IndentDetailID", DbType.Decimal, item2.IndentDetailID);
                        dbServer.AddInParameter(command3, "IndentDetailUnitID", DbType.Decimal, item2.IndentDetailUnitID);
                        //dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.Quantity);
                        dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, item2.Quantity * Convert.ToDecimal(item2.BaseConversionFactor));
                        dbServer.AddInParameter(command3, "ItemID", DbType.Int64, item2.ItemID);
                        int intStatus4 = dbServer.ExecuteNonQuery(command3, trans);
                    }
                }

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrder = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;

        }

        public override IValueObject UpdateRemarkForCancellationPO(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsUpdateRemarkForCancellationPO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsUpdateRemarkForCancellationPO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateRemarkForCancellationPO");
                command.Connection = con;

                dbServer.AddInParameter(command, "CancellationRemark", DbType.String, objBizActionVO.PurchaseOrder.CancellationRemark);
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.PurchaseOrder.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.PurchaseOrder.UnitId);
                dbServer.AddInParameter(command, "CancelledByID", DbType.Int64, UserVo.ID);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrder = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;

        }


        public override IValueObject AddRateContract(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddRateContractBizActionVO BizActionObj = valueObject as clsAddRateContractBizActionVO;

            if (BizActionObj.RateContract.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo);
            else

                //BizActionObj.RateContract.IsEditAfterFreeze = true;  //Added by Prashant Channe on 18/10/2018

                BizActionObj = UpdateDetails(BizActionObj, UserVo);

            return valueObject;
        }

        private clsAddRateContractBizActionVO AddDetails(clsAddRateContractBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsRateContractVO objDetailsVO = BizActionObj.RateContract;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddRateContract");
                dbServer.AddInParameter(command, "Code", DbType.String, objDetailsVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objDetailsVO.Description);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objDetailsVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objDetailsVO.ToDate);
                dbServer.AddInParameter(command, "ContractDate ", DbType.DateTime, objDetailsVO.ContractDate);
                dbServer.AddInParameter(command, "ContractValue", DbType.Decimal, objDetailsVO.ContractValue);
                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, objDetailsVO.IsFreeze);
                dbServer.AddInParameter(command, "SupplierRepresentative", DbType.String, objDetailsVO.SupplierRepresentative);
                dbServer.AddInParameter(command, "ClinicRepresentativeID", DbType.Int64, objDetailsVO.ClinicRepresentativeID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.RateContract.ID = (long)dbServer.GetParameterValue(command, "ID");
                objDetailsVO.UnitId = UserVo.UserLoginInfo.UnitId;


                if (BizActionObj.SuccessStatus == 1 && objDetailsVO.ContractDetails != null && objDetailsVO.ContractDetails.Count > 0)
                {

                    foreach (var item in objDetailsVO.ContractDetails)
                    {

                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRateContractItemDetails");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                        dbServer.AddInParameter(command1, "ContractID", DbType.Int64, objDetailsVO.ID);
                        dbServer.AddInParameter(command1, "ContractUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, item.Rate);
                        dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP);
                        dbServer.AddInParameter(command1, "TotalRate", DbType.Decimal, item.CostRate);
                        dbServer.AddInParameter(command1, "TotalMRP", DbType.Decimal, item.Amount);
                        dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                        dbServer.AddInParameter(command1, "CoversionFactor", DbType.Single, item.ConversionFactor);
                        dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.ConversionFactor));
                        dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                        dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                        dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                        dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);
                        dbServer.AddInParameter(command1, "BaseRate", DbType.Decimal, item.BaseRate);
                        dbServer.AddInParameter(command1, "BaseMRP", DbType.Decimal, item.BaseMRP);
                        dbServer.AddInParameter(command1, "DiscountPercent", DbType.Decimal, item.DiscountPercent);
                        dbServer.AddInParameter(command1, "DiscountAmount", DbType.Decimal, item.DiscountAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Decimal, item.NetAmount);
                        dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Decimal, item.Quantity);


                        dbServer.AddInParameter(command1, "UnlimitedQuantity", DbType.Boolean, item.UnlimitedQuantity);
                        if (item.SelectedCondition != null)
                            dbServer.AddInParameter(command1, "Condition", DbType.String, item.SelectedCondition.Description);
                        else
                            dbServer.AddInParameter(command1, "Condition", DbType.String, null);
                        dbServer.AddInParameter(command1, "MinQuantity", DbType.Decimal, item.MinQuantity);
                        dbServer.AddInParameter(command1, "MaxQuantity", DbType.Decimal, item.MaxQuantity);
                        dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    }
                }

                trans.Commit();


            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.RateContract = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }
        private clsAddRateContractBizActionVO UpdateDetails(clsAddRateContractBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbTransaction trans = null;
            DbConnection con = null;
            try
            {
                con = dbServer.CreateConnection();
                con.Open();
                trans = con.BeginTransaction();

                clsRateContractVO objDetailsVO = BizActionObj.RateContract;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateRateContract");
                dbServer.AddInParameter(command, "Code", DbType.String, objDetailsVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objDetailsVO.Description);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objDetailsVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objDetailsVO.ToDate);
                dbServer.AddInParameter(command, "ContractDate ", DbType.DateTime, objDetailsVO.ContractDate);
                dbServer.AddInParameter(command, "ContractValue", DbType.Decimal, objDetailsVO.ContractValue);
                dbServer.AddInParameter(command, "IsFreeze", DbType.Boolean, objDetailsVO.IsFreeze);
                dbServer.AddInParameter(command, "SupplierRepresentative", DbType.String, objDetailsVO.SupplierRepresentative);
                dbServer.AddInParameter(command, "ClinicRepresentativeID", DbType.Int64, objDetailsVO.ClinicRepresentativeID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Remark", DbType.String, objDetailsVO.Remark);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "ReasonForEdit", DbType.String, objDetailsVO.ReasonForEdit);//Added by Prashant Channe on 20/10/2018, To Modify on Freeze
                dbServer.AddInParameter(command, "IsEditAfterFreeze", DbType.Boolean, objDetailsVO.IsEditAfterFreeze);//Added by Prashant Channe

                dbServer.AddInParameter(command, "ID", DbType.Int64, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus"));
                BizActionObj.RateContract.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));

                if (objDetailsVO.IsEditAfterFreeze == false)//Added by Prashant Channe on 20/10/2018, in case, not edit operation
                {
                    if (BizActionObj.SuccessStatus == 1 && objDetailsVO.ContractDetails != null && objDetailsVO.ContractDetails.Count > 0)
                    {
                        DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_DeleteRateContaractDetails");
                        dbServer.AddInParameter(command2, "ContractID", DbType.Int64, objDetailsVO.ID);
                        dbServer.AddInParameter(command2, "ContractUnitId", DbType.Int64, objDetailsVO.UnitId);
                        int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);

                    }
                }


                if (BizActionObj.SuccessStatus == 1 && objDetailsVO.ContractDetails != null && objDetailsVO.ContractDetails.Count > 0)
                {
                    foreach (var item in objDetailsVO.ContractDetails)
                    {                        
                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddRateContractItemDetails");
                        dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objDetailsVO.UnitId);
                        dbServer.AddInParameter(command1, "ContractID", DbType.Int64, objDetailsVO.ID);
                        dbServer.AddInParameter(command1, "ContractUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                        dbServer.AddInParameter(command1, "Rate", DbType.Decimal, item.Rate);
                        dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP);
                        dbServer.AddInParameter(command1, "TotalRate", DbType.Decimal, item.CostRate);
                        dbServer.AddInParameter(command1, "TotalMRP", DbType.Decimal, item.Amount);
                        dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);
                        dbServer.AddInParameter(command1, "CoversionFactor", DbType.Single, item.ConversionFactor);
                        dbServer.AddInParameter(command1, "StockingQuantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.ConversionFactor));
                        dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);
                        dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                        dbServer.AddInParameter(command1, "BaseQuantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.BaseConversionFactor));
                        dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);
                        dbServer.AddInParameter(command1, "BaseRate", DbType.Decimal, item.BaseRate);
                        dbServer.AddInParameter(command1, "BaseMRP", DbType.Decimal, item.BaseMRP);
                        dbServer.AddInParameter(command1, "DiscountPercent", DbType.Decimal, item.DiscountPercent);
                        dbServer.AddInParameter(command1, "DiscountAmount", DbType.Decimal, item.DiscountAmount);
                        dbServer.AddInParameter(command1, "NetAmount", DbType.Decimal, item.NetAmount);
                        dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                        dbServer.AddInParameter(command1, "Quantity", DbType.Decimal, item.Quantity);


                        dbServer.AddInParameter(command1, "UnlimitedQuantity", DbType.Boolean, item.UnlimitedQuantity);
                        if (item.SelectedCondition != null)
                            dbServer.AddInParameter(command1, "Condition", DbType.String, item.SelectedCondition.Description);
                        else
                            dbServer.AddInParameter(command1, "Condition", DbType.String, null);
                        dbServer.AddInParameter(command1, "MinQuantity", DbType.Decimal, item.MinQuantity);
                        dbServer.AddInParameter(command1, "MaxQuantity", DbType.Decimal, item.MaxQuantity);

                        dbServer.AddInParameter(command1, "ReasonForEdit", DbType.String, item.ReasonForEdit);  // Added by Prashant Channe on 20/10/2018                        

                        dbServer.AddInParameter(command1, "IsEditAfterFreeze", DbType.Boolean, objDetailsVO.IsEditAfterFreeze);//Added by Prashant Channe on 20/10/2018                     
                        
                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);  // Added by Prashant Channe on 24/10/2018

                        
                        int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                        
                    }
                }

                trans.Commit();

            }
            catch (Exception)
            {
                BizActionObj.SuccessStatus = -1;
                trans.Rollback();
                BizActionObj.RateContract = null;
            }
            finally
            {
                con.Close();
                con = null;
                trans = null;
            }
            return BizActionObj;
        }
        public override IValueObject GetRateContract(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetRateContractBizActionVO objBizActionVO = valueObject as clsGetRateContractBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRateContarctList");

                dbServer.AddInParameter(command, "Code", DbType.String, objBizActionVO.Code);
                dbServer.AddInParameter(command, "Description", DbType.String, objBizActionVO.Description);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizActionVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizActionVO.ToDate);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SupplierID);
                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizActionVO.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.String, objBizActionVO.sortExpression);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.RateContract == null)
                        objBizActionVO.RateContract = new List<clsRateContractVO>();
                    while (reader.Read())
                    {
                        clsRateContractVO obj = new clsRateContractVO();
                        obj.ID = (long)DALHelper.HandleIntegerNull(reader["ID"]);
                        obj.UnitId = (long)DALHelper.HandleIntegerNull(reader["UnitId"]);
                        obj.Code = (string)DALHelper.HandleDBNull(reader["Code"]);
                        obj.Description = (string)DALHelper.HandleDBNull(reader["Description"]);
                        obj.ContractDate = (DateTime)DALHelper.HandleDate(reader["ContractDate"]);
                        obj.ContractValue = Convert.ToDecimal(DALHelper.HandleDBNull(reader["ContractValue"]));
                        obj.SupplierID = (long)DALHelper.HandleIntegerNull(reader["SupplierID"]);
                        obj.FromDate = (DateTime?)DALHelper.HandleDate(reader["FromDate"]);
                        obj.ToDate = (DateTime?)DALHelper.HandleDate(reader["ToDate"]);
                        obj.Supplier = (string)DALHelper.HandleDBNull(reader["Supplier"]);
                        obj.SupplierRepresentative = (string)DALHelper.HandleDBNull(reader["SupplierRepresentative"]);
                        obj.ClinicRepresentativeID = (long)DALHelper.HandleIntegerNull(reader["ClinicRepresentativeID"]);
                        obj.Remark = (string)DALHelper.HandleDBNull(reader["Remark"]);
                        obj.IsFreeze = (bool)DALHelper.HandleBoolDBNull(reader["IsFreeze"]);

                        obj.ReasonForEdit =  Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForEdit"]));     // Added by Prashant Channe for Rate Contract Edit After Freeze on 24Oct2018

                        objBizActionVO.RateContract.Add(obj);
                    }

                }

                reader.NextResult();
                objBizActionVO.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch
            {
            }
            return objBizActionVO;
        }

        public override IValueObject CheckContractValidity(IValueObject valueObject, clsUserVO UserVo)
        {
            clsCheckContractBizActionVO objBizActionVO = valueObject as clsCheckContractBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand command = dbServer.GetStoredProcCommand("CheckRateContract");


                dbServer.AddInParameter(command, "ItemIDs", DbType.String, objBizActionVO.ItemIDs);
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizActionVO.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizActionVO.ToDate);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SupplierID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        objBizActionVO.Result = true;
                    }
                }
            }
            catch
            {
            }
            return objBizActionVO;
        }

        public override IValueObject GetRateContractItemDetail(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetRateContractItemDetailsBizActionVO objBizActionVO = valueObject as clsGetRateContractItemDetailsBizActionVO;
            try
            {
                DbDataReader reader = null;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetRateContractItemDetails");

                dbServer.AddInParameter(command, "ContractID", DbType.Int64, objBizActionVO.ContractID);
                dbServer.AddInParameter(command, "ContractUnitId", DbType.Int64, objBizActionVO.ContractUnitId);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.RateContractList == null)
                        objBizActionVO.RateContractList = new List<clsRateContractDetailsVO>();
                    while (reader.Read())
                    {
                        clsRateContractDetailsVO obj = new clsRateContractDetailsVO();
                        obj.ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"]));       // added by Prashant Channe on 24/10/2018 for RC Edit After Freeze
                        obj.ContractID = (long)DALHelper.HandleIntegerNull(reader["ContractID"]);
                        obj.ContractUnitId = (long)DALHelper.HandleIntegerNull(reader["ContractUnitId"]);
                        obj.ItemID = (long)DALHelper.HandleIntegerNull(reader["ItemID"]);
                        obj.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        obj.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        obj.MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]));
                        obj.Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"]));
                        obj.Rate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"]));
                        obj.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        obj.SelectedUOM = new MasterListItem { ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["TransactionUOMID"])), Description = obj.TransUOM };
                        obj.ConversionFactor = Convert.ToSingle((Double)(DALHelper.HandleDBNull(reader["CoversionFactor"])));
                        obj.SUOMID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StockUOMID"]));
                        obj.BaseConversionFactor = Convert.ToSingle((Double)(DALHelper.HandleDBNull(reader["BaseCF"])));
                        obj.BaseUOMID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BaseUMID"]));
                        obj.BaseRate = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseRate"]));
                        obj.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));
                        obj.MainRate = obj.BaseRate;
                        obj.MainMRP = obj.BaseMRP;
                        obj.DiscountPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPercent"]));
                        obj.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        obj.HSNCode = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));


                        obj.UnlimitedQuantity = Convert.ToBoolean(DALHelper.HandleDBNull(reader["UnlimitedQuantity"]));
                        obj.Condition = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));
                        obj.MinQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MinQuantity"]));
                        obj.MaxQuantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MaxQuantity"]));
                        obj.SelectedCondition.Description = Convert.ToString(DALHelper.HandleDBNull(reader["Condition"]));

                        if (obj.SelectedCondition.Description == "=")
                            obj.SelectedCondition.ID = 1;
                        else if (obj.SelectedCondition.Description == "<")
                            obj.SelectedCondition.ID = 2;
                        else if (obj.SelectedCondition.Description == "Between")
                            obj.SelectedCondition.ID = 3;
                        else if (obj.SelectedCondition.Description == ">")
                            obj.SelectedCondition.ID = 4;
                        else if (obj.SelectedCondition.Description.Equals("No Limit"))
                            obj.SelectedCondition.ID = 5;

                        obj.ReasonForEdit = Convert.ToString(DALHelper.HandleDBNull(reader["ReasonForEdit"]));      // added by Prashant Channe on 24/10/2018 for RC Edit After Freeze

                        objBizActionVO.RateContractList.Add(obj);
                    }
                }
            }
            catch
            {
            }
            return objBizActionVO;
        }

        public override IValueObject ClosePurchaseOrderManually(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsClosePurchaseOrderManuallyBizActionVO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsClosePurchaseOrderManuallyBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;

                command = dbServer.GetStoredProcCommand("CIMS_ClosePurchaseOrderManually");
                command.Connection = con;

                dbServer.AddInParameter(command, "POID", DbType.Int64, objBizActionVO.PurchaseOrder.ID);
                dbServer.AddInParameter(command, "POUnitID", DbType.Int64, objBizActionVO.PurchaseOrder.UnitId);
                dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, UserVo.UserGeneralDetailVO.UpdatedDateTime);
                dbServer.AddInParameter(command, "UpdateBy", DbType.Int64, UserVo.UserGeneralDetailVO.UpdatedBy);
                dbServer.AddInParameter(command, "UpdateOn", DbType.String, UserVo.UserGeneralDetailVO.UpdatedOn);
                dbServer.AddInParameter(command, "UpdateWindowsUserName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objBizActionVO.PurchaseOrder.Remarks);
                dbServer.AddInParameter(command, "UpdatedUnitID", DbType.Int64, UserVo.UserGeneralDetailVO.UnitId);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);


                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrder = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;
        }
        public override IValueObject GetPurchaseOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo)
        {

            DbConnection con = dbServer.CreateConnection();
            clsGetPurchaseOrderForCloseBizActionVO objBizActionVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                objBizActionVO = valueObject as clsGetPurchaseOrderForCloseBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForCloseManually");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizActionVO.SearchStoreID);
                dbServer.AddInParameter(command, "PONO", DbType.String, objBizActionVO.PONO);
                if (objBizActionVO.searchFromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objBizActionVO.searchFromDate);
                if (objBizActionVO.searchToDate != null)
                {
                    objBizActionVO.searchToDate = objBizActionVO.searchToDate.Value.Date.AddDays(1);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objBizActionVO.searchToDate);
                }

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizActionVO.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
                bool potype;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsPurchaseOrderVO>();
                    while (reader.Read())
                    {
                        clsPurchaseOrderVO obj = new clsPurchaseOrderVO();
                        obj.PONO = Convert.ToString(reader["PONO"]);
                        obj.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                        obj.ID = Convert.ToInt64(reader["ID"]);
                        obj.UnitId = Convert.ToInt64(reader["UnitID"]);
                        obj.StoreName = Convert.ToString(reader["StoreName"]);
                        obj.SupplierName = Convert.ToString(reader["SupplierName"]);
                        obj.StoreID = Convert.ToInt64(reader["StoreID"]);
                        obj.SupplierID = Convert.ToInt64(reader["SupplierID"]);
                        obj.TotalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                        obj.TotalDiscount = Convert.ToDecimal(reader["TotalDiscount"]);
                        obj.TotalVAT = Convert.ToDecimal(reader["TotalVat"]);
                        obj.TotalNet = Convert.ToDecimal(reader["TotalNetAmount"]);
                        potype = Convert.ToBoolean(reader["POType"]);
                        obj.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                        if (potype == false)
                            obj.Type = "Mannual";
                        else if (potype == true)
                            obj.Type = "Auto";

                        obj.GRNNowithDate = Convert.ToString(reader["GRN No - Date"]);

                        obj.IndentNowithDate = Convert.ToString(reader["Indent No - Date"]);

                        objBizActionVO.PurchaseOrderList.Add(obj);
                    }
                }
                reader.NextResult();
                objBizActionVO.TotalRowCount = (int)dbServer.GetParameterValue(command, "TotalRows");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null && reader.IsClosed == false)
                {
                    reader.Close();
                }
            }
            return objBizActionVO;
        }


        public override IValueObject UpdateForPOCloseDuration(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsUpdateRemarkForCancellationPO objBizActionVO = null;
            clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsUpdateRemarkForCancellationPO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;

                command = dbServer.GetStoredProcCommand("CIMS_ChangePOCloseDuration"); //CIMS_ChangePOCloseDuration
                command.Connection = con;

                dbServer.AddInParameter(command, "POAutoCloseDuration", DbType.Int32, objBizActionVO.PurchaseOrder.POAutoCloseDuration);
                dbServer.AddInParameter(command, "POAutoCloseDate", DbType.DateTime, objBizActionVO.PurchaseOrder.POAutoCloseDurationDate);
                dbServer.AddInParameter(command, "POAutoCloseReason", DbType.String, objBizActionVO.PurchaseOrder.POAutoCloseReason);
                dbServer.AddInParameter(command, "PONO", DbType.String, objBizActionVO.PurchaseOrder.PONO);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.PurchaseOrder.UnitId);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, DateTime.Now);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrder = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;
        }



        private void SetLogInfo(List<LogInfo> objLogList, long userID)   // BY Umesh
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
    }
}
