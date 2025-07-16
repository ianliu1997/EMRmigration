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
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Collections.ObjectModel;
using PalashDynamics.Pharmacy;

namespace PalashDynamics.Radiology.ItemSearch
{
    public partial class AssignBatch : ChildWindow
    {

        public long StoreID { get; set; }
        public long SelectedItemID { get; set; }
        public string ItemName { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public event RoutedEventHandler OnAddButton_Click;
        
        public PagedSortableCollectionView<clsItemStockVO> BatchList { get; private set; }
        
        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        public bool? IsFree { get; set; }  // Set to Show only Free Item Batches

        public AssignBatch()
        {
            InitializeComponent();

            BatchList = new PagedSortableCollectionView<clsItemStockVO>();
            BatchList.OnRefresh += new EventHandler<RefreshEventArgs>(BatchList_OnRefresh);
            BatchPageSize = 15;
        }

        public int BatchPageSize
        {
            get
            {
                return BatchList.PageSize;
            }
            set
            {
                if (value == BatchList.PageSize) return;
                BatchList.PageSize = value;
                //RaisePropertyChanged("BatchPageSize");
            }
        }


        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (StoreID > 0 && SelectedItemID > 0 && ItemName!="")
            {
                _BatchSelected = new ObservableCollection<clsItemStockVO>();
                txtItemName.Text = ItemName;
                FillStore();
                GetBatches();
            }
        }


    

        private void FillStore()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;
                    cmbStore.SelectedValue = StoreID;


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        
        void BatchList_OnRefresh(object sender, RefreshEventArgs e)
        {
            
            GetBatches();
        }
       

        private void ItemSearchButton_Click(object sender, RoutedEventArgs e)
        {
            GetBatches();
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgItemBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtItemName_LostFocus(object sender, RoutedEventArgs e)
        {

        }


        public void GetBatches()
        {
            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
           
            
            BizAction.StoreID = StoreID;
            BizAction.ItemID = SelectedItemID;
            
            BizAction.ShowExpiredBatches = ShowExpiredBatches;
            BizAction.ShowZeroStockBatches = ShowZeroStockBatches;

            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = BatchList.PageSize;
            BizAction.StartIndex = BatchList.PageIndex * BatchList.PageSize;

            BizAction.IsFree = this.IsFree;  // Set to Show only Free Item Batches

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                    BatchList.Clear();
                    BatchList.TotalItemCount = result.TotalRows;
                    foreach (clsItemStockVO item in result.BatchList)
                    {
                        item.IsBatchEnabled = true;
                        BatchList.Add(item);
                    }

                    dgItemBatches.ItemsSource = null;
                    dgItemBatches.ItemsSource = BatchList;

                    ItemBatchDataPager.Source = null;
                    ItemBatchDataPager.PageSize = BizAction.MaximumRows;
                    ItemBatchDataPager.Source = BatchList;
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            if (_BatchSelected.Count == 0)
            {
                isValid = false;
            }

            if (isValid)
            {

                this.DialogResult = true;

                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string strMsg = "No Batches Selected for Adding";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemBatches.SelectedItem != null)
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                {
                    _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
                    dgItemBatches.CurrentColumn.IsReadOnly = true;
                    foreach (var item in BatchList.ToList())
                    {
                        if (item.BatchID != ((clsItemStockVO)dgItemBatches.SelectedItem).BatchID)
                        {
                            item.IsBatchEnabled = false;
                        }
                        else
                        {
                            item.IsBatchEnabled = true;
                        }
                    }
                }
                else
                {
                    _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);
                    foreach (var item in BatchList.ToList())
                    {
                        item.IsBatchEnabled = true;
                    }
                }
                dgItemBatches.ItemsSource = null;
                dgItemBatches.ItemsSource = BatchList;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        
    }
}

