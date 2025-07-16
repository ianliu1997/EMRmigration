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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using System.Collections.ObjectModel;
using System.Windows.Browser;
using CIMS.Forms;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects;

namespace OPDModule.Forms
{
    public partial class FrmCompanyBillForSettle : ChildWindow
    {

        WaitIndicator Indicator = null;
        public DateTime dtFrom;
        public DateTime dtTO;
        public DateTime dtBillDate;
        public long PatientSourceID;
        public long loBillid;
        public string loInvoiceNo;
        public string loBillNo;
        public long loCompanyId;
        public string strCompanyName;
        public long companyID = 0;
        public long loAssoCompanyId;
        public string strAssoCompanyName;
        public bool isPaidbill = false;
        public long Ispaid;
        bool isTextChange = false;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        public long InvoiceID = 0;
        public long UnitID = 0;
        List<clsCompanyPaymentDetailsVO> lstBill;
        List<clsCompanyPaymentDetailsVO> lstCompanyPaymentDetails;
       public double InvoiceAmount = 0.00;
       public double InvoiceBalanceAmount = 0.00;

        ObservableCollection<clsCompanyPaymentDetailsVO> BillList { get; set; }
        ObservableCollection<clsCompanyPaymentDetailsVO> SelectedBillList { get; set; }

        public FrmCompanyBillForSettle()
        {
            InitializeComponent();
            lstCompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();

            Indicator = new WaitIndicator();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new clsCompanyPaymentDetailsVO()
            {
                UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID,
                UnitName = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitName,
                DepartmentId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID
            };

            BillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
            SelectedBillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
            tbFromDate1.Text = dtBillDate.ToShortDateString();
            tbCompanyName1.Text = strCompanyName;
            lstBill = new List<clsCompanyPaymentDetailsVO>();
        }

        public void FillBillSearchList(long InvoiceID, long UnitID)
        {

            clsGetBillAgainstInvoiceBizActionVO BizAction = new clsGetBillAgainstInvoiceBizActionVO();
            BizAction.InvoiceID = InvoiceID;
            BizAction.UnitID = UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    clsGetBillAgainstInvoiceBizActionVO result = e.Result as clsGetBillAgainstInvoiceBizActionVO;
                    BillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
                    SelectedBillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
                    SelectedBill = new clsCompanyPaymentDetailsVO();
                    lstChargeID = new List<clsChargeVO>();
                    lstCompanyPaymentDetails = new List<clsCompanyPaymentDetailsVO>();
                    if (result.List != null)
                    {
                        foreach (var item in result.List)
                        {
                            if (item.BalanceAmountSelf > 0)
                            {

                                item.IsEnable = true;
                                //txTAmount.Text = (item.BalanceAmountSelf).ToString() ; 
                            }
                            else
                            {
                                item.IsEnable = false;
                            }

                            BillList.Add(item);
                        }
                    }
                    dgBillList.ItemsSource = null;
                    dgBillList.ItemsSource = BillList;



                    //txTAmount.Text = BillList.Sum(z => z.BalanceAmountSelf).ToString();

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        int ClickedFlag = 0;
        public bool IsCompanyBill = false;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            IsCompanyBill = true;
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (SelectedBillList.Count > 0)
                {
                    double Amt = SelectedBillList.Sum(z => z.BalanceAmountSelf);
                    double NewValue = 0;
                    NewValue = Convert.ToDouble(txTAmount.Text);
                    if (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "")
                    {
                        NewValue = Convert.ToDouble(txTAmount.Text) - Convert.ToDouble(txtReceivedAmount.Text);
                    }
                    if (String.IsNullOrEmpty(txtTDS.Text))
                    {
                        txtTDS.Text = "0";
                    }

                    if ((txtReceivedAmount.Text != null && txtReceivedAmount.Text != "") && Convert.ToDouble(txtReceivedAmount.Text) + Convert.ToDouble(txtTDS.Text) > Amt)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgWD =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", " Amount Should Be Less Than Paid Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWD.Show();
                        txtReceivedAmount.Text ="0";
                        txtTDS.Text = "0";
                        ClickedFlag = 0;
                    }

                    if (txtTDS.Text != null && txtTDS.Text != "" && txtReceivedAmount.Text != null && txtReceivedAmount.Text != "")
                    {
                        NewValue = Convert.ToDouble(txTAmount.Text) - (Convert.ToDouble(txtReceivedAmount.Text)); // + Convert.ToDouble(txtTDS.Text));
                    }

                    if (ClickedFlag != 0)
                    {
                        if (txtTDS.Text != null && txtTDS.Text != "")
                        {
                            if (Convert.ToDouble(txtTDS.Text) > NewValue)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "TDS Should be less than Paid Amount ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                                txtTDS.Text = "";
                                ClickedFlag = 0;
                            }
                        }
                    }

                    if (ClickedFlag != 0)
                    {
                        if ((txtTDS.Text != null && txtTDS.Text != "") && (txtReceivedAmount.Text != "0") && (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "" && txtReceivedAmount.Text != null) || (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "" && txtReceivedAmount.Text != null && (txtReceivedAmount.Text != "0")))
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Settle the Bill ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            msgWD.OnMessageBoxClosed += (arg) =>
                            {
                                if (arg == MessageBoxResult.Yes)
                                {
                                    bool isValid = true;
                                    if (isValid)
                                    {
                                        if (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "")
                                        {
                                            PaymentWindow SettlePaymentWin = new PaymentWindow();
                                            SettlePaymentWin.IscompanyBill = IsCompanyBill;
                                            SettlePaymentWin.Initiate("Bill");
                                            double TotalAmt = 0;
                                            double DiscAmt = 0;
                                            double PayableAmt = 0;
                                            foreach (var item in SelectedBillList)
                                            {
                                                if (item.BalanceAmountSelf > 0)
                                                    SettlePaymentWin.TotalAmount += item.BalanceAmountSelf;
                                              

                                                SettlePaymentWin.ConcessionFromPlan = item.IsConcessionAthorization;
                                                SettlePaymentWin.CompanyName = strCompanyName;
                                                SettlePaymentWin.CompanyID = companyID;
                                                TotalAmt += item.NetBillAmount;
                                                DiscAmt += item.TotalConcessionAmount;
                                                PayableAmt += item.BalanceAmountSelf;
                                                SettlePaymentWin.cmbCheckAuthBy.SelectedValue = item.PaymentDetails.ChequeAuthorizedBy;
                                                SettlePaymentWin.txtNarration.Text = item.PaymentDetails.PayeeNarration;
                                                SettlePaymentWin.IsSettleMentBill = true;
                                                SettlePaymentWin.TxtConAmt.Visibility = System.Windows.Visibility.Collapsed;
                                                //SettlePaymentWin.dgCompanyAdvDtl.Visibility = Visibility.Collapsed;
                                                //SettlePaymentWin.dgCompanyAdvDtl.Visibility = Visibility.Collapsed;
                                                SettlePaymentWin.TxtpaidAmt.IsEnabled = false;
                                                // SettlePaymentWin.CompanyBill = true;
                                            }

                                            SettlePaymentWin.txtPayTotalAmount.Text = TotalAmt.ToString();
                                            SettlePaymentWin.txtDiscAmt.Text += DiscAmt.ToString();
                                            SettlePaymentWin.txtPayableAmt.Text += PayableAmt.ToString();
                                            double ReceivedAmt = 0.0;

                                            if (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "")
                                            {
                                                ReceivedAmt = Convert.ToDouble(txtReceivedAmount.Text);
                                            }

                                            SettlePaymentWin.TotalAmount = ReceivedAmt;

                                            //if (txtTDS.Text != null && txtTDS.Text != "")
                                            //    SettlePaymentWin.TDSAmount = Convert.ToDouble(txtTDS.Text);

                                            //SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
                                            //SettlePaymentWin.IsCompanySettlement = true;
                                            SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettleMultiplePaymentWin_OnSaveButton_Click);
                                            SettlePaymentWin.Show();
                                            ClickedFlag = 0;
                                        }
                                        else
                                        {
                                            //clsPaymentVO Payment = new clsPaymentVO();
                                            //Payment.BillAmount = Convert.ToDouble(txTAmount.Text);
                                            //Payment.BillBalanceAmount = Payment.BillAmount - Convert.ToDouble(txtTDS.Text);
                                            //SaveBill(Payment);
                                        }
                                    }
                                }

                                else
                                {
                                    ClickedFlag = 0;

                                    lstChargeID = new List<clsChargeVO>();

                                }

                            };
                            msgWD.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please enter  Amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgWD.Show();
                           
                        }
                    }
                    ClickedFlag = 0;

                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select the Bills", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                    msgWD.Show();
                    ClickedFlag = 0;
                }
            }


        }

        clsPaymentVO pPayDetails = new clsPaymentVO();

        private void SaveBill(clsPaymentVO PayDetailsVO)
        {
            SettleMultipleBill();
        }

        void SettleMultiplePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        //double TDS = 0.0;
                        //if (txtTDS.Text != "" && txtTDS.Text != null)
                        //{
                        //    TDS = Convert.ToDouble(txtTDS.Text);
                        //}
                        pPayDetails = new clsPaymentVO();
                        pPayDetails = ((PaymentWindow)sender).Payment;
                        var results = from r in SelectedBillList
                                      orderby r.Date, r.ID
                                      select r;

                        string strBillId = null;
                        double TAmt = pPayDetails.PaidAmount  ;
                        double BalAmt = 0;
                        double ConsumeAmt = 0;
                        long UnitID = 0;
                        foreach (var item in results.ToList())
                        {
                            BalAmt = item.BalanceAmountSelf;

                            if (TAmt > 0 && BalAmt > 0)
                            {
                                ConsumeAmt = BalAmt;
                                if (TAmt >= ConsumeAmt)
                                {
                                    TAmt = TAmt - ConsumeAmt;
                                }
                                else
                                {
                                    ConsumeAmt = TAmt;
                                    TAmt = TAmt - ConsumeAmt;
                                }



                                item.TempBalanceAmount = BalAmt - ConsumeAmt ;
                                item.TempPaidAmount = (item.BalanceAmountSelf - item.TempBalanceAmount);
                                strBillId = strBillId + item.ID;
                                strBillId = strBillId + ",";

                                UnitID = item.UnitID;
                            }
                            else
                            {
                                if (SelectedBillList.Count > 0)
                                {
                                    SelectedBillList.Remove(item);
                                }
                            }
                        }
                        if (strBillId.EndsWith(","))
                            strBillId = strBillId.Remove(strBillId.Length - 1, 1);

                        GetChargeAgainstMultipleBill(strBillId, UnitID);

                    }

                }
            }
            catch (Exception)
            {

                Indicatior.Close();
            }

        }

        void GetChargeAgainstMultipleBill(string PBillID, long pUnitID)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
                clsGetChargeAgainstBillListBizActionVO BizAction = new clsGetChargeAgainstBillListBizActionVO();
                BizAction.BillID = PBillID;
                BizAction.UnitID = pUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeAgainstBillListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;// = new List<clsChargeVO>();
                            objList = ((clsGetChargeAgainstBillListBizActionVO)arg.Result).List;
                            lstChargeID = new List<clsChargeVO>();
                            foreach (var item in objList)
                            {
                                lstChargeID.Add(item);
                            }

                            if (lstChargeID.Count > 0)
                            {
                                SettleMultipleBill();
                            }
                        }
                        else
                        {
                            SettleMultipleBill();
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


        private void SettleMultipleBill()
        {
            clsCompanySettlementBizActionVO BizAction = new clsCompanySettlementBizActionVO();
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                //  BizAction.InvoiceID = InvoiceID;
                //    BizAction.InvoiceUnitID = UnitID;
                BizAction.PaymentDetailobj = new clsPaymentVO();
                //double TDS1 = 0.0;
                //if (txtTDS.Text != "" && txtTDS.Text != null)
                //{
                //    TDS1 = Convert.ToDouble(txtTDS.Text);
                //}
                //double amt = 0.0;
                //amt = TDS1 + Convert.ToDouble(txtReceivedAmount.Text);
                double TotalAmt2 = pPayDetails.PaidAmount;
                double TotalAmt3 = pPayDetails.PaidAmount;
                double ConsumeAmt = 0;
                double ConAmt = pPayDetails.SettlementConcessionAmount;
                var FinalResult = from r in lstChargeID
                                  orderby r.BillID ascending, r.BillUnitID ascending, r.NetAmount descending
                                  select r;

                var Finalresults = from r in lstChargeID
                                   orderby r.BillID ascending, r.BillUnitID ascending, r.NetAmount descending
                                   select r;

                lstChargeID = new List<clsChargeVO>();

                var lst = from r in SelectedBillList
                          orderby r.ID ascending, r.UnitID ascending
                          select r;

                clsPaymentVO obj1 = new clsPaymentVO();
                obj1.Date = DateTime.Now;
                obj1.PaidAmount = pPayDetails.PaidAmount;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    obj1.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                else
                    obj1.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                obj1.IsBillSettlement = true;
                obj1.Remarks = pPayDetails.Remarks;
                obj1.PayeeNarration = pPayDetails.PayeeNarration;
                obj1.ChequeAuthorizedBy = pPayDetails.ChequeAuthorizedBy;
                obj1.CreditAuthorizedBy = pPayDetails.CreditAuthorizedBy;
                obj1.Status = true;
                obj1.BillAmount = InvoiceAmount;
                obj1.BillBalanceAmount = InvoiceBalanceAmount - pPayDetails.PaidAmount;
                obj1.PaymentDetails = pPayDetails.PaymentDetails;
                obj1.InvoiceID = InvoiceID;
                obj1.InvoiceUnitID = pPayDetails.UnitID;
                BizAction.PaymentDetailobj = obj1;


                if (lst != null && lst.ToList().Count > 0)
                {
                    foreach (var item in lst)
                    {

                        clsPaymentVO obj = new clsPaymentVO();
                        obj.Date = DateTime.Now;
                        obj.ReceiptNo = pPayDetails.ReceiptNo;
                        obj.BillID = item.ID;
                        obj.BillAmount = item.NetBillAmount;
                        //if (txtTDS.Text != null && txtTDS.Text != "")
                        //{
                        //    obj.TDSAmt = Convert.ToDouble(txtTDS.Text);
                        //}
                        obj.BillBalanceAmount = item.TempBalanceAmount;

                        //obj.BillBalanceAmount = amt - item.NetBillAmount;
                        //amt = amt - item.NetBillAmount;
                        obj.TempPaidAmount = item.TempPaidAmount;

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                            obj.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                        else
                            obj.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                        obj.IsBillSettlement = true;
                        obj.Remarks = pPayDetails.Remarks;
                        obj.PayeeNarration = pPayDetails.PayeeNarration;
                        obj.ChequeAuthorizedBy = pPayDetails.ChequeAuthorizedBy;
                        obj.CreditAuthorizedBy = pPayDetails.CreditAuthorizedBy;
                        obj.Status = true;
                        obj.PaymentDetails = pPayDetails.PaymentDetails;
                        obj.InvoiceID = InvoiceID;
                        obj.InvoiceUnitID = item.UnitID;
                        obj.UnitID = item.UnitID;
                        //BizAction.PaymentDetailobj = obj;
                        BizAction.Details.Add(obj);
                        BizAction.BillDetails.Add(item);


                        if (item.Date == DateTime.Now.Date.ToShortDateString())
                        {
                            #region WithOut Concession

                            if (FinalResult != null)
                            {
                                foreach (var item2 in FinalResult.ToList())
                                {
                                    if (item2.BillID == item.ID && item2.BillUnitID == item.UnitID)
                                    {
                                        if (item2.ParentID == 0)
                                        {
                                            item2.IsUpdate = true;

                                            double BalAmt = item2.NetAmount - item2.ServicePaidAmount;
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
                                                item2.ServicePaidAmount = ConsumeAmt + item2.ServicePaidAmount;
                                                item2.BalanceAmount = BalAmt - ConsumeAmt;


                                                //Total Amount
                                                item2.TotalServicePaidAmount = item2.ServicePaidAmount;
                                                item2.TotalConcession = item2.Concession;
                                                item2.TotalNetAmount = item2.NetAmount;
                                            }
                                            else
                                            {
                                                item2.TotalServicePaidAmount = item2.ServicePaidAmount;
                                                item2.TotalConcession = item2.Concession;
                                                item2.TotalNetAmount = item2.NetAmount;
                                                item2.BalanceAmount = item2.NetAmount - item2.ServicePaidAmount;
                                            }

                                        }
                                        lstChargeID.Add(item2);

                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {

                            #region Without Concession
                            // lstChargeID = new List<clsChargeVO>();
                            if (Finalresults != null)
                            {

                                foreach (var item2 in Finalresults.ToList())
                                {
                                    if (item2.BillID == item.ID && item2.BillUnitID == item.UnitID)
                                    {
                                        #region same date
                                        if (item2.Date.Value.Date == DateTime.Now.Date.Date)
                                        {
                                            if (item2.ParentID == 0)
                                            {

                                                item2.IsUpdate = true;
                                                item2.IsSameDate = true;

                                                double BalAmt = item2.SettleNetAmount - item2.ServicePaidAmount;
                                                double TotalServicePaidAmount = item2.ServicePaidAmount;
                                                double TotalConcession = item2.Concession;
                                                double TotalNetAmount = item2.SettleNetAmount;
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

                                                        item2.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item2.ServicePaidAmount;
                                                        item2.BalanceAmount = BalAmt - item2.ServicePaidAmount;
                                                        item2.Concession = ConAmt;


                                                        item2.TotalServicePaidAmount = TotalServicePaidAmount + item2.ServicePaidAmount;
                                                        item2.TotalConcession = TotalConcession + item2.Concession;
                                                        item2.TotalNetAmount = item2.SettleNetAmount;
                                                        item2.TotalConcessionPercentage = 0;

                                                    }
                                                    else
                                                    {
                                                        item2.BalanceAmount = BalAmt;
                                                        item2.ServicePaidAmount = item2.ServicePaidAmount;
                                                        item2.Concession = item2.Concession;
                                                        item2.SettleNetAmount = item2.SettleNetAmount;
                                                        item2.TotalConcession = item2.Concession + TotalConcession;
                                                    }
                                                }
                                                else
                                                {
                                                    item2.SettleNetAmount = item2.SettleNetAmount;
                                                    item2.ServicePaidAmount = 0;
                                                    item2.BalanceAmount = 0;
                                                    item2.Concession = 0;

                                                    //Total Amount
                                                    item2.TotalNetAmount = TotalNetAmount;
                                                    item2.TotalServicePaidAmount = TotalServicePaidAmount + item2.ServicePaidAmount;
                                                    item2.TotalConcession = TotalConcession + item2.Concession;

                                                }

                                            }
                                            lstChargeID.Add(item2);

                                        }
                                        #endregion


                                        #region Differnt date
                                        else
                                        {

                                            if (item2.ParentID == 0)
                                            {

                                                item2.IsUpdate = false;
                                                item2.IsSameDate = false;
                                                double BalAmt = item2.SettleNetAmount - item2.ServicePaidAmount;
                                                double TotalServicePaidAmount = item2.ServicePaidAmount;
                                                double TotalConcession = item2.Concession;
                                                double TotalNetAmount = item2.SettleNetAmount;
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
                                                        item2.ServicePaidAmount = ConsumeAmt;
                                                        PConsumeAmt = item2.ServicePaidAmount;
                                                        item2.BalanceAmount = BalAmt - item2.ServicePaidAmount;
                                                        //Total Amount
                                                        item2.TotalServicePaidAmount = TotalServicePaidAmount + item2.ServicePaidAmount;
                                                        item2.TotalConcession = item2.Concession;
                                                        item2.TotalNetAmount = item2.TotalAmount - item2.Concession;
                                                        item2.Concession = 0;


                                                    }
                                                    else
                                                    {
                                                        item2.BalanceAmount = BalAmt;
                                                        item2.ServicePaidAmount = item2.ServicePaidAmount;
                                                        item2.Concession = ConAmt;
                                                        item2.SettleNetAmount = item2.SettleNetAmount;
                                                        //Total Amount

                                                        item2.TotalServicePaidAmount = item2.ServicePaidAmount;
                                                        item2.TotalConcession = TotalConcession + item2.Concession;
                                                        item2.TotalNetAmount = item2.TotalAmount - (TotalConcession + item2.Concession);

                                                    }
                                                }
                                                else
                                                {
                                                    item2.SettleNetAmount = item2.SettleNetAmount;
                                                    item2.ServicePaidAmount = 0;
                                                    item2.BalanceAmount = 0;
                                                    item2.Concession = 0;
                                                    //Total Amount
                                                    item2.TotalNetAmount = TotalNetAmount;
                                                    item2.TotalServicePaidAmount = TotalServicePaidAmount + item2.ServicePaidAmount;
                                                    item2.TotalConcession = TotalConcession + item2.Concession;

                                                }


                                            }
                                            lstChargeID.Add(item2);

                                        }
                                        #endregion
                                    }
                                }
                            }

                            #endregion
                        }


                        BizAction.ChargeDetails = lstChargeID;




                    }
                }

                double TotalAmt6 = pPayDetails.PaidAmount;
                if (txtReceivedAmount.Text != null && txtReceivedAmount.Text != "")
                {
                    BizAction.InvoicePaidAmount = pPayDetails.PaidAmount;                   //TotalAmt6 - Convert.ToDouble(txtTDS.Text);
                }

                //if (txtTDS.Text != null && txtTDS.Text != "")
                //{
                //    BizAction.InvoiceTDSAmount = Convert.ToDouble(txtTDS.Text);
                //}
                if (txTAmount.Text != null && txTAmount.Text != "")
                {
                    BizAction.InvoiceBalanceAmount = Convert.ToDouble(txTAmount.Text) - BizAction.InvoicePaidAmount; //- BizAction.InvoiceTDSAmount;
                }



                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    if (arg.Error == null && arg.Result != null)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            if (re == MessageBoxResult.OK)
                            {
                                if (OnSaveButton_Click != null)
                                {
                                    OnSaveButton_Click(this, new RoutedEventArgs());
                                }
                                long invoicepID = ((clsCompanySettlementBizActionVO)arg.Result).InvoiceID;
                                if (invoicepID > 0)
                                {
                                    PrintInvoicepayment(invoicepID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                                }
                            }
                        };
                        msgW1.Show();
                        this.DialogResult = false;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch 
            {
                Indicatior.Close();
            }
        }


        private void PrintInvoicepayment(long InvpaymentID, long UnitID)
        {

            string URL = "../Reports/OPD/CompanyInvoicePayment.aspx?ID=" + InvpaymentID + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        #region Single Settlement
        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "";
            if (dgBillList.SelectedItem != null)
            {

                if (((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).IsFreezed == true)
                {
                    //    if (((clsBillVO)dgBillList.SelectedItem).BillType == BillTypes.Pharmacy)
                    //    {
                    //        SettlePharmacyBill();
                    //    }
                    //    else
                    //    {
                    GetCharge(((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).ID, ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).UnitID, ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).IsFreezed, ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).Opd_Ipd_External_Id);

                }
                SettleBill();
            }
            else
            {
                msgText = "Only freezed Bills can be Settled ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.Show();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {

        }

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
                BizAction.UnitID = pUnitID; // ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetChargeListBizActionVO)arg.Result).List != null)
                        {
                            List<clsChargeVO> objList;// = new List<clsChargeVO>();
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
        /// <summary>
        ///  Method is for opening the Payment Window for Bill settlement.
        /// </summary>
        void SettleBill()
        {
            if (dgBillList.SelectedItem != null && ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).IsFreezed == true)

                //InitialiseForm();
                SelectedBill = new clsCompanyPaymentDetailsVO();
            SelectedBill = (clsCompanyPaymentDetailsVO)dgBillList.SelectedItem;

            // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);


            string msgText = "";
            msgText = "Are you sure you want to Settle the Bill ?";
            MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += (arg) =>
            {
                if (arg == MessageBoxResult.Yes)
                {
                    bool isValid = true;
                    //isValid = tr  // CheckValidations();

                    if (isValid)
                    {
                        PaymentWindow SettlePaymentWin = new PaymentWindow();
                        if (SelectedBill.BalanceAmountSelf > 0)
                            SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;

                        SettlePaymentWin.Initiate("Bill");

                        SettlePaymentWin.ConcessionFromPlan = SelectedBill.IsConcessionAthorization;

                        SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.NetBillAmount.ToString();
                        SettlePaymentWin.txtDiscAmt.Text = SelectedBill.TotalConcessionAmount.ToString();
                        SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();

                        SettlePaymentWin.cmbCheckAuthBy.SelectedValue = SelectedBill.PaymentDetails.ChequeAuthorizedBy;
                        SettlePaymentWin.txtNarration.Text = SelectedBill.PaymentDetails.PayeeNarration;
                        SettlePaymentWin.IsSettleMentBill = true;
                        SettlePaymentWin.TxtConAmt.IsReadOnly = false;
                        SettlePaymentWin.dgCompanyAdvDtl.Visibility = Visibility.Collapsed;
                        SettlePaymentWin.dgCompanyAdvDtl.Visibility = Visibility.Collapsed;
                        SettlePaymentWin.TxtpaidAmt.IsEnabled = false;

                        SettlePaymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
                        SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
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

        long PaymentID { get; set; }

        /// <summary>
        /// This method check the dialog result of Payment Window if it is true 
        /// then it creates instance of type clsUpdateBillPaymentDtlsBizActionVO class,
        /// assign the details to it and call service ProcessAsync() method for updating Bill Payment Details.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();

            try
            {
                string msgText = "";

                //if (((clsBillVO)dgBillList.SelectedItem).BillType == BillTypes.Clinical)
                //{
                #region
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
                        //if (txtTDS.Text != null && txtTDS.Text != "")
                        //{
                        //    BizAction.Details.TDSAmt = Convert.ToDouble(txtTDS.Text);
                        //}


                           //for (int i = 0; i < pPayDetails.PaymentDetails.Count; i++)
                           //     {
                           //         clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();

                           //         objPay.PaymentModeID = pPayDetails.PaymentDetails[i].PayModeID;
                           //         objPay.AdvanceID = pPayDetails.PaymentDetails[i].AdvanceID;
                           //         objPay.PaidAmount = pPayDetails.PaymentDetails[i].Amount;
                           //         objPay.Number = pPayDetails.PaymentDetails[i].Number;
                           //         objPay.Date = pPayDetails.PaymentDetails[i].Date;
                           //         if (pPayDetails.PaymentDetails[i].BankID != null)
                           //             objPay.BankID = pPayDetails.PaymentDetails[i].BankID.Value;

                           //         Payment.PaymentDetails.Add(objPay);
                           //     }

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                                    PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                                clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                                mybillPayDetails.Details1 = SelectedBill;
                                mybillPayDetails.FromCompanyPayment = true;
                                mybillPayDetails.CompanyPaymentDetailsInfo = new clsCompanyPaymentDetailsVO();

                                mybillPayDetails.CompanyPaymentDetailsInfo.InvoiceID = InvoiceID;
                                mybillPayDetails.CompanyPaymentDetailsInfo.UnitID = UnitID;
                                if (pPayDetails.SettlementConcessionAmount > 0)
                                {
                                    mybillPayDetails.Details1.TotalConcessionAmount = SelectedBill.TotalConcessionAmount + pPayDetails.SettlementConcessionAmount;
                                }

                                if (pPayDetails.SettlementConcessionAmount > 0)
                                {

                                    mybillPayDetails.Details1.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - (pPayDetails.PaidAmount + pPayDetails.SettlementConcessionAmount);
                                    if (mybillPayDetails.Details1.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                                    mybillPayDetails.Details1.NetBillAmount = SelectedBill.NetBillAmount - pPayDetails.SettlementConcessionAmount;

                                }
                                else
                                {
                                    mybillPayDetails.Details1.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
                                    if (mybillPayDetails.Details1.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                                }

                                if (Convert.ToDateTime(SelectedBill.Date).Date == DateTime.Now.Date.Date)
                                {
                                    #region Concession
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

                                    #region WithOut Concession
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
                                        #region Concession
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
                                                            item.ServicePaidAmount = item.ServicePaidAmount;
                                                            PConsumeAmt = item.ServicePaidAmount;
                                                            item.Concession = item.Concession;
                                                            item.SettleNetAmount = item.SettleNetAmount;
                                                            item.TotalConcession = item.Concession + TotalConcession;
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


                                                            //if (TotalAmt3 == 0)
                                                            //{
                                                            //    item.BalanceAmount = 0;
                                                            //}
                                                            //else
                                                            //{
                                                            //    item.BalanceAmount = BalAmt - item.ServicePaidAmount;
                                                            //}

                                                            //Total Amount

                                                            item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;
                                                            item.TotalNetAmount = item.TotalAmount - item.Concession;

                                                        }
                                                        else
                                                        {
                                                            //if (TotalServicePaidAmount == 0)
                                                            //{
                                                            //    item.BalanceAmount = 0;
                                                            //}
                                                            //else
                                                            //{
                                                            //    item.BalanceAmount = BalAmt;
                                                            //}
                                                            item.BalanceAmount = BalAmt;

                                                            item.ServicePaidAmount = item.ServicePaidAmount;
                                                            PConsumeAmt = item.ServicePaidAmount;
                                                            item.Concession = item.Concession;
                                                            item.SettleNetAmount = item.SettleNetAmount;

                                                            item.TotalNetAmount = item.TotalAmount - item.TotalConcession;
                                                            item.ServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        item.SettleNetAmount = item.SettleNetAmount;
                                                        item.ServicePaidAmount = 0;
                                                        item.BalanceAmount = 0;
                                                        //Total Amount
                                                        item.TotalNetAmount = TotalNetAmount;
                                                        item.TotalServicePaidAmount = TotalServicePaidAmount + item.ServicePaidAmount;


                                                    }

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
                                                                //if (CTotalAmt == 0)
                                                                //{
                                                                //    objcharge.BalanceAmount = 0;
                                                                //}
                                                                //else
                                                                //{
                                                                objcharge.BalanceAmount = CBalAmt - objcharge.ServicePaidAmount;
                                                                //}
                                                                objcharge.TotalServicePaidAmount = CTotalServicePaidAmount + objcharge.ServicePaidAmount;
                                                                //objcharge.TotalNetAmount = objcharge.TotalAmount - objcharge.Concession;

                                                            }
                                                            else
                                                            {
                                                                //if (TotalServicePaidAmount == 0)
                                                                //{
                                                                //    objcharge.BalanceAmount = 0;
                                                                //}
                                                                //else
                                                                //{
                                                                objcharge.BalanceAmount = CBalAmt;
                                                                //}
                                                                objcharge.ServicePaidAmount = objcharge.ServicePaidAmount;
                                                                objcharge.Concession = objcharge.Concession;
                                                                objcharge.SettleNetAmount = objcharge.SettleNetAmount;
                                                                //objcharge.TotalNetAmount = objcharge.TotalAmount - objcharge.TotalConcession;
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
                                        #region Without Concession
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
                                                            item.ServicePaidAmount = item.ServicePaidAmount;
                                                            item.Concession = item.Concession;
                                                            item.SettleNetAmount = item.SettleNetAmount;
                                                            item.TotalConcession = item.Concession + TotalConcession;
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
                                                            item.ServicePaidAmount = item.ServicePaidAmount;
                                                            item.Concession = ConAmt;
                                                            item.SettleNetAmount = item.SettleNetAmount;
                                                            //Total Amount

                                                            item.TotalServicePaidAmount = item.ServicePaidAmount;
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



                                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                Client1.ProcessCompleted += (s1, arg1) =>
                                {
                                    lstChargeID = new List<clsChargeVO>();
                                    Indicatior.Close();
                                    if (arg1.Error == null && arg1.Result != null)
                                    {

                                        FillBillSearchList(InvoiceID, UnitID);

                                        msgText = "Payment Details Saved Successfully";
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                        msgWD.OnMessageBoxClosed += (re) =>
                                        {
                                            if (re == MessageBoxResult.OK)
                                            {
                                                //if (((clsBillVO)dgBillList.SelectedItem).BalanceAmountNonSelf == 0 && ((clsBillVO)dgBillList.SelectedItem).BalanceAmountSelf == 0)
                                                //{
                                                //PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                                                //}
                                            }
                                        };
                                        msgWD.Show();
                                    }
                                    else
                                    {
                                        msgText = "Error while updating Payment Details";
                                        MessageBoxControl.MessageBoxChildWindow msgWD =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                        msgWD.Show();
                                    }
                                };
                                Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client1.CloseAsync();

                            }
                            else
                            {
                                Indicatior.Close();
                                msgText = "Error while Saving Payment Details";
                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgWD.Show();

                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();

                    }
                }
                #endregion
                //}
                //else
                //{

                //#region PharmacyBill
                //if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                //{
                //    if (((PaymentWindow)sender).Payment != null)
                //    {

                //        Indicatior.Show();
                //        clsAddPaymentBizActionVO BizAction = new clsAddPaymentBizActionVO();
                //        clsPaymentVO pPayDetails = new clsPaymentVO();

                //        pPayDetails = ((PaymentWindow)sender).Payment;
                //        pPayDetails.IsBillSettlement = true;
                //        BizAction.Details = pPayDetails;

                //        BizAction.Details.BillID = SelectedBill.ID;
                //        BizAction.Details.BillAmount = SelectedBill.NetBillAmount;
                //        BizAction.Details.Date = DateTime.Now;
                //        double ConAmt = 0;
                //        if (((PaymentWindow)sender).TxtConAmt.Text != null && ((PaymentWindow)sender).TxtConAmt.Text != "")
                //        {
                //            ConAmt = Convert.ToDouble(((PaymentWindow)sender).TxtConAmt.Text);
                //            BizAction.Details.BillBalanceAmount = (pPayDetails.PaidAmount + ConAmt);
                //        }
                //        else
                //        {
                //            BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;
                //        }
                //        //if (txtTDS.Text != null && txtTDS.Text != "")
                //        //{
                //        //    BizAction.Details.TDSAmt = Convert.ToDouble(txtTDS.Text);
                //        //}

                //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //        Client.ProcessCompleted += (s, arg) =>
                //        {
                //            if (arg.Error == null && arg.Result != null)
                //            {
                //                if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
                //                    PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

                //                clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

                //                mybillPayDetails.Details = SelectedBill;

                //                mybillPayDetails.Details.BalanceAmountSelf = Math.Round(mybillPayDetails.Details.BalanceAmountSelf, 0) - (pPayDetails.PaidAmount + ConAmt);
                //                if (Math.Round(mybillPayDetails.Details.BalanceAmountSelf, 0) < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

                //                if (ConAmt != null && ConAmt > 0)
                //                {
                //                    mybillPayDetails.Details.NetBillAmount = SelectedBill.NetBillAmount - ConAmt;
                //                }
                //                if (ConAmt > 0)
                //                {
                //                    mybillPayDetails.Details.TotalConcessionAmount = SelectedBill.TotalConcessionAmount + ConAmt;
                //                }
                //                PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //                Client1.ProcessCompleted += (s1, arg1) =>
                //                {
                //                    Indicatior.Close();
                //                    if (arg1.Error == null && arg1.Result != null)
                //                    {
                //                        FillBillSearchList(InvoiceID, UnitID);
                //                        msgText = "Payment Details Saved Successfully";
                //                        MessageBoxControl.MessageBoxChildWindow msgWD =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //                        msgWD.OnMessageBoxClosed += (re) =>
                //                        {
                //                            if (re == MessageBoxResult.OK)
                //                            {
                //                                PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
                //                            }
                //                        };
                //                        msgWD.Show();
                //                    }
                //                    else
                //                    {
                //                        msgText = "Error while updating Payment Details";
                //                        MessageBoxControl.MessageBoxChildWindow msgWD =
                //                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //                        msgWD.Show();
                //                    }


                //                };

                //                Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
                //                Client1.CloseAsync();
                //            }
                //            else
                //            {
                //                Indicatior.Close();
                //                msgText = "Error while Saving Payment Details";
                //                MessageBoxControl.MessageBoxChildWindow msgWD =
                //                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                //                msgWD.Show();

                //            }
                //        };
                //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //        Client.CloseAsync();

                //    }
                //}
                //#endregion
                //}




            }
            catch (Exception)
            {

                Indicatior.Close();
            }



        }
        clsCompanyPaymentDetailsVO SelectedBill = null;

        void SettlePharmacyBill()
        {
            if (dgBillList.SelectedItem != null && ((clsBillVO)dgBillList.SelectedItem).IsFreezed == true)
            {

                string msgText = "";
                SelectedBill = new clsCompanyPaymentDetailsVO();
                SelectedBill = (clsCompanyPaymentDetailsVO)dgBillList.SelectedItem;

                string msgTitle = "";
                msgText = "Are you sure you want to Settle the Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (arg) =>
                {
                    if (arg == MessageBoxResult.Yes)
                    {
                        bool isValid = true;
                        //isValid = tr  // CheckValidations();

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


                        }
                    }
                    else
                    {


                    }

                };
                msgWD.Show();
            }
        }

        #endregion
        /// <summary>
        /// This method is for printing the receipt against the settlled bill.
        /// </summary>
        /// <param name="iBillId"></param>
        /// <param name="iUnitID"></param>
        /// <param name="iPaymentID"></param>


        private void PrintSettleBill(long iBillId, long iUnitID, long iPaymentID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (BillList != null)
            {
                foreach (var item in BillList)
                {
                    if (chk.IsChecked == true)
                    {
                        if (item.BalanceAmountSelf > 0)
                        {
                            var res = from r in SelectedBillList
                                      where r.ID == item.ID && r.UnitID == item.UnitID
                                      select r;
                            if (res.ToList().Count > 0)
                            {

                            }
                            else
                            {
                                SelectedBillList.Add(item);
                                item.Selected = true;
                                item.IsEnable = true;
                            }
                        }
                        double amt = 0.0;
                        foreach (var item1 in BillList)
                        {
                            amt += item1.BalanceAmountSelf;
                        }
                        txtReceivedAmount.Text = String.Format("{0:0.00}", amt);

                    }
                    else
                    {
                        if (item.BalanceAmountSelf > 0)
                        {

                            item.IsEnable = true;

                        }
                        item.Selected = false;
                        SelectedBillList.Remove(item);

                        txtReceivedAmount.Text = String.Format("{0:0.00}", "0.00");
                        textBefore = null;
                        //txtReceivedAmount.Text = "0.00";
                    }
                }
          
                dgBillList.ItemsSource = null;
                dgBillList.ItemsSource = BillList;
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                if (((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).BalanceAmountSelf > 0)
                {

                    SelectedBillList.Add((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem);
                    ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).Selected = true;
                   
                }
            }
            else
            {
                ((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem).Selected = false;
                SelectedBillList.Remove((clsCompanyPaymentDetailsVO)dgBillList.SelectedItem);
               
            }
            textBefore = null;
            double amt2=0.0;
            foreach (var item in SelectedBillList)
            {
                amt2 += item.BalanceAmountSelf;
            }
            txtReceivedAmount.Text = String.Format("{0:0.00}", amt2);

        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtReceivedAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsPositiveNumber() && textBefore != null)
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

        private void txtReceivedAmount_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtTDS_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            {
                if (!((TextBox)sender).Text.IsPositiveNumber() && textBefore != null)
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

        private void txtTDS_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

    }
}
