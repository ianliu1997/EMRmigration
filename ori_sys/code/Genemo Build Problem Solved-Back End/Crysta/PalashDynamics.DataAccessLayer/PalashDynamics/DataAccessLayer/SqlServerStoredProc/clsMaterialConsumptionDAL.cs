namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Billing;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Billing;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Inventory.MaterialConsumption;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class clsMaterialConsumptionDAL : clsBaseMaterialConsumptionDAL
    {
        private Database dbServer;
        private LogManager logManager;

        private clsMaterialConsumptionDAL()
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

        public override IValueObject Add(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddMaterialConsumptionBizActionVO nvo = valueObject as clsAddMaterialConsumptionBizActionVO;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                clsMaterialConsumptionVO consumptionDetails = nvo.ConsumptionDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddMaterialConsumption");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, consumptionDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, consumptionDetails.PackageID);
                this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, consumptionDetails.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, consumptionDetails.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatient", DbType.Int64, consumptionDetails.IsAgainstPatient);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, consumptionDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, consumptionDetails.Date);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, consumptionDetails.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, consumptionDetails.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "TotalItems", DbType.Decimal, consumptionDetails.TotalItems);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_Id", DbType.Int64, consumptionDetails.Opd_Ipd_External_Id);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External_UnitId", DbType.Int64, consumptionDetails.Opd_Ipd_External_UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Opd_Ipd_External", DbType.Int64, consumptionDetails.Opd_Ipd_External);
                this.dbServer.AddInParameter(storedProcCommand, "TotalMRPAmount", DbType.Double, consumptionDetails.TotalMRPAmount);
                this.dbServer.AddInParameter(storedProcCommand, "LinkPatientID", DbType.Int64, consumptionDetails.LinkPatientID);
                this.dbServer.AddInParameter(storedProcCommand, "LinkPatientUnitID", DbType.Int64, consumptionDetails.LinkPatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsAgainstPatientIndent", DbType.Boolean, consumptionDetails.IsAgainstPatientIndent);
                this.dbServer.AddInParameter(storedProcCommand, "IsPackageConsumable", DbType.Boolean, consumptionDetails.IsPackageConsumable);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, userVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, userVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now.Date);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, userVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ConsumptionDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                bool flag = false;
                foreach (clsMaterialConsumptionItemDetailsVO svo in nvo.ConsumptionDetails.ConsumptionItemDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddMaerialConsumptionDetails");
                    command2.Connection = myConnection;
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "ConsumptionID", DbType.Int64, nvo.ConsumptionDetails.ID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, svo.BatchId);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command2, "UsedQty", DbType.Decimal, svo.UsedQty);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Decimal, svo.Rate);
                    this.dbServer.AddInParameter(command2, "Amount", DbType.Decimal, svo.Amount);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                    this.dbServer.AddInParameter(command2, "BatchCode", DbType.String, svo.BatchCode);
                    this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.Date, svo.ExpiryDate);
                    this.dbServer.AddInParameter(command2, "ItemName", DbType.String, svo.ItemName);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Decimal, svo.MRP);
                    this.dbServer.AddInParameter(command2, "Opd_Ipd_External_Id", DbType.Int64, consumptionDetails.Opd_Ipd_External_Id);
                    this.dbServer.AddInParameter(command2, "Opd_Ipd_External_UnitId", DbType.Int64, consumptionDetails.Opd_Ipd_External_UnitId);
                    this.dbServer.AddInParameter(command2, "PackageID", DbType.Int64, consumptionDetails.PackageID);
                    this.dbServer.AddInParameter(command2, "PackageBillID", DbType.Int64, consumptionDetails.PackageBillID);
                    this.dbServer.AddInParameter(command2, "PackageBillUnitID", DbType.Int64, consumptionDetails.PackageBillUnitID);
                    this.dbServer.AddInParameter(command2, "PatientID", DbType.Int64, consumptionDetails.PatientId);
                    this.dbServer.AddInParameter(command2, "PatientUnitID", DbType.Int64, consumptionDetails.PatientUnitID);
                    this.dbServer.AddInParameter(command2, "TotalPatientIndentReceiveQty", DbType.Decimal, svo.TotalPatientIndentReceiveQty - svo.TotalPatientIndentConsumptionQty);
                    if (consumptionDetails.IsPackageConsumable && !consumptionDetails.ApplicableToAll)
                    {
                        this.dbServer.AddInParameter(command2, "PackageConsumptionTypeID", DbType.Int64, 1);
                    }
                    else if (consumptionDetails.IsAgainstPatientIndent && consumptionDetails.ApplicableToAll)
                    {
                        this.dbServer.AddInParameter(command2, "PackageConsumptionTypeID", DbType.Int64, 2);
                    }
                    else if (!consumptionDetails.IsAgainstPatientIndent && (!consumptionDetails.IsPackageConsumable && consumptionDetails.ApplicableToAll))
                    {
                        this.dbServer.AddInParameter(command2, "PackageConsumptionTypeID", DbType.Int64, 3);
                    }
                    this.dbServer.AddOutParameter(command2, "ConsumptionAmount", DbType.Double, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "PreviousConsumptionAmount", DbType.Double, 0x7fffffff);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Decimal, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockingOty", DbType.Decimal, svo.StockOty);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Decimal, svo.ConversionFactor);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    nvo.ConsumptionDetails.ConsumptionAmount = Convert.ToDouble(this.dbServer.GetParameterValue(command2, "ConsumptionAmount"));
                    if (!flag)
                    {
                        flag = true;
                        nvo.ConsumptionDetails.PreviousConsumptionAmount = Convert.ToDouble(this.dbServer.GetParameterValue(command2, "PreviousConsumptionAmount"));
                    }
                    svo.StockDetails.BatchID = svo.BatchId;
                    svo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                    svo.StockDetails.ItemID = svo.ItemId;
                    svo.StockDetails.TransactionTypeID = InventoryTransactionType.MaterialConsumption;
                    svo.StockDetails.TransactionID = nvo.ConsumptionDetails.ID;
                    svo.StockDetails.TransactionQuantity = svo.BaseOty;
                    svo.StockDetails.BaseUOMID = svo.BaseUOMID;
                    svo.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                    svo.StockDetails.SUOMID = svo.SUOMID;
                    svo.StockDetails.ConversionFactor = svo.ConversionFactor;
                    svo.StockDetails.SelectedUOM = svo.SelectedUOM;
                    svo.StockDetails.StockingQuantity = (double) svo.StockOty;
                    svo.StockDetails.InputTransactionQuantity = (float) svo.UsedQty;
                    svo.StockDetails.Date = (DALHelper.HandleDBNull(consumptionDetails.Date) != null) ? Convert.ToDateTime(consumptionDetails.Date) : DateTime.Now;
                    svo.StockDetails.Time = (DALHelper.HandleDBNull(consumptionDetails.Date) != null) ? Convert.ToDateTime(consumptionDetails.Time) : DateTime.Now;
                    svo.StockDetails.StoreID = new long?(consumptionDetails.StoreID);
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo3 = new clsAddItemStockBizActionVO {
                        Details = svo.StockDetails
                    };
                    nvo3.Details.ID = 0L;
                    nvo3 = (clsAddItemStockBizActionVO) instance.Add(nvo3, userVO, myConnection, transaction);
                    if (nvo3.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    svo.StockDetails.ID = nvo3.Details.ID;
                }
                if (((consumptionDetails != null) && ((((consumptionDetails.Opd_Ipd_External == 0L) && (consumptionDetails.IsPackageConsumable || consumptionDetails.IsAgainstPatientIndent)) || consumptionDetails.ApplicableToAll) || (consumptionDetails.Opd_Ipd_External == 1L))) && ((consumptionDetails.Opd_Ipd_External == 1L) || (consumptionDetails.Opd_Ipd_External == 0L)))
                {
                    clsBaseBillDAL instance = clsBaseBillDAL.GetInstance();
                    clsAddBillBizActionVO objBillDetails = new clsAddBillBizActionVO();
                    if (consumptionDetails.PackageID <= 0L)
                    {
                        if ((consumptionDetails.Opd_Ipd_External == 1L) && (((clsAddBillBizActionVO) instance.Add(nvo.ObjBillDetails, userVO, myConnection, transaction)).SuccessStatus == -1))
                        {
                            throw new Exception();
                        }
                    }
                    else
                    {
                        double pharmacyFixedRate = 0.0;
                        if (consumptionDetails.Opd_Ipd_External != 0L)
                        {
                            if (consumptionDetails.Opd_Ipd_External == 1L)
                            {
                                pharmacyFixedRate = consumptionDetails.PharmacyFixedRate;
                            }
                        }
                        else if (consumptionDetails.IsAgainstPatientIndent)
                        {
                            pharmacyFixedRate = consumptionDetails.PharmacyFixedRate;
                        }
                        else if (consumptionDetails.IsPackageConsumable)
                        {
                            pharmacyFixedRate = consumptionDetails.PackageConsumableLimit;
                        }
                        else if (!consumptionDetails.ApplicableToAll && (!consumptionDetails.IsPackageConsumable && !consumptionDetails.IsAgainstPatientIndent))
                        {
                            pharmacyFixedRate = consumptionDetails.PharmacyFixedRate;
                        }
                        else if (consumptionDetails.ApplicableToAll)
                        {
                            pharmacyFixedRate = consumptionDetails.PharmacyFixedRate;
                        }
                        if ((((((consumptionDetails.Opd_Ipd_External == 0L) && consumptionDetails.IsAgainstPatientIndent) || consumptionDetails.ApplicableToAll) || (consumptionDetails.Opd_Ipd_External == 1L)) && (nvo.ConsumptionDetails.ConsumptionAmount > 0.0)) && (pharmacyFixedRate > 0.0))
                        {
                            if (nvo.ConsumptionDetails.PreviousConsumptionAmount == 0.0)
                            {
                                objBillDetails = nvo.ObjBillDetails;
                                double num2 = 0.0;
                                double num4 = pharmacyFixedRate;
                                foreach (clsItemSalesDetailsVO svo2 in objBillDetails.Details.PharmacyItems.Items)
                                {
                                    double num5 = svo2.MRP * svo2.Quantity;
                                    if (num4 <= 0.0)
                                    {
                                        svo2.PackageConcessionAmount = (num5 / 100.0) * svo2.DiscountOnPackageItem;
                                        svo2.PackageConcessionPercentage = svo2.DiscountOnPackageItem;
                                        continue;
                                    }
                                    if (nvo.ObjBillDetails.Details.PharmacyItems.Items.Count<clsItemSalesDetailsVO>() <= 1)
                                    {
                                        if (num5 <= num4)
                                        {
                                            svo2.PackageConcessionAmount = num5;
                                            num4 -= num5;
                                            continue;
                                        }
                                        double num7 = 0.0;
                                        svo2.PackageConcessionAmount = num4;
                                        num7 = ((num5 - num4) / 100.0) * svo2.DiscountOnPackageItem;
                                        svo2.PackageConcessionAmount += num7;
                                        continue;
                                    }
                                    if (num5 <= num4)
                                    {
                                        svo2.PackageConcessionAmount = num5;
                                        num4 -= num5;
                                        continue;
                                    }
                                    double num6 = 0.0;
                                    svo2.PackageConcessionAmount = num4;
                                    num6 = ((num5 - num4) / 100.0) * svo2.DiscountOnPackageItem;
                                    svo2.PackageConcessionAmount += num6;
                                    num4 = 0.0;
                                }
                                double num10 = nvo.ObjBillDetails.Details.NetBillAmount - num2;
                                objBillDetails.Details.NetBillAmount = num10;
                                objBillDetails.Details.SelfAmount = num10;
                                objBillDetails.Details.BalanceAmountSelf = num10;
                                objBillDetails.Details.TotalConcessionAmount = num2;
                            }
                            else if (nvo.ConsumptionDetails.PreviousConsumptionAmount >= pharmacyFixedRate)
                            {
                                objBillDetails = nvo.ObjBillDetails;
                                double num20 = 0.0;
                                foreach (clsItemSalesDetailsVO svo4 in objBillDetails.Details.PharmacyItems.Items)
                                {
                                    double num21 = svo4.MRP * svo4.Quantity;
                                    svo4.ConcessionAmount = (num21 / 100.0) * svo4.DiscountOnPackageItem;
                                    svo4.ConcessionPercentage = svo4.DiscountOnPackageItem;
                                    num20 += svo4.ConcessionAmount;
                                }
                                objBillDetails.Details.TotalConcessionAmount += num20;
                                objBillDetails.Details.PharmacyItems.ConcessionAmount = objBillDetails.Details.TotalConcessionAmount + num20;
                                double num24 = nvo.ObjBillDetails.Details.TotalBillAmount - objBillDetails.Details.TotalConcessionAmount;
                                objBillDetails.Details.NetBillAmount = num24;
                                objBillDetails.Details.SelfAmount = num24;
                                objBillDetails.Details.BalanceAmountSelf = num24;
                            }
                            else
                            {
                                objBillDetails = nvo.ObjBillDetails;
                                double num11 = 0.0;
                                double num13 = pharmacyFixedRate - nvo.ConsumptionDetails.PreviousConsumptionAmount;
                                foreach (clsItemSalesDetailsVO svo3 in objBillDetails.Details.PharmacyItems.Items)
                                {
                                    double num14 = svo3.MRP * svo3.Quantity;
                                    if (num13 <= 0.0)
                                    {
                                        svo3.PackageConcessionAmount = (num14 / 100.0) * svo3.DiscountOnPackageItem;
                                        svo3.PackageConcessionPercentage = svo3.DiscountOnPackageItem;
                                        continue;
                                    }
                                    if (nvo.ObjBillDetails.Details.PharmacyItems.Items.Count<clsItemSalesDetailsVO>() <= 1)
                                    {
                                        if (num14 <= num13)
                                        {
                                            svo3.PackageConcessionAmount = num14;
                                            num13 -= num14;
                                            continue;
                                        }
                                        double num16 = 0.0;
                                        svo3.PackageConcessionAmount = num13;
                                        num16 = ((num14 - num13) / 100.0) * svo3.DiscountOnPackageItem;
                                        svo3.PackageConcessionAmount += num16;
                                        continue;
                                    }
                                    if (num14 <= num13)
                                    {
                                        svo3.PackageConcessionAmount = num14;
                                        num13 -= num14;
                                        continue;
                                    }
                                    double num15 = 0.0;
                                    svo3.PackageConcessionAmount = num13;
                                    num15 = ((num14 - num13) / 100.0) * svo3.DiscountOnPackageItem;
                                    svo3.PackageConcessionAmount += num15;
                                    num13 = 0.0;
                                }
                                objBillDetails.Details.TotalConcessionAmount = num11;
                                double num19 = nvo.ObjBillDetails.Details.NetBillAmount - num11;
                                objBillDetails.Details.NetBillAmount = num19;
                                objBillDetails.Details.SelfAmount = num19;
                                objBillDetails.Details.BalanceAmountSelf = num19;
                            }
                            objBillDetails = (clsAddBillBizActionVO) instance.Add(objBillDetails, userVO, myConnection, transaction);
                            if (objBillDetails.SuccessStatus == -1)
                            {
                                throw new Exception();
                            }
                            DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateMaterialConsumptionBillID");
                            command3.Connection = myConnection;
                            this.dbServer.AddInParameter(command3, "MaterialConsumptionID", DbType.Int64, nvo.ConsumptionDetails.ID);
                            this.dbServer.AddInParameter(command3, "MaterialConsumptionUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.AddInParameter(command3, "BillID", DbType.Int64, objBillDetails.Details.ID);
                            this.dbServer.AddInParameter(command3, "BillUnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                            this.dbServer.ExecuteNonQuery(command3, transaction);
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.ConsumptionDetails = null;
                nvo.SuccessStatus = -1;
                throw;
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

        public override IValueObject GetMaterialConsumptionItemList(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMatarialConsumptionItemListBizActionVO nvo = valueObject as clsGetMatarialConsumptionItemListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ GetMaterialConsumptionItemList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ConsumptionID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsMaterialConsumptionItemDetailsVO item = new clsMaterialConsumptionItemDetailsVO {
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]),
                            ConsumptionID = (long) DALHelper.HandleDBNull(reader["ConsumptionID"]),
                            ItemId = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            BatchId = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                            BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                            UsedQty = (decimal) DALHelper.HandleDBNull(reader["UsedQty"]),
                            Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            IsAgainstPatientIndent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsAgainstPatientIndent"]))
                        };
                        item.Amount = !(item.UsedQty == 1M) ? (!item.IsAgainstPatientIndent ? (item.Rate * item.UsedQty) : (Convert.ToDecimal(item.MRP) * item.UsedQty)) : (!item.IsAgainstPatientIndent ? Convert.ToDecimal(DALHelper.HandleDBNull(reader["Rate"])) : Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])));
                        item.Remark = (string) DALHelper.HandleDBNull(reader["Remark"]);
                        item.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        item.ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]);
                        item.TransactionUOM = (string) DALHelper.HandleDBNull(reader["Description"]);
                        item.Flag = true;
                        nvo.ItemList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetMaterialConsumptionList(IValueObject valueObject, clsUserVO UserVo)
        {
            DbDataReader reader = null;
            clsGetMatarialConsumptionListBizActionVO nvo = valueObject as clsGetMatarialConsumptionListBizActionVO;
            clsMaterialConsumptionVO item = null;
            try
            {
                if (!nvo.IsPatientAgainstMaterialConsumption)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMaterialConsumption");
                    this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreId);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsMaterialConsumptionVO {
                                Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                                TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"])),
                                ConsumptionNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConsumptionNo"])),
                                Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                                TotalItems = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalItems"])),
                                PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"])),
                                BillNo = Convert.ToString(DALHelper.HandleDBNull(reader["BillNo"])),
                                BillDate = DALHelper.HandleDate(reader["BillDate"]),
                                TotalBillAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                                PatientName = Convert.ToString(DALHelper.HandleDBNull(reader["PatientName"])),
                                Opd_Ipd_External = Convert.ToInt64(DALHelper.HandleDBNull(reader["Opd_Ipd_External"])),
                                PackageName = Convert.ToString(DALHelper.HandleDBNull(reader["PackageName"]))
                            };
                            nvo.ConsumptionList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientAgainstMaterialConsumption");
                    this.dbServer.AddInParameter(storedProcCommand, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "AdmID", DbType.Int64, nvo.AdmID);
                    this.dbServer.AddInParameter(storedProcCommand, "AdmissionUnitID", DbType.Int64, nvo.AdmissionUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new clsMaterialConsumptionVO {
                                Date = Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"])),
                                ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                                StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                                TotalAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalAmount"])),
                                ConsumptionNo = Convert.ToString(DALHelper.HandleDBNull(reader["ConsumptionNo"])),
                                Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                                PatientId = Convert.ToInt64(DALHelper.HandleDBNull(reader["PatientId"])),
                                TotalBillAmount = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBillAmount"])),
                                ItemId = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                                BatchId = (long) DALHelper.HandleDBNull(reader["BatchID"]),
                                BatchCode = (string) DALHelper.HandleDBNull(reader["BatchCode"]),
                                UsedQty = (decimal) DALHelper.HandleDBNull(reader["UsedQty"]),
                                Rate = (decimal) DALHelper.HandleDBNull(reader["Rate"]),
                                ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]),
                                ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                                TransactionUOM = (string) DALHelper.HandleDBNull(reader["Description"]),
                                MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                                PackageName = (string) DALHelper.HandleDBNull(reader["PackageName"])
                            };
                            nvo.ConsumptionList.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetPatientIndentReceiveStock(IValueObject valueObject, clsUserVO UserVo)
        {
            clsGetMatarialConsumptionItemListBizActionVO nvo = valueObject as clsGetMatarialConsumptionItemListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPatientIndentReceiveStockForMaterialConsm");
                this.dbServer.AddInParameter(storedProcCommand, "PatientId", DbType.Int64, nvo.objConsumptionVO.PatientId);
                this.dbServer.AddInParameter(storedProcCommand, "PatientUnitID", DbType.Int64, nvo.objConsumptionVO.PatientUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDs", DbType.String, nvo.objConsumptionVO.ItemIDs);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.objConsumptionVO.StoreID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsMaterialConsumptionItemDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        clsMaterialConsumptionItemDetailsVO item = new clsMaterialConsumptionItemDetailsVO {
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            TotalPatientIndentReceiveQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalPatientIndentReceiveQty"])),
                            TotalPatientIndentConsumptionQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalPatientIndentConsumptionQty"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }
    }
}

