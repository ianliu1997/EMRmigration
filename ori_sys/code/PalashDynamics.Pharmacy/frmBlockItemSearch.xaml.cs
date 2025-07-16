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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.Windows.Data;

namespace PalashDynamics.Pharmacy
{
    public partial class frmBlockItemSearch : ChildWindow
    {
        public long StoreID { get; set; }
        bool UserStoreAssigned = false;
       
        List<clsStoreVO> UserStores = new List<clsStoreVO>();
        List<MasterListItem> tempRackCombo = new List<MasterListItem>();
        List<MasterListItem> tempStoreCombo = new List<MasterListItem>();
        List<MasterListItem> tempShelfCombo = new List<MasterListItem>();
        List<MasterListItem> tempBinCombo = new List<MasterListItem>();
        public event RoutedEventHandler OnSaveButton_Click;
        public clsUserVO loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
        public frmBlockItemSearch()
        {
            InitializeComponent();
            DataList = new PagedSortableCollectionView<clsBlockItemsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;

            FillStores(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            FillRack();
            FillContainer();
            FillShelf();
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetList();

        }
        #region Fill All Combo

        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
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

                   
                    objList.Add(new MasterListItem(0, "- Select -", true));
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
                        {
                            cmbStore.SelectedItem = objList[1];
                        }
                        else
                        {
                            cmbStore.SelectedItem = objList[0];
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        //private void FillStore()
        //{
           
        //    clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
        //    clsItemMasterVO obj = new clsItemMasterVO();
        //    obj.RetrieveDataFlag = false;
        //    BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.ProcessCompleted += (s, args) =>
        //    {
        //        if (args.Error == null && args.Result != null)
        //        {
        //            BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
        //            clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true };
        //            BizActionObj.ItemMatserDetails.Insert(0, Default);
        //            var result1 = from item in BizActionObj.ItemMatserDetails
        //                          where item.Status == true
        //                          select item;

        //            var result = from item in BizActionObj.ItemMatserDetails
        //                         where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
        //                         select item;
        //            List<clsUserUnitDetailsVO> UserUnit = new List<clsUserUnitDetailsVO>();
        //            UserUnit = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;
        //            UserStores.Clear();
        //            foreach (var item in UserUnit)
        //            {
        //                if (item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
        //                {
        //                    UserStores = item.UserUnitStore;
        //                }
        //            }

        //            var resultnew = from item in UserStores select item;
        //            if (UserStores != null && UserStores.Count > 0)
        //            {
        //                UserStoreAssigned = true;
        //            }
        //            List<clsStoreVO> StoreListForClinic = (List<clsStoreVO>)result.ToList();
        //            StoreListForClinic.Insert(0, Default);
        //            cmbStore.ItemsSource = result1.ToList();
        //            if (result1.ToList().Count > 0)
        //            {
        //                cmbStore.SelectedItem = result1.ToList()[0];
        //            }
        //            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
        //            {
        //                cmbStore.ItemsSource = result1.ToList();

        //                cmbStore.ItemsSource = BizActionObj.ItemMatserDetails;

        //                cmbStore.ItemsSource = result1.ToList();
                       
        //                if (result1.ToList().Count > 0)
        //                {
        //                    cmbStore.SelectedItem = (result1.ToList())[0];
        //                }
        //            }
        //            else
        //            {
        //                cmbStore.ItemsSource = StoreListForClinic;
        //                if (StoreListForClinic.Count > 0)
        //                {
        //                    cmbStore.SelectedItem = StoreListForClinic[0];
                          
        //                }
        //            }
        //            foreach (var item in tempStoreCombo.ToList())
        //            {
        //                foreach (var item1 in result1.ToList())
        //                {
        //                    item.ID = item1.StoreId;
        //                    item.Description = item1.StoreName;
        //                }
        //            }
        //        }
        //    };
        //    client.CloseAsync();
        //}

        private void FillRack()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RackMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        tempRackCombo = objList.ToList();
                        cmbRack.ItemsSource = null;
                        cmbRack.ItemsSource = objList;
                        cmbRack.SelectedItem = objList[0];
                       
                      
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillContainer()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ContainerMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        tempBinCombo = objList.ToList();
                        cmbContainer.ItemsSource = null;
                        cmbContainer.ItemsSource = objList;
                        cmbContainer.SelectedItem = objList[0];
                 
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void FillShelf()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ShelfMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        tempShelfCombo = objList.ToList();
                        cmbShelf.ItemsSource = null;
                        cmbShelf.ItemsSource = objList;
                        cmbShelf.SelectedItem = objList[0];
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearchBlockItem_Click(object sender, RoutedEventArgs e)
        {
            GetList();
        }
        public PagedSortableCollectionView<clsBlockItemsVO> DataList { get; private set; }
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
        private void GetList()
        {
            try
            {
                clsGetBlockItemsListBizActionVO BizAction = new clsGetBlockItemsListBizActionVO();
                BizAction.ItemDetails = new clsBlockItemsVO();
                if (cmbStore.SelectedItem != null)
                    BizAction.ItemDetails.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                if (cmbRack.SelectedItem != null)
                    BizAction.ItemDetails.RackID = ((MasterListItem)cmbRack.SelectedItem).ID;
                if (cmbShelf.SelectedItem != null)
                    BizAction.ItemDetails.ShelfID = ((MasterListItem)cmbShelf.SelectedItem).ID;
                if (cmbContainer.SelectedItem != null)
                    BizAction.ItemDetails.ContainerID = ((MasterListItem)cmbContainer.SelectedItem).ID;
                if (txtItemName.Text != string.Empty)
                    BizAction.ItemDetails.ItemName = txtItemName.Text.Trim();

                BizAction.PagingEnabled = true;
                BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizAction.MaximumRows = DataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetBlockItemsListBizActionVO)args.Result).ItemDetailsList != null)
                        {

                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetBlockItemsListBizActionVO)args.Result).TotalRowCount;
                            foreach (var item in ((clsGetBlockItemsListBizActionVO)args.Result).ItemDetailsList)
                            {
                                DataList.Add(item);
                            }

                            //PagedCollectionView SortableList = new PagedCollectionView(DataList);
                            //SortableList.GroupDescriptions.Add(new PropertyGroupDescription("ItemName"));
                         
                            dgBlockItemList.ItemsSource = null;
                            dgBlockItemList.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                            txtTotalCountRecords.Text = DataList.TotalItemCount.ToString();
                        }

                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private ObservableCollection<clsBlockItemsVO> _BatchSelected;
        public ObservableCollection<clsBlockItemsVO> SelectedBatches { get { return _BatchSelected; } }
        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dgBlockItemList.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsBlockItemsVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                {
                    ((clsBlockItemsVO)dgBlockItemList.SelectedItem).IsChecked = true;
                    _BatchSelected.Add((clsBlockItemsVO)dgBlockItemList.SelectedItem);
                }
                else
                {
                    ((clsBlockItemsVO)dgBlockItemList.SelectedItem).IsChecked = false;
                    _BatchSelected.Remove((clsBlockItemsVO)dgBlockItemList.SelectedItem);
                    chkAll.IsChecked = false;
                }
            }
        }

        private void cmdSearchBlockItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                GetList();
        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {

            PagedSortableCollectionView<clsBlockItemsVO> ObjList = new PagedSortableCollectionView<clsBlockItemsVO>();
            if (dgBlockItemList.ItemsSource != null)
            {
                ObjList = (PagedSortableCollectionView<clsBlockItemsVO>)dgBlockItemList.ItemsSource;

                if (ObjList != null && ObjList.Count > 0)
                {
                    if (chkAll.IsChecked == true)
                    {
                        foreach (var item in ObjList)
                        {
                            item.IsChecked = true;
                            if (_BatchSelected == null)
                                _BatchSelected = new ObservableCollection<clsBlockItemsVO>();
                            _BatchSelected.Add(item);

                        }
                    }
                    else
                    {
                        foreach (var item in ObjList)
                        {
                            item.IsChecked = false;
                            if (_BatchSelected != null)
                                _BatchSelected.Remove(item);
                        }
                    }

                    dgBlockItemList.ItemsSource = null;
                    dgBlockItemList.ItemsSource = ObjList;
                    dgBlockItemList.UpdateLayout();
                  
                }
                else
                    chkAll.IsChecked = false;
            }

           
           
        }
    }
}

