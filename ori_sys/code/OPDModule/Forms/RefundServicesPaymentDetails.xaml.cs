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
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using System.Reflection;
using CIMS;
using CIMS.Forms;
using PalashDynamics.Service.PalashTestServiceReference;


namespace OPDModule.Forms
{
    public partial class RefundServicesPaymentDetails : ChildWindow
    {
        #region Variable declaration
        int ClickedFlag = 0;
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }

        public BillPaymentTypes AgainstBill { get; set; }
        public double TotalRefundAmount = 0;
        WaitIndicator Indicatior = null;
        enum PaymentForType
        {
            None = 0,
            Advance = 1,
            Refund = 2,
            Bill = 3
        }
        PaymentForType PaymentFor = PaymentForType.None;
        ObservableCollection<clsPayment> PaymentList { get; set; }

        public clsPaymentVO Payment { get; set; }

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        public bool IsRefundToAdvance { get; set; }     // Refund to Advance 20042017

        #endregion

        public RefundServicesPaymentDetails()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PaymentDetails_Loaded);
            TotalPaymentAmount = 0;
        }

        #region FillCombobox
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

        public enum PaymentTransactionType
        {
            None = 0,
            SelfPayment = 1,
            CompanyPayment = 2,
            AdvancePayment = 3,
            RefundPayment = 4

        }

        void FillPayMode()
        {
            //List<MasterListItem> mlPaymode = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //mlPaymode.Insert(0, Default);
            //EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);
            //cmbPayMode.ItemsSource = mlPaymode;
            //// cmbPayMode.SelectedItem = Default;
            //cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;

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
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundAmount == 0 || ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundPayModeID == 0)
                    {
                        cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
                        cmbPayMode.IsEnabled = true;
                    }
                    else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundAmount <= Convert.ToDouble(txtPaidAmount.Text))
                    {
                        cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundPayModeID;
                        //cmbPayMode.IsEnabled = false;       // Refund to Advance 20042017
                        cmbPayMode.IsEnabled = true;          // Refund to Advance 20042017
                    }
                    else
                    {
                        cmbPayMode.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PaymentModeID;
                        cmbPayMode.IsEnabled = true;
                    }
                }
            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
        }

        #endregion

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

        void PaymentDetails_Loaded(object sender, RoutedEventArgs e)
        {
            FillBank();
            FillPayMode();
        }

        #region Save Data
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                if (CheckValidation())
                {

                    string msgTitle = "";
                    string msgText = "Are you sure you want to save the Refund Details?";
                    bool IsValid = true;
                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {

                            if (res == MessageBoxResult.Yes)
                            {

                                this.DialogResult = true;
                                // DateTime payDate = DateTime.Now;
                                Payment = new clsPaymentVO();
                                Payment.CreditAuthorizedBy = 0;
                                Payment.ChequeAuthorizedBy = 0;
                                Payment.RefundID = (long)PaymentForType.Refund;
                                Payment.RefundAmount = Convert.ToDouble(txtPaidAmount.Text);

                                clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();
                                objPay.PaymentModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                                objPay.PaidAmount = Convert.ToDouble(txtPaidAmount.Text);
                                objPay.Number = txtNumber.Text;
                                objPay.Date = dtpDate.SelectedDate;
                                if (cmbBank.SelectedItem != null)
                                    objPay.BankID = ((MasterListItem)cmbBank.SelectedItem).ID;
                                Payment.PaymentDetails.Add(objPay);

                                if (OnSaveButton_Click != null)
                                    OnSaveButton_Click(this, new RoutedEventArgs());
                            }
                            else
                                ClickedFlag = 0;



                        };
                    msgW.Show();
                }
                ClickedFlag = 0;
            }
        }

        #endregion

        #region Validation
        private bool CheckValidation()
        {
            bool result = true;
            bool isValid = true;

            if (txtPaidAmount.Text == Convert.ToString(0))
            {
                string msgTitle = "";
                string msgText = "You can not save the Payment with Zero amount";

                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgWD.Show();
                txtPaidAmount.Focus();
                ClickedFlag = 0;
                result = false;
                return result;


                //txtPaidAmount.SetValidation("You can not save the Charges with Zero amount");
                //txtPaidAmount.RaiseValidationError();
                //txtPaidAmount.Focus();
                //result = false;
                //ClickedFlag = 0;
            }
            //else
            //    txtPaidAmount.ClearValidationError();

            if (txtPaidAmount.Text == "")
            {
                txtPaidAmount.SetValidation("Please Enter Paid Amount");
                txtPaidAmount.RaiseValidationError();
                txtPaidAmount.Focus();
                result = false;
                ClickedFlag = 0;
            }
            else
                txtPaidAmount.ClearValidationError();

            long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

            //if (!((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
            if (!((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree || (MaterPayModeList)ID == MaterPayModeList.PatientAdvance))  // Refund to Advance 20042017
            {
                if (((MasterListItem)cmbBank.SelectedItem).ID == 0)
                {
                    cmbBank.TextBox.SetValidation("Please select the bank");
                    cmbBank.TextBox.RaiseValidationError();
                    cmbBank.Focus();
                    result = false;
                    isValid = false;
                    ClickedFlag = 0;
                }
                else
                    cmbBank.TextBox.ClearValidationError();
            }
            if (isValid)
            {
                if (AgainstBill == BillPaymentTypes.AgainstBill)
                {
                    if (Convert.ToDouble(txtPaidAmount.Text) > TotalRefundAmount)
                    {
                        txtPaidAmount.SetValidation("Paid amount can not be greater than Bill Amount");
                        txtPaidAmount.RaiseValidationError();
                        txtPaidAmount.Focus();
                        result = false;
                        isValid = false;
                        ClickedFlag = 0;
                    }
                    else
                        txtPaidAmount.ClearValidationError();

                }

                if (((MasterListItem)cmbBank.SelectedItem).ID > 0)
                {

                    if (dtpDate.SelectedDate == null)
                    {
                        dtpDate.SetValidation("Date Required");
                        dtpDate.RaiseValidationError();
                        dtpDate.Focus();
                        result = false;
                        isValid = false;
                        ClickedFlag = 0;
                    }
                    else if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date)
                    {
                        dtpDate.SetValidation("Date must be greater than Todays Date.");
                        dtpDate.RaiseValidationError();
                        dtpDate.Focus();
                        result = false;
                        isValid = false;
                        ClickedFlag = 0;
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
                            ClickedFlag = 0;
                        }
                        else if (txtNumber.Text.Length < txtNumber.MaxLength)
                        {

                            txtNumber.SetValidation("Number not valid");
                            txtNumber.RaiseValidationError();
                            txtNumber.Focus();
                            result = false;
                            isValid = false;
                            ClickedFlag = 0;
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

        #endregion

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void cmbPayMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPayMode.SelectedItem != null)
            {
                long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;

                if (ID > 0)
                {

                    txtPaidAmount.IsEnabled = true;

                    //if (((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
                    if (((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree || (MaterPayModeList)ID == MaterPayModeList.PatientAdvance))       // Refund to Advance 20042017
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
                            txtNumber.MaxLength = 16;
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
                            txtNumber.MaxLength = 4;
                            break;
                        case MaterPayModeList.DebitCard:
                            txtNumber.MaxLength = 4;
                            break;
                        case MaterPayModeList.StaffFree:
                            break;
                        case MaterPayModeList.CompanyAdvance:

                            break;
                        case MaterPayModeList.PatientAdvance:
                            break;
                        case MaterPayModeList.NEFTRTGS:
                            txtNumber.MaxLength = 16;
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

                #region Refund to Advance 20042017
                if (ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundToAdvancePayModeID)    //if (ID == 8)
                {
                    IsRefundToAdvance = true;
                }
                else
                {
                    IsRefundToAdvance = false;
                }
                #endregion

            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        #region Validation
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
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
            if ((MaterPayModeList)ID == MaterPayModeList.NEFTRTGS)
            {
                if (((TextBox)sender).Text.Trim() != "" && !string.IsNullOrEmpty(((TextBox)sender).Text) && !((TextBox)sender).Text.IsItSpecialChar())
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
            else
            {
                if (((TextBox)sender).Text.Trim() != "" && !((TextBox)sender).Text.IsNumberValid())
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

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtRecivedAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            //double Amount = 0;
            //Amount = Convert.ToDouble(txtPaidAmount.Text);
            //double ReceivedAmount = 0;
            //ReceivedAmount = Convert.ToDouble(txtRecivedAmount.Text);
            //if (ReceivedAmount > Amount)
            //{
            //    txtAmounttobereturned.Text = (ReceivedAmount - Amount).ToString();
            //}

        }

        #endregion

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateBillAmounttobeRecieveandRefund CalcWin = new CalculateBillAmounttobeRecieveandRefund();
            CalcWin.txtBillAmt.Text = txtPaidAmount.Text;
            CalcWin.Show();
        }


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

}

