using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Windows.Browser;
using PalashDynamics.Pharmacy.ItemSearch;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;

namespace PalashDynamics.Pharmacy
{
    public partial class ReturnItem : UserControl
    {
        #region Variable Declarations
        private SwivelAnimation objAnimation;
        #region 'Paging'

        public PagedSortableCollectionView<clsReturnListVO> ReturnDataList { get; private set; }

        public int DataListPageSize
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


        public PagedSortableCollectionView<clsIssueListVO> IssueDataList { get; private set; }

        public int IssueDataListPageSize
        {
            get
            {
                return IssueDataList.PageSize;
            }
            set
            {
                if (value == IssueDataList.PageSize) return;
                IssueDataList.PageSize = value;

            }
        }

        #endregion
        bool flagResetControls = false;
        void IssueDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            // FillIssueList();
        }

        void ReturnDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillReturnList();
        }

        WaitIndicator Indicatior;

        int ClickedFlag1 = 0;

        private ObservableCollection<clsReturnItemDetailsVO> _ocReturnItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
        public ObservableCollection<clsReturnItemDetailsVO> ocReturnItemDetailsList
        {
            get
            {
                return _ocReturnItemDetailsList;
            }
            set
            {
                _ocReturnItemDetailsList = value;
            }
        }

        decimal retQty;

        bool flagZeroTransit = false;

        private clsIssueListVO _SelectedIssue = new clsIssueListVO();
        public clsIssueListVO SelectedIssue
        {
            get
            {
                return _SelectedIssue;
            }
            set
            {
                _SelectedIssue = value;
            }
        }


        bool IsAgainstPatient { get; set; }
        #endregion

        #region Constructor and Loaded Event
        public ReturnItem()
        {
            Indicatior = new WaitIndicator();
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            ReturnDataList = new PagedSortableCollectionView<clsReturnListVO>();
            ReturnDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ReturnDataList_OnRefresh);
            DataListPageSize = 5;
            ReturnDataList.PageSize = DataListPageSize;
            dgdpReturnList.Source = ReturnDataList;

            //Paging Issue list
            IssueDataList = new PagedSortableCollectionView<clsIssueListVO>();
            IssueDataList.OnRefresh += new EventHandler<RefreshEventArgs>(IssueDataList_OnRefresh);
            IssueDataListPageSize = 15;
            //dgdpIssueList.PageSize = IssueDataListPageSize;
            //dgdpIssueList.Source = IssueDataList;

            dgIssueItemList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgIssueItemList_CellEditEnded);
            this.Loaded += new RoutedEventHandler(ReturnItem_Loaded);
        }

        void ReturnItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgIssueItemList.ItemsSource == null)
            {
                dgIssueItemList.ItemsSource = this.ocReturnItemDetailsList;
                flagResetControls = false;
            }


            dpReturnDate.SelectedDate = DateTime.Now;
            dpReturnFromDate.SelectedDate = DateTime.Now;
            dpReturnToDate.SelectedDate = DateTime.Now;
            dpReturnDate.IsEnabled = false;
            FillStore();
            FillReturnList();
            FillReturnByList();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Fill return by drop down.
        /// Added on 12 November 2013. 
        /// </summary>
        private void FillReturnByList()
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
                    cmbReturnBy.ItemsSource = null;
                    cmbReturnBy.ItemsSource = objList;
                    cmbReturnBy.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

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
            BizActionObj.ToStoreList = new List<clsStoreVO>();

            clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
            BizActionObj.ItemMatserDetails.Insert(0, Default);
            BizActionObj.ToStoreList.Insert(0, Default);

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);



            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                 select item;

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.Status == true
                                  select item;

                    var NonQSAndUserAssignedStores = from item in BizActionObj.ToStoreList.ToList()
                                                     where item.IsQuarantineStore == false
                                                     select item;

                    NonQSAndUserAssignedStores.ToList().Insert(0, Default);

                    cmbFromStore.ItemsSource = NonQSAndUserAssignedStores.ToList(); //(List<clsStoreVO>)result.ToList();
                    cmbFromStore.SelectedItem = NonQSAndUserAssignedStores.ToList()[0]; //result.ToList()[0];

                    cmbReturnFromStoreSrch.ItemsSource = NonQSAndUserAssignedStores.ToList(); //(List<clsStoreVO>)result.ToList();
                    cmbReturnFromStoreSrch.SelectedItem = NonQSAndUserAssignedStores.ToList()[0]; //result.ToList()[0];

                    //cmbToStore.ItemsSource = (List<clsStoreVO>)result.ToList(); //result1.ToList();
                    //cmbToStore.SelectedItem = result.ToList()[0];//result1.ToList()[0];

                    cmbToStore.ItemsSource = result1.ToList();
                    cmbToStore.SelectedItem = result1.ToList()[0];


                    //cmbReturnToStoreSrch.ItemsSource = (List<clsStoreVO>)result.ToList(); //result1.ToList();
                    //cmbReturnToStoreSrch.SelectedItem = result.ToList()[0];//result1.ToList()[0];

                    cmbReturnToStoreSrch.ItemsSource = result1.ToList();
                    cmbReturnToStoreSrch.SelectedItem = result1.ToList()[0];

                }

            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void ResetControls()
        {
            dpReturnDate.SelectedDate = DateTime.Now;

            txtReturnNumber.Text = String.Empty;
            txtIssueNumber.Text = String.Empty;
            //dgIssueItemList.ItemsSource = null;
            this.ocReturnItemDetailsList.Clear();
            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            // txtTotalVAT.Text = String.Empty;
            txtRemark.Text = String.Empty;

            

        }

        private void FillReturnList()
        {

            try
            {
                Indicatior.Show();
                dgReturnList.ItemsSource = null;
                dgReturnItemList.ItemsSource = null;
                clsGetReturnListBizActionVO BizAction = new clsGetReturnListBizActionVO();
                BizAction.ReturnList = new List<clsReturnListVO>();
                BizAction.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID; //added By Ashish Z. on dated 13Apr16
                BizAction.ReturnNumberSrc = txtRetunNumberSrc.Text.Trim();

                BizAction.ReturnFromDate = dpReturnFromDate.SelectedDate;
                if (dpReturnToDate.SelectedDate == null)
                    BizAction.ReturnToDate = dpReturnToDate.SelectedDate;
                else
                    BizAction.ReturnToDate = dpReturnToDate.SelectedDate.Value.AddDays(1);


                if (cmbReturnFromStoreSrch.SelectedItem != null)
                {
                    BizAction.ReturnFromStoreId = ((clsStoreVO)cmbReturnFromStoreSrch.SelectedItem).StoreId;
                    if (BizAction.ReturnFromStoreId == 0)
                        BizAction.ReturnFromStoreId = null;
                }

                if (cmbReturnToStoreSrch.SelectedItem != null)
                {
                    BizAction.ReturnToStoreId = ((clsStoreVO)cmbReturnToStoreSrch.SelectedItem).StoreId;
                    if (BizAction.ReturnToStoreId == 0)
                        BizAction.ReturnToStoreId = null;
                }

                //change by harish 17 apr
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId; //0;
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
                        BizAction = ((clsGetReturnListBizActionVO)e.Result);


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
                        txtTotalCountRecords.Text = ReturnDataList.TotalItemCount.ToString();

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

        private void SumOfTotals()
        {

            var results = from r in this.ocReturnItemDetailsList
                          select r;

            //foreach (clsReturnItemDetailsVO item in results)
            //{

            //}


            decimal? TotalReturnItem = results.Sum(cnt => cnt.ReturnQty);
            //txtNoOfItems.Text = TotalReturnItem.ToString();
            txtNoOfItems.Text = String.Format("{0:0.00}", TotalReturnItem);

            decimal? TotalVATAmount = results.Sum(cnt => cnt.ItemVATAmount);
            //txtTotalVAT.Text = TotalVATAmount.ToString();

            decimal? TotalAmount = results.Sum(cnt => cnt.ItemTotalAmount);
            //txtTotalAmount.Text = TotalAmount.ToString();
            txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);


        }

        private void FillItemListByReturnId()
        {
            dgReturnItemList.ItemsSource = null;
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
                        BizAction.ItemList = ((clsGetItemListByReturnIdBizActionVO)e.Result).ItemList;
                        dgReturnItemList.ItemsSource = null;
                        dgReturnItemList.ItemsSource = BizAction.ItemList;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            else
            {
                dgReturnItemList.ItemsSource = null;
            }
        }
        #endregion

        #region Clicked Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            //cmbFromStore.SelectedItem = null;
            //cmbToStore.SelectedItem = null;
            rdbIsIssue.IsChecked = true;
            cmbFromStore.SelectedValue = (long)0;
            cmbToStore.SelectedValue = (long)0;

            txtPatientDetails.Text = string.Empty;
            this.IsAgainstPatient = false;
            ((IApplicationConfiguration)App.Current).SelectedPatient = new ValueObjects.Patient.clsPatientGeneralVO();

            objAnimation.Invoke(RotationType.Forward);
        }

        private void cmdSearchReturn_Click(object sender, RoutedEventArgs e)
        {
            FillReturnList();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;

            //Indicatior.Show();

            try
            {
                if (ClickedFlag1 == 1)
                {
                    Boolean IsValidationComplete = true;

                    if (dpReturnDate.SelectedDate == null)
                    {
                        dpReturnDate.SetValidation("Return Date can not be blank.");
                        dpReturnDate.RaiseValidationError();
                        dpReturnDate.Focus();
                        IsValidationComplete = false;
                        //Indicatior.Close();
                        ClickedFlag1 = 0;
                        return;

                    }

                    if (dpReturnDate.SelectedDate != null)
                    {
                        if (dpReturnDate.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            dpReturnDate.SetValidation("Return Date can not be less than Today's Date");
                            dpReturnDate.RaiseValidationError();
                            dpReturnDate.Focus();
                            IsValidationComplete = false;
                            //Indicatior.Close();
                            ClickedFlag1 = 0;
                            return;
                        }
                        if (dpReturnDate.SelectedDate.Value.Date > DateTime.Now.Date)
                        {
                            dpReturnDate.SetValidation("Return Date can not be greater than Today's Date");
                            dpReturnDate.RaiseValidationError();
                            dpReturnDate.Focus();
                            IsValidationComplete = false;
                            //Indicatior.Close();
                            ClickedFlag1 = 0;
                            return;
                        }
                    }

                    if (cmbFromStore.SelectedItem == null || ((clsStoreVO)cmbFromStore.SelectedItem).StoreId == 0)
                    {
                        cmbFromStore.SetValidation("Return From Store can not be blank.");
                        cmbFromStore.RaiseValidationError();
                        cmbFromStore.Focus();
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Return From Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        IsValidationComplete = false;
                        //Indicatior.Close();
                        ClickedFlag1 = 0;
                        return;
                    }
                    if (cmbToStore.SelectedItem == null || ((clsStoreVO)cmbToStore.SelectedItem).StoreId == 0)
                    {
                        cmbToStore.SetValidation("Return To Store can not be blank.");
                        cmbToStore.RaiseValidationError();
                        cmbToStore.Focus();
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Return To Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        IsValidationComplete = false;
                        //Indicatior.Close();
                        ClickedFlag1 = 0;
                        return;
                    }

                    if (((clsStoreVO)cmbFromStore.SelectedItem).StoreId == ((clsStoreVO)cmbToStore.SelectedItem).StoreId)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issue from store and issue to store cannot be same. Please select different stores .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                        IsValidationComplete = false;
                        ///Indicatior.Close();
                        ClickedFlag1 = 0;
                        return;
                    }
                    if (this.ocReturnItemDetailsList != null)
                    {

                        if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>().Count <= 0)
                        {
                            IsValidationComplete = false;
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Select Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                            IsValidationComplete = false;
                            //Indicatior.Close();
                            ClickedFlag1 = 0;
                            return;

                        }
                        else
                        {
                            float fltOne = 1;
                            float fltZero = 0;
                            float Infinity = fltOne / fltZero;
                            float NaN = fltZero / fltZero;
                            for (int i = 0; i < this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>().Count; i++)
                            {
                                if (rdbIsIssue.IsChecked == true)
                                {
                                    if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ConversionFactor <= 0 || this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ConversionFactor == Infinity || this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ConversionFactor == NaN)
                                    {
                                        IsValidationComplete = false;
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        mgbx.Show();
                                        break;
                                    }
                                    else if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BaseConversionFactor <= 0 || this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BaseConversionFactor == Infinity || this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BaseConversionFactor == NaN)
                                    {
                                        IsValidationComplete = false;
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        mgbx.Show();
                                        break;
                                    }

                                    if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty < 0)
                                    {
                                        IsValidationComplete = false;
                                        this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty = 0;
                                        this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ItemTotalAmount = 0;
                                        string msgText = "Please Enter Quantity Greater Than Zero.";
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWD.Show();
                                        break;
                                    }

                                    if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BalanceQty != 0)
                                    {

                                        if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].SelectedUOM != null && this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].SelectedUOM.ID == this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BaseUOMID && (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty % 1) != 0)
                                        {
                                            IsValidationComplete = false;
                                            this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty = 0;
                                            string msgText = "Return Quantity Cannot Be In Fraction for Base UOM";
                                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgWD.Show();
                                            break;
                                        }
                                        else if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty == 0)
                                        {
                                            IsValidationComplete = false;
                                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Return Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgWin.Show();
                                            IsValidationComplete = false;
                                            break;
                                        }
                                        else if (Convert.ToDouble(this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].StockingQuantity) > this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].AvailableStock)
                                        {
                                            IsValidationComplete = false;
                                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Return Quantity Less Than Or Equal To Avaialble Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgWin.Show();
                                            this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty = 0;
                                            this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ItemTotalAmount = 0;
                                            IsValidationComplete = false;
                                            break;
                                        }
                                        else if (Convert.ToDouble(this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BaseQuantity) > Convert.ToDouble(this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BalanceQty))  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                         new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Return Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty = 0;
                                            this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ItemTotalAmount = 0;
                                            IsValidationComplete = false;
                                            break;
                                        }

                                    }

                                }
                                else
                                    if (rdbIsWithoutIssue.IsChecked == true)
                                    {
                                        if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].ReturnQty == 0)
                                        {
                                            IsValidationComplete = false;
                                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Return Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgWin.Show();
                                            IsValidationComplete = false;
                                            break;
                                        }

                                    }

                            }
                            bool returnedFlag = false;
                            for (int i = 0; i < this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>().Count; i++)
                            {
                                if (rdbIsIssue.IsChecked == true)
                                {
                                    if (this.ocReturnItemDetailsList.ToList<clsReturnItemDetailsVO>()[i].BalanceQty != 0)
                                    {
                                        returnedFlag = false;
                                        break;
                                    }

                                    returnedFlag = true;
                                    //MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Unable To Save. Items Are Already Returned!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    //msgWin.Show();
                                    //IsValidationComplete = false;


                                }

                            }
                            if (returnedFlag)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Unable To Save. Items Are Already Returned!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                returnedFlag = false;
                                IsValidationComplete = false;

                            }

                        }
                    }

                    //var resultSelectedItem = from r in this.ocReturnItemDetailsList
                    //             where r.IsChecked == true
                    //             select r;
                    var resultSelectedItem = from r in this.ocReturnItemDetailsList
                                             where r.ReturnQty > 0
                                             select r;



                    if (IsValidationComplete == true && resultSelectedItem != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                Indicatior.Show();
                                clsAddReturnItemToStoreBizActionVO BizAction = new clsAddReturnItemToStoreBizActionVO();

                                BizAction.ReturnItemDetails = new clsReturnItemVO(); ;
                                BizAction.ReturnItemDetails.ReturnItemDetailsList = new List<clsReturnItemDetailsVO>();

                                if (SelectedIssue != null)
                                {
                                    BizAction.ReturnItemDetails.IssueID = SelectedIssue.IssueId;
                                    BizAction.ReturnItemDetails.IssueUnitID = SelectedIssue.IssueUnitID;
                                    BizAction.ReturnItemDetails.IsIndent = SelectedIssue.IsIndent;
                                }
                                BizAction.ReturnItemDetails.ReceivedID = resultSelectedItem.ToList<clsReturnItemDetailsVO>()[0].ReceivedID;

                                BizAction.ReturnItemDetails.ReturnDate = dpReturnDate.SelectedDate;
                                BizAction.ReturnItemDetails.ReturnFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                                BizAction.ReturnItemDetails.ReturnID = ((MasterListItem)cmbReturnBy.SelectedItem).ID;
                                BizAction.ReturnItemDetails.ReturnNumber = null;
                                BizAction.ReturnItemDetails.ReturnToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                                BizAction.ReturnItemDetails.Remark = txtRemark.Text.Trim();
                                BizAction.ReturnItemDetails.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text.Trim());
                                BizAction.ReturnItemDetails.TotalItems = Convert.ToDecimal(txtNoOfItems.Text.Trim());
                                BizAction.ReturnItemDetails.IsIssue = rdbIsIssue.IsChecked == true ? true : false;

                                if (IsAgainstPatient && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                                {
                                    BizAction.ReturnItemDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                                    BizAction.ReturnItemDetails.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                                    BizAction.ReturnItemDetails.IsAgainstPatient = this.IsAgainstPatient;
                                }
                                else
                                {
                                    BizAction.ReturnItemDetails.PatientID =0;
                                    BizAction.ReturnItemDetails.PatientUnitID = 0;
                                    BizAction.ReturnItemDetails.IsAgainstPatient = false;
                                }




                                BizAction.ReturnItemDetails.ReturnItemDetailsList = resultSelectedItem.ToList<clsReturnItemDetailsVO>();

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, e1) =>
                                {
                                    Indicatior.Close();
                                    ClickedFlag1 = 0;
                                    if (e1.Error == null && e1.Result != null && ((clsAddReturnItemToStoreBizActionVO)e1.Result).ReturnItemDetails != null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Return details saved successfully with return no. " + ((clsAddReturnItemToStoreBizActionVO)e1.Result).ReturnItemDetails.ReturnNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                        FillReturnList();
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", "Some error occurred while saving.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                    }

                                };
                                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                                objAnimation.Invoke(RotationType.Backward);
                                FillReturnList();
                            }
                            else
                                ClickedFlag1 = 0;
                        };
                        msgWindow.Show();
                    }
                    else
                    {
                        ClickedFlag1 = 0;
                        Indicatior.Close();
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
            //    this.ocReturnItemDetailsList.Add((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem);
            //}
            //else
            //{
            //    this.ocReturnItemDetailsList.Remove((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem);
            //}
        }

        private void rdbIsIssue_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            if (rdbIsIssue.IsChecked == true)
            {
                cmdGetIssue.IsEnabled = true;
                cmdAddItem.IsEnabled = false;
                dgIssueItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                //dgIssueItemList.Columns[6].Visibility = System.Windows.Visibility.Visible;

                cmdSearchPatient.IsEnabled = false;
                this.IsAgainstPatient = false;
            }
            else
            {
                cmdGetIssue.IsEnabled = false;
                cmdAddItem.IsEnabled = true;
            }
        }

        private void cmdGetIssue_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsAgainstPatient)
            {
                this._SelectedIssue = new clsIssueListVO();
                //this._ocReturnItemDetailsList.Clear();

                SearchIssue win = new SearchIssue();
                if (cmbToStore.SelectedItem != null)
                    win.IssueToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                if (cmbFromStore.SelectedItem != null)
                    win.IssueFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                win.OnItemSelectionCompleted += new SearchIssue.ItemSelection(win_OnItemSelectionCompleted);
                win.Show();
            }
            else
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Select Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWin.Show();
                }
                else
                {
                    this._SelectedIssue = new clsIssueListVO();
                    SearchIssue win = new SearchIssue();
                    if (cmbToStore.SelectedItem != null)
                        win.IssueToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                    if (cmbFromStore.SelectedItem != null)
                        win.IssueFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;

                    win.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    win.PatientunitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                    win.IsAgainstPatient = this.IsAgainstPatient;
                    win.OnItemSelectionCompleted += new SearchIssue.ItemSelection(win_OnItemSelectionCompleted);
                    win.Show();
                }
            }
        }

        private void cmdAddItem_Click(object sender, RoutedEventArgs e)
        {

            if (cmbFromStore.SelectedItem != null && ((clsStoreVO)cmbFromStore.SelectedItem).StoreId != 0)
            {
                ItemListNew win = new ItemListNew();
                win.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                win.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                win.ShowExpiredBatches = true;
                win.ClinicID = ((clsStoreVO)cmbFromStore.SelectedItem).ClinicId;
                win.cmbStore.IsEnabled = false;
                win.StoreID = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);

                win.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "From Store can not be Empty. Please Select From Store.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                mgbx.Show();
            }


        }

        void win_OnItemSelectionCompleted(object sender, EventArgs e)
        {
            if (this.SelectedIssue.IssueId != ((SearchIssue)sender).SelectedIssue.IssueId)
            {
                ResetControls();
                this.SelectedIssue = ((SearchIssue)sender).SelectedIssue;
                this._ocReturnItemDetailsList = ((SearchIssue)sender).ocSelectedItemDetailsList;
                txtIssueNumber.Text = this.SelectedIssue.IssueNumber;
                dgIssueItemList.ItemsSource = this.ocReturnItemDetailsList;
                foreach (clsReturnItemDetailsVO item in ((SearchIssue)sender).ocSelectedItemDetailsList)
                {
                    retQty = item.ReturnQty;

                    //if (item.IsIndent == 0)//item.IsIndent == false   //By anjali..........................
                    //{
                    //    item.AvailableStock = item.AvailableStock / item.ConversionFactor;
                    //    item.AvailableStock = Math.Floor(Convert.ToDouble(item.AvailableStock));
                    //}

                    if (item.BatchCode == null || item.BatchCode == "")
                    {
                        item.ExpiryDate = null;
                    }
                    item.IsChecked = true;
                }
            }
            else
            {
                foreach (clsReturnItemDetailsVO item in ((SearchIssue)sender).ocSelectedItemDetailsList)
                {
                    if (this._ocReturnItemDetailsList.Where(q => q.BatchId == item.BatchId).Count() == 0)
                    {
                        this._ocReturnItemDetailsList.Add(item);
                        retQty = item.ReturnQty;
                        item.IsChecked = true;
                    }
                }
            }

            flagResetControls = true;

            var results = from r in ((List<clsStoreVO>)cmbFromStore.ItemsSource)
                          where r.StoreId == this.SelectedIssue.IssueToStoreId
                          select r;

            foreach (clsStoreVO item in results)
            {
                cmbFromStore.SelectedItem = item;
                //  cmbFromStore.IsEnabled = false;

            }

            var results1 = from r in ((List<clsStoreVO>)cmbToStore.ItemsSource)
                           where r.StoreId == this.SelectedIssue.IssueFromStoreId
                           select r;

            foreach (clsStoreVO item in results1)
            {
                cmbToStore.SelectedItem = item;
                //     cmbToStore.IsEnabled = false;
            }

            flagResetControls = false;

            this.SumOfTotals();
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (dgIssueItemList.ItemsSource == null)
                dgIssueItemList.ItemsSource = this.ocReturnItemDetailsList;

            //foreach (clsItemStockVO item in (((ItemList)sender).SelectedBatches))
            //{
            //    //(List<clsItemMasterVO>(((ItemList)sender).SelectedItems).Where(q => q.ItemID == item.ItemID));

            //    //String ItemCode = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemCode;
            //    //String ItemName = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemName;

            //    //String ItemName = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;
            //    //String ItemCo = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;

            //    var result = from r in ((ItemList)sender).SelectedItems
            //                 where r.ID == item.ItemID
            //                 select r;

            //    String ItemCode = String.Empty, ItemName = String.Empty;
            //    bool BatchRequired = false;

            //    if (result.Count() > 0)
            //    {
            //        ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
            //        ItemName = ((clsItemMasterVO)result.First()).ItemName;
            //        BatchRequired = ((clsItemMasterVO)result.First()).BatchesRequired;
            //    }

            //    //clsIssueItemDetailsVO IssueItem = new clsIssueItemDetailsVO
            //    //{
            //    //    AvailableStock = Convert.ToDecimal(item.AvailableStock),
            //    //    BatchCode = item.BatchCode,
            //    //    BatchId = item.BatchID,
            //    //    ExpiryDate = item.ExpiryDate,
            //    //    ItemId = item.ItemID,
            //    //    MRP = Convert.ToDecimal(item.MRP),
            //    //    PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
            //    //    ItemName = ItemName,
            //    //    ItemCode = ItemCode
            //    //};
            //    clsReturnItemDetailsVO returnItem = new clsReturnItemDetailsVO
            //    {
            //        BatchCode = item.BatchCode,
            //        BatchId = item.BatchID,
            //        ExpiryDate = item.ExpiryDate,
            //        IsChecked = true,
            //        //IssueQty = 0,
            //        ItemCode = ItemCode,
            //        ItemId = item.ItemID,
            //        ItemName =ItemName,
            //        //ItemTotalAmount = 0,
            //        //ItemVATAmount = 0,
            //        //ItemVATPercentage = 0,

            //        MRP = Convert.ToDecimal(item.MRP),
            //        PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
            //        AvailableStock = item.AvailableStock

            //        //ReturnQty = 0

            //    };

            //    if (ocReturnItemDetailsList.Where(returnItems => returnItems.ItemId == item.ItemID).Any() == true)
            //    {
            //        if (ocReturnItemDetailsList.Where(returnItems => (returnItems.BatchId == item.BatchID)).Any() == false)
            //           this.ocReturnItemDetailsList.Add(returnItem);
            //    }
            //    else
            //        this.ocReturnItemDetailsList.Add(returnItem);

            //}

            foreach (var item in (((ItemListNew)sender).ItemBatchList))
            {
                //(List<clsItemMasterVO>(((ItemList)sender).SelectedItems).Where(q => q.ItemID == item.ItemID));

                //String ItemCode = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemCode;
                //String ItemName = ((clsItemMasterVO)((((ItemList)sender).SelectedItems).Where(y => y.ItemID == item.ItemID)).First()).ItemName;

                //String ItemName = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;
                //String ItemCo = ((clsItemMasterVO)((ItemList)sender).dgItemBatches.SelectedItem).ItemName;

                //var result = from r in ((ItemList)sender).SelectedItems
                //             where r.ID == item.ItemID
                //             select r;

                String ItemCode = String.Empty, ItemName = String.Empty;
                bool BatchRequired = false;

                //if (result.Count() > 0)
                //{
                //    ItemCode = ((clsItemMasterVO)result.First()).ItemCode;
                //    ItemName = ((clsItemMasterVO)result.First()).ItemName;
                //    BatchRequired = ((clsItemMasterVO)result.First()).BatchesRequired;
                //}

                ItemCode = item.ItemCode;
                ItemName = item.ItemName;
                BatchRequired = item.BatchesRequired;
                //clsIssueItemDetailsVO IssueItem = new clsIssueItemDetailsVO
                //{
                //    AvailableStock = Convert.ToDecimal(item.AvailableStock),
                //    BatchCode = item.BatchCode,
                //    BatchId = item.BatchID,
                //    ExpiryDate = item.ExpiryDate,
                //    ItemId = item.ItemID,
                //    MRP = Convert.ToDecimal(item.MRP),
                //    PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
                //    ItemName = ItemName,
                //    ItemCode = ItemCode
                //};
                clsReturnItemDetailsVO returnItem = new clsReturnItemDetailsVO
                {
                    BatchCode = item.BatchCode,
                    BatchId = item.BatchID,
                    ExpiryDate = item.ExpiryDate,
                    IsChecked = true,
                    //IssueQty = 0,
                    ItemCode = ItemCode,
                    ItemId = item.ItemID,
                    ItemName = ItemName,
                    //ItemTotalAmount = 0,
                    //ItemVATAmount = 0,
                    //ItemVATPercentage = 0,

                    MRP = Convert.ToDecimal(item.MRP),
                    PurchaseRate = Convert.ToDecimal(item.PurchaseRate),
                    AvailableStock = item.AvailableStock,
                    //ReturnQty = 0

                };

                if (ocReturnItemDetailsList.Where(returnItems => returnItems.ItemId == item.ItemID).Any() == true)
                {
                    if (ocReturnItemDetailsList.Where(returnItems => (returnItems.BatchId == item.BatchID)).Any() == false)
                        this.ocReturnItemDetailsList.Add(returnItem);
                }
                else
                    this.ocReturnItemDetailsList.Add(returnItem);

            }

        }

        private void rdbIsWithoutIssue_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();

            if (rdbIsWithoutIssue.IsChecked == true)
            {
                cmdGetIssue.IsEnabled = false;
                cmdAddItem.IsEnabled = true;
                dgIssueItemList.Columns[5].Visibility = System.Windows.Visibility.Collapsed;
                SelectedIssue = null;
                //dgIssueItemList.Columns[6].Visibility = System.Windows.Visibility.Collapsed;

                if (ocReturnItemDetailsList != null)
                {
                    ocReturnItemDetailsList.Clear();
                    ocReturnItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
                }

            }
            else
            {
                cmdAddItem.IsEnabled = true;
                cmdAddItem.IsEnabled = false;

                if (ocReturnItemDetailsList != null)
                {
                    ocReturnItemDetailsList.Clear();
                    ocReturnItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
                }
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgIssueItemList.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to delete the item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ocReturnItemDetailsList.RemoveAt(dgIssueItemList.SelectedIndex);
                        dgIssueItemList.Focus();
                        dgIssueItemList.UpdateLayout();
                        dgIssueItemList.SelectedIndex = ocReturnItemDetailsList.Count - 1;
                        SumOfTotals();

                    }
                };
                msgWD.Show();
            }
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgReturnList.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/ReturnItemPrint.aspx?ReturnID=" + ((clsReturnListVO)dgReturnList.SelectedItem).ReturnId + "&UnitID=" + (((clsReturnListVO)dgReturnList.SelectedItem).UnitId)), "_blank");
            }
        }
        #endregion

        #region CellEditEnded/SelectionChanged/KeyDown Events
        private void dgIssueItemList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgIssueItemList.SelectedItem != null)
            {
                clsReturnItemDetailsVO objVO = new clsReturnItemDetailsVO();
                objVO = (clsReturnItemDetailsVO)dgIssueItemList.SelectedItem;

                if (e.Column.Header.ToString().Equals("Return Qty") || e.Column.Header.ToString().Equals("Return UOM"))
                {
                    if (objVO.ReturnQty < 0)
                    {
                        objVO.ReturnQty = 0;
                        objVO.ItemTotalAmount = 0;
                        string msgText = "Please Enter Quantity Greater Than Zero.";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    objVO.ReturnQty = System.Math.Round(objVO.ReturnQty, 1);
                    if (((int)objVO.ReturnQty).ToString().Length > 5)
                    {
                        objVO.ReturnQty = 1;
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Return Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                        return;
                        //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                    }
                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                    {
                        objVO.StockingQuantity = Convert.ToDouble(objVO.ReturnQty * Convert.ToDecimal(objVO.ConversionFactor));
                        objVO.BaseQuantity = Convert.ToSingle(objVO.ReturnQty) * objVO.BaseConversionFactor;
                        objVO.ItemTotalAmount = Convert.ToDecimal(objVO.BaseQuantity) * objVO.PurchaseRate;

                        if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.ReturnQty % 1) != 0)
                        {
                            objVO.ReturnQty = 0;
                            objVO.ItemTotalAmount = 0;
                            string msgText = "Return Quantity Cannot Be In Fraction for Base UOM";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                            return;
                        }

                        if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.BalanceQty)  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Return Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objVO.ReturnQty = 0;
                            objVO.ItemTotalAmount = 0;
                            return;
                        }

                        if (objVO.StockingQuantity > objVO.AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                              new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater Than Available Stock. Please Enter Return Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objVO.ReturnQty = 0;
                            objVO.ItemTotalAmount = 0;
                            return;
                        }

                        if (this.IsAgainstPatient && (objVO.BaseQuantity > (objVO.TotalPatientIndentReceiveQty - objVO.TotalPatientIndentConsumptionQty)))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                              new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater Than Patient Remaining Qty! Available Qty is " + (objVO.TotalPatientIndentReceiveQty - objVO.TotalPatientIndentConsumptionQty), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            objVO.ReturnQty = 0;
                            objVO.ItemTotalAmount = 0;
                            return;
                        }

                    }
                    else
                    {
                        CalculateConversionFactorCentral(objVO.SelectedUOM.ID, objVO.SUOMID);
                    }
                }
                this.SumOfTotals();
            }

        }

        private void dgReturnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByReturnId();
        }

        private void cmbFromStore_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbFromStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FillIssueList();
            //ResetControls();
            if (!flagResetControls)
                ResetControls();

        }

        private void cmbToStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // FillIssueList();

            if (!flagResetControls)
                ResetControls();
        }

        private void dgIssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //FillItemListByIssueId();
        }

        #endregion

        #region Conversion Factor
        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList == null || (((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList.Count == 0)))
            {
                FillUOMConversions(cmbConversions);
            }
        }

        // Method To Fill Unit Of Mesurements with Conversion Factors for Selected Item
        private void FillUOMConversions(AutoCompleteBox cmbConversions)
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = Convert.ToInt64(((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ItemId);
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

                        if (dgIssueItemList.SelectedItem != null)
                        {
                            ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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
            if (dgIssueItemList.SelectedItem != null)
            {
                clsConversionsVO objConversion = new clsConversionsVO();
                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                //((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = 0;
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = 0;

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

                if (dgIssueItemList.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    //objConversionVO.MainMRP =   ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).MainMRP;
                    //objConversionVO.MainRate =   ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ReturnQty);

                    long BaseUOMID = ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    //((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).Rate = objConversionVO.Rate;
                    //((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).MRP = objConversionVO.MRP;


                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = objConversionVO.Quantity;
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = Convert.ToDecimal(((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) * ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).PurchaseRate;

                }
            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgIssueItemList.SelectedItem != null)
            {
                Conversion win = new Conversion();
                win.FillUOMConversions(Convert.ToInt64(((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).ItemId), ((clsReturnItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnCFSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnCFSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;
            clsReturnItemDetailsVO objVO = new clsReturnItemDetailsVO();
            objVO = (clsReturnItemDetailsVO)dgIssueItemList.SelectedItem;

            if (objVO != null)
            {
                objVO.SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
                objVO.UOMList = Itemswin.UOMConvertLIst;
                objVO.UOMConversionList = Itemswin.UOMConversionLIst;

                CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, objVO.SUOMID);

                if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID > 0)
                {
                    objVO.StockingQuantity = Convert.ToDouble(objVO.ReturnQty * Convert.ToDecimal(objVO.ConversionFactor));
                    objVO.BaseQuantity = Convert.ToSingle(objVO.ReturnQty) * objVO.BaseConversionFactor;

                    if (objVO.SelectedUOM != null && objVO.SelectedUOM.ID == objVO.BaseUOMID && (objVO.ReturnQty % 1) != 0)
                    {

                        string msgText = "Return Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        objVO.ReturnQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }

                    if (Convert.ToDecimal(objVO.BaseQuantity) > objVO.BalanceQty)  //((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssuePendingQuantity
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater than Pending Quantity. Please Enter Return Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReturnQty = 0;
                        objVO.ItemTotalAmount = 0;
                        return;
                    }

                    if (objVO.StockingQuantity > objVO.AvailableStock) // (((clsIssueItemDetailsVO)dgItemList.SelectedItem).IssueQty > ((clsIssueItemDetailsVO)dgItemList.SelectedItem).AvailableStock)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("Greater Return Quantity!", "Return Quantity In The List Can't Be Greater Than Available Stock. Please Enter Return Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        objVO.ReturnQty = 0;
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

        #region Return Against Patient

        private void cmdSearchPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new ValueObjects.Patient.clsPatientGeneralVO();
                }
                PatientSearch Win = new PatientSearch();
                Win.isfromCouterSale = true;
                Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
                Win.Show();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                txtPatientDetails.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
            }
        }

        private void rdbIsAgaimnstPatient_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            if (rdbIsAgaimnstPatient.IsChecked == true)
            {
                cmdSearchPatient.IsEnabled = true;
                cmdGetIssue.IsEnabled = true;
                cmdAddItem.IsEnabled = false;
                dgIssueItemList.Columns[5].Visibility = System.Windows.Visibility.Visible;
                IsAgainstPatient = true;

                if (ocReturnItemDetailsList != null)
                {
                    ocReturnItemDetailsList.Clear();
                    ocReturnItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
                }
            }
            else
            {
                cmdSearchPatient.IsEnabled = false;
                cmdGetIssue.IsEnabled = false;
                cmdAddItem.IsEnabled = true;
                IsAgainstPatient = false;

                if (ocReturnItemDetailsList != null)
                {
                    ocReturnItemDetailsList.Clear();
                    ocReturnItemDetailsList = new ObservableCollection<clsReturnItemDetailsVO>();
                }
            }
        }
        #endregion


    }
}
