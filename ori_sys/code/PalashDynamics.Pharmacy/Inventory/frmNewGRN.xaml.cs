using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.Inventory;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using System.Windows.Browser;
using MessageBoxControl;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Data;
using PalashDynamics.Pharmacy.ItemSearch;
using System.Windows.Input;
using PalashDynamics.Pharmacy.Inventory;
using PalashDynamics.ValueObjects.Administration.UserRights;
using OPDModule.Forms;
using System.Text;
using PalashDynamics.ValueObjects.Log;
using System.Reflection;

namespace PalashDynamics.Pharmacy
{
    public partial class frmNewGRN : UserControl
    {
        #region Variable Declarations
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        bool IsPageLoaded = false;
        public event RoutedEventHandler OnBatchSelection_Click;
        private List<clsGRNDetailsVO> lstGRNDetailsDeepCopy = new List<clsGRNDetailsVO>();
        string LoggedinUserName = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
        int iCount, iItemsCount = 0;

        public List<clsGRNDetailsVO> GRNAddedItems { get; set; }
        public ObservableCollection<clsGRNDetailsVO> GRNPOAddedItems { get; set; }
        public List<clsGRNDetailsVO> GRNDeletedMainItems { get; set; }  // Use to maintain list of deleted Main items
        public PagedCollectionView PCVData;
        double orgVatPer = 0;
        double orgTax = 0;

        public Boolean ISEditMode = false;

        byte[] File;
        string fileName;
        bool IsFileAttached = false;
        public string PreviousBatchValue = "";
        double CostRateNew = 0;

        #region Free Items Variables

        public List<clsGRNDetailsFreeVO> GRNAddedFreeItems { get; set; }  //Use for Free Items grid
        public List<clsGRNDetailsFreeVO> GRNDeletedFreeItems { get; set; }  // Use to maintain list of deleted Free items
        int iCountFree, iItemsCountFree = 0;
        public string PreviousBatchValueFree = "";

        public bool SetFreeItem { get; set; } // to reset the drop down selected item in Main Item column on Free Item tab.

        List<clsFreeMainItems> MainList = new List<clsFreeMainItems>();

        enum GRNTabs
        {
            Main = 0,
            Free = 1
        }

        #endregion
        //added by rohinee dated 27/9/2016 for audit trail==============================================
        bool IsAuditTrail = false;
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        //==============================================================================================
        #region 'Paging'

        public PagedSortableCollectionView<clsGRNVO> DataList { get; private set; }
        public Boolean blnDelete;
        public Boolean blnAdd;
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
                // RaisePropertyChanged("DataListPageSize");
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
            FillGRNSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 

        MasterListItem SelectedItem = new MasterListItem();

        //Added By CDS
        public bool IsApproveded = false;
        public bool ISFromAproveGRN = false;
        public clsGRNVO SelectedGRN = null;
        public Boolean ISEditAndAprove = false;


        clsUserRightsVO objUser;
        bool Approved = false;
        //END

        private void FillGRNSearchList()
        {
            indicator.Show();
            dgGRNList.ItemsSource = null;
            dgGRNItems.ItemsSource = null;

            clsGetGRNListBizActionVO BizAction = new clsGetGRNListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;
            //if (txtGRNNo.SelectedItem != null)
            //    BizAction.GRNNo = txtGRNNo.;
            BizAction.GRNNo = txtGRNNo.Text;
            if (cmbSearchSupplier.SelectedItem != null)
                BizAction.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;
            BizAction.IsForViewInvoice = false;

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetGRNListBizActionVO result = e.Result as clsGetGRNListBizActionVO;
                    DataList.TotalItemCount = result.TotalRows;

                    if (result.List != null)
                    {
                        DataList.Clear();
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);


                        }

                        dgGRNList.ItemsSource = null;
                        dgGRNList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;

                    }

                }
                indicator.Close();

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    //((TextBox)sender).SelectionStart = selectionStart;
                    //((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        #endregion
        #endregion

        #region Constructor and Loaded
        public frmNewGRN()
        {
            //Added
            try
            {
                InitializeComponent();
                this.DataContext = new clsGRNVO();
                _flip = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
                dgAddGRNItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgAddGRNItems_CellEditEnded);
                dtpFromDate.SelectedDate = DateTime.Now.Date;
                dtpToDate.SelectedDate = DateTime.Now.Date;

                //======================================================
                //Paging
                DataList = new PagedSortableCollectionView<clsGRNVO>();
                DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
                DataListPageSize = 15;
                dgDataPager.PageSize = DataListPageSize;
                dgDataPager.Source = DataList;
                //======================================================

                dgAddGRNFreeItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgAddGRNFreeItems_CellEditEnded);

                // Reset the Activity Log List added by rohinee dated 26/9/2016
                LogInfoList = new List<LogInfo>();
                IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            rdbDirectPur.IsChecked = true;
            FillState(); //Added By Bhushan For GST 24062017
            //dgPO.Height = 90;
            //dgPO.Visibility = Visibility.Visible;
            if (!IsPageLoaded)
            {
                this.DataContext = new clsGRNVO();

                long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    clinicId = 0;
                }
                else
                {
                    clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                //Commented By CDS
                //FillStores(clinicId);
                //FillReceivedByList();
                //FillSupplier();
                //END


                FillStores(clinicId);  // Added By CDS 

                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);

                //======================================================
                //Paging
                GetGRNNumber();
                FillGRNSearchList();
                //======================================================

                SetCommandButtonState("New");
                _flip.Invoke(RotationType.Backward);
                GRNAddedItems = new List<clsGRNDetailsVO>();
                GRNPOAddedItems = new ObservableCollection<clsGRNDetailsVO>();
                //dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.ItemsSource = new ObservableCollection<clsGRNDetailsVO>();
                IsPageLoaded = true;
                dpInvDt.SelectedDate = DateTime.Today;
                IsFileAttached = false;
                dpInvDt.SelectedDate = DateTime.Now.Date;
                //dpInvDt.IsEnabled = false;

                GRNAddedFreeItems = new List<clsGRNDetailsFreeVO>();  //Use for Free Items grid

                dpInvDt.SetValidation("Please Select Proper Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();

                //txtinvno.SetValidation("Please Select Invoice Number");
                //txtinvno.RaiseValidationError();
                //txtinvno.Focus();
            }

        }
        #endregion

        #region Clicked Events
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Boolean blnValid = false;
            bool Expiry = false;
            int count = 0;
            iItemsCount = 0;

            #region For Free Items

            Boolean blnValidFree = false;
            int countFree = 0;
            iItemsCountFree = 0;

            #endregion

            iCount = GRNAddedItems.Count;

            iCountFree = GRNAddedFreeItems.Count;

            //for (int iGrnItems = 0; iGrnItems < GRNAddedItems.Count; iGrnItems++)
            //{
            if (GRNAddedItems == null || GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return;
            }
            if (Convert.ToDouble(txtGRNDiscount.Text) > ((clsGRNVO)this.DataContext).NetAmount)
            {
                txtGRNDiscount.SetValidation("Discount Amount must be less than Net Amount");
                txtGRNDiscount.RaiseValidationError();
                return;
            }
            else
                txtGRNDiscount.ClearValidationError();

            //if (ChkBatchExistsOrNot())
            if (ChkBatchExistsOrNot() && ChkBatchExistsOrNotFree())    // For Free Items
            {
                foreach (clsGRNDetailsVO item in GRNAddedItems)
                {
                    bool b = ValidateOnSave(item);
                    if (b)
                    {
                        blnValid = true;

                        iItemsCount++;
                    }
                    if (!b)
                    {
                        return;
                    }

                    //Commented By CDS 25/12/2015
                    //By Umesh
                    //if (item.IsBatchRequired && item.ExpiryDate.HasValue)
                    //{
                    //    TimeSpan day = item.ExpiryDate.Value - DateTime.Now;
                    //    int Day1 = (int)day.TotalDays;
                    //    if (Day1 < 60)
                    //    {
                    //        Expiry = true;
                    //        count++;
                    //    }
                    //}
                    //End
                }

                if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count > 0)
                {
                    foreach (clsGRNDetailsFreeVO item in GRNAddedFreeItems)
                    {
                        bool c = ValidateOnSaveFree(item);
                        if (c)
                        {
                            blnValidFree = true;

                            iItemsCountFree++;
                        }
                        if (!c)
                        {
                            return;
                        }
                    }
                }

                bool isSaveMain = false;
                bool isSaveFree = false;

                if (blnValid && iCount != 0 && iItemsCount == iCount)
                {
                    isSaveMain = true;
                }

                if (iCountFree > 0)
                {
                    if (blnValidFree && iCountFree != 0 && iItemsCountFree == iCountFree)
                    {
                        isSaveFree = true;
                    }
                }
                else
                {
                    isSaveFree = true;
                }

                //if (blnValid && iCount != 0 && iItemsCount == iCount)
                if (isSaveMain == true && isSaveFree == true)  //if (blnValid && iCount != 0 && iItemsCount == iCount && (blnValidFree && iCountFree != 0 && iItemsCountFree == iCountFree))
                {
                    string msgTitle = "Palash";
                    string msgText = "";
                    chkIsFinalized.IsChecked = true;
                    //Commented By CDS 25/12/2015
                    //if (Expiry)  //By Umesh
                    //{

                    //    msgText = count + " Item has expiry date less than 60 days,do you want to save GRN ?";
                    //}
                    //else 
                    //END


                    //Added By CDS..............................
                    StringBuilder strError = new StringBuilder();

                    if (GRNAddedItems != null && GRNAddedItems.ToList<clsGRNDetailsVO>().Count > 0)
                    {
                        var item1 = from r in GRNAddedItems.ToList<clsGRNDetailsVO>()
                                    where (r.IsBatchRequired == true && r.ItemExpiredInDays == 0)
                                    select r;

                        if (item1 != null && item1.ToList().Count > 0)
                        {
                            foreach (var clsGRNDetailsVO in item1.ToList())
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(item1.ToList()[0].ItemName);

                            }
                        }
                    }

                    //...............................................
                    //string msgTitle = "";
                    //string msgText = "";
                    //if (strError != null && Convert.ToString(strError) != string.Empty)
                    //{
                    //    msgText = "Expired In Days Are not Defined In Master For " + strError + " Item ,\n Are You Sure You Want To Save Opening Balance Details?";
                    //}
                    //else
                    //{
                    //    msgText = "Are you sure you want to save the Opening Balance Details?";
                    //}

                    if (chkConsignment.IsChecked == false)
                    {
                        if (ISFromAproveGRN == true)
                        {
                            if (strError != null && Convert.ToString(strError) != string.Empty)
                                //msgText = "Some Items do not have Expired In Days In Master " + ",\n Are you sure you want to Modify and Approve GRN?";     
                                msgText = "Some Items do not have Expired In Days In Master " + ",\n Are you sure you want to Verify and Approve GRN?";   //Added by Ajit date 14/9/2016 Change Msg //***//
                            else
                                msgText = "Are you sure you want to Modify and Approve GRN ?";
                        }
                        else if (strError != null && Convert.ToString(strError) != string.Empty)
                            msgText = "Some Items do not have Expired In Days In Master " + ",\n Are you sure you want to Save GRN ?";
                        else
                            msgText = "Are you sure you want to Save GRN ?";
                    }
                    else
                    {
                        msgText = "Are you sure you want to Save Consignment GRN ?";
                    }


                    MessageBoxControl.MessageBoxChildWindow msgWin =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWin.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin_OnMessageBoxClosed);
                    msgWin.Show();
                }
            }
            else
            {
                return;
            }
        }

        private void msgWin_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {

                SaveGRN(false);
            }
            else if (result == MessageBoxResult.No)
            {
                iItemsCount = 0;
                chkIsFinalized.IsChecked = true;
                iItemsCountFree = 0;  // For Free Items
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("ClickNew");
            InitialiseForm();
            //this.DataContext = new clsGRNVO();

            //long clinicId = 0; //= ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            //{
            //    clinicId = 0;
            //}
            //else
            //{
            //    clinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            //FillStores(clinicId);
            //FillSupplier();
            GRNAddedItems = new List<clsGRNDetailsVO>();
            dgAddGRNItems.ItemsSource = null;

            GRNAddedFreeItems = new List<clsGRNDetailsFreeVO>(); // Free Items
            dgAddGRNFreeItems.ItemsSource = null;

            BdrItemSearch.Visibility = Visibility.Collapsed;
            if (rdbDirectPur.IsChecked == true)
            {
                dgAddGRNItems.Columns[6].Visibility = Visibility.Collapsed;
                dgAddGRNItems.Columns[28].Visibility = Visibility.Collapsed;

            }
            _flip.Invoke(RotationType.Forward);
            dpInvDt.SelectedDate = DateTime.Today;
            cmbReceivedBy.SelectedValue = 0;
            //DataGridColumnVisibility();
            // TO Get Latest GRN Count For that User So put It Here 
            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
        }

        /// <summary>
        /// Cancel button click
        /// </summary>
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (ISFromAproveGRN == true)
            {
                frmApproveNewGRN AprvGRN = new frmApproveNewGRN();


                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                mElement.Text = "Approve Goods Received Note";
                ((IApplicationConfiguration)App.Current).OpenMainContent(AprvGRN);
            }
            else
            {
                SetCommandButtonState("New");
                this.DataContext = new clsGRNVO();
                //if (PreviousGRNType!= null)
                //((clsGRNVO)this.DataContext).GRNType = PreviousGRNType;
                //InitialiseForm();
                _flip.Invoke(RotationType.Backward);
            }

        }

        /// <summary>
        /// Add item button click
        /// </summary>
        #region Commented
        //private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        //{

        //    if (cmbStore.SelectedItem == null || ((MasterListItem)cmbStore.SelectedItem).ID == 0)
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                      new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();


        //    }
        //    else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
        //    {
        //        MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                      new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        msgW1.Show();
        //    }
        //    else
        //    {
        //        //POSearchWindow
        //        if (this.ISEditMode == false && GRNAddedItems != null)
        //            GRNAddedItems.Clear();
        //        txtPONO.Text = String.Empty;
        //        PurchaseOrder_SearchWindow POWin = new PurchaseOrder_SearchWindow();
        //        POWin.cboStoreName.IsEnabled = false;
        //        POWin.cboSuppName.IsEnabled = false;
        //        POWin.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
        //        POWin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;

        //        POWin.OnSaveButton_Click += new RoutedEventHandler(POWin_OnSaveButton_Click);
        //        POWin.Show();
        //    }

        //}
        #endregion Commented
        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {

            if (((clsGRNVO)this.DataContext).GRNType == ValueObjects.InventoryGRNType.Direct)
            {
                ItemList Itemswin = new ItemList();
                Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                Itemswin.ShowBatches = false;
                if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)     //((MasterListItem)cmbStore.SelectedItem).ID
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (String.IsNullOrEmpty(txtinvno.Text) || txtinvno.Text.Trim().Length == 0) //***//19
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Invoice No.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {

                    Itemswin.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;    // ((MasterListItem)cmbStore.SelectedItem).ID;
                    Itemswin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    Itemswin.cmbStore.IsEnabled = false;
                    Itemswin.IsFromGRN = true;

                    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
                    Itemswin.Show();
                }
            }
            else
            //if (((clsGRNVO)this.DataContext).GRNType == ValueObjects.InventoryGRNType.AgainstPO)
            {
                txtPONO.Text = String.Empty;
                if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)   //((MasterListItem)cmbStore.SelectedItem).ID
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else if (String.IsNullOrEmpty(txtinvno.Text) || txtinvno.Text.Trim().Length == 0) //***//19
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please Enter Invoice No.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {
                    //POSearchWindow

                    //if (this.ISEditMode == false && GRNAddedItems != null)
                    //    GRNAddedItems.Clear();
                    //if (this.ISEditMode == false && GRNPOAddedItems != null)
                    //    GRNPOAddedItems.Clear();
                    if (this.ISEditMode == false && GRNAddedItems.Count == 0)
                        GRNAddedItems.Clear();
                    if (this.ISEditMode == false && GRNPOAddedItems.Count == 0)
                        GRNPOAddedItems.Clear();

                    txtPONO.Text = String.Empty;
                    PurchaseOrder_SearchWindow POWin = new PurchaseOrder_SearchWindow();
                    POWin.cboStoreName.IsEnabled = false;
                    POWin.cboSuppName.IsEnabled = false;
                    POWin.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;   // ((MasterListItem)cmbStore.SelectedItem).ID;
                    POWin.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    POWin.OnSaveButton_Click += new RoutedEventHandler(POWin_OnSaveButton_Click);
                    POWin.Show();

                }
            }
        }

        /// <summary>
        /// Search button click
        /// </summary>
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            bool res = true;



            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                if (dtpFromDate.SelectedDate.Value.Date > dtpToDate.SelectedDate.Value.Date)
                {
                    //dtpFromDate.SetValidation("From Date should be less than To Date");
                    //dtpFromDate.RaiseValidationError();
                    //dtpFromDate.Focus();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                             new MessageBoxControl.MessageBoxChildWindow("", "From Date should be less than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    dtpFromDate.SelectedDate = dtpToDate.SelectedDate;
                    res = false;
                }
                else
                {
                    dtpFromDate.ClearValidationError();
                }
            }



            else if (dtpFromDate.SelectedDate != null && dtpFromDate.SelectedDate.Value.Date > DateTime.Now.Date)
            {
                //dtpFromDate.SetValidation("From Date should not be greater than Today's Date");
                //dtpFromDate.RaiseValidationError();
                //dtpFromDate.Focus();
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "From Date should not be greater than Today's Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                dtpFromDate.SelectedDate = DateTime.Now;
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Plase Select To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate == null)
            {
                dtpFromDate.SetValidation("Plase Select From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                //======================================================
                //Paging
                dgDataPager.PageIndex = 0;
                FillGRNSearchList();
                //======================================================
            }

        }

        /// <summary>
        /// Items Search window ok button click
        /// </summary>
        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            ItemList Itemswin = (ItemList)sender;
            RowCount = 0;
            dgAddGRNItems.Columns[5].Visibility = Visibility.Collapsed;
            dgAddGRNItems.Columns[6].Visibility = Visibility.Collapsed;
            dgAddGRNItems.Columns[7].Visibility = Visibility.Collapsed;
            dgAddGRNItems.Columns[8].Visibility = Visibility.Collapsed;
            dgAddGRNItems.Columns[4].Visibility = Visibility.Visible;

            dgAddGRNItems.Columns[16].IsReadOnly = false;  //Added by Prashant Channe 27/10/2018, to make Cost Price editable when items added for Direct PO

            //dgAddGRNItems.Columns[16].IsReadOnly = true;
            //dgAddGRNItems.Columns[18].IsReadOnly = true; //"MRP"  MRP Editable as per Client Requirement on 11-03-2016
            dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;   // "VAT Amount"


            if (Itemswin.SelectedItems != null)
            {
                if (GRNAddedItems == null)
                    GRNAddedItems = new List<clsGRNDetailsVO>();

                if (GRNPOAddedItems == null)
                    GRNPOAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                foreach (var item in Itemswin.SelectedItems)
                {
                    clsGRNDetailsVO grnObj = new clsGRNDetailsVO();
                    grnObj.IsBatchRequired = item.BatchesRequired;
                    //if (item.BatchesRequired == false)
                    //{ 
                    //    dgAddGRNItems.
                    //}
                    grnObj.ItemID = item.ID;
                    //grnObj.ItemTax = item.TotalPerchaseTaxPercent;
                    grnObj.ItemTax = Convert.ToDouble(item.ItemVatPer);
                    grnObj.ItemName = item.ItemName;
                    grnObj.VATPercent = Convert.ToDouble(item.VatPer);
                    //grnObj.Rate = Convert.ToDouble(item.PurchaseRate);
                    grnObj.Rate = Convert.ToDouble(item.PurchaseRate) * Convert.ToDouble(item.PurchaseToBaseCF);
                    grnObj.MainRate = Convert.ToSingle(item.PurchaseRate);

                    //Added By Bhushanp 24062017 For GST                   
                    grnObj.SGSTPercent = ((MasterListItem)cmbStoreState.SelectedItem).ID == ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.SGSTPercent) : 0;
                    grnObj.CGSTPercent = ((MasterListItem)cmbStoreState.SelectedItem).ID == ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.CGSTPercent) : 0;
                    grnObj.IGSTPercent = ((MasterListItem)cmbStoreState.SelectedItem).ID != ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.IGSTPercent) : 0;

                    //grnObj.MRP = Convert.ToDouble(item.MRP);
                    grnObj.MRP = Convert.ToDouble(item.MRP) * Convert.ToDouble(item.PurchaseToBaseCF);
                    grnObj.MainMRP = Convert.ToSingle(item.MRP);

                    grnObj.AbatedMRP = Convert.ToDouble(item.AbatedMRP);
                    grnObj.GRNItemVatType = item.ItemVatType;
                    grnObj.GRNItemVatApplicationOn = item.ItemVatApplicationOn;

                    grnObj.OtherGRNItemTaxType = item.ItemOtherTaxType;
                    grnObj.OtherGRNItemTaxApplicationOn = item.OtherItemApplicationOn;
                    //Added By Bhushanp 24062017 For GST                

                    grnObj.GRNSGSTVatType = item.SGSTtaxtype;
                    grnObj.GRNSGSTVatApplicationOn = item.SGSTapplicableon;
                    grnObj.GRNCGSTVatType = item.CGSTtaxtype;
                    grnObj.GRNCGSTVatApplicationOn = item.CGSTapplicableon;
                    grnObj.GRNIGSTVatType = item.IGSTtaxtype;
                    grnObj.GRNIGSTVatApplicationOn = item.IGSTapplicableon;

                    grnObj.StockUOM = item.SUOM;
                    grnObj.PurchaseUOM = item.PUOM;
                    //grnObj.ConversionFactor = Convert.ToDouble(item.ConversionFactor);
                    grnObj.ItemExpiredInDays = Convert.ToInt64(item.ItemExpiredInDays);

                    grnObj.PUOM = item.PUOM;
                    grnObj.MainPUOM = item.PUOM;
                    grnObj.SUOM = item.SUOM;
                    //grnObj.ConversionFactor = Convert.ToSingle(item.ConversionFactor);

                    grnObj.PUOMID = item.PUM;
                    grnObj.SUOMID = item.SUM;
                    grnObj.BaseUOMID = item.BaseUM;
                    grnObj.BaseUOM = item.BaseUMString;
                    grnObj.SellingUOMID = item.SellingUM;
                    grnObj.SellingUOM = item.SellingUMString;

                    grnObj.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                    grnObj.TransUOM = item.PUOM;

                    grnObj.ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                    grnObj.BaseConversionFactor = item.PurchaseToBaseCF;

                    grnObj.BaseRate = Convert.ToSingle(item.PurchaseRate);
                    grnObj.BaseMRP = Convert.ToSingle(item.MRP);

                    grnObj.Rackname = item.Rackname;
                    grnObj.Shelfname = item.Shelfname;
                    grnObj.Containername = item.Containername;

                    //if (item.AssignSupplier == false)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Supplier is not assigned to item", "Do you want to assign supplier for the item:" + item.ItemName, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Question);
                    //    msgWin.OnMessageBoxClosed += (re) =>
                    //    {
                    //        MessageBoxResult reply = msgWin.Result;

                    //        if (re == MessageBoxResult.OK)
                    //        {

                    //            grnObj.AssignSupplier = true;
                    //        }
                    //        else
                    //            grnObj.AssignSupplier = false;
                    //    };
                    //    msgWin.Show();
                    //}
                    //else
                    //    grnObj.AssignSupplier = item.AssignSupplier;

                    if (item.BatchesRequired == false)
                    {
                        GetAvailableQuantityForNonBatchItems(item.ID);
                    }
                    var item2 = from r in GRNAddedItems
                                where r.ItemID == item.ID && item.BatchesRequired == false
                                select r;

                    if (item2.ToList().Count > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", item.ItemName + " already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWin.Show();
                    }
                    else
                    {
                        if (item.BatchesRequired == false)
                        {
                            GRNAddedItems.Add(grnObj);
                        }

                    }
                    if (item.BatchesRequired == true)
                    {
                        GRNAddedItems.Add(grnObj);
                    }

                }
                #region tempcommented
                //int grnAddedeItemCnt = GRNAddedItems.Count;
                //#region tempcommented
                //foreach (var item in GRNAddedItems)
                //{
                //    grnAddedeItemCnt++;
                //    clsCheckItemSupplierFromGRNBizActionVO BizAction = new clsCheckItemSupplierFromGRNBizActionVO();
                //    BizAction.ItemID = item.ID;
                //    if (cmbSupplier.SelectedItem != null && ((MasterListItem)cmbSupplier.SelectedItem).ID != 0)
                //        BizAction.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;


                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //    Client.ProcessCompleted += (s, arg) =>
                //    {
                //        if (arg.Error == null && arg.Result != null)
                //        {
                //            if (((clsCheckItemSupplierFromGRNBizActionVO)arg.Result).checkSupplier == true)
                //            {
                //                item.AssignSupplier = false;
                //            }
                //            else
                //                item.AssignSupplier = true;



                //        }
                //        if (grnAddedeItemCnt - 1 == GRNAddedItems.Count)
                //        {
                //            dgAddGRNItems.Focus();
                //            dgAddGRNItems.UpdateLayout();
                //            dgAddGRNItems.ItemsSource = null;
                //            dgAddGRNItems.ItemsSource = GRNAddedItems;
                //            dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;
                //        }

                //    };

                //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //    Client.CloseAsync();

                //}
                #endregion

                #region Set SrNo

                long mainCnt = 0;

                var itemMax = GRNAddedItems.Max(x => x.SrNo);

                if (itemMax != null)
                    mainCnt = itemMax;

                foreach (clsGRNDetailsVO itemMain in GRNAddedItems)
                {
                    if (itemMain.SrNo == 0)
                    {
                        mainCnt++;
                        itemMain.SrNo = mainCnt;
                    }
                }

                #endregion

                dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.Focus();
                dgAddGRNItems.UpdateLayout();

                dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;

                //added by rohinee dated 26/9/2016 for audit trail    
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    objGUID = new Guid();
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 111 : Get Items : "
                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " ";
                        foreach (var item in GRNAddedItems)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.ItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Item Name : " + Convert.ToString(item.ItemName) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                }
                //=========================================================================

            }
        }

        /// <summary>
        /// OK button on PO search window click
        /// </summary>
        void POWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PurchaseOrder_SearchWindow Itemswin = (PurchaseOrder_SearchWindow)sender;
            RowCount = 0;
            if (Itemswin.SelectedPO != null && Itemswin.ItemsSelected != null)
            {
                if (rdbDirectPur.IsChecked == true)
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Collapsed;  //"VAT Amount"
                }
                else
                {
                    dgAddGRNItems.Columns[5].Visibility = Visibility.Visible;
                    dgAddGRNItems.Columns[6].Visibility = Visibility.Visible;
                    dgAddGRNItems.Columns[7].Visibility = Visibility.Visible;
                    dgAddGRNItems.Columns[8].Visibility = Visibility.Visible;

                    dgAddGRNItems.Columns[16].IsReadOnly = true;
                    //dgAddGRNItems.Columns[18].IsReadOnly = true;   MRP Editable as per Client Requirement on 11-03-2016

                    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible; //"VAT Amount"
                }
                if (this.DataContext != null)
                {
                    ((clsGRNVO)this.DataContext).POID = Itemswin.SelectedPO.ID;
                    ((clsGRNVO)this.DataContext).PONO = Itemswin.SelectedPO.PONO;
                    ((clsGRNVO)this.DataContext).PODate = Itemswin.SelectedPO.Date;
                }

                foreach (var item in Itemswin.ItemsSelected)
                {

                    if (item.PendingQuantity != 0)
                    {
                        StringBuilder sbPOIDList = new StringBuilder();
                        StringBuilder sbPODetailIDList = new StringBuilder();

                        clsGRNDetailsVO GINItem = new clsGRNDetailsVO();
                        GINItem.IsBatchRequired = item.BatchesRequired;
                        GINItem.ItemID = item.ItemID.Value;
                        GINItem.ItemName = item.ItemName;
                        GINItem.Quantity = 0;
                        GINItem.POQuantity = Convert.ToDouble(item.Quantity);
                        GINItem.Rate = Convert.ToDouble(item.Rate);
                        GINItem.MRP = Convert.ToDouble(item.MRP);
                        GINItem.AbatedMRP = Convert.ToDouble(item.AbatedMRP);

                        GINItem.SchDiscountPercent = Convert.ToDouble(item.DiscPercent);
                        if (GINItem.SchDiscountPercent == 0) GINItem.SchDiscountAmount = Convert.ToDouble(item.DiscountAmount);
                        GINItem.VATPercent = Convert.ToDouble(item.VATPer);
                        if (GINItem.VATPercent == 0) GINItem.VATAmount = Convert.ToDouble(item.VATAmount);
                        //Added By Bhushanp For GST 24062017
                        GINItem.SGSTPercent = Convert.ToDouble(item.SGSTPercent);
                        if (GINItem.SGSTPercent == 0) GINItem.SGSTAmount = Convert.ToDouble(item.SGSTAmount);
                        GINItem.CGSTPercent = Convert.ToDouble(item.CGSTPercent);
                        if (GINItem.CGSTPercent == 0) GINItem.CGSTAmount = Convert.ToDouble(item.CGSTAmount);
                        GINItem.IGSTPercent = Convert.ToDouble(item.IGSTPercent);
                        if (GINItem.IGSTPercent == 0) GINItem.IGSTAmount = Convert.ToDouble(item.IGSTAmount);



                        GINItem.POPendingQuantity = Convert.ToDouble(item.PendingQuantity);
                        GINItem.PendingQuantity = Convert.ToDouble(item.PendingQuantity);
                        GINItem.PoItemsID = item.PoItemsID;
                        GINItem.ItemTax = item.ItemTax;
                        GINItem.PODate = item.PODate;
                        GINItem.POID = item.POID;
                        GINItem.POUnitID = item.POUnitID;
                        GINItem.PODetailID = item.PoDetailsID;
                        GINItem.PODetailUnitID = item.PoDetailsUnitID;
                        GINItem.PONO = item.PONO;
                        GINItem.PODate = item.PODate;
                        GINItem.PurchaseUOM = item.PurchaseUOM;
                        GINItem.StockUOM = item.StockUOM;
                        GINItem.ConversionFactor = item.ConversionFactor;
                        GINItem.UnitRate = Convert.ToDouble(item.Rate);
                        GINItem.UnitMRP = Convert.ToDouble(item.MRP);
                        //GINItem.BarCode = Convert.ToString(item.BarCode);
                        GINItem.ItemCategory = item.ItemCategory;
                        GINItem.ItemGroup = item.ItemGroup;

                        GINItem.ItemExpiredInDays = Convert.ToInt64(item.ItemExpiredInDays);
                        #region
                        //Added BY CDS
                        //GINItem.MainRate = Convert.ToSingle(item.PurchaseRate);
                        //GINItem.MainMRP = Convert.ToSingle(item.MRP);
                        //GINItem.GRNItemVatType = item.ItemVatType;
                        //GINItem.GRNItemVatApplicationOn = item.ItemVatApplicationOn;
                        //GINItem.OtherGRNItemTaxType = item.ItemOtherTaxType;
                        //GINItem.OtherGRNItemTaxApplicationOn = item.OtherItemApplicationOn;
                        //GINItem.ConversionFactor = Convert.ToSingle(item.ConversionFactor);
                        //GINItem.PUOMID = item.PUM;
                        //GINItem.SUOMID = item.SUM;
                        //GINItem.BaseUOMID = item.BaseUM;
                        //GINItem.BaseUOM = item.BaseUMString;
                        //GINItem.SellingUOMID = item.SellingUM;
                        //GINItem.SellingUOM = item.SellingUMString;
                        ////

                        // Added By CDS 
                        //GINItem.ItemTax = Convert.ToDouble(item.ItemVatPer);
                        #endregion
                        GINItem.ItemTax = Convert.ToDouble(item.ItemVATPercent);
                        //GINItem.ItemVATAmount = item.ItemVATAmount;
                        GINItem.GRNItemVatType = item.POItemVatType;
                        GINItem.GRNItemVatApplicationOn = item.POItemVatApplicationOn;
                        GINItem.OtherGRNItemTaxType = item.POItemOtherTaxType;
                        GINItem.OtherGRNItemTaxApplicationOn = item.POItemOtherTaxApplicationOn;
                        //Added By Bhushanp For GST
                        GINItem.GRNSGSTVatType = item.GRNSGSTVatType;
                        GINItem.GRNSGSTVatApplicationOn = item.GRNSGSTVatApplicableOn;
                        GINItem.GRNCGSTVatType = item.GRNCGSTVatType;
                        GINItem.GRNCGSTVatApplicationOn = item.GRNCGSTVatApplicableOn;
                        GINItem.GRNIGSTVatType = item.GRNIGSTVatType;
                        GINItem.GRNIGSTVatApplicationOn = item.GRNIGSTVatApplicableOn;

                        GINItem.OtherCharges = (double)item.OtherCharges;
                        //GINItem.PODiscount = (decimal)item.PODiscount;
                        GINItem.BaseConversionFactor = Convert.ToSingle(item.BaseConversionFactor);
                        GINItem.BaseRate = item.BaseRate;
                        GINItem.BaseMRP = item.BaseMRP;
                        GINItem.BaseRateBeforAddItem = item.BaseRate;
                        GINItem.BaseMRPBeforAddItem = item.BaseMRP;
                        GINItem.BaseQuantity = item.BaseQuantity;
                        GINItem.BaseUOM = item.BaseUOM;
                        GINItem.MainRate = item.BaseRate;
                        GINItem.MainMRP = item.BaseMRP;
                        GINItem.SelectedUOM = new MasterListItem { ID = item.SelectedUOM.ID, Description = item.TransUOM };
                        GINItem.SUOMID = item.SUOMID;
                        GINItem.BaseUOMID = item.BaseUOMID;
                        GINItem.SUOM = item.StockUOM;
                        GINItem.TransUOM = item.TransUOM;
                        GINItem.POTransUOM = item.TransUOM;
                        // End 

                        //GINItem.CostRate = (double)item.Quantity * (double)item.Rate;
                        GINItem.Remarks = item.Specification;
                        GINItem.Rackname = item.Rackname;
                        GINItem.Shelfname = item.Shelfname;
                        GINItem.Containername = item.Containername;
                        GINItem.GRNApprItemQuantity = item.GRNApprItemQuantity;
                        GINItem.GRNPendItemQuantity = item.GRNPendItemQuantity;
                        //GINItem.POIDList = Convert.ToString(item.POID); // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                        //GINItem.PODetailsIDList = Convert.ToString(item.PoDetailsID); // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                        //END
                        GINItem.POFinalPendingQuantity = item.POFinalPendingQuantity;

                        if (!String.IsNullOrEmpty(txtPONO.Text) && !txtPONO.Text.Trim().Contains(item.PONO.Trim()))
                            txtPONO.Text = String.Format(txtPONO.Text + "," + item.PONO.Trim());

                        if (GINItem.IsBatchRequired == false)
                        {
                            GetAvailableQuantityForNonBatchItems(item.ItemID.Value);
                        }
                        GINItem.IndentID = Convert.ToInt64(item.IndentID);
                        GINItem.IndentUnitID = Convert.ToInt64(item.IndentUnitID);

                        var item5 = from r5 in GRNAddedItems
                                    where r5.ItemID == item.ItemID && r5.POID == item.POID && r5.POUnitID == item.POUnitID
                                    select r5;

                        var item6 = from r6 in GRNPOAddedItems
                                    where r6.ItemID == item.ItemID && r6.POID == item.POID && r6.POUnitID == item.POUnitID
                                    select r6;

                        // Check the list GRNAddedItem and GRNPOAdded Item contain any element.
                        if ((item5 != null && item5.ToList().Count == 0) && (item6 != null && item6.ToList().Count == 0))
                        {
                            if (GRNAddedItems.Count > 0)
                            {
                                var item2 = from r in GRNAddedItems
                                            where r.ItemID == item.ItemID && r.BaseRate == item.BaseRate && r.BaseMRP == item.BaseMRP
                                            select r;

                                if (item2 != null && item2.ToList().Count > 0)
                                {
                                    clsGRNDetailsVO GRNItem = GRNAddedItems.Where(g => g.ItemID == item.ItemID).FirstOrDefault();

                                    if (GRNItem != null)
                                    {
                                        var item3 = from r1 in GRNPOAddedItems
                                                    where r1.ItemID == item.ItemID && r1.POID == item.POID && r1.POUnitID == item.POUnitID
                                                    select r1;

                                        if (item3 != null && item3.ToList().Count == 0)
                                        {
                                            GRNPOAddedItems.Add(GINItem.DeepCopy());
                                        }

                                        GRNItem.POQuantity += Convert.ToInt64(item.Quantity);
                                        GRNItem.POPendingQuantity += Convert.ToInt64(item.PendingQuantity);
                                        GRNItem.PendingQuantity += Convert.ToInt64(item.PendingQuantity);

                                        //GRNItem.POIDList = string.Format(GRNItem.POIDList + "," + item.POID); // Added by Ashish Z. for Concurrency between two users.. on Dated 261116
                                        //GRNItem.PODetailsIDList = string.Format(GRNItem.PODetailsIDList + "," + item.PoDetailsID); // Added by Ashish Z. for Concurrency between two users.. on Dated 261116

                                        //GRNItem.PODetailsIDList += item.PoDetailsID;
                                    }
                                }
                                else
                                {
                                    GRNAddedItems.Add(GINItem);
                                    var item4 = from r2 in GRNPOAddedItems
                                                where r2.ItemID == item.ItemID && r2.POID == item.POID && r2.POUnitID == item.POUnitID
                                                select r2;

                                    if (item4 != null && item4.ToList().Count == 0)
                                    {
                                        GRNPOAddedItems.Add(GINItem.DeepCopy());
                                    }

                                }
                            }
                            else
                            {
                                GRNAddedItems.Add(GINItem);
                                GRNPOAddedItems.Add(GINItem.DeepCopy());
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Item PO combination already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Warning);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "You Can Not Save Pending Quantity Value as Zero", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        if (txtPONO.Text.Trim().Contains(item.PONO.Trim()))
                            txtPONO.Text = (txtPONO.Text.Replace(item.PONO, string.Empty)).Trim(',');
                    }

                }

                #region Set SrNo

                long mainCnt = 0;

                var itemMax = GRNAddedItems.Max(x => x.SrNo);

                if (itemMax != null)
                    mainCnt = itemMax;

                foreach (clsGRNDetailsVO itemMain in GRNAddedItems)
                {
                    if (itemMain.SrNo == 0)
                    {
                        mainCnt++;
                        itemMain.SrNo = mainCnt;
                    }
                }

                #endregion

                PCVData = new PagedCollectionView(GRNAddedItems);
                PCVData.GroupDescriptions.Add(new PropertyGroupDescription("PONO"));
                List<clsGRNDetailsVO> ob = new List<clsGRNDetailsVO>();
                ob = (List<clsGRNDetailsVO>)(PCVData).SourceCollection;
                dgAddGRNItems.ItemsSource = PCVData;

                dgAddGRNItems.Focus();
                dgAddGRNItems.UpdateLayout();
                CalculateGRNSummary();
                dgAddGRNItems.SelectedIndex = GRNAddedItems.Count - 1;

                //added by rohinee dated 26/9/2016 for audit trail 
                #region
                if (GRNPOAddedItems != null && GRNPOAddedItems.Count > 0)
                {
                    objGUID = new Guid();
                    if (IsAuditTrail)
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 112 : Get PO Items : "
                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " ";
                        foreach (var item in GRNPOAddedItems)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.ItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Item Name : " + Convert.ToString(item.ItemName) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            + " , Quantity : " + Convert.ToString(item.Quantity) + " "
                                            + " , POQuantity : " + Convert.ToString(item.POQuantity) + " "
                                            + " , AbatedMRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , SchDiscountPercent : " + Convert.ToString(item.SchDiscountPercent) + " "
                                            + " , VATPercent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , POPendingQuantity : " + Convert.ToString(item.POPendingQuantity) + " "
                                            + " , PendingQuantity : " + Convert.ToString(item.PendingQuantity) + " "
                                            + " , PoItemsID : " + Convert.ToString(item.PoItemsID) + " "
                                            + " , ItemTax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , PODate : " + Convert.ToString(item.PODate) + " "
                                            + " , POID : " + Convert.ToString(item.POID) + " "
                                            + " , PONO : " + Convert.ToString(item.PONO) + " "
                                            + " , POUnitID : " + Convert.ToString(item.POUnitID) + " "
                                            + " , PODate : " + Convert.ToString(item.PODate) + " "
                                            + " , PurchaseUOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , StockUOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , ConversionFactor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , UnitRate : " + Convert.ToString(item.UnitRate) + " "
                                            + " , UnitMRP : " + Convert.ToString(item.UnitMRP) + " "
                                            + " , BarCode : " + Convert.ToString(item.BarCode) + " "
                                            + " , Item : " + Convert.ToString(item.ItemCategory) + " "
                                            + " , Item : " + Convert.ToString(item.ItemCategory) + " "
                                            + " , ItemGroup : " + Convert.ToString(item.ItemGroup) + " "
                                            + " , ItemExpiredInDays : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , ItemTax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , GRNItemVatType : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRNItemVatApplicationOn : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , OtherGRNItemTaxType : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , OtherGRNItemTaxApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , BaseConversionFactor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , BaseRate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , BaseMRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , BaseQuantity : " + Convert.ToString(item.BaseQuantity) + " "
                                            + " , BaseUOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , MainRate : " + Convert.ToString(item.MainRate) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , BaseUOMID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , TransUOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , POTransUOM : " + Convert.ToString(item.POTransUOM) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            + " , GRNApprItemQuantity : " + Convert.ToString(item.GRNApprItemQuantity) + " "
                                            + " , GRNPendItemQuantity : " + Convert.ToString(item.GRNPendItemQuantity) + " "
                                            + " , IndentID : " + Convert.ToString(item.IndentID) + " "
                                            + " , IndentUnitID : " + Convert.ToString(item.IndentUnitID) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                }
                #endregion
                //=========================================================================
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetCommandButtonState("Modify");
                InitialiseForm();

                clsGRNVO obj = null;
                if (ISFromAproveGRN == true)
                {
                    obj = SelectedGRN;
                }
                else
                {
                    obj = (clsGRNVO)dgGRNList.SelectedItem;
                }

                if (obj != null)
                {
                    this.DataContext = obj;
                    ((clsGRNVO)this.DataContext).GRNNO = obj.GRNNO;
                    ((clsGRNVO)this.DataContext).Date = obj.Date;

                    ((clsGRNVO)this.DataContext).StoreID = obj.StoreID;
                    ((clsGRNVO)this.DataContext).SupplierID = obj.SupplierID;



                    //if (cmbStore.ItemsSource != null)
                    //{
                    //    var results = from r in ((List<MasterListItem>)cmbStore.ItemsSource)
                    //                  where r.ID == obj.StoreID
                    //                  select r;

                    //    foreach (MasterListItem item in results)
                    //    {
                    //        cmbStore.SelectedItem = item;
                    //    }
                    //}

                    if (cmbStore.ItemsSource != null)
                    {
                        var results = from r in ((List<clsStoreVO>)cmbStore.ItemsSource)
                                      where r.StoreId == obj.StoreID
                                      select r;

                        foreach (clsStoreVO item in results)
                        {
                            cmbStore.SelectedItem = item;
                        }
                    }

                    if (cmbSupplier.ItemsSource != null)
                    {
                        var results1 = from r in ((List<MasterListItem>)cmbSupplier.ItemsSource)
                                       where r.ID == obj.SupplierID
                                       select r;

                        foreach (MasterListItem item in results1)
                        {
                            cmbSupplier.SelectedItem = item;
                        }
                    }

                    ((clsGRNVO)this.DataContext).ID = obj.ID;
                    ((clsGRNVO)this.DataContext).POID = obj.POID;

                    ((clsGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                    ((clsGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;

                    ((clsGRNVO)this.DataContext).PaymentModeID = obj.PaymentModeID;

                    if (obj.PaymentModeID == MaterPayModeList.Cash)
                    {
                        rdbCashMode.IsChecked = true;
                    }
                    else if (obj.PaymentModeID == MaterPayModeList.Credit)
                    {
                        rdbCreditMode.IsChecked = true;
                    }

                    if (obj.GRNType == InventoryGRNType.Direct)
                    {
                        rdbDirectPur.IsChecked = true;
                        dgAddGRNItems.Columns[5].Visibility = Visibility.Collapsed;
                        dgAddGRNItems.Columns[6].Visibility = Visibility.Collapsed;
                        dgAddGRNItems.Columns[7].Visibility = Visibility.Collapsed;
                        dgAddGRNItems.Columns[8].Visibility = Visibility.Collapsed;
                        dgAddGRNItems.Columns[4].Visibility = Visibility.Visible;

                        if (ISFromAproveGRN == true) //Added by Prashant Channe 27/10/2018, to make Cost Price Editable when items added for Direct PO on Approve
                        {
                            dgAddGRNItems.Columns[16].IsReadOnly = false;
                        }
                        else
                        {
                            dgAddGRNItems.Columns[16].IsReadOnly = true;
                        }
                        //dgAddGRNItems.Columns[18].IsReadOnly = true;  comented by Ashish z. for MRP Editable as per Client Requirement on 11-03-2016
                        dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;  //"VAT Amount"
                    }
                    else if (obj.GRNType == InventoryGRNType.AgainstPO)
                    {
                        rdbPo.IsChecked = true;
                        dgAddGRNItems.Columns[5].Visibility = Visibility.Visible;
                        dgAddGRNItems.Columns[6].Visibility = Visibility.Visible;
                        dgAddGRNItems.Columns[7].Visibility = Visibility.Visible;
                        dgAddGRNItems.Columns[8].Visibility = Visibility.Visible;
                        dgAddGRNItems.Columns[16].IsReadOnly = true;
                        //dgAddGRNItems.Columns[18].IsReadOnly = true;  comented by Ashish z. for MRP Editable as per Client Requirement on 11-03-2016
                        dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;  //"VAT Amount"
                    }
                    if (obj.IsConsignment == true)
                    {
                        chkConsignment.IsChecked = true;
                        chkConsignment.IsEnabled = false;
                    }
                    else
                    {
                        chkConsignment.IsChecked = false;
                        chkConsignment.IsEnabled = true;
                    }

                    ((clsGRNVO)this.DataContext).GatePassNo = obj.GatePassNo;
                    txtGateEntryNo.Text = Convert.ToString(obj.GatePassNo);

                    ((clsGRNVO)this.DataContext).InvoiceDate = obj.InvoiceDate;
                    dpInvDt.SelectedDate = obj.InvoiceDate;

                    ((clsGRNVO)this.DataContext).InvoiceNo = obj.InvoiceNo;
                    txtinvno.Text = Convert.ToString(obj.InvoiceNo);

                    ((clsGRNVO)this.DataContext).TotalAmount = obj.TotalAmount;
                    txttotcamt.Text = obj.TotalAmount.ToString("0.00");

                    ((clsGRNVO)this.DataContext).TotalCDiscount = obj.TotalCDiscount;
                    txtcdiscamt.Text = obj.TotalCDiscount.ToString();

                    ((clsGRNVO)this.DataContext).TotalSchDiscount = obj.TotalSchDiscount;
                    txthdiscamt.Text = obj.TotalSchDiscount.ToString();

                    ((clsGRNVO)this.DataContext).TotalTAxAmount = obj.TotalTAxAmount;
                    txttaxamount.Text = obj.TotalTAxAmount.ToString();

                    ((clsGRNVO)this.DataContext).TotalVAT = obj.TotalVAT;
                    txtVatamt.Text = String.Format("{0:0.00}", obj.TotalVAT);//obj.TotalVAT.ToString();

                    ((clsGRNVO)this.DataContext).NetAmount = obj.NetAmount;
                    txtNetamt.Text = obj.NetAmount.ToString();

                    ((clsGRNVO)this.DataContext).Freezed = obj.Freezed;
                    chkIsFinalized.IsChecked = obj.Freezed;

                    ((clsGRNVO)this.DataContext).Remarks = obj.Remarks;
                    txtremarks.Text = Convert.ToString(obj.Remarks);

                    if (cmbReceivedBy.ItemsSource != null)
                    {
                        var results2 = from r in ((List<MasterListItem>)cmbReceivedBy.ItemsSource)
                                       where r.ID == obj.ReceivedByID
                                       select r;

                        ((MasterListItem)cmbReceivedBy.SelectedItem).ID = obj.ReceivedByID;
                    }
                    ISEditMode = true;

                    //FillGRNAddDetailslList(((clsGRNVO)dgGRNList.SelectedItem).ID, ((clsGRNVO)dgGRNList.SelectedItem).Freezed, ((clsGRNVO)dgGRNList.SelectedItem).UnitId);

                    // Commented By CDS
                    //FillGRNItems(((clsGRNVO)dgGRNList.SelectedItem).ID, ((clsGRNVO)dgGRNList.SelectedItem).UnitId);
                    // END

                    //....Added By CDS 
                    FillGRNItems(obj.ID, obj.UnitId);
                    //....END

                    dgAddGRNItems.UpdateLayout();
                    dgAddGRNItems.Focus();


                    txtPONO.Text = String.Empty;

                    ////foreach (clsGRNDetailsVO item in ((List<clsGRNDetailsVO>)(dgGRNItems.ItemsSource)))
                    ////{
                    ////    if (!txtPONO.Text.Trim().Contains(item.PONO))
                    ////        txtPONO.Text = String.Format((txtPONO.Text.Trim() + "," + item.PONO.Trim()).Trim(','));
                    ////}

                    //SetCommandButtonState("Modify");


                    if (ISFromAproveGRN == true)
                    {
                        chkIsFinalized.IsChecked = SelectedGRN.Freezed;

                        //ViewAttachment(obj.FileName, obj.File);  // To  Bind (See) Image to View Invoice

                        if (obj.Freezed == true)
                        {
                            cmdSave.IsEnabled = false;
                            cmdDraft.IsEnabled = false;
                        }
                        else
                        {
                            cmdSave.IsEnabled = true;
                            cmdDraft.IsEnabled = true;
                        }
                    }
                    else
                    {
                        chkIsFinalized.IsChecked = ((clsGRNVO)dgGRNList.SelectedItem).Freezed;

                        if (((clsGRNVO)dgGRNList.SelectedItem).Freezed == true)
                        {
                            cmdSave.IsEnabled = false;
                            cmdDraft.IsEnabled = false;
                        }
                        else
                        {
                            cmdSave.IsEnabled = true;
                            cmdDraft.IsEnabled = true;
                        }
                    }
                    //....Added By CDS 
                    if (ISFromAproveGRN == true)
                    {
                        cmdSave.Content = "Approve"; //Modify   
                        cmdSave.IsEnabled = true;
                    }
                    IsFileAttached = true;
                    txtFileName.Text = obj.FileName;
                    //....END

                    _flip.Invoke(RotationType.Forward);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            clsGRNDetailsVO obj = (dgAddGRNItems.SelectedItem) as clsGRNDetailsVO;
            clsGRNDetailsVO objGRNDetails = obj.DeepCopy();
            string strMessage = String.Empty;
            if (obj != null && obj.IsBatchRequired == true)
            {
                if (GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.POID == obj.POID && z.UnitId == obj.UnitId && z.Quantity != null && z.Quantity == 0).Any())
                {
                    strMessage = "Please enter the Quantity first.";
                }
                else if (objGRNDetails != null && objGRNDetails.POPendingQuantity <= 0 && rdbDirectPur.IsChecked == false)
                {
                    strMessage = "Cannot add item cause pending quantity is zero.";
                }
                else if (rdbDirectPur.IsChecked == false)
                {
                    List<clsGRNDetailsVO> lstDetails = GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.POID == obj.POID && z.UnitId == obj.UnitId).ToList();
                    if (lstDetails.Count > 1)
                    {
                        double item = (from grndetail in lstDetails
                                       where grndetail.ItemID == obj.ItemID && grndetail.POID == obj.POID && grndetail.UnitId == obj.UnitId
                                       orderby grndetail.POPendingQuantity ascending
                                       select grndetail.POPendingQuantity).First();
                        if (item <= 0)
                        {
                            strMessage = "Cannot add item cause pending quantity is zero.";
                        }
                    }
                }
                if (String.IsNullOrEmpty(strMessage))
                {
                    List<clsGRNDetailsVO> lstDetails = GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.POID == obj.POID && z.UnitId == obj.UnitId).ToList();
                    if (lstDetails.Count > 1)
                    {
                        double item = (from grndetail in lstDetails
                                       where grndetail.ItemID == obj.ItemID && grndetail.POID == obj.POID && grndetail.UnitId == obj.UnitId
                                       orderby grndetail.POPendingQuantity ascending
                                       select grndetail.POPendingQuantity).First();
                        //objGRNDetails.PendingQuantity = item;
                        objGRNDetails.POPendingQuantity = item;

                    }
                    else
                        objGRNDetails.PendingQuantity = obj.PendingQuantity;
                    objGRNDetails.Quantity = 0;
                    objGRNDetails.BatchID = 0;
                    objGRNDetails.BatchCode = string.Empty;
                    objGRNDetails.ExpiryDate = null;
                    objGRNDetails.IsBatchAssign = false;
                    objGRNDetails.FreeQuantity = 0;
                    objGRNDetails.AvailableQuantity = 0;

                    #region Set SrNo

                    long mainCnt = 0;

                    var itemMax = GRNAddedItems.Max(x => x.SrNo);

                    if (itemMax != null)
                        mainCnt = itemMax;

                    mainCnt++;

                    objGRNDetails.SrNo = mainCnt;

                    //foreach (clsGRNDetailsVO itemMain in GRNAddedItems)
                    //{
                    //    if (itemMain.SrNo == 0)
                    //    {
                    //        mainCnt++;
                    //        itemMain.SrNo = mainCnt;
                    //    }
                    //}

                    #endregion


                    var item55 = from r55 in GRNPOAddedItems
                                 where r55.GRNDetailID == objGRNDetails.GRNDetailID
                                 select r55;

                    if (item55 != null && item55.ToList().Count > 0)
                    {
                        objGRNDetails.POID = (item55.FirstOrDefault()).POID.DeepCopy();
                        objGRNDetails.POUnitID = (item55.FirstOrDefault()).POUnitID.DeepCopy();
                        objGRNDetails.PODetailID = (item55.FirstOrDefault()).PODetailID.DeepCopy();
                        objGRNDetails.PODetailUnitID = (item55.FirstOrDefault()).PODetailUnitID.DeepCopy();
                        objGRNDetails.PoItemsID = (item55.FirstOrDefault()).PoItemsID.DeepCopy();
                    }


                    objGRNDetails.GRNDetailID = 0;
                    objGRNDetails.ID = 0;
                    objGRNDetails.GRNID = 0;
                    objGRNDetails.BarCode = "";
                    objGRNDetails.IsFromAddItem = true;
                    if (!((dgAddGRNItems.SelectedItem) as clsGRNDetailsVO).IsFromAddItem)
                        ((dgAddGRNItems.SelectedItem) as clsGRNDetailsVO).IsFromAddItem = true;


                    if (obj.IsBatchAssign == true)
                    {
                        objGRNDetails.BaseRate = objGRNDetails.BaseRateBeforAddItem;
                        objGRNDetails.MainRate = objGRNDetails.BaseRateBeforAddItem;
                        objGRNDetails.Rate = objGRNDetails.BaseRateBeforAddItem * objGRNDetails.BaseConversionFactor;

                        objGRNDetails.BaseMRP = objGRNDetails.BaseMRPBeforAddItem;
                        objGRNDetails.MainMRP = objGRNDetails.BaseMRPBeforAddItem;
                        objGRNDetails.MRP = objGRNDetails.BaseMRPBeforAddItem * objGRNDetails.BaseConversionFactor;

                        objGRNDetails.BaseAvailableQuantity = 0;
                        objGRNDetails.AvailableQuantity = 0;
                    }

                    GRNAddedItems.Add(objGRNDetails);

                    GRNPOAddedItems.Add(objGRNDetails);  // added by Ashish Z. for Deleting the Added Link Items. on 06042016



                    dgAddGRNItems.ItemsSource = GRNAddedItems.ToList();

                    //PCVData = new PagedCollectionView(GRNPOAddedItems);
                    //PCVData.GroupDescriptions.Add(new PropertyGroupDescription("PONO"));
                    //List<clsGRNDetailsVO> ob = new List<clsGRNDetailsVO>();
                    //ob = (List<clsGRNDetailsVO>)(PCVData).SourceCollection;
                    //dgAddGRNItems.ItemsSource = PCVData;

                    //dgAddGRNItems.UpdateLayout();
                    //if (!GRNAddedItems.Where(z => z.ItemID == objGRNDetails.ItemID && z.POID == objGRNDetails.POID && z.POUnitID == objGRNDetails.POUnitID).Any())
                    //    GRNPOAddedItems.Add(objGRNDetails);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.None);
                    msgWindow.Show();
                }
            }
        }

        private void chkConsignment_Click(object sender, RoutedEventArgs e)
        {
            if (chkConsignment.IsChecked == false)
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    if (GRNAddedItems.Where(z => z.IsConsignment).Any() == true)
                    {
                        GRNAddedItems = new List<clsGRNDetailsVO>();
                        dgAddGRNItems.ItemsSource = GRNAddedItems;

                    }
                }
            }
            else
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    if (GRNAddedItems.Where(z => z.IsBatchAssign).Any() == true)
                    {
                        if (GRNAddedItems.Where(z => z.IsConsignment).Any() == false)
                        {
                            GRNAddedItems = new List<clsGRNDetailsVO>();
                            dgAddGRNItems.ItemsSource = GRNAddedItems;

                        }
                    }
                }
            }
        }

        /// <summary>
        /// print button click
        /// </summary>
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                //string Parameters = "";
                long GunitID = 0;
                long ID = 0;
                InventoryGRNType GRNType;
                ID = ((clsGRNVO)dgGRNList.SelectedItem).ID;
                GunitID = ((clsGRNVO)dgGRNList.SelectedItem).UnitId;
                GRNType = ((clsGRNVO)dgGRNList.SelectedItem).GRNType;
                // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source,  "../Reports/InventoryPharmacy/GRNPrint.aspx?"+Parameters , "_blank");

                string URL = "../Reports/InventoryPharmacy/GRNPrint.aspx?ID=" + ID + "&UnitID=" + GunitID + "&GRNType=" + GRNType;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        /// <summary>
        /// Delete item click
        /// </summary>
        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxControl.MessageBoxChildWindow msgWD =
                   new MessageBoxControl.MessageBoxChildWindow("", "Are you sure you want to delete the item ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        clsGRNDetailsVO objSelectedGRNItem = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem); //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).DeepCopy();
                        clsGRNDetailsVO objGRNDetails = new clsGRNDetailsVO();
                        if (dgAddGRNItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        {
                            objGRNDetails = ((List<clsGRNDetailsVO>)((PagedCollectionView)(dgAddGRNItems.ItemsSource)).SourceCollection)[dgAddGRNItems.SelectedIndex];
                        }
                        else
                            objGRNDetails = ((List<clsGRNDetailsVO>)(dgAddGRNItems.ItemsSource))[dgAddGRNItems.SelectedIndex];
                        double qty = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity * ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor; //added by Ashish Z. double qty = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity;
                        long itemid = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

                        foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POID == objSelectedGRNItem.POID).ToList())
                        {
                            //item.POPendingQuantity = item.POQuantity - item.Quantity;
                            item.POPendingQuantity = (item.POQuantity * item.BaseConversionFactor) - item.Quantity * item.BaseConversionFactor;
                        }

                        GRNAddedItems.RemoveAt(dgAddGRNItems.SelectedIndex);
                        if (rdbPo.IsChecked == true)
                            GRNPOAddedItems.RemoveAt(dgAddGRNItems.SelectedIndex);
                        dgAddGRNItems.ItemsSource = null;

                        if (ISEditMode == true)
                            GRNDeletedMainItems.Add(objSelectedGRNItem);  // Use to maintain list of Deleted Main Items

                        //foreach (clsGRNDetailsVO item1 in GRNPOAddedItems)
                        //{
                        //    if (objGRNDetails.ItemID == item1.ItemID && objGRNDetails.POID == item1.POID && objGRNDetails.POUnitID == item1.POUnitID && objGRNDetails.PODetailID == item1.PODetailID && objGRNDetails.PODetailUnitID == item1.PODetailUnitID)
                        //    {
                        //        GRNPOAddedItems.Remove(item1);
                        //    }
                        //}   

                        foreach (var item in GRNAddedItems.Where(Z => Z.ItemID == itemid && Z.POID == objSelectedGRNItem.POID && Z.POUnitID == objSelectedGRNItem.POUnitID).ToList())
                        {
                            if (objGRNDetails.GRNID > 0)
                            {
                                item.POPendingQuantity = item.POPendingQuantity + qty;
                                item.ItemQuantity += qty;
                            }
                        }

                        ////Added by Ashish Z. on dated 150416
                        //if (GRNAddedItems != null && GRNAddedItems.Count() > 0 && rdbDirectPur.IsChecked == false)
                        //{
                        //    double Pendingqty = 0;
                        //    //Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID).ToList().Sum(x => (x.PendingQuantity));//.FirstOrDefault().PendingQuantity; // 
                        //    Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).First().PendingQuantity;
                        //    double itemQty = 0;
                        //    itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                        //    if (itemQty <= Pendingqty)
                        //    {
                        //        foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).ToList())
                        //        {
                        //            item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                        //        }
                        //    }
                        //}

                        if (GRNAddedItems != null && GRNAddedItems.Count() > 0 && rdbDirectPur.IsChecked == false)
                        {
                            double Pendingqty = 0;

                            foreach (var item in GRNAddedItems.ToList())
                            {
                                if (item.ItemID == objSelectedGRNItem.ItemID && item.POID == objSelectedGRNItem.POID && item.POUnitID == objSelectedGRNItem.POUnitID)
                                {
                                    Pendingqty = item.PendingQuantity;
                                    break;
                                }
                            }

                            //Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).First().PendingQuantity;
                            double itemQty = 0;
                            foreach (var item in GRNAddedItems.ToList())
                            {
                                if (item.ItemID == objSelectedGRNItem.ItemID && item.POID == objSelectedGRNItem.POID && item.POUnitID == objSelectedGRNItem.POUnitID)
                                {
                                    itemQty += item.Quantity * item.BaseConversionFactor;
                                }
                            }
                            //itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                            if (itemQty <= Pendingqty)
                            {
                                foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.POUnitID == objSelectedGRNItem.POUnitID).ToList())
                                {
                                    item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                                }
                            }
                        }

                        ////End

                        dgAddGRNItems.ItemsSource = GRNAddedItems;
                        dgAddGRNItems.UpdateLayout();
                        CalculateGRNSummary();
                        txtPONO.Text = String.Empty;

                        if (rdbPo.IsChecked == true)
                        {
                            foreach (clsGRNDetailsVO objDetail in GRNAddedItems)
                            {
                                if (!txtPONO.Text.Contains(objDetail.PONO))
                                    txtPONO.Text = String.Format(txtPONO.Text + "," + objDetail.PONO);
                            }
                            txtPONO.Text = txtPONO.Text.Trim(',');
                        }
                        //dgAddGRNItems.ItemsSource = GRNAddedItems;
                        //dgAddGRNItems.UpdateLayout();
                        //CalculateGRNSummary();
                        //txtPONO.Text = String.Empty;

                        #region Delete Free Item linked with Main Item

                        //clsGRNDetailsFreeVO objSelectedGRNItemFree = (clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem;
                        //clsGRNDetailsFreeVO objGRNDetailsFree = new clsGRNDetailsFreeVO();

                        //if (dgAddGRNFreeItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        //{
                        //    objGRNDetailsFree = ((List<clsGRNDetailsFreeVO>)((PagedCollectionView)(dgAddGRNFreeItems.ItemsSource)).SourceCollection)[dgAddGRNFreeItems.SelectedIndex];
                        //}
                        //else
                        //    objGRNDetailsFree = ((List<clsGRNDetailsFreeVO>)(dgAddGRNFreeItems.ItemsSource))[dgAddGRNFreeItems.SelectedIndex];

                        //double qty = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity * ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor; //added by Ashish Z. double qty = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity;
                        //long itemid = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID;

                        ////foreach (clsGRNDetailsFreeVO item in GRNAddedFreeItems.Where(z => z.FreeItemID == objSelectedGRNItemFree.FreeItemID && z.POID == objSelectedGRNItemFree.POID && z.POID == objSelectedGRNItemFree.POID).ToList())
                        ////{
                        ////    //item.POPendingQuantity = item.POQuantity - item.Quantity;
                        ////    item.POPendingQuantity = (item.POQuantity * item.BaseConversionFactor) - item.Quantity * item.BaseConversionFactor;
                        ////}

                        //GRNAddedFreeItems.RemoveAt(dgAddGRNFreeItems.SelectedIndex);

                        ////if (rdbPo.IsChecked == true)
                        ////    GRNPOAddedItems.RemoveAt(dgAddGRNFreeItems.SelectedIndex);

                        if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count > 0)
                        {
                            List<clsGRNDetailsFreeVO> GRNRemoveFreeItems = new List<clsGRNDetailsFreeVO>();
                            //clsGRNDetailsFreeVO ItemFreeRemove = null;

                            foreach (clsGRNDetailsFreeVO itemFree in GRNAddedFreeItems)  // To Delete Free Item linked with Main Item
                            {
                                if (itemFree.MainSrNo == objSelectedGRNItem.SrNo && itemFree.MainItemID == objSelectedGRNItem.ItemID)  //if (itemFree.MainItemID == objSelectedGRNItem.ItemID && itemFree.MainBatchCode == objSelectedGRNItem.BatchCode && itemFree.MainExpiryDate == objSelectedGRNItem.ExpiryDate && itemFree.MainItemMRP == objSelectedGRNItem.MRP && itemFree.MainItemCostRate == objSelectedGRNItem.CostRate)
                                {
                                    //ItemFreeRemove = itemFree;
                                    //break;

                                    GRNRemoveFreeItems.Add(itemFree);
                                }
                            }

                            //if (ItemFreeRemove != null)
                            //    GRNAddedFreeItems.Remove(ItemFreeRemove);

                            //if (ISEditMode == true)
                            //    GRNDeletedFreeItems.Add(ItemFreeRemove);  // Use to maintain list of Deleted Free Items

                            if (GRNRemoveFreeItems != null && GRNRemoveFreeItems.Count > 0)
                            {
                                foreach (clsGRNDetailsFreeVO itemRemove in GRNRemoveFreeItems)
                                {
                                    GRNAddedFreeItems.Remove(itemRemove);

                                    if (ISEditMode == true)
                                        GRNDeletedFreeItems.Add(itemRemove);  // Use to maintain list of Deleted Free Items
                                }
                            }

                            dgAddGRNFreeItems.ItemsSource = null;

                            dgAddGRNFreeItems.ItemsSource = GRNAddedFreeItems;
                            dgAddGRNFreeItems.UpdateLayout();
                            CalculateGRNSummary();
                            FillFreeMainItems();
                        }

                        #endregion

                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            };
            msgWD.Show();
        }

        private void SetFreeMainItems()
        {
            try
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    List<clsFreeMainItems> objMainList = new List<clsFreeMainItems>();
                    clsFreeMainItems objMain = new clsFreeMainItems();

                    objMain.ItemName = "- Select -";
                    objMainList.Add(objMain);

                    foreach (clsGRNDetailsVO itemMain in GRNAddedItems)
                    {

                        objMain = new clsFreeMainItems();

                        objMain.ItemID = itemMain.ItemID;
                        objMain.ItemName = itemMain.ItemName;
                        objMain.ItemCode = itemMain.ItemCode;
                        objMain.BatchID = itemMain.BatchID;
                        objMain.BatchCode = itemMain.BatchCode;
                        objMain.ExpiryDate = itemMain.ExpiryDate;
                        objMain.MRP = itemMain.MRP;
                        objMain.CostRate = itemMain.CostRate;

                        if (itemMain.IsBatchRequired == true)
                            objMain.ExpiryDateString = itemMain.ExpiryDate.Value.ToString("MMM-yyyy");

                        objMain.MainItemName = objMain.ItemName + " - " + objMain.BatchCode + " - " + objMain.ExpiryDateString;

                        objMainList.Add(objMain);
                    }

                    MainList = objMainList.DeepCopy();

                    cmbMainItem.ItemsSource = objMainList;
                    cmbMainItem.SelectedValue = (long)0;

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNItems.SelectedItem != null)
            {
                BarcodeForm win = new BarcodeForm();
                string date;
                long ItemID = 0;
                string BatchID = null;
                string BatchCode = null;
                string ItemCode = null;
                // ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemID;
                string ItemName = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemName;

                if (dgGRNItems.SelectedItem != null)
                {
                    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate != null)
                    {
                        date = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate.Value.ToShortDateString();
                    }
                    else
                        date = null;
                }
                else
                {
                    date = null;
                }
                if (dgGRNItems.SelectedItem != null)
                {
                    ItemID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemID;
                    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode != null)
                    {
                        string str = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode;
                        BatchID = Convert.ToString(((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchID);
                        BatchCode = str.Substring(0, 3) + "/" + BatchID.ToString() + "B";
                    }
                    else
                    {
                        BatchID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchID + "BI";
                    }
                }

                //if (BatchCode == null && BatchID == null)
                //{
                //    ItemID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ID;
                //    string str1 = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemCode;
                //    ItemCode = str1.Substring(0, 4) + "I";
                //}


                //if (BatchCode != null)
                //    win.PrintData = "*" + ItemID.ToString() + "-" + BatchCode + "*";
                //else
                //{
                //    if (BatchCode == null && BatchID != null)
                //    {
                //        win.PrintData = "*" + ItemID.ToString() + "-" + BatchID + "*";
                //    }
                //    else
                //    {
                //        win.PrintData = "*" + ItemID.ToString() + "-" + ItemCode + "*";
                //    }
                //}

                win.PrintData = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BarCode;
                win.PrintItem = ItemName;
                win.PrintDate = date;

                win.PrintFrom = "GRN";
                win.GRNItemDetails = (clsGRNDetailsVO)dgGRNItems.SelectedItem;

                win.Show();


                #region Old Code
                //BarcodeForm win = new BarcodeForm();
                //string date;
                //long ItemID = 0;
                //string BatchID = null;
                //string BatchCode = null;
                //string ItemCode = null;
                //// ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemID;
                //string ItemName = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemName;

                //if (dgGRNItems.SelectedItem != null)
                //{
                //    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate != null)
                //    {
                //        date = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate.Value.ToShortDateString();
                //    }
                //    else
                //        date = null;
                //}
                //else
                //{
                //    date = null;
                //}
                //if (dgGRNItems.SelectedItem != null)
                //{
                //    ItemID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemID;
                //    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode != null)
                //    {
                //        string str = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchCode;
                //        BatchID = Convert.ToString(((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchID);
                //        BatchCode = str.Substring(0, 3) + "/" + BatchID.ToString() + "B";
                //    }
                //    else
                //    {
                //        BatchID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BatchID + "BI";
                //    }
                //}

                //if (BatchCode == null && BatchID == null)
                //{
                //    ItemID = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ID;
                //    string str1 = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemCode;
                //    ItemCode = str1.Substring(0, 4) + "I";
                //}


                //if (BatchCode != null)
                //    win.PrintData = "*" + ItemID.ToString() + "-" + BatchCode + "*";
                //else
                //{
                //    if (BatchCode == null && BatchID != null)
                //    {
                //        win.PrintData = "*" + ItemID.ToString() + "-" + BatchID + "*";
                //    }
                //    else
                //    {
                //        win.PrintData = "*" + ItemID.ToString() + "-" + ItemCode + "*";
                //    }
                //}
                //win.PrintItem = ItemName;
                //win.PrintDate = date;

                //win.PrintFrom = "GRN";
                //win.GRNItemDetails = (clsGRNDetailsVO)dgGRNItems.SelectedItem;

                //win.Show();
                #endregion
            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();
            }
        }
        #endregion

        #region Private Methods
        // User Rights Wise Fill Store 
        private void FillStores(long pClinicID)
        {
            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            clsItemMasterVO obj = new clsItemMasterVO();
            obj.RetrieveDataFlag = false;
            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionObj.ToStoreList = new List<clsStoreVO>();
            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizActionObj.ItemMatserDetails = ((clsGetStoreDetailsBizActionVO)args.Result).ItemMatserDetails;
                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    clsStoreVO select = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true };
                    BizActionObj.ItemMatserDetails.Insert(0, select);
                    BizActionObj.ToStoreList.Insert(0, Default);
                    var result = from item in BizActionObj.ItemMatserDetails
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.Status == true
                                 select item;

                    var WithoutQSAndUserDefinedStores = from item in BizActionObj.ToStoreList
                                                        where item.IsQuarantineStore == false
                                                        select item;

                    WithoutQSAndUserDefinedStores.ToList().Insert(0, Default); ;

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
                    {
                        if (WithoutQSAndUserDefinedStores.ToList().Count > 0)
                        {
                            //cmbSearchStore.ItemsSource = result.ToList();
                            //cmbSearchStore.SelectedItem = result.ToList()[0];
                            cmbStore.ItemsSource = WithoutQSAndUserDefinedStores.ToList();
                            cmbStore.SelectedItem = WithoutQSAndUserDefinedStores.ToList()[0];
                        }
                    }
                    else
                    {
                        if (WithoutQSAndUserDefinedStores.ToList().Count > 0)
                        {
                            //cmbSearchStore.ItemsSource = BizActionObj.ToStoreList.ToList();
                            //cmbSearchStore.SelectedItem = BizActionObj.ToStoreList.ToList()[0];
                            cmbStore.ItemsSource = WithoutQSAndUserDefinedStores.ToList();
                            cmbStore.SelectedItem = WithoutQSAndUserDefinedStores.ToList()[0];
                        }
                    }

                    // Newly Added By CDS  25/12/2015
                    FillReceivedByList();
                }
            };
            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        // User Rights Wise Fill Store 


        /// <summary>
        /// Fills supplier
        /// </summary>
        private void FillSupplier()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsGST = true; //Added By Bhushan For GST Get StateID 24062017
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

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    foreach (var item in objList)
                    {
                        item.Description = item.Code + " - " + item.Description;
                    }
                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbSupplier.ItemsSource = null;
                    cmbSupplier.ItemsSource = objList;

                    cmbSearchSupplier.ItemsSource = null;
                    cmbSearchSupplier.ItemsSource = objList;


                    cmbSupplier.SelectedItem = objList[0];
                    cmbSearchSupplier.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbSupplier.SelectedValue = ((clsGRNVO)this.DataContext).SupplierID;
                        cmbSearchSupplier.SelectedValue = ((clsGRNVO)this.DataContext).SupplierID;

                    }

                    // Newly Added By CDS on 25/12/2015
                    if (ISFromAproveGRN == true)
                    {
                        Update_Click(null, null);
                        //FillNewGRNFromApproveGRN();
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region DataGrid CellEdit Events
        /// <summary>
        /// Update the Pending quantity on Quantity Change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellEditChanges(object sender, DataGridCellEditEndedEventArgs e, double lngPOPendingQuantity)
        {

            clsGRNDetailsVO objSelectedGRNItem = (clsGRNDetailsVO)dgAddGRNItems.SelectedItem;

            double itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));

            double Pendingqty = 0;
            if (objSelectedGRNItem.GRNID != 0 && chkIsFinalized.IsChecked == false)
            {
                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;
            }
            else
                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;

            if (itemQty <= Pendingqty)
            {
                foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                {
                    item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                }
            }
            else
            {

                objSelectedGRNItem.Quantity = 0;
                itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();
                foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                {
                    item.POPendingQuantity = Pendingqty - itemQty; //(itemQty * item.BaseConversionFactor);
                }
            }
        }

        void dgAddGRNItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            DataGrid dgItems = (DataGrid)sender;
            clsGRNDetailsVO obj = (clsGRNDetailsVO)dgAddGRNItems.SelectedItem;
            double lngPOPendingQuantity = 0;
            Boolean blnAddItem = false;
            if (dgAddGRNItems.SelectedItem != null)
            {
                if (e.Column.Header != null)
                {
                    if (e.Column.Header.ToString().Equals("Free Received Quantity") || e.Column.Header.ToString().Equals("Cost Price")) //(e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price"))  Free Received Quantity
                    {
                        //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP > 0 && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP < ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate)
                        if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP > 0 && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP <= ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate)  // Added by Prashant Channe on 09/01/2019, to always validate MRP > Rate
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "MRP should be greater than rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;

                            //Start: Added by Prashant Channe on 12/24/2018
                            if (dgAddGRNItems.SelectedItem != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor > 0)
                            {
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseRate = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                            }
                            //End: Added by Prashant Channe on 12/24/2018

                            return;
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 114 : Free Quantity changed or Cost Price Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Cost Price : " + Convert.ToString(obj.Rate) + " "
                                                                  + "FreeQuantity : " + Convert.ToString(obj.FreeQuantity) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //

                    }
                    if (e.Column.Header.ToString().Equals("MRP"))
                    {
                        if (obj.MRP > 0 && obj.ConversionFactor > 0)
                        {
                            obj.UnitMRP = obj.MRP;
                        }
                        //Added By CDS

                        //if (obj.MRP > 0 && obj.ConversionFactor > 0)
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;
                        //}

                        if (e.Column.Header.ToString().Equals("MRP"))
                        {
                            if (dgAddGRNItems.SelectedItem != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor > 0)
                            {
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;

                            }
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 115 : MRP changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }

                    if (e.Column.Header.ToString().Equals("Received Quantity"))//(e.Column.Header.ToString().Equals("Quantity"))
                    {
                        lngPOPendingQuantity = obj.POPendingQuantity;
                        //if (lngPOPendingQuantity > 0)
                        //{
                        if (GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.UnitId == obj.UnitId && z.POID == obj.POID).Count() > 1)
                        {
                            if (rdbDirectPur.IsChecked != true)
                            {
                                CellEditChanges(sender, e, lngPOPendingQuantity);
                                blnAddItem = true;
                            }
                        }
                        //}
                        //else
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //             new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    obj.Quantity = 0;
                        //    return;
                        //}

                        if (rdbDirectPur.IsChecked == false && obj.Quantity * obj.BaseConversionFactor == 0 && obj.FreeQuantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Quantity In The List Can't Be Zero. Please enter Quantity or Free Received Quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();

                            obj.PendingQuantity = obj.PendingQuantity;
                            return;
                        }
                        else if (obj.Quantity * obj.BaseConversionFactor == 0 && obj.FreeQuantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Quantity In The List Can't Be Zero. Please enter Quantity or Free Received Quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            return;
                        }

                        if (!blnAddItem)
                        {
                            //By Anjali........................................................
                            //if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //    msgW3.Show();
                            //    obj.Quantity = 0;
                            //    obj.PendingQuantity = obj.PendingQuantity;
                            //    //CalculateGRNSummary();
                            //    return;
                            //}
                            //if ((((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //    msgW3.Show();
                            //    obj.Quantity = 0;
                            //    obj.PendingQuantity = obj.PendingQuantity;
                            //    //CalculateGRNSummary();
                            //    return;
                            //}

                            if (obj.GRNPendItemQuantity > 0)
                            {
                                //if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor))) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                                //{
                                //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                //    msgW3.Show();
                                //    obj.Quantity = 0;
                                //    obj.PendingQuantity = obj.PendingQuantity;
                                //    //CalculateGRNSummary();
                                //    return;
                                //}
                                if ((obj.PendingQuantity < ((obj.GRNPendItemQuantity - obj.GRNDetailsViewTimeQty) + (obj.Quantity * obj.BaseConversionFactor))) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                                //else if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }


                                if (obj.GRNApprItemQuantity > 0 && (((obj.PendingQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }
                                else if ((((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }
                            }
                            else
                            {
                                if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }
                                else if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor))) && (((obj.PendingQuantity + obj.GRNApprItemQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }

                                if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                                //if (obj.GRNApprItemQuantity > 0 && (((obj.PendingQuantity) < (obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }
                                else if ((((obj.PendingQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //CalculateGRNSummary();
                                    return;
                                }
                            }



                            //if ((obj.PendingQuantity < obj.GRNApprItemQuantity + obj.GRNPendItemQuantity) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //    msgW3.Show();
                            //    obj.Quantity = 0;
                            //    obj.PendingQuantity = obj.PendingQuantity;
                            //    //CalculateGRNSummary();
                            //    return;
                            //}


                            //if ((obj.PendingQuantity - obj.GRNApprItemQuantity + obj.GRNPendItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //    msgW3.Show();
                            //    obj.Quantity = 0;
                            //    obj.PendingQuantity = obj.PendingQuantity;
                            //    //CalculateGRNSummary();
                            //    return;
                            //}

                            if (obj.ItemQuantity > 0)
                            {
                                if ((obj.ItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                    msgW3.Show();

                                    //obj.Quantity = obj.ItemQuantity;
                                    obj.Quantity = 0;
                                    obj.PendingQuantity = obj.PendingQuantity;
                                    //obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                                    return;
                                }
                                else
                                    obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                                //obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity;
                            }

                            else if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                //obj.Quantity = obj.POPendingQuantity;
                                obj.Quantity = 0;
                                obj.PendingQuantity = obj.PendingQuantity;
                                //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                                //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                                CalculateGRNSummary();
                                return;
                            }
                            else
                            {
                                if (rdbDirectPur.IsChecked != true)
                                {
                                    //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                                    obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                                }

                            }
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 116 : Quantity changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Quantity : " + Convert.ToString(obj.Quantity) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //

                    }
                    if (e.Column.Header.ToString().Equals("Batch Code"))
                    {
                        ////PreviousBatchValue = obj.BatchCode;
                        //if (PreviousBatchValue != obj.BatchCode)
                        //{
                        //    obj.BarCode = obj.BatchCode;
                        //}

                        //CalculateGRNSummary();
                        //CalculateNewNetAmount();

                        ////added by rohinee dated 28/9/2016 for audit trail
                        //if (IsAuditTrail)
                        //{
                        //    LogInformation = new LogInfo();
                        //    LogInformation.guid = objGUID;
                        //    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        //    LogInformation.TimeStamp = DateTime.Now;
                        //    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        //    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        //    LogInformation.Message = " 117 : Batch Code changed : " //+ Convert.ToString(lineNumber)
                        //                                          + "ItemName : " + Convert.ToString(obj.ItemName) + " "
                        //                                          + "ItemID : " + Convert.ToString(obj.ItemID) + " "
                        //                                          + "BarCode : " + Convert.ToString(obj.BarCode) + " "
                        //                                           + "BatchCode : " + Convert.ToString(obj.BatchCode) + " "
                        //                                          + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                        //                                          + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                        //                                          + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                        //                                          + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                        //                                          + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                        //                                          + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                        //                                          + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                        //                                          + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                        //                                          + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                        //                                          ;
                        //    LogInfoList.Add(LogInformation);
                        //}
                        ////
                    }
                    if (e.Column.Header.ToString().Equals("C.Disc. %"))
                    {
                        if (obj.CDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Concession Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();

                            obj.CDiscountPercent = 0;
                            return;
                        }
                        if (obj.NetAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Discount Amount must be less than Net Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CDiscountPercent = 0;
                            return;
                        }

                        if (obj.CDiscountPercent == 100)
                        {

                            if (obj.SchDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter C. Discount Percentage! Sch.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();

                                obj.CDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }
                        if (obj.CDiscountPercent >= 0 && obj.CDiscountPercent < 100)
                        {
                            if (obj.SchDiscountPercent == 100)
                            {
                                obj.CDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }

                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 118 : C.Disc. % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "ItemTax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VATPercent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "SchDiscountPercent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "CDiscountPercent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "NetAmount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }
                    if (e.Column.Header.ToString().Equals("Sch.Disc. %"))
                    {
                        if (obj.SchDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Sch. Discount Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SchDiscountPercent = 0;
                            return;
                        }
                        if (obj.NetAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Discount Amount must be less than Net Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SchDiscountPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter Sch. Discount Percentage! C.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.SchDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }

                        }
                        if (obj.SchDiscountPercent >= 0 && obj.SchDiscountPercent < 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                obj.SchDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 119 : Sch.Disc. % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }
                    if (e.Column.Header.ToString().Equals("VAT%"))
                    {
                        if (obj.VATPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.VATPercent = 0;
                            return;
                        }
                        if (obj.VATPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.VATPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.VATPercent > 0)
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }

                        // Added BY CDS
                        //if (obj.VATPercent > 0 )
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    //obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;

                        //    //obj.VATAmount = (obj.MRP) - (((obj.MRP) / ((obj.VATPercent) + 100)) * 100);

                        //    obj.VATAmount = (obj.Amount) - (((obj.Amount) / (obj.VATPercent + 100)) * 100);

                        //}
                        //if (obj.VATPercent == 0)
                        //{
                        //    obj.VATAmount = 0;
                        //}

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 119 : VAT% changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //


                    }

                    // Added By CDS to CST Tax %
                    //if (e.Column.Header.ToString().Equals("CST Tax %"))

                    if (e.Column.Header.ToString().Equals("Item Tax %"))
                    {
                        if (obj.ItemTax > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "ItemTax Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ItemTax = 0;
                            return;
                        }
                        if (obj.ItemTax < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "ItemTax Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ItemTax = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.VATPercent > 0)
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }

                        // Added BY CDS
                        //if (obj.ItemTax > 0)
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    //obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;

                        //    //obj.TaxAmount = (obj.MRP) - (((obj.MRP) / ((obj.ItemTax) + 100)) * 100);

                        //    obj.TaxAmount = (obj.Amount) - (((obj.Amount) / ((obj.ItemTax) + 100)) * 100);
                        //}
                        //if (obj.ItemTax == 0)
                        //{
                        //    obj.TaxAmount = 0;
                        //}
                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 120 : Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //

                    }
                    //END

                    if (e.Column.Header.ToString().Equals("SGST %"))
                    {
                        if (obj.SGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "SGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SGSTPercent = 0;
                            return;
                        }
                        if (obj.SGSTPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "SGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.SGSTPercent > 0)
                            {
                                if (obj.SGSTPercent != 0)
                                {
                                    orgVatPer = obj.SGSTPercent;
                                    obj.SGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                    }

                    if (e.Column.Header.ToString().Equals("CGST %"))
                    {
                        if (obj.CGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "CGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CGSTPercent = 0;
                            return;
                        }
                        if (obj.CGSTPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "CGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.CGSTPercent > 0)
                            {
                                if (obj.CGSTPercent != 0)
                                {
                                    orgVatPer = obj.CGSTPercent;
                                    obj.CGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                    }
                    if (e.Column.Header.ToString().Equals("IGST %"))
                    {
                        if (obj.IGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "IGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.IGSTPercent = 0;
                            return;
                        }
                        if (obj.IGSTAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "IGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.IGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.IGSTPercent > 0)
                            {
                                if (obj.IGSTPercent != 0)
                                {
                                    orgVatPer = obj.IGSTPercent;
                                    obj.IGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                    }
                    if (e.Column.Header.ToString().Equals("Cost Price"))
                    {
                        if (obj != null)
                        {
                            if (obj.Rate == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Rate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                return;
                            }
                            if (obj.Rate < 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Negative Cost Price.", "Cost Price Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.Rate = 0;
                                return;
                            }
                            else if (obj.Rate > 0 && obj.ConversionFactor > 0)
                            {
                                obj.UnitRate = obj.Rate / obj.ConversionFactor;
                            }

                            //Start: Added by Prashant Channe on 12/24/2018
                            if (dgAddGRNItems.SelectedItem != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor > 0)
                            {
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseRate = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                            }
                            //End: Added by Prashant Channe on 12/24/2018

                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 121 : Cost Price changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }

                    //   if (e.Column.Header.ToString().Equals("Batch Code") || e.Column.Header.ToString().Equals("Expiry Date"))
                    if (e.Column.Header.ToString().Equals("Batch Code"))
                    {
                        if (obj != null)
                        {
                            foreach (var item in GRNAddedItems)
                            {
                                if (item.ItemID == obj.ItemID)
                                {
                                    if (item.IsBatchRequired == true)
                                    {
                                        if (obj.BatchCode == string.Empty || obj.BatchCode == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            return;

                                        }
                                        //      else if (obj.ExpiryDate == null)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          return;

                                        //      }
                                        //      else if (obj.ExpiryDate < DateTime.Now.Date)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date must not be less than today's date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          obj.ExpiryDate = DateTime.Now.Date;
                                        //      }
                                    }

                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 122 : Batch Code changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "BatchCode : " + Convert.ToString(obj.BatchCode) + " "
                                                                  + "Batch ID : " + Convert.ToString(obj.BatchID) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }


                    if (e.Column.Header.ToString().Equals("Expiry Date"))
                    {
                        if (obj != null)
                        {
                            foreach (var item in GRNAddedItems)
                            {
                                if (item.ItemID == obj.ItemID)
                                {
                                    if (item.IsBatchRequired == true)
                                    {
                                        //      if (obj.BatchCode == string.Empty || obj.BatchCode == null)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          return;

                                        //      }
                                        if (obj.ExpiryDate == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                  new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            return;
                                        }
                                        else if (obj.ExpiryDate < DateTime.Now.Date)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date must not be less than today's date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            obj.ExpiryDate = DateTime.Now.Date;
                                        }
                                        else if (obj.ExpiryDate > DateTime.Now.AddYears(3))
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date is greater than three years!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            return;
                                        }

                                        if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays > 0)
                                        {
                                            TimeSpan day = item.ExpiryDate.Value - DateTime.Now;
                                            int Day1 = (int)day.TotalDays;
                                            Int64 ExpiredDays = item.ItemExpiredInDays;
                                            if (Day1 < ExpiredDays)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item has Expiry date less than" + ExpiredDays + " days !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                msgW3.Show();
                                                //isValid = false;
                                                //return isValid;
                                            }
                                        }
                                        else if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays == 0)
                                        {

                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Item does not have Expiry In Days At Master Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                        }
                                    }

                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 123 : Expiry Date changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "BatchCode : " + Convert.ToString(obj.BatchCode) + " "
                                                                  + "Batch ID : " + Convert.ToString(obj.BatchID) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "ExpiryDate: " + Convert.ToString(obj.ExpiryDate) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }


                    if (e.Column.Header.ToString().Equals("Conversion Factor"))
                    {
                        if (obj.ConversionFactor <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Conversion factor must be greater than the zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ConversionFactor = 1;
                        }
                        else if (obj.ConversionFactor != null && obj.ConversionFactor > 0)
                        {
                            obj.MRP = obj.UnitMRP;
                            obj.Rate = obj.UnitRate;
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 124 : Conversion Factor changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Unit Rate : " + Convert.ToString(obj.UnitRate) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "Conversion Factor: " + Convert.ToString(obj.ConversionFactor) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //

                    }

                    if (e.Column.Header.ToString().Equals("Received Quantity")) //(e.Column.Header.ToString().Equals("Quantity"))    // if (e.Column.DisplayIndex == 9)
                    {

                        if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList == null || ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList.Count == 0)
                        {
                            //DataGridColumn column = dgAddGRNItems.Columns[11];
                            //FrameworkElement fe = column.GetCellContent(e.Row);
                            //if (fe != null)
                            //{
                            //    //DataGridCell cell = (DataGridCell)result;
                            //    FillUOMConversions();
                            //}

                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity) * ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;

                            //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity > ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableStockInBase)
                            //{
                            //    //float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;
                            //    //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase / ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor)));
                            //    //string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                            //    //ConversionsForAvailableStock();
                            //    //MessageBoxControl.MessageBoxChildWindow msgWD =
                            //    //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //    //msgWD.Show();
                            //}
                            //else 
                            if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                //obj.Quantity = obj.POPendingQuantity;
                                obj.Quantity = 0;
                                obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                                //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                                CalculateGRNSummary();
                                return;
                            }
                        }
                        else if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM.ID > 0)
                        {
                            ////((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                            //////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                            //CalculateConversionFactor(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);
                            // Function Parameters
                            // FromUOMID - Transaction UOM
                            // ToUOMID - Stocking UOM
                            // if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList.Count > 0)
                            CalculateConversionFactorCentral(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM.ID, ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOMID);
                            // else
                            // ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity * ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor);
                        }
                        else
                        {
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity = 0;
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SingleQuantity = 0;

                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = 0;
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor = 0;

                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP;
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate;
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {

                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 125 : Quantity Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.ItemID) + " "
                                                                  + "Unit Rate : " + Convert.ToString(obj.UnitRate) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Single Quantity : " + Convert.ToString(obj.SingleQuantity) + " "
                                                                  + "Quantity : " + Convert.ToString(obj.Quantity) + " "
                                                                  + "Conversion Factor: " + Convert.ToString(obj.ConversionFactor) + " "
                                                                  + "Base Conversion Factor: " + Convert.ToString(obj.BaseConversionFactor) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "PO Pending Quantity : " + Convert.ToString(obj.POPendingQuantity) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }


                    //if (e.Column.Header.ToString().Equals("PO Pending Quantity") || e.Column.Header.ToString().Equals("Quantity") || e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price") || e.Column.Header.ToString().Equals("Amount")
                    //   || e.Column.Header.ToString().Equals("C.Disc. Amount") || e.Column.Header.ToString().Equals("Sch.Disc. %") || e.Column.Header.ToString().Equals("C.Disc. %"))

                }
            }
        }
        #endregion


        #region Set Command Button State New/Save/Modify/Print
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdDraft.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    txtFileName.Text = "";
                    IsFileAttached = false;
                    break;

                case "Save":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdDraft.IsEnabled = false;
                    cmdSave.Content = "Save";
                    cmdCancel.IsEnabled = false;
                    txtFileName.Text = "";
                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true; //false;
                    cmdSave.Content = "Save"; //"Modify";
                    cmdDraft.IsEnabled = true;  //false;
                    cmdCancel.IsEnabled = true;
                    txtFileName.Text = "";
                    break;

                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdSave.Content = "Save";
                    cmdDraft.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    txtFileName.Text = "";
                    IsFileAttached = false;
                    break;

                default:
                    break;
            }
        }

        #endregion

        /// <summary>
        /// Fills store according to clinicid
        /// </summary>
        /// OLD
        //private void FillStores(long pClinicID)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_Store;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    if (pClinicID > 0)
        //    {
        //        BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //            //cmbBloodGroup.ItemsSource = null;
        //            //cmbBloodGroup.ItemsSource = objList;
        //            cmbStore.ItemsSource = null;
        //            cmbStore.ItemsSource = objList;

        //            //if (objList.Count > 1)
        //            //    cmbStore.SelectedItem = objList[1];
        //            //else
        //            cmbStore.SelectedItem = objList[0];
        //            if (this.DataContext != null)
        //            {
        //                cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;


        //            }

        //            // Newly Added By CDS  25/12/2015
        //            FillReceivedByList();
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    Client.CloseAsync();
        //}
        /// OLD

        ItemSearchRowForGRN _ItemSearchRowControl = null;
        private void AttachItemSearchControl(long StoreID, long SupplierID)
        {
            //   BdrItemSearch.Visibility = Visibility.Visible;
            ItemSearchStackPanel.Children.Clear();
            _ItemSearchRowControl = new ItemSearchRowForGRN(StoreID, SupplierID);
            _ItemSearchRowControl.OnQuantityEnter_Click += new RoutedEventHandler(_ItemSearchRowControl_OnQuantityEnter_Click);
            // _ItemSearchRowControl.SetFocus();
            ItemSearchStackPanel.Children.Add(_ItemSearchRowControl);
        }

        void _ItemSearchRowControl_OnQuantityEnter_Click(object sender, RoutedEventArgs e)
        {
            ItemSearchRowForGRN _ItemSearchRowControl = (ItemSearchRowForGRN)sender;
            if (GRNAddedItems != null)
            {
                if (GRNAddedItems.Count.Equals(0))
                {
                    GRNAddedItems = new List<clsGRNDetailsVO>();
                }
            }
            else
            {
                GRNAddedItems = new List<clsGRNDetailsVO>();
            }


            _ItemSearchRowControl.SelectedItems[0].RowID = GRNAddedItems.Count + 1;




            #region non Editable row
            //if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
            //{
            //    DataGridColumn column = dgAddGRNItems.Columns[2];

            //    FrameworkElement fe = column.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            //    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            //    if (result != null)
            //    {
            //        DataGridCell cell = (DataGridCell)result;
            //        cell.IsEnabled = false;
            //    }
            //}
            if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
            {
                _ItemSearchRowControl.SelectedItems[0].IsReadOnly = true;
            }
            else
            {
                _ItemSearchRowControl.SelectedItems[0].IsReadOnly = false;
            }




            DataGridColumn column2 = dgAddGRNItems.Columns[3];

            FrameworkElement fe1 = column2.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

            if (result1 != null)
            {
                DataGridCell cell = (DataGridCell)result1;
                cell.IsEnabled = false;
            }

            DataGridColumn column3 = dgAddGRNItems.Columns[8];
            FrameworkElement fe2 = column3.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

            if (result2 != null)
            {
                DataGridCell cell = (DataGridCell)result2;
                cell.IsEnabled = false;
            }

            DataGridColumn column4 = dgAddGRNItems.Columns[11];
            FrameworkElement fe4 = column4.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

            if (result3 != null)
            {
                DataGridCell cell = (DataGridCell)result3;
                cell.IsEnabled = false;
            }

            DataGridColumn column5 = dgAddGRNItems.Columns[12];
            FrameworkElement fe5 = column5.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

            if (result4 != null)
            {
                DataGridCell cell = (DataGridCell)result4;
                cell.IsEnabled = false;
            }



            #endregion
            bool IsItemAlreadyAdded = false;
            foreach (var item in GRNAddedItems)
            {
                if (item.ItemCode == _ItemSearchRowControl.SelectedItems[0].ItemCode && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                    IsItemAlreadyAdded = true;
                else
                    IsItemAlreadyAdded = false;
                ////By Anjali..........
                //item.BarCode = item.BatchCode;
                ////...........................


            }
            if (IsItemAlreadyAdded == false)
            {
                //By Anjali...............
                //By Somnath...............
                //if (GRNAddedItems != null && GRNAddedItems.Count() > 0)
                //{
                //    foreach (var item in GRNAddedItems)
                //    {
                //        if (item.ItemID == _ItemSearchRowControl.SelectedItems[0].ItemID && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                //        {
                //            _ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                //            GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);
                //        }
                //    }
                //}
                //else
                //{
                //    _ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                //    GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);
                //}
                //_ItemSearchRowControl.SelectedItems[0].BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                GRNAddedItems.Add(_ItemSearchRowControl.SelectedItems[0]);
                if (_ItemSearchRowControl.SelectedItems[0].BarCode == null)
                {
                    foreach (var item in GRNAddedItems)
                    {
                        if (item.ItemID == _ItemSearchRowControl.SelectedItems[0].ItemID && item.BatchCode == _ItemSearchRowControl.SelectedItems[0].BatchCode)
                        {
                            //item.BarCode = _ItemSearchRowControl.SelectedItems[0].BatchCode;
                        }
                    }
                }


                //.......................

                if (rdbDirectPur.IsChecked == true)
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Collapsed;  //"VAT Amount"
                }
                else
                {
                    dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;  //"VAT Amount"
                }

                dgAddGRNItems.ItemsSource = null;
                dgAddGRNItems.ItemsSource = GRNAddedItems;
                dgAddGRNItems.UpdateLayout();
                // CalculateTotal();
                CalculateGRNSummary();
                //_ItemSearchRowControl.SetFocus();
                //_ItemSearchRowControl.cmbItemName.Focus();
                if (_ItemSearchRowControl.SelectedItems[0].IsBatchRequired == false)
                {
                    set();
                }

                _ItemSearchRowControl.SetFocus();
                _ItemSearchRowControl = null;
                _ItemSearchRowControl = new ItemSearchRowForGRN();
                _ItemSearchRowControl.cmbItemName.Focus();

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item with same batch already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                _ItemSearchRowControl.SetFocus();

            }
        }


        private void set()
        {

            DataGridColumn column = dgAddGRNItems.Columns[2];

            FrameworkElement fe = column.GetCellContent(_ItemSearchRowControl.SelectedItems[0]);
            FrameworkElement result = GetParent(fe, typeof(DataGridCell));

            if (result != null)
            {
                DataGridCell cell = (DataGridCell)result;
                cell.IsEnabled = false;
            }
        }
        /// <summary>
        /// Fills Received by
        /// </summary>
        private void FillReceivedByList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.IsDecode = false;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    var result = from item in objList
                                 where item.ID == ((IApplicationConfiguration)App.Current).CurrentUser.ID
                                 select item;


                    cmbReceivedBy.ItemsSource = null;
                    cmbReceivedBy.ItemsSource = objList;
                    //cmbReceivedBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    //((clsGetUserMasterListBizActionVO)(cmbReceivedBy.SelectedItem)).ID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    cmbReceivedBy.SelectedItem = result.ToList()[0];

                    // Newly Added By CDS  25/12/2015
                    FillSupplier();
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        void win_OnOkButtonClick(object sender, RoutedEventArgs e)
        {


        }

        private void dgDirectPurchase_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txttotcamt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        /// <summary>
        /// Fills GRN Details List according to GRNID
        /// </summary>

        private void FillGRNDetailslList(long pGRNID)
        {
            clsGetGRNDetailsListBizActionVO BizAction = new clsGetGRNDetailsListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = ((clsGRNVO)dgGRNList.SelectedItem).UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();
                    List<clsGRNDetailsFreeVO> objFreeList = new List<clsGRNDetailsFreeVO>();

                    objList = ((clsGetGRNDetailsListBizActionVO)e.Result).List;
                    objFreeList = ((clsGetGRNDetailsListBizActionVO)e.Result).FreeItemsList;

                    dgGRNItems.ItemsSource = null;
                    dgGRNItems.ItemsSource = objList;

                    dgfrontfreeGRNItems.ItemsSource = null;
                    dgfrontfreeGRNItems.ItemsSource = objFreeList;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }



        int ClickedFlag1 = 0;
        public clsGRNVO GRNItemDetails;



        private void ConfirmSave(bool IsDraft)
        {
            string msgTitle = "";
            string msgText = "Are you sure you want to save the GRN Details";

            if (IsDraft)
                msgText += " as Draft";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += (res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    SaveGRN(IsDraft);
                }
                else
                    ClickedFlag1 = 0;
            };

            msgW.Show();
        }


        /// <summary>
        /// Validates form
        /// </summary>
        private bool ValidateForm()
        {
            bool isValid = true;

            if (cmbStore.SelectedItem == null)
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }
            else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)   //((MasterListItem)cmbStore.SelectedItem).ID 
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }
            else
                cmbStore.TextBox.ClearValidationError();


            if (cmbSupplier.SelectedItem == null)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }
            else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }
            else
                cmbSupplier.TextBox.ClearValidationError();

            if (GRNAddedItems == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                isValid = false;
                return false;
            }
            else if (GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW2 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW2.Show();
                isValid = false;
                return false;
            }

            if (dpInvDt.SelectedDate == null)
            {
                dpInvDt.SetValidation("Please Select The Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            //else if (dpInvDt.SelectedDate < Convert.ToDateTime(((clsGRNVO)this.DataContext).PODate) || dpInvDt.SelectedDate > dpgrDt.SelectedDate) //.ToShortDateString()
            //{
            //    dpInvDt.SetValidation("Invoice Date Must be between PO date and GRN date");
            //    dpInvDt.RaiseValidationError();
            //    dpInvDt.Focus();
            //    isValid = false;
            //    return false;
            //}
            else
            {
                dpInvDt.ClearValidationError();
            }

            //if (string.IsNullOrEmpty(txtinvno.Text))
            //{
            //    txtinvno.SetValidation("Please Select The Invoice Number");
            //    txtinvno.RaiseValidationError();
            //    txtinvno.Focus();
            //    isValid = false;
            //    return false;
            //}
            //else
            //{
            //    txtinvno.ClearValidationError();
            //}

            List<clsGRNDetailsVO> objList = GRNAddedItems.ToList<clsGRNDetailsVO>();
            double TotalQuantity = 0;
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    TotalQuantity = 0;
                    if (item.IsBatchRequired == true)
                    {
                        if (item.BatchCode == "" || item.BatchCode == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msg =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msg.Show();
                            isValid = false;
                            break;
                        }
                        if (item.ExpiryDate == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                          new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            isValid = false;
                            break;
                        }
                    }
                    TotalQuantity = item.Quantity + item.FreeQuantity;
                    if (TotalQuantity == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                         new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW3.Show();
                        isValid = false;
                        break;

                    }
                    if (item.Rate == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                        new MessageBoxControl.MessageBoxChildWindow("Zero Rate.", "Rate In The List Can't Be Zero. Please Enter Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        break;
                    }
                    if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
                    {
                        if (item.MRP <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW4 =
                                                   new MessageBoxControl.MessageBoxChildWindow("Zero MRP.", "MRP In The List Can't Be Zero. Please Enter MRP Greater Than Rate.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW4.Show();
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                            isValid = false;
                            break;
                        }
                    }
                    if (item.Quantity > item.POPendingQuantity && rdbDirectPur.IsChecked == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW4 =
                                               new MessageBoxControl.MessageBoxChildWindow("Quantity.", "Quantity must be equal or less than the Purchase Order pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW4.Show();
                        isValid = false;
                        break;
                    }

                    if (isValid)
                    {
                        var chkitem = from r in GRNAddedItems
                                      where r.BatchCode == item.BatchCode && r.ExpiryDate == item.ExpiryDate
                                      && r.ItemID == item.ItemID && item.IsBatchRequired == true
                                      select new clsGRNDetailsVO
                                      {
                                          Status = r.Status,
                                          ID = r.ID
                                      };
                        if (chkitem.ToList().Count > 1)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            break;
                        }


                        var chkitem1 = from r in GRNAddedItems
                                       where r.ItemID == item.ItemID && item.IsBatchRequired == false
                                       select new clsGRNDetailsVO
                                       {
                                           Status = r.Status,
                                           ID = r.ID
                                       };
                        if (chkitem1.ToList().Count > 1)
                        {
                            isValid = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Same Item appears more than once for NonBatchWise item " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.Show();
                            break;
                        }

                    }
                }
            }
            return isValid;
        }

        WaitIndicator Indicatior;
        public Boolean ValidateOnSave(clsGRNDetailsVO item)
        {
            double TotalQuantity = 0;
            Boolean isValid = true;

            float fltOne = 1;
            float fltZero = 0;
            float Infinity = fltOne / fltZero;
            float NaN = fltZero / fltZero;

            if (item.ConversionFactor <= 0 || item.ConversionFactor == Infinity || item.ConversionFactor == NaN)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                isValid = false;
                return false;
            }
            else if (item.BaseConversionFactor <= 0 || item.BaseConversionFactor == Infinity || item.BaseConversionFactor == NaN)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.ItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                isValid = false;
                return false;
            }

            if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)   //((MasterListItem)cmbStore.SelectedItem).ID
            {
                cmbStore.TextBox.SetValidation("Please Select the Store");
                cmbStore.TextBox.RaiseValidationError();
                cmbStore.Focus();
                isValid = false;
                return false;
            }
            else
                cmbStore.TextBox.ClearValidationError();
            if (cmbSupplier.SelectedItem == null || ((MasterListItem)cmbSupplier.SelectedItem).ID == 0)
            {
                cmbSupplier.TextBox.SetValidation("Please Select The Supplier");
                cmbSupplier.TextBox.RaiseValidationError();
                cmbSupplier.Focus();
                isValid = false;
                return false;
            }

            else
                cmbSupplier.TextBox.ClearValidationError();
            if (dpInvDt.SelectedDate == null)
            {
                dpInvDt.SetValidation("Please Select The Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else if (dpInvDt.SelectedDate > DateTime.Now)
            {
                dpInvDt.SetValidation("Please Select Proper Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else if (rdbDirectPur.IsChecked == true && dpInvDt.SelectedDate < DateTime.Now.AddYears(-1))
            {
                dpInvDt.SetValidation("You have Select backdated one year Date So Please Select Proper Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else if (rdbPo.IsChecked == true && item.PODate != null && dpInvDt.SelectedDate > item.PODate.AddYears(1))
            {
                dpInvDt.SetValidation("Invoice Date is Greater than One Year Than PO Date So Please Select Proper Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else if (rdbPo.IsChecked == true && item.PODate != null && dpInvDt.SelectedDate < item.PODate.Date.Date)
            {
                dpInvDt.SetValidation("Invoice Date is Less than Than PO Date So Please Select Proper Invoice Date");
                dpInvDt.RaiseValidationError();
                dpInvDt.Focus();
                isValid = false;
                return false;
            }
            else
            {
                dpInvDt.ClearValidationError();
            }

            //if (string.IsNullOrEmpty(txtinvno.Text))
            //{
            //    txtinvno.SetValidation("Please Select The Invoice Number");
            //    txtinvno.RaiseValidationError();
            //    txtinvno.Focus();
            //    isValid = false;
            //    return false;
            //}
            //else
            //{
            //    txtinvno.ClearValidationError();
            //}


            if ((item.IsBatchRequired == true) && (string.IsNullOrEmpty(item.BatchCode)))
            {
                MessageBoxControl.MessageBoxChildWindow msg =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msg.Show();
                isValid = false;
                return isValid;
            }
            if (item.IsBatchRequired == true && item.ExpiryDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW3.Show();
                isValid = false;
                return isValid;
            }

            //Added BY CDS

            if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays > 0)
            {
                TimeSpan day = item.ExpiryDate.Value - DateTime.Now;
                int Day1 = (int)day.TotalDays;
                Int64 ExpiredDays = item.ItemExpiredInDays;
                if (Day1 < ExpiredDays)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                  new MessageBoxControl.MessageBoxChildWindow("Item has Expiry date less than" + ExpiredDays + "days !", "Please Enter Proper Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW3.Show();
                    isValid = false;
                    return isValid;
                }
            }

            // else if (item.IsBatchRequired && item.ExpiryDate.HasValue && item.ItemExpiredInDays ==0)
            // {               
            // MessageBoxControl.MessageBoxChildWindow msgW3 =
            //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Item does not have Expiry In Days At Master Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);       
            //msgW3.Show();                  
            // }

            // Added By CDS 2/03/16

            if (item.GRNPendItemQuantity > 0)
            {
                //if (item.GRNApprItemQuantity > 0 && (item.PendingQuantity < ((item.Quantity * item.BaseConversionFactor))) && (((item.PendingQuantity + item.GRNApprItemQuantity) - item.GRNPendItemQuantity) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgW3.Show();
                //    item.Quantity = 0;
                //    item.PendingQuantity = item.PendingQuantity;
                //    isValid = false;
                //    return isValid;
                //}
                if ((item.PendingQuantity < ((item.GRNPendItemQuantity - item.GRNDetailsViewTimeQty) + (item.Quantity * item.BaseConversionFactor))) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                //else if ((item.PendingQuantity < ((item.Quantity * item.BaseConversionFactor) + item.GRNApprItemQuantity)) && (((item.PendingQuantity + item.GRNApprItemQuantity) - item.GRNPendItemQuantity) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }


                if (item.GRNApprItemQuantity > 0 && (((item.PendingQuantity) - item.GRNPendItemQuantity) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
                else if ((((item.PendingQuantity + item.GRNApprItemQuantity) - item.GRNPendItemQuantity) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
            }
            else
            {
                if (item.GRNApprItemQuantity > 0 && (item.PendingQuantity < ((item.Quantity * item.BaseConversionFactor) + item.GRNApprItemQuantity)) && (((item.PendingQuantity + item.GRNApprItemQuantity)) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
                else if ((item.PendingQuantity < ((item.Quantity * item.BaseConversionFactor))) && (((item.PendingQuantity + item.GRNApprItemQuantity)) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }

                if (item.GRNApprItemQuantity > 0 && (item.PendingQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                //if (item.GRNApprItemQuantity > 0 && (((item.PendingQuantity)) < (item.Quantity * item.BaseConversionFactor) + item.GRNApprItemQuantity) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
                else if ((((item.PendingQuantity)) < (item.Quantity * item.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();
                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
            }


            //if ((item.PendingQuantity < item.GRNApprItemQuantity + item.GRNPendItemQuantity) && rdbDirectPur.IsChecked == false && ISEditMode == true)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW3.Show();
            //    item.Quantity = 0;
            //    item.PendingQuantity = item.PendingQuantity;
            //    //CalculateGRNSummary();
            //    isValid = false;
            //    return isValid;
            //}

            //if ((item.PendingQuantity - item.GRNApprItemQuantity + item.GRNPendItemQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //    msgW3.Show();
            //    item.Quantity = 0;
            //    item.PendingQuantity = item.PendingQuantity;
            //    //CalculateGRNSummary();
            //    isValid = false;
            //    return isValid;
            //}
            //END

            // Added By CDS 23/02/16
            if (item.ItemQuantity > 0)
            {
                if ((item.ItemQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW3.Show();

                    item.Quantity = 0;
                    item.PendingQuantity = item.PendingQuantity;
                    isValid = false;
                    return isValid;
                }
                else
                    item.POPendingQuantity = item.ItemQuantity - item.Quantity * item.BaseConversionFactor;
            }
            else if ((item.PendingQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                 new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW3.Show();

                item.Quantity = 0;
                item.PendingQuantity = item.PendingQuantity;
                CalculateGRNSummary();
                isValid = false;
                return isValid;
            }
            else
            {
                if (rdbDirectPur.IsChecked != true && GRNAddedItems.Where(z => z.ItemID == item.ItemID && z.UnitId == item.UnitId).Count() <= 1) //added by Ashish Z. for check same Item Multiple Times,   if (rdbDirectPur.IsChecked != true)
                {
                    item.POPendingQuantity = item.PendingQuantity - item.Quantity * item.BaseConversionFactor;
                }
            }
            // END  23/02/16

            //END

            //if (!IsFileAttached)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please attach The Invoice !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW3.Show();
            //    isValid = false;
            //    return isValid;
            //}

            //Int64 Count = 3;

            //if (objUser.IsDirectPurchase == true) 
            if (rdbDirectPur.IsChecked == true)
            {
                if (objUser.IsDirectPurchase == true)
                {
                    if (objUser.UserGRNCountForMonth > objUser.FrequencyPerMonth)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "GRN Count Exceeds the limit for this month !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW3.Show();
                        isValid = false;
                        return isValid;
                    }

                    if (Convert.ToDouble(txtNewNetAmount.Text.ToString()) > Convert.ToDouble(objUser.MaxPurchaseAmtPerTrans))  //Convert.ToDecimal(txtNewNetAmount.ToString())
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "GRN Net Amount Exceeds the limit for this Transaction !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW3.Show();
                        isValid = false;
                        return isValid;
                    }
                }
            }

            //if ( Convert.ToDecimal(txtNewNetAmount.ToString()) > objUser.MaxPurchaseAmtPerTrans && rdbDirectPur.IsChecked == true)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //   new MessageBoxControl.MessageBoxChildWindow("Palash", "GRN Net Amount Exceeds the limit for this Transaction !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW3.Show();
            //    isValid = false;
            //    return isValid;
            //}

            //END

            //TotalQuantity = item.Quantity + item.FreeQuantity;
            //TotalQuantity = item.Quantity *item.BaseConversionFactor + item.FreeQuantity;
            TotalQuantity = item.Quantity * item.BaseConversionFactor + item.FreeQuantity * item.BaseConversionFactor;
            if (TotalQuantity == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                 new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW3.Show();
                isValid = false;
                return isValid;

            }
            if (item.Rate == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Cost Price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW4.Show();
                isValid = false;
                return isValid;

            }
            if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
            {
                if (item.MRP <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW4 =
                                           new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW4.Show();
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                    isValid = false;
                    return isValid;
                }
            }

            //if (item.MRP > 0 && item.MRP < item.Rate)
            if (item.MRP > 0 && item.MRP <= item.Rate)  //Added by Prashant Channe on 09/01/2019, to always validate MRP > Rate
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                       new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW4.Show();
                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;

                //Start: Added by Prashant Channe on 09/01/2019
                if (dgAddGRNItems.SelectedItem != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor > 0)
                {
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                }
                //End: Added by Prashant Channe on 09/01/2019

                isValid = false;
                return isValid;
            }

            //if (cmdSave.IsPressed == true && item.IsBatchRequired == true && string.IsNullOrEmpty(item.BarCode))
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW4 =
            //                           new MessageBoxControl.MessageBoxChildWindow("Palash.", "Please enter the BarCode.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW4.Show();
            //    isValid = false;
            //    return isValid;
            //}
            var lst = GRNAddedItems.Where(itm => itm.ItemID.Equals(item.ItemID));

            //if (lst != null && lst.Count() == 1 && this.ISEditMode == false)  // Commented By CDS
            if (lst != null && lst.Count() == 1)
            {
                if (item.ItemQuantity > 0)
                {
                    //if (chkIsFinalized.IsChecked == false && (item.ItemQuantity < item.Quantity || item.POQuantity < item.Quantity) && rdbDirectPur.IsChecked == false)
                    //{
                    //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                    //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //    msgW3.Show();
                    //    isValid = false;
                    //    return isValid;
                    //}

                    //if (chkIsFinalized.IsChecked == false && (item.ItemQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false) //by CDS
                    if ((item.ItemQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW3 =
                         new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW3.Show();
                        isValid = false;
                        return isValid;
                    }
                    else
                        item.POPendingQuantity = item.PendingQuantity - item.Quantity * item.BaseConversionFactor;
                    //item.POPendingQuantity = item.PendingQuantity - item.BaseQuantity;
                    //item.POPendingQuantity = item.ItemQuantity - item.Quantity;
                }
                //else if (chkIsFinalized.IsChecked == false && (item.PendingQuantity < item.Quantity || item.POQuantity < item.Quantity) && rdbDirectPur.IsChecked == false)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW3.Show();
                //    isValid = false;
                //    return isValid;
                //}
                else if (chkIsFinalized.IsChecked == false && (item.PendingQuantity < item.Quantity * item.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW3.Show();
                    isValid = false;
                    return isValid;
                }
                else if (rdbDirectPur.IsChecked != true)
                {
                    item.POPendingQuantity = item.PendingQuantity - item.Quantity * item.BaseConversionFactor;
                    //item.POPendingQuantity = item.PendingQuantity - item.BaseQuantity;
                    //item.POPendingQuantity = item.PendingQuantity - item.Quantity;
                }


            }


            return isValid;
        }

        public Boolean ValidateOnSaveFree(clsGRNDetailsFreeVO item)
        {
            double TotalQuantity = 0;
            Boolean isValid = true;

            float fltOne = 1;
            float fltZero = 0;
            float Infinity = fltOne / fltZero;
            float NaN = fltZero / fltZero;

            if (item.ConversionFactor <= 0 || item.ConversionFactor == Infinity || item.ConversionFactor == NaN)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.FreeItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                isValid = false;
                return false;
            }
            else if (item.BaseConversionFactor <= 0 || item.BaseConversionFactor == Infinity || item.BaseConversionFactor == NaN)
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Unable to Save, Please assign Conversion Factor to '" + item.FreeItemName + "' Item", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
                isValid = false;
                return false;
            }

            if ((item.IsBatchRequired == true) && (string.IsNullOrEmpty(item.FreeBatchCode)))
            {
                MessageBoxControl.MessageBoxChildWindow msg =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Batch Code is Required for " + item.FreeItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msg.Show();
                isValid = false;
                return isValid;
            }
            if (item.IsBatchRequired == true && item.FreeExpiryDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
              new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW3.Show();
                isValid = false;
                return isValid;
            }

            //Added BY CDS

            if (item.IsBatchRequired && item.FreeExpiryDate.HasValue && item.ItemExpiredInDays > 0)
            {
                TimeSpan day = item.FreeExpiryDate.Value - DateTime.Now;
                int Day1 = (int)day.TotalDays;
                Int64 ExpiredDays = item.ItemExpiredInDays;
                if (Day1 < ExpiredDays)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW3 =
                  new MessageBoxControl.MessageBoxChildWindow("Item has Expiry date less than" + ExpiredDays + "days !", "Please Enter Proper Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW3.Show();
                    isValid = false;
                    return isValid;
                }
            }


            TotalQuantity = item.Quantity * item.BaseConversionFactor + item.FreeQuantity * item.BaseConversionFactor;

            if (TotalQuantity == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW3 =
                 new MessageBoxControl.MessageBoxChildWindow("Zero Quantity.", "Quantity In The List Can't Be Zero. Please Enter Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW3.Show();
                isValid = false;
                return isValid;

            }
            if (item.Rate == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Cost Price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW4.Show();
                isValid = false;
                return isValid;

            }
            if (item.ItemGroup != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.InventoryCatagoryID)
            {
                if (item.MRP <= 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW4 =
                                           new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW4.Show();
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                    isValid = false;
                    return isValid;
                }
            }

            //if (item.MRP > 0 && item.MRP < item.Rate)
            if (item.MRP > 0 && item.MRP <= item.Rate) //Added by Prashant Channe on 09/01/2019, to always validate MRP > Rate
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                       new MessageBoxControl.MessageBoxChildWindow("MRP.", "MRP must be greater than the cost price.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW4.Show();
                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;

                //Start: Added by Prashant Channe on 09/01/2019
                if (dgAddGRNItems.SelectedItem != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor > 0)
                {
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) / ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor;
                }
                //End: Added by Prashant Channe on 09/01/2019

                isValid = false;
                return isValid;
            }

            //if (cmdSave.IsPressed == true && item.IsBatchRequired == true && string.IsNullOrEmpty(item.BarCode))
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW4 =
            //                           new MessageBoxControl.MessageBoxChildWindow("Palash.", "Please enter the BarCode.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgW4.Show();
            //    isValid = false;
            //    return isValid;
            //}

            if (item.SelectedMainItem == null || ((clsFreeMainItems)item.SelectedMainItem).SrNo == 0)  //((clsFreeMainItems)item.SelectedMainItem).ItemID == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW4 =
                                       new MessageBoxControl.MessageBoxChildWindow("Link", "Main Item is not link with the Free Item.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW4.Show();
                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate + 1;
                isValid = false;
                return isValid;
            }

            return isValid;
        }

        /// <summary>
        /// Saves GRN
        /// </summary>
        /// 
        private void SaveGRN(bool IsDraft)
        {
            Boolean blnValidSave = false;
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            if (rdbPo.IsChecked == true)
            {
                var POIDs = GRNAddedItems.GroupBy(i => i.PONO).Distinct().ToList();
                for (Int32 iPOID = 0; iPOID < POIDs.Count && !blnValidSave; iPOID++)
                {
                    var lstGRNItemIDWise = POIDs[iPOID].Where(z => z.ItemID == POIDs[iPOID].ToList()[0].ItemID).ToList();
                    List<clsGRNDetailsVO> lstTarget = lstGRNItemIDWise.ToList();

                    if (lstGRNItemIDWise.ToList().Count > 1)
                    {
                        if (lstTarget.Count > 1)
                            lstTarget.RemoveAt(0);
                        Int32 iQuantity = 0;
                        Int32 iPendingQuantity = 0;
                        foreach (clsGRNDetailsVO objGRNDetailsVO in lstTarget.ToList())
                        {
                            if (objGRNDetailsVO.ItemID == lstGRNItemIDWise.ToList()[0].ItemID && objGRNDetailsVO.PONO.Trim() == lstGRNItemIDWise.ToList()[0].PONO.Trim() && objGRNDetailsVO.BatchCode.Trim() == lstGRNItemIDWise.ToList()[0].BatchCode.Trim())
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "You cannot save the same Batch Code for the item : " + objGRNDetailsVO.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    return;
                                };
                                Indicatior.Close();
                                blnValidSave = true;
                                msgW1.Show();
                                break;
                            }
                            if (objGRNDetailsVO.ItemID == lstGRNItemIDWise.ToList()[0].ItemID && objGRNDetailsVO.PONO.Trim() == lstGRNItemIDWise.ToList()[0].PONO.Trim() && objGRNDetailsVO.BatchCode.Trim() == lstGRNItemIDWise.ToList()[0].BatchCode.Trim())
                            {
                                iQuantity += Convert.ToInt32(lstGRNItemIDWise.ToList()[0].Quantity) + Convert.ToInt32(objGRNDetailsVO.Quantity);
                                iPendingQuantity += Convert.ToInt32(lstGRNItemIDWise.ToList()[0].POPendingQuantity) + Convert.ToInt32(objGRNDetailsVO.Quantity);
                                if (iQuantity > iPendingQuantity)
                                {
                                    blnValidSave = true;
                                    Indicatior.Close();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        return;
                                    };
                                    msgW1.Show();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            try
            {
                if (!blnValidSave)
                {
                    ///////////////////////////////////////////////////////////////////////////////////////

                    bool isSave = false;
                    List<clsCheckDuplicasyVO> tmpobj = new List<clsCheckDuplicasyVO>();
                    if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                    {
                        //GrnObj.Items = new List<clsGRNDetailsVO>();
                        //GrnObj.Items = GRNAddedItems.ToList();
                        foreach (var item in GRNAddedItems)
                        {
                            if (item.BatchID > 0)
                            {
                                // No Need to Check Duplicacy
                            }
                            else if (item.IsBatchRequired == false)
                            {
                                // No Need to Check Duplicacy
                            }
                            else
                            {
                                tmpobj.Add(new clsCheckDuplicasyVO
                                {
                                    ItemID = item.ItemID,
                                    BatchCode = item.BatchCode,
                                    ExpiryDate = item.ExpiryDate,
                                    CostPrice = item.BaseRate,
                                    MRP = item.BaseMRP,
                                    IsBatchRequired = item.IsBatchRequired,
                                    ItemName = item.ItemName,
                                    TransactionTypeID = InventoryTransactionType.GoodsReceivedNote,//1//item.TransactionTypeID
                                    StoreID = StoreID
                                });
                            }
                        }
                    }

                    # region Begin : Commented on 27Oct2018 to fix batch duplication logic issue : as no BatchCode,ExpiryDate,IsBatchRequired is set in the list while item add in GRNPOAddedItems

                    //if (GRNPOAddedItems != null && GRNPOAddedItems.Count > 0)
                    //{
                    //    //GrnObj.ItemsPOGRN = new List<clsGRNDetailsVO>();
                    //    //GrnObj.ItemsPOGRN = GRNPOAddedItems.ToList();
                    //    foreach (var item in GRNPOAddedItems)
                    //    {
                    //        if (item.BatchID > 0)
                    //        {
                    //            // No Need to Check Duplicacy
                    //        }
                    //        else if (item.IsBatchRequired == false)
                    //        {
                    //            // No Need to Check Duplicacy
                    //        }
                    //        else
                    //        {
                    //            tmpobj.Add(new clsCheckDuplicasyVO
                    //            {
                    //                ItemID = item.ItemID,
                    //                BatchCode = item.BatchCode,
                    //                ExpiryDate = item.ExpiryDate,
                    //                CostPrice = item.BaseRate,
                    //                MRP = item.BaseMRP,
                    //                IsBatchRequired = item.IsBatchRequired,
                    //                ItemName = item.ItemName,
                    //                TransactionTypeID = InventoryTransactionType.GoodsReceivedNote, //1//item.TransactionTypeID
                    //                StoreID = StoreID
                    //            });
                    //        }
                    //    }
                    //}

                    #endregion //End : Commented on 27Oct2018 to fix batch duplication logic issue : as no BatchCode,ExpiryDate,IsBatchRequired is set in the list while item add in GRNPOAddedItems

                    //Adedd By Ashish Z. on Dated 28122016
                    foreach (var GRNItem in GRNAddedItems.ToList())
                    {
                        float fltBaseQuantity = 0;
                        fltBaseQuantity = GRNItem.BaseQuantity;

                        foreach (var POGRNItems in GRNPOAddedItems.ToList())   //GRNPOAddedItems.OrderBy(z => z.POID).ToList()
                        {
                            if (GRNItem.ItemID == POGRNItems.ItemID && fltBaseQuantity > 0)
                            {
                                if (POGRNItems.POFinalPendingQuantity >= fltBaseQuantity)
                                {
                                    POGRNItems.Quantity = fltBaseQuantity;
                                    fltBaseQuantity = 0;
                                }
                                else
                                {
                                    POGRNItems.Quantity = POGRNItems.POFinalPendingQuantity;
                                    fltBaseQuantity = Convert.ToSingle(fltBaseQuantity - POGRNItems.POFinalPendingQuantity);
                                }
                            }
                        }
                    }


                    //foreach (var GRNItem in GRNAddedItems.ToList())
                    //{
                    //    float fltBaseQuantity = 0;
                    //    fltBaseQuantity = GRNItem.BaseQuantity;

                    //    foreach (var POGRNItems in GRNPOAddedItems.ToList())   //GRNPOAddedItems.OrderBy(z => z.POID).ToList()
                    //    {
                    //        if (GRNItem.ItemID == POGRNItems.ItemID && fltBaseQuantity > 0)
                    //        {
                    //            if (POGRNItems.POFinalPendingQuantity >= fltBaseQuantity)
                    //            {
                    //                POGRNItems.Quantity = fltBaseQuantity;
                    //                POGRNItems.POFinalPendingQuantity = POGRNItems.POFinalPendingQuantity - POGRNItems.Quantity;
                    //                fltBaseQuantity = 0;
                    //            }
                    //            else
                    //            {
                    //                POGRNItems.Quantity = POGRNItems.POFinalPendingQuantity;
                    //                POGRNItems.POFinalPendingQuantity = POGRNItems.POFinalPendingQuantity - POGRNItems.Quantity;
                    //                fltBaseQuantity = Convert.ToSingle(fltBaseQuantity - POGRNItems.POFinalPendingQuantity);
                    //            }
                    //        }
                    //    }
                    //}


                    //End By Ashish Z. on Dated 28122016


                    #region For Free Items Check Duplication

                    if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count > 0)   // For Free Items
                    {
                        //GrnObj.Items = new List<clsGRNDetailsVO>();
                        //GrnObj.Items = GRNAddedItems.ToList();
                        foreach (var item in GRNAddedFreeItems)
                        {
                            if (item.FreeBatchID > 0)
                            {
                                // No Need to Check Duplicacy
                            }
                            else if (item.IsBatchRequired == false)
                            {
                                // No Need to Check Duplicacy
                            }
                            else
                            {
                                tmpobj.Add(new clsCheckDuplicasyVO
                                {
                                    ItemID = item.FreeItemID,
                                    BatchCode = item.FreeBatchCode,
                                    ExpiryDate = item.FreeExpiryDate,
                                    CostPrice = item.BaseRate,
                                    MRP = item.BaseMRP,
                                    IsBatchRequired = item.IsBatchRequired,
                                    ItemName = item.FreeItemName,
                                    TransactionTypeID = InventoryTransactionType.GoodsReceivedNote,//1//item.TransactionTypeID
                                    StoreID = StoreID,
                                    IsFree = true
                                });
                            }
                        }
                    }

                    #endregion

                    clsCheckDuplicasyBizActionVO BizAction = new clsCheckDuplicasyBizActionVO();
                    BizAction.lstDuplicasy = tmpobj;
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    client1.ProcessCompleted += (s, args) =>
                    {
                        string ItemName = "";
                        string BatchCode = "";

                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsCheckDuplicasyBizActionVO)args.Result).SuccessStatus == 0)
                            {
                                isSave = true;
                            }
                        }
                        if (isSave)
                        {
                            //string msgTitle = "";
                            //string msgText = "Are you sure you want to save the GRN Details";

                            //MessageBoxControl.MessageBoxChildWindow msgW =
                            //    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            //msgW.OnMessageBoxClosed += (res) =>
                            //{
                            //    if (res == MessageBoxResult.Yes)
                            //    {

                            SaveGRNWithoutDuplicate(IsDraft);
                            //    }
                            //    else
                            //        ClickedFlag1 = 0;
                            //};

                            //msgW.Show();
                        }
                        else
                        {
                            ItemName = ((clsCheckDuplicasyBizActionVO)args.Result).ItemName;
                            BatchCode = ((clsCheckDuplicasyBizActionVO)args.Result).BatchCode;
                            string msgtext = "";
                            if (((clsCheckDuplicasyBizActionVO)args.Result).IsBatchRequired == true)
                            {
                                msgtext = "Item " + "'" + ItemName + "'" + " with Batch " + "'" + BatchCode + "'" + " already exists";
                            }
                            else
                            {
                                msgtext = "Item " + "'" + ItemName + "'" + " already exists for same details";
                            }

                            chkIsFinalized.IsChecked = false;
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", msgtext, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);  //Batch already exists
                            ClickedFlag1 = 0;
                            Indicatior.Close();
                            msgW1.Show();

                        }
                    };
                    client1.ProcessAsync(BizAction, new clsUserVO());
                    client1.CloseAsync();

                    //}

                    ///////////////////////////////////////////////////////////////////////////////////////

                }
                else
                    Indicatior.Close();
            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                Indicatior.Close();
            }
        }

        private void SaveGRNWithoutDuplicate(bool IsDraft)
        {
            try
            {
                clsGRNVO GrnObj = new clsGRNVO();
                GrnObj = (clsGRNVO)this.DataContext;

                if (rdbPo.IsChecked == true)
                {
                    GrnObj.GRNType = InventoryGRNType.AgainstPO;
                }
                else if (rdbDirectPur.IsChecked == true)
                {
                    GrnObj.GRNType = InventoryGRNType.Direct;
                }
                if (cmbSupplier.SelectedItem != null)
                    GrnObj.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                if (cmbStore.SelectedItem != null)
                    GrnObj.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                //GrnObj.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;

                if (cmbReceivedBy.SelectedItem != null && ((MasterListItem)cmbReceivedBy.SelectedItem).ID > 0)
                    GrnObj.ReceivedByID = ((MasterListItem)cmbReceivedBy.SelectedItem).ID;
                else
                    GrnObj.ReceivedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;

                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    GrnObj.Items = new List<clsGRNDetailsVO>();
                    GrnObj.Items = GRNAddedItems.ToList();
                }
                foreach (clsGRNDetailsVO item33 in GRNPOAddedItems)
                {
                    var item55 = from r55 in GRNAddedItems
                                 where r55.ItemID == item33.ItemID //&& r55.POID == item33.POID && r55.POUnitID == item33.POUnitID
                                 select r55;

                    if (item55 != null && item55.ToList().Count > 0)
                    {
                        //item33.BatchCode = (item55.FirstOrDefault()).BatchCode.DeepCopy();
                    }
                }

                if (GRNPOAddedItems != null && GRNPOAddedItems.Count > 0)
                {
                    GrnObj.ItemsPOGRN = new List<clsGRNDetailsVO>();
                    GrnObj.ItemsPOGRN = GRNPOAddedItems.ToList();
                }

                if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count > 0)  // For Free Items
                {
                    GrnObj.ItemsFree = new List<clsGRNDetailsFreeVO>();
                    GrnObj.ItemsFree = GRNAddedFreeItems.ToList();
                }

                if (GRNDeletedMainItems != null && GRNDeletedMainItems.Count > 0)
                {
                    GrnObj.ItemsDeletedMain = GRNDeletedMainItems.ToList();
                }

                if (GRNDeletedFreeItems != null && GRNDeletedFreeItems.Count > 0)
                {
                    GrnObj.ItemsDeletedFree = GRNDeletedFreeItems.ToList();
                }

                clsAddGRNBizActionVO BizAction = new clsAddGRNBizActionVO();
                BizAction.Details = new clsGRNVO();
                BizAction.Details = GrnObj;
                BizAction.IsDraft = IsDraft;

                BizAction.IsEditMode = ISEditMode;

                if (chkIsFinalized.IsChecked == true)
                {
                    BizAction.Details.Freezed = true;
                }
                else
                {
                    BizAction.Details.Freezed = false;
                }

                //Added By CDS To Approve GRN
                if (ISFromAproveGRN == true)
                {
                    BizAction.Details.EditForApprove = true;
                }
                else
                {
                    BizAction.Details.EditForApprove = false;
                }
                // END

                if (chkConsignment.IsChecked == true)
                {
                    BizAction.Details.IsConsignment = true;
                }
                else
                    BizAction.Details.IsConsignment = false;

                if (IsFileAttached)
                {
                    BizAction.IsFileAttached = true;
                    BizAction.File = File;
                    BizAction.FileName = fileName;
                }
                else
                    BizAction.IsFileAttached = false;
                //added by rohinee dated 28/9/2016 for audit trail==================================================================================                                     
                if (IsAuditTrail)
                {
                    objGUID = new Guid();
                    LogInformation = new LogInfo();
                    LogInformation.guid = objGUID;
                    LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    LogInformation.TimeStamp = DateTime.Now;
                    LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                    LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                    LogInformation.Message = " 139 : On Save Click : "
                        + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                        + " , GRNType : " + Convert.ToString(GrnObj.GRNType) + " "
                        + " , Store : " + Convert.ToString(GrnObj.StoreID) + " "
                        + " , Supplier : " + Convert.ToString(GrnObj.SupplierID) + " "
                        + " , Received By ID : " + Convert.ToString(GrnObj.ReceivedByID) + " "
                        + " , Store : " + Convert.ToString(BizAction.IsDraft) + " "
                        + " , Is Edit Mode : " + Convert.ToString(BizAction.IsEditMode) + " "
                        + " , Freezed : " + Convert.ToString(BizAction.Details.Freezed) + " "
                        + " , Edit For Approve : " + Convert.ToString(BizAction.Details.EditForApprove) + " "
                        + " , Is Consignment : " + Convert.ToString(BizAction.Details.IsConsignment) + " "
                        + " , Is File Attached : " + Convert.ToString(BizAction.IsFileAttached) + " "
                        + " , Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                        + " , C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                        + " , Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                        + " , Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                        + " , VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                        + " , Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                        + " , Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                        + " , Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                        + " , Remarks : " + Convert.ToString(txtremarks.Text) + " "
                        + " , Round Off Net Amount : " + Convert.ToString(txtGRNRoundOffNetAmount.Text) + " "
                        + " , GRN Discount : " + Convert.ToString(txtGRNDiscount.Text) + " "
                        + " , Other Charges : " + Convert.ToString(txtOtherCharges.Text) + " "
                        + " , GRNO : " + Convert.ToString(txtgrno.Text) + " "
                        + " , date : " + Convert.ToString(dpgrDt.Text) + " "
                        + " , Gate Entry No : " + Convert.ToString(txtGateEntryNo.Text) + " "
                        + " , Pay Mode : " + Convert.ToString(rdbCashMode.IsChecked) + " " + Convert.ToString(rdbCreditMode.IsChecked) + " "
                        ;
                    if (GrnObj.ItemsPOGRN != null && GrnObj.ItemsPOGRN.Count > 0)
                    {
                        foreach (var item in GrnObj.ItemsPOGRN)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.ItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Item Name : " + Convert.ToString(item.ItemName) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                    if (GrnObj.ItemsFree != null && GrnObj.ItemsFree.Count > 0)
                    {
                        foreach (var item in GrnObj.ItemsFree)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.FreeItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Free Item Name : " + Convert.ToString(item.FreeItemName) + " "
                                            + " , Free Item Code : " + Convert.ToString(item.FreeItemCode) + " "
                                            + " , Free Batch Code : " + Convert.ToString(item.FreeBatchCode) + " "
                                            + " , Free Quantity : " + Convert.ToString(item.FreeQuantity) + " "
                                            + " , Free Expiry Date : " + Convert.ToString(item.FreeExpiryDate) + " "
                                            + " , Free GRN Detail ID : " + Convert.ToString(item.FreeGRNDetailID) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                    if (GrnObj.ItemsDeletedMain != null && GrnObj.ItemsDeletedMain.Count > 0)
                    {
                        foreach (var item in GrnObj.ItemsDeletedMain)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.ItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Item Name : " + Convert.ToString(item.ItemName) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }
                    if (GrnObj.ItemsDeletedFree != null && GrnObj.ItemsDeletedFree.Count > 0)
                    {
                        foreach (var item in GrnObj.ItemsDeletedFree)
                        {
                            LogInformation.Message = LogInformation.Message
                                            + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                            + " , Item ID : " + Convert.ToString(item.FreeItemID) + " "
                                            + " , Item Tax : " + Convert.ToString(item.ItemTax) + " "
                                            + " , Free Item Name : " + Convert.ToString(item.FreeItemName) + " "
                                            + " , Free Item Code : " + Convert.ToString(item.FreeItemCode) + " "
                                            + " , Free Batch Code : " + Convert.ToString(item.FreeBatchCode) + " "
                                            + " , Free Quantity : " + Convert.ToString(item.FreeQuantity) + " "
                                            + " , Free Expiry Date : " + Convert.ToString(item.FreeExpiryDate) + " "
                                            + " , Free GRN Detail ID : " + Convert.ToString(item.FreeGRNDetailID) + " "
                                            + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                            + " , Rate : " + Convert.ToString(item.Rate) + " "
                                            + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                            + " , MRP : " + Convert.ToString(item.MRP) + " "
                                            + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                            + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                            + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                            + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                            + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                            + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                            + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                            + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                            + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                            + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                            + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                            + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                            + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                            + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                            + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                            + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                            + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                            + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                            + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                            + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                            + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                            + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                            + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                            + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                            + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                            + " , Containername : " + Convert.ToString(item.Containername) + " "
                                            ;
                        }
                        LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                        LogInfoList.Add(LogInformation);
                    }


                    BizAction.LogInfoList = new List<LogInfo>();  // For the Activity Log List
                    BizAction.LogInfoList = LogInfoList.DeepCopy();
                }

                //====================================================================================================================================

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag1 = 0;
                    if (((clsAddGRNBizActionVO)arg.Result).SuccessStatus == -2)
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Unable to Save! PO Pending Qty is Less than Received Qty for ItemName "
                                       + ((clsAddGRNBizActionVO)arg.Result).ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    else if (arg.Error == null && arg.Result != null && ((clsAddGRNBizActionVO)arg.Result).Details != null)
                    {
                        if (((clsAddGRNBizActionVO)arg.Result).Details != null)
                        {
                            GRNItemDetails = ((clsAddGRNBizActionVO)arg.Result).Details;

                            //if (((clsAddGRNBizActionVO)arg.Result).IsDraft == false)
                            //{
                            //    PrintGRNItemBarCode barcodeWin = new PrintGRNItemBarCode();
                            //    barcodeWin.GRNItemDetails = GRNItemDetails;
                            //    barcodeWin.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);
                            //    barcodeWin.Show();
                            //}

                            FillGRNSearchList();
                            string message = "";
                            if (ISFromAproveGRN == true)
                                //message = "GRN details Approve And Updated successfully With GRNNo " + ((clsAddGRNBizActionVO)arg.Result).Details.GRNNO;
                                message = "GRN details  Verified and Approved successfully With GRNNo " + ((clsAddGRNBizActionVO)arg.Result).Details.GRNNO; //Added by Ajit date 14/9/2016 Change Msg //***//
                            else
                                message = "GRN details saved successfully With GRNNo " + ((clsAddGRNBizActionVO)arg.Result).Details.GRNNO;

                            if (((clsAddGRNBizActionVO)arg.Result).IsDraft)
                                message += " as Draft";

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            SetCommandButtonState("Save");
                            msgW1.Show();

                            if (ISFromAproveGRN == true)
                            {
                                frmApproveNewGRN AprvGRN = new frmApproveNewGRN();

                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                mElement.Text = "Approve Goods Received Note";
                                ((IApplicationConfiguration)App.Current).OpenMainContent(AprvGRN);
                            }
                            else
                                _flip.Invoke(RotationType.Backward);

                        }
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding GRN details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ClickedFlag1 = 0;
                Indicatior.Close();
            }
        }

        void BarcodeWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            FillGRNSearchList();
        }
        private static void MessgBoxQuantityAndPendingQuantity()
        {
            MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            msgW1.Show();
        }




        /// <summary>
        /// Initialize form
        /// </summary>
        private void InitialiseForm()
        {
            rdbCashMode.IsChecked = true;

            rdbDirectPur.IsChecked = true;

            this.DataContext = new clsGRNVO();

            GRNAddedItems.Clear();
            GRNPOAddedItems.Clear();
            cmbReceivedBy.SelectedValue = 0;

            cmbSupplier.SelectedValue = ((clsGRNVO)this.DataContext).SupplierID;
            cmbSearchSupplier.SelectedValue = ((clsGRNVO)this.DataContext).SupplierID;
            cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;

            GRNAddedItems = new List<clsGRNDetailsVO>();
            GRNPOAddedItems = new ObservableCollection<clsGRNDetailsVO>();
            txtPONO.Text = String.Empty;

            ISEditMode = false;
            chkIsFinalized.IsChecked = false;
            chkConsignment.IsChecked = false;

            GRNAddedFreeItems.Clear();
            GRNAddedFreeItems = new List<clsGRNDetailsFreeVO>();

            GRNInfo.SelectedIndex = 0;
            GRNDeletedMainItems = new List<clsGRNDetailsVO>();
            GRNDeletedFreeItems = new List<clsGRNDetailsFreeVO>();
        }





        MessageBoxControl.MessageBoxChildWindow msgW1 = null;




        /// <summary>
        /// GRN Type radio button click
        /// </summary>
        /// 
        public InventoryGRNType PreviousGRNType { get; set; }
        private void rbGRNType_Checked(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                PreviousGRNType = ((clsGRNVO)this.DataContext).GRNType;
                if (rdbDirectPur.IsChecked == true)
                {
                    ((clsGRNVO)this.DataContext).GRNType = ValueObjects.InventoryGRNType.Direct;

                    txtPONO.Text = "";
                    //     BdrItemSearch.Visibility = Visibility.Visible;
                    if (GRNAddedItems != null)
                    {
                        ((clsGRNVO)this.DataContext).POID = 0;
                        ((clsGRNVO)this.DataContext).PONO = "";
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                    if (GRNAddedFreeItems != null)   // For Free Quantity
                    {
                        GRNAddedFreeItems.Clear();
                        CalculateGRNSummary();
                    }

                }
                else if (rdbPo.IsChecked == true)
                {
                    ((clsGRNVO)this.DataContext).GRNType = ValueObjects.InventoryGRNType.AgainstPO;
                    BdrItemSearch.Visibility = Visibility.Collapsed;
                    if (GRNAddedItems != null)
                    {
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        CalculateGRNSummary();
                    }

                    if (GRNAddedFreeItems != null)   // For Free Quantity
                    {
                        GRNAddedFreeItems.Clear();
                        CalculateGRNSummary();
                    }
                }
            }

        }

        /// <summary>
        /// radio button payment mode click
        /// </summary>

        private void rbPayMode_Click(object sender, RoutedEventArgs e)
        {
            if (IsPageLoaded)
            {
                if (rdbCashMode.IsChecked == true)
                    ((clsGRNVO)this.DataContext).PaymentModeID = ValueObjects.MaterPayModeList.Cash;
                else if (rdbCreditMode.IsChecked == true)
                    ((clsGRNVO)this.DataContext).PaymentModeID = ValueObjects.MaterPayModeList.Credit;
            }
        }


        /// <summary>
        /// Calculates GRN summary
        /// </summary>
        private void CalculateGRNSummary()
        {

            double CDiscount, SchDiscount, VATAmount, Amount, NetAmount, ItemTaxAmount, CostAmount, VATAmountFree, ItemTaxAmountFree, SGSTAmount, CGSTAmount, IGSTAmount, SGSTAmountFree, CGSTAmountFree, IGSTAmountFree;

            CDiscount = SchDiscount = VATAmount = Amount = NetAmount = ItemTaxAmount = CostAmount = VATAmountFree = ItemTaxAmountFree = SGSTAmount = CGSTAmount = IGSTAmount = SGSTAmountFree = CGSTAmountFree = IGSTAmountFree = 0;

            double GRNOtherCha, GRNDis, GRNRound;
            GRNOtherCha = GRNDis = GRNRound = 0;

            foreach (var item in GRNAddedItems)
            {
                Amount += Math.Round(item.Amount, 2);  //Math.Round(123.4567, 2, MidpointRounding.AwayFromZero)
                CostAmount += Math.Round(item.CostRate, 2);
                CDiscount += Math.Round(item.CDiscountAmount, 2);
                SchDiscount += Math.Round(item.SchDiscountAmount, 2);
                VATAmount += Math.Round(item.VATAmount, 2);
                SGSTAmount += Math.Round(item.SGSTAmount, 2);//Added By Bhushanp 2406217 For GST
                CGSTAmount += Math.Round(item.CGSTAmount, 2);
                IGSTAmount += Math.Round(item.IGSTAmount, 2);
                NetAmount += Math.Round(item.NetAmount, 2);//System.Math.Round(Convert.ToDouble(item.NetAmount),0);
                ItemTaxAmount += Math.Round(item.TaxAmount, 2);

                GRNOtherCha = Math.Round(item.OtherCharges, 2);
                GRNDis = Math.Round(item.GRNDiscount, 2);
                GRNRound = Math.Round(item.GRNRoundOff, 2);
            }

            foreach (clsGRNDetailsFreeVO itemFree in GRNAddedFreeItems)  //For Free Items
            {
                VATAmount += Math.Round(itemFree.VATAmount, 2);
                VATAmountFree += Math.Round(itemFree.VATAmount, 2);

                ItemTaxAmount += Math.Round(itemFree.TaxAmount, 2);
                ItemTaxAmountFree += Math.Round(itemFree.TaxAmount, 2);

                SGSTAmount += Math.Round(itemFree.SGSTAmount, 2);
                SGSTAmountFree += Math.Round(itemFree.SGSTAmount, 2);

                CGSTAmount += Math.Round(itemFree.CGSTAmount, 2);
                CGSTAmountFree += Math.Round(itemFree.CGSTAmount, 2);

                IGSTAmount += Math.Round(itemFree.IGSTAmount, 2);
                IGSTAmountFree += Math.Round(itemFree.IGSTAmount, 2);
            }

            ((clsGRNVO)this.DataContext).TotalAmount = Amount;
            ((clsGRNVO)this.DataContext).TotalAmount = CostAmount;
            ((clsGRNVO)this.DataContext).TotalCDiscount = CDiscount;
            ((clsGRNVO)this.DataContext).TotalSchDiscount = SchDiscount;
            ((clsGRNVO)this.DataContext).TotalVAT = VATAmount;
            //((clsGRNVO)this.DataContext).NetAmount = NetAmount;
            //Added By Bhushanp 2406217 For GST

            ((clsGRNVO)this.DataContext).TotalSGST = SGSTAmount;
            ((clsGRNVO)this.DataContext).TotalCGST = CGSTAmount;
            ((clsGRNVO)this.DataContext).TotalIGST = IGSTAmount;

            ((clsGRNVO)this.DataContext).TotalTAxAmount = ItemTaxAmount;

            //NetAmount = NetAmount + VATAmountFree + ItemTaxAmountFree ;  // NetAmount + Free Items VAT & Tax Amount //Commented By Bhushanp 
            NetAmount = NetAmount + VATAmountFree + ItemTaxAmountFree + SGSTAmountFree + CGSTAmountFree + IGSTAmountFree;
            // Added By CDS
            ((clsGRNVO)this.DataContext).PrevNetAmount = NetAmount;
            ((clsGRNVO)this.DataContext).OtherCharges = GRNOtherCha;
            ((clsGRNVO)this.DataContext).GRNDiscount = GRNDis;


            ((clsGRNVO)this.DataContext).NetAmount = NetAmount + GRNOtherCha - GRNDis;
            //((clsGRNVO)this.DataContext).NetAmount = NetAmount + GRNOtherCha - GRNDis + GRNRound;
            ((clsGRNVO)this.DataContext).GRNRoundOff = System.Math.Round((((clsGRNVO)this.DataContext).NetAmount), 0);

            //((clsGRNVO)this.DataContext).GRNRoundOff = System.Math.Round(Convert.ToDouble(netamt), 0);

            txttotcamt.Text = String.Format("{0:0.00}", Amount);//Amount.ToString();
            txttotcamt.Text = String.Format("{0:0.00}", CostAmount);//Amount.ToString();
            txtcdiscamt.Text = String.Format("{0:0.00}", CDiscount);//CDiscount.ToString();
            txthdiscamt.Text = String.Format("{0:0.00}", SchDiscount); //SchDiscount.ToString();
            txttaxamount.Text = String.Format("{0:0.00}", ItemTaxAmount); //ItemTaxAmount.ToString();
            txtVatamt.Text = String.Format("{0:0.00}", VATAmount);//VATAmount.ToString();
            txtNetamt.Text = String.Format("{0:0.00}", NetAmount); //NetAmount.ToString();
            //Added By Bhushan For GST 26062017
            txtSGSTamt.Text = String.Format("{0:0.00}", SGSTAmount);//VATAmount.ToString();
            txtCGSTamt.Text = String.Format("{0:0.00}", CGSTAmount);//VATAmount.ToString();
            txtIGSTamt.Text = String.Format("{0:0.00}", IGSTAmount);//VATAmount.ToString();
            txtVatamtFree.Text = String.Format("{0:0.00}", VATAmountFree);  // For Free Items

            txtSGSTamtFree.Text = String.Format("{0:0.00}", SGSTAmountFree);
            txtCGSTamtFree.Text = String.Format("{0:0.00}", CGSTAmountFree);
            txtIGSTamtFree.Text = String.Format("{0:0.00}", IGSTAmountFree);
            txttaxamountFree.Text = String.Format("{0:0.00}", ItemTaxAmountFree);  // For Free Items

            //dgAddGRNItems.ItemsSource = null;
            //dgAddGRNItems.ItemsSource = GRNAddedItems;

            dgGRNItems.Focus();
            dgAddGRNItems.UpdateLayout();

            //SetControlForExistingBatch();
            if (_ItemSearchRowControl != null)
                _ItemSearchRowControl.SetFocus();
        }

        private void SetControlForExistingBatch()
        {
            foreach (var item in GRNAddedItems)
            {
                if (((clsGRNDetailsVO)item).IsBatchAssign == true)
                {
                    clsGRNDetailsVO objRateContractDetails = ((clsGRNDetailsVO)item);

                    FrameworkElement fe = dgAddGRNItems.Columns[2].GetCellContent(item);
                    FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                    if (result != null)
                    {
                        DataGridCell cell = (DataGridCell)result;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column2 = dgAddGRNItems.Columns[3];
                    FrameworkElement fe1 = column2.GetCellContent(item);
                    FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                    if (result1 != null)
                    {
                        DataGridCell cell = (DataGridCell)result1;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column3 = dgAddGRNItems.Columns[8];
                    FrameworkElement fe2 = column3.GetCellContent(item);
                    FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                    if (result2 != null)
                    {
                        DataGridCell cell = (DataGridCell)result2;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column4 = dgAddGRNItems.Columns[11];
                    FrameworkElement fe4 = column4.GetCellContent(item);
                    FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                    if (result3 != null)
                    {
                        DataGridCell cell = (DataGridCell)result3;
                        cell.IsEnabled = false;
                    }

                    DataGridColumn column5 = dgAddGRNItems.Columns[12];
                    FrameworkElement fe5 = column5.GetCellContent(item);
                    FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                    if (result4 != null)
                    {
                        DataGridCell cell = (DataGridCell)result4;
                        cell.IsEnabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// GRN list selection changed.
        /// </summary>
        private void dgGRNList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                FillGRNDetailslList(((clsGRNVO)dgGRNList.SelectedItem).ID);
            }
        }



        /// <summary>
        /// Store selection changed
        /// </summary>
        /// 
        private long StoreID { get; set; }
        private long SupplierID { get; set; }
        private void cmbStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStore.SelectedItem != null)
                {
                    if (cmbStoreState.ItemsSource != null)
                    {
                        cmbStoreState.SelectedValue = ((clsStoreVO)cmbStore.SelectedItem).StateID;
                    }
                    if (GRNAddedItems != null)
                    {
                        txtPONO.Text = String.Empty;
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        GRNAddedFreeItems.Clear();  // For Free Quantity
                        CalculateGRNSummary();
                    }

                    //if (((MasterListItem)cmbStore.SelectedItem).ID > 0)
                    //{
                    //    StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                    //    if (_ItemSearchRowControl != null)
                    //    {
                    //        _ItemSearchRowControl.StoreID = StoreID;
                    //    }
                    //}

                    if (((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)
                    {
                        StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
                        if (_ItemSearchRowControl != null)
                        {
                            _ItemSearchRowControl.StoreID = StoreID;
                        }
                    }

                    if (StoreID > 0 && SupplierID > 0 && rdbDirectPur.IsChecked == true)
                    {
                        AttachItemSearchControl(StoreID, SupplierID);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Supplier selection changed
        /// </summary>
        private void cmbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbSupplier.SelectedItem != null)
                {
                    if (cmbSupplierState.ItemsSource != null)
                    {
                        cmbSupplierState.SelectedValue = ((MasterListItem)cmbSupplier.SelectedItem).StateID;
                    }
                    if (GRNAddedItems != null)
                    {
                        txtPONO.Text = "";
                        GRNAddedItems.Clear();
                        GRNPOAddedItems.Clear();
                        GRNAddedFreeItems.Clear();  // For Free Quantity
                        CalculateGRNSummary();
                    }
                    SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    if (StoreID > 0 && SupplierID > 0 && rdbDirectPur.IsChecked == true)
                    {
                        AttachItemSearchControl(StoreID, SupplierID);
                    }

                }
                else
                {
                    SupplierID = 0;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assign Batch click
        /// </summary>

        private void AddBatch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkIsFinalized.IsChecked == false || ISFromAproveGRN == true)
                {
                    if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID > 0 && ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)  //((MasterListItem)cmbStore.SelectedItem).ID
                    {
                        cmbStore.ClearValidationError();
                        PalashDynamics.Radiology.ItemSearch.AssignBatch BatchWin = new PalashDynamics.Radiology.ItemSearch.AssignBatch();
                        BatchWin.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId; //((MasterListItem)cmbStore.SelectedItem).ID;
                        BatchWin.SelectedItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;
                        BatchWin.ItemName = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemName;

                        BatchWin.ShowZeroStockBatches = true;  // To show zero stock batches to add stock to it using GRN.

                        BatchWin.IsFree = false;  // Set False to Show only Main Item Batches

                        BatchWin.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButton_Click);
                        BatchWin.Show();
                    }
                    else
                    {
                        if (cmbStore.SelectedItem == null)
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                        else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)   //((MasterListItem)cmbStore.SelectedItem).ID 
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Assign batch ok button click
        /// </summary>
        /// 
        public bool IsBatchAssign = false;
        void OnAddBatchButton_Click(object sender, RoutedEventArgs e)
        {
            PalashDynamics.Radiology.ItemSearch.AssignBatch AssignBatchWin = (PalashDynamics.Radiology.ItemSearch.AssignBatch)sender;
            if (AssignBatchWin.DialogResult == true && AssignBatchWin.SelectedBatches != null)
            {
                foreach (var item in AssignBatchWin.SelectedBatches)
                {
                    if (dgAddGRNItems.ItemsSource != null)
                    {
                        List<clsGRNDetailsVO> obclnGRNDetails = new List<clsGRNDetailsVO>();

                        if (dgAddGRNItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        {
                            obclnGRNDetails = (List<clsGRNDetailsVO>)((PagedCollectionView)(dgAddGRNItems.ItemsSource)).SourceCollection;
                        }
                        else
                            obclnGRNDetails = ((List<clsGRNDetailsVO>)(dgAddGRNItems.ItemsSource));
                        List<clsGRNDetailsVO> grnItemList = obclnGRNDetails;
                        foreach (var BatchItems in grnItemList.Where(x => x.ItemID == item.ItemID && x.PONO == ((clsGRNDetailsVO)(dgAddGRNItems.SelectedItem)).PONO))
                        {
                            if (BatchItems.BatchID == item.BatchID)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item with same batch already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                return;
                            }
                        }
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchID = item.BatchID;
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchCode = item.BatchCode;
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ExpiryDate = item.ExpiryDate;
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableQuantity = item.AvailableStock;
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseAvailableQuantity = item.AvailableStockInBase;


                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = item.MRP;
                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = item.PurchaseRate;

                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = Convert.ToDouble(item.PurchaseRate) * Convert.ToDouble(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor);
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate = Convert.ToSingle(item.PurchaseRate);
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseRate = Convert.ToSingle(item.PurchaseRate);

                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = Convert.ToDouble(item.MRP) * Convert.ToDouble(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor);
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP = Convert.ToSingle(item.MRP);
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP = Convert.ToSingle(item.MRP);


                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = item.BatchCode;//By Umeshitem.BarCode;

                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = item.ConversionFactor;

                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = item.IsConsignment;
                        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsBatchAssign = true;

                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).TransUOM = item.PUOM;


                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOM = item.SUOM;
                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOMID = item.SUM;

                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor = item.PurchaseToBaseCF;

                        if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment == true)
                        {
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = true;
                            chkConsignment.IsChecked = true;

                        }
                        else
                        {
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment = false;
                            chkConsignment.IsChecked = false;
                        }





                        DataGridColumn column = dgAddGRNItems.Columns[2];

                        FrameworkElement fe = column.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column2 = dgAddGRNItems.Columns[3];
                        FrameworkElement fe1 = column2.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                        if (result1 != null)
                        {
                            DataGridCell cell = (DataGridCell)result1;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column3 = dgAddGRNItems.Columns[9];
                        FrameworkElement fe2 = column3.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column4 = dgAddGRNItems.Columns[12];
                        FrameworkElement fe4 = column4.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column5 = dgAddGRNItems.Columns[13];
                        FrameworkElement fe5 = column5.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                        if (result4 != null)
                        {
                            DataGridCell cell = (DataGridCell)result4;
                            cell.IsEnabled = false;
                        }
                        //Added By CDS
                        DataGridColumn qty = dgAddGRNItems.Columns[9];
                        FrameworkElement feqty = qty.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement resultqty = GetParent(feqty, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)resultqty;
                            cell.IsEnabled = true;
                        }

                        //Added By Ashish Z on 07042016 for MRP Column
                        DataGridColumn dgcMRP = dgAddGRNItems.Columns[18];   //"MRP"
                        FrameworkElement feMRP = dgcMRP.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement fe1MRP = GetParent(feMRP, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1MRP;
                            cell.IsEnabled = false;
                        }

                        //Added By Ashish Z on 11042016 for VAT% Column
                        DataGridColumn dgcVAT = dgAddGRNItems.Columns[25];   //"Item Tax %"
                        FrameworkElement feVAT = dgcVAT.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement fe1VAT = GetParent(feVAT, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1VAT;
                            cell.IsEnabled = false;
                        }

                        //Added By Ashish Z on 11042016 for ItemTAX% Column
                        DataGridColumn dgcITAX = dgAddGRNItems.Columns[27];   //"VAT%"
                        FrameworkElement feITAX = dgcITAX.GetCellContent(dgAddGRNItems.SelectedItem);
                        FrameworkElement fe1ITAX = GetParent(feITAX, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1ITAX;
                            cell.IsEnabled = false;
                        }
                        //End

                        dgAddGRNItems.UpdateLayout();
                        dgAddGRNItems.Focus();

                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        //added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {

                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 126 : Batch Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID) + " "
                                                                  + "BatchCode : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchCode) + " "
                                                                  + "ExpiryDate : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ExpiryDate) + " "
                                                                  + "AvailableQuantity : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableQuantity) + " "
                                                                  + "BaseAvailableQuantity : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseAvailableQuantity) + " "
                                                                  + "Rate : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) + " "
                                                                  + "MainRate : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate) + " "
                                                                  + "BaseRate : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseRate) + " "
                                                                  + "MRP : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) + " "
                                                                  + "MainMRP : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP) + " "
                                                                  + "BaseMRP : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP) + " "
                                                                  + "BarCode : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode) + " "
                                                                  + "IsConsignment : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment) + " "
                                                                  + "IsBatchAssign : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsBatchAssign) + " "
                                                                  + "IsConsignment : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).IsConsignment) + " "
                                                                  + "Unit Rate : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UnitRate) + " "
                                                                  + "MRP : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP) + " "
                                                                  + "Single Quantity : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SingleQuantity) + " "
                                                                  + "Quantity : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity) + " "
                                                                  + "Conversion Factor: " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor) + " "
                                                                  + "Base Conversion Factor: " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor) + " "
                                                                  + "Cost Price: " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate) + " "
                                                                  + "PO Pending Quantity : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).POPendingQuantity) + " "
                                                                  + "VAT Percent : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        //
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
            }
        }

        private FrameworkElement GetParent(FrameworkElement child, Type targetType)
        {
            object parent = null;
            if (child != null)
            {
                parent = child.Parent;
            }
            if (parent != null)
            {
                if (parent.GetType() == targetType)
                {
                    return (FrameworkElement)parent;
                }
                else
                {
                    return GetParent((FrameworkElement)parent, targetType);
                }
            }
            return null;

        }

        #region Added By Ashish Thombre

        private void cmdAddAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFileAttached)
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                bool? dialogResult = OpenFile.ShowDialog();
                FileInfo Attachment;

                if (dialogResult.Value)
                {
                    try
                    {
                        using (Stream stream = OpenFile.File.OpenRead())
                        {
                            // Only allows file less than 5mb.
                            if (stream.Length < 5120000)
                            {
                                File = new byte[stream.Length];
                                stream.Read(File, 0, (int)stream.Length);
                                Attachment = OpenFile.File;
                                fileName = OpenFile.File.Name;
                                IsFileAttached = true;
                                txtFileName.Text = fileName.ToString();
                            }
                            else
                            {
                                MessageBox.Show("File must be less than 5 MB");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                if (this.ISEditMode)
                {
                    MessageBoxChildWindow cw;
                    cw = new MessageBoxChildWindow("", "Invoice File already attached.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    cw.Show();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWin5 =
                    new MessageBoxControl.MessageBoxChildWindow("", "Invoice File already attached, Do you want to Update?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWin5.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin5_OnMessageBoxClosed);

                    msgWin5.Show();
                }
            }
        }

        private void msgWin5_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                OpenFileDialog OpenFile = new OpenFileDialog();
                bool? dialogResult = OpenFile.ShowDialog();
                FileInfo Attachment;

                if (dialogResult.Value)
                {
                    try
                    {
                        using (Stream stream = OpenFile.File.OpenRead())
                        {
                            // Only allows file less than 5mb.
                            if (stream.Length < 5120000)
                            {
                                File = new byte[stream.Length];
                                stream.Read(File, 0, (int)stream.Length);
                                Attachment = OpenFile.File;
                                fileName = OpenFile.File.Name;
                                IsFileAttached = true;
                                txtFileName.Text = fileName.ToString();
                            }
                            else
                            {
                                MessageBox.Show("File must be less than 5 MB");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        private void ViewAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNList.SelectedItem != null)
            {
                GetGRNInvoiceFile((dgGRNList.SelectedItem as clsGRNVO).ID, (dgGRNList.SelectedItem as clsGRNVO).UnitId);
            }
        }

        private void ViewAttachment(string fileName, byte[] data)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

            client.UploadETemplateFileCompleted += (s, args) =>
            {
                if (args.Error == null)
                {
                    HtmlPage.Window.Invoke("openAttachment", new string[] { fileName });
                }
            };
            client.UploadETemplateFileAsync(fileName, data);
        }

        private void cmdDraft_Click(object sender, RoutedEventArgs e)
        {

            if (GRNAddedItems == null || GRNAddedItems.Count == 0)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("", "At least one item is compulsory for saving Goods Received Note", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                return;
            }

            Boolean blnValid = false;
            int iCount, iItemsCount = 0;
            iCount = GRNAddedItems.Count;
            foreach (clsGRNDetailsVO item in GRNAddedItems)
            {
                bool b = ValidateOnSave(item);
                if (b)
                {
                    blnValid = true;
                    iItemsCount++;
                }
                if (!b)
                {
                    return;
                }
            }
            if (blnValid && iItemsCount == iCount)
            {
                chkIsFinalized.IsChecked = false;

                string msgTitle = "Palash";
                string msgText = "";
                if (chkConsignment.IsChecked == false)
                {
                    msgText = "Are you sure you want to Save GRN as Draft?";
                }
                else
                {
                    msgText = "Are you sure you want to Save Consignment GRN ?";
                }

                MessageBoxControl.MessageBoxChildWindow msgWin1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWin1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWin1_OnMessageBoxClosed);

                msgWin1.Show();
            }

        }

        private void msgWin1_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                SaveGRN(true);
            }
            else if (result == MessageBoxResult.No)
                iItemsCount = 0;
        }

        #endregion
        private void FillGRNItems(long pGRNID, long UnitId)
        {
            try
            {
                clsGetGRNDetailsListByIDBizActionVO BizAction = new clsGetGRNDetailsListByIDBizActionVO();
                BizAction.GRNID = pGRNID;
                BizAction.UnitId = UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();
                        objList = new List<clsGRNDetailsVO>();
                        objList = ((clsGetGRNDetailsListByIDBizActionVO)e.Result).List;
                        GRNAddedItems.Clear();

                        foreach (var item in objList)
                        {
                            item.ItemQuantity = item.POPendingQuantity;
                            //item.BarCode = item.BatchCode;
                            item.SelectedUOM = new MasterListItem { ID = item.SelectedUOM.ID, Description = item.TransUOM };
                            item.POTransUOM = item.PurchaseUOM;
                            item.GRNApprItemQuantity = item.GRNApprItemQuantity;
                            item.GRNPendItemQuantity = item.GRNPendItemQuantity;
                            item.BaseRate = item.MainRate.DeepCopy();  // set Base Rate. 
                            item.BaseMRP = item.MainMRP.DeepCopy();    // set Base MRP which is use/reset when user edit MRP in GRN
                            GRNAddedItems.Add(item);
                        }

                        GRNPOAddedItems.Clear();
                        GRNPOAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                        List<clsGRNDetailsVO> objList2 = new List<clsGRNDetailsVO>();
                        objList2 = ((clsGetGRNDetailsListByIDBizActionVO)e.Result).POGRNList;

                        foreach (var item in objList2)
                        {
                            GRNPOAddedItems.Add(item);
                        }


                        #region For Free Items

                        List<clsGRNDetailsFreeVO> objListFree = new List<clsGRNDetailsFreeVO>();
                        objListFree = new List<clsGRNDetailsFreeVO>();
                        objListFree = ((clsGetGRNDetailsListByIDBizActionVO)e.Result).ListFree;

                        if (((clsGetGRNDetailsListByIDBizActionVO)e.Result).ListFree != null && ((clsGetGRNDetailsListByIDBizActionVO)e.Result).ListFree.Count > 0)
                        {
                            List<clsFreeMainItems> objMainList = new List<clsFreeMainItems>();
                            clsFreeMainItems objMain = new clsFreeMainItems();

                            objMain.ItemName = "- Select -";
                            objMainList.Add(objMain);

                            if (objListFree != null)
                            {
                                foreach (clsGRNDetailsFreeVO itemMain in objListFree)
                                {

                                    objMain = new clsFreeMainItems();

                                    objMain.ItemID = itemMain.MainItemID;
                                    objMain.ItemName = itemMain.MainItemName;
                                    objMain.ItemCode = itemMain.MainItemCode;
                                    objMain.BatchID = itemMain.MainBatchID;
                                    objMain.BatchCode = itemMain.MainBatchCode;
                                    objMain.ExpiryDate = itemMain.MainExpiryDate;
                                    objMain.MRP = itemMain.MainItemMRP;
                                    objMain.CostRate = itemMain.MainItemCostRate;

                                    if (itemMain.IsBatchRequired == true && itemMain.MainExpiryDate != null)
                                        objMain.ExpiryDateString = itemMain.MainExpiryDate.Value.ToString("MMM-yyyy");

                                    objMain.MainItemName = objMain.ItemName + " - " + objMain.BatchCode + " - " + objMain.ExpiryDateString;

                                    objMain.SrNo = itemMain.MainSrNo;  // Set & Use to link Free Item with Main Item


                                    if (GRNAddedItems.Where(z => z.ItemID == itemMain.FreeItemID && z.ItemID == itemMain.MainItemID).Count() > 0)
                                    {
                                        foreach (var item in GRNAddedItems.Where(a => a.ItemID == itemMain.FreeItemID && a.ItemID == itemMain.MainItemID).ToList())
                                        {
                                            item.FreeItemQuantity = itemMain.Quantity;
                                            item.FreeGSTAmount = itemMain.CGSTAmount + itemMain.SGSTAmount + itemMain.IGSTAmount;
                                        }
                                    }

                                    objMainList.Add(objMain);
                                }
                            }

                            MainList = new List<clsFreeMainItems>();
                            MainList = objMainList.DeepCopy();

                            cmbMainItem.ItemsSource = null;
                            cmbMainItem.ItemsSource = objMainList;
                            cmbMainItem.SelectedValue = (long)0;

                        }


                        GRNAddedFreeItems.Clear();

                        if (objListFree != null && objListFree.Count > 0)
                        {
                            foreach (var itemFree in objListFree)
                            {

                                //itemFree.BarCode = itemFree.FreeBatchCode;
                                itemFree.SelectedUOM = new MasterListItem { ID = itemFree.SelectedUOM.ID, Description = itemFree.TransUOM };
                                itemFree.POTransUOM = itemFree.PurchaseUOM;
                                itemFree.BaseRate = itemFree.MainRate.DeepCopy();  // set Base Rate. 
                                itemFree.BaseMRP = itemFree.MainMRP.DeepCopy();    // set Base MRP which is use/reset when user edit MRP in GRN

                                itemFree.MainList = this.MainList.DeepCopy();
                                var MainItem = (from r in MainList
                                                where r.SrNo == itemFree.MainSrNo && r.ItemID == itemFree.MainItemID
                                                select r);

                                if (MainItem != null && MainItem.ToList().Count > 0)
                                    itemFree.SelectedMainItem = ((List<clsFreeMainItems>)MainItem.ToList())[0];
                                GRNAddedFreeItems.Add(itemFree);
                            }
                        }
                        dgAddGRNFreeItems.ItemsSource = null;
                        dgAddGRNFreeItems.ItemsSource = GRNAddedFreeItems;
                        dgAddGRNFreeItems.UpdateLayout();
                        #endregion
                        CalculateGRNSummary();
                        dgAddGRNItems.ItemsSource = null;
                        dgAddGRNItems.ItemsSource = GRNAddedItems;
                        dgAddGRNItems.UpdateLayout();
                        dgAddGRNItems.Focus();

                        //***Added By Ashish Z on 15042016 for Enable or Disabled the Columns for Assigned Batch.
                        foreach (clsGRNDetailsVO item in dgAddGRNItems.ItemsSource)
                        {
                            if (item.BatchID > 0)
                            {
                                DataGridColumn column = dgAddGRNItems.Columns[2];

                                FrameworkElement fe = column.GetCellContent(item);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column2 = dgAddGRNItems.Columns[3];
                                FrameworkElement fe1 = column2.GetCellContent(item);
                                FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                                if (result1 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result1;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column3 = dgAddGRNItems.Columns[9];
                                FrameworkElement fe2 = column3.GetCellContent(item);
                                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                                if (result2 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result2;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column4 = dgAddGRNItems.Columns[12];
                                FrameworkElement fe4 = column4.GetCellContent(item);
                                FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                                if (result3 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result3;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column5 = dgAddGRNItems.Columns[13];
                                FrameworkElement fe5 = column5.GetCellContent(item);
                                FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                                if (result4 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result4;
                                    cell.IsEnabled = false;
                                }
                                //Added By CDS
                                DataGridColumn qty = dgAddGRNItems.Columns[9];
                                FrameworkElement feqty = qty.GetCellContent(item);
                                FrameworkElement resultqty = GetParent(feqty, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)resultqty;
                                    cell.IsEnabled = true;
                                }


                                DataGridColumn dgcMRP = dgAddGRNItems.Columns[18];  //"MRP"
                                FrameworkElement feMRP = dgcMRP.GetCellContent(item);
                                FrameworkElement fe1MRP = GetParent(feMRP, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1MRP;
                                    cell.IsEnabled = false;
                                }

                                //Added By Ashish Z on 11042016 for VAT% Column
                                DataGridColumn dgcVAT = dgAddGRNItems.Columns[25];
                                FrameworkElement feVAT = dgcVAT.GetCellContent(item);
                                FrameworkElement fe1VAT = GetParent(feVAT, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1VAT;
                                    cell.IsEnabled = false;
                                }

                                //Added By Ashish Z on 11042016 for ItemTAX% Column
                                DataGridColumn dgcITAX = dgAddGRNItems.Columns[27];
                                FrameworkElement feITAX = dgcITAX.GetCellContent(item);
                                FrameworkElement fe1ITAX = GetParent(feITAX, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1ITAX;
                                    cell.IsEnabled = false;
                                }
                                //End
                            }
                        }
                        //***

                        foreach (clsGRNDetailsVO item in dgAddGRNItems.ItemsSource)
                        {
                            if (item.IsBatchRequired == false)
                            {
                                DataGridColumn column = dgAddGRNItems.Columns[2];
                                FrameworkElement fe = column.GetCellContent(item);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.IsEnabled = false;
                                }
                            }

                            //Added BY CDS TO Handle null Reffernce 
                            if (!txtPONO.Text.Trim().Contains(item.PONO))
                                txtPONO.Text = String.Format((txtPONO.Text.Trim() + "," + item.PONO.Trim()).Trim(','));
                            //END
                        }


                        #region For Free Items

                        foreach (clsGRNDetailsFreeVO item in dgAddGRNFreeItems.ItemsSource)
                        {
                            if (item.FreeBatchID > 0)
                            {
                                DataGridColumn column = dgAddGRNFreeItems.Columns[3];  // Bathc Code 2

                                FrameworkElement fe = column.GetCellContent(item);
                                FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                                if (result != null)
                                {
                                    DataGridCell cell = (DataGridCell)result;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column2 = dgAddGRNFreeItems.Columns[4];  // Expiry Date 3
                                FrameworkElement fe1 = column2.GetCellContent(item);
                                FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                                if (result1 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result1;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column3 = dgAddGRNFreeItems.Columns[6];  // Quantity (Free) 
                                FrameworkElement fe2 = column3.GetCellContent(item);
                                FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                                if (result2 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result2;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column4 = dgAddGRNFreeItems.Columns[8];  // S. UOM  12
                                FrameworkElement fe4 = column4.GetCellContent(item);
                                FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                                if (result3 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result3;
                                    cell.IsEnabled = false;
                                }

                                DataGridColumn column5 = dgAddGRNFreeItems.Columns[9];   // Conversion Factor  13
                                FrameworkElement fe5 = column5.GetCellContent(item);
                                FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                                if (result4 != null)
                                {
                                    DataGridCell cell = (DataGridCell)result4;
                                    cell.IsEnabled = false;
                                }
                                //Added By CDS
                                DataGridColumn qty = dgAddGRNFreeItems.Columns[6];   // Quantity (Free) 9
                                FrameworkElement feqty = qty.GetCellContent(item);
                                FrameworkElement resultqty = GetParent(feqty, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)resultqty;
                                    cell.IsEnabled = true;
                                }


                                DataGridColumn dgcMRP = dgAddGRNFreeItems.Columns[14];  // MRP 18 
                                FrameworkElement feMRP = dgcMRP.GetCellContent(item);
                                FrameworkElement fe1MRP = GetParent(feMRP, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1MRP;
                                    cell.IsEnabled = false;
                                }

                                //Added For Item Tax % Column
                                DataGridColumn dgcVAT = dgAddGRNFreeItems.Columns[21];  // Item Tax % 25
                                FrameworkElement feVAT = dgcVAT.GetCellContent(item);
                                FrameworkElement fe1VAT = GetParent(feVAT, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1VAT;
                                    cell.IsEnabled = false;
                                }

                                //Added For VAT % Column
                                DataGridColumn dgcITAX = dgAddGRNFreeItems.Columns[23];  // VAT % 27
                                FrameworkElement feITAX = dgcITAX.GetCellContent(item);
                                FrameworkElement fe1ITAX = GetParent(feITAX, typeof(DataGridCell));

                                if (resultqty != null)
                                {
                                    DataGridCell cell = (DataGridCell)fe1ITAX;
                                    cell.IsEnabled = false;
                                }
                                //End
                            }
                        }

                        #endregion

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
        private void FillGRNAddDetailslList(long pGRNID, bool Freezed, long UnitId)
        {
            clsGetGRNDetailsListBizActionVO BizAction = new clsGetGRNDetailsListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();

                    objList = ((clsGetGRNDetailsListBizActionVO)e.Result).List;
                    GRNAddedItems.Clear();

                    int iGRN = 0;
                    double TotalQty = 0;

                    foreach (var item in objList)
                    {
                        iGRN = ((clsGetGRNDetailsListBizActionVO)e.Result).POGRNList.Where(z => z.ItemID == item.ItemID).Count();

                        if (iGRN != null && iGRN == 1 && item.PendingQuantity > 0)
                        {
                            //item.ItemQuantity = item.POPendingQuantity + item.Quantity;
                            item.ItemQuantity = item.POPendingQuantity + item.Quantity * item.BaseConversionFactor;
                        }
                        else
                        {
                            TotalQty = ((clsGetGRNDetailsListBizActionVO)e.Result).POGRNList.Where(z => z.ItemID == item.ItemID).ToList().Select(z => z.PendingQuantity).Sum();
                            Double dGrnQty = objList.Where(z => z.ItemID == item.ItemID && z.POID == item.POID && z.POUnitID == item.POUnitID).ToList().Select(z => z.Quantity).Sum();
                            item.PendingQuantity = TotalQty;
                            item.POPendingQuantity = TotalQty - dGrnQty;


                            //item.ItemQuantity = item.POPendingQuantity + item.Quantity;
                            item.ItemQuantity = item.POPendingQuantity + item.Quantity * item.BaseConversionFactor;

                        }

                        GRNAddedItems.Add(item);
                    }

                    lstGRNDetailsDeepCopy = GRNAddedItems.ToList().DeepCopy();
                    GRNPOAddedItems.Clear();
                    GRNPOAddedItems = new ObservableCollection<clsGRNDetailsVO>();

                    List<clsGRNDetailsVO> objList2 = new List<clsGRNDetailsVO>();
                    objList2 = ((clsGetGRNDetailsListBizActionVO)e.Result).POGRNList;

                    foreach (var item in objList2)
                    {
                        GRNPOAddedItems.Add(item);
                    }

                    dgAddGRNItems.ItemsSource = null;
                    dgAddGRNItems.ItemsSource = GRNAddedItems;

                    CalculateGRNSummary();
                    foreach (var item2 in GRNAddedItems)
                    {
                        CheckBatch(item2);
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CheckBatch(clsGRNDetailsVO item)
        {
            if (GRNAddedItems != null && GRNAddedItems.Count > 0)
            {

                WaitIndicator In = new WaitIndicator();
                In.Show();
                clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
                BizAction.BatchList = new List<clsItemStockVO>();
                if (cmbStore.SelectedItem != null)
                    BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;  // ((MasterListItem)cmbStore.SelectedItem).ID;
                BizAction.ItemID = item.ItemID;
                BizAction.ShowZeroStockBatches = true;

                BizAction.IsPagingEnabled = true;
                BizAction.MaximumRows = 100000;
                BizAction.StartIndex = DataList.PageIndex * 100000;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;
                        foreach (var item2 in result.BatchList)
                        {
                            if (item2.BatchCode == item.BatchCode)
                            {
                                item.IsBatchAssign = true;
                                item.AvailableQuantity = item2.AvailableStock;
                                SetControlForExistingBatch();
                                In.Close();
                            }

                        }
                        In.Close();
                    }
                    else
                    {
                        In.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }

        }



        public PagedSortableCollectionView<clsItemStockVO> BatchList { get; private set; }
        int RowCount = 0;
        AutoCompleteBox txtBatchCode;

        private void DataGridColumnVisibility()
        {
            Boolean blnReadOnly = false;
            if (chkIsFinalized.IsChecked == true)
            {
                blnReadOnly = true;
            }
            dgAddGRNItems.Columns[2].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[3].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[4].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[6].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[7].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[8].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[12].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[13].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[15].IsReadOnly = blnReadOnly;
            dgAddGRNItems.Columns[26].IsReadOnly = blnReadOnly;
            if (txtBatchCode != null) txtBatchCode.IsEnabled = blnReadOnly;
        }

        private void GetBatches(long ItemID, AutoCompleteBox CmbBatchCode)
        {
            BatchList = new PagedSortableCollectionView<clsItemStockVO>();
            clsGetItemStockBizActionVO BizAction = new clsGetItemStockBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId; // ((MasterListItem)cmbStore.SelectedItem).ID;
            BizAction.ItemID = ItemID;
            BizAction.ShowExpiredBatches = false;
            BizAction.ShowZeroStockBatches = true;
            BizAction.IsPagingEnabled = true;
            BizAction.MaximumRows = DataList.PageSize;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetItemStockBizActionVO result = ea.Result as clsGetItemStockBizActionVO;

                    BatchList.Clear();
                    //BatchList.TotalItemCount = result.TotalRows;
                    List<MasterListItem> ObjList = new List<MasterListItem>();
                    foreach (clsItemStockVO item in result.BatchList)
                    {
                        ObjList.Add(new MasterListItem() { ID = item.BatchID, Description = item.BatchCode });
                        BatchList.Add(item);
                    }
                    if (ObjList.Count > 0)
                    {
                        CmbBatchCode.ItemsSource = null;
                        CmbBatchCode.ItemsSource = ObjList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        void txtBatchCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //List<clsItemStockVO> ItemBatchDetailList = new List<clsItemStockVO>();
            //if (((AutoCompleteBox)sender).SelectedItem != null)
            //{
            //    string BatchCode = ((AutoCompleteBox)sender).SelectedItem.ToString();
            //    //List<clsItemStockVO> ItemBatchDetailList = new List<clsItemStockVO>();

            //    foreach (var item in BatchList)
            //    {
            //        if (dgAddGRNItems.SelectedItem != null && item.ItemID == ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID)
            //        {
            //            ItemBatchDetailList.Add(item);
            //        }
            //    }
            //    foreach (var batch in ItemBatchDetailList)
            //    {
            //        if (dgAddGRNItems.ItemsSource != null)
            //        {
            //            if (batch.ItemID == ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID && batch.BatchCode == BatchCode)
            //            {
            //                List<clsGRNDetailsVO> lstGRNItems = GRNAddedItems.ToList();
            //                int iDuplicateBatchCode = (from r in GRNAddedItems
            //                                           where r.BatchCode == batch.BatchCode
            //                                           && r.PONO != ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).PONO
            //                                           select new clsGRNDetailsVO
            //                                           {
            //                                               Status = r.Status,
            //                                               ID = r.ID
            //                                           }).ToList().Count();
            //                foreach (DataGridColumn dgColumn in dgAddGRNItems.Columns)
            //                {
            //                    dgColumn.IsReadOnly = true;
            //                }
            //                dgAddGRNItems.Columns[6].IsReadOnly = false;
            //                dgAddGRNItems.Columns[2].IsReadOnly = false;
            //                dgAddGRNItems.Columns[7].IsReadOnly = false;
            //                dgAddGRNItems.Columns[26].IsReadOnly = false;
            //                if (iDuplicateBatchCode > 1)
            //                {
            //                    (((AutoCompleteBox)sender).SelectedItem) = String.Empty;
            //                    MessageBoxControl.MessageBoxChildWindow msgW3 =
            //                                                           new MessageBoxControl.MessageBoxChildWindow("", "Same batch already exists!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //                    msgW3.Show();
            //                    break;
            //                }
            //                else
            //                {
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchID = batch.BatchID;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BatchCode = batch.BatchCode;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ExpiryDate = batch.ExpiryDate;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableQuantity = batch.AvailableStock;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = batch.MRP;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = batch.PurchaseRate;
            //                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = batch.BarCode;

            //                    break;
            //                }
            //            }
            //            else
            //            {
            //                //foreach (DataGridColumn dgColumn in dgAddGRNItems.Columns)
            //                //{
            //                //    dgColumn.IsReadOnly = false;
            //                //}
            //                //dgAddGRNItems.Columns[4].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[5].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[9].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[10].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[13].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[18].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[19].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[22].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[24].IsReadOnly = true;
            //                //dgAddGRNItems.Columns[25].IsReadOnly = true;

            //            }
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }
            //    }
            //}
        }

        clsGRNDetailsVO objBatchCode;
        private void GetAvailableQuantityForNonBatchItems(long itemID)
        {
            clsGetItemCurrentStockListBizActionVO BizAction = new clsGetItemCurrentStockListBizActionVO();
            BizAction.BatchList = new List<clsItemStockVO>();
            BizAction.ItemID = itemID;
            if (cmbStore.SelectedItem != null && ((clsStoreVO)cmbStore.SelectedItem).StoreId != 0)   //((MasterListItem)cmbStore.SelectedItem).ID 
            {
                BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;   //((MasterListItem)cmbStore.SelectedItem).ID;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if ((clsGetItemCurrentStockListBizActionVO)arg.Result != null)
                    {
                        clsGetItemCurrentStockListBizActionVO result = arg.Result as clsGetItemCurrentStockListBizActionVO;


                        foreach (var item in result.BatchList)
                        {
                            foreach (var item1 in GRNAddedItems)
                            {
                                if (item.ItemID == item1.ItemID)
                                {
                                    item1.AvailableQuantity = item.AvailableStock;
                                }
                            }
                        }

                    }

                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }

        private void GetGRNNumber()
        {
            clsGetAllGRNNumberBetweenDateRangeBizActionVO BizActionObject = new clsGetAllGRNNumberBetweenDateRangeBizActionVO();
            BizActionObject.ItemList = new List<clsItemMasterVO>();
            BizActionObject.MasterList = new List<MasterListItem>();
            if (cmbSearchSupplier.SelectedItem != null)
                BizActionObject.SupplierID = ((MasterListItem)cmbSearchSupplier.SelectedItem).ID;
            else
                BizActionObject.SupplierID = 0;
            BizActionObject.FromDate = dtpFromDate.SelectedDate;
            BizActionObject.ToDate = dtpToDate.SelectedDate;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Result != null && ea.Error == null)
                {
                    clsGetAllGRNNumberBetweenDateRangeBizActionVO result = ea.Result as clsGetAllGRNNumberBetweenDateRangeBizActionVO;

                    txtGRNNo.ItemsSource = null;
                    txtGRNNo.ItemsSource = result.MasterList;

                }

            };
            client.ProcessAsync(BizActionObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        MasterListItem SelectedItem1 = new MasterListItem();
        private void cmbItemName_KeyUp(object sender, KeyEventArgs e)
        {
            SelectedItem1 = new MasterListItem();
            SelectedItem1 = ((MasterListItem)txtGRNNo.SelectedItem);

        }

        private void cmbSearchSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetGRNNumber();
        }

        private void dgAddGRNItems_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                PreviousBatchValue = ((clsGRNDetailsVO)((DataGrid)sender).SelectedItem).BatchCode;


            }
        }

        //private void txtBatchCode_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    AutoCompleteBox tb = (AutoCompleteBox)sender;

        //    string BatchCode;
        //    BatchCode = tb.Text;
        //    //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem) != null)
        //    //{
        //    //    //ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

        //    //    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = BatchCode;
        //    //    if (BatchCode == "" || BatchCode == null)
        //    //        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = null;
        //    //}
        //    foreach (clsGRNDetailsVO item in (List<clsGRNDetailsVO>)dgAddGRNItems.ItemsSource)
        //    {
        //        if (item.BatchCode == BatchCode)
        //        {
        //            item.BarCode = BatchCode;
        //            break;
        //        }
        //    }
        //}

        private void txtBatchCode_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox tb = (AutoCompleteBox)sender;

            string BatchCode;
            BatchCode = tb.Text;
            //if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem) != null)
            //{
            //    //ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;

            //    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = BatchCode;
            //    if (BatchCode == "" || BatchCode == null)
            //        ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BarCode = null;
            //}

            //if (PreviousBatchValue != BatchCode)
            //{
            //    foreach (clsGRNDetailsVO item in (List<clsGRNDetailsVO>)dgAddGRNItems.ItemsSource)
            //    {

            //        if (item.BatchCode == BatchCode)
            //        {
            //            item.BarCode = BatchCode;
            //            break;
            //        }
            //    }
            //}
        }



        private void txtOtherCharges_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtOtherCharges_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveDoubleValid()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtGRNRoundOff_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsValidPositiveNegative()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtOtherCharges_LostFocus(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(txtOtherCharges.Text) && txtOtherCharges.Text.IsValueDouble())
            {
                CalculateNewNetAmount();
            }
            else
            {
                txtOtherCharges.Text = "0.00";
                ((clsGRNVO)this.DataContext).OtherCharges = Convert.ToDouble(txtOtherCharges.Text);
            }

            //if (!string.IsNullOrEmpty(txtOtherCharges.Text) && txtOtherCharges.Text.IsValueDouble())
            //{
            //    CalculateNewNetAmount();
            //}
            //else
            //{
            //    txtOtherCharges.Text = "0.00";
            //    ((clsGRNVO)this.DataContext).OtherCharges = Convert.ToDouble(txtOtherCharges.Text);
            //}
        }

        private void txtGRNDiscount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGRNDiscount.Text) && txtGRNDiscount.Text.IsValueDouble())
            {
                if (Convert.ToDouble(txtGRNDiscount.Text) > ((clsGRNVO)this.DataContext).NetAmount)
                {
                    txtGRNDiscount.SetValidation("Discount Amount must be less than Net Amount");
                    txtGRNDiscount.RaiseValidationError();
                }
                else
                {
                    txtGRNDiscount.ClearValidationError();
                    CalculateNewNetAmount();
                }
            }
            else
            {
                txtGRNDiscount.Text = "0.00";
                ((clsGRNVO)this.DataContext).GRNDiscount = Convert.ToDouble(txtGRNDiscount.Text);
            }

            //if (!string.IsNullOrEmpty(txtGRNDiscount.Text) && txtGRNDiscount.Text.IsValueDouble())
            //{
            //    if (Convert.ToDouble(txtGRNDiscount.Text) > ((clsGRNVO)this.DataContext).NetAmount)
            //    {
            //        txtGRNDiscount.SetValidation("Discount Amount must be less than Net Amount");
            //        txtGRNDiscount.RaiseValidationError();
            //    }
            //    else
            //    {
            //        txtGRNDiscount.ClearValidationError();
            //        CalculateNewNetAmount();
            //    }
            //}
            //else
            //{
            //    txtGRNDiscount.Text = "0.00";
            //    ((clsGRNVO)this.DataContext).GRNDiscount = Convert.ToDouble(txtGRNDiscount.Text);
            //}
        }

        private void txtGRNRoundOff_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtGRNRoundOffNetAmount.Text) && txtGRNRoundOffNetAmount.Text.IsValueDouble())
            {
                if (Convert.ToDouble(txtGRNRoundOffNetAmount.Text) + Comman.MinMaxRoundOff > ((clsGRNVO)this.DataContext).NetAmount)    //if (Convert.ToDouble(txtGRNRoundOffNetAmount.Text) + 5 > ((clsGRNVO)this.DataContext).NetAmount) 
                {
                    txtGRNRoundOffNetAmount.SetValidation("GRN Round Off Amount must not be Greater than Net Amount");
                    txtGRNRoundOffNetAmount.RaiseValidationError();
                }
                else if (Convert.ToDouble(txtGRNRoundOffNetAmount.Text) - Comman.MinMaxRoundOff < ((clsGRNVO)this.DataContext).NetAmount)  //else if( Convert.ToDouble(txtGRNRoundOffNetAmount.Text) - 5 < ((clsGRNVO)this.DataContext).NetAmount)
                {
                    txtGRNRoundOffNetAmount.SetValidation("GRN Round Off Amount must not be less than Net Amount");
                    txtGRNRoundOffNetAmount.RaiseValidationError();
                }
                else
                {
                    txtGRNRoundOffNetAmount.ClearValidationError();
                    //CalculateNewNetAmount();
                }
            }
            else
            {
                txtGRNRoundOffNetAmount.Text = "0.00";
                ((clsGRNVO)this.DataContext).GRNRoundOff = Convert.ToDouble(txtGRNRoundOffNetAmount.Text);
            }
        }

        private void CalculateNewNetAmount()
        {

            ((clsGRNVO)this.DataContext).OtherCharges = Convert.ToDouble(txtOtherCharges.Text);
            ((clsGRNVO)this.DataContext).GRNDiscount = Convert.ToDouble(txtGRNDiscount.Text);


            string netamt = Convert.ToString(((clsGRNVO)this.DataContext).PrevNetAmount + ((clsGRNVO)this.DataContext).OtherCharges - ((clsGRNVO)this.DataContext).GRNDiscount);
            txtNewNetAmount.Text = Convert.ToString(System.Math.Round(Convert.ToDecimal(netamt), 3));
            ((clsGRNVO)this.DataContext).NetAmount = Convert.ToDouble(txtNewNetAmount.Text);

            //((clsGRNVO)this.DataContext).NetAmount = System.Math.Round(Convert.ToDouble(netamt), 0);

            ((clsGRNVO)this.DataContext).GRNRoundOff = System.Math.Round(Convert.ToDouble(netamt), 0);

            //((clsGRNVO)this.DataContext).OtherCharges = Convert.ToDouble(txtOtherCharges.Text);
            //((clsGRNVO)this.DataContext).GRNDiscount = Convert.ToDouble(txtGRNDiscount.Text);
            //((clsGRNVO)this.DataContext).GRNRoundOff = Convert.ToDouble(txtGRNRoundOff.Text);


            //string netamt = Convert.ToString(((clsGRNVO)this.DataContext).PrevNetAmount + ((clsGRNVO)this.DataContext).OtherCharges - ((clsGRNVO)this.DataContext).GRNDiscount + ((clsGRNVO)this.DataContext).GRNRoundOff);
            //txtNewNetAmount.Text = Convert.ToString(System.Math.Round(Convert.ToDecimal(netamt), 3));
            //((clsGRNVO)this.DataContext).NetAmount = Convert.ToDouble(txtNewNetAmount.Text);
        }

        private bool ChkBatchExistsOrNot()
        {
            bool result = true;
            List<clsGRNDetailsVO> objList = GRNAddedItems.ToList<clsGRNDetailsVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    var chkitem = from r in GRNAddedItems
                                  where r.BatchCode != null && r.ExpiryDate != null && r.BatchCode == item.BatchCode && r.ExpiryDate.Value.Year == item.ExpiryDate.Value.Year && r.ExpiryDate.Value.Month == item.ExpiryDate.Value.Month &&
                                    r.ItemID == item.ItemID
                                  select new clsGRNDetailsVO
                                  {
                                      Status = r.Status,
                                      ID = r.ID
                                  };

                    if (chkitem.ToList().Count > 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.ItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                }
            }
            return result;
        }

        private bool ChkBatchExistsOrNotFree()
        {
            bool result = true;
            List<clsGRNDetailsFreeVO> objList = GRNAddedFreeItems.ToList<clsGRNDetailsFreeVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    var chkitem = from r in GRNAddedFreeItems
                                  where r.FreeBatchCode != null && r.FreeExpiryDate != null && r.FreeBatchCode == item.FreeBatchCode && r.FreeExpiryDate.Value.Year == item.FreeExpiryDate.Value.Year && r.FreeExpiryDate.Value.Month == item.FreeExpiryDate.Value.Month &&
                                    r.FreeItemID == item.FreeItemID
                                  select new clsGRNDetailsFreeVO
                                  {
                                      Status = r.Status,
                                      ID = r.ID
                                  };

                    if (chkitem.ToList().Count > 1)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Same Batch appears more than once for " + item.FreeItemName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        result = false;
                        return result;
                    }
                }
            }
            return result;
        }

        private void ViewAttachInvoice_Click(object sender, RoutedEventArgs e)
        {

            if (ISFromAproveGRN == true)
            {
                //clsGRNVO obj = SelectedGRN;
                //ViewAttachment(obj.FileName, obj.File);
                GetGRNInvoiceFile(SelectedGRN.ID, SelectedGRN.UnitId);
                IsFileAttached = true;
            }
            else
            {
                if (dgGRNList.SelectedItem != null && ISEditMode == true)
                {
                    //clsGRNVO obj = (clsGRNVO)dgGRNList.SelectedItem;
                    //ViewAttachment(obj.FileName, obj.File);
                    GetGRNInvoiceFile(((clsGRNVO)dgGRNList.SelectedItem).ID, ((clsGRNVO)dgGRNList.SelectedItem).UnitId);
                    IsFileAttached = true;
                }
                else if (IsFileAttached)
                {
                    //BizAction.IsFileAttached = true;
                    //BizAction.File = File;
                    //BizAction.FileName = fileName;

                    //clsGRNVO obj = (clsGRNVO)dgGRNList.SelectedItem;
                    ViewAttachment(fileName, File);
                }
            }
        }


        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserGRNCountWithRightsAndFrequencyBizActionVO objBizVO = new clsGetUserGRNCountWithRightsAndFrequencyBizActionVO();

                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserGRNCountWithRightsAndFrequencyBizActionVO)ea.Result).objUserRight;

                        //if (objUser.IsCrossAppointment)
                        //{
                        //    cmbAUnit.IsEnabled = true;
                        //}
                        //else
                        //{
                        //    cmbAUnit.IsEnabled = false;
                        //}


                        if (objUser.IsDirectPurchase == true)
                        {
                            //UserGRNCountForMonth
                            rdbDirectPur.Visibility = Visibility.Visible;
                            Int64 MaxCnt = objUser.FrequencyPerMonth;
                            decimal MaxPuAmt = objUser.MaxPurchaseAmtPerTrans;
                        }
                        else
                        {
                            rdbDirectPur.Visibility = Visibility.Collapsed;
                            rdbPo.IsChecked = true;
                        }
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private void cmbPUOM_GotFocus(object sender, RoutedEventArgs e)
        {
            AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList == null || ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList.Count == 0))
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

                BizAction.ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;
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

                        if (dgAddGRNItems.SelectedItem != null)
                        {
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();
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



        private void dgAddGRNItems_LostFocus(object sender, RoutedEventArgs e)
        {
            //CalculateGRNSummary();
        }
        private void dgAddGRNItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //((clsGRNDetailsVO)dgAddGRNItems.Columns[9]) = 10;
            //((DataGridTextColumn)dgAddGRNItems.Columns[9]).M = 10;

            //DataGridColumn qty = dgAddGRNItems.Columns[9];
            //FrameworkElement feqty = qty.GetCellContent(dgAddGRNItems.SelectedItem);
            //FrameworkElement resultqty = GetParent(feqty, typeof(DataGridCell));

            //if (resultqty != null)
            //{
            //    DataGridCell cell = (DataGridCell)resultqty;
            //    cell.le
            //    cell.IsEnabled = true;
            //}

            //if ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem != null)
            //{
            //    AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;

            //    if (cmbConversions.ItemsSource == null || (cmbConversions.ItemsSource != null && ((List<MasterListItem>)cmbConversions.ItemsSource).ToList().Count == 0) || (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList == null || ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList.Count == 0))
            //    {
            //        FillUOMConversions(cmbConversions);
            //    }
            //}

            //if (((clsGRNDetailsVO)e.Row.DataContext).SelectedUOM != null)
            //{
            //    AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
            //    FillUOMConversions(cmbConversions);

            //    ((clsGRNDetailsVO)e.Row.DataContext).SelectedUOM.ID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM.ID;
            //}
        }

        #region Conversion Factor/UOM
        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddGRNItems.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID, ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM.ID);
                win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                win.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButton_Click);
                win.Show();
            }
        }

        void win_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Conversion Itemswin = (Conversion)sender;

            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOMID);



        }

        private void cmbPUOM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAddGRNItems.SelectedItem != null)
            {
                //List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                //if (dgAddOpeningBalanceItems.SelectedItem != null)
                //    UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbConversions = (AutoCompleteBox)sender;
                long SelectedUomId = 0;

                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SingleQuantity = 0;
                ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity = 0;


                if (cmbConversions.SelectedItem != null && ((MasterListItem)cmbConversions.SelectedItem).ID > 0)
                {
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    ////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                    //CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOMID);
                }
                else
                {
                    //MasterListItem objConversionSet = new MasterListItem();
                    //objConversionSet.ID = 0;
                    //objConversionSet.Description = "- Select -";

                    //cmbConversions.SelectedItem = objConversionSet;

                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = 0;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor = 0;

                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate;
                }

                ////if (cmbConversions.SelectedItem != null)
                ////    SelectedUomId = ((MasterListItem)cmbConversions.SelectedItem).ID;

                ////if (UOMConvertLIst.Count > 0)
                ////    objConversion = UOMConvertLIst.Where(z => z.FromUOMID == SelectedUomId && z.ToUOMID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID).FirstOrDefault();

                //if (cmbConversions.SelectedItem != null)
                //{
                //    if (((MasterListItem)cmbConversions.SelectedItem).ID == ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                //    {
                //        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 1;
                //    }
                //    else
                //    {
                //        if (objConversion != null)
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = objConversion.ConversionFactor;
                //        }
                //        else
                //        {
                //            ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor = 0;
                //        }
                //    }
                //}

                ////objConversion.ID = 0;
                ////objConversion.Description = "- Select -";
                ////UOMConvertLIst.Add(objConversion);

                ////UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                ////cmbConversions.ItemsSource = UOMConvertLIst.DeepCopy();

                ////List<clsConversionsVO> ConvertLst = new List<clsConversionsVO>();
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());
                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.ToUOM).Select(y => y.First()).ToList());

                ////List<clsConversionsVO> MainConvertLst = new List<clsConversionsVO>();
                ////MainConvertLst = UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList().DeepCopy();

                ////ConvertLst.AddRange(UOMConversionLIst.GroupBy(x => x.FromUOM).Select(y => y.First()).ToList());


                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SingleQuantity = 0;
                //((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Quantity = 0;


                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID)
                ////{
                ////    if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor > 0)
                ////    {
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////        ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    }
                ////}
                ////else
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}

                ////if (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedPUM.Description != ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).PUOM)
                ////{

                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP;
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate;

                ////}
                ////else
                ////{


                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MRP = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainMRP / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);
                ////    ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).Rate = (((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).MainRate / ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).ConversionFactor);

                ////}


                //CalculateOpeningBalanceSummary();
                CalculateGRNSummary();
                CalculateNewNetAmount();
            }
        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgAddGRNItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity);
                    //objConversionVO.SingleQuantity = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Rate = objConversionVO.Rate;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).MRP = objConversionVO.MRP;


                    //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity = objConversionVO.Quantity;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity = objConversionVO.SingleQuantity;
                    //((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).TotalQuantity =Convert.ToDouble(objConversionVO.Quantity);
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;


                    clsGRNDetailsVO obj = (clsGRNDetailsVO)dgAddGRNItems.SelectedItem;
                    if (dgAddGRNItems.SelectedItem != null)
                    {
                        if (obj.ItemQuantity > 0)
                        {
                            if ((obj.ItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                 new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                //obj.Quantity = obj.ItemQuantity;
                                obj.Quantity = 0;
                                obj.POPendingQuantity = obj.ItemQuantity;
                                return;
                            }
                            else
                                obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                        }
                        else if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.Quantity = 0;
                            obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                            return;
                        }
                        else if (rdbDirectPur.IsChecked == false)
                        {
                            clsGRNDetailsVO objSelectedGRNItem = (clsGRNDetailsVO)dgAddGRNItems.SelectedItem;

                            double itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));

                            double Pendingqty = 0;
                            if (objSelectedGRNItem.GRNID != 0 && chkIsFinalized.IsChecked == false)
                            {
                                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;
                            }
                            else
                                Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;

                            if (itemQty <= Pendingqty)
                            {
                                foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                                {
                                    item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                                }
                            }
                            else
                            {

                                objSelectedGRNItem.Quantity = 0;
                                itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                                {
                                    item.POPendingQuantity = Pendingqty - itemQty; //(itemQty * item.BaseConversionFactor);
                                }
                            }

                            //if (rdbDirectPur.IsChecked != true)
                            //{
                            //    obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                            //}
                        }
                    }
                    CalculateGRNSummary();
                    CalculateNewNetAmount();
                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void FillUOMConversions()
        {
            WaitIndicator IndicatiorConversions = new WaitIndicator();
            try
            {

                IndicatiorConversions.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).ItemID;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        IndicatiorConversions.Close();

                        List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (dgAddGRNItems.SelectedItem != null)
                        {
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();

                        }
                        CalculateConversionFactorCentral(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SelectedUOM.ID, ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).SUOMID);

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                IndicatiorConversions.Close();
                throw;
            }
        }
        #endregion


        private void txtqty_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateGRNSummary();
            CalculateNewNetAmount();
        }

        void dgAddGRNFreeItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            DataGrid dgItems = (DataGrid)sender;
            clsGRNDetailsFreeVO obj = (clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem;

            double lngPOPendingQuantity = 0;
            Boolean blnAddItem = false;

            if (dgAddGRNFreeItems.SelectedItem != null)
            {
                if (e.Column.Header != null)
                {
                    #region Average Cost Calculations on 10042018
                    if (e.Column.Header.ToString().Equals("Free Received Quantity") || e.Column.Header.ToString().Equals("Cost Price") || e.Column.Header.ToString().Equals("SGST %") ||
                        e.Column.Header.ToString().Equals("CGST %") || e.Column.Header.ToString().Equals("IGST %"))
                    {
                        if (GRNAddedItems.Where(z => z.ItemID == obj.FreeItemID && z.ItemID == obj.SelectedMainItem.ItemID).Count() > 0)
                        {
                            foreach (var item in GRNAddedItems.Where(a => a.ItemID == obj.FreeItemID && a.ItemID == obj.SelectedMainItem.ItemID).ToList())
                            {
                                item.FreeItemQuantity = obj.Quantity;
                                item.FreeGSTAmount = obj.CGSTAmount + obj.SGSTAmount + obj.IGSTAmount;
                            }
                        }
                    }
                    #endregion

                    if (e.Column.Header.ToString().Equals("Free Received Quantity") || e.Column.Header.ToString().Equals("Cost Price")) //(e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price"))    //
                    {
                        //if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP > 0 && ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP < ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate)
                        if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP > 0 && ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP <= ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate) //Added by Prashant Channe on 09/01/2019, to always validate MRP > Rate
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "MRP should be greater than rate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate + 1;
                            return;
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 127 : Free Item Quantity Changed or Cost Price Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Cost Price : " + Convert.ToString(obj.Rate) + " "
                                                                  + "FreeQuantity : " + Convert.ToString(obj.FreeQuantity) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion

                    }
                    if (e.Column.Header.ToString().Equals("MRP"))
                    {
                        if (obj.MRP > 0 && obj.ConversionFactor > 0)
                        {
                            obj.UnitMRP = obj.MRP;
                        }
                        //Added By CDS

                        //if (obj.MRP > 0 && obj.ConversionFactor > 0)
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;
                        //}

                        if (e.Column.Header.ToString().Equals("MRP"))
                        {
                            if (dgAddGRNFreeItems.SelectedItem != null && ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor > 0)
                            {
                                ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseMRP = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP) / ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor;
                                ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainMRP = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP) / ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor;

                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 128 : Free Item MRP changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    if (e.Column.Header.ToString().Equals("Free Received Quantity"))  //(e.Column.Header.ToString().Equals("Free Quantity"))
                    {
                        #region  commented
                        //lngPOPendingQuantity = obj.POPendingQuantity;
                        ////if (lngPOPendingQuantity > 0)
                        ////{
                        //if (GRNAddedItems.Where(z => z.ItemID == obj.ItemID && z.UnitId == obj.UnitId && z.POID == obj.POID).Count() > 1)
                        //{
                        //    if (rdbDirectPur.IsChecked != true)
                        //    {
                        //        CellEditChanges(sender, e, lngPOPendingQuantity);
                        //        blnAddItem = true;
                        //    }
                        //}
                        ////}
                        ////else
                        ////{
                        ////    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        ////             new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        ////    msgW3.Show();
                        ////    obj.Quantity = 0;
                        ////    return;
                        ////}

                        //if (rdbDirectPur.IsChecked == false && obj.Quantity * obj.BaseConversionFactor == 0 && obj.FreeQuantity == 0)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //    new MessageBoxControl.MessageBoxChildWindow("", "Quantity In The List Can't Be Zero. Please enter Quantity or free quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();

                        //    obj.PendingQuantity = obj.PendingQuantity;
                        //    return;
                        //}
                        //else 
                        #endregion
                        if (obj.Quantity * obj.BaseConversionFactor == 0) //if (obj.Quantity * obj.BaseConversionFactor == 0 && obj.FreeQuantity == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                             new MessageBoxControl.MessageBoxChildWindow("", "Free Received Quantity In The List Can't Be Zero. Please enter Free Received Quantity greater than zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            return;
                        }
                        #region Commented
                        //if (!blnAddItem)
                        //{
                        ////By Anjali........................................................
                        ////if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        ////{
                        ////    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        ////     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        ////    msgW3.Show();
                        ////    obj.Quantity = 0;
                        ////    obj.PendingQuantity = obj.PendingQuantity;
                        ////    //CalculateGRNSummary();
                        ////    return;
                        ////}
                        ////if ((((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        ////{
                        ////    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        ////     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        ////    msgW3.Show();
                        ////    obj.Quantity = 0;
                        ////    obj.PendingQuantity = obj.PendingQuantity;
                        ////    //CalculateGRNSummary();
                        ////    return;
                        ////}

                        //if (obj.GRNPendItemQuantity > 0)
                        //{
                        //    //if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor))) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //    //{
                        //    //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //    //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    //    msgW3.Show();
                        //    //    obj.Quantity = 0;
                        //    //    obj.PendingQuantity = obj.PendingQuantity;
                        //    //    //CalculateGRNSummary();
                        //    //    return;
                        //    //}
                        //    if ((obj.PendingQuantity < ((obj.GRNPendItemQuantity - obj.GRNDetailsViewTimeQty) + (obj.Quantity * obj.BaseConversionFactor))) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //    //else if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }


                        //    if (obj.GRNApprItemQuantity > 0 && (((obj.PendingQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }
                        //    else if ((((obj.PendingQuantity + obj.GRNApprItemQuantity) - obj.GRNPendItemQuantity) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && (((obj.PendingQuantity + obj.GRNApprItemQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }
                        //    else if ((obj.PendingQuantity < ((obj.Quantity * obj.BaseConversionFactor))) && (((obj.PendingQuantity + obj.GRNApprItemQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }

                        //    if (obj.GRNApprItemQuantity > 0 && (obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //    //if (obj.GRNApprItemQuantity > 0 && (((obj.PendingQuantity) < (obj.Quantity * obj.BaseConversionFactor) + obj.GRNApprItemQuantity)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }
                        //    else if ((((obj.PendingQuantity)) < (obj.Quantity * obj.BaseConversionFactor)) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //CalculateGRNSummary();
                        //        return;
                        //    }
                        //}



                        //if ((obj.PendingQuantity < obj.GRNApprItemQuantity + obj.GRNPendItemQuantity) && rdbDirectPur.IsChecked == false && ISEditMode == true)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    obj.Quantity = 0;
                        //    obj.PendingQuantity = obj.PendingQuantity;
                        //    //CalculateGRNSummary();
                        //    return;
                        //}


                        //if ((obj.PendingQuantity - obj.GRNApprItemQuantity + obj.GRNPendItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false && ISEditMode == false)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "This exceeds Po Quantity Against Total Quantity Of GRN Raised + Being Raised.Can Not Save This Transaction ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    obj.Quantity = 0;
                        //    obj.PendingQuantity = obj.PendingQuantity;
                        //    //CalculateGRNSummary();
                        //    return;
                        //}

                        //if (obj.ItemQuantity > 0)
                        //{
                        //    if ((obj.ItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                        //    {
                        //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //        msgW3.Show();

                        //        //obj.Quantity = obj.ItemQuantity;
                        //        obj.Quantity = 0;
                        //        obj.PendingQuantity = obj.PendingQuantity;
                        //        //obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                        //        return;
                        //    }
                        //    else
                        //        obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                        //    //obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity;
                        //}

                        //else if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                        //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        //    msgW3.Show();
                        //    //obj.Quantity = obj.POPendingQuantity;
                        //    obj.Quantity = 0;
                        //    obj.PendingQuantity = obj.PendingQuantity;
                        //    //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                        //    //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                        //    CalculateGRNSummary();
                        //    return;
                        //}
                        //else
                        //{
                        //    if (rdbDirectPur.IsChecked != true)
                        //    {
                        //        //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                        //        obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                        //    }

                        //}
                        //}
                        #endregion
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region  added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 128 : Free Item Quantity Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Quantity : " + Convert.ToString(obj.Quantity) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion

                    }
                    if (e.Column.Header.ToString().Equals("Free Batch Code"))
                    {
                        //PreviousBatchValue = obj.BatchCode;
                        if (PreviousBatchValueFree != obj.FreeBatchCode)
                        {
                            //obj.BarCode = obj.FreeBatchCode;
                        }


                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 129 : Free Item Batch Code Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "ItemName : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "ItemID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "BarCode : " + Convert.ToString(obj.BarCode) + " "
                                                                   + "BatchCode : " + Convert.ToString(obj.FreeBatchCode) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }



                    if (e.Column.Header.ToString().Equals("C.Disc. %"))
                    {
                        if (obj.CDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Concession Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();

                            obj.CDiscountPercent = 0;
                            return;
                        }
                        if (obj.NetAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Discount Amount must be less than Net Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CDiscountPercent = 0;
                            return;
                        }

                        if (obj.CDiscountPercent == 100)
                        {

                            if (obj.SchDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter C. Discount Percentage! Sch.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();

                                obj.CDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }
                        if (obj.CDiscountPercent >= 0 && obj.CDiscountPercent < 100)
                        {
                            if (obj.SchDiscountPercent == 100)
                            {
                                obj.CDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }

                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 130 : Free Item C.Disc. % Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "ItemTax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VATPercent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "SchDiscountPercent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "CDiscountPercent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "NetAmount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion

                    }
                    if (e.Column.Header.ToString().Equals("Sch.Disc. %"))
                    {
                        if (obj.SchDiscountPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Sch. Discount Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SchDiscountPercent = 0;
                            return;
                        }
                        if (obj.NetAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                       new MessageBoxControl.MessageBoxChildWindow("", "Discount Amount must be less than Net Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SchDiscountPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                                          new MessageBoxControl.MessageBoxChildWindow("", "Can't Enter Sch. Discount Percentage! C.Discount Percent is 100% ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.SchDiscountPercent = 0;
                                return;
                            }
                            else
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }

                        }
                        if (obj.SchDiscountPercent >= 0 && obj.SchDiscountPercent < 100)
                        {
                            if (obj.CDiscountPercent == 100)
                            {
                                obj.SchDiscountPercent = 0;
                            }
                            else
                            {
                                if (obj.VATPercent == 0)
                                {
                                    obj.VATPercent = orgVatPer;
                                }
                                if (obj.ItemTax == 0)
                                {
                                    obj.ItemTax = orgTax;
                                }
                            }
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 131 : Free Sch.Disc. % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }
                    if (e.Column.Header.ToString().Equals("VAT%"))
                    {
                        if (obj.VATPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                       new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.VATPercent = 0;
                            return;
                        }
                        if (obj.VATPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                      new MessageBoxControl.MessageBoxChildWindow("", "VAT Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.VATPercent = 0;
                            return;
                        }

                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.VATPercent > 0)
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }

                        // Added BY CDS
                        //if (obj.VATPercent > 0 )
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    //obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;

                        //    //obj.VATAmount = (obj.MRP) - (((obj.MRP) / ((obj.VATPercent) + 100)) * 100);

                        //    obj.VATAmount = (obj.Amount) - (((obj.Amount) / (obj.VATPercent + 100)) * 100);

                        //}
                        //if (obj.VATPercent == 0)
                        //{
                        //    obj.VATAmount = 0;
                        //}

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 132 : Free VAT% changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    // Added By CDS to CST Tax %
                    //if (e.Column.Header.ToString().Equals("CST Tax %"))

                    if (e.Column.Header.ToString().Equals("Item Tax %"))
                    {
                        if (obj.ItemTax > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "ItemTax Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ItemTax = 0;
                            return;
                        }
                        if (obj.ItemTax < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "ItemTax Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ItemTax = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.VATPercent > 0)
                            {
                                if (obj.VATPercent != 0)
                                {
                                    orgVatPer = obj.VATPercent;
                                    obj.VATPercent = 0;
                                }
                                if (obj.ItemTax != 0)
                                {
                                    orgTax = obj.ItemTax;
                                    obj.ItemTax = 0;
                                }
                            }
                        }

                        // Added BY CDS
                        //if (obj.ItemTax > 0)
                        //{
                        //    //((M_ItemMaster.MRP)/ (ISNULL(M_ItemMaster.VatPer,0)+100)) *100 As  AbatedMRP;
                        //    //obj.AbatedMRP = ((obj.MRP) / ((obj.VATPercent) + 100)) * 100;

                        //    //obj.TaxAmount = (obj.MRP) - (((obj.MRP) / ((obj.ItemTax) + 100)) * 100);

                        //    obj.TaxAmount = (obj.Amount) - (((obj.Amount) / ((obj.ItemTax) + 100)) * 100);
                        //}
                        //if (obj.ItemTax == 0)
                        //{
                        //    obj.TaxAmount = 0;
                        //}
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }
                    //END

                    if (e.Column.Header.ToString().Equals("SGST %"))
                    {
                        if (obj.SGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "SGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SGSTPercent = 0;
                            return;
                        }
                        if (obj.SGSTPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "SGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.SGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.SGSTPercent > 0)
                            {
                                if (obj.SGSTPercent != 0)
                                {
                                    orgVatPer = obj.SGSTPercent;
                                    obj.SGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region Auditail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    if (e.Column.Header.ToString().Equals("CGST %"))
                    {
                        if (obj.CGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "CGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CGSTPercent = 0;
                            return;
                        }
                        if (obj.CGSTPercent < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "CGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.CGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.CGSTPercent > 0)
                            {
                                if (obj.CGSTPercent != 0)
                                {
                                    orgVatPer = obj.CGSTPercent;
                                    obj.CGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        #region Auditrail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }
                    if (e.Column.Header.ToString().Equals("IGST %"))
                    {
                        if (obj.IGSTPercent > 100)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                     new MessageBoxControl.MessageBoxChildWindow("", "IGST Percentage Greater Than 100%", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.IGSTPercent = 0;
                            return;
                        }
                        if (obj.IGSTAmount < 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "IGST Percentage can not be Negative", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.IGSTPercent = 0;
                            return;
                        }
                        if (obj.SchDiscountPercent == 100 || obj.CDiscountPercent == 100)
                        {
                            if (obj.IGSTPercent > 0)
                            {
                                if (obj.IGSTPercent != 0)
                                {
                                    orgVatPer = obj.IGSTPercent;
                                    obj.IGSTPercent = 0;
                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                        #region Auditrail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 133 : Free Item Tax % changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    if (e.Column.Header.ToString().Equals("Cost Price"))
                    {
                        if (obj != null)
                        {
                            if (obj.Rate == 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Zero Cost Price.", "Cost Price In The List Can't Be Zero. Please Enter Rate Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                return;
                            }
                            if (obj.Rate < 0)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                   new MessageBoxControl.MessageBoxChildWindow("Negative Cost Price.", "Cost Price Can't Be Negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                msgW3.Show();
                                obj.Rate = 0;
                                return;
                            }
                            else if (obj.Rate > 0 && obj.ConversionFactor > 0)
                            {
                                obj.UnitRate = obj.Rate / obj.ConversionFactor;
                            }

                            //Start: Added by Prashant Channe on 12/24/2018                            
                            if (dgAddGRNFreeItems.SelectedItem != null && ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor > 0)
                            {
                                ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseRate = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate) / ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor;
                                ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainRate = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate) / ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor;
                            }
                            //End: Added by Prashant Channe on 12/24/2018

                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 134 : Free Item Cost Price Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    //   if (e.Column.Header.ToString().Equals("Batch Code") || e.Column.Header.ToString().Equals("Expiry Date"))

                    if (e.Column.Header.ToString().Equals("Free Batch Code"))
                    {
                        if (obj != null)
                        {
                            foreach (var item in GRNAddedFreeItems)
                            {
                                if (item.FreeItemID == obj.FreeItemID)
                                {
                                    if (item.IsBatchRequired == true)
                                    {
                                        if (obj.FreeBatchCode == string.Empty || obj.FreeBatchCode == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                  new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW3.Show();
                                            return;

                                        }
                                        //      else if (obj.ExpiryDate == null)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          return;

                                        //      }
                                        //      else if (obj.ExpiryDate < DateTime.Now.Date)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date must not be less than today's date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          obj.ExpiryDate = DateTime.Now.Date;
                                        //      }
                                    }

                                }
                            }
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 135 : Free Item Batch Code Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Free BatchCode : " + Convert.ToString(obj.FreeBatchCode) + " "
                                                                  + "Free Batch ID : " + Convert.ToString(obj.FreeBatchID) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }


                    if (e.Column.Header.ToString().Equals("Free Expiry Date"))
                    {
                        if (obj != null)
                        {
                            foreach (var item in GRNAddedFreeItems)
                            {
                                if (item.FreeItemID == obj.FreeItemID)
                                {
                                    if (item.IsBatchRequired == true)
                                    {
                                        //      if (obj.BatchCode == string.Empty || obj.BatchCode == null)
                                        //      {
                                        //          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                        //new MessageBoxControl.MessageBoxChildWindow("Batch Required!.", "Please Enter Batch!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                        //          msgW3.Show();
                                        //          return;

                                        //      }
                                        if (obj.FreeExpiryDate == null)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                  new MessageBoxControl.MessageBoxChildWindow("Expiry Date Required!.", "Please Enter Expiry Date!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            return;
                                        }
                                        else if (obj.FreeExpiryDate < DateTime.Now.Date)
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date must not be less than today's date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            obj.FreeExpiryDate = DateTime.Now.Date;
                                        }
                                        else if (obj.FreeExpiryDate > DateTime.Now.AddYears(3))
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Expiry date is greater than three years!", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                            return;
                                        }

                                        if (item.IsBatchRequired && item.FreeExpiryDate.HasValue && item.ItemExpiredInDays > 0)
                                        {
                                            TimeSpan day = item.FreeExpiryDate.Value - DateTime.Now;
                                            int Day1 = (int)day.TotalDays;
                                            Int64 ExpiredDays = item.ItemExpiredInDays;
                                            if (Day1 < ExpiredDays)
                                            {
                                                MessageBoxControl.MessageBoxChildWindow msgW3 =
                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item has Expiry date less than" + ExpiredDays + " days !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                                msgW3.Show();
                                                //isValid = false;
                                                //return isValid;
                                            }
                                        }
                                        else if (item.IsBatchRequired && item.FreeExpiryDate.HasValue && item.ItemExpiredInDays == 0)
                                        {

                                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Item does not have Expiry In Days At Master Level", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                            msgW3.Show();
                                        }
                                    }

                                }
                            }
                        }
                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 136 : Free Item Expiry Date Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "BatchCode : " + Convert.ToString(obj.FreeBatchCode) + " "
                                                                  + "Free Batch ID : " + Convert.ToString(obj.FreeBatchID) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "Free ExpiryDate: " + Convert.ToString(obj.FreeExpiryDate) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }


                    if (e.Column.Header.ToString().Equals("Conversion Factor"))
                    {
                        if (obj.ConversionFactor <= 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW3 =
                                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Conversion factor must be greater than the zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW3.Show();
                            obj.ConversionFactor = 1;
                        }
                        else if (obj.ConversionFactor != null && obj.ConversionFactor > 0)
                        {
                            obj.MRP = obj.UnitMRP;
                            obj.Rate = obj.UnitRate;
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();

                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 137 : Free Item Conversion Factor changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Free Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Free Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Unit Rate : " + Convert.ToString(obj.UnitRate) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Bar Code : " + Convert.ToString(obj.BarCode) + " "
                                                                  + "Conversion Factor: " + Convert.ToString(obj.ConversionFactor) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "Item Tax : " + Convert.ToString(obj.ItemTax) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion
                    }

                    if (e.Column.Header.ToString().Equals("Free Received Quantity")) //(e.Column.Header.ToString().Equals("Free Quantity"))    // if (e.Column.DisplayIndex == 9)  Free Received Quantity
                    {

                        if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).UOMConversionList == null || ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).UOMConversionList.Count == 0)
                        {
                            ////DataGridColumn column = dgAddGRNItems.Columns[11];
                            ////FrameworkElement fe = column.GetCellContent(e.Row);
                            ////if (fe != null)
                            ////{
                            ////    //DataGridCell cell = (DataGridCell)result;
                            ////    FillUOMConversions();
                            ////}

                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity) * ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor;

                            ////if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity > ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).AvailableStockInBase)
                            ////{
                            ////    //float availQty = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase;
                            ////    //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).Quantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).AvailableStockInBase / ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor)));
                            ////    //string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                            ////    //ConversionsForAvailableStock();
                            ////    //MessageBoxControl.MessageBoxChildWindow msgWD =
                            ////    //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            ////    //msgWD.Show();
                            ////}
                            ////else 
                            //if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                            //     new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            //    msgW3.Show();
                            //    //obj.Quantity = obj.POPendingQuantity;
                            //    obj.Quantity = 0;
                            //    obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                            //    //obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity;
                            //    CalculateGRNSummary();
                            //    return;
                            //}
                        }
                        else if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM != null && ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM.ID > 0)
                        {
                            //////((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);
                            ////////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                            ////CalculateConversionFactor(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);
                            //// Function Parameters
                            //// FromUOMID - Transaction UOM
                            //// ToUOMID - Stocking UOM
                            //// if (((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList != null && ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).UOMConversionList.Count > 0)

                            CalculateConversionFactorCentralFree(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM.ID, ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SUOMID);

                            //// else
                            //// ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity * ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).BaseConversionFactor);
                        }
                        else
                        {
                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity = 0;
                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SingleQuantity = 0;

                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).ConversionFactor = 0;
                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor = 0;

                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainMRP;
                            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainRate;
                        }

                        CalculateGRNSummary();
                        CalculateNewNetAmount();


                        #region added by rohinee dated 28/9/2016 for audit trail
                        if (IsAuditTrail)
                        {

                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 138 : Free Item Quantity Changed : " //+ Convert.ToString(lineNumber)
                                                                  + "Item Name : " + Convert.ToString(obj.FreeItemName) + " "
                                                                  + "Item ID : " + Convert.ToString(obj.FreeItemID) + " "
                                                                  + "Unit Rate : " + Convert.ToString(obj.UnitRate) + " "
                                                                  + "MRP : " + Convert.ToString(obj.MRP) + " "
                                                                  + "Single Quantity : " + Convert.ToString(obj.SingleQuantity) + " "
                                                                  + "Quantity : " + Convert.ToString(obj.Quantity) + " "
                                                                  + "Conversion Factor: " + Convert.ToString(obj.ConversionFactor) + " "
                                                                  + "Base Conversion Factor: " + Convert.ToString(obj.BaseConversionFactor) + " "
                                                                  + "Cost Price: " + Convert.ToString(obj.Rate) + " "
                                                                  + "PO Pending Quantity : " + Convert.ToString(obj.POPendingQuantity) + " "
                                                                  + "VAT Percent : " + Convert.ToString(obj.VATPercent) + " "
                                                                  + "Sch Discount Percent : " + Convert.ToString(obj.SchDiscountPercent) + " "
                                                                  + "C Discount Percent : " + Convert.ToString(obj.CDiscountPercent) + " "
                                                                  + "Net Amount : " + Convert.ToString(obj.NetAmount) + " "
                                                                  + "Total Amount : " + Convert.ToString(txttotcamt.Text) + " "
                                                                  + "C.Disc. Amount : " + Convert.ToString(txtcdiscamt.Text) + " "
                                                                  + "Sch.Disc. Amount : " + Convert.ToString(txthdiscamt.Text) + " "
                                                                  + "Item Tax Amount : " + Convert.ToString(txttaxamount.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamt.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNetamt.Text) + " "
                                                                  + "Free Item VAT Amount : " + Convert.ToString(txtVatamtFree.Text) + " "
                                                                  + "Free Item Tax Amount : " + Convert.ToString(txttaxamountFree.Text) + " "
                                                                  + "Net Amount : " + Convert.ToString(txtNewNetAmount.Text) + " "
                                                                  ;
                            LogInfoList.Add(LogInformation);
                        }
                        #endregion

                    }


                    //if (e.Column.Header.ToString().Equals("PO Pending Quantity") || e.Column.Header.ToString().Equals("Quantity") || e.Column.Header.ToString().Equals("Free Quantity") || e.Column.Header.ToString().Equals("Cost Price") || e.Column.Header.ToString().Equals("Amount")
                    //   || e.Column.Header.ToString().Equals("C.Disc. Amount") || e.Column.Header.ToString().Equals("Sch.Disc. %") || e.Column.Header.ToString().Equals("C.Disc. %"))

                }
            }
        }

        private void lnkAddFreeItems_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemList ItemswinFree = new ItemList();
                ItemswinFree.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
                ItemswinFree.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                ItemswinFree.ShowBatches = false;
                if (cmbStore.SelectedItem == null || ((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)     //((MasterListItem)cmbStore.SelectedItem).ID
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();


                }
                else if (((MasterListItem)cmbSupplier.SelectedItem).ID == 0 || cmbSupplier.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("", "Please select supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                {

                    ItemswinFree.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;    // ((MasterListItem)cmbStore.SelectedItem).ID;
                    ItemswinFree.SupplierID = ((MasterListItem)cmbSupplier.SelectedItem).ID;
                    ItemswinFree.cmbStore.IsEnabled = false;
                    ItemswinFree.IsFromGRN = true;

                    ItemswinFree.OnSaveButton_Click += new RoutedEventHandler(ItemswinFree_OnSaveButton_Click);
                    ItemswinFree.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void ItemswinFree_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ItemList ItemswinFree = (ItemList)sender;
                RowCount = 0;
                //dgAddGRNItems.Columns[5].Visibility = Visibility.Collapsed;
                //dgAddGRNItems.Columns[6].Visibility = Visibility.Collapsed;
                //dgAddGRNItems.Columns[7].Visibility = Visibility.Collapsed;
                //dgAddGRNItems.Columns[8].Visibility = Visibility.Collapsed;
                //dgAddGRNItems.Columns[4].Visibility = Visibility.Visible;
                //dgAddGRNItems.Columns[16].IsReadOnly = true;
                ////dgAddGRNItems.Columns[18].IsReadOnly = true;  comented by Ashish z. for MRP Editable as per Client Requirement on 11-03-2016
                //dgAddGRNItems.Columns[28].Visibility = Visibility.Visible;


                if (ItemswinFree.SelectedItems != null)
                {
                    if (GRNAddedFreeItems == null)
                        GRNAddedFreeItems = new List<clsGRNDetailsFreeVO>();



                    foreach (var item in ItemswinFree.SelectedItems)
                    {
                        clsGRNDetailsFreeVO grnFreeObj = new clsGRNDetailsFreeVO();
                        grnFreeObj.IsBatchRequired = item.BatchesRequired;

                        //if (item.BatchesRequired == false)
                        //{ 
                        //    dgAddGRNItems.
                        //}

                        grnFreeObj.MainList = MainList;

                        //grnFreeObj.MainItemID = ((clsFreeMainItems)cmbMainItem.SelectedItem).ItemID;   //item.ID;
                        grnFreeObj.FreeItemID = item.ID;

                        //grnFreeObj.ItemTax = item.TotalPerchaseTaxPercent;
                        grnFreeObj.ItemTax = Convert.ToDouble(item.ItemVatPer);

                        //grnFreeObj.MainItemName = ((clsFreeMainItems)cmbMainItem.SelectedItem).ItemName;   //item.ID;
                        grnFreeObj.FreeItemName = item.ItemName;

                        //grnFreeObj.MainBatchCode = ((clsFreeMainItems)cmbMainItem.SelectedItem).BatchCode;
                        //grnFreeObj.MainExpiryDate = ((clsFreeMainItems)cmbMainItem.SelectedItem).ExpiryDate;

                        grnFreeObj.VATPercent = Convert.ToDouble(item.VatPer);
                        //grnFreeObj.Rate = Convert.ToDouble(item.PurchaseRate);
                        grnFreeObj.Rate = Convert.ToDouble(item.PurchaseRate) * Convert.ToDouble(item.PurchaseToBaseCF);
                        grnFreeObj.MainRate = Convert.ToSingle(item.PurchaseRate);

                        //grnFreeObj.MRP = Convert.ToDouble(item.MRP);
                        grnFreeObj.MRP = Convert.ToDouble(item.MRP) * Convert.ToDouble(item.PurchaseToBaseCF);
                        grnFreeObj.MainMRP = Convert.ToSingle(item.MRP);

                        grnFreeObj.AbatedMRP = Convert.ToDouble(item.AbatedMRP);
                        grnFreeObj.GRNItemVatType = item.ItemVatType;
                        grnFreeObj.GRNItemVatApplicationOn = item.ItemVatApplicationOn;

                        grnFreeObj.OtherGRNItemTaxType = item.ItemOtherTaxType;
                        grnFreeObj.OtherGRNItemTaxApplicationOn = item.OtherItemApplicationOn;

                        grnFreeObj.StockUOM = item.SUOM;
                        grnFreeObj.PurchaseUOM = item.PUOM;
                        //grnFreeObj.ConversionFactor = Convert.ToDouble(item.ConversionFactor);
                        grnFreeObj.ItemExpiredInDays = Convert.ToInt64(item.ItemExpiredInDays);

                        grnFreeObj.PUOM = item.PUOM;
                        grnFreeObj.MainPUOM = item.PUOM;
                        grnFreeObj.SUOM = item.SUOM;
                        //grnFreeObj.ConversionFactor = Convert.ToSingle(item.ConversionFactor);

                        grnFreeObj.PUOMID = item.PUM;
                        grnFreeObj.SUOMID = item.SUM;
                        grnFreeObj.BaseUOMID = item.BaseUM;
                        grnFreeObj.BaseUOM = item.BaseUMString;
                        grnFreeObj.SellingUOMID = item.SellingUM;
                        grnFreeObj.SellingUOM = item.SellingUMString;

                        grnFreeObj.SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                        grnFreeObj.TransUOM = item.PUOM;

                        grnFreeObj.ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        grnFreeObj.BaseConversionFactor = item.PurchaseToBaseCF;

                        grnFreeObj.BaseRate = Convert.ToSingle(item.PurchaseRate);
                        grnFreeObj.BaseMRP = Convert.ToSingle(item.MRP);

                        grnFreeObj.Rackname = item.Rackname;
                        grnFreeObj.Shelfname = item.Shelfname;
                        grnFreeObj.Containername = item.Containername;
                        //Added By Bhushanp 24062017 For GST                   
                        grnFreeObj.SGSTPercent = 0;  //((MasterListItem)cmbStoreState.SelectedItem).ID == ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.SGSTPercent) : 0;
                        grnFreeObj.CGSTPercent = 0;  //((MasterListItem)cmbStoreState.SelectedItem).ID == ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.CGSTPercent) : 0;
                        grnFreeObj.IGSTPercent = 0; // ((MasterListItem)cmbStoreState.SelectedItem).ID != ((MasterListItem)cmbSupplierState.SelectedItem).ID ? Convert.ToDouble(item.IGSTPercent) : 0;

                        grnFreeObj.GRNSGSTVatType = item.SGSTtaxtype;
                        grnFreeObj.GRNSGSTVatApplicationOn = item.SGSTapplicableon;
                        grnFreeObj.GRNCGSTVatType = item.CGSTtaxtype;
                        grnFreeObj.GRNCGSTVatApplicationOn = item.CGSTapplicableon;
                        grnFreeObj.GRNIGSTVatType = item.IGSTtaxtype;
                        grnFreeObj.GRNIGSTVatApplicationOn = item.IGSTapplicableon;


                        //if (item.AssignSupplier == false)
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Supplier is not assigned to item", "Do you want to assign supplier for the item:" + item.ItemName, MessageBoxControl.MessageBoxButtons.OkCancel, MessageBoxControl.MessageBoxIcon.Question);
                        //    msgWin.OnMessageBoxClosed += (re) =>
                        //    {
                        //        MessageBoxResult reply = msgWin.Result;

                        //        if (re == MessageBoxResult.OK)
                        //        {

                        //            grnFreeObj.AssignSupplier = true;
                        //        }
                        //        else
                        //            grnFreeObj.AssignSupplier = false;
                        //    };
                        //    msgWin.Show();
                        //}
                        //else
                        //    grnFreeObj.AssignSupplier = item.AssignSupplier;

                        if (item.BatchesRequired == false)
                        {
                            GetAvailableQuantityForNonBatchItems(item.ID);
                        }
                        var item2 = from r in GRNAddedFreeItems
                                    where r.FreeItemID == item.ID && item.BatchesRequired == false
                                    select r;

                        if (item2.ToList().Count > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWin = new MessageBoxControl.MessageBoxChildWindow("Palash", item.ItemName + " already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgWin.Show();
                        }
                        else
                        {
                            if (item.BatchesRequired == false)
                            {
                                GRNAddedFreeItems.Add(grnFreeObj);
                            }

                        }
                        if (item.BatchesRequired == true)
                        {
                            GRNAddedFreeItems.Add(grnFreeObj);
                        }

                    }

                    dgAddGRNFreeItems.ItemsSource = GRNAddedFreeItems;
                    dgAddGRNFreeItems.Focus();
                    dgAddGRNFreeItems.UpdateLayout();

                    dgAddGRNFreeItems.SelectedIndex = GRNAddedFreeItems.Count - 1;

                    //added by rohinee dated 27/9/2016 for audit trail    
                    #region
                    if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count > 0)
                    {
                        objGUID = new Guid();
                        if (IsAuditTrail)
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 112 : Get Free Items : "
                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " ";
                            foreach (var item in GRNAddedFreeItems)
                            {
                                LogInformation.Message = LogInformation.Message
                                                + " , Is Batch Required : " + Convert.ToString(item.IsBatchRequired) + " "
                                                + " , FreeItemID : " + Convert.ToString(item.FreeItemID) + " "
                                                + " , ItemTax : " + Convert.ToString(item.ItemTax) + " "
                                                + " , FreeItemName : " + Convert.ToString(item.FreeItemName) + " "
                                                + " , VAT Percent : " + Convert.ToString(item.VATPercent) + " "
                                                + " , Rate : " + Convert.ToString(item.Rate) + " "
                                                + " , Main Rate : " + Convert.ToString(item.MainRate) + " "
                                                + " , MRP : " + Convert.ToString(item.MRP) + " "
                                                + " , Main MRP : " + Convert.ToString(item.MainMRP) + " "
                                                + " , Abated MRP : " + Convert.ToString(item.AbatedMRP) + " "
                                                + " , GRN Item Vat Type : " + Convert.ToString(item.GRNItemVatType) + " "
                                                + " , GRN Item Vat Application On : " + Convert.ToString(item.GRNItemVatApplicationOn) + " "
                                                + " , Other GRN Item Tax Type : " + Convert.ToString(item.OtherGRNItemTaxType) + " "
                                                + " , Other GRN Item Tax ApplicationOn : " + Convert.ToString(item.OtherGRNItemTaxApplicationOn) + " "
                                                + " , Stock UOM : " + Convert.ToString(item.StockUOM) + " "
                                                + " , Purchase UOM : " + Convert.ToString(item.PurchaseUOM) + " "
                                                + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                                + " , Item Expired In Days : " + Convert.ToString(item.ItemExpiredInDays) + " "
                                                + " , PUOM : " + Convert.ToString(item.PUOM) + " "
                                                + " , Main PUOM : " + Convert.ToString(item.MainPUOM) + " "
                                                + " , SUOM : " + Convert.ToString(item.SUOM) + " "
                                                + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                                + " , SUOMID : " + Convert.ToString(item.SUOMID) + " "
                                                + " , Base UOM ID : " + Convert.ToString(item.BaseUOMID) + " "
                                                + " , Base UOM : " + Convert.ToString(item.BaseUOM) + " "
                                                + " , Selling UOM ID : " + Convert.ToString(item.SellingUOMID) + " "
                                                + " , Selling UOM : " + Convert.ToString(item.SellingUOM) + " "
                                                + " , Selected UOM : " + Convert.ToString(item.SelectedUOM) + " "
                                                + " , Trans UOM : " + Convert.ToString(item.TransUOM) + " "
                                                + " , Conversion Factor : " + Convert.ToString(item.ConversionFactor) + " "
                                                + " , Base Conversion Factor : " + Convert.ToString(item.BaseConversionFactor) + " "
                                                + " , Base Rate : " + Convert.ToString(item.BaseRate) + " "
                                                + " , Base MRP : " + Convert.ToString(item.BaseMRP) + " "
                                                + " , Rackname : " + Convert.ToString(item.Rackname) + " "
                                                + " , Shelfname : " + Convert.ToString(item.Shelfname) + " "
                                                + " , Containername : " + Convert.ToString(item.Containername) + " "
                                                + " , AssignSupplier : " + Convert.ToString(item.AssignSupplier) + " "
                                                ;
                            }
                            LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                            LogInfoList.Add(LogInformation);
                        }
                    }
                    #endregion
                    //=========================================================================

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgAddGRNFreeItems_GotFocus(object sender, RoutedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
            {
                PreviousBatchValueFree = ((clsGRNDetailsFreeVO)((DataGrid)sender).SelectedItem).FreeBatchCode;
            }
        }

        private void AddFreeBatch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkIsFinalized.IsChecked == false || ISFromAproveGRN == true)
                {
                    if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID > 0 && ((clsStoreVO)cmbStore.SelectedItem).StoreId > 0)  //((MasterListItem)cmbStore.SelectedItem).ID
                    {
                        cmbStore.ClearValidationError();
                        PalashDynamics.Radiology.ItemSearch.AssignBatch BatchWinFree = new PalashDynamics.Radiology.ItemSearch.AssignBatch();
                        BatchWinFree.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId; //((MasterListItem)cmbStore.SelectedItem).ID;
                        BatchWinFree.SelectedItemID = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID;
                        BatchWinFree.ItemName = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemName;

                        BatchWinFree.ShowZeroStockBatches = true;  // To show zero stock batches to add stock to it using GRN.

                        BatchWinFree.IsFree = true;  // Set to Show only Free Item Batches

                        BatchWinFree.OnAddButton_Click += new RoutedEventHandler(OnAddBatchButtonFree_Click);
                        BatchWinFree.Show();
                    }
                    else
                    {
                        if (cmbStore.SelectedItem == null)
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                        else if (((clsStoreVO)cmbStore.SelectedItem).StoreId == 0)   //((MasterListItem)cmbStore.SelectedItem).ID 
                        {
                            cmbStore.TextBox.SetValidation("Please select the store");
                            cmbStore.TextBox.RaiseValidationError();

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void OnAddBatchButtonFree_Click(object sender, RoutedEventArgs e)
        {
            PalashDynamics.Radiology.ItemSearch.AssignBatch AssignBatchWinFree = (PalashDynamics.Radiology.ItemSearch.AssignBatch)sender;

            if (AssignBatchWinFree.DialogResult == true && AssignBatchWinFree.SelectedBatches != null)
            {
                foreach (var item in AssignBatchWinFree.SelectedBatches)
                {
                    if (dgAddGRNFreeItems.ItemsSource != null)
                    {
                        List<clsGRNDetailsFreeVO> obclnGRNDetails = new List<clsGRNDetailsFreeVO>();

                        if (dgAddGRNFreeItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        {
                            obclnGRNDetails = (List<clsGRNDetailsFreeVO>)((PagedCollectionView)(dgAddGRNFreeItems.ItemsSource)).SourceCollection;
                        }
                        else
                            obclnGRNDetails = ((List<clsGRNDetailsFreeVO>)(dgAddGRNFreeItems.ItemsSource));

                        List<clsGRNDetailsFreeVO> grnItemList = obclnGRNDetails;

                        foreach (var BatchItems in grnItemList.Where(x => x.FreeItemID == item.ItemID && x.PONO == ((clsGRNDetailsFreeVO)(dgAddGRNFreeItems.SelectedItem)).PONO))
                        {
                            if (BatchItems.FreeBatchID == item.BatchID)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                             new MessageBoxControl.MessageBoxChildWindow("Palash", "Item with same batch already exists", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                return;
                            }
                        }

                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeBatchID = item.BatchID;
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeBatchCode = item.BatchCode;
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeExpiryDate = item.ExpiryDate;
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).AvailableQuantity = item.AvailableStock;
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseAvailableQuantity = item.AvailableStockInBase;


                        //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP = item.MRP;
                        //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate = item.PurchaseRate;

                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate = Convert.ToDouble(item.PurchaseRate) * Convert.ToDouble(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor);
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainRate = Convert.ToSingle(item.PurchaseRate);
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseRate = Convert.ToSingle(item.PurchaseRate);

                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP = Convert.ToDouble(item.MRP) * Convert.ToDouble(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor);
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainMRP = Convert.ToSingle(item.MRP);
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseMRP = Convert.ToSingle(item.MRP);


                        //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BarCode = item.BatchCode;//By Umeshitem.BarCode;

                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).ConversionFactor = item.ConversionFactor;

                        //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsConsignment = item.IsConsignment;
                        ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsBatchAssign = true;

                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM = new MasterListItem { ID = item.PUM, Description = item.PUOM };
                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).TransUOM = item.PUOM;


                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SUOM = item.SUOM;
                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SUOMID = item.SUM;

                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).ConversionFactor = item.PurchaseToBaseCF / item.StockingToBaseCF;
                        ////((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor = item.PurchaseToBaseCF;

                        //if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsConsignment == true)
                        //{
                        //    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsConsignment = true;
                        //    chkConsignment.IsChecked = true;

                        //}
                        //else
                        //{
                        //    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsConsignment = false;
                        //    chkConsignment.IsChecked = false;
                        //}





                        DataGridColumn column = dgAddGRNFreeItems.Columns[3];  //BatchCode  2

                        FrameworkElement fe = column.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement result = GetParent(fe, typeof(DataGridCell));

                        if (result != null)
                        {
                            DataGridCell cell = (DataGridCell)result;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column2 = dgAddGRNFreeItems.Columns[4];   //Expiry Date  3
                        FrameworkElement fe1 = column2.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement result1 = GetParent(fe1, typeof(DataGridCell));

                        if (result1 != null)
                        {
                            DataGridCell cell = (DataGridCell)result1;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column3 = dgAddGRNFreeItems.Columns[6]; // Quantity  9
                        FrameworkElement fe2 = column3.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement result2 = GetParent(fe2, typeof(DataGridCell));

                        if (result2 != null)
                        {
                            DataGridCell cell = (DataGridCell)result2;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column4 = dgAddGRNFreeItems.Columns[8];  // S.UOM  12
                        FrameworkElement fe4 = column4.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement result3 = GetParent(fe4, typeof(DataGridCell));

                        if (result3 != null)
                        {
                            DataGridCell cell = (DataGridCell)result3;
                            cell.IsEnabled = false;
                        }

                        DataGridColumn column5 = dgAddGRNFreeItems.Columns[9];   // Conversion Factor  13
                        FrameworkElement fe5 = column5.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement result4 = GetParent(fe5, typeof(DataGridCell));

                        if (result4 != null)
                        {
                            DataGridCell cell = (DataGridCell)result4;
                            cell.IsEnabled = false;
                        }
                        //Added By CDS
                        DataGridColumn qty = dgAddGRNFreeItems.Columns[6];  // Quantity  9
                        FrameworkElement feqty = qty.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement resultqty = GetParent(feqty, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)resultqty;
                            cell.IsEnabled = true;
                        }

                        DataGridColumn cst = dgAddGRNFreeItems.Columns[12];  // Cost  16
                        FrameworkElement fcst = cst.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement resultcst = GetParent(fcst, typeof(DataGridCell));

                        if (resultcst != null)
                        {
                            DataGridCell cell = (DataGridCell)resultcst;
                            cell.IsEnabled = false;
                        }

                        //Added By Ashish Z on 07042016 for MRP Column
                        DataGridColumn dgcMRP = dgAddGRNFreeItems.Columns[14];  // MRP   18
                        FrameworkElement feMRP = dgcMRP.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement fe1MRP = GetParent(feMRP, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1MRP;
                            cell.IsEnabled = false;
                        }

                        //Added By Ashish Z on 11042016 for Item Tax % Column
                        DataGridColumn dgcVAT = dgAddGRNFreeItems.Columns[21];  // Item Tax %  25
                        FrameworkElement feVAT = dgcVAT.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement fe1VAT = GetParent(feVAT, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1VAT;
                            cell.IsEnabled = false;
                        }

                        //Added By Ashish Z on 11042016 for VAT % Column
                        DataGridColumn dgcITAX = dgAddGRNFreeItems.Columns[23];   //  VAT%   27
                        FrameworkElement feITAX = dgcITAX.GetCellContent(dgAddGRNFreeItems.SelectedItem);
                        FrameworkElement fe1ITAX = GetParent(feITAX, typeof(DataGridCell));

                        if (resultqty != null)
                        {
                            DataGridCell cell = (DataGridCell)fe1ITAX;
                            cell.IsEnabled = false;
                        }
                        //End

                        dgAddGRNFreeItems.UpdateLayout();
                        dgAddGRNFreeItems.Focus();

                        CalculateGRNSummary();
                        CalculateNewNetAmount();
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Items already added", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                }
                //added by rohinee dated 27/9/2016 for audit trail
                if (IsAuditTrail)
                {
                    if (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem) != null)   //item sales return details                        {
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = " 113 : Batch Changed : " //+ Convert.ToString(lineNumber)
                                                              + "FreeBatchID : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeBatchID) + " "
                                                              + "FreeBatchCode : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeBatchCode) + " "
                                                              + "AvailableQuantity : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).AvailableQuantity) + " "
                                                              + "BaseAvailableQuantity : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseAvailableQuantity) + " "
                                                              + "Rate : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate) + " "
                                                              + "MainRate : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainRate) + " "
                                                              + "BaseRate : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseRate) + " "
                                                              + "MRP : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP) + " "
                                                              + "MainMRP : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainMRP) + " "
                                                              + "BaseMRP : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseMRP) + " "
                                                              + "BarCode : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BarCode) + " "
                                                              + "IsBatchAssign : " + Convert.ToString(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IsBatchAssign) + " "
                                                              ;
                        LogInfoList.Add(LogInformation);
                    }
                }
                //
            }
        }

        private void HyperlinkButtonFree_Click(object sender, RoutedEventArgs e)
        {
            if (dgAddGRNFreeItems.SelectedItem != null)
            {
                Conversion winFree = new Conversion();

                winFree.FillUOMConversions(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID, ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM.ID);
                winFree.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButtonFree_Click);
                winFree.OnCancelButton_Click += new RoutedEventHandler(win_OnCancelButtonFree_Click);
                winFree.Show();
            }
        }

        void win_OnSaveButtonFree_Click(object sender, RoutedEventArgs e)
        {
            Conversion ItemswinFree = (Conversion)sender;

            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedUOM = (MasterListItem)ItemswinFree.cmbConversion.SelectedItem;
            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).UOMList = ItemswinFree.UOMConvertLIst;
            ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).UOMConversionList = ItemswinFree.UOMConversionLIst;

            CalculateConversionFactorCentralFree(((MasterListItem)ItemswinFree.cmbConversion.SelectedItem).ID, ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SUOMID);

        }

        void win_OnCancelButtonFree_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CalculateConversionFactorCentralFree(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgAddGRNFreeItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).UOMConversionList;



                    objConversionVO.UOMConvertLIst = UOMConvertLIst;

                    objConversionVO.MainMRP = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainMRP;
                    objConversionVO.MainRate = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainRate;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity);
                    //objConversionVO.SingleQuantity = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SingleQuantity;

                    long BaseUOMID = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseUOMID;
                    //long TransactionUOMID = ((clsGRNDetailsFreeVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID;

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    // BaseUOMID - Base UOM
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Rate = objConversionVO.Rate;
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MRP = objConversionVO.MRP;


                    //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity = objConversionVO.Quantity;
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity = objConversionVO.SingleQuantity;
                    //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).TotalQuantity =Convert.ToDouble(objConversionVO.Quantity);
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseRate = objConversionVO.BaseRate;
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseMRP = objConversionVO.BaseMRP;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;


                    clsGRNDetailsFreeVO obj = (clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem;

                    #region Comented

                    //if (dgAddGRNItems.SelectedItem != null)
                    //{
                    //    if (obj.ItemQuantity > 0)
                    //    {
                    //        if ((obj.ItemQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                    //        {
                    //            MessageBoxControl.MessageBoxChildWindow msgW3 =
                    //             new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //            msgW3.Show();
                    //            //obj.Quantity = obj.ItemQuantity;
                    //            obj.Quantity = 0;
                    //            obj.POPendingQuantity = obj.ItemQuantity;
                    //            return;
                    //        }
                    //        else
                    //            obj.POPendingQuantity = obj.ItemQuantity - obj.Quantity * obj.BaseConversionFactor;
                    //    }
                    //    else if ((obj.PendingQuantity < obj.Quantity * obj.BaseConversionFactor) && rdbDirectPur.IsChecked == false)
                    //    {
                    //        MessageBoxControl.MessageBoxChildWindow msgW3 =
                    //         new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //        msgW3.Show();
                    //        obj.Quantity = 0;
                    //        obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                    //        return;
                    //    }
                    //    else if (rdbDirectPur.IsChecked == false)
                    //    {
                    //        clsGRNDetailsVO objSelectedGRNItem = (clsGRNDetailsVO)dgAddGRNItems.SelectedItem;

                    //        double itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));

                    //        double Pendingqty = 0;
                    //        if (objSelectedGRNItem.GRNID != 0 && chkIsFinalized.IsChecked == false)
                    //        {
                    //            Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;
                    //        }
                    //        else
                    //            Pendingqty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).First().PendingQuantity;

                    //        if (itemQty <= Pendingqty)
                    //        {
                    //            foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                    //            {
                    //                item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                    //            }
                    //        }
                    //        else
                    //        {

                    //            objSelectedGRNItem.Quantity = 0;
                    //            itemQty = GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                    //            MessageBoxControl.MessageBoxChildWindow msgW3 =
                    //            new MessageBoxControl.MessageBoxChildWindow("Quantity", "Quantity must not be greater than the pending quantity", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //            msgW3.Show();
                    //            foreach (clsGRNDetailsVO item in GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItem.ItemID && z.POID == objSelectedGRNItem.POID && z.UnitId == objSelectedGRNItem.UnitId).ToList())
                    //            {
                    //                item.POPendingQuantity = Pendingqty - itemQty; //(itemQty * item.BaseConversionFactor);
                    //            }
                    //        }

                    //        //if (rdbDirectPur.IsChecked != true)
                    //        //{
                    //        //    obj.POPendingQuantity = obj.PendingQuantity - obj.Quantity * obj.BaseConversionFactor;
                    //        //}
                    //    }
                    //}

                    #endregion

                    CalculateGRNSummary();
                    CalculateNewNetAmount();
                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void cmdDeleteFreeItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxControl.MessageBoxChildWindow msgWD =
                  new MessageBoxControl.MessageBoxChildWindow("", "Are you sure you want to delete the item ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        clsGRNDetailsFreeVO objSelectedGRNItemFree = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).DeepCopy();
                        clsGRNDetailsFreeVO objGRNDetailsFree = new clsGRNDetailsFreeVO();

                        if (dgAddGRNFreeItems.ItemsSource.GetType().Name.Equals("PagedCollectionView"))
                        {
                            objGRNDetailsFree = ((List<clsGRNDetailsFreeVO>)((PagedCollectionView)(dgAddGRNFreeItems.ItemsSource)).SourceCollection)[dgAddGRNFreeItems.SelectedIndex];
                        }
                        else
                            objGRNDetailsFree = ((List<clsGRNDetailsFreeVO>)(dgAddGRNFreeItems.ItemsSource))[dgAddGRNFreeItems.SelectedIndex];

                        double qty = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity * ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).BaseConversionFactor; //added by Ashish Z. double qty = ((clsGRNDetailsVO)dgAddGRNItems.SelectedItem).Quantity;
                        long itemid = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID;

                        //foreach (clsGRNDetailsFreeVO item in GRNAddedFreeItems.Where(z => z.FreeItemID == objSelectedGRNItemFree.FreeItemID && z.POID == objSelectedGRNItemFree.POID && z.POID == objSelectedGRNItemFree.POID).ToList())
                        //{
                        //    //item.POPendingQuantity = item.POQuantity - item.Quantity;
                        //    item.POPendingQuantity = (item.POQuantity * item.BaseConversionFactor) - item.Quantity * item.BaseConversionFactor;
                        //}

                        #region Average Cost Calculations on 10042018
                        if (GRNAddedItems.Where(z => z.ItemID == objSelectedGRNItemFree.FreeItemID && z.ItemID == objSelectedGRNItemFree.SelectedMainItem.ItemID).Count() > 0)
                        {
                            foreach (var item in GRNAddedItems.Where(a => a.ItemID == objSelectedGRNItemFree.FreeItemID && a.ItemID == objSelectedGRNItemFree.SelectedMainItem.ItemID).ToList())
                            {
                                item.FreeItemQuantity = 0;
                                item.FreeGSTAmount = 0;
                            }
                        }
                        #endregion

                        GRNAddedFreeItems.RemoveAt(dgAddGRNFreeItems.SelectedIndex);

                        if (ISEditMode == true)
                            GRNDeletedFreeItems.Add(objSelectedGRNItemFree);  // Use to maintain list of Deleted Free Items

                        //if (rdbPo.IsChecked == true)
                        //    GRNPOAddedItems.RemoveAt(dgAddGRNFreeItems.SelectedIndex);

                        dgAddGRNFreeItems.ItemsSource = null;

                        ////foreach (clsGRNDetailsVO item1 in GRNPOAddedItems)
                        ////{
                        ////    if (objGRNDetails.ItemID == item1.ItemID && objGRNDetails.POID == item1.POID && objGRNDetails.POUnitID == item1.POUnitID && objGRNDetails.PODetailID == item1.PODetailID && objGRNDetails.PODetailUnitID == item1.PODetailUnitID)
                        ////    {
                        ////        GRNPOAddedItems.Remove(item1);
                        ////    }
                        ////}   

                        //foreach (var item in GRNAddedFreeItems.Where(Z => Z.FreeItemID == itemid && Z.POID == objSelectedGRNItemFree.POID && Z.POID == objSelectedGRNItemFree.POID).ToList())
                        //{
                        //    if (objGRNDetailsFree.GRNID > 0)
                        //    {
                        //        item.POPendingQuantity = item.POPendingQuantity + qty;
                        //        item.ItemQuantity += qty;
                        //    }
                        //}

                        ////Added by Ashish Z. on dated 150416
                        //if (GRNAddedFreeItems != null && GRNAddedFreeItems.Count() > 0)
                        //{
                        //    double Pendingqty = 0;
                        //    Pendingqty = GRNAddedFreeItems.Where(z => z.FreeItemID == objSelectedGRNItemFree.FreeItemID && z.POID == objSelectedGRNItemFree.POID && z.POID == objSelectedGRNItemFree.POID).First().PendingQuantity;

                        //    double itemQty = 0;
                        //    itemQty = GRNAddedFreeItems.Where(z => z.FreeItemID == objSelectedGRNItemFree.FreeItemID && z.POID == objSelectedGRNItemFree.POID && z.POID == objSelectedGRNItemFree.POID).ToList().Sum(x => (x.Quantity * x.BaseConversionFactor));
                        //    if (itemQty <= Pendingqty)
                        //    {
                        //        foreach (clsGRNDetailsFreeVO item in GRNAddedFreeItems.Where(z => z.FreeItemID == objSelectedGRNItemFree.FreeItemID && z.POID == objSelectedGRNItemFree.POID && z.POID == objSelectedGRNItemFree.POID).ToList())
                        //        {
                        //            item.POPendingQuantity = Pendingqty - itemQty;  //(itemQty * item.BaseConversionFactor);
                        //        }
                        //    }
                        //}
                        ////End

                        dgAddGRNFreeItems.ItemsSource = GRNAddedFreeItems;
                        dgAddGRNFreeItems.UpdateLayout();
                        CalculateGRNSummary();

                        //txtPONO.Text = String.Empty;

                        //if (rdbPo.IsChecked == true)
                        //{
                        //    foreach (clsGRNDetailsFreeVO objDetail in GRNAddedFreeItems)
                        //    {
                        //        if (!txtPONO.Text.Contains(objDetail.PONO))
                        //            txtPONO.Text = String.Format(txtPONO.Text + "," + objDetail.PONO);
                        //    }
                        //    txtPONO.Text = txtPONO.Text.Trim(',');
                        //}

                        ////dgAddGRNItems.ItemsSource = GRNAddedItems;
                        ////dgAddGRNItems.UpdateLayout();
                        ////CalculateGRNSummary();
                        ////txtPONO.Text = String.Empty;

                    }
                    catch (Exception Ex)
                    {
                        throw Ex;
                    }
                }
            };
            msgWD.Show();
        }

        private void AddFreeItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgAddGRNFreeItems_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void dgAddGRNFreeItems_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void GRNInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPageLoaded)
            {
                GRNTabs SelectedTab = (GRNTabs)GRNInfo.SelectedIndex;

                if (SelectedTab == GRNTabs.Free)
                {
                    if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                    {

                        bool goToFree = true;

                        foreach (clsGRNDetailsVO item in GRNAddedItems)
                        {
                            if (item.IsBatchRequired == true)
                            {
                                if (item.BatchCode == null || item.BatchCode == "" || item.ExpiryDate == null)
                                {
                                    goToFree = false;
                                    break;
                                }
                            }
                        }

                        if (goToFree == true)
                        {
                            FillFreeMainItems();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter Batch Code and Expiry for all required items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();

                            GRNInfo.SelectedIndex = (int)GRNTabs.Main;
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW =
                                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Main Items.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW.Show();

                        GRNInfo.SelectedIndex = (int)GRNTabs.Main;
                    }
                }


            }

        }

        private void FillFreeMainItems()
        {
            try
            {
                if (GRNAddedItems != null && GRNAddedItems.Count > 0)
                {
                    List<clsFreeMainItems> objMainList = new List<clsFreeMainItems>();
                    clsFreeMainItems objMain = new clsFreeMainItems();

                    objMain.ItemName = "- Select -";
                    objMainList.Add(objMain);

                    foreach (clsGRNDetailsVO itemMain in GRNAddedItems)
                    {

                        objMain = new clsFreeMainItems();

                        objMain.ItemID = itemMain.ItemID;
                        objMain.ItemName = itemMain.ItemName;
                        objMain.ItemCode = itemMain.ItemCode;
                        objMain.BatchID = itemMain.BatchID;
                        objMain.BatchCode = itemMain.BatchCode;
                        objMain.ExpiryDate = itemMain.ExpiryDate;
                        objMain.MRP = itemMain.MRP;
                        objMain.CostRate = itemMain.CostRate;

                        if (itemMain.IsBatchRequired == true)
                            objMain.ExpiryDateString = itemMain.ExpiryDate.Value.ToString("MMM-yyyy");

                        objMain.MainItemName = objMain.ItemName + " - " + objMain.BatchCode + " - " + objMain.ExpiryDateString;

                        objMain.SrNo = itemMain.SrNo;  // Set & use to link SrNo of Main Item with Free Item

                        objMainList.Add(objMain);
                    }

                    MainList = objMainList.DeepCopy();

                    cmbMainItem.ItemsSource = objMainList;
                    cmbMainItem.SelectedValue = (long)0;

                    //clsGRNDetailsVO GRNItem;

                    foreach (clsGRNDetailsFreeVO itemFree in GRNAddedFreeItems)
                    {
                        //GRNItem = itemMain.Where(g => g.ItemID == item.ItemID).FirstOrDefault();

                        var MainItem = (from r in MainList
                                        where r.SrNo == itemFree.MainSrNo && r.ItemID == itemFree.MainItemID
                                        select r);

                        itemFree.MainList = MainList.DeepCopy();

                        if (MainItem != null && MainItem.ToList().Count > 0 && ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].ItemID > 0)
                        {
                            this.SetFreeItem = true;  // to reset the drop down selected item in Main Item column on Free Item tab.

                            itemFree.MainItemID = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].ItemID;

                            itemFree.MainItemName = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].ItemName;

                            itemFree.MainBatchCode = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].BatchCode;
                            itemFree.MainExpiryDate = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].ExpiryDate;

                            itemFree.MainItemMRP = Convert.ToSingle(((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].MRP);
                            itemFree.MainItemCostRate = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].CostRate;

                            itemFree.MainSrNo = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0].SrNo;  // Set & use to link SrNo of Main Item with Free Item

                            itemFree.SelectedMainItem = ((List<clsFreeMainItems>)MainItem.ToList()).DeepCopy()[0];

                            this.SetFreeItem = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private void cmbMain_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void cmbMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAddGRNFreeItems.SelectedItem != null)
            {
                //List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                //if (dgAddOpeningBalanceItems.SelectedItem != null)
                //    UOMConvertLIst = ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).UOMConversionList;

                clsConversionsVO objConversion = new clsConversionsVO();

                AutoCompleteBox cmbItemMain = (AutoCompleteBox)sender;

                if (cmbItemMain.SelectedItem != null && ((clsFreeMainItems)cmbItemMain.SelectedItem).ItemID > 0 && this.SetFreeItem == false)  // && (((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem == null || ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem.SrNo == 0 || ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem.SrNo != ((clsFreeMainItems)cmbItemMain.SelectedItem).SrNo))
                {
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainItemID = ((clsFreeMainItems)cmbItemMain.SelectedItem).ItemID;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainItemName = ((clsFreeMainItems)cmbItemMain.SelectedItem).ItemName;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainBatchCode = ((clsFreeMainItems)cmbItemMain.SelectedItem).BatchCode;
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainExpiryDate = ((clsFreeMainItems)cmbItemMain.SelectedItem).ExpiryDate;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainItemMRP = Convert.ToSingle(((clsFreeMainItems)cmbItemMain.SelectedItem).MRP);
                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainItemCostRate = ((clsFreeMainItems)cmbItemMain.SelectedItem).CostRate;

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).MainSrNo = ((clsFreeMainItems)cmbItemMain.SelectedItem).SrNo;  // Set & use to link SrNo of Main Item with Free Item

                    ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem = ((clsFreeMainItems)cmbItemMain.SelectedItem);

                    //((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem = ((clsFreeMainItems)cmbItemMain.SelectedItem);


                    if (GRNAddedItems.Where(z => z.ItemID == ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID && z.ItemID == ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem.ItemID).Count() > 0)
                    {
                        foreach (var item in GRNAddedItems.Where(a => a.ItemID == ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).FreeItemID && a.ItemID == ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SelectedMainItem.ItemID).ToList())
                        {
                            item.FreeItemQuantity = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).Quantity;
                            item.FreeGSTAmount = ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).CGSTAmount + ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).SGSTAmount + ((clsGRNDetailsFreeVO)dgAddGRNFreeItems.SelectedItem).IGSTAmount;
                        }
                    }


                }
                else
                {
                    SetFreeItem = false;

                }

            }
        }

        //Added by Ashish Z. for getting GRNInvoice on Dated 25012017
        private void GetGRNInvoiceFile(long GRNID, long GRNUnitID)
        {
            clsGetGRNListBizActionVO BizAction = new clsGetGRNListBizActionVO();
            BizAction.GRNVO = new clsGRNVO();
            BizAction.IsForViewInvoice = true;
            BizAction.GRNVO.ID = GRNID;
            BizAction.GRNVO.UnitId = GRNUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetGRNListBizActionVO result = e.Result as clsGetGRNListBizActionVO;
                    ViewAttachment(result.GRNVO.FileName, result.GRNVO.File);
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        //New Added By Bhushan For GST 24062017
        private void FillState()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_StateMaster;
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
                            if (item.ID != 0)
                            {
                                item.Description = item.Description + " - (" + item.Code + ")";
                            }
                        }
                        cmbStoreState.ItemsSource = null;
                        cmbStoreState.ItemsSource = objList;
                        cmbSupplierState.ItemsSource = null;
                        cmbSupplierState.ItemsSource = objList;

                        cmbStoreState.SelectedItem = objList[0];
                        cmbSupplierState.SelectedItem = objList[0];
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

        //End

        //private void dgAddGRNItems_LoadingRow(object sender, DataGridRowEventArgs e)
        //{

        //    DataGridColumn column22 = dgAddGRNItems.Columns[2];

        //    FrameworkElement fe22 = column22.GetCellContent(e.Row);
        //    FrameworkElement result22 = GetParent(fe22, typeof(DataGridCell));



        //    //if (((clsGRNVO)e.Row.DataContext). != null)
        //    //{
        //    //    ((clsChargeVO)e.Row.DataContext).SelectedDoctor = DoctorList.FirstOrDefault(p => p.ID == ((clsChargeVO)e.Row.DataContext).DoctorId);
        //    //}
        //    //if (((clsChargeVO)e.Row.DataContext).PackageID > 0)
        //    //{
        //    //    e.Row.IsEnabled = false;
        //    //}
        //    //else if (IsfromApprovalRequestWindow == true)
        //    //{
        //    //    for (int i = 0; i < dgCharges.Columns.Count(); i++)
        //    //    {
        //    //        if (i != 7 && i != 8)
        //    //            dgCharges.Columns[i].IsReadOnly = true;
        //    //    }
        //    //}

        //   // clsGRNDetailsVO item = (clsGRNDetailsVO)e.Row.DataContext;

        //    //for (int i = 0; i < dgAddGRNItems.Columns.Count(); i++)
        //    //{
        //    //    TextBox cbo = dgAddGRNItems.Columns[1].GetCellContent(e.Row) as TextBox;




        //        //FrameworkElement e2;
        //        //e2 = this.dgAddGRNItems.Columns[2].GetCellContent(e.Row);
        //        //DataGridCell changeCell = GetParent2(e2, typeof(DataGridCell)) as DataGridCell;
        //        foreach (var item1 in GRNAddedItems)
        //        {



        //           if (item1.IsBatchRequired == false)
        //           {
        //               DataGridCell cell = (DataGridCell)result22;
        //               cell.IsEnabled = false;
        //        //        cbo.IsEnabled = false;


        //                // (clsGRNVO)e.Row.DataContext as clsGRNVO();

        //                //  dgAddGRNItems.Columns[2].
        //                // FrameworkElement file;
        //                //dgAddGRNItems.Columns[2].IsReadOnly = true;

        //                //file = dgAddGRNItems.Columns[2].GetCellContent(e.Row)
        //                //file.Style.IsSealed = false;



        //                //dgvOpeningStock.Rows[e.RowIndex].Cells["ItemName"].Value 
        //                //((clsGRNVO)e.Rows[e.RowIndex].Cells[e.ColumnIndex].Value)

        //    }
        //          else
        //         {
        //             DataGridCell cell = (DataGridCell)result22;
        //             cell.IsEnabled = true;
        //    //            cbo.IsEnabled = true;
        //         }
        //    }
        //    //}

        //}


        //private FrameworkElement GetParent2(FrameworkElement child, Type targetType)
        //{
        //    object parent = child.Parent;
        //    if (parent != null)
        //    {
        //        if (parent.GetType() == targetType)
        //        {
        //            return (FrameworkElement)parent;
        //        }
        //        else
        //        {
        //            return GetParent2((FrameworkElement)parent, targetType);
        //        }
        //    }
        //    return null;
        //}
    }
}

