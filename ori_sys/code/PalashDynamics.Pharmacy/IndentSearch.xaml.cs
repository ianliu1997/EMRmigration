
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
using System.Linq;
using CIMS;

using System;
using System.Globalization;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using Groupbox;

namespace PalashDynamics.Pharmacy
{
    public partial class IndentSearch : ChildWindow
    {
        public delegate void ItemSelection(object sender, EventArgs e);
        public event ItemSelection OnItemSelectionCompleted;

        public long? IndentFromStoreId { get; set; }
        public long? IndentToStoreId { get; set; }
        public clsIndentMasterVO SelectedIndent { get; set; }
        public bool IsOpenFromPO { get; set; }

        public IndentSearch()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IndentSearch_Loaded);
        }

        void IndentSearch_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsOnlyItems == true)
            {
                gbIndentItem.Height = 500;
                gbIndentItem.Width = 860;
                dgIndentItemList.Columns[0].Visibility = System.Windows.Visibility.Visible;
                chkSelectAll.Visibility = System.Windows.Visibility.Visible;
                gbItemBatch.Visibility = System.Windows.Visibility.Collapsed;
                gbSelectedItem.Visibility = System.Windows.Visibility.Collapsed;

            }
            else
            {
                //gbIndentItem.SetValue(GroupBox.HeightProperty, 200);
                //gbIndentItem.SetValue(GroupBox.WidthProperty, 430);
                dgIndentItemList.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
                chkSelectAll.Visibility = System.Windows.Visibility.Collapsed;
                gbItemBatch.Visibility = System.Windows.Visibility.Visible;
                gbSelectedItem.Visibility = System.Windows.Visibility.Visible;
            }
            txtIndentNote.Text = IsOpenFromPO == false ? string.Empty : "Note : Issue will be against single Purchase Requisition only";
            FillStore();
        }

        public Boolean? IsOnlyItems { get; set; }
        //By Anjali...........................
        // public Boolean IsIndent = false;
        public int IsIndent = 0;
        //...............................

        /// <summary>
        /// Ok button click
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsOnlyItems != true)
            {
                if (this.ocSelectedItemDetailsList.Count() > 0)
                {
                    if (SelectedIndent == null)
                        SelectedIndent = new clsIndentMasterVO();

                    this.SelectedIndent = ((clsIndentMasterVO)cmbIndentList.SelectedItem);

                    OnItemSelectionCompleted(this, e);
                    this.DialogResult = true;
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Item Batch", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            else
            {
                if (this.ocSelectedItemList.Count() > 0)
                {
                    OnItemSelectionCompleted(this, e);
                    this.DialogResult = true;
                }
                else
                {
                    //please select Item
                }
            }

        }


        /// <summary>
        /// Cancel button click
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public long? unitID = 0;

        /// <summary>
        /// Fetches indent list
        /// </summary>
        private void BindIndentList()
        {
            clsGetIndenListByStoreIdBizActionVO BizActionObj = new clsGetIndenListByStoreIdBizActionVO();
            //False when we want to fetch all items
            clsIndentMasterVO obj = new clsIndentMasterVO();

            BizActionObj.IndentList = new List<clsIndentMasterVO>();
            BizActionObj.FromIndentStoreId = ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId;
            BizActionObj.ToIndentStoreId = ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId;
            BizActionObj.LoginUserUnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
            //    BizActionObj.UnitID = 0;
            //else
            //    BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizActionObj.UnitID = 0;
            BizActionObj.IsIndent = IsIndent;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.IndentList = ((clsGetIndenListByStoreIdBizActionVO)args.Result).IndentList;

                    if (BizActionObj.IndentList.Count == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash!", "Purchase Requisition Not Found!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                    }
                    //  dgIndentList.ItemsSource = BizActionObj.IndentList;

                    cmbIndentList.ItemsSource = BizActionObj.IndentList;

                    var result = from item in BizActionObj.IndentList
                                 where item.ToStoreID == BizActionObj.ToIndentStoreId
                                 select item;

                    cmbIndentList.ItemsSource = result;


                    // A custom search, the same that is used in the basic lambda file
                    cmbIndentList.FilterMode = AutoCompleteFilterMode.Custom;
                    cmbIndentList.ItemFilter = (search, item) =>
                    {
                        if (string.IsNullOrEmpty(search))
                        {
                            return true;
                        }
                        clsIndentMasterVO IndentMaster = item as clsIndentMasterVO;
                        if (IndentMaster == null)
                        {
                            return false;
                        }
                        // Interested in: Name, City, FAA code
                        string filter = search.ToUpper(CultureInfo.InvariantCulture);
                        return (IndentMaster.IndentNumber.ToUpper(CultureInfo.InvariantCulture).Contains(filter)

                          );
                    };

                }

            };

            client.CloseAsync();
        }

        /// <summary>
        /// Fills stores
        /// </summary>
        private void FillStore()
        {

            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", Status = true, ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId };
                    BizActionObj.ItemMatserDetails.Insert(0, Default);
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var NonQSAndUserAssigned = from item in BizActionObj.ToStoreList.ToList()
                                               where item.IsQuarantineStore == false
                                               select item;
                    NonQSAndUserAssigned.ToList().Insert(0, Default);

                    cmbFromIndentStore.ItemsSource = NonQSAndUserAssigned.ToList();//BizActionObj.ItemMatserDetails;
                    //cmbToIndentStore.ItemsSource = BizActionObj.ItemMatserDetails;  // commented by Ashish for getting all store in ToStore.

                    if (IndentFromStoreId != null)
                    {
                        var res = from r in NonQSAndUserAssigned.ToList() //BizActionObj.ItemMatserDetails
                                  where r.StoreId == IndentFromStoreId
                                  select r;
                        cmbFromIndentStore.SelectedItem = ((clsStoreVO)res.First());
                        //cmbToIndentStore.SelectedItem = ((clsStoreVO)res.First());  // commented by Ashish for getting all store in ToStore.
                    }
                    else
                    {
                        cmbFromIndentStore.SelectedItem = NonQSAndUserAssigned.ToList()[0]; //BizActionObj.ItemMatserDetails[0];
                        //cmbToIndentStore.SelectedItem = BizActionObj.ItemMatserDetails[0];  // commented by Ashish for getting all store in ToStore.
                    }

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.IsQuarantineStore == false
                                 select item;


                    cmbToIndentStore.ItemsSource = (List<clsStoreVO>)result.ToList();

                    if (IndentToStoreId != null)
                    {
                        var res = from r in (List<clsStoreVO>)result.ToList()
                                  where r.StoreId == IndentToStoreId
                                  select r;

                        cmbToIndentStore.SelectedItem = ((clsStoreVO)res.First());
                    }
                    else
                    {
                        cmbToIndentStore.SelectedItem = BizActionObj.ItemMatserDetails[0];
                    }
                }

            };

            client.CloseAsync();

        }

        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Search button click
        /// </summary>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            cmbIndentList.ItemsSource = null;
            dgIndentItemList.ItemsSource = null;
            this.ocBatchList.Clear();
            this.ocSelectedItemDetailsList.Clear();

            Boolean isFormValid = true;
            if (cmbFromIndentStore.SelectedItem == null || ((clsStoreVO)cmbFromIndentStore.SelectedItem).StoreId == 0)
            {
                cmbFromIndentStore.TextBox.SetValidation("From Store can not be blank.");
                cmbFromIndentStore.TextBox.RaiseValidationError();
                cmbFromIndentStore.TextBox.Focus();
                isFormValid = false;
            }
            else
                cmbFromIndentStore.TextBox.ClearValidationError();

            if (cmbToIndentStore.SelectedItem == null || ((clsStoreVO)cmbToIndentStore.SelectedItem).StoreId == 0)
            {
                cmbToIndentStore.TextBox.SetValidation("To Store can not be blank.");
                cmbToIndentStore.TextBox.RaiseValidationError();
                cmbToIndentStore.Focus();
                isFormValid = false;
            }
            else
                cmbToIndentStore.TextBox.ClearValidationError();

            if (isFormValid == true)
            {
                BindIndentList();
            }

        }

        private void DropDownToggle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cmbIndentList.SearchText))
            {
                cmbIndentList.Text = string.Empty;
            }
            cmbIndentList.IsDropDownOpen = !cmbIndentList.IsDropDownOpen;
        }


        private ObservableCollection<clsItemListByIndentId> _ocSelectedItemList = new ObservableCollection<clsItemListByIndentId>();
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

        public long? unitID1 = 0;
        long toStoreID = 0;
        long fromStoreID = 0;

        /// <summary>
        /// Fills indent items for indentlist selection changed
        /// </summary>
        private void cmbIndentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgIndentItemList.ItemsSource = null;
            dgBatchList.ItemsSource = null;
            this.ocIssueItemDetailsList.Clear();
            this.ocBatchList.Clear();
            this.ocSelectedItemDetailsList.Clear();
            chkSelectAll.IsChecked = false;

            if (cmbIndentList.SelectedItem != null)
            {
                clsGetItemListByIndentIdSrchBizActionVO BizActionObj = new clsGetItemListByIndentIdSrchBizActionVO();

                BizActionObj.IndentId = ((clsIndentMasterVO)cmbIndentList.SelectedItem).ID;
                BizActionObj.ItemList = new List<clsItemListByIndentId>();

                BizActionObj.TransactionType = ValueObjects.InventoryTransactionType.Issue;
                BizActionObj.UnitID = ((clsIndentMasterVO)cmbIndentList.SelectedItem).UnitID;

                unitID1 = ((clsIndentMasterVO)cmbIndentList.SelectedItem).IndentUnitID;
                toStoreID = (long)((clsIndentMasterVO)cmbIndentList.SelectedItem).ToStoreID;
                fromStoreID = (long)((clsIndentMasterVO)cmbIndentList.SelectedItem).FromStoreID;
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
                        dgIndentItemList.ItemsSource = BizActionObj.ItemList;
                        if (ocSelectedItemList != null && ocSelectedItemList.Count > 0)
                        {
                            for (Int32 iCount = 0; iCount < BizActionObj.ItemList.Count; iCount++)
                            {
                                foreach (clsItemListByIndentId item in ocSelectedItemList)
                                {
                                    if (item.ItemCode == BizActionObj.ItemList[iCount].ItemCode && cmbIndentList.Text.Trim().Contains(item.IndentNumber.Trim()))
                                    {
                                        BizActionObj.ItemList[iCount].IsChecked = true;
                                        iSelectedItemCount += 1;
                                        break;
                                    }
                                }
                            }
                        }
                        if (BizActionObj.ItemList.Count == iSelectedItemCount)
                        {
                            chkSelectAll.IsChecked = true;
                        }
                    }

                };

                client.CloseAsync();

                //BindItemListByIndentId();
            }
        }

        private List<clsIssueItemDetailsVO> _ocIssueItemDetailsList = new List<clsIssueItemDetailsVO>();
        public List<clsIssueItemDetailsVO> ocIssueItemDetailsList
        {
            get
            {
                return _ocIssueItemDetailsList;
            }
            set
            {
                _ocIssueItemDetailsList = value;
            }
        }
        private ObservableCollection<clsIssueItemDetailsVO> _ocBatchList = new ObservableCollection<clsIssueItemDetailsVO>();
        public ObservableCollection<clsIssueItemDetailsVO> ocBatchList
        {
            get
            {
                return _ocBatchList;
            }
            set
            {
                _ocBatchList = value;
            }
        }

        private ObservableCollection<clsIssueItemDetailsVO> _ocSelectedItemDetailsList = new ObservableCollection<clsIssueItemDetailsVO>();
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

        /// <summary>
        /// Fills items by indent id
        /// </summary>
        private void BindItemListByIndentId(long? itemid)
        {
            // dgBatchList.ItemsSource = null;
            try
            {
                if (cmbIndentList.SelectedItem != null)
                {

                    GetItemListByIndentIdForIssueItemBizActionVO BizActionObj = new GetItemListByIndentIdForIssueItemBizActionVO();
                    BizActionObj.IndentID = ((clsIndentMasterVO)cmbIndentList.SelectedItem).ID;
                    BizActionObj.IssueFromStoreId = ((clsIndentMasterVO)cmbIndentList.SelectedItem).ToStoreID;
                    BizActionObj.IssueToStoreId = ((clsIndentMasterVO)cmbIndentList.SelectedItem).FromStoreID;
                    BizActionObj.ItemID = itemid;
                    BizActionObj.IssueItemDetailsList = new List<clsIssueItemDetailsVO>();
                    BizActionObj.TransactionType = ValueObjects.InventoryTransactionType.Issue;
                    //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    //    BizActionObj.UnitID = 1;
                    //else
                    BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    //  BizActionObj.UnitID = ((clsIndentMasterVO)cmbIndentList.SelectedItem).UnitID;
                    unitID = BizActionObj.IndentUnitID;
                    //BizActionObj.IndentUnitID = ((clsIndentMasterVO)cmbIndentList.SelectedItem).IndentUnitID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            BizActionObj.IssueItemDetailsList = ((GetItemListByIndentIdForIssueItemBizActionVO)args.Result).IssueItemDetailsList;

                            //  this.ocIssueItemDetailsList = new List<clsIssueItemDetailsVO>();
                            foreach (clsIssueItemDetailsVO item in BizActionObj.IssueItemDetailsList)
                            {
                                //item.AvailableStock=Convert.ToDecimal(Math.Floor(Convert.ToDouble(item.AvailableStock / Convert.ToDecimal(item.ConversionFactor))));
                                this.ocIssueItemDetailsList.Add(item);
                            }

                            clsItemListByIndentId objItem = new clsItemListByIndentId();
                            objItem = ((clsItemListByIndentId)dgIndentItemList.SelectedItem);
                            foreach (clsIssueItemDetailsVO item in ocIssueItemDetailsList)
                            {
                                item.IssuePendingQuantity = objItem.IssuePendingQuantity;
                                item.BalanceQty = objItem.BalanceQty;
                                item.IndentQty = objItem.IndentQty;
                                item.IssueQty = Convert.ToDecimal(objItem.IssueQty);
                                item.IndentUnitID = objItem.IndentUnitID;
                                item.IndentDetailsID = objItem.IndentDetailsID;
                                item.IndentID = (long)objItem.IndentId;
                                item.ToStoreID = objItem.ToStoreID;
                                item.FromStoreID = objItem.FromStoreID;
                                item.IsIndent = objItem.IsIndent;

                                item.UOMID = objItem.UOMID;
                                item.UOM = objItem.UOM;
                                item.BaseUOMID = objItem.BaseUOMID;
                                item.BaseUOM = objItem.BaseUOM;
                                item.SUOMID = objItem.SUOMID;
                                item.SUOM = objItem.SUOM;
                                item.StockConversionFactor = objItem.StockConversionFactor;
                                item.ConversionFactor = objItem.StockConversionFactor;
                                item.BaseConversionFactor = objItem.BaseConversionFactor;
                                item.SelectedUOM = new ValueObjects.MasterListItem { ID = item.UOMID, Description = item.UOM };

                                item.ItemVATPercentage = 0;
                                item.GRNItemVatApplicationOn = 0;
                                item.GRNItemVatType = 0;
                                item.ItemTaxPercentage = 0;
                                item.OtherGRNItemTaxApplicationOn = 0;
                                item.OtherGRNItemTaxType = 0;
                            }
                            dgBatchList.ItemsSource = null;
                            dgBatchList.ItemsSource = ocIssueItemDetailsList;
                            if (ocSelectedItemDetailsList != null && ocSelectedItemDetailsList.Count > 0)
                            {
                                for (Int32 iCount = 0; iCount < BizActionObj.IssueItemDetailsList.Count; iCount++)
                                {
                                    foreach (clsIssueItemDetailsVO item in ocSelectedItemDetailsList.ToList())
                                    {
                                        if (item.BatchId == BizActionObj.IssueItemDetailsList[iCount].BatchId && objItem.IndentId.Equals(item.IndentID) && objItem.IndentUnitID.Equals(item.IndentUnitID))
                                        {
                                            BizActionObj.IssueItemDetailsList[iCount].IsChecked = true;
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
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Indent Item List se
        /// </summary>
        private void dgIndentItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ocIssueItemDetailsList.Clear();
            this.ocBatchList.Clear();
            long? itemid = 0;
            chkSelectAll.IsChecked = false;
            if (dgIndentItemList.SelectedItem != null)
            {
                itemid = ((clsItemListByIndentId)dgIndentItemList.SelectedItem).ItemId;
                if (!((clsItemListByIndentId)dgIndentItemList.SelectedItem).IsItemBlock)
                    BindItemListByIndentId(itemid);
                else
                {
                    dgBatchList.ItemsSource = null;
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", "'" + ((clsItemListByIndentId)dgIndentItemList.SelectedItem).ItemName + "' Item is Suspended, You cannot Issue.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW3.Show();
                }
            }

            if (dgIndentItemList.SelectedIndex != -1)
            {
                int iCount = ((List<clsItemListByIndentId>)dgIndentItemList.ItemsSource).Count;
                Int32 iIndentItem = ocSelectedItemList.Where(itemList => itemList.IndentNumber.Trim().Equals(cmbIndentList.Text.Split(' ')[0])).Count();
                chkSelectAll.IsChecked = iCount == iIndentItem ? true : false;
            }
            // BindBatchList();
        }

        private void BindBatchList()
        {
            if (dgIndentItemList.SelectedItem != null && this.ocIssueItemDetailsList != null)
            {

                dgBatchList.ItemsSource = null;
                this.ocBatchList.Clear();
                //  this.ocBatchList = new List<clsIssueItemDetailsVO>();

                var result = from r in this.ocIssueItemDetailsList
                             where r.ItemId == ((clsItemListByIndentId)dgIndentItemList.SelectedItem).ItemId && r.ToStoreID == toStoreID && r.IndentID == ((clsItemListByIndentId)dgIndentItemList.SelectedItem).IndentId && r.FromStoreID == fromStoreID
                             select r;

                foreach (clsIssueItemDetailsVO item in result)
                {
                    if (ocBatchList.Where(batchItems => batchItems.BatchId == item.BatchId).Any() == false)
                        this.ocBatchList.Add(item);
                }
                dgBatchList.ItemsSource = this.ocBatchList;
            }
            else
            {

            }
        }
        clsIssueItemDetailsVO _SelectedIssueItem;
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            this._SelectedIssueItem = ((clsIssueItemDetailsVO)dgBatchList.SelectedItem);
            if (dgSelectedItemList.ItemsSource == null)
                dgSelectedItemList.ItemsSource = this.ocSelectedItemDetailsList;
            if (((CheckBox)sender).IsChecked == true)
            {
                this.ocSelectedItemDetailsList.Add((clsIssueItemDetailsVO)dgBatchList.SelectedItem);
            }
            else
            {
                clsIssueItemDetailsVO objItem = this.ocSelectedItemDetailsList.Where(z => z.BatchId == _SelectedIssueItem.BatchId && z.IndentDetailsID == _SelectedIssueItem.IndentDetailsID).FirstOrDefault();
                this.ocSelectedItemDetailsList.Remove(objItem);
            }
        }

        private void CheckBox1_Click(object sender, RoutedEventArgs e)
        {
            clsIssueItemDetailsVO objIssueItem = ((clsIssueItemDetailsVO)dgSelectedItemList.SelectedItem);
            if (dgSelectedItemList.ItemsSource == null)
                dgSelectedItemList.ItemsSource = this.ocSelectedItemDetailsList;
            {
                this.ocSelectedItemDetailsList.Remove((clsIssueItemDetailsVO)dgSelectedItemList.SelectedItem);
                this.ocIssueItemDetailsList.Where(z => z.IndentDetailsID == objIssueItem.IndentDetailsID && z.BatchId == objIssueItem.BatchId).Select(z => z.IsChecked = false);
            }
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            //ocSelectedItemList
            if (((CheckBox)sender).IsChecked == true)
            {
                this.ocSelectedItemList.Add((clsItemListByIndentId)dgIndentItemList.SelectedItem);
            }
            else
            {
                clsItemListByIndentId objIndentItem = (clsItemListByIndentId)dgIndentItemList.SelectedItem;
                objIndentItem = this.ocSelectedItemList.Where(z => z.IndentId == objIndentItem.IndentId && z.IndentNumber == objIndentItem.IndentNumber && z.ItemId == objIndentItem.ItemId).First();
                this.ocSelectedItemList.Remove(objIndentItem);
            }
            if (dgIndentItemList.SelectedIndex != -1)
            {
                int iCount = ((List<clsItemListByIndentId>)dgIndentItemList.ItemsSource).Count;
                Int32 iIndentItem = ocSelectedItemList.Where(itemList => itemList.IndentNumber.Trim().Equals(cmbIndentList.Text.Split(' ')[0])).Count();
                chkSelectAll.IsChecked = iCount == iIndentItem ? true : false;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<clsItemListByIndentId> ItemList = new List<clsItemListByIndentId>();
                if (dgIndentItemList.ItemsSource != null)
                {
                    ItemList = (List<clsItemListByIndentId>)dgIndentItemList.ItemsSource;
                    if (ItemList != null && ItemList.Count > 0)
                    {
                        if (chkSelectAll.IsChecked == true)
                        {
                            foreach (var item in ItemList)
                            {
                                item.IsChecked = true;
                                if (ocSelectedItemList == null)
                                    ocSelectedItemList = new ObservableCollection<clsItemListByIndentId>();
                                ocSelectedItemList.Add(item);
                            }
                        }
                        else
                        {
                            foreach (var item in ItemList)
                            {
                                item.IsChecked = false;
                                if (ocSelectedItemList != null)
                                    ocSelectedItemList.Remove(item);
                            }
                        }
                        dgIndentItemList.ItemsSource = null;
                        dgIndentItemList.ItemsSource = ItemList;
                    }
                    else
                        chkSelectAll.IsChecked = false;
                }

            }
            catch (Exception Ex)
            {
                throw;
            }
        }

    }
}

