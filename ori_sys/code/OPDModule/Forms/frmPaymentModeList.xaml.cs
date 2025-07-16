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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.Collections;
using PalashDynamics.UserControls;
using CIMS;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;

namespace OPDModule.Forms
{
    public partial class frmPaymentModeList : ChildWindow
    {
      
        public string MRNO = string.Empty;
        public long UnitID = 0;
        public long BillID = 0;
        double TotalPaymentAmount { get; set; }
        public double TotalAmount { get; set; }
        ObservableCollection<clsPayment> PaymentList { get; set; }
        ObservableCollection<clsBillVO> PaymentListNew { get; set; }

        public PagedSortableCollectionView<clsBillVO> DataList { get; private set; }
        WaitIndicator indicator = new WaitIndicator();
        public event RoutedEventHandler OnCancelButton_Click;
        PaymentForType PaymentFor = PaymentForType.None;
        public clsPaymentVO Payment { get; set; }
        int ClickedFlag = 0;
        public bool IsRefundAdvance = false;
        public bool IsSettleBill = false;
        public bool IsSettleMentBill { get; set; }
        public bool IsEdit = false;
        public event RoutedEventHandler OnSaveButton_Click;

        public frmPaymentModeList()
        {
            InitializeComponent();
            TotalPaymentAmount = 0;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataList = new PagedSortableCollectionView<clsBillVO>();
           
            TxtpaidAmt.IsEnabled = false;
            cmbPayMode.IsEnabled = false;
            txtNumber.IsEnabled = false;
            cmbBank.IsEnabled = false;
            dtpDate.IsEnabled = false;
            cmdAddPayment.IsEnabled = false;
            TxtpaidAmt.Text = TotalAmount.ToString();
            cmbCheckAuthBy.IsEnabled = false;
            cmbCreditAuthBy.IsEnabled = false;
            cmbCompanyAdvAuthBy.IsEnabled = false;
            fillPaymentModeList();

            FillUserList();
          //  FillBank();
            FillPayMode();
           // FillConcessionAutBy();

            //if (PaymentFor == PaymentForType.Advance || PaymentFor == PaymentForType.Refund)
            //{
            //   // tabControl1.Visibility = System.Windows.Visibility.Collapsed;
            //    //PanelBillDetails.Visibility = System.Windows.Visibility.Collapsed;
            //    //borderPaySummary.Visibility = System.Windows.Visibility.Collapsed;
            //}
            //else if (PaymentFor == PaymentForType.Bill)
            //{
            //    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID)
            //    {
            //        //tabCompanyAdvance.IsEnabled = false;
            //       // dgCompanyAdvDtl.IsEnabled = false;
            //    }
            //    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
            //    {
            //        FillPatientAdvance();
            //    }
            //}
            if (PaymentList == null)
            {
                PaymentList = new ObservableCollection<clsPayment>();
                //dgPayment.ItemsSource = PaymentList;
            }
           
        }
        void CalculateTotalPaidAmount()
        {
            double mTotalAmt = 0;
            foreach (var item in PaymentList)
            {
                mTotalAmt += item.Amount;
            }

            TotalPaymentAmount = mTotalAmt;
            //if (TxtConAmt.Text != null && TxtConAmt.Text != "")
            //{
            //    TxtpaidAmt.Text = ((TotalAmount - Convert.ToDouble(TxtConAmt.Text)) - TotalPaymentAmount).ToString();
            //    TotalConAmount = Convert.ToDouble(TxtConAmt.Text);
            //}
            //else
            //{
            //    TxtpaidAmt.Text = ((TotalAmount) - TotalPaymentAmount).ToString();
            //}
        }

        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {
            clsUpdatePaymentDetailsBizActionVO bizAction = new clsUpdatePaymentDetailsBizActionVO();
            ClickedFlag += 1;
            string msgTitle = "";
            string msgText = "";
            bool IsValid = true;
            TxtpaidAmt.IsEnabled = false;

            if (ClickedFlag == 1)
            {
                //CalculateTotalPaidAmount();

                if (cmbPayMode.SelectedItem != null)
                {
                    clsPayment Pay = new clsPayment();
                    Pay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;


                    if ((MaterPayModeList)Pay.PayModeID == MaterPayModeList.Credit)
                    {

                        //if (cmbCreditAuthBy.SelectedItem == null)
                        //{
                        //    cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                        //    cmbCreditAuthBy.TextBox.RaiseValidationError();
                        //    cmbCreditAuthBy.Focus();
                        //    IsValid = false;
                        //}
                        //else if (((MasterListItem)cmbCreditAuthBy.SelectedItem).ID == 0)
                        //{
                        //    cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                        //    cmbCreditAuthBy.TextBox.RaiseValidationError();
                        //    cmbCreditAuthBy.Focus();
                        //    IsValid = false;
                        //}
                        //else
                        //    cmbCreditAuthBy.TextBox.ClearValidationError();
                    }
                    else if (((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.Cheque || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.CreditCard || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.DD || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.DebitCard)
                    {
                        if (cmbBank.SelectedItem != null && ((MasterListItem)cmbBank.SelectedItem).ID > 0)
                        {
                            cmbBank.ClearValidationError();
                    //        IsValid = true;
                        }
                        else
                        {
                            cmbBank.TextBox.SetValidation("Please Select The Bank.");
                            cmbBank.TextBox.RaiseValidationError();
                            cmbBank.Focus();
                            IsValid = false;
                        }
                        if (((MasterListItem)cmbBank.SelectedItem).ID > 0)
                        {
                            if (dtpDate.SelectedDate == null)
                            {
                                dtpDate.SetValidation("Date Required");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                IsValid = false;
                            }
                            else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                            {
                                dtpDate.SetValidation("Date Must Be Greater Than Today's Date.");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                IsValid = false;
                            }
                            else
                            {
                                dtpDate.ClearValidationError();
                             //   IsValid = true;
                            }

                            if (txtNumber.Text != null && txtNumber.Text.Trim().Length > 0)
                            {
                                if (txtNumber.IsEnabled == true && txtNumber.Text.Length < txtNumber.MaxLength)
                                {
                                    txtNumber.SetValidation("Number Not Valid");
                                    txtNumber.RaiseValidationError();
                                    txtNumber.Focus();
                                    IsValid = false;
                                }
                                else
                                {
                                    txtNumber.ClearValidationError();
                            //        IsValid = true;
                                }
                            }
                            else
                            {
                                txtNumber.SetValidation("Number Is Required");
                                txtNumber.RaiseValidationError();
                                txtNumber.Focus();
                                IsValid = false;
                            }
                        }
                    }
                    if (((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.Cash)
                    {
                        txtNumber.Text = " ";
                        cmbBank.ItemsSource = null;
                        txtNumber.IsEnabled = false;
                        cmbBank.IsEnabled = false;
                    }

                }
                #region
                //if (IsRefundAdvance == true)
                //{
                //    if (PaymentList.Count == 0)
                //    {

                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0 ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        IsValid = false;
                //        msgW1.Show();

                //    }
                //}

                //if (IsSettleBill == true)
                //{
                //    if (PaymentList.Count == 0)
                //    {

                //        MessageBoxControl.MessageBoxChildWindow msgW1 =
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0 ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //        IsValid = false;
                //        msgW1.Show();

                //    }
                //}

                
                //if (txtDiscAmt.Text != null && txtDiscAmt.Text != "" && Convert.ToDouble(txtDiscAmt.Text) > 0.0)
                //{
                //    if (ConcessionFromPlan == false)
                //    {
                //        if (cmbConcessionAuthBy.SelectedItem != null)
                //        {
                //            if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID > 0)
                //            {
                //                if (txtNarration.Text == null)
                //                {
                //                    txtNarration.SetValidation("Please enter narration");
                //                    txtNarration.RaiseValidationError();
                //                    txtNarration.Focus();
                //                    IsValid = false;

                //                }
                //                else if (txtNarration.Text == "")
                //                {
                //                    txtNarration.SetValidation("Please enter narration");
                //                    txtNarration.RaiseValidationError();
                //                    txtNarration.Focus();
                //                    IsValid = false;

                //                }
                //                else
                //                    txtNarration.ClearValidationError();

                //            }

                //        }

                //        if (cmbConcessionAuthBy.SelectedItem == null)
                //        {
                //            cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                //            cmbConcessionAuthBy.TextBox.RaiseValidationError();
                //            cmbConcessionAuthBy.Focus();
                //            IsValid = false;
                //        }
                //        else if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID == 0)
                //        {
                //            cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                //            cmbConcessionAuthBy.TextBox.RaiseValidationError();
                //            cmbConcessionAuthBy.Focus();
                //            IsValid = false;
                //        }
                //        else
                //            cmbConcessionAuthBy.TextBox.ClearValidationError();
                //    }

                //}

                //if (TxtConAmt.Text != null && TxtConAmt.Text != "" && Convert.ToDouble(TxtConAmt.Text) > 0.0)
                //{


                //    if (cmbConcessionAuthBy.SelectedItem == null)
                //    {
                //        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                //        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                //        cmbConcessionAuthBy.Focus();
                //        IsValid = false;
                //    }
                //    else if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID == 0)
                //    {
                //        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                //        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                //        cmbConcessionAuthBy.Focus();
                //        IsValid = false;
                //    }
                //    else
                //        cmbConcessionAuthBy.TextBox.ClearValidationError();

                //}
               
                /*For Checking The PaymentList Count which is Inserted into the PaymentDetails IF Paid Amount > 0 (i.e. TotalPaymentAmount) */


                //if ((PaymentList == null || PaymentList.Count == 0) && TotalPaymentAmount > 0)
                //{
                //    IsValid = false;
                //    msgText = "Payment Details Are not Added Properly";
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                //    msgW1.Show();

                //}

                //if ((Convert.ToDouble(TxtpaidAmt.Text) > (((IApplicationConfiguration)App.Current).ApplicationConfigurations).CreditLimitOPD))
                //{
                //    IsValid = false;
                //    msgText = "Credit Amount Must be less than vredit limit";
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //    msgW1.Show();

                //}

                #endregion

                if (IsValid)
                {
#region
                    //switch (PaymentFor)
                    //{
                    //    case PaymentForType.None:
                    //        break;
                    //    case PaymentForType.Advance:
                    //        msgText = "Are you sure you want to save the Advance Details ?";
                    //        break;
                    //    case PaymentForType.Refund:
                    //        msgText = "Are you sure you want to save the Refund Details ?";
                    //        break;
                    //    case PaymentForType.Bill:
                    //        msgText = "Are you sure you want to save the Payment Details ?";
                    //        break;
                    //    default:
                    //        break;
                    //}



                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //   new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    //msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                    //{
                    //    if (res == MessageBoxResult.Yes)
                    //    {
                            //this.DialogResult = true;

                            //Payment = new clsPaymentVO();

                            //Payment.PayeeNarration = txtNarration.Text;
                            //Payment.CreditAuthorizedBy = 0;
                            //Payment.ChequeAuthorizedBy = 0;
                            //Payment.PaidAmount = TotalPaymentAmount;

                            //if (IsSettleMentBill == true)
                            //{
                            //    Payment.SettlementConcessionAmount = TotalConAmount;
                            //}

                            //if (PaymentFor == PaymentForType.Bill)
                            //{

                            //    Payment.BillPaymentType = this.BillPaymentType;
                            //    Payment.BillAmount = Convert.ToDouble(txtPayTotalAmount.Text);

                            //    // In Case OF Bills  Concensation Then The Payment table BillBalanceAmount Settelemt Done Through Here //
                            //    if (TxtConAmt.Text != null && TxtConAmt.Text != "")
                            //    {
                            //        //Payment.BillBalanceAmount = Payment.BillAmount - (Payment.PaidAmount + Convert.ToDouble((TxtConAmt.Text)));
                            //        Payment.BillBalanceAmount = TotalAmount - (Payment.PaidAmount + Convert.ToDouble((TxtConAmt.Text)));
                            //    }
                            //    else
                            //    {
                            //        //Payment.BillBalanceAmount = Payment.BillAmount - Payment.PaidAmount;
                            //        Payment.BillBalanceAmount = TotalAmount - Payment.PaidAmount;
                            //    }
                            //}



                            //if (cmbCreditAuthBy.SelectedItem != null)
                            //    Payment.CreditAuthorizedBy = ((MasterListItem)cmbCreditAuthBy.SelectedItem).ID;

                            //if (cmbConcessionAuthBy.SelectedItem != null)
                            //    Payment.ChequeAuthorizedBy = ((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID;

                            //if (cmbCompanyAdvAuthBy.SelectedItem != null)
                            //    Payment.ComAdvAuthorizedBy = ((MasterListItem)cmbCompanyAdvAuthBy.SelectedItem).ID;


                            //for (int i = 0; i < PaymentList.Count; i++)
                            //{
                            //    clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();

                            //    objPay.PaymentModeID = PaymentList[i].PayModeID;
                            //    objPay.AdvanceID = PaymentList[i].AdvanceID;
                            //    objPay.PaidAmount = PaymentList[i].Amount;
                            //    objPay.Number = PaymentList[i].Number;
                            //    objPay.Date = PaymentList[i].Date;
                            //    if (PaymentList[i].BankID != null)
                            //        objPay.BankID = PaymentList[i].BankID.Value;

                            //    Payment.PaymentDetails.Add(objPay);
                            //}

                            //for (int i = 0; i < PaymentList.Count; i++)
                    //{

# endregion
                                clsUpdatePaymentDetailsBizActionVO objPay = new clsUpdatePaymentDetailsBizActionVO();

                                objPay.PaymentModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                                objPay.PaidAmount = Convert.ToDouble(TxtpaidAmt.Text);
                                objPay.Number = txtNumber.Text;
                                if (cmbBank.SelectedItem != null)
                                {
                                    objPay.BankID = ((MasterListItem)cmbBank.SelectedItem).ID;
                                }
                                
                                //objPay.BiLLNo = ((clsBillVO)dgPaymentModeList.SelectedItem).BillNo;
                                objPay.PaymentDetailId = ((clsBillVO)dgPaymentModeList.SelectedItem).PaymentDetailID;
                                objPay.PaymentId = ((clsBillVO)dgPaymentModeList.SelectedItem).PaymentID;
                                objPay.UnitID = ((clsBillVO)dgPaymentModeList.SelectedItem).UnitID;
                                if (((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.Cash || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.StaffFree)
                                {
                                    objPay.Date = DateTime.Now;
                                   // objPay.BankID = ((MasterListItem)cmbBank.SelectedItem).ID;
                                }
                                else
                                {
                                    objPay.Date = dtpDate.SelectedDate.Value;
                                }

                                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                                client.ProcessCompleted += (s, arg) =>
                                  {
                                      if (arg.Error == null)
                                      {
                                          
                                          MessageBoxControl.MessageBoxChildWindow msgW3 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "PaymentMode modified successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                          msgW3.Show();
                                          msgW3.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                                          ClickedFlag = 0;

                                          cmbBank.IsEnabled = false;
                                          cmbPayMode.IsEnabled = false;
                                          txtNumber.IsEnabled = false;
                                          TxtpaidAmt.IsEnabled = false;
                                          dtpDate.IsEnabled = false;
                                          cmbPayMode.SelectedValue = (long)0;
                                         // cmbPayMode.ItemsSource = null;
                                        //  cmbBank.ItemsSource = null;
                                          txtNumber.Text = string.Empty;
                                          TxtpaidAmt.Text = string.Empty;
                                        //  fillPaymentModeList();
                                      }



                                      else
                                      {
                                          MessageBoxControl.MessageBoxChildWindow msgW2 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "PaymentMode modificatoion is unsuccessfull", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                          msgW2.Show();
                                          ClickedFlag = 0; 
                                      }
                                  };

                                client.ProcessAsync(objPay, ((IApplicationConfiguration)App.Current).CurrentUser);
                                client.CloseAsync();                           
                                                                              
                                



                            //}



                            //if (OnSaveButton_Click != null)
                            //    OnSaveButton_Click(this, new RoutedEventArgs());
                        }
                        else
                            ClickedFlag = 0;
                    //};
                    //msgW.Show();
                //}
                //else
                //    ClickedFlag = 0;
            }

        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            clsGetFreezedBillListBizActionVO BizAction = new clsGetFreezedBillListBizActionVO();



            BizAction.MRNO = MRNO;
            BizAction.UnitID = UnitID;
            BizAction.BillID = BillID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPaymentModeList.ItemsSource = null;
            Client.ProcessCompleted += (s, e) =>
            {
                //win.dgPaymentModeList.ItemsSource = ((clsGetFreezedBillListBizActionVO)e.Result).AppointmentList;
                if (e.Error == null && e.Result != null)
                {
                    clsGetFreezedBillListBizActionVO result1 = e.Result as clsGetFreezedBillListBizActionVO;



                    DataList.Clear();
                    //  DataList.TotalItemCount = result.TotalRows;
                    if (result1.List != null)
                    {

                        foreach (var item in result1.List)
                        {
                            item.Date1 = item.Date.ToShortDateString();
                            //item.PatientName = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                            DataList.Add(item);

                        }
                    }
                    dgPaymentModeList.ItemsSource = null;
                    dgPaymentModeList.ItemsSource = DataList;


                    //win.dgPaymentModeList.Source = null;
                    //win.dgPaymentModeList.PageSize = BizAction.MaximumRows;
                    //dgDataPager.Source = DataList;


                    // checkFreezColumn = true;      
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        
        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {

            if (OnCancelButton_Click != null)
            {
                OnCancelButton_Click((clsBillVO)(this.DataContext), e);
            }
            this.DialogResult = false;
            //this.Close();
          


            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.frmChangePaymentMode");

            ////if (OnCancelButton_Click != null)
            ////    OnCancelButton_Click(this, new RoutedEventArgs());


            //base.OnClosed(e);
            //Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            ClickedFlag = 0; 
        }

        void FillPayMode()
        {
            // List<MasterListItem> mlPaymode = new List<MasterListItem>();
            // MasterListItem Default = new MasterListItem(0, "- Select -");
            // mlPaymode.Insert(0, Default);
            // EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);
            // cmbPayMode.ItemsSource = mlPaymode;
            //// cmbPayMode.SelectedItem = Default;
            // cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ModeOfPayment;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbPayMode.ItemsSource = null;
                    cmbPayMode.ItemsSource = objList;
                   
                    cmbPayMode.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
            
        }

        enum PaymentForType
        {
            None = 0,
            Advance = 1,
            Refund = 2,
            Bill = 3
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;


        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
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
         
       
        private void FillBank()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BankMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    //cmbMaritalStatus.ItemsSource = null;
                    //cmbMaritalStatus.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbBank.ItemsSource = null;
                    cmbBank.ItemsSource = objList;
                
                        cmbBank.SelectedItem = objList[0];
                    
                }
            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }
        private void FillUserList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    //objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);

                    cmbCreditAuthBy.ItemsSource = null;
                    cmbCreditAuthBy.ItemsSource = objList;
                    cmbCreditAuthBy.SelectedItem = objList[0];

                    cmbCheckAuthBy.ItemsSource = null;
                    cmbCheckAuthBy.ItemsSource = objList;
                    cmbCheckAuthBy.SelectedItem = objList[0];

                    cmbCompanyAdvAuthBy.ItemsSource = null;
                    cmbCompanyAdvAuthBy.ItemsSource = objList;
                    cmbCompanyAdvAuthBy.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }
        private void FillConcessionAutBy()
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    if (((clsGetStaffMasterDetailsBizActionVO)e.Result).StaffMasterList != null)
                    {
                        clsGetStaffMasterDetailsBizActionVO result = e.Result as clsGetStaffMasterDetailsBizActionVO;

                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        foreach (var item in result.StaffMasterList)
                        {
                            MasterListItem Obj = new MasterListItem();
                            Obj.ID = item.ID;
                            Obj.Description = (item.FirstName + " " + item.MiddleName + " " + item.LastName);
                            Obj.Status = item.Status;
                            objList.Add(Obj);
                        }


                        cmbConcessionAuthBy.ItemsSource = null;
                        cmbConcessionAuthBy.ItemsSource = objList;
                        cmbConcessionAuthBy.SelectedItem = objList[0];
                    }
                }


            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }


        private void cmdAddPayment_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            TxtpaidAmt.IsEnabled = false;
            TxtpaidAmt.ClearValidationError();
            txtNumber.ClearValidationError();
            dtpDate.ClearValidationError();
            cmbBank.TextBox.ClearValidationError();

            clsPayment mPay = new clsPayment();
            mPay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
            mPay.PayMode = ((MasterListItem)cmbPayMode.SelectedItem).Description;
            if (!((MaterPayModeList)mPay.PayModeID == MaterPayModeList.Cash || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.StaffFree))
            {
                if (((MasterListItem)cmbBank.SelectedItem).ID == 0)
                {
                    cmbBank.TextBox.SetValidation("Please Select The Bank");
                    cmbBank.TextBox.RaiseValidationError();
                    cmbBank.Focus();
                    isValid = false;
                }

                if (isValid)
                {
                    if (((MasterListItem)cmbBank.SelectedItem).ID > 0)
                    {
                        mPay.BankID = ((MasterListItem)cmbBank.SelectedItem).ID;
                        mPay.BankName = ((MasterListItem)cmbBank.SelectedItem).Description;

                        if (dtpDate.SelectedDate == null)
                        {
                            dtpDate.SetValidation("Date Required");
                            dtpDate.RaiseValidationError();
                            dtpDate.Focus();
                            isValid = false;
                        }
                        else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                        {
                            dtpDate.SetValidation("Date Must Be Greater Than Today's Date.");
                            dtpDate.RaiseValidationError();
                            dtpDate.Focus();
                            isValid = false;
                        }
                        else
                        {
                            mPay.Date = dtpDate.SelectedDate;
                        }

                        if (txtNumber.Text != null && txtNumber.Text.Trim().Length > 0)
                        {
                            if (txtNumber.IsEnabled == true && txtNumber.Text.Length < txtNumber.MaxLength)
                            {
                                txtNumber.SetValidation("Number Not Valid");
                                txtNumber.RaiseValidationError();
                                txtNumber.Focus();
                                isValid = false;
                            }
                            else
                            {
                                txtNumber.ClearValidationError();
                                mPay.Number = txtNumber.Text;
                            }
                        }
                        else
                        {
                            txtNumber.SetValidation("Number Required");
                            txtNumber.RaiseValidationError();
                            txtNumber.Focus();
                            isValid = false;
                        }
                    }
                }
            }

            if (isValid)
            {
                double amt = 0;
                if (TxtpaidAmt.Text != null && TxtpaidAmt.Text.Trim().Length > 0)
                {
                    if (TxtpaidAmt.IsEnabled == true && TxtpaidAmt.Text.Length > TxtpaidAmt.MaxLength)
                    {
                        TxtpaidAmt.SetValidation("Amount Not Valid");
                        TxtpaidAmt.RaiseValidationError();
                        TxtpaidAmt.Focus();
                        isValid = false;
                    }
                    else
                    {
                        TxtpaidAmt.ClearValidationError();
                        mPay.Number = txtNumber.Text;
                    }
                }

                
                if (TxtpaidAmt.Text != null && TxtpaidAmt.Text.Length > 0)
                {
                    amt = double.Parse(TxtpaidAmt.Text);
                    if (amt > 0)
                    {
                        if ((amt + TotalPaymentAmount) > TotalAmount)
                        {
                            TxtpaidAmt.SetValidation("Total Amount Is Exceeding The Total Payment Amount : " + TotalAmount);
                            TxtpaidAmt.RaiseValidationError();
                            TxtpaidAmt.Focus();
                            isValid = false;
                        }
                        else
                        {
                            mPay.Amount = double.Parse(TxtpaidAmt.Text);
                            TotalPaymentAmount += amt;
                        }
                    }
                    else if (double.Parse(TxtpaidAmt.Text) < 0)
                    {
                        TxtpaidAmt.SetValidation("Amount Cannot Be Negative");
                        TxtpaidAmt.RaiseValidationError();
                        TxtpaidAmt.Focus();
                        isValid = false;
                    }
                }
                else
                {
                    TxtpaidAmt.SetValidation("Amount Required");
                    TxtpaidAmt.RaiseValidationError();
                    TxtpaidAmt.Focus();
                    isValid = false;
                }
            }

            ////Added By Yogita
            #region
            //if (Convert.ToDouble(txtPatTotalAmt.Text) < 0.0)
            //{
            //    txtPatTotalAmt.SetValidation("Amount Cannot Be Negative.");
            //    txtPatTotalAmt.RaiseValidationError();
            //    txtPatTotalAmt.Focus();
            //}
            //else
            //{
            //    txtPatTotalAmt.ClearValidationError();
            //}
            //if (Convert.ToDouble(txtPatBalanceAmt.Text) < 0.0)
            //{
            //    txtPatBalanceAmt.SetValidation("Amount Cannot Be Negative.");
            //    txtPatBalanceAmt.RaiseValidationError();
            //    txtPatBalanceAmt.Focus();

            //}
            //else
            //{
            //    txtPatBalanceAmt.ClearValidationError();
            //}
            //if (Convert.ToDouble(txtPatConsumedAmt.Text) < 0.0)
            //{

            //    txtPatConsumedAmt.SetValidation("Amount Cannot Be Negative.");
            //    txtPatConsumedAmt.RaiseValidationError();
            //    txtPatConsumedAmt.Focus();

            //}
            //else
            //{
            //    txtPatConsumedAmt.ClearValidationError();
            //}
            ////End
            #endregion
            if (isValid)
            {
                TxtpaidAmt.Text = "0";
                txtNumber.Text = "";
                cmbBank.SelectedValue = (long)0;
                dtpDate.SelectedDate = null;
                PaymentList.Add(mPay);
                TxtpaidAmt.Text = (TotalAmount - TotalPaymentAmount).ToString();
            }

        }
        private void txtReceivedAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtReceivedAmt.Text) && txtReceivedAmt.Text.IsNumberValid())
            //{
            //    double receivedAmt = Convert.ToDouble(txtReceivedAmt.Text);
            //    txtAmtToBeReturn.Text = (receivedAmt - TotalPaymentAmount).ToString();
            //}
        }
     
      

        private void cmbPayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TxtpaidAmt.IsEnabled = false;
            try{
                if (cmbPayMode.SelectedItem != null)
                {
                    long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                    if (ID > 0)
                    {
                        cmdAddPayment.IsEnabled = true;
                        TxtpaidAmt.IsEnabled = true;
                        if (((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
                        {
                            txtNumber.IsEnabled = false;
                            cmbBank.IsEnabled = false;
                            dtpDate.IsEnabled = false;
                            TxtpaidAmt.IsEnabled = false;
                            
                                txtNumber.Text = " ";
                                cmbBank.ItemsSource = null;
                                txtNumber.IsEnabled = false;
                                cmbBank.IsEnabled = false;
                                dtpDate.SelectedDate = null;
                        }
                        else
                        {
                            TxtpaidAmt.IsEnabled = false;
                            txtNumber.IsEnabled = true;
                            cmbBank.IsEnabled = true;
                            dtpDate.IsEnabled = true;
                            dtpDate.SelectedDate = null;
                            FillBank();
                        }
                        txtNumber.MaxLength = 20;
                        TxtpaidAmt.MaxLength = 5;
                        cmbCheckAuthBy.IsEnabled = false;
                        cmbCreditAuthBy.IsEnabled = false;
                        cmbCompanyAdvAuthBy.IsEnabled = false;

                        switch ((MaterPayModeList)ID)
                        {
                            
                            case MaterPayModeList.Credit:
                                cmbCreditAuthBy.IsEnabled = true;
                                break;
                            case MaterPayModeList.Cash:
                                txtNumber.MaxLength = 6;
                                break;
                            case MaterPayModeList.Cheque:
                                txtNumber.MaxLength = 6;
                                cmbCheckAuthBy.IsEnabled = true;
                                break;
                            case MaterPayModeList.DD:
                                txtNumber.MaxLength = 6;
                                break;
                            case MaterPayModeList.CreditCard:
                                txtNumber.MaxLength = 4;
                                break;
                            case MaterPayModeList.DebitCard:
                                txtNumber.MaxLength = 4;
                                break;
                            case MaterPayModeList.StaffFree:
                                break;
                            case MaterPayModeList.CompanyAdvance:
                                cmbCompanyAdvAuthBy.IsEnabled = true;
                                break;
                            case MaterPayModeList.PatientAdvance:
                                break;
                            default:
                                break;
                        }

                        TxtpaidAmt.Focus();
                    }
                    else
                    {
                        TxtpaidAmt.IsEnabled = false;
                        txtNumber.IsEnabled = false;
                        cmbBank.IsEnabled = false;
                        dtpDate.IsEnabled = false;
                        cmdAddPayment.IsEnabled = false;
                    }
                }
            }
            catch (Exception E)
            {

            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
             textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtNumber_LostFocus(object sender, RoutedEventArgs e)
        {

        }

      
        private void fillPaymentModeList()
        {
            clsGetFreezedBillListBizActionVO BizAction = new clsGetFreezedBillListBizActionVO();
            
            

            BizAction.MRNO = MRNO;
            BizAction.UnitID = UnitID;
            BizAction.BillID = BillID;
            
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPaymentModeList.ItemsSource = null;
            Client.ProcessCompleted += (s, e) =>
            {
                //win.dgPaymentModeList.ItemsSource = ((clsGetFreezedBillListBizActionVO)e.Result).AppointmentList;
                if (e.Error == null && e.Result != null)
                {
                    clsGetFreezedBillListBizActionVO result = e.Result as clsGetFreezedBillListBizActionVO;



                   // DataList.Clear();
                  //  DataList.TotalItemCount = result.TotalRows;
                    if (result.List != null)
                    {

                        foreach (var item in result.List)
                        {
                            item.Date1 = item.Date.ToShortDateString();
                            //item.PatientName = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                            DataList.Add(item);
                            
                        } 
                    }
                    dgPaymentModeList.ItemsSource = null;
                    dgPaymentModeList.ItemsSource = DataList;

                    //win.dgPaymentModeList.Source = null;
                    //win.dgPaymentModeList.PageSize = BizAction.MaximumRows;
                    //dgDataPager.Source = DataList;


                    // checkFreezColumn = true;      
                }
                indicator.Close();
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbEdit_Click(object sender, RoutedEventArgs e)
        {

            IsEdit = true;
            clsMaintainPaymentModeLogBizActionVO BizAction = new clsMaintainPaymentModeLogBizActionVO();
            
            cmbBank.IsEnabled = false;
            txtNumber.IsEnabled = false;
            cmbPayMode.IsEnabled = true; 
            TxtpaidAmt.IsEnabled = false;
            dtpDate.IsEnabled = true;
           // FillPayMode();
          //  cmbPayMode.SelectedValue = ((MasterListItem)dgPaymentModeList.SelectedItem).ID;
            cmbPayMode.SelectedValue = ((clsBillVO)dgPaymentModeList.SelectedItem).PaymentModeId;
            dtpDate.SelectedDate = ((clsBillVO)dgPaymentModeList.SelectedItem).Date.Date;
            if (((clsBillVO)dgPaymentModeList.SelectedItem).PaymentMode != "Cash")
            {
                TxtpaidAmt.IsEnabled = false;
                cmbBank.IsEnabled = true;
                txtNumber.IsEnabled = true;
                FillBank();
            }
               
            TxtpaidAmt.Text = ((clsBillVO)dgPaymentModeList.SelectedItem).PaidAmount.ToString();
            if (((clsBillVO)dgPaymentModeList.SelectedItem).Number != null || ((clsBillVO)dgPaymentModeList.SelectedItem).Bank != null)
            {
                TxtpaidAmt.IsEnabled = false;
               
                txtNumber.Text = ((clsBillVO)dgPaymentModeList.SelectedItem).Number.ToString();
                //cmbBank.SelectedItem = ((clsBillVO)dgPaymentModeList.SelectedItem).Bank;
                cmbBank.SelectedValue = ((clsBillVO)dgPaymentModeList.SelectedItem).Bank;
            }
              
               

                BizAction.PaymentID = ((clsBillVO)dgPaymentModeList.SelectedItem).PaymentID;
                BizAction.PaymentDetailId = ((clsBillVO)dgPaymentModeList.SelectedItem).PaymentDetailID;
                TxtpaidAmt.IsEnabled = false;


            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

         
            //Client.ProcessCompleted += (s, arg) =>
            //{
            //    if (arg.Error == null)
            //    {
            //        //MessageBoxControl.MessageBoxChildWindow msgW3 =
            //        //new MessageBoxControl.MessageBoxChildWindow("Palash", "PaymentMode modified successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        //msgW3.Show();
            //    }



            //    else
            //    {
            //        //MessageBoxControl.MessageBoxChildWindow msgW2 =
            //        //new MessageBoxControl.MessageBoxChildWindow("Palash", "PaymentMode modificatoion is unsuccessfull", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
            //        //msgW2.Show();
            //    }

            //};

            //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //Client.CloseAsync();




        }

        private void cmbBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}

