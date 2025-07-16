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
using PalashDynamics.Pharmacy.ViewModels;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Collections;

namespace PalashDynamics.Administration
{
    public partial class ItemListWithCategory : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;

        public ItemListWithCategory()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(ItemListWithCategory_Loaded);
            ShowBatches = true;
            ShowExpiredBatches = false;
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
        public bool ShowQuantity { get; set; }
        public double RequiredQuantity { get; set; }

        public bool IsCategoryOn { get; set; }
        public bool IsGroupOn { get; set; }

        void ItemListWithCategory_Loaded(object sender, RoutedEventArgs e)
        {
            if (AllowStoreSelection == false)
            {
                cmbStore.IsEnabled = false;
            }

            FillStores(ClinicID);
            FillItemCategory();
            FillItemGroup();
            FillMolecule();
            FillManufacturedBy();

            //if (ShowQuantity == true)
            //{
            //    dataGrid2.Columns[5].Visibility = System.Windows.Visibility.Visible;
            //}

            //if (ShowZeroStockBatches == true)
            //    dataGrid2.Columns[3].Visibility = System.Windows.Visibility.Visible;
            //else
            //    dataGrid2.Columns[3].Visibility = System.Windows.Visibility.Collapsed;


            if (rdbItemsCategory.IsChecked == true) //if (!ShowBatches)
            {
                //CategoryContainer.Visibility = System.Windows.Visibility.Collapsed;
                //ItemContainer.Height = 355;

                chkSelectAll.Visibility = System.Windows.Visibility.Visible;
                CategoryContainer.Visibility = System.Windows.Visibility.Visible;

                ItemContainer.Visibility = Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Collapsed;

                //if (AllowStoreSelection == false)
                //{
                //    cmbStore.IsEnabled = false;
                //}
            }
            else if (rdbItems.IsChecked == true)
            {
                ItemContainer.Visibility = Visibility.Visible;
                chkSelectAll.Visibility = Visibility.Collapsed;
                //dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Collapsed;

                CategoryContainer.Visibility = Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Collapsed;
            }
            else if (rdbItemsGroup.IsChecked == true)
            {
                GroupContainer.Visibility = Visibility.Visible;
                chkSelectAll.Visibility = Visibility.Visible;
                //dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Collapsed;

                CategoryContainer.Visibility = Visibility.Collapsed;
                ItemContainer.Visibility = Visibility.Collapsed;
            }

            if (rdbItemsCategory.IsChecked == true)
            {
                //DocSpecilization.Visibility = System.Windows.Visibility.Visible;
                //DocService.Visibility = System.Windows.Visibility.Collapsed;
                FillItemCategory();
                SearchPanel.Visibility = Visibility.Collapsed;

            }
            else if (rdbItems.IsChecked == true)
            {
                //DocService.Visibility = System.Windows.Visibility.Visible;
                //DocSpecilization.Visibility = System.Windows.Visibility.Collapsed;
                SearchPanel.Visibility = Visibility.Visible;
                FillItems();  // FillDataGrid();

            }
            else if (rdbItemsGroup.IsChecked == true)
            {
                //DocSpecilization.Visibility = System.Windows.Visibility.Visible;
                //DocService.Visibility = System.Windows.Visibility.Collapsed;
                FillItemGroup();
                SearchPanel.Visibility = Visibility.Collapsed;

            }


            FillItemCategory();
            FillItemGroup();

        }

        public bool ShowBatches { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }

        //private ObservableCollection<clsItemStockVO> _BatchSelected;
        //public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        private ObservableCollection<MasterListItem> _CategorySelected;
        public ObservableCollection<MasterListItem> SelectedCategories { get { return _CategorySelected; } }

        private ObservableCollection<MasterListItem> _GroupSelected;
        public ObservableCollection<MasterListItem> SelectedGroups { get { return _GroupSelected; } }

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
            FillItems();

        }

        private void FillItems()
        {
            bool res = true;

            //if (cmbStore.SelectedItem == null)
            //{
            //    cmbStore.SetValidation("Please select the store");
            //    cmbStore.RaiseValidationError();
            //    res = false;
            //}
            //else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
            //{
            //    cmbStore.SetValidation("Please select the store");
            //    cmbStore.RaiseValidationError();
            //    res = false;
            //}
            //else
            //    cmbStore.ClearValidationError();


            if (res)
            {
                //if (!ShowBatches)
                //    ((ItemSearchViewModel)this.DataContext).PageSize = 10;

                ((ItemSearchViewModel)this.DataContext).PageSize = 10;
                //((ItemSearchViewModel)this.DataContext).BizActionObject.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                //  ((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                ((ItemSearchViewModel)this.DataContext).SupplierID = SupplierID;
                ((ItemSearchViewModel)this.DataContext).ShowExpiredBatches = ShowExpiredBatches;
                ((ItemSearchViewModel)this.DataContext).ShowZeroStockBatches = ShowZeroStockBatches;
                ((ItemSearchViewModel)this.DataContext).ShowScrapItems = ShowScrapItems;
                ((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;

                if (cmbCategory.SelectedItem != null)
                    ((ItemSearchViewModel)this.DataContext).BizActionObject.ItemCategoryId = ((MasterListItem)cmbCategory.SelectedItem).ID;

                if (cmbGroup.SelectedItem != null)
                    ((ItemSearchViewModel)this.DataContext).BizActionObject.ItemGroupId = ((MasterListItem)cmbGroup.SelectedItem).ID;

                if (cmbMolecule.SelectedItem != null)
                    ((ItemSearchViewModel)this.DataContext).BizActionObject.MoleculeName = ((MasterListItem)cmbMolecule.SelectedItem).ID;

                if (cboMfg.SelectedItem!=null)
                    ((ItemSearchViewModel)this.DataContext).BizActionObject.ManufactureCompanyID = ((MasterListItem)cboMfg.SelectedItem).ID;

                ((ItemSearchViewModel)this.DataContext).GetData();


                // GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                //peopleDataPager.PageIndex =-1;
            }
        }

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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    ////Added by Saily P on 08.03.13 Purpose - fill only those stores which are assigned to the user.
                    //List<clsUserUnitDetailsVO> TempUnitDetails = new List<clsUserUnitDetailsVO>();
                    //TempUnitDetails=((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.UnitDetails;
                    //foreach (var item in TempUnitDetails)
                    //{
                    //    if (item.IsDefault == true && item.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
                    //    {

                    //    }
                    //}
                    ////

                    if (StoreID > 0)
                    {
                        cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                        {
                            cmbStore.SelectedItem = objList[1];
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

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        private void FillItemCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();

            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ItemCategory;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //if (rdbItems.IsChecked == true)
                    //{
                    objList.Add(new MasterListItem(0, "- Select -", true));
                    //}

                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //if (rdbItems.IsChecked == true)
                    //{
                    cmbCategory.ItemsSource = null;
                    cmbCategory.ItemsSource = objList.DeepCopy();

                    cmbCategory.SelectedItem = objList[0];
                    //}

                    List<MasterListItem> objList2 = new List<MasterListItem>();
                    objList2.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //if (rdbItemsCategory.IsChecked == true)
                    //{
                    dgItemCategories.ItemsSource = null;
                    dgItemCategories.ItemsSource = objList2;  // objList.DeepCopy();
                    //}

                }
            };

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        private void FillItemGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ItemGroup;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    ////objList.Add(new MasterListItem(0, "- Select -", true));

                    //if (rdbItems.IsChecked == true)
                    //{
                    objList.Add(new MasterListItem(0, "- Select -", true));
                    //}

                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //if (rdbItems.IsChecked == true)
                    //{
                    cmbGroup.ItemsSource = null;
                    cmbGroup.ItemsSource = objList;

                    cmbGroup.SelectedItem = objList[0];
                    //}

                    List<MasterListItem> objList2 = new List<MasterListItem>();
                    objList2.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //if (rdbItemsGroup.IsChecked == true)
                    //{
                    dgItemGroups.ItemsSource = null;
                    dgItemGroups.ItemsSource = objList2;  // objList.DeepCopy();
                    //}


                }
            };

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        private void FillManufacturedBy()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ItemCompany;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizAction, new clsUserVO());
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    cboMfg.ItemsSource = null;
                    cboMfg.ItemsSource = objList;
                    cboMfg.SelectedItem = objList[0];
                }
                client.CloseAsync();
            };
        }


        private void FillMolecule()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Molecule;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);
                    cmbMolecule.ItemsSource = objList;
                    cmbMolecule.SelectedItem = objList[0];
                }

            };

            client.ProcessAsync(BizAction, loggedinUser);
            client.CloseAsync();

            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
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
            //// ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            //if (ShowBatches == true)
            //{
            //    if (_ItemSelected == null)
            //        _ItemSelected = new ObservableCollection<clsItemMasterVO>();

            //    _ItemSelected.Clear();
            //    _ItemSelected.Add((clsItemMasterVO)dataGrid2.SelectedItem);

            //    if (_BatchSelected != null)
            //        _BatchSelected.Clear();

            //    if (dataGrid2.SelectedItem != null)
            //    {
            //        ((ItemSearchViewModel)this.DataContext).SelectedItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ID;
            //        ((ItemSearchViewModel)this.DataContext).GetBatches();
            //    }
            //}     
        }

        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            _ItemSelected = null;
            _CategorySelected = null;
            _GroupSelected = null;

            this.DialogResult = false;

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            //StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            //if (OnSaveButton_Click != null)
            //    OnSaveButton_Click(this, new RoutedEventArgs());
            bool isValid = true;

            if (rdbItems.IsChecked == true && (_ItemSelected != null && _ItemSelected.Count > 0))  //if (ShowBatches == false)
            {
                if (_ItemSelected == null)
                    isValid = false;
                else if (_ItemSelected.Count == 0)
                    isValid = false;
            }
            else if (rdbItemsCategory.IsChecked == true && (_CategorySelected != null && _CategorySelected.Count > 0))
            {
                if (_CategorySelected == null)
                {
                    isValid = false;
                }
                else if (_CategorySelected.Count <= 0)
                {
                    isValid = false;
                }
            }
            else if (rdbItemsGroup.IsChecked == true && (_GroupSelected != null && _GroupSelected.Count > 0))
            {
                if (_GroupSelected == null)
                {
                    isValid = false;
                }
                else if (_GroupSelected.Count <= 0)
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

                if (rdbItemsCategory.IsChecked == true)  //if (ShowBatches)
                    msgText = "No Category Selected.";  //"No Batch Selected.";
                else if (rdbItems.IsChecked == true)
                    msgText = "No Item Selected.";
                else if (rdbItemsGroup.IsChecked == true)
                    msgText = "No Group Selected.";

                //if (ShowBatches == true && _ItemSelected != null && _ItemSelected.Count > 0 && _ItemSelected[0].BatchesRequired == false)
                //    msgText = "Items Stock not Selected.";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }

        }

        private void dgItemCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void AddBatch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgItemBatches.SelectedItem != null)
        //    {
        //        if (_BatchSelected == null)
        //            _BatchSelected = new ObservableCollection<clsItemStockVO>();

        //        CheckBox chk = (CheckBox)sender;

        //        if (chk.IsChecked == true)
        //            _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
        //        else
        //            _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

        //    }
        //}

        //chkItemStatus Click Event for Add Selected items
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

        private void chkCategory_Click(object sender, RoutedEventArgs e)
        {
            if (IsGroupOn == false && (_GroupSelected == null || _GroupSelected.Count == 0))
            {
                if (dgItemCategories.SelectedItem != null)
                {
                    if (_CategorySelected == null)
                        _CategorySelected = new ObservableCollection<MasterListItem>();

                    if (((CheckBox)sender).IsChecked == true)
                    {
                        _CategorySelected.Add((MasterListItem)dgItemCategories.SelectedItem); //check[dgSpecilization.SelectedIndex] = true;
                    }
                    else
                    {
                        _CategorySelected.Remove((MasterListItem)dgItemCategories.SelectedItem); //check[dgSpecilization.SelectedIndex] = false;
                    }
                }
            }
            else
            {
                ((CheckBox)sender).IsChecked = false;
                dgItemCategories.UpdateLayout();
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Category cannot be add as Category & Group can not be added at a time!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW3.Show();
            }
        }

        private void chkGroup_Click(object sender, RoutedEventArgs e)
        {
            if (IsCategoryOn == false && (_CategorySelected == null || _CategorySelected.Count == 0))
            {
                if (dgItemGroups.SelectedItem != null)
                {
                    if (_GroupSelected == null)
                        _GroupSelected = new ObservableCollection<MasterListItem>();

                    if (((CheckBox)sender).IsChecked == true)
                    {
                        _GroupSelected.Add((MasterListItem)dgItemGroups.SelectedItem); //check[dgSpecilization.SelectedIndex] = true;
                    }
                    else
                    {
                        _GroupSelected.Remove((MasterListItem)dgItemGroups.SelectedItem); //check[dgSpecilization.SelectedIndex] = false;
                    }
                }
            }
            else
            {
                ((CheckBox)sender).IsChecked = false;
                dgItemGroups.UpdateLayout();
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Group cannot be add as Category & Group can not be added at a time!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW3.Show();
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            PagedSortableCollectionView<clsItemMasterVO> ObjList = new PagedSortableCollectionView<clsItemMasterVO>();
            if (dataGrid2.ItemsSource != null)
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

        private void rdbItems_Click(object sender, RoutedEventArgs e)
        {
            if (rdbItems.IsChecked == true) //if (!ShowBatches)
            {
                //FillItems();
                ////dataGrid2.ItemsSource = null;
                chkSelectAll.Visibility = System.Windows.Visibility.Collapsed;
                ////dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
                SearchPanel.Visibility = Visibility.Visible;
                ItemContainer.Visibility = System.Windows.Visibility.Visible;
                CategoryContainer.Visibility = System.Windows.Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Collapsed;
            }
            else
            {
                //dataGrid2.ItemsSource = null;
                //FillItemCategory();
                //FillItemGroup();
                ItemContainer.Visibility = System.Windows.Visibility.Collapsed;
                ////CategoryContainer.Visibility = System.Windows.Visibility.Visible;
                SearchPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void rdbItemsCategory_Click(object sender, RoutedEventArgs e)
        {
            if (rdbItemsCategory.IsChecked == true)
            {
                //FillItemCategory();
                ////dgItemCategories.ItemsSource = null;

                ////CategoryContainer.Visibility = System.Windows.Visibility.Collapsed;
                ////ItemContainer.Height = 355;
                chkSelectAll.Visibility = System.Windows.Visibility.Visible;
                SearchPanel.Visibility = Visibility.Collapsed;
                CategoryContainer.Visibility = System.Windows.Visibility.Visible;
                ItemContainer.Visibility = System.Windows.Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Collapsed;
            }
            else
            {
                //dgItemCategories.ItemsSource = null;
                //FillItems();
                //FillItemGroup();
                CategoryContainer.Visibility = System.Windows.Visibility.Collapsed;
                ////ItemContainer.Visibility = System.Windows.Visibility.Visible;
                ////SearchPanel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void rdbItemsGroup_Click(object sender, RoutedEventArgs e)
        {
            if (rdbItemsGroup.IsChecked == true)
            {
                //FillItemGroup();
                ////dgItemCategories.ItemsSource = null;

                ////CategoryContainer.Visibility = System.Windows.Visibility.Collapsed;
                ////ItemContainer.Height = 355;
                chkSelectAll.Visibility = Visibility.Visible;
                SearchPanel.Visibility = Visibility.Collapsed;
                GroupContainer.Visibility = Visibility.Visible;
                CategoryContainer.Visibility = Visibility.Collapsed;
                ItemContainer.Visibility = Visibility.Collapsed;
            }
            else
            {
                //dgItemGroups.ItemsSource = null;
                ////dgItemCategories.ItemsSource = null;


                //FillItems();
                //FillItemCategory();

                GroupContainer.Visibility = Visibility.Collapsed;
                ////CategoryContainer.Visibility = Visibility.Collapsed;
                ////ItemContainer.Visibility = Visibility.Visible;
                ////SearchPanel.Visibility = Visibility.Visible;
            }
        }

        private void dgItemGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }






    }
}

