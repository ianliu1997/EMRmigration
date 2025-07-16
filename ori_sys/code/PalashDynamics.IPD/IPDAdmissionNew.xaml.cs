using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CIMS;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.IO;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using System.Reflection;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration.IPD;
using System.Windows.Media;
using System.Xml.Linq;
using CIMS.Forms;
using PalashDynamics.IPD.Forms;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects.Administration;
using System.Windows.Browser;

namespace PalashDynamics.IPD
{
    public partial class IPDAdmissionNew : UserControl, IInitiateCIMS
    {
        #region Variable Declarations
        bool Flagref = false;

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private bool PatientEditMode = false;
        public bool Status { get; set; }
        SponsorWindowIPD Sponsortwin = null;
        AdmissionDetailsWin Admissionwin = null;
        bool IsPageLoded = false;
        int ClickedFlag = 0;
        bool IsPatientExist = false;
        bool PatientSponsorNewMode = true;
        bool PatientAdmissionNewMode = true;
        //int ClickedFlag = 0;
        WaitIndicator Indicatior = null;
        bool IsSaveSponsor = true;
        #endregion

        #region IInitiateCIMS Members

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
                        CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID,
                        StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID,
                        CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID,
                        RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID,
                        Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                        PrefixId = 0,
                        IdentityID = 0,
                        IsInternationalPatient = false,
                    };

                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                    ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                    ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                    ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                    ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Female;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode = "+91";

                    break;

                case "EDIT":
                    #region Patient Edit Mode
                    PatientEditMode = true;
                    IsPatientExist = true;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
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
                                    txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode;
                                    txtOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName;
                                    txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();
                                    if (this.KinInformationListShow == null)
                                        this.KinInformationListShow = new List<clsKinInformationVO>();

                                    this.KinInformationListShow = ((clsPatientVO)this.DataContext).KinInformationList;
                                    BindKinInformation();
                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                    mElement.Text = " : " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName +
                                        " " + ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " +
                                        ((clsPatientVO)this.DataContext).GeneralDetails.LastName;

                                    PatientEditMode = true;
                                    CmdOPD.Visibility = System.Windows.Visibility.Collapsed;
                                }
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        PatientEditMode = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        IsPatientExist = false;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        PatientEditMode = false;
                        break;
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
                                this.DataContext = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;
                                if (this.DataContext != null)
                                {
                                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;

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

                                    txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();
                                    txtOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName;

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
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    };

                    Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client1.CloseAsync();

                    grpHeader.IsEnabled = false;
                    grpPersonalDetails.IsEnabled = false;
                    grpContactDetails.IsEnabled = false;
                    sponsorContent.IsEnabled = false;


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
                    };
                    #endregion
                    break;
            }

        }

        #endregion

        #region Constructor and Loaded
        public IPDAdmissionNew()
        {
            InitializeComponent();
        }

        void IPDAdmissionNew_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                ChangeBorderColors();
                CheckValidations();

                if (this.DataContext == null)
                {
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
                        Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                        MobileCountryCode = "+91"
                    };

                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                    ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                    ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                    ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                    ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Female;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode = "+91";
                }
                if (!PatientEditMode)
                {
                    //======================================
                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.SponsorWindowIPD") as UIElement;

                    Sponsortwin = new SponsorWindowIPD();
                    Sponsortwin = (SponsorWindowIPD)mydata;
                    ((IInitiateCIMS)Sponsortwin).Initiate("NEWR");
                    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                    Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                    dtpRegistrationDate.SelectedDate = System.DateTime.Now;
                    sponsorContent.Content = Sponsortwin;
                    //======================================
                    UIElement mydata1 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.AdmissionDetailsWin") as UIElement;
                    Admissionwin = new AdmissionDetailsWin();
                    Admissionwin = (AdmissionDetailsWin)mydata1;
                    ((IInitiateCIMS)Admissionwin).Initiate("NEWR");
                    Admissionwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                    AdmissionContent.Content = Admissionwin;
                    //======================================

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCountry.ItemsSource = objList;
                    txtCountry.SelectedItem = objM;
                    txtState.ItemsSource = objList;
                    txtState.SelectedItem = objM;
                    txtCity.ItemsSource = objList;
                    txtCity.SelectedItem = objM;
                    txtArea.ItemsSource = objList;
                    txtArea.SelectedItem = objM;

                    FillMaritalStatus();
                    FillPatientType();
                    FillGender();
                    FillIdentity();
                    FillRelation();
                    formSaveType = SaveType.Registration;
                    FillReferralName();
                    FillRefDoctor();
                    FillPreffix();
                    FillBloodGroup();
                    FillReligion();
                    FillOccupation();
                    FillOccupation();

                    FillCountry();
                    _captureSource = new CaptureSource();

                }
            }
            IsPageLoded = true;
        }
        #endregion

        #region Private Methods/Functions
        private void SavePatient(bool SavePatientRegister)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddPatientBizActionVO BizAction = new clsAddPatientBizActionVO();

            BizAction.IsAddWithtransaction = true;   // set to save Patient Details, Sponsor Details, Admission Details with transaction object to save/rollback all data at a time in case of any error
            BizAction.IsSavePatientFromIPD = true;    // set to decide save Patient Detials from IPD or not

            BizAction.IsSaveInTRegistration = SavePatientRegister; // set to decide save Patient Detials In T_Registration Table 

            BizAction.PatientDetails = (clsPatientVO)this.DataContext;
            BizAction.PatientDetails.IsIPDPatient = true;

            if (cmbMaritalStatus.SelectedItem != null && (((MasterListItem)cmbMaritalStatus.SelectedItem).ID != 0))
                BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            if (cmbBloodGroup.SelectedItem != null && ((MasterListItem)cmbBloodGroup.SelectedItem).ID != 0)
                BizAction.PatientDetails.BloodGroupID = ((MasterListItem)cmbBloodGroup.SelectedItem).ID;

            if (cmbGender.SelectedItem != null && ((MasterListItem)cmbGender.SelectedItem).ID != 0)
                BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbReligion.SelectedItem != null)
                BizAction.PatientDetails.ReligionID = ((MasterListItem)cmbReligion.SelectedItem).ID;

            if (cmbOccupation.SelectedItem != null)
                BizAction.PatientDetails.OccupationId = ((MasterListItem)cmbOccupation.SelectedItem).ID;

            if (cmbPatientType.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.PatientTypeID = ((MasterListItem)cmbPatientType.SelectedItem).ID;

            BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
            BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.ResiNoCountryCode = long.Parse(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.PatientDetails.ResiSTDCode = long.Parse(txtResiSTD.Text.Trim());

            if (cmbReferralName.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

            if (cmbReferralDoctor.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralDoctorID = ((MasterListItem)cmbReferralDoctor.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtReferralDetail.Text.Trim()))
                BizAction.PatientDetails.GeneralDetails.ReferralDetail = txtReferralDetail.Text.Trim();

            if (txtCountry.SelectedItem != null)
                BizAction.PatientDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.PatientDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.PatientDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.PatientDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;

            if (cmbIdentity.SelectedItem != null)
                BizAction.PatientDetails.IdentityID = ((MasterListItem)cmbIdentity.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtIdNumber.Text.Trim()))
                BizAction.PatientDetails.IdentityNumber = txtIdNumber.Text.Trim();

            if (cmbPreffix.SelectedItem != null)
                BizAction.PatientDetails.PrefixId = ((MasterListItem)cmbPreffix.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtRemark.Text.Trim()))
                BizAction.PatientDetails.RemarkForPatientType = txtRemark.Text.Trim();

            //Added by AJ Date4/11/2016
            if (cmbEducation.SelectedItem != null)
                BizAction.PatientDetails.EducationID = ((MasterListItem)cmbEducation.SelectedItem).ID;            
            //***//----------
            if (ChkInternationalPatient.IsChecked == true)
                BizAction.PatientDetails.IsInternationalPatient = true;
            else
                BizAction.PatientDetails.IsInternationalPatient = false;

            //======================================================
            BizAction.PatientDetails.KinInformationList = ((List<clsKinInformationVO>)dgKinInfo.ItemsSource);

            //Commented by AJ Date 14/10/2016
            //if (sponsorContent.Content != null && PatientEditMode == false && PatientSponsorNewMode == false)  
            //{
            //    if (IsSearchOnList == true) BizAction.IsSaveSponsor = IsSaveSponsor;
            //    else BizAction.IsSaveSponsor = true;
            //    BizAction.BizActionVOSaveSponsor = SaveSponsorWithTransaction();
            //}

            if (AdmissionContent.Content != null && PatientEditMode == false && PatientAdmissionNewMode == false)
            {
                BizAction.IsSaveAdmission = true;
                BizAction.BizActionVOSaveAdmission = SaveAdmissionWithrTransaction();
            }

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    ClickedFlag = 0;
                    CmdSave.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientID = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                    ((clsPatientVO)this.DataContext).GeneralDetails.MRNo = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                    ((clsPatientVO)this.DataContext).GeneralDetails.UnitId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId;
                    ((clsPatientVO)this.DataContext).SpouseDetails.PatientID = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.SpouseDetails.PatientID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.UnitId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.SpouseDetails.UnitId;
                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.ClassID = ((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveAdmission.List[0].BedCategoryID;
                    ((IApplicationConfiguration)App.Current).SelectedPatient.BillingToBedCategoryID = ((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveAdmission.List[0].BillingToBedCategoryID;
                   


                    GetAdmissionList(((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID, ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId);
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
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Admitted Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (MessageBoxResult re) =>
                    {
                        if (re == MessageBoxResult.OK)
                        {
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.Forms.frmAdmissionList") as UIElement;
                            ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);

                            string msgText = "";
                            msgText = "Are You Sure \n You Want To Print Patient Admission Report?";
                            MessageBoxControl.MessageBoxChildWindow msgW2 =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Information);
                            msgW2.OnMessageBoxClosed += (MessageBoxResult re1) =>
                            {
                                if (re1 == MessageBoxResult.Yes)
                                {
                                    if (((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveAdmission.Details.ID > 0 && ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId > 0)
                                    {

                                        string URL = "../Reports/IPD/PatientAdmissionSheet.aspx?AdmissionID=" + ((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveAdmission.Details.ID + "&AdmissionUnitID=" + ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId;
                                        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
                                    }
                                }
                            };
                            msgW2.Show();
                        }
                    };
                    msgW1.Show();

                    
                }
                else
                {
                    CmdSave.IsEnabled = true;
                    Indicatior.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occurred while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    ClickedFlag = 0;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        void rptPatientCase(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                long ID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                long UnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;

               
            }
        }

        private void GetAdmissionList(long Patid, long patUid)  //, long PatientSoID, long PatTarID
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetIPDAdmissionListBizActionVO BizAction = new clsGetIPDAdmissionListBizActionVO();
                BizAction.AdmList = new List<clsIPDAdmissionVO>();
                BizAction.AdmDetails = new clsIPDAdmissionVO();

                if (!((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    BizAction.AdmDetails.PatientId = Patid;
                    BizAction.AdmDetails.PatientUnitID = patUid;
                }
                else
                {
                    BizAction.AdmDetails.PatientId = Patid;
                    BizAction.AdmDetails.PatientUnitID = patUid;
                    BizAction.AdmDetails.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                }
                BizAction.IsPagingEnabled = true;
                BizAction.StartIndex = 0; 
                BizAction.MaximumRows = 1; 

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {

                        clsGetIPDAdmissionListBizActionVO result = arg.Result as clsGetIPDAdmissionListBizActionVO;
                        if (result.AdmList != null && result.AdmList.Count > 0)
                        {                            
                            for (int i = 0; i < 1; i++)
                            {
                                ((IApplicationConfiguration)App.Current).SelectedIPDPatient = result.AdmList[i];
                            }
                        }
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
            finally
            {

            }
        }

        private void SaveAdmission()
        {
            try
            {
                WaitIndicator Indicatior = new WaitIndicator();
                Indicatior.Show();
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID; ;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientUnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientSourceID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).IPDNO = ((clsPatientVO)this.DataContext).GeneralDetails.MRNo;
                clsSaveIPDAdmissionBizActionVO BizAction = new clsSaveIPDAdmissionBizActionVO();
                BizAction.Details = ((clsIPDAdmissionVO)Admissionwin.DataContext);
                BizAction.List = new List<clsIPDBedMasterVO>();
                if (Admissionwin.cmbUnit.SelectedItem != null)
                    BizAction.Details.UnitId = ((MasterListItem)Admissionwin.cmbUnit.SelectedItem).ID;
                if (Admissionwin.cmbDepartmet.SelectedItem != null)
                    BizAction.Details.DepartmentID = ((MasterListItem)Admissionwin.cmbDepartmet.SelectedItem).ID;
                if (Admissionwin.cmbDoctor.SelectedItem != null)
                    BizAction.Details.DoctorID = ((MasterListItem)Admissionwin.cmbDoctor.SelectedItem).ID;
                if (Admissionwin.dtpEffectiveDate.SelectedDate != null)
                    BizAction.Details.Date = (DateTime)Admissionwin.dtpEffectiveDate.SelectedDate.Value.Date;
                if (Admissionwin.tpToTime.Value != null)
                    BizAction.Details.Date = BizAction.Details.Date.Value.Add(Admissionwin.tpToTime.Value.Value.TimeOfDay);

                 BizAction.Details.Time = DateTime.Now;

                if (Admissionwin.cmbAdmissionType.SelectedItem != null)
                    BizAction.Details.AdmissionTypeID = ((MasterListItem)Admissionwin.cmbAdmissionType.SelectedItem).ID;
                if (Admissionwin.cmbRefEntity.SelectedItem != null)
                    BizAction.Details.RefEntityID = ((MasterListItem)Admissionwin.cmbRefEntity.SelectedItem).ID;
                if (Admissionwin.cmbRefEntityType.SelectedItem != null)
                    BizAction.Details.RefEntityTypeID = ((MasterListItem)Admissionwin.cmbRefEntityType.SelectedItem).ID;
                if (Admissionwin.cmbPatSource.SelectedItem != null)
                    BizAction.Details.PatientSourceID = ((MasterListItem)Admissionwin.cmbPatSource.SelectedItem).ID;
                BizAction.Details.ProvisionalDiagnosis = Admissionwin.txtProvisionalDiagnosis.Text;
                BizAction.Details.Remarks = Admissionwin.txtRemarks.Text;
                foreach (clsIPDBedMasterVO item in Admissionwin.MasterList)
                {
                    if (item.Status == true)
                    {
                        if (Admissionwin.cmbBilling.SelectedItem != null)
                            item.BillingToBedCategoryID = ((MasterListItem)Admissionwin.cmbBilling.SelectedItem).ID;

                        BizAction.List.Add(item);
                    }
                }

                foreach (clsIPDBedMasterVO item in Admissionwin.MasterNonCensusList)
                {
                    if (item.Status == true)
                    {
                        if (Admissionwin.cmbBilling.SelectedItem != null)
                            item.BillingToBedCategoryID = ((MasterListItem)Admissionwin.cmbBilling.SelectedItem).ID;

                        BizAction.List.Add(item);
                    }
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {

                        if (arg.Result != null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Admitted Sucessfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.OK)
                                {
                                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.Forms.frmAdmissionList") as UIElement;
                                    ((IApplicationConfiguration)App.Current).OpenMainContent(mydata);
                                }
                            };
                            msgW1.Show();
                        }
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
                Indicatior.Close();
            }
            catch (Exception ex) { throw ex; }
        }

        private clsSaveIPDAdmissionBizActionVO SaveAdmissionWithrTransaction()
        {
            try
            {
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID; ;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientUnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).PatientSourceID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
                ((clsIPDAdmissionVO)Admissionwin.DataContext).IPDNO = ((clsPatientVO)this.DataContext).GeneralDetails.MRNo;

                clsSaveIPDAdmissionBizActionVO BizAction = new clsSaveIPDAdmissionBizActionVO();

                BizAction.Details = ((clsIPDAdmissionVO)Admissionwin.DataContext);
                BizAction.List = new List<clsIPDBedMasterVO>();

                if (Admissionwin.cmbUnit.SelectedItem != null)
                    BizAction.Details.UnitId = ((MasterListItem)Admissionwin.cmbUnit.SelectedItem).ID;
                if (Admissionwin.cmbDepartmet.SelectedItem != null)
                    BizAction.Details.DepartmentID = ((MasterListItem)Admissionwin.cmbDepartmet.SelectedItem).ID;
                if (Admissionwin.cmbDoctor.SelectedItem != null)
                    BizAction.Details.DoctorID = ((MasterListItem)Admissionwin.cmbDoctor.SelectedItem).ID;
                if (Admissionwin.dtpEffectiveDate.SelectedDate != null)
                    BizAction.Details.Date = (DateTime)Admissionwin.dtpEffectiveDate.SelectedDate.Value.Date;
                if (Admissionwin.tpToTime.Value != null)
                    BizAction.Details.Date = BizAction.Details.Date.Value.Add(Admissionwin.tpToTime.Value.Value.TimeOfDay);

                if (Admissionwin.cmbAdmissionType.SelectedItem != null)
                    BizAction.Details.AdmissionTypeID = ((MasterListItem)Admissionwin.cmbAdmissionType.SelectedItem).ID;
                if (Admissionwin.cmbRefEntity.SelectedItem != null)
                    BizAction.Details.RefEntityID = ((MasterListItem)Admissionwin.cmbRefEntity.SelectedItem).ID;
                if (Admissionwin.cmbRefEntityType.SelectedItem != null)
                    BizAction.Details.RefEntityTypeID = ((MasterListItem)Admissionwin.cmbRefEntityType.SelectedItem).ID;
                if (Admissionwin.cmbPatSource.SelectedItem != null)
                    BizAction.Details.PatientSourceID = ((MasterListItem)Admissionwin.cmbPatSource.SelectedItem).ID;

                BizAction.Details.ProvisionalDiagnosis = Admissionwin.txtProvisionalDiagnosis.Text;
                BizAction.Details.Remarks = Admissionwin.txtRemarks.Text;

                foreach (clsIPDBedMasterVO item in Admissionwin.MasterList)
                {
                    if (item.Status == true)
                    {
                        if (Admissionwin.cmbBilling.SelectedItem != null)
                            item.BillingToBedCategoryID = ((MasterListItem)Admissionwin.cmbBilling.SelectedItem).ID;

                        BizAction.List.Add(item);
                    }
                }

                foreach (clsIPDBedMasterVO item in Admissionwin.MasterNonCensusList)
                {
                    if (item.Status == true)
                    {
                        if (Admissionwin.cmbBilling.SelectedItem != null)
                            item.BillingToBedCategoryID = ((MasterListItem)Admissionwin.cmbBilling.SelectedItem).ID;

                        BizAction.List.Add(item);
                    }
                }
                return BizAction;
            }
            catch (Exception ex)
            {
                //throw ex;
                string err = ex.Message;
                return null;
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

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {

                        }
                        if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 7) //Couple
                        {
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).ID = 0;
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).SpouseDetails.PatientID;
                            ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).SpouseDetails.UnitId;
                            BizAction.PatientSponsorDetails = (clsPatientSponsorVO)Sponsortwin.DataContext;
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
                            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            Client1.ProcessCompleted += (s1, arg1) =>
                            {
                                if (arg.Error == null && arg.Result != null)
                                {

                                }
                                else
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Spouse Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                    ClickedFlag = 0;
                                }

                            };

                            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            Client1.CloseAsync();
                        }


                        sponsorContent.Content = null;
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        ClickedFlag = 0;
                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                Indicatior.Close();
            }
        }

        private clsAddPatientSponsorBizActionVO SaveSponsorWithTransaction()
        {
           try
            {
                ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails = (clsPatientSponsorVO)Sponsortwin.DataContext;
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

                return BizAction;           
            }
            catch (Exception ex)
            {
                string err = ex.Message;               
                return null;
            }
        }

        enum RegistrationTabs
        {            
            Patient = 0,
            Admission = 1,
            Sponsor = 2
        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {                
                if (cmbReferralName.SelectedItem == null || ((MasterListItem)cmbReferralName.SelectedItem).ID == 0)
                {
                    cmbReferralName.TextBox.SetValidation("Gender is required");
                    cmbReferralName.TextBox.RaiseValidationError();
                    cmbReferralName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if (cmbPreffix.SelectedItem == null || ((MasterListItem)cmbPreffix.SelectedItem).ID == 0)
                {
                    cmbPreffix.TextBox.SetValidation("Gender is required");
                    cmbPreffix.TextBox.RaiseValidationError();
                    cmbPreffix.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if (txtContactNo1.Text == "")
                {
                    txtContactNo1.Textbox.SetValidation("Please Enter Mobile No.");
                    txtContactNo1.Textbox.RaiseValidationError();
                    txtContactNo1.Focus();
                    result = false;
                }
                else
                    txtContactNo1.Textbox.ClearValidationError();

                //if (txtAddress1.Text == "")
                //{
                //    txtAddress1.SetValidation("Address is Required.");
                //    txtAddress1.RaiseValidationError();
                //    txtAddress1.Focus();
                //    result = false;
                //}
                //else
                //    txtAddress1.ClearValidationError();

                if (cmbGender.SelectedItem == null || ((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Gender is required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbGender.TextBox.ClearValidationError();

                //if (cmbIdentity.SelectedItem == null || ((MasterListItem)cmbIdentity.SelectedItem).ID == 0)
                //{
                //    cmbIdentity.TextBox.SetValidation("Please Select Identity");
                //    cmbIdentity.TextBox.RaiseValidationError();
                //    cmbIdentity.TextBox.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbIdentity.TextBox.ClearValidationError();

                //if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID > 0 && String.IsNullOrEmpty(txtIdNumber.Text))
                //{
                //    txtIdNumber.SetValidation("Please, Enter Identity Number");
                //    txtIdNumber.RaiseValidationError();
                //    txtIdNumber.Focus();
                //    result = false;
                //}
                //else
                //    txtIdNumber.ClearValidationError();

                //if (cmbMaritalStatus.SelectedItem == null || ((MasterListItem)cmbMaritalStatus.SelectedItem).ID == 0)
                //{
                //    cmbMaritalStatus.TextBox.SetValidation("Please, Select Marital Status");
                //    cmbMaritalStatus.TextBox.RaiseValidationError();
                //    cmbMaritalStatus.TextBox.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbMaritalStatus.TextBox.ClearValidationError();

              //----Commented by Aj Date 3/11/2016
                //if (String.IsNullOrEmpty(txtLastName.Text))
                //{
                //    txtLastName.SetValidation("Last Name is required");
                //    txtLastName.RaiseValidationError();
                //    txtLastName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;

                //}
                //else
                //    txtLastName.ClearValidationError();
              //***//---------------------
                if (String.IsNullOrEmpty(txtFirstName.Text))
                {
                    txtFirstName.SetValidation("First Name is required.");
                    txtFirstName.RaiseValidationError();
                    txtFirstName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtFirstName.ClearValidationError();
                //if (String.IsNullOrEmpty(txtEmail.Text))
                //{
                //    txtEmail.SetValidation("Please Enter Email ID");
                //    txtEmail.RaiseValidationError();
                //    txtEmail.Focus();
                //}
                //else
                //    txtEmail.ClearValidationError();
                //if (String.IsNullOrEmpty(txtDD.Text))
                //{
                //    txtDay.SetValidation("Please Enter Day");
                //    txtDay.RaiseValidationError();
                //    txtDay.Focus();
                //}
                //else
                //    txtDay.ClearValidationError();
                //if (String.IsNullOrEmpty(txtMM.Text))
                //{
                //    txtMonth.SetValidation("Please Enter Month");
                //    txtMonth.RaiseValidationError();
                //    txtMonth.Focus();
                //}
                //else
                //    txtMonth.ClearValidationError();
                //if (String.IsNullOrEmpty(txtYY.Text))
                //{
                //    txtYear.SetValidation("Please Enter Birth Year");
                //    txtYear.RaiseValidationError();
                //    txtYear.Focus();
                //}
                //else
                //    txtYear.ClearValidationError();
                if (string.IsNullOrEmpty(txtYY.Text) && string.IsNullOrEmpty(txtMM.Text) && string.IsNullOrEmpty(txtDD.Text))
                {
                    txtYY.SetValidation("Age Is Required");
                    txtYY.RaiseValidationError();
                    txtMM.SetValidation("Age Is Required");
                    txtMM.RaiseValidationError();
                    txtDD.SetValidation("Age Is Required");
                    txtDD.RaiseValidationError();
                    result = false;
                    txtYY.Focus();
                    //  PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                }
                else
                {
                    txtYY.ClearValidationError();
                    txtMM.ClearValidationError();
                    txtDD.ClearValidationError();
                }

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

                //if (String.IsNullOrEmpty(txtYear.Text))
                //{
                //    if (txtYear.Text.Length > 4)
                //    {
                //        txtYear.SetValidation("Please enter valid birth year");
                //        txtYear.RaiseValidationError();
                //        txtYear.Focus();
                //        result = false;
                //    }
                //    else if (txtYear.Text.Length < 4)
                //    {
                //        txtYear.SetValidation("Please enter valid birth year");
                //        txtYear.RaiseValidationError();
                //        txtYear.Focus();
                //        result = false;
                //    }
                //    else
                //        txtYear.ClearValidationError();
                //}


                //if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null)
                //{
                //    dtpDOB.SetValidation("Birth Date is required");
                //    dtpDOB.RaiseValidationError();
                //    dtpDOB.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth.Value.Date >= DateTime.Now.Date)
                //{
                //    dtpDOB.SetValidation("Birth Date can not be greater than Today's Date");
                //    dtpDOB.RaiseValidationError();
                //    dtpDOB.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    dtpDOB.ClearValidationError();


                if (!String.IsNullOrEmpty(txtYY.Text))
                {
                    if (Convert.ToInt16(txtYY.Text) > 120)
                    {
                        txtYY.SetValidation("Age can not be greater than 121");
                        txtYY.RaiseValidationError();
                        txtYY.Focus();
                        result = false;
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    }
                    else
                        txtYY.ClearValidationError();
                }

            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }

        private void GetAreaList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
            BizAction.TableName = "T_Registration";
            BizAction.ColumnName = "Area";
            BizAction.IsDecode = true;
            if (!string.IsNullOrEmpty(pCity))
            {
                pCity = pCity.Trim();
                BizAction.Parent = new KeyValue { Key = "City", Value = pCity };
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = ((clsGetAutoCompleteListVO)e.Result).List.DeepCopy();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetPinCodeList(string pCity)
        {
            clsGetAutoCompleteListVO BizAction = new clsGetAutoCompleteListVO();
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
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void ChangeBorderColors()
        {
            cmbMaritalStatus.BorderBrush = new SolidColorBrush(Colors.Red);
            cmbIdentity.BorderBrush = new SolidColorBrush(Colors.Red);
            cmbGender.BorderBrush = new SolidColorBrush(Colors.Red);
        }
        #endregion

        #region Fill ComboBoxes
        private void FillMaritalStatus()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
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
                    cmbMaritalStatus.ItemsSource = null;
                    cmbMaritalStatus.ItemsSource = objList.DeepCopy();
                    cmbMaritalStatus.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPatientType()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
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

                    cmbPatientType.ItemsSource = null;
                    cmbPatientType.ItemsSource = objList;
                    cmbPatientType.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillGender()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
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
                    cmbGender.SelectedItem = objList[0];
                }
                FillEducationMaster(); //Added by AJ Date 3/11/2016
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        //Added by AJ Date 3/11/2016
        private void FillEducationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();             
                BizAction.MasterTable = MasterTableNameList.M_EducationDetailsMaster;
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
                        cmbEducation.ItemsSource = null;
                        cmbEducation.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbEducation.SelectedValue = ((clsPatientVO)this.DataContext).EducationID;                      
                    }
                    

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {                
                throw;
            }
        }
        //***//---------------------

        List<MasterListItem> objRelationShipList = new List<MasterListItem>();
        private void FillRelation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
            BizAction.IsActive = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {

                    objRelationShipList.Add(new MasterListItem(0, "- Select -"));
                    objRelationShipList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbRelationship.ItemsSource = null;
                    cmbRelationship.ItemsSource = objRelationShipList;
                    cmbRelationship.SelectedItem = objRelationShipList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillIdentity()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IdentityMaster;
            BizAction.IsActive = true;
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
                    cmbIdentity.ItemsSource = null;
                    cmbIdentity.ItemsSource = objList;
                    cmbIdentity.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //**Added By Ashish on 26-10-2015
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

        private void FillRefDoctor()
        {
            clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {

                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetRefernceDoctorBizActionVO)e.Result).ComboList != null)
                    {
                        clsGetRefernceDoctorBizActionVO result = (clsGetRefernceDoctorBizActionVO)e.Result;
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        if (result.ComboList != null)
                        {
                            foreach (var item in result.ComboList)
                            {
                                MasterListItem Objmaster = new MasterListItem();
                                Objmaster.ID = item.ID;
                                Objmaster.Description = item.Value;
                                objList.Add(Objmaster);
                            }
                        }
                        cmbReferralDoctor.ItemsSource = null;
                        cmbReferralDoctor.ItemsSource = objList;
                        cmbReferralDoctor.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            cmbReferralDoctor.SelectedValue = ((clsPatientVO)this.DataContext).ReferralDoctorID;
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillPreffix()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_Preffixmaster;
            BizAction.IsActive = true;
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
                    cmbPreffix.ItemsSource = null;
                    cmbPreffix.ItemsSource = objList.DeepCopy();
                }
                if (this.DataContext != null)
                {
                    cmbPreffix.SelectedValue = ((clsPatientVO)this.DataContext).PrefixId;
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
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbBloodGroup.ItemsSource = null;
                    cmbBloodGroup.ItemsSource = objList.DeepCopy();
                }
                if (this.DataContext != null)
                {
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;                    
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
                }
                if (this.DataContext != null)
                {
                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
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
                }
                if (this.DataContext != null)
                {
                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
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
                }
                if (this.DataContext != null)
                {
                    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
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
                    MasterListItem objM = new MasterListItem(0, "- Select -");
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
                    if (this.DataContext != null)
                    {
                        txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                    }
                    else
                    {
                        txtState.SelectedItem = objM;
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
                    MasterListItem objM = new MasterListItem(0, "- Select -");
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
                    if (this.DataContext != null)
                    {
                        txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                    }
                    else
                    {
                        txtCity.SelectedItem = objM;
                    }
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        List<clsRegionVO> RegionList;
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
                    MasterListItem objM = new MasterListItem(0, "- Select -");
                    objList.Add(objM);
                    if (BizAction.ListRegionDetails != null)
                    {
                        if (BizAction.ListRegionDetails.Count > 0)
                        {
                            RegionList = new List<clsRegionVO>();
                            RegionList = BizAction.ListRegionDetails;
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
                    if (this.DataContext != null)
                    {
                        txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
                    }
                    else
                    {
                        txtArea.SelectedItem = objM;
                    }
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }

        //**
        #endregion

        #region Button Click Events
        private void ChkInternationalPatient_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                cmbIdentity.SelectedValue = (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient);
            }
        }

        private void CmdOPD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SavePatientButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {

                bool saveDtls = true;
                saveDtls = CheckValidations();
                if (saveDtls == true)
                {
                    if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                }
                //============================================Admission Window===================================================//
                if (saveDtls == true)
                    if (saveDtls == true && Admissionwin != null && PatientAdmissionNewMode == false && PatientEditMode == false)
                    {
                        Admissionwin.IsFromLoaded = true;  // flag for showing the Error msg for Bed Selection.
                        saveDtls = Admissionwin.CheckValidations();
                        if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Admission;
                    }
                    else
                    {
                        saveDtls = false;
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Admission;
                    }
                //============================================Sponsor Window===================================================//
                if (saveDtls == true)
                    if (saveDtls == true && Sponsortwin != null && PatientSponsorNewMode == false && PatientEditMode == false)
                    {
                        if (IsSearchOnList == false)
                            saveDtls = Sponsortwin.CheckValidations();
                        else
                        {
                            if (!saveDtls == Sponsortwin.CheckValidationFromSearchList())
                            {
                                if (Sponsortwin.IsMsgShow == true)
                                {
                                    saveDtls = false;
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
                                }
                                IsSaveSponsor = false;
                            }
                            else IsSaveSponsor = true;
                        }
                        if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
                    }
                    else
                    {
                        saveDtls = false;
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
                    }
                //================================================================================================================//
                if (saveDtls == true)
                {
                    if (PatientEditMode == false)
                    {
                        clsCheckPatientDuplicacyBizActionVO BizAction = new clsCheckPatientDuplicacyBizActionVO();
                        BizAction.PatientDetails = ((clsPatientVO)this.DataContext);
                        if ((MasterListItem)cmbGender.SelectedItem != null)
                            BizAction.PatientDetails.GeneralDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, ea) =>
                        {
                            ClickedFlag = 0;
                            if (ea.Error == null && ea.Result != null)
                            {
                                if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).ResultStatus == true && IsSearchOnList == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Same Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.OnMessageBoxClosed += (re) =>
                                    {
                                    };
                                    msgW1.Show();
                                    ClickedFlag = 0;
                                }
                                else
                                {
                                    string msgText = "";
                                    msgText = "Are you sure you want to save the Patient Details ?";
                                    MessageBoxControl.MessageBoxChildWindow msgW =
                                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                                    {
                                        if (res == MessageBoxResult.Yes)
                                        {
                                            if (res == MessageBoxResult.Yes && IsSearchOnList == false)
                                            {
                                                SavePatient(true);
                                            }
                                            else if (res == MessageBoxResult.Yes && IsSearchOnList == true && PatientEditMode == false)
                                            {
                                                SavePatient(false);
                                            }
                                        }
                                    };
                                    msgW.Show();
                                    ClickedFlag = 0;
                                }
                            }

                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                    else
                    {
                        string msgText = "";
                        msgText = "Are you sure you want to save the Patient Details ?";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                        {
                            if (res == MessageBoxResult.Yes)
                            {
                                if (res == MessageBoxResult.Yes)
                                    SavePatient(true);
                            }

                        };
                        msgW.Show();
                        ClickedFlag = 0;
                    }

                }
                else
                    ClickedFlag = 0;

            }
            else
                ClickedFlag = 0;
        }

        void ObjPatientExists_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            string msgText = "";
            msgText = "Are you sure you want to save the Patient Details ?";
            MessageBoxControl.MessageBoxChildWindow msgW =
                  new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
            {
                if (res == MessageBoxResult.Yes)
                {
                    if (res == MessageBoxResult.Yes)
                        SavePatient(true);
                    //SavePatient();
                }
            };
            msgW.Show();
        }

        private enum SaveType
        {
            Registration = 0,
            IPD = 1,
            OPD = 2,
            Billing = 3
        }

        private SaveType formSaveType = SaveType.Registration;

        private void ClosePatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            frmAdmissionList _AdmissionListObject = new frmAdmissionList();
            ((IApplicationConfiguration)App.Current).OpenMainContent(_AdmissionListObject);
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
            mElement.Text = "Admission List";


        }

        private void txtCountry_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = String.Empty;
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
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
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
        void phWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((CIMS.Forms.PhotoWindow)sender).DialogResult == true)
                ((clsPatientVO)this.DataContext).Photo = ((CIMS.Forms.PhotoWindow)sender).MyPhoto;
            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
            bmp.FromByteArray(((CIMS.Forms.PhotoWindow)sender).MyPhoto);
            imgPhoto.Source = bmp;

        }

        #region UnUsed Lost Focus Events
        private void txtDistrict_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtCountry_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtTaluka_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseFirstName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseLastName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dtpSpouseDOB_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseYY_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseMM_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseDD_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void btnSpousePhoto_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseEmail_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtSpouseArea_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtState_LostFocus(object sender, RoutedEventArgs e)
        {

        }
        private void cmbGender_LosstFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth > DateTime.Now)
            {
                txtYY.SetValidation("Age Is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age Is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age Is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Date of Birth Can Not Be Greater Than Today");
                dtpDOB.RaiseValidationError();
                txtYY.Text = "0";
                txtMM.Text = "0";
                txtDD.Text = "0";
            }
            else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null)
            {
                txtYY.SetValidation("Age Is Required");
                txtYY.RaiseValidationError();
                txtMM.SetValidation("Age Is Required");
                txtMM.RaiseValidationError();
                txtDD.SetValidation("Age Is Required");
                txtDD.RaiseValidationError();

                dtpDOB.SetValidation("Please Select The Date of Birth");
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
        #endregion

        private void cmdAddRegisteredKinMember_Click(object sender, RoutedEventArgs e)
        {
            grpOtherKinDetails.Visibility = Visibility.Collapsed;
            PatientSearch Win = new PatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnKinInfoSaveButton_Click);
            Win.Show();
        }

        void Win_OnKinInfoSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                String tmpMobileNumber;

                if (((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1 == "" || ((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1 == String.Empty)
                    tmpMobileNumber = null;
                else
                    tmpMobileNumber = ((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1;


                if (KinInformationListShow.Count > 0)
                {
                    var item1 = from r in KinInformationListShow
                                where (r.KinPatientID == ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID)
                                select r;
                    if (item1.ToList().Count == 0)
                    {
                        clsKinInformationVO objItem = new clsKinInformationVO();
                        objItem.Address = "";
                        objItem.IsRegisteredPatient = true;
                        objItem.KinFirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                        objItem.KinMiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                        objItem.KinLastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                        objItem.MobileCountryCode = 0000;
                        objItem.MobileNumber = Convert.ToString(tmpMobileNumber);
                        objItem.Relationship = objRelationShipList[0].Description;
                        objItem.RelationshipID = objRelationShipList[0].ID;
                        objItem.KinPatientID = Convert.ToInt64(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID);
                        objItem.KinPatientUnitID = Convert.ToInt64(((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                        objItem.MRCode = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        KinInformationListShow.Add(objItem);

                    }
                }
                else
                {

                    clsKinInformationVO objItem = new clsKinInformationVO();
                    objItem.Address = ((IApplicationConfiguration)App.Current).SelectedPatient.AddressLine1;  // "";
                    objItem.IsRegisteredPatient = true;
                    objItem.KinFirstName = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName;
                    objItem.KinMiddleName = ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName;
                    objItem.KinLastName = ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    objItem.MobileCountryCode = 0000;
                    objItem.MobileNumber = Convert.ToString(tmpMobileNumber);
                    objItem.KinPatientID = Convert.ToInt64(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID);
                    objItem.KinPatientUnitID = Convert.ToInt64(((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                    objItem.MRCode = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    KinInformationListShow.Add(objItem);
                }


                ClearKinInfoControl();

                BindKinInformation();
            }
        }

        private void cmdNewKinMember_Click(object sender, RoutedEventArgs e)
        {
            grpOtherKinDetails.Visibility = Visibility.Visible;
        }
        #endregion

        #region Save KinInformation
        private void SaveKinInforamtion()
        {

        }

        public clsKinInformationVO RemoveableItem { get; set; }
        private List<clsKinInformationVO> _KinInformationListShow = new List<clsKinInformationVO>();
        public List<clsKinInformationVO> KinInformationListShow
        {
            get { return _KinInformationListShow; }
            set { _KinInformationListShow = value; }
        }

        private void cmdAddKinMember_Click(object sender, RoutedEventArgs e)
        {
            bool saveKinDtls = true;

            saveKinDtls = CheckKinInfo();
            if (saveKinDtls == true)
            {
                string viewMrNo = "";
                if (RemoveableItem != null)
                    viewMrNo = RemoveableItem.MRCode;

                if (RemoveableItem != null)
                    this.KinInformationListShow.Remove(RemoveableItem);

                clsKinInformationVO objItem = new clsKinInformationVO();
                if (!String.IsNullOrEmpty(txtKinAddress.Text))
                    objItem.Address = txtKinAddress.Text.Trim();
                objItem.IsRegisteredPatient = false;
                if (!String.IsNullOrEmpty(txtKinFirstName.Text))
                    objItem.KinFirstName = txtKinFirstName.Text.Trim();
                if (!String.IsNullOrEmpty(txtKinMiddleName.Text))
                    objItem.KinMiddleName = txtKinMiddleName.Text.Trim();
                if (!String.IsNullOrEmpty(txtKinLastName.Text))
                    objItem.KinLastName = txtKinLastName.Text.Trim();
                if (txtKinCountryCode.Text.Trim() != String.Empty)
                    objItem.MobileCountryCode = Convert.ToInt64(txtKinCountryCode.Text.Trim());
                if (txtKinContactNo1.Text.Trim() != String.Empty)
                    objItem.MobileNumber = txtKinContactNo1.Text.Trim();
                if (cmbRelationship.SelectedItem != null)
                {
                    objItem.Relationship = ((MasterListItem)cmbRelationship.SelectedItem).Description;
                    objItem.RelationshipID = ((MasterListItem)cmbRelationship.SelectedItem).ID;
                }

                objItem.MRCode = viewMrNo;

                KinInformationListShow.Add(objItem);
                ClearKinInfoControl();
                dgKinInfo.ItemsSource = null;
                dgKinInfo.ItemsSource = KinInformationListShow;
            }
        }

        private void BindKinInformation()
        {
            dgKinInfo.ItemsSource = null;
            dgKinInfo.ItemsSource = KinInformationListShow;
        }

        private void ClearKinInfoControl()
        {
            txtKinAddress.Text = String.Empty;
            txtKinFirstName.Text = String.Empty;
            txtKinMiddleName.Text = String.Empty;
            txtKinLastName.Text = String.Empty;
            txtKinCountryCode.Text = String.Empty;
            txtKinContactNo1.Text = String.Empty;
            cmbRelationship.SelectedItem = objRelationShipList[0];
        }

        private bool CheckKinInfo()
        {
            bool result = true;
            try
            {
                if (txtKinFirstName.Text == String.Empty)
                {
                    txtKinFirstName.SetValidation("Kin First Name is required");
                    txtKinFirstName.RaiseValidationError();
                    txtKinFirstName.Focus();
                    result = false;
                }
                else
                    txtKinFirstName.ClearValidationError();

                if (txtKinLastName.Text == String.Empty)
                {
                    txtKinLastName.SetValidation("Kin Last Name is required");
                    txtKinLastName.RaiseValidationError();
                    txtKinLastName.Focus();
                    result = false;
                }
                else
                    txtKinLastName.ClearValidationError();
                if (cmbRelationship.SelectedItem == null)
                {
                    cmbRelationship.TextBox.SetValidation("Please, Select Relationship");
                    cmbRelationship.TextBox.RaiseValidationError();
                    cmbRelationship.TextBox.Focus();
                    result = false;
                }
                else
                    cmbRelationship.ClearValidationError();
            }
            catch (Exception)
            {

            }
            return result;
        }

        private void hlbEditKin_Click(object sender, RoutedEventArgs e)
        {
            grpOtherKinDetails.Visibility = Visibility.Visible;
            RemoveableItem = (clsKinInformationVO)dgKinInfo.SelectedItem;
            if (!String.IsNullOrEmpty(RemoveableItem.Address))
                txtKinAddress.Text = RemoveableItem.Address;
            txtKinContactNo1.Text = Convert.ToString(RemoveableItem.MobileNumber);
            if (!String.IsNullOrEmpty(Convert.ToString(RemoveableItem.MobileCountryCode)))
                txtKinCountryCode.Text = Convert.ToString(RemoveableItem.MobileCountryCode);
            if (!String.IsNullOrEmpty(RemoveableItem.KinFirstName))
                txtKinFirstName.Text = RemoveableItem.KinFirstName;
            if (!String.IsNullOrEmpty(RemoveableItem.KinMiddleName))
                txtKinMiddleName.Text = RemoveableItem.KinMiddleName;
            if (!String.IsNullOrEmpty(RemoveableItem.KinLastName))
                txtKinLastName.Text = RemoveableItem.KinLastName;
            cmbRelationship.SelectedValue = RemoveableItem.RelationshipID;
        }

        private void dgKinInfo_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            ComboBox cmb = (ComboBox)e.EditingElement;
            cmb.SetBinding(ComboBox.ItemsSourceProperty, new System.Windows.Data.Binding()
            {
                Source = objRelationShipList
            });
            cmb.SetBinding(ComboBox.SelectedItemProperty, new System.Windows.Data.Binding("Relationship")
            {
                Source = e.Row.DataContext,
                Mode = System.Windows.Data.BindingMode.TwoWay

            });
            cmb.SetBinding(ComboBox.SelectedValueProperty, new System.Windows.Data.Binding("RelationshipID")
            {
                Source = e.Row.DataContext,
                Mode = System.Windows.Data.BindingMode.TwoWay

            });

        }

        private void hlbDeleteKin_Click(object sender, RoutedEventArgs e)
        {
            RemoveableItem = (clsKinInformationVO)dgKinInfo.SelectedItem;

            if (RemoveableItem != null)
                this.KinInformationListShow.Remove(RemoveableItem);

            BindKinInformation();
            ClearKinInfoControl();
        }
        #endregion

        #region OnTextChanged and SelectionChanged and LostFocus Events
        private void txtOfficeSTD_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((WaterMarkTextbox)sender).Text = textBefore;
                ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
            else if (((WaterMarkTextbox)sender).Text.Length > 6)
            {
                ((WaterMarkTextbox)sender).Text = textBefore;
                ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtOfficeContactNo2_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((WaterMarkTextbox)sender).Text = textBefore;
                ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
            else if (((WaterMarkTextbox)sender).Text.Length > 10)
            {
                ((WaterMarkTextbox)sender).Text = textBefore;
                ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtPinCode_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text) && !((AutoCompleteBox)sender).Text.IsNumberValid())
            {
                ((AutoCompleteBox)sender).Text = textBefore;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }
            else if (((AutoCompleteBox)sender).Text.Length > 6)
            {
                ((AutoCompleteBox)sender).Text = textBefore;
                ((AutoCompleteBox)sender).SelectedItem = textBefore;
            }
        }

        private void txtState_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsOnlyCharacters())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((AutoCompleteBox)sender).Text.Length > 15)
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    ((AutoCompleteBox)sender).SelectedItem = textBefore;
                }
            }
        }

        private void txtContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
            {
                if (!((WaterMarkTextbox)sender).Text.IsNumberValid() && textBefore != null)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
                else if (((WaterMarkTextbox)sender).Text.Length > 10)
                {
                    ((WaterMarkTextbox)sender).Text = textBefore;
                    ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                    ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsValidCountryCode() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 4)
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
                        textBefore = String.Empty;
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 10)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = String.Empty;
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                }
            }
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

        private void txtAutocompleteText_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsPersonNameValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;
                    textBefore = String.Empty;
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        //===================SelectionChanged Events========================================//
        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;
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
                    ((clsPatientVO)this.DataContext).StateID = ((MasterListItem)txtState.SelectedItem).ID;
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

        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)txtCity.SelectedItem).ID > 0)
                        ((clsPatientVO)this.DataContext).CityID = ((MasterListItem)txtCity.SelectedItem).ID;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    txtArea.ItemsSource = null;
                    txtArea.SelectedItem = objM;
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext) != null)
            {
                if (((MasterListItem)txtArea.SelectedItem != null))
                {
                    if (((MasterListItem)txtArea.SelectedItem).ID > 0)
                    {
                        ((clsPatientVO)this.DataContext).RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
                        ((clsPatientVO)this.DataContext).Pincode = (RegionList.Where(t => t.Id == ((MasterListItem)txtArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault());
                    }
                    else
                    {
                        ((clsPatientVO)this.DataContext).Pincode = "";
                    }
                }
            }
        }

        private void txtPinCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbPatientType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PatPersonalInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsPageLoded)
            {
                //CmdSave.Visibility = System.Windows.Visibility.Visible;
                //CmdClose.Visibility = System.Windows.Visibility.Visible;
                RegistrationTabs SelectedTab = (RegistrationTabs)PatPersonalInfo.SelectedIndex;
                switch (SelectedTab)
                {
                    case RegistrationTabs.Patient:
                        txtFirstName.UpdateLayout();
                        txtFirstName.Focus();
                        break;

                    case RegistrationTabs.Sponsor:
                        PatientSponsorNewMode = false;
                        break;
                    case RegistrationTabs.Admission:
                        PatientAdmissionNewMode = false;


                        break;
                }
            }
        }
        //===================LostFocus Events=========================//
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            txtFamilyName.Text = txtLastName.Text.Trim();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
            txtMiddleName.Text = txtMiddleName.Text.Trim();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        private void txtDay_LostFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void txtMonth_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void txtYear_LostFocus(object sender, RoutedEventArgs e)
        {
            
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

        private void Address_OnLostFocus(object sender, RoutedEventArgs e)
        {
            txtAddress1.Text = txtAddress1.Text.ToTitleCase();
        }
        
        private void txtKinMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        //===================KeyDown Events=========================//
        private void Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMRNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CmdPatientSearch_Click_1(sender, e);
            }
        }

        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtAutocompleteText_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }
        #endregion

        #region Patient DateOfBirth
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
            }
        }

        private DateTime? CalculateDateFromAge(int Year, int Month, int Days)
        {
            try
            {
                DateTime BirthDate;
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
                    if (Convert.ToDateTime(dtpDOB.SelectedDate).ToShortDateString() != BirthDate.ToShortDateString())
                        BirthDate = BirthDate.AddDays(-Days);
                }
                return BirthDate;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }

        void GetDOB(int yy, int mm, int dd)
        {
            if (dd <= 31 && mm <= 12)
            {
                DateTime DOB = new DateTime(yy, mm, dd);
                dtpDOB.SelectedDate = DOB;

                if (dtpDOB.SelectedDate != null)
                {
                    if (dtpDOB.SelectedDate <= DateTime.Now)
                    {
                        txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                    }
                }
            }
        }

        private string ConvertDate(object Datevalue, string parameter)
        {
            if (Datevalue != null)
            {
                try
                {
                    DateTime BirthDate = (DateTime)Datevalue;
                    TimeSpan difference = DateTime.Now.Subtract(BirthDate);
                    DateTime age = DateTime.MinValue + difference;
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
                            {
                                if (BirthDate.ToShortDateString() == DateTime.Now.ToShortDateString())
                                    result = "1";
                                else
                                {
                                    int day = BirthDate.Day;
                                    int curday = DateTime.Now.Day;
                                    int dif = 0;
                                    if (day > curday)
                                        dif = (curday + 30) - day;
                                    else
                                        dif = curday - day;
                                    result = dif.ToString();
                                }
                                break;
                            }
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
        private void txtYY_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtMM.SelectAll();
        }

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
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

        private void txtMM_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
            txtDD.SelectAll();
        }

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
        }

        private void txtDay_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //    if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //    else if (((TextBox)sender).Text.Length > 2)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //    else if (Convert.ToInt32(((TextBox)sender).Text) > 31)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}
        }

        private void txtMonth_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //{
            //    if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //    else if (((TextBox)sender).Text.Length > 2)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //    else if (Convert.ToInt32(((TextBox)sender).Text) > 12)
            //    {
            //        ((TextBox)sender).Text = textBefore;
            //        ((TextBox)sender).SelectionStart = selectionStart;
            //        ((TextBox)sender).SelectionLength = selectionLength;
            //        textBefore = String.Empty;
            //        selectionStart = 0;
            //        selectionLength = 0;
            //    }
            //}
        }

        private void txtYear_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //    if (!string.IsNullOrEmpty(((TextBox)sender).Text))
            //    {
            //        if (!((TextBox)sender).Text.IsNumberValid() && textBefore != null)
            //        {
            //            ((TextBox)sender).Text = textBefore;
            //            ((TextBox)sender).SelectionStart = selectionStart;
            //            ((TextBox)sender).SelectionLength = selectionLength;
            //            textBefore = String.Empty;
            //            selectionStart = 0;
            //            selectionLength = 0;
            //        }
            //        else if (((TextBox)sender).Text.Length > 4)
            //        {
            //            ((TextBox)sender).Text = textBefore;
            //            ((TextBox)sender).SelectionStart = selectionStart;
            //            ((TextBox)sender).SelectionLength = selectionLength;
            //            textBefore = String.Empty;
            //            selectionStart = 0;
            //            selectionLength = 0;
            //        }
            //    }
            //    if (txtYear.Text != "")
            //    {
            //        if (((TextBox)sender).Text.IsNumberValid() || textBefore != null)
            //        {
            //            if (txtYear.Text != null && txtYear.Text != String.Empty)
            //            {
            //                if (txtYear.Text != String.Empty && Convert.ToInt32(txtYear.Text) > 0 && txtMonth.Text == String.Empty && txtDay.Text == String.Empty)
            //                    GetDOB(Convert.ToInt32(txtYear.Text), DateTime.Now.Month, DateTime.Now.Day);
            //                else if (txtYear.Text != String.Empty && Convert.ToInt32(txtYear.Text) > 0 && txtMonth.Text != String.Empty && txtDay.Text != String.Empty)
            //                    GetDOB(Convert.ToInt32(txtYear.Text), Convert.ToInt32(txtMonth.Text), Convert.ToInt32(txtDay.Text));
            //                else if (txtYear.Text == String.Empty && Convert.ToInt32(txtYear.Text) > 0 && txtMonth.Text != String.Empty && txtDay.Text != String.Empty)
            //                    GetDOB(DateTime.Now.Year, Convert.ToInt32(txtMonth.Text), Convert.ToInt32(txtDay.Text));
            //            }
            //        }
            //        else
            //        {
            //            txtYear.SetValidation("Please, Enter Number");
            //            txtYear.RaiseValidationError();
            //            txtYear.Focus();
            //        }
            //    }
        }
        #endregion

        #region Find Patient
        public string ModuleName { get; set; }
        public string Action { get; set; }
        public bool IsSearchOnList = false;
        public clsPatientVO objPatientInfo = null;
        private void CmdPatientSearch_Click_1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtMRNumber.Text))
            {
                IsSearchOnList = true;
                clsCheckMRnoBizActionVO BizActionObj = new clsCheckMRnoBizActionVO();
                BizActionObj.Details = new clsAppointmentVO();
                BizActionObj.MRNO = txtMRNumber.Text;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        Status = ((clsCheckMRnoBizActionVO)arg.Result).SuccessStatus;
                        //FindPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNumber.Text.Trim());
                        GetActiveAdmissionOfPatient(0, ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID, txtMRNumber.Text.Trim());
                        if (Status == false)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW8 =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Registration not found against this M.R. Number", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            msgW8.Show();
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occurred while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        msgW1.Show();
                    }
                };
                client.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow ms =
                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Enter M.R. Number", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                ms.Show();
            }
        }
        private void btnSearchPatient_Click(object sender, RoutedEventArgs e)
        {
            IsSearchOnList = true;
            ModuleName = "OPDModule";
            Action = "OPDModule.Forms.PatientSearch";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
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
                if (myData is IInitiateCIMS)
                {
                    ((IInitiateCIMS)myData).Initiate(PatientTypes.All.ToString());
                }

                if (IsSearchOnList == true)  //Added by AJ date 14/11/2016
                {
                    ((IInitiateCIMS)myData).Initiate("ISIPD");
                }
                ChildWindow cw = new ChildWindow();
                cw = (ChildWindow)myData;
                cw.Closed += new EventHandler(cw_Closed);
                //if (IsViewClick == false)
                //{
                //    cw.Closed += new EventHandler(cw_Closed);
                //}
                cw.Show();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void cw_Closed(object sender, EventArgs e)
        {
            if ((bool)((OPDModule.Forms.PatientSearch)sender).DialogResult)
            {
                GetSelectedPatientDetails();
            }
            else
            {

            }
        }
        private void GetSelectedPatientDetails()
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
            {
                long PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                long UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                string MRNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                GetActiveAdmissionOfPatient(PatientID, UnitId, MRNo);
            }
        }
        
        private void GetActiveAdmissionOfPatient(long PatientID, long PatientUnitID, string MRNO)
        {
            clsGetActiveAdmissionBizActionVO BizObject = new clsGetActiveAdmissionBizActionVO();
            BizObject.PatientID = PatientID;
            BizObject.PatientUnitID = PatientUnitID;
            BizObject.MRNo = MRNO;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetActiveAdmissionBizActionVO)arg.Result).Details != null && ((clsGetActiveAdmissionBizActionVO)arg.Result).Details.AdmID > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient is already admitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            FindPatient(PatientID, PatientUnitID, MRNO);
                        }
                    }
                }
            };
            Client.ProcessAsync(BizObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FindPatient(long PatientID, long PatientUnitId, string MRNO)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.IsFromSearchWindow = true;
            BizAction.PatientDetails.GeneralDetails.PatientID = PatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = PatientUnitId;
            BizAction.PatientDetails.GeneralDetails.MRNo = MRNO;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (!((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID.Equals(0))
                    {
                        BindSelectedPatientDetails((clsGetPatientBizActionVO)arg.Result, Indicatior);

                    }
                    else
                    {
                        Indicatior.Close();
                    }
                }
                else
                {
                    Indicatior.Close();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //public void FillSponsorDetails()
        //{

        //    try
        //    {
        //        Indicatior = new WaitIndicator();
        //        Indicatior.Show();

        //        clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();

        //        BizAction.SponsorID = 0;
        //        BizAction.PatientID = ((clsPatientSponsorVO)this.DataContext).PatientId;
        //        BizAction.PatientUnitID = ((clsPatientSponsorVO)this.DataContext).PatientUnitId;

        //        dgSponserList.ItemsSource = null;

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, arg) =>
        //        {
        //            if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
        //            {

        //                dgSponserList.ItemsSource = null;
        //                dgSponserList.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;

        //            }
        //            Indicatior.Close();
        //        };
        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        Indicatior.Close();
        //        // throw;
        //    }
        //}

        DateTime? DOB = null;

        private void BindSelectedPatientDetails(clsGetPatientBizActionVO PatientVO, WaitIndicator Indicatior)
        {
            try
            {
                if (PatientVO.PatientDetails.GeneralDetails.MRNo != null)
                {
                    objPatientInfo = PatientVO.PatientDetails;
                    cmbGender.IsEnabled = false;
                    txtMobileCountryCode.IsEnabled = false;
                    txtContactNo2.IsEnabled = false;
                    cmbMaritalStatus.IsEnabled = false;
                    txtContactNo1.IsEnabled = false;
                    cmbPatientType.IsEnabled = false;
                    txtCity.IsEnabled = false;
                    txtArea.IsEnabled = false;
                    cmbIdentity.IsEnabled = false;
                    txtIdNumber.IsEnabled = false;
                    txtOfficeName.IsEnabled = false;
                    cmbReferralName.IsEnabled = false;
                    cmbPreffix.IsEnabled = false;
                    cmbReligion.IsEnabled = false;
                    cmbOccupation.IsEnabled = false;
                    txtResiCountryCode.IsEnabled = false;
                    txtResiSTD.IsEnabled = false;
                    txtCountry.IsEnabled = false;
                    txtState.IsEnabled = false;
                    txtCity.IsEnabled = false;
                    txtArea.IsEnabled = false;
                    txtYY.IsEnabled = false;
                    txtMM.IsEnabled = false;
                    txtDD.IsEnabled = false;
                    cmbEducation.IsEnabled = false;
                    cmbBloodGroup.IsEnabled = false;
                    txtFirstName.IsEnabled = false;
                    txtMiddleName.IsEnabled = false;
                    txtLastName.IsEnabled = false;
                    dtpDOB.IsEnabled = false;
                    txtFamilyName.IsEnabled = false;
                    ChkInternationalPatient.IsEnabled = false;
                    txtAddress1.IsEnabled = false;
                    txtPinCode.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtReferralDetail.IsEnabled = false;
                    cmbReferralDoctor.IsEnabled = false;
                    txtReferralDetail.IsEnabled = false;
                    cmdBrowse.IsEnabled = false;
                    cmdStartCapture.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    this.DataContext = objPatientInfo;
                    cmbGender.SelectedValue = objPatientInfo.GenderID;
                    txtMobileCountryCode.Text = objPatientInfo.MobileCountryCode;
                    txtContactNo2.Text = objPatientInfo.ContactNo2;
                    cmbMaritalStatus.SelectedValue = objPatientInfo.MaritalStatusID;
                    txtContactNo1.Text = objPatientInfo.ContactNo1;

                    cmbPatientType.SelectedValue = PatientVO.PatientDetails.GeneralDetails.PatientTypeID;
                    txtCity.Text = objPatientInfo.City;
                    txtArea.Text = objPatientInfo.Area;
                    cmbIdentity.SelectedValue = objPatientInfo.IdentityID;
                    txtIdNumber.Text = objPatientInfo.IdentityNumber;
                    txtState.Text = objPatientInfo.State;
                    txtOfficeName.Text = objPatientInfo.CompanyName;
                    if (objPatientInfo.Photo != null)
                    {
                        byte[] imageBytes1 = objPatientInfo.Photo;
                        WriteableBitmap bmp1 = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                        bmp1.FromByteArray(imageBytes1);
                        imgPhoto.Source = bmp1;
                    }
                    cmbReferralName.SelectedValue = objPatientInfo.ReferralTypeID;
                    cmbReferralDoctor.SelectedValue = objPatientInfo.ReferralDoctorID;
                    cmbPreffix.SelectedValue = objPatientInfo.PrefixId;
                    cmbReligion.SelectedValue = objPatientInfo.ReligionID;
                    cmbOccupation.SelectedValue = objPatientInfo.OccupationId;
                    txtResiCountryCode.Text = Convert.ToString(objPatientInfo.ResiNoCountryCode);
                    txtResiSTD.Text = Convert.ToString(objPatientInfo.ResiSTDCode);
                    txtCountry.SelectedValue = objPatientInfo.CountryID;
                    txtState.SelectedValue = objPatientInfo.StateID;
                    txtCity.SelectedValue = objPatientInfo.CityID;
                    txtArea.SelectedValue = objPatientInfo.RegionID;
                    //---Added by AJ Date 3/11/2016

                    if (((clsPatientVO)this.DataContext).GeneralDetails.IsAge == false)
                    {
                        txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                    }
                    else
                    {
                        DOB = ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirthFromAge;
                        txtYY.Text = ConvertDate(((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirthFromAge, "YY");
                        txtMM.Text = ConvertDate(((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirthFromAge, "MM");
                        txtDD.Text = ConvertDate(((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirthFromAge, "DD");
                    }

                    cmbEducation.SelectedValue = objPatientInfo.EducationID;
                    cmbBloodGroup.SelectedValue = objPatientInfo.BloodGroupID;
                    if (((clsPatientVO)this.DataContext).ImageName != null && ((clsPatientVO)this.DataContext).ImageName.Length > 0)
                    {
                        imgPhoto.Source = new BitmapImage(new Uri(((clsPatientVO)this.DataContext).ImageName, UriKind.Absolute));
                    }
                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IPD.SponsorWindowIPD") as UIElement;

                    Sponsortwin = new SponsorWindowIPD();
                    Sponsortwin = (SponsorWindowIPD)mydata;

                    Sponsortwin.PatientID = PatientVO.PatientDetails.GeneralDetails.PatientID;
                    Sponsortwin.PatientUnitID = PatientVO.PatientDetails.GeneralDetails.UnitId;

                    ((IInitiateCIMS)Sponsortwin).Initiate("NEWR");
                    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                    Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                    Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                    dtpRegistrationDate.SelectedDate = System.DateTime.Now;
                    sponsorContent.Content = Sponsortwin;
                }
                Indicatior.Close();
            }
            catch (Exception ex)
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occoured While Processsing......", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW1.Show();
                Indicatior.Close();
            }
        }
        #endregion

        #region Patient Photo
        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {
                WriteableBitmap imageSource = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                try
                {
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    imgPhoto.Source = imageSource;
                    // MyPhoto = imageToByteArray(OpenFile.File);


                    WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                    bmp.Render(imgPhoto, new MatrixTransform());
                    bmp.Invalidate();

                    int[] p = bmp.Pixels;
                    int len = p.Length * 4;
                    byte[] result = new byte[len]; // ARGB
                    Buffer.BlockCopy(p, 0, result, 0, len);

                    ((clsPatientVO)this.DataContext).Photo = result;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }

        CaptureSource _captureSource = new CaptureSource();
        WriteableBitmap _images = null;
        private void cmdStartCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_captureSource != null)
                {
                    _captureSource.Stop(); // stop whatever device may be capturing
                    VideoBrush vidBrush = new VideoBrush();
                    vidBrush.SetSource(_captureSource);
                    WebcamCapture.Fill = vidBrush; // paint the brush on the rectangle

                    // request user permission and display the capture
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                    {
                        _captureSource.Start();

                    }
                    cmdStartCapture.Visibility = Visibility.Collapsed;
                    cmdCaptureImage.Visibility = Visibility.Visible;
                    borderimage.Visibility = System.Windows.Visibility.Collapsed;
                    borderwebcap.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmdCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_captureSource != null)
                {
                    // capture the current frame and add it to our observable collection                
                    _captureSource.CaptureImageAsync();
                    _captureSource.CaptureImageCompleted += ((s, args) =>
                    {
                        _images = (args.Result);
                        imgPhoto.Source = _images;

                        WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                        bmp.Render(imgPhoto, new MatrixTransform());
                        bmp.Invalidate();

                        int[] p = bmp.Pixels;
                        int len = p.Length * 4;
                        byte[] result = new byte[len]; // ARGB
                        Buffer.BlockCopy(p, 0, result, 0, len);

                        ((clsPatientVO)this.DataContext).Photo = result;

                    });
                    borderimage.Visibility = System.Windows.Visibility.Visible;
                    borderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    cmdStartCapture.Visibility = Visibility.Visible;
                    cmdCaptureImage.Visibility = Visibility.Collapsed;
                    _captureSource.Stop();

                }
            }
            catch (System.Exception ex)
            {
                throw;

            }
        }

        private void cmdStopCapture_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion


    }
}
