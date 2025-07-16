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

namespace PalashDynamics.Pharmacy
{
    public partial class IndentSearchDuplicate : ChildWindow
    {
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

        #region Constructor
        public IndentSearchDuplicate()
        {
            InitializeComponent();
            //======================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsIndentMasterVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 6;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //======================================================
            this.Loaded += new RoutedEventHandler(IndentSearch_Loaded);
        }
        #endregion Constructor

        #region 'Paging'

        public PagedSortableCollectionView<clsIndentMasterVO> DataList { get; private set; }

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

        #region Events

        void IndentSearch_Loaded(object sender, RoutedEventArgs e)
        {
            FillStore();
            FillIndentList1();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnlyItems != true)
            {
            }
            else
            {
                if (this.ocSelectedItemList.Count() > 0)
                {
                    OnItemSelectionCompleted(this, e);
                    this.DialogResult = true;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                               where r.Status == true
                               select r;

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

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            Boolean isFormValid = true;
            if (cmbFromIndentStore.SelectedItem == null || ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId == 0)
            {
                cmbFromIndentStore.SetValidation("Indent From Store can not be blank.");
                cmbFromIndentStore.RaiseValidationError();
                cmbFromIndentStore.Focus();
                isFormValid = false;
            }
            else if (cmbToIndentStore.SelectedItem == null || ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId == 0)
            {
                cmbToIndentStore.SetValidation("Indent To Store can not be blank.");
                cmbToIndentStore.RaiseValidationError();
                cmbToIndentStore.Focus();
                isFormValid = false;
            }
            else if (dtpFromDate.SelectedDate != null || dtpToDate.SelectedDate != null || ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId != 0 || ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId != 0 || !(String.IsNullOrEmpty(txtIndentNo.Text)))
            {
                isFormValid = true;
            }
            if (isFormValid == true)
            {
                dgIndentList.ItemsSource = null;
                this.ocSelectedItemList.Clear();
                this.ocSelectedItemDetailsList.Clear();
                dgSelectedIndentItemList.ItemsSource = null;
                dgSelectedItemList.ItemsSource = null;
                FillIndentList1();
            }
        }

        private void dgSelectedIndentItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null && String.IsNullOrEmpty("Test"))
            {
                clsGetItemListByIndentIdSrchBizActionVO BizActionObj = new clsGetItemListByIndentIdSrchBizActionVO();

                BizActionObj.IndentId = ((clsIndentMasterVO)dgIndentList.SelectedItem).ID;
                BizActionObj.ItemList = new List<clsItemListByIndentId>();

                BizActionObj.TransactionType = ValueObjects.InventoryTransactionType.Issue;
                BizActionObj.UnitID = ((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID;

                unitID1 = ((clsIndentMasterVO)dgIndentList.SelectedItem).IndentUnitID;
                toStoreID = (long)((clsIndentMasterVO)dgIndentList.SelectedItem).ToStoreID;
                fromStoreID = (long)((clsIndentMasterVO)dgIndentList.SelectedItem).FromStoreID;
                BizActionObj.IssueIndentFlag = IsOpenFromPO;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        int iSelectedItemCount = 0;
                        BizActionObj = ((clsGetItemListByIndentIdSrchBizActionVO)args.Result);
                        dgSelectedIndentItemList.ItemsSource = BizActionObj.ItemList;
                        if (ocSelectedItemList != null && ocSelectedItemList.Count > 0)
                        {
                            for (Int32 iCount = 0; iCount < BizActionObj.ItemList.Count; iCount++)
                            {
                                foreach (clsItemListByIndentId item in ocSelectedItemList)
                                {
                                    if (item.ItemCode == BizActionObj.ItemList[iCount].ItemCode && ((clsIndentMasterVO)(dgIndentList.SelectedItem)).ID.Equals(item.IndentId))
                                    {
                                        BizActionObj.ItemList[iCount].IsChecked = true;
                                        iSelectedItemCount += 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                };

                client.CloseAsync();
            }
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                var Result = from r in ocSelectedItemList
                             where (r.SupplierID != ((clsItemListByIndentId)dgSelectedIndentItemList.SelectedItem).SupplierID)
                             select r;

                if (Result.ToList().Count == 0)
                {
                    this.ocSelectedItemList.Add((clsItemListByIndentId)dgSelectedIndentItemList.SelectedItem);
                    foreach (var item in IndItemList)
                    {
                        if (item.ItemId == ((clsItemListByIndentId)dgSelectedIndentItemList.SelectedItem).ItemId && item.SupplierID == ((clsItemListByIndentId)dgSelectedIndentItemList.SelectedItem).SupplierID)
                        {
                            item.IsChecked = true;
                        }
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item Supplier combination different", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                    msgW1.Show();
                    ((CheckBox)sender).IsChecked = false;
                }
            }
            else
            {
                clsItemListByIndentId objItemByIndentId = (clsItemListByIndentId)dgSelectedIndentItemList.SelectedItem;
                objItemByIndentId = this.ocSelectedItemList.Where(z => z.IndentId == objItemByIndentId.IndentId && z.IndentNumber == objItemByIndentId.IndentNumber && z.ItemId == objItemByIndentId.ItemId).First();
                this.ocSelectedItemList.Remove(objItemByIndentId);
            }
            dgSelectedIndentItemList.UpdateLayout();
            dgSelectedIndentItemList.Focus();
            FillSelectedItemList();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
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

                //List<clsItemListByIndentId> lst = ((List<clsItemListByIndentId>)dgSelectedIndentItemList.ItemsSource).ToList();

                // PagedCollectionView pcv = new PagedCollectionView(dgSelectedIndentItemList.ItemsSource as IEnumerable);

                //List<clsItemListByIndentId> lst = new List<clsItemListByIndentId>(IEnumerable<clsItemListByIndentId>pcv).ToList();

                foreach (var item in IndItemList)
                {
                    if (item.ItemId == objItemByIndentId.ItemId)
                    {
                        item.IsChecked = false;


                    }
                }
                //var l1 = IndItemList.Where(z => z.IndentId == objItemByIndentId.IndentId && z.IndentNumber == objItemByIndentId.IndentNumber && z.ItemId == objItemByIndentId.ItemId).FirstOrDefault();
                //if (l1 != null)
                //{
                    //l1.IsChecked = false;
                    dgSelectedIndentItemList.ItemsSource = null;

                    PagedCollectionView collection = new PagedCollectionView(IndItemList);
                    if (chkSupplier.IsChecked == true)
                    {
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("Supplier"));
                    }
                    else if (chkIndent.IsChecked == true)
                    {
                        collection.GroupDescriptions.Add(new PropertyGroupDescription("IndentNumber"));
                    }
                    dgSelectedIndentItemList.ItemsSource = collection;
                //}
            }
        }

        List<clsItemListByIndentId> IndItemList = new List<clsItemListByIndentId>();
        private void dgIndentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgIndentList.SelectedItem != null)
            {
                clsGetItemListByIndentIdSrchBizActionVO BizActionObj = new clsGetItemListByIndentIdSrchBizActionVO();

                BizActionObj.IndentId = ((clsIndentMasterVO)dgIndentList.SelectedItem).ID;
                BizActionObj.ItemList = new List<clsItemListByIndentId>();

                BizActionObj.UnitID = ((clsIndentMasterVO)dgIndentList.SelectedItem).UnitID;
                BizActionObj.TransactionType = ValueObjects.InventoryTransactionType.Issue;
                toStoreID = (long)((clsIndentMasterVO)dgIndentList.SelectedItem).ToStoreID;
                fromStoreID = (long)((clsIndentMasterVO)dgIndentList.SelectedItem).FromStoreID;
                BizActionObj.IssueIndentFlag = IsOpenFromPO;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        int iSelectedItemCount = 0;
                        BizActionObj = ((clsGetItemListByIndentIdSrchBizActionVO)args.Result);
                        IndItemList = new List<clsItemListByIndentId>();
                        IndItemList = BizActionObj.ItemList.DeepCopy();
                        PagedCollectionView collection = new PagedCollectionView(BizActionObj.ItemList);
                        if (chkSupplier.IsChecked == true)
                        {
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("Supplier"));
                        }
                        else if (chkIndent.IsChecked == true)
                        {
                            collection.GroupDescriptions.Add(new PropertyGroupDescription("IndentNumber"));
                        }

                        //dgSelectedIndentItemList.ItemsSource = BizActionObj.ItemList;
                        dgSelectedIndentItemList.ItemsSource = collection;
                        if (ocSelectedItemList != null && ocSelectedItemList.Count > 0)
                        {
                            for (Int32 iCount = 0; iCount < BizActionObj.ItemList.Count; iCount++)
                            {
                                foreach (clsItemListByIndentId item in ocSelectedItemList)
                                {
                                    if (item.ItemCode == BizActionObj.ItemList[iCount].ItemCode && item.IndentNumber.Trim().Equals(BizActionObj.ItemList[iCount].IndentNumber.Trim()))
                                    {
                                        BizActionObj.ItemList[iCount].IsChecked = true;
                                        iSelectedItemCount += 1;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                };

                client.CloseAsync();
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion Events

        #region Methods

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>

        private void FillIndentList1()
        {
            try
            {

                //False when we want to fetch all items
                clsIndentMasterVO obj = new clsIndentMasterVO();
                indicator.Show();
                clsGetIndenListByStoreIdBizActionVO BizActionObj = new clsGetIndenListByStoreIdBizActionVO();
                BizActionObj.IndentList = new List<clsIndentMasterVO>();
                if (!String.IsNullOrEmpty(txtIndentNo.Text))
                    BizActionObj.IndentNumber = txtIndentNo.Text;
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

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        if (args.Result != null)
                        {
                            clsGetIndenListByStoreIdBizActionVO result = args.Result as clsGetIndenListByStoreIdBizActionVO;
                            BizActionObj.IndentList = ((clsGetIndenListByStoreIdBizActionVO)args.Result).IndentList;
                            DataList.TotalItemCount = result.TotalRowCount;
                            if (result.IndentList != null)
                            {
                                DataList.Clear();
                                foreach (var item in BizActionObj.IndentList)
                                {
                                    DataList.Add(item);
                                }
                                dgIndentList.ItemsSource = null;
                                dgIndentList.ItemsSource = DataList;
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding Opening Balance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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

        private void FillSelectedItemList()
        {
            PagedCollectionView pcvItemsByIndentId = new PagedCollectionView(ocSelectedItemList);
            pcvItemsByIndentId.GroupDescriptions.Add(new PropertyGroupDescription("IndentNumber"));
            dgSelectedItemList.ItemsSource = pcvItemsByIndentId;
            dgSelectedItemList.UpdateLayout();
        }
        #endregion Methods

        private void dgIndentItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }




    }
}

