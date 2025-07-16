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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Inventory;
using CIMS;
//using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.IPD.Forms;
using PalashDynamics.ValueObjects.Patient;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.Collections;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;

namespace PalashDynamics.IPD
{
    public partial class frmPharmacyBill : UserControl
    {
        WaitIndicator Indicatior = null;
        public ObservableCollection<clsItemSalesDetailsVO> PharmacyItems { get; set; }
        public ObservableCollection<clsBillVO> DataList { get; private set; }

        public long StoreID { get; set; }
        clsBillVO SelectedBill { get; set; }
        WaitIndicator indicator = new WaitIndicator();
        public frmPharmacyBill()
        {
            InitializeComponent();
            FillBillSearchList();
            InitialiseForm();
            FillPharmacyItemsList();


        }

        private void InitialiseForm()
        {
            DataList = new ObservableCollection<clsBillVO>();
            PharmacyItems = new ObservableCollection<clsItemSalesDetailsVO>();
            dgPharmacyItems.ItemsSource = PharmacyItems;
            dgPharmacyItems.UpdateLayout();
        }
        private void FillPharmacyItemsList()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetItemSalesCompleteBizActionVO BizAction = new clsGetItemSalesCompleteBizActionVO();
                BizAction.AdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
                BizAction.AdmissionUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient Client = new PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetItemSalesCompleteBizActionVO)arg.Result).Details != null)
                        {
                            clsItemSalesVO mobj = ((clsGetItemSalesCompleteBizActionVO)arg.Result).Details;
                            StoreID = mobj.StoreID;
                            foreach (var item in mobj.Items)
                            {
                                if (item.ConcessionPercentageBill > 100)
                                {
                                    item.ConcessionPercentageBill = 100;
                                }
                                PharmacyItems.Add(item);
                            }

                            dgPharmacyItems.Focus();
                            dgPharmacyItems.UpdateLayout();
                            CalculatePharmacySummaryForBill();
                        }
                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void CalculatePharmacySummaryForBill()
        {
            double Total, Concession, NetAmount, TotalVat, NetBillAmount;
            Total = Concession = NetAmount = TotalVat = NetBillAmount = 0;
            for (int i = 0; i < PharmacyItems.Count; i++)
            {
                Total += (PharmacyItems[i].AmountBill);
                Concession += PharmacyItems[i].ConcessionAmountBill;
                TotalVat += PharmacyItems[i].VATAmountBill;
                NetAmount += PharmacyItems[i].NetAmountBill;
                NetBillAmount = PharmacyItems[i].NetAmount;
            }
            txtPharmacyTotal.Text = String.Format("{0:0.00}", Total);
            txtPharmacyConcession.Text = String.Format("{0:0.00}", Concession);
            txtPharmacyVatAmount.Text = String.Format("{0:0.00}", TotalVat);
            //txtPharmacyNetAmount.Text = String.Format("{0:0.00}", NetAmount);
           // txtNetAmount.Text = String.Format("{0:0.00}", NetBillAmount);
            txtNetAmount.Text = Math.Round(NetAmount).ToString();

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            SelectedBill = new clsBillVO();
            SelectedBill = (clsBillVO)dgBillList.SelectedItem;
            isValid = CheckValidations();
            if (isValid)
            {
                PaymentWindow SettlePaymentWin = new PaymentWindow();              
                SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;
                SettlePaymentWin.Initiate("Bill");

              //  SettlePaymentWin.txtPayTotalAmount.Text = txtNetAmount.Text;
                //SettlePaymentWin.txtPayTotalAmount.Text = txtPharmacyTotal.Text;
                SettlePaymentWin.txtPayTotalAmount.Text = txtNetAmount.Text;
                SettlePaymentWin.txtDiscAmt.Text = txtPharmacyConcession.Text;
               // SettlePaymentWin.txtPayableAmt.Text = txtNetAmount.Text;

                SettlePaymentWin.txtPayableAmt.Text = txtNetAmount.Text;
                SettlePaymentWin.TotalAmount = double.Parse(txtNetAmount.Text);

                SettlePaymentWin.Opd_Ipd_External = 1;
                SettlePaymentWin.IsFromPharmacyBill = true;
                SettlePaymentWin.PatientCategoryID = SelectedBill.PatientCategoryId;
                SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
                SettlePaymentWin.Show();
            }
        }


        void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((PaymentWindow)sender).Payment != null)
                {

                    Indicatior.Show();
                    clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                    clsPaymentVO pPayDetails = new clsPaymentVO();

                    pPayDetails = ((PaymentWindow)sender).Payment;
                    pPayDetails.IsBillSettlement = false;
                    pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                    BizAction.Details = pPayDetails;

                    BizAction.Details.BillID = SelectedBill.ID;
                    BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                    BizAction.Details.Date = DateTime.Now;
                    double ConAmt = 0;

                    BizAction.Details.BillBalanceAmount = ((PaymentWindow)sender).TotalAmount - pPayDetails.PaidAmount;
                    clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();
                    SelectedBill.IsFreezed = true; 
                    mybillPayDetails.Details = SelectedBill;
                    //mybillPayDetails.Details.BalanceAmountSelf = ((PaymentWindow)sender).TotalAmount - pPayDetails.PaidAmount; //***//
                    ConAmt = 0;

                    mybillPayDetails.Details.BalanceAmountSelf = Math.Round(mybillPayDetails.Details.BalanceAmountSelf, 0) - (pPayDetails.PaidAmount + ConAmt);
                    mybillPayDetails.Details.IsFreezed = true;
                    mybillPayDetails.Details.ISForMaterialConsumption = true;
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

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient Client = new PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null)
                        {
                            if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                Indicatior.Close();

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWD.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                    // PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                                    PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
                                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                    frmAdmissionList _AdmissionListObject = new frmAdmissionList();
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                    mElement.Text = "Admission List";
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
            catch (Exception)
            {
                Indicatior.Close();
            }

        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
        }

        private void FillBillSearchList()
        {

            indicator.Show();

            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.Opd_Ipd_External_Id = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmID;
            BizAction.Opd_Ipd_External_UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionUnitID;
            BizAction.Opd_Ipd_External = 1;
            BizAction.BillType = (BillTypes)(2); //BizAction.BillType = (BillTypes)(2);
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = BizAction.PatientUnitID.Value;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IPDCounterID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient Client = new PalashDynamics.Service.PalashTestServiceReference.PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    if (result.List != null)
                    {
                        dgBillList.SelectedItem = null;
                        foreach (var item in result.List)
                        {
                            DataList.Add(item);
                            dgBillList.SelectedItem = item;
                        }
                        var itemlist = result.List.ToList().Where(t => t.IsFreezed == true).ToList();
                        if (itemlist.Count > 0)
                        {
                            chkFreezBill.IsChecked = true;
                            cmdSave.IsEnabled = false;
                        }
                    }
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void PrintPharmacyBill(long iBillId, long IUnitID, long PrintID)
        {
            if (iBillId > 0)
            {
                long UnitID = IUnitID;
                string URL = "../Reports/OPD/PharmacyBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID + "&IsIPDBill=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (((clsBillVO)dgBillList.SelectedItem) != null)
            {
                PrintPharmacyBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, 4);
            }
        }

        private bool CheckValidations()
        {
            bool isValid = true;

            if (PharmacyItems.Count == 0)
            {
                isValid = false;
                string msgText = "";
                msgText = "You Can Not Save The Bill Without Pharmacy Items";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD.Show();
            }
            if ((clsBillVO)dgBillList.SelectedItem != null)
            {
                if ((SelectedBill.BalanceAmountSelf == 0 || SelectedBill.BalanceAmountSelf == null) && SelectedBill.TotalBillAmount != SelectedBill.TotalConcessionAmount)
                {
                    isValid = false;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("", "Your Pharmacy Bill Is Paid.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }

            return isValid;

        }
    }
}
