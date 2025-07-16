namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    using com.seedhealthcare.hms.Web.ConfigurationManager;
    using PalashDynamics.ValueObjects;
    using System;

    public abstract class clsBaseItemMasterDAL
    {
        private static clsBaseItemMasterDAL _Instance;

        protected clsBaseItemMasterDAL()
        {
        }

        public abstract IValueObject AddItemBarCodefromAssigned(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddItemClinic(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemSupplier(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemTax(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddMRPAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddOpeningBalanceDetail(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddScrapSalesDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddStockAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddTariffItem(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject AddUpdateItemClinicDetail(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdateItemOtherDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdatePhysicalItemStock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdateSupplier(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject ApproveMRPAdjustment(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject ApproveScrapSalesDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject CheckForTaxExistance(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsAddItemsEnquiry(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject clsAddUpdateItemLocation(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsAddUpdateItemStoreLocationDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsAddUpdateStoreDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsAddUpdateStoreLocationLock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetBlockItemList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetGRNNumberBetweenDateRange(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetItemLocation(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetItemStoreLocationDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreLocationLock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreLocationLockForView(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreWithCategoryDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetValuesforScrapSalesbyItemID(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject clsItemListForSuspendStockSearch(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsUpdateCentralStoreDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsUpdateStoreStatus(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject DeleteItemStore(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject DeleteTax(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetAllItemList(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetAllItemTaxDetail(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetBarCodeCounterSale(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetBinMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetDOSReturnDetails(IValueObject valueObject, clsUserVO uservo);
        public static clsBaseItemMasterDAL GetInstance()
        {
            try
            {
                if (_Instance == null)
                {
                    string str = "Inventory.clsItemMasterDAL";
                    _Instance = (clsBaseItemMasterDAL) Activator.CreateInstance(Type.GetType(HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + str), true);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _Instance;
        }

        public abstract IValueObject GetItemBatchList(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemClinicDetailList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemClinicList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemEnquiry(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemEnquiryDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemListForTariffItemLink(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemOtherDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemsByItemCategoryStore(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemSearchList(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemSearchListForWorkOrder(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemsStoreLocationLock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemSupplierList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemTaxList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetMRPAdjustmentList(IValueObject valueObject, clsUserVO userVo);
        public abstract IValueObject GetMRPAdjustmentMainList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPackageItemListForCounterSaleSearch(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetPhysicalItemStock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPhysicalItemStockDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRackMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetScrapSalesDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetScrapSalesItemsDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetShelfMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetStockAdjustmentList(IValueObject valueObject, clsUserVO userVo);
        public abstract IValueObject GetStockAdjustmentListMain(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetStockForStockAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetStoreItemTaxList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetStores(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetStoreStatus(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetSupplierDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject IsForCheckInTransitItems(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject RejectStockAdjustment(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject SetUnBlockRecords(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject UpdateStockAdjustment(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject UpdateTariffItem(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject ValidationForDuplicateRecords(IValueObject valueObject, clsUserVO userVO);
    }
}

