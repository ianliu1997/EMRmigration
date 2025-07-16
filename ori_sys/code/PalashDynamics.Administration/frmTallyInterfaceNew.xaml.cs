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
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Billing;
using System.Windows.Data;
using System.Xml;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Text;
using System.Xml.Linq;
using PalashDynamics.UserControls;


namespace PalashDynamics.Administration
{
    public partial class frmTallyInterfaceNew : UserControl, CIMS.Comman.IPreInitiateCIMS
    {
        #region IPreInitiateCIMS Members

        PalashDynamics.ValueObjects.Administration.clsMenuVO _SelfMenuDetails = null;

        public void PreInitiate(PalashDynamics.ValueObjects.Administration.clsMenuVO _MenuDetails)
        {
            _SelfMenuDetails = _MenuDetails;
        }

        #endregion

        public clsAccountsVO myAccounts { get; set; }

        private SaveFileDialog dialog = null;
        private bool? showDialog = null;

        public frmTallyInterfaceNew()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmTallyInterface_Loaded);

            myAccounts = new clsAccountsVO();
        }

        void frmTallyInterface_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                mElement1.Text = "Export Day Book";

                //dgOPDSelfBillLedgers.GroupDescriptions.Add(new PropertyGroupDescription("Department"));

                FillUnit();


                this.DataContext = myAccounts;
                //DisplayLedgers();

            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void FillStores(long pClinicID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_Store;
            BizAction.MasterList = new List<MasterListItem>();

            if (pClinicID > 0)
            {
                BizAction.Parent = new KeyValue { Value = "ClinicID", Key = pClinicID.ToString() };
            }

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

                    //cmbBloodGroup.ItemsSource = null;
                    //cmbBloodGroup.ItemsSource = objList;
                    cmbStore.ItemsSource = null;
                    cmbStore.ItemsSource = objList;

                    //if (objList.Count > 1)
                    //    cmbStore.SelectedItem = objList[1];
                    //else
                    cmbStore.SelectedItem = objList[0];

                    //if (this.DataContext != null)
                    //{
                    //    cmbStore.SelectedValue = ((clsGRNVO)this.DataContext).StoreID;
                    //}

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        clsAccountsVO objAccount = null;
        private void GenerateData()
        {
            WaitIndicator wait = new WaitIndicator();
            wait.Show();
            clsGetTotalBillAccountsLedgersVO BizAction = new clsGetTotalBillAccountsLedgersVO();
            BizAction.Details = new clsAccountsVO();
            BizAction.Details.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            BizAction.Details.ExportDate = dtpExportForDate.SelectedDate.Value.Date;
            if (cmbStore.ItemsSource != null)
                if ((((MasterListItem)cmbStore.SelectedItem).ID) > 0)
                    BizAction.Details.StoreID = ((MasterListItem)cmbStore.SelectedItem).ID;
                else
                    BizAction.Details.StoreID = 0;
            objAccount = new clsAccountsVO();
            BizAction.IsGenrateXML = false;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Result != null)
                {
                    if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details != null)
                    {
                        objAccount = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details;
                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDBillsLedgerAccount != null)
                        {
                            dgSelfIncomeBill.ItemsSource = null;
                            dgSelfIncomeBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDBillsLedgerAccount;

                            txtSelfBillDr.Text = String.Format("{0:0.000}",Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDBillsLedgerAccount.Sum(S => S.DR)), 2).ToString();
                            txtSelfBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDBillsLedgerAccount.Sum(S => S.CR)), 2).ToString();
                        }
                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDSelfCreditBillAccount != null)
                        {
                            dgSelfCrBill.ItemsSource = null;
                            dgSelfCrBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDSelfCreditBillAccount;
                            txtSelfCrBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDSelfCreditBillAccount.Sum(S => S.DR)), 2).ToString();
                            txtSelfCrBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDSelfCreditBillAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDReceiptLedgerAccount != null)
                        {
                            dgSelfReceipt.ItemsSource = null;
                            dgSelfReceipt.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDReceiptLedgerAccount;

                            txtSelfReceiptDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDReceiptLedgerAccount.Sum(S => S.DR)), 2).ToString();
                            txtSelfReceiptCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDReceiptLedgerAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyCreditBillAccount != null)
                        {

                            dgCompanyBill.ItemsSource = null;
                            dgCompanyBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyCreditBillAccount;

                            txtCompanyBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyCreditBillAccount.Sum(S => S.DR)), 2).ToString();
                            txtCompanyBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyCreditBillAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyReceiptBillAccount != null)
                        {

                            dgCompanyReceipt.ItemsSource = null;
                            dgCompanyReceipt.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyReceiptBillAccount;

                            txtCompanyReceiptDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyReceiptBillAccount.Sum(S => S.DR)), 2).ToString();
                            txtCompanyReceiptCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDCompanyReceiptBillAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceLedgerAccount != null)
                        {
                            dgAdvance.ItemsSource = null;
                            dgAdvance.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceLedgerAccount;

                            txtAdvanceDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceLedgerAccount.Sum(S => S.DR)), 2).ToString();
                            txtAdvanceCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceLedgerAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceRefundLedgerAccount != null)
                        {
                            dgRefundOfAdvance.ItemsSource = null;
                            dgRefundOfAdvance.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceRefundLedgerAccount;

                            txtRefundOfAdvanceDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceRefundLedgerAccount.Sum(S => S.DR)), 2).ToString();
                            txtRefundOfAdvanceCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDAdvanceRefundLedgerAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDRefundBillLedgerAccount != null)
                        {
                            dgRefundOfBill.ItemsSource = null;
                            dgRefundOfBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDRefundBillLedgerAccount;

                            txtRefundOfBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDRefundBillLedgerAccount.Sum(S => S.DR)), 2).ToString();
                            txtRefundOfBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.OPDRefundBillLedgerAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleIncome != null)
                        {

                            dgSaleIncomeReceipt.ItemsSource = null;
                            dgSaleIncomeReceipt.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleIncome;

                            txtSaleIncomeReceiptDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleIncome.Sum(S => S.DR)),2).ToString();
                            txtSaleIncomeReceiptCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleIncome.Sum(S => S.CR)),2).ToString();

                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCredit != null)
                        {
                            dgSaleCreditBills.ItemsSource = null;
                            dgSaleCreditBills.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCredit;

                            txtSaleCreditBillsDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCredit.Sum(S => S.DR)), 2).ToString();
                            txtSaleCreditBillsCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCredit.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleReturn != null)
                        {
                            dgSalesReturn.ItemsSource = null;
                            dgSalesReturn.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleReturn;

                            txtSalesReturnDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleReturn.Sum(S => S.DR)), 2).ToString();
                            txtSalesReturnCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleReturn.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleSelfReceiptLedgers != null)
                        {
                            dgSalesSelfReceive.ItemsSource = null;
                            dgSalesSelfReceive.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleSelfReceiptLedgers;

                            txtSalesSelfReceiveDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleSelfReceiptLedgers.Sum(S => S.DR)), 2).ToString();
                            txtSalesSelfReceiveCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleSelfReceiptLedgers.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCompanyReceiptLedgers != null)
                        {
                            dgCompanySelfReceive.ItemsSource = null;
                            dgCompanySelfReceive.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCompanyReceiptLedgers;

                            txtCompanySelfReceiveDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCompanyReceiptLedgers.Sum(S => S.DR)), 2).ToString();
                            txtCompanySelfReceiveCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.SaleCompanyReceiptLedgers.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.ScrapLedgers != null)
                        {
                            dgScrapSale.ItemsSource = null;
                            dgScrapSale.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.ScrapLedgers;

                            txtScrapSaleDr.Text = String.Format("{0:0.000}",Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.ScrapLedgers.Sum(S => S.DR)),2).ToString();
                            txtScrapSaleCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.ScrapLedgers.Sum(S => S.CR)),2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseLedger != null)
                        {
                            dgPurchase.ItemsSource = null;
                            dgPurchase.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseLedger;

                            txtPurchaseDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseLedger.Sum(S => S.DR)),2).ToString();
                            txtPurchaseCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseLedger.Sum(S => S.CR)),2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseReturnLedger != null)
                        {
                            dgPurchaseReturn.ItemsSource = null;
                            dgPurchaseReturn.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseReturnLedger;

                            txtPurchaseReturnDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseReturnLedger.Sum(S => S.DR)), 2).ToString();
                            txtPurchaseReturnCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.PurchaseReturnLedger.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger != null)
                        {
                            dgIPDMiscIncome.ItemsSource = null;
                            dgIPDMiscIncome.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger;

                            txtIPDMiscIncomeDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger.Sum(S => S.DR)), 2).ToString();
                            txtIPDMiscIncomeCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscExpenseLedgers != null)
                        {
                            dgIPDMiscExpence.ItemsSource = null;
                            dgIPDMiscExpence.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscExpenseLedgers;

                            txtIPDMiscExpenceDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscExpenseLedgers.Sum(S => S.DR)), 2).ToString();
                            txtIPDMiscExpenceCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscExpenseLedgers.Sum(S => S.CR)), 2).ToString();
                        }

                        //if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger != null)
                        //{
                        //    dgIPDMiscIncome.ItemsSource = null;
                        //    dgIPDMiscIncome.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger;

                        //    txtIPDMiscIncomeDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger.Sum(S => S.DR)), 2).ToString();
                        //    txtIPDMiscIncomeCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.MiscIncomeLedger.Sum(S => S.CR)), 2).ToString();
                        //}                        

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.DoctorPaymentLedgers != null)
                        {
                            dgDoctorPayment.ItemsSource = null;
                            dgDoctorPayment.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.DoctorPaymentLedgers;

                            txtDoctorBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.DoctorPaymentLedgers.Sum(S => S.DR)), 2).ToString();
                            txtDoctorBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.DoctorPaymentLedgers.Sum(S => S.CR)), 2).ToString();
                        }
                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfBillAccount != null)
                        {
                            dgIPDSelfBill.ItemsSource = null;
                            dgIPDSelfBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfBillAccount;

                            txtIPDSelfBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfBillAccount.Sum(S => S.DR)), 2).ToString();
                            txtIPDSelfBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfBillAccount.Sum(S => S.CR)), 2).ToString();
                        }

                        if (((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfCreditForTallyInterface != null)
                        {
                            dgIPDCreditBill.ItemsSource = null;
                            dgIPDCreditBill.ItemsSource = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfCreditForTallyInterface;

                            txtIPDCreditBillDr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfCreditForTallyInterface.Sum(S => S.DR)), 2).ToString();
                            txtIPDCreditBillCr.Text = String.Format("{0:0.000}", Convert.ToDecimal(((clsGetTotalBillAccountsLedgersVO)e.Result).Details.IPDSelfCreditForTallyInterface.Sum(S => S.CR)), 2).ToString();
                        }
                        wait.Close();
                    }

                }
                wait.Close();
            };
            wait.Close();

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void DisplayLedgers()
        {
            //WaitIndicator indicator = new WaitIndicator();

            //try
            //{


            //    indicator.Show();

            //    clsGetTotalBillAccountsLedgersVO BizAction = new clsGetTotalBillAccountsLedgersVO();
            //    clsAccountsVO tempAccounts = new clsAccountsVO();
            //    if(dtpExportForDate.SelectedDate.HasValue)
            //        tempAccounts.ExportDate = dtpExportForDate.SelectedDate.Value.Date;

            //    if (cmbUnit.SelectedItem != null && ((MasterListItem)cmbUnit.SelectedItem).ID > 0)
            //        tempAccounts.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;

            //    BizAction.Details = tempAccounts;


            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, e) =>
            //    {
            //        if (e.Error == null && e.Result != null)
            //        {
            //            clsAccountsVO objAcc = ((clsGetTotalBillAccountsLedgersVO)e.Result).Details;

            //            if (objAcc.OPDSelfBillLedgers != null)
            //            {
            //                foreach (var item in objAcc.OPDSelfBillLedgers)
            //                {
            //                    switch (item.LedgerName.ToUpper())
            //                    {
            //                        case "CASH":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                            break;
            //                        case "CHQDD":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
            //                            break;
            //                        case "CRDB":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
            //                            break;
            //                        case "CC":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ConsultationLedgerName;
            //                            break;
            //                        case "DC":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.DiagnosticLedgerName;
            //                            break;
            //                        case "OC":
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.OtherServicesLedgerName;
            //                            break;
            //                        default:
            //                            break;
            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.OPDRefundOfBillLedgers != null)
            //                {
            //                    foreach (var item in objAcc.OPDRefundOfBillLedgers)
            //                    {
            //                        switch (item.LedgerName.ToUpper())
            //                        {
            //                            case "CASH":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                                break;
            //                            case "CHQDD":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
            //                                break;
            //                            case "CRDB":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
            //                                break;
            //                            case "CC":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ConsultationLedgerName;
            //                                break;
            //                            case "DC":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.DiagnosticLedgerName;
            //                                break;
            //                            case "OC":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.OtherServicesLedgerName;
            //                                break;
            //                            default:
            //                                break;
            //                        }
            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }


            //                if (objAcc.ItemSaleLedgers != null)
            //                {
            //                    foreach (var item in objAcc.ItemSaleLedgers)
            //                    {
            //                        switch (item.LedgerName.ToUpper())
            //                        {
            //                            case "CASH":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                                break;
            //                            case "CHQDD":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
            //                                break;
            //                            case "CRDB":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
            //                                break;
            //                            case "COGS":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.COGSLedgerName;
            //                                break;
            //                            case "PROFIT":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ProfitLedgerName;
            //                                break;

            //                            default:
            //                                break;
            //                        }
            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.ItemSaleReturnLedgers != null)
            //                {
            //                    foreach (var item in objAcc.ItemSaleReturnLedgers)
            //                    {
            //                        switch (item.LedgerName.ToUpper())
            //                        {
            //                            case "CASH":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                                break;
            //                            case "CHQDD":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
            //                                break;
            //                            case "CRDB":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CrDbBankName;
            //                                break;
            //                            case "COGS":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.COGSLedgerName;
            //                                break;
            //                            case "PROFIT":
            //                                item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ProfitLedgerName;
            //                                break;

            //                            default:
            //                                break;
            //                        }
            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.PurchaseCashLedgers != null)
            //                {
            //                    foreach (var item in objAcc.PurchaseCashLedgers)
            //                    {
            //                       if(item.CR.HasValue)
            //                           item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                       else if(item.DR.HasValue)
            //                           item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.PurchaseCreditLedgers != null)
            //                {
            //                    foreach (var item in objAcc.PurchaseCreditLedgers)
            //                    {
            //                        if (item.DR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.PurchaseReturnCashLedgers != null)
            //                {
            //                    foreach (var item in objAcc.PurchaseReturnCashLedgers)
            //                    {
            //                        if (item.DR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                        else if (item.CR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                if (objAcc.PurchaseReturnCreditLedgers != null)
            //                {
            //                    foreach (var item in objAcc.PurchaseReturnCreditLedgers)
            //                    {
            //                        if (item.CR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.PurchaseLedgerName;

            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }


            //                if (objAcc.ConsumptionLedgers != null)
            //                {
            //                    foreach (var item in objAcc.ConsumptionLedgers)
            //                    {
            //                        if (item.CR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CurrentAssetLedgerName;
            //                        else if (item.DR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ExpenseLedgerName;
            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }


            //                if (objAcc.ScrapLedgers != null)
            //                {
            //                    foreach (var item in objAcc.ScrapLedgers)
            //                    {
            //                        if (item.DR.HasValue)
            //                        {

            //                            switch (item.LedgerName.ToUpper())
            //                            {
            //                                case "CS":
            //                                    item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName;
            //                                    break;
            //                                case "CHQS":
            //                                    item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ChequeDDBankName;
            //                                    break;

            //                                default:
            //                                    break;
            //                            }
            //                        }

            //                        else if (item.CR.HasValue)
            //                            item.LedgerName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.ScrapIncomeLedgerName;

            //                    }
            //                    cmdSave.IsEnabled = true;
            //                }

            //                myAccounts = objAcc;

            //                PagedCollectionView collection1 = new PagedCollectionView(myAccounts.OPDRefundOfBillLedgers);
            //                collection1.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgOPDRefundOfBillLedgers.ItemsSource = null;
            //                dgOPDRefundOfBillLedgers.ItemsSource = collection1;

            //                PagedCollectionView collection = new PagedCollectionView(myAccounts.OPDSelfBillLedgers);
            //                collection.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgOPDSelfBillLedgers.ItemsSource = null;
            //                dgOPDSelfBillLedgers.ItemsSource = collection;


            //                PagedCollectionView collection2 = new PagedCollectionView(myAccounts.ItemSaleLedgers);
            //                collection2.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgSaleCashLedgers.ItemsSource = null;
            //                dgSaleCashLedgers.ItemsSource = collection2;

            //                PagedCollectionView collection3 = new PagedCollectionView(myAccounts.ItemSaleReturnLedgers);
            //                collection3.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgSaleReturnCashLedgers.ItemsSource = null;
            //                dgSaleReturnCashLedgers.ItemsSource = collection3;

            //                PagedCollectionView collection4 = new PagedCollectionView(myAccounts.PurchaseCashLedgers);
            //                collection4.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgPurchaseCashLedgers.ItemsSource = null;
            //                dgPurchaseCashLedgers.ItemsSource = collection4;

            //                PagedCollectionView collection5 = new PagedCollectionView(myAccounts.PurchaseCreditLedgers);
            //                collection5.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgPurchaseCreditLedgers.ItemsSource = null;
            //                dgPurchaseCreditLedgers.ItemsSource = collection5;

            //                PagedCollectionView collection6 = new PagedCollectionView(myAccounts.PurchaseReturnCashLedgers);
            //                collection6.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgPurchaseReturnCashLedgers.ItemsSource = null;
            //                dgPurchaseReturnCashLedgers.ItemsSource = collection6;

            //                PagedCollectionView collection7 = new PagedCollectionView(myAccounts.PurchaseReturnCreditLedgers);
            //                collection7.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgPurchaseReturnCreditLedgers.ItemsSource = null;
            //                dgPurchaseReturnCreditLedgers.ItemsSource = collection7;

            //                PagedCollectionView collection8 = new PagedCollectionView(myAccounts.ConsumptionLedgers);
            //                collection8.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgConsumptionLedgers.ItemsSource = null;
            //                dgConsumptionLedgers.ItemsSource = collection8;

            //                PagedCollectionView collection9 = new PagedCollectionView(myAccounts.ScrapLedgers);
            //                collection9.GroupDescriptions.Add(new PropertyGroupDescription("Clinic"));
            //                dgScrapLedgers.ItemsSource = null;
            //                dgScrapLedgers.ItemsSource = collection9;

            //                this.DataContext = null;
            //                this.DataContext = myAccounts;
            //            }
            //        }
            //        else
            //        {
            //            //Indicatior.Close();

            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //            msgW1.Show();
            //        }
            //        indicator.Close();
            //    };
            // Indicatior.Close();

            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();
            //}
            //catch (Exception)
            //{
            //    indicator.Close();
            //    // throw;
            //}
        }

        private void FillUnit()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- All -"));

                    if (e.Error == null && e.Result != null)
                    {
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    }
                    cmbUnit.ItemsSource = null;
                    cmbUnit.ItemsSource = objList;

                    if (((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId > 0)
                    {
                        cmbUnit.SelectedItem = objList.SingleOrDefault(S => S.ID.Equals(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId));

                    }
                    else
                    {
                        cmbUnit.SelectedValue = objList[0].ID;
                    }
                    if (cmbUnit.ItemsSource != null)
                        FillStores(((MasterListItem)cmbUnit.SelectedItem).ID);

                    //if(!(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO))
                    //{
                    //    cmbUnit.IsEnabled = false;
                    //}
                };
                // Indicatior.Close();

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                ///Indicatior.Close();
                // throw;
            }
        }
        //SaveFileDialog sfd;

        //private void cmdSave_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //        //sfd = new SaveFileDialog();
        //        //sfd.Filter = "Video Files (.wmv) | *.wmv";

        //        //bool? result = sfd.ShowDialog();
        //        //if (result == true)
        //        //    MessageBox.Show("File saved to" + sfd.SafeFileName);

        //        //SaveFileDialog saveFileDialog = new SaveFileDialog();

        //        //saveFileDialog.DefaultExt = "xml";
        //        //saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All files (*.*)|*.*";
        //        //saveFileDialog.FilterIndex = 1;
        //        // Dim result As System.Nullable(Of Boolean) = sfd.ShowDialog()

        //        //if (saveFileDialog.ShowDialog() == true)
        //        //{
        //        //    using (Stream stream = saveFileDialog.OpenFile())
        //        //    {
        //        //        StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.UTF8);
        //        //        sw.Write(GetGeneratedXML().ToString());
        //        //        sw.Close();

        //        //        stream.Close();
        //        //    }
        //        //}

        //        // this.cmdSave.IsEnabled = false;

        //        dialog = new SaveFileDialog { Filter = "XML files|*.xml", DefaultExt = "xml" };
        //        showDialog = dialog.ShowDialog();



        //        this.SaveExport();
        //        //this.Finalizitaion();

        //    }
        //    catch(Exception)
        //    {

        //        throw;
        //    }
        //}

        private void SaveExport()
        {
            //if(showDialog == true)
            //{
            //    using(Stream exportStream = dialog.OpenFile())
            //    {

            //        StreamWriter sw = new StreamWriter(exportStream);
            //        GetGeneratedXML(sw);
            //        sw.Close();

            //        exportStream.Close();

            //    }

            //    this.Finalizitaion();
            //}
        }

        private void Finalizitaion()
        {
            MessageBoxResult result = MessageBox.Show("Export Completed!");

            if (result == MessageBoxResult.OK)
            {
                //this.cmdSave.IsEnabled = true;

            }


        }

        private void cmdGenerateData_Click(object sender, RoutedEventArgs e)
        {
            GenerateData();
        }

        #region Commented Code
        //string qt = @"""";

        //private void GetGeneratedXML(StreamWriter m_streamWriter)
        //{

        //    //Start Writing


        //    m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
        //    //Header Part
        //    m_streamWriter.WriteLine("<ENVELOPE>");
        //    m_streamWriter.WriteLine("<HEADER>");
        //    m_streamWriter.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>");
        //    m_streamWriter.WriteLine("</HEADER>");
        //    m_streamWriter.WriteLine("<BODY>");
        //    m_streamWriter.WriteLine("<IMPORTDATA>");
        //    m_streamWriter.WriteLine("<REQUESTDESC>");
        //    m_streamWriter.WriteLine("<REPORTNAME>All Masters</REPORTNAME>");
        //    m_streamWriter.WriteLine("<STATICVARIABLES>");
        //    m_streamWriter.WriteLine("<SVCURRENTCOMPANY></SVCURRENTCOMPANY>");
        //    m_streamWriter.WriteLine("</STATICVARIABLES>");
        //    m_streamWriter.WriteLine("</REQUESTDESC>");
        //    m_streamWriter.WriteLine("<REQUESTDATA>");

        //    //if (myAccounts.OPDSelfBillLedgers != null && myAccounts.OPDSelfBillLedgers.Count > 0)
        //    //    m_streamWriter = TransferSelfBills(m_streamWriter);

        //    if(myAccounts.OPDSelfCreditBillAccount != null && myAccounts.OPDSelfCreditBillAccount.Count > 0)
        //        m_streamWriter = TransferRefundOfBills(m_streamWriter);

        //    if(myAccounts.OPDCompanyCreditBillAccount != null && myAccounts.OPDCompanyCreditBillAccount.Count > 0)
        //        m_streamWriter = TransferItemSale(m_streamWriter);

        //    //if (myAccounts.ItemSaleReturnLedgers != null && myAccounts.ItemSaleReturnLedgers.Count > 0)
        //    //    m_streamWriter = TransferItemSaleReturn(m_streamWriter);

        //    if(myAccounts.PurchaseCashLedgers != null && myAccounts.PurchaseCashLedgers.Count > 0)
        //        m_streamWriter = TransferCashPurchase(m_streamWriter);

        //    if(myAccounts.PurchaseReturnCashLedgers != null && myAccounts.PurchaseReturnCashLedgers.Count > 0)
        //        m_streamWriter = TransferCashPurchaseReturn(m_streamWriter);


        //    if(myAccounts.PurchaseCreditLedgers != null && myAccounts.PurchaseCreditLedgers.Count > 0)
        //        m_streamWriter = TransferCreditPurchase(m_streamWriter);

        //    if(myAccounts.PurchaseReturnCreditLedgers != null && myAccounts.PurchaseReturnCreditLedgers.Count > 0)
        //        m_streamWriter = TransferCreditPurchaseReturn(m_streamWriter);

        //    if(myAccounts.ConsumptionLedgers != null && myAccounts.ConsumptionLedgers.Count > 0)
        //        m_streamWriter = TransferConsuptionLedgers(m_streamWriter);

        //    if(myAccounts.ScrapLedgers != null && myAccounts.ScrapLedgers.Count > 0)
        //        m_streamWriter = TransferScrapLedgers(m_streamWriter);

        //    //=============================
        //    //Footer

        //    m_streamWriter.WriteLine("</REQUESTDATA>");
        //    m_streamWriter.WriteLine("</IMPORTDATA>");
        //    m_streamWriter.WriteLine("</BODY>");
        //    m_streamWriter.WriteLine("</ENVELOPE>");

        //    //End 
        //    m_streamWriter.Flush();
        //    m_streamWriter.Close();



        //}


        //private StreamWriter TransferConsuptionLedgers(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Scrap Ledgers


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.ConsumptionLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Journal" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Journal" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.ConsumptionLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

        //            }


        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }



        //    return m_streamWriter;
        //}
        //private StreamWriter TransferScrapLedgers(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Scrap Ledgers


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.ScrapLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.ScrapLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");

        //            }


        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }



        //    return m_streamWriter;
        //}

        //private StreamWriter TransferCreditPurchaseReturn(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Purchase Return Credit Ledgers


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.PurchaseReturnCreditLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {

        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Debit Note" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Debit Note" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<STATADJUSTMENTTYPE>Others</STATADJUSTMENTTYPE>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.PurchaseReturnCreditLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {

        //                string str = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                else if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<BILLALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Narration + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("<BILLCREDITPERIOD></BILLCREDITPERIOD>");
        //                    m_streamWriter.WriteLine("<BILLTYPE>Agst Ref</BILLTYPE>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</BILLALLOCATIONS.LIST>");
        //                }

        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");


        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }



        //    return m_streamWriter;
        //}
        //private StreamWriter TransferCreditPurchase(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Credit Purchase


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.PurchaseCreditLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {


        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Purchase" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Purchase" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.PurchaseCreditLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {

        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "Yes";
        //                    amt = item.CR.Value;
        //                }
        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                else if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<BILLALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Narration + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("<BILLCREDITPERIOD></BILLCREDITPERIOD>");
        //                    m_streamWriter.WriteLine("<BILLTYPE>New Ref</BILLTYPE>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</BILLALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }
        //    }

        //    return m_streamWriter;
        //}

        //private StreamWriter TransferCashPurchaseReturn(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Purchase Return Cash Ledgers


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.PurchaseReturnCashLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {


        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");


        //            var results = from a in myAccounts.PurchaseReturnCashLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {

        //                string str = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");


        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");


        //        }


        //    }



        //    return m_streamWriter;
        //}
        //private StreamWriter TransferCashPurchase(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Cash Purchase


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.PurchaseCashLedgers
        //                    group a by a.Narration into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {


        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.PurchaseCashLedgers
        //                          where a.UnitID == (long)mID.Key && a.Narration == mID.Value
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {

        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "Yes";
        //                    amt = item.CR.Value;
        //                }
        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }
        //    }

        //    return m_streamWriter;
        //}

        //private StreamWriter TransferItemSale(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Item sales


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.OPDCompanyCreditBillAccount
        //                    group a by a.UnitID into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Receipt" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Receipt" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.OPDCompanyCreditBillAccount
        //                          where a.UnitID == (long)mID.Key
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }



        //    return m_streamWriter;
        //}
        //private StreamWriter TransferItemSaleReturn(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Item sales Return


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.IPDSelfBillAccount
        //                    group a by a.UnitID into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.IPDSelfBillAccount
        //                          where a.UnitID == (long)mID.Key
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "Yes";
        //                    amt = item.CR.Value;
        //                }
        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }



        //    return m_streamWriter;
        //}

        //private StreamWriter TransferSelfBills(StreamWriter m_streamWriter)
        //{

        //    //===============================
        //    //'    Transfer Self Bills


        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.OPDBillsLedgerAccount
        //                    group a by a.UnitID into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Journal" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Journal" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.OPDBillsLedgerAccount
        //                          where a.UnitID == (long)mID.Key
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    amt = item.CR.Value;
        //                }

        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + str + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                if(item.CR.HasValue && item.CR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }


        //    return m_streamWriter;

        //}
        //private StreamWriter TransferRefundOfBills(StreamWriter m_streamWriter)
        //{
        //    //===============================
        //    //'    Transfer Refund of Bills



        //    List<KeyValue> IDList = new List<KeyValue>();

        //    var resIDList = from a in myAccounts.OPDSelfCreditBillAccount
        //                    group a by a.UnitID into grouped
        //                    select new KeyValue { Key = grouped.First().UnitID, Value = grouped.First().Narration };


        //    IDList = resIDList.ToList();

        //    if(IDList != null && IDList.Count > 0)
        //    {

        //        foreach(var mID in IDList)
        //        {
        //            m_streamWriter.WriteLine(@"<TALLYMESSAGE xmlns:UDF=" + qt + "TallyUDF" + qt + ">"); // + ">");
        //            m_streamWriter.WriteLine(@"<Voucher REMOTEID=".ToUpper() + qt + qt + " VCHTYPE=" + qt + "Payment" + qt + " ACTION=" + qt + "Create" + qt + ">");
        //            m_streamWriter.WriteLine("<Date>".ToUpper() + myAccounts.ExportDate.ToString("yyyyMMdd") + "</Date>".ToUpper());
        //            m_streamWriter.WriteLine("<GUID></GUID>");
        //            m_streamWriter.WriteLine("<NARRATION>" + mID.Value + "</NARRATION>");
        //            m_streamWriter.WriteLine("<VOUCHERTYPENAME>" + "Payment" + "</VOUCHERTYPENAME>");
        //            m_streamWriter.WriteLine("<PARTYLEDGERNAME>" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Accounts.CashLedgerName + "</PARTYLEDGERNAME>");
        //            m_streamWriter.WriteLine("<CSTFORMISSUETYPE/>");
        //            m_streamWriter.WriteLine("<CSTFORMRECVTYPE/>");
        //            m_streamWriter.WriteLine("<FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
        //            m_streamWriter.WriteLine("<VCHGSTCLASS/>");
        //            m_streamWriter.WriteLine("<DIFFACTUALQTY>No</DIFFACTUALQTY>");
        //            m_streamWriter.WriteLine("<AUDITED>No</AUDITED>");
        //            m_streamWriter.WriteLine("<FORJOBCOSTING>No</FORJOBCOSTING>");
        //            m_streamWriter.WriteLine("<ISOPTIONAL>No</ISOPTIONAL>");
        //            m_streamWriter.WriteLine("<EFFECTIVEDATE>" + myAccounts.ExportDate.ToString("yyyyMMdd") + "</EFFECTIVEDATE>");
        //            m_streamWriter.WriteLine("<USEFORINTEREST>No</USEFORINTEREST>");
        //            m_streamWriter.WriteLine("<USEFORGAINLOSS>No</USEFORGAINLOSS>");
        //            m_streamWriter.WriteLine("<USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
        //            m_streamWriter.WriteLine("<USEFORCOMPOUND>No</USEFORCOMPOUND>");
        //            m_streamWriter.WriteLine("<ALTERID> 2666</ALTERID>");
        //            m_streamWriter.WriteLine("<EXCISEOPENING>No</EXCISEOPENING>");
        //            m_streamWriter.WriteLine("<USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION>");
        //            m_streamWriter.WriteLine("<ISCANCELLED>No</ISCANCELLED>");
        //            m_streamWriter.WriteLine("<HASCASHFLOW>Yes</HASCASHFLOW>");
        //            m_streamWriter.WriteLine("<ISPOSTDATED>No</ISPOSTDATED>");
        //            m_streamWriter.WriteLine("<USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
        //            m_streamWriter.WriteLine("<ISINVOICE>No</ISINVOICE>");
        //            m_streamWriter.WriteLine("<MFGJOURNAL>No</MFGJOURNAL>");
        //            m_streamWriter.WriteLine("<HASDISCOUNTS>No</HASDISCOUNTS>");
        //            m_streamWriter.WriteLine("<ASPAYSLIP>No</ASPAYSLIP>");
        //            m_streamWriter.WriteLine("<ISCOSTCENTRE>No</ISCOSTCENTRE>");
        //            m_streamWriter.WriteLine("<ISDELETED>No</ISDELETED>");
        //            m_streamWriter.WriteLine("<ASORIGINAL>No</ASORIGINAL>");

        //            var results = from a in myAccounts.OPDSelfCreditBillAccount
        //                          where a.UnitID == (long)mID.Key
        //                          orderby a.DR descending
        //                          select a;

        //            List<clsLedgerVO> objList = new List<clsLedgerVO>();
        //            objList = results.ToList();

        //            foreach(var item in objList)
        //            {
        //                string str = "";
        //                string strPriVch = "";
        //                double amt = 0;

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    str = "Yes";
        //                    strPriVch = "No";
        //                    amt = -item.DR.Value;
        //                }
        //                else
        //                {
        //                    str = "No";
        //                    strPriVch = "Yes";
        //                    amt = item.CR.Value;
        //                }
        //                string strAmt = String.Format("{0:0.00}", amt);

        //                m_streamWriter.WriteLine("<ALLLEDGERENTRIES.LIST>");
        //                m_streamWriter.WriteLine("<LEDGERNAME>" + item.LedgerName + "</LEDGERNAME>");
        //                m_streamWriter.WriteLine("<GSTCLASS/>");
        //                m_streamWriter.WriteLine("<ISDEEMEDPOSITIVE>" + str + "</ISDEEMEDPOSITIVE>");
        //                m_streamWriter.WriteLine("<LEDGERFROMITEM>No</LEDGERFROMITEM>");
        //                m_streamWriter.WriteLine("<REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
        //                m_streamWriter.WriteLine("<ISPARTYLEDGER>" + strPriVch + "</ISPARTYLEDGER>");
        //                m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");

        //                if(item.DR.HasValue && item.DR > 0)
        //                {
        //                    m_streamWriter.WriteLine("<CATEGORYALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<CATEGORY>Primary Cost Category</CATEGORY>");
        //                    m_streamWriter.WriteLine("<COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("<NAME>" + item.Clinic + "</NAME>");
        //                    m_streamWriter.WriteLine("<AMOUNT>" + strAmt + "</AMOUNT>");
        //                    m_streamWriter.WriteLine("</COSTCENTREALLOCATIONS.LIST>");
        //                    m_streamWriter.WriteLine("</CATEGORYALLOCATIONS.LIST>");
        //                }
        //                m_streamWriter.WriteLine("</ALLLEDGERENTRIES.LIST>");
        //            }

        //            m_streamWriter.WriteLine("</Voucher>".ToUpper());
        //            m_streamWriter.WriteLine("</TALLYMESSAGE>");

        //        }


        //    }


        //    return m_streamWriter;
        //}
        #endregion

        private void cmdTransferInventory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdTransferDoctorBill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdExportMaster_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdCancle_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbEMRInformation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region Generate XML Code

        long Count = 0;
        private void cmdTransferOPD_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator wait = new WaitIndicator();
            wait.Show();
            Count = 0;
            DeleteFiles();
            wait.Close();
        }

        private void DeleteFiles()
        {
            clsGetTotalBillAccountsLedgersVO BizAction = new clsGetTotalBillAccountsLedgersVO();
            BizAction.IsDeleteFiles = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Result != null)
                {
                    GenrateXML();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GenrateXML()
        {
            if (objAccount != null)
            {
                //DeleteFiles();
                ENVELOPE EnvOPD = new ENVELOPE();
                GenerateXML(EnvOPD, Convert.ToInt64(IPDOPDSale.OPD));

                ENVELOPE EnvIPD = new ENVELOPE();
                GenerateXML(EnvIPD, Convert.ToInt64(IPDOPDSale.IPD));

                ENVELOPE EnvSale = new ENVELOPE();
                GenerateXML(EnvSale, Convert.ToInt64(IPDOPDSale.Sale));

                ENVELOPE EnvMisc = new ENVELOPE();
                GenerateXML(EnvMisc, Convert.ToInt64(IPDOPDSale.Misc));

                ENVELOPE EnvDoct = new ENVELOPE();
                GenerateXML(EnvDoct, Convert.ToInt64(IPDOPDSale.Doctor));
            }
        }

        public void GenerateXML(ENVELOPE Env, long OPDIPDSaleID)
        {
            WaitIndicator wait = new WaitIndicator();
            wait.Show();
            try
            {
                clsGetTotalBillAccountsLedgersVO BizAction = new clsGetTotalBillAccountsLedgersVO();
                BizAction.ENVELOPEList = new List<ENVELOPE>();
                BizAction.IsGenrateXML = true;
                Env.Header = GetHeader();
                if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.OPD))
                {
                    BizAction.strGenrateXMLName = "OPD";
                }
                if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.IPD))
                {
                    BizAction.strGenrateXMLName = "IPD";
                }
                else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Sale))
                {
                    BizAction.strGenrateXMLName = "Inventory";
                }
                else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Misc))
                {
                    BizAction.strGenrateXMLName = "Misc";
                }
                else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Doctor))
                {
                    BizAction.strGenrateXMLName = "Doctor";
                }
                Env.Body = GetBody(OPDIPDSaleID);
                BizAction.ENVELOPEList.Add(Env);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Result != null)
                    {
                        Count++;
                        if (Count == 4)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "XML generated successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            wait.Close();
                        }
                    }
                    wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
            }
        }

        private HEADER GetHeader()
        {
            HEADER _H = new HEADER();
            _H.VERSION = 1;
            _H.TALLYREQUEST = "Import";
            _H.TYPE = "Data";
            _H.ID = "Vouchers";
            return _H;
        }

        private BODY GetBody(long OPDIPDSaleID)
        {
            BODY _B = new BODY();
            _B.DESC = new Desc();
            _B.DATA = new List<DATA>();

            #region OPD
            if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.OPD))
            {
                if (objAccount.OPDBillsLedgerAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDBillsLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDBillsLedgerAccount, true));
                    }
                }

                if (objAccount.OPDSelfCreditBillAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDSelfCreditBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDSelfCreditBillAccount, true));
                    }
                }

                if (objAccount.OPDReceiptLedgerAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDReceiptLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDReceiptLedgerAccount, true));
                    }
                }

                if (objAccount.OPDCompanyCreditBillAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDCompanyCreditBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDCompanyCreditBillAccount, true));
                    }
                }

                if (objAccount.OPDCompanyReceiptBillAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDCompanyReceiptBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDCompanyReceiptBillAccount, true));
                    }
                }

                if (objAccount.OPDAdvanceLedgerAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDAdvanceLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDAdvanceLedgerAccount, true));
                    }
                }

                if (objAccount.OPDAdvanceRefundLedgerAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDAdvanceRefundLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDAdvanceRefundLedgerAccount, true));
                    }
                }

                if (objAccount.OPDRefundBillLedgerAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.OPDRefundBillLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.OPDRefundBillLedgerAccount, true));
                    }
                }
            }
            #endregion

            #region IPD
            else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.IPD))
            {
                if (objAccount.IPDSelfBillAccount != null)
                {
                    foreach (clsLedgerVO item in objAccount.IPDSelfBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.IPDSelfBillAccount, true));
                    }
                }

                if (objAccount.IPDSelfCreditForTallyInterface != null)
                {
                    foreach (clsLedgerVO item in objAccount.IPDSelfCreditForTallyInterface.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.IPDSelfCreditForTallyInterface, true));
                    }
                }

                //if (objAccount.IPDReceiptLedgerAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDReceiptLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDReceiptLedgerAccount, false));
                //    }
                //}

                //if (objAccount.IPDCompanyBillAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDCompanyBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDCompanyBillAccount, false));
                //    }
                //}

                //if (objAccount.IPDCompanyReceiptBillAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDCompanyReceiptBillAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDCompanyReceiptBillAccount, false));
                //    }
                //}

                //if (objAccount.IPDAdvanceLedgerAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDAdvanceLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDAdvanceLedgerAccount, false));
                //    }
                //}

                //if (objAccount.IPDAdvanceRefundLedgerAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDAdvanceRefundLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDAdvanceRefundLedgerAccount, false));
                //    }
                //}

                //if (objAccount.IPDRefundBillLedgerAccount != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.IPDRefundBillLedgerAccount.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.IPDRefundBillLedgerAccount, false));
                //    }
                //}

                //if (objAccount.MiscIncomeLedger != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.MiscIncomeLedger.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.MiscIncomeLedger, false));
                //    }
                //}

                //if (objAccount.MiscExpenseLedgers != null)
                //{
                //    foreach (clsLedgerVO item in objAccount.MiscExpenseLedgers.Where(S => S.RowID.Equals(1)).ToList())
                //    {
                //        _B.DATA.Add(GetData(item, objAccount.MiscExpenseLedgers, false));
                //    }
                //}

            }
            #endregion

            #region Sale
            else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Sale))
            {

                if (objAccount.PurchaseLedger != null)
                {
                    foreach (clsLedgerVO item in objAccount.PurchaseLedger.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.PurchaseLedger, true));
                    }
                }

                if (objAccount.PurchaseReturnLedger != null)
                {
                    foreach (clsLedgerVO item in objAccount.PurchaseReturnLedger.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.PurchaseReturnLedger, true));
                    }
                }

                if (objAccount.SaleIncome != null)
                {
                    foreach (clsLedgerVO item in objAccount.SaleIncome.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.SaleIncome, true));
                    }
                }

                if (objAccount.SaleCredit != null)
                {
                    foreach (clsLedgerVO item in objAccount.SaleCredit.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.SaleCredit, true));
                    }
                }

                if (objAccount.SaleCompanyReceiptLedgers != null)
                {
                    foreach (clsLedgerVO item in objAccount.SaleCompanyReceiptLedgers.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.SaleCompanyReceiptLedgers, true));
                    }
                }

                if (objAccount.SaleSelfReceiptLedgers != null)
                {
                    foreach (clsLedgerVO item in objAccount.SaleSelfReceiptLedgers.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.SaleSelfReceiptLedgers, true));
                    }
                }

                if (objAccount.SaleReturn != null)
                {
                    foreach (clsLedgerVO item in objAccount.SaleReturn.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.SaleReturn, true));
                    }
                }

            }
            #endregion

            #region Misc

            else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Misc))
            {
                if (objAccount.MiscIncomeLedger != null)
                {
                    foreach (clsLedgerVO item in objAccount.MiscIncomeLedger.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.MiscIncomeLedger, true));
                    }
                }
                if (objAccount.MiscExpenseLedgers != null)
                {
                    foreach (clsLedgerVO item in objAccount.MiscExpenseLedgers.Where(S => S.RowID.Equals(1)).ToList())
                    {
                        _B.DATA.Add(GetData(item, objAccount.MiscExpenseLedgers, true));
                    }
                }
            }

            #endregion

            #region Doctor
            //else if (OPDIPDSaleID == Convert.ToInt64(IPDOPDSale.Sale))
            //{
            //    if (objAccount.DoctorLedgers != null)
            //    {
            //        foreach (clsLedgerVO item in objAccount.DoctorLedgers.Where(S => S.RowID.Equals(1)).ToList())
            //        {
            //            _B.DATA.Add(GetData(item, objAccount.DoctorLedgers, true));
            //        }
            //    }

            //    if (objAccount.DoctorPaymentLedgers != null)
            //    {
            //        foreach (clsLedgerVO item in objAccount.DoctorPaymentLedgers.Where(S => S.RowID.Equals(1)).ToList())
            //        {
            //            _B.DATA.Add(GetData(item, objAccount.DoctorPaymentLedgers, true));
            //        }
            //    }            

            //}
            #endregion
            return _B;
        }

        private DATA GetData(clsLedgerVO item, List<clsLedgerVO> itemList, bool IsConsolated)
        {
            DATA _Data = new DATA();
            _Data.TALLYMESSAGE = GetTALLYMESSAGE(item, itemList, IsConsolated);
            return _Data;
        }

        private TALLYMESSAGE GetTALLYMESSAGE(clsLedgerVO item, List<clsLedgerVO> itemList, bool IsConsolated)
        {
            TALLYMESSAGE _msg = new TALLYMESSAGE();
            _msg.Voucher = GetVoucher(item, itemList, IsConsolated);
            return _msg;
        }

        private VOUCHER GetVoucher(clsLedgerVO item, List<clsLedgerVO> itemList, bool IsConsolated)
        {
            VOUCHER _voucher = new VOUCHER();
            _voucher.VCHTYPE = item.VoucherType;
            _voucher.ACTION = "Create";
            _voucher.DATE = dtpExportForDate.SelectedDate.Value.Date.ToString("yyyyMMdd");
            _voucher.NARRATION = item.Narration;
            _voucher.VOUCHERTYPENAME = item.VoucherType;
            _voucher.EFFECTIVEDATE = dtpExportForDate.SelectedDate.Value.Date.ToString("yyyyMMdd");

            _voucher.ALLEDGERENTRIES_LIST = GetALLEDGERENTRIESList(item, itemList, IsConsolated);
            return _voucher;
        }

        private List<ALLEDGERENTRIES> GetALLEDGERENTRIESList(clsLedgerVO item, List<clsLedgerVO> itemList, bool IsConsolated)
        {
            List<ALLEDGERENTRIES> ALLEDGERENTRIES_LIST = new List<ALLEDGERENTRIES>();
            try
            {
                if (IsConsolated)
                {
                    foreach (clsLedgerVO _item in itemList)
                    {
                        ALLEDGERENTRIES objLedger = new ALLEDGERENTRIES();
                        objLedger.LEDGERNAME = _item.LedgerName;
                        if (_item.DR > 0)
                        {
                            objLedger.ISDEEMEDPOSITIVE = "Yes";
                            objLedger.AMOUNT = -_item.DR;
                        }
                        else
                        {
                            objLedger.ISDEEMEDPOSITIVE = "No";
                            objLedger.AMOUNT = _item.CR;
                        }

                        //if(_item.IsCredit)
                        //{
                        //    objLedger.BILLALLOCATIONS = new BILLALLOCATIONS();
                        //    objLedger.BILLALLOCATIONS.BILLTYPE = _item.VoucherMode;    //"New Ref";
                        //    objLedger.BILLALLOCATIONS.AMOUNT = _item.CR;
                        //    objLedger.BILLALLOCATIONS.NAME = _item.Reference;
                        //}
                        ALLEDGERENTRIES_LIST.Add(objLedger);
                    }
                }
                else
                {
                    foreach (clsLedgerVO _item in itemList.Where(S => S.Reference.Equals(item.Reference)))
                    {
                        ALLEDGERENTRIES objLedger = new ALLEDGERENTRIES();
                        objLedger.LEDGERNAME = _item.LedgerName;
                        if (_item.DR > 0)
                        {
                            objLedger.ISDEEMEDPOSITIVE = "Yes";
                            objLedger.AMOUNT = -_item.DR;
                        }
                        else
                        {
                            objLedger.ISDEEMEDPOSITIVE = "No";
                            objLedger.AMOUNT = _item.CR;
                        }

                        if (_item.IsCredit)
                        {
                            objLedger.BILLALLOCATIONS = new BILLALLOCATIONS();
                            objLedger.BILLALLOCATIONS.BILLTYPE = _item.VoucherMode;
                            objLedger.BILLALLOCATIONS.AMOUNT = _item.CR;
                            objLedger.BILLALLOCATIONS.NAME = _item.Reference;
                        }
                        ALLEDGERENTRIES_LIST.Add(objLedger);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return ALLEDGERENTRIES_LIST;
        }

        #endregion

        #region Save Functionality

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "";
            string msgText = "Are you sure you want to Save ?";

            MessageBoxControl.MessageBoxChildWindow msgW =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

            msgW.Show();
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();
            }
        }

        private void Save()
        {
            try
            {
                clsAddVoucherHeaderBizActionVO BizAction = new clsAddVoucherHeaderBizActionVO();
                BizAction.Details = objAccount;
                BizAction.Details.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
                BizAction.Details.ExportDate = Convert.ToDateTime(dtpExportForDate.SelectedDate);
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if ((clsAddVoucherHeaderBizActionVO)arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("", "Data saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        private void cmbUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbUnit.ItemsSource != null)
                if (!((MasterListItem)cmbUnit.SelectedItem).ID.Equals(0))
                {
                    FillStores(((MasterListItem)cmbUnit.SelectedItem).ID);
                }
        }
    }

    public enum IPDOPDSale
    {
        OPD = 1,
        IPD = 2,
        Sale = 3,
        Misc = 4,
        Doctor = 5,
    }
}
