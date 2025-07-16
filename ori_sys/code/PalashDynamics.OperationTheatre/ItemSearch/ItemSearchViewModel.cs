using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Collections;

using PalashDynamics.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel;
using CIMS;

using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;


namespace PalashDynamics.OperationTheatre.ViewModels
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
            PageSize =5;
            BatchPageSize = 5;
            //BizActionObject.VisitFromDate = DateTime.Now.Date;
           //BizActionObject.VisitToDate = DateTime.Now.Date;
           // GetData();


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
        public long SelectedItemID { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }

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
        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        public void GetData()
        {
            indicator.Show();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            BizActionObject.ShowScrapItems = ShowScrapItems;
            BizActionObject.MaximumRows = DataList.PageSize; ;
            BizActionObject.SupplierID = SupplierID;
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
                        SelectedItemID = DataList[0].ID;
                        if (SelectedItemID > 0)
                            GetBatches();
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

                 clsGetItemStockBizActionVO BizAction  = new clsGetItemStockBizActionVO();
                 BizAction.BatchList = new List<clsItemStockVO>();
                 BizAction.IsPagingEnabled = true;
                 BizAction.MaximumRows = BatchList.PageSize ;
                 BizAction.StartIndex = BatchList.PageIndex * BatchList.PageSize;
                 BizAction.StoreID = StoreID;
                 BizAction.ItemID = SelectedItemID;
                 BizAction.ShowExpiredBatches = ShowExpiredBatches;
                 BizAction.ShowZeroStockBatches = ShowZeroStockBatches;
                 

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
