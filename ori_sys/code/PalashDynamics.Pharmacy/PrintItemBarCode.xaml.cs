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
using MessageBoxControl;

namespace PalashDynamics.Pharmacy
{
    public partial class PrintItemBarCode : UserControl
    {
        public PrintItemBarCode()
        {
            InitializeComponent();
            this.DataContext = new ItemSearchViewModel();
            this.Loaded += new RoutedEventHandler(PrintItemBarCode_Loaded);
            ShowBatches = true;
            ShowExpiredBatches = false;
        }

        void PrintItemBarCode_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            if (AllowStoreSelection == false)
            {
                cmbStore.IsEnabled = true;
            }
            FillStores(ClinicID);
            loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //FetchItems();
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
                            FetchItems();
                        }
                        else
                        {
                            cmbStore.SelectedItem = objList[0];
                            GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
                            FetchItems();
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, loggedinUser);
            Client.CloseAsync();
        }
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

        public long StoreID { get; set; }
        public long SupplierID { get; set; }
        //Added by Pallavi for the items that are selected without grn on grnreturn
        public long GRNReturnStoreId { get; set; }
        public bool AllowStoreSelection { get; set; }
        public bool ShowExpiredBatches { get; set; }
        public bool ShowZeroStockBatches { get; set; }
        public bool ShowScrapItems { get; set; }
        public clsUserVO loggedinUser { get; set; }

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {

        }
        public bool ShowBatches { get; set; }

        private ObservableCollection<clsItemMasterVO> _ItemSelected;
        public ObservableCollection<clsItemMasterVO> SelectedItems { get { return _ItemSelected; } }

        private ObservableCollection<clsItemStockVO> _BatchSelected;
        public ObservableCollection<clsItemStockVO> SelectedBatches { get { return _BatchSelected; } }

        private ObservableCollection<clsItembatchSearchVO> _ItemBatchList;
        public ObservableCollection<clsItembatchSearchVO> ItemBatchList { get { return _ItemBatchList; } }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                    ((ItemSearchViewModel)this.DataContext).GetBatches();
                }
            }
        }

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
                FetchItems();
            }
        }

        private void FetchItems()
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

        private void CmdPrintSpermBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedItem != null)
            {
                    BarcodeForm win = new BarcodeForm();
                    string date;
                    long ItemID = 0;
                    string BatchID = null;
                    string BatchCode = null;
                    string ItemCode = null;
                    // ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemID;
                    string ItemName = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemName;

                    if (dgItemBatches.SelectedItem != null)
                    {
                        if (((clsItemStockVO)dgItemBatches.SelectedItem).ExpiryDate != null)
                        {
                            date = ((clsItemStockVO)dgItemBatches.SelectedItem).ExpiryDate.Value.ToShortDateString();
                        }
                        else
                            date = null;
                    }
                    else
                    {
                        date = null;
                    }


                    if (dgItemBatches.SelectedItem != null)
                    {
                        ItemID = ((clsItemStockVO)dgItemBatches.SelectedItem).ItemID;
                        if (((clsItemStockVO)dgItemBatches.SelectedItem).BatchCode != null)
                        {
                            string str = ((clsItemStockVO)dgItemBatches.SelectedItem).BatchCode;
                            BatchID = Convert.ToString(((clsItemStockVO)dgItemBatches.SelectedItem).BatchID);
                            BatchCode = str.Substring(0, 3) +"/"+ BatchID.ToString() + "B";
                           // BatchCode = ((clsItemStockVO)dgItemBatches.SelectedItem).BatchCode+"B";
                           

                        }
                        else
                        {
                            BatchID = ((clsItemStockVO)dgItemBatches.SelectedItem).BatchID + "BI";
                        }
                    }

                    if (BatchCode == null && BatchID == null)
                    {
                        ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ID;
                        string str1 = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemCode;
                        ItemCode = str1.Substring(0,4) + "I";
                    }


                    if (BatchCode != null)
                        win.PrintData = "*" + ItemID.ToString() + "-" + BatchCode + "*";
                    else
                    {
                        if (BatchCode == null && BatchID != null)
                        {
                            win.PrintData = "*" + ItemID.ToString() + "-" + BatchID + "*";
                        }
                        else
                        {
                            win.PrintData = "*" + ItemID.ToString() + "-" + ItemCode + "*";
                        }
                    }
                    win.PrintItem = ItemName;
                    win.PrintDate = date;

                    // win.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);
                    win.Show();
                


            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
        }

        //void BarcodeWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //ClickedFlag = 0;
        //   // FetchItems();
        //}

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
            GRNReturnStoreId = ((MasterListItem)cmbStore.SelectedItem).ID;
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

        private void dgItemBatches_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
