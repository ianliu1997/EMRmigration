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
using System.ComponentModel;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Patient;
using OPDModule;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Administration.UserRights;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;


namespace CIMS.Forms
{
    public partial class frmAdvanceRefund : UserControl, IInitiateCIMS
    {
        int ClickedFlag = 0;
        long PatientID = 0;
        public bool IsApprovalRequest = false;
        public long AdvanceID = 0;
        public long AdvanceUnitID = 0;
        public string Action { get; set; }
        public string ModuleName { get; set; }
        UIElement myData = null;
        enum RefundModes
        {
            None = 0,
            RefundPatientAdvance = 1,
            RefundCompanyAdvance = 2
        }

        RefundModes RefundMode = RefundModes.None;

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();

            switch (Mode.ToUpper())
            {
                case "RPA":

                    RefundMode = RefundModes.RefundPatientAdvance;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        return;
                    }

                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsRefundVO()
                    {
                        PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer,
                        Date = DateTime.Now
                    };

                    break;

                case "RCA":
                    RefundMode = RefundModes.RefundCompanyAdvance;

                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                    mElement1.Text = "";

                    this.DataContext = new clsRefundVO() { Date = DateTime.Now, LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer };

                    break;

            }


        }

        #endregion

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;

        public frmAdvanceRefund()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(frmAdvanceRefund_Loaded);

        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsNumberValid()))
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
            catch (Exception)
            {

                //  throw;
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        void frmAdvanceRefund_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (RefundMode == RefundModes.RefundPatientAdvance && PatientID <= 0)
                {
                    cmdSave.IsEnabled = false;
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }
                else
                {
                    if (!IsPageLoded)
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                              && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                            {
                                //do  nothing
                            }
                            else
                                cmdSave.IsEnabled = false;
                        }
                        FillAdvanceDetails();
                        FillRefundDetails();
                        CheckValidations();
                        txtRefundAmount.Focus();
                        IsPageLoded = true;
                    }
                }
                GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
                SetCommandButtonState("New");
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void FillAdvanceDetails()
        {
            clsGetAdvanceListBizActionVO BizAction = new clsGetAdvanceListBizActionVO();

            BizAction.ID = 0;
            BizAction.PatientID = ((clsRefundVO)this.DataContext).PatientID;
            BizAction.PatientUnitID = ((clsRefundVO)this.DataContext).PatientUnitID;

            if (RefundMode == RefundModes.RefundCompanyAdvance)
            {
                BizAction.AllCompanies = true;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    clsAdvanceVO obj = new clsAdvanceVO();
                    dgAdvances.ItemsSource = null;
                    if (IsApprovalRequest)
                    {

                        foreach (clsAdvanceVO item in ((clsGetAdvanceListBizActionVO)arg.Result).Details)
                        {
                            if (item.ID == AdvanceID && item.UnitID == AdvanceUnitID)
                            {
                                item.SelectCharge = true;
                                dgAdvances.Columns[0].Visibility = Visibility.Visible;
                                dgAdvances.IsEnabled = false;
                                SetCommandButtonState("Approved");
                                obj = item;
                            }
                            else
                            {
                                item.SelectCharge = false;
                            }
                        }
                    }
                    dgAdvances.ItemsSource = ((clsGetAdvanceListBizActionVO)arg.Result).Details;
                    if (obj != null)
                    {
                        dgAdvances.SelectedItem = obj;
                    }
                    DisplaySummary(); // Added By CDS
                }

            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillRefundDetails()
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();

            clsGetRefundListBizActionVO BizAction = new clsGetRefundListBizActionVO();

            BizAction.ID = 0;
            BizAction.PatientID = ((clsRefundVO)this.DataContext).PatientID;
            BizAction.PatientUnitID = ((clsRefundVO)this.DataContext).PatientUnitID;

            if (RefundMode == RefundModes.RefundCompanyAdvance)
            {
                BizAction.AllCompanies = true;
            }

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }

            BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    dgRefund.ItemsSource = null;
                    dgRefund.ItemsSource = ((clsGetRefundListBizActionVO)arg.Result).Details;
                    DisplaySummary();
                }
                Indicatior.Close();
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void DisplaySummary()
        {
            double TotalAdavnce, TotalBalance, TotalUsed, TotalRefund;

            TotalAdavnce = TotalBalance = TotalUsed = TotalRefund = 0;

            if (dgAdvances.ItemsSource != null)
            {
                List<clsAdvanceVO> AdvList = new List<clsAdvanceVO>();
                AdvList = (List<clsAdvanceVO>)dgAdvances.ItemsSource;

                foreach (var item in AdvList)
                {
                    TotalAdavnce += item.Total;
                    TotalUsed += item.Used;
                    TotalBalance += item.Balance;

                }


            }

            txtTotalAdvance.Text = String.Format("{0:0.00}", TotalAdavnce);
            txtAdvanceConsumed.Text = String.Format("{0:0.00}", TotalUsed);

            if (dgRefund.ItemsSource != null)
            {
                List<clsRefundVO> RefList = new List<clsRefundVO>();
                RefList = (List<clsRefundVO>)dgRefund.ItemsSource;

                foreach (var item in RefList)
                {
                    TotalRefund += item.Amount;


                }


            }

            txtTotalRefund.Text = String.Format("{0:0.00}", TotalRefund);
            txtAdvanceAvailable.Text = String.Format("{0:0.00}", TotalBalance);


        }

        private bool CheckValidations()
        {
            txtRefundAmount.ClearValidationError();

            bool isFormValid = true;

            if ((clsAdvanceVO)dgAdvances.SelectedItem != null)
            {
                if (((clsAdvanceVO)dgAdvances.SelectedItem).AdvanceAgainst.Equals("Package") && ((clsAdvanceVO)dgAdvances.SelectedItem).IsPackageBillFreeze == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Package Bill is not Freezed, you cannot Refund the Advace.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    isFormValid = false;
                    txtRefundAmount.Text = (0).ToString();
                }
                else if (((clsAdvanceVO)dgAdvances.SelectedItem).Balance < Convert.ToDouble(txtRefundAmount.Text))
                {
                    txtRefundAmount.SetValidation("Amount should not be greater than Balance Amount");
                    txtRefundAmount.RaiseValidationError();
                    txtRefundAmount.Focus();
                    isFormValid = false;
                }
            }
            else if (txtRefundAmount.Text == "" || !(txtRefundAmount.Text.IsNumberValid()))
            {
                txtRefundAmount.SetValidation("Amount Required");
                txtRefundAmount.RaiseValidationError();
                txtRefundAmount.Focus();

                isFormValid = false;
            }
            else if (Convert.ToDouble(txtRefundAmount.Text) <= 0)
            {
                txtRefundAmount.SetValidation("Amount should be greater than zero");
                txtRefundAmount.RaiseValidationError();
                txtRefundAmount.Focus();

                isFormValid = false;
            }

            return isFormValid;

        }

        private bool CheckValidationsForSendApproval()
        {
            txtRefundAmount.ClearValidationError();

            bool isFormValid = true;

            if ((clsAdvanceVO)dgAdvances.SelectedItem != null)
            {
                if (txtRefundAmount.Text == "" || !(txtRefundAmount.Text.IsNumberValid()))
                {
                    txtRefundAmount.SetValidation("Amount Required");
                    txtRefundAmount.RaiseValidationError();
                    txtRefundAmount.Focus();

                    isFormValid = false;
                }
                else if (Convert.ToDouble(txtRefundAmount.Text) <= 0)
                {
                    txtRefundAmount.SetValidation("Amount should be greater than zero");
                    txtRefundAmount.RaiseValidationError();
                    txtRefundAmount.Focus();

                    isFormValid = false;
                }
                else if (((clsAdvanceVO)dgAdvances.SelectedItem).AdvanceAgainst.Equals("Package") && ((clsAdvanceVO)dgAdvances.SelectedItem).IsPackageBillFreeze == false)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Package Bill is not Freezed, you cannot Refund the Advace.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    isFormValid = false;
                    txtRefundAmount.Text = (0).ToString();
                }
                else if (((clsAdvanceVO)dgAdvances.SelectedItem).Balance < Convert.ToDouble(txtRefundAmount.Text))
                {
                    txtRefundAmount.SetValidation("Amount should not be greater than Balance Amount");
                    txtRefundAmount.RaiseValidationError();
                    txtRefundAmount.Focus();
                    isFormValid = false;
                }
            }
            else if (txtRefundAmount.Text == "" || !(txtRefundAmount.Text.IsNumberValid()))
            {
                txtRefundAmount.SetValidation("Amount Required");
                txtRefundAmount.RaiseValidationError();
                txtRefundAmount.Focus();

                isFormValid = false;
            }
            else if (Convert.ToDouble(txtRefundAmount.Text) <= 0)
            {
                txtRefundAmount.SetValidation("Amount should be greater than zero");
                txtRefundAmount.RaiseValidationError();
                txtRefundAmount.Focus();

                isFormValid = false;
            }

            return isFormValid;

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            bool isFormValid = CheckValidations();
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (dgAdvances.SelectedItem == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Advance.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    isFormValid = false;
                }
                else if (((clsAdvanceVO)dgAdvances.SelectedItem).Balance == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Balance for the selected Advance is zero.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    isFormValid = false;
                }
                else if (Convert.ToDouble(txtRefundAmount.Text) > ((clsAdvanceVO)dgAdvances.SelectedItem).Balance)
                {
                    txtRefundAmount.SetValidation("Amount must be less than the balance of selected Advance");
                    txtRefundAmount.RaiseValidationError();
                    txtRefundAmount.Focus();
                    isFormValid = false;
                }

                if (isFormValid)
                {
                    PaymentWindow paymentWin = new PaymentWindow();
                    paymentWin.TotalAmount = double.Parse(txtRefundAmount.Text);
                    paymentWin.Initiate("Refund");

                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                    paymentWin.Show();
                    ClickedFlag = 0;
                }
                else
                    ClickedFlag = 0;
            }
        }

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
                {
                    clsRefundVO advObj = (clsRefundVO)this.DataContext;
                    if (((PaymentWindow)sender).Payment != null)
                    {
                        advObj.PaymentDetails = ((PaymentWindow)sender).Payment;
                        // Added By CDS
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                            advObj.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Payment;
                        else
                            advObj.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                    }

                    advObj.Date = DateTime.Now;
                    if (!string.IsNullOrEmpty(txtRefundAmount.Text))
                        advObj.Amount = double.Parse(txtRefundAmount.Text);
                    advObj.AdvanceID = ((clsAdvanceVO)dgAdvances.SelectedItem).ID;
                    advObj.CompanyID = ((clsAdvanceVO)dgAdvances.SelectedItem).CompanyID;
                    advObj.Remarks = txtRemark.Text;
                    //Added By Bhushanp 01062017
                    advObj.ApprovalRequestID = ((clsAdvanceVO)dgAdvances.SelectedItem).ApprovalRequestID;
                    advObj.ApprovalRequestUnitID = ((clsAdvanceVO)dgAdvances.SelectedItem).ApprovalRequestUnitID;


                    clsAddRefundBizActionVO BizAction = new clsAddRefundBizActionVO();
                    BizAction.Details = advObj;

                    // Added By CDS
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                        BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Advance
                    else
                        BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag = 0;
                        if (arg.Error == null && arg.Result != null && ((clsAddRefundBizActionVO)arg.Result).Details != null)
                        {

                            if (((clsAddRefundBizActionVO)arg.Result).Details != null)
                            {
                                FillAdvanceDetails();
                                FillRefundDetails();
                                ClearFormData();
                                // System.Windows.Browser.HtmlPage.Window.Alert("Visit saved successfully.");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Refund details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                {
                                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                    if (res1 == MessageBoxResult.OK)
                                    {
                                        cmdSave.IsEnabled = false;
                                        try
                                        {
                                            string URL = "../Reports/OPD/Patient_AdvanceRefund.aspx?ID=" + ((clsAddRefundBizActionVO)arg.Result).Details.ID + "&UnitID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId + "&ReportID=" + 2 + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
                                            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                    ClickedFlag = 0;
                                };
                                msgW1.Show();
                            }


                        }
                        else
                        {
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Refund details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else
                    ClickedFlag = 0;
            }
            catch (Exception)
            {
                ClickedFlag = 0;
                //throw;
            }
            finally
            {

            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            // ClearFormData();
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Billing");
        }

        private void ClearFormData()
        {
            txtRefundAmount.Text = (0).ToString();
            txtRemark.Text = string.Empty;
        }

        private void btnAdvPrintRefund_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgRefund.SelectedItem != null)
                {

                    //long ID = ((clsGetRefundListBizActionVO)dgRefund.SelectedItem).ID;
                    //long PUnitID = ((clsGetRefundListBizActionVO)dgRefund.SelectedItem).UnitID;
                    //long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;


                    string URL = "../Reports/OPD/Patient_AdvanceRefund.aspx?ID=" + ((clsRefundVO)dgRefund.SelectedItem).ID + "&UnitID=" + ((clsRefundVO)dgRefund.SelectedItem).UnitID + "&ReportID=" + 2 + "&PrintFomatID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                    //                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Advance. ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    //msgW1.Show();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


            //try
            //{
            //    if (dgAdvances.SelectedItem != null)
            //    {
            //        long PatientID = ((clsAdvanceVO)dgAdvances.SelectedItem).PatientID;
            //        long PatientUnitID = ((clsAdvanceVO)dgAdvances.SelectedItem).PatientUnitID;
            //        long AdvanceID = ((clsAdvanceVO)dgAdvances.SelectedItem).ID;
            //        long AdvanceUnitID = ((clsAdvanceVO)dgAdvances.SelectedItem).UnitID;
            //        long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
            //        PrintAdvance(PatientID, PatientUnitID, AdvanceID, AdvanceUnitID, PRID);
            //    }
            //    else
            //    {
            //        MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Advance. ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgW1.Show();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        private void PrintAdvance(long PatientID, long PatientUnitID, long AdvanceID, long AdvanceUnitID, long PrintID)
        {
            if (AdvanceID > 0 || PatientID > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long PUnitID = PatientUnitID;
                long PID = PatientID;
                long AID = AdvanceID;
                long AUID = AdvanceUnitID;
                // ReportID =  1 for Advance Receipt          and                2 is for Advance Refund Receipt;.

                string URL = "../Reports/OPD/Patient_AdvanceRefund.aspx?PatientID=" + PID + "&PatientUnitID=" + PUnitID + "&AdvanceID=" + AID + "&AdvanceUnitID=" + AUID + "&ReportID=" + 2 + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        int ApproveFlag = 0;
        private void cmdApproved_Click(object sender, RoutedEventArgs e)
        {
            if (objUser.PatientAdvRefundAuthLvlID != null)
            {
                if ((clsAdvanceVO)dgAdvances.SelectedItem != null)
                {
                    if (CheckValidations())
                    {
                        ApproveFlag += 1;
                        if (ApproveFlag == 1)
                        {
                            if (objUser.PatientAdvRefundAuthLvlID == ((clsAdvanceVO)dgAdvances.SelectedItem).LevelID)
                            {
                                if (objUser.PatientAdvRefundAuthLvlID == 0)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please assign approval rights to the User", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                    msgWindowUpdate.Show();
                                    ApproveFlag = 0;
                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Advance Refund is already approved with Level" + objUser.PatientAdvRefundAuthLvlID + " rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                    msgWindowUpdate.Show();
                                    ApproveFlag = 0;
                                }
                            }
                            else if (objUser.PatientAdvRefundAuthLvlID < ((clsAdvanceVO)dgAdvances.SelectedItem).LevelID)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Advance Refund is already approved with higher rights ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgWindowUpdate.Show();
                                ApproveFlag = 0;
                            }
                            else if ((objUser.PatientAdvRefundAmmount == 0 ? true : objUser.PatientAdvRefundAmmount < Convert.ToDecimal(txtRefundAmount.Text)))
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Given Refund amount exceeds authorised amount", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgWindowUpdate.Show();
                                ApproveFlag = 0;
                            }
                            else
                            {
                                MessageBoxControl.MessageBoxChildWindow msgWindowUpdate =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Are you sure you want to Approve the request with Level " + objUser.PatientAdvRefundAuthLvlID + " rights", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                msgWindowUpdate.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageBoxClosed);

                                msgWindowUpdate.Show();
                            }
                        }
                    }
                }
            }
        }

        private void msgWindowUpdate_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                ApproveRequest((clsAdvanceVO)dgAdvances.SelectedItem);
            }
            else
            {
                ApproveFlag = 0;
            }
        }
        private void ApproveRequest(clsAdvanceVO objAdvance)
        {

            clsApproveSendRequestVO BizAction = new clsApproveSendRequestVO();
            try
            {
                BizAction.IsForApproval = true;
                BizAction.IsAdvanceRefund = true;
                BizAction.ApprovalRequestID = objAdvance.ApprovalRequestID;
                BizAction.ApprovalRequestUnitID = objAdvance.ApprovalRequestUnitID;
                BizAction.RefundAmount = Convert.ToDouble(txtRefundAmount.Text);
                BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
                if (objUser.PatientAdvRefundAuthLvlID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForPatientAdvRefundID)
                    BizAction.IsApproved = true;
                else
                    BizAction.IsApproved = false;
                BizAction.LevelID = objUser.PatientAdvRefundAuthLvlID;
                BizAction.ApprovalRemark = Convert.ToString(txtRemark.Text);
                BizAction.ApprovedDateTime = DateTime.Now;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (sa, args) =>
                {
                    if (args.Error == null)
                    {
                        if (objUser.PatientAdvRefundAuthLvlID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForPatientAdvRefundID)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageRequestApproved);

                            msgW.Show();
                            cmdApproved.IsEnabled = false;

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully, still higher level approval is required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.Show();
                            cmdApproved.IsEnabled = false;
                        }

                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Approving request.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    ApproveFlag = 0;
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                Indicatior.Close();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }

        // msgWindowUpdate_OnMessageRequestApproved
        private void msgWindowUpdate_OnMessageRequestApproved(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                ModuleName = "OPDModule";
                Action = "CIMS.Forms.frmAdvRefundApprovalRequests";
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }
        private void cmdSendApproval_Click(object sender, RoutedEventArgs e)
        {
            if ((clsAdvanceVO)dgAdvances.SelectedItem != null)
            {
                if (CheckValidationsForSendApproval())
                {
                    SendRequest(((clsAdvanceVO)dgAdvances.SelectedItem).ID, ((clsAdvanceVO)dgAdvances.SelectedItem).UnitID);
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Advance.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void SendRequest(long AdvID, long AdvUnitID)
        {

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsSendRequestForApprovalVO obj = new clsSendRequestForApprovalVO();
                obj.IsAdvanceRefund = true;
                obj.Details = new clsRefundVO();
                obj.Details.AdvanceID = AdvID;
                obj.Details.AdvanceUnitID = AdvUnitID;
                obj.RefundAmount = Convert.ToDouble(txtRefundAmount.Text);
                obj.Details.Remarks = Convert.ToString(txtRemark.Text);
                obj.Details.RequestTypeID = 3;
                obj.Details.RequestType = "AdvanceRefund";
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Send Successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWindowUpdate_OnMessageSendRequest);


                        msgW1.Show();
                        cmdSendApproval.IsEnabled = false;

                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        cmdSave.IsEnabled = true;
                    }
                };
                Client.ProcessAsync(obj, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                cmdSave.IsEnabled = true;
                string err = ex.Message;
                throw;
            }
            finally
            {
                Indicatior.Close();
            }
        }

        private void msgWindowUpdate_OnMessageSendRequest(MessageBoxResult result)
        {
            if (result == MessageBoxResult.OK)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            }
        }
        //private void ApproveRequest(long ApprovalRequestID, long ApprovalRequestUnitID)
        //{

        //    clsApproveSendRequestVO BizAction = new clsApproveSendRequestVO();
        //    Indicatior = new WaitIndicator();
        //    Indicatior.Show();
        //    try
        //    {                
        //        BizAction.IsForApproval = true;
        //        BizAction.ApprovalRequestID = ApprovalRequestID;
        //        BizAction.ApprovalRequestUnitID = ApprovalRequestUnitID;
        //        BizAction.ApprovedByID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
        //        BizAction.ApprovedByName = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.Name;
        //        if (objUser.IpdBillAuthLvlID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForConcessionID)
        //            BizAction.IsApproved = true;
        //        else
        //            BizAction.IsApproved = false;
        //        BizAction.LevelID = objUser.IpdBillAuthLvlID;
        //        BizAction.ApprovalRemark = txtRemark.Text;
        //        BizAction.ApprovedDateTime = DateTime.Now;                
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (sa, args) =>
        //        {
        //            if (args.Error == null)
        //            {
        //                if (objUser.IpdAuthLvl == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AuthorizationLevelForRefundID)
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow msgW =
        //                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully,you can freeze the bill and make a payment", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
        //                    msgW.Show();
        //                    cmdApproved.IsEnabled = false;
        //                }
        //                else
        //                {
        //                    MessageBoxControl.MessageBoxChildWindow msgW =
        //                                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Request Approved Successfully, still higher level approval is required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
        //                    msgW.Show();
        //                }

        //            }
        //            else
        //            {
        //                MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Approving request.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //                msgW1.Show();
        //            }
        //            //ApproveFlag = 0;
        //        };
        //        client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //        Indicatior.Close();
        //    }
        //    catch (Exception)
        //    {
        //        Indicatior.Close();
        //        throw;
        //    }
        //}


        clsUserRightsVO objUser = new clsUserRightsVO();
        public void GetUserRights(long UserId)
        {
            try
            {
                clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
                objBizVO.objUserRight = new clsUserRightsVO();
                objBizVO.objUserRight.UserID = UserId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;
                    }
                };
                client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void dgAdvances_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsAdvanceVO)dgAdvances.SelectedItem != null)
            {
                if (((clsAdvanceVO)dgAdvances.SelectedItem).IsSendForApproval && ((clsAdvanceVO)dgAdvances.SelectedItem).ApprovalStatus != true)
                {
                    txtRefundAmount.IsEnabled = true;
                    if (IsApprovalRequest)
                    {
                        SetCommandButtonState("Approved");
                        txtRefundAmount.Text = Convert.ToString(((clsAdvanceVO)dgAdvances.SelectedItem).Refund);
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Approval Request already Send", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        txtRefundAmount.Text = Convert.ToString(0);
                    }
                }
                else if (((clsAdvanceVO)dgAdvances.SelectedItem).IsRefund || !((clsAdvanceVO)dgAdvances.SelectedItem).IsSendForApproval)
                {
                    txtRefundAmount.Text = "0";
                    txtRefundAmount.IsEnabled = true;
                    SetCommandButtonState("SendApproval");
                }
                else
                {
                    txtRefundAmount.IsEnabled = false;
                    SetCommandButtonState("Save");
                    txtRefundAmount.Text = Convert.ToString(((clsAdvanceVO)dgAdvances.SelectedItem).Refund);
                }
            }
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Approved":
                    cmdApproved.IsEnabled = true;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdSendApproval.IsEnabled = false;
                    break;

                case "SendApproval":
                    cmdApproved.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdSendApproval.IsEnabled = true;
                    break;

                case "Save":
                    cmdApproved.IsEnabled = false;
                    cmdSave.IsEnabled = true;
                    cmdCancel.IsEnabled = true;
                    cmdSendApproval.IsEnabled = false;
                    break;


                case "New":
                    cmdApproved.IsEnabled = false;
                    cmdSave.IsEnabled = false;
                    cmdCancel.IsEnabled = true;
                    cmdSendApproval.IsEnabled = false;
                    break;

                default:
                    break;
            }
        }

        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                ((IApplicationConfiguration)App.Current).SelectedPatient = null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
