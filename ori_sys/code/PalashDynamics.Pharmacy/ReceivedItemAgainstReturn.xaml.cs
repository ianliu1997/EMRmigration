using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;


namespace PalashDynamics.Pharmacy
{
    public partial class ReceivedItemAgainstReturn : UserControl
    {
        #region Variable Declaration
        private SwivelAnimation objAnimation;

        #region 'Paging'

        public PagedSortableCollectionView<clsReceivedAgainstReturnListVO> ReceivedDataList { get; private set; }

        public int DataListPageSize
        {
            get
            {
                return ReceivedDataList.PageSize;
            }
            set
            {
                if (value == ReceivedDataList.PageSize) return;
                ReceivedDataList.PageSize = value;

            }
        }


        public PagedSortableCollectionView<clsReturnListVO> ReturnDataList { get; private set; }

        public int ReturnDataListPageSize
        {
            get
            {
                return ReturnDataList.PageSize;
            }
            set
            {
                if (value == ReturnDataList.PageSize) return;
                ReturnDataList.PageSize = value;

            }
        }

        #endregion

        private ObservableCollection<clsReceivedItemAgainstReturnDetailsVO> _ocReceivedItemDetailsList = new ObservableCollection<clsReceivedItemAgainstReturnDetailsVO>();
        public ObservableCollection<clsReceivedItemAgainstReturnDetailsVO> ocReceivedItemDetailsList
        {
            get
            {
                return _ocReceivedItemDetailsList;
            }
            set
            {
                _ocReceivedItemDetailsList = value;
            }
        }
        int ClickedFlag1 = 0;
        bool flagZeroTransit = false;
        WaitIndicator Indicatior;
        #endregion

        #region Constructor/Loaded
        public ReceivedItemAgainstReturn()
        {
            Indicatior = new WaitIndicator();
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            ReceivedDataList = new PagedSortableCollectionView<clsReceivedAgainstReturnListVO>();
            ReceivedDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ReceivedDataList_OnRefresh);
            DataListPageSize = 15;
            ReceivedDataList.PageSize = DataListPageSize;
            dgdpReceivedList.Source = ReceivedDataList;

            //Paging Return list
            ReturnDataList = new PagedSortableCollectionView<clsReturnListVO>();
            ReturnDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ReturnDataList_OnRefresh);
            ReturnDataListPageSize = 15;
            dgdpReturnList.PageSize = ReturnDataListPageSize;
            dgdpReturnList.Source = ReturnDataList;
            dgReturnItemList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgReturnItemList_CellEditEnded);
            //dgReturnItemList
            this.Loaded += new RoutedEventHandler(ReceivedItem_Loaded);
        }


        void ReturnDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillReturnList();
        }

        void ReceivedDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillReceivedList();
        }

        void ReceivedItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgReturnItemList.ItemsSource == null)
                dgReturnItemList.ItemsSource = this.ocReceivedItemDetailsList;

            dpReceivedDate.SelectedDate = DateTime.Now;
            dpReceivedFromDate.SelectedDate = DateTime.Now;
            dpReceivedToDate.SelectedDate = DateTime.Now;
            dpReceivedDate.IsEnabled = false;
            FillStore();
            FillReceivedList();
            FillReceivedByList();
        }
        #endregion

        #region Private Methods
        private void FillStore()
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    clsStoreVO additem = new clsStoreVO();
                    additem.StoreName = "--Select--";
                    additem.StoreId = 0;
                    additem.Status = true;

                    List<clsStoreVO> Item = (List<clsStoreVO>)result.ToList();
                    BizActionObj.ItemMatserDetails.Insert(0, additem);

                    Item.Insert(0, additem);
                    BizActionObj.ToStoreList.Insert(0, additem);

                    var NonQSAndUserAssigned = from item in BizActionObj.ToStoreList.ToList()
                                               where item.IsQuarantineStore == false
                                               select item;

                    NonQSAndUserAssigned.ToList().Insert(0, additem);

                    cmbFromStore.ItemsSource = NonQSAndUserAssigned.ToList(); //Item;// (List<clsStoreVO>)result.ToList();
                    cmbReceivedFromStoreSrch.ItemsSource = NonQSAndUserAssigned.ToList();//Item; //(List<clsStoreVO>)result.ToList();

                    ////cmbToStore.ItemsSource = BizActionObj.ItemMatserDetails;
                    //cmbToStore.ItemsSource = Item.ToList();//result1.ToList();

                    cmbToStore.ItemsSource = result1.ToList();

                    //cmbReceivedToStoreSrch.ItemsSource = BizActionObj.ItemMatserDetails;
                    //cmbReceivedToStoreSrch.ItemsSource = Item.ToList();//result1.ToList();

                    cmbReceivedToStoreSrch.ItemsSource = result1.ToList();

                    cmbFromStore.SelectedValue = (long)0;
                    cmbReceivedFromStoreSrch.SelectedValue = (long)0;
                    cmbToStore.SelectedValue = (long)0;
                    cmbReceivedToStoreSrch.SelectedValue = (long)0;

                }

            };

            client.CloseAsync();

        }

        private void ResetControls()
        {
            if (ReturnDataList != null)
            {
                ReturnDataList = new PagedSortableCollectionView<clsReturnListVO>();
                ReturnDataList.Clear();
                dgReturnList.ItemsSource = ReturnDataList;
            }
            dpReceivedDate.SelectedDate = DateTime.Now;

            txtReceivedNumber.Text = String.Empty;
            //dgReturnItemList.ItemsSource = null;
            this.ocReceivedItemDetailsList.Clear();
            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            // txtTotalVAT.Text = String.Empty;
            txtRemark.Text = String.Empty;

        }

        private void FillReceivedList()
        {
            Indicatior.Show();
            try
            {
                clsGetReceivedListAgainstReturnBizActionVO BizAction = new clsGetReceivedListAgainstReturnBizActionVO();
                BizAction.ReceivedList = new List<clsReceivedAgainstReturnListVO>();

                BizAction.ReceivedNumberSrc = txtRetunNumberSrc.Text.Trim();

                BizAction.ReceivedFromDate = dpReceivedFromDate.SelectedDate;

                BizAction.ReceivedToDate = dpReceivedToDate.SelectedDate;
                if (BizAction.ReceivedToDate != null)
                    BizAction.ReceivedToDate = BizAction.ReceivedToDate.Value.Date.AddDays(1);
                //Change By harish 17 apr
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
                }
                else
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                if (cmbReceivedFromStoreSrch.SelectedItem != null)
                    BizAction.ReceivedFromStoreId = ((clsStoreVO)cmbReceivedFromStoreSrch.SelectedItem).StoreId;


                if (cmbReceivedToStoreSrch.SelectedItem != null)
                    BizAction.ReceivedToStoreId = ((clsStoreVO)cmbReceivedToStoreSrch.SelectedItem).StoreId;

                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = ReceivedDataList.PageIndex * ReceivedDataList.PageSize + 1;
                BizAction.MaximumRows = ReceivedDataList.PageSize;


                foreach (SortDescription sortDesc in ReceivedDataList.SortDescriptions)
                {
                    BizAction.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                }
                #endregion


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = ((clsGetReceivedListAgainstReturnBizActionVO)e.Result);


                        ReceivedDataList.TotalItemCount = BizAction.TotalRows;

                        ReceivedDataList.Clear();
                        foreach (var item in BizAction.ReceivedList)
                        {
                            ReceivedDataList.Add(item);
                        }

                        dgReceivedList.ItemsSource = null;
                        dgReceivedList.ItemsSource = ReceivedDataList;

                        dgdpReceivedList.Source = null;
                        dgdpReceivedList.PageSize = BizAction.MaximumRows;
                        dgdpReceivedList.Source = ReceivedDataList;
                        txtTotalCountRecords.Text = ReceivedDataList.TotalItemCount.ToString();
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }

        private bool CheckValidation()
        {
            Boolean IsValidationComplete = true;

            if (dpReceivedDate.SelectedDate == null)
            {
                dpReceivedDate.SetValidation("Received Date can not be blank.");
                dpReceivedDate.RaiseValidationError();
                dpReceivedDate.Focus();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }

            if (dpReceivedDate.SelectedDate != null)
            {
                if (dpReceivedDate.SelectedDate.Value.Date < DateTime.Now.Date)
                {
                    dpReceivedDate.SetValidation("Return Date can not be less than Today's Date");
                    dpReceivedDate.RaiseValidationError();
                    dpReceivedDate.Focus();
                    IsValidationComplete = false;
                    Indicatior.Close();
                    ClickedFlag1 = 0;
                    return IsValidationComplete;
                }
                if (dpReceivedDate.SelectedDate.Value.Date > DateTime.Now.Date)
                {
                    dpReceivedDate.SetValidation("Return Date can not be greater than Today's Date");
                    dpReceivedDate.RaiseValidationError();
                    dpReceivedDate.Focus();
                    IsValidationComplete = false;
                    Indicatior.Close();
                    ClickedFlag1 = 0;
                    return IsValidationComplete;
                }
            }

            if (cmbFromStore.SelectedItem == null || ((clsStoreVO)cmbFromStore.SelectedItem).StoreId == 0)
            {
                cmbFromStore.TextBox.SetValidation("Received To Store can not be blank.");
                cmbFromStore.TextBox.RaiseValidationError();
                cmbFromStore.Focus();
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Please Select Received To Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }

            if (cmbToStore.SelectedItem == null || ((clsStoreVO)cmbToStore.SelectedItem).StoreId == 0)
            {
                cmbToStore.TextBox.SetValidation("Return From Store can not be blank.");
                cmbToStore.TextBox.RaiseValidationError();
                cmbToStore.Focus();
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Return From Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }

            if (this.ocReceivedItemDetailsList != null)
            {
                if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>().Count <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Received Items are not Available.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    IsValidationComplete = false;
                    msgWin.Show();
                    Indicatior.Close();
                    ClickedFlag1 = 0;
                    return IsValidationComplete;
                }
                else
                {
                    float fltOne = 1;
                    float fltZero = 0;
                    float Infinity = fltOne / fltZero;
                    float NaN = fltZero / fltZero;
                    for (int i = 0; i < this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>().Count; i++)
                    {
                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ConversionFactor <= 0 || this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ConversionFactor == Infinity || this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ConversionFactor == NaN)
                        {
                            IsValidationComplete = false;
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            break;
                        }
                        else if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BaseConversionFactor <= 0 || this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BaseConversionFactor == Infinity || this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BaseConversionFactor == NaN)
                        {
                            IsValidationComplete = false;
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            break;
                        }

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ReceivedQty < 0)
                        {
                            IsValidationComplete = false;
                            this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ReceivedQty = 0;
                            this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ItemTotalAmount = 0;
                            MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            mgbx.Show();
                            break;
                            //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                        }

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].SelectedUOM != null && this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].SelectedUOM.ID == this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BaseUOMID && (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ReceivedQty % 1) != 0)
                        {
                            IsValidationComplete = false;
                            this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ReceivedQty = 0;
                            string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                            break;
                        }

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BalanceQty != 0)
                        {
                            if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].ReceivedQty == 0)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Receive Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                break;
                            }
                        }
                    }

                    bool flagReceivedItem = false;
                    for (int i = 0; i < this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>().Count; i++)
                    {
                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemAgainstReturnDetailsVO>()[i].BalanceQty != 0)
                        {
                            flagReceivedItem = false;
                            break;
                        }

                        flagReceivedItem = true;
                    }
                    if (flagReceivedItem)
                    {
                        IsValidationComplete = false;
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Unable To Save! Quantity is Already Received!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        flagReceivedItem = false;
                        Indicatior.Close();
                        ClickedFlag1 = 0;
                        return IsValidationComplete;
                    }
                }
            }

            dpReceivedDate.ClearValidationError();
            cmbFromStore.ClearValidationError();
            cmbToStore.ClearValidationError();
            return IsValidationComplete;
        }

        private void FillReturnList()
        {


            Indicatior.Show();
            try
            {
                clsGetReturnListBizActionVO BizAction = new clsGetReturnListBizActionVO();
                BizAction.ReturnList = new List<clsReturnListVO>();

                // BizAction.IndentCriteria = 0;
                BizAction.UserId = 0; //added By Ashish Z. on dated 13Apr16

                BizAction.ReturnFromDate = null;
                BizAction.ReturnToDate = null;

                if (cmbToStore.SelectedItem != null)
                    BizAction.ReturnFromStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                else
                    BizAction.ReturnFromStoreId = 0;

                if (cmbFromStore.SelectedItem != null)
                    BizAction.ReturnToStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                else
                    BizAction.ReturnToStoreId = 0;
                BizAction.transactionType = ValueObjects.InventoryTransactionType.ReceivedItemAgainstReturn;

                // Change By harish 17 apr
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitId = 0;
                }
                else
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }


                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = ReturnDataList.PageIndex * ReturnDataList.PageSize + 1;
                BizAction.MaximumRows = ReturnDataList.PageSize;


                foreach (SortDescription sortDesc in ReturnDataList.SortDescriptions)
                {
                    BizAction.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                }
                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetReturnListBizActionVO)e.Result;

                        BizAction.ReturnList = ((clsGetReturnListBizActionVO)e.Result).ReturnList;
                        ReturnDataList.TotalItemCount = BizAction.TotalRows;

                        ReturnDataList.Clear();
                        foreach (var item in BizAction.ReturnList)
                        {
                            ReturnDataList.Add(item);
                        }

                        dgReturnList.ItemsSource = null;
                        dgReturnList.ItemsSource = ReturnDataList;

                        dgdpReturnList.Source = null;
                        dgdpReturnList.PageSize = BizAction.MaximumRows;
                        dgdpReturnList.Source = ReturnDataList;

                        Indicatior.Close();
                    }


                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }

        private void FillItemListByReturnId()
        {
            if (dgReturnList.SelectedItem != null)
            {
                clsGetItemListByReturnIdBizActionVO BizAction = new clsGetItemListByReturnIdBizActionVO();

                BizAction.ReturnId = ((clsReturnListVO)dgReturnList.SelectedItem).ReturnId;
                BizAction.UnitId = ((clsReturnListVO)dgReturnList.SelectedItem).UnitId;
                BizAction.ItemList = new List<clsReturnItemDetailsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemListAgainstReturn = ((clsGetItemListByReturnIdBizActionVO)e.Result).ItemListAgainstReturn;
                        // this.ocReceivedItemDetailsList = BizAction.ReceivedItemList;
                        //  dgReturnItemList.ItemsSource = null;
                        this.ocReceivedItemDetailsList.Clear();
                        foreach (clsReceivedItemAgainstReturnDetailsVO item in BizAction.ItemListAgainstReturn)
                        {
                            if (item.BalanceQty > 0)
                            {
                                item.IsChecked = true;



                                if (item.BatchCode == null || item.BatchCode == "")
                                {
                                    item.ExpiryDate = null;
                                }
                                this.ocReceivedItemDetailsList.Add(item);
                            }
                        }

                        //dgReturnItemList.ItemsSource = this.ocReceivedItemDetailsList;
                        SumOfTotals();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                //dgReturnItemList.ItemsSource = null;
                this.ocReceivedItemDetailsList.Clear();
            }
        }

        private void SumOfTotals()
        {

            var results = from r in this.ocReceivedItemDetailsList
                          select r;

            //foreach (clsReceivedItemDetailsVO item in results)
            //{

            //}


            decimal? TotalReceivedItem = results.Sum(cnt => cnt.ReturnQty);
            // txtNoOfItems.Text = TotalReceivedItem.ToString();
            txtNoOfItems.Text = String.Format("{0:0.00}", TotalReceivedItem);

            decimal? TotalVATAmount = results.Sum(cnt => cnt.ItemVATAmount);
            //txtTotalVAT.Text = TotalVATAmount.ToString();

            decimal? TotalAmount = results.Sum(cnt => cnt.ItemTotalAmount);
            //txtTotalAmount.Text = TotalAmount.ToString();
            txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);


        }

        /// <summary>
        /// Method is used to populate the user dropdown.
        /// Added on 12November2013.
        /// </summary>
        private void FillReceivedByList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    cmbReceivedBy.ItemsSource = null;
                    cmbReceivedBy.ItemsSource = objList;
                    cmbReceivedBy.SelectedItem = objList[0];

                    if (((IApplicationConfiguration)App.Current).CurrentUser != null)
                        cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void FillItemListByReceivedId()
        {
            if (dgReceivedList.SelectedItem != null)
            {
                clsGetItemListByReturnReceivedIdBizActionVO BizAction = new clsGetItemListByReturnReceivedIdBizActionVO();

                BizAction.ReceivedId = ((clsReceivedAgainstReturnListVO)dgReceivedList.SelectedItem).ReceivedId;
                BizAction.UnitId = ((clsReceivedAgainstReturnListVO)dgReceivedList.SelectedItem).UnitId;
                BizAction.ItemList = new List<clsReceivedItemAgainstReturnDetailsVO>();
                BizAction.ReceivedUnitId = ((clsReceivedAgainstReturnListVO)dgReceivedList.SelectedItem).ReturnUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemList = ((clsGetItemListByReturnReceivedIdBizActionVO)e.Result).ItemList;



                        dgReceivedItemList.ItemsSource = null;
                        dgReceivedItemList.ItemsSource = BizAction.ItemList;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                dgReceivedItemList.ItemsSource = null;
            }
        }
        #endregion

        #region Clicked Evets
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            //Indicatior.Show();

            try
            {
                if (ClickedFlag1 == 1)
                {

                    Boolean IsValidation = CheckValidation();


                    var resultSelectedItem = from r in this.ocReceivedItemDetailsList
                                             where r.ReceivedQty > 0 && r.IsChecked == true
                                             select r;
                    if (IsValidation == true && dgReturnList.ItemsSource != null && resultSelectedItem != null)
                    {
                         MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                         msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                         {
                             if (res == MessageBoxResult.Yes)
                             {
                                 Indicatior.Show();
                                 clsAddReceivedItemAgainstReturnBizActionVO BizAction = new clsAddReceivedItemAgainstReturnBizActionVO();

                                 BizAction.ReceivedItemAgainstReturnDetails = new clsReceivedItemAgainstReturnVO(); ;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedItemAgainstReturnDetailsList = new List<clsReceivedItemAgainstReturnDetailsVO>();

                                 BizAction.ReceivedItemAgainstReturnDetails.ReturnID = ((clsReturnListVO)dgReturnList.SelectedItem).ReturnId;
                                 BizAction.ReceivedItemAgainstReturnDetails.IsIndent = ((clsReturnListVO)dgReturnList.SelectedItem).IsIndent;

                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedDate = dpReceivedDate.SelectedDate;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedID = 0;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedNumber = null;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                                 BizAction.ReceivedItemAgainstReturnDetails.ReturnUnitId = ((clsReturnListVO)dgReturnList.SelectedItem).UnitId;

                                 BizAction.ReceivedItemAgainstReturnDetails.Remark = txtRemark.Text.Trim();
                                 BizAction.ReceivedItemAgainstReturnDetails.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim());
                                 BizAction.ReceivedItemAgainstReturnDetails.TotalItems = Convert.ToDecimal(txtNoOfItems.Text.Trim());
                                 //BizAction.ReceivedItemDetails.TotalVATAmount = Convert.ToDecimal(txtTotalVAT.Text.Trim());

                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedByID = cmbReceivedBy.SelectedItem != null ? ((MasterListItem)cmbReceivedBy.SelectedItem).ID : 0;

                                 //  BizAction.ReceivedItemDetails.ReceivedItemDetailsList = this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>();

                                 BizAction.ReceivedItemAgainstReturnDetails.ReceivedItemAgainstReturnDetailsList = resultSelectedItem.ToList<clsReceivedItemAgainstReturnDetailsVO>();

                                 Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                 PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                 Client.ProcessCompleted += (s, e1) =>
                                 {
                                     Indicatior.Close();
                                     ClickedFlag1 = 0;
                                     if (e1.Error == null && e1.Result != null && ((clsAddReceivedItemAgainstReturnBizActionVO)e1.Result).ReceivedItemAgainstReturnDetails != null)
                                     {
                                         MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Received details saved successfully with receive no. " + ((clsAddReceivedItemAgainstReturnBizActionVO)e1.Result).ReceivedItemAgainstReturnDetails.ReceivedNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                         msgWin.Show();
                                         FillReceivedList();
                                     }
                                     else
                                     {
                                         MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                         msgWin.Show();
                                     }


                                 };
                                 Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                 Client.CloseAsync();
                                 objAnimation.Invoke(RotationType.Backward);
                             }
                             else
                                 ClickedFlag1 = 0;
                         };
                         msgWindow.Show();

                    }
                    else
                    {
                        //Indicatior.Close();
                        ClickedFlag1 = 0;
                    }

                }
            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                Indicatior.Close();
                throw;
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    this.ocReceivedItemDetailsList.Add((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem);
            //}
            //else
            //{
            //    this.ocReceivedItemDetailsList.Remove((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem);
            //}
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgReturnItemList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ocReceivedItemDetailsList.RemoveAt(dgReturnItemList.SelectedIndex);
                        dgReturnItemList.Focus();
                        dgReturnItemList.UpdateLayout();
                        dgReturnItemList.SelectedIndex = ocReceivedItemDetailsList.Count - 1;
                        SumOfTotals();

                    }
                };
                msgWD.Show();
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceivedList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/ReceiveReturnItemPrint.aspx?ReceivedId=" + ((clsReceivedAgainstReturnListVO)dgReceivedList.SelectedItem).ReceivedId + "&UnitID=" + (((clsReceivedAgainstReturnListVO)dgReceivedList.SelectedItem).UnitId)), "_blank");
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            cmbFromStore.SelectedValue = (long)0;
            cmbToStore.SelectedValue = (long)0;
            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdSearchReceived_Click(object sender, RoutedEventArgs e)
        {
            FillReceivedList();
        }
        #endregion

        #region KeyDown/SelectionChanged/GotFocus
        private void cmbFromStore_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbFromStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFromStore.SelectedItem != null && ((clsStoreVO)cmbFromStore.SelectedItem).StoreId > 0)
            {
                if (cmbToStore.SelectedItem != null && cmbFromStore.SelectedItem != null)
                {
                    if (((clsStoreVO)cmbFromStore.SelectedItem).StoreId != ((clsStoreVO)cmbToStore.SelectedItem).StoreId)
                    {
                        FillReturnList();
                    }

                }
                else
                {
                    FillReturnList();
                }
            }

        }

        private void cmbToStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbToStore.SelectedItem != null && ((clsStoreVO)cmbToStore.SelectedItem).StoreId > 0)
            {
                if (cmbToStore.SelectedItem != null && cmbFromStore.SelectedItem != null)
                {
                    if (((clsStoreVO)cmbFromStore.SelectedItem).StoreId != ((clsStoreVO)cmbToStore.SelectedItem).StoreId)
                    {
                        FillReturnList();
                    }

                }
                else
                {
                    FillReturnList();
                }
            }
        }

        private void dgReturnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReturnList.SelectedItem != null)
            {
                FillItemListByReturnId();
            }
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).UOMConversionList == null || (((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).UOMConversionList.Count == 0)))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        private void dgReceivedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByReceivedId();
        }
        #endregion

        #region DataGrid Events(CellEditEnded)
        private void dgReturnItemList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            #region Added By Pallavi
            if (e.Column.Header.ToString().Equals("Receive Quantity") || e.Column.Header.ToString().Equals("Receive UOM"))//Received Quantity
            {
                clsReceivedItemAgainstReturnDetailsVO objVO = new clsReceivedItemAgainstReturnDetailsVO();
                objVO = (clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem;

                if (objVO.ReceivedQty < 0)
                {
                    objVO.ReceivedQty = 0;
                    objVO.ItemTotalAmount = 0;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                    //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                }


                objVO.ReceivedQty = System.Math.Round(objVO.ReceivedQty, 1);
                if (((int)objVO.ReceivedQty).ToString().Length > 5)
                {
                    objVO.ReceivedQty = 1;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Receive Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                    //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                }

                if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                {
                    objVO.StockingQuantity = Convert.ToDouble(objVO.ReceivedQty) * objVO.ConversionFactor;
                    objVO.BaseQuantity = Convert.ToSingle(objVO.ReceivedQty) * objVO.BaseConversionFactor;
                    objVO.ItemTotalAmount = Convert.ToDecimal(objVO.BaseQuantity) * objVO.PurchaseRate;

                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.ReceivedQty % 1) != 0)
                    {
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;
                        string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.BalanceQty)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Greater Receive Quantity!", "Received Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Received Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;

                        return;
                    }

                    if (Convert.ToDecimal(objVO.BaseQuantity) > (objVO.ReceivedQty * Convert.ToDecimal(objVO.BaseConversionFactor)))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Received Quantity In The List Can't Be Greater Than Returned Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }
                }


                //if (!flagZeroTransit)
                //{
                //    if (((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BalanceQty == 0)
                //    {

                //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                //                  new MessageBoxControl.MessageBoxChildWindow("Can't Enter Received Quantity!", "Returned Quantity Is Already Received!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        msgW3.Show();
                //        ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReceivedQty = 0;
                //        flagZeroTransit = true;
                //        return;
                //    }
                //    else if (((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReceivedQty > ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BalanceQty)
                //    {
                //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                //                  new MessageBoxControl.MessageBoxChildWindow("Greater Received Quantity!", " Received Quantity In The List Can't Be Greater Than Transit Quantity. Please Enter Received Quantity Less Than Or Equal To Transit Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //        msgW3.Show();
                //        ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReceivedQty = ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BalanceQty;
                //        return;
                //    }
                //}

                //if (((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SelectedUOM.ID > 0)
                //{
                //    // Function Parameters
                //    // FromUOMID - Transaction UOM
                //    // ToUOMID - Stocking UOM
                //    CalculateConversionFactorCentral(((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SelectedUOM.ID, ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SUOMID);
                //}
                //else
                //{
                //    //((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReturnQty = 0;
                //    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseQuantity = 0;

                //    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ConversionFactor = 0;
                //    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseConversionFactor = 0;
                //}


            }
            #endregion
            SumOfTotals();
        }
        #endregion

        #region Conversion Factor
        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = Convert.ToInt64(((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ItemId);
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        Indicatior.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                        if (UOMConvertLIst != null)
                            cmbConversions.SelectedItem = UOMConvertLIst[0];

                        //List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                        //List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                        //MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                        //ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());



                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgReturnItemList.SelectedItem != null)
                        {
                            ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw;
            }
        }

        private void cmbUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgReturnItemList.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReceivedQty = 0;
                ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ConversionFactor = 0;
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseConversionFactor = 0;

                    //((clsIssueItemDetailsVO)dgItemList.SelectedItem).MRP = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainMRP;
                    //(((clsIssueItemDetailsVO)grdStoreIndent.SelectedItem).Rate = ((clsIssueItemDetailsVO)dgItemList.SelectedItem).MainRate;
                }
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();
            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgReturnItemList.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP =   ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).MainMRP;
                    //objConversionVO.MainRate =   ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ReceivedQty);

                    long BaseUOMID = ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).MRP = objConversionVO.MRP;


                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseQuantity = objConversionVO.Quantity;
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    decimal? _temp = Convert.ToDecimal(((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).BaseQuantity);
                    ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ItemTotalAmount = _temp * ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).PurchaseRate;

                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgReturnItemList.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).ItemId), ((clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnCFSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnCFSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;
            clsReceivedItemAgainstReturnDetailsVO objVO = new clsReceivedItemAgainstReturnDetailsVO();
            objVO = (clsReceivedItemAgainstReturnDetailsVO)dgReturnItemList.SelectedItem;

            if (objVO != null)
            {
                objVO.SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
                objVO.UOMList = Itemswin.UOMConvertLIst;
                objVO.UOMConversionList = Itemswin.UOMConversionLIst;

                CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, objVO.SUOMID);

                if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                {
                    objVO.StockingQuantity = Convert.ToDouble(objVO.ReceivedQty * Convert.ToDecimal(objVO.ConversionFactor));
                    objVO.BaseQuantity = Convert.ToSingle(objVO.ReceivedQty) * objVO.BaseConversionFactor;
                    objVO.ItemTotalAmount = Convert.ToDecimal(objVO.BaseQuantity) * objVO.PurchaseRate;


                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.ReceivedQty % 1) != 0)
                    {
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;
                        string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }
                    else if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.BalanceQty)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Greater Received Quantity!", "Received Quantity In The List Can't Be Greater than Pending Quantity. Please Enter Received Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }
                    else if (Convert.ToDecimal(objVO.BaseQuantity) > (objVO.ReceivedQty * Convert.ToDecimal(objVO.BaseConversionFactor)))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Received Quantity In The List Can't Be Greater Than Returned Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReceivedQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }

                }
                else
                {
                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(objVO.SelectedUOM.ID, objVO.SUOMID);
                }
            }
            this.SumOfTotals();
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
