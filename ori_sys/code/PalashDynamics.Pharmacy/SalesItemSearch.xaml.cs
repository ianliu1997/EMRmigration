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
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.ComponentModel;
using PalashDynamics.Collections;
namespace PalashDynamics.Pharmacy
{
    public partial class SalesItemSearch : ChildWindow, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Variables
        int i = 0;
        public event RoutedEventHandler OnSaveButton_Click;
        public PagedSortableCollectionView<clsItemSalesVO> ItemSalesList { get; private set; }
        public PagedSortableCollectionView<clsItemSalesDetailsVO> ItemSalesDetailsList { get; private set; }
        #endregion

        #region Propertiies
        public ObservableCollection<clsItemSalesDetailsVO> SelectedSalesDetailsItem { get; set; }
        public clsItemSalesVO SelectedItem { get; set; }
        #endregion

        #region Paging Properties
        public int PageSizeSalesList
        {
            get
            {
                return ItemSalesList.PageSize;
            }
            set
            {
                if (value == ItemSalesList.PageSize) return;
                ItemSalesList.PageSize = value;
                OnPropertyChanged("PageSizeSalesList");
            }
        }
        public int PageSizeSalesDetailsList
        {
            get
            {
                return ItemSalesDetailsList.PageSize;
            }
            set
            {
                if (value == ItemSalesDetailsList.PageSize) return;
                ItemSalesDetailsList.PageSize = value;
                OnPropertyChanged("PageSizeSalesDetailsList");
            }
        }
        #endregion

        #region Validation
        public Boolean CheckValidationForSearch()
        {

            if (dtItemSalesFromDate.SelectedDate != null && dtItemSalesToDate.SelectedDate == null)
            {
                dtItemSalesToDate.SetValidation("Please Enter From Date");
                dtItemSalesToDate.RaiseValidationError();
                dtItemSalesToDate.Focus();
                return false;

            }
            else if (dtItemSalesFromDate.SelectedDate == null && dtItemSalesToDate.SelectedDate != null)
            {
                dtItemSalesFromDate.SetValidation("Please Enter To Date");
                dtItemSalesFromDate.RaiseValidationError();
                dtItemSalesFromDate.Focus();
                return false;
            }
            else if (dtItemSalesFromDate.SelectedDate > dtItemSalesToDate.SelectedDate)
            {
                dtItemSalesFromDate.SetValidation("From Date Must be Less than To Date");
                dtItemSalesFromDate.RaiseValidationError();
                dtItemSalesFromDate.Focus();
                return false;
            }
            else
            {
                dtItemSalesToDate.ClearValidationError();
                dtItemSalesFromDate.ClearValidationError();
                return true;
            }
        }

        #endregion Validation


        public SalesItemSearch()
        {
            InitializeComponent();

            SelectedSalesDetailsItem = new ObservableCollection<clsItemSalesDetailsVO>();
            SelectedItem = new clsItemSalesVO();

            ItemSalesList = new PagedSortableCollectionView<clsItemSalesVO>();
            ItemSalesList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesList_OnRefresh);
            PageSizeSalesList = 5;
            this.dataGrid2Pager.DataContext = ItemSalesList;
            this.dgItemSales.DataContext = ItemSalesList;
            dtItemSalesFromDate.SelectedDate = DateTime.Now;
            dtItemSalesToDate.SelectedDate = DateTime.Now;
            setItemSalesGrid();


        }

        void ItemSalesList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setItemSalesGrid();
        }


        public void setItemSalesGrid()
        {
            clsGetItemSalesBizActionVO BizAction = new clsGetItemSalesBizActionVO();
            BizAction.FromDate = dtItemSalesFromDate.SelectedDate;
            BizAction.ToDate = dtItemSalesToDate.SelectedDate;
            BizAction.SerachExpression = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString();
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = ItemSalesList.PageSize;
            BizAction.isBillFreezed = true;
            BizAction.StartRowIndex = ItemSalesList.PageIndex * ItemSalesList.PageSize;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;
            BizAction.BillNo = txtBillNo.Text;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.Details = new List<clsItemSalesVO>();
                    BizAction.Details = ((clsGetItemSalesBizActionVO)args.Result).Details;
                    ItemSalesList.Clear();
                    ItemSalesList.TotalItemCount = (int)(((clsGetItemSalesBizActionVO)args.Result).TotalRows);
                    ///Setup Page Fill DataGrid

                    foreach (clsItemSalesVO item in BizAction.Details)
                    {
                        ItemSalesList.Add(item);
                    }

                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void setItemSalesDetailsGrid(long ItemSaleId, long iUnitID)
        {
            clsGetItemSalesDetailsBizActionVO BizAction = new clsGetItemSalesDetailsBizActionVO();
            BizAction.IsFromItemSaleReturn = true;
            BizAction.ItemSalesID = ItemSaleId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = ItemSalesDetailsList.PageSize;
            BizAction.StartRowIndex = ItemSalesDetailsList.PageIndex * ItemSalesDetailsList.PageSize;
            BizAction.UnitID = iUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.Details = new List<clsItemSalesDetailsVO>();
                    BizAction.Details = ((clsGetItemSalesDetailsBizActionVO)args.Result).Details;


                    ItemSalesDetailsList.Clear();
                    ItemSalesDetailsList.TotalItemCount = (int)(((clsGetItemSalesDetailsBizActionVO)args.Result).TotalRows);
                    ///Setup Page Fill DataGrid

                    foreach (clsItemSalesDetailsVO item in BizAction.Details)
                    {
                        //item.PendingQuantity=Bi

                        ItemSalesDetailsList.Add(item);
                    }



                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedSalesDetailsItem != null && SelectedSalesDetailsItem.Count != 0)
            {
                if (((clsItemSalesVO)this.dgItemSales.SelectedItem).BalAmount != 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Selected Bill Is Not Fully Paid", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    this.DialogResult = true;
                    if (OnSaveButton_Click != null)
                        OnSaveButton_Click(this, new RoutedEventArgs());
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select The Items For Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
            
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedSalesDetailsItem = null;
            this.DialogResult = false;
        }

        private void dgItemSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemSales.SelectedItem != null)
            {
                SelectedItem = ((clsItemSalesVO)dgItemSales.SelectedItem).DeepCopy<clsItemSalesVO>();
                ItemSalesDetailsList = new PagedSortableCollectionView<clsItemSalesDetailsVO>();
                ItemSalesDetailsList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesDetailsList_OnRefresh);
                PageSizeSalesDetailsList = 5;
                this.dataGrid2PagerSalesDetails.DataContext = ItemSalesDetailsList;
                this.dgItemSalesDetails.DataContext = ItemSalesDetailsList;
                setItemSalesDetailsGrid(SelectedItem.ID, SelectedItem.UnitId);
            }
        }

        void ItemSalesDetailsList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setItemSalesDetailsGrid(SelectedItem.ID, SelectedItem.UnitId);
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidationForSearch())
            {
                ItemSalesList = new PagedSortableCollectionView<clsItemSalesVO>();
                ItemSalesList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesList_OnRefresh);
                PageSizeSalesList = 5;
                this.dataGrid2Pager.DataContext = ItemSalesList;
                this.dgItemSales.DataContext = ItemSalesList;
                setItemSalesGrid();
            }
        }

        private void chkStatus_Checked(object sender, RoutedEventArgs e)
        {
            clsItemSalesDetailsVO p1 = ((CheckBox)e.OriginalSource).DataContext as clsItemSalesDetailsVO;
            if (p1.CheckStatus == false)
            {
                if (this.dgItemSalesDetails.SelectedItem is clsItemSalesDetailsVO)
                {
                    SelectedSalesDetailsItem.Add(((clsItemSalesDetailsVO)this.dgItemSalesDetails.SelectedItem));
                }
            }

        }


        private void chkStatus_Unchecked(object sender, RoutedEventArgs e)
        {
            clsItemSalesDetailsVO p1 = ((CheckBox)e.OriginalSource).DataContext as clsItemSalesDetailsVO;
            if (p1.Status == true)
            {
                clsItemSalesDetailsVO person = this.dgItemSalesDetails.SelectedItem as clsItemSalesDetailsVO;
                if (person != null)
                {
                    var item =
                    from p in SelectedSalesDetailsItem
                    where p.ItemID == person.ItemID && p.BatchID == person.BatchID
                    select p;
                    if (((List<clsItemSalesDetailsVO>)item.ToList<clsItemSalesDetailsVO>()).Count > 0)
                    {
                        SelectedSalesDetailsItem.Remove(((List<clsItemSalesDetailsVO>)item.ToList<clsItemSalesDetailsVO>())[0]);
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtSearchItemSale_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter || e.Key == Key.Back || e.Key == Key.Delete)
            if (e.Key == Key.Enter)
            {
                setItemSalesGrid();
            }
        }

        private void chkStatus_Click(object sender, RoutedEventArgs e)
        {
            
           // clsItemSalesDetailsVO p1 = ((CheckBox)e.OriginalSource).DataContext as clsItemSalesDetailsVO;
            if (((CheckBox)sender).IsChecked==true)
            {
                if (this.dgItemSalesDetails.SelectedItem is clsItemSalesDetailsVO)
                {
                    SelectedSalesDetailsItem.Add(((clsItemSalesDetailsVO)this.dgItemSalesDetails.SelectedItem));
                }
            }
            else if (((CheckBox)sender).IsChecked ==false)
            {
                clsItemSalesDetailsVO person = this.dgItemSalesDetails.SelectedItem as clsItemSalesDetailsVO;
                if (person != null)
                {
                    var item =
                    from p in SelectedSalesDetailsItem
                    where p.ItemID == person.ItemID && p.BatchID == person.BatchID
                    select p;
                    if (((List<clsItemSalesDetailsVO>)item.ToList<clsItemSalesDetailsVO>()).Count > 0)
                    {
                        SelectedSalesDetailsItem.Remove(((List<clsItemSalesDetailsVO>)item.ToList<clsItemSalesDetailsVO>())[0]);
                    }
                }
            }
        }
    }
}

