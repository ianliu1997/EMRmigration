using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

using PalashDynamics.Pharmacy.ViewModels;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Billing;
//using PalashDynamics.IVF;

namespace PalashDynamics.Pharmacy.ItemSearch
{
    public partial class PackageItemList : ChildWindow
    {
        string msgText = "";
        public event RoutedEventHandler OnSaveButton_Click;
        public PackageItemList()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(ItemList_Loaded);
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
        public long GRNReturnStoreId { get; set; }
        public bool AllowStoreSelection { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }
        public clsUserVO loggedinUser { get; set; }
        public bool IsFromStockAdjustment { get; set; }
        public bool IsFromCounterSale { get; set; }
        public bool ShowNonZeroStockBatchesSetFromCounterSale { get; set; }
        public bool ShowNotShowPlusThreeMonthExp { get; set; }
        public bool ShowPlusThreeMonthExpSetFromCounterSale { get; set; }
        public long PackageID { get; set; }
        public string PackageName { get; set; }
        public long PatientCatagoryL1 { get; set; }
        public long PatientCatagoryL2 { get; set; }
        public long PatientCatagoryL3 { get; set; }
        public long CompanyID { get; set; }
        public long PatientGenderID { get; set; }
        public DateTime PatientDateOfBirth { get; set; }
        public long PatientID { get; set; }
        public long PatientUnitID { get; set; }
        public clsUserVO objUserVO { get; set; }
        public bool IsSellBySellingUnit { get; set; }

        void ItemList_Loaded(object sender, RoutedEventArgs e)
        {
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
            }
            else
            {
                chkSelectAll.Visibility = System.Windows.Visibility.Collapsed;
                dataGrid2.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            dataGrid2.Columns[1].Visibility = System.Windows.Visibility.Visible;
            //lblPackageName.Text = "Package Name: " + this.PackageName;
        }

        public bool ShowBatches { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }

        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        private ObservableCollection<clsItembatchSearchVO> _ItemBatchList;
        public ObservableCollection<clsItembatchSearchVO> ItemBatchList { get { return _ItemBatchList; } }



        private long _ClinicID;//= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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

        private void Search()
        {
            bool res = true;

            if (cmbStore.SelectedItem == null)
            {
                msgText = "Please select the store";
                cmbStore.TextBox.SetValidation(msgText);
                cmbStore.TextBox.RaiseValidationError();
                res = false;
            }
            else if (((MasterListItem)cmbStore.SelectedItem).ID == 0)
            {
                msgText = "Please select the store";
                cmbStore.TextBox.SetValidation(msgText);
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
                ((ItemSearchViewModel)this.DataContext).IsFromStockAdjustment = IsFromStockAdjustment;   //By Umesh
                if (cmbMolecule.SelectedItem != null)
                    ((ItemSearchViewModel)this.DataContext).BizActionObject.MoleculeName = ((MasterListItem)cmbMolecule.SelectedItem).ID;

                ((ItemSearchViewModel)this.DataContext).ShowZeroStockBatches = ShowZeroStockBatches;   //to show items with > 0 AvailableStock
                ((ItemSearchViewModel)this.DataContext).ShowNonZeroStockBatchesSetFromCounterSale = ShowNonZeroStockBatchesSetFromCounterSale;

                ((ItemSearchViewModel)this.DataContext).ShowNotShowPlusThreeMonthExp = ShowNotShowPlusThreeMonthExp;   //to show items with > 0 AvailableStock
                ((ItemSearchViewModel)this.DataContext).ShowPlusThreeMonthExpSetFromCounterSale = ShowPlusThreeMonthExpSetFromCounterSale;
                ((ItemSearchViewModel)this.DataContext).IsForPackageItemsSearchForCS = true;
                ((ItemSearchViewModel)this.DataContext).PackageID = this.PackageID;
                ((ItemSearchViewModel)this.DataContext).GetData();
                GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                peopleDataPager.PageIndex = 0;
            }
        }

        private void ItemSearchButton_Click(object sender, RoutedEventArgs e)
        {
            Search();
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
                    FillMolecule();
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
                    cmbMolecule.ItemsSource = objList;
                    cmbMolecule.SelectedItem = objList[0];
                    Search();
                }

            };

            client.ProcessAsync(BizAction, loggedinUser);
            client.CloseAsync();
        }

        #endregion

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {

            _ItemSelected = null;
            _BatchSelected = null;

            this.DialogResult = false;

        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
            if (dataGrid2.SelectedItem != null)
            {
                if (((clsItemMasterVO)dataGrid2.SelectedItem).IsItemBlock == true) //&& IsFromCounterSale == true
                {
                }
                else
                {

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
                            ((ItemSearchViewModel)this.DataContext).ShowZeroStockBatches = ShowZeroStockBatches;   //to show items with > 0 AvailableStock
                            ((ItemSearchViewModel)this.DataContext).ShowNonZeroStockBatchesSetFromCounterSale = ShowNonZeroStockBatchesSetFromCounterSale;

                            ((ItemSearchViewModel)this.DataContext).GetBatches();
                        }
                    }
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

            if (ItemBatchList == null)
            {
                isValid = false;
            }
            else if (ItemBatchList.Count <= 0)
            {
                isValid = false;
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

                if (ShowBatches && _BatchSelected != null && _BatchSelected.Count == 0)
                    msgText = "No Batch Selected";
                else
                    msgText = "No Item Selected";
                if (ShowBatches == true && _ItemSelected != null && _ItemSelected.Count > 0 && _ItemSelected[0] != null && _ItemSelected[0].BatchesRequired == false)
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
            if (dgItemBatches.SelectedItem != null)
            {
                if (_BatchSelected == null)
                    _BatchSelected = new ObservableCollection<clsItemStockVO>();

                CheckBox chk = (CheckBox)sender;

                if (chk.IsChecked == true)
                    _BatchSelected.Add((clsItemStockVO)dgItemBatches.SelectedItem);
                else
                    _BatchSelected.Remove((clsItemStockVO)dgItemBatches.SelectedItem);

                if (_ItemBatchList == null)
                    _ItemBatchList = new ObservableCollection<clsItembatchSearchVO>();
                clsItembatchSearchVO objItem = new clsItembatchSearchVO();
                if (dataGrid2.SelectedItem == null)
                {
                    objItem.ItemCode = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemCode;
                    objItem.BrandName = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BrandName;
                    objItem.ItemName = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemName;
                    objItem.BatchesRequired = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BatchesRequired;
                    objItem.Status = false;
                    objItem.SUOM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUOM;
                    objItem.PUOM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PUOM;
                    objItem.SUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BaseUM;
                    objItem.InclusiveOfTax = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].InclusiveOfTax;
                    objItem.Manufacturer = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].Manufacturer;
                    objItem.PreganancyClass = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PreganancyClass;
                    objItem.TotalPerchaseTaxPercent = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].TotalPerchaseTaxPercent;
                    objItem.TotalSalesTaxPercent = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].TotalSalesTaxPercent;
                    objItem.AssignSupplier = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].AssignSupplier;
                    objItem.CategoryId = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemCategory;
                    objItem.GroupId = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemGroup;
                    //By Anjali.................
                    objItem.ItemVatPer = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SItemVatPer;
                    objItem.ItemVatType = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SItemVatType;
                    objItem.ItemVatApplicationOn = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SItemVatApplicationOn;
                    objItem.VATPerc = Convert.ToDouble(((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SVatPer);
                    objItem.ConversionFactor = Convert.ToSingle(((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ConversionFactor);

                    objItem.PUM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PUM;
                    objItem.SUM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BaseUM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BaseUM;
                    objItem.BaseUMString = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BaseUMString;
                    objItem.SellingUM = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SellingUM;
                    objItem.SellingUMString = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SellingUMString;
                    objItem.SellingCF = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SellingCF;
                    objItem.StockingCF = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].StockingCF;
                    objItem.PurchaseToBaseCF = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].PurchaseToBaseCF;
                    objItem.StockingToBaseCF = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].StockingToBaseCF;

                    objItem.Rackname = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].Rackname;
                    objItem.Containername = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].Containername;
                    objItem.Shelfname = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].Shelfname;

                    #region For Quarantine Items (Expired, DOS)

                    // Use For Vat/Tax Calculations
                    objItem.ItemOtherTaxType = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemOtherTaxType;
                    objItem.OtherItemApplicationOn = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].OtherItemApplicationOn;

                    objItem.IssueVatPer = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].VatPer;
                    objItem.IssueItemVatPer = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemVatPer;

                    objItem.IssueItemVatType = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemVatType;
                    objItem.IssueItemVatApplicationOn = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ItemVatApplicationOn;

                    #endregion

                    //.......................
                }
                else
                {
                    objItem.ItemCode = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemCode;
                    objItem.BrandName = ((clsItemMasterVO)(dataGrid2.SelectedItem)).BrandName;
                    objItem.ItemName = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemName;
                    objItem.BatchesRequired = ((clsItemMasterVO)(dataGrid2.SelectedItem)).BatchesRequired;
                    objItem.Status = false;
                    objItem.SUOM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SUOM;
                    objItem.PUOM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PUOM;
                    objItem.SUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BaseUM;
                    objItem.InclusiveOfTax = ((clsItemMasterVO)(dataGrid2.SelectedItem)).InclusiveOfTax;
                    objItem.Manufacturer = ((clsItemMasterVO)(dataGrid2.SelectedItem)).Manufacturer;
                    objItem.PreganancyClass = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PreganancyClass;
                    objItem.TotalPerchaseTaxPercent = ((clsItemMasterVO)(dataGrid2.SelectedItem)).TotalPerchaseTaxPercent;
                    objItem.TotalSalesTaxPercent = ((clsItemMasterVO)(dataGrid2.SelectedItem)).TotalSalesTaxPercent;
                    objItem.AssignSupplier = ((clsItemMasterVO)(dataGrid2.SelectedItem)).AssignSupplier;
                    objItem.CategoryId = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemCategory;
                    objItem.GroupId = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemGroup;
                    //By Anjali.................
                    objItem.ItemVatPer = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SItemVatPer;
                    objItem.ItemVatType = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SItemVatType;
                    objItem.ItemVatApplicationOn = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SItemVatApplicationOn;
                    objItem.VATPerc = Convert.ToDouble(((clsItemMasterVO)(dataGrid2.SelectedItem)).SVatPer);
                    objItem.ConversionFactor = Convert.ToSingle(((clsItemMasterVO)(dataGrid2.SelectedItem)).ConversionFactor);

                    objItem.PUM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PUM;
                    objItem.SUM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SUM;
                    objItem.BaseUM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).BaseUM;
                    objItem.BaseUMString = ((clsItemMasterVO)(dataGrid2.SelectedItem)).BaseUMString;
                    objItem.SellingUM = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SellingUM;
                    objItem.SellingUMString = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SellingUMString;
                    objItem.SellingCF = ((clsItemMasterVO)(dataGrid2.SelectedItem)).SellingCF;
                    objItem.StockingCF = ((clsItemMasterVO)(dataGrid2.SelectedItem)).StockingCF;
                    objItem.PurchaseToBaseCF = ((clsItemMasterVO)(dataGrid2.SelectedItem)).PurchaseToBaseCF;
                    objItem.StockingToBaseCF = ((clsItemMasterVO)(dataGrid2.SelectedItem)).StockingToBaseCF;


                    objItem.Rackname = ((clsItemMasterVO)(dataGrid2.SelectedItem)).Rackname;
                    objItem.Containername = ((clsItemMasterVO)(dataGrid2.SelectedItem)).Containername;
                    objItem.Shelfname = ((clsItemMasterVO)(dataGrid2.SelectedItem)).Shelfname;

                    #region For Quarantine Items (Expired, DOS)

                    // Use For Vat/Tax Calculations
                    objItem.ItemOtherTaxType = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemOtherTaxType;
                    objItem.OtherItemApplicationOn = ((clsItemMasterVO)(dataGrid2.SelectedItem)).OtherItemApplicationOn;

                    objItem.IssueVatPer = ((clsItemMasterVO)(dataGrid2.SelectedItem)).VatPer;
                    objItem.IssueItemVatPer = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemVatPer;

                    objItem.IssueItemVatType = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemVatType;
                    objItem.IssueItemVatApplicationOn = ((clsItemMasterVO)(dataGrid2.SelectedItem)).ItemVatApplicationOn;

                    #endregion

                    //.......................
                }

                if (dgItemBatches.SelectedItem != null)
                {
                    objItem.ID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ID;
                    objItem.ConversionFactor = Convert.ToSingle(((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].ConversionFactor);
                    //      objItem.SUOM = ((clsItemStockVO)(dgItemBatches.SelectedItem)).SUOMID;
                    objItem.SUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].SUM;
                    objItem.BUOMID = ((PagedSortableCollectionView<clsItemMasterVO>)dataGrid2.ItemsSource)[0].BaseUM;
                    objItem.Status = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Status;
                    objItem.StoreID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).StoreID;
                    objItem.ItemID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ItemID;
                    objItem.BatchID = ((clsItemStockVO)(dgItemBatches.SelectedItem)).BatchID;
                    objItem.AvailableStock = ((clsItemStockVO)(dgItemBatches.SelectedItem)).AvailableStock;
                    objItem.CurrentStock = ((clsItemStockVO)(dgItemBatches.SelectedItem)).CurrentStock;
                    objItem.BatchCode = ((clsItemStockVO)(dgItemBatches.SelectedItem)).BatchCode;
                    objItem.ExpiryDate = ((clsItemStockVO)(dgItemBatches.SelectedItem)).ExpiryDate;
                    objItem.MRP = ((clsItemStockVO)(dgItemBatches.SelectedItem)).MRP;
                    objItem.PurchaseRate = ((clsItemStockVO)(dgItemBatches.SelectedItem)).PurchaseRate;

                    objItem.Date = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Date;
                    objItem.VATAmt = ((clsItemStockVO)(dgItemBatches.SelectedItem)).VATAmt;
                    objItem.DiscountOnSale = ((clsItemStockVO)(dgItemBatches.SelectedItem)).DiscountOnSale;
                    objItem.Re_Order = ((clsItemStockVO)(dgItemBatches.SelectedItem)).Re_Order;
                    objItem.AvailableStockInBase = ((clsItemStockVO)(dgItemBatches.SelectedItem)).AvailableStockInBase;
                    objItem.PurchaseToBaseCFForStkAdj = ((clsItemStockVO)(dgItemBatches.SelectedItem)).PurchaseToBaseCF;   //By Umesh
                    objItem.StockingToBaseCFForStkAdj = ((clsItemStockVO)(dgItemBatches.SelectedItem)).StockingToBaseCF;   //By Umesh
                    //.....................................

                }

                var item2 = from r in _ItemBatchList
                            where r.BatchID == ((clsItemStockVO)(dgItemBatches.SelectedItem)).BatchID
                            select r;

                if (item2.ToList().Count > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Item already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();

                    chk.IsChecked = false;
                }

                if (item2.ToList().Count == 0)
                {
                    if (chk.IsChecked == true)
                        _ItemBatchList.Add(objItem);
                    else
                    {
                        foreach (var item in _ItemBatchList)
                        {
                            if (item.ItemID == objItem.ItemID && item.BatchCode == objItem.BatchCode)
                            {
                                _ItemBatchList.Remove(item);
                                break;
                            }
                        }
                    }
                }

                dgSelectedItemList.ItemsSource = null;
                dgSelectedItemList.ItemsSource = ItemBatchList;


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
                    msgText = "Should not enter special characters";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                    msgText = "Should not enter special characters";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
            GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID; ;
        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {
            if (dgSelectedItemList.ItemsSource == null)
                dgSelectedItemList.ItemsSource = this.ItemBatchList;

            if (((CheckBox)sender).IsChecked == true)
            {
                this.ItemBatchList.Add((clsItembatchSearchVO)dgSelectedItemList.SelectedItem);
                foreach (var BatchItems in _BatchSelected.Where(x => x.BatchID == ((clsItembatchSearchVO)dgSelectedItemList.SelectedItem).BatchID))
                {
                    BatchItems.Status = true;
                }
            }
            else
            {
                long BatchID = ((clsItembatchSearchVO)dgSelectedItemList.SelectedItem).BatchID;
                this.ItemBatchList.Remove((clsItembatchSearchVO)dgSelectedItemList.SelectedItem);
                foreach (var BatchItems in _BatchSelected.Where(x => x.BatchID == BatchID))
                {
                    BatchItems.Status = false;
                }
            }
        }

        private void dataGrid2_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (((clsItemMasterVO)e.Row.DataContext).IsItemBlock == true)
            {
                e.Row.IsEnabled = false;
            }
            else
            {
                e.Row.IsEnabled = true;
            }
        }

        private void cmdApplyRule_Click(object sender, RoutedEventArgs e)
        {
            GetPackageItemDiscountNew();
        }

        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
        public ObservableCollection<clsItemSalesDetailsVO> ItemList = new ObservableCollection<clsItemSalesDetailsVO>();
        bool ConcessionFromPlan = false;
        float TotalBudget;
        float TotalCalculatedBudget;
        float Budget;
        float CalculatedBudget;


        private void GetPackageItemDiscountNew()
        {
            clsApplyPackageDiscountRateToItems BizAction = new clsApplyPackageDiscountRateToItems();
            BizAction.objApplyItemPackageDiscountRateDetails = new PalashDynamics.ValueObjects.Billing.clsApplyPackageDiscountRateOnItemVO();
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL1 = this.PatientCatagoryL1;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL2 = this.PatientCatagoryL2;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientCatagoryL3 = this.PatientCatagoryL3;
            BizAction.objApplyItemPackageDiscountRateDetails.CompanyID = this.CompanyID;

            string ItemIDs = "";
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            if (ItemBatchList != null && ItemBatchList.Count > 0)
            {
                foreach (var item in ItemBatchList)   //  PharmacyItems
                {
                    ItemIDs = ItemIDs + item.ItemID;
                    ItemIDs = ItemIDs + ",";

                    clsItemSalesDetailsVO ObjAddItem = new clsItemSalesDetailsVO();
                    ObjAddItem.ItemCode = item.ItemCode;
                    ObjAddItem.ItemID = item.ItemID;
                    ObjAddItem.ItemName = item.ItemName;
                    ObjAddItem.Manufacture = item.Manufacturer;
                    ObjAddItem.PregnancyClass = item.PreganancyClass;
                    ObjAddItem.BatchID = item.BatchID;
                    ObjAddItem.BatchCode = item.BatchCode;
                    ObjAddItem.ExpiryDate = item.ExpiryDate;
                    ObjAddItem.Quantity = 1;
                    ObjAddItem.InclusiveOfTax = item.InclusiveOfTax;
                    ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                    ObjAddItem.AvailableQuantity = item.AvailableStock;
                    ObjAddItem.PurchaseRate = item.PurchaseRate;
                    ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                    ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                    ObjAddItem.Amount = ObjAddItem.Amount;
                    ObjAddItem.VATPercent = item.VATPerc;
                    //vatper = item.VATPerc;
                    ObjAddItem.MRP = item.MRP;
                    ObjAddItem.OriginalMRP = ObjAddItem.MRP;
                    ObjAddItem.ItemVatType = item.ItemVatType;
                    ObjAddItem.AvailableQuantity = item.AvailableStock;
                    ObjAddItem.PurchaseRate = item.PurchaseRate;
                    ObjAddItem.ConcessionPercentage = item.DiscountOnSale;
                    ObjAddItem.ConcessionAmount = ObjAddItem.ConcessionAmount;
                    ObjAddItem.Amount = ObjAddItem.Amount;
                    ObjAddItem.VATPercent = item.TotalSalesTaxPercent;
                    ObjAddItem.NetAmount = ObjAddItem.NetAmount;
                    ObjAddItem.Shelfname = item.Shelfname;
                    ObjAddItem.Containername = item.Containername;
                    ObjAddItem.Rackname = item.Rackname;
                    ObjAddItem.AvailableStockInBase = item.AvailableStockInBase;
                    ObjAddItem.StockUOM = item.SUOM;
                    ObjAddItem.PurchaseUOM = item.PUOM;
                    ObjAddItem.PUOM = item.PUOM;
                    ObjAddItem.MainPUOM = item.PUOM;
                    ObjAddItem.SUOM = item.SUOM;
                    ObjAddItem.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                    ObjAddItem.PUOMID = item.PUM;
                    ObjAddItem.SUOMID = item.SUM;
                    ObjAddItem.BaseUOMID = item.BaseUM;
                    ObjAddItem.BaseUOM = item.BaseUMString;
                    ObjAddItem.SellingUOMID = item.SellingUM;
                    ObjAddItem.SellingUOM = item.SellingUMString;
                    ObjAddItem.MainMRP = Convert.ToSingle(item.MRP);
                    ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                    if (this.IsSellBySellingUnit == true)
                    {
                        ObjAddItem.SelectedUOM = new MasterListItem(ObjAddItem.SellingUOMID, ObjAddItem.SellingUOM);

                        float CalculatedFromCF = item.SellingCF / item.StockingCF;
                        ObjAddItem.ConversionFactor = CalculatedFromCF;
                        ObjAddItem.BaseConversionFactor = item.SellingCF;
                        ObjAddItem.BaseQuantity = 1 * item.SellingCF;
                        ObjAddItem.MainRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                        ObjAddItem.BaseRate = Convert.ToSingle(item.PurchaseRate) / Convert.ToSingle(item.StockingCF);
                        ObjAddItem.PurchaseRate = ObjAddItem.BaseRate * item.SellingCF;
                        ObjAddItem.MainMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                        ObjAddItem.BaseMRP = Convert.ToSingle(item.MRP) / Convert.ToSingle(item.StockingCF);
                        ObjAddItem.MRP = ObjAddItem.BaseMRP * item.SellingCF;
                    }
                    else
                    {
                        ObjAddItem.SelectedUOM = new MasterListItem(0, "--Select--");
                    }
                    PharmacyItems.Add(ObjAddItem);
                }

                if (ItemIDs.EndsWith(","))
                    ItemIDs = ItemIDs.Remove(ItemIDs.Length - 1, 1);
            }

            BizAction.objApplyItemPackageDiscountRateDetails.ItemIDs = ItemIDs;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientGenderID = this.PatientGenderID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientDateOfBirth = this.PatientDateOfBirth;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientID = this.PatientID;
            BizAction.objApplyItemPackageDiscountRateDetails.PatientUnitID = this.PatientUnitID;
            if (this.PackageID > 0)
            {
                BizAction.objApplyItemPackageDiscountRateDetails.PackageID = this.PackageID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null && ((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                {
                    if (((clsApplyPackageDiscountRateToItems)arg.Result).objApplyItemPackageDiscountRate != null)
                    {
                        clsApplyPackageDiscountRateToItems ItemDiscountList = ((clsApplyPackageDiscountRateToItems)arg.Result);
                        ItemList = new ObservableCollection<clsItemSalesDetailsVO>();
                        foreach (var item in PharmacyItems)
                        {
                            ItemList.Add(item.DeepCopy()); //ItemList.Add(item.DeepCopy());
                        }
                        if (ItemDiscountList.objApplyItemPackageDiscountRate.Count > 0)
                        {
                            TotalCalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedTotalBudget;
                            TotalBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].TotalBudget;
                            Budget = ItemDiscountList.objApplyItemPackageDiscountRate[0].Budget;
                            CalculatedBudget = ItemDiscountList.objApplyItemPackageDiscountRate[0].CalculatedBudget;
                        }
                        double PreviousConcession = 0;

                        if (CalculatedBudget < Budget)
                        {
                            foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                            {
                                foreach (var item1 in ItemList)
                                {
                                    if (item1.ItemID == item.ItemId)
                                    {
                                        item1.Budget = item.Budget;
                                        item1.DiscountPerc = item.DiscountedPercentage;
                                        item1.CalculatedBudget = item.CalculatedBudget;
                                        item1.IsPackageForItem = true;
                                        item1.PackageID = this.PackageID;
                                    }
                                }
                            }
                        }


                        if (TotalCalculatedBudget < TotalBudget)
                        {
                            foreach (var item in ItemDiscountList.objApplyItemPackageDiscountRate)
                            {
                                foreach (var item1 in ItemList)
                                {
                                    //if (item.ApplicableToAllDiscount > 0)
                                    //{
                                    //    if (TotalCalculatedBudget < TotalBudget)
                                    //    {
                                    //        item1.PackageID = this.PackageID;
                                    //        item1.ConcessionPercentage = 0;
                                    //        item1.ConcessionAmount = 0;
                                    //        item1.ConcessionPercentage = item.DiscountedPercentage;
                                    //        ConcessionFromPlan = true;
                                    //        TotalCalculatedBudget = TotalCalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    PreviousConcession = 0;

                                    //    if (item.IsCategory == false && item.IsGroup == false)
                                    //    {
                                    //        if (item1.ItemID == item.ItemId)
                                    //        {
                                    //            if (TotalCalculatedBudget < TotalBudget)
                                    //            {
                                    //                item1.PackageID = this.PackageID;
                                    //                item1.Budget = item.Budget;
                                    //                item1.CalculatedBudget = item.CalculatedBudget;

                                    //                PreviousConcession = item1.ConcessionPercentage;

                                    //                item1.ConcessionAmount = 0;
                                    //                item1.ConcessionPercentage = 0;
                                    //                item1.ConcessionPercentage = item.DiscountedPercentage;
                                    //                ConcessionFromPlan = true;

                                    //                //TotalCalculatedBudget = TotalCalculatedBudget + Convert.ToSingle(item1.NetAmount);
                                    //                TotalCalculatedBudget = TotalCalculatedBudget + Convert.ToSingle(item1.ActualNetAmt);

                                    //                if (TotalCalculatedBudget < TotalBudget)
                                    //                {
                                    //                    if (item1.Budget < item1.NetAmount)
                                    //                    {
                                    //                        item1.NetAmtCalculation = item1.Budget;
                                    //                    }
                                    //                    else
                                    //                    {
                                    //                        item1.NetAmtCalculation = item1.NetAmount;
                                    //                    }
                                    //                }
                                    //                else
                                    //                {
                                    //                    item1.ConcessionAmount = 0;
                                    //                    item1.ConcessionPercentage = PreviousConcession;
                                    //                    item1.NetAmtCalculation = 0;
                                    //                }

                                    //                item1.IsPackageForItem = true;
                                    //                item1.IsPackageForCategory = false;
                                    //                item1.IsPackageForGroup = false;
                                    //            }
                                    //            else
                                    //            {
                                    //                //item1.Budget = item.Budget;
                                    //            }
                                    //        }
                                    //        else
                                    //        {
                                    //            item1.IsPackageForItem = false;
                                    //            item1.IsPackageForCategory = false;
                                    //            item1.IsPackageForGroup = false;
                                    //        }

                                    //    }

                                    //}
                                }
                            }
                        }

                        //ItemComparisonWindowForCounterSale win = new ItemComparisonWindowForCounterSale();
                        //win.dataGrid2.ItemsSource = PharmacyItems;
                        //win.dgSelectedItemList.ItemsSource = ItemList;
                        //win.txtBudget.Text = String.Format("{0:0.00}", TotalBudget);
                        //win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
                        //win.OnCancelButton_Click += new RoutedEventHandler(Win_OnCancelButton_Click);
                        //win.Show();
                        txtTotalBudget.Text = TotalBudget.ToString();
                        dgAppliedRuleSelectedItemList.ItemsSource = null;
                        dgAppliedRuleSelectedItemList.ItemsSource = ItemList.ToList();
                    }
                    else
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding visit details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                    }
                }
            };

            Client.ProcessAsync(BizAction, this.objUserVO); //((IApplicationConfiguration)App.Current).CurrentUser
            Client.CloseAsync();
        }
    }
}

