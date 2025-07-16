using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using com.seedhealthcare.hms.Web.Logging;
using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Inventory;
using System.Data.Common;
using System.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Reflection;
using PalashDynamics.ValueObjects.CompoundDrug;
using PalashDynamics.ValueObjects.Log;

namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{


    public class clsItemSalesDAL : clsBaseItemSalesDAL
    {

        //This Region Contains Variables Which are Used At Form Level
        #region Variables Declaration
        //Declare the database object
        private Database dbServer = null;
        //Declare the LogManager object
        private LogManager logManager = null;
        //By rohinee for audit trail dated 26/9/16
        bool IsAuditTrail = false;
        #endregion

        private clsItemSalesDAL()
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

            clsAddItemSalesBizActionVO BizActionObj = valueObject as clsAddItemSalesBizActionVO;

            // if (BizActionObj.Details.ID == 0)
            BizActionObj = AddDetails(BizActionObj, UserVo, null, null);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;

        }

        public override IValueObject Add(IValueObject valueObject, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            //throw new NotImplementedException();

            clsAddItemSalesBizActionVO BizActionObj = valueObject as clsAddItemSalesBizActionVO;

            // if (BizActionObj.Details.ID == 0)
            BizActionObj = AddDetails(BizActionObj, UserVo, pConnection, pTransaction);
            //else
            // BizActionObj = UpdateDetails(BizActionObj);

            return valueObject;

        }

        #region Previous AddDetails

        //private clsAddItemSalesBizActionVO AddDetails(clsAddItemSalesBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        //{
        //    DbConnection con = null;
        //    DbTransaction trans = null;


        //    try
        //    {
        //        if (pConnection != null) con = pConnection;
        //        else con = dbServer.CreateConnection();

        //        if (con.State == ConnectionState.Closed) con.Open();

        //        if (pTransaction != null) trans = pTransaction;
        //        else trans = con.BeginTransaction();

        //        clsItemSalesVO objDetailsVO = BizActionObj.Details;
        //        double dblPercentage = 0;
        //        double dblVATAmount = 0;
        //        foreach (var item in objDetailsVO.Items)
        //        {
        //            dblPercentage += item.VATPercent > 0.0 ? item.VATPercent : 0.0;
        //            dblVATAmount += item.VATAmount > 0.0 ? item.VATAmount : 0.0;
        //        }
        //        DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemSales");

        //        dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //        if (objDetailsVO.LinkServer != null)
        //            dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

        //        dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
        //        dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Date);
        //        dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
        //        dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
        //        dbServer.AddInParameter(command, "VisitID", DbType.Int64, objDetailsVO.VisitID);
        //        dbServer.AddInParameter(command, "ItemSaleNo", DbType.String, objDetailsVO.ItemSaleNo);
        //        dbServer.AddInParameter(command, "ConcessionPercentage", DbType.Double, objDetailsVO.ConcessionPercentage);
        //        dbServer.AddInParameter(command, "ConcessionAmount", DbType.Double, objDetailsVO.ConcessionAmount);
        //        dbServer.AddInParameter(command, "VATPercentage", DbType.Double, dblPercentage);
        //        dbServer.AddInParameter(command, "VATAmount", DbType.Double, dblVATAmount);
        //        dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
        //        dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
        //        dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
        //        dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
        //        dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.BillID);
        //        dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, objDetailsVO.IsBilled);
        //        //Added By Somnath
        //        dbServer.AddInParameter(command, "DeliveryBY", DbType.Int64, objDetailsVO.DeliveryBY);
        //        dbServer.AddInParameter(command, "DeliveryAddress", DbType.String, objDetailsVO.DeliveryAddress);
        //        dbServer.AddInParameter(command, "PurChaseFrequency", DbType.Int64, objDetailsVO.PurchaseFrequency);
        //        dbServer.AddInParameter(command, "PurChaseFrequencyUnit", DbType.Int64, objDetailsVO.PurchaseFrequencyUnit);


        //        //End
        //        dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

        //        dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //        dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //        dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
        //        dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //        dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //        dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //        dbServer.AddParameter(command, "TokenNo", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.TokenNo);
        //        dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
        //        dbServer.AddInParameter(command, "IsPrescribedDrug", DbType.Boolean, objDetailsVO.IsPrescribedDrug);
        //        dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

        //        //  int intStatus = dbServer.ExecuteNonQuery(command, trans);

        //        int intStatus = 0;


        //        intStatus = dbServer.ExecuteNonQuery(command, trans);


        //        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
        //        BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");
        //        BizActionObj.Details.TokenNo = Convert.ToInt32(dbServer.GetParameterValue(command, "TokenNo"));



        //        if (BizActionObj.CompoundDrugMaster.Count > 0)
        //        {
        //            #region Compaound and Normal drug
        //            foreach (var item2 in BizActionObj.CompoundDrugMaster)
        //            {

        //                clsCompoundDrugMasterVO objCompoundDrug = item2;
        //                DbCommand command11 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrug");
        //                dbServer.AddInParameter(command11, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command11, "Code", DbType.String, objCompoundDrug.Code);
        //                dbServer.AddInParameter(command11, "Description", DbType.String, objCompoundDrug.Description);
        //                dbServer.AddInParameter(command11, "LaborPercentage", DbType.Double, objCompoundDrug.LaborPercentage);
        //                dbServer.AddInParameter(command11, "LaborAmount", DbType.Double, objCompoundDrug.LaborAmount);
        //                dbServer.AddInParameter(command11, "Status", DbType.Boolean, objCompoundDrug.Status);
        //                dbServer.AddInParameter(command11, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command11, "UpdatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command11, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command11, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command11, "AddedDateTime", DbType.DateTime, objCompoundDrug.AddedDateTime);
        //                dbServer.AddInParameter(command11, "UpdatedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command11, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command11, "UpdatedDateTime", DbType.DateTime, objCompoundDrug.AddedDateTime);
        //                dbServer.AddInParameter(command11, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddInParameter(command11, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddParameter(command11, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objCompoundDrug.ID);
        //                dbServer.AddOutParameter(command11, "ResultStatus", DbType.Int32, int.MaxValue);
        //                int intStatus26 = dbServer.ExecuteNonQuery(command11, trans);
        //                objCompoundDrug.ID = Convert.ToInt64(dbServer.GetParameterValue(command11, "ID"));
        //                objCompoundDrug.SuccessStatus = (int)dbServer.GetParameterValue(command11, "ResultStatus");

        //                foreach (var item in objDetailsVO.Items)
        //                {
        //                    if (item.CompoundMaster != null && item.CompoundMaster.Description == objCompoundDrug.Description)
        //                    {
        //                        item.ItemSaleId = BizActionObj.Details.ID;
        //                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemSalesDetails");
        //                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //                        if (objDetailsVO.LinkServer != null)
        //                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
        //                        dbServer.AddInParameter(command1, "ItemSaleId", DbType.Int64, item.ItemSaleId);
        //                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
        //                        dbServer.AddInParameter(command1, "BatchId", DbType.String, item.BatchID);
        //                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
        //                        dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);
        //                        dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
        //                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //                        dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
        //                        dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
        //                        dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.Amount);
        //                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
        //                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
        //                        if (item.CompoundMaster == null)
        //                        {
        //                            dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, 0);
        //                            dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, 0);
        //                        }
        //                        else
        //                        {
        //                            if (objCompoundDrug.ID > 0)
        //                            {
        //                                dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, objCompoundDrug.ID);
        //                                dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                            }
        //                            else
        //                            {
        //                                dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, item.CompoundMaster.ID);
        //                                dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, item.CompoundMaster.UnitID);

        //                            }


        //                        }
        //                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
        //                        intStatus = dbServer.ExecuteNonQuery(command1, trans);
        //                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
        //                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");

        //                        if (objCompoundDrug.SuccessStatus == 1)
        //                        {
        //                            DbCommand command12 = dbServer.GetStoredProcCommand("CIMS_AddCompoundDrugItems");
        //                            dbServer.AddInParameter(command12, "CompoundDrugId", DbType.Int64, objCompoundDrug.ID);
        //                            dbServer.AddInParameter(command12, "CompoundDrugUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                            dbServer.AddInParameter(command12, "ItemID", DbType.Double, item.ItemID);
        //                            dbServer.AddInParameter(command12, "Quantity", DbType.Double, item.Quantity);
        //                            dbServer.AddInParameter(command12, "UnitID", DbType.Double, UserVo.UserLoginInfo.UnitId);
        //                            dbServer.AddParameter(command12, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0);
        //                            dbServer.AddOutParameter(command12, "ResultStatus", DbType.Int32, int.MaxValue);
        //                            int intStatus4 = dbServer.ExecuteNonQuery(command12, trans);
        //                        }


        //                        if (objDetailsVO.IsBilled && objDetailsVO.IsCounterSale == false)
        //                        {
        //                            clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //                            clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
        //                            clsItemStockVO StockDetails = new clsItemStockVO();

        //                            StockDetails.ItemID = item.ItemID;
        //                            StockDetails.BatchID = item.BatchID;
        //                            StockDetails.TransactionTypeID = InventoryTransactionType.ItemsSale;
        //                            StockDetails.TransactionQuantity = item.Quantity;
        //                            StockDetails.TransactionID = item.ItemSaleId;
        //                            StockDetails.Date = BizActionObj.Details.Date;
        //                            StockDetails.Time = BizActionObj.Details.Time;
        //                            StockDetails.OperationType = InventoryStockOperationType.Subtraction;
        //                            StockDetails.StoreID = BizActionObj.Details.StoreID;
        //                            obj.Details = StockDetails;
        //                            //obj.my
        //                            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

        //                            if (obj.SuccessStatus == -1)
        //                            {
        //                                throw new Exception();
        //                            }

        //                            StockDetails.ID = obj.Details.ID;
        //                        }

        //                    }
        //                    else
        //                    {
        //                        item.ItemSaleId = BizActionObj.Details.ID;
        //                        DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemSalesDetails");

        //                        dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //                        if (objDetailsVO.LinkServer != null)
        //                            dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));


        //                        dbServer.AddInParameter(command1, "ItemSaleId", DbType.Int64, item.ItemSaleId);
        //                        dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
        //                        dbServer.AddInParameter(command1, "BatchId", DbType.String, item.BatchID);
        //                        dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
        //                        dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);
        //                        dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
        //                        dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //                        dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
        //                        dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
        //                        dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.Amount);
        //                        dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);

        //                        dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
        //                        if (item.CompoundMaster == null)
        //                        {
        //                            dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, 0);
        //                            dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, 0);
        //                        }
        //                        else
        //                        {
        //                            dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, item.CompoundMaster.ID);
        //                            dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, item.CompoundMaster.UnitID);
        //                        }
        //                        dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                        dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //                        dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //                        dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                        dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                        dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

        //                        dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //                        dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);


        //                        // int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

        //                        //  int intStatus = 0;


        //                        intStatus = dbServer.ExecuteNonQuery(command1, trans);


        //                        BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
        //                        item.ID = (long)dbServer.GetParameterValue(command1, "ID");

        //                        if (objDetailsVO.IsBilled && objDetailsVO.IsCounterSale == false)
        //                        {
        //                            clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //                            clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
        //                            clsItemStockVO StockDetails = new clsItemStockVO();

        //                            StockDetails.ItemID = item.ItemID;
        //                            StockDetails.BatchID = item.BatchID;
        //                            StockDetails.TransactionTypeID = InventoryTransactionType.ItemsSale;
        //                            StockDetails.TransactionQuantity = item.Quantity;
        //                            StockDetails.TransactionID = item.ItemSaleId;
        //                            StockDetails.Date = BizActionObj.Details.Date;
        //                            StockDetails.Time = BizActionObj.Details.Time;
        //                            StockDetails.OperationType = InventoryStockOperationType.Subtraction;
        //                            StockDetails.StoreID = BizActionObj.Details.StoreID;
        //                            obj.Details = StockDetails;
        //                            //obj.my
        //                            obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

        //                            if (obj.SuccessStatus == -1)
        //                            {
        //                                throw new Exception();
        //                            }

        //                            StockDetails.ID = obj.Details.ID;
        //                        }
        //                    }


        //                }



        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region normal Drug
        //            foreach (var item in objDetailsVO.Items)
        //            {
        //                item.ItemSaleId = BizActionObj.Details.ID;
        //                DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemSalesDetails");

        //                dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //                if (objDetailsVO.LinkServer != null)
        //                    dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

        //                dbServer.AddInParameter(command1, "ItemSaleId", DbType.Int64, item.ItemSaleId);
        //                dbServer.AddInParameter(command1, "Date", DbType.DateTime, DateTime.Now);
        //                dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
        //                dbServer.AddInParameter(command1, "BatchId", DbType.String, item.BatchID);
        //                dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
        //                dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);
        //                dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
        //                dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //                dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
        //                dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
        //                dbServer.AddInParameter(command1, "PatientPaidAmount", DbType.Double, item.ItemPaidAmount);
        //                dbServer.AddInParameter(command1, "CompanyPaidAmt", DbType.Double, 0);
        //                //dbServer.AddInParameter(command1, "PatientBalanceAmount", DbType.Double, item.BalanceAmount);
        //                dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.Amount);
        //                dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
        //                dbServer.AddInParameter(command1, "IsCashTariff", DbType.Boolean, item.IsCashTariff);
        //                if (item.IsCashTariff == false)
        //                {
        //                    dbServer.AddInParameter(command1, "DiscountPerc", DbType.Double, item.DiscountPerc);
        //                    dbServer.AddInParameter(command1, "DiscountAmt", DbType.Double, item.DiscountAmt);
        //                    //dbServer.AddInParameter(command1, "CompanyBalanceAmount", DbType.Double, item.NetAmount);
        //                }
        //                else
        //                {
        //                    dbServer.AddInParameter(command1, "DiscountPerc", DbType.Double, 0);
        //                    dbServer.AddInParameter(command1, "DiscountAmt", DbType.Double, 0);
        //                    //dbServer.AddInParameter(command1, "CompanyBalanceAmount", DbType.Double, 0);
        //                }
        //                dbServer.AddInParameter(command1, "DeductionPerc", DbType.Double, item.DeductiblePerc);
        //                dbServer.AddInParameter(command1, "DeductionAmt", DbType.Double, item.DeductibleAmt);
        //                dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
        //                if (item.CompoundMaster == null)
        //                {
        //                    dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, 0);
        //                    dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, 0);
        //                }
        //                else
        //                {
        //                    dbServer.AddInParameter(command1, "CompoundID", DbType.Int64, item.CompoundMaster.ID);
        //                    dbServer.AddInParameter(command1, "CompoundUnitID", DbType.Int64, item.CompoundMaster.UnitID);
        //                }
        //                dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //                dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
        //                //dbServer.AddParameter(command1, "ItemWiseSaleId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ItemWiseSaleId);
        //                dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
        //                intStatus = dbServer.ExecuteNonQuery(command1, trans);
        //                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
        //                item.ID = (long)dbServer.GetParameterValue(command1, "ID");

        //                //item.ItemWiseSaleId = item.ID;
        //                DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_AddItemWiseSalesDetails");

        //                dbServer.AddInParameter(command2, "LinkServer", DbType.String, objDetailsVO.LinkServer);
        //                if (objDetailsVO.LinkServer != null)
        //                    dbServer.AddInParameter(command2, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));


        //                dbServer.AddInParameter(command2, "ItemSaleDetailId", DbType.Int64, item.ID);
        //                // dbServer.AddInParameter(command2, "ItemSaleId", DbType.Int64, item.ItemSaleId);
        //                dbServer.AddInParameter(command2, "Date", DbType.DateTime, DateTime.Now);
        //                dbServer.AddInParameter(command2, "ItemID", DbType.Int64, item.ItemID);
        //                dbServer.AddInParameter(command2, "BatchId", DbType.String, item.BatchID);
        //                dbServer.AddInParameter(command2, "Quantity", DbType.Double, item.Quantity);
        //                dbServer.AddInParameter(command2, "MRP", DbType.Double, item.MRP);
        //                dbServer.AddInParameter(command2, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
        //                dbServer.AddInParameter(command2, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
        //                dbServer.AddInParameter(command2, "VatPercentage", DbType.Double, item.VATPercent);
        //                dbServer.AddInParameter(command2, "VATAmount", DbType.Double, item.VATAmount);
        //                dbServer.AddInParameter(command2, "PatientPaidAmount", DbType.Double, item.ItemPaidAmount);
        //                dbServer.AddInParameter(command2, "CompanyPaidAmt", DbType.Double, 0);
        //                dbServer.AddInParameter(command2, "PatientBalanceAmount", DbType.Double, item.BalanceAmount);
        //                dbServer.AddInParameter(command2, "TotalAmount", DbType.Double, item.Amount);
        //                dbServer.AddInParameter(command2, "NetAmount", DbType.Double, item.NetAmount);
        //                dbServer.AddInParameter(command2, "IsCashTariff", DbType.Boolean, item.IsCashTariff);
        //                if (item.IsCashTariff == false)
        //                {
        //                    dbServer.AddInParameter(command2, "DiscountPerc", DbType.Double, item.DiscountPerc);
        //                    dbServer.AddInParameter(command2, "DiscountAmt", DbType.Double, item.DiscountAmt);
        //                    dbServer.AddInParameter(command2, "CompanyBalanceAmount", DbType.Double, item.NetAmount);
        //                }
        //                else
        //                {
        //                    dbServer.AddInParameter(command2, "DiscountPerc", DbType.Double, 0);
        //                    dbServer.AddInParameter(command2, "DiscountAmt", DbType.Double, 0);
        //                    dbServer.AddInParameter(command2, "CompanyBalanceAmount", DbType.Double, 0);
        //                }
        //                dbServer.AddInParameter(command2, "DeductionPerc", DbType.Double, item.DeductiblePerc);
        //                dbServer.AddInParameter(command2, "DeductionAmt", DbType.Double, item.DeductibleAmt);
        //                dbServer.AddInParameter(command2, "Status", DbType.Boolean, objDetailsVO.Status);
        //                if (item.CompoundMaster == null)
        //                {
        //                    dbServer.AddInParameter(command2, "CompoundID", DbType.Int64, 0);
        //                    dbServer.AddInParameter(command2, "CompoundUnitID", DbType.Int64, 0);
        //                }
        //                else
        //                {
        //                    dbServer.AddInParameter(command2, "CompoundID", DbType.Int64, item.CompoundMaster.ID);
        //                    dbServer.AddInParameter(command2, "CompoundUnitID", DbType.Int64, item.CompoundMaster.UnitID);
        //                }
        //                dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
        //                dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

        //                dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
        //                dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
        //                dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
        //                dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
        //                dbServer.AddParameter(command2, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ItemWiseSaleId);
        //                //dbServer.AddParameter(command2, "ItemWiseSaleId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ItemWiseSaleId);
        //                dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, int.MaxValue);
        //                intStatus = dbServer.ExecuteNonQuery(command2, trans);
        //                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command2, "ResultStatus");
        //                item.ItemWiseSaleId = (long)dbServer.GetParameterValue(command2, "ID");
        //                // item.ItemWiseSaleId = (long)dbServer.GetParameterValue(command2, "ItemWiseSaleId");



        //                if (objDetailsVO.IsBilled && objDetailsVO.IsCounterSale == false)
        //                {
        //                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
        //                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
        //                    clsItemStockVO StockDetails = new clsItemStockVO();
        //                    StockDetails.ItemID = item.ItemID;
        //                    StockDetails.BatchID = item.BatchID;
        //                    StockDetails.TransactionTypeID = InventoryTransactionType.ItemsSale;
        //                    StockDetails.TransactionQuantity = item.Quantity;
        //                    StockDetails.TransactionID = item.ItemSaleId;
        //                    StockDetails.Date = BizActionObj.Details.Date;
        //                    StockDetails.Time = BizActionObj.Details.Time;
        //                    StockDetails.OperationType = InventoryStockOperationType.Subtraction;
        //                    StockDetails.StoreID = BizActionObj.Details.StoreID;
        //                    obj.Details = StockDetails;
        //                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);
        //                    if (obj.SuccessStatus == -1)
        //                    {
        //                        throw new Exception();
        //                    }
        //                    StockDetails.ID = obj.Details.ID;
        //                }



        //            }

        //            #endregion
        //        }
        //        if (pConnection == null) trans.Commit();
        //        BizActionObj.SuccessStatus = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        //throw;
        //        BizActionObj.SuccessStatus = -1;
        //        if (pConnection != null) trans.Rollback();

        //    }
        //    finally
        //    {
        //        if (pConnection == null) con.Close();
        //    }

        //    return BizActionObj;
        //}

        #endregion

        // New AddDetails From ALAMAL
        private clsAddItemSalesBizActionVO AddDetails(clsAddItemSalesBizActionVO BizActionObj, clsUserVO UserVo, DbConnection pConnection, DbTransaction pTransaction)
        {
            DbConnection con = null;
            DbTransaction trans = null;
            try
            {
                if (pConnection != null) con = pConnection;
                else con = dbServer.CreateConnection();

                if (con.State == ConnectionState.Closed) con.Open();

                if (pTransaction != null) trans = pTransaction;
                else trans = con.BeginTransaction();

                clsItemSalesVO objDetailsVO = BizActionObj.Details;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddItemSales");

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "PatientID", DbType.Int64, objDetailsVO.PatientID);
                dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, objDetailsVO.PatientUnitID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, objDetailsVO.VisitID);
                dbServer.AddInParameter(command, "ItemSaleNo", DbType.String, objDetailsVO.ItemSaleNo);
                dbServer.AddInParameter(command, "ConcessionPercentage", DbType.Double, objDetailsVO.ConcessionPercentage);
                dbServer.AddInParameter(command, "ConcessionAmount", DbType.Double, objDetailsVO.ConcessionAmount);
                dbServer.AddInParameter(command, "VATPercentage", DbType.Double, objDetailsVO.VATPercentage);
                dbServer.AddInParameter(command, "VATAmount", DbType.Double, objDetailsVO.VATAmount);
                dbServer.AddInParameter(command, "TotalAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                dbServer.AddInParameter(command, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                dbServer.AddInParameter(command, "BillID", DbType.Int64, objDetailsVO.BillID);
                dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, objDetailsVO.IsBilled);

                //By Anjali............................................
                dbServer.AddInParameter(command, "ReasonForVariance", DbType.String, objDetailsVO.ReasonForVariance);
                dbServer.AddInParameter(command, "ReferenceDoctorID", DbType.Int64, objDetailsVO.ReferenceDoctorID);
                dbServer.AddInParameter(command, "ReferenceDoctor", DbType.String, objDetailsVO.ReferenceDoctor);
                //......................................................

                //Added by AJ DAte 17/2/2017
                dbServer.AddInParameter(command, "AdmissionID", DbType.Int64, objDetailsVO.Opd_Ipd_External_Id);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, objDetailsVO.Opd_Ipd_External_UnitId);
                //------------------
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);                
                dbServer.AddInParameter(command, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                dbServer.AddInParameter(command, "AddedBy", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);

                //  int intStatus = dbServer.ExecuteNonQuery(command, trans);

                int intStatus = 0;
                intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.Details.ID = (long)dbServer.GetParameterValue(command, "ID");

                foreach (var item in objDetailsVO.Items)
                {
                    item.ItemSaleId = BizActionObj.Details.ID;
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddItemSalesDetails");

                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));
                    dbServer.AddInParameter(command1, "ItemSaleId", DbType.Int64, item.ItemSaleId);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "BatchId", DbType.String, item.BatchID);
                    dbServer.AddInParameter(command1, "Quantity", DbType.Double, item.Quantity);
                    dbServer.AddInParameter(command1, "MRP", DbType.Double, item.MRP);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                    dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
                    dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.Amount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);

                    //By Anjali................................
                    dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "StoreID", DbType.Int64, objDetailsVO.StoreID);
                    dbServer.AddInParameter(command1, "PrescriptionDetailsID", DbType.Int64, item.PrescriptionDetailsID);
                    dbServer.AddInParameter(command1, "PrescriptionDetailsUnitID", DbType.Int64, item.PrescriptionDetailsUnitID);
                    dbServer.AddInParameter(command1, "PackageID", DbType.Int64, item.PackageID);


                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Single, item.BaseQuantity);                    // Base Quantity            // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);               // Transaction UOM      // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);                            // Base  UOM            // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID);                            // SUOM UOM                     // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);                    // Stocking ConversionFactor     // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Single, item.Quantity * item.ConversionFactor);  // StockingQuantity // For Conversion Factor

                    dbServer.AddInParameter(command1, "ActualNetAmt", DbType.Double, item.ActualNetAmt);

                    dbServer.AddInParameter(command1, "NetAmtCalculation", DbType.Double, item.NetAmtCalculation); //commented By Ashish Z. dated 140616
                    //dbServer.AddInParameter(command1, "NetAmtCalculation", DbType.Double, item.PendingBudget); //Added By Ashish Z. dated 140616

                    dbServer.AddInParameter(command1, "ItemVatType", DbType.Int32, item.ItemVatType);
                    //............................................................
                    //By AJ Date 16/2/2017--------
                    dbServer.AddInParameter(command1, "ISForMaterialConsumption", DbType.String, objDetailsVO.ISForMaterialConsumption);
                    //-----------------------------

                    #region Package Change 18042017
                    dbServer.AddInParameter(command1, "PackageBillID", DbType.Int64, item.PackageBillID);
                    dbServer.AddInParameter(command1, "PackageBillUnitID", DbType.Int64, item.PackageBillUnitID);

                    dbServer.AddInParameter(command1, "PackageConcessionPercentage", DbType.Double, item.PackageConcessionPercentage);    // For Package New Changes Added on 16062018
                    dbServer.AddInParameter(command1, "PackageConcessionAmount", DbType.Double, item.PackageConcessionAmount);            // For Package New Changes Added on 16062018

                    #endregion

                    #region For GST Changes 26062017 Bhushan P

                    dbServer.AddInParameter(command1, "SGSTPercentage", DbType.Double, item.SGSTPercent);
                    dbServer.AddInParameter(command1, "SGSTAmount", DbType.Double, item.SGSTAmount);
                    dbServer.AddInParameter(command1, "CGSTPercentage", DbType.Double, item.CGSTPercent);
                    dbServer.AddInParameter(command1, "CGSTAmount", DbType.Double, item.CGSTAmount);
                    dbServer.AddInParameter(command1, "IGSTPercentage", DbType.Double, item.IGSTPercent);
                    dbServer.AddInParameter(command1, "IGSTAmount", DbType.Double, item.IGSTAmount);


                    dbServer.AddInParameter(command1, "SGSTTaxType", DbType.Int32, item.SGSTtaxtype);
                    dbServer.AddInParameter(command1, "SGSTApplicableOn", DbType.Int32, item.SGSTapplicableon);
                    dbServer.AddInParameter(command1, "CGSTTaxType", DbType.Int32, item.CGSTtaxtype);
                    dbServer.AddInParameter(command1, "CGSTApplicableOn", DbType.Int32, item.CGSTapplicableon);
                    dbServer.AddInParameter(command1, "IGSTTaxType", DbType.Int32, item.IGSTtaxtype);
                    dbServer.AddInParameter(command1, "IGSTApplicableOn", DbType.Int32, item.IGSTapplicableon);




                    #endregion

                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);

                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);

                    dbServer.AddInParameter(command1, "AddedBy", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "AddedDateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);

                    dbServer.AddParameter(command1, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, item.ID);
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);


                    // int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);

                    //  int intStatus = 0;


                    intStatus = dbServer.ExecuteNonQuery(command1, trans);


                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    item.ID = (long)dbServer.GetParameterValue(command1, "ID");
                    if (objDetailsVO.ISForMaterialConsumption != true) //Added by AJ Date 30/1/2017
                    {
                    if (objDetailsVO.IsBilled)
                    {
                        clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                        clsItemStockVO StockDetails = new clsItemStockVO();

                        StockDetails.ItemID = item.ItemID;
                        StockDetails.BatchID = item.BatchID;
                        StockDetails.TransactionTypeID = InventoryTransactionType.ItemsSale;
                       // StockDetails.TransactionQuantity = item.Quantity;
                        StockDetails.TransactionID = item.ItemSaleId;
                        StockDetails.Date = BizActionObj.Details.Date;
                        StockDetails.Time = BizActionObj.Details.Time;
                        StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        StockDetails.StoreID = BizActionObj.Details.StoreID;

                        //By Anjali...............
                        StockDetails.InputTransactionQuantity = Convert.ToSingle(item.Quantity);  // InputTransactionQuantity // For Conversion Factor   
                        StockDetails.BaseUOMID = item.BaseUOMID;                        // Base  UOM   // For Conversion Factor
                        StockDetails.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor   // For Conversion Factor
                        StockDetails.TransactionQuantity = item.BaseQuantity;         // Base Quantity            // For Conversion Factor
                        StockDetails.SUOMID = item.SUOMID;                           // SUOM UOM                     // For Conversion Factor
                        StockDetails.ConversionFactor = item.ConversionFactor;       // Stocking ConversionFactor     // For Conversion Factor
                        StockDetails.StockingQuantity = item.Quantity * item.ConversionFactor;  // StockingQuantity // For Conversion Factor
                        StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor     

                        //..............................
                        obj.Details = StockDetails;
                        //obj.my
                        obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                        if (obj.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }

                        StockDetails.ID = obj.Details.ID;
                    }
                    }
                }

                if (pConnection == null) trans.Commit();
                BizActionObj.SuccessStatus = 0;

            }
            catch (Exception ex)
            {
                //throw;
                BizActionObj.SuccessStatus = -1;
                if (pConnection == null) trans.Rollback();

            }
            finally
            {
                if (pConnection == null) con.Close();
            }

            return BizActionObj;
        }


        public override IValueObject GetItemSale(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesBizActionVO objItem = valueObject as clsGetItemSalesBizActionVO;
            objItem.Details = new List<clsItemSalesVO>();
            clsItemSalesVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSales");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objItem.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objItem.ToDate);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, objItem.UnitID);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objItem.CostingDivisionID);

                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SerachExpression);
                dbServer.AddInParameter(command, "billFreeze", DbType.Boolean, objItem.isBillFreezed);

                if (objItem.FirstName != null && objItem.FirstName.Length != 0)
                    dbServer.AddInParameter(command, "FirstName", DbType.String, objItem.FirstName + "%");
                else
                    dbServer.AddInParameter(command, "FirstName", DbType.String, null);

                if (objItem.LastName != null && objItem.LastName.Length != 0)
                    dbServer.AddInParameter(command, "LastName", DbType.String, objItem.LastName + "%");
                else
                    dbServer.AddInParameter(command, "LastName", DbType.String, null);

                dbServer.AddInParameter(command, "BillNo", DbType.String,"%" + objItem.BillNo + "%");


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemSalesVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));

                        objItemVO.CostingDivisionID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CostingDivisionID"]));

                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItemVO.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        objItemVO.PatientUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientUnitID"]));
                        objItemVO.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                        objItemVO.ItemSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ItemSaleNo"]));
                        objItemVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        // objItemVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));
                        objItemVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objItemVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItemVO.VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"]));
                        objItemVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.BalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["BalAmt"]));
                        objItemVO.OPDNo = Convert.ToInt64(DALHelper.HandleDBNull(reader["OPDNO"]));
                        objItemVO.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objItemVO.BillID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BillID"]));
                        objItemVO.LastName = (string)DALHelper.HandleDBNull(reader["LastName"]);
                        objItemVO.FirstName = (string)DALHelper.HandleDBNull(reader["FirstName"]);
                        objItemVO.MiddleName = (string)DALHelper.HandleDBNull(reader["MiddleName"]);
                        objItemVO.BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"]));

                        objItem.Details.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
                        reader.Close();
                }
            }
            return objItem;
        }


        public override IValueObject GetItemSaleDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesDetailsBizActionVO bizActionVO = valueObject as clsGetItemSalesDetailsBizActionVO;
            bizActionVO.Details = new List<clsItemSalesDetailsVO>();
            clsItemSalesDetailsVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSalesDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, bizActionVO.ItemSalesID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVO.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);


                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemSalesDetailsVO();
                        if (bizActionVO.IsFromItemSaleReturn == true)
                            objItemVO.IsItemSaleReturn = true;
                        else
                            objItemVO.IsItemSaleReturn = false;
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"]));
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItemVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objItemVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItemVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItemVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItemVO.VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"]));
                        objItemVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));
                        objItemVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objItemVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        // objItemVO.PatientPaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PaidAmount"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        objItemVO.InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));


                        //By Anjali............................................

                        objItemVO.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["ItemVatType"]));
                        objItemVO.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        objItemVO.SelectedUOM.Description = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                         objItemVO.SelectedUOM.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItemVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objItemVO.SellingUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                        objItemVO.PUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"]));
                        objItemVO.PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"]));
                        objItemVO.PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"]));
                        objItemVO.SellingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));
                        objItemVO.ActualNetAmt = Convert.ToSingle(DALHelper.HandleDBNull(reader["ActualNetAmt"]));
                        objItemVO.NetAmtCalculation = Convert.ToSingle(DALHelper.HandleDBNull(reader["NetAmtCalculation"]));
                        objItemVO.StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        objItemVO.SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        objItemVO.BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        //objItemVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItemVO.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        objItemVO.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        objItemVO.SaleUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItemVO.SaleUOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objItemVO.StockCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        //......................................................
                        objItemVO.SGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objItemVO.SGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        objItemVO.SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTTaxType"]));
                        objItemVO.SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTApplicableOn"]));

                        objItemVO.CGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objItemVO.CGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        objItemVO.CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTTaxType"]));
                        objItemVO.CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTApplicableOn"]));

                        objItemVO.IGSTPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objItemVO.IGSTAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        objItemVO.IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTTaxType"]));
                        objItemVO.IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTApplicableOn"]));
                       
                        bizActionVO.Details.Add(objItemVO);
                    }
                }
                reader.NextResult();
                bizActionVO.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
                        reader.Close();
                }
            }
            return bizActionVO;
        }

        public override IValueObject AddItemSaleReturn(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddItemSalesReturnBizActionVO BizActionObj = valueObject as clsAddItemSalesReturnBizActionVO;
            DbConnection con = dbServer.CreateConnection();
            DbTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                clsItemSalesReturnVO objDetailsVO = BizActionObj.ItemMatserDetail;

                DbCommand command = dbServer.GetStoredProcCommand("CIMS_AddtemSaleReturn");
                command.Connection = con;

                dbServer.AddInParameter(command, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                if (objDetailsVO.LinkServer != null)
                    dbServer.AddInParameter(command, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                dbServer.AddInParameter(command, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "CostingDivisionID", DbType.Int64, objDetailsVO.CostingDivisionID);

                dbServer.AddInParameter(command, "Date", DbType.DateTime, objDetailsVO.Date);
                dbServer.AddInParameter(command, "Time", DbType.DateTime, objDetailsVO.Time);
                //dbServer.AddInParameter(command, "SalesReturnNo", DbType.String, objDetailsVO.ItemSaleReturnNo);
                dbServer.AddParameter(command, "SalesReturnNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                dbServer.AddInParameter(command, "ItemSaleId", DbType.Int64, objDetailsVO.ItemSalesID);
                dbServer.AddInParameter(command, "Remarks", DbType.String, objDetailsVO.Remarks);
                dbServer.AddInParameter(command, "ConcessionPercentage", DbType.Double, objDetailsVO.ConcessionPercentage);
                dbServer.AddInParameter(command, "ConcessionAmount", DbType.Double, objDetailsVO.ConcessionAmount);
                dbServer.AddInParameter(command, "VATPercentage", DbType.Double, objDetailsVO.VATPercentage);
                dbServer.AddInParameter(command, "VATAmount", DbType.Double, objDetailsVO.VATAmount);
                //Added By Bhushanp For GST
                dbServer.AddInParameter(command, "TotalSGST", DbType.Double, objDetailsVO.TotalSGST);
                dbServer.AddInParameter(command, "TotalCGST", DbType.Double, objDetailsVO.TotalCGST);
                dbServer.AddInParameter(command, "TotalNetAmount", DbType.Double, objDetailsVO.TotalAmount);
                dbServer.AddInParameter(command, "NetAmount", DbType.Double, objDetailsVO.NetAmount);
                //By Anjali.................
                dbServer.AddInParameter(command, "CalculatedNetAmount", DbType.Double, objDetailsVO.CalculatedNetAmount);
                //............................
                dbServer.AddInParameter(command, "TotalReturnAmount", DbType.Double, objDetailsVO.TotalReturnAmount);
                dbServer.AddInParameter(command, "Status", DbType.Boolean, objDetailsVO.Status);
                dbServer.AddInParameter(command, "AddUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                dbServer.AddInParameter(command, "By", DbType.Int64, UserVo.ID);
                dbServer.AddInParameter(command, "On", DbType.String, UserVo.UserLoginInfo.MachineName);
                dbServer.AddInParameter(command, "DateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                dbServer.AddInParameter(command, "WindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                //dbServer.AddParameter(command, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, objDetailsVO.ID);
                dbServer.AddOutParameter(command, "ResultStatus", DbType.Int32, int.MaxValue);
                dbServer.AddOutParameter(command, "ID", DbType.Int64, 0);

                int intStatus = dbServer.ExecuteNonQuery(command, trans);
                BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command, "ResultStatus");
                BizActionObj.ItemSalesReturnID = (long)dbServer.GetParameterValue(command, "ID");
                BizActionObj.ItemMatserDetail.ItemSaleReturnNo = (string)dbServer.GetParameterValue(command, "SalesReturnNo");

                foreach (var item in BizActionObj.ItemsDetail)
                {
                    DbCommand command1 = dbServer.GetStoredProcCommand("CIMS_AddtemSaleReturnDetails");
                    command1.Connection = con;
                    dbServer.AddInParameter(command1, "LinkServer", DbType.String, objDetailsVO.LinkServer);
                    if (objDetailsVO.LinkServer != null)
                        dbServer.AddInParameter(command1, "LinkServerAlias", DbType.String, objDetailsVO.LinkServer.Replace(@"\", "_"));

                    dbServer.AddInParameter(command1, "ItemSaleReturnId", DbType.Int64, BizActionObj.ItemSalesReturnID);
                    //dbServer.AddInParameter(command1, "ItemSalesReturnUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "ItemID", DbType.Int64, item.ItemID);
                    dbServer.AddInParameter(command1, "BatchId", DbType.String, item.BatchID);
                    dbServer.AddInParameter(command1, "ReturnedQuantity", DbType.Double, item.ReturnQuantity);
                    dbServer.AddInParameter(command1, "Rate", DbType.Double, item.MRP);
                    dbServer.AddInParameter(command1, "ConcessionPercentage", DbType.Double, item.ConcessionPercentage);
                    dbServer.AddInParameter(command1, "ConcessionAmount", DbType.Double, item.ConcessionAmount);
                    dbServer.AddInParameter(command1, "VatPercentage", DbType.Double, item.VATPercent);
                    dbServer.AddInParameter(command1, "VATAmount", DbType.Double, item.VATAmount);
                    //Added By Bhushanp For GST 14072017
                    dbServer.AddInParameter(command1, "SGSTPercent", DbType.Double, item.SGSTPercent);
                    dbServer.AddInParameter(command1, "SGSTAmount", DbType.Double, item.SGSTAmount);

                    dbServer.AddInParameter(command1, "CGSTPercent", DbType.Double, item.CGSTPercent);
                    dbServer.AddInParameter(command1, "CGSTAmount", DbType.Double, item.CGSTAmount);

                    dbServer.AddInParameter(command1, "TotalAmount", DbType.Double, item.Amount);
                    dbServer.AddInParameter(command1, "NetAmount", DbType.Double, item.NetAmount);
                    dbServer.AddInParameter(command1, "Status", DbType.Boolean, objDetailsVO.Status);
                    dbServer.AddInParameter(command1, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "AddUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    dbServer.AddInParameter(command1, "By", DbType.Int64, UserVo.ID);
                    dbServer.AddInParameter(command1, "On", DbType.String, UserVo.UserLoginInfo.MachineName);
                    dbServer.AddInParameter(command1, "DateTime", DbType.DateTime, objDetailsVO.AddedDateTime);
                    dbServer.AddInParameter(command1, "WindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    //By Anjali............................
                    //dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.ReturnQuantity);
                    dbServer.AddInParameter(command1, "PendingQuantity", DbType.Double, item.BaseQuantity);
                    dbServer.AddInParameter(command1, "ItemVatType", DbType.Int32, item.ItemVatType); 
                    dbServer.AddInParameter(command1, "BaseQuantity", DbType.Single, item.BaseQuantity);                    // Base Quantity            // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseCF", DbType.Single, item.BaseConversionFactor);
                    dbServer.AddInParameter(command1, "TransactionUOMID", DbType.Int64, item.SelectedUOM.ID);               // Transaction UOM      // For Conversion Factor
                    dbServer.AddInParameter(command1, "BaseUMID", DbType.Int64, item.BaseUOMID);                            // Base  UOM            // For Conversion Factor
                    //dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SaleUOMID);     // Commented by Ashish Z. on Dated 16-09-2016                       // SUOM UOM                     // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockUOMID", DbType.Int64, item.SUOMID); // Added by Ashish Z. on Dated 16-09-2016    
                    dbServer.AddInParameter(command1, "StockCF", DbType.Single, item.ConversionFactor);                    // Stocking ConversionFactor     // For Conversion Factor
                    dbServer.AddInParameter(command1, "StockingQuantity", DbType.Single, item.ReturnQuantity * item.ConversionFactor);  // StockingQuantity // For Conversion Factor
                    //.....................................
                    dbServer.AddOutParameter(command1, "ResultStatus", DbType.Int32, int.MaxValue);
                    dbServer.AddOutParameter(command1, "ID", DbType.Int64, int.MaxValue);
                    int intStatus1 = dbServer.ExecuteNonQuery(command1, trans);
                    BizActionObj.SuccessStatus = (int)dbServer.GetParameterValue(command1, "ResultStatus");
                    DbCommand command2 = dbServer.GetStoredProcCommand("CIMS_UpdateItemSale");
                    command2.Connection = con;
                    dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                  //  By Anjali............................
                   // dbServer.AddInParameter(command2, "PendingQuantity", DbType.Decimal, item.ReturnQuantity);
                    dbServer.AddInParameter(command2, "PendingQuantity", DbType.Decimal, item.BaseQuantity);
                    //..............................................
                    dbServer.AddInParameter(command2, "SaleDetailsID", DbType.Int64, item.ID);
                    int status2 = dbServer.ExecuteNonQuery(command2, trans);

                    #region commented 22042017
                    //if (BizActionObj.ItemMatserDetail.RefundDetails != null)
                    //{
                    //    clsBaseRefundDAL objBaseRefundDAL = clsBaseRefundDAL.GetInstance();
                    //    clsAddRefundBizActionVO objRefund = new clsAddRefundBizActionVO();
                    //    objRefund.Details = new clsRefundVO();
                    //    objRefund.Details = BizActionObj.ItemMatserDetail.RefundDetails;
                    //    objRefund.Details.ItemSaleReturnID = BizActionObj.ItemSalesReturnID;

                    //    #region Refund to Advance 22042017
                    //    if (BizActionObj.IsRefundToAdvance == true)
                    //    {
                    //        objRefund.IsRefundToAdvance = BizActionObj.IsRefundToAdvance;
                    //        objRefund.RefundToAdvancePatientID = BizActionObj.RefundToAdvancePatientID;
                    //        objRefund.RefundToAdvancePatientUnitID = BizActionObj.RefundToAdvancePatientUnitID;
                    //    }
                    //    #endregion

                    //    objRefund = (clsAddRefundBizActionVO)objBaseRefundDAL.Add(objRefund, UserVo, con, trans);
                    //    if (objRefund.SuccessStatus == -5) throw new Exception();
                    //    BizActionObj.ItemMatserDetail.RefundDetails.ID = objRefund.Details.ID;
                    //}
                    #endregion

                    clsBaseItemStockDAL objBaseDAL = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO obj = new clsAddItemStockBizActionVO();
                    clsItemStockVO StockDetails = new clsItemStockVO();

                    StockDetails.ItemID = item.ItemID;
                    StockDetails.BatchID = item.BatchID;
                    StockDetails.TransactionTypeID = InventoryTransactionType.ItemSaleReturn;
                   ///StockDetails.TransactionQuantity = item.ReturnQuantity;
                    StockDetails.TransactionID = BizActionObj.ItemSalesReturnID;
                    StockDetails.Date = BizActionObj.ItemMatserDetail.Date;
                    StockDetails.Time = BizActionObj.ItemMatserDetail.Time;
                    StockDetails.OperationType = InventoryStockOperationType.Addition;
                    StockDetails.StoreID = BizActionObj.ItemMatserDetail.StoreID;

                    //By Anjali...............
                    StockDetails.InputTransactionQuantity = Convert.ToSingle(item.ReturnQuantity);  // InputTransactionQuantity
                    StockDetails.BaseUOMID = item.BaseUOMID;                        // Base  UOM
                    StockDetails.BaseConversionFactor = item.BaseConversionFactor;  // Base Conversion Factor  
                    StockDetails.TransactionQuantity = item.BaseQuantity;         // Base Quantity 
                    StockDetails.SUOMID = item.SUOMID;                           // SUOM UOM 
                    StockDetails.ConversionFactor = item.ConversionFactor;     //Convert.ToDouble(item.StockCF);// (item.StockCF * item.ConversionFactor);  By Umesh StockCF
                    StockDetails.StockingQuantity = item.ReturnQuantity * item.ConversionFactor;   //item.BaseQuantity * item.ConversionFactor;// item.ReturnQuantity * (item.StockCF * item.ConversionFactor); By Umesh
                    StockDetails.SelectedUOM.ID = item.SelectedUOM.ID;          // Transaction UOM      // For Conversion Factor     

                    //..............................
         
                    obj.Details = StockDetails;

                    obj = (clsAddItemStockBizActionVO)objBaseDAL.Add(obj, UserVo, con, trans);

                    if (obj.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }

                    StockDetails.ID = obj.Details.ID;

                }

                if (BizActionObj.ItemMatserDetail.RefundDetails != null)       // add on 22042017
                {
                    clsBaseRefundDAL objBaseRefundDAL = clsBaseRefundDAL.GetInstance();
                    clsAddRefundBizActionVO objRefund = new clsAddRefundBizActionVO();
                    objRefund.Details = new clsRefundVO();
                    objRefund.Details = BizActionObj.ItemMatserDetail.RefundDetails;
                    objRefund.Details.ItemSaleReturnID = BizActionObj.ItemSalesReturnID;

                    objRefund.Details.PaymentDetails.CostingDivisionID = BizActionObj.ItemMatserDetail.CostingDivisionID;       // Refund To Advance 22042017s

                    #region Refund to Advance 22042017
                    if (BizActionObj.IsRefundToAdvance == true)
                    {
                        objRefund.IsRefundToAdvance = BizActionObj.IsRefundToAdvance;
                        objRefund.RefundToAdvancePatientID = BizActionObj.RefundToAdvancePatientID;
                        objRefund.RefundToAdvancePatientUnitID = BizActionObj.RefundToAdvancePatientUnitID;

                        objRefund.IsExchangeMaterial = BizActionObj.IsExchangeMaterial;
                    }
                    #endregion

                    objRefund = (clsAddRefundBizActionVO)objBaseRefundDAL.Add(objRefund, UserVo, con, trans);
                    if (objRefund.SuccessStatus == -5) throw new Exception();
                    BizActionObj.ItemMatserDetail.RefundDetails.ID = objRefund.Details.ID;
                }

                trans.Commit();

                // By Rohinee for activity log==============================================
                if( IsAuditTrail == true)
                {
                    if (BizActionObj.LogInfoList != null)
                    {
                        if (BizActionObj.ItemsDetail.Count > 0 )  //for item return id
                        {
                            LogInfo LogInformation = new LogInfo();
                            LogInformation.guid = new Guid();
                            LogInformation.UserId = UserVo.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 107 : T_ItemSaleReturnDetails To Get  Item Sale Return Id " //+ Convert.ToString(lineNumber)
                                                   + "Unit Id : " + Convert.ToString(UserVo.UserLoginInfo.UnitId) + " "
                                                    + " ,Item Sales Return ID:" + Convert.ToString(BizActionObj.ItemSalesReturnID)
                                                    + " , Item Sale Return No : " + Convert.ToString(BizActionObj.ItemMatserDetail.ItemSaleReturnNo);
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
                BizActionObj.SuccessStatus = -1;
                logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), ex.ToString());
                BizActionObj.ItemMatserDetail = null;
                BizActionObj.ItemsDetail = null;
                BizActionObj.SuccessStatus = -1;
            }
            finally
            {
                con.Close();
                trans = null;
            }

            return BizActionObj;
        }
        //by Rohinee Dated 26/9/16
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

        public override IValueObject GetItemSaleReturn(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesReturnBizActionVO objItem = valueObject as clsGetItemSalesReturnBizActionVO;
            objItem.Details = new List<clsItemSalesReturnVO>();
            clsItemSalesReturnVO objItemVO = null;


            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSalesReturn");
                dbServer.AddInParameter(command, "FromDate", DbType.DateTime, objItem.FromDate);
                dbServer.AddInParameter(command, "ToDate", DbType.DateTime, objItem.ToDate);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, objItem.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, objItem.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, objItem.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                dbServer.AddInParameter(command, "SearchExpression", DbType.String, objItem.SerachExpression);


                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemSalesReturnVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]));
                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItemVO.ItemSaleReturnNo = Convert.ToString(DALHelper.HandleDBNull(reader["SalesReturnNo"]));
                        objItemVO.ItemSalesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"]));
                        objItemVO.Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"]));
                        objItemVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objItemVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));
                        objItemVO.VATAmount = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATAmount"]));
                        objItemVO.VATPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalNetAmount"]));
                        objItemVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.TotalReturnAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalReturnAmount"]));
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItem.Details.Add(objItemVO);
                    }
                }
                reader.NextResult();
                objItem.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
                        reader.Close();
                }
            }
            return objItem;
        }

        public override IValueObject GetItemSaleReturnDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;

            clsGetItemSalesReturnDetailsBizActionVO bizActionVO = valueObject as clsGetItemSalesReturnDetailsBizActionVO;
            bizActionVO.Details = new List<clsItemSalesReturnDetailsVO>();
            //clsGetItemSales
            clsItemSalesReturnDetailsVO objItemVO = null;
            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSalesReturnDetails");
                dbServer.AddInParameter(command, "ID", DbType.Int64, bizActionVO.ItemSalesReturnID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.UnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemSalesReturnDetailsVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.ItemSaleReturnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleReturnId"]));
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.ReturnQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReturnedQuantity"]));
                        objItemVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"]));
                        objItemVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItemVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));
                        objItemVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objItemVO.VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"]));

                        objItemVO.TotalSalesAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));

                        //By Anjali.......................
                        objItemVO.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["ItemVatType"]));
                       // objItemVO.StockUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        //objItemVO.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        objItemVO.BaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        objItemVO.SellingUOM = Convert.ToString(DALHelper.HandleDBNull(reader["StockUOM"]));
                        objItemVO.SelectedUOM.Description = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        objItemVO.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        //objItemVO.BaseCF = Convert.ToDouble(DALHelper.HandleDBNull(reader["BaseCF"]));
                       // objItemVO.StockCF = Convert.ToString(DALHelper.HandleDBNull(reader["StockCF"]));
                       // objItemVO.StockingQuantity = Convert.ToString(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        //................................

                        bizActionVO.Details.Add(objItemVO);
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
                        reader.Close();
                }
            }
            return bizActionVO;
        }

        public override IValueObject GetItemSaleComplete(IValueObject valueObject, clsUserVO UserVo)
        {
            // throw new NotImplementedException();
            DbDataReader reader = null;

            clsGetItemSalesCompleteBizActionVO bizActionVO = valueObject as clsGetItemSalesCompleteBizActionVO;

            try
            {
                DbCommand command;
             //   command = dbServer.GetStoredProcCommand("CIMS_GetItemSalesComplete");
                command = dbServer.GetStoredProcCommand("CIMS_PharmacyChargeList");
                dbServer.AddInParameter(command, "BillID", DbType.Int64, bizActionVO.BillID);
                dbServer.AddInParameter(command, "UnitId", DbType.Int64, bizActionVO.BillUnitID);
                dbServer.AddInParameter(command, "IsBilled", DbType.Boolean, bizActionVO.IsBilled);
                dbServer.AddInParameter(command, "AdmID", DbType.Int64, bizActionVO.AdmID);
                dbServer.AddInParameter(command, "AdmissionUnitID", DbType.Int64, bizActionVO.AdmissionUnitID);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    bizActionVO.Details = new clsItemSalesVO();

                    while (reader.Read())
                    {
                        bizActionVO.Details.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        bizActionVO.Details.UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        bizActionVO.Details.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        //bizActionVO.Details.Time = Convert.ToDateTime(DALHelper.HandleDate(reader["Time"]));
                        bizActionVO.Details.PatientID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientID"]));
                        bizActionVO.Details.VisitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VisitID"]));
                       // bizActionVO.Details.ItemSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ItemSaleNo"]));                       
                        bizActionVO.Details.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        bizActionVO.Details.IsBilled = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBilled"]));

                        clsItemSalesDetailsVO objItemVO = new clsItemSalesDetailsVO();

                        objItemVO.IsBilled = bizActionVO.Details.IsBilled;
                        objItemVO.ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"]));
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItemVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objItemVO.MRPBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItemVO.AmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItemVO.NetAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.NetBillAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetBillAmount"]));
                        objItemVO.ConcessionPercentageBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));                       
                        objItemVO.ConcessionAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));
                        objItemVO.VATPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.VATAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"]));
                        objItemVO.ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]));
                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]));
                        //objItemVO.AvailableQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        objItemVO.SGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTPercentage"]));
                        objItemVO.SGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["SGSTAmount"]));
                        objItemVO.CGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTPercentage"]));
                        objItemVO.CGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["CGSTAmount"]));
                        objItemVO.IGSTPercentBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTPercentage"]));
                        objItemVO.IGSTAmountBill = Convert.ToDouble(DALHelper.HandleDBNull(reader["IGSTAmount"]));
                        bizActionVO.Details.Items.Add(objItemVO);
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
                        reader.Close();
                }
            }
            return bizActionVO;
        }

        #region Cash Settlement

        public override IValueObject GetItemSaleDetailsForCashSettlement(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSalesDetailsForCashSettlementBizActionVO bizActionVO = valueObject as clsGetItemSalesDetailsForCashSettlementBizActionVO;
            bizActionVO.Details = new List<clsItemSalesDetailsVO>();
            clsItemSalesDetailsVO objItemVO = null;

            try
            {
                DbCommand command;
                command = dbServer.GetStoredProcCommand("CIMS_GetItemSalesDetailsForCashSettlement");
                dbServer.AddInParameter(command, "BillID", DbType.Int64, bizActionVO.BillID);
                dbServer.AddInParameter(command, "VisitID", DbType.Int64, bizActionVO.VisitID);
                dbServer.AddInParameter(command, "UnitID", DbType.Int64, bizActionVO.UnitID);
                dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, bizActionVO.PagingEnabled);
                dbServer.AddInParameter(command, "startRowIndex", DbType.Int64, bizActionVO.StartRowIndex);
                dbServer.AddInParameter(command, "maximumRows", DbType.Int64, bizActionVO.MaximumRows);
                dbServer.AddOutParameter(command, "TotalRows", DbType.Int64, int.MaxValue);
                reader = (DbDataReader)dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objItemVO = new clsItemSalesDetailsVO();
                        objItemVO.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        objItemVO.ItemSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemSaleId"]));
                        objItemVO.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"]));
                        objItemVO.Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]));
                        objItemVO.ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]));
                        objItemVO.BatchID = Convert.ToInt32(DALHelper.HandleDBNull(reader["BatchId"]));
                        objItemVO.Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"]));
                        objItemVO.NetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.SettleNetAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"]));
                        objItemVO.PendingQuantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["PendingQuantity"]));
                        objItemVO.MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"]));
                        objItemVO.Amount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalAmount"]));
                        objItemVO.ConcessionPercentage = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionPercentage"]));
                        objItemVO.ConcessionAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["ConcessionAmount"]));

                        objItemVO.DiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountPerc"]));
                        objItemVO.DiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountAmt"]));
                        objItemVO.DeductiblePerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeductionPerc"]));
                        objItemVO.DeductibleAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["DeductionAmt"]));
                        objItemVO.CompanyDiscountPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyDiscountPerc"]));
                        objItemVO.CompanyDiscountAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyDiscountAmt"]));
                        objItemVO.VATPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        objItemVO.VATAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATAmount"]));
                        objItemVO.ItemPaidAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["PatientPaidAmount"]));
                        objItemVO.CompanyPaidAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["CompanyPaidAmt"]));
                        objItemVO.IsCashTariff = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCashTariff"]));
                        objItemVO.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"]));
                        objItemVO.BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"]));
                        objItemVO.ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]));
                        bizActionVO.Details.Add(objItemVO);
                    }
                }
                reader.NextResult();
                bizActionVO.TotalRows = (long)dbServer.GetParameterValue(command, "TotalRows");
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
                        reader.Close();
                }
            }
            return bizActionVO;
        }

        #endregion
    }
}
