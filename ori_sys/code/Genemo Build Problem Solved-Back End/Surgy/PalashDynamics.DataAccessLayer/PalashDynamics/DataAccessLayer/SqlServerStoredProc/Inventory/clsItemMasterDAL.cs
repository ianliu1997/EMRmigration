namespace PalashDynamics.DataAccessLayer.SqlServerStoredProc.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using com.seedhealthcare.hms.Web.Logging;
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using PalashDynamics.DataAccessLayer;
    using PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory;
    using PalashDynamics.ValueObjects;
    using PalashDynamics.ValueObjects.Administration;
    using PalashDynamics.ValueObjects.Inventory;
    using PalashDynamics.ValueObjects.Inventory.BarCode;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;

    public class clsItemMasterDAL : clsBaseItemMasterDAL
    {
        private Database dbServer;
        private LogManager logManager;
        private string IsApprovedDirect = string.Empty;

        private clsItemMasterDAL()
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
                this.IsApprovedDirect = ConfigurationManager.AppSettings["IsApprovedDirect"];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override IValueObject AddItemBarCodefromAssigned(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddAssignedItemBarcodeBizActionVO nvo = valueObject as clsAddAssignedItemBarcodeBizActionVO;
            nvo.ItemList = new List<clsItemMasterVO>();
            try
            {
                clsItemMasterVO rvo1 = new clsItemMasterVO();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemBarCodefromAssignedFrm");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, nvo.BatchCode);
                this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, nvo.BarCode);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject AddItemClinic(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO nvo = valueObject as clsAddItemClinicBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                DbCommand command2 = null;
                clsItemStoreVO itemStore = nvo.ItemStore;
                int num = 0;
                if (itemStore.DeletedStoreList.Count > 0)
                {
                    foreach (clsItemStoreVO evo2 in itemStore.DeletedStoreList)
                    {
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteItemClinic");
                        this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, evo2.UnitId);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, evo2.ItemID);
                        this.dbServer.AddInParameter(command2, "StoreId", DbType.Int64, evo2.StoreID);
                        this.dbServer.ExecuteNonQuery(command2);
                    }
                }
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemClinic");
                if (itemStore.StoreList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (itemStore.StoreList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddParameter(storedProcCommand, "Id", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, itemStore.StoreList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemStore.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, itemStore.ItemID);
                        this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, itemStore.StoreList[num2].StoreID);
                        this.dbServer.AddInParameter(storedProcCommand, "Min", DbType.Decimal, itemStore.StoreList[num2].Min);
                        this.dbServer.AddInParameter(storedProcCommand, "Max", DbType.Decimal, itemStore.StoreList[num2].Max);
                        this.dbServer.AddInParameter(storedProcCommand, "Re_Order", DbType.Decimal, itemStore.StoreList[num2].Re_Order);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, itemStore.StoreList[num2].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemStore.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemStore.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemStore.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemStore.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemStore.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemStore.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        nvo.ItemStoreDetails = new clsItemStoreVO();
                        nvo.ItemStoreDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                        num2++;
                    }
                }
                else if ((nvo.ItemTaxList == null) || (nvo.ItemTaxList.Count <= 0))
                {
                    nvo.SuccessStatus = 1;
                }
                else
                {
                    DbCommand command3 = null;
                    if ((nvo.ItemTaxList != null) && (nvo.ItemTaxList.Count > 0))
                    {
                        foreach (clsItemTaxVO xvo in nvo.ItemTaxList)
                        {
                            command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
                            this.dbServer.AddInParameter(command3, "Id", DbType.Int64, xvo.ID);
                            this.dbServer.AddInParameter(command3, "TaxID", DbType.Int64, xvo.TaxID);
                            this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, xvo.StoreID);
                            this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, itemStore.ItemID);
                            this.dbServer.AddInParameter(command3, "VATPercentage", DbType.Decimal, xvo.Percentage);
                            this.dbServer.AddInParameter(command3, "taxtype", DbType.Int32, xvo.TaxType);
                            this.dbServer.AddInParameter(command3, "ApplicableFor", DbType.Int32, xvo.ApplicableForId);
                            this.dbServer.AddInParameter(command3, "VATApplicableOn", DbType.Int32, xvo.ApplicableOnId);
                            this.dbServer.AddInParameter(command3, "ApplicableFromDate", DbType.DateTime, xvo.ApplicableFrom);
                            this.dbServer.AddInParameter(command3, "ApplicableToDate", DbType.DateTime, xvo.ApplicableTo);
                            this.dbServer.AddInParameter(command3, "IsGST", DbType.Boolean, xvo.IsGST);
                            this.dbServer.AddInParameter(command3, "Status", DbType.Boolean, xvo.status);
                            this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, itemStore.CreatedUnitID);
                            this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, itemStore.AddedBy);
                            this.dbServer.AddInParameter(command3, "AddedOn", DbType.String, itemStore.AddedOn);
                            this.dbServer.AddInParameter(command3, "AddedDateTime", DbType.DateTime, itemStore.AddedDateTime);
                            this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, itemStore.AddedWindowsLoginName);
                            this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command3);
                        }
                        nvo.SuccessStatus = 1;
                        nvo.ResultStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddItemMaster(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemMasterBizActionVO nvo = valueObject as clsAddItemMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                clsItemMasterVO itemMatserDetails = nvo.ItemMatserDetails;
                if (itemMatserDetails.EditMode)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateItemMaster");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, itemMatserDetails.ID);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_InsertItemMaster");
                    this.dbServer.AddOutParameter(storedProcCommand, "Id", DbType.Int64, 0);
                }
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, itemMatserDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BrandName", DbType.String, itemMatserDetails.BrandName);
                this.dbServer.AddInParameter(storedProcCommand, "Strength", DbType.String, itemMatserDetails.Strength);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, itemMatserDetails.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "MoleculeName", DbType.Int64, itemMatserDetails.MoleculeName);
                this.dbServer.AddInParameter(storedProcCommand, "ItemGroup", DbType.Int64, itemMatserDetails.ItemGroup);
                this.dbServer.AddInParameter(storedProcCommand, "ItemCategory", DbType.Int64, itemMatserDetails.ItemCategory);
                this.dbServer.AddInParameter(storedProcCommand, "DispencingType", DbType.Int64, itemMatserDetails.DispencingType);
                this.dbServer.AddInParameter(storedProcCommand, "StoreageType", DbType.Int64, itemMatserDetails.StoreageType);
                this.dbServer.AddInParameter(storedProcCommand, "PregClass", DbType.Int64, itemMatserDetails.PregClass);
                this.dbServer.AddInParameter(storedProcCommand, "TherClass", DbType.Int64, itemMatserDetails.TherClass);
                this.dbServer.AddInParameter(storedProcCommand, "MfgBy", DbType.Int64, itemMatserDetails.MfgBy);
                this.dbServer.AddInParameter(storedProcCommand, "MrkBy", DbType.Int64, itemMatserDetails.MrkBy);
                this.dbServer.AddInParameter(storedProcCommand, "PUM", DbType.Int64, itemMatserDetails.PUM);
                this.dbServer.AddInParameter(storedProcCommand, "SUM", DbType.Int64, itemMatserDetails.SUM);
                this.dbServer.AddInParameter(storedProcCommand, "ConversionFactor", DbType.String, itemMatserDetails.ConversionFactor);
                this.dbServer.AddInParameter(storedProcCommand, "BaseUM", DbType.Int64, itemMatserDetails.BaseUM);
                this.dbServer.AddInParameter(storedProcCommand, "SellingUM", DbType.Int64, itemMatserDetails.SellingUM);
                this.dbServer.AddInParameter(storedProcCommand, "ConvFactStockBase", DbType.String, itemMatserDetails.ConvFactStockBase);
                this.dbServer.AddInParameter(storedProcCommand, "ConvFactBaseSell", DbType.String, itemMatserDetails.ConvFactBaseSale);
                this.dbServer.AddInParameter(storedProcCommand, "ItemExpiredInDays", DbType.Int64, itemMatserDetails.ItemExpiredInDays);
                this.dbServer.AddInParameter(storedProcCommand, "Route", DbType.Int64, itemMatserDetails.Route);
                this.dbServer.AddInParameter(storedProcCommand, "PurchaseRate", DbType.Decimal, itemMatserDetails.PurchaseRate);
                this.dbServer.AddInParameter(storedProcCommand, "MRP", DbType.Decimal, itemMatserDetails.MRP);
                this.dbServer.AddInParameter(storedProcCommand, "VatPer", DbType.Decimal, itemMatserDetails.VatPer);
                this.dbServer.AddInParameter(storedProcCommand, "ReorderQnt", DbType.Int32, itemMatserDetails.ReorderQnt);
                this.dbServer.AddInParameter(storedProcCommand, "BatchesRequired", DbType.Boolean, itemMatserDetails.BatchesRequired);
                this.dbServer.AddInParameter(storedProcCommand, "InclusiveOfTax", DbType.Boolean, itemMatserDetails.InclusiveOfTax);
                this.dbServer.AddInParameter(storedProcCommand, "DiscountOnSale", DbType.Double, itemMatserDetails.DiscountOnSale);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, itemMatserDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemMatserDetails.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemMatserDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemMatserDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemMatserDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemMatserDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, itemMatserDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, itemMatserDetails.UpdateddOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, itemMatserDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemMatserDetails.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, itemMatserDetails.UpdateWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "DispencingTypeString", DbType.String, itemMatserDetails.DispencingTypeString);
                this.dbServer.AddInParameter(storedProcCommand, "StorageDegree", DbType.Decimal, itemMatserDetails.StorageDegree);
                this.dbServer.AddInParameter(storedProcCommand, "MinStock", DbType.Decimal, itemMatserDetails.MinStock);
                this.dbServer.AddInParameter(storedProcCommand, "ItemMarginID", DbType.Int64, itemMatserDetails.ItemMarginID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemMovementID", DbType.Int64, itemMatserDetails.ItemMovementID);
                this.dbServer.AddInParameter(storedProcCommand, "Margin", DbType.Decimal, itemMatserDetails.Margin);
                this.dbServer.AddInParameter(storedProcCommand, "HighestRetailPrice", DbType.Decimal, itemMatserDetails.HighestRetailPrice);
                this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, itemMatserDetails.BarCode);
                this.dbServer.AddInParameter(storedProcCommand, "IsABC", DbType.Boolean, itemMatserDetails.IsABC);
                this.dbServer.AddInParameter(storedProcCommand, "IsFNS", DbType.Boolean, itemMatserDetails.IsFNS);
                this.dbServer.AddInParameter(storedProcCommand, "IsVED", DbType.Boolean, itemMatserDetails.IsVED);
                this.dbServer.AddInParameter(storedProcCommand, "StrengthUnitTypeID", DbType.Int64, itemMatserDetails.StrengthUnitTypeID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.AddInParameter(storedProcCommand, "HSNCodesID", DbType.Int64, itemMatserDetails.HSNCodesID);
                this.dbServer.AddInParameter(storedProcCommand, "StaffDiscount", DbType.Double, itemMatserDetails.StaffDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "WalkinDiscount", DbType.Double, itemMatserDetails.WalkinDiscount);
                this.dbServer.AddInParameter(storedProcCommand, "RegisteredPatientsDiscount", DbType.Double, itemMatserDetails.RegisteredPatientsDiscount);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ItemID = (long) this.dbServer.GetParameterValue(storedProcCommand, "Id");
                nvo.ItemMatserDetails = this.GetItem(nvo.ItemID);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddItemSupplier(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemSupplierbizActionVO nvo = valueObject as clsAddItemSupplierbizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsItemSupllierVO itemSupplier = nvo.ItemSupplier;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemSupplier");
                if (nvo.ItemSupplierList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemSupplierList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, 0);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemSupplier.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, itemSupplier.ItemID);
                        this.dbServer.AddInParameter(storedProcCommand, "SuplierID", DbType.Int64, nvo.ItemSupplierList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "HPLavel", DbType.Int32, nvo.ItemSupplierList[num2].SelectedHPLevel.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemSupplierList[num2].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemSupplier.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemSupplier.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemSupplier.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemSupplier.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemSupplier.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemSupplier.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        int num3 = this.dbServer.ExecuteNonQuery(storedProcCommand);
                        if (num3 > 0)
                        {
                            num = 1;
                        }
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddItemTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemTaxBizActionVO nvo = valueObject as clsAddItemTaxBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsItemTaxVO itemTax = nvo.ItemTax;
                int num = 0;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemTax");
                clsUserVO rvo1 = new clsUserVO();
                if (nvo.ItemTaxList.Count > 0)
                {
                    int num2 = 0;
                    while (true)
                    {
                        if (num2 > (nvo.ItemTaxList.Count - 1))
                        {
                            nvo.SuccessStatus = num;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, itemTax.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemTax.UnitId);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, itemTax.ItemID);
                        this.dbServer.AddInParameter(storedProcCommand, "TaxID", DbType.Int64, nvo.ItemTaxList[num2].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Percentage", DbType.Decimal, nvo.ItemTaxList[num2].Percentage);
                        this.dbServer.AddInParameter(storedProcCommand, "ApplicableFor", DbType.Int32, nvo.ItemTaxList[num2].ApplicableFor.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "ApplicableOn", DbType.Int32, nvo.ItemTaxList[num2].ApplicableOn.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemTaxList[num2].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemTax.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemTax.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemTax.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemTax.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemTax.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemTax.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        num = 1;
                        num2++;
                    }
                }
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject AddMRPAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMRPAdjustmentBizActionVO nvo = valueObject as clsAddMRPAdjustmentBizActionVO;
            if (nvo.MRPAdjustmentMainVO == null)
            {
                nvo.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
            }
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddMRPAdjustmentMain");
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.MRPAdjustmentMainVO.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "MRPAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.MRPAdjustmentMainVO.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.MRPAdjustmentMainVO.MRPAdjustmentNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "MRPAdjustmentNo");
                foreach (clsMRPAdjustmentVO tvo in nvo.MRPAdjustmentItems)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddMRPAdjustmentDetails");
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "MRPAdjustmentMainID", DbType.Int64, nvo.MRPAdjustmentMainVO.ID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, tvo.ItemId);
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, tvo.BatchId);
                    this.dbServer.AddInParameter(command2, "PreviousMRP", DbType.Double, tvo.MRP);
                    this.dbServer.AddInParameter(command2, "UpdatedMRP", DbType.Double, tvo.UpdatedMRP);
                    this.dbServer.AddInParameter(command2, "UpdatedPurchaseRate", DbType.Double, tvo.UpdatedPurchaseRate);
                    this.dbServer.AddInParameter(command2, "PreviousPurchaseRate", DbType.Double, tvo.PurchaseRate);
                    this.dbServer.AddInParameter(command2, "PreviousBatchCode", DbType.String, tvo.BatchCode);
                    this.dbServer.AddInParameter(command2, "UpdatedBatchCode", DbType.String, tvo.UpdatedBatchCode);
                    this.dbServer.AddInParameter(command2, "PreExpiryDate", DbType.DateTime, tvo.ExpiryDate);
                    this.dbServer.AddInParameter(command2, "UpdatedExpiryDate", DbType.DateTime, tvo.UpdatedExpiryDate);
                    this.dbServer.AddInParameter(command2, "Remarks", DbType.String, tvo.Remarks);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    if (nvo.SuccessStatus == -2)
                    {
                        nvo.ItemName = tvo.ItemName;
                        throw new Exception();
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                if (nvo.SuccessStatus != -2)
                {
                    nvo.SuccessStatus = -1;
                }
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO userVO)
        {
            clsItemMasterVO itemMatserDetails = null;
            clsAddOpeningBalanceBizActionVO nvo = valueObject as clsAddOpeningBalanceBizActionVO;
            try
            {
                itemMatserDetails = nvo.ItemMatserDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddOpeningBalanceMaster");
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, itemMatserDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, itemMatserDetails.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, itemMatserDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, itemMatserDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, itemMatserDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, itemMatserDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, itemMatserDetails.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServerName", DbType.String, itemMatserDetails.LinkServerName);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServerAlias", DbType.String, itemMatserDetails.LinkServerAlias);
                this.dbServer.AddInParameter(storedProcCommand, "LinkServerDBName", DbType.String, itemMatserDetails.LinkServerDBName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, itemMatserDetails.AddedWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    itemMatserDetails.PrimaryKeyViolationError = true;
                }
                else
                {
                    itemMatserDetails.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject AddOpeningBalanceDetail(IValueObject valueObject, clsUserVO userVO)
        {
            return (valueObject as clsAddOpeningBalanceDetailBizActionVO);
        }

        public override IValueObject AddScrapSalesDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddScrapSalesDetailsBizActionVO nvo = valueObject as clsAddScrapSalesDetailsBizActionVO;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsSrcapVO itemMatserDetail = nvo.ItemMatserDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddtemScrapSale");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemMatserDetail.UnitId);
                userVO.UserLoginInfo.UnitId = itemMatserDetail.UnitId;
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, itemMatserDetail.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, itemMatserDetail.SupplierID);
                this.dbServer.AddParameter(storedProcCommand, "ScrapSaleNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, itemMatserDetail.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierName", DbType.String, itemMatserDetail.SupplierName);
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, itemMatserDetail.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, itemMatserDetail.Time);
                this.dbServer.AddInParameter(storedProcCommand, "TotalAmount", DbType.Double, itemMatserDetail.TotalAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalVatAmount", DbType.Double, itemMatserDetail.TotalVatAmount);
                this.dbServer.AddInParameter(storedProcCommand, "TotalTaxAmount", DbType.Double, itemMatserDetail.TotalTaxAmount);
                this.dbServer.AddInParameter(storedProcCommand, "NetAmount", DbType.Double, itemMatserDetail.TotalNetAmount);
                this.dbServer.AddInParameter(storedProcCommand, "ModeOfTransport", DbType.String, itemMatserDetail.ModeOfTransport);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, itemMatserDetail.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, itemMatserDetail.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, itemMatserDetail.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, itemMatserDetail.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, itemMatserDetail.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, itemMatserDetail.WindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "PaymentModeID", DbType.Int16, itemMatserDetail.PaymentModeID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ItemScrapSaleId = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ItemMatserDetail.ScrapSaleNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "ScrapSaleNo");
                foreach (clsSrcapDetailsVO svo in nvo.ItemsDetail)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddItemScrapSaleDetails");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, svo.ScrapSalesItemID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, itemMatserDetail.UnitId);
                    this.dbServer.AddInParameter(command2, "ItemScrapSaleId", DbType.Int64, nvo.ItemScrapSaleId);
                    this.dbServer.AddInParameter(command2, "ItemId", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, svo.BatchID);
                    this.dbServer.AddInParameter(command2, "ExpiryDate", DbType.DateTime, svo.BatchExpiryDate);
                    this.dbServer.AddInParameter(command2, "SaleQty", DbType.Double, svo.SaleQty);
                    this.dbServer.AddInParameter(command2, "Rate", DbType.Double, svo.Rate);
                    this.dbServer.AddInParameter(command2, "TotalPurchaseRate", DbType.Single, svo.TotalPurchaseRate);
                    this.dbServer.AddInParameter(command2, "MRP", DbType.Single, svo.MRP);
                    this.dbServer.AddInParameter(command2, "TotalAmount", DbType.Single, svo.MRPAmount);
                    this.dbServer.AddInParameter(command2, "TaxPercentage", DbType.Double, svo.ItemTax);
                    this.dbServer.AddInParameter(command2, "TaxAmount", DbType.Double, svo.TaxAmount);
                    this.dbServer.AddInParameter(command2, "VATPercentage", DbType.Double, svo.VATPerc);
                    this.dbServer.AddInParameter(command2, "VATAmount", DbType.Double, svo.VATAmount);
                    this.dbServer.AddInParameter(command2, "NetAmount", DbType.Double, svo.NetAmount);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Single, svo.SaleQty * svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, svo.BaseUOMID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, svo.SUOMID);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Single, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "othertaxApplicableon", DbType.Int16, svo.OtherGRNItemTaxApplicationOn);
                    this.dbServer.AddInParameter(command2, "otherTaxType", DbType.Int16, svo.OtherGRNItemTaxType);
                    this.dbServer.AddInParameter(command2, "VatApplicableon", DbType.Int16, svo.GRNItemVatApplicationOn);
                    this.dbServer.AddInParameter(command2, "Vattype", DbType.Int16, svo.GRNItemVatType);
                    this.dbServer.AddInParameter(command2, "ReceivedID", DbType.Int64, svo.ReceivedID);
                    this.dbServer.AddInParameter(command2, "ReceivedUnitID", DbType.Int64, svo.ReceivedUnitID);
                    this.dbServer.AddInParameter(command2, "ReceivedDetailID", DbType.Int64, svo.ReceivedDetailID);
                    this.dbServer.AddInParameter(command2, "ReceivedDetailUnitID", DbType.Int64, svo.ReceivedDetailUnitID);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                    this.dbServer.AddInParameter(command2, "Amount", DbType.Double, svo.TotalAmount);
                    this.dbServer.AddInParameter(command2, "ScrapRate", DbType.String, svo.ScrapRate);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    long receivedDetailID = svo.ReceivedDetailID;
                    if (svo.ReceivedDetailID > 0L)
                    {
                        DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdateReceive");
                        command3.Connection = connection;
                        this.dbServer.AddInParameter(command3, "ReceiveItemDetailsID", DbType.Int64, svo.ReceivedDetailID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, svo.ReceivedDetailUnitID);
                        this.dbServer.AddInParameter(command3, "BalanceQty", DbType.Decimal, svo.SaleQty * svo.BaseConversionFactor);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.ItemMatserDetail = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddStockAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddStockAdjustmentBizActionVO nvo = valueObject as clsAddStockAdjustmentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddItemStockAdjustmentMain");
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.objMainStock.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.objMainStock.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RequestDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.objMainStock.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "RequestedBy", DbType.Int64, nvo.objMainStock.RequestedBy);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, nvo.objMainStock.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromPST", DbType.Boolean, nvo.objMainStock.IsFromPST);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalItemID", DbType.Int64, nvo.objMainStock.PhysicalItemID);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalItemUnitID", DbType.Int64, nvo.objMainStock.PhysicalItemUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddParameter(storedProcCommand, "StockAdjustmentNo", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.objMainStock.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.objMainStock.UnitID = UserVo.UserLoginInfo.UnitId;
                nvo.objMainStock.StockAdjustmentNo = (string) this.dbServer.GetParameterValue(storedProcCommand, "StockAdjustmentNo");
                foreach (clsAdjustmentStockVO kvo in nvo.StockAdustmentItems)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddItemStockAdjustmentDetails");
                    if (kvo.RadioStatusNo)
                    {
                        kvo.OperationType = InventoryStockOperationType.Subtraction;
                    }
                    if (kvo.RadioStatusYes)
                    {
                        kvo.OperationType = InventoryStockOperationType.Addition;
                    }
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, kvo.ItemId);
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, kvo.BatchId);
                    this.dbServer.AddInParameter(command2, "StockAdjustmentID", DbType.Int64, nvo.objMainStock.ID);
                    this.dbServer.AddInParameter(command2, "StockAdjustmentUnitID", DbType.Int64, nvo.objMainStock.UnitID);
                    this.dbServer.AddInParameter(command2, "AdjustmentQuantity", DbType.Double, kvo.AdjustmentQunatitiy);
                    this.dbServer.AddInParameter(command2, "CurrentBalance", DbType.Double, kvo.AvailableStock);
                    this.dbServer.AddInParameter(command2, "CurrentBalanceInBase", DbType.Double, kvo.AvailableStockInBase);
                    this.dbServer.AddInParameter(command2, "Date", DbType.DateTime, nvo.DateTime);
                    this.dbServer.AddInParameter(command2, "Time", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, kvo.StoreID);
                    this.dbServer.AddInParameter(command2, "OperationType", DbType.Int16, (short) kvo.OperationType);
                    this.dbServer.AddInParameter(command2, "Remarks", DbType.String, kvo.Remarks);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, nvo.DateTime);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command2, "BaseUOMID", DbType.Int64, kvo.BaseUMID);
                    this.dbServer.AddInParameter(command2, "StockUOMID", DbType.Int64, kvo.StockingUMID);
                    this.dbServer.AddInParameter(command2, "TransactionUOMID", DbType.Int64, kvo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "StockingQuantity", DbType.Double, kvo.AdjustmentQunatitiy * kvo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Double, kvo.BaseQuantity);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Double, kvo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "StockCF", DbType.Double, kvo.ConversionFactor);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    nvo.StockAdjustmentId = (long) this.dbServer.GetParameterValue(command2, "ID");
                }
                if (nvo.objMainStock.IsFromPST)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_UpdatePhysicalStockItem");
                    this.dbServer.AddInParameter(command3, "PhysicalItemID", DbType.Int64, nvo.objMainStock.PhysicalItemID);
                    this.dbServer.AddInParameter(command3, "PhysicalItemUnitID", DbType.Int64, nvo.objMainStock.PhysicalItemUnitID);
                    this.dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                    this.dbServer.AddInParameter(command3, "UpdatedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.StockAdustmentItem = null;
                nvo.StockAdustmentItems = null;
                nvo.SuccessStatus = -1;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddTariffItem(IValueObject valueObject, clsUserVO userVO)
        {
            DbTransaction transaction = null;
            clsAddTariffItemBizActionVO nvo = valueObject as clsAddTariffItemBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddTariffLinkingWithItems");
                storedProcCommand.Parameters.Clear();
                this.dbServer.AddInParameter(storedProcCommand, "ApplyForAll", DbType.Boolean, nvo.IsAllItemsSelected);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDList", DbType.String, nvo.ItemIDList);
                this.dbServer.AddInParameter(storedProcCommand, "ItemDisList", DbType.String, nvo.ItemDisList);
                this.dbServer.AddInParameter(storedProcCommand, "ItemDedList", DbType.String, nvo.ItemDedList);
                this.dbServer.AddInParameter(storedProcCommand, "DiscountPer", DbType.Decimal, nvo.DiscountPer);
                this.dbServer.AddInParameter(storedProcCommand, "DeductablePer", DbType.Decimal, nvo.DeductablePer);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.ItemMasterDetails.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, nvo.ItemMasterDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.ItemMasterDetails.UpdateddOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.ItemMasterDetails.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.ItemMasterDetails.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, nvo.ItemMasterDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "SucessStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "SucessStatus");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateItemClinicDetail(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateItemClinicDetailBizActionVO nvo = valueObject as clsAddUpdateItemClinicDetailBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemClinicDetail");
                if (nvo.ItemTaxList.Count > 0)
                {
                    int num = 0;
                    while (true)
                    {
                        if (num > (nvo.ItemTaxList.Count - 1))
                        {
                            nvo.SuccessStatus = 1;
                            break;
                        }
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "Id", DbType.Int64, nvo.ItemTaxList[num].ItemClinicDetailId);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemClinicId", DbType.Int64, nvo.StoreClinicID);
                        this.dbServer.AddInParameter(storedProcCommand, "TaxID", DbType.Int64, nvo.ItemTaxList[num].ID);
                        this.dbServer.AddInParameter(storedProcCommand, "VATPercentage", DbType.Decimal, nvo.ItemTaxList[num].Percentage);
                        this.dbServer.AddInParameter(storedProcCommand, "ApplicableFor", DbType.Int32, nvo.ItemTaxList[num].ApplicableForId);
                        this.dbServer.AddInParameter(storedProcCommand, "VATApplicableOn", DbType.Int32, nvo.ItemTaxList[num].ApplicableOnId);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.ItemTaxList[num].status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.ItemTax.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, nvo.ItemTax.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, nvo.ItemTax.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, nvo.ItemTax.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, nvo.ItemTax.AddedWindowsLoginName);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        num++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject AddUpdateItemOtherDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateItemMasterOtherDetailsBizActionVO nvo = valueObject as clsAddUpdateItemMasterOtherDetailsBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemOtherDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ItemOtherDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, userVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemOtherDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "Contradiction", DbType.String, nvo.ItemOtherDetails.Contradiction);
                this.dbServer.AddInParameter(storedProcCommand, "SideEffect", DbType.String, nvo.ItemOtherDetails.SideEffect);
                this.dbServer.AddInParameter(storedProcCommand, "URL", DbType.String, nvo.ItemOtherDetails.URL);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdatePhysicalItemStock(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddUpdatePhysicalItemStockBizActionVO nvo = valueObject as clsAddUpdatePhysicalItemStockBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddPhysicalStockItem");
                storedProcCommand.Connection = connection;
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ItemDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ItemDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RequestDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "RequestedBy", DbType.Int64, nvo.ItemDetails.RequestedBy);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ItemDetails.ID = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ItemDetails.UnitID = UserVO.UserLoginInfo.UnitId;
                foreach (clsPhysicalItemsVO svo in nvo.ItemDetailsList)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddPhysicalStockItemDetails");
                    this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "PhysicalItemID", DbType.Int64, nvo.ItemDetails.ID);
                    this.dbServer.AddInParameter(command2, "PhysicalItemUnitID", DbType.Int64, nvo.ItemDetails.UnitID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, svo.ItemID);
                    this.dbServer.AddInParameter(command2, "BatchID", DbType.Int64, svo.BatchID);
                    this.dbServer.AddInParameter(command2, "AvailableStock", DbType.Single, svo.AvailableStock);
                    this.dbServer.AddInParameter(command2, "OperationTypeID", DbType.Int32, svo.intOperationType);
                    this.dbServer.AddInParameter(command2, "AdjustmentQunatity", DbType.Single, svo.AdjustmentQunatity);
                    this.dbServer.AddInParameter(command2, "PhysicalQuantity", DbType.Single, svo.PhysicalQuantity);
                    this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, true);
                    this.dbServer.AddInParameter(command2, "Remark", DbType.String, svo.Remark);
                    this.dbServer.AddInParameter(command2, "ShelfID", DbType.Int64, svo.ShelfID);
                    this.dbServer.AddInParameter(command2, "ContainerID", DbType.Int64, svo.ContainerID);
                    this.dbServer.AddInParameter(command2, "RackID", DbType.Int64, svo.RackID);
                    this.dbServer.AddInParameter(command2, "CreatedUnitId", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVO.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddInParameter(command2, "UOMID", DbType.Int64, svo.SelectedUOM.ID);
                    this.dbServer.AddInParameter(command2, "BaseUMID", DbType.Int64, svo.BaseUMID);
                    this.dbServer.AddInParameter(command2, "StockingUMID", DbType.Int64, svo.StockingUMID);
                    this.dbServer.AddInParameter(command2, "StockingCF", DbType.Single, svo.ConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseCF", DbType.Single, svo.BaseConversionFactor);
                    this.dbServer.AddInParameter(command2, "BaseQuantity", DbType.Single, svo.BaseQuantity);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                }
                transaction.Commit();
                nvo.SuccessStatus = 0;
            }
            catch (Exception)
            {
                nvo.SuccessStatus = -1;
                nvo.SuccessStatus = -1;
                transaction.Rollback();
            }
            finally
            {
                connection.Close();
                connection = null;
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject AddUpdateSupplier(IValueObject valueObject, clsUserVO userVO)
        {
            SupplierVO rvo = new SupplierVO();
            clsAddUpdateSupplierBizActionVO nvo = valueObject as clsAddUpdateSupplierBizActionVO;
            try
            {
                rvo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateSupplier");
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, rvo.SupplierId);
                this.dbServer.AddInParameter(storedProcCommand, "MFlag", DbType.Boolean, rvo.MFlag);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, rvo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, rvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, rvo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, rvo.SupplierName);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine1", DbType.String, rvo.Address1);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine2", DbType.String, rvo.Address2);
                this.dbServer.AddInParameter(storedProcCommand, "AddressLine3", DbType.String, rvo.Address3);
                this.dbServer.AddInParameter(storedProcCommand, "Country", DbType.String, rvo.Country);
                this.dbServer.AddInParameter(storedProcCommand, "State", DbType.String, rvo.State);
                this.dbServer.AddInParameter(storedProcCommand, "District", DbType.String, rvo.District);
                this.dbServer.AddInParameter(storedProcCommand, "City", DbType.String, rvo.City);
                this.dbServer.AddInParameter(storedProcCommand, "Area", DbType.String, rvo.Area);
                this.dbServer.AddInParameter(storedProcCommand, "Pincode", DbType.String, rvo.Pincode);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1Name", DbType.String, rvo.ContactPerson1Name);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1MobileNo", DbType.String, rvo.ContactPerson1MobileNo);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson1EmailId", DbType.String, rvo.ContactPerson1Email);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2Name", DbType.String, rvo.ContactPerson2Name);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2MobileNo", DbType.String, rvo.ContactPerson2MobileNo);
                this.dbServer.AddInParameter(storedProcCommand, "ContactPerson2EmailId", DbType.String, rvo.ContactPerson2Email);
                this.dbServer.AddInParameter(storedProcCommand, "PhoneNo", DbType.String, rvo.PhoneNo);
                this.dbServer.AddInParameter(storedProcCommand, "Fax", DbType.String, rvo.Fax);
                this.dbServer.AddInParameter(storedProcCommand, "ModeofPayment", DbType.String, rvo.ModeOfPayment);
                this.dbServer.AddInParameter(storedProcCommand, "TaxNature", DbType.String, rvo.TaxNature);
                this.dbServer.AddInParameter(storedProcCommand, "TermofPayment", DbType.String, rvo.TermofPayment);
                this.dbServer.AddInParameter(storedProcCommand, "Currency", DbType.String, rvo.Currency);
                this.dbServer.AddInParameter(storedProcCommand, "MSTNumber", DbType.String, rvo.MSTNumber);
                this.dbServer.AddInParameter(storedProcCommand, "VATNumber", DbType.String, rvo.VAT);
                this.dbServer.AddInParameter(storedProcCommand, "CSTNumber", DbType.String, rvo.CSTNumber);
                this.dbServer.AddInParameter(storedProcCommand, "DrugLicenceNumber", DbType.String, rvo.DRUGLicence);
                this.dbServer.AddInParameter(storedProcCommand, "ServiceTaxNumber", DbType.String, rvo.ServiceTaxNumber);
                this.dbServer.AddInParameter(storedProcCommand, "PANNumber", DbType.String, rvo.PANNumber);
                this.dbServer.AddInParameter(storedProcCommand, "Depreciation", DbType.String, rvo.Depreciation);
                this.dbServer.AddInParameter(storedProcCommand, "RatingSystem", DbType.String, rvo.RatingSystem);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierCategoryId", DbType.Int64, rvo.SupplierCategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "POAutoCloseDays", DbType.Int32, rvo.POAutoCloseDays);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, rvo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, rvo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "GSTINNo", DbType.String, rvo.GSTINNo);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, rvo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, rvo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, rvo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, rvo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, rvo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, rvo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, rvo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, rvo.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    rvo.PrimaryKeyViolationError = true;
                }
                else
                {
                    rvo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject ApproveMRPAdjustment(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddMRPAdjustmentBizActionVO nvo = valueObject as clsAddMRPAdjustmentBizActionVO;
            if (nvo.MRPAdjustmentMainVO == null)
            {
                nvo.MRPAdjustmentMainVO = new clsMRPAdjustmentMainVO();
            }
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ApproveMRPAdjustment");
                this.dbServer.AddInParameter(storedProcCommand, "MRPAdjustmentID", DbType.Int64, nvo.MRPAdjustmentMainVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "MRPAdjustmentUnitId", DbType.Int64, nvo.MRPAdjustmentMainVO.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVo.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                foreach (clsMRPAdjustmentVO tvo in nvo.MRPAdjustmentItems)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_ApproveMRPAdjustmentDetails");
                    this.dbServer.AddInParameter(command2, "MRPAdjustmentID", DbType.Int64, nvo.MRPAdjustmentMainVO.ID);
                    this.dbServer.AddInParameter(command2, "MRPAdjustmentUnitId", DbType.Int64, nvo.MRPAdjustmentMainVO.UnitID);
                    this.dbServer.AddInParameter(command2, "UnitId", DbType.Int64, UserVo.UserLoginInfo.UnitId);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, tvo.ItemId);
                    this.dbServer.AddInParameter(command2, "BatchId", DbType.Int64, tvo.BatchId);
                    this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, nvo.MRPAdjustmentMainVO.StoreID);
                    this.dbServer.AddInParameter(command2, "PreExpiryDate", DbType.DateTime, tvo.ExpiryDate);
                    this.dbServer.AddInParameter(command2, "PreviousBatchCode", DbType.String, tvo.BatchCode);
                    this.dbServer.AddInParameter(command2, "PreviousPurchaseRate", DbType.Double, tvo.PurchaseRate);
                    this.dbServer.AddInParameter(command2, "PreviousMRP", DbType.Double, tvo.MRP);
                    this.dbServer.AddInParameter(command2, "UpdatedExpiryDate", DbType.DateTime, tvo.UpdatedExpiryDate);
                    this.dbServer.AddInParameter(command2, "UpdatedBatchCode", DbType.String, tvo.UpdatedBatchCode);
                    this.dbServer.AddInParameter(command2, "UpdatedPurchaseRate", DbType.Double, tvo.UpdatedPurchaseRate);
                    this.dbServer.AddInParameter(command2, "UpdatedMRP", DbType.Double, tvo.UpdatedMRP);
                    this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, UserVo.ID);
                    this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, UserVo.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, UserVo.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject ApproveScrapSalesDetails(IValueObject valueObject, clsUserVO UserVo)
        {
            clsAddScrapSalesDetailsBizActionVO nvo = valueObject as clsAddScrapSalesDetailsBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateScrapSaleForApprove");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "ItemScrapSaleId", DbType.Int64, nvo.ItemMatserDetail.ScrapID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemScrapSaleUnitId", DbType.Int64, nvo.ItemMatserDetail.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "ApproveOrRejectedBy", DbType.Int64, UserVo.ID);
                this.dbServer.AddParameter(storedProcCommand, "ReasultStatus", DbType.Int32, ParameterDirection.InputOutput, null, DataRowVersion.Default, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ReasultStatus"));
                if (nvo.SuccessStatus == -10)
                {
                    throw new Exception();
                }
                List<clsSrcapDetailsVO> itemsDetail = nvo.ItemsDetail;
                if (itemsDetail.Count > 0)
                {
                    foreach (clsSrcapDetailsVO svo in itemsDetail.ToList<clsSrcapDetailsVO>())
                    {
                        svo.StockDetails.PurchaseRate = svo.Rate;
                        svo.StockDetails.BatchID = Convert.ToInt64(svo.BatchID);
                        svo.StockDetails.OperationType = InventoryStockOperationType.Subtraction;
                        svo.StockDetails.ItemID = svo.ItemId;
                        svo.StockDetails.TransactionTypeID = InventoryTransactionType.ScrapSale;
                        svo.StockDetails.TransactionID = Convert.ToInt64(svo.ItemScrapSaleId);
                        svo.StockDetails.TransactionQuantity = svo.BaseQuantity;
                        svo.StockDetails.Date = DateTime.Now;
                        svo.StockDetails.Time = DateTime.Now;
                        svo.StockDetails.StoreID = new long?(svo.StoreID);
                        svo.StockDetails.InputTransactionQuantity = Convert.ToSingle(svo.SaleQty);
                        svo.StockDetails.BaseUOMID = svo.BaseUOMID;
                        svo.StockDetails.BaseConversionFactor = svo.BaseConversionFactor;
                        svo.StockDetails.SUOMID = svo.SUOMID;
                        svo.StockDetails.ConversionFactor = svo.ConversionFactor;
                        svo.StockDetails.StockingQuantity = svo.SaleQty * svo.ConversionFactor;
                        svo.StockDetails.SelectedUOM.ID = svo.TransactionUOMID;
                        svo.StockDetails.ExpiryDate = svo.BatchExpiryDate;
                        svo.StockDetails.UnitId = svo.UnitId;
                        clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                        clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                            Details = svo.StockDetails
                        };
                        nvo2.Details.ID = 0L;
                        nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVo, myConnection, transaction);
                        if (nvo2.SuccessStatus == -1)
                        {
                            throw new Exception();
                        }
                        svo.StockDetails.ID = nvo2.Details.ID;
                    }
                }
                transaction.Commit();
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                this.logManager.LogError(UserVo.ID, DateTime.Now, MethodBase.GetCurrentMethod().DeclaringType.ToString(), MethodBase.GetCurrentMethod().ToString(), exception.ToString());
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject CheckForTaxExistance(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemTaxBizActionVO nvo = valueObject as clsGetItemTaxBizActionVO;
            bool flag = false;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckForTaxExistance");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemDetails.ItemTaxID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                flag = this.dbServer.ExecuteScalar(storedProcCommand) != null;
                nvo.IsTaxAdded = flag;
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public bool CheckItemSupplier(long ItemID, long SupplierID)
        {
            bool flag;
            this.dbServer.CreateConnection();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_CheckItemSupplier");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, SupplierID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                flag = ((int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus")) == 1;
            }
            catch (Exception)
            {
                throw;
            }
            return flag;
        }

        public override IValueObject clsAddItemsEnquiry(IValueObject valueObject, clsUserVO userVO)
        {
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            clsAddItemsEnquiryBizActionVO nvo = valueObject as clsAddItemsEnquiryBizActionVO;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                clsItemEnquiryVO itemMatserDetail = nvo.ItemMatserDetail;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddtemEnquiry");
                this.dbServer.AddInParameter(storedProcCommand, "UnitId", DbType.Int64, itemMatserDetail.UnitId);
                this.dbServer.AddParameter(storedProcCommand, "EnquiryNO", DbType.String, ParameterDirection.InputOutput, null, DataRowVersion.Default, "??????????????????????????????????????????????????");
                this.dbServer.AddInParameter(storedProcCommand, "Date", DbType.DateTime, itemMatserDetail.Date);
                this.dbServer.AddInParameter(storedProcCommand, "Time", DbType.DateTime, itemMatserDetail.Time);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, itemMatserDetail.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "Header", DbType.String, itemMatserDetail.Header);
                this.dbServer.AddInParameter(storedProcCommand, "Notes", DbType.String, itemMatserDetail.Notes);
                this.dbServer.AddInParameter(storedProcCommand, "AddUnitID", DbType.Int64, itemMatserDetail.AddUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "By", DbType.Int64, itemMatserDetail.By);
                this.dbServer.AddInParameter(storedProcCommand, "On", DbType.String, itemMatserDetail.On);
                this.dbServer.AddInParameter(storedProcCommand, "DateTime", DbType.DateTime, itemMatserDetail.DateTime);
                this.dbServer.AddInParameter(storedProcCommand, "WindowsLoginName", DbType.String, itemMatserDetail.WindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddOutParameter(storedProcCommand, "ID", DbType.Int64, 0);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ItemEnquiryId = (long) this.dbServer.GetParameterValue(storedProcCommand, "ID");
                nvo.ItemMatserDetail.EnquiryNO = (string) this.dbServer.GetParameterValue(storedProcCommand, "EnquiryNO");
                foreach (clsItemsEnquirySupplierVO rvo in nvo.ItemsSupplierDetail)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_AddItemsEnquirySupplier");
                    this.dbServer.AddInParameter(command2, "EnquiryID", DbType.Int64, nvo.ItemEnquiryId);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, rvo.UnitId);
                    this.dbServer.AddInParameter(command2, "SupplierID", DbType.Int64, rvo.SupplierID);
                    this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                foreach (clsItemsEnquiryTermConditionVO nvo2 in nvo.ItemsTermsConditionDetail)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddItemsEnquiryTermsCondtn");
                    this.dbServer.AddInParameter(command3, "EnquiryID", DbType.Int64, nvo.ItemEnquiryId);
                    this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, nvo2.UnitId);
                    this.dbServer.AddInParameter(command3, "TermsConditionID", DbType.Int64, nvo2.TermsConditionID);
                    this.dbServer.AddInParameter(command3, "Remarks", DbType.String, nvo2.Remarks);
                    this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command3, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command3, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                foreach (clsEnquiryDetailsVO svo in nvo.ItemsDetail)
                {
                    DbCommand command4 = this.dbServer.GetStoredProcCommand("CIMS_AddItemsEnquiryDetails");
                    this.dbServer.AddInParameter(command4, "EnquiryID", DbType.Int64, nvo.ItemEnquiryId);
                    this.dbServer.AddInParameter(command4, "UnitID", DbType.Int64, itemMatserDetail.UnitId);
                    this.dbServer.AddInParameter(command4, "ItemID", DbType.Int64, svo.ItemId);
                    this.dbServer.AddInParameter(command4, "Quantity", DbType.Double, svo.Quantity);
                    this.dbServer.AddInParameter(command4, "PackSize", DbType.Double, svo.PackSize);
                    this.dbServer.AddInParameter(command4, "Remarks", DbType.String, svo.Remarks);
                    this.dbServer.AddOutParameter(command4, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.AddOutParameter(command4, "ID", DbType.Int64, 0);
                    this.dbServer.ExecuteNonQuery(command4, transaction);
                    nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.ItemMatserDetail = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject clsAddUpdateItemLocation(IValueObject valueObject, clsUserVO UserVO)
        {
            clsAddUpdateItemLocationBizActionVO nvo = valueObject as clsAddUpdateItemLocationBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteItemLocationDetails");
                if (nvo.StoreDetails.Count > 0)
                {
                    foreach (long local1 in nvo.StoreDetails)
                    {
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ItemLocationDetails.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemLocationDetails.ItemID);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                }
                DbCommand command2 = null;
                command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemLocationDetails");
                if (nvo.StoreDetails.Count > 0)
                {
                    foreach (long num in nvo.StoreDetails)
                    {
                        command2.Parameters.Clear();
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.ItemLocationDetails.UnitID);
                        this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, nvo.ItemLocationDetails.ItemID);
                        this.dbServer.AddInParameter(command2, "StoreID", DbType.Int64, num);
                        this.dbServer.AddInParameter(command2, "RackID", DbType.Int64, nvo.ItemLocationDetails.RackID);
                        this.dbServer.AddInParameter(command2, "ShelfID", DbType.Int64, nvo.ItemLocationDetails.ShelfID);
                        this.dbServer.AddInParameter(command2, "ContainerID", DbType.Int64, nvo.ItemLocationDetails.ContainerID);
                        this.dbServer.AddInParameter(command2, "Status", DbType.Boolean, nvo.ItemLocationDetails.Status);
                        this.dbServer.AddInParameter(command2, "CreatedUnitID", DbType.Int64, nvo.ItemLocationDetails.CreatedUnitID);
                        this.dbServer.AddInParameter(command2, "AddedBy", DbType.Int64, nvo.ItemLocationDetails.AddedBy);
                        this.dbServer.AddInParameter(command2, "AddedOn", DbType.String, nvo.ItemLocationDetails.AddedOn);
                        this.dbServer.AddInParameter(command2, "AddedDateTime", DbType.DateTime, nvo.ItemLocationDetails.AddedDateTime);
                        this.dbServer.AddInParameter(command2, "AddedWindowsLoginName", DbType.String, nvo.ItemLocationDetails.AddedWindowsLoginName);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.ItemLocationDetails.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command2, "ID"));
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command2, "ResultStatus");
                    }
                }
                nvo.SuccessStatus = 1;
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsAddUpdateItemStoreLocationDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            ItemStoreLocationDetailsBizActionVo vo = valueObject as ItemStoreLocationDetailsBizActionVo;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateItemStoreLocationDetails");
                if (vo.StoreDetails.Count <= 0)
                {
                    storedProcCommand.Parameters.Clear();
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, vo.ItemStoreLocationDetail.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, vo.ItemStoreLocationDetail.UnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, vo.ItemStoreLocationDetail.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, vo.ItemStoreLocationDetail.StoreID);
                    this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, vo.ItemStoreLocationDetail.RackID);
                    this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, vo.ItemStoreLocationDetail.ShelfID);
                    this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, vo.ItemStoreLocationDetail.ContainerID);
                    this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, vo.ItemStoreLocationDetail.Status);
                    this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, vo.ItemStoreLocationDetail.CreatedUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, vo.ItemStoreLocationDetail.AddedBy);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, vo.ItemStoreLocationDetail.AddedOn);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, vo.ItemStoreLocationDetail.AddedDateTime);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, vo.ItemStoreLocationDetail.UpdatedUnitID);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                    this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                    this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                    this.dbServer.ExecuteNonQuery(storedProcCommand);
                    vo.ItemStoreLocationDetail.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                    vo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    vo.SuccessStatus = 1;
                }
                else
                {
                    foreach (long num in vo.StoreDetails)
                    {
                        storedProcCommand.Parameters.Clear();
                        this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, vo.ItemStoreLocationDetail.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, vo.ItemStoreLocationDetail.UnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, vo.ItemStoreLocationDetail.ItemID);
                        this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, num);
                        this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, vo.ItemStoreLocationDetail.RackID);
                        this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, vo.ItemStoreLocationDetail.ShelfID);
                        this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, vo.ItemStoreLocationDetail.ContainerID);
                        this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, vo.ItemStoreLocationDetail.Status);
                        this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, vo.ItemStoreLocationDetail.CreatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, vo.ItemStoreLocationDetail.AddedBy);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, vo.ItemStoreLocationDetail.AddedOn);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, vo.ItemStoreLocationDetail.AddedDateTime);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, vo.ItemStoreLocationDetail.UpdatedUnitID);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(storedProcCommand);
                        vo.ItemStoreLocationDetail.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                        vo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                    }
                    vo.SuccessStatus = 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return vo;
        }

        public override IValueObject clsAddUpdateStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsStoreVO evo = new clsStoreVO();
            clsAddUpdateStoreDetailsBizActionVO nvo = valueObject as clsAddUpdateStoreDetailsBizActionVO;
            try
            {
                evo = nvo.ItemMatserDetails[0];
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStore");
                this.dbServer.AddParameter(storedProcCommand, "StoreId", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, evo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, evo.StoreName);
                this.dbServer.AddInParameter(storedProcCommand, "CostCenterCode", DbType.Int64, evo.CostCenterCodeID);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, evo.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, evo.Code);
                this.dbServer.AddInParameter(storedProcCommand, "OpeningBalance", DbType.Boolean, evo.OpeningBalance);
                this.dbServer.AddInParameter(storedProcCommand, "Indent", DbType.Boolean, evo.Indent);
                this.dbServer.AddInParameter(storedProcCommand, "Issue", DbType.Boolean, evo.Issue);
                this.dbServer.AddInParameter(storedProcCommand, "IsQuarantineStore", DbType.Boolean, evo.IsQuarantineStore);
                this.dbServer.AddInParameter(storedProcCommand, "ItemReturn", DbType.Boolean, evo.ItemReturn);
                this.dbServer.AddInParameter(storedProcCommand, "GoodsReceivedNote", DbType.Boolean, evo.GoodsReceivedNote);
                this.dbServer.AddInParameter(storedProcCommand, "GRNReturn", DbType.Boolean, evo.GRNReturn);
                this.dbServer.AddInParameter(storedProcCommand, "ItemsSale", DbType.Boolean, evo.ItemsSale);
                this.dbServer.AddInParameter(storedProcCommand, "ItemsSaleReturn", DbType.Boolean, evo.ItemSaleReturn);
                this.dbServer.AddInParameter(storedProcCommand, "ExpiryItemReturn", DbType.Boolean, evo.ExpiryItemReturn);
                this.dbServer.AddInParameter(storedProcCommand, "ReceiveIssue", DbType.Boolean, evo.ReceiveIssue);
                this.dbServer.AddInParameter(storedProcCommand, "ReceiveIssueReturn", DbType.Boolean, evo.ReceiveItemReturn);
                this.dbServer.AddInParameter(storedProcCommand, "Isparent", DbType.Int64, evo.Parent);
                this.dbServer.AddInParameter(storedProcCommand, "ItemCatagoryID", DbType.String, evo.ItemCatagoryID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, evo.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IsCentralStore", DbType.Boolean, evo.isCentralStore);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, evo.CreatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, evo.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, evo.AddedBy);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, evo.AddedOn);
                this.dbServer.AddInParameter(storedProcCommand, "AddedDateTime", DbType.DateTime, evo.AddedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, evo.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, evo.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, evo.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, evo.AddedWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, evo.UpdateWindowsLoginName);
                this.dbServer.AddInParameter(storedProcCommand, "ApplyAllItems", DbType.Boolean, nvo.ApplyallItems);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                nvo.StoreID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "StoreId"));
                if ((nvo.ApplyallItems != null) && (nvo.SuccessStatus == 1))
                {
                    DbCommand command2;
                    bool? applyallItems = nvo.ApplyallItems;
                    if (applyallItems.GetValueOrDefault() && (applyallItems != null))
                    {
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteStoreAndCategoriesRelation");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command2, "StoreId", DbType.Int64, nvo.StoreID);
                        this.dbServer.AddInParameter(command2, "IsForCategories", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "IsForAll", DbType.Boolean, false);
                        this.dbServer.ExecuteNonQuery(command2);
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreAndCategoriesRelation");
                        this.dbServer.AddInParameter(command2, "ID", DbType.Int64, 0);
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command2, "StoreId", DbType.Int64, nvo.StoreID);
                        this.dbServer.AddInParameter(command2, "IsForAll", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "IsForCategories", DbType.Boolean, false);
                        this.dbServer.AddInParameter(command2, "CategotyID", DbType.Int64, 0);
                        this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command2);
                        nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                    }
                    else
                    {
                        command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteStoreAndCategoriesRelation");
                        this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, 1);
                        this.dbServer.AddInParameter(command2, "StoreId", DbType.Int64, nvo.StoreID);
                        this.dbServer.AddInParameter(command2, "IsForCategories", DbType.Boolean, true);
                        this.dbServer.AddInParameter(command2, "IsForAll", DbType.Boolean, false);
                        this.dbServer.ExecuteNonQuery(command2);
                        foreach (long num in nvo.ItemCategoryID)
                        {
                            command2 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreAndCategoriesRelation");
                            this.dbServer.AddOutParameter(command2, "ID", DbType.Int64, 0);
                            this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, 1);
                            this.dbServer.AddInParameter(command2, "StoreId", DbType.Int64, nvo.StoreID);
                            this.dbServer.AddInParameter(command2, "IsForCategories", DbType.Boolean, true);
                            this.dbServer.AddInParameter(command2, "IsForAll", DbType.Boolean, false);
                            this.dbServer.AddInParameter(command2, "CategotyID", DbType.Int64, num);
                            this.dbServer.AddOutParameter(command2, "ResultStatus", DbType.Int64, 0x7fffffff);
                            this.dbServer.ExecuteNonQuery(command2);
                            nvo.ID = Convert.ToInt64(this.dbServer.GetParameterValue(command2, "ID"));
                            nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(command2, "ResultStatus"));
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                if (exception1.Message.Contains("duplicate"))
                {
                    evo.PrimaryKeyViolationError = true;
                }
                else
                {
                    evo.GeneralError = true;
                }
            }
            return nvo;
        }

        public override IValueObject clsAddUpdateStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {
            AddUpdateStoreLocationLockBizActionVO nvo = valueObject as AddUpdateStoreLocationLockBizActionVO;
            DbConnection connection = null;
            DbTransaction transaction = null;
            try
            {
                DbCommand storedProcCommand = null;
                connection = this.dbServer.CreateConnection();
                connection.Open();
                transaction = connection.BeginTransaction();
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreLocationLock");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ObjStoreLocationLock.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "date", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.ObjStoreLocationLock.Remark);
                this.dbServer.AddInParameter(storedProcCommand, "IsFreeze", DbType.Boolean, nvo.ObjStoreLocationLock.IsFreeze);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "CreatedUnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "AddedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.AddParameter(storedProcCommand, "ID", DbType.Int64, ParameterDirection.InputOutput, null, DataRowVersion.Default, nvo.ObjStoreLocationLock.ID);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                nvo.ObjStoreLocationLock.ID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "ID"));
                nvo.ObjStoreLocationLock.UnitID = Convert.ToInt64(this.dbServer.GetParameterValue(storedProcCommand, "UnitID"));
                nvo.SuccessStatus = 1;
                if (nvo.IsModify)
                {
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_DeleteStoreLocationLock");
                    command2.Connection = connection;
                    this.dbServer.AddInParameter(command2, "StoreLocationID", DbType.Int64, nvo.ObjStoreLocationLock.ID);
                    this.dbServer.AddInParameter(command2, "StoreLocationUnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                    this.dbServer.ExecuteNonQuery(command2, transaction);
                }
                if (nvo.StoreLocationLockDetails.Count > 0)
                {
                    DbCommand command3 = this.dbServer.GetStoredProcCommand("CIMS_AddUpdateStoreLocationLockDetails");
                    command3.Connection = connection;
                    foreach (StoreLocationLockVO kvo in nvo.StoreLocationLockDetails)
                    {
                        command3.Parameters.Clear();
                        this.dbServer.AddInParameter(command3, "ID", DbType.Int64, kvo.ID);
                        this.dbServer.AddInParameter(command3, "UnitID", DbType.Int64, kvo.UnitID);
                        this.dbServer.AddInParameter(command3, "StoreID", DbType.Int64, kvo.StoreID);
                        this.dbServer.AddInParameter(command3, "RackID", DbType.Int64, kvo.RackID);
                        this.dbServer.AddInParameter(command3, "ShelfID", DbType.Int64, kvo.ShelfID);
                        this.dbServer.AddInParameter(command3, "ContainerID", DbType.Int64, kvo.ContainerID);
                        this.dbServer.AddInParameter(command3, "ItemID", DbType.Int64, kvo.ItemID);
                        this.dbServer.AddInParameter(command3, "BlockType", DbType.String, kvo.BlockType);
                        this.dbServer.AddInParameter(command3, "BlockTypeID", DbType.Int64, kvo.BlockTypeID);
                        if (nvo.ObjStoreLocationLock.IsFreeze)
                        {
                            this.dbServer.AddInParameter(command3, "IsBlock", DbType.Boolean, nvo.ObjStoreLocationLock.IsFreeze);
                        }
                        this.dbServer.AddInParameter(command3, "LockDate", DbType.DateTime, kvo.LockDate);
                        this.dbServer.AddInParameter(command3, "status", DbType.Boolean, kvo.Status);
                        this.dbServer.AddInParameter(command3, "CreatedUnitID", DbType.Int64, kvo.CreatedUnitID);
                        this.dbServer.AddInParameter(command3, "AddedBy", DbType.Int64, kvo.AddedBy);
                        this.dbServer.AddInParameter(command3, "addedOn", DbType.String, kvo.AddOn);
                        this.dbServer.AddInParameter(command3, "addedDateTime", DbType.DateTime, kvo.AddedDateTime);
                        this.dbServer.AddInParameter(command3, "UpdatedBy", DbType.Int64, UserVO.ID);
                        this.dbServer.AddInParameter(command3, "UpdatedUnitID", DbType.Int64, kvo.UpdatedUnitID);
                        this.dbServer.AddInParameter(command3, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                        this.dbServer.AddInParameter(command3, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                        this.dbServer.AddInParameter(command3, "AddedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "UpdateWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                        this.dbServer.AddInParameter(command3, "StoreLocationID", DbType.Int64, nvo.ObjStoreLocationLock.ID);
                        this.dbServer.AddInParameter(command3, "StoreLocationUnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                        this.dbServer.AddOutParameter(command3, "ResultStatus", DbType.Int32, 0x7fffffff);
                        this.dbServer.ExecuteNonQuery(command3, transaction);
                        nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(command3, "ResultStatus");
                    }
                    nvo.SuccessStatus = 1;
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                transaction = null;
                throw;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject clsGetBlockItemList(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO nvo = valueObject as clsGetBlockItemsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBlockItemList");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ItemDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.ItemDetails.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ItemDetails.ShelfID);
                this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, nvo.ItemDetails.ContainerID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemDetails.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemDetailsList == null)
                    {
                        nvo.ItemDetailsList = new List<clsBlockItemsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsBlockItemsVO item = new clsBlockItemsVO {
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"])),
                            Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"])),
                            Container = Convert.ToString(DALHelper.HandleDBNull(reader["Container"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ExpiryDate = (DateTime?) DALHelper.HandleDBNull(reader["ExpiryDate"]),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"])),
                            ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"])),
                            ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"])),
                            SellingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            SellingUM = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            PurchaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PurchaseUMID"])),
                            PurchaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["PurchaseUM"])),
                            StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockingUMID"])),
                            StockingUM = Convert.ToString(DALHelper.HandleDBNull(reader["StockingUM"])),
                            SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"])),
                            StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"])),
                            AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]))
                        };
                        nvo.ItemDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetGRNNumberBetweenDateRange(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllGRNNumberBetweenDateRangeBizActionVO nvo = valueObject as clsGetAllGRNNumberBetweenDateRangeBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetGRNNumberBetweenDateRange");
                this.dbServer.AddInParameter(storedProcCommand, "SupplierID", DbType.Int64, nvo.SupplierID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        clsGRNVO sgrnvo = new clsGRNVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            GRNNO = Convert.ToString(DALHelper.HandleDBNull(reader["GRNNO"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = sgrnvo.ID,
                            Description = sgrnvo.GRNNO
                        };
                        nvo.MasterList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetItemLocation(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetItemLocationBizActionVO nvo = valueObject as clsGetItemLocationBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemLocationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "itemID", DbType.Int64, nvo.ItemLocationDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ItemLocationDetails.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    nvo.ItemLocationDetails = new clsItemLocationVO();
                    nvo.StoreDetails = new List<long>();
                    while (reader.Read())
                    {
                        nvo.ItemLocationDetails.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"]));
                        nvo.ItemLocationDetails.UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"]));
                        nvo.ItemLocationDetails.ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"]));
                        nvo.ItemLocationDetails.StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"]));
                        nvo.ItemLocationDetails.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                        nvo.ItemLocationDetails.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                        nvo.ItemLocationDetails.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                        nvo.ItemLocationDetails.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                        nvo.ItemLocationDetails.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                        nvo.ItemLocationDetails.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                        nvo.ItemLocationDetails.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        nvo.StoreDetails.Add(nvo.ItemLocationDetails.StoreID);
                    }
                }
                reader.NextResult();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetItemStoreLocationDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationDetailsBizActionVO nvo = valueObject as GetItemStoreLocationDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemStoreLocationDetails");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ObjItemStoreLocationDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ObjItemStoreLocationDetails.ItemID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemStoreLocationDetailslist == null)
                    {
                        nvo.ItemStoreLocationDetailslist = new List<ItemStoreLocationDetailsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.Close();
                            break;
                        }
                        ItemStoreLocationDetailsVO item = new ItemStoreLocationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"])),
                            ItemUnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemUnitID"])),
                            Barcode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"])),
                            RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"])),
                            ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"])),
                            ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"])),
                            Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"])),
                            Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"])),
                            Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"])
                        };
                        nvo.ItemStoreLocationDetailslist.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreDetailsBizActionVO nvo = valueObject as clsGetStoreDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StoreType", DbType.Int32, nvo.StoreType);
                this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    clsStoreVO item = null;
                    while (reader.Read())
                    {
                        item = new clsStoreVO {
                            StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsQuarantineStore"]))
                        };
                        item.QuarantineDescription = !item.IsQuarantineStore ? "NO" : "YES";
                        item.OpeningBalance = Convert.ToBoolean(DALHelper.HandleDBNull(reader["OpeningBalance"]));
                        item.Indent = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Indent"]));
                        item.Issue = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Issue"]));
                        item.ItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemReturn"]));
                        item.Parent = Convert.ToInt64(DALHelper.HandleDBNull(reader["Isparent"]));
                        item.ParentName = (Convert.ToString(DALHelper.HandleDBNull(reader["ParentStore"])) == "") ? "NA" : Convert.ToString(DALHelper.HandleDBNull(reader["ParentStore"]));
                        item.GoodsReceivedNote = Convert.ToBoolean(DALHelper.HandleDBNull(reader["GoodsReceivedNote"]));
                        item.GRNReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["GRNReturn"]));
                        item.ItemsSale = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemsSale"]));
                        item.ItemSaleReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ItemsSaleReturn"]));
                        item.ExpiryItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ExpiryItemReturn"]));
                        item.ReceiveIssue = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReceiveIssue"]));
                        item.ReceiveItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader["ReceiveIssueReturn"]));
                        item.Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"]));
                        item.isCentralStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralStore"]));
                        item.StateID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StateID"]));
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                if (nvo.IsUserwiseStores)
                {
                    DbDataReader reader2 = null;
                    DbCommand command2 = this.dbServer.GetStoredProcCommand("CIMS_GetUserWiseStoreDetails");
                    this.dbServer.AddInParameter(command2, "UserID", DbType.Int64, nvo.UserID);
                    this.dbServer.AddInParameter(command2, "UnitID", DbType.Int64, nvo.UnitID);
                    this.dbServer.AddInParameter(command2, "ItemID", DbType.Int64, nvo.ItemID);
                    reader2 = (DbDataReader) this.dbServer.ExecuteReader(command2);
                    clsStoreVO item = null;
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            item = new clsStoreVO {
                                StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["Id"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader2["Code"])),
                                StoreName = Convert.ToString(DALHelper.HandleDBNull(reader2["Description"])),
                                ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader2["ClinicId"])),
                                ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader2["ClinicName"])),
                                IsQuarantineStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsQuarantineStore"]))
                            };
                            item.QuarantineDescription = !item.IsQuarantineStore ? "NO" : "YES";
                            item.OpeningBalance = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["OpeningBalance"]));
                            item.Indent = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["Indent"]));
                            item.Issue = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["Issue"]));
                            item.ItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ItemReturn"]));
                            item.Parent = Convert.ToInt64(DALHelper.HandleDBNull(reader2["Isparent"]));
                            item.ParentName = (Convert.ToString(DALHelper.HandleDBNull(reader2["ParentStore"])) == "") ? "NA" : Convert.ToString(DALHelper.HandleDBNull(reader2["ParentStore"]));
                            item.GoodsReceivedNote = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["GoodsReceivedNote"]));
                            item.GRNReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["GRNReturn"]));
                            item.ItemsSale = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ItemsSale"]));
                            item.ItemSaleReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ItemsSaleReturn"]));
                            item.ExpiryItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ExpiryItemReturn"]));
                            item.ReceiveIssue = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ReceiveIssue"]));
                            item.ReceiveItemReturn = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["ReceiveIssueReturn"]));
                            item.StateID = Convert.ToInt64(DALHelper.HandleDBNull(reader2["StateID"]));
                            nvo.ToStoreList.Add(item);
                        }
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject clsGetStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {
            GetStoreLocationLockBizActionVO nvo = valueObject as GetStoreLocationLockBizActionVO;
            try
            {
                if (nvo.Flag == 1)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreLocaionLock");
                    if (nvo.ObjStoreLocationLock.StoreID > 0L)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ObjStoreLocationLock.StoreID);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                    if ((nvo.ObjStoreLocationLock.Remark != null) && (nvo.ObjStoreLocationLock.Remark != ""))
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "Remark", DbType.String, nvo.ObjStoreLocationLock.Remark);
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, UserVO.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.Date, nvo.ObjStoreLocationLock.FromDate);
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.Date, nvo.ObjStoreLocationLock.ToDate);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                    DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        if (nvo.StoreLocationLock == null)
                        {
                            nvo.StoreLocationLock = new List<StoreLocationLockVO>();
                        }
                        while (true)
                        {
                            if (!reader.Read())
                            {
                                reader.NextResult();
                                nvo.TotalRows = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "TotalRows"));
                                break;
                            }
                            StoreLocationLockVO item = new StoreLocationLockVO {
                                ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                                RequestNo = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RequestNo"])),
                                AddedDateTime = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                                StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"])),
                                Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                                AddedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["AddedBy"])),
                                IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"])),
                                StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                                AddedByname = Convert.ToString(DALHelper.HandleDBNull(reader["AddedbyName"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LockDate"])),
                                IsBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsBlock"])),
                                ReleaseDate = DALHelper.HandleDate(reader["ReleaseDate"])
                            };
                            nvo.StoreLocationLock.Add(item);
                        }
                    }
                }
                if (nvo.Flag == 2)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreLocaionLockDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "StoreLocationID", DbType.Int64, nvo.ObjStoreLocationLock.ID);
                    this.dbServer.AddInParameter(storedProcCommand, "StoreLocationUnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                    DbDataReader reader2 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader2.HasRows)
                    {
                        if (nvo.StoreLocationLock == null)
                        {
                            nvo.StoreLocationLock = new List<StoreLocationLockVO>();
                        }
                        while (true)
                        {
                            if (!reader2.Read())
                            {
                                reader2.Close();
                                break;
                            }
                            StoreLocationLockVO item = new StoreLocationLockVO {
                                ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["ID"])),
                                UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["UnitID"])),
                                StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["StoreID"])),
                                ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["ShelfID"])),
                                ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["ContainerID"])),
                                RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["RackID"])),
                                ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["ItemID"])),
                                LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader2["LockDate"])),
                                ReleaseDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader2["ReleaseDate"]))),
                                Remark = Convert.ToString(DALHelper.HandleDBNull(reader2["Remark"])),
                                StoreName = Convert.ToString(DALHelper.HandleDBNull(reader2["StoreName"])),
                                Rackname = Convert.ToString(DALHelper.HandleDBNull(reader2["RackName"])),
                                Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader2["ShelfName"])),
                                Containername = Convert.ToString(DALHelper.HandleDBNull(reader2["ContainerName"])),
                                ItemName = Convert.ToString(DALHelper.HandleDBNull(reader2["ItemName"])),
                                IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsFreeze"])),
                                IsBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader2["IsItemBlock"])),
                                BlockType = Convert.ToString(DALHelper.HandleDBNull(reader2["BlockType"])),
                                BlockTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader2["BlockTypeID"]))
                            };
                            nvo.StoreLocationLock.Add(item);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetStoreLocationLockForView(IValueObject valueObject, clsUserVO UserVO)
        {
            GetStoreLocationLockBizActionVO nvo = valueObject as GetStoreLocationLockBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreLocationLockForView");
                this.dbServer.AddInParameter(storedProcCommand, "StoreLocationID", DbType.Int64, nvo.ObjStoreLocationLock.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreLocationUnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            if (nvo.StoreLocationLock == null)
                            {
                                nvo.StoreLocationLockDetailsList = new List<ItemStoreLocationDetailsVO>();
                            }
                            while (reader.Read())
                            {
                                ItemStoreLocationDetailsVO item = new ItemStoreLocationDetailsVO {
                                    ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                                    UnitID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["UnitID"])),
                                    StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"])),
                                    ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"])),
                                    ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"])),
                                    RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"])),
                                    ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemId"])),
                                    LockDate = Convert.ToDateTime(DALHelper.HandleDate(reader["LockDate"])),
                                    ReleaseDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ReleaseDate"])),
                                    StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                                    Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"])),
                                    Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"])),
                                    Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"])),
                                    ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                                    BlockType = Convert.ToString(DALHelper.HandleDBNull(reader["BlockType"])),
                                    BlockTypeID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["BlockTypeID"]))
                                };
                                nvo.StoreLocationLockDetailsList.Add(item);
                            }
                            break;
                        }
                        nvo.ObjStoreLocationLock.StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"]));
                        nvo.ObjStoreLocationLock.Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]));
                        nvo.ObjStoreLocationLock.IsFreeze = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFreeze"]));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsGetStoreWithCategoryDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreWithCategoryDetailsBizActionVO nvo = valueObject as clsGetStoreWithCategoryDetailsBizActionVO;
            clsStoreVO evo = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreCategoryDetailsByStoreID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.String, nvo.StoreId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        evo = new clsStoreVO();
                        nvo.IsForAll = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForAll"]));
                        evo.StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]));
                        nvo.IsForCategories = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsForCategories"]));
                        evo.CategoryID = Convert.ToInt64(DALHelper.HandleDBNull(reader["CategotyID"]));
                        nvo.ItemMatserCategoryDetails.Add(evo.CategoryID);
                    }
                }
                reader.NextResult();
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

        public override IValueObject clsGetValuesforScrapSalesbyItemID(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreDetailsBizActionVO nvo = valueObject as clsGetStoreDetailsBizActionVO;
            clsStoreVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsStoreVO {
                            StoreId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                            ClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ClinicId"])),
                            ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["ClinicName"])),
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["status"])),
                            isCentralStore = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsCentralStore"]))
                        };
                        nvo.ItemMatserDetails.Add(item);
                    }
                }
                reader.NextResult();
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

        public override IValueObject clsItemListForSuspendStockSearch(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO nvo = valueObject as clsGetBlockItemsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListForSuspendStockSearch");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ItemDetails.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ItemDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.ItemDetails.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ItemDetails.ShelfID);
                this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, nvo.ItemDetails.ContainerID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemDetails.ItemName);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemDetailsList == null)
                    {
                        nvo.ItemDetailsList = new List<clsBlockItemsVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsBlockItemsVO item = new clsBlockItemsVO {
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"])),
                            ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"])),
                            ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            Rack = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"])),
                            Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"])),
                            Container = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))
                        };
                        nvo.ItemDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsUpdateCentralStoreDetails(IValueObject valueObject, clsUserVO userVO)
        {
            clsStoreVO storeMasterDetails = new clsStoreVO();
            clsUpdateCentralStoreDetailsBizActionVO nvo = valueObject as clsUpdateCentralStoreDetailsBizActionVO;
            try
            {
                storeMasterDetails = nvo.StoreMasterDetails;
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateCentralStoreDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, storeMasterDetails.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "Description", DbType.String, storeMasterDetails.StoreName);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicId", DbType.Int64, storeMasterDetails.ClinicId);
                this.dbServer.AddInParameter(storedProcCommand, "Code", DbType.String, storeMasterDetails.Code);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, storeMasterDetails.Status);
                this.dbServer.AddInParameter(storedProcCommand, "IsCentralStore", DbType.Boolean, storeMasterDetails.isCentralStore);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, storeMasterDetails.UpdatedUnitID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int32, storeMasterDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, storeMasterDetails.UpdatedOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, storeMasterDetails.UpdatedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, storeMasterDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int64, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = Convert.ToInt16(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject clsUpdateStoreStatus(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddUpdateStoreDetailsBizActionVO nvo = valueObject as clsAddUpdateStoreDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStoreStatus");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, nvo.Status);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
            }
            catch (Exception)
            {
            }
            return nvo;
        }

        public override IValueObject DeleteItemStore(IValueObject valueObject, clsUserVO userVO)
        {
            clsDeleteItemStoresBizActionVO nvo = valueObject as clsDeleteItemStoresBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                clsItemMasterVO itemMatserDetails = nvo.ItemMatserDetails;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteItemStore");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, itemMatserDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, itemMatserDetails.ClinicID);
                nvo.SuccessStatus = (this.dbServer.ExecuteNonQuery(storedProcCommand) <= 0) ? 0 : 1;
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return nvo;
        }

        public override IValueObject DeleteTax(IValueObject valueObject, clsUserVO userVO)
        {
            clsAddItemClinicBizActionVO nvo = valueObject as clsAddItemClinicBizActionVO;
            DbCommand storedProcCommand = null;
            storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_DeleteItemClinicDetail");
            this.dbServer.AddInParameter(storedProcCommand, "TaxID", DbType.Int64, nvo.DeleteTaxID);
            this.dbServer.ExecuteNonQuery(storedProcCommand);
            return valueObject;
        }

        public override IValueObject GetAllItemList(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllItemListBizActionVO nvo = valueObject as clsGetAllItemListBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemsforAutocompleteSearch_New");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "IsFromSale", DbType.Boolean, nvo.IsFromSale);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            Code = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            isChecked = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])),
                            NonBatchItemBarcode = Convert.ToString(DALHelper.HandleDBNull(reader["NonBatchItemBarcode"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUM"])),
                            IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]))
                        };
                        nvo.MasterList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetAllItemTaxDetail(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetAllItemTaxDetailBizActionVO nvo = valueObject as clsGetAllItemTaxDetailBizActionVO;
            try
            {
                if (nvo.ItemTaxList == null)
                {
                    nvo.ItemTaxList = new List<clsItemTaxVO>();
                }
                nvo.ItemTaxList = this.GetItemTaxes(0L, nvo.StoreID, nvo.ApplicableFor);
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

        public override IValueObject GetBarCodeCounterSale(IValueObject valueObject, clsUserVO objUserVO)
        {
            clsCounterSaleBarCodeBizActionVO nvo = valueObject as clsCounterSaleBarCodeBizActionVO;
            nvo.ItemBatchListForBarCode = new List<clsItembatchSearchVO>();
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemBatchListForSearchForBarCode");
                this.dbServer.AddInParameter(storedProcCommand, "BarCode", DbType.String, nvo.BarCode);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItembatchSearchVO item = new clsItembatchSearchVO {
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                            Status = false,
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"])),
                            BUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])),
                            Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"])),
                            PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"])),
                            TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"])),
                            TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"])),
                            AssignSupplier = false,
                            CategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            GroupId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])),
                            ItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"])),
                            ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"])),
                            ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"])),
                            ConversionFactor = Convert.ToSingle(Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]))),
                            SGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTTax"])),
                            CGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTTax"])),
                            IGSTPercent = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTTax"])),
                            SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtype"])),
                            SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableon"])),
                            CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtype"])),
                            CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableon"])),
                            IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtype"])),
                            IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableon"])),
                            SGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SGSTPercentage"])),
                            CGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["CGSTPercentage"])),
                            IGSTPercentSale = Convert.ToDecimal(DALHelper.HandleDBNull(reader["IGSTPercentage"])),
                            SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"])),
                            SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"])),
                            CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"])),
                            CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"])),
                            IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"])),
                            IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"])),
                            PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"])),
                            BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"])),
                            StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"])),
                            PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"])),
                            StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"])),
                            Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"])),
                            Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"])),
                            Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"])),
                            ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"])),
                            OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"])),
                            IssueVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])),
                            IssueItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"])),
                            IssueItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"])),
                            IssueItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"])),
                            DiscountOnPackageItem = 0M,
                            StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"])),
                            WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"])),
                            RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"])),
                            ID = Convert.ToInt64(reader["ID"].HandleDBNull()),
                            StoreID = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            CurrentStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentStock"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ExpiryDate = DALHelper.HandleDate(reader["ExpiryDate"]),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            Date = Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])),
                            VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatPercentage"])),
                            VATAmt = Convert.ToDouble(DALHelper.HandleDBNull(reader["VatAmount"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"])),
                            AvailableStockInBase = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStockInBase"])),
                            PurchaseToBaseCFForStkAdj = 0f,
                            StockingToBaseCFForStkAdj = 0f
                        };
                        nvo.ItemBatchListForBarCode.Add(item);
                    }
                }
                reader.NextResult();
                int parameterValue = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return nvo;
        }

        public override IValueObject GetBinMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetBinMasterBizActionVO nvo = valueObject as GetBinMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetBinMaster");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ShelfID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetDOSReturnDetails(IValueObject valueObject, clsUserVO uservo)
        {
            return valueObject;
        }

        public clsItemMasterVO GetItem(long itemId)
        {
            DbDataReader reader = null;
            clsItemMasterVO rvo2;
            try
            {
                clsItemMasterVO rvo = new clsItemMasterVO();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetItemById");
                this.dbServer.AddInParameter(storedProcCommand, "ItemId", DbType.Int64, itemId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                while (true)
                {
                    if (!reader.Read())
                    {
                        reader.Close();
                        rvo2 = rvo;
                        break;
                    }
                    rvo.ItemCode = (reader["ItemCode"].HandleDBNull() == null) ? "" : ((string) reader["ItemCode"]);
                    rvo.ItemName = (reader["ItemName"].HandleDBNull() == null) ? "" : ((string) reader["ItemName"]);
                    rvo.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    rvo.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                    rvo.UnitID = (reader["UnitID"].HandleDBNull() == null) ? ((long) 0) : ((long) reader["UnitID"]);
                    rvo.BrandName = (string) DALHelper.HandleDBNull(reader["BrandName"]);
                    rvo.Strength = (string) DALHelper.HandleDBNull(reader["Strength"]);
                    rvo.MoleculeName = (long) DALHelper.HandleDBNull(reader["MoleculeName"]);
                    rvo.MoleculeNameString = (string) DALHelper.HandleDBNull(reader["MoleculeNameString"]);
                    rvo.ItemGroup = (long) DALHelper.HandleDBNull(reader["ItemGroup"]);
                    rvo.ItemGroupString = (string) DALHelper.HandleDBNull(reader["ItemGroupString"]);
                    rvo.ItemGroupString = (reader["ItemGroupString"].HandleDBNull() == null) ? "" : ((string) reader["ItemGroupString"]);
                    rvo.ItemCategory = (long) DALHelper.HandleDBNull(reader["ItemCategory"]);
                    rvo.ItemCategoryString = (reader["ItemCategoryString"].HandleDBNull() == null) ? "" : ((string) reader["ItemCategoryString"]);
                    rvo.DispencingType = (reader["DispencingType"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["DispencingType"]);
                    rvo.StoreageType = (reader["StoreageType"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["StoreageType"]);
                    rvo.PregClass = (reader["PregClass"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["PregClass"]);
                    rvo.TherClass = (reader["TherClass"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["TherClass"]);
                    rvo.MfgBy = (reader["MfgBy"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["MfgBy"]);
                    rvo.MfgByString = (reader["MfgByString"].HandleDBNull() == null) ? "" : ((string) reader["MfgByString"]);
                    rvo.MrkBy = (reader["MrkBy"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["MrkBy"]);
                    rvo.MrkByString = (reader["MrkByString"].HandleDBNull() == null) ? "" : ((string) reader["MrkByString"]);
                    rvo.PUM = (reader["PUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["PUM"]);
                    rvo.PUMString = (reader["PUMString"].HandleDBNull() == null) ? "" : ((string) reader["PUMString"]);
                    rvo.SUM = (reader["SUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["SUM"]);
                    rvo.ConversionFactor = (reader["ConversionFactor"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConversionFactor"]);
                    rvo.BaseUM = (reader["BaseUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["BaseUM"]);
                    rvo.SellingUM = (reader["SellingUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["SellingUM"]);
                    rvo.ConvFactStockBase = (reader["ConvFactStockBase"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConvFactStockBase"]);
                    rvo.ConvFactBaseSale = (reader["ConvFactBaseSale"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConvFactBaseSale"]);
                    rvo.Route = (reader["Route"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["Route"]);
                    rvo.PurchaseRate = (reader["PurchaseRate"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["PurchaseRate"]);
                    rvo.MRP = (reader["MRP"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["MRP"]);
                    rvo.VatPer = (reader["VatPer"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["VatPer"]);
                    rvo.ReorderQnt = (reader["ReorderQnt"].HandleDBNull() == null) ? 0 : Convert.ToInt32(reader["ReorderQnt"]);
                    rvo.BatchesRequired = (reader["BatchesRequired"].HandleDBNull() != null) && Convert.ToBoolean(reader["BatchesRequired"]);
                    rvo.InclusiveOfTax = (reader["InclusiveOfTax"].HandleDBNull() != null) && Convert.ToBoolean(reader["InclusiveOfTax"]);
                    rvo.DiscountOnSale = (reader["DiscountOnSale"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["DiscountOnSale"]);
                    rvo.ItemMarginID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemMarginID"]));
                    rvo.ItemMargin = (string) DALHelper.HandleDBNull(reader["ItemMargin"]);
                    rvo.ItemMovementID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemMovementID"]));
                    rvo.ItemMovement = (string) DALHelper.HandleDBNull(reader["ItemMovement"]);
                    rvo.MinStock = (reader["MinStock"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["MinStock"]);
                    rvo.Margin = (reader["Margin"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["Margin"]);
                    rvo.HighestRetailPrice = (reader["HighestRetailPrice"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["HighestRetailPrice"]);
                    rvo.StorageDegree = (reader["StorageDegree"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["StorageDegree"]);
                    rvo.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                }
            }
            catch (Exception)
            {
                if ((reader != null) && !reader.IsClosed)
                {
                    reader.Close();
                }
                throw;
            }
            return rvo2;
        }

        public override IValueObject GetItemBatchList(IValueObject valueObject, clsUserVO uservo)
        {
            DbDataReader reader = null;
            clsGetItemListForNewItemBatchMasterBizActionVO nvo = valueObject as clsGetItemListForNewItemBatchMasterBizActionVO;
            nvo.ItemList = new List<clsItemMasterVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListForSearchForNewform");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                if ((nvo.ItemName != null) && (nvo.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
                }
                if ((nvo.BatchCode != null) && (nvo.BatchCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BatchCode", DbType.String, nvo.BatchCode);
                }
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsItemMasterVO item = new clsItemMasterVO {
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            ExpiryDate = Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"])),
                            BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetItemClinicDetailList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemClinicDetailBizActionVO nvo = valueObject as clsGetItemClinicDetailBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemClinicDetail");
                this.dbServer.AddInParameter(storedProcCommand, "ItemClinicId", DbType.String, nvo.ItemClinicId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemTaxList == null)
                    {
                        nvo.ItemTaxList = new List<clsItemTaxVO>();
                    }
                    while (reader.Read())
                    {
                        Func<MasterListItem, bool> predicate = null;
                        Func<MasterListItem, bool> func2 = null;
                        clsItemTaxVO objItemMaster = new clsItemTaxVO {
                            ItemClinicDetailId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                            ApplicableOn = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"])) }
                        };
                        if (objItemMaster.ApplicableOnList != null)
                        {
                            if (predicate == null)
                            {
                                predicate = S => S.ID.Equals(objItemMaster.ApplicableOn.ID);
                            }
                            objItemMaster.ApplicableOn = objItemMaster.ApplicableOnList.SingleOrDefault<MasterListItem>(predicate);
                        }
                        objItemMaster.ApplicableFor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        if (objItemMaster.ApplicableForList != null)
                        {
                            if (func2 == null)
                            {
                                func2 = S => S.ID.Equals(objItemMaster.ApplicableFor.ID);
                            }
                            objItemMaster.ApplicableFor = objItemMaster.ApplicableForList.SingleOrDefault<MasterListItem>(func2);
                        }
                        objItemMaster.ItemClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemClinicId"]));
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        nvo.ItemTaxList.Add(objItemMaster);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetItemClinicList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemClinicBizActionVO nvo = valueObject as clsGetItemClinicBizActionVO;
            try
            {
                DbCommand storedProcCommand;
                if (nvo.ItemDetails.RetrieveDataFlag)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemClinicListByItemName");
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemDetails.ItemName);
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemClinicList");
                    this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemDetails.ItemID);
                    this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ItemDetails.UnitID);
                }
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO item = new clsItemMasterVO {
                            Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"])),
                            Min = Convert.ToDouble(DALHelper.HandleDBNull(reader["Min"])),
                            Max = Convert.ToDouble(DALHelper.HandleDBNull(reader["Max"])),
                            Re_Order = Convert.ToDouble(DALHelper.HandleDBNull(reader["Re_Order"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ClinicID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
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
            return valueObject;
        }

        public override IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemDetailsByIDBizActionVO nvo = valueObject as clsGetItemDetailsByIDBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("GetItemDetailsByID");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsItemMasterVO rvo = new clsItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                            Status = false,
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"])),
                            PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])),
                            Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"])),
                            PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"])),
                            TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"])),
                            TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                            ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])),
                            ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"])),
                            ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"])),
                            ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"])),
                            AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"])),
                            ItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["othertax"])),
                            ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"])),
                            ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"])),
                            ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"])),
                            OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]))
                        };
                        if (nvo.StoreID > 0L)
                        {
                            rvo.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            rvo.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                        }
                        rvo.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                        rvo.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                        rvo.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                        rvo.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                        rvo.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                        rvo.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                        rvo.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                        rvo.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        nvo.objItem = new clsItemMasterVO();
                        nvo.objItem = rvo;
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

        public override IValueObject GetItemEnquiry(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemEnquiryBizActionVO nvo = valueObject as clsGetItemEnquiryBizActionVO;
            clsItemEnquiryVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemsEnquiry");
                this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.Int64, nvo.SupplierId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, nvo.UserID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsItemEnquiryVO {
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            EnquiryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitId"])),
                            EnquiryNO = Convert.ToString(DALHelper.HandleDBNull(reader["EnquiryNO"])),
                            Supplier = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"]))
                        };
                        nvo.ItemMatserDetail.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetItemEnquiryDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemEnquiryDetailsBizActionVO nvo = valueObject as clsGetItemEnquiryDetailsBizActionVO;
            clsEnquiryDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemsEnquiryDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ItemEnquiryId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsEnquiryDetailsVO {
                            Id = Convert.ToInt64(DALHelper.HandleDBNull(reader["Id"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            Quantity = Convert.ToDouble(DALHelper.HandleDBNull(reader["Quantity"])),
                            UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"])),
                            Remarks = Convert.ToString(DALHelper.HandleDBNull(reader["Remarks"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"]))
                        };
                        nvo.ItemMatserDetail.Add(item);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListBizActionVO nvo = valueObject as clsGetItemListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand;
                if (nvo.FromEmr)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListMultipleFiltered");
                    this.dbServer.AddInParameter(storedProcCommand, "FilterGroupId", DbType.Int64, nvo.FilterIGroupID);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterCategoryId", DbType.Int64, nvo.FilterICatId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterDispensingId", DbType.Int64, nvo.FilterIDispensingId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterTherClassId", DbType.Int64, nvo.FilterITherClassId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterMolecule", DbType.Int64, nvo.FilterIMoleculeNameId);
                    this.dbServer.AddInParameter(storedProcCommand, "BrandName", DbType.String, nvo.BrandName);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterClinicId", DbType.Int64, nvo.FilterClinicId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterStoreId", DbType.Int64, nvo.FilterStoreId);
                    this.dbServer.AddInParameter(storedProcCommand, "ForReportFilter", DbType.Boolean, nvo.ForReportFilter);
                    this.dbServer.AddInParameter(storedProcCommand, "IsQtyShow", DbType.Boolean, nvo.IsQtyShow);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                }
                else if (nvo.ItemDetails.RetrieveDataFlag)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListByItemName");
                    if (nvo.ItemDetails != null)
                    {
                        this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemDetails.ItemName);
                        this.dbServer.AddInParameter(storedProcCommand, "ItemCode", DbType.String, nvo.ItemDetails.ItemCode);
                        this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                        this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                        this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                        this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                    }
                }
                else if (nvo.FilterCriteria == 0L)
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemList");
                }
                else
                {
                    storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListMultipleFiltered");
                    this.dbServer.AddInParameter(storedProcCommand, "FilterGroupId", DbType.Int64, nvo.FilterIGroupID);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterCategoryId", DbType.Int64, nvo.FilterICatId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterDispensingId", DbType.Int64, nvo.FilterIDispensingId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterTherClassId", DbType.Int64, nvo.FilterITherClassId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterMolecule", DbType.Int64, nvo.FilterIMoleculeNameId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterClinicId", DbType.Int64, nvo.FilterClinicId);
                    this.dbServer.AddInParameter(storedProcCommand, "FilterStoreId", DbType.Int64, nvo.FilterStoreId);
                    this.dbServer.AddInParameter(storedProcCommand, "ForReportFilter", DbType.Boolean, nvo.ForReportFilter);
                    this.dbServer.AddInParameter(storedProcCommand, "BrandName", DbType.String, nvo.BrandName);
                    this.dbServer.AddInParameter(storedProcCommand, "IsQtyShow", DbType.Boolean, nvo.IsQtyShow);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                }
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    if ((nvo.MasterList == null) && nvo.ForReportFilter)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        if (nvo.ForReportFilter)
                        {
                            MasterListItem item = new MasterListItem {
                                Code = (reader["Strength"].HandleDBNull() == null) ? "" : ((string) reader["Strength"]),
                                Description = (reader["BrandName"].HandleDBNull() == null) ? "" : ((string) reader["BrandName"]),
                                Name = (reader["ItemName"].HandleDBNull() == null) ? "" : ((string) reader["ItemName"]),
                                ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                                FilterID = (long) DALHelper.HandleDBNull(reader["Route"]),
                                Value = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                                PrintDescription = (reader["RouteName"].HandleDBNull() == null) ? "" : ((string) reader["RouteName"]),
                                StockQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStock"])),
                                UOM = (reader["UOM"].HandleDBNull() == null) ? "" : ((string) reader["UOM"]),
                                UOMID = Convert.ToInt64((reader["UOMID"].HandleDBNull() == null) ? "" : reader["UOMID"]),
                                Route = (reader["RouteName"].HandleDBNull() == null) ? "" : Convert.ToString(reader["RouteName"])
                            };
                            nvo.MasterList.Add(item);
                            continue;
                        }
                        clsItemMasterVO rvo = new clsItemMasterVO {
                            ItemCode = (reader["ItemCode"].HandleDBNull() == null) ? "" : ((string) reader["ItemCode"]),
                            ItemName = (reader["ItemName"].HandleDBNull() == null) ? "" : ((string) reader["ItemName"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            UnitID = (reader["UnitID"].HandleDBNull() == null) ? ((long) 0) : ((long) reader["UnitID"]),
                            BrandName = (string) DALHelper.HandleDBNull(reader["BrandName"]),
                            Strength = (string) DALHelper.HandleDBNull(reader["Strength"]),
                            MoleculeName = (long) DALHelper.HandleDBNull(reader["MoleculeName"]),
                            MoleculeNameString = (string) DALHelper.HandleDBNull(reader["MoleculeNameString"]),
                            ItemGroup = (long) DALHelper.HandleDBNull(reader["ItemGroup"]),
                            ItemGroupString = (string) DALHelper.HandleDBNull(reader["ItemGroupString"])
                        };
                        rvo.ItemGroupString = (reader["ItemGroupString"].HandleDBNull() == null) ? "" : ((string) reader["ItemGroupString"]);
                        rvo.ItemCategory = (long) DALHelper.HandleDBNull(reader["ItemCategory"]);
                        rvo.ItemCategoryString = (reader["ItemCategoryString"].HandleDBNull() == null) ? "" : ((string) reader["ItemCategoryString"]);
                        rvo.DispencingType = (reader["DispencingType"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["DispencingType"]);
                        rvo.StoreageType = (reader["StoreageType"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["StoreageType"]);
                        rvo.PregClass = (reader["PregClass"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["PregClass"]);
                        rvo.TherClass = (reader["TherClass"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["TherClass"]);
                        rvo.MfgBy = (reader["MfgBy"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["MfgBy"]);
                        rvo.MfgByString = (reader["MfgByString"].HandleDBNull() == null) ? "" : ((string) reader["MfgByString"]);
                        rvo.MrkBy = (reader["MrkBy"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["MrkBy"]);
                        rvo.MrkByString = (reader["MrkByString"].HandleDBNull() == null) ? "" : ((string) reader["MrkByString"]);
                        rvo.PUM = (reader["PUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["PUM"]);
                        rvo.PUMString = (reader["PUMString"].HandleDBNull() == null) ? "" : ((string) reader["PUMString"]);
                        rvo.SUM = (reader["SUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["SUM"]);
                        rvo.ConversionFactor = (reader["ConversionFactor"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConversionFactor"]);
                        rvo.BaseUM = (reader["BaseUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["BaseUM"]);
                        rvo.SellingUM = (reader["SellingUM"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["SellingUM"]);
                        rvo.ConvFactStockBase = (reader["ConvFactStockBase"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConvFactStockBase"]);
                        rvo.ConvFactBaseSale = (reader["ItemExpiredInDays"].HandleDBNull() == null) ? "" : Convert.ToString(reader["ConvFactBaseSale"]);
                        rvo.Route = (reader["Route"].HandleDBNull() == null) ? 0L : Convert.ToInt64(reader["Route"]);
                        rvo.PurchaseRate = (reader["PurchaseRate"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["PurchaseRate"]);
                        rvo.MRP = (reader["MRP"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["MRP"]);
                        rvo.VatPer = (reader["VatPer"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["VatPer"]);
                        rvo.ReorderQnt = (reader["ReorderQnt"].HandleDBNull() == null) ? 0 : Convert.ToInt32(reader["ReorderQnt"]);
                        rvo.BatchesRequired = (reader["BatchesRequired"].HandleDBNull() != null) && Convert.ToBoolean(reader["BatchesRequired"]);
                        rvo.InclusiveOfTax = (reader["InclusiveOfTax"].HandleDBNull() != null) && Convert.ToBoolean(reader["InclusiveOfTax"]);
                        rvo.DiscountOnSale = (reader["DiscountOnSale"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["DiscountOnSale"]);
                        rvo.ItemMarginID = DALHelper.HandleIntegerNull(reader["ItemMarginID"]);
                        rvo.ItemMargin = (string) DALHelper.HandleDBNull(reader["ItemMargin"]);
                        rvo.ItemMovementID = DALHelper.HandleIntegerNull(reader["ItemMovementID"]);
                        rvo.ItemMovement = (string) DALHelper.HandleDBNull(reader["ItemMovement"]);
                        rvo.MinStock = (reader["MinStock"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["MinStock"]);
                        rvo.Margin = (reader["Margin"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["Margin"]);
                        rvo.HighestRetailPrice = (reader["HighestRetailPrice"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["HighestRetailPrice"]);
                        rvo.StorageDegree = (reader["StorageDegree"].HandleDBNull() == null) ? 0M : Convert.ToDecimal(reader["StorageDegree"]);
                        rvo.BarCode = Convert.ToString(DALHelper.HandleDBNull(reader["BarCode"]));
                        rvo.IsABC = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsABC"]));
                        rvo.IsFNS = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFNS"]));
                        rvo.IsVED = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsVED"]));
                        rvo.StrengthUnitTypeID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StrengthUnitTypeID"]));
                        rvo.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        if (nvo.ItemDetails.RetrieveDataFlag)
                        {
                            rvo.HSNCodesID = Convert.ToInt64(DALHelper.HandleDBNull(reader["HSNCodesID"]));
                            rvo.StaffDiscount = (reader["StaffDiscount"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["StaffDiscount"]);
                            rvo.RegisteredPatientsDiscount = (reader["RegisteredPatientsDiscount"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["RegisteredPatientsDiscount"]);
                            rvo.WalkinDiscount = (reader["WalkinDiscount"].HandleDBNull() == null) ? 0.0 : Convert.ToDouble(reader["WalkinDiscount"]);
                        }
                        nvo.ItemList.Add(rvo);
                    }
                }
                reader.NextResult();
                if (nvo.ForReportFilter)
                {
                    nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
                if (!nvo.FromEmr && nvo.ItemDetails.RetrieveDataFlag)
                {
                    nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetItemListForTariffItemLink(IValueObject valueObject, clsUserVO userVO)
        {
            clsGetItemListBizActionVO nvo = valueObject as clsGetItemListBizActionVO;
            DbDataReader reader = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListForTariffLinking");
                if ((nvo.ItemDetails.ItemName != null) && (nvo.ItemDetails.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemDetails.ItemName);
                }
                if (nvo.ItemDetails.ItemCode != null)
                {
                    int length = nvo.ItemDetails.ItemCode.Length;
                    this.dbServer.AddInParameter(storedProcCommand, "ItemCode", DbType.String, nvo.ItemDetails.ItemCode);
                }
                long itemCategory = nvo.ItemDetails.ItemCategory;
                if (nvo.ItemDetails.ItemCategory != 0L)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemCategory", DbType.String, nvo.ItemDetails.ItemCategory);
                }
                else
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemCategory", DbType.String, null);
                }
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "operationType", DbType.Int64, nvo.OperationType);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsItemMasterVO item = new clsItemMasterVO {
                            ID = Convert.ToInt64(reader["ID"]),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryString"])),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["brandname"])),
                            PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            DeductiblePerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DeductablePer"])),
                            DiscountPerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["DiscountPer"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            return valueObject;
        }

        public override IValueObject GetItemOtherDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemMasterOtherDetailsBizActionVO nvo = valueObject as clsGetItemMasterOtherDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemOtherDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemOtherDetails == null)
                    {
                        nvo.ItemOtherDetails = new clsItemMasterOtherDetailsVO();
                    }
                    while (reader.Read())
                    {
                        nvo.ItemOtherDetails.ID = (long) DALHelper.HandleDBNull(reader["ID"]);
                        nvo.ItemOtherDetails.UnitID = (long) DALHelper.HandleDBNull(reader["UnitID"]);
                        nvo.ItemOtherDetails.ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]);
                        nvo.ItemOtherDetails.ItemName = (string) DALHelper.HandleDBNull(reader["ItemName"]);
                        nvo.ItemOtherDetails.Contradiction = (string) DALHelper.HandleDBNull(reader["Contradiction"]);
                        nvo.ItemOtherDetails.SideEffect = (string) DALHelper.HandleDBNull(reader["SideEffect"]);
                        nvo.ItemOtherDetails.URL = (string) DALHelper.HandleDBNull(reader["URL"]);
                    }
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
            return valueObject;
        }

        public override IValueObject GetItemsByItemCategoryStore(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetAllItemListBizActionVO nvo = valueObject as clsGetAllItemListBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemsByItemCategoryStore");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemCategory", DbType.Int64, nvo.ItemCategory);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO rvo = new clsItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"]))
                        };
                        MasterListItem item = new MasterListItem {
                            ID = rvo.ID,
                            Description = rvo.ItemName,
                            Code = rvo.ItemCode,
                            isChecked = rvo.BatchesRequired
                        };
                        nvo.MasterList.Add(item);
                    }
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetItemSearchList(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForSearchBizActionVO nvo = valueObject as clsGetItemListForSearchBizActionVO;
            try
            {
                DbCommand command = !nvo.IsFromOpeningBalance ? this.dbServer.GetStoredProcCommand("CIMS_GetItemListForSearch") : this.dbServer.GetStoredProcCommand("CIMS_GetItemListForOpeningBalanceSearch");
                this.dbServer.AddInParameter(command, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(command, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(command, "ID", DbType.Int64, nvo.ID);
                if ((nvo.ItemCode != null) && (nvo.ItemCode.Length != 0))
                {
                    this.dbServer.AddInParameter(command, "ItemCode", DbType.String, nvo.ItemCode);
                }
                if ((nvo.ItemName != null) && (nvo.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(command, "ItemName", DbType.String, nvo.ItemName);
                }
                if ((nvo.BrandName != null) && (nvo.BrandName.Length != 0))
                {
                    this.dbServer.AddInParameter(command, "BrandName", DbType.String, nvo.BrandName);
                }
                this.dbServer.AddInParameter(command, "CategoryID", DbType.Int64, nvo.ItemCategoryId);
                this.dbServer.AddInParameter(command, "ManufactureCompanyID", DbType.Int64, nvo.ManufactureCompanyID);
                this.dbServer.AddInParameter(command, "GroupId", DbType.Int64, nvo.ItemGroupId);
                this.dbServer.AddInParameter(command, "MoleculeName", DbType.Int64, nvo.MoleculeName);
                this.dbServer.AddInParameter(command, "ScrapCategoryItems", DbType.Boolean, nvo.ShowScrapItems);
                this.dbServer.AddInParameter(command, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(command, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(command, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(command, "ShowNonZeroStockBatches", DbType.Boolean, nvo.ShowZeroStockBatches);
                this.dbServer.AddInParameter(command, "PlusThreeMonthFlag", DbType.Boolean, nvo.ShowNotShowPlusThreeMonthExp);
                this.dbServer.AddInParameter(command, "PackageID", DbType.Int64, nvo.PackageID);
                if (!nvo.IsFromOpeningBalance)
                {
                    this.dbServer.AddInParameter(command, "PatientID", DbType.Int64, nvo.PatientID);
                    this.dbServer.AddInParameter(command, "PatientUnitID", DbType.Int64, nvo.PatientUnitID);
                }
                this.dbServer.AddOutParameter(command, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(command);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO item = new clsItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                            Status = false,
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"])),
                            PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])),
                            Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"])),
                            PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"])),
                            TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"])),
                            TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                            ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])),
                            ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]))
                        };
                        if (nvo.ShowZeroStockBatches)
                        {
                            item.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        }
                        item.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        if (nvo.IsFromOpeningBalance)
                        {
                            item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        }
                        if (nvo.StoreID != 0L)
                        {
                            item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        }
                        if (!nvo.IsFromOpeningBalance && (nvo.StoreID != 0L))
                        {
                            item.SGSTPercent = (decimal) DALHelper.HandleDBNull(reader["SGSTTax"]);
                            item.CGSTPercent = (decimal) DALHelper.HandleDBNull(reader["CGSTTax"]);
                            item.IGSTPercent = (decimal) DALHelper.HandleDBNull(reader["IGSTTax"]);
                            item.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                            item.SGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtype"]));
                            item.SGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableon"]));
                            item.CGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtype"]));
                            item.CGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableon"]));
                            item.IGSTtaxtype = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtype"]));
                            item.IGSTapplicableon = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableon"]));
                            item.SGSTPercentSale = (decimal) DALHelper.HandleDBNull(reader["SGSTTaxSale"]);
                            item.CGSTPercentSale = (decimal) DALHelper.HandleDBNull(reader["CGSTTaxSale"]);
                            item.IGSTPercentSale = (decimal) DALHelper.HandleDBNull(reader["IGSTTaxSale"]);
                            item.SGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTtaxtypeSale"]));
                            item.SGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["SGSTapplicableonSale"]));
                            item.CGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTtaxtypeSale"]));
                            item.CGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["CGSTapplicableonSale"]));
                            item.IGSTtaxtypeSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTtaxtypeSale"]));
                            item.IGSTapplicableonSale = Convert.ToInt32(DALHelper.HandleDBNull(reader["IGSTapplicableonSale"]));
                            item.TotalBatchAvailableStock = Convert.ToDecimal(DALHelper.HandleDBNull(reader["TotalBatchAvailableStock"]));
                            item.StaffDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["StaffDiscount"]));
                            item.WalkinDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["WalkinDiscount"]));
                            item.RegisteredPatientsDiscount = Convert.ToDouble(DALHelper.HandleDBNull(reader["RegisteredPatientsDiscount"]));
                        }
                        if (!nvo.IsFromOpeningBalance && ((nvo.StoreID != 0L) && !nvo.IsFromStockAdjustment))
                        {
                            item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            item.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                            item.ItemVatPer = (decimal) DALHelper.HandleDBNull(reader["othertax"]);
                            item.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                            item.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                            item.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                            item.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                            if (nvo.StoreID > 0L)
                            {
                                item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                                item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            }
                            item.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                            item.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                            item.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                            item.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                            item.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                            item.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                            item.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                            item.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                            if (!nvo.ShowZeroStockBatches)
                            {
                                item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                                item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                                item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                            }
                            if (!nvo.IsFromOpeningBalance && (nvo.PackageID > 0L))
                            {
                                item.DiscountPerc = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Discount"]));
                            }
                        }
                        if (!nvo.IsFromOpeningBalance)
                        {
                            item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            item.HSNCodes = Convert.ToString(DALHelper.HandleDBNull(reader["HSNCode"]));
                        }
                        else
                        {
                            item.StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"]));
                            item.ClinicName = Convert.ToString(DALHelper.HandleDBNull(reader["UnitName"]));
                            item.RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"]));
                            item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"]));
                            item.ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"]));
                            item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"]));
                            item.ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"]));
                            item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"]));
                        }
                        if (nvo.IsFromOpeningBalance && !nvo.ShowZeroStockBatches)
                        {
                            item.SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"]));
                            item.SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"]));
                            item.BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                            item.BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                            item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                            item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            item.InclusiveForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["InclusiveOfTax"]));
                            item.ApplicableOnForOB = Convert.ToInt32(DALHelper.HandleDBNull(reader["VATApplicableOn"]));
                            item.Re_Order = Convert.ToSingle(DALHelper.HandleDBNull(reader["Re_Order"]));
                        }
                        if (this.IsApprovedDirect.Equals("Yes"))
                        {
                            item.IsApprovedDirect = true;
                        }
                        else if (this.IsApprovedDirect.Equals("No"))
                        {
                            item.IsApprovedDirect = false;
                        }
                        item.AssignSupplier = this.CheckItemSupplier(item.ID, nvo.SupplierID);
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(command, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetItemSearchListForWorkOrder(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForWorkOrderBizActionVO nvo = valueObject as clsGetItemListForWorkOrderBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemListForWorkOrder");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                if ((nvo.ItemCode != null) && (nvo.ItemCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemCode", DbType.String, nvo.ItemCode);
                }
                if ((nvo.ItemName != null) && (nvo.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO item = new clsItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]))
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetItemsStoreLocationLock(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationLockBizActionVO nvo = valueObject as GetItemStoreLocationLockBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemsStoreLocaionLockDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ObjStoreLocationLockVO.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.ObjStoreLocationLockVO.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ObjStoreLocationLockVO.ShelfID);
                this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, nvo.ObjStoreLocationLockVO.ContainerID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StoreLocationLockDetailsList == null)
                    {
                        nvo.StoreLocationLockDetailsList = new List<ItemStoreLocationDetailsVO>();
                    }
                    while (reader.Read())
                    {
                        ItemStoreLocationDetailsVO item = new ItemStoreLocationDetailsVO {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["StoreID"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            RackID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["RackID"])),
                            Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["RackName"])),
                            ShelfID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ShelfID"])),
                            Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["ShelfName"])),
                            ContainerID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ContainerID"])),
                            Containername = Convert.ToString(DALHelper.HandleDBNull(reader["ContainerName"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"]))
                        };
                        nvo.StoreLocationLockDetailsList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetItemSupplierList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemSupplierBizActionVO nvo = valueObject as clsGetItemSupplierBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemSupplierList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemSupplier.ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemSupplierList == null)
                    {
                        nvo.ItemSupplierList = new List<clsItemSupllierVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemSupllierVO item = new clsItemSupllierVO {
                            status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ID = (long) DALHelper.HandleDBNull(reader["SuplierID"]),
                            SelectedHPLevel = { ID = (int) DALHelper.HandleDBNull(reader["HPLavel"]) },
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"])
                        };
                        nvo.ItemSupplierList.Add(item);
                    }
                }
                reader.NextResult();
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

        private List<clsItemTaxVO> GetItemTaxes(long ItemID, long StoreID, long ApplicableFor)
        {
            DbDataReader reader = null;
            List<clsItemTaxVO> list = new List<clsItemTaxVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemTaxDetail");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ApplicableFor", DbType.Int64, ApplicableFor);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Func<MasterListItem, bool> predicate = null;
                        Func<MasterListItem, bool> func2 = null;
                        clsItemTaxVO objItemMaster = new clsItemTaxVO {
                            status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                            ApplicableOn = { ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"])) }
                        };
                        if (objItemMaster.ApplicableOnList != null)
                        {
                            if (predicate == null)
                            {
                                predicate = S => S.ID.Equals(objItemMaster.ApplicableOn.ID);
                            }
                            objItemMaster.ApplicableOn = objItemMaster.ApplicableOnList.SingleOrDefault<MasterListItem>(predicate);
                        }
                        objItemMaster.ApplicableFor.ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"]));
                        if (objItemMaster.ApplicableForList != null)
                        {
                            if (func2 == null)
                            {
                                func2 = S => S.ID.Equals(objItemMaster.ApplicableFor.ID);
                            }
                            objItemMaster.ApplicableFor = objItemMaster.ApplicableForList.SingleOrDefault<MasterListItem>(func2);
                        }
                        objItemMaster.Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"]));
                        list.Add(objItemMaster);
                    }
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
            return list;
        }

        public override IValueObject GetItemTaxList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemTaxBizActionVO nvo = valueObject as clsGetItemTaxBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemTaxList");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.String, nvo.ItemTaxDetails.ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemTaxList == null)
                    {
                        nvo.ItemTaxList = new List<clsItemTaxVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemTaxVO item = new clsItemTaxVO {
                            status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"]),
                            ApplicableOn = { ID = (int) DALHelper.HandleDBNull(reader["AppOn"]) },
                            ApplicableFor = { ID = (int) DALHelper.HandleDBNull(reader["AppFor"]) },
                            TaxID = (long) DALHelper.HandleDBNull(reader["TaxID"]),
                            Percentage = (decimal) DALHelper.HandleDBNull(reader["Percentage"]),
                            ItemID = (long) DALHelper.HandleDBNull(reader["ItemID"]),
                            TaxName = (string) DALHelper.HandleDBNull(reader["Description"])
                        };
                        nvo.ItemTaxList.Add(item);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetMRPAdjustmentList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetMRPAdustmentListBizActionVO nvo = valueObject as clsGetMRPAdustmentListBizActionVO;
            nvo.AdjustmentList = new List<clsMRPAdjustmentVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMRPAdjustmentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "MRPAdjustmentID", DbType.Int64, nvo.MRPAdjustmentMainVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "MRPAdjustmentUnitID", DbType.Int64, nvo.MRPAdjustmentMainVO.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsMRPAdjustmentVO item = new clsMRPAdjustmentVO {
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"]))),
                            UpdatedBatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["UpdatedBatchCode"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            AdjustmentDate = Convert.ToDateTime(DALHelper.HandleDate(reader["AdjustmentDate"])),
                            MRPAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRPAdjustmentNo"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["PreviousMRP"])),
                            UpdatedMRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpdatedMRP"])),
                            PurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["PreviousPurchaseRate"])),
                            UpdatedPurchaseRate = Convert.ToDouble(DALHelper.HandleDBNull(reader["UpdatedPurchaseRate"])),
                            ExpiryDate = DALHelper.HandleDate(reader["PreExpiryDate"])
                        };
                        item.ExpiryDateString = (item.ExpiryDate == null) ? "" : item.ExpiryDate.Value.ToString("MMM-yyyy");
                        item.UpdatedExpiryDate = DALHelper.HandleDate(reader["UpdatedExpiryDate"]);
                        item.Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]));
                        item.stBatchUpdateStatus = Convert.ToString(DALHelper.HandleDBNull(reader["BatchUpdateStatus"]));
                        nvo.AdjustmentList.Add(item);
                    }
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

        public override IValueObject GetMRPAdjustmentMainList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetMRPAdustmentListBizActionVO nvo = valueObject as clsGetMRPAdustmentListBizActionVO;
            nvo.AdjustmentList = new List<clsMRPAdjustmentVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetMRPAdjustmentMain");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsMRPAdjustmentMainVO item = new clsMRPAdjustmentMainVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"]))),
                            MRPAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["MRPAdjustmentNo"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            IsApprove = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApprove"])),
                            IsReject = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsReject"])),
                            stApproveRejectBy = Convert.ToString(DALHelper.HandleDBNull(reader["ApproveOrRejecteBy"])),
                            ApproveRejectDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ApproveRejectDate"])))
                        };
                        nvo.AdjustmentMainList.Add(item);
                    }
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

        public override IValueObject GetPackageItemListForCounterSaleSearch(IValueObject valueObject, clsUserVO uservo)
        {
            clsGetItemListForSearchBizActionVO nvo = valueObject as clsGetItemListForSearchBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPackageItemListForCounterSaleSearch");
                this.dbServer.AddInParameter(storedProcCommand, "IdColumnName", DbType.String, "ID");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ID);
                if ((nvo.ItemCode != null) && (nvo.ItemCode.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemCode", DbType.String, nvo.ItemCode);
                }
                if ((nvo.ItemName != null) && (nvo.ItemName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "ItemName", DbType.String, nvo.ItemName);
                }
                if ((nvo.BrandName != null) && (nvo.BrandName.Length != 0))
                {
                    this.dbServer.AddInParameter(storedProcCommand, "BrandName", DbType.String, nvo.BrandName);
                }
                this.dbServer.AddInParameter(storedProcCommand, "CategoryID", DbType.Int64, nvo.ItemCategoryId);
                this.dbServer.AddInParameter(storedProcCommand, "GroupId", DbType.Int64, nvo.ItemGroupId);
                this.dbServer.AddInParameter(storedProcCommand, "MoleculeName", DbType.Int64, nvo.MoleculeName);
                this.dbServer.AddInParameter(storedProcCommand, "ScrapCategoryItems", DbType.Boolean, nvo.ShowScrapItems);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.IsPagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "ShowNonZeroStockBatches", DbType.Boolean, nvo.ShowZeroStockBatches);
                this.dbServer.AddInParameter(storedProcCommand, "PlusThreeMonthFlag", DbType.Boolean, nvo.ShowNotShowPlusThreeMonthExp);
                this.dbServer.AddInParameter(storedProcCommand, "PackageID", DbType.Int64, nvo.PackageID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO item = new clsItemMasterVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            BrandName = Convert.ToString(DALHelper.HandleDBNull(reader["BrandName"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            BatchesRequired = Convert.ToBoolean(DALHelper.HandleDBNull(reader["BatchesRequired"])),
                            Status = false,
                            SUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            StockUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SUM"])),
                            PurchaseUOM = Convert.ToString(DALHelper.HandleDBNull(reader["PUM"])),
                            SUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SUMID"])),
                            PUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["PUMID"])),
                            SellingUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["SellingUMID"])),
                            SellingUMString = Convert.ToString(DALHelper.HandleDBNull(reader["SellingUM"])),
                            BaseUM = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            BaseUMString = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"])),
                            InclusiveOfTax = Convert.ToBoolean(DALHelper.HandleDBNull(reader["InclusiveOfTax"])),
                            Manufacturer = Convert.ToString(DALHelper.HandleDBNull(reader["Manufacture"])),
                            PreganancyClass = Convert.ToString(DALHelper.HandleDBNull(reader["PreganancyClass"])),
                            TotalPerchaseTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["PurchaseTax"])),
                            TotalSalesTaxPercent = Convert.ToDouble(DALHelper.HandleDBNull(reader["SalesTax"])),
                            DiscountOnSale = Convert.ToDouble(DALHelper.HandleDBNull(reader["DiscountOnSale"])),
                            PurchaseRate = Convert.ToDecimal(DALHelper.HandleDBNull(reader["PurchaseRate"])),
                            VatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VatPer"])),
                            MRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["MRP"])),
                            ItemCategory = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemCategory"])),
                            ItemCategoryString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCategoryName"])),
                            ItemGroup = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemGroup"])),
                            ItemGroupString = Convert.ToString(DALHelper.HandleDBNull(reader["ItemGroupName"]))
                        };
                        if (nvo.ShowZeroStockBatches)
                        {
                            item.AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"]));
                        }
                        item.ConversionFactor = Convert.ToString(DALHelper.HandleDBNull(reader["ConversionFactor"]));
                        if (nvo.IsFromOpeningBalance)
                        {
                            item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                        }
                        if (nvo.StoreID != 0L)
                        {
                            item.IsItemBlock = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsItemBlock"]));
                        }
                        if (!nvo.IsFromOpeningBalance && ((nvo.StoreID != 0L) && !nvo.IsFromStockAdjustment))
                        {
                            item.ItemExpiredInDays = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemExpiredInDays"]));
                            item.AbatedMRP = Convert.ToDecimal(DALHelper.HandleDBNull(reader["AbatedMRP"]));
                            item.ItemVatPer = (decimal) DALHelper.HandleDBNull(reader["othertax"]);
                            item.ItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"]));
                            item.ItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["applicableon"]));
                            item.ItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxtype"]));
                            item.OtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherapplicableon"]));
                            if (nvo.StoreID > 0L)
                            {
                                item.PurchaseToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["PTBCF"]));
                                item.StockingToBaseCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["STBCF"]));
                            }
                            item.SVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["SVatPer"]));
                            item.SItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Staxtype"]));
                            item.SItemVatPer = Convert.ToDecimal(DALHelper.HandleDBNull(reader["Sothertax"]));
                            item.SItemOtherTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sothertaxtype"]));
                            item.SItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sapplicableon"]));
                            item.SOtherItemApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["Sotherapplicableon"]));
                            item.SellingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["SellingCF"]));
                            item.StockingCF = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                            if (!nvo.ShowZeroStockBatches)
                            {
                                item.Rackname = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"]));
                                item.Shelfname = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]));
                                item.Containername = Convert.ToString(DALHelper.HandleDBNull(reader["Container"]));
                            }
                        }
                        item.AssignSupplier = this.CheckItemSupplier(item.ID, nvo.SupplierID);
                        item.Budget = Convert.ToSingle(DALHelper.HandleDBNull(reader["Budget"]));
                        item.TotalBudget = Convert.ToSingle(DALHelper.HandleDBNull(reader["TotalBudget"]));
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPhysicalItemStock(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetPhysicalItemStockBizActionVO nvo = valueObject as clsGetPhysicalItemStockBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPhysicalStockItem");
                if (nvo.FromDate != null)
                {
                    this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                }
                if (nvo.ToDate != null)
                {
                    if (nvo.FromDate != null)
                    {
                        nvo.ToDate = new DateTime?(nvo.ToDate.Value.Date.AddDays(1.0));
                    }
                    this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                }
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int32, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int32, nvo.MaximumRows);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, UserVO.ID);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int32, 0x7fffffff);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemDetailsList == null)
                    {
                        nvo.ItemDetailsList = new List<clsPhysicalItemsMainVO>();
                    }
                    while (reader.Read())
                    {
                        clsPhysicalItemsMainVO item = new clsPhysicalItemsMainVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            RequestDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["RequestDateTime"]))),
                            RequestedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestedBy"])),
                            RequestedByName = Convert.ToString(DALHelper.HandleDBNull(reader["RequestedByName"])),
                            IsConvertedToSA = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsConvertedToSA"])),
                            PhysicalItemStockNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhysicalItemStockNo"]))
                        };
                        nvo.ItemDetailsList.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRowCount = (int) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            return valueObject;
        }

        public override IValueObject GetPhysicalItemStockDetails(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetPhysicalItemStockDetailsBizActionVO nvo = valueObject as clsGetPhysicalItemStockDetailsBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetPhysicalStockItemDetails");
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalItemID", DbType.Int64, nvo.PhysicalItemID);
                this.dbServer.AddInParameter(storedProcCommand, "PhysicalItemUnitID", DbType.Int64, nvo.PhysicalItemUnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemDetailsList == null)
                    {
                        nvo.ItemDetailsList = new List<clsPhysicalItemsVO>();
                    }
                    while (reader.Read())
                    {
                        clsPhysicalItemsVO item = new clsPhysicalItemsVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            AdjustmentQunatity = Convert.ToSingle(DALHelper.HandleDBNull(reader["AdjustmentQunatity"])),
                            AvailableStock = Convert.ToSingle(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            Container = Convert.ToString(DALHelper.HandleDBNull(reader["Container"])),
                            ContainerID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ContainerID"])),
                            intOperationType = Convert.ToInt32(DALHelper.HandleDBNull(reader["OperationType"])),
                            PhysicalItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemID"])),
                            PhysicalItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemUnitID"])),
                            PhysicalQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["PhysicalQuantity"])),
                            Rack = Convert.ToString(DALHelper.HandleDBNull(reader["Rack"])),
                            RackID = Convert.ToInt64(DALHelper.HandleDBNull(reader["RackID"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"])),
                            ShelfID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ShelfID"])),
                            Shelf = Convert.ToString(DALHelper.HandleDBNull(reader["Shelf"]))
                        };
                        item.intOperationTypeName = (item.intOperationType != 1) ? ((item.intOperationType != 2) ? InventoryStockOperationType.None.ToString() : InventoryStockOperationType.Subtraction.ToString()) : InventoryStockOperationType.Addition.ToString();
                        item.UOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UOMID"]));
                        item.BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"]));
                        item.StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockingUMID"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingCF"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["UOM"]));
                        item.BaseUM = Convert.ToString(DALHelper.HandleDBNull(reader["BaseUM"]));
                        item.StockingUM = Convert.ToString(DALHelper.HandleDBNull(reader["StockingUM"]));
                        item.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        item.AvailableStockInBase = Convert.ToDouble(DALHelper.HandleDBNull(reader["AvailableStockInBase"]));
                        item.SelectedUOM.Description = item.UOM;
                        item.SelectedUOM.ID = item.UOMID;
                        nvo.ItemDetailsList.Add(item);
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

        public override IValueObject GetRackMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetRackMasterBizActionVO nvo = valueObject as GetRackMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetRackMaster");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetScrapSalesDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetScrapSalesDetailsBizActionVO nvo = valueObject as clsGetScrapSalesDetailsBizActionVO;
            clsSrcapVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetScrapSaleDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UserID", DbType.Int64, userVO.ID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsSrcapVO {
                            Date = new DateTime?(Convert.ToDateTime(DALHelper.HandleDBNull(reader["Date"]))),
                            ScrapID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["SupplierName"])),
                            StoreName = Convert.ToString(DALHelper.HandleDBNull(reader["StoreName"])),
                            TotalAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["NetAmount"])),
                            TotalTaxAmount = Convert.ToDouble(DALHelper.HandleDBNull(reader["TotalTaxAmount"])),
                            ScrapSaleNo = Convert.ToString(DALHelper.HandleDBNull(reader["ScrapSaleNo"])),
                            IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"]))
                        };
                        item.IsApprovedStatus = !item.IsApproved ? "No" : "Yes";
                        item.ModeOfTransport = Convert.ToString(DALHelper.HandleDBNull(reader["ModeOfTransport"]));
                        nvo.MasterDetail.Add(item);
                    }
                }
                reader.NextResult();
                nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject GetScrapSalesItemsDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetScrapSalesItemsDetailsBizActionVO nvo = valueObject as clsGetScrapSalesItemsDetailsBizActionVO;
            clsSrcapDetailsVO item = null;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetScrapSaleItemsDetails");
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ItemScrapSaleId);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new clsSrcapDetailsVO {
                            ReceivedNo = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedNumber"])),
                            ReceivedDate = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ReceivedDate"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemId"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchExpiryDate = (DateTime?) DALHelper.HandleDBNull(reader["ExpiryDate"]),
                            ReceivedQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["ReceiveddQuantity"])),
                            ReceivedQtyUOM = Convert.ToString(DALHelper.HandleDBNull(reader["ReceivedQtyUOM"])),
                            SaleQty = Convert.ToDouble(DALHelper.HandleDBNull(reader["SaleQty"])),
                            TransactionUOM = Convert.ToString(DALHelper.HandleDBNull(reader["SaleQtyUOM"])),
                            Rate = Convert.ToDouble(DALHelper.HandleDBNull(reader["Rate"])),
                            MRP = Convert.ToDouble(DALHelper.HandleDBNull(reader["MRP"])),
                            ItemTax = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxPercentage"])),
                            OtherGRNItemTaxApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["othertaxApplicableon"])),
                            OtherGRNItemTaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["otherTaxType"])),
                            VATPerc = Convert.ToDouble(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            GRNItemVatApplicationOn = Convert.ToInt32(DALHelper.HandleDBNull(reader["VatApplicableon"])),
                            GRNItemVatType = Convert.ToInt32(DALHelper.HandleDBNull(reader["Vattype"])),
                            UnitId = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            ItemScrapSaleId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemScrapSaleId"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            BatchID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchID"])),
                            BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"])),
                            BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"])),
                            BaseUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUMID"])),
                            SUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"])),
                            ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]))
                        };
                        nvo.MasterDetail.Add(item);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetShelfMaster(IValueObject valueObject, clsUserVO UserVO)
        {
            GetShelfMasterBizActionVO nvo = valueObject as GetShelfMasterBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetShelfMaster");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.RackID);
                DbDataReader reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.MasterList == null)
                    {
                        nvo.MasterList = new List<MasterListItem>();
                    }
                    while (reader.Read())
                    {
                        MasterListItem item = new MasterListItem {
                            ID = Convert.ToInt64(DALHelper.HandleIntegerNull(reader["ID"])),
                            Description = Convert.ToString(DALHelper.HandleDBNull(reader["Description"]))
                        };
                        nvo.MasterList.Add(item);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject GetStockAdjustmentList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStockAdustmentListBizActionVO nvo = valueObject as clsGetStockAdustmentListBizActionVO;
            nvo.AdjustStock = new List<clsAdjustmentStockVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStockAdjustmentDetails");
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentID", DbType.Int64, nvo.StockAdjustmentID);
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentUnitID", DbType.Int64, nvo.StockAdjustmentUnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            break;
                        }
                        clsAdjustmentStockVO item = new clsAdjustmentStockVO {
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemID"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchiD"]))),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            AvailableStock = Convert.ToDouble(DALHelper.HandleDBNull(reader["CurrentBalance"])),
                            AdjustmentQunatitiy = Convert.ToDouble(DALHelper.HandleDBNull(reader["AdjustmentQuantity"])),
                            DateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Time"])))
                        };
                        item.Time = string.Format("{0:T}", item.DateTime);
                        item.DateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["Date"])));
                        item.StockAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["StockAdjustmentNo"]));
                        item.intOperationType = Convert.ToInt16(DALHelper.HandleDBNull(reader["OperationType"]));
                        item.StockingQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockingQuantity"]));
                        item.BaseQuantity = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseQuantity"]));
                        item.BaseUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["BaseUOMID"]));
                        item.BaseConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["BaseCF"]));
                        item.StockingUMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockUOMID"]));
                        item.ConversionFactor = Convert.ToSingle(DALHelper.HandleDBNull(reader["StockCF"]));
                        item.TransactionUOMID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TransactionUOMID"]));
                        item.UOM = Convert.ToString(DALHelper.HandleDBNull(reader["TransactionUOM"]));
                        item.stOperationType = (item.intOperationType != 1) ? ((item.intOperationType != 2) ? InventoryStockOperationType.None.ToString() : InventoryStockOperationType.Subtraction.ToString()) : InventoryStockOperationType.Addition.ToString();
                        nvo.AdjustStock.Add(item);
                    }
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

        public override IValueObject GetStockAdjustmentListMain(IValueObject valueObject, clsUserVO UserVO)
        {
            DbDataReader reader = null;
            clsGetStockAdustmentListMainBizActionVO nvo = valueObject as clsGetStockAdustmentListMainBizActionVO;
            nvo.AdjustStock = new List<clsAdjustmentStockMainVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStockAdjustmentDetailsMain");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "FromDate", DbType.DateTime, nvo.FromDate);
                this.dbServer.AddInParameter(storedProcCommand, "ToDate", DbType.DateTime, nvo.ToDate);
                this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (true)
                    {
                        if (!reader.Read())
                        {
                            reader.NextResult();
                            nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
                            break;
                        }
                        clsAdjustmentStockMainVO item = new clsAdjustmentStockMainVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            UnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["UnitID"])),
                            RequestDateTime = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["RequestDateTime"]))),
                            RequestedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["RequestedBy"])),
                            IsApproved = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsApproved"])),
                            ApprovedBy = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApprovedBy"])),
                            ApprovedDateTime = DALHelper.HandleDate(reader["ApprovedDateTime"]),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"])),
                            StockAdjustmentNo = Convert.ToString(DALHelper.HandleDBNull(reader["StockAdjustmentNo"])),
                            IsFromPST = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsFromPST"])),
                            PhysicalItemID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemID"])),
                            PhysicalItemUnitID = Convert.ToInt64(DALHelper.HandleDBNull(reader["PhysicalItemUnitID"])),
                            RequestedByName = Convert.ToString(DALHelper.HandleDBNull(reader["RequestedByName"])),
                            ApprovedByName = Convert.ToString(DALHelper.HandleDBNull(reader["ApprovedByName"])),
                            IsRejected = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsRejected"])),
                            Remark = Convert.ToString(DALHelper.HandleDBNull(reader["Remark"]))
                        };
                        nvo.AdjustStock.Add(item);
                    }
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

        public override IValueObject GetStockForStockAdjustment(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStockDetailsForStockAdjustmentBizActionVO nvo = valueObject as clsGetStockDetailsForStockAdjustmentBizActionVO;
            nvo.StockList = new List<clsAdjustmentStockVO>();
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetAvailableStockForStockAdjustment");
                this.dbServer.AddInParameter(storedProcCommand, "StoreId", DbType.Int64, nvo.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        clsAdjustmentStockVO item = new clsAdjustmentStockVO {
                            StockId = Convert.ToInt64(DALHelper.HandleDBNull(reader["StockId"])),
                            AvailableStock = Convert.ToInt64(DALHelper.HandleDBNull(reader["AvailableStock"])),
                            ExpiryDate = new DateTime?(Convert.ToDateTime(DALHelper.HandleDate(reader["ExpiryDate"]))),
                            ItemCode = Convert.ToString(DALHelper.HandleDBNull(reader["ItemCode"])),
                            ItemName = Convert.ToString(DALHelper.HandleDBNull(reader["ItemName"])),
                            ItemId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemId"])),
                            BatchCode = Convert.ToString(DALHelper.HandleDBNull(reader["BatchCode"])),
                            BatchId = new long?(Convert.ToInt64(DALHelper.HandleDBNull(reader["BatchId"]))),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreId"])),
                            Store = Convert.ToString(DALHelper.HandleDBNull(reader["Store"]))
                        };
                        nvo.StockList.Add(item);
                    }
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

        public override IValueObject GetStoreItemTaxList(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetItemStoreTaxListBizActionVO nvo = valueObject as clsGetItemStoreTaxListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetItemStoreTaxList");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.StoreItemTaxDetails.ID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.StoreItemTaxDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "IsForAllStore", DbType.Boolean, nvo.IsForAllStore);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.StoreItemTaxList == null)
                    {
                        nvo.StoreItemTaxList = new List<clsItemTaxVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemTaxVO item = new clsItemTaxVO {
                            ID = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                            TaxID = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxID"])),
                            TaxName = Convert.ToString(DALHelper.HandleDBNull(reader["TaxName"])),
                            ApplicableOnId = Convert.ToInt64(DALHelper.HandleDBNull(reader["VATApplicableOn"])),
                            ApplicableForId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ApplicableFor"])),
                            ApplicableOnDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableOnDesc"])),
                            ApplicableForDesc = Convert.ToString(DALHelper.HandleDBNull(reader["ApplicableForDesc"])),
                            ItemClinicId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ItemClinicId"])),
                            Percentage = Convert.ToDecimal(DALHelper.HandleDBNull(reader["VATPercentage"])),
                            status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                            StoreID = Convert.ToInt64(DALHelper.HandleDBNull(reader["StoreID"])),
                            TaxType = Convert.ToInt32(DALHelper.HandleDBNull(reader["taxtype"])),
                            ApplicableFrom = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApplicableFromDate"])),
                            ApplicableTo = Convert.ToDateTime(DALHelper.HandleDBNull(reader["ApplicableToDate"])),
                            IsGST = Convert.ToBoolean(DALHelper.HandleDBNull(reader["IsGST"]))
                        };
                        item.TaxTypeName = (item.TaxType != 1) ? "Exclusive" : "Inclusive";
                        nvo.StoreItemTaxList.Add(item);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetStores(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoresBizActionVO nvo = valueObject as clsGetStoresBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreByClinicID");
                this.dbServer.AddInParameter(storedProcCommand, "ClinicID", DbType.Int64, nvo.ItemDetails.ClinicID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemDetails.ItemID);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        clsItemMasterVO item = new clsItemMasterVO {
                            StoreName = (string) DALHelper.HandleDBNull(reader["Description"]),
                            Status = (bool) DALHelper.HandleDBNull(reader["Status"]),
                            ID = (long) DALHelper.HandleDBNull(reader["ID"])
                        };
                        nvo.ItemList.Add(item);
                    }
                }
                reader.NextResult();
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
            return valueObject;
        }

        public override IValueObject GetStoreStatus(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            clsGetStoreStatusBizActionVO nvo = valueObject as clsGetStoreStatusBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetStoreStatus");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ItemDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, userVO.UserLoginInfo.UnitId);
                reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                if (reader.HasRows)
                {
                    if (nvo.ItemList == null)
                    {
                        nvo.ItemList = new List<clsItemMasterVO>();
                    }
                    while (reader.Read())
                    {
                        nvo.ItemDetails = new clsItemMasterVO();
                        nvo.ItemDetails.Status = (bool) DALHelper.HandleDBNull(reader["Status"]);
                    }
                }
                reader.NextResult();
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

        public override IValueObject GetSupplierDetails(IValueObject valueObject, clsUserVO userVO)
        {
            DbDataReader reader = null;
            ClsGetSupplierDetailsBizActionVO nvo = valueObject as ClsGetSupplierDetailsBizActionVO;
            SupplierVO item = null;
            try
            {
                if (nvo.SupplierPaymentMode)
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSupplierPaymentMode");
                    this.dbServer.AddInParameter(storedProcCommand, "SupplierId", DbType.String, nvo.SupplierId);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            nvo.ModeOfPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModeofPayment"]));
                        }
                    }
                }
                else
                {
                    DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_GetSupplierDetails");
                    this.dbServer.AddInParameter(storedProcCommand, "SearchExpression", DbType.String, nvo.SearchExpression);
                    this.dbServer.AddInParameter(storedProcCommand, "PagingEnabled", DbType.Boolean, nvo.PagingEnabled);
                    this.dbServer.AddInParameter(storedProcCommand, "startRowIndex", DbType.Int64, nvo.StartRowIndex);
                    this.dbServer.AddInParameter(storedProcCommand, "maximumRows", DbType.Int64, nvo.MaximumRows);
                    this.dbServer.AddOutParameter(storedProcCommand, "TotalRows", DbType.Int64, 0x7fffffff);
                    reader = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            item = new SupplierVO {
                                SupplierId = Convert.ToInt64(DALHelper.HandleDBNull(reader["ID"])),
                                Code = Convert.ToString(DALHelper.HandleDBNull(reader["Code"])),
                                SupplierName = Convert.ToString(DALHelper.HandleDBNull(reader["Description"])),
                                Address1 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine1"])),
                                Address2 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine2"])),
                                Address3 = Convert.ToString(DALHelper.HandleDBNull(reader["AddressLine3"])),
                                City = Convert.ToInt64(DALHelper.HandleDBNull(reader["City"])),
                                State = Convert.ToInt64(DALHelper.HandleDBNull(reader["State"])),
                                Country = Convert.ToInt64(DALHelper.HandleDBNull(reader["Country"])),
                                District = Convert.ToInt64(DALHelper.HandleDBNull(reader["District"])),
                                Zone = Convert.ToInt64(DALHelper.HandleDBNull(reader["Area"])),
                                Area = Convert.ToInt64(DALHelper.HandleDBNull(reader["Area"])),
                                Pincode = Convert.ToString(DALHelper.HandleDBNull(reader["Pincode"])),
                                ContactPerson1Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1Name"])),
                                ContactPerson1MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1MobileNo"])),
                                ContactPerson1Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson1EmailId"])),
                                ContactPerson2Name = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2Name"])),
                                ContactPerson2MobileNo = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2MobileNo"])),
                                ContactPerson2Email = Convert.ToString(DALHelper.HandleDBNull(reader["ContactPerson2EmailId"])),
                                PhoneNo = Convert.ToString(DALHelper.HandleDBNull(reader["PhoneNo"])),
                                Fax = Convert.ToString(DALHelper.HandleDBNull(reader["Fax"])),
                                ModeOfPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["ModeofPayment"])),
                                TermofPayment = Convert.ToInt64(DALHelper.HandleDBNull(reader["TermofPayment"])),
                                TaxNature = Convert.ToInt64(DALHelper.HandleDBNull(reader["TaxNature"])),
                                Currency = Convert.ToInt64(DALHelper.HandleDBNull(reader["Currency"])),
                                MSTNumber = Convert.ToString(DALHelper.HandleDBNull(reader["MSTNumber"])),
                                CSTNumber = Convert.ToString(DALHelper.HandleDBNull(reader["CSTNumber"])),
                                VAT = Convert.ToString(DALHelper.HandleDBNull(reader["VATNumber"])),
                                DRUGLicence = Convert.ToString(DALHelper.HandleDBNull(reader["DrugLicenceNumber"])),
                                ServiceTaxNumber = Convert.ToString(DALHelper.HandleDBNull(reader["ServiceTaxNumber"])),
                                PANNumber = Convert.ToString(DALHelper.HandleDBNull(reader["PANNumber"])),
                                Status = Convert.ToBoolean(DALHelper.HandleDBNull(reader["Status"])),
                                GSTINNo = Convert.ToString(DALHelper.HandleDBNull(reader["GSTINNo"])),
                                Depreciation = Convert.ToString(DALHelper.HandleDBNull(reader["Depreciation"])),
                                RatingSystem = Convert.ToString(DALHelper.HandleDBNull(reader["RatingSystem"])),
                                SupplierCategoryId = Convert.ToInt64(DALHelper.HandleDBNull(reader["SupplierCategoryID"])),
                                POAutoCloseDays = Convert.ToInt32(DALHelper.HandleDBNull(reader["POAutoCloseDays"]))
                            };
                            nvo.ItemMatserDetails.Add(item);
                        }
                    }
                    reader.NextResult();
                    nvo.TotalRows = (long) this.dbServer.GetParameterValue(storedProcCommand, "TotalRows");
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

        public override IValueObject IsForCheckInTransitItems(IValueObject valueObject, clsUserVO UserVO)
        {
            clsGetBlockItemsListBizActionVO nvo = valueObject as clsGetBlockItemsListBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_IsForCheckInTransitItemsForSuspendStock");
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ItemDetails.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ItemDetails.ItemID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ItemDetails.UnitID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                DbDataReader reader1 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }

        public override IValueObject RejectStockAdjustment(IValueObject valueObject, clsUserVO UserVO)
        {
            clsUpdateStockAdjustmentBizActionVO nvo = valueObject as clsUpdateStockAdjustmentBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_RejectStockAdjustment");
                storedProcCommand.Connection = connection;
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentID", DbType.Int64, nvo.objMainStock.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentUnitID", DbType.Int64, nvo.objMainStock.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsRejected", DbType.Boolean, nvo.objMainStock.IsRejected);
                this.dbServer.AddInParameter(storedProcCommand, "RejectedBy", DbType.Int64, nvo.objMainStock.RejectedBy);
                this.dbServer.AddInParameter(storedProcCommand, "RejectedDateTime", DbType.DateTime, nvo.objMainStock.RejectedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "ResonForRejection", DbType.String, nvo.objMainStock.ResonForRejection);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.objMainStock = null;
            }
            finally
            {
                connection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject SetUnBlockRecords(IValueObject valueObject, clsUserVO UserVO)
        {
            GetItemStoreLocationLockBizActionVO nvo = valueObject as GetItemStoreLocationLockBizActionVO;
            DbConnection connection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_SetUnBlockRecords");
                this.dbServer.AddInParameter(storedProcCommand, "IsForMainRecord", DbType.Boolean, nvo.IsForMainRecord);
                this.dbServer.AddInParameter(storedProcCommand, "ID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "BlockTypeID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.BlockTypeID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.ShelfID);
                this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.ContainerID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemID", DbType.Int64, nvo.ObjStoreLocationLockDetailsVO.ItemID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = Convert.ToInt32(this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus"));
                if (nvo.SuccessStatus == -1)
                {
                    transaction.Rollback();
                }
                else if (nvo.SuccessStatus == 1)
                {
                    transaction.Commit();
                }
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            return nvo;
        }

        public override IValueObject UpdateStockAdjustment(IValueObject valueObject, clsUserVO UserVO)
        {
            clsUpdateStockAdjustmentBizActionVO nvo = valueObject as clsUpdateStockAdjustmentBizActionVO;
            DbConnection myConnection = this.dbServer.CreateConnection();
            DbTransaction transaction = null;
            try
            {
                myConnection.Open();
                transaction = myConnection.BeginTransaction();
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateStockAdjustment");
                storedProcCommand.Connection = myConnection;
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentID", DbType.Int64, nvo.objMainStock.ID);
                this.dbServer.AddInParameter(storedProcCommand, "StockAdjustmentUnitID", DbType.Int64, nvo.objMainStock.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "IsApproved", DbType.Boolean, nvo.objMainStock.IsApproved);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedBy", DbType.Int64, nvo.objMainStock.ApprovedBy);
                this.dbServer.AddInParameter(storedProcCommand, "ApprovedDateTime", DbType.DateTime, nvo.objMainStock.ApprovedDateTime);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedUnitID", DbType.Int64, UserVO.UserLoginInfo.UnitId);
                this.dbServer.AddInParameter(storedProcCommand, "Status", DbType.Boolean, true);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, UserVO.ID);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, UserVO.UserLoginInfo.MachineName);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, Convert.ToDateTime(DateTime.Now));
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedWindowsLoginName", DbType.String, UserVO.UserLoginInfo.WindowsUserName);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand, transaction);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
                foreach (clsAdjustmentStockVO kvo in nvo.StockAdustmentItems)
                {
                    kvo.StockAdjustmentNo = nvo.objMainStock.StockAdjustmentNo;
                    nvo.StockAdustmentItem.StockDetails.ItemID = kvo.ItemId;
                    nvo.StockAdustmentItem.StockDetails.BatchID = Convert.ToInt64(kvo.BatchId);
                    nvo.StockAdustmentItem.StockDetails.TransactionTypeID = InventoryTransactionType.StockAdujustment;
                    nvo.StockAdustmentItem.StockDetails.TransactionQuantity = kvo.BaseQuantity;
                    nvo.StockAdustmentItem.StockDetails.TransactionID = nvo.objMainStock.ID;
                    nvo.StockAdustmentItem.StockDetails.Date = Convert.ToDateTime(nvo.objMainStock.ApprovedDateTime);
                    nvo.StockAdustmentItem.StockDetails.Time = DateTime.Now;
                    nvo.StockAdustmentItem.StockDetails.OperationType = (InventoryStockOperationType) kvo.intOperationType;
                    nvo.StockAdustmentItem.StockDetails.StoreID = new long?(nvo.objMainStock.StoreID);
                    nvo.StockAdustmentItem.StockDetails.UnitId = nvo.objMainStock.UnitID;
                    nvo.StockAdustmentItem.StockDetails.BaseUOMID = kvo.BaseUMID;
                    nvo.StockAdustmentItem.StockDetails.SUOMID = kvo.StockingUMID;
                    nvo.StockAdustmentItem.StockDetails.BaseConversionFactor = kvo.BaseConversionFactor;
                    nvo.StockAdustmentItem.StockDetails.ConversionFactor = kvo.ConversionFactor;
                    nvo.StockAdustmentItem.StockDetails.StockingQuantity = kvo.AdjustmentQunatitiy * kvo.ConversionFactor;
                    nvo.StockAdustmentItem.StockDetails.SelectedUOM.ID = kvo.TransactionUOMID;
                    nvo.StockAdustmentItem.StockDetails.InputTransactionQuantity = Convert.ToSingle(kvo.AdjustmentQunatitiy);
                    clsBaseItemStockDAL instance = clsBaseItemStockDAL.GetInstance();
                    clsAddItemStockBizActionVO nvo2 = new clsAddItemStockBizActionVO {
                        Details = nvo.StockAdustmentItem.StockDetails
                    };
                    nvo2.Details.ID = 0L;
                    nvo2 = (clsAddItemStockBizActionVO) instance.Add(nvo2, UserVO, myConnection, transaction);
                    if (nvo2.SuccessStatus == -1)
                    {
                        throw new Exception();
                    }
                    nvo.StockAdustmentItem.StockDetails.ID = nvo2.Details.ID;
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                nvo.SuccessStatus = -1;
                nvo.objMainStock = null;
                nvo.StockAdustmentItems = null;
                nvo.StockAdustmentItem = null;
            }
            finally
            {
                myConnection.Close();
                transaction = null;
            }
            return valueObject;
        }

        public override IValueObject UpdateTariffItem(IValueObject valueObject, clsUserVO userVO)
        {
            DbTransaction transaction = null;
            clsAddTariffItemBizActionVO nvo = valueObject as clsAddTariffItemBizActionVO;
            try
            {
                DbCommand storedProcCommand = null;
                storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_UpdateTariffLinkingWithItems");
                storedProcCommand.Parameters.Clear();
                this.dbServer.AddInParameter(storedProcCommand, "ApplyForAll", DbType.Boolean, nvo.IsAllItemsSelected);
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "TariffID", DbType.Int64, nvo.TariffID);
                this.dbServer.AddInParameter(storedProcCommand, "ItemIDList", DbType.String, nvo.ItemIDList);
                this.dbServer.AddInParameter(storedProcCommand, "ItemDisList", DbType.String, nvo.ItemDisList);
                this.dbServer.AddInParameter(storedProcCommand, "ItemDedList", DbType.String, nvo.ItemDedList);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedBy", DbType.Int64, nvo.ItemMasterDetails.UpdatedBy);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedOn", DbType.String, nvo.ItemMasterDetails.UpdateddOn);
                this.dbServer.AddInParameter(storedProcCommand, "UpdatedDateTime", DbType.DateTime, DateTime.Now);
                this.dbServer.AddInParameter(storedProcCommand, "UpdateWindowsLoginName", DbType.String, nvo.ItemMasterDetails.UpdateWindowsLoginName);
                this.dbServer.AddOutParameter(storedProcCommand, "SucessStatus", DbType.Int32, 0x7fffffff);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "SucessStatus");
            }
            catch (Exception exception)
            {
                transaction.Rollback();
                throw exception;
            }
            finally
            {
                transaction = null;
            }
            return nvo;
        }

        public override IValueObject ValidationForDuplicateRecords(IValueObject valueObject, clsUserVO UserVO)
        {
            GetStoreLocationLockBizActionVO nvo = valueObject as GetStoreLocationLockBizActionVO;
            try
            {
                DbCommand storedProcCommand = this.dbServer.GetStoredProcCommand("CIMS_ValidationForDuplicateRecordsForSuspendStock");
                this.dbServer.AddInParameter(storedProcCommand, "UnitID", DbType.Int64, nvo.ObjStoreLocationLock.UnitID);
                this.dbServer.AddInParameter(storedProcCommand, "StoreID", DbType.Int64, nvo.ObjStoreLocationLock.StoreID);
                this.dbServer.AddInParameter(storedProcCommand, "RackID", DbType.Int64, nvo.ObjStoreLocationLock.RackID);
                this.dbServer.AddInParameter(storedProcCommand, "ShelfID", DbType.Int64, nvo.ObjStoreLocationLock.ShelfID);
                this.dbServer.AddInParameter(storedProcCommand, "ContainerID", DbType.Int64, nvo.ObjStoreLocationLock.ContainerID);
                this.dbServer.AddOutParameter(storedProcCommand, "ResultStatus", DbType.Int32, 0);
                DbDataReader reader1 = (DbDataReader) this.dbServer.ExecuteReader(storedProcCommand);
                this.dbServer.ExecuteNonQuery(storedProcCommand);
                nvo.SuccessStatus = (int) this.dbServer.GetParameterValue(storedProcCommand, "ResultStatus");
            }
            catch (Exception)
            {
                throw;
            }
            return nvo;
        }
    }
}

