using System;
using System.Collections.Generic;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using com.seedhealthcare.hms.Web.Logging;
using Microsoft.Practices.EnterpriseLibrary.Data;
using PalashDynamics.ValueObjects;
using System.Data.Common;
using System.Data;
using System.Reflection;
using PalashDynamics.ValueObjects.Inventory.WorkOrder;
using System.Windows;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    class clsWorkOrderDAL : clsBaseWorkOrderDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsWorkOrderDAL()
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

        public override IValueObject FreezWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
             DbConnection con = dbServer.CreateConnection();
            clsFreezWorkOrderBizActionVO objBizActionVO = null;
            clsWorkOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                //con.Open();
                //trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_FreezWorkOrder");
                objBizActionVO = valueObject as clsFreezWorkOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                dbServer.AddInParameter(command, "ID", DbType.Int64, objPurchaseOrderVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                dbServer.AddInParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus1 = dbServer.ExecuteNonQuery(command);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrderList = null;
                con.Close();
            }
            finally
            {
                con.Close();




            }

            return objBizActionVO;
        }
        // By Anumani 
        public override IValueObject GetWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetWorkOrderBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();


                objBizActionVO = valueObject as clsGetWorkOrderBizActionVO;
               // objBizActionVO.PurchaseOrder = valueObject;
                if (objBizActionVO.flagWOFromGRN == true)
                {
                    valueObject = GetWorkOrderGromGRN(valueObject, UserVo);
                    return valueObject;
                }

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWorkOrder");
            dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
            dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
            dbServer.AddInParameter(command, "CancelWO", DbType.Boolean, objBizActionVO.CancelWO);
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
            dbServer.AddInParameter(command, "WONO", DbType.String, objBizActionVO.WONO);
            dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objBizActionVO.PagingEnabled);
            dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, objBizActionVO.StartRowIndex);
            dbServer.AddInParameter(command, "maximumRows", DbType.Int32, objBizActionVO.MaximumRows);
            dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0);
            bool poStatus;
            bool potype;

            reader = (DbDataReader)dbServer.ExecuteReader(command);
            if (reader.HasRows)
            {
                if (objBizActionVO.WorkOrderList == null)
                    objBizActionVO.WorkOrderList = new List<clsWorkOrderVO>();
                while (reader.Read())
                {
                    clsWorkOrderVO obj = new clsWorkOrderVO();
                    poStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    //if (poStatus)
                    //{
                    obj.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                    obj.WONO = (string)DALHelper.HandleDBNull(reader["WONO"]);
                    obj.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                    obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                    obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                    obj.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                    obj.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                    obj.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                    obj.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                    // obj.EnquiryID = (long)DALHelper.HandleDBNull(reader["EnquiryID"]);
                    obj.DeliveryDuration = Convert.ToInt64(DALHelper.HandleDBNull(reader["DeliveryDuration"]));// (long)DALHelper.HandleDBNull(reader["DeliveryDuration"]);
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
                    obj.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                   
                    //obj.IndentNumber = (string)DALHelper.HandleDBNull(reader["IndentNumber"]);
                   // obj.DeliveryDays = (Int64)DALHelper.HandleIntegerNull(reader["DeliveryDays"]);
                  //  potype = (bool)DALHelper.HandleDBNull(reader["POType"]);
                 //   
                    obj.IsApproveded = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsApproved"]));
                    obj.ApprovedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedBy"]));
                    //if (potype == false)
                    //    obj.Type = "Mannual";
                    //else if (potype == true)
                    //    obj.Type = "Auto";

                    #region Added by MMBABU

                 //   obj.GRNNowithDate = (string)DALHelper.HandleDBNull(reader["GRN No - Date"]);

                    //if (obj.IndentNumber != "")
                    //    obj.IndentNowithDate = (string)DALHelper.HandleDBNull(reader["Indent No - Date"]);

                    #endregion
                 //   obj.Direct = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["Direct"]));

                    objBizActionVO.WorkOrderList.Add(obj);
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
            //finally
            //{
            //    if (reader != null)
            //    {
            //        if (reader.IsClosed == false)
            //        {
            //            reader.Close();

            //        }
            //    }

            //}

            return objBizActionVO;
        }

         
        public override IValueObject GetWorkOrderDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetWorkOrderDetailsBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsGetWorkOrderDetailsBizActionVO;
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetWorkOrderDetails_1");
                dbServer.AddInParameter(command, "ID", DbType.Int64, objBizActionVO.SearchID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
                dbServer.AddInParameter(command, "FilterPendingQuantity", DbType.Boolean, objBizActionVO.FilterPendingQuantity);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.PurchaseOrderList == null)
                        objBizActionVO.PurchaseOrderList = new List<clsWorkOrderDetailVO>();
                    while (reader.Read())
                    {
                        clsWorkOrderDetailVO obj = new clsWorkOrderDetailVO();
                        obj.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                        obj.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                     
                        obj.Quantity = (decimal)DALHelper.HandleDBNull(reader["Quantity"]);
                        obj.Rate = (decimal)DALHelper.HandleDBNull(reader["Rate"]);
                        obj.Amount = (decimal)DALHelper.HandleDBNull(reader["Amount"]);
                        obj.DiscountPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.DiscountAmount = (decimal)DALHelper.HandleDBNull(reader["DiscountAmount"]);
                        obj.VATPercent = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                        obj.VATAmount = (decimal)DALHelper.HandleDBNull(reader["VATAmount"]);
                        obj.NetAmount = (decimal)DALHelper.HandleDBNull(reader["NetAmount"]);
                        obj.Specification = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        obj.DiscPercent = (decimal)DALHelper.HandleDBNull(reader["DiscountPercent"]);
                        obj.VATPer = (decimal)DALHelper.HandleDBNull(reader["VATPercent"]);
                      
                        obj.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        obj.PendingQuantity = (decimal)DALHelper.HandleDBNull(reader["PendingQuantity"]);
                        obj.WoItemsID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        
                       
                        obj.WONO = Convert.ToString(DALHelper.HandleDBNull(reader["WONO"]));
                        obj.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                        obj.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        obj.WODate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["PODate"]));
                      
                       
                     

                        //obj.RateContractCondition = Convert.ToString(DALHelper.HandleDBNull(reader["RCCondition"]));
                        //obj.RateContractID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractID"]));
                        //obj.RateContractUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RateContractUnitID"]));
                   
                        objBizActionVO.PurchaseOrderList.Add(obj);
                    }




                }
                reader.NextResult();
                if (objBizActionVO.PoIndentList == null)
                    objBizActionVO.PoIndentList = new List<clsWorkOrderDetailVO>();

                while (reader.Read())
                {
                    clsWorkOrderDetailVO obj = new clsWorkOrderDetailVO();
                    obj.WOID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOID"]));
                    obj.WOUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["WOUnitID"]));
                  //  obj.IndentID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentID"]));
                    obj.IndentUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentUnitID"]));
                    //obj.IndentDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailID"]));
                //    obj.IndentDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["IndentDetailUnitID"]));
                    obj.Quantity = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Quantity"]));
                    obj.ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"]));
                    objBizActionVO.PoIndentList.Add(obj);
                }

                reader.NextResult();
                if (objBizActionVO.POTerms == null)
                    objBizActionVO.POTerms = new List<clsWorkOrderTerms>();
                while (reader.Read())
                {
                    clsWorkOrderTerms obj = new clsWorkOrderTerms();
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
        public override IValueObject UpdateWorkOrderForApproval(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsUpdateWorkOrderForApproval objBizActionVO = null;
            clsWorkOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsUpdateWorkOrderForApproval;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                foreach (var item in objBizActionVO.PurchaseOrder.ids)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateWorkOrderApprovalStatus");
                    command.Connection = con;

                    dbServer.AddInParameter(command, "ApprovedBy", DbType.String, objBizActionVO.PurchaseOrder.ApprovedBy);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, item);
                    dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.PurchaseOrder.UnitId);
                    int intStatus1 = dbServer.ExecuteNonQuery(command, trans);






                }

                foreach (var item in objPurchaseOrderVO.Items)
                {
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateIndetFromPO");
                    command2.Connection = con;
                    //dbServer.AddInParameter(command2, "IndentID", DbType.Int64, item.IndentID);
                    //dbServer.AddInParameter(command2, "IndentUnitID", DbType.Int64, item.IndentUnitID);
                    dbServer.AddInParameter(command2, "POID", DbType.Int64, item.WOID);
                    dbServer.AddInParameter(command2, "POUnitID", DbType.Decimal, item.WOUnitID);
                    dbServer.AddInParameter(command2, "BalanceQty", DbType.Decimal, item.Quantity);
                    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                    int intStatus4 = dbServer.ExecuteNonQuery(command2, trans);
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

        public override IValueObject UpdateRemarkForCancellationWO(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsUpdateRemarkForCancellationWO objBizActionVO = null;
            clsWorkOrderVO objWorkOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsUpdateRemarkForCancellationWO;
                objWorkOrderVO = objBizActionVO.WorkOrder;

                command = dbServer.GetStoredProcCommand("CIMS_UpdateRemarkForCancellationWO");
                command.Connection = con;

                dbServer.AddInParameter(command, "CancellationRemark", DbType.String, objBizActionVO.WorkOrder.CancellationRemark);
                dbServer.AddInParameter(command, "POID", DbType.Int64, objBizActionVO.WorkOrder.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.WorkOrder.UnitId);
                int intStatus1 = dbServer.ExecuteNonQuery(command, trans);
                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.WorkOrder = null;
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;
        }

        public IValueObject GetWorkOrderGromGRN(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsGetWorkOrderBizActionVO objBizActionVO = null;
            //clsPurchaseOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();


                objBizActionVO = valueObject as clsGetWorkOrderBizActionVO;
                //objBizActionVO.PurchaseOrder = valueObject;


                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderFromGRN");

                dbServer.AddInParameter(command, "FromGRN", DbType.Boolean, objBizActionVO.flagWOFromGRN);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objBizActionVO.UnitID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizActionVO.SearchStoreID);
                dbServer.AddInParameter(command, "WONO", DbType.String, objBizActionVO.WONO);
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
                    if (objBizActionVO.WorkOrderList == null)
                        objBizActionVO.WorkOrderList = new List<clsWorkOrderVO>();
                    while (reader.Read())
                    {
                        clsWorkOrderVO obj = new clsWorkOrderVO();
                        poStatus = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        if (poStatus)
                        {
                            obj.WONO = (string)DALHelper.HandleDBNull(reader["WONO"]);
                            obj.Date = (DateTime)DALHelper.HandleDBNull(reader["Date"]);
                            obj.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                            obj.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                            obj.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                            obj.SupplierName = (string)DALHelper.HandleDBNull(reader["SupplierName"]);
                            obj.StoreID = (long)DALHelper.HandleDBNull(reader["StoreID"]);
                            obj.SupplierID = (long)DALHelper.HandleDBNull(reader["SupplierID"]);
                            //obj.IndentID = (long)DALHelper.HandleDBNull(reader["IndentID"]);
                            //obj.EnquiryID = (long)DALHelper.HandleDBNull(reader["EnquiryID"]);
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
                           // obj.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));


                            objBizActionVO.WorkOrderList.Add(obj);
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

        public override IValueObject GetWorkOrderToCloseManually(IValueObject valueObject, clsUserVO UserVo)
        {

            DbConnection con = dbServer.CreateConnection();
            clsGetWorkOrderForCloseBizActionVO objBizActionVO = null;
            DbTransaction trans = null;
            DbDataReader reader = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                objBizActionVO = valueObject as clsGetWorkOrderForCloseBizActionVO;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetPurchaseOrderForCloseManually");
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objBizActionVO.SearchSupplierID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objBizActionVO.SearchStoreID);
                dbServer.AddInParameter(command, "WONO", DbType.String, objBizActionVO.WONO);
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
                bool wotype;
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (objBizActionVO.WorkOrderList == null)
                        objBizActionVO.WorkOrderList = new List<clsWorkOrderVO>();
                    while (reader.Read())
                    {
                        clsWorkOrderVO obj = new clsWorkOrderVO();
                        obj.WONO = Convert.ToString(reader["WONO"]);
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
                        wotype = Convert.ToBoolean(reader["WOType"]);
                        obj.DeliveryLocation = Convert.ToString(DALHelper.HandleDBNull(reader["DeliveryLocation"]));
                        if (wotype == false)
                            obj.Type = "Mannual";
                        else if (wotype == true)
                            obj.Type = "Auto";

                        obj.GRNNowithDate = Convert.ToString(reader["GRN No - Date"]);

                        //obj.IndentNowithDate = Convert.ToString(reader["Indent No - Date"]);

                        objBizActionVO.WorkOrderList.Add(obj);
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

        public override IValueObject AddWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsAddWorkOrderBizActionVO objBizActionVO = null;
            clsWorkOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();

                objBizActionVO = valueObject as clsAddWorkOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;

                if (objBizActionVO.IsEditMode == true)
                {
                    command = dbServer.GetStoredProcCommand("CIMS_UpdateWorkOrder");
                    command.Connection = con;
                    dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, objPurchaseOrderVO.UpdatedBy);
                    if (objPurchaseOrderVO.UpdatedOn != null) objPurchaseOrderVO.UpdatedOn = objPurchaseOrderVO.UpdatedOn.Trim();
                    dbServer.AddInParameter(command, "UpdatedOn", DbType.String, objPurchaseOrderVO.UpdatedOn);
                    dbServer.AddInParameter(command, "UpdatedDateTime", DbType.DateTime, objPurchaseOrderVO.UpdatedDateTime);
                    if (objPurchaseOrderVO.UpdatedWindowsLoginName != null) objPurchaseOrderVO.UpdatedWindowsLoginName = objPurchaseOrderVO.UpdatedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, objPurchaseOrderVO.UpdatedWindowsLoginName);
                    dbServer.AddInParameter(command, "ID", DbType.Int64, objPurchaseOrderVO.ID);
                    dbServer.AddInParameter(command, "UpdatedUnitId", DbType.Int64, objPurchaseOrderVO.UpdatedUnitId);
                }
                else
                {
                    command = dbServer.GetStoredProcCommand("CIMS_AddWorkOrder");
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
                    dbServer.AddInParameter(command, "WONO", DbType.String, objPurchaseOrderVO.WONO);
                }

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objPurchaseOrderVO.LinkServer);
                if (objPurchaseOrderVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPurchaseOrderVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objPurchaseOrderVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objPurchaseOrderVO.Time);

                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objPurchaseOrderVO.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objPurchaseOrderVO.SupplierID);               
                dbServer.AddInParameter(command, "DeliveryDays", DbType.Int64, objPurchaseOrderVO.DeliveryDays);
                dbServer.AddInParameter(command, "PaymentMode", DbType.Int16, (Int16)objPurchaseOrderVO.PaymentModeID);
                dbServer.AddInParameter(command, "PaymentTerm", DbType.Int64, objPurchaseOrderVO.PaymentTerms);
                dbServer.AddInParameter(command, "Guarantee_Warrantee", DbType.String, objPurchaseOrderVO.Guarantee_Warrantee);
                dbServer.AddInParameter(command, "Schedule", DbType.Int64, objPurchaseOrderVO.Schedule);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objPurchaseOrderVO.Remarks);
                dbServer.AddInParameter(command, "TotalDiscount", DbType.Decimal, objPurchaseOrderVO.TotalDiscount);
                dbServer.AddInParameter(command, "TotalVAT", DbType.Decimal, objPurchaseOrderVO.TotalVAT);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Decimal, objPurchaseOrderVO.TotalAmount);
                dbServer.AddInParameter(command, "TotalNetAmount", DbType.Decimal, objPurchaseOrderVO.TotalNet);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, objPurchaseOrderVO.UnitId);
                //dbServer.AddInParameter(command, "IndentID", DbType.Int64, objPurchaseOrderVO.IndentID);
                //dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, objPurchaseOrderVO.IndentUnitID);
                //dbServer.AddInParameter(command, "EnquiryID", DbType.Int64, objPurchaseOrderVO.EnquiryID);
               // dbServer.AddInParameter(command, "DeliveryDuration", DbType.String, objPurchaseOrderVO.DeliveryDuration);
                //dbServer.AddInParameter(command, "Direct", DbType.Boolean, objPurchaseOrderVO.Direct);

              
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddParameter(command, "WorkOrderNumber", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "000000000000000000000000000000000000000000000000000");
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                if (objBizActionVO.IsEditMode == false)
                {
                    objBizActionVO.PurchaseOrder.ID = (long)dbServer.GetParameterValue(command, "ID");
                }

                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                objBizActionVO.PurchaseOrder.WONO = (string)dbServer.GetParameterValue(command, "WorkOrderNumber");
                if (objBizActionVO.IsEditMode == true)
                {
                    clsBaseWorkOrderDAL obj = clsBaseWorkOrderDAL.GetInstance();
                    IValueObject objtest = obj.DeleteWorkOrderItems(objBizActionVO, UserVo);
                }


                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddWorkOrderDetails");
                command1.Connection = con;
                foreach (var item in objBizActionVO.PurchaseOrder.Items)
                {
                    command1.Parameters.Clear();
                    item.WOID = objBizActionVO.PurchaseOrder.ID;

                    dbServer.AddInParameter(command, "LinkServer", DbType.String, objPurchaseOrderVO.LinkServer);
                    if (objPurchaseOrderVO.LinkServer != null)
                        dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objPurchaseOrderVO.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, objPurchaseOrderVO.Date);
                    dbServer.AddInParameter(command1, "Time", DbType.DateTime, objPurchaseOrderVO.Time);
                    dbServer.AddInParameter(command1, "WOID", DbType.Int64, item.WOID);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Decimal, item.Quantity * Convert.ToDecimal(item.ConversionFactor));
                    dbServer.AddInParameter(command1, "Rate", DbType.Decimal, item.Rate / Convert.ToDecimal(item.ConversionFactor));
                    dbServer.AddInParameter(command1, "Amount", DbType.Decimal, item.Amount);
                    dbServer.AddInParameter(command1, "DiscountPercent", DbType.Decimal, item.DiscountPercent);
                    dbServer.AddInParameter(command1, "DiscountAmount", DbType.Decimal, item.DiscountAmount);
                    dbServer.AddInParameter(command1, "VatPercent", DbType.Decimal, item.VATPercent);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Decimal, item.VATAmount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Decimal, item.NetAmount);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Specification);
                    dbServer.AddInParameter(command1, "MRP", DbType.Decimal, item.MRP / Convert.ToDecimal(item.ConversionFactor));
                    //if (item.ConditionFound)
                    //{
                    //    //dbServer.AddInParameter(command1, "RateContractID", DbType.Int64, item.RateContractID);
                    //    //dbServer.AddInParameter(command1, "RateContractUnitID", DbType.Int64, item.RateContractID);
                    //    //dbServer.AddInParameter(command1, "RateContractCondition", DbType.String, item.RateContractCondition);
                    //}
                    //else
                    //{
                    //    dbServer.AddInParameter(command1, "RateContractID", DbType.Int64, 0);
                    //    dbServer.AddInParameter(command1, "RateContractUnitID", DbType.Int64, 0);
                    //    dbServer.AddInParameter(command1, "RateContractCondition", DbType.String, string.Empty);
                    //}
                    if (item.SelectedCurrency != null)
                    {
                        dbServer.AddInParameter(command1, "CurrencyID", DbType.Int64, item.SelectedCurrency.ID);
                    }
                    else
                    {
                        dbServer.AddInParameter(command1, "CurrencyID", DbType.Int64, 0);

                    }
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                  //  long PoDetailsID = (long)dbServer.GetParameterValue(command1, "ID");
                   
                    #region POIndent Details Commented
                    //if (objBizActionVO.POIndentList != null && objBizActionVO.POIndentList.Count > 0)
                    //{

                    //    foreach (var item2 in objBizActionVO.POIndentList)
                    //    {
                    //        if (item2.ItemID == item.ItemID)
                    //        {
                    //            item.WOID = objBizActionVO.PurchaseOrder.ID;
                    //            DbCommand command4 = dbServer.GetStoredProcCommand("CIMS_AddPOIndentdetails");
                    //            command4.Connection = con;
                    //            command4.Parameters.Clear();
                    //            item.WOID = objBizActionVO.PurchaseOrder.ID;
                    //            dbServer.AddInParameter(command4, "WOID", DbType.Int64, item.WOID);
                    //            dbServer.AddInParameter(command4, "PoUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                    //            dbServer.AddInParameter(command4, "PODetailsID", DbType.Int64, PoDetailsID);
                    //            dbServer.AddInParameter(command4, "PODetailsUnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                    //            dbServer.AddInParameter(command4, "IndentID", DbType.Int64, item2.IndentID);
                    //            dbServer.AddInParameter(command4, "IndentUnitId", DbType.Int64, item2.IndentUnitID);
                    //            dbServer.AddInParameter(command4, "IndentDetailID", DbType.Int64, item2.IndentDetailID);
                    //            dbServer.AddInParameter(command4, "IndentDetailUnitID", DbType.Int64, item2.IndentDetailUnitID);
                    //            dbServer.AddInParameter(command4, "ItemID", DbType.Int64, item2.ItemID);
                    //            dbServer.AddInParameter(command4, "Quantity", DbType.Decimal, 0);
                    //            dbServer.AddInParameter(command4, "PendingQuantity", DbType.Decimal, 0);
                    //            dbServer.AddInParameter(command4, "UnitId", DbType.Decimal, objPurchaseOrderVO.UnitId);
                    //            dbServer.AddOutParameter(command4, "ID", DbType.Int64, 0);
                    //            int intStatus4 = dbServer.ExecuteNonQuery(command4, trans);
                    //        }

                    //    }

                    //}
                    #endregion
                }
                if (objBizActionVO.POTerms != null)
                {
                    foreach (clsWorkOrderTerms Terms in objBizActionVO.POTerms)
                    {
                        DbCommand command5 = dbServer.GetStoredProcCommand("CIMS_AddWOTermsAndCondition");
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

                trans.Commit();

            }
            catch (Exception ex)
            {
                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrder = null;
                // Added by rohit
                MessageBox.Show("Error occurred :" + ex.Message);
            }
            finally
            {
                con.Close();
                trans = null;
                con = null;
            }
            return objBizActionVO;
        }

        public override IValueObject DeleteWorkOrderItems(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            clsAddWorkOrderBizActionVO objBizActionVO = null;
            clsWorkOrderVO objPurchaseOrderVO = null;
            DbTransaction trans = null;
            DbCommand command;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                command = dbServer.GetStoredProcCommand("CIMS_DeleteWorkOrderDetails");
                objBizActionVO = valueObject as clsAddWorkOrderBizActionVO;
                objPurchaseOrderVO = objBizActionVO.PurchaseOrder;
                dbServer.AddInParameter(command, "WOID", DbType.Int64, objPurchaseOrderVO.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objPurchaseOrderVO.UnitId);
                dbServer.AddInParameter(command, "ResultStatus", DbType.Int64, 0);
                int intStatus1 = dbServer.ExecuteNonQuery(command);
                objBizActionVO.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");

            }
            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                objBizActionVO.SuccessStatus = -1;
                objBizActionVO.PurchaseOrderList = null;
            }

            return objBizActionVO;
        }

        public override IValueObject CancelWorkOrder(IValueObject valueObject, clsUserVO UserVo)
        {
            try
            {
                clsCancelWorkOrderBizActionVO bizActionVO = null;
                DbCommand command = null;

                command = dbServer.GetStoredProcCommand("CIMS_CancelWO");
                bizActionVO = valueObject as clsCancelWorkOrderBizActionVO;
                dbServer.AddInParameter(command, "POID", DbType.Int64, bizActionVO.WorkOrder.ID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.WorkOrder.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, false);
                int intStatus1 = dbServer.ExecuteNonQuery(command);

                return bizActionVO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
