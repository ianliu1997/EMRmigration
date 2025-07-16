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
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Windows.Browser;
using OPDModule.Forms;
using PalashDynamics.Pharmacy.Inventory;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Log;
using System.Reflection;

namespace PalashDynamics.Pharmacy
{
    public partial class ItemSalesReturn : UserControl, INotifyPropertyChanged
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

        private SwivelAnimation objAnimation = null;

        string msgTitle = "PALASHDYNAMICS";
        string msgText = "";
        double TotalAmount = 0;
        double NetAmount = 0;
        double NetReturnAmount = 0;
        double TotalVATAmount = 0;
        double TotalConsessionAmount = 0;
        double TotalSGSTAmt = 0;
        double TotalCGSTAmt = 0;
        Int64 ItemSaleId = 0;
        DateTime? FromDate;
        DateTime? ToDate;
        bool Flag = false;
        public PagedSortableCollectionView<clsItemSalesReturnVO> ItemSalesReturnList { get; private set; }
        List<clsItemSalesReturnDetailsVO> ItemSalesReturnDetailsList;
        List<clsItemSalesDetailsVO> ItemSalesDetailsList;
        public ObservableCollection<clsItemSalesDetailsVO> SelectedSalesDetailsItem { get; set; }
        public clsItemSalesVO SelectedItem { get; set; }

        public double PendingQty { get; set; }

        public long BillID { get; set; }
        public long ItemSaleReturnID { get; set; }

        //added by rohinee dated 26/9/2016 for audit trail
        bool IsAuditTrail = false;
        int lineNumber = 0;
        List<LogInfo> LogInfoList = new List<LogInfo>();  // For the Activity Log List
        LogInfo LogInformation;
        Guid objGUID;
        #endregion Variables

        #region Properties
        public ObservableCollection<clsItemSalesReturnDetailsVO> AddItemSalesReturnDetails { get; set; }
        //public ObservableCollection<clsItemSalesDetailsVO> AddItemSalesDetails{get;set;}

        #endregion

        #region Paging Properties
        public int PageSizeSalesReturnList
        {
            get
            {
                return ItemSalesReturnList.PageSize;
            }
            set
            {
                if (value == ItemSalesReturnList.PageSize) return;
                ItemSalesReturnList.PageSize = value;
                OnPropertyChanged("PageSizeSalesReturnList");
            }
        }

        #endregion

        #region Validation

        public Boolean CheckValidationForSave()
        {
            if (dtItemReturnSalesDate.SelectedDate == null)
            {
                dtItemReturnSalesDate.SetValidation("Please Enter Item Sales Return Date");
                dtItemReturnSalesDate.RaiseValidationError();
                dtItemReturnSalesDate.Focus();
                return false;
            }
            else if (AddItemSalesReturnDetails.Count == 0)
            {
                msgText = "Item Sales Return Grid Cannot be Blank!";
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgWindow.Show();
                dgItemSalesReturnDetials.Focus();
                return false;
            }
            List<clsItemSalesDetailsVO> objList = SelectedSalesDetailsItem.ToList<clsItemSalesDetailsVO>();
            if (objList != null && objList.Count > 0)
            {
                foreach (var item in objList)
                {
                    if (item.Quantity == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgWindow.Show();
                        dgItemSalesReturnDetials.Focus();
                        return false;
                    }

                }
                return true;
            }
            else
            {
                dtItemReturnSalesDate.ClearValidationError();

                return true;
            }

        }

        public Boolean CheckValidationForSearch()
        {
            if (dtpFromDate.SelectedDate == null && dtpToDate.SelectedDate != null)
            {
                dtpFromDate.SetValidation("Please Enter From Date");
                dtpFromDate.RaiseValidationError();
                dtpFromDate.Focus();
                return false;
            }
            else if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Enter To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                return false;
            }
            else if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
            {
                dtpToDate.SetValidation("From Date Must be Less than To Date");
                dtpToDate.RaiseValidationError();
                dtpToDate.Focus();
                return false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
                dtpToDate.ClearValidationError();
                return true;
            }
        }

        #endregion Validation

        #region Constructor
        public ItemSalesReturn()
        {
            InitializeComponent();
            objAnimation = new PalashDynamics.Animations.SwivelAnimation(LayoutRoot1, LayoutRoot2, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            this.Loaded += new RoutedEventHandler(ItemSalesReturn_Loaded);
            dtItemReturnSalesDate.SelectedDate = System.DateTime.Now;
            dtpFromDate.SelectedDate = DateTime.Now.Date.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date.Date;
            this.SelectedItem = new clsItemSalesVO();
            this.SelectedSalesDetailsItem = new ObservableCollection<clsItemSalesDetailsVO>();
            SetCommandButtonState("New");
            ItemSalesReturnList = new PagedSortableCollectionView<clsItemSalesReturnVO>();
            ItemSalesReturnList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesReturnList_OnRefresh);
            PageSizeSalesReturnList = 5;
            this.dataGrid2Pager.DataContext = ItemSalesReturnList;
            this.dgItemSalesReturnList.DataContext = ItemSalesReturnList;
            setItemSalesReturnGrid();
            // Reset the Activity Log List added by rohinee dated 26/9/2016
            LogInfoList = new List<LogInfo>();
            IsAuditTrail = ((IApplicationConfiguration)App.Current).CurrentUser.IsAuditTrail;

        }

        void ItemSalesReturnList_OnRefresh(object sender, RefreshEventArgs e)
        {
            setItemSalesReturnGrid();
        }
        #endregion Constructor

        #region Load Event
        void ItemSalesReturn_Loaded(object sender, RoutedEventArgs e)
        {
            AddItemSalesReturnDetails = new ObservableCollection<clsItemSalesReturnDetailsVO>();
            ItemSalesReturnDetailsList = new List<clsItemSalesReturnDetailsVO>();
            ItemSalesDetailsList = new List<clsItemSalesDetailsVO>();

            dgItemSalesReturnDetials.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgItemSalesReturnDetials_CellEditEnded);

        }

        double OrgvatPer = 0;
        void dgItemSalesReturnDetials_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgItemSalesReturnDetials.SelectedItem != null)
            {
                clsItemSalesReturnDetailsVO SalesDetails = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).DeepCopy<clsItemSalesReturnDetailsVO>();

                //if (SalesDetails.BaseQuantity > SalesDetails.PendingQuantity)
                //{
                //    msgText = "Item's Return Quantity is Greater than Item's Sales Pending Quantity";
                //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                //    msgWindow.Show();
                //    SalesDetails.ReturnQuantity = (SalesDetails.BaseQuantity / SalesDetails.ConversionFactor);
                //    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = (SalesDetails.BaseQuantity / SalesDetails.ConversionFactor);

                //}
                //else
                //{



                if (e.Column.DisplayIndex == 6)
                {

                    if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMConversionList == null || ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMConversionList.Count == 0)
                    {
                        //DataGridColumn column = dgItemSalesReturnDetials.Columns[7];
                        //FrameworkElement fe = column.GetCellContent(e.Row);
                        //if (fe != null)
                        //{
                        //    FillUOMConversions();
                        //}

                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity = Convert.ToSingle(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity) * ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor;

                        if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity <= 0)
                        {

                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = 1;
                            string msgText = "Quantity Cannot be less then or equal to zero ";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                        else
                            if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity > ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity)
                            {
                                float availQty = Convert.ToSingle(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity);

                                ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = Convert.ToSingle(Math.Floor(Convert.ToDouble(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity / ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor)));
                                string msgText = "Quantity Must Be Less Than Or Equal To Pending Quantity ";
                                ConversionsForAvailableStock();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                            }
                            else if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM != null && ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID == ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOMID && (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity % 1) != 0)
                            {
                                ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = 1;
                                string msgText = "Quantity Cannot be in fraction";
                                ConversionsForAvailableStock();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();

                            }
                    }
                    else if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM != null && ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID > 0)
                    {

                        //CalculateConversionFactorCentral(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID, ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleUOMID);  // Commented by Ashish Z. on Dated 16-09-2016
                        CalculateConversionFactorCentral(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID, ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SUOMID);  // Added by Ashish Z. on Dated 16-09-2016

                    }
                    else
                    {
                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).Quantity = 0;
                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = 0;

                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConversionFactor = 0;
                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor = 0;

                    }
                    CalculateSummary();
                    #region by Rohinee For AuditTrail
                    if (IsAuditTrail == true && ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem != null))
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = "102 : Sales Item Quantity Changed : " //+ Convert.ToString(lineNumber)
                              + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                              + " , Amount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).Amount) + " "
                            //        + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor) + " "
                            //        + " , BaseMRP : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseMRP) + " "
                              + " , BaseQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity) + " "
                            //        + " , BaseRate : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseRate) + " "
                            //        + " , BaseSaleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseSaleQuantity) + " "
                            //         + " , BaseUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOM) + " "
                            //       + " , BaseUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOMID) + " "
                              + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConcessionAmount) + " "
                            //        + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConcessionPercentage) + " "
                            //        + " , ConversionFactor : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConversionFactor) + " "
                              + " , ItemCode : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemCode) + " "
                              + " , ItemID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemID) + " "
                              + " , ItemName : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemName) + " "
                            //        + " , ItemSaleId : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemSaleId) + " "
                            //         + " , MRP : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).MRP) + " "
                              + " , NetAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).NetAmount) + " "
                              + " , NetAmtCalculation : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).NetAmtCalculation) + " "
                              + " , PendingQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity) + " "
                              + " , Quantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).Quantity) + " "
                              + " , ReturnQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity) + " "
                            //         + " , SaleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleQuantity) + " "
                            //          + " , SaleUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleUOM) + " "
                            //          + " , SaleUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleUOMID) + " "
                            //          + " , SellingUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SellingUOM) + " "
                            //           + " , SellingUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SellingUOMID) + " "
                            //           + " , SelectedUOM ID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID) + " "
                            //          + " , SelectedUOM Name : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.Name) + " "
                              + " , SingleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SingleQuantity) + " "
                            //         + " , StockCF : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).StockCF) + " "
                            //         + " , SUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SUOMID) + " "
                              + " , VATAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATAmount) + " "
                            //         + " , VATPercent : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent) + " "
                              + " , Net Amount : " + Convert.ToString(txtNetAmount.Text) + " "
                              + " , Total Amount : " + Convert.ToString(txtTotalAmount.Text) + " "
                              + " , Total Return Amount : " + Convert.ToString(txtTotalReturnAmount.Text) + " "
                              + " , Total Concession Amount : " + Convert.ToString(txtTotalConcessionAmount.Text) + " "
                              + " , Round Off Amount : " + Convert.ToString(txtRountOffAmount.Text) + " "
                              + " , Total VAT Amount : " + Convert.ToString(txtTotalVATAmount.Text) + " "
                              ;
                        LogInfoList.Add(LogInformation);
                    }
                    #endregion
                }

                if (e.Column.DisplayIndex == 11)
                {
                    if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConcessionPercentage == 100)
                    {
                        if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent != 0)
                        {
                            OrgvatPer = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent;
                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent = 0;
                        }
                    }
                    else
                    {
                        if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent == 0)
                        {
                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent = OrgvatPer;
                        }
                    }
                    CalculateSummary();
                }

                if (e.Column.DisplayIndex == 14)
                {
                    if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent > 100)
                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent = 0;

                    CalculateSummary();
                }
                //  }                


            }
        }


        private void CalculateSummary()
        {
            TotalAmount = 0;
            NetAmount = 0;
            NetReturnAmount = 0;
            TotalVATAmount = 0;
            TotalConsessionAmount = 0;
            TotalCGSTAmt = 0;
            TotalSGSTAmt = 0;
            foreach (var item in AddItemSalesReturnDetails)
            {
                TotalAmount = item.Amount + TotalAmount;
                TotalConsessionAmount = TotalConsessionAmount + item.ConcessionAmount;
                TotalVATAmount = TotalVATAmount + (item.VATAmount);
                TotalSGSTAmt = TotalSGSTAmt + item.SGSTAmount;
                TotalCGSTAmt = TotalCGSTAmt + item.CGSTAmount;
                NetAmount = NetAmount + item.NetAmount;
                NetReturnAmount = NetReturnAmount + (item.TotalSalesAmount - item.NetAmount);



            }
            //Added By Bhushanp 
            txtTotalSGSTAmount.Text = String.Format("{0:0.00}", TotalSGSTAmt);
            txtTotalCGSTAmount.Text = String.Format("{0:0.00}", TotalCGSTAmt);

            //txtNetAmount.Text = NetAmount.ToString();
            txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);

            //txtTotalAmount.Text = TotalAmount.ToString();
            txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);

            //txtTotalReturnAmount.Text = NetReturnAmount.ToString();
            txtTotalReturnAmount.Text = String.Format("{0:0.00}", NetReturnAmount);

            //txtTotalConcessionAmount.Text = TotalConsessionAmount.ToString();
            txtTotalConcessionAmount.Text = String.Format("{0:0.00}", TotalConsessionAmount);

            //By Anjali............................
            txtRountOffAmount.Text = Math.Round(NetAmount).ToString();
            roundAmt = Math.Round(NetAmount);
            //.....................................

            // txtTotalVATAmount.Text = TotalVATAmount.ToString();
            txtTotalVATAmount.Text = String.Format("{0:0.00}", TotalVATAmount);
        }

        #endregion Load Event

        #region Public Methods

        public void setItemSalesReturnGrid()
        {
            clsGetItemSalesReturnBizActionVO BizAction = new clsGetItemSalesReturnBizActionVO();
            if (dtpFromDate.SelectedDate != null)

                BizAction.FromDate = dtpFromDate.SelectedDate;
            else
                BizAction.FromDate = null;

            if (dtpToDate.SelectedDate != null)

                BizAction.ToDate = dtpToDate.SelectedDate.Value.AddDays(1);
            else
                BizAction.ToDate = null;

            //change by harish 17 apr
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.SerachExpression = "0";
            }
            else
            {
                BizAction.SerachExpression = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId.ToString();
            }
            BizAction.PagingEnabled = true;
            BizAction.MaximumRows = ItemSalesReturnList.PageSize;
            BizAction.StartRowIndex = ItemSalesReturnList.PageIndex * ItemSalesReturnList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.Details = new List<clsItemSalesReturnVO>();
                    BizAction.Details = ((clsGetItemSalesReturnBizActionVO)args.Result).Details;

                    ItemSalesReturnList.Clear();
                    ItemSalesReturnList.TotalItemCount = (int)(((clsGetItemSalesReturnBizActionVO)args.Result).TotalRows);
                    ///Setup Page Fill DataGrid

                    foreach (clsItemSalesReturnVO item in BizAction.Details)
                    {
                        ItemSalesReturnList.Add(item);
                    }

                    dgItemSalesReturnList.ItemsSource = null;
                    dgItemSalesReturnList.ItemsSource = ItemSalesReturnList;
                    txtTotalCountRecords.Text = ItemSalesReturnList.TotalItemCount.ToString();

                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        public void setItemSalesReturnDeatilsGrid(long ItemSalesReturnID, long iUnitID)
        {
            clsGetItemSalesReturnDetailsBizActionVO BizAction = new clsGetItemSalesReturnDetailsBizActionVO();
            BizAction.ItemSalesReturnID = ItemSalesReturnID;
            BizAction.UnitID = iUnitID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.Details = new List<clsItemSalesReturnDetailsVO>();
                    BizAction.Details = ((clsGetItemSalesReturnDetailsBizActionVO)args.Result).Details;


                    dgItemSalesReturnDetalsList.ItemsSource = null;
                    dgItemSalesReturnDetalsList.ItemsSource = BizAction.Details;


                }

            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }


        //public void setItemSalesGrid()
        //{
        //    clsGetItemSalesBizActionVO BizAction = new clsGetItemSalesBizActionVO();
        //    BizAction.FromDate = FromDate;
        //    BizAction.ToDate = ToDate;


        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            BizAction.Details = new List<clsItemSalesVO>();
        //            BizAction.Details = ((clsGetItemSalesBizActionVO)args.Result).Details;


        //            dgItemSales.ItemsSource = null;
        //            dgItemSales.ItemsSource = BizAction.Details;


        //        }

        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        //public void setItemSalesDetailsGrid()
        //{
        //    clsGetItemSalesDetailsBizActionVO BizAction = new clsGetItemSalesDetailsBizActionVO();
        //    BizAction.ItemSalesID = ItemSaleId;
        //    ItemSalesDetailsList = new List<clsItemSalesDetailsVO>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (s, args) =>
        //    {

        //        if (args.Error == null && args.Result != null)
        //        {

        //            BizAction.Details = new List<clsItemSalesDetailsVO>();
        //            BizAction.Details = ((clsGetItemSalesDetailsBizActionVO)args.Result).Details;


        //            dgItemSalesDetails.ItemsSource = null;
        //            ItemSalesDetailsList = BizAction.Details;
        //            dgItemSalesDetails.ItemsSource = BizAction.Details;


        //        }

        //    };

        //    client.ProcessAsync(BizAction, new clsUserVO());
        //    client.CloseAsync();
        //}

        public void ClearUI()
        {
            txtNetAmount.Text = "0.00";
            txtRemarks.Text = "";
            txtTotalAmount.Text = "0.00";
            //By Anjali...................
            txtRountOffAmount.Text = "";
            //..........................
            txtTotalConcessionAmount.Text = "0.00";
            txtTotalReturnAmount.Text = "0.00";
            txtTotalVATAmount.Text = "0.00";
            txtTotalSGSTAmount.Text = "0.00";
            txtTotalCGSTAmount.Text = "0.00";

            dtItemReturnSalesDate.SelectedDate = System.DateTime.Now;
            dtpFromDate.SelectedDate = System.DateTime.Now;
            dtpToDate.SelectedDate = System.DateTime.Now;
            AddItemSalesReturnDetails = new ObservableCollection<clsItemSalesReturnDetailsVO>();
            dgItemSalesReturnDetials.ItemsSource = null;

        }

        #endregion Public Methods

        #region Set Command Button State New/Save/Cancel
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "Save":

                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = false;
                    break;

                case "Modify":

                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "ClickNew":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;

                    break;

                default:
                    break;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do nothing
                }
                else
                {
                    cmdSave.IsEnabled = false;
                }
            }
        }
        #endregion

        #region Click Event

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Forward);
            SetCommandButtonState("ClickNew");
            ClearUI();
        }

        #region Added by Priyanka
        int ClickedFlag1 = 0;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            //* Added by - Ajit Jadhav
            //* Added Date - 8/9/2016 //13/9/2016
            //* Comments - Check Item Sales Details List no empty // Check  validation 
            ItemSalesReturnDetailsList = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());
            //***//---------
            if (dgItemSalesReturnDetials.ItemsSource != null && ItemSalesReturnDetailsList.Count() > 0)
            {
                ClickedFlag1 = ClickedFlag1 + 1;
                bool isValid = true;
                //By Anjali.......
                foreach (var item in AddItemSalesReturnDetails.ToList())
                {
                    if (item.SelectedUOM.ID == 0 || item.SelectedUOM.ID == null)
                    {
                        isValid = false;
                        string msgText = "Please Select UOM For Item" + item.ItemName;

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        ClickedFlag1 = 0;
                        break;
                    }
                    //***//------------------
                    if (item.ReturnQuantity <= 0)
                    {
                        isValid = false;
                        item.ReturnQuantity = 0;
                        string msgText = "Quantity Cannot be less then or equal to zero ";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        ClickedFlag1 = 0;
                        break;
                    }
                    else if (item.BaseQuantity > item.PendingQuantity)
                    {
                        isValid = false;
                        float availQty = Convert.ToSingle(item.PendingQuantity);
                        item.ReturnQuantity = Math.Floor(Convert.ToDouble(item.PendingQuantity / item.BaseConversionFactor));
                        string msgText = "Quantity Must Be Less Than Or Equal To Pending Quantity ";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        ClickedFlag1 = 0;
                        break;
                    }

                    else if (item.SelectedUOM != null && (item.SelectedUOM.ID == item.BaseUOMID && (item.ReturnQuantity % 1) != 0))
                    {
                        isValid = false;
                        item.ReturnQuantity = 0;
                        string msgText = "Quantity Cannot be in fraction";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        ClickedFlag1 = 0;
                        break;
                    }
                    //***//-----------------------------------
                }


                if (ClickedFlag1 == 1 && resultround == true && isValid)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Exchange the Material ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.No)
                        {
                            cmdSave.IsEnabled = false;     //Added by Ashish Z. on 240816
                            PaymentDetails paymentWin = new PaymentDetails();
                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.OnCancelButton_Click += new RoutedEventHandler(paymentWin_CancelButton_Click);
                            // paymentWin.txtPaidAmount.Text = txtNetAmount.Text;

                            // paymentWin.txtPaidAmount.Text = Convert.ToString(Math.Round(double.Parse(txtNetAmount.Text), 0));
                            paymentWin.txtPaidAmount.Text = Convert.ToString(Math.Round(double.Parse(txtRountOffAmount.Text), 0));

                            // paymentWin.txtPaidAmount.Text = Convert.ToString(double.Parse(txtNetAmount.Text) - double.Parse(txtTotalVATAmount.Text));
                            paymentWin.Show();
                        }
                        else if (res == MessageBoxResult.Yes)
                        {
                            SaveExchangeMaterial();
                        }
                    };
                    msgWD.Show();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select The Items For Return", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWindow.Show();
                ClickedFlag1 = 0;
            }
        }

        void paymentWin_CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1 = 0;
            cmdSave.IsEnabled = true;     //Added by Ashish Z. on 240816
        }

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidationForSave())
            {
                cmdSave.IsEnabled = false;     //Added by Ashish Z. on 240816
                clsRefundVO RefundObj = new clsRefundVO();

                if (((PaymentDetails)sender).Payment != null)
                {
                    RefundObj.PaymentDetails = ((PaymentDetails)sender).Payment;
                }

                RefundObj.Date = DateTime.Now;


                //RefundObj.BillID = BillID;
                RefundObj.ItemSaleReturnID = ItemSaleReturnID;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    RefundObj.Amount = Convert.ToDouble(((PaymentDetails)sender).txtPaidAmount.Text); //RefundObj.Amount = double.Parse(txtNetAmount.Text);

                clsAddItemSalesReturnBizActionVO bizactionVO = new clsAddItemSalesReturnBizActionVO();
                bizactionVO.ItemMatserDetail = new clsItemSalesReturnVO();
                clsItemSalesReturnVO addNewItemSalesReturn = new clsItemSalesReturnVO();

                try
                {

                    bizactionVO.ItemsDetail = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());

                    addNewItemSalesReturn.Date = dtItemReturnSalesDate.SelectedDate.Value;
                    addNewItemSalesReturn.Time = System.DateTime.Now;
                    addNewItemSalesReturn.ItemSalesID = this.SelectedItem.ID;
                    addNewItemSalesReturn.StoreID = this.SelectedItem.StoreID;
                    addNewItemSalesReturn.Remarks = txtRemarks.Text;
                    addNewItemSalesReturn.ConcessionAmount = Convert.ToDouble(txtTotalConcessionAmount.Text);
                    addNewItemSalesReturn.VATAmount = Convert.ToDouble(txtTotalVATAmount.Text);
                    addNewItemSalesReturn.TotalAmount = Convert.ToDouble(txtTotalAmount.Text);
                    //Added By Bhushanp For GST 14072017
                    addNewItemSalesReturn.TotalSGST = Convert.ToDouble(txtTotalSGSTAmount.Text);
                    addNewItemSalesReturn.TotalCGST = Convert.ToDouble(txtTotalCGSTAmount.Text);
                    //By Anjali..........................
                    // addNewItemSalesReturn.NetAmount = Convert.ToDouble(txtNetAmount.Text);
                    addNewItemSalesReturn.NetAmount = Convert.ToDouble(txtRountOffAmount.Text);
                    addNewItemSalesReturn.CalculatedNetAmount = Convert.ToDouble(txtNetAmount.Text);
                    //.....................................
                    addNewItemSalesReturn.TotalReturnAmount = Convert.ToDouble(txtTotalReturnAmount.Text);
                    addNewItemSalesReturn.Status = true;

                    //By Anjali....................
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                        addNewItemSalesReturn.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    else
                        addNewItemSalesReturn.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                    //.............................

                    bizactionVO.ItemMatserDetail = addNewItemSalesReturn;
                    //added by rohinee dated 26/9/2016 for audit trail===================================================================================================
                    if (IsAuditTrail)
                    {

                        if (bizactionVO.ItemMatserDetail != null)   //item sales return details                        {
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 104 : Payment Details : " //+ Convert.ToString(lineNumber)
                                                                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                                    + " , BankID : " + Convert.ToString(((clsPaymentDetailsVO)RefundObj.PaymentDetails.PaymentDetails[0]).BankID) + " "
                                                                    + " , PaymentModeID : " + Convert.ToString(((clsPaymentDetailsVO)RefundObj.PaymentDetails.PaymentDetails[0]).PaymentModeID) + " "
                                                                    + " , PaidAmount : " + Convert.ToString(((clsPaymentDetailsVO)RefundObj.PaymentDetails.PaymentDetails[0]).PaidAmount) + " "
                                                                    + " , Number : " + Convert.ToString(((clsPaymentDetailsVO)RefundObj.PaymentDetails.PaymentDetails[0]).Number) + " "
                                                                    + " , Date : " + Convert.ToString(((clsPaymentDetailsVO)RefundObj.PaymentDetails.PaymentDetails[0]).Date) + " "
                                                                    ;


                            LogInfoList.Add(LogInformation);
                        }
                        if (addNewItemSalesReturn != null)   //item sales return details                        {
                        {
                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 105 :T_ItemSaleReturn : " //+ Convert.ToString(lineNumber)
                                                + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                                                + "Date : " + Convert.ToString(addNewItemSalesReturn.Date) + " "
                                                + " , Time : " + Convert.ToString(addNewItemSalesReturn.Time) + " "
                                                + " , Item Sales ID : " + Convert.ToString(addNewItemSalesReturn.ItemSalesID) + " "
                                                + " , Store ID : " + Convert.ToString(addNewItemSalesReturn.StoreID) + " "
                                                + " , Remarks : " + Convert.ToString(addNewItemSalesReturn.Remarks) + " "
                                                + " , Concession Amount : " + Convert.ToString(addNewItemSalesReturn.ConcessionAmount) + " "
                                                + " , VAT Amount: " + Convert.ToString(addNewItemSalesReturn.VATAmount) + " "
                                                + " , Total Amount : " + Convert.ToString(addNewItemSalesReturn.TotalAmount) + " "
                                                + " , Net Amount : " + Convert.ToString(addNewItemSalesReturn.NetAmount) + " "
                                                + " , Calculated Net Amount : " + Convert.ToString(addNewItemSalesReturn.CalculatedNetAmount) + " "
                                                + " , Total Return Amount : " + Convert.ToString(addNewItemSalesReturn.TotalReturnAmount) + " "
                                                + " , Costing Division ID : " + Convert.ToString(addNewItemSalesReturn.CostingDivisionID) + " "
                                                ;
                            LogInfoList.Add(LogInformation);
                        }
                        if (bizactionVO.ItemsDetail != null && bizactionVO.ItemsDetail.Count > 0)   //item sales return  get items details
                        {
                            objGUID = new Guid();

                            LogInformation = new LogInfo();
                            LogInformation.guid = objGUID;
                            LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                            LogInformation.TimeStamp = DateTime.Now;
                            LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                            LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                            LogInformation.Message = " 106 : T_ItemSaleReturnDetails : "
                            + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " ";
                            foreach (var item in bizactionVO.ItemsDetail)
                            {
                                LogInformation.Message = LogInformation.Message
                                                + " , ID : " + Convert.ToString(item.ID) + " "
                                                + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                                + " , ItemName : " + Convert.ToString(item.ItemName) + " "
                                                + " , ItemCode : " + Convert.ToString(item.ItemCode) + " "
                                                + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                                + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                                + " , ItemSaleId : " + Convert.ToString(item.ItemSaleId) + " "
                                                + " , ItemSaleReturnId : " + Convert.ToString(item.ItemSaleReturnId) + " "
                                                + " , ItemVatType : " + Convert.ToString(item.ItemVatType) + " "
                                                + " , Manufacture : " + Convert.ToString(item.Manufacture) + " "
                                                + " , MRP : " + Convert.ToString(item.MRP) + " "
                                                + " , Net Amount : " + Convert.ToString(item.NetAmount) + " "
                                                + " , NetAmtCalculation : " + Convert.ToString(item.NetAmtCalculation) + " "
                                                + " , PendingQuantity : " + Convert.ToString(item.PendingQuantity) + " "
                                                + " , PregnancyClass : " + Convert.ToString(item.PregnancyClass) + " "
                                                + " , PUOMID : " + Convert.ToString(item.PUOMID) + " "
                                                + " , Quantity : " + Convert.ToString(item.Quantity) + " "
                                                + " , ReturnQuantity : " + Convert.ToString(item.ReturnQuantity) + " "
                                                + " , SaleQuantity : " + Convert.ToString(item.SaleQuantity) + " "
                                                + " , SaleUOM : " + Convert.ToString(item.SaleUOM) + " "
                                                + " , SaleUOMID : " + Convert.ToString(item.SaleUOMID) + " "
                                                + " , SellingUOM : " + Convert.ToString(item.SellingUOM) + " "
                                                + " , SellingUOMID : " + Convert.ToString(item.SellingUOMID) + " "
                                                + " , SingleQuantity : " + Convert.ToString(item.SingleQuantity) + " "
                                                + " , StockCF : " + Convert.ToString(item.StockCF) + " "
                                                + " , SUOMID : " + Convert.ToString(item.SUOMID) + " ";
                                //   LogInfoList.Add(LogInformation);
                            }
                            LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                            LogInfoList.Add(LogInformation);

                            bizactionVO.LogInfoList = new List<LogInfo>();  // For the Activity Log List
                            bizactionVO.LogInfoList = LogInfoList.DeepCopy();

                        }
                    }

                    //===============================================================================================================================================================

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)      // Refund to Advance 22042017
                        RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    else
                        RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                    bizactionVO.ItemMatserDetail.RefundDetails = RefundObj;

                    bizactionVO.IsRefundToAdvance = ((PaymentDetails)sender).IsRefundToAdvance;     // Refund to Advance 22042017

                    bizactionVO.RefundToAdvancePatientID = this.SelectedItem.PatientID;        // Refund to Advance 22042017
                    bizactionVO.RefundToAdvancePatientUnitID = this.SelectedItem.UnitId;       // Refund to Advance 22042017



                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        cmdSave.IsEnabled = true;     //Added by Ashish Z. on 240816
                        ClickedFlag1 = 0;
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddItemSalesReturnBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Item sale return saved successfully with item sale return no. " + ((clsAddItemSalesReturnBizActionVO)args.Result).ItemMatserDetail.ItemSaleReturnNo, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                   {
                                       if (res == MessageBoxResult.OK)
                                       {
                                           if (((clsAddItemSalesReturnBizActionVO)args.Result).ItemSalesReturnID > 0)
                                           {
                                               PrintReport(((clsAddItemSalesReturnBizActionVO)args.Result).ItemSalesReturnID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                                           }
                                       }
                                   };
                                msgW1.Show();

                                //After Insertion Back to BackPanel and Setup Page
                                SetCommandButtonState("Save");
                                setItemSalesReturnGrid();


                                objAnimation.Invoke(RotationType.Backward);
                                FromDate = null;
                                ToDate = null;

                                FromDate = dtpFromDate.SelectedDate;
                                ToDate = dtpToDate.SelectedDate;
                                if (CheckValidationForSearch())
                                {
                                    ItemSalesReturnList = new PagedSortableCollectionView<clsItemSalesReturnVO>();
                                    ItemSalesReturnList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesReturnList_OnRefresh);
                                    PageSizeSalesReturnList = 5;
                                    this.dataGrid2Pager.DataContext = ItemSalesReturnList;
                                    this.dgItemSalesReturnList.DataContext = ItemSalesReturnList;
                                    setItemSalesReturnGrid();
                                }
                            }

                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

                catch (Exception ex)
                {
                    ClickedFlag1 = 0;
                    throw;
                }
            }
            else
                ClickedFlag1 = 0;

        }

        #endregion

        private void PrintReport(long Id, long UnitId)
        {

            string URL = "../Reports/OPD/ItemSalesReturnReport.aspx?ID=" + Id + "&UnitID=" + UnitId;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }


        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            SetCommandButtonState("New");
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

            FromDate = null;
            ToDate = null;

            FromDate = dtpFromDate.SelectedDate;
            ToDate = dtpToDate.SelectedDate;
            if (CheckValidationForSearch())
            {
                ItemSalesReturnList = new PagedSortableCollectionView<clsItemSalesReturnVO>();
                ItemSalesReturnList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesReturnList_OnRefresh);
                PageSizeSalesReturnList = 5;
                this.dataGrid2Pager.DataContext = ItemSalesReturnList;
                this.dgItemSalesReturnList.DataContext = ItemSalesReturnList;
                setItemSalesReturnGrid();
            }
        }

        //private void chkStatus_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (dgItemSalesDetails.SelectedItem != null)
        //    {
        //        clsItemSalesDetailsVO item = ((clsItemSalesDetailsVO)dgItemSalesDetails.SelectedItem).DeepCopy<clsItemSalesDetailsVO>();
        //        ItemSalesReturnDetailsList = new List<clsItemSalesReturnDetailsVO>();
        //        ItemSalesReturnDetailsList = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());

        //        if (ItemSalesReturnDetailsList.Count > 0)
        //        {
        //            Flag = true;
        //            msgText = "";
        //            foreach (var itemsalesreturn in ItemSalesReturnDetailsList)
        //            {
        //                if (itemsalesreturn.ItemSaleId == item.ItemSaleId)
        //                {
        //                    if (itemsalesreturn.BatchID == item.BatchID)
        //                    {
        //                        msgText = msgText + " Item Name:: " + item.ItemName + " BatchId:: " + item.BatchID + ",";
        //                        Flag = false;
        //                        break;

        //                    }

        //                }
        //                else
        //                {
        //                    Flag = true;
        //                }

        //            }
        //            if (Flag == true)
        //            {
        //                AddItemSalesReturnDetails.Add(new clsItemSalesReturnDetailsVO() { ItemID = item.ItemID, ItemName = item.ItemName, ItemSaleId = item.ItemSaleId, BatchID = item.BatchID, ReturnQuantity = item.Quantity, Quantity = item.Quantity, MRP = item.MRP, ConcessionPercentage = item.ConcessionPercentage, ConcessionAmount = item.ConcessionAmount, VATPercent = item.VATPercent, VATAmount = item.VATAmount, Amount = item.Amount, NetAmount = item.NetAmount });
        //                dgItemSalesReturnDetials.ItemsSource = AddItemSalesReturnDetails;
        //            }
        //            else
        //            {
        //                msgText = msgText + "Item is Already Added! ";
        //                MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
        //                msgWindow.Show();
        //            }
        //        }
        //        else
        //        {
        //            AddItemSalesReturnDetails.Add(new clsItemSalesReturnDetailsVO() { ItemID = item.ItemID, ItemName = item.ItemName, ItemSaleId = item.ItemSaleId, BatchID = item.BatchID, ReturnQuantity = item.Quantity, Quantity = item.Quantity, MRP = item.MRP, ConcessionPercentage = item.ConcessionPercentage, ConcessionAmount = item.ConcessionAmount, VATPercent = item.VATPercent, VATAmount = item.VATAmount, Amount = item.Amount, NetAmount = item.NetAmount });
        //            dgItemSalesReturnDetials.ItemsSource = AddItemSalesReturnDetails;
        //        }

        //    }
        //    ItemSalesReturnDetailsList = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());

        //    TotalAmount = 0;
        //    NetAmount = 0;
        //    NetReturnAmount = 0;
        //    foreach (var item in AddItemSalesReturnDetails)
        //    {
        //        TotalAmount = item.Amount + TotalAmount;
        //        NetAmount = NetAmount + item.NetAmount;
        //        foreach (clsItemSalesDetailsVO item1 in dgItemSalesDetails.ItemsSource)
        //        {
        //            if (item1.ItemID == item.ItemID)
        //            {
        //                if (item1.BatchID == item.BatchID)
        //                {
        //                    if (item1.NetAmount != item.NetAmount)
        //                    {
        //                        NetReturnAmount = NetReturnAmount + (item1.NetAmount - item.NetAmount);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        NetReturnAmount = NetReturnAmount + (item1.NetAmount);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    txtNetAmount.Text = NetAmount.ToString();
        //    txtTotalAmount.Text = TotalAmount.ToString();
        //    txtTotalReturnAmount.Text = NetReturnAmount.ToString();


        //}

        //private void cmdSearchForItemSales_Click(object sender, RoutedEventArgs e)
        //{
        //    FromDate = null;
        //    ToDate = null;

        //    FromDate = dtItemSalesFromDate.SelectedDate;
        //    ToDate = dtItemSalesToDate.SelectedDate;
        //    if (CheckValidationForSearch())
        //    {
        //        setItemSalesGrid();
        //    }
        //}

        private void cmdDeleteReturnSales_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgItemSalesReturnDetials.SelectedItem is clsItemSalesReturnDetailsVO)
            {
                clsItemSalesReturnDetailsVO person = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).DeepCopy<clsItemSalesReturnDetailsVO>();
                AddItemSalesReturnDetails.Remove(((clsItemSalesReturnDetailsVO)this.dgItemSalesReturnDetials.SelectedItem));
            }

            CalculateSummary();
            //TotalAmount = 0;
            //NetAmount = 0;
            //NetReturnAmount = 0;
            //TotalVATAmount = 0;
            //TotalConsessionAmount = 0;
            //foreach (var item in AddItemSalesReturnDetails)
            //{
            //    TotalAmount = item.Amount + TotalAmount;
            //    TotalConsessionAmount = TotalConsessionAmount + item.ConcessionAmount;
            //    TotalVATAmount = TotalVATAmount + (item.VATAmount);
            //    NetAmount = NetAmount + item.NetAmount;

            //    NetReturnAmount = NetReturnAmount + (item.TotalSalesAmount - item.NetAmount);



            //}
            ////txtNetAmount.Text = NetAmount.ToString();
            //txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);

            ////txtTotalAmount.Text = TotalAmount.ToString();
            //txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);

            ////txtTotalReturnAmount.Text = NetReturnAmount.ToString();
            //txtTotalReturnAmount.Text = String.Format("{0:0.00}", NetReturnAmount);

            ////txtTotalConcessionAmount.Text = TotalConsessionAmount.ToString();
            //txtTotalConcessionAmount.Text = String.Format("{0:0.00}", TotalConsessionAmount);

            //// txtTotalVATAmount.Text = TotalVATAmount.ToString();
            //txtTotalVATAmount.Text = String.Format("{0:0.00}", TotalVATAmount);

            //if (dgItemSalesReturnDetials.SelectedItem != null)
            //{
            //    clsItemSalesReturnDetailsVO SalesDetails = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).DeepCopy<clsItemSalesReturnDetailsVO>();
            //    int i = 0;
            //    foreach (var item in ItemSalesReturnDetailsList)
            //    {
            //        if (item.ItemSaleId == SalesDetails.ItemSaleId)
            //        {
            //            if (item.BatchID == SalesDetails.BatchID)
            //            {
            //                AddItemSalesReturnDetails.RemoveAt(i);

            //            }
            //        }
            //        i++;
            //    }

            //    var authors = (from x in AddItemSalesReturnDetails where x.ItemSaleId == SalesDetails.ItemSaleId && x.BatchID == SalesDetails.BatchID select x);
            //    for (int j = 0; j < (authors.ToList().Count); j++)
            //    {
            //        AddItemSalesReturnDetails.Remove(authors.ToList()[0]);
            //    }



            //}

            ItemSalesReturnDetailsList = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            SalesItemSearch newSalesItemSearch = new SalesItemSearch();
            newSalesItemSearch.OnSaveButton_Click += new RoutedEventHandler(newSalesItemSearch_OnSaveButton_Click);
            newSalesItemSearch.Show();


        }

        void newSalesItemSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            dgItemSalesReturnDetials.ItemsSource = null;
            SalesItemSearch SalesItemList = ((SalesItemSearch)sender);
            this.SelectedSalesDetailsItem = new ObservableCollection<clsItemSalesDetailsVO>();
            this.SelectedSalesDetailsItem = SalesItemList.SelectedSalesDetailsItem;
            if (SalesItemList.SelectedSalesDetailsItem != null)
            {

                foreach (clsItemSalesDetailsVO item in SalesItemList.SelectedSalesDetailsItem)
                {
                    if (AddItemSalesReturnDetails.Where(saleRetItems => saleRetItems.ItemSaleId != item.ItemSaleId).Any() == true)
                        AddItemSalesReturnDetails = new ObservableCollection<clsItemSalesReturnDetailsVO>();
                    if (AddItemSalesReturnDetails.Where(saleRetItems => (saleRetItems.ItemID == item.ItemID && saleRetItems.BatchID == item.BatchID)).Any() == false)
                        AddItemSalesReturnDetails.Add(new clsItemSalesReturnDetailsVO()
                        {
                            TotalSalesAmount = item.NetAmount,
                            BatchCode = item.BatchCode,
                            BatchID = item.BatchID,
                            ConcessionAmount = item.ConcessionAmount,
                            ConcessionPercentage = item.ConcessionPercentage,
                            ExpiryDate = item.ExpiryDate,
                            ID = item.ID,
                            ItemCode = item.ItemCode,
                            ItemID = item.ItemID,
                            ItemName = item.ItemName,
                            ItemSaleId = item.ItemSaleId,
                            MRP = item.MRP,
                            BaseMRP = Convert.ToSingle(item.MRP) / item.BaseConversionFactor,  //by Umesh
                            // NetAmount = item.NetAmount,
                            PregnancyClass = item.PregnancyClass,
                            //  Quantity = item.Quantity,
                            PendingQuantity = item.PendingQuantity,
                            VATAmount = item.VATAmount,
                            VATPercent = item.VATPercent,
                            ItemVatType = item.ItemVatType,
                            //Added By Bhushan For GST 14072017


                            SGSTAmount = item.SGSTAmount,
                            SGSTPercent = item.SGSTPercent,
                            SGSTtaxtype = item.SGSTtaxtype,

                            CGSTAmount = item.CGSTAmount,
                            CGSTPercent = item.CGSTPercent,
                            CGSTtaxtype = item.CGSTtaxtype,

                            IGSTAmount = item.IGSTAmount,
                            IGSTPercent = item.IGSTPercent,
                            IGSTtaxtype = item.IGSTtaxtype,

                            SelectedUOM = new MasterListItem(item.SelectedUOM.ID, item.SelectedUOM.Description),
                            BaseUOM = item.BaseUOM,
                            BaseUOMID = item.BaseUOMID,
                            SaleUOMID = item.SaleUOMID,
                            SaleUOM = item.SaleUOM,
                            ConversionFactor = item.StockCF,
                            ReturnQuantity = 1,
                            BaseConversionFactor = item.BaseConversionFactor,
                            BaseQuantity = (1 * item.BaseConversionFactor),
                            NetAmtCalculation = (item.NetAmtCalculation / item.Quantity),
                            SaleQuantity = item.Quantity,
                            // Quantity = item.BaseQuantity * item.StockCF ,
                            BaseSaleQuantity = item.BaseQuantity,
                            SUOMID = item.SUOMID,
                            StockCF = item.StockCF,



                        });
                }
            }
            this.SelectedItem = SalesItemList.SelectedItem;
            BillID = SalesItemList.SelectedItem.BillID;


            CalculateSummary();

            //TotalAmount = 0;
            //NetAmount = 0;
            //NetReturnAmount = 0;
            //TotalVATAmount = 0;
            //TotalConsessionAmount = 0;
            //foreach (var item in AddItemSalesReturnDetails)
            //{
            //    TotalAmount = item.Amount + TotalAmount;
            //    TotalConsessionAmount = TotalConsessionAmount + item.ConcessionAmount;
            //    TotalVATAmount = TotalVATAmount + (item.VATAmount);
            //    NetAmount = NetAmount + item.NetAmount;
            //    NetReturnAmount = NetReturnAmount + (item.TotalSalesAmount - item.NetAmount);



            //}
            ////txtNetAmount.Text = NetAmount.ToString();
            //txtNetAmount.Text = String.Format("{0:0.00}", NetAmount);

            ////txtTotalAmount.Text = TotalAmount.ToString();
            //txtTotalAmount.Text = String.Format("{0:0.00}", TotalAmount);

            ////txtTotalReturnAmount.Text = NetReturnAmount.ToString();
            //txtTotalReturnAmount.Text = String.Format("{0:0.00}", NetReturnAmount);

            ////txtTotalConcessionAmount.Text = TotalConsessionAmount.ToString();
            //txtTotalConcessionAmount.Text = String.Format("{0:0.00}", TotalConsessionAmount);

            //// txtTotalVATAmount.Text = TotalVATAmount.ToString();
            //txtTotalVATAmount.Text = String.Format("{0:0.00}", TotalVATAmount);
            ////By Anjali............................
            //txtRountOffAmount.Text = Math.Round(NetAmount).ToString();
            //roundAmt = Math.Round(NetAmount);
            ////.....................................

            //added by rohinee dated 26/9/2016 for audit trail    
            if (AddItemSalesReturnDetails != null && AddItemSalesReturnDetails.Count > 0)
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
                    LogInformation.Message = " 101 : Get Sales : "
                    + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " ";
                    foreach (var item in AddItemSalesReturnDetails)
                    {
                        LogInformation.Message = LogInformation.Message
                                        + " , ID : " + Convert.ToString(item.ID) + " "
                                        + " , ItemID : " + Convert.ToString(item.ItemID) + " "
                                        + " , ItemName : " + Convert.ToString(item.ItemName) + " "
                                        + " , ItemCode : " + Convert.ToString(item.ItemCode) + " "
                                        + " , BatchID : " + Convert.ToString(item.BatchID) + " "
                                        + " , BatchCode : " + Convert.ToString(item.BatchCode) + " "
                                        + " , ItemSaleId : " + Convert.ToString(item.ItemSaleId) + " "
                                        + " , ItemSaleReturnId : " + Convert.ToString(item.ItemSaleReturnId) + " "
                                        + " , ItemVatType : " + Convert.ToString(item.ItemVatType) + " "
                                        + " , Manufacture : " + Convert.ToString(item.Manufacture) + " "
                                        + " , MRP : " + Convert.ToString(item.MRP) + " "
                                        + " , NetAmount : " + Convert.ToString(item.NetAmount) + " "
                                        + " , ID : " + Convert.ToString(item.NetAmtCalculation) + " "
                                        + " , ItemID : " + Convert.ToString(item.PendingQuantity) + " "
                                        + " , ItemName : " + Convert.ToString(item.PregnancyClass) + " "
                                        + " , ItemCode : " + Convert.ToString(item.PUOMID) + " "
                                        + " , BatchID : " + Convert.ToString(item.Quantity) + " "
                                        + " , BatchCode : " + Convert.ToString(item.ReturnQuantity) + " "
                                        + " , ItemSaleId : " + Convert.ToString(item.SaleQuantity) + " "
                                        + " , ItemSaleReturnId : " + Convert.ToString(item.SaleUOM) + " "
                                        + " , ItemVatType : " + Convert.ToString(item.SaleUOMID) + " "
                                        + " , Manufacture : " + Convert.ToString(item.SellingUOM) + " "
                                        + " , MRP : " + Convert.ToString(item.SellingUOMID) + " "
                                        + " , NetAmount : " + Convert.ToString(item.SingleQuantity) + " "
                                        + " , Manufacture : " + Convert.ToString(item.StockCF) + " "
                                        + " , MRP : " + Convert.ToString(item.SUOMID) + " ";
                        //LogInfoList.Add(LogInformation);
                    }
                    LogInformation.Message = LogInformation.Message.Replace("\r\n", Environment.NewLine);
                    LogInfoList.Add(LogInformation);
                }
            }


            //=========================================================================
            dgItemSalesReturnDetials.ItemsSource = AddItemSalesReturnDetails;

        }

        double roundAmt = 0;
        #endregion Click Event

        #region Selection Changed Event
        //private void dgItemSales_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (dgItemSales.SelectedItem != null)
        //    {
        //        clsItemSalesVO Item = ((clsItemSalesVO)dgItemSales.SelectedItem).DeepCopy<clsItemSalesVO>();
        //        ItemSaleId = Item.ID;
        //        dgItemSalesDetails.ItemsSource = null;
        //        setItemSalesDetailsGrid();
        //        AddItemSalesReturnDetails = new ObservableCollection<clsItemSalesReturnDetailsVO>();
        //        dgItemSalesReturnDetials.ItemsSource = AddItemSalesReturnDetails;


        //    }

        //}

        private void dgItemSalesReturnList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgItemSalesReturnList.SelectedItem != null)
            {
                clsItemSalesReturnVO Item = ((clsItemSalesReturnVO)dgItemSalesReturnList.SelectedItem).DeepCopy<clsItemSalesReturnVO>();
                setItemSalesReturnDeatilsGrid(Item.ID, Item.UnitId);
                //AddItemSalesReturnDetails = new ObservableCollection<clsItemSalesReturnDetailsVO>();
                //dgItemSalesReturnDetials.ItemsSource = AddItemSalesReturnDetails;

            }
        }
        #endregion

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgItemSalesReturnList.SelectedItem != null)
            {
                PrintReport(((clsItemSalesReturnVO)dgItemSalesReturnList.SelectedItem).ID, ((clsItemSalesReturnVO)dgItemSalesReturnList.SelectedItem).UnitId);//ID
            }



        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            if (dgItemSalesReturnDetials.SelectedItem != null)
            {
                Conversion win = new Conversion();

                win.FillUOMConversions(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemID, ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID);
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

            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM = (MasterListItem)Itemswin.cmbConversion.SelectedItem;
            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMList = Itemswin.UOMConvertLIst;
            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMConversionList = Itemswin.UOMConversionLIst;

            CalculateConversionFactorCentral(((MasterListItem)Itemswin.cmbConversion.SelectedItem).ID, ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SUOMID);



        }

        private void CalculateConversionFactorCentral(long FromUOMID, long ToUOMID)
        {
            clsConversions objConversionVO = new clsConversions();

            try
            {
                List<clsConversionsVO> UOMConvertLIst = new List<clsConversionsVO>();

                if (dgItemSalesReturnDetials.SelectedItem != null)
                {
                    UOMConvertLIst = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMConversionList;
                    objConversionVO.UOMConvertLIst = UOMConvertLIst;
                    objConversionVO.SingleQuantity = Convert.ToSingle(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity);
                    objConversionVO.MainMRP = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseMRP;   //by Umesh
                    long BaseUOMID = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOMID;
                    objConversionVO.CalculateConversionFactor(FromUOMID, ToUOMID, BaseUOMID);
                    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = objConversionVO.SingleQuantity;
                    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity = objConversionVO.BaseQuantity;
                    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConversionFactor = objConversionVO.ConversionFactor;
                    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor = objConversionVO.BaseConversionFactor;
                    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).MRP = objConversionVO.MRP;


                    //if (SalesDetails.BaseQuantity > SalesDetails.PendingQuantity)
                    //{
                    //    msgText = "Item's Return Quantity is Greater than Item's Sales Pending Quantity";
                    //    MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    //    msgWindow.Show();
                    //    SalesDetails.ReturnQuantity = (SalesDetails.BaseQuantity / SalesDetails.ConversionFactor);
                    //    ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = (SalesDetails.BaseQuantity / SalesDetails.ConversionFactor);

                    //}



                    if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity <= 0)
                    {

                        ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = 1;
                        string msgText = "Quantity Cannot be less then or equal to zero ";
                        ConversionsForAvailableStock();
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                    }
                    else
                        if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity > ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity)
                        {
                            float availQty = Convert.ToSingle(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity);

                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = Math.Floor(Convert.ToDouble(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity / objConversionVO.BaseConversionFactor));
                            string msgText = "Quantity Must Be Less Than Or Equal To Pending Quantity ";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();
                        }
                        else if (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM != null && ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID == ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOMID && (((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity % 1) != 0)
                        {
                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity = 1;
                            string msgText = "Quantity Cannot be in fraction";
                            ConversionsForAvailableStock();
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.Show();

                        }
                    CalculateSummary();
                    #region by Rohinee For AuditTrail
                    if (IsAuditTrail == true && ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem != null))
                    {
                        LogInformation = new LogInfo();
                        LogInformation.guid = objGUID;
                        LogInformation.UserId = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                        LogInformation.TimeStamp = DateTime.Now;
                        LogInformation.ClassName = MethodBase.GetCurrentMethod().DeclaringType.ToString();
                        LogInformation.MethodName = MethodBase.GetCurrentMethod().ToString();
                        LogInformation.Message = "103 : Sales Item UOM Changed : " //+ Convert.ToString(lineNumber)
                              + "Unit Id : " + Convert.ToString(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId) + " "
                              + " , Amount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).Amount) + " "
                              + " , BaseConversionFactor : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor) + " "
                            //      + " , BaseMRP : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseMRP) + " "
                              + " , BaseQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity) + " "
                            //     + " , BaseRate : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseRate) + " "
                              + " , BaseSaleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseSaleQuantity) + " "
                            //    + " , BaseUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOM) + " "
                            //     + " , BaseUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseUOMID) + " "
                              + " , ConcessionAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConcessionAmount) + " "
                              + " , ConcessionPercentage : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConcessionPercentage) + " "
                              + " , ConversionFactor : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ConversionFactor) + " "
                              + " , ItemCode : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemCode) + " "
                              + " , ItemID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemID) + " "
                              + " , ItemName : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemName) + " "
                            //     + " , ItemSaleId : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemSaleId) + " "
                              + " , MRP : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).MRP) + " "
                              + " , NetAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).NetAmount) + " "
                              + " , NetAmtCalculation : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).NetAmtCalculation) + " "
                              + " , PendingQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).PendingQuantity) + " "
                              + " , Quantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).Quantity) + " "
                              + " , ReturnQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity) + " "
                              + " , SaleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleQuantity) + " "
                            //      + " , SaleUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleUOM) + " "
                            //      + " , SaleUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SaleUOMID) + " "
                            //      + " , SellingUOM : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SellingUOM) + " "
                            //      + " , SellingUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SellingUOMID) + " "
                              + " , SelectedUOM ID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID) + " "
                              + " , SelectedUOM Name : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.Name) + " "
                              + " , SingleQuantity : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SingleQuantity) + " "
                            //      + " , StockCF : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).StockCF) + " "
                            //      + " , SUOMID : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SUOMID) + " "
                              + " , VATAmount : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATAmount) + " "
                            //       + " , VATPercent : " + Convert.ToString(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).VATPercent) + " "
                              + " , Net Amount : " + Convert.ToString(txtNetAmount.Text) + " "
                              + " , Total Amount : " + Convert.ToString(txtTotalAmount.Text) + " "
                              + " , Total Return Amount : " + Convert.ToString(txtTotalReturnAmount.Text) + " "
                              + " , Total Concession Amount : " + Convert.ToString(txtTotalConcessionAmount.Text) + " "
                              + " , Round Off Amount : " + Convert.ToString(txtRountOffAmount.Text) + " "
                              + " , Total VAT Amount : " + Convert.ToString(txtTotalVATAmount.Text) + " "
                              ;
                        LogInfoList.Add(LogInformation);
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                objConversionVO = null;
            }
        }
        private void ConversionsForAvailableStock()
        {
            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseQuantity = Convert.ToSingle(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ReturnQuantity) * ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseRate = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainRate * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            //((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseMRP = ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).MainMRP * ((clsItemSalesDetailsVO)dgPharmacyItems.SelectedItem).BaseConversionFactor;
            CalculateSummary();
        }
        private void FillUOMConversions()
        {
            WaitIndicator IndicatiorConversions = new WaitIndicator();
            try
            {

                IndicatiorConversions.Show();

                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).ItemID;
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

                        if (dgItemSalesReturnDetials.SelectedItem != null)
                        {
                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMConversionList = UOMConversionLIst.DeepCopy();
                            ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).UOMList = UOMConvertLIst.DeepCopy();

                        }
                        CalculateConversionFactorCentral(((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SelectedUOM.ID, ((clsItemSalesReturnDetailsVO)dgItemSalesReturnDetials.SelectedItem).SUOMID);


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
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtRountOffAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveNumber()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtRountOffAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }
        bool resultround = true;
        private void txtRountOffAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRountOffAmount.Text) && txtRountOffAmount.Text.IsValueDouble())
            {
                if (Convert.ToDouble(txtRountOffAmount.Text) > Comman.MinMaxRoundOff + roundAmt)
                {
                    txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ", Round Off Amount Exceeds the value");
                    txtRountOffAmount.RaiseValidationError();
                    resultround = false;
                }
                else if (Comman.MinMaxRoundOff > roundAmt)
                {

                    if ((Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff) && Convert.ToDouble(txtRountOffAmount.Text) < 0)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount can not be less than zero");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else if (Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount is less then the value");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else
                    {
                        txtRountOffAmount.ClearValidationError();
                        resultround = true;
                    }

                }
                else if (Comman.MinMaxRoundOff < roundAmt)
                {
                    if (Convert.ToDouble(txtRountOffAmount.Text) < roundAmt - Comman.MinMaxRoundOff)
                    {
                        txtRountOffAmount.SetValidation("You can Round up to (+ or -)" + Comman.MinMaxRoundOff + ",Round Off Amount is less then the value");
                        txtRountOffAmount.RaiseValidationError();
                        resultround = false;
                    }
                    else
                    {
                        txtRountOffAmount.ClearValidationError();
                        resultround = true;
                    }
                }
                else
                {
                    txtRountOffAmount.ClearValidationError();
                    resultround = true;
                }
            }
            else
            {
                txtRountOffAmount.Text = roundAmt.ToString(); ;
                resultround = true;
            }
        }

        public clsPaymentVO Payment { get; set; }

        void SaveExchangeMaterial()
        {
            #region from Payment Window
            Payment = new clsPaymentVO();
            Payment.CreditAuthorizedBy = 0;
            Payment.ChequeAuthorizedBy = 0;
            Payment.RefundID = 2; // (long)PaymentForType.Refund;
            Payment.RefundAmount = Math.Round(double.Parse(txtRountOffAmount.Text), 0); // Convert.ToDouble(txtPaidAmount.Text);

            PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO objPay = new PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO();
            objPay.PaymentModeID = 8; //for advance //((MasterListItem)cmbPayMode.SelectedItem).ID;
            objPay.PaidAmount = Math.Round(double.Parse(txtRountOffAmount.Text), 0); // Convert.ToDouble(txtPaidAmount.Text);
            objPay.Number = ""; //txtNumber.Text;
            objPay.Date = null; //dtpDate.SelectedDate;
            objPay.BankID = 0; //((MasterListItem)cmbBank.SelectedItem).ID;
            Payment.PaymentDetails.Add(objPay);
            #endregion

            if (CheckValidationForSave())
            {
                //cmdSave.IsEnabled = false;     //Added by Ashish Z. on 240816
                clsRefundVO RefundObj = new clsRefundVO();

                //if (((PaymentDetails)sender).Payment != null)
                //{
                //    RefundObj.PaymentDetails = ((PaymentDetails)sender).Payment;
                //}
                RefundObj.PaymentDetails = Payment;

                RefundObj.Date = DateTime.Now;

                //RefundObj.BillID = BillID;
                RefundObj.ItemSaleReturnID = ItemSaleReturnID;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    RefundObj.Amount = Math.Round(double.Parse(txtRountOffAmount.Text), 0); // Convert.ToDouble(((PaymentDetails)sender).txtPaidAmount.Text); 

                clsAddItemSalesReturnBizActionVO bizactionVO = new clsAddItemSalesReturnBizActionVO();
                bizactionVO.ItemMatserDetail = new clsItemSalesReturnVO();
                bizactionVO.IsExchangeMaterial = true;
                clsItemSalesReturnVO addNewItemSalesReturn = new clsItemSalesReturnVO();

                try
                {

                    bizactionVO.ItemsDetail = ((List<clsItemSalesReturnDetailsVO>)AddItemSalesReturnDetails.ToList<clsItemSalesReturnDetailsVO>());

                    addNewItemSalesReturn.Date = dtItemReturnSalesDate.SelectedDate.Value;
                    addNewItemSalesReturn.Time = System.DateTime.Now;
                    addNewItemSalesReturn.ItemSalesID = this.SelectedItem.ID;
                    addNewItemSalesReturn.StoreID = this.SelectedItem.StoreID;
                    addNewItemSalesReturn.Remarks = txtRemarks.Text;
                    addNewItemSalesReturn.ConcessionAmount = Convert.ToDouble(txtTotalConcessionAmount.Text);
                    addNewItemSalesReturn.VATAmount = Convert.ToDouble(txtTotalVATAmount.Text);
                    addNewItemSalesReturn.TotalAmount = Convert.ToDouble(txtTotalAmount.Text);
                    addNewItemSalesReturn.TotalSGST = Convert.ToDouble(txtTotalSGSTAmount.Text);
                    addNewItemSalesReturn.TotalCGST = Convert.ToDouble(txtTotalCGSTAmount.Text);
                    addNewItemSalesReturn.NetAmount = Convert.ToDouble(txtRountOffAmount.Text);
                    addNewItemSalesReturn.CalculatedNetAmount = Convert.ToDouble(txtNetAmount.Text);
                    addNewItemSalesReturn.TotalReturnAmount = Convert.ToDouble(txtTotalReturnAmount.Text);
                    addNewItemSalesReturn.Status = true;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                        addNewItemSalesReturn.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    else
                        addNewItemSalesReturn.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                    bizactionVO.ItemMatserDetail = addNewItemSalesReturn;


                    //===============================================================================================================================================================

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)      // Refund to Advance 22042017
                        RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    else
                        RefundObj.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCounterID;

                    bizactionVO.ItemMatserDetail.RefundDetails = RefundObj;

                    bizactionVO.IsRefundToAdvance = true; //((PaymentDetails)sender).IsRefundToAdvance;     // Refund to Advance 22042017

                    bizactionVO.RefundToAdvancePatientID = this.SelectedItem.PatientID;        // Refund to Advance 22042017
                    bizactionVO.RefundToAdvancePatientUnitID = this.SelectedItem.UnitId;       // Refund to Advance 22042017



                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        ClickedFlag1 = 0;
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsAddItemSalesReturnBizActionVO)args.Result).SuccessStatus == 1)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Exchange the Material saved to Patient Advance successfully with no. " + ((clsAddItemSalesReturnBizActionVO)args.Result).ItemMatserDetail.ItemSaleReturnNo, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                    {
                                        //if (((clsAddItemSalesReturnBizActionVO)args.Result).ItemSalesReturnID > 0)
                                        //{
                                        //    PrintReport(((clsAddItemSalesReturnBizActionVO)args.Result).ItemSalesReturnID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                                        //}
                                    }
                                };
                                msgW1.Show();

                                //After Insertion Back to BackPanel and Setup Page
                                SetCommandButtonState("Save");
                                setItemSalesReturnGrid();


                                objAnimation.Invoke(RotationType.Backward);
                                FromDate = null;
                                ToDate = null;

                                FromDate = dtpFromDate.SelectedDate;
                                ToDate = dtpToDate.SelectedDate;
                                if (CheckValidationForSearch())
                                {
                                    ItemSalesReturnList = new PagedSortableCollectionView<clsItemSalesReturnVO>();
                                    ItemSalesReturnList.OnRefresh += new EventHandler<RefreshEventArgs>(ItemSalesReturnList_OnRefresh);
                                    PageSizeSalesReturnList = 5;
                                    this.dataGrid2Pager.DataContext = ItemSalesReturnList;
                                    this.dgItemSalesReturnList.DataContext = ItemSalesReturnList;
                                    setItemSalesReturnGrid();
                                }
                            }

                        }
                        else
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };
                    client.ProcessAsync(bizactionVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

                catch (Exception ex)
                {
                    ClickedFlag1 = 0;
                    throw;
                }
            }
            else
                ClickedFlag1 = 0;

        }
    }
}
