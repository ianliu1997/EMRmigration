using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalashDynamics.ValueObjects.Inventory
{
    public class clsGetItemListBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private clsItemMasterVO objItemMater = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains OPDPatient Details Which is Added.
        /// </summary>
        public clsItemMasterVO ItemDetails
        {
            get { return objItemMater; }
            set { objItemMater = value; }
        }

        public List<clsItemMasterVO> ItemList { get; set; }

        public bool FromEmr { get; set; }
        public List<MasterListItem> MasterList { get; set; }

        public int TotalRowCount { get; set; }
        public int MaximumRows { get; set; }

        public Boolean PagingEnabled { get; set; }
        public int StartRowIndex { get; set; }
        public bool ListForTariffItemLink { get; set; }

        private long _OperationType;
        public long OperationType
        {
            get { return _OperationType; }
            set { _OperationType = value; }

        }

        private long _TariffID;
        public long TariffID
        {
            get { return _TariffID; }
            set { _TariffID = value; }

        }

        #region Filtering for Reports

        public long FilterCriteria { get; set; }
        public long FilterIGroupID { get; set; }
        public long FilterICatId { get; set; }
        public long FilterIDispensingId { get; set; }
        public long FilterITherClassId { get; set; }
        public long FilterIMoleculeNameId { get; set; }
        public string BrandName { get; set; }

        public bool ForReportFilter { get; set; }
        #region Added BY Pallavi
        public long FilterClinicId { get; set; }
        public long FilterStoreId { get; set; }
        #endregion

        public bool IsQtyShow { get; set; }

        #endregion

    }

    //public class clsGetWoItemListForSearchBizActionVO : IBizActionValueObject
    //{
    //    public string GetBizAction()
    //    {
    //        return "PalashDynamics.BusinessLayer.Inventory.clsGetWoItemListForSearchBizAction";
    //    }

    //    public string ToXml()
    //    {
    //        return this.ToXml();
    //    }


    //    private int _SuccessStatus;
    //    /// <summary>
    //    /// Output Property.
    //    /// This property states the outcome of BizAction Process.
    //    /// </summary>
    //    public int SuccessStatus
    //    {
    //        get { return _SuccessStatus; }
    //        set { _SuccessStatus = value; }
    //    }


    //    private clsItemMasterVO objItemMater = null;

    //    public int TotalRows { get; set; }
    //    public int StartIndex { get; set; }
    //    public int MaximumRows { get; set; }
    //    public bool IsPagingEnabled { get; set; }
    //    public bool ShowScrapItems { get; set; }


    //    public string ItemCode { get; set; }

    //    public string ItemName { get; set; }

    //    public string BrandName { get; set; }

    //    public long StoreID { get; set; }
    //    public long SupplierID { get; set; }

    //    public long ItemCategoryId { get; set; }

    //    public long ItemGroupId { get; set; }

    //    public long MoleculeName { get; set; }
    //    public long ID { get; set; }
    //    public List<clsItemMasterVO> ItemList { get; set; }

    //    public bool ShowZeroStockBatches { get; set; }  //to show items with > 0 AvailableStock

    //    public bool ShowNotShowPlusThreeMonthExp { get; set; }

    //    public bool fromStoreIndent { get; set; }
    //    //public List<clsItem

    //    public bool IsFromOpeningBalance { get; set; }

    //}

    public class clsGetItemListForWorkOrderBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListForWorkOrderBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }
        private int _SuccessStatus;
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private clsItemMasterVO objItemMater = null;

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        //public bool ShowScrapItems { get; set; }


        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        //public string BrandName { get; set; }

        public long StoreID { get; set; }
        public long SupplierID { get; set; }

        //public long ItemCategoryId { get; set; }

        //public long ItemGroupId { get; set; }

        //public long MoleculeName { get; set; }
        public long ID { get; set; }
        public List<clsItemMasterVO> ItemList { get; set; }

        //public bool ShowZeroStockBatches { get; set; }  //to show items with > 0 AvailableStock

        //public bool ShowNotShowPlusThreeMonthExp { get; set; }

        //public bool fromStoreIndent { get; set; }
        //public List<clsItem

        //public bool IsFromOpeningBalance { get; set; }

    }
    public class clsGetItemListForSearchBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListForSearchBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        private clsItemMasterVO objItemMater = null;

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool ShowScrapItems { get; set; }


        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string BrandName { get; set; }

        public long StoreID { get; set; }
        public long SupplierID { get; set; }

        public long ItemCategoryId { get; set; }

        public long ItemGroupId { get; set; }

        public long MoleculeName { get; set; }

        public long ManufactureCompanyID { get; set; }

        public long ID { get; set; }
        public List<clsItemMasterVO> ItemList { get; set; }

        public bool ShowZeroStockBatches { get; set; }  //to show items with > 0 AvailableStock

        public bool ShowNotShowPlusThreeMonthExp { get; set; }

        public bool fromStoreIndent { get; set; }
        //public List<clsItem

        public bool IsFromOpeningBalance { get; set; }
        public bool IsFromStockAdjustment { get; set; }

        public bool IsForPackageItemsSearchForCS { get; set; }  //set true on Package Item List Window.. from Counter Sale
        public long PackageID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
    }

    public class clsGetItemMasterOtherDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemOtherDetailBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public long ItemID { get; set; }

        private clsItemMasterOtherDetailsVO objItemMaterOtherDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Item Other Details.
        /// </summary>
        public clsItemMasterOtherDetailsVO ItemOtherDetails
        {
            get { return objItemMaterOtherDetails; }
            set { objItemMaterOtherDetails = value; }
        }
    }

    public class clsAddUpdateItemMasterOtherDetailsBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddUpdateItemOtherDetailBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private clsItemMasterOtherDetailsVO objItemMaterOtherDetails = null;
        /// <summary>
        /// Output Property.
        /// This Property Contains Item Other Details.
        /// </summary>
        public clsItemMasterOtherDetailsVO ItemOtherDetails
        {
            get { return objItemMaterOtherDetails; }
            set { objItemMaterOtherDetails = value; }
        }
    }

    public class clsGetAllItemListBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetAllItemListBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        private bool _IsItemsByStoreCategory = false;
        public bool IsItemByStoreCategory
        {
            get { return _IsItemsByStoreCategory; }
            set { _IsItemsByStoreCategory = value; }
        }

        private clsItemMasterVO objItemMater = null;

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool ShowScrapItems { get; set; }

        public string ItemCode { get; set; }

        public string ItemName { get; set; }

        public string BrandName { get; set; }

        public long StoreID { get; set; }

        public bool IsFromSale { get; set; }

        public long ItemCategory { get; set; }

        public long SupplierID { get; set; }

        public bool IsForautocompleteSearch { get; set; }

        public List<clsItemMasterVO> ItemList { get; set; }

        public List<MasterListItem> MasterList { get; set; }
    }

    public class clsGetAllGRNNumberBetweenDateRangeBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetAllGRNNumberBetweenDateRangeBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long SupplierID { get; set; }
        public List<clsItemMasterVO> ItemList { get; set; }

        public List<MasterListItem> MasterList { get; set; }
    }


    //Added by 
    public class clsGetItemListForNewItemBatchMasterBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemListForNewItemBatchMasterBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }

        public int TotalRows { get; set; }
        public int StartIndex { get; set; }
        public int MaximumRows { get; set; }
        public bool IsPagingEnabled { get; set; }

        public List<clsItemMasterVO> ItemList { get; set; }

        public string ItemName { get; set; }
        public long UnitID { get; set; }
        public string BatchCode { get; set; }




    }

    public class clsAddAssignedItemBarcodeBizActionVO : IBizActionValueObject
    {

        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsAddAssignedItemBarcodeBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }


        private int _SuccessStatus;
        /// <summary>
        /// Output Property.
        /// This property states the outcome of BizAction Process.
        /// </summary>
        public int SuccessStatus
        {
            get { return _SuccessStatus; }
            set { _SuccessStatus = value; }
        }


        public List<clsItemMasterVO> ItemList { get; set; }

        public string ItemName { get; set; }
        public long UnitID { get; set; }
        public string BatchCode { get; set; }
        public string BarCode { get; set; }




    }

    #region For Item Selection Control

    public class clsGetItemDetailsByIDBizActionVO : IBizActionValueObject
    {
        public string GetBizAction()
        {
            return "PalashDynamics.BusinessLayer.Inventory.clsGetItemDetailsByIDBizAction";
        }

        public string ToXml()
        {
            return this.ToXml();
        }

        public clsItemMasterVO objItem = null;

        public long ItemID { get; set; }

        public long StoreID { get; set; }

    }

    #endregion

}