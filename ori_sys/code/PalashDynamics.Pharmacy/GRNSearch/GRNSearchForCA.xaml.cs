using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace PalashDynamics.Pharmacy.GRNSearch
{
    public partial class GRNSearchForCA : ChildWindow
    {
        //Added
        public long StoreID { get; set; }
        public long SupplierID { get; set; }

        private ObservableCollection<clsCAGRNDetailsVO> _ItemsSelected;
        public ObservableCollection<clsCAGRNDetailsVO> ItemsSelected { get { return _ItemsSelected; } }
        public GRNSearchForCA()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(GRNSearchForCA_Loaded);
            DataList = new PagedSortableCollectionView<clsCAGRNVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        }

        void GRNSearchForCA_Loaded(object sender, RoutedEventArgs e)
        {
            long clinicId = 0;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                clinicId = 0;
            }
            else
            {
                clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            FillSupplier();
            FillStores(clinicId);
            txtGRNnumber.Focus();
            SearchGRN();

        }

        #region 'Paging'

        public PagedSortableCollectionView<clsCAGRNVO> DataList { get; private set; }

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
            SearchGRN();

        }



        #endregion
        public event RoutedEventHandler OnSaveButton_Click;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;

                        if (SupplierID > 0)
                        {
                            cmbSupplier.SelectedValue = SupplierID;
                        }
                        else
                        {
                            if (objList.Count > 1)
                            {
                                cmbSupplier.SelectedItem = objList[1];
                            }
                            else
                            {
                                cmbSupplier.SelectedItem = objList[0];
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

                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                            cmbStore.SelectedItem = objList[1];
                        else
                            cmbStore.SelectedItem = objList[0];
                    }

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchGRN();
        }

        private void SearchGRN()
        {
            dgGRNSearchList.ItemsSource = null;
            dgGRNSearchItems.ItemsSource = null;


            clsCAGRNBizActionVO BizAction = new clsCAGRNBizActionVO();

            //if (dtpGRNFromDate.SelectedDate == null)
            //    BizAction.FromDate = null;
            //else
            //    BizAction.FromDate = dtpGRNFromDate.SelectedDate.Value.Date;

            //if (dtpGRNToDate.SelectedDate == null)
            //    BizAction.ToDate = null;
            //else
            //    BizAction.ToDate = dtpGRNToDate.SelectedDate.Value.Date;

            //if (IsSearchForGRNReturn == true)
            //    BizAction.Freezed = true;


            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            BizAction.SupplierID = SupplierID;
            BizAction.StoreID = StoreID;
            BizAction.Freezed = true;
            BizAction.GRNNO = txtGRNnumber.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsCAGRNBizActionVO objList = ((clsCAGRNBizActionVO)args.Result);
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

        private void dgGRNSearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNSearchList.SelectedItem != null)
            {
                
               // this._SelectedGrnItems.Clear();
                dgGRNSearchItems.ItemsSource = null;
                FillGRNDetailslList(((clsCAGRNVO)dgGRNSearchList.SelectedItem).ID, ((clsCAGRNVO)dgGRNSearchList.SelectedItem).UnitId);
            }
        }

        private void FillGRNDetailslList(long pGRNID, long Unitid)
        {
            clsGetCAGRNItemDetailListBizActionVO BizAction = new clsGetCAGRNItemDetailListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = Unitid;

           // if (IsSearchForGRNReturn == true)
           //     BizAction.Freezed = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsCAGRNDetailsVO> objList = new List<clsCAGRNDetailsVO>();

                    objList = ((clsGetCAGRNItemDetailListBizActionVO)e.Result).List;
                   
                    dgGRNSearchItems.ItemsSource = null;
                    dgGRNSearchItems.ItemsSource = objList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        PagedCollectionView collection;
        private void chkItemStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNSearchItems.SelectedItem != null)
            {
                if (_ItemsSelected == null)
                    _ItemsSelected = new ObservableCollection<clsCAGRNDetailsVO>();
                ObservableCollection<clsCAGRNDetailsVO> RemovedItemsSelected = new ObservableCollection<clsCAGRNDetailsVO>();
                RemovedItemsSelected = _ItemsSelected;
                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                {
                    _ItemsSelected.Add((clsCAGRNDetailsVO)dgGRNSearchItems.SelectedItem);
                }
                else
                {
                    //_ItemsSelected.Remove((clsPurchaseOrderDetailVO)dgPOItems.SelectedItem);
                    for (int i = 0; i < RemovedItemsSelected.Count; i++)
                    {
                        if (RemovedItemsSelected[i].ItemID == ((clsCAGRNDetailsVO)dgGRNSearchItems.SelectedItem).ItemID && RemovedItemsSelected[i].ItemID == ((clsCAGRNDetailsVO)dgGRNSearchItems.SelectedItem).ItemID)
                        {
                            _ItemsSelected.Remove(RemovedItemsSelected[i]);
                        }
                    }
                }



                collection = new PagedCollectionView(_ItemsSelected);
                //collection.GroupDescriptions.Add(new PropertyGroupDescription("PONO"));
                dgSelectedItemList.ItemsSource = collection;
            }

        }
    }
}

