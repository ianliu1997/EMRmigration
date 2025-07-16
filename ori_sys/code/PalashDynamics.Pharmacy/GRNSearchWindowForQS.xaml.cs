using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Linq;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy
{
    public partial class GRNSearchWindowForQS : ChildWindow
    {
        #region Public Variable Declarations
        public long StoreID { get; set; }
        private ObservableCollection<clsGRNReturnDetailsVO> _GRNReturnAddedItems;
        public ObservableCollection<clsGRNReturnDetailsVO> GRNReturnAddedItems { get { return _GRNReturnAddedItems; } set { _GRNReturnAddedItems = value; } }
        public event RoutedEventHandler OnSaveButton_Click;

        private ObservableCollection<clsGRNDetailsVO> _SelectedGrnItems;
        public ObservableCollection<clsGRNDetailsVO> SelectedGrnItems { get { return _SelectedGrnItems; } }

        private List<clsItemStockVO> _BatchList;
        public List<clsItemStockVO> BatchList { get { return _BatchList; } }

        public bool IsSearchForGRNReturn { get; set; }
        int ClickedFlag = 0;
        WaitIndicator objWIndicator = new WaitIndicator();
        #endregion

        #region 'Paging'

        public PagedSortableCollectionView<clsGRNDetailsVO> DataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
            }
        }


        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillGRNDetailslList();
        }

        #endregion

        #region Constructor and Loaded
        public GRNSearchWindowForQS()
        {
            InitializeComponent();
            _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();
            _GRNReturnAddedItems.Clear();
            dtpGRNFromDate.SelectedDate = DateTime.Now;
            dtpGRNToDate.SelectedDate = DateTime.Now;

            //Paging======================================================
            DataList = new PagedSortableCollectionView<clsGRNDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
            objWIndicator = new WaitIndicator();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillStores();
            FillSupplier();
        }
        #endregion

        #region Clicked Events

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;

            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
            else
                ClickedFlag = 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkGrnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedItems.SelectedItem != null)
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == false)
                {
                    _SelectedGrnItems.Remove(dgSelectedItems.SelectedItem as clsGRNDetailsVO);
                }
                dgSelectedItems.ItemsSource = null;
                dgSelectedItems.ItemsSource = SelectedGrnItems;



            }
            //if (dgGRNSearchItems.SelectedItem != null)
            //{
            //    if (_GRNReturnAddedItems == null)
            //        _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();
            //    CheckBox chk = (CheckBox)sender;
            //    if (chk.IsChecked == true)

            //        _SelectedGrnItems.Add((clsGRNDetailsVO)dgGRNSearchItems.SelectedItem);
            //    else
            //        _SelectedGrnItems.Remove((clsGRNDetailsVO)dgGRNSearchItems.SelectedItem);

            //}
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillGRNDetailslList();
            DataPager.PageIndex = 0;
            if (_BatchList != null)
            {
                _BatchList.Clear();
                _BatchList = new List<clsItemStockVO>();
            }
            dgGRNItemBatches.ItemsSource = null;
            dgGRNItemBatches.ItemsSource = BatchList;
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNItemBatches.SelectedItem != null)
            {
                if ((dgGRNItemBatches.SelectedItem as clsItemStockVO).AvailableStock > 0)
                {
                    CheckBox chk = (CheckBox)sender;
                    if (_SelectedGrnItems == null)
                        _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();
                    if (chk.IsChecked == true)
                    {
                        var item2 = from r in _SelectedGrnItems
                                    where r.BatchID == ((clsItemStockVO)(dgGRNItemBatches.SelectedItem)).BatchID && r.ItemID == ((clsItemStockVO)(dgGRNItemBatches.SelectedItem)).ItemID
                                          && r.GRNUnitID == ((clsItemStockVO)(dgGRNItemBatches.SelectedItem)).UnitId  //r.GRNID == ((clsItemStockVO)(dgGRNItemBatches.SelectedItem)).TransactionID &&
                                    select r;

                        if (item2.ToList().Count > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();

                            chk.IsChecked = false;
                        }
                        else
                        {
                            clsGRNDetailsVO objSelectedGRNItems = new clsGRNDetailsVO();
                            if (dgGRNSearchList.SelectedItem != null)
                            {
                                objSelectedGRNItems.GRNDate = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNDate;
                                objSelectedGRNItems.GRNNo = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNNo;
                                objSelectedGRNItems.StoreName = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).StoreName;
                                objSelectedGRNItems.ItemName = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).ItemName;
                                objSelectedGRNItems.ItemCode = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).ItemCode;
                                objSelectedGRNItems.SupplierName = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SupplierName;
                                objSelectedGRNItems.Quantity = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).Quantity;
                                objSelectedGRNItems.TransUOM = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).TransUOM;
                                objSelectedGRNItems.PendingQuantity = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).PendingQuantity; //GRN QSPending Quantity
                                objSelectedGRNItems.Rate = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).Rate;
                                objSelectedGRNItems.MRP = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).MRP;
                                objSelectedGRNItems.GRNID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNID;
                                objSelectedGRNItems.GRNUnitID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNUnitID;
                                objSelectedGRNItems.GRNDetailID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNDetailID;
                                objSelectedGRNItems.GRNDetailUnitID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNDetailUnitID;
                                objSelectedGRNItems.StoreID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).StoreID;
                                objSelectedGRNItems.BatchID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).BatchID;
                                objSelectedGRNItems.ItemID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).ItemID;
                                objSelectedGRNItems.SupplierID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SupplierID;
                                objSelectedGRNItems.TransactionUOMID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).TransactionUOMID;
                                objSelectedGRNItems.TransUOM = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).TransUOM;
                                objSelectedGRNItems.SUOMID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SUOMID;
                                objSelectedGRNItems.SUOM = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SUOM;
                                objSelectedGRNItems.ConversionFactor = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).ConversionFactor;
                                objSelectedGRNItems.BaseUOMID = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).BaseUOMID;
                                objSelectedGRNItems.BaseUOM = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).BaseUOM;
                                objSelectedGRNItems.BaseConversionFactor = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).BaseConversionFactor;
                                objSelectedGRNItems.VATPercent = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).VATPercent;
                                if (!(objSelectedGRNItems.VATPercent > 0)) objSelectedGRNItems.VATAmount = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).VATAmount;
                                objSelectedGRNItems.CDiscountPercent = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).CDiscountPercent;
                                if (!(objSelectedGRNItems.CDiscountPercent > 0)) objSelectedGRNItems.CDiscountAmount = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).CDiscountAmount;
                                objSelectedGRNItems.SchDiscountPercent = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SchDiscountPercent;
                                if (!(objSelectedGRNItems.SchDiscountPercent > 0)) objSelectedGRNItems.SchDiscountAmount = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).SchDiscountAmount;
                                objSelectedGRNItems.ItemTax = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).ItemTax;
                                objSelectedGRNItems.TaxAmount = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).TaxAmount;
                                objSelectedGRNItems.GRNItemVatType = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNItemVatType;
                                objSelectedGRNItems.GRNItemVatApplicationOn = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).GRNItemVatApplicationOn;
                                objSelectedGRNItems.OtherGRNItemTaxType = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).OtherGRNItemTaxType;
                                objSelectedGRNItems.OtherGRNItemTaxApplicationOn = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).OtherGRNItemTaxApplicationOn;
                                objSelectedGRNItems.IsFreeItem = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).IsFreeItem;

                                objSelectedGRNItems.BatchCode = (dgGRNItemBatches.SelectedItem as clsItemStockVO).BatchCode;
                                objSelectedGRNItems.ExpiryDate = (dgGRNItemBatches.SelectedItem as clsItemStockVO).ExpiryDate;
                                objSelectedGRNItems.AvailableQuantity = (dgGRNItemBatches.SelectedItem as clsItemStockVO).AvailableStock;
                                objSelectedGRNItems.AvailableStockUOM = (dgGRNItemBatches.SelectedItem as clsItemStockVO).StockingUOM;
                                //objSelectedGRNItems.Rate = Convert.ToSingle((dgGRNItemBatches.SelectedItem as clsItemStockVO).PurchaseRate);
                                //objSelectedGRNItems.MRP = Convert.ToSingle((dgGRNItemBatches.SelectedItem as clsItemStockVO).MRP);
                                objSelectedGRNItems.NetAmount = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO).NetAmount;
                                objSelectedGRNItems.SelectItem = true;
                            }
                            chk.IsChecked = false;
                            _SelectedGrnItems.Add(objSelectedGRNItems);
                        }
                    }
                    dgSelectedItems.ItemsSource = null;
                    dgSelectedItems.ItemsSource = SelectedGrnItems;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin = new 
                        MessageBoxControl.MessageBoxChildWindow("Palash", "Available Stock is Zero for selected Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWin.Show();
                }
            }
        }

        #endregion

        #region Private Methods
        private void FillSupplier()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsFromPOGRN = true;
            BizAction.MasterTable = MasterTableNameList.M_Supplier;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    foreach (var item in objList)
                    {
                        item.Description = item.Code + " - " + item.Description;
                    }
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;
                    cmbSupplier.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillStores()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = result.ToList<clsStoreVO>();

                    if (this.StoreID > 0)
                    {
                        cmbStore.SelectedValue = this.StoreID;
                    }
                    else
                    {
                        cmbStore.SelectedItem = result.ToList<clsStoreVO>()[0];
                    }
                    FillGRNDetailslList();
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        /// <summary>
        /// Fills GRN items for GRN ID
        /// </summary>
        private void FillGRNDetailslList()
        {
            try
            {
                if (objWIndicator == null) objWIndicator = new WaitIndicator();
                objWIndicator.Show();
                clsGetGRNDetailsForGRNReturnListBizActionVO BizAction = new clsGetGRNDetailsForGRNReturnListBizActionVO();
                BizAction.List = new List<clsGRNDetailsVO>();
                BizAction.IsForQS = true;
                if (dtpGRNFromDate.SelectedDate != null)
                    BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

                if (dtpGRNToDate.SelectedDate != null)
                    BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

                if (this.StoreID > 0)
                    BizAction.StoreId = this.StoreID;
                else if (cmbStore.SelectedItem != null)
                    BizAction.StoreId = (cmbStore.SelectedItem as clsStoreVO).StoreId;

                if (cmbSupplier.SelectedItem != null)
                    BizAction.SupplierId = (cmbSupplier.SelectedItem as MasterListItem).ID;

                if (!string.IsNullOrEmpty(txtGRNNo.Text))
                    BizAction.GRNNo = txtGRNNo.Text.ToString();

                if (!string.IsNullOrEmpty(txtItemName.Text))
                    BizAction.ItemName = txtItemName.Text.ToString();

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        clsGetGRNDetailsForGRNReturnListBizActionVO objList = ((clsGetGRNDetailsForGRNReturnListBizActionVO)e.Result);

                        if (objList.List.ToList() != null)
                        {
                            DataList.TotalItemCount = objList.TotalRows; //(int)(((clsGetGRNDetailsForGRNReturnListBizActionVO)e.Result).TotalRows);
                            DataList.Clear();
                            foreach (var item in objList.List.ToList())
                            {
                                DataList.Add(item);
                            }

                            dgGRNSearchList.ItemsSource = null;
                            dgGRNSearchList.ItemsSource = DataList;
                            //dgGRNSearchList.SelectedIndex = -1;
                            DataPager.Source = DataList;
                        }
                    }
                    objWIndicator.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
            }
        }

        /// <summary>
        /// Fills GRN Items
        /// </summary>
        //private void SerachGRNItems()
        //{
        //    _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
        //    _GRNReturnAddedItems.Clear();
        //    dgGRNSearchList.ItemsSource = null;
        //    dgGRNSearchItems.ItemsSource = null;


        //    clsGetGRNListBizActionVO BizAction = new clsGetGRNListBizActionVO();

        //    if (dtpGRNFromDate.SelectedDate == null)
        //        BizAction.FromDate = null;
        //    else
        //        BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

        //    if (dtpGRNToDate.SelectedDate == null)
        //        BizAction.ToDate = null;
        //    else
        //        BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

        //    if (IsSearchForGRNReturn == true)
        //        BizAction.Freezed = true;


        //    BizAction.IsPagingEnabled = true;
        //    BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
        //    BizAction.MaximumRows = DataList.PageSize;
        //    BizAction.GrnReturnSearch = true;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            clsGetGRNListBizActionVO objList = ((clsGetGRNListBizActionVO)args.Result);
        //            if (objList.List != null)
        //            {
        //                DataList.TotalItemCount = objList.TotalRows;
        //                DataList.Clear();
        //                foreach (var item in objList.List)
        //                {
        //                    DataList.Add(item);
        //                }
        //                dgGRNSearchList.ItemsSource = null;
        //                dgGRNSearchList.ItemsSource = DataList;
        //                dgGRNSearchList.SelectedIndex = -1;
        //                DataPager.Source = DataList;
        //            }
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}

        private void FillGRNItemWiseBatches(long ItemID, long StoreID, long BatchID, long TransactionID, long UnitID)
        {
            try
            {
                if (objWIndicator == null) objWIndicator = new WaitIndicator();
                //if (objWIndicator == null) objWIndicator = new WaitIndicator();
                //objWIndicator.Show();
                clsGetItemStockBizActionVO objVO = new clsGetItemStockBizActionVO();
                objVO.BatchList = new List<clsItemStockVO>();
                objVO.IsForGRNItemsToQS = true;
                objVO.ItemID = ItemID;
                objVO.StoreID = StoreID;
                objVO.BatchID = BatchID;
                objVO.TransactionID = TransactionID;
                objVO.UnitID = UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        objWIndicator.Show();
                        objVO.BatchList = (e.Result as clsGetItemStockBizActionVO).BatchList;
                        objVO.BatchList.ForEach(z => z.Status = false);

                        _BatchList = objVO.BatchList.ToList();

                        dgGRNItemBatches.ItemsSource = null;
                        dgGRNItemBatches.ItemsSource = BatchList;
                        objWIndicator.Close();
                    }
                    //objWIndicator.Close();
                };

                Client.ProcessAsync(objVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
            }
        }
        #endregion

        #region Selection SelectionChanged Events
        private void dgGRNSearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNSearchList.SelectedItem != null)
            {
                clsGRNDetailsVO objVO = (dgGRNSearchList.SelectedItem as clsGRNDetailsVO);
                FillGRNItemWiseBatches(objVO.ItemID, objVO.StoreID, objVO.BatchID, objVO.GRNID, objVO.GRNUnitID);

                //_GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
                //_GRNReturnAddedItems.Clear();
                //this._SelectedGrnItems.Clear();
                //dgGRNSearchItems.ItemsSource = null;
                //FillGRNDetailslList(((clsGRNVO)dgGRNSearchList.SelectedItem).ID, ((clsGRNVO)dgGRNSearchList.SelectedItem).UnitId);
            }

        }
        #endregion

        private void dgGRNItemBatches_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgGRNItemBatches.ItemsSource != null)
            {
                if (((clsItemStockVO)e.Row.DataContext).IsItemBlock)
                    e.Row.IsEnabled = false;
                else
                    e.Row.IsEnabled = true;
            }
        }
    }

}

