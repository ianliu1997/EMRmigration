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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Animations;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Master.CompanyPayment;
using PalashDynamics.UserControls;
using PalashDynamics.Collections;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;

namespace OPDModule.Forms
{
    public partial class FrmCompanyPayment : UserControl
    {
        PalashDynamics.Animations.SwivelAnimation _flip = null;

        WaitIndicator ind = new WaitIndicator();

        public ObservableCollection<clsCompanyPaymentDetailsVO> SelectedBillList;
        frmSelCompanyBillChild win = new frmSelCompanyBillChild();
        WaitIndicator indicator = new WaitIndicator();
        public PagedSortableCollectionView<clsCompanyPaymentDetailsVO> DataList { get; private set; }
        public PagedSortableCollectionView<clsCompanyPaymentDetailsVO> DataList1 { get; private set; }
        public PagedSortableCollectionView<clsCompanyPaymentDetailsVO> DataList2 { get; private set; }

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

        public int DataListPageSize1
        {
            get
            {
                return DataList1.PageSize;
            }
            set
            {
                if (value == DataList1.PageSize) return;
                DataList1.PageSize = value;
            }
        }

        public int DataListPageSize2
        {
            get
            {
                return DataList2.PageSize;
            }
            set
            {
                if (value == DataList2.PageSize) return;
                DataList2.PageSize = value;
            }
        }

        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillData();

        }

        void DataList1_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillData();

        }
        void DataList2_OnRefresh(object sender, RefreshEventArgs e)
        {

            FillDataByInvoiceID();

        }

        #region Constructor
        public FrmCompanyPayment()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmCompanyPayment_Loaded);
            _flip = new PalashDynamics.Animations.SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));

            dtpFromDate.SelectedDate = DateTime.Now;

            dtpToDate.SelectedDate = DateTime.Now;
            DataList = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
            DataListPageSize = 20;


            DataList1 = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
            DataList1.OnRefresh += new EventHandler<RefreshEventArgs>(DataList1_OnRefresh);
            DataListPageSize1 = 15;
            dgDataPager.PageSize = DataListPageSize1;
            dgDataPager.Source = DataList1;

            DataList2 = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
            DataList2.OnRefresh += new EventHandler<RefreshEventArgs>(DataList2_OnRefresh);
            DataListPageSize2 = 20;


            cmbUnit.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;
            SelectedBillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();
        }
        #endregion

        public bool Iscancel = false;
        public bool IsView = false;
        private void btnnew_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Save");
            _flip.Invoke(RotationType.Forward);
            dtpInvoice.SelectedDate = DateTime.Now;
            Iscancel = true;
            IsView = true;
        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
           if(DataList2.Count > 0)
           {
                    if (cmbCompanyList1.SelectedItem != null && ((MasterListItem)cmbCompanyList1.SelectedItem).ID > 0)
                    {

                        if (SelectedBillList.Count == 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW.Show();
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Generate the Invoice?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                        }
                    }

                else
                {
                    cmbCompanyList1.SetValidation("Please Select Company");
                    cmbCompanyList1.RaiseValidationError();
                    cmbCompanyList1.Focus();
                }
          }
          else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Bill ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //if (IsFreeze == true)
                //{
                //    indicator = new WaitIndicator();
                //    indicator.Show();

                //    clsAddCompanyInvoiceBizActionVO BizAction = new clsAddCompanyInvoiceBizActionVO();
                //    BizAction.BillDetails = new List<clsCompanyPaymentDetailsVO>();
                //    BizAction.CompanyDetails = new clsCompanyPaymentDetailsVO();
                //    BizAction.BillDetails = SelectedBillList.ToList();
                //    BizAction.CompanyDetails.CompanyID = ((MasterListItem)cmbCompanyList1.SelectedItem).ID;
                //    BizAction.CompanyDetails.TotalAmount = Convert.ToDouble(txtClinicalTotal.Text);
                //    BizAction.CompanyDetails.BalAmt = totalBalAmount;
                //    BizAction.CompanyDetails.PaidAmount = totalPaidAmount; 

                //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                //    client.ProcessCompleted += (s, arg) =>
                //    {
                //        if (arg.Error == null)
                //        {
                //            if (((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails != null)
                //            {
                //                DataList = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
                //                dgCompPayList.ItemsSource = DataList;

                //                invoiceID = ((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails.InvoiceID;

                //                MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you save ", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);


                //                PaymentWindow paymentWin = new PaymentWindow();
                //                paymentWin.TotalAmount = totalAmount;
                //                paymentWin.Initiate("Bill");
                //                paymentWin.txtPayTotalAmount.Text = totalAmount.ToString();
                //                paymentWin.txtDiscAmt.Text = string.Empty;
                //                paymentWin.TxtpaidAmt.IsEnabled = true;
                //                paymentWin.cmdAddPayment.IsEnabled = true;
                //                paymentWin.txtPayableAmt.Text = this.txtPayAmount.Text;

                //                paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                //                paymentWin.Show();
                //                indicator.Close();

                //            }

                //        }
                //        else
                //        {
                //            indicator.Close();
                //        }
                //    };
                //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //    client.CloseAsync();  


                //}
                //else
                //{
                Save();
                // }
            }



        }

        long invoiceID;

        private void Save()
        {
            indicator = new WaitIndicator();
            indicator.Show();

            clsAddCompanyInvoiceBizActionVO BizAction = new clsAddCompanyInvoiceBizActionVO();
            BizAction.BillDetails = new List<clsCompanyPaymentDetailsVO>();
            BizAction.CompanyDetails = new clsCompanyPaymentDetailsVO();
            BizAction.BillDetails = SelectedBillList.ToList();
            BizAction.CompanyDetails.CompanyID = ((MasterListItem)cmbCompanyList1.SelectedItem).ID;
            BizAction.CompanyDetails.TotalAmount = Convert.ToDouble(txtClinicalTotal.Text);
            BizAction.CompanyDetails.BalAmt = totalBalAmount;
            BizAction.CompanyDetails.PaidAmount = totalPaidAmount;
            BizAction.CompanyDetails.IsFreezed = IsFreeze;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails != null)
                    {
                        DataList = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
                        dgCompPayList.ItemsSource = DataList;



                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Invoice Generated Successfully ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);



                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            if (re == MessageBoxResult.OK)
                            {
                                indicator.Close();
                                _flip.Invoke(RotationType.Backward);
                                DataList2.Clear();
                                SelectedBillList.Clear();
                                dgCharges.ItemsSource = null;
                                txtClinicalTotal.Text = string.Empty;
                                cmbCompanyList1.IsEnabled = true;
                                lnkCompanyBill.IsEnabled = true;
                                chkFreezBill1.IsEnabled = true;
                                IsFreeze = false;
                                chkFreezBill1.IsChecked = false;
                                cmbCompanyList1.SelectedValue = (long)0;
                                txtinvoiceno.Text = string.Empty;
                                dtpInvoice.SelectedDate = DateTime.Now;
                                SetCommandButtonState("New");
                                FillData();
                                invoiceID = ((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails.InvoiceID;
                                
                                if (((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails.IsFreezed == true)
                                {
                                    if (invoiceID > 0)
                                    {
                                        PrintInvoice(((clsAddCompanyInvoiceBizActionVO)arg.Result).CompanyDetails.InvoiceID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);

                                    }
                                }
                            }
                        };
                        msgW1.Show();


                       
                    }

                }
                else
                {
                    indicator.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void PrintInvoice(long ID, long UnitID)
        {

            string URL = "../Reports/OPD/CompanyInvoice.aspx?ID=" + ID + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        clsPaymentVO pPayDetails = new clsPaymentVO();

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        pPayDetails = new clsPaymentVO();
                        pPayDetails = ((PaymentWindow)sender).Payment;
                        var results = from r in SelectedBillList
                                      orderby r.Date, r.ID
                                      select r;

                        string strBillId = null;
                        double TAmt = pPayDetails.PaidAmount;
                        double BalAmt = 0;
                        double ConsumeAmt = 0;
                        long UnitID = 0;

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                            pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                        else
                            pPayDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                        foreach (var item in results.ToList())
                        {
                            BalAmt = item.BalAmt;

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
                                item.TempBalanceAmount = BalAmt - ConsumeAmt;
                                item.TempPaidAmount = (item.BalAmt - item.TempBalanceAmount);

                                UnitID = item.UnitID;
                                strBillId = strBillId + item.ID;
                                strBillId = strBillId + ",";
                            }
                            else
                            {
                                //if (SelectedBillList.Count > 0)
                                //{
                                //    SelectedBillList.Remove(item);
                                //}
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
                                Savepayment();

                            }
                        }
                        else
                        {
                            Savepayment();

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


        List<clsChargeVO> lstChargeID = new List<clsChargeVO>();

        private void Savepayment()
        {
            clsCompanySettlementBizActionVO BizAction = new clsCompanySettlementBizActionVO();
            BizAction.PaymentDetailobj = new clsPaymentVO();

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

            if (lst != null && lst.ToList().Count > 0)
            {
                foreach (var item in lst)
                {

                    clsPaymentVO obj = new clsPaymentVO();
                    obj.Date = DateTime.Now;
                    obj.ReceiptNo = pPayDetails.ReceiptNo;
                    obj.BillID = item.ID;
                    obj.BillAmount = item.NetBillAmount;
                    obj.BillBalanceAmount = item.TempBalanceAmount;
                    obj.TempPaidAmount = item.TempPaidAmount;
                    obj.CostingDivisionID = pPayDetails.CostingDivisionID;
                    obj.IsBillSettlement = true;
                    obj.Remarks = pPayDetails.Remarks;
                    obj.PayeeNarration = pPayDetails.PayeeNarration;
                    obj.ChequeAuthorizedBy = pPayDetails.ChequeAuthorizedBy;
                    obj.CreditAuthorizedBy = pPayDetails.CreditAuthorizedBy;
                    obj.Status = true;
                    obj.PaymentDetails = pPayDetails.PaymentDetails;
                    obj.InvoiceID = invoiceID;
                    obj.InvoiceUnitID = item.UnitID;
                    BizAction.PaymentDetailobj = obj;
                    BizAction.Details.Add(obj);
                    BizAction.BillDetails.Add(item);


                    if (Convert.ToDateTime(item.Date).Date == DateTime.Now.Date.Date)
                    {
                        #region WithOut Concession

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

                        #endregion
                    }
                    else
                    {

                        #region Without Concession
                        // lstChargeID = new List<clsChargeVO>();

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

                        #endregion
                    }


                    BizAction.ChargeDetails = lstChargeID;




                }
            }


            BizAction.InvoiceBalanceAmount = totalBalAmount;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {


                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.OnMessageBoxClosed += (re) =>
                    {
                        if (re == MessageBoxResult.OK)
                        {
                            FillData();
                            long invoicepID = ((clsCompanySettlementBizActionVO)arg.Result).InvoiceID;
                            if (invoicepID > 0)
                            {
                                PrintInvoicepayment(invoicepID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                            }
                        }
                    };
                    msgW1.Show();



                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void PrintInvoicepayment(long InvpaymentID, long UnitID)
        {

            string URL = "../Reports/OPD/CompanyInvoicePayment.aspx?ID=" + InvpaymentID + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }


        private void btnmodify_Click(object sender, RoutedEventArgs e)
        {
            if (DataList2.Count > 0 )
            {
                ModifyData();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Add Bill ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }

        }

        private void ModifyData()
        {
            indicator = new WaitIndicator();
            indicator.Show();
            totalBalAmount=0.00;
            clsModifyCompanyInvoiceBizActionVO BizAction = new clsModifyCompanyInvoiceBizActionVO();
            BizAction.BillDetails = new List<clsCompanyPaymentDetailsVO>();
            BizAction.CompanyDetails = new clsCompanyPaymentDetailsVO();
            BizAction.BillDetails = DataList2.ToList();
            BizAction.UnitID = invUnitID;
            BizAction.InvoiceID = InvoiceID;
            BizAction.CompanyDetails.CompanyID = companyID;

            BizAction.CompanyDetails.IsFreezed = IsFreeze;
            BizAction.CompanyDetails.TotalAmount = Convert.ToDouble(txtClinicalTotal.Text);
            foreach (var item in DataList2)
            {
                totalBalAmount += item.BalAmt;
            }
            BizAction.CompanyDetails.BalAmt = totalBalAmount;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsModifyCompanyInvoiceBizActionVO)arg.Result).CompanyDetails != null)
                    {


                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Invoice Updated Successfully ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.OnMessageBoxClosed += (re) =>
                        {
                            if (re == MessageBoxResult.OK)
                            {
                                FillData();

                                SetCommandButtonState("New");
                                indicator.Close();
                                _flip.Invoke(RotationType.Backward);
                                txtClinicalTotal.Text = string.Empty;
                                SelectedBillList.Clear();
                                DataList2.Clear();
                                IsFreeze = false;
                                cmbCompanyList1.IsEnabled = true;
                                lnkCompanyBill.IsEnabled = true;
                                chkFreezBill1.IsEnabled = true;
                                chkFreezBill1.IsChecked = false;
                                cmbCompanyList1.SelectedValue = (long)0;
                                txtinvoiceno.Text = string.Empty;
                                dtpInvoice.SelectedDate = DateTime.Now;

                                if (((clsModifyCompanyInvoiceBizActionVO)arg.Result).CompanyDetails.IsFreezed == true)
                                {
                                    if (InvoiceID > 0)
                                    {
                                        PrintInvoice(InvoiceID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
                                    }
                                }
                            }
                        };
                        msgW1.Show();


                    }

                }
                else
                {
                    indicator.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Iscancel == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
            else
            {
                _flip.Invoke(RotationType.Backward);
                txtClinicalTotal.Text = string.Empty;
                SelectedBillList.Clear();
                DataList2.Clear();
                IsFreeze = false;
                cmbCompanyList1.IsEnabled = true;
                lnkCompanyBill.IsEnabled = true;
                chkFreezBill1.IsEnabled = true;
                chkFreezBill1.IsChecked = false;
                cmbCompanyList1.SelectedValue = (long)0;
                txtinvoiceno.Text = string.Empty;
                dtpInvoice.SelectedDate = DateTime.Now;
                SetCommandButtonState("New");
                FillData();
            }
            dgCharges.ItemsSource = null;

        }

        private void frmCompanyPayment_Loaded(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("New");
            SelectedBillList = new ObservableCollection<clsCompanyPaymentDetailsVO>();



            dgCharges.ItemsSource = SelectedBillList;
            FillUnitList();
            FillCompanyName();
            FillData();

        }

        private void FillData()
        {
            ind.Show();
            clsGetCompanyInvoiceDetailsBizActionVO BizAction = new clsGetCompanyInvoiceDetailsBizActionVO();

            if (cmbUnit.SelectedItem != null)
            {
                BizAction.UnitID = ((MasterListItem)cmbUnit.SelectedItem).ID;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            if (cmbCompanyList.SelectedItem != null && ((MasterListItem)cmbCompanyList.SelectedItem).ID > 0)
            {
                BizAction.CompanyID = ((MasterListItem)cmbCompanyList.SelectedItem).ID.ToString();

            }
            //else
            //{
            //    BizAction.CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID.ToString();
            //}

            BizAction.FromDate = dtpFromDate.SelectedDate;
            BizAction.ToDate = dtpToDate.SelectedDate;
            BizAction.InvoiceNo = txtInvoiceNo.Text;

            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList1.PageIndex * DataList1.PageSize;
            BizAction.MaximumRows = DataList1.PageSize;



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetCompanyInvoiceDetailsBizActionVO)arg.Result).CompanyPaymentDetails != null)
                    {

                        clsGetCompanyInvoiceDetailsBizActionVO result = arg.Result as clsGetCompanyInvoiceDetailsBizActionVO;
                        DataList1.TotalItemCount = result.TotalRows;

                        if (result.CompanyPaymentDetails != null)
                        {
                            DataList1.Clear();

                            foreach (var item in result.CompanyPaymentDetails)
                            {
                                DataList1.Add(item);
                            }

                        }

                        dgCompPayList.ItemsSource = DataList1;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList1;

                    }
                    else
                    {
                        dgCompPayList.ItemsSource = null;
                    }
                }
                else
                {
                    ind.Close();
                }
                ind.Close();
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

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

        private void FillCompanyName()
        {
            //============================== Show the Company Name====================
            List<MasterListItem> ObjList = new List<MasterListItem>();
            List<clsCompanyVO> objCompList = new List<clsCompanyVO>();
            clsGetCompanyDetailsBizActionVO bizActionVO = new clsGetCompanyDetailsBizActionVO();
            bizActionVO.ItemMatserDetails = new List<clsCompanyVO>();
            bizActionVO.CompanyId = 0;
            try
            {
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        clsCompanyVO selectObject = new clsCompanyVO();
                        selectObject.Id = 0;
                        selectObject.Description = "-- Select --";
                        objCompList.Add(selectObject);
                        objCompList.AddRange(((clsGetCompanyDetailsBizActionVO)args.Result).ItemMatserDetails);

                        foreach (var item in objCompList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = Convert.ToInt64(item.Id);
                            Obj.Description = item.Description;
                            Obj.Status = false;
                            ObjList.Add(Obj);
                        }
                        cmbCompanyList.ItemsSource = null;
                        cmbCompanyList.ItemsSource = ObjList;
                        cmbCompanyList.SelectedItem = ObjList[0];

                        cmbCompanyList1.ItemsSource = null;
                        cmbCompanyList1.ItemsSource = ObjList;
                        cmbCompanyList1.SelectedItem = ObjList[0];

                        // cmbCompanyList.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                        msgW1.Show();
                    }
                };
                client.ProcessAsync(bizActionVO, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception) { }
            //==================================================
        }

        #region Set Command Button State New/Save/Modify/Print
        /// <summary>
        /// Method is for setting Command Button States based on strFormMode New/Save/Modify/Print
        /// </summary>
        /// <param name="strFormMode"></param>
        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "New":
                    btnnew.Visibility = Visibility.Visible;
                    btnsave.Visibility = Visibility.Collapsed;
                    btnmodify.Visibility = Visibility.Collapsed;
                    btnCancel.Content = "Close";
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                case "Save":
                    btnnew.Visibility = Visibility.Collapsed;
                    btnsave.Visibility = Visibility.Visible;
                    btnmodify.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                case "Modify":
                    btnsave.Visibility = Visibility.Collapsed;
                    btnnew.Visibility = Visibility.Collapsed;
                    btnmodify.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;

                case "cancel":
                    break;

                default:
                    break;
            }

        }

        #endregion
        string CompanyName = string.Empty;
        private void lnkCompanyBill_Click(object sender, RoutedEventArgs e)
        {
            bool Result = true;

            if (cmbCompanyList1.SelectedItem == null)
            {
                cmbCompanyList1.TextBox.SetValidation("Please Select company");
                cmbCompanyList1.TextBox.RaiseValidationError();
                cmbCompanyList1.Focus();
                Result = false;
            }
            else if (((MasterListItem)cmbCompanyList1.SelectedItem).ID == 0)
            {
                cmbCompanyList1.TextBox.SetValidation("Please Select company");
                cmbCompanyList1.TextBox.RaiseValidationError();
                cmbCompanyList1.Focus();
                Result = false;
            }

            if (Result == true)
            {
                cmbCompanyList1.ClearValidationError();
                win = new frmSelCompanyBillChild();

                if (cmbCompanyList1.SelectedItem != null)
                {
                    win.CompanyID = ((MasterListItem)cmbCompanyList1.SelectedItem).ID;

                    CompanyName = ((MasterListItem)cmbCompanyList1.SelectedItem).Description.ToString();
                    win.ccPaymentList.Content = CompanyName + " Company Bill Details ";
                }


                win.OnAddButton_Click += new RoutedEventHandler(win_OnAddButton_Click);
                win.Show();
            }
        }
        double totalAmount = 0;
        double totalBalAmount = 0;
        double totalPaidAmount = 0;
        void win_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            DataList2 = new PagedSortableCollectionView<clsCompanyPaymentDetailsVO>();
            if (win.SelectedItems != null)
            {
                dgCharges.SelectedIndex = -1;
                foreach (var item in win.SelectedItems)
                {
                    var item12 = from r in SelectedBillList
                                 where r.ID == item.ID
                                 select r;

                    if (item12.ToList().Count == 0)
                    {
                        SelectedBillList.Add(
                        new clsCompanyPaymentDetailsVO()
                        {
                            ID = item.ID,
                            UnitID = item.UnitID,
                            BillNo = item.BillNo,
                            BillDate = item.BillDate,
                            CompanyName = item.CompanyName,
                            CompanyID = item.CompanyID,
                            MRNo = item.MRNo,
                            FirstName = item.FirstName,
                            MiddleName = item.MiddleName,
                            LastName = item.LastName,
                            NetBillAmount = item.NetBillAmount,
                            PaidAmount = item.PaidAmount,
                            TotalConcessionAmount = item.TotalConcessionAmount,
                            TotalAmount = item.TotalAmount,
                            BalAmt = item.BalAmt
                        });
                    }
                }


            }
            totalAmount = 0.00;
            totalBalAmount = 0.00;
            if (SelectedBillList != null)
            {
                foreach (var item in SelectedBillList)
                {
                    DataList2.Add(item);
                 
                }
                foreach (var item5 in DataList2)
                {
                    totalAmount += item5.NetBillAmount;
                    totalBalAmount += item5.BalAmt;
                    totalPaidAmount += item5.PaidAmount;
                }
                dgCharges.ItemsSource = DataList2;
                dgCharges.Focus();
                dgCharges.UpdateLayout();
                txtClinicalTotal.Text = String.Format("{0:0.00}", totalAmount);
            }
        }

        private void cmbBillStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public bool IsFreeze = false;

        private void chkFreezBill_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;

            if (chk.IsChecked == true)
            {
                IsFreeze = true;
            }
            else
            {
                IsFreeze = false;
            }
        }

        private void btnPrintInvoice_Click(object sender, RoutedEventArgs e)
        {
            long ID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).ID;
            long UnitID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).UnitID;

            string URL = "../Reports/OPD/CompanyInvoicePayment.aspx?ID=" + ID + "&UnitID=" + UnitID;
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
        }

        private void btnSettle_Click(object sender, RoutedEventArgs e)
        {
            FrmCompanyBillForSettle objDrPayDetails = new FrmCompanyBillForSettle();
            if (dtpFromDate.SelectedDate != null)
            {
                objDrPayDetails.dtFrom = dtpFromDate.SelectedDate.Value;
            }
            if (dtpToDate.SelectedDate != null)
            {
                objDrPayDetails.dtTO = dtpToDate.SelectedDate.Value;
            }
            objDrPayDetails.loInvoiceNo = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).InvoiceNo.ToString();
            objDrPayDetails.loBillNo = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).BillNo;
            objDrPayDetails.dtBillDate = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).BillDate;
            objDrPayDetails.txtInvoice.Text = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).InvoiceNo;
            objDrPayDetails.strCompanyName = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).CompanyName;
            objDrPayDetails.companyID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).CompanyID;
            objDrPayDetails.txTAmount.Text = (((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).BalAmt).ToString();
            objDrPayDetails.txtReceivedAmount.Text = (((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).BalAmt).ToString();
            objDrPayDetails.txtTDS.Text = "0";
            objDrPayDetails.isPaidbill = false;
            objDrPayDetails.InvoiceAmount = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).TotalAmount;
            objDrPayDetails.InvoiceBalanceAmount = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).BalAmt;
            objDrPayDetails.InvoiceID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).ID;
            objDrPayDetails.UnitID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).UnitID;
            objDrPayDetails.FillBillSearchList(((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).ID, ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).UnitID);

            objDrPayDetails.OnSaveButton_Click += new RoutedEventHandler(GetDetails_OnSaveButton_Click);
            objDrPayDetails.OnCancelButton_Click += new RoutedEventHandler(objDrPayDetails_OnCancelButton_Click);
            objDrPayDetails.Show();
        }

        void GetDetails_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        void objDrPayDetails_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {
            FillData();
        }

        private void cmdDeleteCharges_Click(object sender, RoutedEventArgs e)
        {
            if (dgCharges.SelectedItem != null)
            {
                string msgText = "Are You Sure \n  You Want To Delete The Selected Bill ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {
                        long BillID = ((clsCompanyPaymentDetailsVO)dgCharges.SelectedItem).ID;
                        long BillUnitID = ((clsCompanyPaymentDetailsVO)dgCharges.SelectedItem).UnitID;

                        DataList2.RemoveAt(dgCharges.SelectedIndex);
                        SelectedBillList.RemoveAt(dgCharges.SelectedIndex);

                      
                      
                        totalBalAmount = 0.00;

                        clsDeleteCompanyInvoiceBillBizActionVO BizAction = new clsDeleteCompanyInvoiceBillBizActionVO();
                        BizAction.UnitID = BillUnitID;
                        BizAction.BillID = BillID;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {

                            }
                        };
                        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();

                        totalAmount = 0.00;
                        totalBalAmount = 0.00;
                        foreach (var item in DataList2)
                        {
                            totalAmount += item.NetBillAmount;
                            totalBalAmount += item.BalAmt;
                            totalPaidAmount += item.PaidAmount;
                        }
                        txtClinicalTotal.Text = String.Format("{0:0.00}", totalAmount);
                    }
                };
                msgWD.Show();
            }

        }

        long InvoiceID = 0;
        long invUnitID = 0;
        long companyID = 0;
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Iscancel = true;
            IsView = false;
            SetCommandButtonState("Modify");

            if (dgCompPayList.SelectedItem != null)
            {
                if (((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).IsFreezed == true)
                {
                    btnmodify.IsEnabled = false;
                    lnkCompanyBill.IsEnabled = false;
                    chkFreezBill1.IsEnabled = false;
                }
                else
                {
                    btnmodify.IsEnabled = true;
                    lnkCompanyBill.IsEnabled = true;
                    chkFreezBill1.IsEnabled = true;
                }


                _flip.Invoke(RotationType.Forward);


                if (((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).ID > 0)
                {
                    InvoiceID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).ID;
                    invUnitID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).UnitID;
                    companyID = ((clsCompanyPaymentDetailsVO)dgCompPayList.SelectedItem).CompanyID;
                    FillDataByInvoiceID();
                }
            }


        }

        private void FillDataByInvoiceID()
        {
            WaitIndicator Indicator = new WaitIndicator();
            Indicator.Show();
            clsGetCompanyInvoiceForModifyVO BizAction = new clsGetCompanyInvoiceForModifyVO();

            if (invUnitID > 0)
                BizAction.UnitID = invUnitID;
            else
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            if (InvoiceID > 0)
            {
                BizAction.InvoiceID = InvoiceID;
            }
            if (companyID > 0)
            {
                BizAction.CompanyID = companyID.ToString();
            }
            BizAction.IsPagingEnabled = true;
            BizAction.StartRowIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = 15;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    clsGetCompanyInvoiceForModifyVO result = arg.Result as clsGetCompanyInvoiceForModifyVO;
                    DataList2.Clear();
                    DataList2.TotalItemCount = result.TotalRows;
                    if (result.CompanyPaymentDetails != null)
                    {
                        double totalamount = 0.00;
                        foreach (var item in result.CompanyPaymentDetails)
                        {
                            DataList2.Add(item);
                            SelectedBillList.Add(item);
                            totalamount += item.NetBillAmount;
                        }
                        txtClinicalTotal.Text = String.Format("{0:0.00}", totalamount);
                        if (DataList2.Count > 0)
                        {
                            txtinvoiceno.Text = DataList2[0].InvoiceNo;
                            dtpInvoice.SelectedDate = DataList2[0].InvoiceDate;
                            chkFreezBill1.IsChecked = DataList2[0].IsFreezed;
                            cmbCompanyList1.SelectedValue = DataList2[0].CompanyID;
                            cmbCompanyList1.IsEnabled = false;

                        }
                    }
                    dgCharges.ItemsSource = null;

                    dgCharges.ItemsSource = DataList2;
                    dgCharges.UpdateLayout();
                    //dgDataPager.Source = null;
                    //dgDataPager.PageSize = BizAction.MaximumRows;
                    //dgDataPager.Source = DataList2;

                    Indicator.Close();
                }
                else
                {
                    Indicator.Close();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void dgCompPayList_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            DataGridRow row = e.Row;
            if (((clsCompanyPaymentDetailsVO)(row.DataContext)).BalAmt > 0 && ((clsCompanyPaymentDetailsVO)(row.DataContext)).IsFreezed == true)
            {
                FrameworkElement fe = dgCompPayList.Columns[11].GetCellContent(e.Row);
                FrameworkElement parent = GetParent(fe, typeof(DataGridCell));
                var thisCell = (DataGridCell)parent;
                HyperlinkButton txt = thisCell.Content as HyperlinkButton;
                txt.Visibility = Visibility.Visible;

            }
            else
            {
                FrameworkElement fe = dgCompPayList.Columns[11].GetCellContent(e.Row);
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

        private void cmbCompanyList1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsView == true)
            {
                dgCharges.ItemsSource = null;
                SelectedBillList.Clear();
                DataList2.Clear();
                txtClinicalTotal.Text = "0.0";
            }
        }
    }
}
