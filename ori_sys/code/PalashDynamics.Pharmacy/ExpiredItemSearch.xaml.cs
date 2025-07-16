using System;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Linq;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Collections.Generic;

namespace PalashDynamics.Pharmacy
{
    public partial class ExpiredItemSearch : ChildWindow
    {
        WaitIndicator objWIndicator;
        public event RoutedEventHandler OnSaveButton_Click;
        public Int64 StoreID { get; set; }
        public Int64 SupplierID { get; set; }
        public bool IsFromDOS { get; set; }  // Set true from ScrapSale.xaml.cs

        #region 'Paging'
        public PagedSortableCollectionView<clsExpiredItemReturnDetailVO> DataList { get; private set; }

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
            GetExpiredItems();
        }
        #endregion

        public ExpiredItemSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ExpiredItemSearch_Loaded);
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            objWIndicator = new WaitIndicator();

            //Paging======================================================
            DataList = new PagedSortableCollectionView<clsExpiredItemReturnDetailVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            //======================================================
        }

        void ExpiredItemSearch_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsFromDOS)
            {
                lblFromDate.Text = "Received From Date";
                lblToDate.Text = "Received To Date";
            }
            else
            {
                lblFromDate.Text = "Expiry From Date";
                lblToDate.Text = "Expiry To Date";
            }



            FillStore();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private ObservableCollection<clsExpiredItemReturnDetailVO> _ItemSelected;
        public ObservableCollection<clsExpiredItemReturnDetailVO> SelectedItems { get { return _ItemSelected; } }

        /// <summary>
        /// Fills store combo box
        /// </summary>
        private void FillStore()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = true, Parent = 0 };
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var result = from item in BizActionObj.ToStoreList
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.IsQuarantineStore == true
                                 select item;

                    cmbStore.ItemsSource = result.ToList();
                    cmbStore.SelectedItem = result.ToList()[0];

                    if (this.StoreID > 0)
                    {
                        cmbStore.SelectedValue = this.StoreID;
                        cmbStore.IsEnabled = false;
                    }
                    GetExpiredItems();
                }
            };
            client.CloseAsync();
        }


        /// <summary>
        /// Ok button click
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        /// <summary>
        /// Cancel button click
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _ItemSelected = null;
            this.DialogResult = false;
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }


        private void GetExpiredItems()
        {
            try
            {
                if (objWIndicator == null) objWIndicator = new WaitIndicator();
                objWIndicator.Show();
                clsGetExpiredItemListBizActionVO BizAction = new clsGetExpiredItemListBizActionVO();
                BizAction.objExpiredItemsVO = new clsExpiredItemReturnDetailVO();
                BizAction.IsFromDOS = this.IsFromDOS;  // Set true from ScrapSale.xaml.cs
                if (dtpFromDate.SelectedDate == null)
                    BizAction.FromDate = null;
                else
                    BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;

                if (dtpToDate.SelectedDate == null)
                    BizAction.ToDate = null;
                else
                    BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

                if (this.StoreID > 0)
                {
                    BizAction.StoreID = this.StoreID;
                }
                else if (cmbStore.SelectedItem != null)
                {
                    BizAction.StoreID = (cmbStore.SelectedItem as clsStoreVO).StoreId;
                }

                if (!string.IsNullOrEmpty(txtReceivedNo.Text))
                    BizAction.objExpiredItemsVO.ReceivedNo = txtReceivedNo.Text.ToString();

                if (!string.IsNullOrEmpty(txtItemName.Text))
                    BizAction.objExpiredItemsVO.ItemName = txtItemName.Text.ToString();

                if (!string.IsNullOrEmpty(txtBatchCode.Text))
                    BizAction.objExpiredItemsVO.BatchCode = txtBatchCode.Text.ToString();

                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetExpiredItemListBizActionVO objList = ((clsGetExpiredItemListBizActionVO)arg.Result);
                        DataList.Clear();
                        if (objList.ExpiredItemList != null)
                        {
                            DataList.TotalItemCount = objList.TotalRows;

                            foreach (var item in objList.ExpiredItemList)
                            {
                                item.Status = false;
                                item.SelectedUOM = new MasterListItem(item.TransactionUOMID, item.ReceivedQtyUOM);
                                item.ReturnQty = item.ReceivedQty;
                                DataList.Add(item);
                            }
                            dgexpItem.ItemsSource = null;
                            dgexpItem.ItemsSource = DataList;
                            dgexpItem.SelectedIndex = -1;
                            dataPager.Source = DataList;
                        }
                    }
                    objWIndicator.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
            }
        }

        clsExpiredItemReturnDetailVO _SelectedGRNItem;
        private void chkSelectItem_Click(object sender, RoutedEventArgs e)
        {
            //this._SelectedGRNItem = ((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem);
            //if (dgSelectedItems.ItemsSource == null)
            //    dgSelectedItems.ItemsSource = this.SelectedItems;
            //if (_ItemSelected == null)
            //    _ItemSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    this._ItemSelected.Add(_SelectedGRNItem);
            //}
            //else
            //{
            //    clsExpiredItemReturnDetailVO objItem = this.SelectedItems.Where(z => z.ItemID == _SelectedGRNItem.ItemID && z.BatchID == _SelectedGRNItem.BatchID &&
            //                                                                    z.ReceivedID == _SelectedGRNItem.ReceivedID && z.ReceivedUnitID == _SelectedGRNItem.ReceivedUnitID
            //                                                                    && z.ReceivedDetailID == _SelectedGRNItem.ReceivedDetailID && z.ReceivedDetailUnitID == _SelectedGRNItem.ReceivedDetailUnitID).FirstOrDefault();
            //    this._ItemSelected.Remove(objItem);
            //}

            //dgSelectedItems.ItemsSource = null;
            //dgSelectedItems.ItemsSource = SelectedItems.DeepCopy();

            if (dgexpItem.SelectedItem != null)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                CheckBox chk = (CheckBox)sender;
                clsExpiredItemReturnDetailVO objIssueItem = (clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem;
                if (chk.IsChecked == true)
                {
                    if (_ItemSelected.Where(z => z.ItemID == objIssueItem.ItemID && z.BatchID == objIssueItem.BatchID &&
                                        z.ReceivedID == objIssueItem.ReceivedID && z.ReceivedUnitID == objIssueItem.ReceivedUnitID
                                        && z.ReceivedDetailID == objIssueItem.ReceivedDetailID && z.ReceivedDetailUnitID == objIssueItem.ReceivedDetailUnitID).Count() > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash!", "Item already added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                    }
                    else
                    {
                        _ItemSelected.Add((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem);
                    }
                }
                else
                    _ItemSelected.Remove((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem);

                dgSelectedItems.ItemsSource = null;
                dgSelectedItems.ItemsSource = SelectedItems.DeepCopy();
            }
        }

        private void chkSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            //clsExpiredItemReturnDetailVO objIssueItem = ((clsExpiredItemReturnDetailVO)dgSelectedItems.SelectedItem);
            //if (_ItemSelected == null)
            //    _ItemSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
            //if (dgSelectedItems.ItemsSource == null)
            //    dgSelectedItems.ItemsSource = this.SelectedItems;
            //{
            //    this._ItemSelected.Remove((clsExpiredItemReturnDetailVO)dgSelectedItems.SelectedItem);
            //    this.DataList.Where(z => z.ItemID == objIssueItem.ItemID && z.BatchID == objIssueItem.BatchID &&
            //                        z.ReceivedID == objIssueItem.ReceivedID && z.ReceivedUnitID == objIssueItem.ReceivedUnitID
            //                        && z.ReceivedDetailID == objIssueItem.ReceivedDetailID && z.ReceivedDetailUnitID == objIssueItem.ReceivedDetailUnitID).Select(z => z.Status = false);
            //}

            if (dgSelectedItems.SelectedItem != null)
            {
                clsExpiredItemReturnDetailVO objIssueItem = ((clsExpiredItemReturnDetailVO)dgSelectedItems.SelectedItem);
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsExpiredItemReturnDetailVO>();
                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _ItemSelected.Add((clsExpiredItemReturnDetailVO)dgexpItem.SelectedItem);
                else
                {
                    clsExpiredItemReturnDetailVO objRemoveVo = _ItemSelected.Where(z => z.ItemID == objIssueItem.ItemID && z.BatchID == objIssueItem.BatchID &&
                                        z.ReceivedID == objIssueItem.ReceivedID && z.ReceivedUnitID == objIssueItem.ReceivedUnitID
                                        && z.ReceivedDetailID == objIssueItem.ReceivedDetailID && z.ReceivedDetailUnitID == objIssueItem.ReceivedDetailUnitID).FirstOrDefault();

                    _ItemSelected.Remove(objRemoveVo);
                    //this.DataList.Where(z => z.ItemID == objIssueItem.ItemID && z.BatchID == objIssueItem.BatchID &&
                    //                    z.ReceivedID == objIssueItem.ReceivedID && z.ReceivedUnitID == objIssueItem.ReceivedUnitID
                    //                    && z.ReceivedDetailID == objIssueItem.ReceivedDetailID && z.ReceivedDetailUnitID == objIssueItem.ReceivedDetailUnitID).Select(z => z.Status = false);
                }

                dgSelectedItems.ItemsSource = null;
                dgSelectedItems.ItemsSource = SelectedItems;

                //dgexpItem.ItemsSource = null;
                //dgexpItem.ItemsSource = DataList;
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            GetExpiredItems();
        }
    }
}

