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
using OPDModule.Forms;

namespace CIMS.Forms
{
    public partial class frmBillSearch : UserControl
    {
        clsBillVO SelectedBill { get; set; }

        #region added by Prashant Channe to read reports config on 27thNov2019
        string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
        string URL = null;
        #endregion

        #region "Paging"
        int isInline = 0;

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
                // RaisePropertyChanged("DataListPageSize");
            }
        }

        public Boolean AgainstDonor = false;
        public long LinkPatientID;
        public long LinkPatientUnitID;
        public long LinkCompanyID;
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
            //checkFreezColumn = false;
            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            //BizAction.IsActive = true;
            if (dtpFromDate.SelectedDate != null)
                BizAction.FromDate = dtpFromDate.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.MRNO = txtMRNO.Text;
            BizAction.BillNO = txtBillNO.Text;
            BizAction.OPDNO = txtOPDNO.Text;
            BizAction.FirstName = txtFirstName.Text;
            BizAction.MiddleName = txtMiddleName.Text;
            BizAction.LastName = txtLastName.Text;

            if (cmbBillStatus.SelectedItem != null)
            {
                BizAction.BillStatus = (Int16)(((MasterListItem)cmbBillStatus.SelectedItem).ID);
            }
            if (cmbBillTypeClPh.SelectedItem != null)
            {
                BizAction.BillType = (BillTypes)((MasterListItem)cmbBillTypeClPh.SelectedItem).ID;     //BillType = (BillTypes)(((MasterListItem)cmbBillTypeClPh.SelectedItem).ID);
            }


            //dtpFromDate.Visibility = System.Windows.Visibility.Visible;
            //BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
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

            //BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

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
                    dgBillList.SelectedIndex = -1; // Added by Ashish z. for first record isn't coming in color coding. on dated 15122016

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
                //if (this.DataContext != null)
                //{
                //    cmbUnit.SelectedValue = ((clsBillVO)this.DataContext).UnitID;

                //}
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
            objList.Add(new MasterListItem(3, "Unfreezed"));

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

        public frmBillSearch()
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
                dtpFromDate.SetValidation("Please Select From Date");
                dtpFromDate.RaiseValidationError();

                string strMsg = "Please Select From Date";

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

                //  BillID = ((clsBillVO)dgBillList.SelectedItem).ID;
                if (SelectedBill.IsFreezed == true)
                {
                    //if (SelectedBill.VisitTypeID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyVisitTypeID)

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
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            }
                            else
                            {
                                isInline = 1;
                                PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((MasterListItem)cmbPrintFormat.SelectedItem).ID);
                            }
                        };
                        msgWD.Show();
                    }
                    else
                    {
                        if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                        }
                        else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                        {

                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                        }
                        else
                        {
                            PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                            PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);

                        }
                        isInline = 0;
                    }






                    /////////////OLD Code////////////
                    //if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                    //{
                    //    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                    //}
                    //else if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                    //{

                    //    PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                    //}
                    //else
                    //{
                    //    PrintBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
                    //    PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);

                    //}
                    ///////////////END/////////////////

                }
            }
        }

        private void PrintPharmacyBill(long iBillId, long iUnitID, long PrintID)
        {
            if (iBillId > 0)
            {
                

                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                long PackageID = 0; //***//
                if (((clsBillVO)dgBillList.SelectedItem).PackageID != null)
                {
                    PackageID = Convert.ToInt64(((clsBillVO)dgBillList.SelectedItem).PackageID);
                }
                //Modified by Prashant Channe on 27Nov2019 for ReportsFolder configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&PackageID=" + PackageID;
                }
                else
                {
                    URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&PackageID=" + PackageID;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }
        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {            

            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                //Modified by Prashant Channe on 27Nov2019 for ReportsFolder configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&isInline=" + isInline;
                }
                else
                {
                    URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&isInline=" + isInline;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");

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

                            #region To Show Patient Advance added on 05062018

                            SettlePaymentWin.PatientID = SelectedBill.PatientID;
                            SettlePaymentWin.PatientName = SelectedBill.FirstName + " " + SelectedBill.MiddleName + " " + SelectedBill.LastName;
                            SettlePaymentWin.PatientUNitID = SelectedBill.PatientUnitID;
                            SettlePaymentWin.UnitID = SelectedBill.UnitID;
                            SettlePaymentWin.CompanyIDForBill = SelectedBill.CompanyId;
                            SettlePaymentWin.MRNO = SelectedBill.MRNO;

                            #endregion

                            SettlePaymentWin.LinkPatientID = SelectedBill.LinkPatientID;
                            SettlePaymentWin.LinkPatientUnitID = SelectedBill.LinkPatientUnitID;

                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;

                            if (SelectedBill.PackageBillID > 0)     // For Package New Changes Added on 18062018
                            {
                                if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Pharmacy)
                                {
                                    SettlePaymentWin.IsNewPackageFlow = true;     // Set to apply new logic to retreive Patient,Package advance both & its auto consume logic : added on 16062016
                                    SettlePaymentWin.IsFromPharmacyBill = true;   // to Round off txtPayTotalAmount.Text amount in ApplyDiscountAll() method only when call payment window for pharmacy bill : added on 01082018
                                }

                                SettlePaymentWin.PackageID = SelectedBill.PackageID;
                                SettlePaymentWin.PackageBillID = SelectedBill.PackageBillID;
                                SettlePaymentWin.PackageBillUnitID = SelectedBill.UnitID;
                                //SettlePaymentWin.PackageConcenssionAmt = SelectedBill.PackageConcessionAmount;                    // commented on 01082018
                                SettlePaymentWin.PackageConcenssionAmt = Math.Round(SelectedBill.PackageConcessionAmount, 0);       // added on 01082018

                                //SettlePaymentWin.txtPackagePayableAmt.Text = SelectedBill.PackageConcessionAmount.ToString();                 // commented on 01082018
                                SettlePaymentWin.txtPackagePayableAmt.Text = Math.Round(SelectedBill.PackageConcessionAmount, 0).ToString();    // added on 01082018
                                SettlePaymentWin.lblPackagePayableAmt.Visibility = Visibility.Visible;
                                SettlePaymentWin.txtPackagePayableAmt.Visibility = Visibility.Visible;
                            }

                            // In  Case Of Pharmacy Bill
                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(NewSettlePaymentWin_OnSaveButton_Click);
                            SettlePaymentWin.OnCancelButton_Click += new RoutedEventHandler(NewSettlePaymentWin_OnCancelButton_Click);

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

                //if ((((clsBillVO)dgBillList.SelectedItem).RelationID == 15) && (((clsBillVO)dgBillList.SelectedItem).PatientCategoryId == 8 || ((clsBillVO)dgBillList.SelectedItem).PatientCategoryId == 9))
                //{
                //    DonorCoupleLinkedList Win = new DonorCoupleLinkedList();
                //    Win.DonorID = ((clsBillVO)dgBillList.SelectedItem).MRNO;
                //    Win.DonorUnitID = ((clsBillVO)dgBillList.SelectedItem).UnitID;
                //    Win.DonorName = ((clsBillVO)dgBillList.SelectedItem).FirstName + " " + ((clsBillVO)dgBillList.SelectedItem).MiddleName + " " + ((clsBillVO)dgBillList.SelectedItem).LastName;

                //    Win.OnSaveButton_Click += new RoutedEventHandler(ModifyDonorLinWin_OnSaveButton_Click);
                //    Win.OnCancelButton_Click += new RoutedEventHandler(DonorLinWin_OnCancelButton_Click);
                //    Win.Show();
                //}
                //else
                //{

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

                            SettlePaymentWin.LinkPatientID = SelectedBill.LinkPatientID;
                            SettlePaymentWin.LinkPatientUnitID = SelectedBill.LinkPatientUnitID;

                            SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                            SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                            SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                            SettlePaymentWin.cmbConcessionAuthBy.SelectedValue = SelectedBill.PaymentDetails.ChequeAuthorizedBy;
                            SettlePaymentWin.txtNarration.Text = SelectedBill.PaymentDetails.PayeeNarration;
                            SettlePaymentWin.IsSettleMentBill = true;
                            SettlePaymentWin.TxtConAmt.IsReadOnly = false;

                            if (lstChargeID != null && lstChargeID.Count > 0)
                            {
                                var items12 = from r in lstChargeID
                                              where r.isPackageService == true
                                              select r;

                                if (items12.ToList().Count > 0)
                                {
                                    SettlePaymentWin.IsNoAutoAdvanceConsume = true;       // Set to call auto collect advance or not : added on 22062018
                                }
                            }

                            SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

                            if (SelectedBill.PackageBillID > 0)     // For Package New Changes Added on 27062018
                            {
                                if (SelectedBill.BillType == PalashDynamics.ValueObjects.BillTypes.Clinical)
                                    SettlePaymentWin.IsNewPackageFlow = true;     // Set to apply new logic to retreive Patient,Package advance both & its auto consume logic : added on 27062018
                                SettlePaymentWin.PackageID = SelectedBill.PackageID;
                                SettlePaymentWin.PackageBillID = SelectedBill.PackageBillID;
                                SettlePaymentWin.PackageBillUnitID = SelectedBill.UnitID;
                                SettlePaymentWin.PackageConcenssionAmt = SelectedBill.PackageConcessionAmount;

                                SettlePaymentWin.txtPackagePayableAmt.Text = SelectedBill.PackageConcessionAmount.ToString();
                                SettlePaymentWin.lblPackagePayableAmt.Visibility = Visibility.Visible;
                                SettlePaymentWin.txtPackagePayableAmt.Visibility = Visibility.Visible;
                            }

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
                //}
            }
            ClickedFlag = 0;
        }


        //***//--------------------
        //public void ModifyDonorLinWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (((DonorCoupleLinkedList)sender).DialogResult == true)
        //    {
        //        AgainstDonor = ((DonorCoupleLinkedList)sender).DonorLink.IsAgainstDonor;
        //        LinkPatientID = ((DonorCoupleLinkedList)sender).DonorLink.PatientID;
        //        LinkPatientUnitID = ((DonorCoupleLinkedList)sender).DonorLink.PatientUnitID;
        //        LinkCompanyID = ((DonorCoupleLinkedList)sender).DonorLink.CompanyID;
        //    }

        //    SelectedBill = new clsBillVO();
        //    SelectedBill = (clsBillVO)dgBillList.SelectedItem;

        //    string msgTitle = "";
        //    string msgText = "Are you sure you want to Settle the Bill ?";

        //    MessageBoxControl.MessageBoxChildWindow msgWD =
        //        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //    msgWD.OnMessageBoxClosed += (arg) =>
        //    {
        //        if (arg == MessageBoxResult.Yes)
        //        {
        //            bool isValid = true;


        //            if (isValid)
        //            {
        //                PaymentWindow SettlePaymentWin = new PaymentWindow();
        //                if (SelectedBill.BalanceAmountSelf > 0)
        //                    SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;


        //                SettlePaymentWin.PatientID = SelectedBill.PatientID;
        //                SettlePaymentWin.PatientName = SelectedBill.FirstName + " " + SelectedBill.MiddleName + " " + SelectedBill.LastName;
        //                SettlePaymentWin.PatientUNitID = SelectedBill.UnitID;
        //                SettlePaymentWin.UnitID = SelectedBill.UnitID;
        //                SettlePaymentWin.CompanyIDForBill = SelectedBill.CompanyId;
        //                SettlePaymentWin.MRNO = SelectedBill.MRNO;

        //                SettlePaymentWin.Initiate("Bill");

        //                SettlePaymentWin.LinkPatientID = LinkPatientID;
        //                SettlePaymentWin.LinkPatientUnitID = LinkPatientUnitID;
        //                SettlePaymentWin.LinkCompanyID = LinkCompanyID;

        //                SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
        //                SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
        //                SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

        //                SettlePaymentWin.cmbConcessionAuthBy.SelectedValue = SelectedBill.PaymentDetails.ChequeAuthorizedBy;
        //                SettlePaymentWin.txtNarration.Text = SelectedBill.PaymentDetails.PayeeNarration;
        //                SettlePaymentWin.IsSettleMentBill = true;
        //                SettlePaymentWin.TxtConAmt.IsReadOnly = false;

        //                SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;

        //                // In Case Clinical Bill

        //                SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(NewSettlePaymentWin_OnSaveButton_Click);

        //                SettlePaymentWin.Show();
        //            }
        //            else
        //            {
        //                lstChargeID = new List<clsChargeVO>();

        //            }
        //        }
        //        else
        //        {

        //            lstChargeID = new List<clsChargeVO>();

        //        }

        //    };
        //    msgWD.Show();
        //}


        //void DonorLinWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //if (SelectedBill != null)
        //    //    cmdSave.IsEnabled = false;
        //    //else
        //    //    cmdSave.IsEnabled = true;
        //}
        //-------------------------

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
                                    //if (item.ParentID == 0)                       // For Package New Changes commented on 29062018
                                    if (item.ParentID == 0 || item.ParentID > 0)    // For Package New Changes added on 29062018
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
                                            //if (item.ParentID == 0)                       // For Package New Changes commented on 29062018
                                            if (item.ParentID == 0 || item.ParentID > 0)    // For Package New Changes added on 29062018
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

                            /*
                                   ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                  //  To Update the BalanceAmountSelf IN T_Bill Tabel                           
                                  //  To Update the NetBillAmount IN T_Bill Tabel 
                                                       if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                                                       {
                                                           mybillPayDetails.Details.NetBillAmount = SelectedBill.NetBillAmount -  Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text));
                               
                                                       }
                                  //  To Update the TotalConcessionAmount IN T_Bill Tabel 
                                                       if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "" && Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text)) > 0)
                                                       {
                                                           mybillPayDetails.Details.TotalConcessionAmount = SelectedBill.TotalConcessionAmount + Convert.ToDouble((((PaymentWindow)sender).TxtConAmt.Text));
                               
                                                       }
                                  ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                           */

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

        void NewSettlePaymentWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
        }

        #region OLD Code Commented By CDS
        //void SettleBill()
        //{
        //    if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
        //    {

        //        //InitialiseForm();
        //        SelectedBill = new clsBillVO();
        //        SelectedBill = (clsBillVO)dgBillList.SelectedItem;
        //        GetPatientDetails(SelectedBill.PatientID, SelectedBill.UnitID);
        //        // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);


        //        string msgTitle = "";
        //        string msgText = "Are you sure you want to Settle the Bill ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //        msgWD.OnMessageBoxClosed += (arg) =>
        //        {
        //            if (arg == MessageBoxResult.Yes)
        //            {
        //                bool isValid = true;
        //                //isValid = tr  // CheckValidations();

        //                if (isValid)
        //                {
        //                    PaymentWindow SettlePaymentWin = new PaymentWindow();
        //                    if (SelectedBill.BalanceAmountSelf > 0)
        //                        SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;

        //                    SettlePaymentWin.Initiate("Bill");

        //                    SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
        //                    SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
        //                    SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

        //                    //....................................
        //                    SettlePaymentWin.IsFromBillSearchWin = true;

        //                    //....................................


        //                    SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
        //                    SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
        //                    SettlePaymentWin.Show();
        //                }
        //                else
        //                {
        //                    // InitialiseForm();

        //                }
        //            }
        //            else
        //            {
        //                // InitialiseForm();

        //            }

        //        };
        //        msgWD.Show();
        //    }
        //}
        #endregion
        long PaymentID { get; set; }

        #region OLD Code Commented By CDS
        //void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    WaitIndicator Indicatior = new WaitIndicator();

        //    try
        //    {


        //        if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
        //        {
        //            if (((PaymentWindow)sender).Payment != null)
        //            {

        //                Indicatior.Show();
        //                clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
        //                clsPaymentVO pPayDetails = new clsPaymentVO();

        //                pPayDetails = ((PaymentWindow)sender).Payment;
        //                pPayDetails.IsBillSettlement = true;
        //                BizAction.Details = pPayDetails;

        //                BizAction.Details.BillID = SelectedBill.ID;
        //                BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
        //                BizAction.Details.Date = DateTime.Now;
        //                // BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;

        //                BizAction.Details.BillBalanceAmount = ((PaymentWindow)sender).TotalAmount - pPayDetails.PaidAmount;

        //                if (SelectedBill != null && SelectedBill.BillType == BillTypes.Pharmacy)
        //                {
        //                    //BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PharmacyCostingDivisionID;  //Costing Divisions for Pharmacy Billing
        //                    BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
        //                }

        //                if (SelectedBill != null && SelectedBill.BillType == BillTypes.Clinical)
        //                {
        //                    //BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.ClinicalCostingDivisionID;  //Costing Divisions for Clinical Billing
        //                    BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID; //Costing Divisions for Pharmacy Billing
        //                }

        //                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //                Client.ProcessCompleted += (s, arg) =>
        //                {
        //                    if (arg.Error == null && arg.Result != null)
        //                    {
        //                        if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
        //                            PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

        //                        clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

        //                        mybillPayDetails.Details = SelectedBill;

        //                        mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
        //                        if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

        //                        PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //                        Client1.ProcessCompleted += (s1, arg1) =>
        //                        {

        //                            Indicatior.Close();
        //                            if (arg1.Error == null && arg1.Result != null)
        //                            {



        //                                if (dgBillList.ItemsSource != null)
        //                                {

        //                                    dgBillList.ItemsSource = null;
        //                                    dgBillList.ItemsSource = DataList;

        //                                }


        //                                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);



        //                                msgWD.OnMessageBoxClosed += (re) =>
        //                              {
        //                                  if (re == MessageBoxResult.OK)
        //                                  {
        //                                      PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID);
        //                                  }
        //                              };
        //                                msgWD.Show();


        //                            }
        //                            else
        //                            {

        //                                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while updating Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                                msgWD.Show();
        //                            }


        //                        };

        //                        Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                        Client1.CloseAsync();



        //                    }
        //                    else
        //                    {
        //                        Indicatior.Close();
        //                        MessageBoxControl.MessageBoxChildWindow msgWD =
        //                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while Saving Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                        msgWD.Show();

        //                    }
        //                };
        //                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                Client.CloseAsync();

        //            }
        //        }
        //        else
        //        {

        //        }



        //    }
        //    catch (Exception)
        //    {

        //        Indicatior.Close();
        //    }



        //}
        #endregion

        #region OLD Code Commented By CDS
        //private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID, long PrintID)
        //{
        //    if (iBillId > 0)
        //    {                
        //        long UnitID = iUnitID;
        //        string URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID + "&PrintFomatID=" + PrintID;
        //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        //    }

        //}
        #endregion

        private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID)
        {
            #region added by Prashant Channe to read reports config on 3rdDec2019
            string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
            string URL = null;
            #endregion

            if (iBillId > 0)
            {
                long UnitID = iUnitID;
                //Added by Prashant Channe on 3rdDec2019 for reports configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
                else
                {
                    URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
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

            //if (e.Key == Key.Enter)
            //{
            //    Serach_Bill();
            //}
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

            //if (e.Key == Key.Enter)
            //{
            //    Serach_Bill();
            //}
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            //if (e.Key == Key.Enter)
            //{
            //    Serach_Bill();
            //}
        }

        private void dtpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Serach_Bill();
        }

        private void dtpToDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //Serach_Bill();
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
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            //if (e.Key == Key.Enter)
            //{
            //    Serach_Bill();
            //}
        }

        private void cmbBillStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Serach_Bill();
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

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Nullable<DateTime> dtFT = null;
            Nullable<DateTime> dtTT = null;
            if (DataList.IsEmpty)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("", "There are no reports to print", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();

            }
            else
            {

                if (dtpFromDate.SelectedDate != null)
                {
                    dtFT = dtpFromDate.SelectedDate.Value.Date.Date;
                }
                if (dtpToDate.SelectedDate != null)
                {
                    dtTT = dtpToDate.SelectedDate.Value.Date.Date.AddDays(1);
                }

                long UnitID = 0;
                long CostingDivisionID = 0;
                string MRNO = null;
                string BillNO = null;
                string OPDNO = null;
                string FirstName = null;
                string MiddleName = null;
                string LastName = null;

                long BillStatus = 0;
                long BillType = 0;


                MRNO = txtMRNO.Text;
                BillNO = txtBillNO.Text;
                OPDNO = txtOPDNO.Text;
                FirstName = txtFirstName.Text;
                MiddleName = txtMiddleName.Text;
                LastName = txtLastName.Text;

                if (cmbBillStatus.SelectedItem != null)
                {
                    BillStatus = (Int16)(((MasterListItem)cmbBillStatus.SelectedItem).ID);
                   
                }
                if (cmbBillTypeClPh.SelectedItem != null)
                {
                    BillType = Convert.ToInt64((BillTypes)((MasterListItem)cmbBillTypeClPh.SelectedItem).ID);
                }

                if (cmbUnit.SelectedItem != null)
                {
                    UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                }
                else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    UnitID = 0;
                }
                else
                {
                    UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                else
                    CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;



                string URL;
                if (dtFT != null && dtTT != null)
                {
                    URL = "../Reports/OPD/BillListReport.aspx?FromDate=" + dtFT.Value.ToString("dd-MMM-yyyy") + "&ToDate=" + dtTT.Value.ToString("dd-MMM-yyyy") + "&UnitID=" + UnitID + "&MRNO=" + MRNO + "&BillNO=" + BillNO + "&OPDNO=" + OPDNO + "&FirstName=" + FirstName + "&MiddleName=" + MiddleName + "&LastName=" + LastName + "&BillType=" + BillType + "&BillStatus=" + BillStatus + "&CostingDivisionID=" + CostingDivisionID + "&Excel=" + chkExcel.IsChecked;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    //URL = "../Reports/OPD/AppointmentListReport.aspx?ClinicID=" + clinic + "&DepartmentID=" + dept + "&DoctorID=" + doc;
                    //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
            }

        }


    }

}
