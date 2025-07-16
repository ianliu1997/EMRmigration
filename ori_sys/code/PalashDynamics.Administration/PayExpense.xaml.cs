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
using CIMS;
using PalashDynamics.ValueObjects.Billing;

namespace PalashDynamics.Administration
{
    public partial class PayExpense : ChildWindow
    {
        public PayExpense()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PayExpense_Loaded);
            this.dgPayment.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPayment_CellEditEnded);
            this.dgPayment.BindingValidationError += new EventHandler<ValidationErrorEventArgs>(dgPayment_BindingValidationError);
            TotalPaymentAmount = 0;
        }
        int ClickedFlag = 0;
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        WaitIndicator Indicatior = null;
        public event RoutedEventHandler OnCancelButton_Click;
        public event RoutedEventHandler OnSaveButton_Click;
        ObservableCollection<clsPayment> PaymentList { get; set; }
        public clsExpensesVO Amount { get; set; }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            bool IsValid = true;

            if (PaymentList != null && PaymentList.Count > 0)
            {
                if (ClickedFlag == 1)
                {
                    CalculateTotalPaidAmount();

                    if (TotalPaymentAmount < TotalAmount)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Total Payment must equals to Total amount : " + TotalAmount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();
                    }
                 
                    Amount = new clsExpensesVO();
                    Amount.Amount = TotalPaymentAmount;
                    for (int i = 0; i < PaymentList.Count; i++)
                    {
                        clsExpensesDetailsVO objPay = new clsExpensesDetailsVO();
                        objPay.PaymentMode = PaymentList[i].PayModeID;
                        objPay.PaymentAmount = PaymentList[i].Amount;
                        objPay.ChequeNo = PaymentList[i].Number;
                        objPay.ChequeDate = PaymentList[i].Date;
                        if (PaymentList[i].BankID != null)
                            objPay.TransferToBank = PaymentList[i].BankID.Value;
                        Amount.ExpenseDetails.Add(objPay);
                    }
                    if (OnSaveButton_Click != null)
                        OnSaveButton_Click(this, new RoutedEventArgs());
                  
                };
             
            }
            ClickedFlag = 0;
            this.DialogResult = true;
        }
        void CalculateTotalPaidAmount()
        {
            double mTotalAmt = 0;
            foreach (var item in PaymentList)
            {
                mTotalAmt += item.Amount;
            }
            TotalPaymentAmount = mTotalAmt;
            TxtpaidAmt.Text = (TotalAmount - TotalPaymentAmount).ToString();
        }

        void dgPayment_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            dgPayment.CancelEdit(DataGridEditingUnit.Cell);
        }
        public class clsPayment
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

        void PayExpense_Loaded(object sender, RoutedEventArgs e)
        {
            TotalAmount = Math.Ceiling(TotalAmount);
            txtPayableAmt.Text = TotalAmount.ToString();
            TxtpaidAmt.IsEnabled = false;
            txtNumber.IsEnabled = false;
            cmbBank.IsEnabled = false;
            dtpDate.IsEnabled = false;
            cmdAddPayment.IsEnabled = false;
            TxtpaidAmt.Text = TotalAmount.ToString();
            cmbCheckAuthBy.IsEnabled = false;
            cmbCreditAuthBy.IsEnabled = false;
            FillUserList();
            FillBank();
            FillPayMode();
            if (PaymentList == null)
            {
                PaymentList = new ObservableCollection<clsPayment>();
                dgPayment.ItemsSource = PaymentList;
            }
        }
        private void FillBank()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_BankMaster;
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
                    cmbBank.ItemsSource = null;
                    cmbBank.ItemsSource = objList;
                    cmbBank.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        void FillPayMode()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(MaterPayModeList), mlPaymode);
            cmbPayMode.ItemsSource = mlPaymode;
            cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
        }

        private void FillUserList()
        {
            clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
            BizAction.IsDecode = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetUserMasterListBizActionVO)e.Result).MasterList);
                    cmbCreditAuthBy.ItemsSource = null;
                    cmbCreditAuthBy.ItemsSource = objList;
                    cmbCreditAuthBy.SelectedItem = objList[0];
                    cmbCheckAuthBy.ItemsSource = null;
                    cmbCheckAuthBy.ItemsSource = objList;
                    cmbCheckAuthBy.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, new clsUserVO());
            Client.CloseAsync();
        }

        void dgPayment_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (dgPayment.SelectedItem != null)
            {
                CalculateTotalPaidAmount();
                if (TotalPaymentAmount > TotalAmount)
                {
                    ((clsPayment)dgPayment.SelectedItem).Amount = 0;
                    CalculateTotalPaidAmount();
                    string msgTitle = "PALASH";
                    string msgText = "Total Amount is exceeding the Total Payment amount : " + TotalAmount;

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
        }
        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }

        private void cmdAddPayment_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
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
            #region

            #endregion
            if (isValid)
            {
                TxtpaidAmt.Text = "0";
                txtNumber.Text = "";
                cmbBank.SelectedValue = (long)0;
                dtpDate.SelectedDate = null;
                PaymentList.Add(mPay);
            }
        }

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
                            clsPayment item = (clsPayment)dgPayment.SelectedItem;
                            PaymentList.Remove((clsPayment)dgPayment.SelectedItem);
                            CalculateTotalPaidAmount();
                        }
                    };
                    msgWD.Show();
                }
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
        private void txtReceivedAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReceivedAmt.Text) && txtReceivedAmt.Text.IsNumberValid())
            {
                double receivedAmt = Convert.ToDouble(txtReceivedAmt.Text);
                txtAmtToBeReturn.Text = (receivedAmt - TotalPaymentAmount).ToString();
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

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

                    cmbCheckAuthBy.IsEnabled = false;
                    cmbCreditAuthBy.IsEnabled = false;


                    switch ((MaterPayModeList)ID)
                    {
                        case MaterPayModeList.Credit:
                            cmbCreditAuthBy.IsEnabled = true;
                            break;
                        case MaterPayModeList.Cash:
                            break;
                        case MaterPayModeList.Cheque:
                            txtNumber.MaxLength = 6;
                            cmbCheckAuthBy.IsEnabled = true;
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
    }
}

