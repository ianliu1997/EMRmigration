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
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using CIMS;
using OPDModule.Forms;
using C1.Silverlight;


namespace PalashDynamics.IPD
{
    public partial class IPDPaymentWindow : ChildWindow
    {
        int ClickedFlag = 0;
        public bool IsSettleMentBill { get; set; }
        double TotalConAmount { get; set; }
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        WaitIndicator Indicatior = null;
        public bool FromPatient = false;

        double PatTotalAmt = 0;
        double PatBalanceAmt = 0;
        double CompanyTotalAmt = 0;
        double CompanyBalanceAmt = 0;
        List<clsAdvanceVO> objPatientAdvanceList = new List<clsAdvanceVO>();
        List<clsAdvanceVO> objCompanyAdvanceList = new List<clsAdvanceVO>();

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
        // List<clsPayment> PaymentList = new List<clsPayment>();

        public BillPaymentTypes BillPaymentType = BillPaymentTypes.AgainstBill;

        ObservableCollection<clsPayment> PaymentList { get; set; }

        public clsPaymentVO Payment { get; set; }

        public bool IsSettleBill = false;
        public bool IsRefundAdvance = false;

        #region IInitiateCIMS Members

        /// <summary>
        /// Function is for initializing the form based on Mode Passes as Paramater.
        /// </summary>
        /// <param name="Mode"></param>
        public void Initiate(string Mode)
        {
            try
            {

                SampleHeader.Text += " : " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.LastName;
                //" " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient + " " +


                SampleHeader.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;

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
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
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

        /// <summary>
        /// Function is for filling Payment Mode List 
        /// </summary>
        void FillPayMode()
        {
            //List<MasterListItem> mlPaymode = new List<MasterListItem>();
            //MasterListItem Default = new MasterListItem(0, "- Select -");
            //mlPaymode.Insert(0, Default);
            //EnumToList(typeof(MaterPayModeList), mlPaymode, PaymentTransactionType.None);

            //var results = from r in mlPaymode
            //              where r.ID != 3 && r.ID != 6 && r.ID != 8 && r.ID != 9 && r.ID != 7
            //              select r;
            //cmbPayMode.ItemsSource = results.ToList();
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
                    cmbPayMode.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //App.SessionUser
            Client.CloseAsync();
        }

        public static void EnumToList(Type EnumType, List<MasterListItem> TheMasterList, PaymentTransactionType sTransactionType)
        {
            Array Values = GetValues(EnumType);
            foreach (int Value in Values)
            {
                //if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.AdvancePayment || (PaymentTransactionType)sTransactionType == PaymentTransactionType.RefundPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance ||
                //         (MaterPayModeList)Value == MaterPayModeList.StaffFree)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.SelfPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.CompanyAdvance || (MaterPayModeList)Value == MaterPayModeList.PatientAdvance)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.CompanyPayment)
                //{
                //    if ((MaterPayModeList)Value == MaterPayModeList.PatientAdvance || (MaterPayModeList)Value == MaterPayModeList.CompanyAdvance)
                //        break;
                //}
                //else if ((PaymentTransactionType)sTransactionType == PaymentTransactionType.None)
                //{
                //    //Do Nothing
                //}
                string Display = Enum.GetName(EnumType, Value);
                MasterListItem Item = new MasterListItem(Value, Display);

                TheMasterList.Add(Item);
            }
        }
        public IPDPaymentWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PaymentWindow_Loaded);
            this.dgPayment.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgPayment_CellEditEnded);
            this.dgPayment.BindingValidationError += new EventHandler<ValidationErrorEventArgs>(dgPayment_BindingValidationError);
            TotalPaymentAmount = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        void dgPayment_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            // throw new NotImplementedException();

            //((clsPayment)dgPayment.SelectedItem).Amount = 0;
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
                    ((clsPayment)dgPayment.SelectedItem).Amount = 0;
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
            if (IsSettleMentBill == false)
            {
                double mTotalAmt = 0;
                foreach (var item in PaymentList)
                {
                    mTotalAmt += item.Amount;
                }

                TotalPaymentAmount = mTotalAmt;

                TxtpaidAmt.Text = (TotalAmount - TotalPaymentAmount).ToString();
            }
            else
            {
                double mTotalAmt = 0;
                double mTotalConAmt = 0;
                foreach (var item in PaymentList)
                {
                    mTotalAmt += item.Amount;
                    mTotalConAmt += item.ConAmount;
                }

                TotalPaymentAmount = mTotalAmt;
                TotalConAmount = mTotalConAmt;
                TxtpaidAmt.Text = (TotalAmount - (TotalPaymentAmount + mTotalConAmt)).ToString();
            }
        }

        void PaymentWindow_Loaded(object sender, RoutedEventArgs e)
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
            cmbCompanyAdvAuthBy.IsEnabled = false;
            // cmbConcessionAuthBy.IsEnabled = false;

            if (txtDiscAmt.Text != "")
            {
                if (txtDiscAmt.Text != null && Convert.ToDouble(txtDiscAmt.Text) > 0.0)
                {
                    cmbConcessionAuthBy.IsEnabled = true;

                }
            }
            FillUserList();
            FillBank();
            FillPayMode();
            FillConcessionAutBy();

            if (PaymentFor == PaymentForType.Advance || PaymentFor == PaymentForType.Refund)
            {
                tabControl1.Visibility = System.Windows.Visibility.Collapsed;
                PanelBillDetails.Visibility = System.Windows.Visibility.Collapsed;
                //borderPaySummary.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (PaymentFor == PaymentForType.Bill)
            {
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID) //((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID
                {
                    //////////////////////

                    //tabCompanyAdvance.IsEnabled = false;
                    //dgCompanyAdvDtl.IsEnabled = false;
                    dgCompanyAdvDtl.IsEnabled = true;
                }
                if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId > 0)
                {
                    FillPatientAdvance();
                }
                //FillPatientAdvance();
            }
            if (PaymentList == null)
            {
                PaymentList = new ObservableCollection<clsPayment>();
                dgPayment.ItemsSource = PaymentList;
            }
        }

        /// <summary>
        /// Function is for fetching advance details for the Patient.
        /// </summary>
        private void FillPatientAdvance()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetPatientAdvanceListBizActionVO BizAction = new clsGetPatientAdvanceListBizActionVO();

                BizAction.ID = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                BizAction.CompanyID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.CompanyID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;

                //BizAction.
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {
                        List<clsAdvanceVO> objList = new List<clsAdvanceVO>();
                        objList = ((clsGetPatientAdvanceListBizActionVO)arg.Result).Details;

                        if (objList != null)
                        {
                            //double PatTotalAmt = 0;
                            //double PatBalanceAmt = 0;
                            //double CompanyTotalAmt = 0;
                            //double CompanyBalanceAmt = 0;

                            //List<clsAdvanceVO> objPatientAdvanceList = new List<clsAdvanceVO>();
                            //List<clsAdvanceVO> objCompanyAdvanceList = new List<clsAdvanceVO>();

                            foreach (var item in objList)
                            {
                                item.Used = 0;
                                if (item.CompanyID > 0)
                                {
                                    objCompanyAdvanceList.Add(item);
                                    CompanyTotalAmt += item.Total;
                                    CompanyBalanceAmt += item.Balance;
                                }
                                else
                                {
                                    objPatientAdvanceList.Add(item);
                                    PatTotalAmt += item.Total;
                                    PatBalanceAmt += item.Balance;
                                    // ------added by Ashish
                                    //if (item.Total > item.Balance)
                                    //    item.Total = item.Balance;
                                    //-----
                                }
                            }

                            txtPatTotalAmt.Text = PatTotalAmt.ToString();
                            txtPatBalanceAmt.Text = PatBalanceAmt.ToString();
                            txtPatConsumedAmt.Text = (PatTotalAmt - PatBalanceAmt).ToString();

                            txtCompanyTotAmt.Text = CompanyTotalAmt.ToString();
                            txtCompanyBalAmt.Text = CompanyBalanceAmt.ToString();
                            txtCompanyConsAmt.Text = (CompanyTotalAmt - CompanyBalanceAmt).ToString();

                            dgCompanyAdvDtl.ItemsSource = null;
                            dgCompanyAdvDtl.ItemsSource = objCompanyAdvanceList;

                            dgPatientAdvDtl.ItemsSource = null;
                            dgPatientAdvDtl.ItemsSource = objPatientAdvanceList;
                            ApplyDiscount();
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

        private void ApplyDiscount()
        {
            double BalAmt = 0;
            BalAmt = TotalAmount;

            if (PatBalanceAmt < TotalAmount) //PatTotalAmt
                lblInsufficient.Text = "**Insufficient Advance";
            else
                lblInsufficient.Text = string.Empty;

            foreach (var item in objPatientAdvanceList)
            {
                clsPayment mPay = new clsPayment();
                mPay.PayModeID = 8; // for PatientAdvance Convert.ToInt64(MaterPayModeList.PatientAdvance);
                mPay.PayMode = "PatientAdvance";// MaterPayModeList.PatientAdvance.ToString();
                if (item.Balance > 0)
                {
                    if (BalAmt > item.Balance) //item.Total
                    {
                        mPay.Amount = item.Balance; //item.Total;
                        item.Used = item.Balance;
                        item.Balance = 0;
                        BalAmt = BalAmt - item.Total;
                        mPay.AdvanceID = item.ID;
                        PaymentList.Add(mPay);
                    }
                    else
                    {
                        mPay.Amount = BalAmt;
                        item.Used = BalAmt;
                        item.Balance = item.Total - item.Used;
                        mPay.AdvanceID = item.ID;
                        PaymentList.Add(mPay);
                    }
                }
            }
            CalculateTotalPaidAmount();

        }

        /// <summary>
        /// Method checks for the validations and creates object of type clsPaymentVO as assigns values to it
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
                    clsPayment Pay = new clsPayment();
                    Pay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                    //Client Requirment
                    //if ((MaterPayModeList)Pay.PayModeID == MaterPayModeList.Credit)
                    //{

                    //    if (cmbCreditAuthBy.SelectedItem == null)
                    //    {
                    //        cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                    //        cmbCreditAuthBy.TextBox.RaiseValidationError();
                    //        cmbCreditAuthBy.Focus();
                    //        IsValid = false;
                    //    }
                    //    else if (((MasterListItem)cmbCreditAuthBy.SelectedItem).ID == 0)
                    //    {
                    //        cmbCreditAuthBy.TextBox.SetValidation("Please select Credit Authorized By");
                    //        cmbCreditAuthBy.TextBox.RaiseValidationError();
                    //        cmbCreditAuthBy.Focus();
                    //        IsValid = false;
                    //    }
                    //    else
                    //        cmbCreditAuthBy.TextBox.ClearValidationError();
                    //}
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
                //if (TotalPaymentAmount < TotalAmount && BillPaymentType == BillPaymentTypes.AgainstServices)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Total Payment must equals to Total amount : " + TotalAmount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    IsValid = false;
                //    msgW1.Show();

                //}

                if (txtDiscAmt.Text != null && txtDiscAmt.Text != "" && Convert.ToDouble(txtDiscAmt.Text) > 0.0)
                {
                    if (cmbConcessionAuthBy.SelectedItem != null)
                    {
                        if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID > 0)
                        {
                            if (txtNarration.Text == null)
                            {
                                txtNarration.SetValidation("Please enter narration");
                                txtNarration.RaiseValidationError();
                                txtNarration.Focus();
                                IsValid = false;

                            }
                            else if (txtNarration.Text == "")
                            {
                                txtNarration.SetValidation("Please enter narration");
                                txtNarration.RaiseValidationError();
                                txtNarration.Focus();
                                IsValid = false;

                            }
                            else
                                txtNarration.ClearValidationError();

                        }

                    }

                    if (cmbConcessionAuthBy.SelectedItem == null)
                    {
                        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                        cmbConcessionAuthBy.Focus();
                        IsValid = false;
                    }
                    else if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID == 0)
                    {
                        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                        cmbConcessionAuthBy.Focus();
                        IsValid = false;
                    }
                    else
                        cmbConcessionAuthBy.TextBox.ClearValidationError();

                }

                if (TxtConAmt.Text != null && TxtConAmt.Text != "" && Convert.ToDouble(TxtConAmt.Text) > 0.0)
                {


                    if (cmbConcessionAuthBy.SelectedItem == null)
                    {
                        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                        cmbConcessionAuthBy.Focus();
                        IsValid = false;
                    }
                    else if (((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID == 0)
                    {
                        cmbConcessionAuthBy.TextBox.SetValidation("Please select Concession Authorized By");
                        cmbConcessionAuthBy.TextBox.RaiseValidationError();
                        cmbConcessionAuthBy.Focus();
                        IsValid = false;
                    }
                    else
                        cmbConcessionAuthBy.TextBox.ClearValidationError();

                }

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
                        if (res == MessageBoxResult.Yes)
                        {
                            this.DialogResult = true;
                            // DateTime payDate = DateTime.Now;
                            Payment = new clsPaymentVO();

                            Payment.PayeeNarration = txtNarration.Text;
                            Payment.CreditAuthorizedBy = 0;
                            Payment.ChequeAuthorizedBy = 0;
                            Payment.PaidAmount = TotalPaymentAmount;

                            if (IsSettleMentBill == true)
                            {
                                Payment.SettlementConcessionAmount = TotalConAmount;
                            }

                            if (PaymentFor == PaymentForType.Bill)
                            {

                                Payment.BillPaymentType = this.BillPaymentType;
                                Payment.BillAmount = Convert.ToDouble(txtPayTotalAmount.Text);
                                Payment.BillBalanceAmount = Payment.BillAmount - Payment.PaidAmount;
                            }



                            if (cmbCreditAuthBy.SelectedItem != null)
                                Payment.CreditAuthorizedBy = ((MasterListItem)cmbCreditAuthBy.SelectedItem).ID;

                            if (cmbConcessionAuthBy.SelectedItem != null)
                                Payment.ChequeAuthorizedBy = ((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID;

                            if (cmbCompanyAdvAuthBy.SelectedItem != null)
                                Payment.ComAdvAuthorizedBy = ((MasterListItem)cmbCompanyAdvAuthBy.SelectedItem).ID;



                            // Payment.co
                            for (int i = 0; i < PaymentList.Count; i++)
                            {
                                clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();

                                objPay.PaymentModeID = PaymentList[i].PayModeID;
                                objPay.AdvanceID = PaymentList[i].AdvanceID;
                                objPay.PaidAmount = PaymentList[i].Amount;
                                objPay.Number = PaymentList[i].Number;
                                objPay.Date = PaymentList[i].Date;
                                if (PaymentList[i].BankID != null)
                                    objPay.BankID = PaymentList[i].BankID.Value;
                                Payment.AdvanceID = Convert.ToInt64(PaymentList[i].AdvanceID);
                                Payment.AdvanceUsed = AdvUsed;
                                Payment.AdvanceID = AdvID;
                                objPay.AdvanceID = AdvID;
                                Payment.PaymentDetails.Add(objPay);
                            }

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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            #region OLD CODE
            //if (FromPatient == false)
            //{
            //    this.DialogResult = false;
            //    if (OnSaveButton_Click != null)
            //        OnSaveButton_Click(this, new RoutedEventArgs());
            //}

            //if (FromPatient == true)
            //{
            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment details incomplete,Do you want to save patient details without service details ?", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.OnMessageBoxClosed += (re) =>
            //    {
            //        if (re == MessageBoxResult.Yes)
            //        {
            //            this.DialogResult = false;
            //            if (OnCancelButton_Click != null)
            //                OnCancelButton_Click(this, new RoutedEventArgs());

            //        }
            //    };
            //    msgW1.Show();
            //}

            #endregion

            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());

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
                    if (ID == 1)  //(((MaterPayModeList)ID == MaterPayModeList.Cash)) //Client Requirment || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
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
                        dtpDate.SelectedDate = DateTime.Now;
                    }

                    txtNumber.MaxLength = 20;

                    cmbCheckAuthBy.IsEnabled = false;
                    cmbCreditAuthBy.IsEnabled = false;
                    cmbCompanyAdvAuthBy.IsEnabled = false;

                    switch (ID)
                    {
                        case 1: //for Cash
                            break;

                        case 2: //for Cheque
                            txtNumber.MaxLength = 6;
                            cmbCheckAuthBy.IsEnabled = true;
                            break;

                        case 4://for CreditCard
                            txtNumber.MaxLength = 4;
                            break;

                        case 5: //for DebitCard
                            txtNumber.MaxLength = 4;
                            break;

                        case 10: //for NEFT/RTGS
                            txtNumber.MaxLength = 11;
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
                            //TotalPaymentAmount -= ((clsPayment)dgPayment.SelectedItem).Amount;
                            //CalculateTotalPaidAmount();
                            clsPayment item = (clsPayment)dgPayment.SelectedItem;
                            if (item.AdvanceID > 0)
                            {
                                bool advanceFound = false;
                                List<clsAdvanceVO> AdvanceList = new List<clsAdvanceVO>();


                                if (dgPatientAdvDtl.ItemsSource != null)
                                {
                                    AdvanceList = (List<clsAdvanceVO>)dgPatientAdvDtl.ItemsSource;

                                    foreach (var advance in AdvanceList)
                                    {
                                        if (item.AdvanceID == advance.ID)
                                        {
                                            advanceFound = true;
                                            // ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance 
                                            //advance.Balance += advance.Used;
                                            advance.Balance += item.Amount;
                                            advance.Used = 0;
                                            break;
                                        }
                                    }
                                    if (advanceFound)
                                    {
                                        dgPatientAdvDtl.ItemsSource = null;
                                        dgPatientAdvDtl.ItemsSource = AdvanceList;

                                    }
                                    else
                                    {
                                        AdvanceList = new List<clsAdvanceVO>();
                                        AdvanceList = (List<clsAdvanceVO>)dgCompanyAdvDtl.ItemsSource;

                                        foreach (var advance in AdvanceList)
                                        {
                                            if (item.AdvanceID == advance.ID)
                                            {
                                                advanceFound = true;
                                                //advance.Balance += advance.Used;
                                                advance.Balance += item.Amount;
                                                advance.Used = 0;
                                                break;
                                            }
                                        }
                                        if (advanceFound)
                                        {
                                            dgCompanyAdvDtl.ItemsSource = null;
                                            dgCompanyAdvDtl.ItemsSource = AdvanceList;

                                        }
                                    }
                                }


                            }
                            PaymentList.Remove((clsPayment)dgPayment.SelectedItem);
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

            clsPayment mPay = new clsPayment();
            mPay.PayModeID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
            mPay.PayMode = ((MasterListItem)cmbPayMode.SelectedItem).Description;


            if (!(((MasterListItem)cmbPayMode.SelectedItem).ID == 1)) // for Cash //Client Requirment || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.Credit || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.StaffFree))
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
                            if (IsSettleMentBill == false)
                            {
                                mPay.Amount = double.Parse(TxtpaidAmt.Text);
                                TotalPaymentAmount += amt;
                            }
                            else
                            {
                                if (TxtConAmt.Text != "")
                                {
                                    mPay.Amount = double.Parse(TxtpaidAmt.Text) - double.Parse(TxtConAmt.Text);
                                }
                                else
                                {
                                    mPay.Amount = double.Parse(TxtpaidAmt.Text);
                                }

                                if (TxtConAmt.Text != "")
                                {
                                    mPay.ConAmount = double.Parse(TxtConAmt.Text);
                                    TotalPaymentAmount += amt - mPay.ConAmount;
                                }
                                else
                                {
                                    TotalPaymentAmount += amt;

                                }
                            }
                        }
                    }
                    else
                    {
                        TxtpaidAmt.SetValidation("Amount should be greater than 0");
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

            if (isValid)
            {
                TxtpaidAmt.Text = "0";
                txtNumber.Text = "";
                cmbBank.SelectedValue = (long)0;
                dtpDate.SelectedDate = null;
                PaymentList.Add(mPay);
                TxtConAmt.Text = "";

                CalculateTotalPaidAmount();
            }
        }

        /// <summary>
        /// This method checks the validation for entered advance amount to be consumed 
        /// against selected advance in the grid and adds payment details into the Payment List.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private Int64 AdvID;
        private double AdvUsed;
        private void chkAddPatientAdvance_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                clsAdvanceVO obj;

                if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                    obj = (clsAdvanceVO)dgPatientAdvDtl.SelectedItem;
                else
                    obj = (clsAdvanceVO)dgCompanyAdvDtl.SelectedItem;
                AdvUsed = obj.Used;
                AdvID = obj.ID;
                if (obj != null)
                {
                    if (obj.Used > obj.Balance)
                    {
                        if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                            ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance;
                        else
                            ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Balance;

                        ((CheckBox)sender).IsChecked = false;
                        string msgTitle = "";
                        string msgText = "You can not consume amount greater than Balance amount.";

                        MessageBoxControl.MessageBoxChildWindow msgWD =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {

                        };

                        msgWD.Show();
                    }
                    else
                    {
                        clsPayment mPay = new clsPayment();

                        //Client Requirment
                        if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                        {
                            mPay.PayModeID = 8;  //for PatientAdvance  //(long)MaterPayModeList.PatientAdvance;
                            mPay.PayMode = "PatientAdvance"; //MaterPayModeList.PatientAdvance.ToString();
                        }
                        else
                        {
                            mPay.PayModeID = 7; //for CompanyAdvance  (long)MaterPayModeList.CompanyAdvance;
                            mPay.PayMode = "CompanyAdvance"; // MaterPayModeList.CompanyAdvance.ToString();
                        }

                        double amt = 0;

                        amt = obj.Used;
                        if (amt <= 0)
                        {
                            ((CheckBox)sender).IsChecked = false;
                            string msgTitle = "";
                            string msgText = "Amount should be greater than zero.";

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {

                            };

                            msgWD.Show();
                        }
                        else if ((amt + TotalPaymentAmount) > TotalAmount)
                        {
                            ((CheckBox)sender).IsChecked = false;
                            string msgTitle = "";
                            string msgText = "Total Amount is exceeding the Total Payment amount : " + TotalAmount;

                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {

                            };

                            msgWD.Show();
                        }
                        else
                        {
                            mPay.Amount = obj.Used;
                            mPay.AdvanceID = obj.ID;
                            //TotalPaymentAmount += amt;
                            PaymentList.Add(mPay);
                            CalculateTotalPaidAmount();
                            if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance") && dgPatientAdvDtl.SelectedItem != null)
                                ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance -= ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Used;
                            else if (dgCompanyAdvDtl.SelectedItem != null)
                                ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Balance -= ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Used;

                        }
                    }
                }
            }
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

        /// <summary>
        /// Method is for opening calculator window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CalculateBillAmounttobeRecieveandRefund CalcWin = new CalculateBillAmounttobeRecieveandRefund();
            //CalcWin.txtBillAmt.Text = TotalPaymentAmount.ToString();
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
        public double ConAmount { get; set; }

    }
}

