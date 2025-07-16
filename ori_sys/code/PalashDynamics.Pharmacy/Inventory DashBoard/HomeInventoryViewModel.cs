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

using PalashDynamics.ValueObjects.Inventory;

using System.Collections.Generic;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using CIMS;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;



namespace PalashDynamics.Pharmacy.ViewModels
{
    public class HomeInventoryViewModel : ViewModelBase
    {

        #region Variables

        public bool isOrderBy;

        public PagedSortableCollectionView<clsItemStockVO> ItemStockList { get; private set; }
        public clsGetItemCurrentStockListBizActionVO BizActionObject { get; set; }

        public PagedSortableCollectionView<clsIndentMasterVO> ItemIndentList { get; private set; }
        public clsGetIndentListForInventorDashBoardBizActionVO BizActionObjectForIndent { get; set; }


        public PagedSortableCollectionView<clsPurchaseOrderVO> PurchaseOrderList { get; private set; }
        public clsGetPendingPurchaseOrderBizActionVO BizActionObjectForPO { get; set; }


        public PagedSortableCollectionView<clsExpiredItemReturnDetailVO> ExpiryItemList { get; private set; }
        public clsGetExpiryItemForDashBoardBizActionVO BizActionObjectForExpiryItemList { get; set; }

        public PagedSortableCollectionView<clsItemReorderDetailVO> ReorderItemList { get; private set; }
        public clsGetItemReorderQuantityBizActionVO BizActionObjectForReorder { get; set; }


        #endregion


        public int PageSize
        {
            get
            {
                return ItemIndentList.PageSize;
            }
            set
            {
                if (value == ItemIndentList.PageSize) return;
                ItemIndentList.PageSize = value;
                PurchaseOrderList.PageSize = value;
                ExpiryItemList.PageSize = value;
                ReorderItemList.PageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }
        public HomeInventoryViewModel()
        {
            BizActionObject = new clsGetItemCurrentStockListBizActionVO();
            ItemStockList = new PagedSortableCollectionView<clsItemStockVO>();
            ItemStockList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemStockList_OnRefresh);



            BizActionObjectForIndent = new clsGetIndentListForInventorDashBoardBizActionVO();
            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemIndentList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemIndentList_OnRefresh);



            BizActionObjectForPO = new clsGetPendingPurchaseOrderBizActionVO();
            PurchaseOrderList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            PurchaseOrderList.OnRefresh += new EventHandler<RefreshEventArgs>(PurchaseOrderList_OnRefresh);



            ExpiryItemList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();
            BizActionObjectForExpiryItemList = new clsGetExpiryItemForDashBoardBizActionVO();
            ExpiryItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ExpiryItemList_OnRefresh);

            ReorderItemList = new PagedSortableCollectionView<clsItemReorderDetailVO>();
            BizActionObjectForReorder = new clsGetItemReorderQuantityBizActionVO();
            ReorderItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ReorderItemList_OnRefresh);

            PageSize = 10;
            GetIndentList();
            GetExpiryList();
            GetPendingPurchaseOrder();
            GetItemCurrentStockList();
            GetItemReorderList();
        }

        void ReorderItemList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetItemReorderList();
            //throw new NotImplementedException();
        }

        public HomeInventoryViewModel(bool? isExpiryItemOrderBy)
        {
            BizActionObject = new clsGetItemCurrentStockListBizActionVO();
            ItemStockList = new PagedSortableCollectionView<clsItemStockVO>();
            ItemStockList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemStockList_OnRefresh);

            BizActionObjectForIndent = new clsGetIndentListForInventorDashBoardBizActionVO();
            BizActionObjectForIndent.IsOrderBy = isExpiryItemOrderBy;
            ItemIndentList = new PagedSortableCollectionView<clsIndentMasterVO>();
            ItemIndentList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemIndentList_OnRefresh);

            BizActionObjectForPO = new clsGetPendingPurchaseOrderBizActionVO();
            BizActionObjectForPO.IsOrderBy = isExpiryItemOrderBy;
            PurchaseOrderList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            PurchaseOrderList.OnRefresh += new EventHandler<RefreshEventArgs>(PurchaseOrderList_OnRefresh);

            ExpiryItemList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();
            BizActionObjectForExpiryItemList = new clsGetExpiryItemForDashBoardBizActionVO();
            BizActionObjectForExpiryItemList.IsOrderBy = isExpiryItemOrderBy;
            ExpiryItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ExpiryItemList_OnRefresh);

            ReorderItemList = new PagedSortableCollectionView<clsItemReorderDetailVO>();

            BizActionObjectForReorder = new clsGetItemReorderQuantityBizActionVO();
            BizActionObjectForReorder.IsOrderBy = isExpiryItemOrderBy;
            ReorderItemList.OnRefresh += new EventHandler<RefreshEventArgs>(ReorderItemList_OnRefresh);


            PageSize = 10;
            GetExpiryList();
            GetIndentList();
            GetPendingPurchaseOrder();
            GetItemCurrentStockList();
            GetItemReorderList();

        }

        void ExpiryItemList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetExpiryList();
        }

        void ItemIndentList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetIndentList();
        }

        void ItemStockList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //GetItemCurrentStockList();
        }

        void PurchaseOrderList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetPendingPurchaseOrder();
        }

        public void GetIndentList()
        {
            clsGetIndentListForInventorDashBoardBizActionVO BizAction = new clsGetIndentListForInventorDashBoardBizActionVO();
            BizAction.IndentList = new List<clsIndentMasterVO>();

            if (BizActionObjectForExpiryItemList.IsOrderBy != null)
                BizAction.IsOrderBy = BizActionObjectForIndent.IsOrderBy;

            BizAction.Date = null;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecords = ItemIndentList.PageSize;
            BizAction.StartRowIndex = ItemIndentList.PageIndex * ItemIndentList.PageSize;
            BizAction.IsIndent = true;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetIndentListForInventorDashBoardBizActionVO)arg.Result != null)
                    {
                        clsGetIndentListForInventorDashBoardBizActionVO result = arg.Result as clsGetIndentListForInventorDashBoardBizActionVO;
                        ItemIndentList.TotalItemCount = (int)result.TotalRow;
                        if (result.IndentList != null)
                        {
                            ItemIndentList.Clear();
                            foreach (var item in result.IndentList)
                            {
                                item.isIndentClosed = true;
                                ItemIndentList.Add(item);
                            }

                        }
                    }



                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        public void GetExpiryList()
        {
            clsGetExpiryItemForDashBoardBizActionVO BizAction = new clsGetExpiryItemForDashBoardBizActionVO();
            BizAction.ExpiredItemList = new List<clsExpiredItemReturnDetailVO>();

            if (BizActionObjectForExpiryItemList.IsOrderBy != null)
                BizAction.IsOrderBy = BizActionObjectForExpiryItemList.IsOrderBy;

            BizAction.Date = DateTime.Now;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitId = 0;
            }
            else
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.IsPaging = true;
            BizAction.NoOfRecordShow = ExpiryItemList.PageSize;
            BizAction.StartIndex = ExpiryItemList.PageIndex * ExpiryItemList.PageSize;
            BizAction.Day = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ItemExpiredIndays;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if ((clsGetExpiryItemForDashBoardBizActionVO)arg.Result != null)
                    {
                        clsGetExpiryItemForDashBoardBizActionVO result = arg.Result as clsGetExpiryItemForDashBoardBizActionVO;
                        ExpiryItemList.TotalItemCount = (int)result.TotalRow;
                        if (result.ExpiredItemList != null)
                        {
                            ExpiryItemList.Clear();
                            foreach (var item in result.ExpiredItemList)
                            {
                                ExpiryItemList.Add(item);
                            }

                        }
                    }



                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        public void GetItemCurrentStockList()
        {
            clsGetItemCurrentStockListBizActionVO BizAction = new clsGetItemCurrentStockListBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = PurchaseOrderList.PageSize;
            BizAction.StartIndex = PurchaseOrderList.PageIndex * PurchaseOrderList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsGetItemCurrentStockListBizActionVO result = arg.Result as clsGetItemCurrentStockListBizActionVO;
                    {
                        if (result.BatchList != null)
                            foreach (clsItemStockVO Item in result.BatchList)
                            {
                                ItemStockList.Add(Item);
                            }
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        public void GetPendingPurchaseOrder()
        {
            clsGetPendingPurchaseOrderBizActionVO BizAction = new clsGetPendingPurchaseOrderBizActionVO();
            BizAction.PurchaseOrderList = new List<clsPurchaseOrderVO>();
            BizAction.Date = null;

            if (BizActionObjectForExpiryItemList.IsOrderBy != null)
                BizAction.IsOrderBy = BizActionObjectForPO.IsOrderBy;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecordShow = PurchaseOrderList.PageSize;
            BizAction.StartIndex = PurchaseOrderList.PageIndex * PurchaseOrderList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if ((clsGetPendingPurchaseOrderBizActionVO)arg.Result != null)
                    {
                        clsGetPendingPurchaseOrderBizActionVO result = arg.Result as clsGetPendingPurchaseOrderBizActionVO;
                        PurchaseOrderList.TotalItemCount = (int)result.OutputTotalRows;
                        if (result.PurchaseOrderList != null)
                        {
                            PurchaseOrderList.Clear();
                            foreach (var item in result.PurchaseOrderList)
                            {
                                PurchaseOrderList.Add(item);
                            }

                        }
                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void GetItemReorderList()
        {
            clsGetItemReorderQuantityBizActionVO BizAction = new clsGetItemReorderQuantityBizActionVO();
            BizAction.ItemReorderList = new List<clsItemReorderDetailVO>();

            if (BizActionObjectForReorder.IsOrderBy != null)
                BizAction.IsOrderBy = BizActionObjectForReorder.IsOrderBy;

            //BizAction.Date = null;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.Unit = 0;
            }
            else
                BizAction.Unit = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            BizAction.IsPagingEnabled = true;
            BizAction.NoOfRecords = ReorderItemList.PageSize;
            BizAction.StartRowIndex = ReorderItemList.PageIndex * ReorderItemList.PageSize;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetItemReorderQuantityBizActionVO)arg.Result != null)
                    {
                        clsGetItemReorderQuantityBizActionVO result = arg.Result as clsGetItemReorderQuantityBizActionVO;
                        //result.TotalRow = result.ItemReorderList.Count;
                        ReorderItemList.TotalItemCount = (int)result.TotalRow;
                        if (result.ItemReorderList != null)
                        {
                            ReorderItemList.Clear();
                            foreach (var item in result.ItemReorderList)
                            {
                                ReorderItemList.Add(item);
                            }

                        }
                    }



                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


    }
}
