using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.seedhealthcare.hms.Web.ConfigurationManager;
using PalashDynamics.ValueObjects;
namespace PalashDynamics.DataAccessLayer.BaseDataAccessLayer.Inventory
{
    public abstract class clsBaseItemMasterDAL
    {

        static private clsBaseItemMasterDAL _Instance = null;

        public static clsBaseItemMasterDAL GetInstance()
        {
            try
            {
                if (_Instance==null)
                {
                    string _DerivedClassName = "Inventory.clsItemMasterDAL";
                    string FullQualifiedDerivedClassName = HMSConfigurationManager.GetDataAccesslayerNameSpace() + "." + _DerivedClassName;
                    _Instance = (clsBaseItemMasterDAL)Activator.CreateInstance(Type.GetType(FullQualifiedDerivedClassName),true);

                }
            }
            catch (Exception ex)
            {
                
                throw;
            }
            return _Instance;
        }

        public abstract IValueObject AddItemMaster(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemSupplier(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemListForTariffItemLink(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemTax(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject CheckForTaxExistance(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemTaxList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemSupplierList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemClinicList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddItemClinic(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddOpeningBalanceDetail(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject DeleteItemStore(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetStoreStatus(IValueObject valueObject, clsUserVO uservo);
        //Manisha
        public abstract IValueObject GetItemSearchList(IValueObject valueObject, clsUserVO uservo);

        public abstract IValueObject GetPackageItemListForCounterSaleSearch(IValueObject valueObject, clsUserVO uservo);

        //***//19------
        public abstract IValueObject AddMultipleStoreTax(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemMapStoreList(IValueObject valueObject, clsUserVO userVO); 
        //------------------------------
        public abstract IValueObject AddOpeningBalance(IValueObject valueObject, clsUserVO uservo);

        #region Add by Shikha
        public abstract IValueObject AddUpdateSupplier(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetSupplierDetails(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsGetStoreDetails(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsGetStoreWithCategoryDetails(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsAddUpdateStoreDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsUpdateCentralStoreDetails(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsGetValuesforScrapSalesbyItemID(IValueObject valueObject, clsUserVO uservo);
        
        public abstract IValueObject AddScrapSalesDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject ApproveScrapSalesDetails(IValueObject valueObject, clsUserVO uservo); 
        public abstract IValueObject GetScrapSalesDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetScrapSalesItemsDetails(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetDOSReturnDetails(IValueObject valueObject, clsUserVO uservo);  // To get DOS Main List & Item List

        public abstract IValueObject clsAddItemsEnquiry(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemEnquiry(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemEnquiryDetails(IValueObject valueObject, clsUserVO uservo);

        public abstract IValueObject GetStockForStockAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddStockAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddMRPAdjustment(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetStockAdjustmentList(IValueObject valueObject, clsUserVO userVo);
        public abstract IValueObject GetMRPAdjustmentList(IValueObject valueObject, clsUserVO userVo);
        

     
        #endregion 


        public abstract IValueObject GetStores(IValueObject valueObject, clsUserVO userVO);

        #region Added by Harish
        public abstract IValueObject GetItemOtherDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdateItemOtherDetails(IValueObject valueObject, clsUserVO userVO);
        #endregion




        public abstract IValueObject GetBarCodeCounterSale(IValueObject valueObject, clsUserVO uservo);

        //Added By Somnath For New GRN
        public abstract IValueObject GetAllItemTaxDetail(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetItemsByItemCategoryStore(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetAllItemList(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject GetItemClinicDetailList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdateItemClinicDetail(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject GetStoreItemTaxList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetMultipleStoreItemTaxList(IValueObject valueObject, clsUserVO userVO);//***//19
        public abstract IValueObject DeleteMultipleStoreTax(IValueObject valueObject, clsUserVO userVO);//***//19 

        public abstract IValueObject clsAddUpdateItemLocation(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsGetItemLocation(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetGRNNumberBetweenDateRange(IValueObject valueObject, clsUserVO userVO);


        #region tariffItemLinking

        public abstract IValueObject AddTariffItem(IValueObject valueObject, clsUserVO objUserVO);
        public abstract IValueObject UpdateTariffItem(IValueObject valueObject, clsUserVO objUserVO);
        
        #endregion


        // added by 
        public abstract IValueObject GetItemBatchList(IValueObject valueObject, clsUserVO uservo);
        public abstract IValueObject AddItemBarCodefromAssigned(IValueObject valueObject, clsUserVO uservo);
        //added by akshays
        public abstract IValueObject clsAddUpdateItemStoreLocationDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetItemStoreLocationDetails(IValueObject valueObject, clsUserVO userVO);
        //closed by akshays
        //added by akshays on 5-11-2015
        public abstract IValueObject clsAddUpdateStoreLocationLock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreLocationLock(IValueObject valueObject, clsUserVO userVO);        
        //closed by akshays
        public abstract IValueObject GetItemsStoreLocationLock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject clsGetStoreLocationLockForView(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject ValidationForDuplicateRecords(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject SetUnBlockRecords(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetRackMaster(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject GetShelfMaster(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject GetBinMaster(IValueObject valueObject, clsUserVO userVO);


        
        //By Anjali............................
        public abstract IValueObject clsGetBlockItemList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject AddUpdatePhysicalItemStock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPhysicalItemStock(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetPhysicalItemStockDetails(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject GetStockAdjustmentListMain(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject UpdateStockAdjustment(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject RejectStockAdjustment(IValueObject valueObject, clsUserVO userVO);
        
        
        
        //.....................................

        // By Anumani 
        public abstract IValueObject GetItemSearchListForWorkOrder(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject DeleteTax(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsItemListForSuspendStockSearch(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject IsForCheckInTransitItems(IValueObject valueObject, clsUserVO userVO);

        public abstract IValueObject clsUpdateStoreStatus(IValueObject valueObject, clsUserVO userVO);

        #region For Item Selection Control

        public abstract IValueObject GetItemDetailsByID(IValueObject valueObject, clsUserVO userVO);

        #endregion

        public abstract IValueObject GetMRPAdjustmentMainList(IValueObject valueObject, clsUserVO userVO);
        public abstract IValueObject ApproveMRPAdjustment(IValueObject valueObject, clsUserVO userVO); 

    }
}
