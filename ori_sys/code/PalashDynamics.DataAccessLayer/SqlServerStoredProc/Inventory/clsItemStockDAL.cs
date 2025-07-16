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

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{

    public class clsItemStockDAL : clsBaseItemStockDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsItemStockDAL()
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

            clsAddItemStockBizActionVO BizActionObj = valueObject as clsAddItemStockBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, null, null);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;

        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {

            clsAddItemStockBizActionVO BizActionObj = valueObject as clsAddItemStockBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetails(BizActionObj, UserVo, myConnection, myTransaction);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;

        }

        private clsAddItemStockBizActionVO AddDetails(clsAddItemStockBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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


                clsItemStockVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemStock");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objDetailsVO.ItemID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, objDetailsVO.BatchID);
                if (objDetailsVO.BatchCode != null)
                {
                    if (objDetailsVO.BatchCode.Trim().Length > 0)
                    {
                        dbServer.AddInParameter(command, "BatchCode", DbType.String, objDetailsVO.BatchCode);
                        dbServer.AddInParameter(command, "ExpiryDateNew", DbType.DateTime, objDetailsVO.ExpiryDate);
                    }
                }
                dbServer.AddInParameter(command, "TransactionTypeID", DbType.Int16, (Int16)objDetailsVO.TransactionTypeID);
                dbServer.AddInParameter(command, "TransactionID", DbType.Int64, objDetailsVO.TransactionID);
                dbServer.AddInParameter(command, "OperationType", DbType.Int16, (Int16)objDetailsVO.OperationType);
                dbServer.AddInParameter(command, "TransactionQuantity", DbType.Double, objDetailsVO.TransactionQuantity);
                dbServer.AddInParameter(command, "InputTransactionQuantity", DbType.Double, objDetailsVO.InputTransactionQuantity);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.TransactionTypeID.ToString());

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                #region For Conversion Factor

                dbServer.AddInParameter(command, "BaseUOMID", DbType.Int64, objDetailsVO.BaseUOMID);
                dbServer.AddInParameter(command, "BaseCF", DbType.Double, objDetailsVO.BaseConversionFactor);
                dbServer.AddInParameter(command, "StockUOMID", DbType.Int64, objDetailsVO.SUOMID);
                dbServer.AddInParameter(command, "StockCF", DbType.Double, objDetailsVO.ConversionFactor);
                dbServer.AddInParameter(command, "TransactionUOMID", DbType.Int64, objDetailsVO.SelectedUOM.ID);
                dbServer.AddInParameter(command, "StockingQuantity", DbType.Double, objDetailsVO.StockingQuantity);

                #endregion

                # region//For new batch for nonbatchitem from OB
                dbServer.AddInParameter(command, "PurchaseRate1", DbType.Single, objDetailsVO.PurchaseRate);
                dbServer.AddInParameter(command, "MRP1", DbType.Single, objDetailsVO.MRP);
                dbServer.AddInParameter(command, "IsFromOpeningBalance", DbType.Boolean, objDetailsVO.IsFromOpeningBalance);
                # endregion

                if (objDetailsVO.UnitId > 0)
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                else
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);




                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                // Added By Rohit
                dbServer.AddInParameter(command, "BarCode", DbType.String, objDetailsVO.BarCode);
                // End              
                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                if (BizActionObj.SuccessStatus == -2) throw new Exception();//Added By Bhushanp For Store Mandetory

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




        public override IValueObject GetItemBatchwiseStock(IValueObject valueObject, clsUserVO UserVo)
        {

            clsGetItemStockBizActionVO BizActionObj = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> expiredNullItemList = new List<clsItemStockVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearch");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command, "ShowExpiredBatches", DbType.Boolean, BizActionObj.ShowExpiredBatches);
                dbServer.AddInParameter(command, "ShowZeroStockBatches", DbType.Boolean, BizActionObj.ShowZeroStockBatches);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsFree", DbType.Boolean, BizActionObj.IsFree);  // Set False to show only Main Item Batches

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchList == null)
                        BizActionObj.BatchList = new List<clsItemStockVO>();
                    while (reader.Read())
                    {
                        clsItemStockVO objVO = new clsItemStockVO();

                        objVO.ID = Convert.ToInt64((DALExtension.HandleDBNull(reader["ID"])));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"]));
                        //#region Added By Pallavi
                        //if (objVO.AvailableStock <= 0)
                        //    continue;
                        //#endregion
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);

                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);


                        //#region Added By Pallavi
                        //if (objVO.ExpiryDate < DateTime.Now)
                        //    continue;
                        //#endregion
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        //objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]))*(objVO.ConversionFactor);
                        //objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]))*objVO.ConversionFactor;
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                        objVO.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objVO.Status = false;
                        objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objVO.IsConsignment = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsConsignment"]));
                        //By Anjali....................................
                        objVO.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        objVO.AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        //Added By Bhushanp For GST 26062017
                        objVO.SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objVO.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        objVO.CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objVO.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        objVO.IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objVO.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        //..............................................
                        // Added By CDS
                        if (BizActionObj.ShowZeroStockBatches == true)
                        {

                            objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                            objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));

                            objVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                            objVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));

                            objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }
                        objVO.StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.IsFreeItem = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsFree"]));
                        //***//-------------
                        objVO.StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                        objVO.WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"]));
                        objVO.RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"]));
                        if (BizActionObj.ShowZeroStockBatches != true)
                        {
                            objVO.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                            objVO.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                            objVO.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                        }
                        //-------------------------

                        //#region TempCommented
                        //if (objVO.ExpiryDate == null)
                        //{
                        //    clsItemStockVO objVO1 = new clsItemStockVO();
                        //    objVO1.ID = objVO.ID;
                        //    objVO1.StoreID = objVO.StoreID;
                        //    objVO1.ItemID = objVO.ItemID;
                        //    objVO1.BatchID = objVO.BatchID;
                        //    objVO1.AvailableStock = objVO.AvailableStock;

                        //    objVO1.BatchCode = objVO.BatchCode;

                        //    objVO1.ExpiryDate = objVO.ExpiryDate;



                        //    objVO1.MRP = objVO.MRP;
                        //    objVO1.PurchaseRate = objVO.PurchaseRate;
                        //    objVO1.Date = objVO.Date;
                        //    objVO1.VATAmt = objVO.VATAmt;
                        //    objVO1.VATPerc = objVO.VATPerc;
                        //    objVO1.DiscountOnSale = objVO.DiscountOnSale;
                        //    objVO1.Status = objVO.Status;
                        //    expiredNullItemList.Add(objVO1);
                        //}
                        //if (objVO.ExpiryDate == null)
                        //    continue;
                        //#endregion
                        BizActionObj.BatchList.Add(objVO);

                    }

                }
                reader.NextResult();

                //for (int i = 0; i < expiredNullItemList.Count ; i++)
                //{
                //    BizActionObj.BatchList.Add(expiredNullItemList[i]);
                //}
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


        public override IValueObject GetItemCurrentStockList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemCurrentStockListBizActionVO BizActionObj = valueObject as clsGetItemCurrentStockListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemCurrentStockList");

                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");

                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.UserID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, BizActionObj.BatchID);

                //Added by Ashish Z. for Indent wise Stock
                if (BizActionObj.IsForCentralPurChaseFromApproveIndent)
                {
                    dbServer.AddInParameter(command, "IsFromIndent", DbType.Boolean, BizActionObj.IsForCentralPurChaseFromApproveIndent);
                    dbServer.AddInParameter(command, "IndentID", DbType.Int64, BizActionObj.IndentID);
                    dbServer.AddInParameter(command, "IndentUnitID", DbType.Int64, BizActionObj.IndentUnitID);
                }
                //

                if (BizActionObj.ItemName != null && BizActionObj.ItemName.Length != 0)
                    dbServer.AddInParameter(command, "ItemName", DbType.String, "%" + BizActionObj.ItemName + "%");

                if (BizActionObj.BatchCode != null && BizActionObj.BatchCode.Length != 0)
                    dbServer.AddInParameter(command, "BatchCode", DbType.String, "%" + BizActionObj.BatchCode + "%");

                #region Added by MMBABU

                if (BizActionObj.FromDate != null)
                {
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                }

                //if (BizActionObj.ToDate != null)
                //{
                //    BizActionObj.ToDate = BizActionObj.ToDate.Value.Date.AddDays(1);
                //    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                //}

                dbServer.AddInParameter(command, "isStockZero", DbType.Boolean, BizActionObj.IsStockZero);
                
                dbServer.AddInParameter(command, "IsConsignment", DbType.Boolean, BizActionObj.IsConsignment);

                #endregion

                // Begin : added by aniketk on 20Oct2018 for Item Group & Category filter
                dbServer.AddInParameter(command, "ItemGroupID", DbType.Int64, BizActionObj.ItemGroupID);
                dbServer.AddInParameter(command, "ItemCategoryID", DbType.Int64, BizActionObj.ItemCategoryID);
                // End : added by aniketk on 20Oct2018 for Item Group & Category filter

                

                //Begin : Added by AniketK on 10-Dec-2018 for Stock as On Date
                if (BizActionObj.ToDate != null)
                {
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                }
                //End : Added by AniketK on 10-Dec-2018 for Stock as On Date

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "sortExpression", DbType.Int32, BizActionObj.SortExpression);

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchList == null)
                        BizActionObj.BatchList = new List<clsItemStockVO>();
                    while (reader.Read())
                    {
                        clsItemStockVO objVO = new clsItemStockVO();

                        objVO.ID = Convert.ToInt64((DALExtension.HandleDBNull(reader["ID"])));
                        objVO.UnitId = Convert.ToInt64((DALExtension.HandleDBNull(reader["UnitId"])));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRPPACK"]));
                        objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["CPPack"]));
                        //objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                        objVO.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        objVO.UnitName = (string)DALHelper.HandleDBNull(reader["UnitName"]);
                        objVO.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.StoreName = (string)DALHelper.HandleDBNull(reader["StoreName"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.StockingUOM = (string)DALHelper.HandleDBNull(reader["UOM"]);

                        objVO.BaseCP = Convert.ToSingle(DALHelper.HandleDBNull(reader["PurchaseRate"]));   //Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCP"]));
                        objVO.BaseMRP = Convert.ToSingle(DALHelper.HandleDBNull(reader["MRP"])); //Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseMRP"]));

                        objVO.TotalNetCP = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalNetCP"]));

                        objVO.IsFreeItem = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFree"]));

                        //added by neena
                        objVO.SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objVO.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        objVO.CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objVO.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        objVO.IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objVO.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        //

                        // Begin : added by aniketk on 20-Oct-2018 for Item Group & Category filter
                        objVO.ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupString"]));
                        objVO.ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryString"]));
                        // End : added by aniketk on 20-Oct-2018 for Item Group & Category filter

                        //objVO.Status = false;
                        BizActionObj.BatchList.Add(objVO);

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


        public override IValueObject GetAvailableStockList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetAvailableStockListBizActionVO BizActionObj = valueObject as clsGetAvailableStockListBizActionVO;
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_rpt_AvailableStock");
                DbDataReader reader;

                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.ToDate);
                dbServer.AddInParameter(command, "clincId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "storeId", DbType.Int64, 0);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchList == null)
                        BizActionObj.BatchList = new List<clsItemStockVO>();
                    while (reader.Read())
                    {
                        clsItemStockVO objVO = new clsItemStockVO();


                        //objVO.ItemCode = (string)DALHelper.HandleDBNull(reader["ItemCode"]);
                        objVO.ItemName = (string)DALHelper.HandleDBNull(reader["ItemName"]);
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.PhysicalStock = 0;
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        BizActionObj.BatchList.Add(objVO);

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

        // Add Stock For Free Items
        public override IValueObject AddFree(IValueObject valueObject, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
        {
            clsAddItemStockBizActionVO BizActionObj = valueObject as clsAddItemStockBizActionVO;

            if (BizActionObj.Details.ID == 0)
                BizActionObj = AddDetailsFree(BizActionObj, UserVo, myConnection, myTransaction);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;
        }

        // Add Stock For Free Items
        private clsAddItemStockBizActionVO AddDetailsFree(clsAddItemStockBizActionVO BizActionObj, clsUserVO UserVo, DbConnection myConnection, DbTransaction myTransaction)
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


                clsItemStockVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemStockFree");  // Add Stock For Free Items  //CIMS_AddItemStock
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "DepartmentID", DbType.Int64, objDetailsVO.DepartmentID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, objDetailsVO.ItemID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, objDetailsVO.BatchID);
                if (objDetailsVO.BatchCode != null)
                {
                    if (objDetailsVO.BatchCode.Trim().Length > 0)
                    {
                        dbServer.AddInParameter(command, "BatchCode", DbType.String, objDetailsVO.BatchCode);
                        dbServer.AddInParameter(command, "ExpiryDateNew", DbType.DateTime, objDetailsVO.ExpiryDate);
                    }
                }
                dbServer.AddInParameter(command, "TransactionTypeID", DbType.Int16, (Int16)objDetailsVO.TransactionTypeID);
                dbServer.AddInParameter(command, "TransactionID", DbType.Int64, objDetailsVO.TransactionID);
                dbServer.AddInParameter(command, "OperationType", DbType.Int16, (Int16)objDetailsVO.OperationType);
                dbServer.AddInParameter(command, "TransactionQuantity", DbType.Double, objDetailsVO.TransactionQuantity);
                dbServer.AddInParameter(command, "InputTransactionQuantity", DbType.Double, objDetailsVO.InputTransactionQuantity);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.TransactionTypeID.ToString());

                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                #region For Conversion Factor

                dbServer.AddInParameter(command, "BaseUOMID", DbType.Int64, objDetailsVO.BaseUOMID);
                dbServer.AddInParameter(command, "BaseCF", DbType.Double, objDetailsVO.BaseConversionFactor);
                dbServer.AddInParameter(command, "StockUOMID", DbType.Int64, objDetailsVO.SUOMID);
                dbServer.AddInParameter(command, "StockCF", DbType.Double, objDetailsVO.ConversionFactor);
                dbServer.AddInParameter(command, "TransactionUOMID", DbType.Int64, objDetailsVO.SelectedUOM.ID);
                dbServer.AddInParameter(command, "StockingQuantity", DbType.Double, objDetailsVO.StockingQuantity);

                #endregion

                # region//For new batch for nonbatchitem from OB
                dbServer.AddInParameter(command, "PurchaseRate1", DbType.Single, objDetailsVO.PurchaseRate);
                dbServer.AddInParameter(command, "MRP1", DbType.Single, objDetailsVO.MRP);
                dbServer.AddInParameter(command, "IsFromOpeningBalance", DbType.Boolean, objDetailsVO.IsFromOpeningBalance);
                # endregion

                if (objDetailsVO.UnitId > 0)
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, objDetailsVO.UnitId);
                else
                    dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);




                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                // Added By Rohit
                dbServer.AddInParameter(command, "BarCode", DbType.String, objDetailsVO.BarCode);
                // End

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");

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

        // Use to show only Free Item Batches
        public override IValueObject GetItemBatchwiseStockFree(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemStockBizActionVO BizActionObj = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> expiredNullItemList = new List<clsItemStockVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearchFree");  //CIMS_GetItemBatchListForSearch
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command, "ShowExpiredBatches", DbType.Boolean, BizActionObj.ShowExpiredBatches);
                dbServer.AddInParameter(command, "ShowZeroStockBatches", DbType.Boolean, BizActionObj.ShowZeroStockBatches);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.MaximumRows);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "IsFree", DbType.Boolean, BizActionObj.IsFree);  // Set to show only Free Item Batches

                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchList == null)
                        BizActionObj.BatchList = new List<clsItemStockVO>();
                    while (reader.Read())
                    {
                        clsItemStockVO objVO = new clsItemStockVO();

                        objVO.ID = Convert.ToInt64((DALExtension.HandleDBNull(reader["ID"])));
                        objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"]));
                        objVO.BatchCode = (string)DALHelper.HandleDBNull(reader["BatchCode"]);
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                        objVO.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"]));
                        //Added By Bhushanp
                        objVO.SGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objVO.SGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"]));

                        objVO.CGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objVO.CGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"]));

                        objVO.IGSTPercentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objVO.IGSTAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"]));

                        objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                        objVO.Status = false;
                        objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        objVO.IsConsignment = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsConsignment"]));
                        //By Anjali....................................
                        objVO.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        objVO.AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        //..............................................
                        // Added By CDS
                        if (BizActionObj.ShowZeroStockBatches == true)
                        {
                            objVO.SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"]));
                            objVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"]));
                            objVO.SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"]));
                            objVO.PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                            objVO.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            objVO.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }
                        BizActionObj.BatchList.Add(objVO);
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

        //Use to show only GRN Item Batches added by Ashish Z. on 04May16 
        public override IValueObject GetGRNItemBatchwiseStockForQS(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetItemStockBizActionVO BizActionObj = valueObject as clsGetItemStockBizActionVO;
            List<clsItemStockVO> expiredNullItemList = new List<clsItemStockVO>();
            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetGRNItemBatchwiseStockForQS");
                DbDataReader reader;

                dbServer.AddInParameter(command, "ItemID", DbType.Int64, BizActionObj.ItemID);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.StoreID);
                dbServer.AddInParameter(command, "BatchID", DbType.Int64, BizActionObj.BatchID);
                dbServer.AddInParameter(command, "TransactionID", DbType.Int64, BizActionObj.TransactionID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.UnitID);

                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.BatchList == null)
                        BizActionObj.BatchList = new List<clsItemStockVO>();
                    while (reader.Read())
                    {
                        clsItemStockVO objVO = new clsItemStockVO();

                        objVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        objVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objVO.AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objVO.StockingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objVO.PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"]));
                        objVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));
                        objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        objVO.TransactionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionID"]));
                        objVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objVO.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));



                        //objVO.ID = Convert.ToInt64((DALExtension.HandleDBNull(reader["ID"])));
                            //objVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                            //objVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                            //objVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]));

                        //objVO.CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"]));


                        //objVO.ConversionFactor = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConversionFactor"]));



                        //objVO.VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"]));
                            //objVO.VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"]));
                            //objVO.DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"]));
                            //objVO.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                            //objVO.IsConsignment = Convert.ToBoolean(DALHelper.HandleBoolDBNull(reader["IsConsignment"]));
                            //objVO.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                            //objVO.AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        BizActionObj.BatchList.Add(objVO);
                    }
                }
                reader.NextResult();


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

    }
}
