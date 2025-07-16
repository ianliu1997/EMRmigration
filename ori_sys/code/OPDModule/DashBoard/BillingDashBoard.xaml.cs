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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Windows.Browser;

namespace OPDModule.DashBoard
{
    public partial class BillingDashBoard : UserControl
    {
        clsBillVO SelectedBill { get; set; }
        #region "Paging"
        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsBillVO> DataList_Pharamcy { get; private set; }
        public PagedSortableCollectionView<clsBillVO> DataList_IVF { get; private set; }
        public PagedSortableCollectionView<clsBillVO> DataList_USG { get; private set; }
     
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

        public int DataListPageSize_Pharmacy
        {
            get
            {
                return DataList_Pharamcy.PageSize;
            }
            set
            {
                if (value == DataList_Pharamcy.PageSize) return;
                DataList_Pharamcy.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        public int DataListPageSize_IVF
        {
            get
            {
                return DataList_IVF.PageSize;
            }
            set
            {
                if (value == DataList_IVF.PageSize) return;
                DataList_IVF.PageSize = value;
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        public int DataListPageSize_USG
        {
            get
            {
                return DataList_USG.PageSize;
            }
            set
            {
                if (value == DataList_USG.PageSize) return;
                DataList_USG.PageSize = value;
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
            FillBillSearchList_clinical();

        }
        void DataList_OnRefresh_Pharmcy(object sender, RefreshEventArgs e)
        {
            FillBillSearchList_Pharamcy();
        }
        void DataList_OnRefresh_IVF(object sender, RefreshEventArgs e)
        {
            FillBillSearchList_Pharamcy();
        }
        void DataList_OnRefresh_USG(object sender, RefreshEventArgs e)
        {
            FillBillSearchList_Pharamcy();
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
    

        #endregion

        public BillingDashBoard()
        {
            InitializeComponent();
            dtpDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            //   =======================CLINICAL=====================================
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 5;
            DataPagerClinicalBill.PageSize = DataListPageSize;
            DataPagerClinicalBill.Source = DataList;
            //========================IVF=====================================
            DataList_IVF = new PagedSortableCollectionView<clsBillVO>();
            DataList_IVF.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh_IVF);
            DataListPageSize = 5;
            DataPagerClinicalBill.PageSize = DataListPageSize_IVF;
            DataPagerClinicalBill.Source = DataList_Pharamcy;
            // ==============================================================

            //======================Pharmacy=======================================
            DataList_Pharamcy = new PagedSortableCollectionView<clsBillVO>();
            DataList_Pharamcy.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh_Pharmcy);
            DataListPageSize = 5;
            DataPagerClinicalBill.PageSize = DataListPageSize_Pharmacy;
            DataPagerClinicalBill.Source = DataList_Pharamcy;
            // ==============================================================

            //=====================USG========================================
            DataList_USG = new PagedSortableCollectionView<clsBillVO>();
            DataList_USG.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh_USG);
            DataListPageSize = 5;
            DataPagerClinicalBill.PageSize = DataListPageSize_USG;
            DataPagerClinicalBill.Source = DataList_USG;
            // ==============================================================
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            dtpDate.SelectedDate = DateTime.Now;
            dtpToDate.SelectedDate = DateTime.Now;
            FillClinic();
            FillBillSearchList_clinical();
            FillBillSearchList_Pharamcy();
            FillUSG_List();
            FillIVF_List();
        }

        private void FillClinic()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedItem = objList[0];

                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ScheduleForETOPU_Maximized(object sender, EventArgs e)
        {

        }

        private void FillBillSearchList_clinical()
        {
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();

            if (dtpDate.SelectedDate != null)
                BizAction.FromDate = dtpDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;   
            BizAction.FirstName = txtFirstName.Text;       
            BizAction.LastName = txtLastName.Text;

            BizAction.BillType = (BillTypes?)1;
           
            if (cmbClinic.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;

                    DataList.Clear();
                    DataList.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {

                        foreach (var item in result.List)
                        {
                            DataList.Add(item);
                        }
                    }
                    grdClinicalBill.ItemsSource = null;
                    grdClinicalBill.ItemsSource = DataList;

                    DataPagerClinicalBill.Source = null;
                    DataPagerClinicalBill.PageSize = BizAction.MaximumRows;
                    DataPagerClinicalBill.Source = DataList;
                    // checkFreezColumn = true;      
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillBillSearchList_Pharamcy()
        {
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
          
            if (dtpDate.SelectedDate != null)
                BizAction.FromDate = dtpDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;

            BizAction.BillType = (BillTypes?)2;

            if (cmbClinic.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList_Pharamcy.PageIndex * DataList_Pharamcy.PageSize;
            BizAction.MaximumRows = DataList_Pharamcy.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;

                    DataList_Pharamcy.Clear();
                    DataList_Pharamcy.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            DataList_Pharamcy.Add(item);
                        }
                    }
                    grdPharmcyBill.ItemsSource = null;
                    grdPharmcyBill.ItemsSource = DataList_Pharamcy;

                    DataPagerPharamcy.Source = null;
                    DataPagerPharamcy.PageSize = BizAction.MaximumRows;
                    DataPagerPharamcy.Source = DataList_Pharamcy;                
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public void FillIVF_List()
        {
            indicator.Show();
            clsGetBillSearch_IVF_List_DashBoardBizActionVO BizAction = new clsGetBillSearch_IVF_List_DashBoardBizActionVO();
            if (dtpDate.SelectedDate != null)
                BizAction.FromDate = dtpDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;

           BizAction.BillType = 4;

            if (cmbClinic.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList_IVF.PageIndex * DataList_IVF.PageSize;
            BizAction.MaximumRows = DataList_IVF.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearch_IVF_List_DashBoardBizActionVO result = e.Result as clsGetBillSearch_IVF_List_DashBoardBizActionVO;

                    DataList_IVF.Clear();
                    DataList_IVF.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            DataList_IVF.Add(item);
                        }
                    }
                    grdIvf.ItemsSource = null;
                    grdIvf.ItemsSource = DataList_IVF;

                    grdPgrIvf.Source = null;
                    grdPgrIvf.PageSize = BizAction.MaximumRows;
                    grdPgrIvf.Source = DataList_IVF;
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        public void FillUSG_List()
        {
            indicator.Show();
            clsGetBillSearch_USG_List_DashBoardBizActionVO BizAction = new clsGetBillSearch_USG_List_DashBoardBizActionVO();
            if (dtpDate.SelectedDate != null)
                BizAction.FromDate = dtpDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.LastName = txtLastName.Text;

            BizAction.BillType =3;

            if (cmbClinic.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList_USG.PageIndex * DataList_USG.PageSize;
            BizAction.MaximumRows = DataList_USG.PageSize;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearch_USG_List_DashBoardBizActionVO result = e.Result as clsGetBillSearch_USG_List_DashBoardBizActionVO;

                    DataList_USG.Clear();
                    DataList_USG.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            DataList_USG.Add(item);
                        }
                    }
                    grdUltraSoundBill.ItemsSource = null;
                    grdUltraSoundBill.ItemsSource = DataList_USG;

                    DataPagerUltraSoundBill.Source = null;
                    DataPagerUltraSoundBill.PageSize = BizAction.MaximumRows;
                    DataPagerUltraSoundBill.Source = DataList_USG;
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            FillBillSearchList_clinical();
            FillBillSearchList_Pharamcy();
            FillUSG_List();
            FillIVF_List();
        }

        private void ScheduleForETOPU_Minimized(object sender, EventArgs e)
        {

        }

        private void grdPharmcyBill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PatientQueue_Maximized(object sender, EventArgs e)
        {

        }

        private void hlCloseVisit_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void ClinicalBill_Minimized(object sender, EventArgs e)
        {

        }

        private void ClinicalBill_Maximized(object sender, EventArgs e)
        {

        }

        private void grdClinicalBill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Appointments_Maximized(object sender, EventArgs e)
        {

        }

        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {

        }
             
        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToTitleCase();
            }
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtLastName.Text.Trim()))
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToTitleCase();
            }
        }

        private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (grdClinicalBill.SelectedItem != null)
            {
                frmReceiptList receiptWin = new frmReceiptList();
                receiptWin.BillUnitID = ((clsBillVO)grdClinicalBill.SelectedItem).UnitID;
                receiptWin.BillID = ((clsBillVO)grdClinicalBill.SelectedItem).ID;
                receiptWin.BillNo = ((clsBillVO)grdClinicalBill.SelectedItem).BillNo;
                receiptWin.Show();
            }          
        }
        private void btnReceipt_pharamcy_Click(object sender, RoutedEventArgs e)
        {
            if (grdPharmcyBill.SelectedItem != null)
            {
                frmReceiptList receiptWin = new frmReceiptList();
                receiptWin.BillUnitID = ((clsBillVO)grdPharmcyBill.SelectedItem).UnitID;
                receiptWin.BillID = ((clsBillVO)grdPharmcyBill.SelectedItem).ID;
                receiptWin.BillNo = ((clsBillVO)grdPharmcyBill.SelectedItem).BillNo;
                receiptWin.Show();
            }
        }   

        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {   
            if (grdClinicalBill.SelectedItem != null)
                {
                    if (((clsBillVO)grdClinicalBill.SelectedItem).IsFreezed == true)
                        SettleBill();
                }
                else
                {
                    string msgText = "Only Freezed Bills Can Be Settled ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.Show();
                }    
        }

        private void btnSettle_pharmcy_Click(object sender, RoutedEventArgs e)
        {
            if (grdPharmcyBill.SelectedItem != null)
            {
                if (((clsBillVO)grdPharmcyBill.SelectedItem).IsFreezed == true)
                    SettleBill();
            }
            else
            {
                string msgText = "Only Freezed Bills Can Be Settled ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.Show();
            }
        }

        void SettleBill()
        {
            if (grdClinicalBill.SelectedItem != null && ((clsBillVO)grdClinicalBill.SelectedItem).IsFreezed == true)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)grdClinicalBill.SelectedItem;           
                string msgText = "Are You Sure \n You Want To Settle The Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid = true;
                        if (isValid)
                        {
                            PaymentWindow SettlePaymentWin = new PaymentWindow();
                            if (SelectedBill.BalanceAmountSelf > 0)
                                SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;

                            SettlePaymentWin.Initiate("Bill");

                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();
                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
                            SettlePaymentWin.Show();
                        }
                        else
                        {
                            // InitialiseForm();
                        }
                    }
                    else
                    {
                        // InitialiseForm();
                    }
                };
                msgWD.Show();
            }
            else
            {
            if (grdPharmcyBill.SelectedItem != null && ((clsBillVO)grdPharmcyBill.SelectedItem).IsFreezed == true)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)grdPharmcyBill.SelectedItem;
                string msgText = "Are you sure you want to Settle the Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid = true;
                        if (isValid)
                        {
                            PaymentWindow SettlePaymentWin = new PaymentWindow();
                            if (SelectedBill.BalanceAmountSelf > 0)
                                SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;

                            SettlePaymentWin.Initiate("Bill");

                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_Phrmcy_OnSaveButton_Click);
                            SettlePaymentWin.Show();
                        }
                        else
                        {                          
                        }
                    }
                    else
                    {
                    }
                };
                msgWD.Show();
            }
        }
        }

        long PaymentID { get; set; }

        void SettlePaymentWin_Phrmcy_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        Indicatior.Show();
                        clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                        clsPaymentVO pPayDetails = new clsPaymentVO();

                        pPayDetails = ((PaymentWindow)sender).Payment;
                        pPayDetails.IsBillSettlement = true;
                        BizAction.Details = pPayDetails;

                        BizAction.Details.BillID = SelectedBill.ID;
                        BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                        BizAction.Details.Date = DateTime.Now;
                        BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                    PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                                clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                                mybillPayDetails.Details = SelectedBill;

                                mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
                                if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client1.ProcessCompleted += (s1, arg1) =>
                                {
                                    Indicatior.Close();
                                    if (arg1.Error == null && arg1.Result != null)
                                    {
                                        if (grdPharmcyBill.ItemsSource != null)
                                        {
                                            grdPharmcyBill.ItemsSource = null;
                                            grdPharmcyBill.ItemsSource = DataList;
                                        }
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWD.OnMessageBoxClosed += (re) =>
                                        {
                                            if (re == MessageBoxResult.OK)
                                            {
                                                PrintSettleBill(((clsBillVO)grdPharmcyBill.SelectedItem).ID, ((clsBillVO)grdPharmcyBill.SelectedItem).UnitID, PaymentID);
                                            }
                                        };
                                        msgWD.Show();
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while updating Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgWD.Show();
                                    }
                                };
                                Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client1.CloseAsync();
                            }
                            else
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while Saving Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgWD.Show();
                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();

                    }
                }
                else
                {
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }

        }

        void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        Indicatior.Show();
                        clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                        clsPaymentVO pPayDetails = new clsPaymentVO();

                        pPayDetails = ((PaymentWindow)sender).Payment;
                        pPayDetails.IsBillSettlement = true;
                        BizAction.Details = pPayDetails;

                        BizAction.Details.BillID = SelectedBill.ID;
                        BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                        BizAction.Details.Date = DateTime.Now;
                        BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;
                        
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                    PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                                clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                                mybillPayDetails.Details = SelectedBill;

                                mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
                                if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client1.ProcessCompleted += (s1, arg1) =>
                                {

                                    Indicatior.Close();
                                    if (arg1.Error == null && arg1.Result != null)
                                    {
                                        if (grdClinicalBill.ItemsSource != null)
                                        {
                                            grdClinicalBill.ItemsSource = null;
                                            grdClinicalBill.ItemsSource = DataList;
                                        }
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                      msgWD.OnMessageBoxClosed += (re) =>
                                        {
                                            if (re == MessageBoxResult.OK)
                                            {
                                                PrintSettleBill(((clsBillVO)grdClinicalBill.SelectedItem).ID, ((clsBillVO)grdClinicalBill.SelectedItem).UnitID, PaymentID);
                                            }
                                        };
                                        msgWD.Show();
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while updating Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                       msgWD.Show();
                                    }
                                };
                                Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client1.CloseAsync();
                            }
                            else
                            {
                                Indicatior.Close();
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while Saving Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgWD.Show();
                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();

                    }
                }
                else
                {                    
                }
            }
            catch (Exception)
            {   
                Indicatior.Close();
            }

        }
            
        private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID)
        {
            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (grdClinicalBill.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)grdClinicalBill.SelectedItem;

                if (SelectedBill.IsFreezed == true)
                {
                    if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                    {
                        PrintBill(((clsBillVO)grdClinicalBill.SelectedItem).ID, ((clsBillVO)grdClinicalBill.SelectedItem).UnitID);
                    }
                    else
                    {
                        PrintPharmacyBill(((clsBillVO)grdPharmcyBill.SelectedItem).ID, ((clsBillVO)grdPharmcyBill.SelectedItem).UnitID);
                    }
                }
            }           
            
        }


        private void btnPrint_Phrmcy_Click(object sender, RoutedEventArgs e)
        {
            if (grdPharmcyBill.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)grdPharmcyBill.SelectedItem;

                if (SelectedBill.IsFreezed == true)
                {
                    if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                    {
                        PrintPharmacyBill(((clsBillVO)grdPharmcyBill.SelectedItem).ID, ((clsBillVO)grdPharmcyBill.SelectedItem).UnitID);
                    }
                    else
                    {
                        PrintBill(((clsBillVO)grdClinicalBill.SelectedItem).ID, ((clsBillVO)grdClinicalBill.SelectedItem).UnitID);                     
                    }
                }
            }
        }

        private void PrintPharmacyBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {              
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID+"&ReportID=" +2; 
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void PrintBill(long iBillId, long iUnitID)
        {
            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
          

    }
}
