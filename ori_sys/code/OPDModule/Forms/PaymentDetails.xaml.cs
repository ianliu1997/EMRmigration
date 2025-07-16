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
using System.Reflection;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using OPDModule;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Collections.ObjectModel;
using OPDModule.Forms;

namespace CIMS.Forms
{
    public partial class PaymentDetails : ChildWindow
    {

        public event RoutedEventHandler OnPaymentSaveButton_Click;
        public event RoutedEventHandler OnPaymentCancelButton_Click;
        public clsPaymentVO Payment { get; set; }
       

        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        PaymentForType PaymentFor = PaymentForType.None;
        int ClickedFlag = 0;
   

        enum PaymentForType
        {
            None = 0,
            Advance = 1,
            Refund = 2,
            Bill = 3
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

        public enum PayMode
        {
            Cash = 1,
            Cheque = 2,
            DD = 3,
            CreditCard = 4,
            DebitCard = 5,
            StaffFree = 6,
            UsedCompanyAdvance = 7,
            UsedPatientAdvance = 8,
            Concession = 9
        }

        public enum PaymentTransactionType
        {
            SelfPayment = 1,
            CompanyPayment = 2,
            AdvancePayment = 3,
            RefundPayment = 4
        }

        void FillPayMode()
        {
            List<MasterListItem> mlPaymode = new List<MasterListItem>();
            MasterListItem Default = new MasterListItem(0, "- Select -");
            mlPaymode.Insert(0, Default);
            EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.AdvancePayment);
            cmbPayMode.ItemsSource = mlPaymode;
            cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
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
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);
                TheMasterList.Add(Item);
            }
        }



        public PaymentDetails()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PaymentDetails_Loaded);
            TotalPaymentAmount = 0;
        }

        void PaymentDetails_Loaded(object sender, RoutedEventArgs e)
        {
            FillBank();
            FillPayMode();

           

        }
             
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                string msgTitle = "";
                string msgText = "";
                bool IsValid = true;



                if (TotalPaymentAmount < TotalAmount)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Total Payment must equals to Total amount : " + TotalAmount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    IsValid = false;
                    msgW1.Show();
                    ClickedFlag = 0;

                }

                if (IsValid && CheckValidation())
                {
                    Payment = new clsPaymentVO();
                    Payment.CreditAuthorizedBy = 0;
                    Payment.ChequeAuthorizedBy = 0;


                    clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();
                    objPay.PaymentModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

                    objPay.PaidAmount = Convert.ToDouble(txtPaidAmount.Text);
                    objPay.Number = txtNumber.Text;
                    objPay.Date = dtpDate.SelectedDate;
                    if (cmbBank.SelectedItem != null)
                        objPay.BankID = ((MasterListItem)cmbBank.SelectedItem).ID;


                    Payment.PaymentDetails.Add(objPay);

                    this.DialogResult = true;
                    if (OnPaymentSaveButton_Click != null)
                        OnPaymentSaveButton_Click(this, new RoutedEventArgs());

                }
                else
                    ClickedFlag = 0;
            }
        }


        private bool CheckValidation()
        {
            bool result = true;
            bool isValid = true;



            if (txtPaidAmount.Text == "")
            {
                txtPaidAmount.SetValidation("Please Enter Paid Amount");
                txtPaidAmount.RaiseValidationError();
                txtPaidAmount.Focus();
                result = false;
               
            }
                //Added By Yogita
            else if (Convert.ToInt32(txtPaidAmount.Text) < 0)
            {
                txtPaidAmount.SetValidation("Paid Amount Cannot Be Negative");
                txtPaidAmount.RaiseValidationError();
                txtPaidAmount.Focus();
                result = false;
            
            }
            else
                txtPaidAmount.ClearValidationError();
            // End

            long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

            if (!((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
            {
                if (((MasterListItem)cmbBank.SelectedItem).ID == 0)
                {
                    cmbBank.TextBox.SetValidation("Please select the bank");
                    cmbBank.TextBox.RaiseValidationError();
                    cmbBank.Focus();
                    result = false;
                    isValid = false;
                   
                }
                else
                    cmbBank.TextBox.ClearValidationError();
            }
            if (isValid)
            {
                if (((MasterListItem)cmbBank.SelectedItem).ID > 0)
                {

                    if (dtpDate.SelectedDate == null)
                    {
                        dtpDate.SetValidation("Date Required");
                        dtpDate.RaiseValidationError();
                        dtpDate.Focus();
                        result = false;
                        isValid = false;
                        
                    }
                    else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                    {
                        dtpDate.SetValidation("Date must be greater than Todays Date.");
                        dtpDate.RaiseValidationError();
                        dtpDate.Focus();
                        result = false;
                        isValid = false;
                        
                    }
                    else
                        dtpDate.ClearValidationError();

                    if (txtNumber.IsEnabled == true)
                    {
                        if (txtNumber.Text == "")
                        {
                            txtNumber.SetValidation("Number Required");
                            txtNumber.RaiseValidationError();
                            txtNumber.Focus();
                            result = false;
                            isValid = false;
                            
                        }
                        else if (txtNumber.Text.Length < txtNumber.MaxLength)
                        {

                            txtNumber.SetValidation("Number not valid");
                            txtNumber.RaiseValidationError();
                            txtNumber.Focus();
                            result = false;
                            isValid = false;
                           
                        }

                        else
                        {
                            txtNumber.ClearValidationError();
                        }
                    }



                }
            }

            return result;

        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnPaymentCancelButton_Click != null)
                OnPaymentCancelButton_Click(this, new RoutedEventArgs());

        }

        private void cmbPayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPayMode.SelectedItem != null)
            {
                long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

                if (ID > 0)
                {

                    txtPaidAmount.IsEnabled = true;
                    if (((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
                    {
                        txtNumber.IsEnabled = false;
                        txtNumber.Text = string.Empty;

                        cmbBank.IsEnabled = false;
                        cmbBank.SelectedValue = (long)0;

                        dtpDate.IsEnabled = false;
                        dtpDate.SelectedDate = null;

                    }
                    else
                    {
                        txtNumber.IsEnabled = true;
                        cmbBank.IsEnabled = true;
                        dtpDate.IsEnabled = true;
                        dtpDate.SelectedDate = null;
                    }
                    txtNumber.MaxLength = 20;


                    switch ((MaterPayModeList)ID)
                    {
                        case MaterPayModeList.Credit:

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

                    txtPaidAmount.Focus();
                }
                else
                {
                    txtPaidAmount.IsEnabled = false;
                    txtNumber.IsEnabled = false;
                    cmbBank.IsEnabled = false;
                    dtpDate.IsEnabled = false;

                }
            }
        }

        

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateBillAmounttobeRecieveandRefund CalcWin = new CalculateBillAmounttobeRecieveandRefund();
            CalcWin.txtBillAmt.Text = txtPaidAmount.Text;
            CalcWin.Show();
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
            //if (txtNumber.IsEnabled)
            //{
            //    if (txtNumber.Text.Length < txtNumber.MaxLength)
            //    {
            //        txtNumber.SetValidation("Number not valid");
            //        txtNumber.RaiseValidationError();
            //    }
            //    else
            //        txtNumber.ClearValidationError();

            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;

            //}
        }
    }
}

