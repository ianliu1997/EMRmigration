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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using OPDModule;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using CIMS.Forms;
namespace PalashDynamics.IPD
{ 

    public partial class frmIPDBillSearch : UserControl
    {
        clsBillVO SelectedBill { get; set; }
        int isInline = 0;

        #region "Paging"


        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }

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
            FillBillSearchList();

        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        /// 
        private void FillBillSearchList()
        {       
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();      
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;
            BizAction.BillNO = txtBillNO.Text;
            BizAction.IPDNO = txtOPDNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.MiddleName = txtMiddleName.Text;
            BizAction.LastName = txtLastName.Text;
            BizAction.IsIPDBillList = true;

            if (cmbBillStatus.SelectedItem != null)
            {
                BizAction.BillStatus = (Int16)(((MasterListItem)cmbBillStatus.SelectedItem).ID);
            }
            if (cmbBillTypeClPh.SelectedItem != null)
            {
                BizAction.BillType = (BillTypes)((MasterListItem)cmbBillTypeClPh.SelectedItem).ID;    
            }
          
            if (cmbUnit.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
                      
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


            BizAction.Opd_Ipd_External = 1;          
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
                    dgBillList.ItemsSource = null;
                    dgBillList.ItemsSource = DataList;

                    dgDataPager.Source = null;
                    dgDataPager.PageSize = BizAction.MaximumRows;
                    dgDataPager.Source = DataList;


                    // checkFreezColumn = true;      
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }



        #endregion

        private void FillUnitList()
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

                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;
                    cmbUnit.SelectedItem = objList[0];

                }
          
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    cmbUnit.IsEnabled = true;
                    cmbUnit.SelectedValue = (long)0;
                }
                else
                {
                    cmbUnit.IsEnabled = false;
                    cmbUnit.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void FillBillTypeClPhList()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "--All--"));
            objList.Add(new MasterListItem(1, "Clinical Bill"));
            objList.Add(new MasterListItem(2, "Pharmacy Bill"));

            cmbBillTypeClPh.ItemsSource = null;
            cmbBillTypeClPh.ItemsSource = objList;
            cmbBillTypeClPh.SelectedItem = objList[0];
        }
        private void FillBillTypeList()
        {

            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- All --"));
            objList.Add(new MasterListItem(1, "Settled"));
            objList.Add(new MasterListItem(2, "Unsettled"));

            cmbBillStatus.ItemsSource = null;
            cmbBillStatus.ItemsSource = objList;
            cmbBillStatus.SelectedItem = objList[0];


        }

        private void FillPrintFormat()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_ReportPrintFormat;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }

                    cmbPrintFormat.ItemsSource = null;
                    cmbPrintFormat.ItemsSource = objList;


                    cmbPrintFormat.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;


                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception)
            {


                throw;
            }

        }

        public frmIPDBillSearch()
        {
            InitializeComponent();
            dtpFromDate.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;

            //============================================================================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsBillVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //============================================================================================================
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FillUnitList();
            FillBillTypeList();
            FillBillSearchList();
            FillPrintFormat();
            fillOpdIpdCombo();
            FillBillTypeClPhList();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Serach_Bill();
        }

        public void Serach_Bill()
        {
            bool res = true;
            ToolTip ToolTipControl = new ToolTip();


            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate != null)
            {

                if (dtpFromDate.SelectedDate > dtpToDate.SelectedDate)
                {

                    dtpFromDate.SetValidation("From Date should be less than To Date");
                    dtpFromDate.RaiseValidationError();
                    //dtpFromDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    //ToolTipControl.Content = "From Date should be less than To Date";
                    //ToolTipService.SetToolTip(dtpFromDate, ToolTipControl);
                    string strMsg = "From Date should be less than To Date";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    dtpFromDate.Focus();
                    res = false;
                }
                else
                {
                    // dtpFromDate.BorderBrush = myBrush;
                    dtpFromDate.ClearValidationError();
                }
            }
            if (dtpFromDate.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Please Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
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

                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDate.Focus();
                res = false;
            }
            else
            {
                dtpFromDate.ClearValidationError();
            }

            if (res)
            {
                dgDataPager.PageIndex = 0;
                FillBillSearchList();
            }
        }

        private void dgBillList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;
          
                if (SelectedBill.IsFreezed == true)
                {
                    if (((clsBillVO)dgBillList.SelectedItem).IsPackageServiceInclude == true)
                    {

                        string msgText = "Are You Sure  Want To Print Details Bill ?";
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgWD.OnMessageBoxClosed += (arg) =>
                        {
                            if (arg == MessageBoxResult.Yes)
                            {
                                isInline = 0;
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                            }
                            else
                            {
                                isInline = 1;
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                            }
                        };
                        msgWD.Show();
                    }
                    else
                    {
                        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                        }
                        else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                        {

                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                        }
                        else
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);

                        }
                    }
                    //}
                }
            }
        }

        private void PrintPharmacyBill(long iBillId, long iUnitID, long PrintID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                //string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&IsIPDBill=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }
        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {
            if (iBillId > 0)
            {

                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&isInline=" + isInline;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                ////long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                //long UnitID = iUnitID;
                //string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

                //string msgText = "Are you sure you want to Print Details Bill ?"; ;
                //MessageBoxControl.MessageBoxChildWindow msgWD =
                //    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //msgWD.OnMessageBoxClosed += (arg) =>
                //{
                //    if (arg == MessageBoxResult.Yes)
                //    {                     
                //        long UnitID = iUnitID;
                //        string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                //    }
                //    else
                //    {
                //        long UnitID = iUnitID;
                //        long isInline = 1;
                //        bool IsBilled = true;
                //        string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&IsBilled=" + IsBilled + "&isInline=" + isInline;
                //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                //    }
                //};
                //msgWD.Show();
            }

        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFirstName.Text.Trim()))
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToTitleCase();
            }
        }

        /// <summary>
        /// Method calls the function settle bill for the selected bill in the grid .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int ClickedFlag = 0;
        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (dgBillList.SelectedItem != null)
                {
                    if (((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
                    {
                        if (((clsBillVO)dgBillList.SelectedItem).BillType == BillTypes.Pharmacy)
                        {
                            SettlePharmacyBill();
                        }
                        else
                        {
                            GetCharge(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((clsBillVO)dgBillList.SelectedItem).IsFreezed, ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_Id);
                        }
                    }   //Added By CDS  SettleBill();
                }
                else
                {
                    string msgText = "Only freezed Bills can be Settled ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.Show();

                    ClickedFlag = 0;
                }
            }
            //else
            //{
            //    ClickedFlag = 0;
            //}
        }

        // Added By CDS
        List<clsChargeVO> lstChargeID = new List<clsChargeVO>();
        void GetCharge(long PBillID, long pUnitID, bool pIsBilled, long pVisitID)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {

                Indicatior.Show();

                clsGetChargeListBizActionVO BizAction = new clsGetChargeListBizActionVO();

                BizAction.ID = 0;
                BizAction.Opd_Ipd_External_Id = pVisitID;
                BizAction.Opd_Ipd_External_UnitId = ((clsBillVO)dgBillList.SelectedItem).Opd_Ipd_External_UnitId;
                BizAction.IsBilled = pIsBilled;
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID;
                //BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will w0ork both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;
                            objList = ((clsGetChargeListBizActionVO)arg.Result).List;

                            foreach (var item in objList)
                            {
                                lstChargeID.Add(item);
                            }
                            if (lstChargeID.Count > 0)
                            {
                                SettleBill();
                            }
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                Indicatior.Close();
            }
        }

        // Added By CDS
        void SettlePharmacyBill()
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {


                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;

                string msgTitle = "";
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
                            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = SelectedBill.PatientID;
                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID = SelectedBill.PatientID;
                            ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID = SelectedBill.UnitID;
                            SettlePaymentWin.PatientID = SelectedBill.PatientID;
                            SettlePaymentWin.PatientUNitID = SelectedBill.UnitID;

                            // In  Case Of Pharmacy Bill
                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(NewSettlePaymentWin_OnSaveButton_Click);

                            SettlePaymentWin.Show();
                        }
                        else
                        {


                        }
                    }
                    else
                    {


                    }
                    ClickedFlag = 0;

                };
                msgWD.Show();
            }
        }

        // Added By CDS
        void SettleBill()
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {

                SelectedBill = new clsBillVO();
                SelectedBill = (clsBillVO)dgBillList.SelectedItem;

                string msgTitle = "";
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


                            SettlePaymentWin.PatientID = SelectedBill.PatientID;
                            SettlePaymentWin.PatientName = SelectedBill.FirstName + " " + SelectedBill.MiddleName + " " + SelectedBill.LastName;
                            SettlePaymentWin.PatientUNitID = SelectedBill.UnitID;
                            SettlePaymentWin.UnitID = SelectedBill.UnitID;
                            SettlePaymentWin.CompanyIDForBill = SelectedBill.CompanyId;
                            SettlePaymentWin.MRNO = SelectedBill.MRNO;

                            SettlePaymentWin.Initiate("Bill");



                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                            SettlePaymentWin.cmbConcessionAuthBy.SelectedValue = SelectedBill.PaymentDetails.ChequeAuthorizedBy;
                            SettlePaymentWin.txtNarration.Text = SelectedBill.PaymentDetails.PayeeNarration;
                            SettlePaymentWin.IsSettleMentBill = true;
                            SettlePaymentWin.TxtConAmt.IsReadOnly = false;

                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                            // In Case Clinical Bill

                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(NewSettlePaymentWin_OnSaveButton_Click);

                            SettlePaymentWin.Show();
                        }
                        else
                        {
                            lstChargeID = new List<clsChargeVO>();

                        }
                    }
                    else
                    {

                        lstChargeID = new List<clsChargeVO>();

                    }

                };
                msgWD.Show();
            }
            ClickedFlag = 0;
        }

        // Added By CDS
        void NewSettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();

            try
            {
                if (((clsBillVO)dgBillList.SelectedItem).BillType == BillTypes.Clinical)
                {
                    #region ClinicalBill
                    if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                    {
                        if (((PaymentWindow)sender).Payment != null)
                        {

                            Indicatior.Show();
                            clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                            clsPaymentVO pPayDetails = new clsPaymentVO();

                            pPayDetails = ((PaymentWindow)sender).Payment;
                            pPayDetails.IsBillSettlement = true;

                            //Added By CDS 
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                            else
                                pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                            //Added By CDS 

                            BizAction.Details = pPayDetails;

                            BizAction.Details.BillID = SelectedBill.ID;
                            //BizAction.Details.BillAmount = SelectedBill.NetBillAmount;

                            if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                            {
                                BizAction.Details.BillAmount = (SelectedBill.NetBillAmount - Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)));
                            }
                            else
                            {
                                BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                            }
                            BizAction.Details.Date = DateTime.Now;
                            double balam = 0;

                            // In Case OF Multiple Bills As Well As Multiple Concensation Then The Payment table BillBalanceAmount Settelemt Done Through Here //

                            ////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                            {
                                balam = ((PaymentWindow)sender).TotalAmount - (pPayDetails.PaidAmount + Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)));
                            }
                            else
                            {
                                balam = ((PaymentWindow)sender).TotalAmount - pPayDetails.PaidAmount;
                            }
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////
                            BizAction.Details.BillBalanceAmount = balam;
                            // Begin Update T_Bill   For Column BalanceAmountSelf  With transaction  //
                            clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                            mybillPayDetails.Details = SelectedBill;

                            //Added By CDS 
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                mybillPayDetails.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                            else
                                mybillPayDetails.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                            //Added By CDS 

                            if (pPayDetails.SettlementConcessionAmount > 0)
                            {
                                mybillPayDetails.Details.TotalConcessionAmount = SelectedBill.TotalConcessionAmount + pPayDetails.SettlementConcessionAmount;
                            }
                            if (pPayDetails.SettlementConcessionAmount > 0)
                            {
                                mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - (pPayDetails.PaidAmount + pPayDetails.SettlementConcessionAmount);
                                if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                                mybillPayDetails.Details.NetBillAmount = SelectedBill.NetBillAmount - pPayDetails.SettlementConcessionAmount;
                            }
                            else
                            {
                                mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
                                if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;
                            }
                            if (SelectedBill.Date.Date == DateTime.Now.Date.Date)
                            {
                                #region With Concession  (First/N Time Settelement Only When Bill Date is Same as Settelement Date)
                                if (pPayDetails.SettlementConcessionAmount > 0)
                                {
                                    double TotalAmt = pPayDetails.PaidAmount;
                                    double ConAmt = pPayDetails.SettlementConcessionAmount;

                                    var results = from r in lstChargeID
                                                  orderby r.ServicePaidAmount, r.NetAmount ascending
                                                  select r;

                                    lstChargeID = new List<clsChargeVO>();

                                    foreach (var item in results.ToList())
                                    {
                                        if (item.ParentID == 0)
                                        {
                                            double BalAmt = item.NetAmount - item.ServicePaidAmount;
                                            double TotalServicePaidAmount = item.ServicePaidAmount;
                                            double TotalConcession = item.Concession;
                                            double PConAmt = 0;

                                            item.IsUpdate = true;
                                            if (ConAmt > 0 && BalAmt > 0)
                                            {
                                                if (ConAmt > BalAmt)
                                                {
                                                    double UsedCon = BalAmt;
                                                    PConAmt = UsedCon;
                                                    item.Concession = TotalConcession + BalAmt;
                                                    item.SettleNetAmount = item.TotalAmount - item.Concession;
                                                    item.BalanceAmount = BalAmt - item.Concession;
                                                    if (item.BalanceAmount < 0)
                                                    {
                                                        item.BalanceAmount = 0;
                                                    }
                                                    item.ServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                    ConAmt = ConAmt - UsedCon;


                                                    //Total
                                                    item.TotalNetAmount = item.SettleNetAmount;
                                                    item.TotalConcession = item.Concession;
                                                    item.TotalServicePaidAmount = item.ServicePaidAmount;

                                                }
                                                else
                                                {
                                                    double UsedCon = ConAmt;
                                                    PConAmt = UsedCon;
                                                    item.Concession = (ConAmt + item.Concession);
                                                    item.SettleNetAmount = item.TotalAmount - item.Concession;
                                                    item.BalanceAmount = BalAmt - item.Concession;
                                                    ConAmt = ConAmt - UsedCon;
                                                    item.ServicePaidAmount = item.ServicePaidAmount;

                                                    //Total
                                                    item.TotalNetAmount = item.SettleNetAmount;
                                                    item.TotalConcession = item.Concession;
                                                    item.TotalServicePaidAmount = item.ServicePaidAmount;

                                                }
                                            }
                                            else
                                            {
                                                item.BalanceAmount = BalAmt;
                                            }
                                        }
                                        lstChargeID.Add(item);

                                    }
                                }

                                #endregion

                                #region WithOut Concession  (First/N Time Settelement Only When Bill Date is Same as Settelement Date) (For Paid Amount)
                                double TotalAmt2 = pPayDetails.PaidAmount;

                                double ConsumeAmt = 0;

                                var FinalResult = from r in lstChargeID
                                                  orderby r.NetAmount descending
                                                  select r;

                                lstChargeID = new List<clsChargeVO>();


                                foreach (var item in FinalResult.ToList())
                                {
                                    if (item.ParentID == 0)
                                    {
                                        item.IsUpdate = true;

                                        double BalAmt = item.NetAmount - item.ServicePaidAmount;
                                        double PConsumeAmt = 0;
                                        if (TotalAmt2 > 0 && BalAmt > 0)
                                        {

                                            ConsumeAmt = BalAmt;
                                            if (TotalAmt2 >= ConsumeAmt)
                                            {
                                                TotalAmt2 = TotalAmt2 - ConsumeAmt;

                                            }
                                            else
                                            {
                                                ConsumeAmt = TotalAmt2;
                                                TotalAmt2 = TotalAmt2 - ConsumeAmt;
                                            }

                                            PConsumeAmt = ConsumeAmt;
                                            item.ServicePaidAmount = ConsumeAmt + item.ServicePaidAmount;
                                            item.BalanceAmount = BalAmt - ConsumeAmt;


                                            //Total Amount
                                            item.TotalServicePaidAmount = item.ServicePaidAmount;
                                            item.TotalConcession = item.Concession;
                                            item.TotalNetAmount = item.NetAmount;
                                        }
                                        else
                                        {
                                            item.TotalServicePaidAmount = item.ServicePaidAmount;
                                            item.TotalConcession = item.Concession;
                                            item.TotalNetAmount = item.NetAmount;
                                            item.BalanceAmount = item.NetAmount - item.ServicePaidAmount;
                                        }

                                    }
                                    lstChargeID.Add(item);

                                }

                                #endregion
                            }
                            else
                            {
                                if (pPayDetails.SettlementConcessionAmount > 0)
                                {
                                    #region With Concession Second/N  Time Settelement
                                    double TotalAmt = pPayDetails.PaidAmount;
                                    double ConAmt = pPayDetails.SettlementConcessionAmount;

                                    var results = from r in lstChargeID
                                                  orderby r.ServicePaidAmount, r.SettleNetAmount ascending
                                                  select r;

                                    lstChargeID = new List<clsChargeVO>();

                                    foreach (var item in results.ToList())
                                    {
                                        if (item.Date.Value.Date == DateTime.Now.Date.Date)
                                        {
                                            #region con Same Date
                                            if (item.ParentID == 0)
                                            {
                                                double BalAmt = item.SettleNetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double PConAmt = 0;
                                                item.IsUpdate = true;
                                                if (ConAmt > 0 && BalAmt > 0)
                                                {
                                                    if (ConAmt > BalAmt)
                                                    {
                                                        double UsedCon = BalAmt;
                                                        PConAmt = UsedCon;
                                                        item.Concession = BalAmt;
                                                        item.SettleNetAmount = item.SettleNetAmount - item.Concession;
                                                        item.BalanceAmount = BalAmt - item.Concession;
                                                        item.ServicePaidAmount = item.ServicePaidAmount;
                                                        ConAmt = ConAmt - UsedCon;

                                                        ////Total Amount
                                                        item.TotalNetAmount = item.TotalAmount - (TotalConcession + item.Concession);
                                                        item.TotalServicePaidAmount = item.ServicePaidAmount;
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.TotalConcessionPercentage = 0;
                                                    }
                                                    else
                                                    {
                                                        double UsedCon = ConAmt;
                                                        PConAmt = UsedCon;
                                                        item.Concession = ConAmt;
                                                        item.SettleNetAmount = item.SettleNetAmount - item.Concession;
                                                        item.BalanceAmount = BalAmt - item.Concession;
                                                        ConAmt = ConAmt - UsedCon;
                                                        item.ServicePaidAmount = item.ServicePaidAmount;

                                                        //Total Amount
                                                        item.TotalNetAmount = item.TotalAmount - (TotalConcession + item.Concession);
                                                        item.TotalServicePaidAmount = item.ServicePaidAmount;
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.TotalConcessionPercentage = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    item.BalanceAmount = BalAmt;
                                                }
                                            }

                                            lstChargeID.Add(item);
                                            #endregion
                                        }
                                        else
                                        {
                                            #region Diff Date
                                            if (item.ParentID == 0)
                                            {
                                                double BalAmt = item.NetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double TotalAmount = item.TotalAmount;
                                                double PConAmt = 0;
                                                if (ConAmt > 0 && BalAmt > 0)
                                                {
                                                    if (ConAmt > BalAmt)
                                                    {
                                                        double UsedCon = BalAmt;
                                                        PConAmt = UsedCon;
                                                        item.Concession = BalAmt;
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.SettleNetAmount = item.SettleNetAmount - item.Concession;

                                                        item.BalanceAmount = BalAmt - item.Concession;
                                                        item.ServicePaidAmount = item.ServicePaidAmount;
                                                        ConAmt = ConAmt - UsedCon;
                                                    }
                                                    else
                                                    {
                                                        double UsedCon = ConAmt;
                                                        PConAmt = UsedCon;
                                                        item.Concession = UsedCon;
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.SettleNetAmount = item.SettleNetAmount - item.Concession;
                                                        item.BalanceAmount = BalAmt - item.Concession;
                                                        ConAmt = ConAmt - UsedCon;
                                                        item.ServicePaidAmount = item.ServicePaidAmount;

                                                    }
                                                }
                                                else
                                                {
                                                    item.BalanceAmount = BalAmt;
                                                    item.TotalConcession = TotalConcession;
                                                    item.Concession = 0;
                                                }
                                            }

                                            lstChargeID.Add(item);

                                            #endregion

                                        }
                                    }

                                    #region Set Paid Amount
                                    double TotalAmt3 = pPayDetails.PaidAmount;
                                    double ConsumeAmt = 0;

                                    var Finalresults = from r in lstChargeID
                                                       orderby r.SettleNetAmount descending
                                                       select r;

                                    lstChargeID = new List<clsChargeVO>();


                                    foreach (var item in Finalresults.ToList())
                                    {

                                        if (item.Date.Value.Date == DateTime.Now.Date.Date)
                                        {
                                            #region SameDate
                                            if (item.ParentID == 0)
                                            {
                                                item.IsUpdate = true;
                                                item.IsSameDate = true;
                                                double BalAmt = item.SettleNetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double TotalNetAmount = item.SettleNetAmount;
                                                double PConsumeAmt = 0;

                                                if (BalAmt > 0)
                                                {
                                                    if (TotalAmt3 > 0 && BalAmt > 0)
                                                    {

                                                        ConsumeAmt = BalAmt;
                                                        if (TotalAmt3 >= ConsumeAmt)
                                                        {
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }
                                                        else
                                                        {
                                                            ConsumeAmt = TotalAmt3;
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }
                                                        item.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.BalanceAmount = BalAmt - item.ServicePaidAmount;
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;

                                                        item.TotalConcession = item.TotalConcession;
                                                        item.TotalNetAmount = item.SettleNetAmount;
                                                        item.TotalConcessionPercentage = 0;

                                                    }
                                                    else
                                                    {

                                                        item.BalanceAmount = BalAmt;
                                                        //item.ServicePaidAmount = item.ServicePaidAmount;  //Previous 
                                                        if (TotalAmt3 == 0)
                                                            item.ServicePaidAmount = 0; //In Case When TotalAmt3 = 0 (i.e. all paidamount is consumed) && BalAmt > 0 

                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.Concession = item.Concession;
                                                        item.SettleNetAmount = item.SettleNetAmount;

                                                        //Total Amount 
                                                        //item.TotalConcession = item.Concession + TotalConcession;   //Previous 
                                                        item.TotalConcession = item.TotalConcession; // New Code 
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;  // New Code                                                           
                                                    }
                                                }
                                                else
                                                {
                                                    item.SettleNetAmount = item.SettleNetAmount;
                                                    item.ServicePaidAmount = 0;
                                                    item.BalanceAmount = 0;
                                                    item.Concession = 0;

                                                    //Total Amount
                                                    item.TotalNetAmount = TotalNetAmount;
                                                    item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                    item.TotalConcession = TotalConcession + item.Concession;
                                                }

                                            }
                                            lstChargeID.Add(item);
                                            #endregion
                                        }

                                        else
                                        {
                                            #region Diff Date
                                            if (item.ParentID == 0)
                                            {
                                                item.IsUpdate = false;
                                                item.IsSameDate = false;
                                                double BalAmt = item.SettleNetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double TotalNetAmount = item.NetAmount;
                                                double PConsumeAmt = 0;
                                                if (BalAmt > 0)
                                                {
                                                    if (TotalAmt3 > 0 && BalAmt > 0)
                                                    {

                                                        ConsumeAmt = BalAmt;
                                                        if (TotalAmt3 >= ConsumeAmt)
                                                        {
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;

                                                        }
                                                        else
                                                        {
                                                            ConsumeAmt = TotalAmt3;
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }

                                                        item.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.BalanceAmount = BalAmt - item.ServicePaidAmount;




                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                        item.TotalNetAmount = item.TotalAmount - item.Concession;

                                                    }
                                                    else
                                                    {

                                                        item.BalanceAmount = BalAmt;

                                                        item.ServicePaidAmount = item.ServicePaidAmount;
                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.Concession = item.Concession;
                                                        item.SettleNetAmount = item.SettleNetAmount;

                                                        //Total Amount
                                                        item.TotalNetAmount = item.TotalAmount - item.TotalConcession;
                                                        //item.ServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;   //Prevoius 
                                                        if (TotalAmt3 == 0)
                                                            item.ServicePaidAmount = 0; //In Case When TotalAmt3 = 0 (i.e. all paidamount is consumed) && BalAmt > 0 
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                    }
                                                }
                                                else
                                                {
                                                    item.SettleNetAmount = item.SettleNetAmount;
                                                    item.ServicePaidAmount = 0;
                                                    item.BalanceAmount = 0;

                                                    //Total Amount

                                                    //  item.TotalNetAmount = TotalNetAmount;   // Prevoius 
                                                    item.TotalNetAmount = item.TotalAmount - item.TotalConcession;
                                                    item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;

                                                }

                                                #region When ParentID > 0 Then

                                                var _List = from obj in Finalresults.ToList()
                                                            where (obj.PackageID.Equals(item.PackageID) && obj.ParentID > 0)
                                                            select obj;

                                                double CTotalAmt = PConsumeAmt;
                                                double CConsumeAmt = 0;

                                                foreach (var objcharge in _List)
                                                {
                                                    objcharge.IsUpdate = false;
                                                    objcharge.IsSameDate = false;
                                                    double CBalAmt = objcharge.SettleNetAmount - objcharge.ServicePaidAmount;
                                                    double CTotalServicePaidAmount = objcharge.ServicePaidAmount;
                                                    double CTotalConcession = objcharge.Concession;
                                                    double CTotalNetAmount = objcharge.TotalNetAmount;

                                                    if (CBalAmt > 0)
                                                    {
                                                        if (CTotalAmt > 0 && CBalAmt > 0)
                                                        {

                                                            CConsumeAmt = CBalAmt;
                                                            if (CTotalAmt >= CConsumeAmt)
                                                            {
                                                                CTotalAmt = CTotalAmt - CConsumeAmt;
                                                            }
                                                            else
                                                            {
                                                                CConsumeAmt = CTotalAmt;
                                                                CTotalAmt = CTotalAmt - CConsumeAmt;
                                                            }

                                                            objcharge.ServicePaidAmount = CConsumeAmt;

                                                            objcharge.BalanceAmount = CBalAmt - objcharge.ServicePaidAmount;

                                                            objcharge.TotalServicePaidAmount = CTotalServicePaidAmount + objcharge.ServicePaidAmount;


                                                        }
                                                        else
                                                        {

                                                            objcharge.BalanceAmount = CBalAmt;

                                                            objcharge.ServicePaidAmount = objcharge.ServicePaidAmount;
                                                            objcharge.Concession = objcharge.Concession;
                                                            objcharge.SettleNetAmount = objcharge.SettleNetAmount;

                                                            objcharge.ServicePaidAmount = CTotalServicePaidAmount + objcharge.ServicePaidAmount;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        objcharge.SettleNetAmount = objcharge.SettleNetAmount;
                                                        objcharge.ServicePaidAmount = 0;
                                                        objcharge.BalanceAmount = 0;
                                                        //Total Amount
                                                        objcharge.TotalNetAmount = CTotalNetAmount;
                                                        objcharge.TotalServicePaidAmount = CTotalServicePaidAmount + objcharge.ServicePaidAmount;


                                                    }

                                                }

                                                #endregion

                                            }
                                            lstChargeID.Add(item);
                                            #endregion
                                        }
                                    }

                                    #endregion

                                    #endregion
                                }
                                else
                                {
                                    #region Without Concession Second/N Time Settelement
                                    double TotalAmt3 = pPayDetails.PaidAmount;
                                    double ConsumeAmt = 0;
                                    double ConAmt = pPayDetails.SettlementConcessionAmount;

                                    var Finalresults = from r in lstChargeID
                                                       orderby r.NetAmount descending
                                                       select r;

                                    lstChargeID = new List<clsChargeVO>();

                                    foreach (var item in Finalresults.ToList())
                                    {
                                        #region same date
                                        if (item.Date.Value.Date == DateTime.Now.Date.Date)
                                        {
                                            if (item.ParentID == 0)
                                            {

                                                item.IsUpdate = true;
                                                item.IsSameDate = true;

                                                double BalAmt = item.SettleNetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double TotalNetAmount = item.SettleNetAmount;
                                                double PConsumeAmt = 0;

                                                if (BalAmt > 0)
                                                {
                                                    if (TotalAmt3 > 0 && BalAmt > 0)
                                                    {
                                                        ConsumeAmt = BalAmt;
                                                        if (TotalAmt3 >= ConsumeAmt)
                                                        {
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }
                                                        else
                                                        {
                                                            ConsumeAmt = TotalAmt3;
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }

                                                        item.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.BalanceAmount = BalAmt - item.ServicePaidAmount;
                                                        item.Concession = ConAmt;


                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.TotalNetAmount = item.SettleNetAmount;
                                                        item.TotalConcessionPercentage = 0;

                                                    }
                                                    else
                                                    {
                                                        item.BalanceAmount = BalAmt;
                                                        //item.ServicePaidAmount = item.ServicePaidAmount;   //Previous 
                                                        if (TotalAmt3 == 0)
                                                            item.ServicePaidAmount = 0; //In Case When TotalAmt3 = 0 (i.e. all paidamount is consumed) && BalAmt > 0 

                                                        item.Concession = item.Concession;
                                                        item.SettleNetAmount = item.SettleNetAmount;

                                                        //Total Amount
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;  // new code as item.TotalServicePaidAmount is not set
                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.TotalNetAmount = item.TotalAmount - (TotalConcession + item.Concession);   // New For T_Chrge  Table 
                                                    }
                                                }
                                                else
                                                {
                                                    item.SettleNetAmount = item.SettleNetAmount;
                                                    item.ServicePaidAmount = 0;
                                                    item.BalanceAmount = 0;
                                                    item.Concession = 0;

                                                    //Total Amount
                                                    item.TotalNetAmount = TotalNetAmount;
                                                    item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                    item.TotalConcession = TotalConcession + item.Concession;

                                                }

                                            }
                                            lstChargeID.Add(item);

                                        }
                                        #endregion


                                        #region Differnt date
                                        else
                                        {

                                            if (item.ParentID == 0)
                                            {

                                                item.IsUpdate = false;
                                                item.IsSameDate = false;
                                                double BalAmt = item.SettleNetAmount - item.ServicePaidAmount;
                                                double TotalServicePaidAmount = item.ServicePaidAmount;
                                                double TotalConcession = item.Concession;
                                                double TotalNetAmount = item.SettleNetAmount;
                                                double PConsumeAmt = 0;

                                                if (BalAmt > 0)
                                                {
                                                    if (TotalAmt3 > 0 && BalAmt > 0)
                                                    {
                                                        ConsumeAmt = BalAmt;
                                                        if (TotalAmt3 >= ConsumeAmt)
                                                        {
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;

                                                        }
                                                        else
                                                        {
                                                            ConsumeAmt = TotalAmt3;
                                                            TotalAmt3 = TotalAmt3 - ConsumeAmt;
                                                        }
                                                        item.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item.ServicePaidAmount;
                                                        item.BalanceAmount = BalAmt - item.ServicePaidAmount;
                                                        //Total Amount
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                        item.TotalConcession = item.Concession;
                                                        item.TotalNetAmount = item.TotalAmount - item.Concession;
                                                        item.Concession = 0;


                                                    }
                                                    else
                                                    {
                                                        item.BalanceAmount = BalAmt;
                                                        //item.ServicePaidAmount = item.ServicePaidAmount;  //Previous 
                                                        if (TotalAmt3 == 0)
                                                            item.ServicePaidAmount = 0; //In Case When TotalAmt3 = 0 (i.e. all paidamount is consumed) && BalAmt > 0 


                                                        item.Concession = ConAmt;
                                                        item.SettleNetAmount = item.SettleNetAmount;
                                                        //Total Amount

                                                        //item.TotalServicePaidAmount = item.ServicePaidAmount;   //Previous 
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;

                                                        item.TotalConcession = TotalConcession + item.Concession;
                                                        item.TotalNetAmount = item.TotalAmount - (TotalConcession + item.Concession);

                                                    }
                                                }
                                                else
                                                {
                                                    item.SettleNetAmount = item.SettleNetAmount;
                                                    item.ServicePaidAmount = 0;
                                                    item.BalanceAmount = 0;
                                                    item.Concession = 0;
                                                    //Total Amount
                                                    item.TotalNetAmount = TotalNetAmount;
                                                    item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                    item.TotalConcession = TotalConcession + item.Concession;

                                                }


                                            }
                                            lstChargeID.Add(item);

                                        }
                                        #endregion
                                    }

                                    #endregion
                                }
                            }


                            mybillPayDetails.ChargeDetails = lstChargeID;
                            BizAction.mybillPayDetails = mybillPayDetails.DeepCopy();
                            BizAction.isUpdateBillPaymentDetails = true;
                            // End Update T_Bill   For Column BalanceAmountSelf  With transaction  //
                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null && arg.Result != null)
                                {
                                    if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                        PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                                    lstChargeID = new List<clsChargeVO>();
                                    Indicatior.Close();

                                    if (dgBillList.ItemsSource != null)
                                    {
                                        dgBillList.ItemsSource = null;
                                        dgBillList.ItemsSource = DataList;
                                    }

                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);


                                    msgWD.OnMessageBoxClosed += (re) =>
                                    {
                                        if (re == MessageBoxResult.OK)
                                        {
                                            if (((clsBillVO)dgBillList.SelectedItem).BalanceAmountNonSelf == 0 && ((clsBillVO)dgBillList.SelectedItem).BalanceAmountSelf == 0)
                                            {
                                                PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                                            }
                                        }
                                    };
                                    msgWD.Show();
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
                    #endregion
                }
                else
                {

                    #region PharmacyBill
                    if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                    {
                        if (((PaymentWindow)sender).Payment != null)
                        {

                            Indicatior.Show();
                            clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                            clsPaymentVO pPayDetails = new clsPaymentVO();

                            pPayDetails = ((PaymentWindow)sender).Payment;
                            pPayDetails.IsBillSettlement = true;
                            pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                            BizAction.Details = pPayDetails;

                            BizAction.Details.BillID = SelectedBill.ID;
                            //BizAction.Details.BillAmount = SelectedBill.NetBillAmount;                            
                            if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                            {
                                BizAction.Details.BillAmount = (SelectedBill.NetBillAmount - Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)));
                            }
                            else
                            {
                                BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                            }

                            BizAction.Details.Date = DateTime.Now;
                            double ConAmt = 0;

                            // In Case OF Multiple Bills As Well As Multiple Concensation Then The Payment table BillBalanceAmount Settelemt Done Through Here //

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                            {
                                BizAction.Details.BillBalanceAmount = ((PaymentWindow)sender).TotalAmount - (pPayDetails.PaidAmount + Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)));
                            }
                            else
                            {
                                BizAction.Details.BillBalanceAmount = ((PaymentWindow)sender).TotalAmount - pPayDetails.PaidAmount;
                            }
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //Begin Update  T_Bill Table                            

                            clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                            mybillPayDetails.Details = SelectedBill;

                            if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                            {
                                ConAmt = Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text));
                            }
                            else
                            {
                                ConAmt = 0;
                            }


                            mybillPayDetails.Details.BalanceAmountSelf = Math.Round(mybillPayDetails.Details.BalanceAmountSelf, 0) - (pPayDetails.PaidAmount + ConAmt);
                            if (Math.Round(mybillPayDetails.Details.BalanceAmountSelf, 0) < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;


                            if (ConAmt != null && ConAmt > 0)
                            {
                                mybillPayDetails.Details.NetBillAmount = SelectedBill.NetBillAmount - ConAmt;
                            }
                            if (ConAmt > 0)
                            {
                                mybillPayDetails.Details.TotalConcessionAmount = SelectedBill.TotalConcessionAmount + ConAmt;
                            }                         

                            BizAction.mybillPayDetails = mybillPayDetails.DeepCopy();

                            BizAction.isUpdateBillPaymentDetails = true;

                            //End Update  T_Bill Table

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null && arg.Result != null)
                                {
                                    if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                        PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                                    Indicatior.Close();


                                    if (dgBillList.ItemsSource != null)
                                    {

                                        dgBillList.ItemsSource = null;
                                        dgBillList.ItemsSource = DataList;

                                    }


                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);



                                    msgWD.OnMessageBoxClosed += (re) =>
                                    {
                                        if (re == MessageBoxResult.OK)
                                        {
                                            PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                                        }
                                    };
                                    msgWD.Show();
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
                    #endregion
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }


        #region OLD Code Commented By CDS
    
        #endregion
        long PaymentID { get; set; }

        #region OLD Code Commented By CDS
     
        #endregion    

        private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID)
        {
            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }

        private void btnReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (dgBillList.SelectedItem != null)
            {
                frmReceiptList receiptWin = new frmReceiptList();
                receiptWin.BillUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                receiptWin.BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                receiptWin.BillNo = ((clsBillVO)dgBillList.SelectedItem).BillNo;
                receiptWin.Show();

            }
        }

        private void dgBillList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            DataGridRow row = e.Row;
            if (((clsBillVO)(row.DataContext)).BalanceAmountSelf > 0)
            {
                SolidColorBrush objBrush = new SolidColorBrush(); objBrush.Color = Colors.Red;
                e.Row.Background = objBrush;
                // e.Row.Background.Equals(Color.FromArgb(0, 0, 0, 0));
            }

            if (((clsBillVO)(row.DataContext)).IsFreezed != true)
            {
                SolidColorBrush objBrush = new SolidColorBrush(); objBrush.Color = Colors.Green;
                e.Row.Background = objBrush;
                //  e.Row.Background.Equals(Color.FromArgb(0, 0, 0, 0));
            }

            if (((clsBillVO)(row.DataContext)).BalanceAmountSelf > 0 && ((clsBillVO)(row.DataContext)).IsFreezed == true && ((clsBillVO)(row.DataContext)).IsInvoiceGenerated == false)
            {
                FrameworkElement fe = dgBillList.Columns[12].GetCellContent(e.Row);
                FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                var thisCell = (DataGridCell)parent;
                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                txt.Visibility = Visibility.Visible;

            }
            else
            {
                FrameworkElement fe = dgBillList.Columns[12].GetCellContent(e.Row);
                FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                var thisCell = (DataGridCell)parent;
                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                txt.Visibility = Visibility.Collapsed;
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
        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;       
        }

        private void txtFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Serach_Bill();
            }
        }

        private void txtMiddleName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
         
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Serach_Bill();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Serach_Bill();
        }

        private void txtMRNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Serach_Bill();
            }
        }

        private void txtBillNO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Serach_Bill();
            }
        }

        private void txtOPDNO_KeyDown(object sender, KeyEventArgs e)
        {           
            if (e.Key == Key.Enter)
            {
                Serach_Bill();
            }
        }

        private void cmbBillStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Serach_Bill();
        }


        //.............................
        private void GetPatientDetails(long PID, long UID)
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = PID;
            BizAction.PatientDetails.GeneralDetails.UnitId = UID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtOPDNO_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        public void fillOpdIpdCombo()
        {
            List<MasterListItem> objList = new List<MasterListItem>();
            objList.Add(new MasterListItem(0, "-- Select --"));
            objList.Add(new MasterListItem(0, " OPD "));
            objList.Add(new MasterListItem(1, " IPD "));
            cmbIPDOPD.ItemsSource = null;
            cmbIPDOPD.ItemsSource = objList;
            cmbIPDOPD.SelectedItem = objList[0];
        }

        private void cmbIPDOPD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Serach_Bill();
        }
    }
}
