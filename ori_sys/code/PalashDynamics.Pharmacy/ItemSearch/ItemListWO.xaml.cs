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
using PalashDynamics.Pharmacy.ViewModels;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;



namespace PalashDynamics.Pharmacy
{
    public partial class ItemListWO : ChildWindow
    {
        string msgText = "";
        bool IsWorkOrder = false;
       // bool isLoaded = false;
        public event RoutedEventHandler OnSaveButton_Click;
        WaitIndicator indicator = new WaitIndicator();
      
        public ItemListWO(bool IsWorkOrder)
        {
            this.IsWorkOrder = IsWorkOrder;
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(ItemListWO_Loaded);
            ShowBatches = true;
            ShowExpiredBatches = false;
            
           // dataGrid2.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dataGrid2_CellEditEnded);
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
        public bool ShowQuantity { get; set; }
        public double  RequiredQuantity { get; set; }
        public Boolean IsFromOpeningBalance { get; set; }
        public long SelectedItemID { get; set; }
        void ItemListWO_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (AllowStoreSelection == false)
            {
                //cmbStore.IsEnabled = false;
            }

            FillStores(ClinicID);
            FillItemCategory();
            FillItemGroup();
            FillMolecule();

            if (ShowQuantity == true)
            {
               // dataGrid2.Columns[5].Visibility = System.Windows.Visibility.Visible;
            }

            //if (ShowZeroStockBatches == true)
            //    dataGrid2.Columns[3].Visibility = System.Windows.Visibility.Visible;
            //else
            //    dataGrid2.Columns[3].Visibility = System.Windows.Visibility.Collapsed;


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
            DataList1 = new PagedSortableCollectionView<clsItemMasterVO>();
            DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
             PageSize = 15;

        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
      

        }
        public int PageSize
        {
            get
            {
                return DataList1.PageSize;
            }
            set
            {
                if (value == DataList1.PageSize) return;
                DataList1.PageSize = value;
           //     RaisePropertyChanged("PageSize");
            }
        }
        public bool ShowBatches { get; set; }
      //  public bool ShowItems { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }


         

        //private ObservableCollection<clsItemStockVO> _BatchSelected;
        //public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

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
        public PagedSortableCollectionView<clsItemMasterVO> DataList1 { get; private set; }
        //public clsGetItemListForWorkOrderBizActionVO BizActionObjectWO { get; set; }

        private void ItemSearchButton_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            
            //if (cmbStore.SelectedItem == null)
            //{
            //    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            //    //{
            //    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgStore");
            //    //}
            //    //else
            //    //{
            //    //    msgText = "Please select the store";
            //    //}
            //    msgText = "Please select the store";
            //    cmbStore.TextBox.SetValidation(msgText);
            //    cmbStore.TextBox.RaiseValidationError();
            //    res = false;
            //}
            //else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
            //{
            //    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
            //    //{
            //    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgStore");
            //    //}
            //    //else
            //    //{
            //    //    msgText = "Please select the store";
            //    //}
            //    msgText = "Please select the store";
            //    cmbStore.TextBox.SetValidation(msgText);
            //    cmbStore.TextBox.RaiseValidationError();
            //    res = false;
            //}
            //else
            //    cmbStore.ClearValidationError();

            #region Commented on 29/12/2015
            //if (res)
            //{
            //    if (!ShowBatches)
            //        ((ItemSearchViewModel)this.DataContext).PageSize = 10;

            //    //((ItemSearchViewModel)this.DataContext).BizActionObject.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
            //    //((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

            //    ((ItemSearchViewModel)this.DataContext).SupplierID = SupplierID;
            //    ((ItemSearchViewModel)this.DataContext).ShowExpiredBatches = ShowExpiredBatches;
            //    ((ItemSearchViewModel)this.DataContext).ShowZeroStockBatches = ShowZeroStockBatches;
            //    ((ItemSearchViewModel)this.DataContext).ShowScrapItems = ShowScrapItems;
            //    ((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;
            //    ((ItemSearchViewModel)this.DataContext).IsFromOpeningBalance = IsFromOpeningBalance;
            //    //if (cmbCategory.SelectedItem != null )
            //    //((ItemSearchViewModel)this.DataContext).BizActionObject.ItemCategoryId = ((MasterListItem)cmbCategory.SelectedItem).ID;

            //    //if (cmbGroup.SelectedItem != null)
            //    //((ItemSearchViewModel)this.DataContext).BizActionObject.ItemGroupId = ((MasterListItem)cmbGroup.SelectedItem).ID;

            //    //if (cmbMolecule.SelectedItem != null)
            //    //    ((ItemSearchViewModel)this.DataContext).BizActionObject.MoleculeName = ((MasterListItem)cmbMolecule.SelectedItem).ID;

            //    ((ItemSearchViewModel)this.DataContext).GetData();

         
            //    //GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
            //    //peopleDataPager.PageIndex = 0;
            //}
            #endregion
            //((ItemSearchViewModel)this.DataContext).StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

            /*** 30/12/2015**/
            //((ItemSearchViewModel)this.DataContext).PageSize = 10;
            //((ItemSearchViewModel)this.DataContext).loggedinUser = loggedinUser;
            //((ItemSearchViewModel)this.DataContext).GetDataWorkOrder();
            //peopleDataPager.PageIndex = 0;

            clsGetItemListForWorkOrderBizActionVO Bizaction = new clsGetItemListForWorkOrderBizActionVO();
                       
                Bizaction.ItemCode = txtItemCode.Text;
                Bizaction.ItemName = txtItemName.Text;
                Bizaction.IsPagingEnabled = true;
                Bizaction.MaximumRows = DataList1.PageSize;
                Bizaction.StartIndex = DataList1.PageSize * DataList1.PageIndex;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetItemListForWorkOrderBizActionVO result = ea.Result as clsGetItemListForWorkOrderBizActionVO;
                        DataList1.Clear();
                        DataList1.TotalItemCount = result.TotalRows;
                        //BatchList1.Clear();
                        //BatchList.TotalItemCount = 0;
                        foreach (clsItemMasterVO item in result.ItemList)
                        {
                            DataList1.Add(item);
                        }
                        dataGrid2.ItemsSource = null;
                        dataGrid2.ItemsSource = DataList1;

                        if (DataList1.Count > 0)
                        {
                            SelectedItemID = DataList1[0].ID;
                            //if (SelectedItemID > 0)
                            //    GetBatches();
                        }

                    }
                    indicator.Close();
                };
                client.ProcessAsync(Bizaction, loggedinUser);
                client.CloseAsync();
            



        }

        void dataGrid2_CellEditEnded(object Sender, DataGridCellEditEndedEventArgs e)
        {
            //clsItemMasterVO obj = new clsItemMasterVO();
            //obj = (clsItemMasterVO)dataGrid2.SelectedItem;
            //if (obj.RequiredQuantity == 0)
            //{
            //    MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Zero. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    obj.RequiredQuantity = 1;
            //    return;
            //}

            //if (obj.RequiredQuantity < 0)
            //{
            //    MessageBoxControl.MessageBoxChildWindow mgbx = null;

            //    mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity In The List Can't Be Negative. Please Enter Quantiy Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    mgbx.Show();
            //    obj.RequiredQuantity = 1;
            //    return;
            //}

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

                    //cmbStore.ItemsSource = null;
                    //cmbStore.ItemsSource = objList;

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
                        //cmbStore.SelectedValue = StoreID;
                    }
                    else
                    {
                        if (objList.Count > 1)
                        {
                        //{cmbStore.SelectedItem = objList[1];
                        //GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                        }
                        else
                        {
                            //cmbStore.SelectedItem = objList[0];
                            //GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction,  loggedinUser );
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

                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbCategory.ItemsSource = null;
                    //cmbCategory.ItemsSource = objList;

                    //cmbCategory.SelectedItem = objList[0];

                    
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

                    objList.Add(new MasterListItem(0, "- Select -", true));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                    //cmbGroup.ItemsSource = null;
                    //cmbGroup.ItemsSource = objList;

                    //cmbGroup.SelectedItem = objList[0];

                }
            };

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }

        #region Added By MMBABU
        
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

                    //cmbMolecule.ItemsSource = objList;
                    //cmbMolecule.SelectedItem = objList[0];
                }

            };

            client.ProcessAsync(BizAction, loggedinUser);
            client.CloseAsync();

            //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //client.CloseAsync();
        }
        
        #endregion

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

                //if (_BatchSelected != null)
                //    _BatchSelected.Clear();

                if (dataGrid2.SelectedItem != null)
                {
                    ((ItemSearchViewModel)this.DataContext).SelectedItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ID;
                    //((ItemSearchViewModel)this.DataContext).GetBatches();
                }
            }          
        }
        
        private void cmdCancelButton_Click(object sender, RoutedEventArgs e)
        {
            _ItemSelected = null;
       //     _BatchSelected = null;
            
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
                //if (_BatchSelected == null)
                //{
                //    isValid = false;
                //}
                //else if (_BatchSelected.Count <= 0)
                //{
                //    isValid = false;
                //}
            }

            if (isValid)
            {
                this.DialogResult = true;
                //StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }
            else
            {
     

                //if (ShowBatches)
                //    msgText = "Batch/s not Selected.";
                //else
                //    msgText = "Item/s not Selected.";
               
                if (ShowBatches)
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgNoBatchSelected");
                    //}
                    //else
                    //{
                    //      msgText = "No Batch Selected";
                    //}

                     msgText = "No Batch Selected";

                else
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgNoItemSelected");
                    //}
                    //else
                    //{
                    //    msgText = "No Item Selected";
                    //}
                msgText = "No Item Selected";

                if (ShowBatches == true && _ItemSelected != null && _ItemSelected.Count > 0 && _ItemSelected[0].BatchesRequired == false)
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgItemsStock");
                    //}
                    //else
                    //{
                    //    msgText = "Items Stock not Selected";
                    //}
                msgText = "Items Stock not Selected";
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
            //if (dgItemBatches.SelectedItem != null)
            ////{
            //    if (_BatchSelected == null)
            //        _BatchSelected = new ObservableCollection<clsItemStockVO>();

            //    CheckBox chk = (CheckBox)sender;
             
            //    if (chk.IsChecked == true)
            //        _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
            //    else
            //        _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

            //}
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
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgspecialcharacters");
                    //}
                    //else
                    //{
                    //    msgText = "Should not enter special characters";
                    //}
                    msgText = "Should not enter special characters";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    txtItemName.Text = "";
                }
            }
        }

        //private void txtBrandName_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (txtBrandName.Text != "")
        //    {
        //        if (Extensions.IsItSpecialChar(txtBrandName.Text) == false)
        //        {
        //            //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
        //            //{
        //            //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgspecialcharacters");
        //            //}
        //            //else
        //            //{
        //            //    msgText = "Should not enter special characters";
        //            //}
        //            msgText = "Should not enter special characters";
        //            MessageBoxControl.MessageBoxChildWindow msgWD =
        //                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgWD.Show();
        //            txtBrandName.Text = "";
        //        }
        //    }
        //}

        private void txtItemCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtItemCode.Text != "")
            {
                if (Extensions.IsItSpecialChar(txtItemCode.Text) == false)
                {
                    //if (((IApplicationConfiguration)App.Current).LocalizedManager != null)
                    //{
                    //    msgText = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("msgspecialcharacters");
                    //}
                    //else
                    //{
                    //    msgText = "Should not enter special characters";
                    //}
                    msgText = "Should not enter special characters";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                    txtItemCode.Text = "";
                }
            }
        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID; ;
            //dataGrid2.ItemsSource = null;
            //if (ShowBatches)
            //    dgItemBatches.ItemsSource = null;
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {

        }
               
    }
}
