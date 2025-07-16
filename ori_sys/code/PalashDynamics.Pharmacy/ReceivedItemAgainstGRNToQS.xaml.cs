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
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Collections.ObjectModel;
using PalashDynamics.Collections;
using System.ComponentModel;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Pharmacy.Inventory;
using OPDModule.Forms;

namespace PalashDynamics.Pharmacy
{
    public partial class ReceivedItemAgainstGRNToQS : UserControl
    {
        private SwivelAnimation objAnimation;
        bool IsUOMClick = false;

        #region 'Paging'

        public PagedSortableCollectionView<clsReceivedListVO> ReceivedDataList { get; private set; }

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

        public ReceivedItemAgainstGRNToQS()
        {
            Indicatior = new WaitIndicator();
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            //Paging
            ReceivedDataList = new PagedSortableCollectionView<clsReceivedListVO>();
            ReceivedDataList.OnRefresh += new EventHandler<RefreshEventArgs>(ReceivedDataList_OnRefresh);
            DataListPageSize = 15;
            ReceivedDataList.PageSize = DataListPageSize;
            dgdpReceivedList.Source = ReceivedDataList;

            //Paging Issue list
            IssueDataList = new PagedSortableCollectionView<clsIssueListVO>();
            IssueDataList.OnRefresh += new EventHandler<RefreshEventArgs>(IssueDataList_OnRefresh);
            IssueDataListPageSize = 15;
            dgdpIssueList.PageSize = IssueDataListPageSize;
            dgdpIssueList.Source = IssueDataList;

            dgIssueItemList.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgIssueItemList_CellEditEnded);

            this.Loaded += new RoutedEventHandler(ReceivedItem_Loaded);
        }

        void IssueDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillIssueList();
        }

        void ReceivedDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FillReceivedList();
        }

        void ReceivedItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgIssueItemList.ItemsSource == null)
                dgIssueItemList.ItemsSource = this.ocReceivedItemDetailsList;

            dpReceivedDate.SelectedDate = DateTime.Now;
            dpReceivedFromDate.SelectedDate = DateTime.Now;
            dpReceivedToDate.SelectedDate = DateTime.Now;
            dpReceivedDate.IsEnabled = false;
            FillStore();
            FillReceivedList();
            FillReceivedByList();
            this.DataContext = new clsReceivedItemVO();
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
            BizActionObj.StoreType = 2;
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


                    clsStoreVO additem = new clsStoreVO();
                    additem.StoreName = "--Select--";
                    additem.StoreId = 0;
                    additem.Status = true;
                    additem.ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    additem.IsQuarantineStore = false;
                    additem.Parent = 0;

                    BizActionObj.ItemMatserDetails.Insert(0, additem);
                    BizActionObj.ToStoreList.Insert(0, additem);

                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == true
                                 select item;

                    var result1 = from item in BizActionObj.ItemMatserDetails
                                  where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true && item.IsQuarantineStore == false
                                  select item;


                    List<clsStoreVO> Item = (List<clsStoreVO>)result.ToList();
                    Item.Insert(0, additem);

                    cmbFromStore.ItemsSource = Item.ToList(); //Item; //(List<clsStoreVO>)result.ToList();
                    cmbReceivedFromStoreSrch.ItemsSource = Item.ToList(); ; // //Item;// (List<clsStoreVO>)result.ToList();

                    cmbToStore.ItemsSource = result1.ToList();
                    cmbReceivedToStoreSrch.ItemsSource = result1.ToList();

                    cmbReceivedFromStoreSrch.SelectedItem = Item.ToList()[0];//(long)0;
                    cmbFromStore.SelectedItem = Item.ToList()[0];//(long)0;

                    cmbReceivedFromStoreSrch.SelectedItem = result1.ToList()[0];//(long)0;
                    cmbFromStore.SelectedItem = result1.ToList()[0];//(long)0;

                    cmbFromStore.SelectedValue = (long)0;
                    cmbReceivedFromStoreSrch.SelectedValue = (long)0;

                    cmbToStore.SelectedValue = (long)0;
                    cmbReceivedToStoreSrch.SelectedValue = (long)0;
                }

            };

            client.CloseAsync();

        }

        clsUserVO userVO;
        private void FillReceivedByList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    userVO = new clsUserVO();
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

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ResetControls();
            cmbFromStore.SelectedValue = (long)0;
            cmbToStore.SelectedValue = (long)0;
            cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            cmbReceivedBy.IsEnabled = false;
            objAnimation.Invoke(RotationType.Forward);
        }

        private void ResetControls()
        {
            dpReceivedDate.SelectedDate = DateTime.Now;

            txtReceivedNumber.Text = String.Empty;
            //dgIssueItemList.ItemsSource = null;
            this.ocReceivedItemDetailsList.Clear();
            txtNoOfItems.Text = String.Empty;
            txtTotalAmount.Text = String.Empty;
            // txtTotalVAT.Text = String.Empty;
            txtRemark.Text = String.Empty;

        }

        private void cmdSearchReceived_Click(object sender, RoutedEventArgs e)
        {
            FillReceivedList();
        }
        WaitIndicator Indicatior;

        private void FillReceivedList()
        {
            Indicatior.Show();
            try
            {
                dgReceivedList.ItemsSource = null;
                clsGetReceivedListBizActionVO BizAction = new clsGetReceivedListBizActionVO();
                BizAction.IsForReceiveGRNToQS = true;  //Use to get already saved Received Items at Quarantine Stores Only

                BizAction.ReceivedList = new List<clsReceivedListVO>();
                BizAction.UserId = (((IApplicationConfiguration)App.Current).CurrentUser).ID;

                BizAction.ReceivedNumberSrc = txtRetunNumberSrc.Text.Trim();

                BizAction.ReceivedFromDate = dpReceivedFromDate.SelectedDate;
                if (dpReceivedToDate.SelectedDate == null)

                    BizAction.ReceivedToDate = dpReceivedToDate.SelectedDate;
                else
                    BizAction.ReceivedToDate = dpReceivedToDate.SelectedDate.Value.AddDays(1);

                if (cmbReceivedFromStoreSrch.SelectedItem != null)
                {
                    BizAction.ReceivedFromStoreId = ((clsStoreVO)cmbReceivedFromStoreSrch.SelectedItem).StoreId;
                    if (BizAction.ReceivedFromStoreId == 0)
                        BizAction.ReceivedFromStoreId = null;
                }

                if (cmbReceivedToStoreSrch.SelectedItem != null)
                {
                    BizAction.ReceivedToStoreId = ((clsStoreVO)cmbReceivedToStoreSrch.SelectedItem).StoreId;
                    if (BizAction.ReceivedToStoreId == 0)
                        BizAction.ReceivedToStoreId = null;
                }

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
                    Indicatior.Close();
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = ((clsGetReceivedListBizActionVO)e.Result);
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

        private void dgReceivedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillItemListByReceivedId();
        }

        private void FillItemListByReceivedId()
        {
            if (dgReceivedList.SelectedItem != null)
            {
                clsGetItemListByIssueReceivedIdBizActionVO BizAction = new clsGetItemListByIssueReceivedIdBizActionVO();

                BizAction.ReceivedId = ((clsReceivedListVO)dgReceivedList.SelectedItem).ReceivedId;
                BizAction.UnitID = ((clsReceivedListVO)dgReceivedList.SelectedItem).UnitID;

                BizAction.ItemList = new List<clsReceivedItemDetailsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ItemList = ((clsGetItemListByIssueReceivedIdBizActionVO)e.Result).ItemList;
                        BizAction.ItemList.ForEach(z => z.PurchaseRate = (z.PurchaseRate * Convert.ToDecimal(z.BaseConversionFactor)));
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
        int ClickedFlag1 = 0;

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
                cmbToStore.TextBox.SetValidation("Issued From Store can not be blank.");
                cmbToStore.TextBox.RaiseValidationError();
                cmbToStore.Focus();
                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Issued From Store can not be blank.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWin.Show();
                IsValidationComplete = false;
                Indicatior.Close();
                ClickedFlag1 = 0;
                return IsValidationComplete;
            }

            if (this.ocReceivedItemDetailsList != null)
            {
                if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>().Count <= 0)
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
                    for (int i = 0; i < this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>().Count; i++)
                    {

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ReceivedQty <= 0)
                        {
                            IsValidationComplete = false;
                            this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ReceivedQty = 0;
                            string msgText = "Please Enter Quantity Greater Than Zero for Item " + this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ItemName;
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                            break;
                        }

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].BalanceQty != 0)
                        {
                            if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ReceivedQty == 0 && !this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].IsItemBlock)
                            {
                                IsValidationComplete = false;
                                MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Receive Quantity For Selected Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgWin.Show();
                                break;
                            }
                        }

                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].SelectedUOM != null && this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].SelectedUOM.ID == this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].BaseUOMID && (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ReceivedQty % 1) != 0)
                        {
                            IsValidationComplete = false;
                            this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].ReceivedQty = 0;
                            string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                            break;
                        }

                    }

                    bool flagReceivedItem = false;
                    for (int i = 0; i < this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>().Count; i++)
                    {
                        if (this.ocReceivedItemDetailsList.ToList<clsReceivedItemDetailsVO>()[i].BalanceQty != 0)
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

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                if (ClickedFlag1 == 1)
                {
                    Boolean IsValidation = CheckValidation();

                    var resultSelectedItem = from r in this.ocReceivedItemDetailsList
                                             where r.ReceivedQty > 0 && r.IsChecked == true
                                             select r;

                    if (IsValidation == true && dgIssueList.ItemsSource != null && resultSelectedItem != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("PALASH", "Do You Want To Save the Record?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWindow.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                if (Indicatior == null) Indicatior = new WaitIndicator();
                                Indicatior.Show();
                                clsAddReceivedItemBizActionVO BizAction = new clsAddReceivedItemBizActionVO();

                                BizAction.ReceivedItemDetails = new clsReceivedItemVO(); ;
                                BizAction.ReceivedItemDetails.ReceivedItemDetailsList = new List<clsReceivedItemDetailsVO>();

                                BizAction.ReceivedItemDetails.IssueID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                                BizAction.ReceivedItemDetails.IsIndent = ((clsIssueListVO)dgIssueList.SelectedItem).IsIndent;
                                BizAction.ReceivedItemDetails.ReceivedDate = dpReceivedDate.SelectedDate;
                                BizAction.ReceivedItemDetails.ReceivedFromStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                                BizAction.ReceivedItemDetails.ReceivedID = 0;
                                BizAction.ReceivedItemDetails.ReceivedNumber = null;
                                BizAction.ReceivedItemDetails.ReceivedToStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;


                                BizAction.ReceivedItemDetails.Remark = txtRemark.Text.Trim();

                                if (cmbReceivedBy.SelectedItem != null)
                                    BizAction.ReceivedItemDetails.ReceivedById = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;

                                BizAction.ReceivedItemDetails.TotalTaxAmount = (this.DataContext as clsReceivedItemVO).TotalTaxAmount;
                                BizAction.ReceivedItemDetails.TotalVATAmount = (this.DataContext as clsReceivedItemVO).TotalVATAmount;
                                BizAction.ReceivedItemDetails.TotalAmount = (this.DataContext as clsReceivedItemVO).TotalAmount;
                                //BizAction.ReceivedItemDetails.TotalItems = Convert.ToDecimal(txtNoOfItems.Text.Trim());
                                BizAction.ReceivedItemDetails.IssueUnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;

                                BizAction.ReceivedItemDetails.ReceivedItemDetailsList = resultSelectedItem.ToList<clsReceivedItemDetailsVO>();
                                BizAction.ReceivedItemDetails.ReceivedItemDetailsList.ForEach(z => z.IsIndent = BizAction.ReceivedItemDetails.IsIndent);

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client.ProcessCompleted += (s, e1) =>
                                {
                                    Indicatior.Close();
                                    ClickedFlag1 = 0;
                                    if (((clsAddReceivedItemBizActionVO)e1.Result).SuccessStatus == 10)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Pending Quantity is Less than Recived Qty for Item " + ((clsAddReceivedItemBizActionVO)e1.Result).ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                    }
                                    else if (e1.Error == null && e1.Result != null && ((clsAddReceivedItemBizActionVO)e1.Result).ReceivedItemDetails != null)
                                    {
                                        FillReceivedList();
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Received details saved successfully with receive No ." + ((clsAddReceivedItemBizActionVO)e1.Result).ReceivedItemDetails.ReceivedNumber, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                    }
                                    else
                                    {
                                        //Indicatior.Close();
                                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("", "Some error occurred while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWin.Show();
                                    }
                                };
                                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                                objAnimation.Invoke(RotationType.Backward);
                            }
                        };
                        msgWindow.Show();
                        ClickedFlag1 = 0;
                        //Indicatior.Close();
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

        private void cmbFromStore_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbFromStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillIssueList();
        }

        private void cmbToStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillIssueList();
        }

        private void dgIssueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsUOMClick = false;
            FillItemListByIssueId();
        }

        private void FillIssueList()
        {

            Indicatior.Show();
            try
            {
                clsGetIssueListBizActionVO BizAction = new clsGetIssueListBizActionVO();
                BizAction.IssueList = new List<clsIssueListVO>();
                BizAction.IsForGRNQS = true;  // for getting the Issues
                BizAction.IssueFromDate = null;
                BizAction.IssueToDate = null;
                if (cmbToStore.SelectedItem != null)
                    BizAction.IssueFromStoreId = ((clsStoreVO)cmbToStore.SelectedItem).StoreId;
                else
                    BizAction.IssueFromStoreId = 0;

                if (cmbFromStore.SelectedItem != null)
                    BizAction.IssueToStoreId = ((clsStoreVO)cmbFromStore.SelectedItem).StoreId;
                else
                    BizAction.IssueToStoreId = 0;
                #region Paging
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = IssueDataList.PageIndex * IssueDataList.PageSize + 1;
                BizAction.MaximumRows = IssueDataList.PageSize;

                foreach (SortDescription sortDesc in IssueDataList.SortDescriptions)
                {
                    BizAction.InputSortExpression = sortDesc.PropertyName + (sortDesc.Direction == ListSortDirection.Ascending ? " ASC" : " DESC");
                    break;
                }
                #endregion

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    Indicatior.Close();
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction = (clsGetIssueListBizActionVO)e.Result;

                        BizAction.IssueList = ((clsGetIssueListBizActionVO)e.Result).IssueList;
                        IssueDataList.TotalItemCount = BizAction.TotalRows;

                        IssueDataList.Clear();
                        foreach (var item in BizAction.IssueList)
                        {

                            IssueDataList.Add(item);
                        }

                        dgIssueList.ItemsSource = null;
                        dgIssueList.ItemsSource = IssueDataList;

                        dgdpIssueList.Source = null;
                        dgdpIssueList.PageSize = BizAction.MaximumRows;
                        dgdpIssueList.Source = IssueDataList;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                BizAction.flagReceiveIssue = false;
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }


        }

        decimal rcvQty = 0;

        private void FillItemListByIssueId()
        {
            if (dgIssueList.SelectedItem != null)
            {
                clsGetItemListByIssueIdBizActionVO BizAction = new clsGetItemListByIssueIdBizActionVO();

                BizAction.IssueId = ((clsIssueListVO)dgIssueList.SelectedItem).IssueId;
                BizAction.ReceivedItemList = new List<clsReceivedItemDetailsVO>();
                BizAction.UnitID = ((clsIssueListVO)dgIssueList.SelectedItem).IssueUnitID;
                // BizAction.flagReceivedIssue = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        BizAction.ReceivedItemList = ((clsGetItemListByIssueIdBizActionVO)e.Result).ReceivedItemList;
                        // this.ocReceivedItemDetailsList = BizAction.ReceivedItemList;
                        //  dgIssueItemList.ItemsSource = null;
                        this.ocReceivedItemDetailsList.Clear();
                        foreach (clsReceivedItemDetailsVO item in BizAction.ReceivedItemList)
                        {
                            if (item.BalanceQty > 0) //&& !item.IsItemBlock
                            {
                                item.ReceivedQty = item.IssueQty;
                                item.StockingQuantity = Convert.ToDouble(item.ReceivedQty) * Convert.ToSingle(item.ConversionFactor);
                                item.BaseQuantity = Convert.ToSingle(item.ReceivedQty) * item.BaseConversionFactor;

                                item.MainMRP = Convert.ToSingle(item.MRP);
                                item.MainRate = Convert.ToSingle(item.PurchaseRate);
                                item.MRP = Convert.ToDecimal(item.MainMRP * item.BaseConversionFactor);
                                item.PurchaseRate = Convert.ToDecimal(item.MainRate * item.BaseConversionFactor);

                                item.ItemTotalAmount = item.BalanceQty * Convert.ToDecimal(item.MainRate);
                                if (item.BatchCode == null || item.BatchCode == "")
                                {
                                    item.ExpiryDate = null;
                                }
                                item.SelectedUOM = new MasterListItem { ID = item.UOMID, Description = item.IssuedUOM };
                                item.IsChecked = true;
                                this.ocReceivedItemDetailsList.Add(item);
                                rcvQty = item.ReceivedQty;
                            }
                        }
                        SumOfTotals();
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                BizAction.flagReceivedIssue = false;
            }
            else
            {
                //dgIssueItemList.ItemsSource = null;
                this.ocReceivedItemDetailsList.Clear();
            }
        }

        private ObservableCollection<clsReceivedItemDetailsVO> _ocReceivedItemDetailsList = new ObservableCollection<clsReceivedItemDetailsVO>();
        public ObservableCollection<clsReceivedItemDetailsVO> ocReceivedItemDetailsList
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

        private void SumOfTotals()
        {
            decimal? ItemTaxAmount, ItemVATAmount, NetAmount;

            ItemTaxAmount = ItemVATAmount = NetAmount = 0;

            foreach (var item in this.ocReceivedItemDetailsList.ToList())
            {
                ItemTaxAmount += Math.Round(Convert.ToDecimal(item.ItemTaxAmount), 2);
                ItemVATAmount += Math.Round(Convert.ToDecimal(item.ItemVATAmount), 2);
                NetAmount += Math.Round((decimal)item.NetAmount, 2);
            }

            ((clsReceivedItemVO)this.DataContext).TotalTaxAmount = ItemTaxAmount;
            ((clsReceivedItemVO)this.DataContext).TotalVATAmount = ItemVATAmount;
            ((clsReceivedItemVO)this.DataContext).TotalAmount = NetAmount;

            txtTotalAmount.Text = String.Format("{0:0.00}", NetAmount);

            //var results = from r in this.ocReceivedItemDetailsList
            //              select r;


            //decimal? TotalReceivedItem = results.Sum(cnt => cnt.ReceivedQty);
            ////txtNoOfItems.Text = TotalReceivedItem.ToString();
            //txtNoOfItems.Text = String.Format("{0:0.00}", TotalReceivedItem);

            //decimal? TotalAmount = results.Sum(cnt => cnt.ItemTotalAmount);
            ////txtTotalAmount.Text = TotalAmount.ToString();
            //txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);
            //decimal? TotalVATAmount = results.Sum(cnt => cnt.ItemVATAmount);
            ////txtTotalVAT.Text = TotalVATAmount.ToString();
        }

        bool flagZeroTransit = false;
        private void dgIssueItemList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.Header.ToString().Equals("Received Quantity") || e.Column.Header.ToString().Equals("Received UOM"))
            {
                if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty <= 0)
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                    //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                }
                else if (Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor > Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty))
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Received Quantity is equal to Issued Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                }
                else if (Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor < Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty))
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Received Quantity is equal to Issued Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                }


                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = System.Math.Round(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty, 1);
                if (((int)((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty).ToString().Length > 5)
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 1;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = 0;
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Received Quantity Length Should Not Be Greater Than 5 Digits.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbx.Show();
                    return;
                    //throw new ValidationException("Issued Quantity Length Should Not Be Greater Than 5 Digits.");
                }

                if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID > 0)
                {
                    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID == ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseUOMID && (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty % 1) != 0)
                    {
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = 0;
                        string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).StockingQuantity = Convert.ToDouble(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).MainRate);

                    if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Greater Receive Quantity!", "Received Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Received Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = 0;

                        return;
                    }

                    if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty * Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQtyBaseCF)))
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Received Quantity In The List Can't Be Greater Than Issued Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = 0;
                        return;
                    }
                }
                else
                {
                    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID == ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseUOMID && (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty % 1) != 0)
                    {
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                        string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        return;
                    }

                    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID > 0)
                    {
                        // Function Parameters
                        // FromUOMID - Transaction UOM
                        // ToUOMID - Stocking UOM
                        CalculateConversionFactorCentral(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID, ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SUOMID);
                    }
                    else
                    {
                        //((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = 0;

                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = 0;
                        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = 0;
                    }


                    if ((((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty) == 0)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                               new MessageBoxControl.MessageBoxChildWindow("Can't Enter Receive Quantity!", "Issue is already received", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) = 0;
                        flagZeroTransit = true;
                        return;

                    }

                    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity > 0)
                    {
                        if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Greater Receive Quantity!", "Receive Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Receive Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty;
                            return;
                        }

                        if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty * Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQtyBaseCF)))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Receive Quantity In The List Can't Be Greater Than Issue Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty;
                            return;
                        }
                    }
                }
            }
            SumOfTotals();
        }

        private void chkItemSelect_Click(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == true)
            //{
            //    this.ocReceivedItemDetailsList.Add((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem);
            //}
            //else
            //{
            //    this.ocReceivedItemDetailsList.Remove((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem);
            //}
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
                        ocReceivedItemDetailsList.RemoveAt(dgIssueItemList.SelectedIndex);
                        dgIssueItemList.Focus();
                        dgIssueItemList.UpdateLayout();
                        dgIssueItemList.SelectedIndex = ocReceivedItemDetailsList.Count - 1;
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
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/ReceiveItemAgainstIssuePrint.aspx?ReceivedId=" + ((clsReceivedListVO)dgReceivedList.SelectedItem).ReceivedId + "&UnitID=" + (((clsReceivedListVO)dgReceivedList.SelectedItem).UnitID)), "_blank");
            }
        }

        private void cmbUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList == null || (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList.Count == 0)))
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

                BizAction.ItemID = ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemId;
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
                            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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

                //((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = 0;

                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SUOMID);
                }
                else
                {
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = 0;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = 0;

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
                    UOMConvertLIst = ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList;

                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty);

                    long BaseUOMID = ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).PurchaseRate = Convert.ToDecimal(objConversionVO.Rate);
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).MRP = Convert.ToDecimal(objConversionVO.MRP);


                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = objConversionVO.Quantity;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).StockingQuantity = objConversionVO.Quantity;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;

                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).MainRate);

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
                win.FillUOMConversions(Convert.ToInt64(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemId), ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            IsUOMClick = true;
            Conversion Itemswin = (Conversion)sender;

            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SUOMID);

            if ((((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty) == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                       new MessageBoxControl.MessageBoxChildWindow("Can't Enter Receive Quantity!", "Issue is already received", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
                (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) = 0;
                (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount) = 0;
                return;

            }

            if (Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor > Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty))
            {
                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Received Quantity is equal to Issued Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                return;
            }
            else if (Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) * ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor < Convert.ToSingle(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty))
            {
                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Received Quantity is equal to Issued Quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                return;
            }

            if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID == ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseUOMID && (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty % 1) != 0)
            {
                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
                ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ItemTotalAmount = 0;
                string msgText = "Received Quantity Cannot Be In Fraction for Base UOM";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
                return;
            }

            if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity > 0)
            {
                if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Greater Receive Quantity!", "Receive Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Receive Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty;
                    return;
                }

                if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty * Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQtyBaseCF)))
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Receive Quantity In The List Can't Be Greater Than Issue Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty;
                    return;
                }
            }
            SumOfTotals();
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_GotFocus(object sender, RoutedEventArgs e)
        {
            //    //AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            //    //if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList == null || (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).UOMConversionList.Count == 0)))
            //    //{
            //    //    FillUOMConversions(cmbConversions);
            //    //}


            //    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM != null && ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID > 0)
            //    {
            //        // Function Parameters
            //        // FromUOMID - Transaction UOM
            //        // ToUOMID - Stocking UOM
            //        CalculateConversionFactorCentral(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SelectedUOM.ID, ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).SUOMID);
            //    }
            //    else
            //    {
            //        //((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;
            //        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity = 0;

            //        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ConversionFactor = 0;
            //        ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseConversionFactor = 0;
            //    }


            //    if ((((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty) == 0)
            //    {

            //        MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                                                               new MessageBoxControl.MessageBoxChildWindow("Can't Enter Receive Quantity!", "Issue is already received", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        msgW3.Show();
            //        (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty) = 0;
            //        flagZeroTransit = true;
            //        return;

            //    }

            //    if (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity > 0)
            //    {
            //        if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Greater Receive Quantity!", "Receive Quantity In The List Can't Be Greater Than Pending Quantity. Please Enter Receive Quantity Less Than Or Equal To Pending Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW3.Show();
            //            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BalanceQty;
            //            return;
            //        }

            //        if (Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).BaseQuantity) > (((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty * Convert.ToDecimal(((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQtyBaseCF)))
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Receive Quantity In The List Can't Be Greater Than Issue Quantity.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //            msgW3.Show();
            //            ((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).ReceivedQty = 0;//((clsReceivedItemDetailsVO)dgIssueItemList.SelectedItem).IssueQty;
            //            return;
            //        }
            //    }


            //    SumOfTotals();
        }

        private void txtRetunNumberSrc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                FillReceivedList();
                dgdpReceivedList.PageIndex = 0;
            }
        }

        private void dgIssueItemList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (dgIssueItemList.ItemsSource != null)
            {
                if (((clsReceivedItemDetailsVO)e.Row.DataContext).IsItemBlock)
                    e.Row.IsEnabled = false;
                else
                    e.Row.IsEnabled = true;
            }
        }

    }
}
