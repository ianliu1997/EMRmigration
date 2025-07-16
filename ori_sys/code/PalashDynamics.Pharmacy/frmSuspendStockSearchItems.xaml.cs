using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;
using CIMS;
using System.Collections.ObjectModel;

namespace PalashDynamics.Pharmacy
{
    public partial class frmSuspendStockSearchItems : ChildWindow
    {
        #region Variable Declarations
        public List<ItemStoreLocationDetailsVO> _SelectedItemList;
        public event RoutedEventHandler OnSaveButton_Click;
        public clsUserVO loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
        public long StoreID { get; set; }
        WaitIndicator objWIndicator;
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
        private ObservableCollection<clsBlockItemsVO> _ItemSelected;
        public ObservableCollection<clsBlockItemsVO> SelectedItems { get { return _ItemSelected; } }
        int ClickedFlag = 0;
        #endregion

        #region Constructor and Loaded
        public frmSuspendStockSearchItems()
        {
            InitializeComponent();
            objWIndicator = new WaitIndicator();
            DataList = new PagedSortableCollectionView<clsBlockItemsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            _SelectedItemList = new List<ItemStoreLocationDetailsVO>();
            _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillStores(loggedinUser.UserLoginInfo.UnitId);
            FillRack();
            FillShelf();
            FillContainer();
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetItemList();
        }
        #endregion

        #region Button Clicked Events
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemList.SelectedItem != null)
            {
                // Added by Ashish Z. for Validation
                if (((CheckBox)sender).IsChecked == true)
                {
                    //ValidationForInTransitItem(((clsBlockItemsVO)dgItemList.SelectedItem).ItemID, ((clsBlockItemsVO)dgItemList.SelectedItem).StoreID);
                    try
                    {
                        objWIndicator.Show();
                        clsGetBlockItemsListBizActionVO BizActionVO = new clsGetBlockItemsListBizActionVO();
                        BizActionVO.ItemDetails = new clsBlockItemsVO();
                        BizActionVO.ItemDetails.ItemID = ((clsBlockItemsVO)dgItemList.SelectedItem).ItemID;// ItemID;
                        BizActionVO.ItemDetails.StoreID = ((clsBlockItemsVO)dgItemList.SelectedItem).StoreID;//StoreID;
                        BizActionVO.ItemDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        BizActionVO.IsForCheckInTransitItems = true;
                        BizActionVO.IsfromSuspendStockSearchItems = false;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            objWIndicator.Close();
                            if (args.Error == null && args.Result != null)
                            {
                                //if ((args.Result as clsGetBlockItemsListBizActionVO).SuccessStatus == 1)
                                //{
                                //    ((CheckBox)sender).IsChecked = false;
                                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                //        new MessageBoxControl.MessageBoxChildWindow("", "'" + ((clsBlockItemsVO)dgItemList.SelectedItem).ItemName + "'" + " Item is in Transit for Issue to Clinic", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                //    msgW1.Show();
                                //}
                                //else 
                                if ((args.Result as clsGetBlockItemsListBizActionVO).SuccessStatus == 2)
                                {
                                    ((CheckBox)sender).IsChecked = false;
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "'" + ((clsBlockItemsVO)dgItemList.SelectedItem).ItemName + "'" + " Item is in Transit for Receive Issue", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                                else if ((args.Result as clsGetBlockItemsListBizActionVO).SuccessStatus == 3)
                                {
                                    ((CheckBox)sender).IsChecked = false;
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "'" + ((clsBlockItemsVO)dgItemList.SelectedItem).ItemName + "'" + "Item is in Transit for Receive Return Issue", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                                else
                                {
                                    if (_SelectedItemList.ToList().Count > 0)
                                    {
                                        var item1 = from r in _SelectedItemList.ToList()
                                                    where r.ItemID == ((clsBlockItemsVO)dgItemList.SelectedItem).ItemID
                                                    select r;

                                        if (item1.ToList().Count == 0)
                                        {
                                            if (_ItemSelected == null) _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
                                            _ItemSelected.Add((clsBlockItemsVO)dgItemList.SelectedItem);
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + ((clsBlockItemsVO)dgItemList.SelectedItem).ItemName + "'" + " Item is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW1.Show();
                                            ((CheckBox)sender).IsChecked = false;
                                        }
                                    }
                                    else
                                    {
                                        if (_ItemSelected == null) _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
                                        _ItemSelected.Add((clsBlockItemsVO)dgItemList.SelectedItem);
                                    }
                                }
                            }
                        };
                        client.ProcessAsync(BizActionVO, new clsUserVO());
                        client.CloseAsync();
                    }
                    catch (Exception Ex)
                    {
                        objWIndicator.Close();
                        throw;
                    }


                    //if (_SelectedItemList.ToList().Count > 0)
                    //{
                    //    var item1 = from r in _SelectedItemList.ToList()
                    //                where r.ItemID == ((clsBlockItemsVO)dgItemList.SelectedItem).ItemID
                    //                select r;

                    //    if (item1.ToList().Count == 0)
                    //    {
                    //        if (_ItemSelected == null) _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
                    //        _ItemSelected.Add((clsBlockItemsVO)dgItemList.SelectedItem);
                    //    }
                    //    else
                    //    {
                    //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                     new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + ((clsBlockItemsVO)dgItemList.SelectedItem).ItemName + "'" + " Item is already Added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //        msgW1.Show();
                    //        ((CheckBox)sender).IsChecked = false;
                    //    }
                    //}
                    //else
                    //{
                    //    if (_ItemSelected == null) _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
                    //    _ItemSelected.Add((clsBlockItemsVO)dgItemList.SelectedItem);
                    //}
                }
                else
                {
                    if (_ItemSelected == null) _ItemSelected = new ObservableCollection<clsBlockItemsVO>();
                    _ItemSelected.Remove((clsBlockItemsVO)dgItemList.SelectedItem);
                }
            }


            //if (dgItemList.SelectedItem != null)
            //{
            //    if (_ItemSelected == null)
            //        _ItemSelected = new ObservableCollection<clsBlockItemsVO>();

            //    CheckBox chk = (CheckBox)sender;

            //    if (chk.IsChecked == true)
            //        _ItemSelected.Add((clsBlockItemsVO)dgItemList.SelectedItem);
            //    else
            //        _ItemSelected.Remove((clsBlockItemsVO)dgItemList.SelectedItem);
            //}
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            GetItemList();
            dgDataPager.PageIndex = 0;

        }

        private void cmdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                GetItemList();
                dgDataPager.PageIndex = 0;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //ClickedFlag += 1;
            //if (ClickedFlag == 1)
            //{
            if (_ItemSelected.Count() > 0)
            {
                this.DialogResult = true;
                if (OnSaveButton_Click != null)
                {
                    OnSaveButton_Click(this, new RoutedEventArgs());
                }
                //ClickedFlag = 0;
            }
            else
            {
                //ClickedFlag = 0;
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Item from List", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
            //}
            //else
            //    ClickedFlag = 0;
        }
        #endregion

        #region Private Methods
        private void ValidationForInTransitItem(long ItemID, long StoreID)
        {
            try
            {
                objWIndicator.Show();
                clsGetBlockItemsListBizActionVO BizActionVO = new clsGetBlockItemsListBizActionVO();
                BizActionVO.ItemDetails = new clsBlockItemsVO();
                BizActionVO.ItemDetails.ItemID = ItemID;
                BizActionVO.ItemDetails.StoreID = StoreID;
                BizActionVO.IsForCheckInTransitItems = true;
                BizActionVO.IsfromSuspendStockSearchItems = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    objWIndicator.Close();
                    if (args.Error == null && args.Result != null)
                    {
                        if ((args.Result as clsGetBlockItemsListBizActionVO).SuccessStatus == 1)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "This Item is already found in Transit for Isssue", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception Ex)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void GetItemList()
        {
            try
            {
                if (objWIndicator == null) objWIndicator = new WaitIndicator();
                objWIndicator.Show();
                clsGetBlockItemsListBizActionVO BizAction = new clsGetBlockItemsListBizActionVO();
                BizAction.IsfromSuspendStockSearchItems = true;
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

                            dgItemList.ItemsSource = null;
                            dgItemList.ItemsSource = DataList;
                            dgDataPager.Source = null;
                            dgDataPager.PageSize = BizAction.MaximumRows;
                            dgDataPager.Source = DataList;
                        }

                    }
                    objWIndicator.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                objWIndicator.Close();
                throw;
            }
        }

        private void FillStores(long pClinicID)
        {
            try
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
                        GetItemList();
                    }
                };

                Client.ProcessAsync(BizAction, loggedinUser);
                Client.CloseAsync();
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

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
                        //tempRackCombo = objList.ToList();
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
                        //tempShelfCombo = objList.ToList();
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
                        //tempBinCombo = objList.ToList();
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
    }
        #endregion
}


