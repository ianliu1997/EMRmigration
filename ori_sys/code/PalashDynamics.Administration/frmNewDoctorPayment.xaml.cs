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
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Animations;
using PalashDynamics.Administration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.ValueObjects.Administration;
using System.Text;
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects;
using System.Windows.Data;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Windows.Browser;




namespace PalashDynamics.Administration
{
    public partial class frmNewDoctorPayment : UserControl
    {
        WaitIndicator Indicator = null;
        WaitIndicator Indicator2 = null;
        bool IsCancel = true;
        public bool calcShare = false;
        public bool btnCalShare1 = false;
        public long DID = 0;
        public long DoctorShareLinkingTypeID;
        public DateTime fromdate = DateTime.Now;
        public long docid;

        public string docname;

        List<long> DIDs = new List<long>();

        public double selectedamtforpayment;
        public double totdoctorshare;
        //public PagedSortableCollectionView<clsCompanyPaymentDetailsVO> DataList { get; private set; }
        List<clsCompanyPaymentDetailsVO> lstDoctorPaymentDetails;
        List<clsDoctorVO> lstSelectedDoctor;
        List<clsCompanyPaymentDetailsVO> lstSelectedDoctorPaymentDetails;
        public PagedSortableCollectionView<clsDoctorPaymentVO> BackPanelDataList { get; private set; }
        public PagedSortableCollectionView<clsDoctorPaymentVO> FrontPanelDataList { get; private set; }
        public List<clsDoctorPaymentVO> SelectedDataList { get; private set; }
        public List<clsDoctorPaymentVO> SelectedDoctor { get; private set; }
        PalashDynamics.Animations.SwivelAnimation _flip = null;
        private SwivelAnimation objAnimation;
        StringBuilder docnames = new StringBuilder();

        public int DataListPageSize
        {
            get
            {
                return BackPanelDataList.PageSize;
            }
            set
            {
                if (value == BackPanelDataList.PageSize) return;
                BackPanelDataList.PageSize = value;
            }
        }

        void BackPanelDataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            FetchData();
        }

        public frmNewDoctorPayment()
        {
            InitializeComponent();
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            _flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            BackPanelDataList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
            BackPanelDataList.OnRefresh += new EventHandler<RefreshEventArgs>(BackPanelDataList_OnRefresh);

            DataListPageSize = 16;
            this.dgPaidDRList1.DataContext = BackPanelDataList;
            this.dgDataPager1.DataContext = BackPanelDataList;
            DIDs = new List<long>();

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (dtpFromDate1.SelectedDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Select From Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                dtpFromDate1.Focus();
                return;
            }
            if (dtpFromDate1.SelectedDate == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Select To Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                dtpFromDate1.Focus();
                return;
            }
            if (dtpFromDate1.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Proper Date.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                dtpFromDate1.Focus();
                return;
            }
            if (txtReferenceDoctor1.Text == null)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
               new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Doctor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                txtReferenceDoctor1.Focus();
                return;
            }
            if ((chkPaidDrPay.IsChecked == false) && (chkUnpaidDrPay.IsChecked == false))
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Select Doctor Pament Type.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                dtpFromDate1.Focus();
                return;
            }

            Indicator.Show();
            clsGetCompanyPaymentDetailsBizActionVO BizAction = new clsGetCompanyPaymentDetailsBizActionVO();

            BizAction.CompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            if (dtpFromDate1.SelectedDate != null)
            {
                dtpF = dtpFromDate1.SelectedDate.Value.Date.Date;
                BizAction.FromDate = Convert.ToDateTime(dtpF).ToString("MM/dd/yyyy");
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate.Value.Date.AddDays(1);
                BizAction.ToDate = Convert.ToDateTime(dtpT).ToString("MM/dd/yyyy");
            }
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.IsCompanyForm = false;
            BizAction.ServiceWise = false;
            BizAction.IsChild = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    Indicator.Close();
                    dgPaidDRList.ItemsSource = null;
                    if (lstDoctorPaymentDetails != null)
                    {
                        lstDoctorPaymentDetails.Clear();
                    }
                    lstDoctorPaymentDetails.AddRange(((clsGetCompanyPaymentDetailsBizActionVO)arg.Result).CompanyPaymentDetails);
                    System.Windows.Data.PagedCollectionView lstDoctorPaymentDetailsPageColl = new System.Windows.Data.PagedCollectionView(lstDoctorPaymentDetails);
                    lstDoctorPaymentDetailsPageColl.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Date"));
                    foreach (var item in lstDoctorPaymentDetails)
                    {
                        item.IsEnabled = true;
                        item.IsSelected = true;
                        if (chkPaidDrPay.IsChecked == true)
                        {
                            item.IsSelected = true;
                            item.IsEnabled = false;
                        }
                        else
                        {
                            item.IsSelected = true;
                            item.IsEnabled = true;
                        }

                    }

                    if (chkPaidDrPay.IsChecked == true)
                    {
                        dgPaidDRList.Columns[9].Visibility = Visibility.Collapsed;
                        dgPaidDRList.Columns[10].Visibility = Visibility.Visible;
                        dgPaidDRList.Columns[0].IsReadOnly = true;

                    }
                    else
                    {
                        dgPaidDRList.Columns[9].Visibility = Visibility.Visible;
                        dgPaidDRList.Columns[10].Visibility = Visibility.Collapsed;
                        dgPaidDRList.Columns[0].IsReadOnly = false;
                    }
                }
                else
                {
                    Indicator.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred While Adding Doctor Payment Details .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void btnPrintVoucher_Click(object sender, RoutedEventArgs e)
        {


            Nullable<DateTime> dtpF = null;
            Nullable<DateTime> dtpT = null;
            Nullable<DateTime> dtpP = null;
            bool IsExporttoExcel = false;
            bool chkToDate = true;
            string msgTitle = "";
            long UnitID = 0;
            UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (dtpFromDate.SelectedDate != null)
            {
                dtpF = dtpFromDate.SelectedDate.Value.Date.Date;
            }

            if (dtpToDate.SelectedDate != null)
            {
                dtpT = dtpToDate.SelectedDate.Value.Date.Date;
                if (dtpF.Value > dtpT.Value)
                {
                    dtpToDate.SelectedDate = dtpFromDate.SelectedDate;
                    dtpT = dtpF;
                    chkToDate = false;
                }
                else
                {
                    dtpP = dtpT;
                    //dtpT = dtpT.Value.Date.AddDays(1);
                    dtpT = dtpT.Value.AddDays(1);
                }
            }

            if (dtpT != null)
            {
                if (dtpF != null)
                {
                    dtpF = dtpFromDate.SelectedDate.Value.Date.Date;

                }
            }


            DateTime? FromDate = dtpF;
            DateTime? ToDate = dtpT;

            string URL = "../Reports/Administrator/DoctorPayment/NewDoctorPayment.aspx?ID=" + 0 + "&DoctorVoucherNumber=" + null + "&Excel=" + true + "&FromDate=" + FromDate.Value.ToString("M/d/yyyy") + "&ToDate=" + ToDate.Value.ToString("M/d/yyyy") + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");


        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            selectedamtforpayment = 0;
            if (SelectedBillList.Count > 0 || chkCheckAll.IsChecked == true)
            {
                selectedamtforpayment = Convert.ToDouble(String.Format("{0:0.000}", Convert.ToDecimal(txtTotalDocShare.Text)));

                DoctorPaymentModeWindow objDoctorPaymentNew = new DoctorPaymentModeWindow(selectedamtforpayment);

                objDoctorPaymentNew.OnSaveButton_Click += new RoutedEventHandler(objDoctorPaymentNew_OnSaveButton_Click);
                objDoctorPaymentNew.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Bill Of Patient To Make Payment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

            }
        }
        public double DoctorPaidAmount = 0;
        public double TOtalAmount = 0;
        public double BalanceAmount = 0;
        List<PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO> PaymentDetailsList = new List<PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO>();
        void objDoctorPaymentNew_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (((DoctorPaymentModeWindow)sender).DialogResult == true)
            {
                if (((DoctorPaymentModeWindow)sender).Payment != null)
                {

                    DoctorPaidAmount = ((DoctorPaymentModeWindow)sender).DoctorPaidAmount;
                    BalanceAmount = ((DoctorPaymentModeWindow)sender).BalanceAmount;
                    TOtalAmount = ((DoctorPaymentModeWindow)sender).TotalAmount;
                    PaymentDetailsList = ((DoctorPaymentModeWindow)sender).Payment.PaymentDetails;
                    string msgTitle = "";
                    string msgText = "Are You Sure \n You Make Payment For Selected Doctor ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.OnMessageBoxClosed += (arg) =>
                    {
                        if (arg == MessageBoxResult.Yes)
                        {
                            SaveDoctorpayment();
                        }
                    };
                    msgWD.Show();
                }
            }
            else
            {
                //////if (!flagFreezFromSearch)
                //////{
                //////    cmdSave.IsEnabled = true;
                //////}
                //////if (flagFreezFromSearch)
                //////{
                //////    ((clsBillVO)dgBillList.SelectedItem).IsFreezed = false;

                //////    chkFreezBill.IsChecked = false;
                //////    flagFreezFromSearch = false;

                //////}
            }
        }

        private void SaveDoctorpayment()
        {
            clsSaveDoctorPaymentDetailsBizActionVO BizAction = new clsSaveDoctorPaymentDetailsBizActionVO();
            long DoctorID = Convert.ToInt64(DoctorIDs);
            BizAction.DoctorID = DoctorID;
            BizAction.PaymentDate = DateTime.Today;
            BizAction.DoctorPaidAmount = DoctorPaidAmount;
            BizAction.TotalAmount = TOtalAmount;
            BizAction.BalanceAmount = BalanceAmount;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsPaymentDone = true;
            BizAction.DoctorServiceLinkingTypeID = DoctorShareLinkingTypeID;
            BizAction.DoctorInfo = new clsDoctorPaymentVO();
            if (DoctorAllBill.BillIDs != null && DoctorAllBill.BillUnitIDs != null)
            {
                BizAction.DoctorInfo.BillIDs = DoctorAllBill.BillIDs;
                BizAction.DoctorInfo.BillUnitIDs = DoctorAllBill.BillUnitIDs;
            }
            else
            {
                if (SelectedBillList != null && SelectedBillList.Count > 0)
                {
                    foreach (var item in SelectedBillList)
                    {
                        BizAction.DoctorInfo.BillIDs = BizAction.DoctorInfo.BillIDs + item.BillID;
                        BizAction.DoctorInfo.BillIDs = BizAction.DoctorInfo.BillIDs + ",";
                    }

                    if (BizAction.DoctorInfo.BillIDs.EndsWith(","))
                        BizAction.DoctorInfo.BillIDs = BizAction.DoctorInfo.BillIDs.Remove(BizAction.DoctorInfo.BillIDs.Length - 1, 1);
                }
                if (SelectedBillList != null && SelectedBillList.Count > 0)
                {
                    foreach (var item in SelectedBillList)
                    {
                        BizAction.DoctorInfo.BillUnitIDs = BizAction.DoctorInfo.BillUnitIDs + item.BillUnitID;
                        BizAction.DoctorInfo.BillUnitIDs = BizAction.DoctorInfo.BillUnitIDs + ",";
                    }

                    if (BizAction.DoctorInfo.BillUnitIDs.EndsWith(","))
                        BizAction.DoctorInfo.BillUnitIDs = BizAction.DoctorInfo.BillUnitIDs.Remove(BizAction.DoctorInfo.BillUnitIDs.Length - 1, 1);
                }
            }


            BizAction.PaymentDetailDetails = PaymentDetailsList;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Payment  Details Added Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    ClearControls();
                    FetchDoctorData();
                    clsSaveDoctorPaymentDetailsBizActionVO result = arg.Result as clsSaveDoctorPaymentDetailsBizActionVO;
                    long ID = DoctorID;
                    long DoctorPaymentID = result.DoctorInfo.ID;
                    DateTime? FromDate = null;
                    DateTime? ToDate = null;
                    string DoctorVoucherNumber = result.DoctorInfo.PaymentNo;
                    long UnitID = 0;
                    UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    string URL = "../Reports/Administrator/DoctorPayment/NewDoctorPayment.aspx?ID=" + ID + "&DoctorVoucherNumber=" + DoctorVoucherNumber + "&Excel=" + true + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&UnitID=" + UnitID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Saving Doctor Payment  Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void btnCalShare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ////////Indicator = new WaitIndicator();
                ////////Indicator.Show();
                ////////objAnimation.Invoke(RotationType.Backward);
                ////////FrontPanel.Visibility = Visibility.Collapsed;
                ////////BackPanel.Visibility = Visibility.Visible;
                ////////Indicator.Close();
                btnPayment.IsEnabled = true;
                btnCalShare1 = true;
                txtblkTotalDocShare.Visibility = Visibility.Visible;
                txtTotalDocShare.Visibility = Visibility.Visible;
                _flip.Invoke(RotationType.Forward);

                txtReferenceDoctor1.Text = "";
                txtReferenceDoctor2.Text = "";
                dgPaidDRList.ItemsSource = null;
                dgPaidDRList.UpdateLayout();
                dgPaidDRList1.ItemsSource = null;
                dgPaidDRList1.UpdateLayout();
                btnPrintVoucher.Visibility = Visibility.Collapsed;
                btnCalShare.Visibility = Visibility.Collapsed;
                btnCalShare.Content = "Finalize";
                //  btnCalShare.IsEnabled = false; 



            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    // btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Save":
                    // btnSave.IsEnabled = true;
                    btnCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Cancel":
                    //  btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    break;
                default:
                    break;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            this.DataContext = null;
            // ClearControl();
            //  FetchData();
            ClearControls();

        }

        void ClearControls()
        {
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = "";

            if (IsCancel == true)
            {
                mElement = (TextBlock)rootPage.FindName("SampleHeader");
                if (btnCalShare1 == false)
                {
                    btnPayment.IsEnabled = true;
                    dgPaidDRList1.ItemsSource = null;
                    dgPaidDRList1.UpdateLayout();
                    dgPaidDRList.ItemsSource = null;
                    dgPaidDRList.UpdateLayout();
                    txtReferenceDoctor1.Text = "";
                    txtReferenceDoctor2.Text = "";
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }
                else
                {
                    _flip.Invoke(RotationType.Backward);
                    btnCalShare1 = false;
                    btnCalShare.IsEnabled = true;
                    btnCalShare.Content = "Calculate Share";
                    txtblkTotalDocShare.Visibility = Visibility.Collapsed;
                    txtTotalDocShare.Visibility = Visibility.Collapsed;
                    txtTotalDocShare.Text = String.Empty;
                    SelectedBillList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
                    btnPayment.IsEnabled = false;
                    dgPaidDRList1.ItemsSource = null;
                    dgPaidDRList1.UpdateLayout();
                    dgPaidDRList.ItemsSource = null;
                    dgPaidDRList.UpdateLayout();
                    txtReferenceDoctor1.Text = "";
                    txtReferenceDoctor2.Text = "";
                    btnPrintVoucher.Visibility = Visibility.Visible;
                    btnCalShare.Visibility = Visibility.Visible;
                }
            }
            else
            {
                IsCancel = true;
            }
        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpFromDate.SelectedDate != null)
                fromdate = dtpFromDate.SelectedDate.Value;
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        string DoctorIDs = "";
        void OnExistingDoctorSaveButton_Click(object sender, RoutedEventArgs e)
        {

            DoctorIDs = "";
            DoctorShareLinkingTypeID = ((ReferalDoctorSearchChildWindow)sender).DoctorShareLinkingTypeID;
            if (((ReferalDoctorSearchChildWindow)sender).SelectedDoc != null && ((ReferalDoctorSearchChildWindow)sender).SelectedDoc.Count > 0)
            {
                foreach (var item in ((ReferalDoctorSearchChildWindow)sender).SelectedDoc)
                {
                    DoctorIDs = DoctorIDs + item.DoctorId;
                    DoctorIDs = DoctorIDs + ",";
                }

                if (DoctorIDs.EndsWith(","))
                    DoctorIDs = DoctorIDs.Remove(DoctorIDs.Length - 1, 1);

            }

            //DID = ((ReferalDoctorSearchChildWindow)sender).DoctorID;

            docnames = new StringBuilder();

            DIDs = new List<long>();
            foreach (var item in ((ReferalDoctorSearchChildWindow)sender).SelectedDoc)
            {
                docnames = docnames.Append(item.DoctorName);
                docnames = docnames.Append(" ");

                //docnames = docnames.Append(item.FirstName);
                //docnames = docnames.Append(" ");
                //docnames = docnames.Append(item.MiddleName);
                //docnames = docnames.Append(" ");
                //docnames = docnames.Append(item.LastName);

                DIDs.Add(item.DoctorId);

                item.IsSelected = true;
                docnames.Append(", ");
                if (calcShare == true)
                    break;
            }

            if (docnames.Length > 2)
            {
                docnames.Remove(docnames.Length - 2, 2);
            }

            txtReferenceDoctor2.IsEnabled = true;
            txtReferenceDoctor2.Text = Convert.ToString(docnames);

            if (calcShare == true)
            {
                txtReferenceDoctor1.IsEnabled = true;
                txtReferenceDoctor1.Text = Convert.ToString(docnames);
            }
            dgPaidDRList.ItemsSource = null;
            lstSelectedDoctor = ((ReferalDoctorSearchChildWindow)sender).SelectedDoc;
        }

        private void chkUnpaidDrPay_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkPaidDrPay_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PaidDoctorList()
        {

        }
        List<clsDoctorPaymentVO> DoctorPaymentServiceDetails = new List<clsDoctorPaymentVO>();
        List<clsDoctorPaymentVO> FinalServiceListForDoctorShareList = new List<clsDoctorPaymentVO>();
        List<clsDoctorPaymentVO> FinalServiceListForDoctorShareList1 = new List<clsDoctorPaymentVO>();

        void chDocPayment_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoctorPaymentServiceDetails = ((ChildDoctorPayment)sender).DataList1;
                FinalServiceListForDoctorShareList = ((ChildDoctorPayment)sender).FinalServiceListForDoctorShareExistingList;
                foreach (var item in FinalServiceListForDoctorShareList)
                {
                    FinalServiceListForDoctorShareList1.Add(item);
                }
                double TotalShareAmountForBill = 0;
                foreach (var item in DoctorPaymentServiceDetails)
                {
                    TotalShareAmountForBill = TotalShareAmountForBill + item.DoctorShareAmount;
                }
                foreach (var item in BackPanelDataList)
                {
                    if (item.BillNo == ((ChildDoctorPayment)sender).BillNumber)
                    {
                        item.TotalShareAmount = TotalShareAmountForBill;
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            if (ForFrontPanel == true)
            {
                dgDrUnPaidList.ItemsSource = null;
                dgDrUnPaidList.ItemsSource = BackPanelDataList;
            }
            else
            {
                dgPaidDRList1.ItemsSource = null;
                dgPaidDRList1.ItemsSource = BackPanelDataList;
            }
        }

        private void txtPayAmount_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtPayAmount_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void chkBillData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void frmDoctorPayment_Loaded(object sender, RoutedEventArgs e)
        {
            IsFromBetweenBillRangeChildWindow = false;
            btnPayment.IsEnabled = false;
            btnCalShare1 = false;
            dtpFromDate.SelectedDate = DateTime.Today.Date;
            dtpToDate.SelectedDate = DateTime.Today.Date;
            dtpFromDate1.SelectedDate = DateTime.Today.Date;
            dtpToDate1.SelectedDate = DateTime.Today.AddDays(30);
            lstDoctorPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
            lstSelectedDoctorPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
            FetchShareRange();
        }
        List<clsDoctorShareRangeList> DoctorShareRangeList = new List<clsDoctorShareRangeList>();
        private void FetchShareRange() 
        {
            clsGetDoctorShareRangeList BizActionVO = new clsGetDoctorShareRangeList();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetDoctorShareRangeList result = args.Result as clsGetDoctorShareRangeList;
                    DoctorShareRangeList = new List<clsDoctorShareRangeList>();
                    DoctorShareRangeList = result.DoctorShareRangeList;

                }
            };
            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();

        }

        private void lnkClearDoctor_Click(object sender, RoutedEventArgs e)
        {
            DID = 0;
            txtReferenceDoctor1.Text = "";
        }

        private void btnSearch_Click1(object sender, RoutedEventArgs e)
        {

        }

        private void lnkCreditBill_Click(object sender, RoutedEventArgs e)
        {

        }

        List<clsDoctorPaymentVO> DoctorPaymentAllBillList = new List<clsDoctorPaymentVO>();
        clsDoctorPaymentVO DoctorAllBill = new clsDoctorPaymentVO();
        private void FetchAllDAta()
        {

            if (chkCheckAll.IsChecked == true)
            {
                clsGetDoctorPaymentShareDetailsBizActionVO BizActionVO = new clsGetDoctorPaymentShareDetailsBizActionVO();
                BizActionVO.FromDate = (DateTime)dtpFromDate1.SelectedDate;
                BizActionVO.ToDate = (DateTime)dtpToDate.SelectedDate;
                BizActionVO.DoctorIds = DoctorIDs;
                BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionVO.IsForAllData = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetDoctorPaymentShareDetailsBizActionVO result = args.Result as clsGetDoctorPaymentShareDetailsBizActionVO;

                        if (result.DoctorInfo != null)
                        {
                            DoctorAllBill = result.DoctorInfo;
                        }
                        if (result.DoctorInfo != null)
                        {
                            totdoctorshare = result.DoctorInfo.TotalBillShareAmount;
                            if (DoctPay.Count > 0)
                            {
                                foreach (var item in DoctPay)
                                {
                                    totdoctorshare = totdoctorshare + item.DoctorShareAmount;
                                }
                            }
                            txtTotalDocShare.Text = "";
                            txtTotalDocShare.Text = Convert.ToString(String.Format("{0:0.000}", totdoctorshare));

                        }
                        if (IsFromBetweenBillRangeChildWindow == true)
                        {
                            SaveDoctorpayment();
                        }
                    }
                };
                client.ProcessAsync(BizActionVO, new clsUserVO());
                client.CloseAsync();
            }
        }


        private void FetchDoctorData()
        {
            WaitIndicator Wt = new WaitIndicator();
            Wt.Show();
            clsGetDoctorPaymentDetailListBizActionVO BizActionVO = new clsGetDoctorPaymentDetailListBizActionVO();
            BizActionVO.DoctorInfo = new clsDoctorPaymentVO();

            FrontPanelDataList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
            FrontPanelDataList.Clear();

            foreach (var doctorid in DIDs)
            {
                BizActionVO.DoctorInfo.FromDate = dtpFromDate.SelectedDate.Value.Date.Date;
                BizActionVO.DoctorInfo.ToDate = dtpToDate.SelectedDate.Value.Date.Date;
                BizActionVO.DoctorInfo.DoctorID = doctorid;
                BizActionVO.DoctorInfo.IsPaymentDone = true;
                BizActionVO.IsPagingEnabled = true;
                BizActionVO.StartRowIndex = BackPanelDataList.PageIndex * BackPanelDataList.PageSize;
                BizActionVO.MaximumRows = BackPanelDataList.PageSize;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetDoctorPaymentDetailListBizActionVO result = args.Result as clsGetDoctorPaymentDetailListBizActionVO;
                        if (result.DoctorDetails != null)
                        {
                            FrontPanelDataList.TotalItemCount = result.TotalRows;


                            if (OnlySettleBills == true)
                            {
                                foreach (var item in result.DoctorDetails)
                                {
                                    if (item.BalanceAmount > 0)
                                    {
                                        FrontPanelDataList.Add(item);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in result.DoctorDetails)
                                {
                                    FrontPanelDataList.Add(item);
                                }
                            }
                            DataListPageSize = 16;
                            pcv = new PagedCollectionView(FrontPanelDataList);
                            dgPaidDRList.ItemsSource = null;
                            dgPaidDRList.ItemsSource = pcv;
                        }
                    }
                };

                client.ProcessAsync(BizActionVO, new clsUserVO());
                client.CloseAsync();
                Wt.Close();
            }
        }
        public bool IsFromBetweenBillRangeChildWindow = false;
        private void FetchData()
        {
            clsGetDoctorPaymentShareDetailsBizActionVO BizActionVO = new clsGetDoctorPaymentShareDetailsBizActionVO();
            BizActionVO.FromDate = fromdate;//(DateTime)dtpFromDate1.SelectedDate;
            BizActionVO.ToDate = (DateTime)dtpToDate.SelectedDate;
            BizActionVO.DoctorIds = DoctorIDs;
            BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionVO.IsForBoth = true;
            BizActionVO.IsCreditBill = true;
            BizActionVO.IsSettleBill = true;
            BizActionVO.IsPagingEnabled = true;
            BizActionVO.StartRowIndex = BackPanelDataList.PageIndex * BackPanelDataList.PageSize;
            BizActionVO.MaximumRows = BackPanelDataList.PageSize;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetDoctorPaymentShareDetailsBizActionVO result = args.Result as clsGetDoctorPaymentShareDetailsBizActionVO;
                    if (result.DoctorInfoList != null)
                    {
                        BackPanelDataList.Clear();
                        BackPanelDataList.TotalItemCount = result.TotalRows;
                        BackPanelDataList.Clear();
                        foreach (var item in result.DoctorInfoList)
                        {
                            if (chkCheckAll.IsChecked == true)
                            {
                                item.IsChecked = true;
                                item.IsEnable = false;
                            }
                            else
                            {
                                item.IsEnable = true;
                            }
                            BackPanelDataList.Add(item);
                        }
                        DataListPageSize = 16;
                        if (SelectedBillList.Count > 0)
                        {
                            foreach (var item in SelectedBillList)
                            {
                                foreach (var item1 in BackPanelDataList)
                                {
                                    if (item.BillNo == item1.BillNo)
                                    {
                                        item1.IsChecked = true;
                                    }
                                }
                            }
                        }
                        if (ForFrontPanel == true)
                        {
                            dgDrUnPaidList.Visibility = Visibility.Visible;
                            pcv = new PagedCollectionView(BackPanelDataList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("DoctorName"));
                            dgDrUnPaidList.ItemsSource = null;
                            dgDrUnPaidList.ItemsSource = pcv;
                        }
                        else
                        {
                            foreach (var item in ChangedList)
                            {
                                foreach (var item1 in BackPanelDataList)
                                {
                                    if (item1.BillNo == item.BillNo)
                                    {
                                        item1.Color = item.Color;
                                        item1.TotalShareAmount = item.TotalShareAmount;
                                    }
                                }
                            }
                            pcv = new PagedCollectionView(BackPanelDataList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                            dgPaidDRList1.ItemsSource = null;
                            dgPaidDRList1.ItemsSource = pcv;
                        }
                    }
                    else
                    {
                        BackPanelDataList.Clear();
                        dgPaidDRList1.ItemsSource = null;
                    }
                    decimal TotalBillAmount = 0;
                    decimal TotalBillShareAmount = 0;
                    string RangeValue = "";
                    if (ForFrontPanel == false)
                    {
                        if (DoctorShareLinkingTypeID == 2)
                        {
                            foreach (var item in BackPanelDataList)
                            {
                                TotalBillAmount = TotalBillAmount + Convert.ToDecimal(String.Format("{0:0.000}", item.TotalBillAmount));
                                //TotalBillShareAmount = TotalBillShareAmount + Convert.ToDecimal(String.Format("{0:0.000}",item.TotalShareAmount));
                            }
                            foreach (var item in DoctorShareRangeList)
                            {
                                //if (TotalBillAmount > item.LowerLimit && TotalBillAmount < item.UpperLimit)
                                //{
                                //    RangeValue = item.LowerLimit + "-" + item.UpperLimit;
                                //    if (item.SharePercentage == 0)
                                //    {
                                //        TotalBillShareAmount = TotalBillShareAmount+item.ShareAmount;
                                //    }
                                //    else
                                //    {
                                //        TotalBillShareAmount = TotalBillShareAmount + (((TotalBillAmount - item.LowerLimit) * item.SharePercentage) / 100);
                                //    }

                                //}
                                //else
                                //{
                                //    if (item.SharePercentage == 0)
                                //    {
                                //        TotalBillShareAmount = TotalBillShareAmount+item.ShareAmount;
                                //    }
                                //    else
                                //    {
                                //        TotalBillShareAmount = TotalBillShareAmount+((item.UpperLimit - item.LowerLimit) * item.SharePercentage) / 100;
                                //    }
                                //}
                                if (item.LowerLimit < TotalBillAmount)
                                {
                                    if (item.SharePercentage == 0)
                                    {
                                        RangeValue = item.LowerLimit + "-" + item.UpperLimit;
                                        TotalBillShareAmount = TotalBillShareAmount + item.ShareAmount;
                                    }
                                    else
                                    {
                                        if (TotalBillAmount > item.UpperLimit)
                                        {
                                            RangeValue = item.LowerLimit + "-" + item.UpperLimit;
                                            TotalBillShareAmount = TotalBillShareAmount + ((item.UpperLimit - item.LowerLimit) * item.SharePercentage) / 100;
                                        }
                                        else
                                        {
                                            RangeValue = item.LowerLimit + "-" + item.UpperLimit;
                                            TotalBillShareAmount = TotalBillShareAmount + ((TotalBillAmount - item.LowerLimit) * item.SharePercentage) / 100;
                                        }
                                        // TotalBillShareAmount = TotalBillShareAmount + ((item.UpperLimit - item.LowerLimit) * item.SharePercentage) / 100;
                                    }
                                }

                            }
                            frmDoctorPaymentBillWiseBetweenRangeChildWindow objPaymentWin = new frmDoctorPaymentBillWiseBetweenRangeChildWindow();
                            objPaymentWin.DoctorShareRangeList = DoctorShareRangeList;

                            objPaymentWin.txtTotalBillAmount.Text = String.Format("{0:0.000}", Convert.ToString(TotalBillAmount));
                            objPaymentWin.txtDoctorShareAmount.Text = String.Format("{0:0.000}", TotalBillShareAmount);
                            objPaymentWin.txtPaidAmount.Text = String.Format("{0:0.000}", TotalBillShareAmount);
                            objPaymentWin.OnSaveButton_Click += new RoutedEventHandler(DoctorpaymentWin_OnSaveButton_Click);
                            objPaymentWin.OnCancelButton_Click += new RoutedEventHandler(DoctorpaymentWin_OnCancelButton_Click);
                            objPaymentWin.ApplicableOn.Content = RangeValue;
                            chkCheckAll.IsChecked = true;
                            IsFromBetweenBillRangeChildWindow = true;
                            objPaymentWin.Show();
                        }
                    }
                }
            };
            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();
        }
        void DoctorpaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).DialogResult == true)
            {
                if (((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).Payment != null)
                {
                    DoctorPaidAmount = ((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).DoctorPaidAmount;
                    BalanceAmount = ((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).BalanceAmount;
                    TOtalAmount = ((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).TotalAmount;
                    PaymentDetailsList = ((frmDoctorPaymentBillWiseBetweenRangeChildWindow)sender).Payment.PaymentDetails;
                    SelectAll();
                }
            }

        }
        void DoctorpaymentWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            btnPayment.IsEnabled = true;
            btnCalShare1 = true;
            txtblkTotalDocShare.Visibility = Visibility.Visible;
            txtTotalDocShare.Visibility = Visibility.Visible;
            txtReferenceDoctor1.Text = "";
            txtReferenceDoctor2.Text = "";
            dgPaidDRList.ItemsSource = null;
            dgPaidDRList.UpdateLayout();
            dgPaidDRList1.ItemsSource = null;
            dgPaidDRList1.UpdateLayout();
            btnPrintVoucher.Visibility = Visibility.Collapsed;
            btnCalShare.Visibility = Visibility.Collapsed;
            btnCalShare.Content = "Finalize";
        }
        private void FetchDataFrontPanel()
        {
            clsGetDoctorPaymentShareDetailsBizActionVO BizActionVO = new clsGetDoctorPaymentShareDetailsBizActionVO();
            foreach (var doctorid in DIDs)
            {
                BizActionVO.FromDate = (DateTime)dtpFromDate1.SelectedDate;
                BizActionVO.ToDate = (DateTime)dtpToDate.SelectedDate;
                BizActionVO.DoctorId = doctorid;
                BizActionVO.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                BizActionVO.IsForBoth = true;
                BizActionVO.IsCreditBill = true;
                BizActionVO.IsSettleBill = true;
                BizActionVO.IsPagingEnabled = true;
                BizActionVO.StartRowIndex = BackPanelDataList.PageIndex * BackPanelDataList.PageSize;
                BizActionVO.MaximumRows = BackPanelDataList.PageSize;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsGetDoctorPaymentShareDetailsBizActionVO result = args.Result as clsGetDoctorPaymentShareDetailsBizActionVO;

                        if (result.DoctorInfoList != null)
                        {
                            BackPanelDataList.TotalItemCount = result.TotalRows;
                            BackPanelDataList.Clear();
                            foreach (var item in result.DoctorInfoList)
                            {
                                if (chkCheckAll.IsChecked == true)
                                {
                                    item.IsChecked = true;
                                }
                                BackPanelDataList.Add(item);
                            }
                            DataListPageSize = 16;
                            if (SelectedBillList.Count > 0)
                            {
                                foreach (var item in SelectedBillList)
                                {
                                    foreach (var item1 in BackPanelDataList)
                                    {
                                        if (item.BillNo == item1.BillNo)
                                        {
                                            item1.IsChecked = true;
                                        }
                                    }
                                }
                            }

                            pcv = new PagedCollectionView(BackPanelDataList);
                            pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                            if (ForFrontPanel == true)
                            {
                                dgDrUnPaidList.Visibility = Visibility.Visible;
                                dgDrUnPaidList.ItemsSource = null;
                                dgDrUnPaidList.ItemsSource = pcv;
                            }
                            else
                            {
                                dgPaidDRList1.ItemsSource = null;
                                dgPaidDRList1.ItemsSource = pcv;
                            }
                        }
                        else
                        {
                            BackPanelDataList.Clear();
                            dgPaidDRList1.ItemsSource = null;
                        }
                    }
                };

                client.ProcessAsync(BizActionVO, new clsUserVO());
                client.CloseAsync();
            }
        }

        private void lnkBothSettleCreditBill_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void SettleBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lnkRemoveExistingDoctor_Click(object sender, RoutedEventArgs e)
        {
            dgPaidDRList1.ItemsSource = null;
            dgPaidDRList1.UpdateLayout();
            dgPaidDRList.ItemsSource = null;
            dgPaidDRList.UpdateLayout();
            txtReferenceDoctor1.Text = "";
        }

        private void btnShowDoctor_Click(object sender, RoutedEventArgs e)
        {
        }

        private void lnkExistingDoctorSearch_Click(object sender, RoutedEventArgs e)
        {
            calcShare = false;
            ReferalDoctorSearchChildWindow ExistingDoctorWin = new ReferalDoctorSearchChildWindow(calcShare);
            ExistingDoctorWin.OnSaveButton_Click += new RoutedEventHandler(OnExistingDoctorSaveButton_Click);
            ExistingDoctorWin.Show();
        }

        private void lnkExistingDoctorSearch_Click1(object sender, RoutedEventArgs e)
        {
            calcShare = true;
            ReferalDoctorSearchChildWindow ExistingDoctorWin = new ReferalDoctorSearchChildWindow(calcShare);
            ExistingDoctorWin.OnSaveButton_Click += new RoutedEventHandler(OnExistingDoctorSaveButton_Click);
            ExistingDoctorWin.Show();
        }

        //R
        //bACKpANEL
        PagedCollectionView pcv = null;
        private void btnShowDoctorShare_Click(object sender, RoutedEventArgs e)
        {
            if (txtReferenceDoctor1.Text != "")
            {
                txtTotalDocShare.Text = "";
                totdoctorshare = 0;
                TOtalAmount = 0;
                if ((DateTime)dtpFromDate1.SelectedDate < (DateTime)dtpToDate1.SelectedDate)
                {
                    ForFrontPanel = false;
                    FetchData();
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should Be Less Than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void lnkRemoveExistingDoctor1_Click(object sender, RoutedEventArgs e)
        {
            string str = docnames.ToString();
            if (str.Length > 0)
            {
                try
                {
                    str = str.Substring(0, str.LastIndexOf(','));

                    docnames.Clear();

                    docnames.Append(str);

                    txtReferenceDoctor2.Text = Convert.ToString(docnames);

                    lstSelectedDoctor.RemoveAt(lstSelectedDoctor.Count - 1);

                    dgPaidDRList.ItemsSource = null;

                    // dgPaidDRList.ItemsSource = lstSelectedDoctor;

                    dgPaidDRList.UpdateLayout();
                }
                catch (Exception ex)
                {
                    str = "";
                    txtReferenceDoctor2.Text = "";
                    docnames.Clear();
                    dgPaidDRList.ItemsSource = null;
                    dgPaidDRList.UpdateLayout();
                }
            }
        }
        //fRONT pANEL
        public bool ForFrontPanel = false;
        public bool OnlySettleBills = false;
        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            dgPaidDRList.ItemsSource = null;
            if (txtReferenceDoctor2.Text != "")
            {
                txtTotalDocShare.Text = "";
                totdoctorshare = 0;
                if ((DateTime)dtpFromDate.SelectedDate <= (DateTime)dtpToDate.SelectedDate)
                {

                    if (chkPaidDrPay.IsChecked == true)
                    {
                        OnlySettleBills = false;
                        dgDrUnPaidList.Visibility = Visibility.Collapsed;
                        dgPaidDRList.Visibility = Visibility.Visible;
                        FetchDoctorData();
                    }
                    else if (chkSettleDrPay.IsChecked == true)
                    {
                        OnlySettleBills = true;
                        dgDrUnPaidList.Visibility = Visibility.Collapsed;
                        dgPaidDRList.Visibility = Visibility.Visible;
                        FetchDoctorData();
                    }
                    else
                    {
                        ForFrontPanel = true;
                        dgDrUnPaidList.Visibility = Visibility.Visible;
                        dgPaidDRList.Visibility = Visibility.Collapsed;
                        FetchData();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", "From Date Should Be Less Than To Date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Doctor ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            SelectedDataList = null;
            SelectedDataList = new List<clsDoctorPaymentVO>();
            SelectedDataList.Add((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem);
            ChildDoctorPayment chDocPayment = new ChildDoctorPayment(SelectedDataList[0].DoctorID, SelectedDataList[0].TariffServiceID, SelectedDataList[0].BillNo, SelectedDataList[0].BillDate, SelectedDataList[0].UnitID, SelectedDataList[0].PatientName);
            chDocPayment.DeepCopyDataList = DoctPay;

            chDocPayment.FinalServiceListForDoctorShareExistingList = FinalServiceListForDoctorShareList1;
            chDocPayment.OnSaveButton_Click += new RoutedEventHandler(chDocPayment_OnSaveButton_Click);
            chDocPayment.OnCancelButtonClick += new RoutedEventHandler(chDocPayment_OnCancelButton_Click);
            chDocPayment.Show();
        }
        public List<clsDoctorPaymentVO> DoctPay = new List<clsDoctorPaymentVO>();
        PagedSortableCollectionView<clsDoctorPaymentVO> ChangedList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
        void chDocPayment_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            // float DoctorShare = 0;
            DoctPay = ((ChildDoctorPayment)sender).DeepCopyDataList;
            foreach (var item in DoctPay)
            {
                foreach (var item1 in BackPanelDataList)
                {
                    if (item.BillNo == item1.BillNo)
                    {
                        item1.TotalShareAmount = item1.TotalShareAmount + item.DoctorShareAmount;
                        if (item.DoctorShareAmount > 0)
                        {
                            item1.SelectedColor = "Green";
                        }
                        else
                        {
                            item1.SelectedColor = null;
                        }
                        ChangedList.Add(item1);

                    }
                }
            }
            pcv = new PagedCollectionView(BackPanelDataList);
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
            dgPaidDRList1.ItemsSource = null;
            dgPaidDRList1.ItemsSource = pcv;

        }


        private void cmdViewBill_Click(object sender, RoutedEventArgs e)
        {
            //SelectedDoctor = null;
            //SelectedDoctor = new List<clsDoctorPaymentVO>();
            //SelectedDoctor.Add((clsDoctorPaymentVO)dgPaidDRList.SelectedItem);
            frmChildDoctorPayment chDocPayment = new frmChildDoctorPayment();
            chDocPayment.DoctorBillInfo = (clsDoctorPaymentVO)dgPaidDRList.SelectedItem;
            chDocPayment.Show();
        }

        public bool IsAllChecked = false;
        private void chkShare_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                SelectAll();
            }
            else
            {
                IsAllChecked = false;
                foreach (var item in BackPanelDataList)
                {
                    item.IsChecked = false;
                    item.IsEnable = true;
                }
                SelectedBillList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
                pcv = new PagedCollectionView(BackPanelDataList);
                pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                TOtalAmount = 0;
                dgPaidDRList1.ItemsSource = null;
                dgPaidDRList1.ItemsSource = pcv;
                DoctorAllBill = new clsDoctorPaymentVO();
                txtTotalDocShare.Text = Convert.ToString("0.000");
            }
        }
        private void SelectAll()
        {
            IsAllChecked = true;
            foreach (var item in BackPanelDataList)
            {
                item.IsChecked = true;
                item.IsEnable = false;
            }
            pcv = new PagedCollectionView(BackPanelDataList);
            pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
            dgPaidDRList1.ItemsSource = null;
            dgPaidDRList1.ItemsSource = pcv;
            FetchAllDAta();
        }

        private void chkShare_Checked_1(object sender, RoutedEventArgs e)
        {

        }

        PagedSortableCollectionView<clsDoctorPaymentVO> SelectedBillList = new PagedSortableCollectionView<clsDoctorPaymentVO>();
        PagedSortableCollectionView<clsDoctorPaymentVO> RemovedBillList = new PagedSortableCollectionView<clsDoctorPaymentVO>();

        private void chkBillChecked_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                foreach (var item in BackPanelDataList)
                {
                    if (((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem).BillNo == item.BillNo)
                    {
                        item.IsChecked = true;

                        item.BalanceAmount = ((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem).BalanceAmount;
                        TOtalAmount = TOtalAmount + Convert.ToDouble(String.Format("{0:0.000}", item.TotalShareAmount));

                        SelectedBillList.Add(item);
                    }
                }
                txtTotalDocShare.Text = "";
                txtTotalDocShare.Text = String.Format("{0:0.000}", TOtalAmount);
                //pcv = new PagedCollectionView(BackPanelDataList);
                //pcv.GroupDescriptions.Add(new PropertyGroupDescription("UnitName"));
                //dgPaidDRList1.ItemsSource = null;
                //dgPaidDRList1.ItemsSource = pcv;
            }
            else
            {
                TOtalAmount = 0;
                TOtalAmount = Convert.ToDouble(txtTotalDocShare.Text);
                RemovedBillList = SelectedBillList;
                if (SelectedBillList.Count == 0)
                {
                    if ((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem != null)
                    {
                        TOtalAmount = TOtalAmount - ((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem).TotalShareAmount;
                    }

                }
                for (int i = 0; i < RemovedBillList.Count; i++)
                {
                    if ((RemovedBillList[i].BillNo == ((clsDoctorPaymentVO)dgPaidDRList1.SelectedItem).BillNo) && RemovedBillList.Count > 0)
                    {

                        TOtalAmount = TOtalAmount - RemovedBillList[i].TotalShareAmount;
                        SelectedBillList.Remove(RemovedBillList[i]);
                    }
                }
                txtTotalDocShare.Text = "";
                //  txtTotalDocShare.Text = TOtalAmount.ToString();

                txtTotalDocShare.Text = (String.Format("{0:0.000}", TOtalAmount));

            }
        }

        private void cmdView1_Click(object sender, RoutedEventArgs e)
        {
            ForFrontPanel = true;
            SelectedDataList = null;
            SelectedDataList = new List<clsDoctorPaymentVO>();
            SelectedDataList.Add((clsDoctorPaymentVO)dgDrUnPaidList.SelectedItem);
            ChildDoctorPayment chDocPayment = new ChildDoctorPayment(SelectedDataList[0].DoctorID, SelectedDataList[0].TariffID, SelectedDataList[0].BillNo, SelectedDataList[0].BillDate, SelectedDataList[0].UnitID, SelectedDataList[0].PatientName);
            chDocPayment.FinalServiceListForDoctorShareExistingList = FinalServiceListForDoctorShareList1;
            chDocPayment.OnSaveButton_Click += new RoutedEventHandler(chDocPayment_OnSaveButton_Click);
            chDocPayment.Show();
        }

        private void dgPaidDRList1_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            clsDoctorPaymentVO item = (clsDoctorPaymentVO)e.Row.DataContext;
            if (item.SelectedColor != null)
                e.Row.Background = new SolidColorBrush(System.Windows.Media.Colors.Green);

            else
                e.Row.Background = null;
        }

        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {

            selectedamtforpayment = ((clsDoctorPaymentVO)dgPaidDRList.SelectedItem).BalanceAmount;
            DoctorPaymentModeWindow objDoctorPaymentNew = new DoctorPaymentModeWindow(selectedamtforpayment);
            objDoctorPaymentNew.OnSaveButton_Click += new RoutedEventHandler(objDoctorSettlePaymentNew_OnSaveButton_Click);
            objDoctorPaymentNew.Show();
        }

        void objDoctorSettlePaymentNew_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (((DoctorPaymentModeWindow)sender).DialogResult == true)
            {
                if (((DoctorPaymentModeWindow)sender).Payment != null)
                {
                    DoctorPaidAmount = ((DoctorPaymentModeWindow)sender).DoctorPaidAmount;
                    BalanceAmount = ((DoctorPaymentModeWindow)sender).BalanceAmount;
                    TOtalAmount = ((DoctorPaymentModeWindow)sender).TotalAmount;
                    PaymentDetailsList = ((DoctorPaymentModeWindow)sender).Payment.PaymentDetails;
                    //string msgTitle = "";
                    //string msgText = "Are You Sure \n You Settle Payment For Selected Doctor ?";
                    //MessageBoxControl.MessageBoxChildWindow msgWD =
                    //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    //msgWD.OnMessageBoxClosed += (arg) =>
                    //{
                    //    if (arg == MessageBoxResult.Yes)
                    //    {
                    //        SettleDoctorPayment();
                    //    }
                    //};
                    //msgWD.Show();
                    SettleDoctorPayment();
                }
            }

        }
        private void SettleDoctorPayment()
        {
            clsSaveDoctorSettlePaymentDetailsBizActionVO BizAction = new clsSaveDoctorSettlePaymentDetailsBizActionVO();
            BizAction.DoctorPaymentInfo = new clsDoctorPaymentVO();
            // BizAction.DoctorPaymentInfo.DoctorPaymentID = ((clsDoctorPaymentVO)dgPaidDRList.SelectedItem).DoctorPaymentID;
            BizAction.VoucherNo = ((clsDoctorPaymentVO)dgPaidDRList.SelectedItem).PaymentNo;
            long DoctorID = Convert.ToInt64(DoctorIDs);
            BizAction.DoctorID = DoctorID;
            BizAction.PaymentDate = DateTime.Today;
            BizAction.DoctorPaidAmount = DoctorPaidAmount;
            BizAction.TotalAmount = TOtalAmount;
            BizAction.BalanceAmount = BalanceAmount;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsPaymentDone = true;
            BizAction.DoctorServiceLinkingTypeID = DoctorShareLinkingTypeID;
            //   BizAction.DoctorPaymentInfo = new clsDoctorPaymentVO();


            BizAction.DoctorPaymentDetails = PaymentDetailsList;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                SetCommandButtonState("Save");

                if (arg.Error == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Doctor Payment  Details Settled Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    btnCalShare1 = true;
                    ClearControls();
                    FetchDoctorData();
                    //clsSaveDoctorPaymentDetailsBizActionVO result = arg.Result as clsSaveDoctorPaymentDetailsBizActionVO;
                    //long ID = DoctorID;
                    //long DoctorPaymentID = result.DoctorInfo.ID;
                    //string DoctorVoucherNumber = result.DoctorInfo.PaymentNo;
                    //string URL = "../Reports/Administrator/DoctorPayment/NewDoctorPayment.aspx?ID=" + ID + "&DoctorPaymentID=" + DoctorPaymentID + "&DoctorVoucherNumber= " + DoctorVoucherNumber + "&Excel=" + true;
                    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured While Saving Doctor Payment  Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            long ID = ((clsDoctorPaymentVO)dgPaidDRList.SelectedItem).DoctorID;
            //  long DoctorPaymentID = result.DoctorInfo.ID;
            DateTime? FromDate = null;
            DateTime? ToDate = null;
            string DoctorVoucherNumber = ((clsDoctorPaymentVO)dgPaidDRList.SelectedItem).PaymentNo;
            long UnitID = 0;
            UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            string URL = "../Reports/Administrator/DoctorPayment/NewDoctorPayment.aspx?ID=" + ID + "&DoctorVoucherNumber=" + DoctorVoucherNumber + "&Excel=" + true + "&FromDate=" + FromDate + "&ToDate=" + ToDate + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void dtpFromDate1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dtpFromDate1.SelectedDate != null)
                fromdate = dtpFromDate1.SelectedDate.Value;
        }
    }
}
