using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Inventory.WorkOrder;
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Collections;
using System.Windows.Data;

namespace PalashDynamics.Pharmacy
{
    public partial class WorkOrder_SearchWindow : ChildWindow
    {

        #region 'Paging'

        public PagedSortableCollectionView<clsWorkOrderVO> DataList { get; private set; }

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillPurchaseOrderList();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void FillPurchaseOrderList()
        {
            try
            {
                indicator.Show();
                clsGetWorkOrderBizActionVO objBizActionVO = new clsGetWorkOrderBizActionVO();
                objBizActionVO.searchFromDate = dtpFromDate.SelectedDate;
                objBizActionVO.searchToDate = dtpToDate.SelectedDate;
                objBizActionVO.SearchStoreID = ((MasterListItem)cboStoreName.SelectedItem) == null ? this.StoreID : ((MasterListItem)cboStoreName.SelectedItem).ID;
                
                
                
                objBizActionVO.SearchDeliveryStoreID = ((MasterListItem)cboStoreName.SelectedItem) == null ? this.StoreID : ((MasterListItem)cboStoreName.SelectedItem).ID;
                objBizActionVO.SearchSupplierID = ((MasterListItem)cboSuppName.SelectedItem) == null ? this.SupplierID : ((MasterListItem)cboSuppName.SelectedItem).ID;
                objBizActionVO.Freezed = true;
                objBizActionVO.WONO = txtpono.Text;
                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;
                objBizActionVO.flagWOFromGRN = true;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetWorkOrderBizActionVO result = arg.Result as clsGetWorkOrderBizActionVO;
                            DataList.TotalItemCount = result.TotalRowCount;
                            if (result.WorkOrderList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.WorkOrderList)
                                {
                                    DataList.Add(item);
                                }
                                dgPurchaseOrder.ItemsSource = null;
                                dgPurchaseOrder.ItemsSource = DataList;
                                PODataPager.Source = null;
                                PODataPager.PageSize = objBizActionVO.MaximumRows;
                                PODataPager.Source = DataList;
                            }
                            else
                            {
                                dgPurchaseOrder.ItemsSource = new PagedSortableCollectionView<clsWorkOrderVO>();
                                PODataPager.Source = new PagedSortableCollectionView<clsWorkOrderVO>();
                                dgPOItems.ItemsSource = new List<clsWorkOrderDetailVO>();
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "No Result Found.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    indicator.Close();
                };
                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        #endregion

        public WorkOrder_SearchWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WorkOrder_SearchWindow_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsWorkOrderVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            PODataPager.PageSize = DataListPageSize;
            PODataPager.Source = DataList;

            //======================================================
        }

        void WorkOrder_SearchWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                clinicId = 0;
            }
            else
            {
                clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            FillStores(clinicId);
            FillSupplier();
            FillPurchaseOrderList();

        }

        private clsWorkOrderVO _SelectedWO;
        public clsWorkOrderVO SelectedWO { get { return _SelectedWO; } }

        private ObservableCollection<clsWorkOrderDetailVO> _ItemsSelected;
        public ObservableCollection<clsWorkOrderDetailVO> ItemsSelected { get { return _ItemsSelected; } }

        private ObservableCollection<clsWorkOrderDetailVO> _SelectedPOItemList;
        public ObservableCollection<clsWorkOrderDetailVO> SelectedPOItemList { get { return _SelectedPOItemList; } }

        private long _ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        public long ClinicID
        {
            get
            {
                return _ClinicID;
            }
            set
            {
                _ClinicID = value;
            }
        }

        public long StoreID { get; set; }
        public long SupplierID { get; set; }

        #region Fill Combo Box

        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    cboStoreName.ItemsSource = null;
                    cboStoreName.ItemsSource = objList;

                    if (StoreID > 0)
                    {
                        cboStoreName.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                            cboStoreName.SelectedItem = objList[1];
                        else
                            cboStoreName.SelectedItem = objList[0];
                    }

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        cboSuppName.ItemsSource = null;
                        cboSuppName.ItemsSource = objList;

                        if (SupplierID > 0)
                        {
                            cboSuppName.SelectedValue = SupplierID;
                        }
                        else
                        {
                            if (objList.Count > 1)
                            {
                                cboSuppName.SelectedItem = objList[1];
                            }
                            else
                            {
                                cboSuppName.SelectedItem = objList[0];
                            }
                        }

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        public event RoutedEventHandler OnSaveButton_Click;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            StoreID = ((MasterListItem)cboStoreName.SelectedItem).ID;

            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgPurchaseOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPurchaseOrder.SelectedItem != null)
            {
                _SelectedWO = (clsWorkOrderVO)dgPurchaseOrder.SelectedItem;
                FillPurchaseOrderDetailsList(_SelectedWO.ID, SelectedWO.UnitId);
            }
        }
        PagedCollectionView collection;

        private void AddPOItem_Click(object sender, RoutedEventArgs e)
        {

            if (dgPOItems.SelectedItem != null)
            {
                if (_ItemsSelected == null)
                    _ItemsSelected = new ObservableCollection<clsWorkOrderDetailVO>();
                ObservableCollection<clsWorkOrderDetailVO> RemovedItemsSelected = new ObservableCollection<clsWorkOrderDetailVO>();
                RemovedItemsSelected = _ItemsSelected;
                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                {
                    _ItemsSelected.Add((clsWorkOrderDetailVO)dgPOItems.SelectedItem);
                }
                else
                {
                    //_ItemsSelected.Remove((clsPurchaseOrderDetailVO)dgPOItems.SelectedItem);
                    for (int i = 0; i < RemovedItemsSelected.Count; i++)
                    {
                        if (RemovedItemsSelected[i].WoItemsID == ((clsWorkOrderDetailVO)dgPOItems.SelectedItem).WoItemsID && RemovedItemsSelected[i].ItemID == ((clsWorkOrderDetailVO)dgPOItems.SelectedItem).ItemID)
                        {
                            _ItemsSelected.Remove(RemovedItemsSelected[i]);
                        }
                    }
                }



                collection = new PagedCollectionView(_ItemsSelected);
                collection.GroupDescriptions.Add(new PropertyGroupDescription("WONO"));
                dgSelectedPOItemList.ItemsSource = collection;
            }
        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {

            ObservableCollection<clsWorkOrderDetailVO> RemovedItemsSelected1 = new ObservableCollection<clsWorkOrderDetailVO>();

            RemovedItemsSelected1 = _ItemsSelected;
            if (((CheckBox)sender).IsChecked == false)
            {
                for (int i = 0; i < RemovedItemsSelected1.Count; i++)
                {
                    if (RemovedItemsSelected1[i].WoItemsID == ((clsWorkOrderDetailVO)dgSelectedPOItemList.SelectedItem).WoItemsID && RemovedItemsSelected1[i].ItemID == ((clsWorkOrderDetailVO)dgSelectedPOItemList.SelectedItem).ItemID)
                    {
                        _ItemsSelected.Remove(RemovedItemsSelected1[i]);
                    }
                }


            }
            dgSelectedPOItemList.ItemsSource = null;
            collection = new PagedCollectionView(_ItemsSelected);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("WONO"));
            dgSelectedPOItemList.ItemsSource = collection;


        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            PODataPager.PageIndex = 0;
            FillPurchaseOrderList();
        }

        private void FillPurchaseOrderDetailsList(long pPOID, long iUnitID)
        {
            try
            {
                indicator.Show();
                clsGetWorkOrderDetailsBizActionVO objBizActionVO = new clsGetWorkOrderDetailsBizActionVO();

                objBizActionVO.SearchID = pPOID;
                objBizActionVO.IsPagingEnabled = true;
                objBizActionVO.StartIndex = 0;
                objBizActionVO.MinRows = 20;
                objBizActionVO.UnitID = iUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetWorkOrderDetailsBizActionVO obj = ((clsGetWorkOrderDetailsBizActionVO)arg.Result);
                            //dgPOItems.ItemsSource = obj.PurchaseOrderList != null ? obj.PurchaseOrderList : null;

                            if (_ItemsSelected != null)
                            {
                                foreach (var item in obj.PurchaseOrderList)
                                {
                                    foreach (var item1 in _ItemsSelected)
                                    {
                                        if (item.WOID == item1.WOID && item.ItemID == item1.ItemID)
                                        {
                                            item.SelectItem = true;
                                        }
                                    }
                                }
                            }
                            dgPOItems.ItemsSource = null;
                            dgPOItems.ItemsSource = obj.PurchaseOrderList;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while fetching PO Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    indicator.Close();
                };

                Client.ProcessAsync(objBizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

    }
}

