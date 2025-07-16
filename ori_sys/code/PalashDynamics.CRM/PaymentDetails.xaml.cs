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
using PalashDynamics.ValueObjects.Billing;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using CIMS;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;


namespace PalashDynamics.CRM
{
    public partial class PaymentDetails : ChildWindow
    {
        int ClickedFlag = 0;
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        PaymentForType PaymentFor = PaymentForType.None;

        public double LPatientCharge { get; set; }

        public event RoutedEventHandler OnPaymentSaveButton_Click;
        public event RoutedEventHandler OnPaymentCancelButton_Click;

        public string ModuleName { get; set; }
        public string Action { get; set; }

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
            ChargeList = new ObservableCollection<clsChargeVO>();
            cmbPayMode.IsEnabled = false;

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {

                bool SavePayment = true;
                SavePayment = CheckValidation();

                if (SavePayment == true)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to save the Charges?";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();

                }
            }
            
        }
       
        private bool CheckValidation()
        {
            bool result = true;
            bool isValid = true;

            if (txtPaidAmount.Text == Convert.ToString(0))
            {
                string msgTitle = "";
                string msgText = "You can not save the Charges with Zero amount";

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
           
            if (!((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
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

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (ClickedFlag == 1)
            {
                if (result == MessageBoxResult.Yes)
                {
                    LPatientCharge = Convert.ToDouble(txtPaidAmount.Text);
                    this.DialogResult = true;
                    if (OnPaymentSaveButton_Click != null)
                        OnPaymentSaveButton_Click(this, new RoutedEventArgs());

                    ClickedFlag = 0;
                }

                else
                    ClickedFlag = 0;
            }

               
        }

        
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            OnPaymentCancelButton_Click(this, new RoutedEventArgs());
            this.Close();

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
        
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        
        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

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



        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
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

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateBillAmount obj = new CalculateBillAmount();
            obj.txtBillAmt.Text = txtPaidAmount.Text;
            obj.Show();
            
             //ModuleName = "OPDModule";
            //Action = "OPDModule.Forms.CalculateBillAmounttobeRecieveandRefund";
            ////UserControl rootPage = Application.Current.RootVisual as UserControl;
            //ChildWindow rootPage = Application.Current.RootVisual as ChildWindow;

            ////TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            ////mElement.Text = "Inventory Configuration";

            //WebClient c = new WebClient();
            //c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            //c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        //void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        //{
        //    try
        //    {

        //        UIElement myData = null;
        //        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

        //        XElement deploy = XDocument.Parse(appManifest).Root;
        //        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
        //                                where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
        //                                select assemblyParts).ToList();
        //        Assembly asm = null;
        //        AssemblyPart asmPart = new AssemblyPart();
        //        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
        //        asm = asmPart.Load(streamInfo.Stream);

        //        myData = asm.CreateInstance(Action) as UIElement;

        //        //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
        //        ChildWindow Win2 = new ChildWindow();
        //        Win2.Content = myData;
        //        Win2.Show();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }



        //}

        
    }
}



