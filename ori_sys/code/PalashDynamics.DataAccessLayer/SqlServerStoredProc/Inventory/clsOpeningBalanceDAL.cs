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
namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    public class clsOpeningBalanceDAL : clsBaseOpeningBalanceDAL
    {
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        #endregion

        private clsOpeningBalanceDAL()
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

        public override IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            clsAddOpeningBalanceBizActionVO BizActionobj = null;
            clsOpeningBalVO objOpeningBalance = null;
            try
            {
                con = dbServer.CreateConnection();
                trans = null;
                con.Open();
                trans = con.BeginTransaction();
                BizActionobj = valueObject as clsAddOpeningBalanceBizActionVO;
                objOpeningBalance = BizActionobj.OpeningBalVO;
                foreach (var item in BizActionobj.OpeningBalanceList)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemBatch");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, item.LinkServer);
                    if (item.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, item.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "Date", DbType.DateTime, item.Date);
                    dbServer.AddInParameter(command1, "Time", DbType.DateTime, item.Time);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "BatchCode", DbType.String, item.BatchCode);
                    dbServer.AddInParameter(command1, "ExpiryDate", DbType.DateTime, item.ExpiryDate);
                    //dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                    //dbServer.AddInParameter(command1, "ConversionFactor", DbType.Double, item.ConversionFactor);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.BaseQuantity);                    // Base Quantity            // For Conversion Factor
                    dbServer.AddInParameter(command1, "ConversionFactor", DbType.Double, item.BaseConversionFactor);    // Base Conversion Factor   // For Conversion Factor

                    //if (item.MainPUOM == item.SelectedPUM.Description)
                    //{
                    //    item.MRP = item.MRP / item.ConversionFactor;
                    //    item.Rate = item.Rate / item.ConversionFactor;
                    //}

                    //dbServer.AddInParameter(command1, "PurchaseRate", DbType.Double, item.Rate);      
                    //dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);

                    dbServer.AddInParameter(command1, "PurchaseRate", DbType.Double, (item.Rate / item.BaseConversionFactor));      // Base Purchase Rate              // For Conversion Factor
                    dbServer.AddInParameter(command1, "MRP", DbType.Double, (item.MRP / item.BaseConversionFactor));                // Base MRP                        // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockingPurchaseRate", DbType.Double, item.Rate);      // Stocking Purchase Rate      // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockingMRP", DbType.Double, item.MRP);                // Stocking MRP                // For Conversion Factor
                    dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
                    dbServer.AddInParameter(command1, "DiscountOnSale", DbType.Double, item.DiscountOnSale);
                    //    dbServer.AddInParameter(command1, "InputTransactionQuantity", DbType.Double, item.SingleQuantity);
                    dbServer.AddInParameter(command1, "Remarks", DbType.String, item.Remarks);
                    double landedrate = item.NetAmount / item.Quantity;
                    dbServer.AddInParameter(command1, "LandedRate", DbType.Double, landedrate);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, objOpeningBalance.UnitId);
                    dbServer.AddInParameter(command1, "StoreID", DbType.Int64, item.StoreID);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, item.CreatedUnitId);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, item.Status);
                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, objOpeningBalance.AddedBy);
                    if (objOpeningBalance.AddedOn != null) objOpeningBalance.AddedOn = objOpeningBalance.AddedOn.Trim();
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, objOpeningBalance.AddedOn);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objOpeningBalance.AddedDateTime);
                    if (objOpeningBalance.AddedWindowsLoginName != null) objOpeningBalance.AddedWindowsLoginName = objOpeningBalance.AddedWindowsLoginName.Trim();
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, 0);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionobj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    item.BatchID = Convert.ToInt64(dbServer.GetParameterValue(command1, "ID"));
                    item.StockDetails.BatchID = item.BatchID;
                    item.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    item.StockDetails.ItemID = item.ItemID;
                    item.StockDetails.TransactionTypeID = InventoryTransactionType.OpeningBalance;
                    item.StockDetails.TransactionID = 0;
                    //item.StockDetails.TransactionQuantity = (item.Quantity);

                    item.StockDetails.TransactionQuantity = (item.BaseQuantity);    // For Conversion Factor : Base Quantity
                    item.StockDetails.StockingQuantity = item.Quantity;             // For Conversion Factor : Stocking Quantity

                    item.StockDetails.BatchCode = item.BatchCode;
                    item.StockDetails.Date = item.Date;
                    item.StockDetails.Time = item.Time;
                    item.StockDetails.StoreID = item.StoreID;
                    item.StockDetails.UnitId = objOpeningBalance.UnitId;
                    item.StockDetails.InputTransactionQuantity = item.SingleQuantity;
                    item.StockDetails.IsFromOpeningBalance = true;
                    item.StockDetails.PurchaseRate = (item.Rate / item.BaseConversionFactor);
                    item.StockDetails.MRP = (item.MRP / item.BaseConversionFactor);

                    # region For Conversion Factor

                    item.StockDetails.BaseUOMID = item.BaseUOMID;
                    item.StockDetails.BaseConversionFactor = item.BaseConversionFactor;
                    item.StockDetails.SUOMID = item.SUOMID;
                    item.StockDetails.ConversionFactor = item.ConversionFactor;
                    item.StockDetails.SelectedUOM = item.SelectedUOM;

                    # endregion

                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();

                    obj.Details = item.StockDetails;

                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    item.StockDetails.ID = obj.Details.ID;
                }

                trans.Commit();
            }

            catch (Exception ex)
            {

                trans.Rollback();
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionobj.SuccessStatus = -1;

                BizActionobj.OpeningBalanceList = null;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                con = null;
                trans = null;
            }
            return BizActionobj;
        }

        public override IValueObject GetStockDetailsForOpeningBalance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStockDetailsForOpeningBalanceBizActionVO BizActionObj = valueObject as clsGetStockDetailsForOpeningBalanceBizActionVO;



            try
            {
                DbCommand command = dbServer.GetStoredProcCommand("CIMS_GetStockDetailsForOpeningBalance");
                DbDataReader reader;

                dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, BizActionObj.OpeningBalance.StoreID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, BizActionObj.OpeningBalance.UnitId);
                dbServer.AddInParameter(command, "UserID", DbType.Int64, BizActionObj.OpeningBalance.UserID);
                if (BizActionObj.OpeningBalance.FromDate != null)
                    dbServer.AddInParameter(command, "FromDate", DbType.DateTime, BizActionObj.OpeningBalance.FromDate);
                if (BizActionObj.OpeningBalance.ToDate != null)
                {
                    BizActionObj.OpeningBalance.ToDate = BizActionObj.OpeningBalance.ToDate;//.Date.AddDays(1);
                    dbServer.AddInParameter(command, "ToDate", DbType.DateTime, BizActionObj.OpeningBalance.ToDate);
                }

                dbServer.AddInParameter(command, "ItemGroupID", DbType.Int64, BizActionObj.OpeningBalance.ItemGroupID);
                dbServer.AddInParameter(command, "ItemName", DbType.String, BizActionObj.OpeningBalance.ItemName);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, BizActionObj.OpeningBalance.IsPagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, BizActionObj.OpeningBalance.StartIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int32, BizActionObj.OpeningBalance.MaxRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, int.MaxValue);


                reader = (DbDataReader)dbServer.ExecuteReader(command);

                if (reader.HasRows)
                {
                    if (BizActionObj.ItemList == null)
                        BizActionObj.ItemList = new List<clsOpeningBalVO>();

                    while (reader.Read())
                    {
                        clsOpeningBalVO objVO = new clsOpeningBalVO();


                        objVO.InputTransactionQuantity = (float)Convert.ToDouble((DALHelper.HandleDBNull(reader["InputTransactionQuantity"])));
                        objVO.TransactionUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objVO.ExpiryDate = (DateTime?)DALHelper.HandleDate(reader["ExpiryDate"]);
                        objVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objVO.MRP = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])));
                        objVO.Rate = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["CostPrice"])));
                        objVO.VATPercent = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])));
                        objVO.VatAmt = Convert.ToSingle((DALHelper.HandleDBNull(reader["VatAmount"])));
                        objVO.DiscountOnSale = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])));
                        objVO.TotalAmount = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalCP"])));
                        objVO.TotalNet = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalNetCP"])));
                        objVO.TotalMRP = (float)(Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalMRP"])));
                        objVO.Barcode = Convert.ToString(DALHelper.HandleDBNull(reader["Barcode"]));
                        objVO.ItemGroup = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroup"]));


                        BizActionObj.ItemList.Add(objVO);
                    }


                }
                reader.NextResult();
                BizActionObj.OpeningBalance.TotalRows = (int)dbServer.GetParameterValue(command, "TotalRows");


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

        //public override IValueObject CheckDuplicasy(IValueObject valueObject, clsUserVO UserVo)
        //{

        //}

    }
}
