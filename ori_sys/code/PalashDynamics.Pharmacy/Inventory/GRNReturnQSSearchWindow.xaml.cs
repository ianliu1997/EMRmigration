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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class GRNReturnQSSearchWindow : ChildWindow
    {
        public long SupplierID { get; set; }
        private ObservableCollection<clsGRNReturnDetailsVO> _GRNReturnAddedItems;
        public ObservableCollection<clsGRNReturnDetailsVO> GRNReturnAddedItems { get { return _GRNReturnAddedItems; } set { _GRNReturnAddedItems = value; } }
        public event RoutedEventHandler OnSaveButton_Click;

        private ObservableCollection<clsGRNDetailsVO> _SelectedGrnItems;
        public ObservableCollection<clsGRNDetailsVO> SelectedGrnItems { get { return _SelectedGrnItems; } }

        public bool IsSearchForGRNReturn { get; set; }
        int ClickedFlag = 0;

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
            SerachGRNItems();
        }
        WaitIndicator objWIndicator;


        #endregion

        public GRNReturnQSSearchWindow()
        {
            InitializeComponent();
            _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();

            _GRNReturnAddedItems.Clear();
            //Paging======================================================
            DataList = new PagedSortableCollectionView<clsGRNDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 5;
            //======================================================
            dtpGRNFromDate.SelectedDate = DateTime.Now;
            dtpGRNToDate.SelectedDate = DateTime.Now;
            objWIndicator = new WaitIndicator();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillSupplier();
            //SerachGRNItems();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
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

        private void dgGRNSearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (dgGRNSearchList.SelectedItem != null)
            //{
            //    _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            //    _GRNReturnAddedItems.Clear();
            //    this._SelectedGrnItems.Clear();
            //    dgGRNSearchItems.ItemsSource = null;
            //    FillGRNDetailslList(((clsGRNVO)dgGRNSearchList.SelectedItem).ID, ((clsGRNVO)dgGRNSearchList.SelectedItem).UnitId);
            //}

        }

        /// <summary>
        /// Fills GRN items for GRN ID
        /// </summary>
        private void FillGRNDetailslList(long pGRNID, long Unitid)
        {
            clsGetGRNDetailsForGRNReturnListBizActionVO BizAction = new clsGetGRNDetailsForGRNReturnListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = Unitid;

            if (IsSearchForGRNReturn == true)
                BizAction.Freezed = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();

                    objList = ((clsGetGRNDetailsForGRNReturnListBizActionVO)e.Result).List;
                    if (objList != null)
                    {
                        objList.ForEach(z => z.AvailableQuantity = Math.Floor(z.AvailableQuantity / z.ConversionFactor));
                        dgGRNSelectedItems.ItemsSource = null;
                        dgGRNSelectedItems.ItemsSource = objList;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        /// <summary>
        /// Fills GRN Items
        /// </summary>
        private void SerachGRNItems()
        {
            try
            {
                if (objWIndicator == null) objWIndicator = new WaitIndicator();
                objWIndicator.Show();
                _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
                _GRNReturnAddedItems.Clear();
                clsGetGRNDetailsForGRNReturnListBizActionVO BizAction = new clsGetGRNDetailsForGRNReturnListBizActionVO();
                BizAction.IsFromGRNReturnQS = true;
                if (dtpGRNFromDate.SelectedDate == null)
                    BizAction.FromDate = null;
                else
                    BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

                if (dtpGRNToDate.SelectedDate == null)
                    BizAction.ToDate = null;
                else
                    BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

                if (cmbStore.SelectedItem != null)
                    BizAction.StoreId = (cmbStore.SelectedItem as clsStoreVO).StoreId;

                if (cmbSupplier.SelectedItem != null)
                    BizAction.SupplierId = (cmbSupplier.SelectedItem as MasterListItem).ID;

                if (!string.IsNullOrEmpty(txtGRNNo.Text))
                    BizAction.GRNNo = txtGRNNo.Text.ToString();

                if (!string.IsNullOrEmpty(txtItemName.Text))
                    BizAction.ItemName = txtItemName.Text.ToString();

                //if (IsSearchForGRNReturn == true)
                //    BizAction.Freezed = true;


                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetGRNDetailsForGRNReturnListBizActionVO objList = ((clsGetGRNDetailsForGRNReturnListBizActionVO)args.Result);
                        DataList.Clear();
                        if (objList.List != null)
                        {
                            DataList.TotalItemCount = objList.TotalRows;
                            
                            foreach (var item in objList.List)
                            {
                                item.Status = false;
                                DataList.Add(item);
                            }
                            dgGRNSearchList.ItemsSource = null;
                            dgGRNSearchList.ItemsSource = DataList;
                            dgGRNSearchList.SelectedIndex = -1;
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

        private void chkSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNSelectedItems.SelectedItem != null)
            {
                if (_GRNReturnAddedItems == null)
                    _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();
                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _SelectedGrnItems.Add((clsGRNDetailsVO)dgGRNSelectedItems.SelectedItem);
                else
                    _SelectedGrnItems.Remove((clsGRNDetailsVO)dgGRNSelectedItems.SelectedItem);

                dgGRNSelectedItems.ItemsSource = null;
                dgGRNSelectedItems.ItemsSource = SelectedGrnItems;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            SerachGRNItems();
            //DataPager.PageIndex = 0;
        }

        private void chkSelectItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNSearchList.SelectedItem != null)
            {
                CheckBox chk = (CheckBox)sender;

                if (_SelectedGrnItems == null)
                    _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();

                if (chk.IsChecked == true)
                    _SelectedGrnItems.Add(dgGRNSearchList.SelectedItem as clsGRNDetailsVO);
                else
                    _SelectedGrnItems.Remove(dgGRNSearchList.SelectedItem as clsGRNDetailsVO);
                
            }

            dgGRNSelectedItems.ItemsSource = null;
            dgGRNSelectedItems.ItemsSource = SelectedGrnItems;
        }


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

                    if (this.SupplierID > 0)
                    {
                        cmbSupplier.SelectedValue = this.SupplierID;
                        cmbSupplier.IsEnabled = false;
                    }
                    else
                    {
                        cmbSupplier.SelectedItem = objList.ToList<MasterListItem>()[0];
                    }
                }
                FillStores();
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
                    cmbStore.SelectedItem = result.ToList<clsStoreVO>()[0];
                    //if (this.StoreID > 0)
                    //{
                    //    cmbStore.SelectedValue = this.StoreID;
                    //}
                    //else
                    //{
                    //    cmbStore.SelectedItem = result.ToList<clsStoreVO>()[0];
                    //}
                }
                SerachGRNItems();
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

       
    }
}

