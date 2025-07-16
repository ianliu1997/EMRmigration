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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;


using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.OperationTheatre.ViewModels;

namespace PalashDynamics.OperationTheatre
{
    public partial class ItemList : ChildWindow
    {
       // bool isLoaded = false;
        public event RoutedEventHandler OnSaveButton_Click;
        public ItemList()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(ItemList_Loaded);
            ShowBatches = true;
            ShowExpiredBatches = false;
           // ShowItems = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public long StoreID { get; set; }
        public long SupplierID { get; set; }
        //Added by Pallavi for the items that are selected without grn on grnreturn
        public long GRNReturnStoreId { get; set; }
        public bool AllowStoreSelection { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }
        void ItemList_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (AllowStoreSelection == false)
            {
                cmbStore.IsEnabled = false;
            }
            FillStores(ClinicID);
            if (!ShowBatches)
            {
                BatchContainer.Visibility = System.Windows.Visibility.Collapsed;
                ItemContainer.Height = 355;
                chkSelectAll.Visibility = System.Windows.Visibility.Visible;
                //Commented By Pallavi For PO
                //if (AllowStoreSelection == false)
                //{
                //    cmbStore.IsEnabled = false;
                //}
            }
            else
            {
                chkSelectAll.Visibility = System.Windows.Visibility.Collapsed;
                dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }

        }

        public bool ShowBatches { get; set; }
      //  public bool ShowItems { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }

        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        private long _ClinicID; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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

        public clsUserVO loggedinUser { get; set; }
        private void ItemSearchButton_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            
            if (cmbStore.SelectedItem == null)
            {
                cmbStore.TextBox.SetValidation("Please select the store");
                cmbStore.TextBox.RaiseValidationError();
                res = false;
            }
            else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                cmbStore.TextBox.SetValidation("Please select the store");
                cmbStore.TextBox.RaiseValidationError();
                res = false;
            }
            else
                cmbStore.ClearValidationError();
           
            
            if (res)
            {
                if (!ShowBatches)
                    ((ItemSearchViewModel)this.DataContext).PageSize = 10;
                ((ItemSearchViewModel)this.DataContext).BizActionObject.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                ((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                ((ItemSearchViewModel)this.DataContext).SupplierID = SupplierID;
                ((ItemSearchViewModel)this.DataContext).ShowExpiredBatches = ShowExpiredBatches;
                ((ItemSearchViewModel)this.DataContext).ShowZeroStockBatches = ShowZeroStockBatches;
                ((ItemSearchViewModel)this.DataContext).ShowScrapItems = ShowScrapItems;
                ((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;

              
                ((ItemSearchViewModel)this.DataContext).GetData();

         
                GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                peopleDataPager.PageIndex = 0;
            }
           
        }
        
        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();
            
            if (pClinicID>0)
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
                    objList.Add(new MasterListItem(0, "- Select -",true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;

                    }
                    else
                    {
                        if (objList.Count > 1)
                        {cmbStore.SelectedItem = objList[1];
                        GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                        }
                        else
                        {
                            cmbStore.SelectedItem = objList[0];
                            GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction,  loggedinUser );
            Client.CloseAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DataContext = new clsGetPatientGeneralDetailsListBizActionVO();
            this.DataContext = new ItemSearchViewModel();
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO(); // (clsPatientGeneralVO)dataGrid2.SelectedItem;
            peopleDataPager.PageIndex = 0;
            //dataGrid2.ItemsSource = null; 
            
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            if (ShowBatches == true)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsItemMasterVO>();

                _ItemSelected.Clear();
                _ItemSelected.Add((clsItemMasterVO)dataGrid2.SelectedItem);

                if (_BatchSelected != null)
                    _BatchSelected.Clear();

                if (dataGrid2.SelectedItem != null)
                {
                    ((ItemSearchViewModel)this.DataContext).SelectedItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ID;
                    ((ItemSearchViewModel)this.DataContext).GetBatches();
                }
            }          
        }
        
        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            _ItemSelected = null;
            _BatchSelected = null;
            
            this.DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

           
            //this.DialogResult = true;
            //StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            //if (OnSaveButton_Click != null)
            //    OnSaveButton_Click(this, new RoutedEventArgs());
            bool isValid = true;

            if (ShowBatches == false)
            {
                if (_ItemSelected == null)
                    isValid = false;
                else if (_ItemSelected.Count == 0)
                    isValid = false;
            }
            else
            {
                if (_BatchSelected == null)
                {
                    isValid = false;
                }
                else if (_BatchSelected.Count <= 0)
                {
                    isValid = false;
                }
            }

            if (isValid)
            {
                this.DialogResult = true;
                StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
            else
            {
                string msgText = "";

                //if (ShowBatches)
                //    msgText = "Batch/s not Selected.";
                //else
                //    msgText = "Item/s not Selected.";

                if (ShowBatches)
                    msgText = "No Batch Selected.";
                else
                    msgText = "No Item Selected.";

                if (ShowBatches == true && _ItemSelected != null && _ItemSelected.Count > 0 && _ItemSelected[0].BatchesRequired == false)
                    msgText = "Items Stock not Selected.";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

        }

        private void dgItemBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemBatches.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsItemStockVO>();

                CheckBox chk = (CheckBox)sender;
             
                if (chk.IsChecked == true)
                    _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
                else
                    _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                if (_ItemSelected == null)
                    _ItemSelected = new ObservableCollection<clsItemMasterVO>();

                CheckBox chk = (CheckBox)sender;
                if (chk.IsChecked == true)
                    _ItemSelected.Add((clsItemMasterVO)dataGrid2.SelectedItem);
                else
                    _ItemSelected.Remove((clsItemMasterVO)dataGrid2.SelectedItem);
                
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsItemMasterVO> ObjList = new PagedSortableCollectionView<clsItemMasterVO>();
            if (dataGrid2.ItemsSource != null )
            {
                ObjList = (PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource;

                if (ObjList != null && ObjList.Count > 0)
                {
                    if (chkSelectAll.IsChecked == true)
                    {
                        foreach (var item in ObjList)
                        {
                            item.Status = true;
                            if (_ItemSelected == null)
                                _ItemSelected = new ObservableCollection<clsItemMasterVO>();
                            _ItemSelected.Add(item);

                        }
                    }
                    else
                    {
                        foreach (var item in ObjList)
                        {
                            item.Status = false;
                            if (_ItemSelected != null)
                                _ItemSelected.Remove(item);
                        }
                    }
                    dataGrid2.ItemsSource = null;
                    dataGrid2.ItemsSource = ObjList;
                }
                else
                    chkSelectAll.IsChecked = false;
            }
        }

        private void txtItemName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtItemName.Text != "")
            {
                if (Extensions.IsItSpecialChar(txtItemName.Text) == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Should not enter special characters", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    txtItemName.Text = "";
                }
            }
        }

        private void txtBrandName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBrandName.Text != "")
            {
                if (Extensions.IsItSpecialChar(txtBrandName.Text) == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Should not enter special characters", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    txtBrandName.Text = "";
                }
            }
        }

        private void txtItemCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtItemCode.Text != "")
            {
                if (Extensions.IsItSpecialChar(txtItemCode.Text) == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Should not enter special characters", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    txtItemCode.Text = "";
                }
            }
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID; ;
            //dataGrid2.ItemsSource = null;
            //if (ShowBatches)
            //    dgItemBatches.ItemsSource = null;
        }
               
    }
}
