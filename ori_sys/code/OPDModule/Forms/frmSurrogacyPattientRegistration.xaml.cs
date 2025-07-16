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
using System.Collections;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using OPDModule;
using System.Windows.Media.Imaging;
using System.IO;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using CIMS.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using OPDModule.Forms;
using PalashDynamics;
using System.Threading;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.DashBoardVO;
namespace OPDModule.Forms
{
    public partial class frmSurrogacyPattientRegistration : UserControl, IInitiateCIMS
    {
        public frmSurrogacyPattientRegistration()
        {
            InitializeComponent();
        }

        private bool PatientEditMode = false;
        bool IsPatientExist = false;
        //bool PatientSourceFromLoyalty = false;

        #region IInitiateCIMS Members

        /// <summary>
        /// Function is for Initializing the form.
        /// </summary>
        /// <param name="Mode">New or Edit</param>        
        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    IsPatientExist = true;
                    UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                    mElement1.Text = "";

                    this.DataContext = new clsPatientVO()
                    {
                        MaritalStatusID = 0,
                        BloodGroupID = 0,
                        GenderID = 0,
                        OccupationId = 0,
                        Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                        State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                        District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                        RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                        City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                        Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,

                    };

                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = 10;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                    ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                    ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                    ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;

                    ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                    ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Male;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;


                    break;

                case "EDIT":
                    #region Patient Edit Mode
                    PatientEditMode = true;
                    IsPatientExist = true;


                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Surrogate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        IsPatientExist = false;

                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Surrogate", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID != 10)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select Surrogate ", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();

                        IsPatientExist = false;
                        break;

                    }


                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.PatientDetails.GeneralDetails.FromForm = 1;
                    BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    BizAction.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {


                                this.DataContext = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;

                                if (this.DataContext != null)
                                {
                                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;

                                    cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;

                                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;

                                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;

                                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;

                                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                    txtContactNo1.Text = ((clsPatientVO)this.DataContext).ContactNo1;
                                    txtContactNo2.Text = ((clsPatientVO)this.DataContext).ContactNo2;
                                    txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode.ToString();
                                    txtOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName.ToString();

                                    txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();

                                    //=======================================================
                                    //Spouse 
                                    if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 10) // Couple
                                    {
                                        dtpSpouseDOB.SelectedDate = ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth;
                                        cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                                        cmbSpouseBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;
                                        cmbSpouseGender.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.GenderID;

                                        cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;

                                        cmbSpouseOccupation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.OccupationId;
                                        txtSpouseYY.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "YY");
                                        txtSpouseMM.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "MM");
                                        txtSpouseDD.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "DD");

                                        txtSpouseContactNo1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo1;
                                        txtSpouseContactNo2.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo2;
                                        txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();

                                        txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                                        txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                                    }
                                    //=======================================================

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                    mElement.Text = " : " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName +
                                        " " + ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " +
                                        ((clsPatientVO)this.DataContext).GeneralDetails.LastName;
                                    PatientEditMode = true;



                                }
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                    #endregion
                    break;

                case "View":
                    #region Patient View Mode
                    PatientEditMode = true;
                    IsPatientExist = true;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        IsPatientExist = false;
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        PatientEditMode = false;

                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        IsPatientExist = false;
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        PatientEditMode = false;
                        break;
                    }
                    else
                    {


                    }
                    clsGetPatientBizActionVO BizAction1 = new clsGetPatientBizActionVO();
                    BizAction1.PatientDetails = new clsPatientVO();
                    BizAction1.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction1.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    BizAction1.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

                    Client1.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {

                                //tabBillingInfo.Visibility = Visibility.Visible;
                                //tabPatEncounterInfo.Visibility = Visibility.Visible;
                                //tabPatSponsorInfo.Visibility = Visibility.Visible;

                                this.DataContext = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;

                                if (this.DataContext != null)
                                {
                                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                                    // Commented by Saily P to get the Patient Source
                                    cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;


                                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                                    //Added By Yogita R
                                    txtOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName.ToString();
                                    //End
                                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;

                                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;

                                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                    txtContactNo1.Text = ((clsPatientVO)this.DataContext).ContactNo1;
                                    txtContactNo2.Text = ((clsPatientVO)this.DataContext).ContactNo2;
                                    txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode.ToString();

                                    txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();

                                    //////=======================================================
                                    ////Spouse 
                                    //if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 7) // Couple
                                    //{
                                    //    cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                                    //    cmbSpouseBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;
                                    //    cmbSpouseGender.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.GenderID;

                                    //    cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;

                                    //    cmbSpouseOccupation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.OccupationId;
                                    //    txtSpouseYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    //    txtSpouseMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    //    txtSpouseDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                    //    txtSpouseContactNo1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo1;
                                    //    txtSpouseContactNo2.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo2;
                                    //    txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();

                                    //    txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                                    //    txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                                    //}
                                    //////=======================================================

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                    mElement.Text = " : " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName +
                                        " " + ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " +
                                        ((clsPatientVO)this.DataContext).GeneralDetails.LastName;

                                    PatientEditMode = true;
                                    IsPatientExist = true;

                                }
                            }
                        }
                        else
                        {
                            //HtmlPage.Window.Alert("Error occured while adding patient.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };

                    Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();

                    grpHeader.IsEnabled = false;
                    grpPersonalDetails.IsEnabled = false;
                    grpContactDetails.IsEnabled = false;


                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    #endregion
                    break;

                case "NEWApp":
                    #region Get Details by AppointmentList
                    IsPatientExist = true;
                    UserControl rootPage3 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement3 = (TextBlock)rootPage3.FindName("SampleSubHeader");
                    mElement3.Text = "";

                    this.DataContext = new clsPatientVO()
                    {

                        MaritalStatusID = 0,
                        BloodGroupID = 0,
                        GenderID = 0,
                        OccupationId = 0,
                        Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                        State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                        District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                        RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                        Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                        City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                        Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,
                        // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                    };
                    //FillTariffMaster();                    
                    FillPatientSponsorDetails();


                    if (mAppointmentID > 0)
                    {
                        // BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                        ShowAppointmentDetails(mAppointmentID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);

                    }

                    #endregion
                    break;
                //case "Surrogacy":
                //    Flagref = true;
                //    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                //     IsPatientExist = true;
                //     UserControl rootPage31 = Application.Current.RootVisual as UserControl;
                //     TextBlock mElement31 = (TextBlock)rootPage31.FindName("SampleSubHeader");
                //     mElement31.Text = "";
                //    this.DataContext = new clsPatientVO()
                //    {
                //        MaritalStatusID = 0,
                //        BloodGroupID = 0,
                //        GenderID = 0,
                //        OccupationId = 0,
                //        Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                //        State =   ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                //        District =  ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                //        RelationID =  ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                //        City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                //        Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area, 

                //       // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                //    };                       

                //    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID; 
                //     ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                //     ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                //     ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                //     ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;

                //     ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                //     ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Male;
                //     ((clsPatientVO)this.DataContext).GeneralDetails.GenderID = (long)Genders.Female;


                //    break;
            }
        }

        #endregion
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        public long mAppointmentID = 0;
        public long mAppointmentUnitID = 0;
        bool PatientSponsorNewMode = true;
        bool Flagref = false;

        private enum SaveType
        {
            Registration = 0,
            IPD = 1,
            OPD = 2
        }

        private SaveType formSaveType = SaveType.Registration;

        void PatientRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

                ModuleName = "PalashDynamics";
                Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }

            if (!IsPageLoded)
            {
                formSaveType = SaveType.Registration;
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                      && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                    {
                        //do nothing
                    }
                    else
                        CmdSave.IsEnabled = false;
                }
                if (PatientEditMode == false)
                    CheckValidations();
                GetFamilyList();
                //FillCountryList();

                //if (this.DataContext != null)
                //{
                //    FillStateList(((clsPatientVO)this.DataContext).Country);
                //    FillCityList(((clsPatientVO)this.DataContext).State);
                //    GetAreaList(((clsPatientVO)this.DataContext).City);
                //    GetPinCodeList(((clsPatientVO)this.DataContext).City);
                //}
                FillReferralName();
                FillSurrogateAgency((long)0);
                FillPatientType();
                FillMaritalStatus();
                FillReligion();
                FillOccupation();
                FillBloodGroup();
                FillGender();
                // FillTariffMaster();
                FillPatientSponsorDetails();
                txtFirstName.Focus();
                cmbGender.IsEnabled = false;
                cmbSpouseGender.IsEnabled = false;
                // FillAgency();
                FillCountry();
                //if (!PatientEditMode)
                //{
                //    //======================================
                //    //Default Sponsor - Initialization
                //    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.SponsorWindow") as UIElement;

                //    Sponsortwin = new SponsorWindow();
                //    Sponsortwin = (SponsorWindow)mydata;
                //    ((IInitiateCIMS)Sponsortwin).Initiate("NEWR");
                //    // Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
                //    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                //    Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                //    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                //    sponsorContent.Content = Sponsortwin;
                //    //======================================
                //}         
                Indicatior.Close();
            }
            txtFirstName.Focus();
            txtFirstName.UpdateLayout();
            IsPageLoded = true;
        }

        /// <summary>
        /// Checks & assigns validations for the controls.
        /// </summary>
        /// <returns></returns>
        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (string.IsNullOrEmpty(txtYY.Text) && string.IsNullOrEmpty(txtMM.Text) && string.IsNullOrEmpty(txtDD.Text))
                {

                    txtYY.SetValidation("Age is required");
                    txtYY.RaiseValidationError();
                    txtMM.SetValidation("Age is required");
                    txtMM.RaiseValidationError();
                    txtDD.SetValidation("Age is required");
                    txtDD.RaiseValidationError();
                    result = false;
                    txtYY.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                }
                else
                {
                    txtYY.ClearValidationError();
                    txtMM.ClearValidationError();
                    txtDD.ClearValidationError();
                }
                if ((MasterListItem)cmbGender.SelectedItem == null)
                {
                    cmbGender.TextBox.SetValidation("Gender is required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Gender is required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbGender.TextBox.ClearValidationError();
                if ((MasterListItem)cmbPatientType.SelectedItem == null)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type is required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type is required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbPatientType.TextBox.ClearValidationError();

                //if ((MasterListItem)cmbPatientSource.SelectedItem == null)
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source is required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source is required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbPatientSource.TextBox.ClearValidationError();

                if (((clsPatientVO)this.DataContext).FamilyName == null || ((clsPatientVO)this.DataContext).FamilyName.Trim() == string.Empty)
                {
                    txtFamilyName.SetValidation("Family Name is required");
                    txtFamilyName.RaiseValidationError();
                    txtFamilyName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtFamilyName.ClearValidationError();


                if (((clsPatientVO)this.DataContext).GeneralDetails.LastName == null || ((clsPatientVO)this.DataContext).GeneralDetails.LastName.Trim() == string.Empty)
                {
                    txtLastName.SetValidation("Last Name is required");
                    txtLastName.RaiseValidationError();
                    txtLastName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtLastName.ClearValidationError();


                if (((clsPatientVO)this.DataContext).GeneralDetails.FirstName == null || ((clsPatientVO)this.DataContext).GeneralDetails.FirstName.Trim() == string.Empty)
                {
                    txtFirstName.SetValidation("First Name is required.");
                    txtFirstName.RaiseValidationError();
                    txtFirstName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtFirstName.ClearValidationError();

                if (IsPageLoded)
                {
                    if (txtEmail.Text.Trim().Length > 0)
                    {
                        if (txtEmail.Text.IsEmailValid())
                            txtEmail.ClearValidationError();
                        else
                        {
                            txtEmail.SetValidation("Please enter valid Email");
                            txtEmail.RaiseValidationError();
                            txtEmail.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            result = false;
                        }
                    }

                    if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null)
                    {
                        dtpDOB.SetValidation("Birth Date is required");
                        dtpDOB.RaiseValidationError();
                        dtpDOB.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth.Value.Date >= DateTime.Now.Date)
                    {
                        dtpDOB.SetValidation("Birth Date can not be greater than Todays Date");
                        dtpDOB.RaiseValidationError();
                        dtpDOB.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        dtpDOB.ClearValidationError();
                    if (txtMM.Text != "")
                    {
                        if (Convert.ToInt16(txtMM.Text) > 12)
                        {
                            txtMM.SetValidation("Month Cannot Be Greater than 12");
                            txtMM.RaiseValidationError();
                            txtMM.Focus();
                            result = false;
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            ClickedFlag = 0;
                        }

                        else
                            txtMM.ClearValidationError();
                    }
                    if (txtYY.Text != "")
                    {
                        if (Convert.ToInt16(txtYY.Text) > 120)
                        {
                            txtYY.SetValidation("Age can not be greater than 121");
                            txtYY.RaiseValidationError();
                            txtYY.Focus();
                            result = false;
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            ClickedFlag = 0;
                        }
                        else
                            txtYY.ClearValidationError();
                    }
                }

                //Spouse
                if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 10 && !PatientEditMode) // Couple
                {

                    if (string.IsNullOrEmpty(txtSpouseYY.Text) && string.IsNullOrEmpty(txtSpouseMM.Text) && string.IsNullOrEmpty(txtSpouseDD.Text))
                    {

                        txtSpouseYY.SetValidation("Age is required");
                        txtSpouseYY.RaiseValidationError();
                        txtSpouseMM.SetValidation("Age is required");
                        txtSpouseMM.RaiseValidationError();
                        txtSpouseDD.SetValidation("Age is required");
                        txtSpouseDD.RaiseValidationError();
                        result = false;
                        txtSpouseYY.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;

                    }
                    else
                    {
                        txtSpouseYY.ClearValidationError();
                        txtSpouseMM.ClearValidationError();
                        txtSpouseDD.ClearValidationError();
                    }
                    if ((MasterListItem)cmbSpouseGender.SelectedItem == null)
                    {
                        cmbSpouseGender.TextBox.SetValidation("Gender is required");
                        cmbSpouseGender.TextBox.RaiseValidationError();
                        cmbSpouseGender.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;

                    }
                    else if (((MasterListItem)cmbSpouseGender.SelectedItem).ID == 0)
                    {
                        cmbSpouseGender.TextBox.SetValidation("Gender is required");
                        cmbSpouseGender.TextBox.RaiseValidationError();
                        cmbSpouseGender.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        cmbGender.TextBox.ClearValidationError();
                    if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                    {
                        cmbPatientType.TextBox.SetValidation("Patient Type is required");
                        cmbPatientType.TextBox.RaiseValidationError();
                        cmbPatientType.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else if ((MasterListItem)cmbPatientType.SelectedItem == null)
                    {
                        cmbPatientType.TextBox.SetValidation("Patient Type is required");
                        cmbPatientType.TextBox.RaiseValidationError();
                        cmbPatientType.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        cmbPatientType.TextBox.ClearValidationError();
                    if ((MasterListItem)cmbPatientSource.SelectedItem == null)
                    {
                        cmbPatientSource.TextBox.SetValidation("Patient Source is required");
                        cmbPatientSource.TextBox.RaiseValidationError();
                        cmbPatientSource.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                    {
                        cmbPatientSource.TextBox.SetValidation("Patient Source is required");
                        cmbPatientSource.TextBox.RaiseValidationError();
                        cmbPatientSource.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        cmbPatientSource.TextBox.ClearValidationError();

                    if (((clsPatientVO)this.DataContext).SpouseDetails.FamilyName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName.Trim() == string.Empty)
                    {
                        txtSpouseFamilyName.SetValidation("Family Name is required");
                        txtSpouseFamilyName.RaiseValidationError();
                        txtSpouseFamilyName.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        txtSpouseFamilyName.ClearValidationError();

                    if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == string.Empty)
                    {
                        txtSpouseLastName.SetValidation("Last Name is required");
                        txtSpouseLastName.RaiseValidationError();
                        txtSpouseLastName.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        txtLastName.ClearValidationError();

                    if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == string.Empty)
                    {
                        txtSpouseFirstName.SetValidation("First Name is required.");
                        txtSpouseFirstName.RaiseValidationError();
                        txtSpouseFirstName.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        txtSpouseFirstName.ClearValidationError();

                    if (IsPageLoded)
                    {
                        if (txtSpouseEmail.Text.Trim().Length > 0)
                        {
                            if (txtSpouseEmail.Text.IsEmailValid())
                                txtSpouseEmail.ClearValidationError();
                            else
                            {
                                txtSpouseEmail.SetValidation("Please enter valid Email");
                                txtSpouseEmail.RaiseValidationError();
                                txtSpouseEmail.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                        }

                        if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
                        {
                            dtpSpouseDOB.SetValidation("Birth Date is required");
                            dtpSpouseDOB.RaiseValidationError();
                            dtpSpouseDOB.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date >= DateTime.Now.Date)
                        {
                            dtpSpouseDOB.SetValidation("Birth Date can not be greater than Todays Date");
                            dtpSpouseDOB.RaiseValidationError();
                            dtpSpouseDOB.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            dtpSpouseDOB.ClearValidationError();

                        if (txtSpouseYY.Text != "")
                        {
                            if (Convert.ToInt16(txtSpouseYY.Text) > 120)
                            {
                                txtSpouseYY.SetValidation("Age can not be greater than 121");
                                txtSpouseYY.RaiseValidationError();
                                txtSpouseYY.Focus();
                                result = false;
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                ClickedFlag = 0;
                            }
                            else
                                txtSpouseYY.ClearValidationError();
                        }
                    }
                }
            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }

        enum RegistrationTabs
        {
            Patient = 0,
            Spouse = 1,
            Sponsor = 2,
            Visit = 3
        }

        EncounterWindow visitwin = null;
        SponsorWindow Sponsortwin = null;
        bool SpouseClick = false;
        private void PatPersonalInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPageLoded)
            {
                CmdSave.Visibility = System.Windows.Visibility.Visible;
                CmdClose.Visibility = System.Windows.Visibility.Visible;
                RegistrationTabs SelectedTab = (RegistrationTabs)PatPersonalInfo.SelectedIndex;
                switch (SelectedTab)
                {
                    case RegistrationTabs.Patient:
                        txtFirstName.UpdateLayout();
                        txtFirstName.Focus();
                        break;

                    case RegistrationTabs.Spouse:
                        if (!PatientEditMode)
                        {
                            if (cmbGender.SelectedItem != null)
                            {
                                if (((MasterListItem)cmbGender.SelectedItem).ID == (long)Genders.Female)
                                    cmbSpouseGender.SelectedValue = (long)Genders.Male;
                                else if (((MasterListItem)cmbGender.SelectedItem).ID == (long)Genders.Male)
                                    cmbSpouseGender.SelectedValue = (long)Genders.Female;
                            }
                        }
                        if (!SpouseClick && !PatientEditMode)
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(txtSpouseYY.Text) && string.IsNullOrEmpty(txtSpouseMM.Text) && string.IsNullOrEmpty(txtSpouseDD.Text))
                                {

                                    txtSpouseYY.SetValidation("Age is required");
                                    txtSpouseYY.RaiseValidationError();
                                    txtSpouseMM.SetValidation("Age is required");
                                    txtSpouseMM.RaiseValidationError();
                                    txtSpouseDD.SetValidation("Age is required");
                                    txtSpouseDD.RaiseValidationError();
                                }
                                else
                                {
                                    txtSpouseYY.ClearValidationError();
                                    txtSpouseMM.ClearValidationError();
                                    txtSpouseDD.ClearValidationError();
                                }
                                if (((clsPatientVO)this.DataContext).SpouseDetails.FamilyName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName.Trim() == "")
                                {
                                    txtSpouseFamilyName.SetValidation("Family Name is required");
                                    txtSpouseFamilyName.RaiseValidationError();
                                }
                                else
                                    txtSpouseFamilyName.ClearValidationError();

                                if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == "")
                                {
                                    txtSpouseLastName.SetValidation("Last Name is required");
                                    txtSpouseLastName.RaiseValidationError();
                                }
                                else
                                    txtSpouseLastName.ClearValidationError();

                                if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                                {
                                    txtSpouseFirstName.SetValidation("First Name is required.");
                                    txtSpouseFirstName.RaiseValidationError();
                                }
                                else
                                    txtSpouseFirstName.ClearValidationError();

                                clsPatientVO myVO = new clsPatientVO();

                                myVO = (clsPatientVO)this.DataContext;

                                txtSpouseLastName.Text = txtLastName.Text;
                                txtSpouseFamilyName.Text = txtFamilyName.Text;
                                txtSpouseAddress1.Text = txtAddress1.Text;
                                txtSpouseAddress2.Text = txtAddress2.Text;
                                txtSpouseAddress3.Text = txtAddress3.Text;
                                txtSpouseArea.Text = txtArea.Text;
                                txtSpouseCountry.Text = txtCountry.Text;
                                txtSpouseState.Text = txtState.Text;
                                txtSpouseCity.Text = txtCity.Text;
                                txtSpouseDistrict.Text = txtDistrict.Text;
                                txtSpouseTaluka.Text = txtTaluka.Text;

                                myVO.SpouseDetails.LastName = myVO.GeneralDetails.LastName;
                                myVO.SpouseDetails.FamilyName = myVO.FamilyName;
                                myVO.SpouseDetails.AddressLine1 = myVO.AddressLine1;
                                myVO.SpouseDetails.AddressLine2 = myVO.AddressLine2;
                                myVO.SpouseDetails.AddressLine3 = myVO.AddressLine3;
                                myVO.SpouseDetails.Area = myVO.Area;
                                //  myVO.SpouseDetails.CompanyName = myVO.CompanyName;
                                myVO.SpouseDetails.Country = myVO.Country;
                                myVO.SpouseDetails.State = myVO.State;
                                myVO.SpouseDetails.City = myVO.City;
                                myVO.SpouseDetails.District = myVO.District;
                                myVO.SpouseDetails.Taluka = myVO.Taluka;
                                myVO.SpouseDetails.Pincode = myVO.Pincode;

                                this.DataContext = null;
                                this.DataContext = myVO;

                                txtSpouseResiCountryCode.Text = txtResiCountryCode.Text;
                                txtSpouseResiSTD.Text = txtResiSTD.Text;
                                txtSpouseMobileCountryCode.Text = txtMobileCountryCode.Text;

                                cmbSpouseMaritalStatus.SelectedItem = cmbMaritalStatus.SelectedItem;
                                cmbSpouseReligion.SelectedItem = cmbReligion.SelectedItem;


                                //this.UpdateLayout();

                                ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((clsPatientVO)this.DataContext).Area;
                                ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((clsPatientVO)this.DataContext).Country;
                                ((clsPatientVO)this.DataContext).SpouseDetails.State = ((clsPatientVO)this.DataContext).State;
                                ((clsPatientVO)this.DataContext).SpouseDetails.City = ((clsPatientVO)this.DataContext).City;
                                ((clsPatientVO)this.DataContext).SpouseDetails.District = ((clsPatientVO)this.DataContext).District;
                                ((clsPatientVO)this.DataContext).SpouseDetails.Taluka = ((clsPatientVO)this.DataContext).Taluka;
                                ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID = ((clsPatientVO)this.DataContext).MaritalStatusID;


                                txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                                txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                                txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();

                                cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;
                                cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;

                            }
                            catch (Exception)
                            {
                                //throw;
                            }

                            SpouseClick = true;
                        }
                        else if (!SpouseClick)
                        {
                            txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                            txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                            txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();
                            SpouseClick = true;
                        }

                        break;

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FillReferralName()
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbReferralName.ItemsSource = null;
                        cmbReferralName.ItemsSource = results.ToList();
                        cmbReferralName.SelectedItem = results.ToList()[0];

                    }
                    else
                    {
                        cmbReferralName.ItemsSource = null;
                        cmbReferralName.ItemsSource = objList;
                        cmbReferralName.SelectedItem = objList[0];
                    }

                }

                if (this.DataContext != null)
                {
                    cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillSurrogateAgency(long Refvalue)
        {
            //cmbReferralName

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_SurrogateAgencyMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = Convert.ToString(Refvalue);
            BizAction.Parent.Value = "ReferralTypeID";
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbAgencyName.ItemsSource = null;
                        cmbAgencyName.ItemsSource = results.ToList();
                        cmbAgencyName.SelectedItem = results.ToList()[0];

                    }
                    else
                    {
                        cmbAgencyName.ItemsSource = null;
                        cmbAgencyName.ItemsSource = objList;
                        cmbAgencyName.SelectedItem = objList[0];
                    }

                }

                if (this.DataContext != null)
                {
                    cmbAgencyName.SelectedValue = ((clsPatientVO)this.DataContext).AgencyID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPatientType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
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

                    if (Flagref == true)
                    {
                        var results = from r in objList
                                      where r.ID == 10
                                      select r;
                        cmbPatientType.ItemsSource = null;
                        cmbPatientType.ItemsSource = results.ToList();
                        cmbPatientType.SelectedItem = results.ToList()[0];
                    }
                    else
                    {
                        cmbPatientType.ItemsSource = null;
                        cmbPatientType.ItemsSource = objList;
                    }
                }

                if (this.DataContext != null)
                {
                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCountryList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Country";
            BizAction.IsDecode = true;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    txtSpouseCountry.ItemsSource = null;
                    txtSpouseCountry.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void FillStateList(string pCountry)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "State";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pCountry))
            {
                pCountry = pCountry.Trim();
                BizAction.Parent = new KeyValue { Key = "Country", Value = pCountry };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    txtSpouseState.ItemsSource = null;
                    txtSpouseState.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillCityList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "City";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                    txtSpouseCity.ItemsSource = null;
                    txtSpouseCity.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetDistrictList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "District";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtDistrict.ItemsSource = null;
                    txtDistrict.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                    txtSpouseDistrict.ItemsSource = null;
                    txtSpouseDistrict.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetTalukaList(string pState)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Taluka";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pState))
            {
                pState = pState.Trim();
                BizAction.Parent = new KeyValue { Key = "State", Value = pState };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtTaluka.ItemsSource = null;
                    txtTaluka.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                    txtSpouseTaluka.ItemsSource = null;
                    txtSpouseTaluka.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                    txtSpouseArea.ItemsSource = null;
                    txtSpouseArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetPinCodeList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Pincode";
            BizAction.IsDecode = true;

            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtPinCode.ItemsSource = null;
                    txtPinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                    txtSpousePinCode.ItemsSource = null;
                    txtSpousePinCode.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
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
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList.DeepCopy();
                    cmbSpouseMaritalStatus.ItemsSource = null;
                    cmbSpouseMaritalStatus.ItemsSource = objList.DeepCopy();
                }

                if (this.DataContext != null)
                {
                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                    cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetFamilyList()
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            //BizAction.IsActive = true;
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "FamilyName";
            BizAction.IsDecode = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy

            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtFamilyName.MinimumPopulateDelay = 100;

                    txtFamilyName.ItemsSource = null;
                    txtFamilyName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                    txtSpouseFamilyName.ItemsSource = null;
                    txtSpouseFamilyName.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillBloodGroup()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_BloodGroupMaster;
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
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbBloodGroup.ItemsSource = null;
                    cmbBloodGroup.ItemsSource = objList.DeepCopy();
                    cmbSpouseBloodGroup.ItemsSource = null;
                    cmbSpouseBloodGroup.ItemsSource = objList.DeepCopy();
                }

                if (this.DataContext != null)
                {
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                    cmbSpouseBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
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
                    cmbGender.ItemsSource = null;
                    cmbGender.ItemsSource = objList.DeepCopy();
                    cmbSpouseGender.ItemsSource = null;
                    cmbSpouseGender.ItemsSource = objList.DeepCopy();
                }

                if (this.DataContext != null)
                {
                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                    cmbSpouseGender.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.GenderID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillReligion()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_ReligionMaster;
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
                    cmbReligion.ItemsSource = null;
                    cmbReligion.ItemsSource = objList.DeepCopy();

                    cmbSpouseReligion.ItemsSource = null;
                    cmbSpouseReligion.ItemsSource = objList.DeepCopy();
                }

                if (this.DataContext != null)
                {
                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
                    cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillOccupation()
        {

            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_OccupationMaster;
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
                    cmbOccupation.ItemsSource = null;
                    cmbOccupation.ItemsSource = objList.DeepCopy();
                    cmbSpouseOccupation.ItemsSource = null;
                    cmbSpouseOccupation.ItemsSource = objList.DeepCopy();
                }

                if (this.DataContext != null)
                {
                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                    cmbSpouseOccupation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.OccupationId;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        public void FillPatientSponsorDetails()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();

                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        cmbPatientSource.ItemsSource = null;
                        cmbPatientSource.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ClosePatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

            ModuleName = "PalashDynamics";
            Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
            WebClient c2 = new WebClient();
            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        private void SavePatientButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = CheckValidations();

                if (saveDtls == true && visitwin != null)
                {
                    saveDtls = visitwin.CheckValidations();
                    if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Visit;
                }
                if (saveDtls == true && Sponsortwin != null && PatientSponsorNewMode == false && PatientEditMode == false)
                {
                    saveDtls = Sponsortwin.CheckValidations();
                    if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
                }
                if (saveDtls == true)
                {
                    string msgTitle = "";
                    string msgText = "";
                    msgText = "Are you sure you want to save Surrogate Details?";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                    ClickedFlag = 0;
            }
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SavePatient();
            else
                ClickedFlag = 0;

        }

        private void SavePatient()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddPatientBizActionVO BizAction = new clsAddPatientBizActionVO();
            BizAction.PatientDetails = (clsPatientVO)this.DataContext;
            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
            if (cmbBloodGroup.SelectedItem != null)
                BizAction.PatientDetails.BloodGroupID = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;
            if (cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbReligion.SelectedItem != null)
                BizAction.PatientDetails.ReligionID = ((MasterListItem)cmbReligion.SelectedItem).ID;

            if (cmbOccupation.SelectedItem != null)
                BizAction.PatientDetails.OccupationId = ((MasterListItem)cmbOccupation.SelectedItem).ID;

            if (cmbPatientType.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.PatientTypeID = ((MasterListItem)cmbPatientType.SelectedItem).ID;

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID != null)
                BizAction.PatientDetails.GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
            BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode =txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.PatientDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());
            BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
            if (cmbReferralName.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

            //By Anjali.........................
            if (txtCountry.SelectedItem != null)
                BizAction.PatientDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.PatientDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.PatientDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.PatientDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
            //...................................

            //======================================================
            //Spouse

            if (cmbSpouseMaritalStatus.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.MaritalStatusID = ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID;

            if (cmbSpouseBloodGroup.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.BloodGroupID = ((MasterListItem)cmbSpouseBloodGroup.SelectedItem).ID;

            if (cmbSpouseGender.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.GenderID = ((MasterListItem)cmbSpouseGender.SelectedItem).ID;

            if (cmbSpouseReligion.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.ReligionID = ((MasterListItem)cmbSpouseReligion.SelectedItem).ID;

            if (cmbSpouseOccupation.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.OccupationId = ((MasterListItem)cmbSpouseOccupation.SelectedItem).ID;

            BizAction.PatientDetails.SpouseDetails.ContactNo1 = txtSpouseContactNo1.Text.Trim();
            BizAction.PatientDetails.SpouseDetails.ContactNo2 = txtSpouseContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtSpouseMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.SpouseDetails.MobileCountryCode = txtSpouseMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtSpouseResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.SpouseDetails.ResiNoCountryCode = long.Parse(txtSpouseResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtSpouseResiSTD.Text.Trim()))
                BizAction.PatientDetails.SpouseDetails.ResiSTDCode = long.Parse(txtSpouseResiSTD.Text.Trim());

            BizAction.PatientDetails.SpouseDetails.Pincode = txtSpousePinCode.Text.Trim();

            if (cmbReferralName.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

            BizAction.PatientDetails.CompanyName = txtSpouseOfficeName.Text.Trim();


            //By Anjali.........................
            if (txtCountry.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.PatientDetails.SpouseDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
            //...................................

            //======================================================

            //======================================================
            //Surrogate Other Details
            BizAction.PatientDetails.FromForm = 1;
            //if (!string.IsNullOrEmpty(txtSourceName.Text.Trim()))
            //    BizAction.PatientDetails.SourceName = txtSourceName.Text.Trim();
            //if (!string.IsNullOrEmpty(txtSourceEmailID.Text.Trim()))
            //    BizAction.PatientDetails.SourceEmail = txtSourceEmailID.Text.Trim();
            //if (!string.IsNullOrEmpty(txtSourceContactNo.Text.Trim()))
            //    BizAction.PatientDetails.SourceContactNo = txtSourceContactNo.Text.Trim();
            //if (!string.IsNullOrEmpty(txtSourceAddress.Text.Trim()))
            //    BizAction.PatientDetails.SourceAddress = txtSourceAddress.Text.Trim();
            if (cmbAgencyName.SelectedItem != null)
                BizAction.PatientDetails.AgencyID = ((MasterListItem)cmbAgencyName.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtOtherDetails.Text.Trim()))
                BizAction.PatientDetails.SurrogateOtherDetails = txtOtherDetails.Text.Trim();



            //======================================================
            //======================================================
            //Surrogate Sponser Details
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID != null)
                BizAction.PatientDetails.TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID;

            //======================================================


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    CmdSave.IsEnabled = false;

                    Indicatior.Close();

                    PatientEditMode = true;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                        txtMRNumber.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;


                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Surrogate Details Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (re) =>
                    {
                        if (formSaveType == SaveType.Registration)
                        {
                            //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

                            ModuleName = "PalashDynamics";
                            Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        }
                    };
                    msgW1.Show();

                }
                else
                {
                    CmdSave.IsEnabled = true;
                    ClickedFlag = 0;
                    Indicatior.Close();

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        UIElement myData = null;
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
                ((IInitiateCIMS)myData).Initiate("REG");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void OpenIPDForm()
        {
            try
            {
                ModuleName = "PalashDynamics.IPD";
                Action = "PalashDynamics.IPD.IPDAdmission";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");

                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            catch (Exception)
            {
                throw;
            }

        }
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                UIElement myData = null;
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
                ((IInitiateCIMS)myData).Initiate("NEW");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void SaveVisit()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();

            ((clsVisitVO)visitwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            ((clsVisitVO)visitwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
            ((clsVisitVO)visitwin.DataContext).SpouseID = ((clsPatientVO)this.DataContext).SpouseDetails.ID;
            clsAddVisitBizActionVO BizAction = new clsAddVisitBizActionVO();
            BizAction.VisitDetails = (clsVisitVO)visitwin.DataContext;

            if (visitwin.cmbDepartment.SelectedItem != null)
                BizAction.VisitDetails.DepartmentID = ((MasterListItem)visitwin.cmbDepartment.SelectedItem).ID;

            if (visitwin.cmbVisitType.SelectedItem != null)
                BizAction.VisitDetails.VisitTypeID = ((MasterListItem)visitwin.cmbVisitType.SelectedItem).ID;

            if (visitwin.cmbCabin.SelectedItem != null)
                BizAction.VisitDetails.CabinID = ((MasterListItem)visitwin.cmbCabin.SelectedItem).ID;

            if (visitwin.cmbDoctor.SelectedItem != null)
                BizAction.VisitDetails.DoctorID = ((MasterListItem)visitwin.cmbDoctor.SelectedItem).ID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsAddVisitBizActionVO)arg.Result).VisitDetails != null)
                        {
                            ((clsVisitVO)visitwin.DataContext).ID = ((clsAddVisitBizActionVO)arg.Result).VisitDetails.ID;
                            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                                ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID = ((clsVisitVO)visitwin.DataContext).ID;

                            Indicatior.Close();

                            if (visitwin.EditMode == false)
                            {
                                if (mAppointmentID > 0)
                                    AddMarkVisitForAppointment();

                                if (BizAction.VisitDetails.VisitTypeID > 0)
                                    SaveVisitTypeCharges(BizAction.VisitDetails.VisitTypeID, BizAction.VisitDetails.ID);
                            }
                        }
                    }
                }
                else
                {
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding visit details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void AddMarkVisitForAppointment()
        {
            clsAddMarkVisitInAppointmenBizActionVO BizAction = new clsAddMarkVisitInAppointmenBizActionVO();
            BizAction.AppointmentDetails = new clsAppointmentVO();
            BizAction.AppointmentDetails.AppointmentID = mAppointmentID;
            BizAction.AppointmentDetails.VisitMark = true;

            BizAction.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsAddMarkVisitInAppointmenBizActionVO)arg.Result).AppointmentDetails != null)
                    {
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                         new MessageBoxControl.MessageBoxChildWindow("", "Error occured while Updating appointment.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void SaveVisitTypeCharges(long pVisitTypeID, long pVisitID)
        {
            clsGetVisitTypeBizActionVO BizAction = new clsGetVisitTypeBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ID = pVisitTypeID;
            //BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetVisitTypeBizActionVO)e.Result).List != null)
                    {
                        clsVisitTypeVO objVO = new clsVisitTypeVO();

                        objVO = ((clsGetVisitTypeBizActionVO)e.Result).List[0];

                        if (objVO.ServiceID > 0)
                        {
                            long tid = 0;

                            if (Sponsortwin.DataContext != null && ((clsPatientSponsorVO)Sponsortwin.DataContext).TariffID.HasValue)
                                tid = ((clsPatientSponsorVO)Sponsortwin.DataContext).TariffID.Value;

                            GetServiceDetails(objVO.ServiceID, tid, pVisitID);
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetServiceDetails(long pServiceID, long pTariffID, long pVisitID)
        {
            clsGetTariffServiceListBizActionVO BizAction = new clsGetTariffServiceListBizActionVO();
            BizAction.ServiceList = new List<clsServiceMasterVO>();
            BizAction.TariffID = pTariffID;
            BizAction.ServiceID = pServiceID;
            if (Sponsortwin != null && Sponsortwin.DataContext != null)
            {

                BizAction.PatientSourceType = ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientSourceType;
                BizAction.PatientSourceTypeID = ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientSourceTypeID;
                BizAction.PatientID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count > 0)
                    {
                        clsServiceMasterVO mService = new clsServiceMasterVO();

                        mService = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[0];

                        //clsChargeVO mCharge = new clsChargeVO() { Opd_Ipd_External = 0, Opd_Ipd_External_Id = pVisitID, ServiceSpecilizationID = mService.Specialization, TariffServiceId = mService.TariffServiceMasterID, ServiceId = mService.ID, ServiceName = mService.Description, Rate = Convert.ToDouble(mService.Rate), Quantity = 1, _
                        //ServiceTaxPercent =  };

                        clsChargeVO itemC = new clsChargeVO();
                        itemC.Opd_Ipd_External = 0;
                        itemC.Opd_Ipd_External_Id = pVisitID;
                        itemC.Opd_Ipd_External_UnitId = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

                        itemC.ServiceSpecilizationID = mService.Specialization;
                        itemC.TariffServiceId = mService.TariffServiceMasterID;
                        itemC.ServiceId = mService.ID;
                        itemC.ServiceName = mService.ServiceName;
                        itemC.Quantity = 1;
                        itemC.RateEditable = mService.RateEditable;
                        itemC.MinRate = Convert.ToDouble(mService.MinRate);
                        itemC.MaxRate = Convert.ToDouble(mService.MaxRate);
                        itemC.Rate = Convert.ToDouble(mService.Rate);

                        itemC.TotalAmount = itemC.Rate * itemC.Quantity;

                        if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 3 || ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 6)
                        { //Staff Or Staff Dependent
                            if (mService.StaffDiscountPercent > 0)
                                itemC.StaffDiscountPercent = Convert.ToDouble(mService.StaffDiscountPercent);
                            else
                                itemC.StaffDiscountAmount = Convert.ToDouble(mService.StaffDiscountAmount);

                            if (mService.StaffDependantDiscountPercent > 0)
                                itemC.StaffParentDiscountPercent = Convert.ToDouble(mService.StaffDependantDiscountPercent);
                            else
                                itemC.StaffParentDiscountAmount = Convert.ToDouble(mService.StaffDependantDiscountAmount);

                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplyConcessionToStaff == true)
                            {
                                if (mService.ConcessionPercent > 0)
                                {
                                    itemC.ConcessionPercent = Convert.ToDouble(mService.ConcessionPercent);
                                }
                                else
                                    itemC.Concession = Convert.ToDouble(mService.ConcessionAmount);
                            }
                        }
                        else
                        {
                            if (mService.ConcessionPercent > 0)
                            {
                                itemC.ConcessionPercent = Convert.ToDouble(mService.ConcessionPercent);
                            }
                            else
                                itemC.Concession = Convert.ToDouble(mService.ConcessionAmount);
                        }

                        if (mService.ServiceTaxPercent > 0)
                            itemC.ServiceTaxPercent = Convert.ToDouble(mService.ServiceTaxPercent);
                        else
                            itemC.ServiceTaxAmount = Convert.ToDouble(mService.ServiceTaxAmount);

                        SaveCharge(itemC);
                    }
                }
                else
                {
                    //HtmlPage.Window.Alert("Error occured while processing.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void SaveCharge(clsChargeVO pCharge)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                clsAddChargeBizActionVO BizAction = new clsAddChargeBizActionVO();
                BizAction.Details = pCharge;
                pCharge.Date = DateTime.Now;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                        }
                        Indicatior.Close();
                    }
                    else
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while saving Visit Type Charges.");
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Visit Type Charges.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                // throw;
                string err = ex.Message;
                Indicatior.Close();
            }
        }

        private void SaveSponsor()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails = (clsPatientSponsorVO)Sponsortwin.DataContext;
                //BizAction.PatientSponsorDetails.PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;

                if (Sponsortwin.cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)Sponsortwin.cmbPatientSource.SelectedItem).ID;
                else if ((MasterListItem)cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                //BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

                if (Sponsortwin.cmbTariff.SelectedItem != null)
                    BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)Sponsortwin.cmbTariff.SelectedItem).ID;
                //Commented by Saily P
                //   else if((MasterListItem)cmbTariff.SelectedItem != null)
                //     BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                if (Sponsortwin.cmbCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;

                if (Sponsortwin.cmbAssCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.AssociatedCompanyID = ((MasterListItem)Sponsortwin.cmbAssCompany.SelectedItem).ID;

                if (Sponsortwin.cmbDesignation.SelectedItem != null)
                    BizAction.PatientSponsorDetails.DesignationID = ((MasterListItem)Sponsortwin.cmbDesignation.SelectedItem).ID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (formSaveType == SaveType.IPD)
                            {
                                OpenIPDForm();
                            }
                            else if (formSaveType == SaveType.OPD)
                            {
                                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.EncounterWindow") as UIElement;
                                visitwin = new EncounterWindow();
                                visitwin.VAppointmentID = mAppointmentID;
                                ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID = BizAction.PatientSponsorDetails.TariffID.Value;
                                ((IInitiateCIMS)visitwin).Initiate("NEW");
                                ((IApplicationConfiguration)App.Current).OpenMainContent(visitwin as UIElement);
                            }
                        }

                        if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 7) //Couple
                        {
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).ID = 0;
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).SpouseDetails.PatientID;
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).SpouseDetails.UnitId;

                            //clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                            BizAction.PatientSponsorDetails = (clsPatientSponsorVO)Sponsortwin.DataContext;
                            //BizAction.PatientSponsorDetails.PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;

                            if (Sponsortwin.cmbPatientSource.SelectedItem != null)
                                BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)Sponsortwin.cmbPatientSource.SelectedItem).ID;

                            if (Sponsortwin.cmbTariff.SelectedItem != null)
                                BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)Sponsortwin.cmbTariff.SelectedItem).ID;

                            if (Sponsortwin.cmbCompany.SelectedItem != null)
                                BizAction.PatientSponsorDetails.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;

                            if (Sponsortwin.cmbAssCompany.SelectedItem != null)
                                BizAction.PatientSponsorDetails.AssociatedCompanyID = ((MasterListItem)Sponsortwin.cmbAssCompany.SelectedItem).ID;

                            if (Sponsortwin.cmbDesignation.SelectedItem != null)
                                BizAction.PatientSponsorDetails.DesignationID = ((MasterListItem)Sponsortwin.cmbDesignation.SelectedItem).ID;

                            //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client1.ProcessCompleted += (s1, arg1) =>
                            {
                                if (arg.Error == null && arg.Result != null)
                                {

                                }
                                else
                                {
                                    //Indicatior.Close();
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Spouse Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            };
                            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            Client1.CloseAsync();
                        }
                        //  sponsorContent.Content = null;
                        Indicatior.Close();
                    }
                    else
                    {
                        // System.Windows.Browser.HtmlPage.Window.Alert("Error occured while saving Sponsor.");
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }

                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();

            }
            catch (Exception ex)
            {
                // throw;
                string err = ex.Message;
                Indicatior.Close();
            }
        }

        private void GetPatientDetails(long lPatientID)
        {
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = lPatientID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                    cmbPatientSource.SelectedValue = ((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientSourceID; // TariffID;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                        txtMRNumber.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void UpdateAppointment(long lPatientID, long lAppointmentID, long lPatientUnitID)
        {
            clsAddAppointmentBizActionVO BizAction = new clsAddAppointmentBizActionVO();
            BizAction.AppointmentDetails = new clsAppointmentVO();
            BizAction.AppointmentDetails.AppointmentID = lAppointmentID;
            BizAction.AppointmentDetails.PatientId = lPatientID;
            BizAction.AppointmentDetails.PatientUnitId = lPatientUnitID;
            BizAction.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    //Do Nothing.
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CmdPatientSearch_Click_1(object sender, RoutedEventArgs e)
        {
            if (PatientEditMode == false && cmbPatientType.SelectedItem != null)
            {

                if (((MasterListItem)cmbPatientType.SelectedItem).ID == 2) //Search Appointment
                {

                    ChildWindow appoitmentForm = new ChildWindow();
                    // ChildWindow appoitmentForm = new ChildWindow();
                    //Open Search Appointment Window
                    AppointmentList win = new AppointmentList();
                    win.UnRegistered = true;

                    win.OnSaveButton_Click += new RoutedEventHandler(win_OnSaveButton_Click);

                    win.tabAppointmentChart.Visibility = Visibility.Collapsed;
                    win.cmdCancelAppointment.Visibility = Visibility.Collapsed;
                    win.cmdPrint.Visibility = Visibility.Collapsed;
                    win.cmdOK.Visibility = Visibility.Visible;
                    win.dgAppointmentList.Columns[8].Visibility = Visibility.Collapsed;
                    appoitmentForm.Content = win;
                    appoitmentForm.Height = 500;
                    appoitmentForm.Show();

                }
                else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 3)//Search Staff
                {
                    StaffWindow StaffWin = new StaffWindow();
                    StaffWin.OnSaveButton_Click += new RoutedEventHandler(StaffWin_OnSaveButton_Click);
                    //  StaffWin.Height = 500;
                    StaffWin.Show();
                }
            }
            else
            {
            }
            if (((MasterListItem)cmbPatientType.SelectedItem).ID == 4)//Search Patients to whom Loyalty card is issued
            {
                LoyaltyProgramWindow Loyaltywin = new LoyaltyProgramWindow();
                Loyaltywin.OnSaveButton_Click += new RoutedEventHandler(Loyaltywin_OnSaveButton_Click);
                Loyaltywin.Show();
            }
        }

        void Loyaltywin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            LoyaltyProgramWindow Win = (LoyaltyProgramWindow)sender;
            if (Win.DialogResult == true)
            {
                clsPatientFamilyDetailsVO tempMember = Win.SelectedMember;
                if (tempMember != null)
                {
                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.PatientDetails.GeneralDetails.PatientID = tempMember.PatientID;
                    BizAction.PatientDetails.GeneralDetails.UnitId = tempMember.UnitId;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null && arg.Result != null && ((clsGetPatientBizActionVO)arg.Result).PatientDetails != null)
                        {
                            clsPatientVO myPatient = new clsPatientVO();
                            // myPatient.GeneralDetails = tempPatient;
                            myPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;
                            myPatient.GenderID = 0;
                            myPatient.ParentPatientID = tempMember.PatientID;
                            myPatient.RelationID = tempMember.RelationID;
                            myPatient.GeneralDetails.FirstName = tempMember.FirstName;
                            myPatient.GeneralDetails.MiddleName = tempMember.MiddleName;
                            myPatient.GeneralDetails.LastName = tempMember.LastName;
                            myPatient.GeneralDetails.DateOfBirth = tempMember.DateOfBirth;
                            //myPatient.GeneralDetails.PatientSourceID = 
                            myPatient.TariffID = tempMember.TariffID;
                            myPatient.GeneralDetails.MRNo = "";
                            myPatient.GeneralDetails.RegistrationDate = DateTime.Now;
                            myPatient.Photo = null;
                            myPatient.BloodGroupID = 0;
                            myPatient.OccupationId = 0;
                            myPatient.CivilID = "";
                            myPatient.GeneralDetails.PatientID = 0;
                            myPatient.MaritalStatusID = 0;
                            myPatient.Email = "";
                            myPatient.ContactNo1 = "";
                            this.DataContext = myPatient;

                            cmbGender.SelectedValue = myPatient.GenderID;
                            cmbBloodGroup.SelectedValue = myPatient.BloodGroupID;
                            cmbMaritalStatus.SelectedValue = myPatient.MaritalStatusID;
                            cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;

                            txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                            txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                            txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                            PatPersonalInfo.SelectedIndex = 0;

                            //if (sponsorContent.Content != null)
                            //{
                            //    Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
                            //    if (Sponsortwin.DataContext != null)
                            //    {
                            //        ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientSourceID = myPatient.GeneralDetails.PatientSourceID;
                            //    }

                            //}

                            IsPatientExist = true;
                            PatientEditMode = false;
                        }
                    };
                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }
            }
        }

        void win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((AppointmentList)sender).DailogResult == true)
            {
                if (((AppointmentList)sender).item != null || ((AppointmentList)sender).item.AppointmentID != 0)
                {
                    WaitIndicator Indicatior = new WaitIndicator();
                    Indicatior.Show();
                    ShowAppointmentDetails(((AppointmentList)sender).item.AppointmentID, ((AppointmentList)sender).item.UnitId);
                    Indicatior.Close();
                }
            }
        }

        void StaffWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                if (((StaffWindow)sender).DialogResult == true)
                {
                    if (((StaffWindow)sender).StaffDetails != null || ((StaffWindow)sender).StaffDetails.ID != 0)
                    {
                        clsGetStaffMasterDetailsByIDBizActionVO BizAction = new clsGetStaffMasterDetailsByIDBizActionVO();
                        BizAction.StaffMasterList = new clsStaffMasterVO();
                        BizAction.StaffID = ((StaffWindow)sender).StaffDetails.ID;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                clsGetStaffMasterDetailsByIDBizActionVO ObjStaff = new clsGetStaffMasterDetailsByIDBizActionVO();
                                ObjStaff = (clsGetStaffMasterDetailsByIDBizActionVO)arg.Result;

                                ((clsPatientVO)this.DataContext).GeneralDetails.FirstName = ObjStaff.StaffMasterList.FirstName;
                                ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName = ObjStaff.StaffMasterList.MiddleName;
                                ((clsPatientVO)this.DataContext).GeneralDetails.LastName = ObjStaff.StaffMasterList.LastName;
                                ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth = ObjStaff.StaffMasterList.DOB;
                                ((clsPatientVO)this.DataContext).GenderID = ObjStaff.StaffMasterList.GenderID;
                                cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                PatPersonalInfo.SelectedIndex = 0;
                            }
                            Indicatior.Close();
                            Indicatior = null;
                            this.UpdateLayout();
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                }
            }
            catch (Exception)
            {
                Indicatior.Close();
                throw;
            }
            finally
            {

            }
        }

        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoWindow phWin = new PhotoWindow();
            if (this.DataContext != null)
                phWin.MyPhoto = ((clsPatientVO)this.DataContext).Photo;
            phWin.OnSaveButton_Click += new RoutedEventHandler(phWin_OnSaveButton_Click);
            phWin.Show();

        }

        private void btnSpousePhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoWindow phWinS = new PhotoWindow();
            if (this.DataContext != null)
                phWinS.MyPhoto = ((clsPatientVO)this.DataContext).SpouseDetails.Photo;
            phWinS.OnSaveButton_Click += new RoutedEventHandler(phWinS_OnSaveButton_Click);
            phWinS.Show();

        }

        void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                ((clsPatientVO)this.DataContext).Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
        }

        void phWinS_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                ((clsPatientVO)this.DataContext).SpouseDetails.Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCountry.Text = txtCountry.Text.ToTitleCase();
            FillStateList(txtCountry.Text);
        }

        private void txtCity_LostFocus(object sender, RoutedEventArgs e)
        {
            txtCity.Text = txtCity.Text.ToTitleCase();
            GetAreaList(txtCity.Text);
            GetPinCodeList(txtCity.Text);
        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {
            txtState.Text = txtState.Text.ToTitleCase();

            FillCityList(txtState.Text);
            GetDistrictList(txtState.Text);
            GetTalukaList(txtState.Text);
        }

        private void txtArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtArea.Text = txtArea.Text.ToTitleCase();

        }

        private void txtSpouseArea_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseArea.Text = txtSpouseArea.Text.ToTitleCase();

        }

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            ((clsPatientVO)this.DataContext).FamilyName = txtLastName.Text;
            //((clsPatientVO)this.DataContext).GeneralDetails.LastName;
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        private void txtDistrict_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDistrict.Text = txtDistrict.Text.ToTitleCase();
        }

        private void txtTaluka_LostFocus(object sender, RoutedEventArgs e)
        {
            txtTaluka.Text = txtTaluka.Text.ToTitleCase();
        }

        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtMM.SelectAll();
        }

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtDD.SelectAll();
        }

        private void txtSpouseYY_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateSpouseBirthDate();
            txtSpouseMM.SelectAll();
        }

        private void txtSpouseMM_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateSpouseBirthDate();
            txtSpouseDD.SelectAll();
        }

        void CalculateSpouseBirthDate()
        {

            int Yearval = 0;
            int Monthval = 0;
            int DayVal = 0;

            if (txtSpouseYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseYY.Text.Trim()))
                Yearval = int.Parse(txtSpouseYY.Text.Trim());

            if (txtSpouseMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseMM.Text.Trim()))
                Monthval = int.Parse(txtSpouseMM.Text.Trim());


            if (txtSpouseDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseDD.Text.Trim()))
                DayVal = int.Parse(txtSpouseDD.Text.Trim());

            if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            {
                dtpSpouseDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
                dtpSpouseDOB.ClearValidationError();
                txtSpouseYY.ClearValidationError();
                txtSpouseMM.ClearValidationError();
                txtSpouseDD.ClearValidationError();
            }
            else
            {
                txtSpouseYY.SetValidation("Age is Required");
                txtSpouseYY.RaiseValidationError();
                txtSpouseMM.SetValidation("Age is Required");
                txtSpouseMM.RaiseValidationError();
                txtSpouseDD.SetValidation("Age is Required");
                txtSpouseDD.RaiseValidationError();

                dtpSpouseDOB.SetValidation("Please select the Date of birth");
                dtpSpouseDOB.RaiseValidationError();
                txtSpouseYY.Text = "0";
                txtSpouseMM.Text = "0";
                txtSpouseDD.Text = "0";
            }
        }

        void CalculateBirthDate()
        {
            int Yearval = 0;
            int Monthval = 0;
            int DayVal = 0;

            if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
                Yearval = int.Parse(txtYY.Text.Trim());
            if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
                Monthval = int.Parse(txtMM.Text.Trim());
            if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
                DayVal = int.Parse(txtDD.Text.Trim());
            if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            {
                dtpDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
                dtpDOB.ClearValidationError();
                txtYY.ClearValidationError();
                txtMM.ClearValidationError();
                txtDD.ClearValidationError();
            }
            else
            {
                txtYY.SetValidation("Age is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Please select the Date of birth");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
        }

        //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
        //{
        //    int val = int.Parse(txtMM.Text.Trim());
        //    if (val > 0)
        //    {
        //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
        //        dtpDOB.ClearValidationError();
        //        txtYY.ClearValidationError();
        //        txtMM.ClearValidationError();
        //        txtDD.ClearValidationError();
        //    }
        //}

        //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
        //{
        //    int val = int.Parse(txtDD.Text.Trim());
        //    if (val > 0)
        //    {
        //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
        //        dtpDOB.ClearValidationError();
        //        txtYY.ClearValidationError();
        //        txtMM.ClearValidationError();
        //        txtDD.ClearValidationError();
        //    }
        //}           

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}
        }
        private void txtSpouseDD_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateSpouseBirthDate();
            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //{
            //    int val = int.Parse(txtYY.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("YY", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //{
            //    int val = int.Parse(txtMM.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("MM", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}

            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //{
            //    int val = int.Parse(txtDD.Text.Trim());
            //    if (val > 0)
            //    {
            //        dtpDOB.SelectedDate = ConvertDateBack("DD", val, dtpDOB.SelectedDate);
            //        dtpDOB.ClearValidationError();
            //        txtYY.ClearValidationError();
            //        txtMM.ClearValidationError();
            //        txtDD.ClearValidationError();

            //    }
            //}
        }
        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth > DateTime.Now)
            {
                txtYY.SetValidation("Age is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Date of birth can not be greater than Today");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null)
            {
                txtYY.SetValidation("Age is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Please select the Date of birth");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else
            {
                dtpDOB.ClearValidationError();
                txtYY.ClearValidationError();
                txtMM.ClearValidationError();
                txtDD.ClearValidationError();

                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            }

            txtYY.SelectAll();
        }

        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);

                    //return date.ToString(parameter.ToString());
                    // This is to convert the timespan to datetime object
                    DateTime age = DateTime.MinValue + difference;

                    // Min value is 01/01/0001
                    // Actual age is say 24 yrs, 9 months and 3 days represented as timespan
                    // Min Valye + actual age = 25 yrs , 10 months and 4 days.
                    // subtract our addition or 1 on all components to get the actual date.
                    string result = "";
                    switch (parameter.ToString().ToUpper())
                    {
                        case "YY":
                            result = (age.Year - 1).ToString();
                            break;
                        case "MM":
                            result = (age.Month - 1).ToString();
                            break;
                        case "DD":
                            int day = BirthDate.Day;
                            int curday = DateTime.Now.Day;
                            int dif = 0;
                            if (day > curday)
                                dif = (curday + 30) - day;
                            else
                                dif = curday - day;
                            result = dif.ToString();
                            //result = (age.Day - 1).ToString();
                            break;
                        default:
                            result = (age.Year - 1).ToString();
                            break;
                    }
                    return result;

                }
                catch (Exception ex)
                {
                    string err = ex.Message;
                    return string.Empty;
                }
            }
            else
                return string.Empty;
        }

        private DateTime? ConvertDateBack(string parameter, int value, DateTime? DateTobeConvert)
        {
            try
            {
                DateTime BirthDate;
                if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                    BirthDate = DateTobeConvert.Value;
                else
                    BirthDate = DateTime.Now;


                int mValue = Int32.Parse(value.ToString());

                switch (parameter.ToString().ToUpper())
                {
                    case "YY":
                        BirthDate = BirthDate.AddYears(-mValue);

                        break;
                    case "MM":
                        BirthDate = BirthDate.AddMonths(-mValue);
                        // result = (age.Month - 1).ToString();
                        break;
                    case "DD":
                        //result = (age.Day - 1).ToString();
                        BirthDate = BirthDate.AddDays(-mValue);
                        break;
                    default:
                        BirthDate = BirthDate.AddYears(-mValue);
                        break;
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }

        }


        private DateTime? CalculateDateFromAge(int Year, int Month, int Days)
        {
            try
            {
                DateTime BirthDate;
                //if (DateTobeConvert != null && parameter.ToString().ToUpper() != "YY")
                //    BirthDate = DateTobeConvert.Value;
                //else
                BirthDate = DateTime.Now;
                if (Year > 0)
                {
                    BirthDate = BirthDate.AddYears(-Year);
                }

                if (Month > 0)
                {
                    BirthDate = BirthDate.AddMonths(-Month);
                }

                if (Days > 0)
                {
                    BirthDate = BirthDate.AddDays(-Days);
                }
                //result = (age.Day - 1).ToString();
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
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

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    //((TextBox)sender).SelectionStart = selectionStart;
                    //((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    //((TextBox)sender).SelectionStart = selectionStart;
                    //((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text.Length > 0)
            {
                if (txtEmail.Text.IsEmailValid())
                    txtEmail.ClearValidationError();
                else
                {
                    txtEmail.SetValidation("Please enter valid Email");
                    txtEmail.RaiseValidationError();
                }
            }
        }

        private void txtSpouseEmail_LostFocus(object sender, RoutedEventArgs e)
        {

            if (txtSpouseEmail.Text.Length > 0)
            {
                if (txtSpouseEmail.Text.IsEmailValid())
                    txtSpouseEmail.ClearValidationError();
                else
                {
                    txtSpouseEmail.SetValidation("Please enter valid Email");
                    txtSpouseEmail.RaiseValidationError();
                }
            }
        }

        private void ShowAppointmentDetails(long AppID, long UnitID)
        {
            clsGetAppointmentByIdBizActionVO BizActionObj = new clsGetAppointmentByIdBizActionVO();
            BizActionObj.AppointmentDetails = new clsAppointmentVO();
            BizActionObj.AppointmentDetails.AppointmentID = AppID;
            BizActionObj.AppointmentDetails.UnitId = UnitID;

            Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client2 = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);
            Client2.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    mAppointmentID = BizActionObj.AppointmentDetails.AppointmentID;
                    mAppointmentUnitID = BizActionObj.AppointmentDetails.UnitId;
                    clsGetAppointmentByIdBizActionVO mObj = new clsGetAppointmentByIdBizActionVO();
                    mObj = (clsGetAppointmentByIdBizActionVO)arg.Result;

                    ((clsPatientVO)this.DataContext).GeneralDetails.FirstName = mObj.AppointmentDetails.FirstName;
                    ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName = mObj.AppointmentDetails.MiddleName;
                    ((clsPatientVO)this.DataContext).GeneralDetails.LastName = mObj.AppointmentDetails.LastName;
                    ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth = mObj.AppointmentDetails.DOB;
                    ((clsPatientVO)this.DataContext).FamilyName = mObj.AppointmentDetails.FamilyName;
                    ((clsPatientVO)this.DataContext).CivilID = mObj.AppointmentDetails.CivilId;
                    //((clsPatientVO)this.DataContext).NationalityID = mObj.AppointmentDetails.NationalityId;
                    ((clsPatientVO)this.DataContext).MaritalStatusID = mObj.AppointmentDetails.MaritalStatusId;
                    ((clsPatientVO)this.DataContext).GenderID = mObj.AppointmentDetails.GenderId;
                    ((clsPatientVO)this.DataContext).BloodGroupID = mObj.AppointmentDetails.BloodId;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = 2; // Appointment

                    //((clsPatientVO)this.DataContext).ContactNo1 = mObj.AppointmentDetails.ContactNo1;
                    //((clsPatientVO)this.DataContext).ContactNo2 = mObj.AppointmentDetails.ContactNo2;
                    txtResiCountryCode.Text = mObj.AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = mObj.AppointmentDetails.ResiSTDCode.ToString();
                    txtMobileCountryCode.Text = mObj.AppointmentDetails.MobileCountryCode.ToString();

                    txtContactNo1.Text = mObj.AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = mObj.AppointmentDetails.ContactNo2;
                    txtEmail.Text = mObj.AppointmentDetails.Email;

                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                    PatPersonalInfo.SelectedIndex = 0;
                    IsPatientExist = true;
                }
            };
            Client2.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client2.CloseAsync();
        }

        private void txtCountry_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtCountry_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtArea_TextChanged(object sender, RoutedEventArgs e)
        {
            if (txtArea.Text.Length > 50)
            {
                ((AutoCompleteBox)sender).Text = textBefore;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;

            }
        }

        private void txtArea_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }
        private void cmbPatientType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSpouseLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseLastName.Text = txtSpouseLastName.Text.ToTitleCase();
            ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName = txtSpouseLastName.Text;
        }

        private void txtSpouseMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseMiddleName.Text = txtSpouseMiddleName.Text.ToTitleCase();
        }

        private void txtSpouseFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseFirstName.Text = txtSpouseFirstName.Text.ToTitleCase();
        }

        private void txtSpouseFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseFamilyName.Text = txtSpouseFamilyName.Text.ToTitleCase();
        }

        private void dtpSpouseDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth > DateTime.Now)
            {
                txtSpouseYY.SetValidation("Age is Required");
                txtSpouseYY.RaiseValidationError();
                txtSpouseMM.SetValidation("Age is Required");
                txtSpouseMM.RaiseValidationError();
                txtSpouseDD.SetValidation("Age is Required");
                txtSpouseDD.RaiseValidationError();

                dtpSpouseDOB.SetValidation("Date of birth can not be greater than Today");
                dtpSpouseDOB.RaiseValidationError();
                txtSpouseYY.Text = "0";
                txtSpouseMM.Text = "0";
                txtSpouseDD.Text = "0";
            }
            else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
            {
                txtSpouseYY.SetValidation("Age is Required");
                txtSpouseYY.RaiseValidationError();
                txtSpouseMM.SetValidation("Age is Required");
                txtSpouseMM.RaiseValidationError();
                txtSpouseDD.SetValidation("Age is Required");
                txtSpouseDD.RaiseValidationError();

                dtpSpouseDOB.SetValidation("Please select the Date of birth");
                dtpSpouseDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else
            {
                dtpSpouseDOB.ClearValidationError();
                txtSpouseYY.ClearValidationError();
                txtSpouseMM.ClearValidationError();
                txtSpouseDD.ClearValidationError();

                txtSpouseYY.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "YY");
                txtSpouseMM.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "MM");
                txtSpouseDD.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "DD");
            }

            txtSpouseYY.SelectAll();
        }

        private void CmdIPD_Click(object sender, RoutedEventArgs e)
        {
            formSaveType = SaveType.IPD;
            SavePatientButton_Click(sender, e);
        }
        //Added By Yogita R
        private void CmdOPD_Click(object sender, RoutedEventArgs e)
        {
            formSaveType = SaveType.OPD;
            SavePatientButton_Click(sender, e);
        }
        //End
        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void FillCountry()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
            BizAction.Parent = new KeyValue();
            BizAction.Parent.Key = "1";
            BizAction.Parent.Value = "Status";
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtCountry.ItemsSource = null;
                    txtCountry.ItemsSource = objList.DeepCopy();
                    // txtCountry.SelectedItem = objList[0];

                    txtSpouseCountry.ItemsSource = null;
                    txtSpouseCountry.ItemsSource = objList.DeepCopy();
                }
                if (this.DataContext != null)
                {
                    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillCountry(long CountryID, long StateID, long CityID, long RegionID)
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_CountryMaster;
                BizAction.Parent = new KeyValue();
                BizAction.Parent.Key = "1";
                BizAction.Parent.Value = "Status";
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                        txtCountry.ItemsSource = null;
                        txtCountry.ItemsSource = objList.DeepCopy();

                        txtSpouseCountry.ItemsSource = null;
                        txtSpouseCountry.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                        txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                    }
                    FillState(CountryID, StateID, CityID, RegionID);
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
        }
        public void FillState(long CountryID, long StateID, long CityID, long RegionID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();

                    txtSpouseState.ItemsSource = null;
                    txtSpouseState.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                        txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                    FillCity(StateID, CityID, RegionID);
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID, long CityID, long RegionID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();

                    txtSpouseCity.ItemsSource = null;
                    txtSpouseCity.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                        txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                    }
                    FillRegion(CityID);
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillState(long CountryID)
        {
            clsGetStateDetailsByCountyIDBizActionVO BizAction = new clsGetStateDetailsByCountyIDBizActionVO();
            BizAction.CountryId = CountryID;
            BizAction.ListStateDetails = new List<clsStateVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {

                if (args.Error == null && args.Result != null)
                {

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails != null)
                    {
                        if (((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails.Count > 0)
                        {
                            foreach (clsStateVO item in ((clsGetStateDetailsByCountyIDBizActionVO)args.Result).ListStateDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }

                    txtState.ItemsSource = null;
                    txtState.ItemsSource = objList.DeepCopy();

                    txtSpouseState.ItemsSource = null;
                    txtSpouseState.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                        txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                    else
                    {
                        txtState.SelectedItem = objM;
                        txtSpouseState.SelectedItem = objM;
                    }

                }


            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillCity(long StateID)
        {
            clsGetCityDetailsByStateIDBizActionVO BizAction = new clsGetCityDetailsByStateIDBizActionVO();
            BizAction.StateId = StateID;
            BizAction.ListCityDetails = new List<clsCityVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListCityDetails = ((clsGetCityDetailsByStateIDBizActionVO)args.Result).ListCityDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListCityDetails != null)
                    {
                        if (BizAction.ListCityDetails.Count > 0)
                        {
                            foreach (clsCityVO item in BizAction.ListCityDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtCity.ItemsSource = null;
                    txtCity.ItemsSource = objList.DeepCopy();

                    txtSpouseCity.ItemsSource = null;
                    txtSpouseCity.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                        txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                    }
                    else
                    {
                        txtCity.SelectedItem = objM;
                        txtSpouseCity.SelectedItem = objM;
                    }

                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        public void FillRegion(long CityID)
        {
            clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
            BizAction.CityId = CityID;
            BizAction.ListRegionDetails = new List<clsRegionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            foreach (clsRegionVO item in BizAction.ListRegionDetails)
                            {
                                MasterListItem obj = new MasterListItem();
                                obj.ID = item.Id;
                                obj.Description = item.Description;
                                objList.Add(obj);
                            }
                        }
                    }
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();

                    txtSpouseArea.ItemsSource = null;
                    txtSpouseArea.ItemsSource = objList.DeepCopy();

                    if (this.DataContext != null)
                    {
                        txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
                        txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                    }
                    else
                    {
                        txtArea.SelectedItem = objM;
                        txtSpouseArea.SelectedItem = objM;
                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        private void txtPinCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
                }
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
        }

        private void txtDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtArea.ItemsSource = null;
                    txtArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSpouseCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSpouseCountry.SelectedItem != null && txtCountry.SelectedValue != null)

                if (((MasterListItem)txtSpouseCountry.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseState.ItemsSource = objList;
                    txtSpouseState.SelectedItem = objM;
                    txtSpouseCity.ItemsSource = objList;
                    txtSpouseCity.SelectedItem = objM;
                    txtSpouseArea.ItemsSource = objList;
                    txtSpouseArea.SelectedItem = objM;
                    FillState(((MasterListItem)txtSpouseCountry.SelectedItem).ID);
                }
        }

        private void txtSpouseState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSpouseState.SelectedItem != null && txtSpouseState.SelectedValue != null)
                if (((MasterListItem)txtSpouseState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseCity.ItemsSource = objList;
                    txtSpouseCity.SelectedItem = objM;
                    txtSpouseArea.ItemsSource = objList;
                    txtSpouseArea.SelectedItem = objM;
                    FillCity(((MasterListItem)txtSpouseState.SelectedItem).ID);
                }
        }

        private void txtSpouseDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSpouseArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSpouseCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSpouseState.SelectedItem != null && txtSpouseState.SelectedValue != null)
                if (((MasterListItem)txtSpouseState.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtSpouseArea.ItemsSource = null;
                    txtSpouseArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtSpouseCity.SelectedItem).ID);
                }
        }

        private void FillAgency()
        {

            cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO BizAction = new cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO();


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO DetailsVO = new cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO();
                    DetailsVO = (cls_IVFDashboard_GetAgencyListOfSurrogateBizActionVO)arg.Result;

                    List<cls_AgencyInfoVO> objList = new List<cls_AgencyInfoVO>();

                    for (int i = 0; i < DetailsVO.AgencyList.Count; i++)
                    {
                        cls_AgencyInfoVO obj = new cls_AgencyInfoVO();
                        //if (i == 0)
                        //{
                        //    obj.ID = 0;
                        //    obj.Description = "-- Select --";
                        //    objList.Add(obj);
                        //}
                        //else
                        //{
                        obj.Description = DetailsVO.AgencyList[i].Description;
                        //}

                        //obj = DetailsVO.AgencyList[i];
                        objList.Add(obj);
                    }
                    //txtSourceName.ItemsSource = null;
                    //txtSourceName.ItemsSource = objList;
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }


        long ReferralNamevalue = 0;
        private void cmbReferralName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbReferralName.SelectedItem != null)
            {
                ReferralNamevalue = ((MasterListItem)cmbReferralName.SelectedItem).ID;
                if (ReferralNamevalue > 0)
                    FillSurrogateAgency(ReferralNamevalue);
            }
        }


    }
}
