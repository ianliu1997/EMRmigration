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

namespace PalashDynamics.Pharmacy
{
    public partial class GRNSearchForm : ChildWindow
    {
        private ObservableCollection<clsGRNReturnDetailsVO> _GRNReturnAddedItems;
        public ObservableCollection<clsGRNReturnDetailsVO> GRNReturnAddedItems { get { return _GRNReturnAddedItems; } set { _GRNReturnAddedItems = value; } }
        public event RoutedEventHandler OnSaveButton_Click;

        private ObservableCollection<clsGRNDetailsVO> _SelectedGrnItems;
        public ObservableCollection<clsGRNDetailsVO> SelectedGrnItems { get { return _SelectedGrnItems; } }

        public bool IsSearchForGRNReturn { get; set; }
        int ClickedFlag = 0;

        #region 'Paging'

        public PagedSortableCollectionView<clsGRNVO> DataList { get; private set; }

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



        #endregion

        public GRNSearchForm()
        {
            InitializeComponent();


            _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();

            _GRNReturnAddedItems.Clear();
            //Paging======================================================
            DataList = new PagedSortableCollectionView<clsGRNVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            DataListPageSize = 5;
            //======================================================
            dtpGRNFromDate.SelectedDate = DateTime.Now;
            dtpGRNToDate.SelectedDate = DateTime.Now;

        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SerachGRNItems();
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
            if (dgGRNSearchList.SelectedItem != null)
            {
                _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
                _GRNReturnAddedItems.Clear();
                this._SelectedGrnItems.Clear();
                dgGRNSearchItems.ItemsSource = null;
                FillGRNDetailslList(((clsGRNVO)dgGRNSearchList.SelectedItem).ID, ((clsGRNVO)dgGRNSearchList.SelectedItem).UnitId);
            }

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
                        dgGRNSearchItems.ItemsSource = null;
                        dgGRNSearchItems.ItemsSource = objList;
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
            _GRNReturnAddedItems = new ObservableCollection<clsGRNReturnDetailsVO>();
            _GRNReturnAddedItems.Clear();
            dgGRNSearchList.ItemsSource = null;
            dgGRNSearchItems.ItemsSource = null;


            clsGetGRNListBizActionVO BizAction = new clsGetGRNListBizActionVO();

            if (dtpGRNFromDate.SelectedDate == null)
                BizAction.FromDate = null;
            else
                BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

            if (dtpGRNToDate.SelectedDate == null)
                BizAction.ToDate = null;
            else
                BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

            if (IsSearchForGRNReturn == true)
                BizAction.Freezed = true;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            BizAction.GrnReturnSearch = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetGRNListBizActionVO objList = ((clsGetGRNListBizActionVO)args.Result);
                    if (objList.List != null)
                    {
                        DataList.TotalItemCount = objList.TotalRows;
                        DataList.Clear();
                        foreach (var item in objList.List)
                        {
                            DataList.Add(item);
                        }
                        dgGRNSearchList.ItemsSource = null;
                        dgGRNSearchList.ItemsSource = DataList;
                        dgGRNSearchList.SelectedIndex = -1;
                        DataPager.Source = DataList;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void chkGrnDetails_Click(object sender, RoutedEventArgs e)
        {

            if (dgGRNSearchItems.SelectedItem != null)
            {
                if (_GRNReturnAddedItems == null)
                    _SelectedGrnItems = new ObservableCollection<clsGRNDetailsVO>();
                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)

                    _SelectedGrnItems.Add((clsGRNDetailsVO)dgGRNSearchItems.SelectedItem);
                else
                    _SelectedGrnItems.Remove((clsGRNDetailsVO)dgGRNSearchItems.SelectedItem);

            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            SerachGRNItems();
        }
    }
}

