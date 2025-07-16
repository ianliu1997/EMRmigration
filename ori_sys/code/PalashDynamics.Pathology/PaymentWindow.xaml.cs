using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

namespace PalashDynamics.Pathology
{
    public partial class PaymentWindow : ChildWindow
    {
        public bool IsFromBillSearchWin = false;
        public bool IscompanyBill = false;
        public bool IsFromBillForm = false;
        public bool IsFromPharmacyBill;

        public long CompanyID = 0;
        public long PatientID = 0;
        public string PatientName = string.Empty;
        public string MRNO = string.Empty;
        public long CompanyIDForBill = 0;
        public long PatientUNitID = 0;
        public long UnitID = 0;


        public double TDSAmount { get; set; }
        int ClickedFlag = 0;
        bool IsPaymentModeEnabled = false;
        public bool IsSettleMentBill { get; set; }
        List<clsAdvanceVO> objPatientAdvanceList = new List<clsAdvanceVO>();
        List<clsAdvanceVO> objCompanyAdvanceList = new List<clsAdvanceVO>();

        // Added By CDS 
        List<clsAdvanceVO> objPatientAdvanceNewList = new List<clsAdvanceVO>();
        List<clsAdvanceVO> objCompanyAdvanceNewList = new List<clsAdvanceVO>();
        public double PackageConcenssionAmt = 0;
        // Added By CDS 
        public double TotalAmount { get; set; }
        double TotalPaymentAmount { get; set; }
        public double DiscountAmount { get; set; }
        WaitIndicator Indicatior = null;
        public bool ConcessionFromPlan = false;
        double PatTotalAmt = 0;
        double PatBalanceAmt = 0;
        double CompanyTotalAmt = 0;
        double CompanyBalanceAmt = 0;
        public long conID = 0;
        public long? PatientCategoryID { get; set; }
        public string CompanyName = string.Empty;
        bool IsNEFTRTGS = false;
        enum PaymentForType
        {
            None = 0,
            Advance = 1,
            Refund = 2,
            Bill = 3
        }
        public event RoutedEventHandler OnCancelButton_Click;
        PaymentForType PaymentFor = PaymentForType.None;
        public event RoutedEventHandler OnSaveButton_Click;
        // List<clsPayment> PaymentList = new List<clsPayment>();

        public BillPaymentTypes BillPaymentType = BillPaymentTypes.AgainstBill;

        ObservableCollection<clsPayment> PaymentList { get; set; }

        public clsPaymentVO Payment { get; set; }


        public bool IsSettleBill = false;
        public bool IsRefundAdvance = false;
        //public bool IsPatientAdvance = false;
        public long StoreID { get; set; }  //Added by umesh to solve error of cashcounter
        public bool IsBillAsDraft = false;  // Added  By CDS
        bool checkCredit = false;     // Added  By CDS

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            try
            {

                SampleHeader.Text += ": " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                       " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                                       ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                SampleHeader.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;


                switch (Mode.ToUpper())
                {
                    case "ADVANCE":
                        PaymentFor = PaymentForType.Advance;
                        grdConcession.Visibility = Visibility.Collapsed;
                        SampleHeader.Text += "  Advance Amt. Rs. " + TotalAmount.ToString();
                        break;

                    case "REFUND":
                        PaymentFor = PaymentForType.Refund;
                        grdConcession.Visibility = Visibility.Collapsed;
                        SampleHeader.Text += "  Refund Amt. Rs. " + TotalAmount.ToString();
                        break;

                    case "BILL":
                        PaymentFor = PaymentForType.Bill;
                        grdConcession.Visibility = Visibility.Visible;

                        if (!String.IsNullOrEmpty(MRNO) && ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != MRNO)
                        {
                            SampleHeader.Text = "";
                            SampleHeader.Text = " Payment Details ";
                            SampleHeader.Text += " : " + PatientName;
                            SampleHeader.Text += " - " + MRNO;
                        }
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
                    // Added  By CDS    
                    if (PaymentFor == PaymentForType.Advance)
                    {
                        var itemToRemove = objList.Single(r => r.ID == 7);
                        objList.Remove(itemToRemove);

                        var itemToRemove1 = objList.Single(r => r.ID == 8);
                        objList.Remove(itemToRemove1);

                        var itemToRemove2 = objList.Single(r => r.ID == 9);
                        objList.Remove(itemToRemove2);

                        cmbPayMode.ItemsSource = null;
                        cmbPayMode.ItemsSource = objList;
                        cmbPayMode.SelectedItem = objList[0];
                    }
                    if (IsPaymentModeEnabled)
                    {
                        cmbPayMode.SelectedValue = (long)2; //MaterPayModeList.Cheque;
                        cmbPayMode.IsEnabled = false;
                    }
                    else
                        cmbPayMode.IsEnabled = true;

                    if (IscompanyBill == true)
                    {
                        cmbPayMode.SelectedItem = objList[2];
                    }
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
                        if (conID != null && conID > 0)
                            cmbConcessionAuthBy.SelectedValue = conID;
                    }
                }


            };
            Client.ProcessAsync(BizAction, App.SessionUser);
            Client.CloseAsync();
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

        public PaymentWindow()
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
                    string msgText = "Total Amount Is Exceeding The Total Payment Amount : " + TotalAmount;
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWD.Show();
                }
            }
        }

        public double TotalConAmount { get; set; }

        void CalculateTotalPaidAmount()
        {
            double mTotalAmt = 0;
            foreach (var item in PaymentList)
            {
                mTotalAmt += item.Amount;
            }

            TotalPaymentAmount = mTotalAmt;
            if (TxtConAmt.Text != null && TxtConAmt.Text != "")
            {
                TxtpaidAmt.Text = ((TotalAmount - Convert.ToDouble(TxtConAmt.Text)) - TotalPaymentAmount).ToString();
                TotalConAmount = Convert.ToDouble(TxtConAmt.Text);
            }
            else
            {
                TxtpaidAmt.Text = ((TotalAmount) - TotalPaymentAmount).ToString();
            }
        }

        void PaymentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TotalAmount = Math.Ceiling(TotalAmount);  Not Get particular values So Use math.Round
            TotalAmount = Math.Round(TotalAmount, 0);
            txtPayableAmt.Text = TotalAmount.ToString();
            DiscountAmount = Math.Round(DiscountAmount, 0);
            txtDiscAmt.Text = DiscountAmount.ToString();
            TxtpaidAmt.IsEnabled = false;
            txtNumber.IsEnabled = false;
            cmbBank.IsEnabled = false;
            dtpDate.IsEnabled = false;
            cmdAddPayment.IsEnabled = false;
            TxtpaidAmt.Text = TotalAmount.ToString();
            cmbCheckAuthBy.IsEnabled = false;
            cmbCreditAuthBy.IsEnabled = false;
            cmbCompanyAdvAuthBy.IsEnabled = false;

            if (IscompanyBill == false)
            {
                if (txtDiscAmt.Text != "")
                {
                    if (txtDiscAmt.Text != null && (Convert.ToDouble(txtDiscAmt.Text) - PackageConcenssionAmt) > 0.0)
                    {
                        //if ((Convert.ToDouble(txtDiscAmt.Text) - PackageConcenssionAmt) > 0.0)
                        //{
                        cmbConcessionAuthBy.IsEnabled = true;
                        //}
                    }
                    //if (txtDiscAmt.Text != null && Convert.ToDouble(txtDiscAmt.Text) > 0.0)
                    //{
                    //    cmbConcessionAuthBy.IsEnabled = true;

                    //}
                }
            }
            else
            {
                cmbConcessionAuthBy.IsEnabled = false;
                SampleHeader.Text = CompanyName + " Payment Details ";

            }

            FillUserList();
            FillBank();
            FillPayMode();
            FillConcessionAutBy();

            if (PaymentFor == PaymentForType.Advance || PaymentFor == PaymentForType.Refund)
            {
                tabControl1.Visibility = System.Windows.Visibility.Collapsed;
                PanelBillDetails.Visibility = System.Windows.Visibility.Collapsed;
                cmbConcessionAuthBy.IsEnabled = false;  // Added By CDS 

                if (PaymentFor == PaymentForType.Refund && Convert.ToDouble(TxtpaidAmt.Text) > ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RefundAmount)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Bydefault Cheque payment mode is selected, Because refund payment amount is exceeding the configured refund amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    IsPaymentModeEnabled = true;
                }

                //borderPaySummary.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (PaymentFor == PaymentForType.Bill)
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID)      ////if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)                
                {
                    //tabCompanyAdvance.IsEnabled = false;
                    dgCompanyAdvDtl.IsEnabled = false;
                }
                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                {
                    FillPatientAdvance();
                }
                if (PatientID > 0)
                {
                    FillPatientAdvance();
                }
            }
            if (IscompanyBill == true)
            {
                FillPatientAdvance();
            }

            if (PaymentList == null)
            {
                PaymentList = new ObservableCollection<clsPayment>();
                dgPayment.ItemsSource = PaymentList;
            }
        }

        private void FillPatientAdvance()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetPatientAdvanceListBizActionVO BizAction = new clsGetPatientAdvanceListBizActionVO();
                if (IscompanyBill == false)
                {
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {
                        BizAction.ID = 0;
                        BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                        BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    }
                    else
                    {
                        BizAction.PatientID = PatientID;
                        BizAction.CompanyID = CompanyIDForBill;
                        BizAction.PatientUnitID = PatientUNitID;
                        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    }
                }
                else
                {
                    BizAction.CompanyID = CompanyID;
                    BizAction.IsFromCompany = true;
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                //BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

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
                            foreach (var item in objList)
                            {
                                item.Used = 0;
                                if (item.CompanyID > 0)
                                {
                                    objCompanyAdvanceList.Add(item);
                                    CompanyTotalAmt += item.Total;
                                    CompanyBalanceAmt += item.Balance;
                                    //CompanyBalanceAmt = item.Balance; // Added By CDS 
                                }
                                else
                                {
                                    objPatientAdvanceList.Add(item);
                                    PatTotalAmt += item.Total;
                                    PatBalanceAmt += item.Balance;
                                    //PatBalanceAmt = item.Balance;// Added By CDS 
                                }
                            }
                            txtPatTotalAmt.Text = PatTotalAmt.ToString();
                            txtPatBalanceAmt.Text = PatBalanceAmt.ToString();
                            txtPatConsumedAmt.Text = (PatTotalAmt - PatBalanceAmt).ToString();
                            txtCompanyTotAmt.Text = CompanyTotalAmt.ToString();
                            txtCompanyBalAmt.Text = CompanyBalanceAmt.ToString();
                            txtCompanyConsAmt.Text = (CompanyTotalAmt - CompanyBalanceAmt).ToString();

                            var CompanyAdvance = from r in objCompanyAdvanceList
                                                 where r.Balance > 0
                                                 select r;

                            //dgCompanyAdvDtl.ItemsSource = null;
                            //dgCompanyAdvDtl.ItemsSource = objCompanyAdvanceList;
                            dgCompanyAdvDtl.ItemsSource = null;
                            dgCompanyAdvDtl.ItemsSource = CompanyAdvance.ToList();

                            //List<clsAdvanceVO> objPatientAdvanceNewList = new List<clsAdvanceVO>();
                            //List<clsAdvanceVO> objCompanyAdvanceNewList = new List<clsAdvanceVO>();

                            var PatientAdvance = from r in objPatientAdvanceList
                                                 where r.Balance > 0
                                                 select r;

                            dgPatientAdvDtl.ItemsSource = null;
                            dgPatientAdvDtl.ItemsSource = PatientAdvance.ToList();

                            //dgPatientAdvDtl.ItemsSource = null;
                            //dgPatientAdvDtl.ItemsSource = objPatientAdvanceList;
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
            if (PatBalanceAmt < TotalAmount) //PatTotalAmt comented by ARZ
            {
                if (CompanyBalanceAmt > 0)
                {
                    if (CompanyBalanceAmt < TotalAmount)
                        lblInsufficient.Text = "**Insufficient Advance";
                }
                else
                    lblInsufficient.Text = "**Insufficient Advance";
            }
            else
                lblInsufficient.Text = string.Empty;

            // Added By CDS For Company Advanced Auto Consume
            foreach (var item in objCompanyAdvanceList)
            {
                clsPayment mPay = new clsPayment();
                mPay.PayModeID = Convert.ToInt64(MaterPayModeList.CompanyAdvance);
                mPay.PayMode = MaterPayModeList.CompanyAdvance.ToString();
                if (item.Balance > 0)
                {
                    if (BalAmt > 0)
                    {
                        if (BalAmt > item.Balance) //PatTotalAmt
                        {
                            mPay.Amount = item.Balance;  //item.Total; comented by ARZ
                            item.Used = item.Balance;  //item.Total; comented by ARZ
                            //BalAmt = BalAmt - item.Total;
                            BalAmt = BalAmt - item.Balance;
                            item.Balance = 0;
                            mPay.AdvanceID = item.ID;
                            PaymentList.Add(mPay);
                        }
                        else
                        {
                            // Added By CDS 
                            mPay.Amount = BalAmt;
                            item.Used = BalAmt;
                            item.Balance = item.Balance - item.Used; //item.Total
                            BalAmt = 0;
                            mPay.AdvanceID = item.ID;
                            PaymentList.Add(mPay);
                            // Added By CDS 

                            // OLD 
                            //mPay.Amount = BalAmt;
                            //item.Used = BalAmt;
                            //item.Balance = PatBalanceAmt - item.Used; //item.Total
                            //mPay.AdvanceID = item.ID;
                            //PaymentList.Add(mPay);
                            //OLD
                        }
                    }
                }
            }
            // Added By CDS 

            foreach (var item in objPatientAdvanceList)
            {
                clsPayment mPay = new clsPayment();
                mPay.PayModeID = Convert.ToInt64(MaterPayModeList.PatientAdvance);
                mPay.PayMode = MaterPayModeList.PatientAdvance.ToString();
                if (item.Balance > 0)
                {
                    if (BalAmt > 0)
                    {
                        if (BalAmt > item.Balance) //PatTotalAmt
                        {
                            mPay.Amount = item.Balance; //item.Total; comented by ARZ
                            item.Used = item.Balance; //item.Total; comented by ARZ
                            //BalAmt = BalAmt - item.Total;
                            BalAmt = BalAmt - item.Balance;
                            item.Balance = 0;
                            mPay.AdvanceID = item.ID;
                            PaymentList.Add(mPay);
                        }
                        else
                        {
                            // Added By CDS 
                            mPay.Amount = BalAmt;
                            item.Used = BalAmt;
                            item.Balance = item.Balance - item.Used; //item.Total
                            BalAmt = 0;
                            mPay.AdvanceID = item.ID;
                            PaymentList.Add(mPay);
                            // Added By CDS 


                            // OLD 
                            //mPay.Amount = BalAmt;
                            //item.Used = BalAmt;
                            //item.Balance = PatBalanceAmt - item.Used; //item.Total
                            //mPay.AdvanceID = item.ID;
                            //PaymentList.Add(mPay);
                            //OLD
                        }
                    }
                }
            }
            CalculateTotalPaidAmount();

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            #region OLD Code Commented By CDS
            //ClickedFlag += 1;
            //string msgTitle = "";
            //string msgText = "";
            //bool IsValid = true;
            //if (ClickedFlag == 1)
            //{
            //    CalculateTotalPaidAmount();
            //    if (TotalPaymentAmount < TotalAmount && BillPaymentType == BillPaymentTypes.AgainstServices)
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Total Payment Must Equals To Total Amount : " + TotalAmount, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        IsValid = false;
            //        msgW1.Show();                }

            //    if (IsValid)
            //    {
            //        switch (PaymentFor)
            //        {
            //            case PaymentForType.None:
            //                break;
            //            case PaymentForType.Advance:
            //                msgText = "Are You Sure You Want To Save The Advance Details";
            //                break;
            //            case PaymentForType.Refund:
            //                msgText = "Are You Sure You Want To Save The Refund Details";
            //                break;
            //            case PaymentForType.Bill:
            //                msgText = "Are You Sure You Want To Save The Payment Details";
            //                break;
            //            default:
            //                break;
            //        }

            //        MessageBoxControl.MessageBoxChildWindow msgW =
            //           new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            //        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            //        {
            //            if (res == MessageBoxResult.Yes)
            //            {
            //                this.DialogResult = true;
            //                // DateTime payDate = DateTime.Now;
            //                Payment = new clsPaymentVO();

            //                Payment.PayeeNarration = txtNarration.Text;
            //                Payment.CreditAuthorizedBy = 0;
            //                Payment.ChequeAuthorizedBy = 0;
            //                Payment.PaidAmount = TotalPaymentAmount;

            //                if (PaymentFor == PaymentForType.Bill)
            //                {                                
            //                    Payment.BillPaymentType = this.BillPaymentType;
            //                    Payment.BillAmount = Convert.ToDouble(txtPayTotalAmount.Text);
            //                    Payment.BillBalanceAmount = Payment.BillAmount - Payment.PaidAmount;
            //                }  
            //                if (cmbCreditAuthBy.SelectedItem != null)
            //                    Payment.CreditAuthorizedBy = ((MasterListItem)cmbCreditAuthBy.SelectedItem).ID;

            //                if (cmbCheckAuthBy.SelectedItem != null)
            //                    Payment.ChequeAuthorizedBy = ((MasterListItem)cmbCheckAuthBy.SelectedItem).ID;

            //                if (cmbCompanyAdvAuthBy.SelectedItem != null)
            //                    Payment.ComAdvAuthorizedBy = ((MasterListItem)cmbCompanyAdvAuthBy.SelectedItem).ID;

            //                // Payment.co
            //                for (int i = 0; i < PaymentList.Count; i++)
            //                {
            //                    clsPaymentDetailsVO objPay = new clsPaymentDetailsVO();

            //                    objPay.PaymentModeID = PaymentList[i].PayModeID;
            //                    objPay.AdvanceID = PaymentList[i].AdvanceID;
            //                    objPay.PaidAmount = PaymentList[i].Amount;
            //                    objPay.Number = PaymentList[i].Number;
            //                    objPay.Date = PaymentList[i].Date;
            //                    if (PaymentList[i].BankID != null)
            //                        objPay.BankID = PaymentList[i].BankID.Value;
            //                    Payment.PaymentDetails.Add(objPay);
            //                }

            //                if (OnSaveButton_Click != null)
            //                    OnSaveButton_Click(this, new RoutedEventArgs());
            //            }
            //            else
            //                ClickedFlag = 0;
            //        };
            //        msgW.Show();
            //    }
            //    else
            //        ClickedFlag = 0;
            //}

            #endregion

            // Added By CDS

            ClickedFlag += 1;
            string msgTitle = "";
            string msgText = "";
            bool IsValid = true;

            if (ClickedFlag == 1)
            {
                CalculateTotalPaidAmount();





                //if (PaymentList.Count == 0)
                //{
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than Zero", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    IsValid = false;
                //    msgW1.Show();
                //    ClickedFlag = 0;
                //}

                if (cmbPayMode.SelectedItem != null)
                {
                    clsPayment Pay = new clsPayment();
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
                    else if (((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.Cheque || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.CreditCard || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.DD || ((MasterListItem)cmbPayMode.SelectedItem).ID == (long)MaterPayModeList.DebitCard)
                    {
                        if (cmbBank.SelectedItem != null && ((MasterListItem)cmbBank.SelectedItem).ID > 0)
                        {
                            cmbBank.ClearValidationError();
                            IsValid = true;
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
                                dtpDate.SetValidation("Date must be greater than Today's Date.");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                IsValid = false;
                            }
                            else
                            {
                                dtpDate.ClearValidationError();
                                IsValid = true;
                            }

                            if (txtNumber.Text != null && txtNumber.Text.Trim().Length > 0)
                            {
                                if (txtNumber.IsEnabled == true && txtNumber.Text.Length < txtNumber.MaxLength)
                                {
                                    txtNumber.SetValidation("Number not valid");
                                    txtNumber.RaiseValidationError();
                                    txtNumber.Focus();
                                    IsValid = false;
                                }
                                else
                                {
                                    txtNumber.ClearValidationError();
                                    IsValid = true;
                                }
                            }
                            else
                            {
                                txtNumber.SetValidation("Number Required");
                                txtNumber.RaiseValidationError();
                                txtNumber.Focus();
                                IsValid = false;
                            }
                        }
                    }
                }

                if (PaymentFor == PaymentForType.Advance || PaymentFor == PaymentForType.Refund)
                {
                    if (PaymentList.Count == 0)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Payment Mode", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();
                    }
                    else if (Convert.ToInt64(TxtpaidAmt.Text) > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Payment Mode", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();
                    }
                    ClickedFlag = 0;
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

                if (IsSettleBill == true || IscompanyBill == true)
                {
                    if (PaymentList.Count == 0)
                    {

                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment must be greater than 0 ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();

                    }
                }

                if (IscompanyBill == true)
                {
                    if (TotalPaymentAmount != Convert.ToDouble(txtPayableAmt.Text) && TotalPaymentAmount > 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Paid Amount Should Be Equal to Payable Amount ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        IsValid = false;
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                }

                if (IscompanyBill == false)
                {
                    //if (txtDiscAmt.Text != null && txtDiscAmt.Text != "" && Convert.ToDouble(txtDiscAmt.Text) > 0.0)
                    if (txtDiscAmt.Text != null && txtDiscAmt.Text != "" && (Convert.ToDouble(txtDiscAmt.Text) - PackageConcenssionAmt) > 0.0)
                    {
                        if (ConcessionFromPlan == false)
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
                        ClickedFlag = 0;
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
                        ClickedFlag = 0;
                    }
                }

                /*For Checking The PaymentList Count which is Inserted into the PaymentDetails IF Paid Amount > 0 (i.e. TotalPaymentAmount) */

                if ((PaymentList == null || PaymentList.Count == 0) && TotalPaymentAmount > 0)
                {
                    IsValid = false;
                    msgText = "Payment Details Are not Added Properly";
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgW1.Show();
                    ClickedFlag = 0;
                }

                //if ((Convert.ToDouble(TxtpaidAmt.Text) > (((IApplicationConfiguration)App.Current).ApplicationConfigurations).CreditLimitOPD))
                //{
                //    IsValid = false;
                //    msgText = "Credit Amount Must be less than vredit limit";
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                //    msgW1.Show();

                //}


                if (IsValid)
                {


                    Payment = new clsPaymentVO();  // Added By CDS 

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

                        // In Case OF Bills  Concensation Then The Payment table BillBalanceAmount Settelemt Done Through Here //
                        if (TxtConAmt.Text != null && TxtConAmt.Text != "")
                        {

                            Payment.BillBalanceAmount = TotalAmount - (Payment.PaidAmount + Convert.ToDouble((TxtConAmt.Text)));
                            //Payment.BillBalanceAmount = TotalAmount - (Convert.ToDouble(TxtpaidAmt.Text) + Convert.ToDouble((TxtConAmt.Text)));
                        }
                        else
                        {
                            Payment.BillBalanceAmount = TotalAmount - Payment.PaidAmount;
                            //Payment.BillBalanceAmount = TotalAmount - Convert.ToDouble(TxtpaidAmt.Text);
                        }
                        long opdCreditlimit = 0;
                        opdCreditlimit = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CreditLimitOPD;

                        bool isCompanyFlag = false;
                        // if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                        // if (((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)
                        if (PatientCategoryID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CategoryID)
                        {
                            isCompanyFlag = true;
                        }

                        isCompanyFlag = true;

                        if (isCompanyFlag == false && Payment.BillBalanceAmount > opdCreditlimit)         // if (Convert.ToDouble(TxtpaidAmt.Text) > opdCreditlimit)                       
                        {

                            //commented by rohini as per disscussion with mangesh sir dated 19/5/16 company have Balence
                            //string msgText1 = "Balance Amount Is Greater Than OPD Credit Limit Are you sure you want to save the Bill Details as Draft ?";

                            //MessageBoxControl.MessageBoxChildWindow msgW1 =
                            //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText1, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                            //checkCredit = true;

                            //msgW1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                            //{
                            //    if (res1 == MessageBoxResult.Yes)
                            //    {

                                    IsBillAsDraft = true;

                                    if (cmbCreditAuthBy.SelectedItem != null)
                                        Payment.CreditAuthorizedBy = ((MasterListItem)cmbCreditAuthBy.SelectedItem).ID;

                                    if (cmbConcessionAuthBy.SelectedItem != null)
                                        Payment.ChequeAuthorizedBy = ((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID;

                                    if (cmbCompanyAdvAuthBy.SelectedItem != null)
                                        Payment.ComAdvAuthorizedBy = ((MasterListItem)cmbCompanyAdvAuthBy.SelectedItem).ID;

                                    this.DialogResult = true;

                                    if (OnSaveButton_Click != null)
                                        OnSaveButton_Click(this, new RoutedEventArgs());

                                   
                            //    }
                            //    else
                            //    {
                            //        //ClickedFlag = 0;
                            //    }
                            //};
                            //msgW1.Show();
                        }
                        else
                        {
                            checkCredit = false;
                            #region Commented
                            //this.DialogResult = true;

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

                            //if (OnSaveButton_Click != null)
                            //    OnSaveButton_Click(this, new RoutedEventArgs());
                            #endregion
                        }

                    }

                    if (checkCredit == false)
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

                                //Payment = new clsPaymentVO(); Commmented By CDS 

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

                                    // In Case OF Bills  Concensation Then The Payment table BillBalanceAmount Settelemt Done Through Here //
                                    if (TxtConAmt.Text != null && TxtConAmt.Text != "")
                                    {
                                        //Payment.BillBalanceAmount = Payment.BillAmount - (Payment.PaidAmount + Convert.ToDouble((TxtConAmt.Text)));
                                        Payment.BillBalanceAmount = TotalAmount - (Payment.PaidAmount + Convert.ToDouble((TxtConAmt.Text)));
                                    }
                                    else
                                    {
                                        //Payment.BillBalanceAmount = Payment.BillAmount - Payment.PaidAmount;
                                        Payment.BillBalanceAmount = TotalAmount - Payment.PaidAmount;
                                    }

                                }


                                //if (PaymentFor == PaymentForType.Advance || PaymentFor == PaymentForType.Refund)
                                //{

                                if (cmbCreditAuthBy.SelectedItem != null)
                                    Payment.CreditAuthorizedBy = ((MasterListItem)cmbCreditAuthBy.SelectedItem).ID;

                                if (cmbConcessionAuthBy.SelectedItem != null)
                                    Payment.ChequeAuthorizedBy = ((MasterListItem)cmbConcessionAuthBy.SelectedItem).ID;

                                if (cmbCompanyAdvAuthBy.SelectedItem != null)
                                    Payment.ComAdvAuthorizedBy = ((MasterListItem)cmbCompanyAdvAuthBy.SelectedItem).ID;


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

                                    Payment.PaymentDetails.Add(objPay);
                                }
                                StoreID = StoreID;
                                if (OnSaveButton_Click != null)
                                    OnSaveButton_Click(this, new RoutedEventArgs());
                                //}
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
            else
                ClickedFlag = 0;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            //if (OnSaveButton_Click != null)
            //    OnSaveButton_Click(this, new RoutedEventArgs());

            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsNEFTRTGS)
            {
                if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsItSpecialChar()) && textBefore != null)
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
                IsNEFTRTGS = false;
                long ID = ((MasterListItem)cmbPayMode.SelectedItem).ID;
                if (ID > 0)
                {
                    cmdAddPayment.IsEnabled = true;
                    TxtpaidAmt.IsEnabled = true;
                    //if (((MaterPayModeList)ID == MaterPayModeList.Cash || (MaterPayModeList)ID == MaterPayModeList.StaffFree))
                    if (((MasterListItem)cmbPayMode.SelectedItem).ID == 1 || ((MasterListItem)cmbPayMode.SelectedItem).ID == 6 || ((MasterListItem)cmbPayMode.SelectedItem).ID == 11)
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
                    cmbCompanyAdvAuthBy.IsEnabled = false;

                    switch ((MaterPayModeList)ID)
                    {
                        case MaterPayModeList.Credit:
                            txtNumber.MaxLength = 16;
                            cmbCreditAuthBy.IsEnabled = true;
                            break;
                        case MaterPayModeList.Cash:
                            break;
                        case MaterPayModeList.Cheque:
                            txtNumber.MaxLength = 6;
                            cmbCheckAuthBy.IsEnabled = true;
                            dtpDate.DisplayDateStart = DateTime.Now.AddDays(-7);
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
                        case MaterPayModeList.NEFTRTGS:
                            txtNumber.MaxLength = 16;
                            IsNEFTRTGS = true;
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
                //if (IsPaymentModeEnabled)
                //{
                //    cmbPayMode.SelectedValue = (long)2; //MaterPayModeList.Cheque;
                //    cmbPayMode.IsEnabled = false;
                //}
                //else
                //    cmbPayMode.IsEnabled = true;

            }
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (PaymentList != null)
            {
                if (dgPayment.SelectedItem != null)
                {
                    string msgText = "Are You Sure \n  You Want To Delete The Row";
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
                                            advance.Balance += advance.Used;
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
                                                advance.Balance += advance.Used;
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
            if (Convert.ToDouble(TxtpaidAmt.Text) == 0)
            {
                TxtpaidAmt.SetValidation("Amount should be greater than Zero");
                TxtpaidAmt.RaiseValidationError();
                TxtpaidAmt.Focus();
                isValid = false;
            }

            //if (!((MaterPayModeList)mPay.PayModeID == MaterPayModeList.Cash || (MaterPayModeList)mPay.PayModeID == MaterPayModeList.StaffFree))
            if (!(((MasterListItem)cmbPayMode.SelectedItem).ID == 1 || ((MasterListItem)cmbPayMode.SelectedItem).ID == 6 || ((MasterListItem)cmbPayMode.SelectedItem).ID == 11))
            {
                if (!IsPaymentModeEnabled && !string.IsNullOrEmpty(txtNumber.Text) && !IsNEFTRTGS)
                {
                    if (Convert.ToInt64(txtNumber.Text) == 0)
                    {
                        txtNumber.SetValidation("Number Not Valid");
                        txtNumber.RaiseValidationError();
                        txtNumber.Focus();
                        isValid = false;
                    }
                }

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

                        if (dtpDate.SelectedDate == null)
                        {
                            dtpDate.SetValidation("Date Required");
                            dtpDate.RaiseValidationError();
                            dtpDate.Focus();
                            isValid = false;
                        }
                        else if (((MasterListItem)cmbPayMode.SelectedItem).ID == 2) // for Cheque
                        {
                            if (dtpDate.SelectedDate.Value.Date < DateTime.Now.Date.AddDays(-7))
                            {
                                dtpDate.SetValidation("You can select only one week prior Date");
                                dtpDate.RaiseValidationError();
                                dtpDate.Focus();
                                isValid = false;
                            }
                            else
                            {
                                mPay.Date = dtpDate.SelectedDate;
                            }
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
                cmbPayMode.SelectedValue = (long)0;
                dtpDate.SelectedDate = null;
                PaymentList.Add(mPay);
                TxtpaidAmt.Text = (TotalAmount - TotalPaymentAmount).ToString();
            }

        }

        private void chkAddPatientAdvance_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                clsAdvanceVO obj;
                if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                    obj = (clsAdvanceVO)dgPatientAdvDtl.SelectedItem;
                else
                    obj = (clsAdvanceVO)dgCompanyAdvDtl.SelectedItem;

                if (obj != null)
                {
                    if (obj.Used > obj.Balance)
                    {
                        if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                            ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance;
                        else
                            ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Balance;

                        ((CheckBox)sender).IsChecked = false;
                        string msgText = "You Can Not Consume Amount Greater Than Balance Amount.";
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

                        if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
                        {
                            mPay.PayModeID = (long)MaterPayModeList.PatientAdvance;
                            mPay.PayMode = MaterPayModeList.PatientAdvance.ToString();
                        }
                        else
                        {
                            mPay.PayModeID = (long)MaterPayModeList.CompanyAdvance;
                            mPay.PayMode = MaterPayModeList.CompanyAdvance.ToString();
                        }

                        double amt = 0;

                        amt = obj.Used;
                        if (amt <= 0)
                        {
                            ((CheckBox)sender).IsChecked = false;
                            string msgText = "Amount Should Be Greater Than Zero.";
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
                            string msgText = "Total Amount Is Exceeding The Total Payment Amount : " + TotalAmount;

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

        //private void AddAdvance()
        //{
        //    clsAdvanceVO obj;

        //    //if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
        //    //    obj = (clsAdvanceVO)dgPatientAdvDtl.SelectedItem;
        //    //else
        //        //obj = (clsAdvanceVO)dgCompanyAdvDtl.SelectedItem;

        //    if (obj != null)
        //    {
        //        if (obj.Used > obj.Balance)
        //        {
        //            if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
        //                ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance;
        //            else
        //                ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Used = ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Balance;

        //            ((CheckBox)sender).IsChecked = false;
        //            string msgText = "You Can Not Consume Amount Greater Than Balance Amount.";
        //            MessageBoxControl.MessageBoxChildWindow msgWD =
        //                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //            msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
        //            {
        //            };
        //            msgWD.Show();
        //        }
        //        else
        //        {
        //            clsPayment mPay = new clsPayment();

        //            if (((CheckBox)sender).Name.Equals("chkAddPatientAdvance"))
        //            {
        //                mPay.PayModeID = (long)MaterPayModeList.PatientAdvance;
        //                mPay.PayMode = MaterPayModeList.PatientAdvance.ToString();
        //            }
        //            else
        //            {
        //                mPay.PayModeID = (long)MaterPayModeList.CompanyAdvance;
        //                mPay.PayMode = MaterPayModeList.CompanyAdvance.ToString();
        //            }

        //            double amt = 0;

        //            amt = obj.Used;
        //            if (amt <= 0)
        //            {
        //                ((CheckBox)sender).IsChecked = false;
        //                string msgText = "Amount Should Be Greater Than Zero.";
        //                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                {
        //                };
        //                msgWD.Show();
        //            }
        //            else if ((amt + TotalPaymentAmount) > TotalAmount)
        //            {
        //                ((CheckBox)sender).IsChecked = false;
        //                string msgText = "Total Amount Is Exceeding The Total Payment Amount : " + TotalAmount;

        //                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
        //                {
        //                };
        //                msgWD.Show();
        //            }
        //            else
        //            {
        //                mPay.Amount = obj.Used;
        //                mPay.AdvanceID = obj.ID;
        //                //TotalPaymentAmount += amt;
        //                PaymentList.Add(mPay);
        //                CalculateTotalPaidAmount();
        //                if (dgPatientAdvDtl.SelectedItem != null)
        //                    ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Balance -= ((clsAdvanceVO)dgPatientAdvDtl.SelectedItem).Used;
        //                else if (dgCompanyAdvDtl.SelectedItem != null)
        //                    ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Balance -= ((clsAdvanceVO)dgCompanyAdvDtl.SelectedItem).Used;
        //            }
        //        }
        //    }

        //}


        private void txtNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtNumber.IsEnabled)
            {
                if (txtNumber.Text.Length < txtNumber.MaxLength)
                {
                    txtNumber.SetValidation("Number Not Valid");
                    txtNumber.RaiseValidationError();
                }
                else
                {
                    txtNumber.ClearValidationError();
                }

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
            //CalculateBillAmounttobeRecieveandRefund CalcWin = new CalculateBillAmounttobeRecieveandRefund();
            //CalcWin.txtBillAmt.Text = TotalPaymentAmount.ToString();
            //CalcWin.Show();
        }

        private void TxtConAmt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtConAmt.Text != null && TxtConAmt.Text != "")
            {
                if (PaymentList.Count == 0)
                {
                    if (Convert.ToDouble(TxtConAmt.Text) > TotalAmount)
                    {
                        TxtConAmt.SetValidation("Concession amount should not be greater than Total Amount");
                        TxtConAmt.RaiseValidationError();
                        TxtConAmt.Focus();

                    }
                    else
                    {
                        TxtConAmt.RaiseValidationError();
                        TxtpaidAmt.Text = (TotalAmount - Convert.ToDouble(TxtConAmt.Text)).ToString();
                    }
                }
                else
                {
                    double Pamt = 0;
                    foreach (var item in PaymentList)
                    {
                        Pamt = Pamt + item.Amount;
                        double Amt = ((TotalAmount) - Convert.ToDouble(TxtConAmt.Text));
                        TxtpaidAmt.Text = (Amt - Pamt).ToString();
                    }
                }
            }
            else
            {
                TxtpaidAmt.Text = TotalAmount.ToString();

            }
        }


        #region OLD Commented
        //private void TxtpaidAmt_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    bool isValid = true;
        //    TxtpaidAmt.ClearValidationError();

        //    double amt = 0;
        //    double opdCreditlimit = 0;
        //    if (TxtpaidAmt.Text != null && TxtpaidAmt.Text.Length > 0)
        //    {
        //        amt = double.Parse(TxtpaidAmt.Text);

        //        // Added By CDS To Avoid Enter More than Config Credit Amount 
        //        opdCreditlimit = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CreditLimitOPD;
        //        if (amt < opdCreditlimit)
        //        {
        //            TxtpaidAmt.SetValidation("Total Amount Is less Than The OPD Credit Limit : " + opdCreditlimit);
        //            TxtpaidAmt.RaiseValidationError();
        //            TxtpaidAmt.Focus();
        //            isValid = false;
        //        }
        //        else
        //        {
        //            if (amt > 0)
        //            {
        //                if ((amt + TotalPaymentAmount) > TotalAmount)
        //                {
        //                    TxtpaidAmt.SetValidation("Total Amount Is Exceeding The Total Payment Amount : " + TotalAmount);
        //                    TxtpaidAmt.RaiseValidationError();
        //                    TxtpaidAmt.Focus();
        //                    isValid = false;
        //                }
        //                //else
        //                //{
        //                //    mPay.Amount = double.Parse(TxtpaidAmt.Text);
        //                //    TotalPaymentAmount += amt;
        //                //}
        //            }
        //            else if (double.Parse(TxtpaidAmt.Text) < 0)
        //            {
        //                TxtpaidAmt.SetValidation("Amount Cannot Be Negative");
        //                TxtpaidAmt.RaiseValidationError();
        //                TxtpaidAmt.Focus();
        //                isValid = false;
        //            }
        //        }    //End Of Else
        //    }
        //}
        #endregion
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

