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
using OPDModule;
using PalashDynamics.ValueObjects;
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.UserControls;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using CIMS;

namespace PalashDynamics.Administration
{
    public partial class DoctorPaymentModeWindow : ChildWindow
    {
        int ClickedFlag = 0;
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        WaitIndicator Indicatior = null;
        public bool FromPatient = false;
        enum PaymentForType
        {
            None = 0,
            Advance = 1,
            Refund = 2,
            Bill = 3
        }
        PaymentForType PaymentFor = PaymentForType.None;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        // List<clsPayment1> PaymentList = new List<clsPayment1>();

        public BillPaymentTypes BillPaymentType = BillPaymentTypes.AgainstBill;

        ObservableCollection<clsPayment1> PaymentList { get; set; }

        public clsPaymentVO Payment { get; set; }
        public long PaymentModeFromRegistrtion { get; set; }
        public bool IsSettleBill = false;
        public bool IsRefundAdvance = false;

        public double DoctorPaidAmount { get; set; }
        public double TotalTransactionAmount { get; set; }
        public double BalanceAmount { get; set; }

        #region IInitiateCIMS Members

        /// <summary>
        /// Function is for initializing the form based on Mode Passes as Paramater.
        /// </summary>
        /// <param name="Mode"></param>
        public void Initiate(string Mode)
        {
            try
            {

                SampleHeader.Text += " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                       " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                                       ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                SampleHeader.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                switch (Mode.ToUpper())
                {
                    case "ADVANCE":
                        PaymentFor = PaymentForType.Advance;
                        break;

                    case "REFUND":
                        PaymentFor = PaymentForType.Refund;
                        break;

                    case "BILL":
                        PaymentFor = PaymentForType.Bill;
                        break;


                    default:

                        break;
                }
            }
            catch (Exception)
            {

                // throw;
            }
        }

        #endregion

        /// <summary>
        /// Function is for fetching Bank List
        /// </summary>
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
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        /// <summary>
        /// Function is for fetching User List
        /// </summary>





        public static object[] GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            List<object> values = new List<object>();

            var fields = from field in enumType.GetFields()
                         where field.IsLiteral
                         select field;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values.ToArray();

        }

        public enum PaymentTransactionType
        {
            None = 0,
            SelfPayment = 1,
            CompanyPayment = 2,
            AdvancePayment = 3,
            RefundPayment = 4

        }
        public long PatientSourceID { get; set; }
        /// <summary>
        /// Function is for filling Payment Mode List 
        /// </summary>
        void FillPayMode()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);
            cmbPayMode.ItemsSource = mlPaymode;
            // cmbPayMode.SelectedItem = Default;
            // cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;

            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
            {
                cmbPayMode.SelectedItem = mlPaymode[9];
                TxtpaidAmt.Text = "0";
                cmdAddPayment.IsEnabled = true;

            }
            else if (PaymentModeFromRegistrtion == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
            {
                cmbPayMode.SelectedItem = mlPaymode[9];
                TxtpaidAmt.Text = "0";
                cmdAddPayment.IsEnabled = true;
            }
            else if (PatientSourceID == 2)
            {
                cmbPayMode.SelectedItem = mlPaymode[9];
                cmdAddPayment.IsEnabled = false;
            }
            else
            {
                cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
            }
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList, PaymentTransactionType sTransactionType)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.AdvancePayment || (PaymentTransactionType)sTransactionType == PaymentTransactionType.RefundPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance ||
                         (MaterPayModeList)Value == MaterPayModeList.StaffFree)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.SelfPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.CompanyAdvance || (MaterPayModeList)Value == MaterPayModeList.PatientAdvance)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.CompanyPayment)
                {
                    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance)
                        break;
                }
                else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.None)
                {
                    //Do Nothing
                }
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);

                TheMasterList.Add(Item);
            }
        }

        public DoctorPaymentModeWindow(Double selectedpayment)
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PaymentWindow_Loaded);
            //this.dgPayment.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPayment_CellEditEnded);
            //this.dgPayment.BindingValidationError += new EventHandler<ValidationErrorEventArgs>(dgPayment_BindingValidationError);
            TxtpaidAmt.Text = Convert.ToString(selectedpayment);
            TotalPaymentAmount = 0;
            TotalAmount = selectedpayment;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        void dgPayment_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            // throw new NotImplementedException();

            //((clsPayment1)dgPayment.SelectedItem).Amount = 0;
            dgPayment.CancelEdit(DataGridEditingUnit.Cell);
            //dgPayment.
        }

        /// <summary>
        /// Handles cell edit end event of the payment grid.
        /// Calls the method CalculateTotalPaidAmount.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgPayment_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //throw new NotImplementedException();
            if (dgPayment.SelectedItem != null)
            {


                CalculateTotalPaidAmount();
                if (TotalPaymentAmount > TotalAmount)
                {
                    ((clsPayment1)dgPayment.SelectedItem).Amount = 0;
                    //dgPayment.CancelEdit(DataGridEditingUnit.Cell);
                    CalculateTotalPaidAmount();
                    string msgTitle = "";
                    string msgText = "Total Amount is exceeding the Total Payment amount : " + TotalAmount;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgWD.Show();

                }

            }

        }

        /// <summary>
        /// Function is for calculating total paid amount
        /// </summary>
        void CalculateTotalPaidAmount()
        {
            double mTotalAmt = 0;
            foreach (var item in PaymentList)
            {
                mTotalAmt += item.Amount;


            }

            TotalPaymentAmount = mTotalAmt;
            clsPayment1 mPay1 = new clsPayment1();
            mPay1.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
            mPay1.PayMode = ((MasterListItem)cmbPayMode.SelectedItem).Description;
            if ((MaterPayModeList)mPay1.PayModeID == MaterPayModeList.Credit)
            {
                cmbPayMode.IsEnabled = true;
                TxtpaidAmt.Text = "0";
                TxtpaidAmt.IsEnabled = false;
            }
            else
            {
                TxtpaidAmt.Text = (TotalAmount - TotalPaymentAmount).ToString();
            }
        }

        void PaymentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TotalAmount = Math.Ceiling(TotalAmount);
            // txtPayableAmt.Text = TotalAmount.ToString();
            //  TxtpaidAmt.IsEnabled = false;
            txtNumber.IsEnabled = false;
            cmbBank.IsEnabled = false;
            dtpDate.IsEnabled = false;
            cmdAddPayment.IsEnabled = false;
          //   TxtpaidAmt.Text = TotalAmount.ToString();
            //cmbCheckAuthBy.IsEnabled = false;
            cmbCreditAuthBy.IsEnabled = false;
            //cmbCompanyAdvAuthBy.IsEnabled = false;
            //cmbConcessionAuthBy.IsEnabled = false;


            FillBank();
            FillPayMode();





            if (PaymentList == null)
            {
                PaymentList = new ObservableCollection<clsPayment1>();

                dgPayment.ItemsSource = PaymentList;
           }
            TxtpaidAmt.Text = TotalAmount.ToString();
        }



        /// <summary>
        /// Method checks for the validations and creates object of type clsPayment1VO as assigns values to it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            string msgTitle = "";
            string msgText = "";
            bool IsValid = true;

            if (ClickedFlag == 1)
            {
                CalculateTotalPaidAmount();

                //if (TotalPaymentAmount == 0)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    IsValid = false;
                //    msgW1.Show();
                //}
                //else
                if (cmbPayMode.SelectedItem != null)
                {
                    clsPayment1 Pay = new clsPayment1();
                    Pay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

                    if ((MaterPayModeList)Pay.PayModeID == MaterPayModeList.Credit)
                    {

                        if (cmbCreditAuthBy.SelectedItem == null)
                        {
                            cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                            cmbCreditAuthBy.TextBox.RaiseValidationError();
                            cmbCreditAuthBy.Focus();
                            IsValid = false;
                        }
                        else if (((MasterListItem)cmbCreditAuthBy.SelectedItem).ID == 0)
                        {
                            cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                            cmbCreditAuthBy.TextBox.RaiseValidationError();
                            cmbCreditAuthBy.Focus();
                            IsValid = false;
                        }
                        else
                            cmbCreditAuthBy.TextBox.ClearValidationError();
                    }
                }

                if (!string.IsNullOrEmpty(TxtpaidAmt.Text) && double.Parse(TxtpaidAmt.Text) > 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Please make a full Payment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValid = false;
                    msgW1.Show();
                }

                if (IsRefundAdvance == true)
                {
                    if (PaymentList.Count == 0)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0 ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();

                    }
                }

                if (IsSettleBill == true)
                {
                    if (PaymentList.Count == 0)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0 ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();

                    }
                }
                if (TotalPaymentAmount < TotalAmount && BillPaymentType == BillPaymentTypes.AgainstServices)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Total Payment must equals to Total amount : " + TotalAmount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValid = false;
                    msgW1.Show();

                }



            }

            PaymentFor = PaymentForType.Bill;
            if (IsValid)
            {
                switch (PaymentFor)
                {
                    case PaymentForType.None:
                        break;
                    case PaymentForType.Advance:
                        msgText = "Are you sure you want to save the Advance Details ?";
                        break;
                    case PaymentForType.Refund:
                        msgText = "Are you sure you want to save the Refund Details ?";
                        break;
                    case PaymentForType.Bill:
                        msgText = "Are you sure you want to save the Payment Details ?";
                        break;
                    default:
                        break;
                }

                MessageBoxControl.MessageBoxChildWindow msgW =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);


                msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                {

                    Payment = new clsPaymentVO();
                    Payment.PaymentDetails = new List<PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO>();


                    if (res == MessageBoxResult.Yes)
                    {
                        this.DialogResult = true;
                        // DateTime payDate = DateTime.Now;

                        DoctorPaidAmount = 0;
                        // Payment.co
                        for (int i = 0; i < PaymentList.Count; i++)
                        {
                            PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO objPay = new PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentDetailsVO();

                            objPay.PaymentModeID = PaymentList[i].PayModeID;
                            objPay.AdvanceID = PaymentList[i].AdvanceID;
                            objPay.PaidAmount = PaymentList[i].Amount;
                            objPay.Number = PaymentList[i].Number;
                            objPay.Date = PaymentList[i].Date;
                            if (PaymentList[i].BankID != null)
                                objPay.BankID = PaymentList[i].BankID.Value;
                            DoctorPaidAmount = DoctorPaidAmount + objPay.PaidAmount;

                            Payment.PaymentDetails.Add(objPay);
                        }
                        BalanceAmount = TotalAmount - DoctorPaidAmount;
                        if (OnSaveButton_Click != null)
                            OnSaveButton_Click(this, new RoutedEventArgs());
                    }
                    else
                        ClickedFlag = 0;
                };
                msgW.Show();
            }
            else
                ClickedFlag = 0;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (FromPatient == false)
            {
                this.DialogResult = false;
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
            }

            if (FromPatient == true)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment details incomplete,Do you want to save patient details without service details ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.OnMessageBoxClosed += (re) =>
                {
                    if (re == MessageBoxResult.Yes)
                    {
                        this.DialogResult = false;
                        if (OnCancelButton_Click != null)
                            OnCancelButton_Click(this, new RoutedEventArgs());

                    }
                };
                msgW1.Show();
            }
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

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }


        private void cmbPayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

                    }
                    else
                    {
                        txtNumber.IsEnabled = true;
                        cmbBank.IsEnabled = true;
                        dtpDate.IsEnabled = true;
                        dtpDate.SelectedDate = null;
                    }

                    txtNumber.MaxLength = 20;


                    cmbCreditAuthBy.IsEnabled = false;


                    switch ((MaterPayModeList)ID)
                    {
                        case MaterPayModeList.Credit:
                            cmbCreditAuthBy.IsEnabled = true;
                            cmbBank.IsEnabled = false;
                            txtNumber.IsEnabled = false;
                            dtpDate.IsEnabled = false;
                            break;
                        case MaterPayModeList.Cash:
                            break;
                        case MaterPayModeList.Cheque:
                            txtNumber.MaxLength = 6;

                            break;
                        case MaterPayModeList.DD:
                            txtNumber.MaxLength = 6;
                            break;
                        case MaterPayModeList.CreditCard:
                            txtNumber.MaxLength = 16;
                            break;
                        case MaterPayModeList.DebitCard:
                            txtNumber.MaxLength = 16;
                            break;
                        case MaterPayModeList.StaffFree:
                            break;
                        case MaterPayModeList.CompanyAdvance:

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

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Event is for deleting selected payment details from the List and calls the method CalculateTotalPaidAmount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentList != null)
            {
                if (dgPayment.SelectedItem != null)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to Delete the row";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            //TotalPaymentAmount -= ((clsPayment1)dgPayment.SelectedItem).Amount;
                            //CalculateTotalPaidAmount();
                            clsPayment1 item = (clsPayment1)dgPayment.SelectedItem;
                            if (item.AdvanceID > 0)
                            {
                                bool advanceFound = false;
                                List<clsAdvanceVO> AdvanceList = new List<clsAdvanceVO>();





                            }
                            PaymentList.Remove((clsPayment1)dgPayment.SelectedItem);

                            clsPayment1 mPay1 = new clsPayment1();
                            mPay1.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                            mPay1.PayMode = ((MasterListItem)cmbPayMode.SelectedItem).Description;
                            if ((MaterPayModeList)mPay1.PayModeID == MaterPayModeList.Credit)
                            {
                                cmbPayMode.IsEnabled = true;
                                TxtpaidAmt.Text = "0";
                                TxtpaidAmt.IsEnabled = false;
                                cmdAddPayment.IsEnabled = true;
                            }
                            CalculateTotalPaidAmount();

                            //CalculateTotalPaidAmount();
                        }
                    };

                    msgWD.Show();
                }

            }
        }

        private void dgPatientAdvDtl_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {



        }

        private void dgCompanyAdvDtl_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        /// <summary>
        /// This method checks the validation of the Payment details and adds the details into the payment list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdAddPayment_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            TxtpaidAmt.ClearValidationError();
            txtNumber.ClearValidationError();
            dtpDate.ClearValidationError();
            cmbBank.TextBox.ClearValidationError();

            clsPayment1 mPay = new clsPayment1();
            mPay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
            mPay.PayMode = ((MasterListItem)cmbPayMode.SelectedItem).Description;


            if (!((MaterPayModeList)mPay.PayModeID == MaterPayModeList.Cash || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.Credit || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.StaffFree))
            {
                if (((MasterListItem)cmbBank.SelectedItem).ID == 0)
                {
                    cmbBank.TextBox.SetValidation("Please select the bank");
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

                        if ((MaterPayModeList)mPay.PayModeID == MaterPayModeList.Cheque)
                        {
                            if (dtpDate.SelectedDate == null)
                            {
                                dtpDate.SetValidation("Date Required");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                isValid = false;
                            }
                            else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date.AddMonths(-3))
                            {
                                dtpDate.SetValidation("Cheque Date not less than three months.");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                isValid = false;
                            }
                            else
                            {
                                mPay.Date = dtpDate.SelectedDate;
                            }
                        }
                        else
                        {
                            if (dtpDate.SelectedDate == null)
                            {
                                dtpDate.SetValidation("Date Required");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                isValid = false;
                            }

                            else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                            {
                                dtpDate.SetValidation("Date must be greater than Today's Date.");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                isValid = false;
                            }
                            else
                            {
                                mPay.Date = dtpDate.SelectedDate;
                            }
                        }


                        if (txtNumber.Text != null && txtNumber.Text.Trim().Length > 0)
                        {
                            if (txtNumber.IsEnabled == true && txtNumber.Text.Length < txtNumber.MaxLength)
                            {
                                txtNumber.SetValidation("Number not valid");
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
                if (TxtpaidAmt.Text != null && TxtpaidAmt.Text.Length > 0)
                {
                    amt = double.Parse(TxtpaidAmt.Text);
                    if (amt > 0)
                    {
                        if ((amt + TotalPaymentAmount) > TotalAmount)
                        {
                            TxtpaidAmt.SetValidation("Total Amount is exceeding the Total Payment amount : " + TotalAmount);
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
                    else
                    {
                        if ((MaterPayModeList)mPay.PayModeID != MaterPayModeList.Credit)
                        {

                            TxtpaidAmt.SetValidation("Amount should be greater than 0");
                            TxtpaidAmt.RaiseValidationError();
                            TxtpaidAmt.Focus();
                            isValid = false;
                        }
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

            if (isValid)
            {
                TxtpaidAmt.Text = "0";
                txtNumber.Text = "";
                cmbBank.SelectedValue = (long)0;
                dtpDate.SelectedDate = null;
                PaymentList.Add(mPay);
                CalculateTotalPaidAmount();

                if ((MaterPayModeList)mPay.PayModeID == MaterPayModeList.Credit)
                {
                    cmbPayMode.IsEnabled = false;
                    TxtpaidAmt.IsEnabled = false;
                    cmdAddPayment.IsEnabled = false;
                }
            }
        }

        /// <summary>
        /// This method checks the validation for entered advance amount to be consumed 
        /// against selected advance in the grid and adds payment details into the Payment List.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void txtNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumber.IsEnabled)
            {
                if (txtNumber.Text.Length < txtNumber.MaxLength)
                {
                    txtNumber.SetValidation("Number not valid");
                    txtNumber.RaiseValidationError();
                }
                else
                    txtNumber.ClearValidationError();

                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;

            }
        }



        /// <summary>
        /// Method is for opening calculator window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //CalculateBillAmounttobeRecieveandRefund CalcWin = new CalculateBillAmounttobeRecieveandRefund();
            //CalcWin.txtBillAmt.Text = TotalPaymentAmount.ToString();
            //CalcWin.Show();
        }

        private void TxtTotal_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TxtTotal_KeyDown(object sender, KeyEventArgs e)
        {

        }



    }

    public class clsPayment1
    {
        public long PayModeID { get; set; }
        public long? AdvanceID { get; set; }
        public string PayMode { get; set; }
        public double Amount { get; set; }
        public string Number { get; set; }
        public string BankName { get; set; }
        public long? BankID { get; set; }
        public DateTime? Date { get; set; }
    }
}

