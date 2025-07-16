using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using System;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Data;
using System.Collections;
using System.Windows.Input;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.Pharmacy
{
    public partial class SupplierIndentSearch : ChildWindow
    {
        public SupplierIndentSearch()
        {
            InitializeComponent();

            DataList = new PagedSortableCollectionView<clsItemListByIndentId>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 100;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
            this.Loaded += new RoutedEventHandler(IndentSearch_Loaded);
        }
        #region Variables
        public long? unitID = 0;
        private ObservableCollection<clsItemListByIndentId> _ocSelectedItemList = new ObservableCollection<clsItemListByIndentId>();
        private ObservableCollection<clsIssueItemDetailsVO> _ocSelectedItemDetailsList = new ObservableCollection<clsIssueItemDetailsVO>();
        public long? unitID1 = 0;
        long toStoreID = 0;
        long fromStoreID = 0;
        long supplierId = 0;

        #endregion Variables

        #region Properties
        public delegate void ItemSelection(object sender, EventArgs e);
        public event ItemSelection OnItemSelectionCompleted;

        public long? IndentFromStoreId { get; set; }
        public long? IndentToStoreId { get; set; }
        public clsIndentMasterVO SelectedIndent { get; set; }
        public Boolean? IsOnlyItems { get; set; }
        public ObservableCollection<clsItemListByIndentId> ocSelectedItemList
        {
            get
            {
                return _ocSelectedItemList;
            }
            set
            {
                _ocSelectedItemList = value;
            }
        }

        public Boolean IsOpenFromPO { get; set; }
        public ObservableCollection<clsIssueItemDetailsVO> ocSelectedItemDetailsList
        {
            get
            {
                return _ocSelectedItemDetailsList;
            }
            set
            {
                _ocSelectedItemDetailsList = value;
            }
        }
        public long SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }

        public long ToStoreID
        {
            get { return toStoreID; }
            set { toStoreID = value; }
        }

        public long FromStoreID
        {
            get { return fromStoreID; }
            set { fromStoreID = value; }
        }
        #endregion Properties

        #region 'Paging'

        public PagedSortableCollectionView<clsItemListByIndentId> DataList { get; private set; }

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
        WaitIndicator indicator = new WaitIndicator();

        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillIndentList1();
        }



        #endregion

        void IndentSearch_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date.Date;
            FillStore();
            FillSupplier();
            FillIndentList1();
        }

        private void FillStore()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true, ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId };
                    BizActionObj.ItemMatserDetails.Insert(0, Default);

                    var res1 = from r in BizActionObj.ItemMatserDetails
                               where r.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && r.Status == true
                               select r;
                    //Added By Yk ClinicID having Unit ID

                    cmbFromIndentStore.ItemsSource = res1.ToList();
                    if (IndentFromStoreId != null)
                    {
                        var res = from r in res1.ToList()
                                  where r.StoreId == IndentFromStoreId && r.Status == true
                                  select r;
                        cmbFromIndentStore.SelectedItem = ((clsStoreVO)res.First());
                    }
                    else
                    {
                        cmbFromIndentStore.SelectedItem = res1.ToList()[0];
                    }


                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    cmbToIndentStore.ItemsSource = (List<clsStoreVO>)result.ToList();

                    if (IndentToStoreId != null)
                    {
                        var res = from r in (List<clsStoreVO>)result.ToList()
                                  where r.StoreId == IndentToStoreId
                                  select r;

                        // cmbToIndentStore.SelectedItem = ((clsStoreVO)res.First());
                    }
                    else
                    {
                        //cmbToIndentStore.SelectedItem = result.ToList()[0];
                    }
                }
            };

            client.CloseAsync();
        }

        private void FillSupplier()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.IsGST = true;//Added By Bhushan For GST Get StateID 24062017
                BizAction.IsFromPOGRN = true;
                BizAction.MasterTable = MasterTableNameList.M_Supplier;
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
                        foreach (var item in objList)
                        {
                            item.Description = item.Code + " - " + item.Description;
                        }
                        cmbSupplier.ItemsSource = null;
                        cmbSupplier.ItemsSource = objList;
                        cmbSupplier.SelectedItem = objList[0];
                        if (this.SupplierId > 0)
                        {
                            cmbSupplier.SelectedValue = this.SupplierId;
                            cmbSupplier.IsEnabled = false;
                        }
                        else
                            cmbSupplier.IsEnabled = true;
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

        bool Validation()
        {
            bool reasult = true;
            if (cmbSupplier.SelectedItem == null)
            {
                cmbSupplier.TextBox.SetValidation("Please Select Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                reasult = false;
            }
            else if ((cmbSupplier.SelectedItem as MasterListItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Please Select Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                reasult = false;
            }
            else
                cmbSupplier.TextBox.ClearValidationError();

            return reasult;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                if (IsOnlyItems != true)
                {
                }
                else
                {
                    if (this.ocSelectedItemList.Count() > 0)
                    {
                        SupplierId = (cmbSupplier.SelectedItem as MasterListItem).ID;
                        OnItemSelectionCompleted(this, e);
                        this.DialogResult = true;
                    }
                }
            }
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                this.ocSelectedItemList.Add((clsItemListByIndentId)dgIndentList.SelectedItem);
            }
            else
            {
                clsItemListByIndentId objItemByIndentId = (clsItemListByIndentId)dgIndentList.SelectedItem;
                objItemByIndentId = this.ocSelectedItemList.Where(z => z.IndentId == objItemByIndentId.IndentId && z.IndentNumber == objItemByIndentId.IndentNumber && z.ItemId == objItemByIndentId.ItemId).First();
                this.ocSelectedItemList.Remove(objItemByIndentId);
            }

            dgIndentList.UpdateLayout();
            dgIndentList.Focus();
            FillSelectedItemList();

            #region Commented on 09042018
            //if (((clsItemListByIndentId)dgIndentList.SelectedItem).SupplierID > 0)
            //{
            //    if (((CheckBox)sender).IsChecked == true)
            //    {
            //        var Result = from r in ocSelectedItemList
            //                     where (r.SupplierID != ((clsItemListByIndentId)dgIndentList.SelectedItem).SupplierID)
            //                     select r;

            //        if (Result.ToList().Count == 0)
            //        {
            //            this.ocSelectedItemList.Add((clsItemListByIndentId)dgIndentList.SelectedItem);

            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Supplier combination different", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //            msgW1.Show();
            //            ((CheckBox)sender).IsChecked = false;
            //        }
            //    }
            //    else
            //    {
            //        clsItemListByIndentId objItemByIndentId = (clsItemListByIndentId)dgIndentList.SelectedItem;
            //        objItemByIndentId = this.ocSelectedItemList.Where(z => z.IndentId == objItemByIndentId.IndentId && z.IndentNumber == objItemByIndentId.IndentNumber && z.ItemId == objItemByIndentId.ItemId).First();
            //        this.ocSelectedItemList.Remove(objItemByIndentId);
            //    }

            //    dgIndentList.UpdateLayout();
            //    dgIndentList.Focus();
            //    FillSelectedItemList();
            //}
            //else
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please assign Supplier to '" + ((clsItemListByIndentId)dgIndentList.SelectedItem).ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
            //    msgW1.Show();
            //    ((CheckBox)sender).IsChecked = false;
            //}
            #endregion

        }

        private void chkSelectedItem_UnCheck(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == false)
            {
                clsItemListByIndentId objItemByIndentId = (clsItemListByIndentId)dgSelectedItemList.SelectedItem;
                ObservableCollection<clsItemListByIndentId> ocItemsByIndentId = new ObservableCollection<clsItemListByIndentId>();
                ocItemsByIndentId = ocSelectedItemList;
                objItemByIndentId = this.ocSelectedItemList.Where(z => z.IndentId == objItemByIndentId.IndentId && z.IndentNumber == objItemByIndentId.IndentNumber && z.ItemId == objItemByIndentId.ItemId).First();
                ocSelectedItemList.Remove(objItemByIndentId);

                foreach (var item in DataList)
                {
                    if (item.ItemId == objItemByIndentId.ItemId)
                    {
                        item.IsChecked = false;


                    }
                }

                dgIndentList.ItemsSource = null;

                PagedCollectionView collection = new PagedCollectionView(DataList);
                if (chkSupplier.IsChecked == true)
                {
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Supplier"));
                }
                else if (chkIndent.IsChecked == true)
                {
                    collection.GroupDescriptions.Add(new PropertyGroupDescription("PRNoWithStoreName"));
                }
                dgIndentList.ItemsSource = collection;

            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Boolean isFormValid = true;
            //if (cmbFromIndentStore.SelectedItem == null || ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId == 0)
            //{
            //    cmbFromIndentStore.SetValidation("Indent From Store can not be blank.");
            //    cmbFromIndentStore.RaiseValidationError();
            //    cmbFromIndentStore.Focus();
            //    isFormValid = false;
            //}

            if (cmbToIndentStore.SelectedItem == null || ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId == 0)
            {
                cmbToIndentStore.SetValidation("To Store can not be blank.");
                cmbToIndentStore.RaiseValidationError();
                cmbToIndentStore.Focus();
                isFormValid = false;
            }
            if (dtpFromDate.SelectedDate != null || dtpToDate.SelectedDate != null || (cmbToIndentStore.SelectedItem != null && ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId != 0) || (cmbFromIndentStore.SelectedItem != null && ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId != 0) || !(String.IsNullOrEmpty(txtIndentNo.Text)))
            {
                isFormValid = true;
            }

            if (isFormValid == true)
            {
                //* Commented by - Ajit Jadhav
                //* Commented Date - 10/9/2016
                //* Comments - Not Clear Checked List

                // dgIndentList.ItemsSource = null;
                // this.ocSelectedItemList.Clear();
                // this.ocSelectedItemDetailsList.Clear();  

                dgIndentList.ItemsSource = null;
                FillIndentList1();
            }

        }

        private void FillIndentList1()
        {
            try
            {

                clsIndentMasterVO obj = new clsIndentMasterVO();
                indicator.Show();
                clsGetIndentListBySupplierBizActionVO BizActionObj = new clsGetIndentListBySupplierBizActionVO();
                BizActionObj.ItemList = new List<clsItemListByIndentId>();
                if (!String.IsNullOrEmpty(txtIndentNo.Text))
                    BizActionObj.SIndentNumber = txtIndentNo.Text;
                BizActionObj.FromIndentStoreId = ((clsStoreVO)cmbFromIndentStore.SelectedItem) == null ? 0 : ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId;
                BizActionObj.ToIndentStoreId = ((clsStoreVO)cmbToIndentStore.SelectedItem) == null ? 0 : ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId;
                BizActionObj.LoginUserUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObj.StartRowIndex = DataList.PageIndex * DataList.PageSize;
                BizActionObj.MaximumRows = DataList.PageSize;
                BizActionObj.Freezed = true;
                BizActionObj.PagingEnabled = true;
                if (dtpFromDate.SelectedDate != null)
                    BizActionObj.FromDate = dtpFromDate.SelectedDate.Value;

                if (dtpToDate.SelectedDate != null)
                    BizActionObj.ToDate = dtpToDate.SelectedDate.Value;

                BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionObj.FromPO = true;

                BizActionObj.SItemName = txtItemName.Text;

                //if (cmbSupplier.SelectedItem != null)
                //    BizActionObj.SSupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        if (args.Result != null)
                        {
                            clsGetIndentListBySupplierBizActionVO result = args.Result as clsGetIndentListBySupplierBizActionVO;
                            BizActionObj.ItemList = ((clsGetIndentListBySupplierBizActionVO)args.Result).ItemList;
                            DataList.TotalItemCount = result.TotalRowCount;
                            if (result.ItemList != null)
                            {
                                DataList.Clear();
                                foreach (var item in BizActionObj.ItemList)
                                {
                                    item.FinalPendindQty = Convert.ToSingle(item.BalanceQty) - item.POPendingItemQty; //(item.BaseQuantity - item.POApprItemQty - item.POPendingItemQty);  // only for Get PR Item Window
                                    item.PurchaseRequisitionNumber = item.IndentNumber;
                                    //* Added by - Ajit Jadhav
                                    //* Added Date - 10/9/2016
                                    //* Comments - check the IsChecked check box
                                    foreach (clsItemListByIndentId selecteditem in ocSelectedItemList)
                                    {
                                        if (item.ItemId == selecteditem.ItemId && item.PurchaseRequisitionNumber == selecteditem.PurchaseRequisitionNumber && item.SupplierID == selecteditem.SupplierID)
                                        {
                                            item.IsChecked = true;
                                            break;
                                        }

                                    }//***//-------------

                                    item.PRNoWithStoreName = item.PurchaseRequisitionNumber + "-" + (cmbFromIndentStore.ItemsSource == null ? "" : (cmbFromIndentStore.ItemsSource as List<clsStoreVO>).Where(z => z.StoreId == item.FromStoreID).FirstOrDefault().StoreName);
                                    //item.PRNoWithStoreName = 
                                    DataList.Add(item);
                                }



                                PagedCollectionView collection = new PagedCollectionView(DataList);
                                if (chkSupplier.IsChecked == true)
                                {
                                    collection.GroupDescriptions.Add(new PropertyGroupDescription("Supplier"));
                                }
                                else if (chkIndent.IsChecked == true)
                                {
                                    collection.GroupDescriptions.Add(new PropertyGroupDescription("PRNoWithStoreName"));
                                }
                                else if (chkItem.IsChecked == true)
                                {
                                    collection.GroupDescriptions.Add(new PropertyGroupDescription("ItemName"));
                                }

                                //foreach (var item1 in collection)
                                //{
                                //    if()
                                //}
                                dgIndentList.ItemsSource = null;
                                dgIndentList.ItemsSource = collection;

                                dgDataPager.Source = DataList;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }

                    indicator.Close();
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void dgIndentList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //if (dgIndentList.ItemsSource != null)
            //{
            //    if (((clsItemListByIndentId)e.Row.DataContext).IsItemBlock)
            //        e.Row.IsEnabled = false;
            //    else
            //        e.Row.IsEnabled = true;
            //}
        }

        private void FillSelectedItemList()
        {
            PagedCollectionView pcvItemsByIndentId = new PagedCollectionView(ocSelectedItemList);
            pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("PRNoWithStoreName"));
            dgSelectedItemList.ItemsSource = pcvItemsByIndentId;
            dgSelectedItemList.UpdateLayout();
        }

        private void chkIndent_Click(object sender, RoutedEventArgs e)
        {

            PagedCollectionView pcvItemsByIndentId = new PagedCollectionView(DataList);
            if (chkSupplier.IsChecked == true)
            {
                pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("Supplier"));
            }
            else if (chkIndent.IsChecked == true)
            {
                pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("PRNoWithStoreName"));
            }
            else if (chkItem.IsChecked == true)
            {
                pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("ItemName"));
            }

            dgIndentList.ItemsSource = pcvItemsByIndentId;
            dgIndentList.UpdateLayout();

        }

        private void txtIndentNo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FillIndentList1();
            }
            else if (e.Key == Key.Tab)
            {
                //FillIndentList1();
            }

        }

        private void txtItemName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtItemName.Text = txtItemName.Text.ToTitleCase();
        }

        private void cboSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbSupplier.SelectedItem != null)
            //    FillIndentList1();
        }

        private void PART_MaximizeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            PurchaseGrid.Visibility = Visibility.Visible;
        }
        private void PART_MaximizeToggle_Checked(object sender, RoutedEventArgs e)
        {
            PurchaseGrid.Visibility = Visibility.Collapsed;
        }



    }
}

