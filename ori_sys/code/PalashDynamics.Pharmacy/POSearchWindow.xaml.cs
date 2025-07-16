using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
using System.Collections.ObjectModel;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PalashDynamics.Pharmacy
{
    public partial class POSearchWindow : ChildWindow
    {

        #region 'Paging'

        public PagedSortableCollectionView<clsPurchaseOrderVO> DataList { get; private set; }

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
                clsGetPurchaseOrderBizActionVO objBizActionVO = new clsGetPurchaseOrderBizActionVO();
                objBizActionVO.searchFromDate = dtpFromDate.SelectedDate;
                objBizActionVO.searchToDate = dtpToDate.SelectedDate;
                objBizActionVO.SearchStoreID = ((MasterListItem)cboStoreName.SelectedItem) == null ? this.StoreID : ((MasterListItem)cboStoreName.SelectedItem).ID;

                objBizActionVO.SearchSupplierID = ((MasterListItem)cboSuppName.SelectedItem) == null ? this.SupplierID : ((MasterListItem)cboSuppName.SelectedItem).ID;
                objBizActionVO.Freezed = true;
                objBizActionVO.PONO = txtpono.Text;
                objBizActionVO.PagingEnabled = true;
                objBizActionVO.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                objBizActionVO.MaximumRows = DataList.PageSize;
                objBizActionVO.flagPOFromGRN = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderBizActionVO result = arg.Result as clsGetPurchaseOrderBizActionVO;
                            DataList.TotalItemCount = result.TotalRowCount;
                            if (result.PurchaseOrderList != null)
                            {
                                DataList.Clear();
                                foreach (var item in result.PurchaseOrderList)
                                {
                                    DataList.Add(item);
                                }
                                dgPO.ItemsSource = null;
                                dgPO.ItemsSource = DataList;
                                dgDataPager.Source = null;
                                dgDataPager.PageSize = objBizActionVO.MaximumRows;
                                dgDataPager.Source = DataList;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        public POSearchWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(POSearchWindow_Loaded);

            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsPurchaseOrderVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

            //======================================================
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        void POSearchWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            FillStores(ClinicID);
            FillSupplier();
            FillPurchaseOrderList();

        }

        private clsPurchaseOrderVO _SelectedPO;
        public clsPurchaseOrderVO SelectedPO { get { return _SelectedPO; } }

        private ObservableCollection<clsPurchaseOrderDetailVO> _ItemsSelected = new ObservableCollection<clsPurchaseOrderDetailVO>();
        public ObservableCollection<clsPurchaseOrderDetailVO> ItemsSelected { get { return _ItemsSelected; } set { _ItemsSelected = value; } }

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            dgDataPager.PageIndex = 0;
            FillPurchaseOrderList();
        }
        /// <summary>
        /// The Event get handled when the item is selected or deselecteed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddPOItem_Click(object sender, RoutedEventArgs e)
        {
            if (dgPOItems.SelectedItem != null)
            {
                if (_ItemsSelected == null)
                    _ItemsSelected = new ObservableCollection<clsPurchaseOrderDetailVO>();

                CheckBox chk = (CheckBox)sender;
                clsPurchaseOrderVO objPurchaseOrder = ((clsPurchaseOrderVO)dgPO.SelectedItem);

                if (chk.IsChecked == true)
                {
                    Int32 iCount = _ItemsSelected.Count;
                    this.ItemsSelected.Add((clsPurchaseOrderDetailVO)dgPOItems.SelectedItem);
                    this.ItemsSelected[iCount].PONO = objPurchaseOrder.PONO;
                    this.ItemsSelected[iCount].PODate = objPurchaseOrder.Date;
                    this.ItemsSelected[iCount].POID = objPurchaseOrder.ID;
                    this.ItemsSelected[iCount].POUnitID = objPurchaseOrder.UnitId;
                }
                else
                {
                    clsPurchaseOrderDetailVO objPurchaseOrderDetail = ((clsPurchaseOrderDetailVO)dgPOItems.SelectedItem);
                    objPurchaseOrderDetail.PONO = objPurchaseOrder.PONO;
                    objPurchaseOrderDetail.PODate = objPurchaseOrder.Date;
                    objPurchaseOrderDetail.POID = objPurchaseOrder.ID;
                    objPurchaseOrderDetail.POUnitID = objPurchaseOrder.UnitId;
                    clsPurchaseOrderDetailVO objPurchaseOrderDetailRemove = this.ItemsSelected.Where(z => z.ItemID == objPurchaseOrderDetail.ItemID && z.POID == objPurchaseOrderDetail.POID && z.PoItemsID == objPurchaseOrderDetail.PoItemsID).First();
                    ItemsSelected.Remove(objPurchaseOrderDetailRemove);
                }
            }
        }
        /// <summary>
        /// The event get handled when the purchase order selection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgPO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPO.SelectedItem != null)
            {
                //if (_ItemsSelected != null)
                //    _ItemsSelected.Clear();
                _SelectedPO = (clsPurchaseOrderVO)dgPO.SelectedItem;
                FillPurchaseOrderDetailsList(_SelectedPO.ID, SelectedPO.UnitId);
            }
        }

        private void FillPurchaseOrderDetailsList(long pPOID, long iUnitID)
        {
            try
            {
                indicator.Show();
                clsGetPurchaseOrderDetailsBizActionVO objBizActionVO = new clsGetPurchaseOrderDetailsBizActionVO();

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
                            clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
                            dgPOItems.ItemsSource = obj.PurchaseOrderList != null ? obj.PurchaseOrderList : null;

                            if (_ItemsSelected != null)
                            {
                                for (Int32 iCount = 0; iCount < obj.PurchaseOrderList.Count; iCount++)
                                {
                                    foreach (clsPurchaseOrderDetailVO item in this._ItemsSelected)
                                    {
                                        if (item.ItemID == obj.PurchaseOrderList[iCount].ItemID && ((clsPurchaseOrderVO)dgPO.SelectedItem).PONO.Trim().Equals(item.PONO))
                                        {
                                            obj.PurchaseOrderList[iCount].SelectItem = true;
                                            break;
                                        }
                                    }
                                }
                            }
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


