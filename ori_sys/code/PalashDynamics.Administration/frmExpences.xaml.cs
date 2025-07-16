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
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.UserControls;
using System.ComponentModel;
using System.Windows.Browser;
using PalashDynamics.Animations;

namespace PalashDynamics.Administration
{
    public partial class frmExpences : UserControl
    {
        clsExpensesVO SelectedBill { get; set; }
        private SwivelAnimation objAnimation;
        int ClickedFlag = 0;
        public frmExpences()
        {
            InitializeComponent();
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmExpense_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 500)));
            //dtpFromDte.SelectedDate = DateTime.Now.Date;
            //dtpToDate.SelectedDate = DateTime.Now.Date;

            //============================================================================================================
            //Paging
            DataList = new PagedSortableCollectionView<clsExpensesVO>();
            DataListPageSize = 15;
            dgDataPager.PageSize = DataListPageSize;
            dgDataPager.Source = DataList;
            //============================================================================================================
        }

        bool IsCancel = true;
        bool IsNew = true;
        WaitIndicator Indicatior = null;
        #region INotifyPropertyChanged Members

        /// <summary>
        /// Implemts the INotifyPropertyChanged interface.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (null != handler)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


        private void frmExpense_Loaded(object sender, RoutedEventArgs e)
        {
            IsNew = false;
            EnableControls(IsNew);
            dtpvoucherDate.SelectedDate = DateTime.Now.Date;
            //dtpFromDte.SelectedDate = DateTime.Now.Date;
            //dtpToDate.SelectedDate = DateTime.Now.Date;   
            FillClinicSearch();
            FillClinic();
            cmbClinic.IsEnabled = false;
            cmbClinicSearch.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            FillExpensesAgnstSearch();
            dtpvoucherDate.DisplayDateEnd = DateTime.Today;
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            FillExpenseDetails();
            SetCommandButtonState("Load");
        }
        #region Paging
        public PagedSortableCollectionView<clsExpensesVO> DataList { get; private set; }
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
                OnPropertyChanged("DataListPageSize");
            }
        }
        #endregion
        private void FillClinicSearch()
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

                    cmbClinicSearch.ItemsSource = null;
                    cmbClinicSearch.ItemsSource = objList;
                    cmbClinicSearch.SelectedItem = objList[0];
                    cmbClinicSearch.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;


                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillClinic()
        {
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_UnitMaster;
            BizAction.MasterList = new List<MasterListItem>();
            //PalashServiceClient Client = null;
            //Client = new PalashServiceClient();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbClinic.ItemsSource = null;
                    cmbClinic.ItemsSource = objList;
                    cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void FillExpensesAgnstSearch()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ExpenseMaster;
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

                    cmbExpensesSearch.ItemsSource = null;
                    cmbExpensesSearch.ItemsSource = objList;
                    cmbExpensesSearch.SelectedItem = objList[0];

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillFillExpensesAgnst()
        {
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_ExpenseMaster;
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

                    cmbExpenses.ItemsSource = null;
                    cmbExpenses.ItemsSource = objList;
                    cmbExpenses.SelectedItem = objList[0];

                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        public void EnableControls(bool IsNew)
        {
            if (IsNew == false)
            {
                FillClinic();
                FillFillExpensesAgnst();
                cmbClinic.IsEnabled = false;
                cmbExpenses.IsEnabled = false;
                dtpvoucherDate.IsEnabled = false;
                txtAmount.IsEnabled = false;
                chkFreezBill.IsEnabled = false;
                txtRemark.IsEnabled = false;
                txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            }
            else
            {
                FillClinic();
                FillFillExpensesAgnst();
                cmbClinic.IsEnabled = true;
                cmbExpenses.IsEnabled = true;
                dtpvoucherDate.IsEnabled = true;
                txtAmount.IsEnabled = true;
                chkFreezBill.IsEnabled = true;
                txtRemark.IsEnabled = true;
                txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;
                case "Cancel":
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    break;

                case "View":
                    cmdSave.IsEnabled = true;
                    cmdNew.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
            IsNew = true;
            IsModify = false;
            dgExpensesList.SelectedItem = null;

            EnableControls(IsNew);
            SetCommandButtonState("New");
            cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            txtVoucherCreatedBY.Text = "";
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            txtVoucherNo.Text = "";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
            mElement.Text = " : " + "New Expense Details";
            objAnimation.Invoke(RotationType.Forward);
        }
        private void ClearData()
        {
            this.DataContext = new clsExpensesVO();
            cmbExpenses.SelectedValue = (long)0;
            dtpvoucherDate.SelectedDate = DateTime.Now.Date;
            dtpFromDte.SelectedDate = DateTime.Now.Date;
            dtpToDate.SelectedDate = DateTime.Now.Date;
            txtAmount.Text = "";
            txtRemark.Text = "";
            cmbClinic.IsEnabled = false;
            chkFreezBill.IsChecked = false;
            cmbClinic.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            dtpvoucherDate.DisplayDateEnd = DateTime.Today;
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            SetCommandButtonState("Cancel");
            objAnimation.Invoke(RotationType.Backward);
            if (IsCancel == true)
            {
                //UserControl rootPage = Application.Current.RootVisual as UserControl;
                //TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                //mElement.Text = "Billing Configuration";
                //UIElement myData = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.Administration.frmBillingConfiguration") as UIElement;
                //((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
            else
            {
                IsCancel = true;
            }
        }

        private bool ChkValidation()
        {
            bool isFormValid = true;
            if (txtAmount.Text != string.Empty)
            {
                if (double.Parse(txtAmount.Text) <= 0)
                {
                    txtAmount.SetValidation("Amount must be greater than zero");
                    txtAmount.RaiseValidationError();
                    txtAmount.Focus();
                    isFormValid = false;
                }
                else
                    txtAmount.ClearValidationError();
            }

            if (cmbExpenses.SelectedItem == null)
            {
                cmbExpenses.TextBox.SetValidation("Please select Expenses");
                cmbExpenses.TextBox.RaiseValidationError();
                cmbExpenses.Focus();
                isFormValid = false;

            }
            else if (((MasterListItem)cmbExpenses.SelectedItem).ID == (long)0)
            {
                cmbExpenses.TextBox.SetValidation("Please select Expenses");
                cmbExpenses.TextBox.RaiseValidationError();
                cmbExpenses.Focus();
                isFormValid = false;

            }
            else
                cmbExpenses.TextBox.ClearValidationError();
            return isFormValid;

        }
        public bool IsModify = false;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            ClickedFlag += 1;

            if (dgExpensesList.SelectedItem != null)
            {
                if (((clsExpensesVO)dgExpensesList.SelectedItem).ID > 0)
                {
                    IsModify = true;
                }

            }
            if (ChkValidation())
            {

                if (chkFreezBill.IsChecked == true)
                {

                    string msgTitle = "";
                    string msgText = "Are you sure you want to Freeze the Expense?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (arg) =>
                    {
                        if (arg == MessageBoxResult.Yes)
                        {

                            PayExpense paymentWin = new PayExpense();

                            paymentWin.TotalAmount = Convert.ToDouble(txtAmount.Text);


                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.Show();
                        }
                        else

                            ClickedFlag = 0;
                        cmdSave.IsEnabled = true;


                    };
                    msgWD.Show();

                }
                else
                {
                    SaveExpense(null, false);
                }
            }
            else
                cmdSave.IsEnabled = true;
        }
        private void SaveExpense(List<clsExpensesDetailsVO> pPayDetails, bool IsFreezed)
        {
            clsExpensesVO advObj = new clsExpensesVO();
            advObj.ExpenseDetails = new List<clsExpensesDetailsVO>();

            if (IsModify == true)
            {
                advObj.ID = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
            }
            advObj.ExpenseDetails = pPayDetails;
            if (cmbClinic.SelectedItem != null)
                advObj.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbExpenses.SelectedItem != null)
                advObj.Expense = ((MasterListItem)cmbExpenses.SelectedItem).ID;

            advObj.ExpenseDate = dtpvoucherDate.SelectedDate.Value;
            if (!string.IsNullOrEmpty(txtAmount.Text))
                advObj.Amount = float.Parse(txtAmount.Text);

            advObj.Remark = txtRemark.Text;
            advObj.VoucherNo = txtVoucherNo.Text;
            advObj.voucherCreatedby = txtVoucherCreatedBY.Text;


            if (chkFreezBill.IsChecked == true)
            {
                advObj.IsFreezed = true;
            }
            else
                advObj.IsFreezed = false;
            clsAddExpensesBizActionVO BizAction = new clsAddExpensesBizActionVO();

            BizAction.Details = advObj;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null && arg.Result != null && ((clsAddExpensesBizActionVO)arg.Result).Details != null)
                {

                    if (((clsAddExpensesBizActionVO)arg.Result).Details != null)
                    {
                        FillExpenseDetails();
                        // System.Windows.Browser.HtmlPage.Window.Alert("Visit saved successfully.");
                        objAnimation.Invoke(RotationType.Backward);
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "without adding payment details Expense saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClearFormData();
                    }

                }
                else
                {
                    //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Expense details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            clsExpensesVO advObj = new clsExpensesVO();
            advObj.ExpenseDetails = new List<clsExpensesDetailsVO>();
            //if (((PalashDynamics.Administration.PayExpense)sender).DialogResult == true)
            //{

            //advObj = (clsExpensesVO)this.DataContext;

            if (((PayExpense)sender).Amount != null)
            {
                advObj.ExpenseDetails = ((PayExpense)sender).Amount.ExpenseDetails;

            }
            if (IsModify == true)
            {
                advObj.ID = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
            }

            //SaveExpense(advObj.ExpenseDetails, true);

            if (cmbClinic.SelectedItem != null)
                advObj.UnitID = ((MasterListItem)cmbClinic.SelectedItem).ID;
            if (cmbExpenses.SelectedItem != null)
                advObj.Expense = ((MasterListItem)cmbExpenses.SelectedItem).ID;

            advObj.ExpenseDate = dtpvoucherDate.SelectedDate.Value;
            if (!string.IsNullOrEmpty(txtAmount.Text))
                advObj.Amount = float.Parse(txtAmount.Text);

            advObj.Remark = txtRemark.Text;
            advObj.VoucherNo = txtVoucherNo.Text;
            advObj.voucherCreatedby = txtVoucherCreatedBY.Text;
            if (chkFreezBill.IsChecked == true)
            {
                advObj.IsFreezed = true;
            }
            else
                advObj.IsFreezed = false;
            clsAddExpensesBizActionVO BizAction = new clsAddExpensesBizActionVO();

            BizAction.Details = advObj;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                ClickedFlag = 0;
                if (arg.Error == null && arg.Result != null && ((clsAddExpensesBizActionVO)arg.Result).Details != null)
                {

                    if (((clsAddExpensesBizActionVO)arg.Result).Details != null)
                    {
                        txtVoucherNo.Text = "";
                        FillExpenseDetails();
                        objAnimation.Invoke(RotationType.Backward);
                        // System.Windows.Browser.HtmlPage.Window.Alert("Visit saved successfully.");
                        string strMsg = string.Empty;
                        if (IsModify) strMsg = "Updated"; else strMsg = "Saved";
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Expense details " + strMsg + " successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClearFormData();
                    }

                }
                else
                {
                    //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Expense details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
            //}
            //else
            //    ClickedFlag = 0;
        }

        private void ClearFormData()
        {
            //tabPatientSearch.IsSelected = true;
            txtVoucherNo.Text = "";
            cmbExpenses.SelectedValue = (long)0;
            txtRemark.Text = txtRemark.Text = string.Empty;
            txtAmount.Text = txtAmount.Text = string.Empty;
            txtVoucherCreatedBY.Text = ((IApplicationConfiguration)App.Current).CurrentUser.LoginName;
            cmdSave.IsEnabled = false;
            cmdNew.IsEnabled = true;

        }

        private void FillExpenseDetails()
        {

            clsGetExpensesListBizActionVO BizAction = new clsGetExpensesListBizActionVO();

            if (cmbClinicSearch.SelectedItem != null && ((MasterListItem)cmbClinicSearch.SelectedItem).ID != 0)
            {
                BizAction.UnitID = ((MasterListItem)cmbClinicSearch.SelectedItem).ID;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == false)
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            else if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId)
            {
                BizAction.UnitID = 1;
            }


            if (cmbExpensesSearch.SelectedItem != null)
                BizAction.Expense = ((MasterListItem)cmbExpensesSearch.SelectedItem).ID;
            //if (Rdbactive.IsChecked == true)
            //{
            //    BizAction.VoStatus = true;
            //}
            //else
            //    BizAction.VoStatus = false;
            //if (Rdbcancelled.IsChecked == true)
            //{ BizAction.VoStatus = true; }
            //else
            //    BizAction.VoStatus = false;
            //if (Rdbcancelled.IsChecked != null)
            //    BizAction.VoStatus = Rdbcancelled.IsChecked.Value;
            if (dtpFromDte.SelectedDate != null)
                BizAction.FromDate = dtpFromDte.SelectedDate.Value.Date;
            if (dtpToDate.SelectedDate != null)
                BizAction.ToDate = dtpToDate.SelectedDate.Value.Date;

            BizAction.Vouchercreatedby = txtVoucherCreatedBY.Text;
            BizAction.Voucherno = txtVoucherNosearch.Text;
            BizAction.IsPagingEnabled = true;
            BizAction.StartIndex = DataList.PageIndex * DataList.PageSize;
            BizAction.MaximumRows = DataList.PageSize;
            //Rdbactive.IsChecked = ((clsExpensesVO)this.DataContext).VoStatus;
            //Rdbcancelled.IsChecked = ((clsExpensesVO)this.DataContext).VoStatus;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetExpensesListBizActionVO)arg.Result).Details != null)
                    {
                        //dgDoctorList.ItemsSource = ((clsGetDoctorScheduleMasterListBizActionVO)arg.Result).DoctorScheduleList;

                        clsGetExpensesListBizActionVO result = arg.Result as clsGetExpensesListBizActionVO;

                        if (result.Details != null)
                        {
                            DataList.Clear();
                            DataList.TotalItemCount = ((clsGetExpensesListBizActionVO)arg.Result).TotalRows;
                            foreach (clsExpensesVO item in result.Details)
                            {
                                DataList.Add(item);
                            }
                        }

                        dgExpensesList.ItemsSource = null;
                        dgExpensesList.ItemsSource = DataList;

                        dgDataPager.Source = null;
                        dgDataPager.PageSize = BizAction.MaximumRows;
                        dgDataPager.Source = DataList;

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }



        private void txtVouchrCretedBy_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtAmount.Text != "")
            {

                if (Extensions.IsItDecimal(txtAmount.Text) == true)
                {
                    txtAmount.SetValidation("Amount should be Numeric");

                    return;
                }
            }
        }

        private void txtRemarkt_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void chkFreezExpenses_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void cmdmodify_Click(object sender, RoutedEventArgs e)
        //{
        //    SetCommandButtonState("Modify");
        //    string msgTitle = "Palash";
        //    string msgText = "Are you sure you want to Update the Expense ?";

        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
        //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //    msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW1_OnMessageBoxClosed);

        //    msgW1.Show();
        //}

        //void msgW1_OnMessageBoxClosed(MessageBoxResult result)
        //{
        //    if (result == MessageBoxResult.Yes)

        //    advObj.ExpenseDetails= new List<clsExpensesDetailsVO>();
        //    advObj.ExpenseDetails = pPayDetails;
        //      SaveExpense(List<clsExpensesDetailsVO> pPayDetails, true)

        //}



        //private void Rdbactive_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Rdbcancelled_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            bool res = true;
            ToolTip ToolTipControl = new ToolTip();


            if (dtpFromDte.SelectedDate != null && dtpToDate.SelectedDate != null)
            {

                if (dtpFromDte.SelectedDate > dtpToDate.SelectedDate)
                {

                    dtpFromDte.SetValidation("From Date should be less than To Date");
                    dtpFromDte.RaiseValidationError();
                    //dtpFromDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    //ToolTipControl.Content = "From Date should be less than To Date";
                    //ToolTipService.SetToolTip(dtpFromDate, ToolTipControl);
                    string strMsg = "From Date should be less than To Date";

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    dtpFromDte.Focus();
                    res = false;
                }
                else
                {
                    // dtpFromDate.BorderBrush = myBrush;
                    dtpFromDte.ClearValidationError();
                }
            }



            if (dtpFromDte.SelectedDate != null && dtpToDate.SelectedDate == null)
            {
                dtpToDate.SetValidation("Please Select To Date");
                dtpToDate.RaiseValidationError();
                string strMsg = "Please Select To Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpToDate.Focus();
                res = false;
            }
            else
            {
                dtpToDate.ClearValidationError();
            }

            if (dtpToDate.SelectedDate != null && dtpFromDte.SelectedDate == null)
            {
                dtpFromDte.SetValidation("Plase Select From Date");
                dtpFromDte.RaiseValidationError();

                string strMsg = "Plase Select From Date";

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();

                dtpFromDte.Focus();
                res = false;
            }
            else
            {
                dtpFromDte.ClearValidationError();
            }

            if (res)
            {
                dgDataPager.PageIndex = 0;
                FillExpenseDetails();

            }
        }
        private void txtVoucherBysearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtVoucherNosearch_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void hlbEditDetails_Click(object sender, RoutedEventArgs e)
        {

        }
        public long SelectedRecord;
        public long UnitID;
        private void hlbPrint_Click(object sender, RoutedEventArgs e)
        {

            if (dgExpensesList.SelectedItem != null)
            {
                SelectedRecord = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
                UnitID = ((clsExpensesVO)dgExpensesList.SelectedItem).UnitID;
            }

            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/CRM/Expense.aspx?ID=" + SelectedRecord + "&UnitID=" + UnitID), "_blank");

        }


        private void hlbEditDetails_Click_1(object sender, RoutedEventArgs e)
        {

            SetCommandButtonState("View");
            SelectedRecord = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
            grdBackPanel.DataContext = ((clsExpensesVO)dgExpensesList.SelectedItem);
            cmbClinic.IsEnabled = true;
            cmbExpenses.IsEnabled = true;
            dtpvoucherDate.IsEnabled = true;
            txtAmount.IsEnabled = true;
            chkFreezBill.IsEnabled = true;
            cmdNew.IsEnabled = true;
            txtRemark.IsEnabled = true;
            //FillFillExpensesAgnst();       
            if (((clsExpensesVO)dgExpensesList.SelectedItem).IsFreezed == true)
            {
                //cmdmodify.IsEnabled = false;
                cmdSave.IsEnabled = false;
            }
            else
            {
                //cmdmodify.IsEnabled = true;
                cmdSave.IsEnabled = true;
            }

            cmbClinic.SelectedValue = ((clsExpensesVO)dgExpensesList.SelectedItem).UnitID;

            cmbExpenses.SelectedValue = ((clsExpensesVO)dgExpensesList.SelectedItem).Expense;
            txtVoucherCreatedBY.Text = ((clsExpensesVO)dgExpensesList.SelectedItem).voucherCreatedby;
            txtVoucherNo.Text = ((clsExpensesVO)dgExpensesList.SelectedItem).VoucherNo;
            dtpvoucherDate.SelectedDate = ((clsExpensesVO)dgExpensesList.SelectedItem).ExpenseDate;
            txtAmount.Text = Convert.ToString(((clsExpensesVO)dgExpensesList.SelectedItem).Amount);
            txtRemark.Text = ((clsExpensesVO)dgExpensesList.SelectedItem).Remark;
            chkFreezBill.IsChecked = ((clsExpensesVO)dgExpensesList.SelectedItem).IsFreezed;
            cmdNew.IsEnabled = false;
            try
            {
                objAnimation.Invoke(RotationType.Forward);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //if (((CheckBox)sender).IsChecked == false)
            //{
            if (dgExpensesList.SelectedItem != null)
            {
                IsModify = true;
                clsExpensesVO advObj = new clsExpensesVO();
                advObj.ExpenseDetails = new List<clsExpensesDetailsVO>();

                if (IsModify == true)
                {
                    advObj.ID = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
                }
                //advObj.ExpenseDetails = pPayDetails;

                advObj.ID = ((clsExpensesVO)dgExpensesList.SelectedItem).ID;
                advObj.VoucherNo = ((clsExpensesVO)dgExpensesList.SelectedItem).VoucherNo;
                advObj.voucherCreatedby = ((clsExpensesVO)dgExpensesList.SelectedItem).voucherCreatedby;
                advObj.UnitID = ((clsExpensesVO)dgExpensesList.SelectedItem).UnitID;
                advObj.Status = Convert.ToBoolean(((System.Windows.Controls.Primitives.ToggleButton)(e.OriginalSource)).IsChecked);
                advObj.Remark = ((clsExpensesVO)dgExpensesList.SelectedItem).Remark;
                advObj.IsFreezed = ((clsExpensesVO)dgExpensesList.SelectedItem).IsFreezed;
                advObj.ExpenseDate = ((clsExpensesVO)dgExpensesList.SelectedItem).ExpenseDate;
                advObj.Expense = ((clsExpensesVO)dgExpensesList.SelectedItem).Expense;
                advObj.Amount = ((clsExpensesVO)dgExpensesList.SelectedItem).Amount;
                advObj.CreatedUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                advObj.AddedBy = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                advObj.AddedOn = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.MachineName;
                advObj.AddedDateTime = System.DateTime.Now;
                advObj.AddedWindowsLoginName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.WindowsUserName;
                clsAddExpensesBizActionVO BizAction = new clsAddExpensesBizActionVO();

                BizAction.Details = advObj;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                    if (arg.Error == null && arg.Result != null && ((clsAddExpensesBizActionVO)arg.Result).Details != null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Expense Status Updated Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
        }

        //private void Rdbactive_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void Rdbcancelled_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void chkRadioButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (Rdbactive.IsChecked == true)
        //    {
        //        ((clsExpensesVO)this.DataContext).VoStatus = true;

        //    }
        //    else
        //        ((clsExpensesVO)this.DataContext).VoStatus = false;
        //}

        //private void btnSettle_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgExpensesList.SelectedItem != null)
        //    {
        //        if (((clsExpensesVO)dgExpensesList.SelectedItem).IsFreezed == true)
        //            SettleBill();
        //    }
        //    else
        //    {

        //        string msgText = "Only freezed Bills can be Settled ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
        //        msgWD.Show();
        //    }
        //}

        //void SettleBill()
        //{
        //    if (dgExpensesList.SelectedItem != null && ((clsExpensesVO)dgExpensesList.SelectedItem).IsFreezed == true)
        //    {

        //        //InitialiseForm();
        //        SelectedBill = new clsExpensesVO();
        //        SelectedBill = (clsExpensesVO)dgExpensesList.SelectedItem;

        //        // FillChargeList(SelectedBill.ID, SelectedBill.UnitID, false, SelectedBill.Opd_Ipd_External_Id);


        //        string msgTitle = "";
        //        string msgText = "Are you sure you want to Settle the Bill ?";

        //        MessageBoxControl.MessageBoxChildWindow msgWD =
        //            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

        //        msgWD.OnMessageBoxClosed += (arg) =>
        //        {
        //            if (arg == MessageBoxResult.Yes)
        //            {
        //                bool isValid = true;
        //                //isValid = tr  // CheckValidations();

        //                if (isValid)
        //                {
        //                    PayExpense SettlePaymentWin = new PayExpense();

        //                    if (SelectedBill.BalanceAmountSelf > 0)
        //                        SettlePaymentWin.TotalAmount = SelectedBill.BalanceAmountSelf;



        //                    SettlePaymentWin.txtPayTotalAmount.Text = SelectedBill.Amount.ToString();

        //                    SettlePaymentWin.txtPayableAmt.Text = SelectedBill.BalanceAmountSelf.ToString();



        //                    SettlePaymentWin.OnSaveButton_Click += new RoutedEventHandler(SettlePaymentWin_OnSaveButton_Click);
        //                    SettlePaymentWin.Show();
        //                }
        //                else
        //                {
        //                    // InitialiseForm();

        //                }
        //            }
        //            else
        //            {
        //                // InitialiseForm();

        //            }

        //        };
        //        msgWD.Show();
        //    }
        //}

        //void SettlePaymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    WaitIndicator Indicatior = new WaitIndicator();

        //    try
        //    {
        //        if (((PalashDynamics.Administration.PayExpense)sender).DialogResult == true)

        //        {
        //            if (((PayExpense)sender).Amount != null)
        //            {

        //                Indicatior.Show();
        //                clsAddExpensesBizActionVO BizAction = new clsAddExpensesBizActionVO();
        //                clsExpensesVO pPayDetails = new clsExpensesVO();

        //                pPayDetails = ((PayExpense)sender).Amount;
        //                pPayDetails.IsSettled = true;
        //                BizAction.Details = pPayDetails;

        //                BizAction.Details.ID = SelectedBill.ID;
        //                BizAction.Details.Amount = SelectedBill.Amount;
        //                BizAction.Details.Date = DateTime.Now;
        //                BizAction.Details.BillBalanceAmount = pPayDetails.PaidAmount;


        //                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //                Client.ProcessCompleted += (s, arg) =>
        //                {
        //                    if (arg.Error == null && arg.Result != null)
        //                    {
        //                        if (((clsAddPaymentBizActionVO)arg.Result).Details != null)
        //                            PaymentID = ((clsAddPaymentBizActionVO)arg.Result).Details.ID;

        //                        clsUpdateBillPaymentDtlsBizActionVO mybillPayDetails = new clsUpdateBillPaymentDtlsBizActionVO();

        //                        mybillPayDetails.Details = SelectedBill;

        //                        mybillPayDetails.Details.BalanceAmountSelf = mybillPayDetails.Details.BalanceAmountSelf - pPayDetails.PaidAmount;
        //                        if (mybillPayDetails.Details.BalanceAmountSelf < 0) mybillPayDetails.Details.BalanceAmountSelf = 0;

        //                        PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //                        Client1.ProcessCompleted += (s1, arg1) =>
        //                        {

        //                            Indicatior.Close();
        //                            if (arg1.Error == null && arg1.Result != null)
        //                            {



        //                                if (dgBillList.ItemsSource != null)
        //                                {

        //                                    dgBillList.ItemsSource = null;
        //                                    dgBillList.ItemsSource = DataList;

        //                                }


        //                                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Payment Details Saved Sucessfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);



        //                                msgWD.OnMessageBoxClosed += (re) =>
        //                                {
        //                                    if (re == MessageBoxResult.OK)
        //                                    {
        //                                        PrintSettleBill(((clsBillVO)dgBillList.SelectedItem).ID, ((clsBillVO)dgBillList.SelectedItem).UnitID, PaymentID);
        //                                    }
        //                                };
        //                                msgWD.Show();


        //                            }
        //                            else
        //                            {

        //                                MessageBoxControl.MessageBoxChildWindow msgWD =
        //                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while updating Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                                msgWD.Show();
        //                            }


        //                        };

        //                        Client1.ProcessAsync(mybillPayDetails, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                        Client1.CloseAsync();



        //                    }
        //                    else
        //                    {
        //                        Indicatior.Close();
        //                        MessageBoxControl.MessageBoxChildWindow msgWD =
        //                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while Saving Payment Details", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

        //                        msgWD.Show();

        //                    }
        //                };
        //                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //                Client.CloseAsync();

        //            }
        //        }
        //        else
        //        {

        //        }



        //    }
        //    catch (Exception)
        //    {

        //        Indicatior.Close();
        //    }



        //}
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;


        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            //bool isValid = true;
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveNumber()))
            {
                if (textBefore == null)
                    textBefore = "0";
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
                //isValid = false;
            }
            //if ( isValid = true && (!((TextBox)sender).Text.IsValueDouble()))
            //{
            //    if (textBefore == null)
            //        textBefore = "0";
            //    ((TextBox)sender).Text = textBefore;
            //    ((TextBox)sender).SelectionStart = selectionStart;
            //    ((TextBox)sender).SelectionLength = selectionLength;
            //    textBefore = "";
            //    selectionStart = 0;
            //    selectionLength = 0;
            //}
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void dtpvoucherDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpvoucherDate.SelectedDate > DateTime.Now)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Voucher Date cannot be greater than todays date", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                dtpvoucherDate.SelectedDate = DateTime.Now;
            }
        }
    }
}
