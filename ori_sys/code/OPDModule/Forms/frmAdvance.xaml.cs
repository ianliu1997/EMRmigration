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
using PalashDynamics.ValueObjects;
using System.Windows.Data;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using OPDModule;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Text;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;

namespace CIMS.Forms
{

    public partial class frmAdvance : UserControl, IInitiateCIMS
    {
        int ClickedFlag = 0;
        #region IInitiateCIMS Members
        long PatientID = 0;
        bool IsPatientExist = true;
        clsAdvanceVO.AdvanceType mAdvanceType = clsAdvanceVO.AdvanceType.Select;

        // Added By CDS
        public clsPatientGeneralVO setpatient = null;
        public bool IsFromRegisterandVisit = false;
        //END

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();

            switch (Mode.ToUpper())
            {
                case "PA":
                    mAdvanceType = clsAdvanceVO.AdvanceType.Patient;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        IsPatientExist = false;
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        return;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        IsPatientExist = false;
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        return;
                    }



                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    // Added By CDS
                    if (IsFromRegisterandVisit == true)
                    {
                        cmdSaveandBill.Visibility = Visibility.Visible;
                        //((IApplicationConfiguration)App.Current).SelectedPatient = setpatient;

                        //mElement = (TextBlock)rootPage.FindName("Patient Detail");
                        //mElement.Text = "Patient Detail";
                    }
                    // END

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsAdvanceVO()
                    {
                        PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer,
                        Date = DateTime.Now
                    };



                    break;

                case "CA":

                    // Added By CDS
                    if (IsFromRegisterandVisit == true)
                    {
                        cmdSaveandBill.Visibility = Visibility.Visible;
                        //((IApplicationConfiguration)App.Current).SelectedPatient = setpatient;
                    }
                    // END
                    mAdvanceType = clsAdvanceVO.AdvanceType.Company;

                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                    mElement1.Text = "";


                    this.DataContext = new clsAdvanceVO()
                    {

                        Date = DateTime.Now,
                        LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer,
                        AdvanceTypeId = (long)mAdvanceType
                    };



                    break;
                default:
                    break;
            }

        }

        #endregion



        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        string PanNumber = string.Empty;
        private const string ATSelected = "ATSE LECTED";
        private const string None = "NONE";

        public decimal PackageRate { get; set; } //***//

        public frmAdvance()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(frmAdvance_Loaded);


        }

        //Added By Yogita
        private void clearUI()
        {
            cmbAdvanceType.SelectedValue = "";
            cmbAdvanceAgainst.SelectedValue = "";
            cmbCompany.SelectedValue = "";
            txtAmount.Text = "";
            txtRemark.Text = "";
        }

        private void frmAdvance_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mAdvanceType == clsAdvanceVO.AdvanceType.Patient && PatientID <= 0)
                {
                    cmdSave.IsEnabled = false;
                    if (IsPatientExist == false)
                    {
                        // ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");

                        //adeed by neena for surrogacy
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                            ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
                        else
                            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

                    }
                }
                else if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSpouse == true) //***//
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Donor Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                        ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy");
                    else
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
                        FillAdvanceAgainstList();
                        FillAdvanceType();
                        FillCompanyList();

                        FillAdvanceDetails();

                        txtAmount.SetValidation("Amount must be greater than zero");
                        txtAmount.RaiseValidationError();

                        cmbAdvanceAgainst.TextBox.SetValidation("Select Advance Against");
                        cmbAdvanceAgainst.TextBox.RaiseValidationError();

                        if (mAdvanceType != clsAdvanceVO.AdvanceType.Company)
                        {
                            cmbAdvanceType.TextBox.SetValidation("Select Advance Type");
                            cmbAdvanceType.TextBox.RaiseValidationError();
                            cmbAdvanceType.Focus();
                        }
                        else
                            cmbCompany.IsEnabled = true;
                        //Added By Bhushanp for Change Package Flow
                        FetchData(((IApplicationConfiguration)App.Current).SelectedPatient.TariffID);
                        FillBillSearchList();
                    }
                }

                UserControl rootPage = Application.Current.RootVisual as UserControl;
                TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                    " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                    ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

            }
            catch (Exception)
            {

                //
            }
        }

        #region FillAdvanceAgainst
        void FillAdvanceAgainstList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_AdvanceAgainst;
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
                    cmbAdvanceAgainst.ItemsSource = null;
                    cmbAdvanceAgainst.ItemsSource = objList;
                    cmbAdvanceAgainst.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region FillCompany
        private void FillCompanyList()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
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

                    foreach (var item in objList.ToList())
                    {
                        if (item.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID)
                        {
                            objList.Remove(item);
                        }
                    }

                    cmbCompany.ItemsSource = null;
                    cmbCompany.ItemsSource = objList;


                    cmbCompany.SelectedItem = objList[0];
                }


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        #endregion

        #region FillAdvanceType

        private void FillAdvanceType()
        {
            List<MasterListItem> objAdvanceType = new List<MasterListItem>();
            MasterListItem Default;
            Default = new MasterListItem();
            Default.ID = 0;
            Default.Description = "- Select -";
            objAdvanceType.Add(Default);

            if (mAdvanceType == clsAdvanceVO.AdvanceType.Company)
            {
                Default = new MasterListItem();
                Default.ID = 1;
                Default.Description = "Company";
                objAdvanceType.Add(Default);
            }
            Default = new MasterListItem();
            Default.ID = 2;
            Default.Description = "Patient";
            objAdvanceType.Add(Default);
            Default = new MasterListItem();
            Default.ID = 3;
            Default.Description = "Patient-Company";
            objAdvanceType.Add(Default);
            cmbAdvanceType.ItemsSource = objAdvanceType;

            if (mAdvanceType == clsAdvanceVO.AdvanceType.Patient)
                cmbAdvanceType.SelectedValue = (long)0;
            else
            {
                cmbAdvanceType.SelectedValue = (long)1;
                cmbAdvanceType.IsEnabled = false;
            }


        }

        #endregion
        decimal PrePatientDailyCashAmount = 0;
        private void FillAdvanceDetails()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetAdvanceListBizActionVO BizAction = new clsGetAdvanceListBizActionVO();

                BizAction.ID = 0;
                BizAction.PatientUnitID = ((clsAdvanceVO)this.DataContext).PatientUnitID;
                BizAction.PatientID = ((clsAdvanceVO)this.DataContext).PatientID;


                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                }
                else
                {
                    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                }

                if (mAdvanceType == clsAdvanceVO.AdvanceType.Company)
                {
                    BizAction.AllCompanies = true;
                }

                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
                BizAction.PatientDailyCashAmount = 0;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {

                    if (arg.Error == null && arg.Result != null)
                    {

                        dgAdvances.ItemsSource = null;
                        dgAdvances.ItemsSource = ((clsGetAdvanceListBizActionVO)arg.Result).Details;
                        PrePatientDailyCashAmount = ((clsGetAdvanceListBizActionVO)arg.Result).PatientDailyCashAmount;
                        FillAdvanceType();
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

        private void cmdSaveandBill_Click(object sender, RoutedEventArgs e)
        {
            FormSaveTypes = SaveType.Billing;
            cmdSave_Click(sender, e);
        }

        private SaveType FormSaveTypes = SaveType.Advanced;
        private enum SaveType
        {
            Advanced = 0,
            Billing = 1,
            PackageBilling = 2
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {

            string msgTitle = "";
            string msgText = "Advance limit exceeds are you sure you want to continue?";


            if (Convert.ToDouble(PackageRate) > 0 && (Convert.ToDouble(txtAmount.Text) > Convert.ToDouble(PackageRate)))
            {
                if (Convert.ToDouble(txtAmount.Text) > Convert.ToDouble(PackageRate))
                {
                    MessageBoxControl.MessageBoxChildWindow msgbox =
                new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(Msgbox_OnMessageBoxClosed);
                    msgbox.Show();

                }

            }

            else if ((dgAdvances.SelectedItem) != null && Convert.ToDouble(txtAmount.Text) > (dgAdvances.SelectedItem as clsAdvanceVO).PackageAdvanceBalance)
            {
                MessageBoxControl.MessageBoxChildWindow msgbox =
            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(Msgbox_OnMessageBoxClosed);
                msgbox.Show();
            }

            else
            {
                bool isFormValid = true;
                ClickedFlag += 1;
                PanNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.PanNumber;
                try
                {
                    if (ClickedFlag == 1)
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


                        MasterListItem objAdvanceAgains = cmbAdvanceAgainst.SelectedItem as MasterListItem;
                        if (objAdvanceAgains.ID <= 0)
                        {
                            cmbAdvanceAgainst.TextBox.SetValidation("Select Advance Against");
                            cmbAdvanceAgainst.TextBox.RaiseValidationError();
                            cmbAdvanceAgainst.Focus();
                            isFormValid = false;
                        }
                        else
                            cmbAdvanceAgainst.TextBox.ClearValidationError();

                        MasterListItem objAdvType = cmbAdvanceType.SelectedItem as MasterListItem;
                        if (objAdvType.ID <= 0)
                        {
                            cmbAdvanceType.TextBox.SetValidation("Select Advance Type");
                            cmbAdvanceType.TextBox.RaiseValidationError();
                            cmbAdvanceType.Focus();
                            isFormValid = false;
                        }
                        else
                            cmbAdvanceType.TextBox.ClearValidationError();

                        if (cmbCompany.IsEnabled == true)
                        {
                            MasterListItem objComp = cmbCompany.SelectedItem as MasterListItem;
                            if (objComp == null)
                            {
                                cmbCompany.TextBox.SetValidation("Select the Company");
                                cmbCompany.TextBox.RaiseValidationError();
                                cmbCompany.Focus();
                                isFormValid = false;
                            }
                            else if (objComp.ID <= 0)
                            {
                                cmbCompany.TextBox.SetValidation("Select the Company");
                                cmbCompany.TextBox.RaiseValidationError();
                                cmbCompany.Focus();
                                isFormValid = false;
                            }
                            else
                                cmbCompany.TextBox.ClearValidationError();
                        }

                        if (cmbAdvanceAgainst.SelectedItem != null)
                        {
                            if (((MasterListItem)cmbAdvanceAgainst.SelectedItem).ID == ((IApplicationConfiguration)App.Current).CurrentUser.PackageID)
                            {
                                if (cmbPackage.SelectedItem == null)
                                {
                                    cmbPackage.TextBox.SetValidation("Add Package");
                                    cmbPackage.TextBox.RaiseValidationError();
                                    cmbPackage.Focus();
                                    isFormValid = false;
                                }
                                else if (((MasterListItem)cmbPackage.SelectedItem).ID == 0)
                                {
                                    cmbPackage.TextBox.SetValidation("Add Package");
                                    cmbPackage.TextBox.RaiseValidationError();
                                    cmbPackage.Focus();
                                    isFormValid = false;
                                }
                                else
                                    cmbPackage.TextBox.ClearValidationError();
                            }
                        }
                        if (isFormValid)
                        {
                            PaymentWindow paymentWin = new PaymentWindow();
                            paymentWin.TotalAmount = Convert.ToDouble(txtAmount.Text);
                            paymentWin.Initiate("Advance");
                            if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                            {
                                paymentWin.txtPanNo.Text = PanNumber;
                                paymentWin.txtPanNo.IsEnabled = false;
                            }
                            paymentWin.PrePatientDailyCashAmount = this.PrePatientDailyCashAmount;
                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.Show();
                            ClickedFlag = 0;
                        }
                        else
                            ClickedFlag = 0;
                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    ClickedFlag = 0;
                }

            }

        }

        void Msgbox_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                bool isFormValid = true;
                ClickedFlag += 1;
                PanNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.PanNumber;
                try
                {
                    if (ClickedFlag == 1)
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


                        MasterListItem objAdvanceAgains = cmbAdvanceAgainst.SelectedItem as MasterListItem;
                        if (objAdvanceAgains.ID <= 0)
                        {
                            cmbAdvanceAgainst.TextBox.SetValidation("Select Advance Against");
                            cmbAdvanceAgainst.TextBox.RaiseValidationError();
                            cmbAdvanceAgainst.Focus();
                            isFormValid = false;
                        }
                        else
                            cmbAdvanceAgainst.TextBox.ClearValidationError();

                        MasterListItem objAdvType = cmbAdvanceType.SelectedItem as MasterListItem;
                        if (objAdvType.ID <= 0)
                        {
                            cmbAdvanceType.TextBox.SetValidation("Select Advance Type");
                            cmbAdvanceType.TextBox.RaiseValidationError();
                            cmbAdvanceType.Focus();
                            isFormValid = false;
                        }
                        else
                            cmbAdvanceType.TextBox.ClearValidationError();

                        if (cmbCompany.IsEnabled == true)
                        {
                            MasterListItem objComp = cmbCompany.SelectedItem as MasterListItem;
                            if (objComp == null)
                            {
                                cmbCompany.TextBox.SetValidation("Select the Company");
                                cmbCompany.TextBox.RaiseValidationError();
                                cmbCompany.Focus();
                                isFormValid = false;
                            }
                            else if (objComp.ID <= 0)
                            {
                                cmbCompany.TextBox.SetValidation("Select the Company");
                                cmbCompany.TextBox.RaiseValidationError();
                                cmbCompany.Focus();
                                isFormValid = false;
                            }
                            else
                                cmbCompany.TextBox.ClearValidationError();
                        }

                        if (cmbAdvanceAgainst.SelectedItem != null)
                        {
                            if (((MasterListItem)cmbAdvanceAgainst.SelectedItem).ID == ((IApplicationConfiguration)App.Current).CurrentUser.PackageID)
                            {
                                if (cmbPackage.SelectedItem == null)
                                {
                                    cmbPackage.TextBox.SetValidation("Add Package");
                                    cmbPackage.TextBox.RaiseValidationError();
                                    cmbPackage.Focus();
                                    isFormValid = false;
                                }
                                else if (((MasterListItem)cmbPackage.SelectedItem).ID == 0)
                                {
                                    cmbPackage.TextBox.SetValidation("Add Package");
                                    cmbPackage.TextBox.RaiseValidationError();
                                    cmbPackage.Focus();
                                    isFormValid = false;
                                }
                                else
                                    cmbPackage.TextBox.ClearValidationError();
                            }
                        }
                        if (isFormValid)
                        {
                            PaymentWindow paymentWin = new PaymentWindow();
                            paymentWin.TotalAmount = Convert.ToDouble(txtAmount.Text);
                            paymentWin.Initiate("Advance");
                            if (!String.IsNullOrEmpty(PanNumber) && PanNumber != "NULL")
                            {
                                paymentWin.txtPanNo.Text = PanNumber;
                                paymentWin.txtPanNo.IsEnabled = false;
                            }
                            paymentWin.PrePatientDailyCashAmount = this.PrePatientDailyCashAmount;
                            paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);
                            paymentWin.Show();
                            ClickedFlag = 0;
                        }
                        else
                            ClickedFlag = 0;
                    }
                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    ClickedFlag = 0;
                }
            }
        }
        

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            if (((CIMS.Forms.PaymentWindow)sender).DialogResult == true)
            {
                clsAdvanceVO advObj = (clsAdvanceVO)this.DataContext;
                if (((PaymentWindow)sender).Payment != null)
                {
                    advObj.PaymentDetails = ((PaymentWindow)sender).Payment;
                    // Added By CDS
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                        advObj.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Payment;
                    else
                        advObj.PaymentDetails.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

                }

                if (cmbAdvanceType.SelectedItem != null)
                    advObj.AdvanceTypeId = ((MasterListItem)cmbAdvanceType.SelectedItem).ID;
                if (cmbAdvanceAgainst.SelectedItem != null)
                    advObj.AdvanceAgainstId = ((MasterListItem)cmbAdvanceAgainst.SelectedItem).ID;

                if (((PaymentWindow)sender).Payment != null)
                    advObj.Total = ((PaymentWindow)sender).Payment.PaidAmount;
                if (cmbCompany.SelectedItem != null)
                    advObj.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;
                advObj.Remarks = txtRemark.Text;
                // Added By Bhushanp For New Package Flow 
                if (cmbPackage.SelectedItem != null)
                {
                    advObj.PackageID = ((MasterListItem)cmbPackage.SelectedItem).ID;
                    if (dgAdvances.SelectedItem != null)
                    {
                        advObj.PackageBillID = (dgAdvances.SelectedItem as clsAdvanceVO).PackageBillID;
                        advObj.PackageBillUnitID = (dgAdvances.SelectedItem as clsAdvanceVO).PackageBillUnitID;
                    }
                }
                else
                {
                    advObj.PackageID = 0;
                }

                clsAddAdvanceBizActionVO BizAction = new clsAddAdvanceBizActionVO();
                BizAction.Details = advObj;

                // Added By CDS
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                    BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Advance
                else
                    BizAction.Details.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

                if (FormSaveTypes != SaveType.PackageBilling)   //added on 16082018 For Package Advance & Bill Save in transaction
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        ClickedFlag = 0;
                        if (arg.Error == null && arg.Result != null && ((clsAddAdvanceBizActionVO)arg.Result).Details != null)
                        {

                            if (((clsAddAdvanceBizActionVO)arg.Result).Details != null)
                            {
                                FillAdvanceDetails();
                                // System.Windows.Browser.HtmlPage.Window.Alert("Visit saved successfully.");
                                string strMsg = string.Empty;
                                if (mAdvanceType == clsAdvanceVO.AdvanceType.Patient) strMsg = "Patient";
                                else if (mAdvanceType == clsAdvanceVO.AdvanceType.Company) strMsg = "Company";
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg + " Advance details saved successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.OnMessageBoxClosed += (MessageBoxResult res1) =>
                                {
                                    if (res1 == MessageBoxResult.OK)
                                    {
                                        try
                                        {
                                            long PatientID = ((clsAddAdvanceBizActionVO)arg.Result).Details.PatientID;
                                            long PatientUnitID = ((clsAddAdvanceBizActionVO)arg.Result).Details.PatientUnitID;
                                            long AdvanceID = ((clsAddAdvanceBizActionVO)arg.Result).Details.ID;
                                            long AdvanceUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                            long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;

                                            PrintAdvance(PatientID, PatientUnitID, AdvanceID, AdvanceUnitID, PRID);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw ex;
                                        }
                                    }
                                    ClickedFlag = 0;
                                };
                                if (FormSaveTypes != SaveType.PackageBilling)
                                {
                                    msgW1.Show();
                                }

                                ClearFormData();

                                //Added By CDS 

                                if (FormSaveTypes == SaveType.Billing)
                                {
                                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmBill") as UIElement;
                                    frmBill win = new frmBill();
                                    win.IsFromSaveAndBill = true;

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                    mElement.Text = "Bill Details";
                                    ((IInitiateCIMS)win).Initiate("VISIT");
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);


                                }   //commented on 20082018 For Package Advance & Bill Save in transaction
                                //else if (FormSaveTypes == SaveType.PackageBilling) //Added By Bhushanp For Package Flow 030820174
                                //{
                                //    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmBill") as UIElement;
                                //    frmBill win = new frmBill();
                                //    win.IsFromSaveAndPackageBill = true;
                                //    win.lServices = lServices;
                                //    win._TariffID = ServiceTariffID;
                                //    win.AdvanceID = ((clsAddAdvanceBizActionVO)arg.Result).Details.ID;
                                //    win.AdvanceUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                                //    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                //    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                //    mElement.Text = "Bill Details";
                                //    ((IInitiateCIMS)win).Initiate("VISIT");
                                //    ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                //}
                            }
                        }
                        else
                        {
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Advance details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
                else if (FormSaveTypes == SaveType.PackageBilling) //added on 16082018 For Package Advance & Bill Save in transaction
                {
                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmBill") as UIElement;
                    frmBill win = new frmBill();
                    win.IsFromSaveAndPackageBill = true;
                    win.lServices = lServices;
                    win._TariffID = ServiceTariffID;
                    //win.AdvanceID = ((clsAddAdvanceBizActionVO)arg.Result).Details.ID;
                    //win.AdvanceUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                    win.AdvanceBizActionVO = BizAction.DeepCopy();
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                    mElement.Text = "Bill Details";
                    ((IInitiateCIMS)win).Initiate("VISIT");
                    ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                }
            }
            else
                ClickedFlag = 0;
        }

        #region Common Functions


        private void ClearFormData()
        {
            //tabPatientSearch.IsSelected = true;
            txtAmount.Text = txtRemark.Text = string.Empty;
            cmbAdvanceAgainst.SelectedValue = (long)0;
            cmbAdvanceType.SelectedValue = (long)0;
        }

        #endregion

        private void cmbAdvanceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MasterListItem Default = new MasterListItem();
            Default = cmbAdvanceType.SelectedItem as MasterListItem;

            // dgAdvances.DataContext = null;

            cmbCompany.SelectedValue = (long)0;
            if ((Default.ID == Convert.ToInt64(clsAdvanceVO.AdvanceType.Patient)) || (Default.ID == Convert.ToInt64(0)))
            {
                cmbCompany.IsEnabled = false;
                //ObjPatientBillingInfo.SetSearchBtnState = true;
                //ObjPatientBillingInfo.SetMrNoState = true;
            }
            else if (Default.ID == Convert.ToInt64(clsAdvanceVO.AdvanceType.Company))
            {
                cmbCompany.IsEnabled = true;
                //ObjPatientBillingInfo.SetSearchBtnState = false;
                //ObjPatientBillingInfo.SetMrNoState = false;
            }
            else if (Default.ID == Convert.ToInt64(clsAdvanceVO.AdvanceType.PatientCompany))
            {
                cmbCompany.IsEnabled = false;
                mAdvanceType = clsAdvanceVO.AdvanceType.PatientCompany;

                if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                {
                    //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Company Sponsor not assigned to the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    cmbAdvanceType.SelectedValue = (long)0;
                }
                else
                    cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;


                //ObjPatientBillingInfo.SetSearchBtnState = true;
                //ObjPatientBillingInfo.SetMrNoState = true;
            }
            else
            {
                //ObjPatientBillingInfo.SetSearchBtnState = false;
                //ObjPatientBillingInfo.SetMrNoState = false;
            }
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MasterListItem Default = new MasterListItem();
            Default = cmbCompany.SelectedItem as MasterListItem;
            if (Default.ID > 0)
            {
                txtAmount.Text = txtRemark.Text = string.Empty;
                cmbAdvanceAgainst.SelectedValue = (long)0;
                //tabAdvancesDtl.IsEnabled = true;
                //SetCommandButtonState(ATSelected);
                // FillPatientCompanyAdvanceDetails();
            }
        }


        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //ClearFormData();
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //added by neena for surrogacy
            if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
            else
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");

            // ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Billing");


        }

        private void btnAdvPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgAdvances.SelectedItem != null)
                {
                    long PatientID = ((clsAdvanceVO)dgAdvances.SelectedItem).PatientID;
                    long PatientUnitID = ((clsAdvanceVO)dgAdvances.SelectedItem).PatientUnitID;
                    long AdvanceID = ((clsAdvanceVO)dgAdvances.SelectedItem).ID;
                    long AdvanceUnitID = ((clsAdvanceVO)dgAdvances.SelectedItem).UnitID;
                    long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;

                    PrintAdvance(PatientID, PatientUnitID, AdvanceID, AdvanceUnitID, PRID);
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Advance. ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
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

                string URL = "../Reports/OPD/Patient_AdvanceRefund.aspx?PatientID=" + PID + "&PatientUnitID=" + PUnitID + "&AdvanceID=" + AID + "&AdvanceUnitID=" + AUID + "&ReportID=" + 1 + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void cmdSaveandPackageBill_Click(object sender, RoutedEventArgs e)
        {
            FormSaveTypes = SaveType.PackageBilling;
            cmdSave_Click(sender, e);
        }
        #region For New Package Change By BhushanP
        private void lnkAddService_Click(object sender, RoutedEventArgs e)
        {
            GetPatientCurrentVisit();
        }

        public List<clsUnavailableItemStockServiceId> UnavailableItemStockService { get; set; }

        public class clsUnavailableItemStockServiceId
        {
            public long ServiceID { get; set; }
        }
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
        List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
        public long ServiceTariffID;
        public long PackageID = 0;
        public bool IsPackageFreezed = false;

        private bool CheckNewPackageAddValidation(long PackageID)
        {
            List<clsAdvanceVO> AdvanceList = new List<clsAdvanceVO>();
            AdvanceList = dgAdvances.ItemsSource == null ? new List<clsAdvanceVO>().ToList() : (dgAdvances.ItemsSource as List<clsAdvanceVO>).Where(z => z.AdvanceAgainst == "Package").ToList();
            bool UnFreezePackgeBill = false;
            foreach (var item in AdvanceList)
            {
                if (PackageID == item.PackageID && item.IsPackageBillFreeze == false)
                {
                    UnFreezePackgeBill = true;
                    return UnFreezePackgeBill;
                }
            }
            return UnFreezePackgeBill;
        }

        void serviceSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ServiceSearchForPackage)sender).DialogResult == true) //if (((ServiceSearch)sender).DialogResult == true)
            {
                lServices = ((ServiceSearchForPackage)sender).SelectedOtherServices.ToList();
                long PackgaeID = 0;
                foreach (var item in lServices)
                {
                    PackgaeID = item.PackageID; //item.ID;
                    PackageRate = item.Rate; //***//

                }
                if (!CheckNewPackageAddValidation(PackgaeID))  // added on 13082018 for new Package Adding.. 
                {
                    List<clsSpecialSpecializationVO> SpecializationList = new List<clsSpecialSpecializationVO>();
                    //lServices = ((ServiceSearchForPackage)sender).SelectedOtherServices.ToList();
                    bool IsMultiSponser = false;
                    ChargeList = new ObservableCollection<clsChargeVO>();
                    var item5 = from p in ChargeList
                                where p.IsDefaultService == true
                                select p;
                    if (item5.ToList().Count != ChargeList.Count)
                    {
                        foreach (var item in lServices)
                        {
                            var item1 = from r in ChargeList
                                        where r.CompanyID == item.CompanyID && r.PatientSourceID == item.PatientSourceID && r.TariffId == item.TariffID && r.IsDefaultService == false
                                        select r;

                            if (item1.ToList().Count > 0)
                            {
                                IsMultiSponser = false;
                            }
                            else
                            {
                                IsMultiSponser = true;
                            }
                            break;
                        }
                    }
                    //Added By Bhushanp 03082017 FOR New Package Changes
                    var items1 = from r in ChargeList
                                 where r.isPackageService == true
                                 select r;

                    var items2 = from r in ChargeList
                                 where r.isPackageService == false && r.PackageID == 0
                                 select r;


                    var items3 = from r in ChargeList
                                 where r.isPackageService == false && r.PackageID > 0
                                 select r;

                    var items4 = from r in lServices
                                 where r.IsPackage == false && r.PackageID == 0
                                 select r;


                    var items5 = from r in lServices
                                 where r.IsPackage == true
                                 select r;

                    var items6 = from r in lServices
                                 where r.IsPackage == false && r.PackageID > 0
                                 select r;

                    string MSG = "";
                    bool IsServiceCheck = false;
                    if (items1.ToList().Count > 0)
                    {
                        IsServiceCheck = true;
                        MSG = "Package is Already Exists You can't add the service or Package.";
                    }
                    else if (items5.ToList().Count > 1)
                    {
                        IsServiceCheck = true;
                        MSG = "Package is Already Exists You can't add the service or Package.";
                    }
                    else if (items2.ToList().Count > 0 && (items5.ToList().Count > 0 || items6.ToList().Count > 0))
                    {
                        IsServiceCheck = true;
                        MSG = "You cannot add package or package consumption with service.";
                    }
                    else if (items3.ToList().Count > 0 && (items4.ToList().Count > 0 || items5.ToList().Count > 0))
                    {
                        IsServiceCheck = true;
                        MSG = "You cannot add package or srevices with package consumption.";
                    }
                    else if (items4.ToList().Count > 0 && items5.ToList().Count > 0 || items4.ToList().Count > 0 && items6.ToList().Count > 0 || items5.ToList().Count > 0 && items6.ToList().Count > 0)
                    {
                        IsServiceCheck = true;
                        MSG = "You cannot add package, package consumption, and services in same Bill.";
                    }
                    ///////---------------------------------------------------------------------
                    if (IsMultiSponser == false && IsServiceCheck == false)
                    {
                        lServices = ((ServiceSearchForPackage)sender).SelectedOtherServices.ToList();
                        #region clsApplyPackageDiscountRateOnServiceBizActionVO
                        foreach (var item in lServices)
                        {
                            cmbPackage.SelectedValue = item.PackageID; //item.ID;
                        }
                        clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                        objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                        objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();

                        objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                        objApplyNewRate.ipVisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                        objApplyNewRate.IsIPD = false;

                        objApplyNewRate.ipPatientGenderID = ((IApplicationConfiguration)App.Current).SelectedPatient.GenderID;

                        if (((IApplicationConfiguration)App.Current).SelectedPatient.IsAge == true)
                        {
                            objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge;
                        }
                        else
                        {
                            objApplyNewRate.ipPatientDateOfBirth = ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth;
                        }

                        objApplyNewRate.ipServiceList = lServices;


                        if ((ServiceSearchForPackage)sender != null && ((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                            objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                        objApplyNewRate.MemberRelationID = 2;

                        objApplyNewRate.SponsorID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorID;
                        objApplyNewRate.SponsorUnitID = ((MasterListItem)((ServiceSearchForPackage)sender).cmbTariff.SelectedItem).SponsorUnitID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;
                                UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();
                                StringBuilder sbServiceName = new StringBuilder();
                                foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                                {
                                    clsServiceMasterVO obj = new clsServiceMasterVO();
                                    obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);
                                    sbServiceName.Append(obj.ServiceName + ",");
                                    lServices.Remove(obj);
                                }
                                ServiceTariffID = ((ServiceSearchForPackage)sender).ServiceTariffID;
                                if (sbServiceName.Length > 0)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgWD =
                                                    new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    msgWD.OnMessageBoxClosed += (re) =>
                                    {
                                        if (re == MessageBoxResult.OK)
                                        {

                                        }
                                    };
                                    msgWD.Show();
                                }
                            }
                        };
                        client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();

                        #endregion
                    }
                    else if (IsServiceCheck == true)
                    {
                        lServices.Clear();
                        FetchData(0);
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", MSG.ToString(), MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW2 =
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Multiple Sponsor Billing is not Allowed", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW2.Show();
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Please freeze the previous package bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
        }
        private void FetchData(long pTariffID)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
                BizAction.ServiceList = new List<clsServiceMasterVO>();
                BizAction.ServiceName = "";
                BizAction.Specialization = 0;
                BizAction.SubSpecialization = 0;
                BizAction.PatientSourceType = 0;
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                BizAction.TariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                BizAction.ClassID = ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID;
                BizAction.UsePackageSubsql = true;  //used to set @subSqlQuerry in CIMS_GetTariffServiceListNew
                BizAction.IsPackage = true;
                BizAction.SponsorID = 0;
                BizAction.SponsorUnitID = 0;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.IsPagingEnabled = false;
                BizAction.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID; // 
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null)
                    {
                        clsGetTariffServiceListBizActionVO result = arg.Result as clsGetTariffServiceListBizActionVO;
                        if (result.ServiceList != null)
                        {
                            List<MasterListItem> ObjList = new List<MasterListItem>();
                            ObjList.Add(new MasterListItem(0, "- Select -"));
                            foreach (var item in result.ServiceList)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.PackageID;//item.ID;
                                obj.Description = item.ServiceName;
                                ObjList.Add(obj);
                            }
                            cmbPackage.ItemsSource = null;
                            cmbPackage.ItemsSource = ObjList;
                            cmbPackage.SelectedItem = ObjList[0];
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }
        #endregion

        private void cmbAdvanceAgainst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbAdvanceAgainst.SelectedItem != null)
            {
                if (((MasterListItem)cmbAdvanceAgainst.SelectedItem).ID == ((IApplicationConfiguration)App.Current).CurrentUser.PackageID)
                {
                    lblPackage.Visibility = Visibility.Visible;
                    cmbPackage.Visibility = Visibility.Visible;
                    lnkAddService.Visibility = Visibility.Visible;
                    cmdSaveandPackageBill.Visibility = Visibility.Visible;
                    cmdSave.Visibility = Visibility.Collapsed;
                    cmdSaveandBill.IsEnabled = false;
                }
                else
                {
                    lblPackage.Visibility = Visibility.Collapsed;
                    cmbPackage.Visibility = Visibility.Collapsed;
                    lnkAddService.Visibility = Visibility.Collapsed;
                    cmdSaveandPackageBill.Visibility = Visibility.Collapsed;
                    cmdSave.Visibility = Visibility.Visible;
                    cmdSaveandBill.IsEnabled = true;
                }
            }
        }

        private void FillBillSearchList()
        {
            clsGetBillSearchListBizActionVO BizAction = new clsGetBillSearchListBizActionVO();
            BizAction.RequestTypeID = 1;
            BizAction.IsRequest = true;
            BizAction.FromDate = null;
            BizAction.ToDate = null;
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;
            else
                BizAction.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;

            BizAction.IsPagingEnabled = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetBillSearchListBizActionVO result = e.Result as clsGetBillSearchListBizActionVO;
                    if (result.List != null)
                    {
                        var items4 = from r in result.List
                                     where r.IsPackageServiceInclude == true && r.IsFreezed == false
                                     select r;
                        if (items4.ToList().Count() > 0)
                        {
                            IsPackageFreezed = true;
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetPatientCurrentVisit()
        {
            try
            {
                if (Indicatior == null) Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
                BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.GetLatestVisit = true;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Close();
                    if (arg.Error == null && arg.Result != null)
                    {
                        if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                        {

                            //if (IsPackageFreezed == true)
                            //{
                            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                            //              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please freeze the previous package bill.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            //    msgW1.Show();
                            //}
                            //else
                            //{
                            ServiceSearchForPackage serviceSearch = null;   //ServiceSearch serviceSearch = null;
                            serviceSearch = new ServiceSearchForPackage();   //serviceSearch = new ServiceSearch();
                            serviceSearch.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                            serviceSearch.IsFromSaveAndPackageBill = true;
                            serviceSearch.OnAddButton_Click += new RoutedEventHandler(serviceSearch_OnAddButton_Click);
                            serviceSearch.Show();
                            //}

                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit is not marked for the Patient", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
        }

        private void btnAddpackageAdvance_Click(object sender, RoutedEventArgs e)
        {
            AddpackageAdvance();
        }

        private void AddpackageAdvance()
        {
            if (dgAdvances.SelectedItem != null && !(dgAdvances.SelectedItem as clsAdvanceVO).IsPackageBillFreeze)
            {
                if ((dgAdvances.SelectedItem as clsAdvanceVO).AdvanceAgainst == "Package")
                {
                    clsAdvanceVO objAdvance = dgAdvances.SelectedItem as clsAdvanceVO;
                    if (objAdvance.AdvanceTypeId != null)
                    {
                        cmbAdvanceType.IsEnabled = false;
                        cmbAdvanceType.SelectedValue = objAdvance.AdvanceTypeId;
                    }

                    if (objAdvance.AdvanceAgainstId != null)
                    {
                        cmbAdvanceAgainst.IsEnabled = false;
                        cmbAdvanceAgainst.SelectedValue = objAdvance.AdvanceAgainstId;
                    }

                    if (objAdvance.PackageID != null)
                    {
                        cmdSaveandPackageBill.Visibility = Visibility.Collapsed;
                        cmdSave.Visibility = Visibility.Visible;
                        lnkAddService.Visibility = Visibility.Collapsed;
                        cmbPackage.Visibility = Visibility.Visible;
                        cmbPackage.SelectedValue = objAdvance.PackageID;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Only for against package.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Package Bill is Feezed, You cannot add Advance.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

    }
}
