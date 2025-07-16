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
using System.Windows.Browser;
using System.ComponentModel;
using PalashDynamics.Collections;
using PalashDynamics.Pharmacy.ItemSearch;
using OPDModule.Forms;
using PalashDynamics.Pharmacy.Inventory;

namespace PalashDynamics.Pharmacy
{
    public partial class ScrapSaleDosApproval : UserControl, INotifyPropertyChanged
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

        #region Validation
        public Boolean BackPanelValidation()
        {
            bool isValid = true;
            if (dtScrabsalesDate.SelectedDate == null)
            {
                dtScrabsalesDate.SetValidation("Please Enter Item Scrap Sales Date");
                dtScrabsalesDate.RaiseValidationError();
                dtScrabsalesDate.Focus();
                isValid = false;
            }

            else if (cboStore.SelectedItem == null)
            {
                msgText = "Please Select Store";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                cboStore.Focus();
                isValid = false;
            }
            else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            {
                msgText = "Please Select Store";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                cboStore.Focus();
                isValid = false;
            }

            //if(string.IsNullOrEmpty(txtbSupplier.Text))
            //{
            //     txtbSupplier.SetValidation("Please Enter Supplier");
            //    txtbSupplier.RaiseValidationError();
            //    txtbSupplier.Focus();
            //    return false;
            //}
            else if (txtbSupplier.SelectedItem == null)
            {
                msgText = "Please Select Supplier";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                txtbSupplier.Focus();
                isValid = false;
            }
            else if (((MasterListItem)txtbSupplier.SelectedItem).ID == 0)
            {
                msgText = "Please Select Supplier";

                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                txtbSupplier.Focus();
                isValid = false;
            }


            else if (string.IsNullOrEmpty(txtModeofTransport.Text))
            {
                txtModeofTransport.SetValidation("Please Enter Mode Of Transport");
                txtModeofTransport.RaiseValidationError();
                txtModeofTransport.Focus();
                isValid = false;
            }

            else if (dgScrapSalesItems.ItemsSource == null)
            {
                msgText = "Scrap Sales Grid Cannot Be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                isValid = false;
            }
            else if (ScarpsAddedItems.Count <= 0)
            {
                msgText = "Scrap Sales Grid Cannot Be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                isValid = false;
            }

                //Added By Pallavi
            else if (ScarpsAddedItems.Count > 0 && ScarpsAddedItems != null)
            {
                List<clsSrcapDetailsVO> objList = ScarpsAddedItems.ToList<clsSrcapDetailsVO>();
                MessageBoxControl.MessageBoxChildWindow mgbx = null;
                if (objList != null && objList.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        if (item.SaleQty == 0)
                        {
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Sale Quantity Zero!.", "Sale Quantity In The List Can't Be Zero. Please Sale Quantity Greater Than Zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            isValid = false;
                            break;
                        }
                        if (item.SaleQty > item.AvailableStock)
                        {
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash!.", "Scrap Sale Quantity In The List Can't Be Greater Than Available Stock. Please Scrap Sale Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            isValid = false;
                            break;
                        }
                        if (Convert.ToDouble(item.ScrapRate) != null && Convert.ToDouble(item.ScrapRate) < 0)
                        {
                            mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash!.", "Scrap rate cannot be negative.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            mgbx.Show();
                            isValid = false;
                            break;
                        }

                    }
                    //return true;
                }
                else
                    isValid = false;
            }
            else
            {

                dtScrabsalesDate.ClearValidationError();
                txtbSupplier.ClearValidationError();
                txtModeofTransport.ClearValidationError();
                isValid = true;
            }

            return isValid;


        }
        #endregion

        #region Variables

        private SwivelAnimation objAnimation = null;
        bool IsPageLoaded = false;
        string msgTitle = "PALASH";
        string msgText = "";
        double TotalTaxAmount = 0;
        double TotalRate = 0;
        double NetAmount = 0;
        Int64 ScrapId = 0;
        String FilterExpr;
        bool Flag = false;
        bool ShowMsg = false;
        public PagedSortableCollectionView<clsSrcapVO> MasterList { get; private set; }
        public ObservableCollection<clsSrcapDetailsVO> ScarpsAddedItems { get; set; }
        #endregion Variables

        #region Properties
        public int PageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
                OnPropertyChanged("PageSize");
            }
        }
        #endregion

        #region Constructor

        public ScrapSaleDosApproval()
        {
            InitializeComponent();
            objAnimation = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(ScrapSale_Loaded);
            ScarpsAddedItems = new ObservableCollection<clsSrcapDetailsVO>();
            FillStore();
            //Added By Pallavi
            FillSupplier();

            dgScrapSalesItems.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgScrapSalesItems_CellEditEnded);
            dgScrapSalesItems.CellEditEnding += new EventHandler<DataGridCellEditEndingEventArgs>(dgScrapSalesItems_CellEditEnding);
            SetCommandButtonState("New");


            MasterList = new PagedSortableCollectionView<clsSrcapVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.dgScrapSalesList.DataContext = MasterList;
            setupPage();
        }

        void MasterList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setupPage();
        }

        #endregion Constructor

        #region Load Event
        void ScrapSale_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFromDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;

            this.DataContext = new clsSrcapVO();
            dtScrabsalesDate.IsEnabled = false;
        }
        #endregion  Load Event

        #region  Public Methods

        public void setupPage()
        {
            clsGetScrapSalesDetailsBizActionVO BizAction = new clsGetScrapSalesDetailsBizActionVO();
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = MasterList.PageSize;
            BizAction.StartRowIndex = MasterList.PageIndex * MasterList.PageSize;
            if (cmbStore.SelectedItem != null)
            {
                BizAction.StoreID = ((clsStoreVO)cmbStore.SelectedItem).StoreId;
            }

            BizAction.FromDate = dtpFromDate.SelectedDate;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            else
                BizAction.ToDate = dtpFromDate.SelectedDate;

            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    BizAction.MasterDetail = new List<clsSrcapVO>();
                    BizAction.MasterDetail = ((clsGetScrapSalesDetailsBizActionVO)args.Result).MasterDetail;
                    MasterList.Clear();
                    MasterList.TotalItemCount = (int)(((clsGetScrapSalesDetailsBizActionVO)args.Result).TotalRows);
                    foreach (clsSrcapVO item in ((clsGetScrapSalesDetailsBizActionVO)args.Result).MasterDetail)
                    {
                        MasterList.Add(item);
                    }
                    dgScrapSalesList.ItemsSource = BizAction.MasterDetail;
                    if (BizAction.MasterDetail != null && BizAction.MasterDetail.Count > 0)
                        dgScrapSalesList.SelectedItem = BizAction.MasterDetail[0];
                    txtTotalCountRecords.Text = MasterList.TotalItemCount.ToString();
                }

                if (this.DataContext != null)
                {
                    //cmbTaxNature.SelectedValue = ((clsItemMasterVO)this.DataContext).;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }



        public void FillScrapItemdatagrid()
        {
            clsGetScrapSalesItemsDetailsBizActionVO BizAction = new clsGetScrapSalesItemsDetailsBizActionVO();
            BizAction.ItemScrapSaleId = ScrapId;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {
                    dgScrapSalesItemsList.ItemsSource = null;
                    dgScrapSalesItemsList.ItemsSource = ((clsGetScrapSalesItemsDetailsBizActionVO)args.Result).MasterDetail;
                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void ClearUI()
        {

            txtModeofTransport.Text = "";
            txtNetAmount.Text = "";
            txtRemarks.Text = "";
            txtTaxAmount.Text = "";
            txtTotalRate.Text = "";


            dgScrapSalesItems.ItemsSource = null;
            chkIsApproved.IsChecked = false;
            dtScrabsalesDate.SelectedDate = DateTime.Now.Date;
            ScarpsAddedItems = new ObservableCollection<clsSrcapDetailsVO>();

            this.DataContext = new clsSrcapVO();
            cboStore.SelectedValue = ((clsSrcapVO)this.DataContext).StoreID;
            txtbSupplier.SelectedValue = ((clsSrcapVO)this.DataContext).SupplierID;

        }

        #endregion  Public Methods

        #region Set Command Button State New/Save
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "Save":
                    cmdPrint.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;

                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdPrint.IsEnabled = false;
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Fill Comboboxes
        #region Added By Pallavi
        public void FillSupplier()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Supplier;

            // BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString() };

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();


                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    txtbSupplier.ItemsSource = null;
                    txtbSupplier.ItemsSource = objList;

                    txtbSupplier.SelectedItem = objList[0];

                }

                if (this.DataContext != null)
                {
                    txtbSupplier.SelectedValue = ((clsSrcapVO)this.DataContext).SupplierID;
                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        #endregion
        public void FillStore()
        {
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_Store;
            //BizAction.IsActive = true;

            //BizAction.Parent = new KeyValue { Value = "ClinicID", Key = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString() };

            //BizAction.MasterList = new List<MasterListItem>();
            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //client.ProcessCompleted += (s, args) =>
            //{

            //    if (args.Error == null && args.Result != null)
            //    {

            //        List<MasterListItem> objList = new List<MasterListItem>();


            //        objList.Add(new MasterListItem(0, "-- Select --", true));
            //        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

            //        cboStore.ItemsSource = null;
            //        cboStore.ItemsSource = objList;
            //        cmbStore.ItemsSource = objList;
            //        cmbStore.SelectedValue = (long)0;
            //        cboStore.SelectedItem = objList[0];

            //    }

            //    if (this.DataContext != null)
            //    {
            //        cmbStore.SelectedValue = ((clsSrcapVO)this.DataContext).StoreID;
            //    }


            //};

            //client.ProcessAsync(BizAction, new clsUserVO());
            //client.CloseAsync();


            clsGetStoreDetailsBizActionVO BizActionObj = new clsGetStoreDetailsBizActionVO();
            //False when we want to fetch all items
            clsItemMasterVO obj = new clsItemMasterVO();
            //obj.RetrieveDataFlag = false;
            ////By Anjali.......................
            //BizActionObj.StoreType = 1;
            ////..................................

            BizActionObj.IsUserwiseStores = true;
            BizActionObj.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
            BizActionObj.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            obj.RetrieveDataFlag = false;

            BizActionObj.ItemMatserDetails = new List<clsStoreVO>();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);


            client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {

                    BizActionObj.ToStoreList = ((clsGetStoreDetailsBizActionVO)args.Result).ToStoreList;
                    clsStoreVO Default = new clsStoreVO { StoreId = 0, StoreName = "--Select--", ClinicId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, Status = true, IsQuarantineStore = true, Parent = 0 };
                    BizActionObj.ToStoreList.Insert(0, Default);

                    var result = from item in BizActionObj.ToStoreList
                                 where item.ClinicId == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && item.IsQuarantineStore == true
                                 select item;




                    cboStore.ItemsSource = null;
                    cboStore.ItemsSource = result.ToList();
                    cmbStore.ItemsSource = result.ToList();
                    cmbStore.SelectedValue = (long)0;
                    cboStore.SelectedItem = result.ToList()[0];



                    if (this.DataContext != null)
                    {
                        cmbStore.SelectedValue = ((clsSrcapVO)this.DataContext).StoreID;
                    }


                }

            };

            client.CloseAsync();
        }

        #endregion Fill Comboboxes

        #region Cell Editing Events

        void dgScrapSalesItems_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            TotalRate = 0;
            TotalTaxAmount = 0;
            NetAmount = 0;
            foreach (clsSrcapDetailsVO item in dgScrapSalesItems.ItemsSource)
            {
                TotalRate = TotalRate + item.Rate;
                TotalTaxAmount = TotalTaxAmount + item.VATAmount;
                NetAmount = NetAmount + item.TotalAmount;


            }
            txtTotalRate.Text = TotalRate.ToString();
            txtTaxAmount.Text = TotalTaxAmount.ToString();
            txtNetAmount.Text = NetAmount.ToString();
        }

        void dgScrapSalesItems_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

            if (e.Column.DisplayIndex == 5)
            {
                //if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty > ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStock)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW3 =
                //                      new MessageBoxControl.MessageBoxChildWindow("Greater Scrap Sale Quantity!", " Scrap Sale Quantity In The List Can't Be Greater Than Available Stock. Please Enter Scrap Sale Quantity Less Than Or Equal To Available Stock.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgW3.Show();
                //    if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStock > 0)
                //        ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = 1;
                //    else
                //        ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = 0;
                //}


                if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).UOMConversionList == null || ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).UOMConversionList.Count == 0)
                {
                    //DataGridColumn column = dgPharmacyItems.Columns[7];
                    //FrameworkElement fe = column.GetCellContent(e.Row);
                    //if (fe != null)
                    //{
                    //    //DataGridCell cell = (DataGridCell)result;

                    //    FillUOMConversions();
                    //}
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty) * ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseConversionFactor;

                    if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseQuantity > ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase)
                    {
                        float availQty = ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase;

                        ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase / ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseConversionFactor)));
                        string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                        ConversionsForAvailableStock();
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }


                }
                else if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SelectedUOM != null && ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SelectedUOM.ID > 0)
                {
                    ////((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM = ((MasterListItem)cmbConversions.SelectedItem);

                    //////objConversion = CalculateConversionFactor(((MasterListItem)cmbConversions.SelectedItem).ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).BaseUOMID);
                    //CalculateConversionFactor(((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SelectedUOM.ID, ((clsOpeningBalVO)dgAddOpeningBalanceItems.SelectedItem).SUOMID);

                    // Function Parameters
                    // FromUOMID - Transaction UOM
                    // ToUOMID - Stocking UOM
                    CalculateConversionFactorCentral(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SelectedUOM.ID, ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SUOMID);

                }
                else
                {
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = 0;
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SingleQuantity = 0;

                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).ConversionFactor = 0;
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseConversionFactor = 0;

                }


            }
            CalculateSummary();
            //TotalRate = 0;
            //TotalTaxAmount = 0;
            //NetAmount = 0;
            //foreach (clsSrcapDetailsVO item in dgScrapSalesItems.ItemsSource)
            //{
            //    TotalRate = TotalRate + item.Rate;
            //    TotalTaxAmount = TotalTaxAmount + item.VATAmount;
            //    NetAmount = NetAmount + item.TotalAmount;


            //}
            ////txtTotalRate.Text = TotalRate.ToString();
            //txtTotalRate.Text = String.Format("{0:0.00}", TotalRate);
            ////txtTaxAmount.Text = TotalTaxAmount.ToString();
            //txtTaxAmount.Text = String.Format("{0:0.00}", TotalTaxAmount);
            ////txtNetAmount.Text = NetAmount.ToString();
            //txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);

        }
        #endregion Cell Editing Events

        #region ButtonClickEvent

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("ClickNew");
            ClearUI();
            objAnimation.Invoke(RotationType.Forward);
            ((clsSrcapVO)this.DataContext).Date = DateTime.Now.Date;
            dtScrabsalesDate.SelectedDate = DateTime.Now.Date;
        }
        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = ClickedFlag1 + 1;
            if (ClickedFlag1 == 1)
            {
                if (BackPanelValidation())
                {
                    clsAddScrapSalesDetailsBizActionVO bizactionVO = new clsAddScrapSalesDetailsBizActionVO();
                    clsSrcapDetailsVO addNewScrapDetailsVO = new clsSrcapDetailsVO();

                    clsSrcapVO addNewScrapVO = new clsSrcapVO();
                    try
                    {
                        addNewScrapVO.IsApproved = (bool)chkIsApproved.IsChecked;
                        if (rdbCashMode.IsChecked == true)
                        {
                            addNewScrapVO.PaymentModeID = (int)MaterPayModeList.Cash;
                        }
                        else if (rdbCreditMode.IsChecked == true)
                        {
                            addNewScrapVO.PaymentModeID = (int)MaterPayModeList.Cheque;
                        }
                        addNewScrapVO.ModeOfTransport = txtModeofTransport.Text;
                        addNewScrapVO.Remark = txtRemarks.Text;
                        //addNewScrapVO.ScrapID = Convert.ToInt64(cmbTermsofPayment.SelectedValue);
                        addNewScrapVO.ScrapSaleNo = null;
                        addNewScrapVO.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                        addNewScrapVO.SupplierID = ((MasterListItem)txtbSupplier.SelectedItem).ID;

                        addNewScrapVO.SupplierName = txtbSupplier.Text;
                        addNewScrapVO.Time = DateTime.Now;
                        addNewScrapVO.Date = dtScrabsalesDate.SelectedDate;
                        addNewScrapVO.TotalAmount = Convert.ToDouble(txtNetAmount.Text);
                        addNewScrapVO.TotalRate = Convert.ToDouble(txtTotalRate.Text);
                        addNewScrapVO.TotalTaxAmount = Convert.ToDouble(txtTaxAmount.Text);

                        addNewScrapVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewScrapVO.Status = true;
                        addNewScrapVO.AddUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        addNewScrapVO.By = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        addNewScrapVO.On = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                        addNewScrapVO.DateTime = System.DateTime.Now;
                        addNewScrapVO.WindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;




                        foreach (clsSrcapDetailsVO items in ScarpsAddedItems)
                        {
                            addNewScrapDetailsVO = new clsSrcapDetailsVO();
                            addNewScrapDetailsVO.ScrapSalesItemID = addNewScrapVO.ScrapID;
                            addNewScrapDetailsVO.Amount = items.Amount;
                            addNewScrapDetailsVO.BatchID = items.BatchID;
                            addNewScrapDetailsVO.DiscAmt = items.DiscAmt;
                            addNewScrapDetailsVO.DiscPerc = items.DiscPerc;
                            addNewScrapDetailsVO.ItemCode = items.ItemCode;
                            addNewScrapDetailsVO.ItemId = items.ItemId;
                            addNewScrapDetailsVO.ItemName = items.ItemName;
                            addNewScrapDetailsVO.Rate = items.Rate;
                            addNewScrapDetailsVO.ScrapRate = items.ScrapRate;
                            addNewScrapDetailsVO.Remark = txtRemarks.Text;
                            addNewScrapDetailsVO.SaleQty = items.SaleQty;
                            addNewScrapDetailsVO.TotalAmount = items.TotalAmount;
                            addNewScrapDetailsVO.NetAmount = items.NetAmount;
                            addNewScrapDetailsVO.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                            addNewScrapDetailsVO.SUOM = items.SUOM;
                            addNewScrapDetailsVO.VATAmount = items.VATAmount;
                            addNewScrapDetailsVO.VATPerc = items.VATPerc;
                            addNewScrapDetailsVO.TaxPercentage = items.VATPerc;
                            addNewScrapDetailsVO.TaxAmount = items.VATAmount;
                            //By Anjali...........................
                            addNewScrapDetailsVO.SelectedUOM = items.SelectedUOM;
                            addNewScrapDetailsVO.ConversionFactor = items.ConversionFactor;
                            addNewScrapDetailsVO.BaseConversionFactor = items.BaseConversionFactor;
                            // addNewScrapDetailsVO.StockCF = items.StockCF;
                            addNewScrapDetailsVO.BaseQuantity = items.BaseQuantity;
                            addNewScrapDetailsVO.BaseUOMID = items.BaseUOMID;
                            addNewScrapDetailsVO.SUOMID = items.SUOMID;
                            //..........................................

                            bizactionVO.ItemsDetail.Add(addNewScrapDetailsVO);
                        }
                        bizactionVO.ItemMatserDetail = addNewScrapVO;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, args) =>
                        {
                            ClickedFlag1 = 0;
                            if (args.Error == null && args.Result != null)
                            {
                                if (((clsAddScrapSalesDetailsBizActionVO)args.Result).SuccessStatus == 1)
                                {
                                    msgText = "Record is successfully submitted!";
                                    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgWindow.Show();
                                    //After Insertion Back to BackPanel and Setup Page
                                    SetCommandButtonState("Save");
                                    setupPage();
                                    objAnimation.Invoke(RotationType.Backward);
                                    //cmdAdd.IsEnabled = true;
                                    //cmdSave.IsEnabled = false;
                                }
                            }

                        };
                        client.ProcessAsync(bizactionVO, new clsUserVO());
                        client.CloseAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                    ClickedFlag1 = 0;
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            dtScrabsalesDate.ClearValidationError();
            txtbSupplier.ClearValidationError();
            txtModeofTransport.ClearValidationError();
            objAnimation.Invoke(RotationType.Backward);
            dtScrabsalesDate.SelectedDate = DateTime.Now.Date;

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            #region Commented By Shikha
            FilterExpr = "";
            if (cmbStore.SelectedItem != null)
            {
                FilterExpr = "StoreID='" + ((MasterListItem)cmbStore.SelectedItem).ID + "' OR";
            }
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                FilterExpr = FilterExpr + "  Date = '" + dtpFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            }
            if (dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate != null)
            {
                FilterExpr = FilterExpr + "Date ='" + dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            }
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {
                FilterExpr = FilterExpr + "  Date >= '" + dtpFromDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            }
            if (dtpFromDate.SelectedDate == null && cmbStore.SelectedItem != null)
            {
                FilterExpr = FilterExpr.Substring(0, FilterExpr.Length - 3);
            }
            if (dtpToDate.SelectedDate != null && dtpFromDate.SelectedDate != null)
            {
                FilterExpr = FilterExpr + " and Date <='" + dtpToDate.SelectedDate.Value.ToString("yyyy-MM-dd") + "'";
            }


            setupPage();
            #endregion
            MasterList = new PagedSortableCollectionView<clsSrcapVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(MasterList_OnRefresh);
            PageSize = 10;
            this.dataGrid2Pager.DataContext = MasterList;
            this.dgScrapSalesList.DataContext = MasterList;
            setupPage();
        }

        private void lnkAddItems_Click(object sender, RoutedEventArgs e)
        {
            #region Commented

            //ItemListNew Itemswin = new ItemListNew();
            //Itemswin.loggedinUser = ((IApplicationConfiguration)App.Current).CurrentUser;
            //Itemswin.ClinicID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //if (cboStore.SelectedItem == null)
            //{
            //    msgText = "Please Select Store";

            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWindow.Show();
            //    cboStore.Focus();

            //}
            //else if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0)
            //{
            //    msgText = "Please Select Store";

            //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //    msgWindow.Show();
            //    cboStore.Focus();

            //}
            //else
            //{

            //    Itemswin.ShowBatches = true;
            //    Itemswin.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
            //    Itemswin.ShowScrapItems = true;
            //    Itemswin.cmbStore.IsEnabled = false;
            //    Itemswin.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click);
            //    Itemswin.Show();
            //}

            #endregion

            try
            {
                if (cboStore.SelectedItem != null && ((clsStoreVO)cboStore.SelectedItem).StoreId != 0 && txtbSupplier.SelectedItem != null && ((MasterListItem)txtbSupplier.SelectedItem).ID != 0)
                {
                    //ExpiryItemReturnMaster = new clsExpiredItemReturnVO();
                    ExpiredItemSearch win = new ExpiredItemSearch();
                    win.StoreID = ((clsStoreVO)cboStore.SelectedItem).StoreId;
                    win.SupplierID = ((MasterListItem)txtbSupplier.SelectedItem).ID;

                    win.dtpFromDate.SelectedDate = dtScrabsalesDate.SelectedDate;

                    win.OnSaveButton_Click += new RoutedEventHandler(Itemswin_OnSaveButton_Click); //win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);
                    win.Show();
                }
                else
                {
                    //Added By Somnath on 05/12/2011

                    if (((clsStoreVO)cboStore.SelectedItem).StoreId == 0 && ((MasterListItem)txtbSupplier.SelectedItem).ID != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                    else if (txtbSupplier.SelectedItem != null && ((MasterListItem)txtbSupplier.SelectedItem).ID == 0 && ((clsStoreVO)cboStore.SelectedItem).StoreId != 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Mandatory Items are not Selected.", "Please Select a Store and Supplier", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                }
            }
            catch (Exception Ex)
            {
                throw;
            }

        }

        void Itemswin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            #region Commented

            //ShowMsg = false;
            //msgText = "";
            //ItemListNew Itemswin = (ItemListNew)sender;
            //Flag = false;

            //List<clsSrcapDetailsVO> ScarpsItemsIndatagrid = new List<clsSrcapDetailsVO>();
            //ScarpsItemsIndatagrid = (List<clsSrcapDetailsVO>)ScarpsAddedItems.ToList<clsSrcapDetailsVO>();

            //if (Itemswin.ItemBatchList != null)
            //{
            //    foreach (var item in Itemswin.ItemBatchList)
            //    {
            //        Flag = false;
            //        if (dgScrapSalesItems.ItemsSource != null)
            //        {
            //            foreach (var dgitems in ScarpsItemsIndatagrid)
            //            {
            //                if (item.BatchID.Equals(dgitems.BatchID)) //i|| !string.IsNullOrEmpty(dgitems.BatchCode))
            //                {
            //                    Flag = true;
            //                    ShowMsg = true;
            //                    msgText = msgText + "   " + item.ItemName;
            //                    if (!string.IsNullOrEmpty(item.BatchCode))
            //                    {
            //                        msgText = msgText + "  Batch Code:: " + item.BatchCode;
            //                    }
            //                    if (string.IsNullOrEmpty(msgText))
            //                    {

            //                    }
            //                    else
            //                    {
            //                        msgText = msgText + "\r ";
            //                    }
            //                    break;
            //                }

            //            }
            //            if (Flag == false)
            //            {
            //                //ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = item.StockingCF, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID =item.SUM});
            //                ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = 1, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID = item.SUM });
            //            }
            //        }
            //        else
            //        {
            //            //ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = item.StockingCF, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID = item.SUM });
            //            ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = 1, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID = item.SUM });
            //        }
            //    }

            //    if (ShowMsg == true)
            //    {
            //        //msgText = msgText + "Item is Already Added! ";
            //        //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        ////msgWindow.Height = 
            //        //msgWindow.Show();

            //    }


            //    dgScrapSalesItems.ItemsSource = null;
            //    dgScrapSalesItems.ItemsSource = ScarpsAddedItems;
            //}

            ////dgScrapSalesItems.Focus();
            ////dgScrapSalesItems.UpdateLayout();

            ////dgScrapSalesItems.SelectedIndex = GRNAddedItems.Count - 1;


            ////TotalRate = 0;
            ////TotalTaxAmount = 0;
            ////NetAmount = 0;
            ////foreach (var item in ScarpsAddedItems)
            ////{
            ////    TotalRate = TotalRate + item.Rate;
            ////    TotalTaxAmount = TotalTaxAmount + item.VATAmount;
            ////    NetAmount = NetAmount + item.TotalAmount;


            ////}
            //////txtTotalRate.Text = TotalRate.ToString();
            ////txtTotalRate.Text = String.Format("{0:0.00}", TotalRate);
            //////txtTaxAmount.Text = TotalTaxAmount.ToString();
            ////txtTaxAmount.Text = String.Format("{0:0.00}", TotalTaxAmount);
            //////txtNetAmount.Text = NetAmount.ToString();
            ////txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);
            //CalculateSummary();

            #endregion

            ShowMsg = false;
            msgText = "";
            ExpiredItemSearch Itemswin = (ExpiredItemSearch)sender;   //ItemListNew Itemswin = (ItemListNew)sender;
            Flag = false;

            List<clsSrcapDetailsVO> ScarpsItemsIndatagrid = new List<clsSrcapDetailsVO>();
            ScarpsItemsIndatagrid = (List<clsSrcapDetailsVO>)ScarpsAddedItems.ToList<clsSrcapDetailsVO>();

            if (Itemswin.SelectedItems != null)   //if (Itemswin.ItemBatchList != null)
            {
                foreach (var item in Itemswin.SelectedItems)  //foreach (var item in Itemswin.ItemBatchList)
                {
                    Flag = false;
                    if (dgScrapSalesItems.ItemsSource != null)
                    {
                        foreach (var dgitems in ScarpsItemsIndatagrid)
                        {
                            if (item.ItemID.Equals(dgitems.ItemId) && item.BatchID.Equals(dgitems.BatchID)) //i|| !string.IsNullOrEmpty(dgitems.BatchCode))
                            {
                                Flag = true;

                                ShowMsg = true;
                                msgText = msgText + "   " + item.ItemName;
                                if (!string.IsNullOrEmpty(item.BatchCode))
                                {
                                    msgText = msgText + "  Batch Code:: " + item.BatchCode;
                                }
                                if (string.IsNullOrEmpty(msgText))
                                {

                                }
                                else
                                {
                                    msgText = msgText + "\r ";
                                }
                                break;
                            }

                        }

                        if (Flag == false)
                        {
                            //ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = item.StockingCF, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID =item.SUM});
                            ScarpsAddedItems.Add(new clsSrcapDetailsVO()
                            {
                                ItemId = item.ItemID,
                                ItemName = item.ItemName,
                                ItemCode = item.ItemCode,
                                AvailableStock = item.AvailableStock,
                                BatchCode = item.BatchCode,
                                BatchID = Convert.ToInt64(item.BatchID),
                                BatchExpiryDate = item.BatchExpiryDate,  //
                                UnitId = item.UnitID,
                                SaleQty = item.ReturnQty / item.StockCF, //1,
                                VATPerc = item.VATPercentage,  //VATPerc = item.VATPerc,
                                Conversion = item.Conversion,  //
                                TaxPercentage = item.TaxPercentage, //
                                ItemTax = item.TaxPercentage, //
                                StockCF = item.StockCF, //
                                ConversionFactor = 1,
                                Rate = item.PurchaseRate * item.StockCF,  //Rate = item.PurchaseRate,

                                BaseRate = Convert.ToSingle(item.PurchaseRate), //
                                BaseMRP = Convert.ToSingle(item.MRP), //
                                MainMRP = Convert.ToSingle(item.MRP), //
                                MainRate = Convert.ToSingle(item.PurchaseRate), //

                                SUOMID = item.StockUOMID, //SUOMID = item.SUM
                                StockUOM = item.StockUOM,
                                SUOM = item.StockUOM,  //SUOM = item.SUOM,
                                AvailableStockUOM = item.StockUOM, //
                                SelectedUOM = new MasterListItem(item.StockUOMID, item.StockUOM), //SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM),

                                BaseUOM = item.BaseUOM,  //BaseUOM = item.BaseUMString,
                                BaseUOMID = item.BaseUOMID,  //BaseUOMID = item.BaseUM,
                                BaseQuantity = (Single)(item.ReturnQty) * item.BaseCF, //(Single)(item.ReturnQty) * item.StockCF, //1 * item.StockCF,  //BaseQuantity = 1 * item.StockingCF,  
                                BaseConversionFactor = item.BaseCF,  //BaseConversionFactor = item.StockingToBaseCF,

                                AvailableStockInBase = Convert.ToSingle(item.AvailableStockInBase),  //AvailableStockInBase =  item.AvailableStockInBase,


                                #region For Quarantine Items (Expired, DOS)

                                // Use For Vat/Tax Calculations

                                OtherGRNItemTaxApplicationOn = item.OtherGRNItemTaxApplicationOn,
                                OtherGRNItemTaxType = item.OtherGRNItemTaxType,
                                GRNItemVatApplicationOn = item.GRNItemVatApplicationOn,
                                GRNItemVatType = item.GRNItemVatType,

                                InclusiveOfTax = item.InclusiveOfTax

                                #endregion

                            }
                            );
                        }
                    }
                    else
                    {
                        ////ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = item.StockingCF, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID = item.SUM });
                        //ScarpsAddedItems.Add(new clsSrcapDetailsVO() { Rate = item.PurchaseRate, SaleQty = 1, VATPerc = item.VATPerc, BatchID = item.BatchID, ItemCode = item.ItemCode, ItemId = item.ItemID, ItemName = item.ItemName, BatchCode = item.BatchCode, SUOM = item.SUOM, AvailableStock = item.AvailableStock, AvailableStockInBase = item.AvailableStockInBase, SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM), BaseConversionFactor = item.StockingToBaseCF, ConversionFactor = 1, BaseQuantity = 1 * item.StockingCF, BaseUOM = item.BaseUMString, BaseUOMID = item.BaseUM, SUOMID = item.SUM });

                        ScarpsAddedItems.Add(new clsSrcapDetailsVO()
                        {
                            ItemId = item.ItemID,
                            ItemName = item.ItemName,
                            ItemCode = item.ItemCode,
                            AvailableStock = item.AvailableStock,
                            BatchCode = item.BatchCode,
                            BatchID = Convert.ToInt64(item.BatchID),
                            BatchExpiryDate = item.BatchExpiryDate,  //
                            UnitId = item.UnitID,
                            SaleQty = item.ReturnQty / item.StockCF, //1,
                            VATPerc = item.VATPercentage,  //VATPerc = item.VATPerc,
                            Conversion = item.Conversion,  //
                            TaxPercentage = item.TaxPercentage, //
                            ItemTax = item.TaxPercentage, //
                            StockCF = item.StockCF, //
                            ConversionFactor = 1,
                            Rate = item.PurchaseRate * item.StockCF,  //Rate = item.PurchaseRate,

                            BaseRate = Convert.ToSingle(item.PurchaseRate), //
                            BaseMRP = Convert.ToSingle(item.MRP), //
                            MainMRP = Convert.ToSingle(item.MRP), //
                            MainRate = Convert.ToSingle(item.PurchaseRate), //

                            SUOMID = item.StockUOMID, //SUOMID = item.SUM
                            StockUOM = item.StockUOM,
                            SUOM = item.StockUOM,  //SUOM = item.SUOM,
                            AvailableStockUOM = item.StockUOM, //
                            SelectedUOM = new MasterListItem(item.StockUOMID, item.StockUOM), //SelectedUOM = new MasterListItem(item.SUOMID, item.SUOM),

                            BaseUOM = item.BaseUOM,  //BaseUOM = item.BaseUMString,
                            BaseUOMID = item.BaseUOMID,  //BaseUOMID = item.BaseUM,
                            BaseQuantity = (Single)(item.ReturnQty) * item.BaseCF, //(Single)(item.ReturnQty) * item.StockCF, //1 * item.StockCF,  //BaseQuantity = 1 * item.StockingCF,  
                            BaseConversionFactor = item.BaseCF,  //BaseConversionFactor = item.StockingToBaseCF,

                            AvailableStockInBase = Convert.ToSingle(item.AvailableStockInBase),  //AvailableStockInBase =  item.AvailableStockInBase,


                            #region For Quarantine Items (Expired, DOS)

                            // Use For Vat/Tax Calculations

                            OtherGRNItemTaxApplicationOn = item.OtherGRNItemTaxApplicationOn,
                            OtherGRNItemTaxType = item.OtherGRNItemTaxType,
                            GRNItemVatApplicationOn = item.GRNItemVatApplicationOn,
                            GRNItemVatType = item.GRNItemVatType,

                            InclusiveOfTax = item.InclusiveOfTax

                            #endregion

                        }
                            );

                    }
                }

                if (ShowMsg == true)
                {
                    //msgText = msgText + "Item is Already Added! ";
                    //MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    ////msgWindow.Height = 
                    //msgWindow.Show();

                }


                dgScrapSalesItems.ItemsSource = null;
                dgScrapSalesItems.ItemsSource = ScarpsAddedItems;
            }

            //dgScrapSalesItems.Focus();
            //dgScrapSalesItems.UpdateLayout();

            //dgScrapSalesItems.SelectedIndex = GRNAddedItems.Count - 1;


            //TotalRate = 0;
            //TotalTaxAmount = 0;
            //NetAmount = 0;
            //foreach (var item in ScarpsAddedItems)
            //{
            //    TotalRate = TotalRate + item.Rate;
            //    TotalTaxAmount = TotalTaxAmount + item.VATAmount;
            //    NetAmount = NetAmount + item.TotalAmount;


            //}
            ////txtTotalRate.Text = TotalRate.ToString();
            //txtTotalRate.Text = String.Format("{0:0.00}", TotalRate);
            ////txtTaxAmount.Text = TotalTaxAmount.ToString();
            //txtTaxAmount.Text = String.Format("{0:0.00}", TotalTaxAmount);
            ////txtNetAmount.Text = NetAmount.ToString();
            //txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);
            CalculateSummary();

        }

        private void CalculateSummary()
        {
            TotalRate = 0;
            TotalTaxAmount = 0;
            NetAmount = 0;
            foreach (var item in ScarpsAddedItems)
            {
                TotalRate = TotalRate + item.Rate;
                TotalTaxAmount = TotalTaxAmount + item.VATAmount;
                NetAmount = NetAmount + item.TotalAmount;


            }
            //txtTotalRate.Text = TotalRate.ToString();
            txtTotalRate.Text = String.Format("{0:0.00}", TotalRate);
            //txtTaxAmount.Text = TotalTaxAmount.ToString();
            txtTaxAmount.Text = String.Format("{0:0.00}", TotalTaxAmount);
            //txtNetAmount.Text = NetAmount.ToString();
            txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);
        }

        #endregion ButtonClickEvent

        #region Selection Changed Event
        private void dgScrapSalesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgScrapSalesItemsList.ItemsSource = null;
            if (dgScrapSalesList.SelectedItem != null)
            {

                clsSrcapVO Item = ((clsSrcapVO)dgScrapSalesList.SelectedItem).DeepCopy<clsSrcapVO>();
                ScrapId = Item.ScrapID;
                FillScrapItemdatagrid();
            }
        }

        #endregion Selection Changed Event

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgScrapSalesList.SelectedItem != null)
            {
                long ID = 0;
                long UnitID = 0;
                ID = ((clsSrcapVO)dgScrapSalesList.SelectedItem).ScrapID;
                UnitID = ((clsSrcapVO)dgScrapSalesList.SelectedItem).UnitId;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/InventoryPharmacy/ScrapPrint.aspx?ID=" + ((clsSrcapVO)dgScrapSalesList.SelectedItem).ScrapID), "_blank");

                string URL = "../Reports/InventoryPharmacy/ScrapPrint.aspx?ID=" + ID + "&UnitID=" + UnitID;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void txtbSupplier_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)e.OriginalSource).Text))
            {
                ((TextBox)e.OriginalSource).ClearValidationError();
            }
        }

        private void dtScrabsalesDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((System.Windows.Controls.TextBox)e.OriginalSource).Text))
            {
                dtScrabsalesDate.ClearValidationError();
            }
        }

        private void cmdDeleteScrapSalesItems_Click(object sender, RoutedEventArgs e)
        {
            if (dgScrapSalesItems.SelectedItem != null)
            {
                string msgTitle = "";
                string msgText = "Are you sure you want to Delete the selected Item ?";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        ScarpsAddedItems.RemoveAt(dgScrapSalesItems.SelectedIndex);

                        CalculateSummary();

                        //TotalRate = 0;
                        //TotalTaxAmount = 0;
                        //NetAmount = 0;
                        //foreach (clsSrcapDetailsVO item in dgScrapSalesItems.ItemsSource)
                        //{
                        //    TotalRate = TotalRate + item.Rate;
                        //    TotalTaxAmount = TotalTaxAmount + item.VATAmount;
                        //    NetAmount = NetAmount + item.TotalAmount;


                        //}
                        ////txtTotalRate.Text = TotalRate.ToString();
                        //txtTotalRate.Text = String.Format("{0:0.00}", TotalRate);
                        ////txtTaxAmount.Text = TotalTaxAmount.ToString();
                        //txtTaxAmount.Text = String.Format("{0:0.00}", TotalTaxAmount);
                        ////txtNetAmount.Text = NetAmount.ToString();
                        //txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);

                    }
                };

                msgWD.Show();
            }
        }

        private void cmdGetIndent_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cboStore_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cboStore.SelectedItem != null)
                {
                    if (ScarpsAddedItems != null)
                        ScarpsAddedItems.Clear();
                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            if (dgScrapSalesItems.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).ItemId, ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SelectedUOM.ID);
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

            ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SUOMID);



        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgScrapSalesItems.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty);
                    long BaseUOMID = ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = objConversionVO.SingleQuantity;
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;



                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;


                    if (((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseQuantity > ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase)
                    {
                        float availQty = ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase;

                        ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).AvailableStockInBase / objConversionVO.BaseConversionFactor)));
                        string msgText = "Quantity Must Be Less Than Or Equal To Available Quantity ";
                        ConversionsForAvailableStock();
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                    CalculateSummary();


                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }

        private void ConversionsForAvailableStock()
        {
            ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseQuantity = Convert.ToSingle(((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).SaleQty) * ((clsSrcapDetailsVO)dgScrapSalesItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseMRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            CalculateSummary();
        }

        private void CmdChangeAndApprove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgScrapSalesList.SelectedItem != null)
                {
                    if (((clsSrcapVO)dgScrapSalesList.SelectedItem).Approve)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "DOS Item Return is already approved .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else if (((clsSrcapVO)dgScrapSalesList.SelectedItem).Reject)
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "DOS Item Return is already Rejected .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        //if (((clsExpiredItemReturnVO)dgExpiredList.SelectedItem).Freezed)
                        //{
                        MessageBoxControl.MessageBoxChildWindow mgBox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Do you want to Change and Approve DOS Item Return ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        mgBox.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                //clsAddGRNBizActionVO BizAction = new clsAddGRNBizActionVO();

                                ScrapSale ObjDOS = new ScrapSale();

                                ObjDOS.IsApproved = true;
                                ObjDOS.ISFromAproveDOS = true;
                                ObjDOS.SelectedDOS = (clsSrcapVO)dgScrapSalesList.SelectedItem;

                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                mElement.Text = "Approve DOS Item Return";
                                ((IApplicationConfiguration)App.Current).OpenMainContent(ObjDOS);
                            }
                        };
                        mgBox.Show();
                        //}
                        //else
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "Expired Item Return is not Freezed \n Please Freeze Expired Item Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    mgbox.Show();
                        //}
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbox = new MessageBoxControl.MessageBoxChildWindow("Palash", "No DOS Item Return is Selected \n Please select a DOS Item Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    mgbox.Show();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).OpenMainContent(new InventoryDashBoard());
        }


    }
}
