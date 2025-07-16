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
    public class clsGRNReturnDAL : clsBaseGRNReturnDAL
    {
        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsGRNReturnDAL()
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

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNReturnBizActionVO BizActionObj = valueObject as clsAddGRNReturnBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;

        }

        private clsAddGRNReturnBizActionVO AddDetails(clsAddGRNReturnBizActionVO BizActionObj, clsUserVO UserVo)
        {
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();


                clsGRNReturnVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddGRNReturn");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
                // dbServer.AddInParameter(command, "GRNReturnNO", DbType.String, objDetailsVO.GRNReturnNO);
                dbServer.AddParameter(command, "GRNReturnNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "GRNID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "GrnUnitID", DbType.Int64, 0);
                dbServer.AddInParameter(command, "GRNReturnType", DbType.Int16, objDetailsVO.GoodReturnType);
                dbServer.AddInParameter(command, "PaymentModeID", DbType.Int16, (Int16)objDetailsVO.PaymentModeID);
                dbServer.AddInParameter(command, "TotalVAT", DbType.Double, objDetailsVO.TotalVAT);
                dbServer.AddInParameter(command, "TotalItemTax", DbType.Double, objDetailsVO.TotalItemTax);

                dbServer.AddInParameter(command, "TotalSGSTAmount", DbType.Double, objDetailsVO.TotalSGSTAmount);
                dbServer.AddInParameter(command, "TotalCGSTAmount", DbType.Double, objDetailsVO.TotalCGSTAmount);
                dbServer.AddInParameter(command, "TotalIGSTAmount", DbType.Double, objDetailsVO.TotalIGSTAmount);

                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, objDetailsVO.SupplierID);
                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = Convert.ToInt64(dbServer.GetParameterValue(command, "ID"));
                BizActionObj.Details.GRNReturnNO = Convert.ToString(dbServer.GetParameterValue(command, "GRNReturnNO"));
                BizActionObj.Details.UnitId = UserVo.UserLoginInfo.UnitId;

                foreach (var item in objDetailsVO.Items)
                {
                    item.GRNReturnID = BizActionObj.Details.ID;
                    item.StockDetails.PurchaseRate = item.Rate;
                    item.GRNReturnUnitID = BizActionObj.Details.UnitId;

                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddGRNReturnItems");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command2, "GRNReturnID", DbType.Int64, item.GRNReturnID);
                    dbServer.AddInParameter(command2, "GRNReturnUnitID", DbType.Int64, item.GRNReturnUnitID);

                    dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command2, "BatchID", DbType.Int64, item.BatchID);

                    dbServer.AddInParameter(command2, "AvailableQuantity", DbType.Double, item.AvailableQuantity);
                    dbServer.AddInParameter(command2, "ReceivedQuantity", DbType.Double, item.ReceivedQuantity);
                    dbServer.AddInParameter(command2, "ReturnedQuantity", DbType.Double, item.ReturnedQuantity * item.BaseConversionFactor);

                    dbServer.AddInParameter(command2, "Rate", DbType.Double, (item.Rate));
                    dbServer.AddInParameter(command2, "MRP", DbType.Double, item.MRP);

                    dbServer.AddInParameter(command2, "Amount", DbType.Double, item.Amount);
                    dbServer.AddInParameter(command2, "VATPercent", DbType.Double, item.VATPercent);
                    dbServer.AddInParameter(command2, "VATAmount", DbType.Double, item.VATAmount);

                    dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
                    dbServer.AddInParameter(command2, "Remarks", DbType.String, item.Remarks);
                    dbServer.AddInParameter(command2, "ReturnReason", DbType.String, item.ReturnReason);

                    //New
                    dbServer.AddInParameter(command2, "GRNID", DbType.Int64, item.GRNID);
                    dbServer.AddInParameter(command2, "GRNUnitID", DbType.Int64, item.GRNUnitID);
                    dbServer.AddInParameter(command2, "GRNDetailID", DbType.Int64, item.GRNDetailID);
                    dbServer.AddInParameter(command2, "GRNDetailUnitID", DbType.Int64, item.GRNDetailUnitID);
                    dbServer.AddInParameter(command2, "StoreID", DbType.Int64, item.StoreID);
                    dbServer.AddInParameter(command2, "InputTransactionQuantity", DbType.Double, item.ReturnedQuantity);
                    dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, item.TransactionUOMID);
                    dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, item.BaseUOMID);
                    dbServer.AddInParameter(command2, "BaseCF", DbType.Double, item.BaseConversionFactor);
                    dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, item.SUOMID);
                    dbServer.AddInParameter(command2, "StockCF", DbType.Double, item.ConversionFactor);
                    dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, item.ReturnedQuantity * item.ConversionFactor);
                    dbServer.AddInParameter(command2, "CDiscountPercent", DbType.Double, item.CDiscountPercent);
                    dbServer.AddInParameter(command2, "CDiscountAmount", DbType.Double, item.CDiscountAmount);
                    dbServer.AddInParameter(command2, "SchDiscountPercent", DbType.Double, item.SchDiscountPercent);
                    dbServer.AddInParameter(command2, "SchDiscountAmount", DbType.Double, item.SchDiscountAmount);
                    dbServer.AddInParameter(command2, "ItemTax", DbType.Double, item.ItemTax);
                    dbServer.AddInParameter(command2, "ItemTaxAmount", DbType.Double, item.TaxAmount);
                    dbServer.AddInParameter(command2, "Vattype", DbType.Int32, item.GRNItemVatType);
                    dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int32, item.GRNItemVatApplicationOn);
                    dbServer.AddInParameter(command2, "otherTaxType", DbType.Int32, item.OtherGRNItemTaxType);
                    dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int32, item.OtherGRNItemTaxApplicationOn);
                    dbServer.AddInParameter(command2, "ReceivedID", DbType.Int64, item.ReceivedID);
                    dbServer.AddInParameter(command2, "ReceivedUnitID", DbType.Int64, item.ReceivedUnitID);
                    dbServer.AddInParameter(command2, "IsFreeItem", DbType.Boolean, item.IsFreeItem);
                    //End

                    dbServer.AddInParameter(command2, "SGSTTaxType", DbType.Int32, item.GRNSGSTVatType);
                    dbServer.AddInParameter(command2, "SGSTApplicableOn", DbType.Int32, item.GRNSGSTVatApplicationOn);
                    dbServer.AddInParameter(command2, "CGSTTaxType", DbType.Int32, item.GRNCGSTVatType);
                    dbServer.AddInParameter(command2, "CGSTApplicableOn", DbType.Int32, item.GRNCGSTVatApplicationOn);
                    dbServer.AddInParameter(command2, "IGSTTaxType", DbType.Int32, item.GRNIGSTVatType);
                    dbServer.AddInParameter(command2, "IGSTApplicableOn", DbType.Int32, item.GRNIGSTVatApplicationOn);

                    dbServer.AddInParameter(command2, "SGSTPercent", DbType.Double, item.SGSTPercent);
                    dbServer.AddInParameter(command2, "SGSTAmount", DbType.Double, item.SGSTAmount);
                    dbServer.AddInParameter(command2, "CGSTPercent", DbType.Double, item.CGSTPercent);
                    dbServer.AddInParameter(command2, "CGSTAmount", DbType.Double, item.CGSTAmount);
                    dbServer.AddInParameter(command2, "IGSTPercent", DbType.Double, item.IGSTPercent);
                    dbServer.AddInParameter(command2, "IGSTAmount", DbType.Double, item.IGSTAmount);



                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, -1);
                    dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);

                    int intStatus2 = dbServer.ExecuteNonQuery(command2, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command2, "ID");


                    if (item.ReceivedDetailID != null && item.ReceivedDetailID > 0)
                    {
                        DbCommand command3 = dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command2.Connection = con;

                        dbServer.AddInParameter(command3, "UnitID", DbType.Int64, item.ReceivedUnitID);
                        //dbServer.AddInParameter(command3, "BalanceQty", DbType.Int64, (item.IssueQty - item.ReceivedQty));
                        dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, item.ReturnedQuantity * item.BaseConversionFactor);//(item.ReturnQty));
                        dbServer.AddInParameter(command3, "ReceiveItemDetailsID", DbType.Int64, item.ReceivedDetailID);
                        int status2 = dbServer.ExecuteNonQuery(command3, trans);
                    }

                    item.StockDetails.BatchID = item.BatchID;
                    item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    item.StockDetails.ItemID = item.ItemID;
                    item.StockDetails.TransactionTypeID = InventoryTransactionType.GRNReturn;
                    item.StockDetails.TransactionID = item.GRNReturnID;
                    item.StockDetails.TransactionQuantity = (item.ReturnedQuantity * item.BaseConversionFactor);
                    item.StockDetails.Date = objDetailsVO.Date;
                    item.StockDetails.Time = objDetailsVO.Time;
                    item.StockDetails.StoreID = item.StoreID;

                    item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReturnedQuantity);
                    item.StockDetails.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                    item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                    item.StockDetails.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                    item.StockDetails.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                    item.StockDetails.StockingQuantity = item.ReturnedQuantity * item.ConversionFactor;  // StockingQuantity // For Conversion Factor
                    item.StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor 
                    item.StockDetails.ExpiryDate = item.ExpiryDate;

                    if (objDetailsVO.IsApproved == true)
                    {
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

        public override IValueObject GRNReturnApprove(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNReturnBizActionVO BizActionObj = valueObject as clsAddGRNReturnBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;
            try
            {
                con.Open();
                trans = con.BeginTransaction();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateGRNReturnForApprove");
                command.Connection = con;
                dbServer.AddInParameter(command, "GRNReturnID", DbType.String, BizActionObj.Details.ID);
                dbServer.AddInParameter(command, "GRNReturnUnitID", DbType.String, BizActionObj.Details.UnitId);
                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                
                if (Convert.ToInt32(dbServer.GetParameterValue(command, "ResultStatus")) == -1)
                {
                    throw new Exception();
                }

                List<clsGRNReturnDetailsVO> objDetailsList = BizActionObj.GRNDetailsList;
                if (objDetailsList.Count > 0)
                {
                    foreach (var item in objDetailsList.ToList())
                    {
                        item.StockDetails.PurchaseRate = item.Rate;
                        item.StockDetails.BatchID = item.BatchID;
                        item.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        item.StockDetails.ItemID = item.ItemID;
                        item.StockDetails.TransactionTypeID = InventoryTransactionType.GRNReturn;
                        item.StockDetails.TransactionID = item.GRNReturnID;
                        item.StockDetails.TransactionQuantity = (item.ReturnedQuantity * item.BaseConversionFactor);
                        item.StockDetails.Date = DateTime.Now; //objDetailsVO.Date;
                        item.StockDetails.Time = DateTime.Now;
                        item.StockDetails.StoreID = item.StoreID;
                        item.StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReturnedQuantity);
                        item.StockDetails.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                        item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                        item.StockDetails.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                        item.StockDetails.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                        item.StockDetails.StockingQuantity = item.ReturnedQuantity * item.ConversionFactor;  // StockingQuantity // For Conversion Factor
                        item.StockDetails.SelectedUOM.ID = item.TransactionUOMID;          // Transaction UOM      // For Conversion Factor 
                        item.StockDetails.ExpiryDate = item.ExpiryDate;

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
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
                trans = null;
            }
            return valueObject;

        }

        public override IValueObject GRNReturnReject(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddGRNReturnBizActionVO BizActionObj = valueObject as clsAddGRNReturnBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            try
            {
                con.Open();
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_UpdateGRNReturnForReject");
                command.Connection = con;
                dbServer.AddInParameter(command, "GRNReturnID", DbType.String, BizActionObj.Details.ID);
                dbServer.AddInParameter(command, "GRNReturnUnitID", DbType.String, BizActionObj.Details.UnitId);
                dbServer.AddInParameter(command, "RejectionRemark", DbType.String, BizActionObj.Details.RejectionRemarks);

                dbServer.AddInParameter(command, "UpdatedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                int intStatus = dbServer.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.SuccessStatus = -1;
                BizActionObj.Details = null;
            }
            finally
            {
                con.Close();
            }
            return valueObject;

        }


        public override IValueObject GetSearchList(IValueObject valueObject, clsUserVO UserVo)
        {
            //throw new NotImplementedException();

            clsGetGRNReturnListBizActionVO BizActionObj = valueObject as clsGetGRNReturnListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNReturnSearchList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "SupplierID", DbType.Int64, BizActionObj.SupplierID);
                if (BizActionObj.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                if (BizActionObj.ToDate != null)
                {
                    if (BizActionObj.FromDate != null)
                    {
                        BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                    }
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNReturnVO>();
                    while (reader.Read())
                    {
                        clsGRNReturnVO objVO = new clsGRNReturnVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.SupplierName = (string)DALHelper.HandleDBNull(reader["Supplier"]);
                        objVO.StoreName = (string)DALHelper.HandleDBNull(reader["Store"]);
                        objVO.GRNID = (long)DALHelper.HandleDBNull(reader["GRNID"]);
                        objVO.GRNNO = (string)DALHelper.HandleDBNull(reader["GRNNO"]);
                        objVO.GRNReturnNO = (string)DALHelper.HandleDBNull(reader["GRNReturnNO"]);
                        objVO.GoodReturnType = (InventoryGoodReturnType)((Int16)DALHelper.HandleDBNull(reader["GRNReturnType"]));
                        objVO.Status = (bool)DALHelper.HandleDBNull(reader["Status"]);
                        objVO.Date = (DateTime)DALHelper.HandleDate(reader["Date"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        //objVO.Remarks = (string)DALHelper.HandleDBNull(reader["Remarks"]);
                        objVO.IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]));
                        objVO.IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRejected"]));
                        objVO.ApprovedOrRejectedBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejectedBy"]));
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

        public override IValueObject GetItemDetailsList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetGRNReturnDetailsListBizActionVO BizActionObj = valueObject as clsGetGRNReturnDetailsListBizActionVO;

            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNReturnItemsList");
                DbDataReader reader;

                dbServer.AddInParameter(command, "GRNReturnID", DbType.Int64, BizActionObj.GRNReturnID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.List == null)
                        BizActionObj.List = new List<clsGRNReturnDetailsVO>();
                    while (reader.Read())
                    {
                        clsGRNReturnDetailsVO objVO = new clsGRNReturnDetailsVO();

                        objVO.ID = (long)DALHelper.HandleDBNull(reader["ID"]);
                        objVO.UnitId = (long)DALHelper.HandleDBNull(reader["UnitID"]);
                        objVO.GRNReturnID = (long)DALHelper.HandleDBNull(reader["GRNReturnID"]);
                        objVO.ItemID = (long)DALHelper.HandleDBNull(reader["ItemID"]);
                        objVO.GRNID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNID"]));
                        objVO.GRNUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNUnitID"]));
                        objVO.GRNDetailID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailID"]));
                        objVO.GRNDetailUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["GRNDetailUnitID"]));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objVO.ReturnedQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReturnedQuantity"])) / objVO.BaseConversionFactor;  // Base Quantity
                        objVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCf"]));
                        objVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        objVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objVO.TransUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReturnedUOM"]));
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.Rate = Convert.ToSingle(DALHelper.HandleDBNull(reader["Rate"]));
                        objVO.MRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.ItemTax = Convert.ToSingle(DALHelper.HandleDBNull(reader["ItemTax"]));
                        objVO.VATPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["VATPercent"]));
                        objVO.CDiscountPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["CDiscountPercent"]));
                        objVO.SchDiscountPercent = Convert.ToSingle(DALHelper.HandleDBNull(reader["SchDiscountPercent"]));
                        objVO.GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"]));
                        objVO.GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"]));
                        objVO.OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"]));
                        objVO.OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"]));
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        //objVO.NetAmount1 = (double)DALHelper.HandleDBNull(reader["NetAmount"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.ReturnReason = (string)DALHelper.HandleDBNull(reader["ReturnReason"]);
                        objVO.ReceivedQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["ReceiveddQuantity"]));
                        objVO.ReceivedQuantityUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"]));
                        objVO.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeItem"]));
                        objVO.GRNNo = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNo"]));
                        objVO.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedUOM"])); //ReceivedQty UOM from Receive Table
                        objVO.ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"]));

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
    }
}
