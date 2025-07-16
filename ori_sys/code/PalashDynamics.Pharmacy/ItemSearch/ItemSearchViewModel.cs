using System;
using System.Windows;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Collections.Generic;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
//using PalashDynamic.Localization;


namespace PalashDynamics.Pharmacy.ViewModels
{
    /// <summary>
    /// Represents the people view model
    /// </summary>
    public class ItemSearchViewModel : ViewModelBase
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="PeopleViewModel"/> class.
        /// </summary>
        public ItemSearchViewModel()
        {
            BizActionObject = new clsGetItemListForSearchBizActionVO();
            BizActionObject.IsPagingEnabled = true;
            // BizActionObject. = true;

            DataList = new PagedSortableCollectionView<clsItemMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            BatchList = new PagedSortableCollectionView<clsItemStockVO>();
            BatchList.OnRefresh += new EventHandler<RefreshEventArgs>(BatchList_OnRefresh);
            // get from database on first call
            PageSize = 5;
            BatchPageSize = 5;
            //BizActionObject.VisitFromDate = DateTime.Now.Date;
            //BizActionObject.VisitToDate = DateTime.Now.Date;
            // GetData();


            /******* Work Order ***************/
            BizActionObjectWO = new clsGetItemListForWorkOrderBizActionVO();
            BizActionObjectWO.IsPagingEnabled = true;
            // BizActionObject. = true;

            DataList1 = new PagedSortableCollectionView<clsItemMasterVO>();
            DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh1);

            // get from database on first call



        }
        void DataList_OnRefresh1(object sender, RefreshEventArgs e)
        {

        }
        public clsUserVO loggedinUser { get; set; }
        void BatchList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //throw new NotImplementedException();
            GetBatches();
        }


        WaitIndicator indicator = new WaitIndicator();

        #endregion

        #region Propertiesh
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<clsItemMasterVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsItemMasterVO> DataList1 { get; private set; }

        public PagedSortableCollectionView<clsItemStockVO> BatchList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }

        public long StoreID { get; set; }
        public long SupplierID { get; set; }
        public string DrugName { get; set; }
        public long SelectedItemID { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }
        public bool IsFromOpeningBalance { get; set; }
        public bool ShowNonZeroStockBatchesSetFromCounterSale { get; set; }
        public bool IsFromStockAdjustment { get; set; }
        public bool IsForPackageItemsSearchForCS { get; set; }  //set true on Package Item List Window.. from Counter Sale
        public long PackageID { get; set; }

        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }

        public bool ShowNotShowPlusThreeMonthExp { get; set; }
        public bool ShowPlusThreeMonthExpSetFromCounterSale { get; set; }

        public int BatchPageSize
        {
            get
            {
                return BatchList.PageSize;
            }
            set
            {
                if (value == BatchList.PageSize) return;
                BatchList.PageSize = value;
                RaisePropertyChanged("BatchPageSize");
            }
        }

        public bool IsItemBlocked { get; set; }
        #endregion

        #region Get Data
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();

        }

        public clsGetItemListForSearchBizActionVO BizActionObject { get; set; }
        public clsGetItemListForWorkOrderBizActionVO BizActionObjectWO { get; set; }


        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        public void GetDataWorkOrder()
        {
            indicator.Show();
            BizActionObjectWO.ItemList = new List<clsItemMasterVO>();

            BizActionObjectWO.MaximumRows = DataList1.PageSize; ;
            //BizActionObjectWO.SupplierID = SupplierID;


            BizActionObjectWO.StartIndex = DataList1.PageIndex * DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemListForWorkOrderBizActionVO result = ea.Result as clsGetItemListForWorkOrderBizActionVO;
                    DataList1.Clear();
                    DataList1.TotalItemCount = result.TotalRows;
                    //BatchList1.Clear();
                    //BatchList.TotalItemCount = 0;
                    foreach (clsItemMasterVO item in result.ItemList)
                    {
                        DataList1.Add(item);
                    }
                    if (DataList1.Count > 0)
                    {
                        SelectedItemID = DataList1[0].ID;
                        //if (SelectedItemID > 0)
                        //    GetBatches();
                    }

                }
                indicator.Close();
            };
            client.ProcessAsync(BizActionObject, loggedinUser);
            client.CloseAsync();
        }
        public void GetData()
        {
            indicator.Show();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            BizActionObject.ShowScrapItems = ShowScrapItems;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.SupplierID = SupplierID;

            BizActionObject.ShowZeroStockBatches = ShowZeroStockBatches;  //to show items with > 0 AvailableStock
            BizActionObject.IsFromOpeningBalance = IsFromOpeningBalance;
            BizActionObject.IsFromStockAdjustment = IsFromStockAdjustment;
            BizActionObject.ShowNotShowPlusThreeMonthExp = ShowNotShowPlusThreeMonthExp;
            BizActionObject.IsForPackageItemsSearchForCS = IsForPackageItemsSearchForCS;
            BizActionObject.PackageID = this.PackageID;

            BizActionObject.PatientID = this.PatientID;
            BizActionObject.PatientUnitID = this.PatientUnitID;

            BizActionObject.StartIndex = DataList.PageIndex * DataList.PageSize;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemListForSearchBizActionVO result = ea.Result as clsGetItemListForSearchBizActionVO;
                    DataList.Clear();
                    DataList.TotalItemCount = result.TotalRows;
                    BatchList.Clear();
                    BatchList.TotalItemCount = 0;
                    foreach (clsItemMasterVO item in result.ItemList)
                    {
                        DataList.Add(item);
                    }
                    if (DataList.Count > 0)
                    {
                        if (DataList[0].IsItemBlock == false)
                        {
                            SelectedItemID = DataList[0].ID;
                            if (SelectedItemID > 0)
                                GetBatches();
                        }
                    }

                }
                indicator.Close();
            };
            client.ProcessAsync(BizActionObject, loggedinUser);
            client.CloseAsync();
        }



        public void GetBatches()
        {


            indicator.Show();

            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = BatchList.PageSize;
            BizAction.StartIndex = BatchList.PageIndex * BatchList.PageSize;
            BizAction.StoreID = StoreID;
            BizAction.ItemID = SelectedItemID;
            BizAction.ShowExpiredBatches = ShowExpiredBatches;

            //BizAction.ShowZeroStockBatches = ShowZeroStockBatches;

            if (ShowNonZeroStockBatchesSetFromCounterSale == true)
            {
                BizAction.ShowZeroStockBatches = false;
            }
            else if(this.IsFromStockAdjustment)
            {
                BizAction.ShowZeroStockBatches = true;   //to show items with all available Stock  added by Ashish Z. on Dated 20092016
            }
            else
            {
                BizAction.ShowZeroStockBatches = ShowZeroStockBatches;   //to show items with > 0 AvailableStock
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                    BatchList.Clear();
                    BatchList.TotalItemCount = result.TotalRows;
                    foreach (clsItemStockVO item in result.BatchList)
                    {
                        if (item.ConversionFactor > 1)
                        {
                            item.MRP = item.MRP; //item.MRP / item.ConversionFactor;
                            item.PurchaseRate =item.PurchaseRate; //item.PurchaseRate / item.ConversionFactor;
                        }
                        BatchList.Add(item);
                    }
                    if (this.IsItemBlocked)
                    {
                        //BatchList = new PagedSortableCollectionView<clsItemStockVO>();
                        BatchList.Clear();
                    }
                }
                indicator.Close();
            };
            client.ProcessAsync(BizAction, loggedinUser);
            client.CloseAsync();
        }

        public void GetBatches(ObservableCollection<clsItemStockVO> ItemBatchList)
        {

            indicator.Show();

            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = BatchList.PageSize;
            BizAction.StartIndex = BatchList.PageIndex * BatchList.PageSize;
            BizAction.StoreID = StoreID;
            BizAction.ItemID = SelectedItemID;
            BizAction.ShowExpiredBatches = ShowExpiredBatches;

            //BizAction.ShowZeroStockBatches = ShowZeroStockBatches;

            if (ShowNonZeroStockBatchesSetFromCounterSale == true)
            {
                BizAction.ShowZeroStockBatches = false;
            }
            else
            {
                BizAction.ShowZeroStockBatches = ShowZeroStockBatches;   //to show items with > 0 AvailableStock
            }


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                    BatchList.Clear();
                    BatchList.TotalItemCount = result.TotalRows;
                    foreach (clsItemStockVO item in result.BatchList)
                    {
                        foreach (clsItemStockVO item1 in ItemBatchList)
                        {
                            if (item.BatchID == item1.BatchID && item1.CompoundDrugMaster != null && item.ItemID == item1.ItemID)
                                item.Status = true;
                        }
                        if (item.ConversionFactor > 1)
                        {
                            item.MRP = item.MRP / item.ConversionFactor;
                            item.PurchaseRate = item.PurchaseRate / item.ConversionFactor;
                        }
                        BatchList.Add(item);
                    }
                }
                indicator.Close();
            };
            client.ProcessAsync(BizAction, loggedinUser);
            client.CloseAsync();
        }
        #endregion
    }
    }
             
             