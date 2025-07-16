namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;

    public class clsOpeningBalanceDAL : clsBaseOpeningBalanceDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsOpeningBalanceDAL()
        {
            try
            {
                if (this.dbServer == null)
                {
                    this.dbServer = HMSConfigurationManager.GetDatabaseReference();
                }
                if (this.logManager == null)
                {
                    this.logManager = LogManager.GetInstance();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO UserVo)
        {
            DbConnection myConnection = null;
            DbTransaction transaction = null;
            clsAddOpeningBalanceBizActionVO nvo = null;
            clsOpeningBalVO openingBalVO = null;
            try
            {
                myConnection = this.dbServer.CreateConnection();
                transaction = null;
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                nvo = valueObject as clsAddOpeningBalanceBizActionVO;
                openingBalVO = nvo.OpeningBalVO;
                foreach (clsOpeningBalVO lvo2 in nvo.OpeningBalanceList)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemBatch");
                    storedProcCommand.Connection = myConnection;
                    this.dbServer.AddInParameter(storedProcCommand, "LinkServer", DbType.String, lvo2.LinkServer);
                    if (lvo2.LinkServer != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, lvo2.LinkServer.Replace(@"\", "_"));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, lvo2.Date);
                    this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, lvo2.Time);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, lvo2.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, lvo2.BatchCode);
                    this.dbServer.AddInParameter(storedProcCommand, "ExpiryDate", DbType.DateTime, lvo2.ExpiryDate);
                    this.dbServer.AddInParameter(storedProcCommand, "Quantity", DbType.Double, lvo2.BaseQuantity);
                    this.dbServer.AddInParameter(storedProcCommand, "ConversionFactor", DbType.Double, lvo2.BaseConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "PurchaseRate", DbType.Double, lvo2.Rate / lvo2.BaseConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Double, lvo2.MRP / lvo2.BaseConversionFactor);
                    this.dbServer.AddInParameter(storedProcCommand, "StockingPurchaseRate", DbType.Double, lvo2.Rate);
                    this.dbServer.AddInParameter(storedProcCommand, "StockingMRP", DbType.Double, lvo2.MRP);
                    this.dbServer.AddInParameter(storedProcCommand, "VatPercentage", DbType.Double, lvo2.VATPercent);
                    this.dbServer.AddInParameter(storedProcCommand, "VATAmount", DbType.Double, lvo2.VATAmount);
                    this.dbServer.AddInParameter(storedProcCommand, "DiscountOnSale", DbType.Double, lvo2.DiscountOnSale);
                    this.dbServer.AddInParameter(storedProcCommand, "Remarks", DbType.String, lvo2.Remarks);
                    double num = lvo2.NetAmount / lvo2.Quantity;
                    this.dbServer.AddInParameter(storedProcCommand, "LandedRate", DbType.Double, num);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, openingBalVO.UnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, lvo2.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, lvo2.CreatedUnitId);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, lvo2.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, openingBalVO.AddedBy);
                    if (openingBalVO.AddedOn != null)
                    {
                        openingBalVO.AddedOn = openingBalVO.AddedOn.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, openingBalVO.AddedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, openingBalVO.AddedDateTime);
                    if (openingBalVO.AddedWindowsLoginName != null)
                    {
                        openingBalVO.AddedWindowsLoginName = openingBalVO.AddedWindowsLoginName.Trim();
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    lvo2.BatchID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                    lvo2.StockDetails.BatchID = lvo2.BatchID;
                    lvo2.StockDetails.OperationType = InventoryStockOperationType.Addition;
                    lvo2.StockDetails.ItemID = lvo2.ItemID;
                    lvo2.StockDetails.TransactionTypeID = InventoryTransactionType.OpeningBalance;
                    lvo2.StockDetails.TransactionID = 0L;
                    lvo2.StockDetails.TransactionQuantity = lvo2.BaseQuantity;
                    lvo2.StockDetails.StockingQuantity = lvo2.Quantity;
                    lvo2.StockDetails.BatchCode = lvo2.BatchCode;
                    lvo2.StockDetails.Date = lvo2.Date;
                    lvo2.StockDetails.Time = lvo2.Time;
                    lvo2.StockDetails.StoreID = new long?(lvo2.StoreID);
                    lvo2.StockDetails.UnitId = openingBalVO.UnitId;
                    lvo2.StockDetails.InputTransactionQuantity = lvo2.SingleQuantity;
                    lvo2.StockDetails.IsFromOpeningBalance = true;
                    lvo2.StockDetails.PurchaseRate = lvo2.Rate / lvo2.BaseConversionFactor;
                    lvo2.StockDetails.MRP = lvo2.MRP / lvo2.BaseConversionFactor;
                    lvo2.StockDetails.BaseUOMID = lvo2.BaseUOMID;
                    lvo2.StockDetails.BaseConversionFactor = lvo2.BaseConversionFactor;
                    lvo2.StockDetails.SUOMID = lvo2.SUOMID;
                    lvo2.StockDetails.ConversionFactor = lvo2.ConversionFactor;
                    lvo2.StockDetails.SelectedUOM = lvo2.SelectedUOM;
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                        Details = lvo2.StockDetails
                    };
                    nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVo, myConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    lvo2.StockDetails.ID = nvo2.Details.ID;
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
                nvo.SuccessStatus = -1;
                nvo.OpeningBalanceList = null;
            }
            finally
            {
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Close();
                }
                myConnection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject GetStockDetailsForOpeningBalance(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetStockDetailsForOpeningBalanceBizActionVO nvo = valueObject as clsGetStockDetailsForOpeningBalanceBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStockDetailsForOpeningBalance");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.OpeningBalance.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.OpeningBalance.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.OpeningBalance.UserID);
                if (nvo.OpeningBalance.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.OpeningBalance.FromDate);
                }
                if (nvo.OpeningBalance.ToDate != null)
                {
                    nvo.OpeningBalance.ToDate = nvo.OpeningBalance.ToDate;
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.OpeningBalance.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "ItemGroupID", DbType.Int64, nvo.OpeningBalance.ItemGroupID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.OpeningBalance.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.OpeningBalance.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.OpeningBalance.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.OpeningBalance.MaxRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsOpeningBalVO>();
                    }
                    while (reader.Read())
                    {
                        clsOpeningBalVO item = new clsOpeningBalVO {
                            InputTransactionQuantity = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["InputTransactionQuantity"])),
                            TransactionUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            MRP = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            Rate = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["CostPrice"])),
                            VATPercent = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])),
                            VatAmt = Convert.ToSingle(DALHelper.HandleDBNull(reader["VatAmount"])),
                            DiscountOnSale = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            TotalAmount = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalCP"])),
                            TotalNet = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalNetCP"])),
                            TotalMRP = (float) Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalMRP"])),
                            Barcode = Convert.ToString(DALHelper.HandleDBNull(reader["Barcode"])),
                            ItemGroup = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroup"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.OpeningBalance.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

