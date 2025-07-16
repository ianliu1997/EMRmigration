using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections;
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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using System.Xml.Linq;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Master.DoctorMaster;

namespace CIMS
{
    public partial class ExpressPatientRegistration : UserControl, IInitiateCIMS
    {
        private bool PatientEditMode = false;
        bool IsPatientExist = false;

        bool IsfromEdit = false;
        bool Flagref = false;
        int blnTaskCount = 0; //Initiate,
        #region IInitiateCIMS Members

        // WaitIndicator Indicatior = new WaitIndicator();
        WaitIndicator wait = new WaitIndicator();
        /// <summary>
        /// Function is for Initializing the form.
        /// </summary>
        /// <param name="Mode">New or Edit</param>        
        public void Initiate(string Mode)
        {
            try
            {
                blnTaskCount++;
                wait.Show();
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                IsPatientExist = true;
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");
                mElement1.Text = ": New Patient";

                this.DataContext = new clsPatientVO()
                {
                    MaritalStatusID = 0,
                    BloodGroupID = 0,
                    GenderID = 0,
                    OccupationId = 0,
                    //Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country,
                    //State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State,
                    //District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District,
                    //RelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID,
                    //City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                    //Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,
                    //CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID,
                    //StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID,
                    //CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID,
                    //RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID,
                    //Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                    PrefixId = 0,
                    IdentityID = 0,
                    IsInternationalPatient = false,

                    NationalityID = 0,
                    PreferredLangID = 0,
                    TreatRequiredID = 0,
                    EducationID = 0,
                    SpecialRegID = 0,

                    // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                };


                ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID;
                ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;
                ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Female;
                ((clsPatientVO)this.DataContext).SpouseDetails.CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID;
                ((clsPatientVO)this.DataContext).SpouseDetails.StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID;
                ((clsPatientVO)this.DataContext).SpouseDetails.CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID;
                ((clsPatientVO)this.DataContext).SpouseDetails.RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID;
                ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode;

                ((clsPatientVO)this.DataContext).SpouseDetails.NationalityID = 0;
                ((clsPatientVO)this.DataContext).SpouseDetails.PreferredLangID = 0;
                ((clsPatientVO)this.DataContext).SpouseDetails.TreatRequiredID = 0;
                ((clsPatientVO)this.DataContext).SpouseDetails.EducationID = 0;
                ((clsPatientVO)this.DataContext).SpouseDetails.SpecialRegID = 0;

                txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;

                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                }

                if (!IsPageLoded)
                {
                    //WaitIndicator Indicatior = new WaitIndicator();
                    // Indicatior.Show();

                    formSaveType = SaveType.Registration;

                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    {
                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                          && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                        {
                            //do nothing
                        }
                        else
                            CmdSave.IsEnabled = false;
                        CmdOPD.IsEnabled = false;
                    }

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);

                    FillPatientType();
                    ReferralDoc.IsChecked = true;

                    FillRefDoctor();
                    FillReferralName();
                    FillPreffix();
                    FillMaritalStatus();
                    FillPatientSponsorDetails();
                    FillGender();
                    FillBdMaster(); //*** Ajit

                    txtFirstName.Focus();

                    if (!PatientEditMode)
                    {
                        //======================================
                        //Default Sponsor - Initialization
                        UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.SponsorWindow") as UIElement;
                        Sponsortwin = new SponsorWindow();
                        Sponsortwin = (SponsorWindow)mydata;
                        ((IInitiateCIMS)Sponsortwin).Initiate("NEWR");
                        // Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
                        Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                        Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                        Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);

                        sponsorContent.Content = Sponsortwin;
                        //======================================
                    }

                    //Indicatior.Close();
                }

                cmbPatientType.Focus();
                cmbPatientType.UpdateLayout();
                IsPageLoded = true;

                if (PatientEditMode == false)
                    CheckValidations();

                if (blnTaskCount >= 8)
                    wait.Close();

            }
            catch (Exception)
            {
                wait.Close();
                throw;
            }

        }

        #endregion
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        public long mAppointmentID = 0;
        public long mAppointmentUnitID = 0;
        bool PatientSponsorNewMode = true;

        public ExpressPatientRegistration()
        {
            InitializeComponent();
        }

        private enum SaveType
        {
            Registration = 0,
            IPD = 1,
            OPD = 2
        }

        private SaveType formSaveType = SaveType.Registration;

        void PatientRegistration_Loaded(object sender, RoutedEventArgs e)
        {

            //if (IsPatientExist == false)
            //{
            //    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //}

            //if (!IsPageLoded)
            //{
            //    //WaitIndicator Indicatior = new WaitIndicator();
            //   // Indicatior.Show();

            //    formSaveType = SaveType.Registration;

            //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            //    {
            //        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
            //          && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
            //        {
            //            //do nothing
            //        }
            //        else
            //            CmdSave.IsEnabled = false;
            //        CmdOPD.IsEnabled = false;
            //    }

            //    List<MasterListItem> objList = new List<MasterListItem>();
            //    MasterListItem objM = new MasterListItem(0, "-- Select --");
            //    objList.Add(objM);

            //    FillReferralName();
            //    FillPreffix();                
            //    FillPatientType();
            //    FillMaritalStatus();
            //    FillPatientSponsorDetails();
            //    FillGender();

            //    txtFirstName.Focus();

            //    if (!PatientEditMode)
            //    {
            //        //======================================
            //        //Default Sponsor - Initialization
            //        UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.SponsorWindow") as UIElement;
            //        Sponsortwin = new SponsorWindow();
            //        Sponsortwin = (SponsorWindow)mydata;
            //        ((IInitiateCIMS)Sponsortwin).Initiate("NEWR");
            //        // Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
            //        Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
            //        Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
            //        Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);

            //        sponsorContent.Content = Sponsortwin;
            //        //======================================
            //    }

            //    //Indicatior.Close();
            //}

            //cmbPatientType.Focus();
            //cmbPatientType.UpdateLayout();
            //IsPageLoded = true;

            if (PatientEditMode == false)
                CheckValidations();
        }

        /// <summary>
        /// Checks & assigns validations for the controls.
        /// </summary>
        /// <returns></returns>
        /// 
        private bool CheckSpouseValidations()
        {
            bool result = true;



            if ((MasterListItem)cmbSpouseGender.SelectedItem == null)
            {
                cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                cmbSpouseGender.TextBox.RaiseValidationError();
                cmbSpouseGender.Focus();
                //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                result = false;
            }
            else if (((MasterListItem)cmbSpouseGender.SelectedItem).ID == 0)
            {
                cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                cmbSpouseGender.TextBox.RaiseValidationError();
                cmbSpouseGender.Focus();
                // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                result = false;
            }
            else
                cmbSpouseGender.TextBox.ClearValidationError();



            //Spouse
            if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 7 && !PatientEditMode) // Couple
            {
                if (string.IsNullOrEmpty(txtSpouseYY.Text) && string.IsNullOrEmpty(txtSpouseMM.Text) && string.IsNullOrEmpty(txtSpouseDD.Text))
                {
                    txtSpouseYY.SetValidation("Age Is Required");
                    txtSpouseYY.RaiseValidationError();
                    txtSpouseMM.SetValidation("Age Is Required");
                    txtSpouseMM.RaiseValidationError();
                    txtSpouseDD.SetValidation("Age Is Required");
                    txtSpouseDD.RaiseValidationError();
                    result = false;
                    txtSpouseYY.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                }
                else
                {
                    txtSpouseYY.ClearValidationError();
                    txtSpouseMM.ClearValidationError();
                    txtSpouseDD.ClearValidationError();
                }

                if ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem == null)
                {
                    cmbSpouseMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                    cmbSpouseMaritalStatus.TextBox.RaiseValidationError();
                    cmbSpouseMaritalStatus.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID == 0)
                {
                    cmbSpouseMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                    cmbSpouseMaritalStatus.TextBox.RaiseValidationError();
                    cmbSpouseMaritalStatus.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    cmbSpouseMaritalStatus.TextBox.ClearValidationError();


                if (txtSpouseMobileCountryCode.Text == null || txtSpouseMobileCountryCode.Text.Trim() == "")
                {
                    txtSpouseMobileCountryCode.SetValidation("Mobile Country Code Is Required");
                    txtSpouseMobileCountryCode.RaiseValidationError();
                    txtSpouseMobileCountryCode.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseMobileCountryCode.ClearValidationError();
                if (txtSpouseContactNo1.Text == null || txtSpouseContactNo1.Text.Trim() == "")
                {
                    txtSpouseContactNo1.SetValidation("Mobile Number Is Required");
                    txtSpouseContactNo1.RaiseValidationError();
                    txtSpouseContactNo1.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseContactNo1.ClearValidationError();



                if ((MasterListItem)cmbSpousePreffix.SelectedItem == null)
                {
                    cmbSpousePreffix.TextBox.SetValidation("Preffix Is Required");
                    cmbSpousePreffix.TextBox.RaiseValidationError();
                    cmbSpousePreffix.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)cmbSpousePreffix.SelectedItem).ID == 0)
                {
                    cmbSpousePreffix.TextBox.SetValidation("Preffix Is Required");
                    cmbSpousePreffix.TextBox.RaiseValidationError();
                    cmbSpousePreffix.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    cmbSpousePreffix.TextBox.ClearValidationError();


                if ((MasterListItem)cmbPatientType.SelectedItem == null)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    cmbPatientType.TextBox.ClearValidationError();



                if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                {
                    txtSpouseFirstName.SetValidation("First Name Is Required");
                    txtSpouseFirstName.RaiseValidationError();
                    txtSpouseFirstName.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseFirstName.ClearValidationError();





                if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                {

                    if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                    {
                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                        dtpSpouseDOB.RaiseValidationError();
                        dtpSpouseDOB.Focus();
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                    {
                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                        dtpSpouseDOB.RaiseValidationError();
                        dtpSpouseDOB.Focus();
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        dtpSpouseDOB.ClearValidationError();
                }

                if (txtSpouseYY.Text != "")
                {

                    if (Convert.ToInt16(txtSpouseYY.Text) > 120)
                    {
                        txtSpouseYY.SetValidation("Age Can Not Be Greater Than 121");
                        txtSpouseYY.RaiseValidationError();
                        txtSpouseYY.Focus();
                        result = false;
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        ClickedFlag = 0;
                    }
                    else
                        txtSpouseYY.ClearValidationError();
                }

            }



            return result;
        }
        private bool CheckValidations()
        {
            bool result = true;
            try
            {
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
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                }
                else
                {
                    txtYY.ClearValidationError();
                    txtMM.ClearValidationError();
                    txtDD.ClearValidationError();
                }


                //* Commented by - Ajit Jadhav
                //* Added Date - 4/8/2016
                //* Comments - Mandatory fields Date of Birth(Birth date Mandatory)

                //if (dtpDOB.SelectedDate != null)

                //{
                //    if (dtpDOB.SelectedDate.Value.Date > DateTime.Now.Date)
                //    {
                //        dtpDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                //        dtpDOB.RaiseValidationError();
                //        dtpDOB.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else if (dtpDOB.SelectedDate.Value.Date == DateTime.Now.Date)
                //    {
                //        dtpDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                //        dtpDOB.RaiseValidationError();
                //        dtpDOB.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else
                //        dtpDOB.ClearValidationError();
                //}

                //* Added by - Ajit Jadhav
                //* Added Date - 4/8/2016
                //* Comments - New Mandatory fields Date of Birth,BDName,Last Name

                if (dtpDOB.SelectedDate != null)
                {
                    if (dtpDOB.SelectedDate.Value.Date > DateTime.Now.Date)
                    {
                        dtpDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                        dtpDOB.RaiseValidationError();
                        dtpDOB.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (dtpDOB.SelectedDate.Value.Date == DateTime.Now.Date)
                    {
                        dtpDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                        dtpDOB.RaiseValidationError();
                        dtpDOB.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        dtpDOB.ClearValidationError();
                }
                else
                {
                    dtpDOB.SetValidation("Date Of Birth Is Required");
                    dtpDOB.RaiseValidationError();
                    dtpDOB.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }

                if ((MasterListItem)cmbBD.SelectedItem == null)
                {
                    cmbBD.TextBox.SetValidation("BDName Is Required");
                    cmbBD.TextBox.RaiseValidationError();
                    cmbBD.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbBD.SelectedItem).ID == 0)
                {
                    cmbBD.TextBox.SetValidation("BDName Is Required");
                    cmbBD.TextBox.RaiseValidationError();
                    cmbBD.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbBD.TextBox.ClearValidationError();

                 //Commented by Ajit 
                 //date 12/10/2016

                //if (((clsPatientVO)this.DataContext).GeneralDetails.LastName == null || ((clsPatientVO)this.DataContext).GeneralDetails.LastName.Trim() == "")
                //{
                //    txtLastName.SetValidation("Last Name Is Required.");
                //    txtLastName.RaiseValidationError();
                //    txtLastName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtLastName.ClearValidationError();

                //***//---------------------------------------

                if ((MasterListItem)cmbGender.SelectedItem == null)
                {
                    cmbGender.TextBox.SetValidation("Gender Is Required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbGender.SelectedItem).ID == 0)
                {
                    cmbGender.TextBox.SetValidation("Gender Is Required");
                    cmbGender.TextBox.RaiseValidationError();
                    cmbGender.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbGender.TextBox.ClearValidationError();

                if ((MasterListItem)cmbReferralDoctor.SelectedItem == null)
                {
                    cmbReferralDoctor.TextBox.SetValidation("Doctor Is Required");
                    cmbReferralDoctor.TextBox.RaiseValidationError();
                    cmbReferralDoctor.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbReferralDoctor.SelectedItem).ID == 0)
                {
                    //* Added by - Ajit Jadhav
                    //* Added Date - 24/8/2016
                    //* Comments - Change Mandatory Fields Condition
                    if (((MasterListItem)cmbReferralName.SelectedItem).ID == 1 || ((MasterListItem)cmbReferralName.SelectedItem).ID == 2)
                   //***//-------
                    {
                        cmbReferralDoctor.TextBox.SetValidation("Doctor Is Required");
                        cmbReferralDoctor.TextBox.RaiseValidationError();
                        cmbReferralDoctor.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                }
                else
                    cmbReferralDoctor.TextBox.ClearValidationError();

                if ((MasterListItem)cmbMaritalStatus.SelectedItem == null)
                {
                    cmbMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                    cmbMaritalStatus.TextBox.RaiseValidationError();
                    cmbMaritalStatus.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbMaritalStatus.SelectedItem).ID == 0)
                {
                    cmbMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                    cmbMaritalStatus.TextBox.RaiseValidationError();
                    cmbMaritalStatus.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }

                else
                    cmbMaritalStatus.TextBox.ClearValidationError();


                if (txtMobileCountryCode.Text == null || txtMobileCountryCode.Text.Trim() == "")
                {
                    txtMobileCountryCode.Textbox.SetValidation("Mobile Country Code Is Required");
                    txtMobileCountryCode.Textbox.RaiseValidationError();
                    txtMobileCountryCode.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtMobileCountryCode.Textbox.ClearValidationError();

                if (txtContactNo1.Text == null || txtContactNo1.Text.Trim() == "")
                {
                    txtContactNo1.Textbox.SetValidation("Mobile Number Is Required");
                    txtContactNo1.Textbox.RaiseValidationError();
                    txtContactNo1.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (txtContactNo1.Text.Trim().Length != 10)
                {
                    txtContactNo1.Textbox.SetValidation("Mobile Number Should Be 10 Digit");
                    txtContactNo1.Textbox.RaiseValidationError();
                    txtContactNo1.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtContactNo1.Textbox.ClearValidationError();

                if ((MasterListItem)cmbPreffix.SelectedItem == null)
                {
                    cmbPreffix.TextBox.SetValidation("Prefix Is Required");
                    cmbPreffix.TextBox.RaiseValidationError();
                    cmbPreffix.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbPreffix.SelectedItem).ID == 0)
                {
                    cmbPreffix.TextBox.SetValidation("Prefix Is Required");
                    cmbPreffix.TextBox.RaiseValidationError();
                    cmbPreffix.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbPreffix.TextBox.ClearValidationError();



                if ((MasterListItem)cmbPatientType.SelectedItem == null)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                {
                    cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                    cmbPatientType.TextBox.RaiseValidationError();
                    cmbPatientType.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbPatientType.TextBox.ClearValidationError();


                if ((MasterListItem)cmbReferralName.SelectedItem == null)
                {
                    cmbReferralName.TextBox.SetValidation("Source Of Reference Is Required");
                    cmbReferralName.TextBox.RaiseValidationError();
                    cmbReferralName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbReferralName.SelectedItem).ID == 0)
                {
                    cmbReferralName.TextBox.SetValidation("Source Of Reference Is Required");
                    cmbReferralName.TextBox.RaiseValidationError();
                    cmbReferralName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbReferralName.TextBox.ClearValidationError();

                //if (txtReferralDetail.Text == string.Empty)
                //{
                //    txtReferralDetail.SetValidation("Referral Detail Is Required");
                //    txtReferralDetail.RaiseValidationError();
                //    txtReferralDetail.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtReferralDetail.ClearValidationError();

                //* Added by - Ajit Jadhav
                //* Added Date - 24/8/2016
                //* Comments - Change Mandatory Fields Condition
                if (txtReferralDetail.Text == string.Empty)
                {
                    if (((MasterListItem)cmbReferralName.SelectedItem) == null)
                    {
                        txtReferralDetail.SetValidation("Referral Detail Is Required");
                        txtReferralDetail.RaiseValidationError();
                        txtReferralDetail.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (((MasterListItem)cmbReferralName.SelectedItem).ID != 1 && ((MasterListItem)cmbReferralName.SelectedItem).ID != 2)  
                    {
                        txtReferralDetail.SetValidation("Referral Detail Is Required");
                        txtReferralDetail.RaiseValidationError();
                        txtReferralDetail.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                }
                else
                    txtReferralDetail.ClearValidationError();

                //***//--------------------------------------------------

                if (((clsPatientVO)this.DataContext).GeneralDetails.FirstName == null || ((clsPatientVO)this.DataContext).GeneralDetails.FirstName.Trim() == "")
                {
                    txtFirstName.SetValidation("First Name Is Required.");
                    txtFirstName.RaiseValidationError();
                    txtFirstName.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtFirstName.ClearValidationError();




                if (IsPageLoded)
                {
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
                            txtYY.SetValidation("Age Can Not Be Greater Than 121");
                            txtYY.RaiseValidationError();
                            txtYY.Focus();
                            result = false;
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            ClickedFlag = 0;
                        }
                        else
                            txtYY.ClearValidationError();
                    }

                    if (txtSpouseMM.Text != "")
                    {
                        if (Convert.ToInt16(txtSpouseMM.Text) > 12)
                        {
                            txtSpouseMM.SetValidation("Month Cannot Be Greater than 12");
                            txtSpouseMM.RaiseValidationError();
                            txtSpouseMM.Focus();
                            result = false;
                            //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            ClickedFlag = 0;
                        }
                        else
                            txtSpouseMM.ClearValidationError();
                    }
                    if (txtSpouseYY.Text != "")
                    {
                        if (Convert.ToInt16(txtSpouseYY.Text) > 120)
                        {
                            txtSpouseYY.SetValidation("Age Can Not Be Greater Than 121");
                            txtSpouseYY.RaiseValidationError();
                            txtSpouseYY.Focus();
                            result = false;
                            //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            ClickedFlag = 0;
                        }
                        else
                            txtSpouseYY.ClearValidationError();
                    }


                    if (IsPageLoded)
                    {


                        //Spouse
                        if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 7 && !PatientEditMode) // Couple
                        {
                            if (string.IsNullOrEmpty(txtSpouseYY.Text) && string.IsNullOrEmpty(txtSpouseMM.Text) && string.IsNullOrEmpty(txtSpouseDD.Text))
                            {
                                txtSpouseYY.SetValidation("Age Is Required");
                                txtSpouseYY.RaiseValidationError();
                                txtSpouseMM.SetValidation("Age Is Required");
                                txtSpouseMM.RaiseValidationError();
                                txtSpouseDD.SetValidation("Age Is Required");
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

                            //if (dtpSpouseDOB.SelectedDate != null)
                            //{
                            //    if (dtpSpouseDOB.SelectedDate.Value.Date > DateTime.Now.Date)
                            //    {
                            //        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                            //        dtpSpouseDOB.RaiseValidationError();
                            //        dtpSpouseDOB.Focus();
                            //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            //        result = false;
                            //    }
                            //    else if (dtpSpouseDOB.SelectedDate.Value.Date == DateTime.Now.Date)
                            //    {
                            //        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equal To Todays Date");
                            //        dtpSpouseDOB.RaiseValidationError();
                            //        dtpSpouseDOB.Focus();
                            //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            //        result = false;
                            //    }
                            //    else
                            //        dtpSpouseDOB.ClearValidationError();
                            //}


                            //***//----------------------------------

                            if (dtpSpouseDOB.SelectedDate != null)
                            {
                                if (dtpSpouseDOB.SelectedDate.Value.Date > DateTime.Now.Date)
                                {
                                    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                                    dtpSpouseDOB.RaiseValidationError();
                                    dtpSpouseDOB.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }
                                else if (dtpSpouseDOB.SelectedDate.Value.Date == DateTime.Now.Date)
                                {
                                    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                                    dtpSpouseDOB.RaiseValidationError();
                                    dtpSpouseDOB.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }
                                else
                                    dtpSpouseDOB.ClearValidationError();
                            }
                            else
                            {
                                dtpSpouseDOB.SetValidation("Date Of Birth Is Required");
                                dtpSpouseDOB.RaiseValidationError();
                                dtpSpouseDOB.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }

                            //Commented by Ajit 
                            //date 12/10/2016

                            //if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == "")
                            //{
                            //    txtSpouseLastName.SetValidation("Last Name Is Required");
                            //    txtSpouseLastName.RaiseValidationError();
                            //    txtSpouseLastName.Focus();
                            //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            //    result = false;
                            //}
                            //else
                            //    txtSpouseLastName.ClearValidationError();

                            //------------------------------------

                            if ((MasterListItem)cmbSpouseGender.SelectedItem == null)
                            {
                                cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                                cmbSpouseGender.TextBox.RaiseValidationError();
                                cmbSpouseGender.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (((MasterListItem)cmbSpouseGender.SelectedItem).ID == 0)
                            {
                                cmbSpouseGender.TextBox.SetValidation("Gender Is Required");
                                cmbSpouseGender.TextBox.RaiseValidationError();
                                cmbSpouseGender.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                cmbSpouseGender.TextBox.ClearValidationError();



                            if ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem == null)
                            {
                                cmbSpouseMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                                cmbSpouseMaritalStatus.TextBox.RaiseValidationError();
                                cmbSpouseMaritalStatus.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID == 0)
                            {
                                cmbSpouseMaritalStatus.TextBox.SetValidation("Marital Status Is Required");
                                cmbSpouseMaritalStatus.TextBox.RaiseValidationError();
                                cmbSpouseMaritalStatus.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }

                            else
                                cmbSpouseMaritalStatus.TextBox.ClearValidationError();

                            if (txtSpouseMobileCountryCode.Text == null || txtSpouseMobileCountryCode.Text.Trim() == "")
                            {
                                txtSpouseMobileCountryCode.Textbox.SetValidation("Mobile Country Code Is Required");
                                txtSpouseMobileCountryCode.Textbox.RaiseValidationError();
                                txtSpouseMobileCountryCode.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                txtSpouseMobileCountryCode.Textbox.ClearValidationError();
                            if (txtSpouseContactNo1.Text == null || txtSpouseContactNo1.Text.Trim() == "")
                            {
                                txtSpouseContactNo1.Textbox.SetValidation("Mobile Number Is Required");
                                txtSpouseContactNo1.Textbox.RaiseValidationError();
                                txtSpouseContactNo1.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (txtSpouseContactNo1.Text.Trim().Length != 10)
                            {
                                txtSpouseContactNo1.Textbox.SetValidation("Mobile Number Should Be 10 Digit");
                                txtSpouseContactNo1.Textbox.RaiseValidationError();
                                txtSpouseContactNo1.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                txtSpouseContactNo1.Textbox.ClearValidationError();

                            if ((MasterListItem)cmbSpousePreffix.SelectedItem == null)
                            {
                                cmbSpousePreffix.TextBox.SetValidation("Preffix Is Required");
                                cmbSpousePreffix.TextBox.RaiseValidationError();
                                cmbSpousePreffix.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (((MasterListItem)cmbSpousePreffix.SelectedItem).ID == 0)
                            {
                                cmbSpousePreffix.TextBox.SetValidation("Preffix Is Required");
                                cmbSpousePreffix.TextBox.RaiseValidationError();
                                cmbSpousePreffix.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                cmbSpousePreffix.TextBox.ClearValidationError();


                            if ((MasterListItem)cmbPatientType.SelectedItem == null)
                            {
                                cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                                cmbPatientType.TextBox.RaiseValidationError();
                                cmbPatientType.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (((MasterListItem)cmbPatientType.SelectedItem).ID == 0)
                            {
                                cmbPatientType.TextBox.SetValidation("Patient Type Is Required");
                                cmbPatientType.TextBox.RaiseValidationError();
                                cmbPatientType.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                cmbPatientType.TextBox.ClearValidationError();




                            if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                            {
                                txtSpouseFirstName.SetValidation("First Name Is Required");
                                txtSpouseFirstName.RaiseValidationError();
                                txtSpouseFirstName.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                txtSpouseFirstName.ClearValidationError();



                            if (txtSpouseYY.Text != "")
                            {
                                if (Convert.ToInt16(txtSpouseYY.Text) > 120)
                                {
                                    txtSpouseYY.SetValidation("Age Can Not Be Greater Than 121");
                                    txtSpouseYY.RaiseValidationError();
                                    txtSpouseYY.Focus();
                                    result = false;
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    ClickedFlag = 0;
                                }
                                else
                                    txtSpouseYY.ClearValidationError();
                            }



                            if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                            {

                                if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                                {
                                    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                                    dtpSpouseDOB.RaiseValidationError();
                                    dtpSpouseDOB.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }
                                else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                                {
                                    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                                    dtpSpouseDOB.RaiseValidationError();
                                    dtpSpouseDOB.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }

                                else
                                    dtpSpouseDOB.ClearValidationError();
                            }

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
                                    txtSpouseYY.SetValidation("Age Is Required");
                                    txtSpouseYY.RaiseValidationError();
                                    txtSpouseMM.SetValidation("Age Is Required");
                                    txtSpouseMM.RaiseValidationError();
                                    txtSpouseDD.SetValidation("Age Is Required");
                                    txtSpouseDD.RaiseValidationError();
                                }
                                else
                                {
                                    txtSpouseYY.ClearValidationError();
                                    txtSpouseMM.ClearValidationError();
                                    txtSpouseDD.ClearValidationError();
                                }

                                if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                                {
                                    txtSpouseFirstName.SetValidation("First Name Is Required");
                                    txtSpouseFirstName.RaiseValidationError();
                                }
                                else
                                    txtSpouseFirstName.ClearValidationError();


                                if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                                {

                                    if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                                    {
                                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                                        dtpSpouseDOB.RaiseValidationError();
                                    }
                                    else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                                    {
                                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                                        dtpSpouseDOB.RaiseValidationError();
                                    }
                                    else
                                        dtpSpouseDOB.ClearValidationError();
                                }





                                clsPatientVO myVO = new clsPatientVO();
                                myVO = (clsPatientVO)this.DataContext;



                                myVO.SpouseDetails.LastName = myVO.GeneralDetails.LastName;
                                myVO.SpouseDetails.FamilyName = myVO.FamilyName;
                                myVO.SpouseDetails.AddressLine1 = myVO.AddressLine1;
                                myVO.SpouseDetails.AddressLine2 = myVO.AddressLine2;
                                myVO.SpouseDetails.AddressLine3 = myVO.AddressLine3;
                                myVO.SpouseDetails.Area = myVO.Area;
                                myVO.SpouseDetails.Country = myVO.Country;
                                myVO.SpouseDetails.State = myVO.State;
                                myVO.SpouseDetails.City = myVO.City;
                                myVO.SpouseDetails.District = myVO.District;
                                myVO.SpouseDetails.Taluka = myVO.Taluka;
                                myVO.SpouseDetails.Pincode = myVO.Pincode;
                                myVO.SpouseDetails.OccupationId = myVO.OccupationId;
                                myVO.SpouseDetails.CountryID = myVO.CountryID;
                                myVO.SpouseDetails.StateID = myVO.StateID;
                                myVO.SpouseDetails.CityID = myVO.CityID;
                                myVO.SpouseDetails.RegionID = myVO.RegionID;


                                this.DataContext = null;
                                this.DataContext = myVO;

                                txtSpouseResiCountryCode.Text = txtResiCountryCode.Text;
                                txtSpouseResiSTD.Text = txtResiSTD.Text;
                                txtSpouseMobileCountryCode.Text = txtMobileCountryCode.Text;

                                cmbSpouseMaritalStatus.SelectedItem = cmbMaritalStatus.SelectedItem;


                                txtSpouseContactNo2.Text = txtContactNo2.Text;
                                //....................................................................



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
                            txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode;
                            SpouseClick = true;
                        }
                        cmbSpousePreffix.Focus();
                        cmbSpousePreffix.UpdateLayout();
                        break;

                    //* Added by - Ajit Jadhav
                    //* Added Date - 9/8/2016
                    //* Comments - Create New Validation in SponsorWindow to check fill All Mandatory fields
                    case RegistrationTabs.Sponsor:
                        //Sponsor
                        PatientSponsorNewMode = false;
                        if (sponsorContent.Content == null)
                        {
                            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.SponsorWindow") as UIElement;
                            Sponsortwin = new SponsorWindow();
                            Sponsortwin = (SponsorWindow)mydata;
                            ((IInitiateCIMS)Sponsortwin).Initiate("NEW");
                            Sponsortwin.cmbPatientSource.Focus();
                            Sponsortwin.cmbPatientSource.UpdateLayout();
                            Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                            Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                            Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                            if (PatientEditMode == true)
                            {
                                Sponsortwin.pnlAddSponser.Visibility = System.Windows.Visibility.Collapsed;
                                sponsorContent.IsEnabled = false;
                            }

                            sponsorContent.Content = Sponsortwin;
                        }
                        break;
                    //***//---------------------------------------------------
                }
            }
        }

        private void FillPatientType()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PatientCategoryMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        var results = from r in objList
                                      where r.ID != 10
                                      select r;
                        cmbPatientType.ItemsSource = null;
                        cmbPatientType.ItemsSource = results.ToList();
                        cmbPatientType.SelectedItem = results.ToList()[0];
                    }

                    if (this.DataContext != null)
                    {
                        cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                    }
                    if (blnTaskCount >= 8)
                        wait.Close();

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void FillMaritalStatus()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_MaritalStatusMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
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
                    if (blnTaskCount >= 8)
                        wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void FillGender()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_GenderMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
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

                    if (blnTaskCount >= 8)
                        wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void FillPreffix()
        {
            try
            {

                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_Preffixmaster;
                BizAction.IsActive = true;
                BizAction.MasterList = new List<MasterListItem>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                        cmbPreffix.ItemsSource = null;
                        cmbPreffix.ItemsSource = objList.DeepCopy();
                        cmbSpousePreffix.ItemsSource = null;
                        cmbSpousePreffix.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbPreffix.SelectedValue = ((clsPatientVO)this.DataContext).PrefixId;
                        cmbSpousePreffix.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.PrefixId;
                    }

                    if (blnTaskCount >= 8)
                        wait.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void FillRefDoctor()
        {
            try
            {
                clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
                BizAction.ComboList = new List<clsComboMasterBizActionVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
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
                    if (blnTaskCount >= 8)
                        wait.Close();
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        private void FillRefDoctor(long DoctorID)
        {
            try
            {
                clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
                BizAction.ComboList = new List<clsComboMasterBizActionVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
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
                            cmbReferralDoctor.SelectedValue = DoctorID;
                        }
                    }
                    if (blnTaskCount >= 8)
                        wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
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
                    blnTaskCount++;
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
                    if (blnTaskCount >= 8)
                        wait.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw ex;
            }
        }

        private void FillReferralName()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_ReferralTypeMaster;
                BizAction.MasterList = new List<MasterListItem>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    blnTaskCount++;
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
                    if (blnTaskCount >= 8)
                        wait.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        //* Added by - Ajit Jadhav
        //* Added Date - 5/8/2016
        //* Comments - Create New Master Table BDMaster And Get Data
        private void FillBdMaster()
        {
            //cmbBD
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BdMaster;
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
                            cmbBD.ItemsSource = null;
                            cmbBD.ItemsSource = results.ToList();
                            cmbBD.SelectedItem = results.ToList()[0];

                        }
                        else
                        {
                            cmbBD.ItemsSource = null;
                            cmbBD.ItemsSource = objList;
                            cmbBD.SelectedItem = objList[0];
                        }
                    }

                    if (this.DataContext != null)
                    {
                        cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }

        //***//-----------------------------------
        private void ClosePatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            // ((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }

        private void SavePatientButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            string msgText = "";

            if (ClickedFlag == 1)
            {
                bool saveDtls = true;

                saveDtls = CheckValidations();

                if (saveDtls == true && visitwin != null)
                {
                    saveDtls = visitwin.CheckValidations();
                    if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Visit;
                }

                if (saveDtls)
                {
                    if (PatientSponsorNewMode == false)
                    {
                        if (Sponsortwin != null && PatientEditMode == false)
                        {
                            saveDtls = Sponsortwin.CheckValidations();
                            if (!saveDtls) PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
                        }
                    }
                }
                //AdultPatient objAdult = checkValidationAdult();

                if (saveDtls == true)  //&& objAdult.IsAdult == false
                {
                    //if (PatientSponsorNewMode == false)
                    //{
                        ////By Anjali.................................


                        if (txtYY.Text != "" && txtSpouseYY.Text != "" && tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && Convert.ToInt16(txtYY.Text) < 18 && Convert.ToInt16(txtSpouseYY.Text) < 21)
                        {

                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Couple is under standard age limit\n still you want to continue with registration", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    //msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed2);
                                    //msgW1.Show();
                                    checkDuplication();
                                }
                                else
                                {
                                    ClickedFlag = 0;
                                }

                            };
                            msgW.Show();
                        }
                        else if (txtYY.Text != "" && Convert.ToInt16(txtYY.Text) < 18)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient age is below 18 years, Kindly Caution the Medical Team \n still you want to continue with registration", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    //msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed2);
                                    //msgW1.Show();
                                    checkDuplication();
                                }
                                else
                                {
                                    ClickedFlag = 0;
                                }

                            };
                            msgW.Show();
                        }
                        else if (tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && txtSpouseYY.Text != "" && Convert.ToInt16(txtSpouseYY.Text) < 21)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                                                              new MessageBoxControl.MessageBoxChildWindow("Palash", "Spouse age is below 21 years, Kindly Caution the Medical Team \n still you want to continue with registration", MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    //MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    //                                     new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                                    //msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed2);
                                    //msgW1.Show();
                                    checkDuplication();
                                }
                                else
                                {
                                    ClickedFlag = 0;
                                }

                            };
                            msgW.Show();
                        }
                        else
                        {
                            //MessageBoxControl.MessageBoxChildWindow msgW1 =
                            //                                                                                   new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            //msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed2);
                            //msgW1.Show();
                            checkDuplication();
                            //ClickedFlag = 0;
                        }
                        //..........................................
                        //}
                    //}

                    //else
                    //{
                    //    msgText = "Please Fill Sponsor Required Fields.";
                    //    MessageBoxControl.MessageBoxChildWindow msgW =
                    //          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                    //    msgW.Show();
                    //    ClickedFlag = 0;

                    //}
                }
                else
                {
                    // msgText = "Are you sure you want to save Patient Details";  //string msgText = "";
                    msgText = "Please Fill Required Fields.";
                    MessageBoxControl.MessageBoxChildWindow msgW =
                          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                    msgW.Show();
                    ClickedFlag = 0;
                }
            }
        }

        void msgW_OnMessageBoxClosed2(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                checkDuplication();
            else
                ClickedFlag = 0;
        }

        AdultPatient checkValidationAdult()
        {
            AdultPatient objAdult = new AdultPatient();

            if (txtYY.Text != "" && txtSpouseYY.Text != "" && tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && Convert.ToInt16(txtYY.Text) < 18 && Convert.ToInt16(txtSpouseYY.Text) < 21)
            {
                objAdult.IsAdult = true;
                objAdult.msgAdult = "Couple is under standard age limit\n still you want to continue with registration";
            }
            else if (txtYY.Text != "" && Convert.ToInt16(txtYY.Text) < 18)
            {
                objAdult.IsAdult = true;
                objAdult.msgAdult = "Patient age is below 18 years\n still you want to continue with registration";
            }
            else if (tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && txtSpouseYY.Text != "" && Convert.ToInt16(txtSpouseYY.Text) < 21)
            {
                objAdult.IsAdult = true;
                objAdult.msgAdult = "Spouse age is below 21 years\n still you want to continue with registration";
            }
            else
            {
                objAdult.IsAdult = false;
            }
            return objAdult;
        }

        void checkDuplication()
        {
            clsCheckPatientDuplicacyBizActionVO BizAction = new clsCheckPatientDuplicacyBizActionVO();

            BizAction.PatientDetails = ((clsPatientVO)this.DataContext);
            BizAction.PatientDetails.SpouseDetails = ((clsPatientVO)this.DataContext).SpouseDetails;
            if ((MasterListItem)cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (dtpDOB.SelectedDate != null)
            {
                BizAction.PatientDetails.GeneralDetails.DateOfBirth = dtpDOB.SelectedDate.Value.Date;
            }

            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text))
            {
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text;
            }

            if (!string.IsNullOrEmpty(txtContactNo1.Text))
            {
                BizAction.PatientDetails.GeneralDetails.ContactNO1 = txtContactNo1.Text;
            }



            if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 7) // Couple
            {
                if (PatientEditMode == false)
                {
                    if ((MasterListItem)cmbSpouseGender.SelectedItem != null)
                        BizAction.PatientDetails.SpouseDetails.GenderID = ((MasterListItem)cmbSpouseGender.SelectedItem).ID;
                    if (dtpSpouseDOB.SelectedDate != null)
                    {
                        BizAction.PatientDetails.SpouseDetails.DateOfBirth = dtpSpouseDOB.SelectedDate.Value.Date;
                    }

                    if (!string.IsNullOrEmpty(txtSpouseMobileCountryCode.Text))
                    {
                        BizAction.PatientDetails.SpouseDetails.MobileCountryCode = txtSpouseMobileCountryCode.Text;
                    }

                    if (!string.IsNullOrEmpty(txtSpouseContactNo1.Text))
                    {
                        BizAction.PatientDetails.SpouseDetails.ContactNO1 = txtSpouseContactNo1.Text;
                    }
                }

            }
            else
            {
                BizAction.PatientDetails.SpouseDetails.GenderID = 0;
                BizAction.PatientDetails.SpouseDetails.DateOfBirth = null;
                BizAction.PatientDetails.SpouseDetails.MobileCountryCode = null;
                BizAction.PatientDetails.SpouseDetails.ContactNO1 = null;
            }

            BizAction.PatientEditMode = PatientEditMode;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus != 0)
                    {
                        ClickedFlag = 0;

                        //MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //    new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //msgW1.Show();

                        string strDuplicateMsg = "";


                        if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus == 3)
                        {
                            strDuplicateMsg = "Mobile Number already exists, Are you sure you want to save the Patient Details ?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                                                    new MessageBoxControl.MessageBoxChildWindow("Palash", strDuplicateMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW1.Show();
                        }
                        else
                        {
                            //strDuplicateMsg = "Patient already exists, Are you sure you want to save the Patient Details ?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                    }
                    else
                    {


                        string strMsg = string.Empty;
                        //if (((MasterListItem)(cmbIdentity.SelectedItem)).ID == 0)//((clsPatientVO)this.DataContext).Photo == null ||
                        //    strMsg = ", without ID Proof ?";
                        //else if ((((MasterListItem)(cmbSpouseIdentity.SelectedItem)).ID == 0) && tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible)//((clsPatientVO)this.DataContext).SpouseDetails.Photo == null ||
                        //    strMsg = ", without spouse ID Proof ?";

                        string msgText = "";
                        msgText = "Are you sure you want to save Patient Details" + strMsg;


                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                                                                                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW1.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        msgW1.Show();


                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();


        }

        class AdultPatient
        {
            public bool IsAdult;
            public string msgAdult;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (Sponsortwin != null && Sponsortwin.cmbPatientCategory.SelectedItem == null)
            {
                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
            }
            if (result == MessageBoxResult.Yes)
            {
                SavePatient();
            }
            else
            {
                ClickedFlag = 0;

                //* Added by - Ajit Jadhav
                //* Added Date - 24/8/2016
                //* Comments - Not Display Patient Details Window
                formSaveType = SaveType.Registration;
                //***//----------------------
            }
        }

        private void SavePatient()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            clsAddPatientBizActionVO BizAction = new clsAddPatientBizActionVO();

            BizAction.IsAddWithtransaction = true;   // set to save Patient Details, Sponsor Details, Admission Details with transaction object to save/rollback all data at a time in case of any error
            BizAction.IsSavePatientFromOPD = true;    // set to decide save Patient Detials from OPD or not

            BizAction.PatientDetails = (clsPatientVO)this.DataContext;

            if (dtpDOB.SelectedDate == null)
            {
                BizAction.PatientDetails.GeneralDetails.IsAge = true;
                if (DOB != null)
                    BizAction.PatientDetails.GeneralDetails.DateOfBirth = DOB.Value.Date;
            }

            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

            if (cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;

            if (cmbPatientType.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.PatientTypeID = ((MasterListItem)cmbPatientType.SelectedItem).ID;

            if (cmbPatientSource.SelectedItem != null) // Changed by Saily P for SFC
                BizAction.PatientDetails.GeneralDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;


            BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.ResiNoCountryCode = Convert.ToInt64(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.PatientDetails.ResiSTDCode = Convert.ToInt64(txtResiSTD.Text.Trim());

            if (cmbReferralName.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;
            if (InhouseDoc.IsChecked == true)
                BizAction.PatientDetails.GeneralDetails.IsReferralDoc = false;
            else
                BizAction.PatientDetails.GeneralDetails.IsReferralDoc = true;

            if (cmbReferralDoctor.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralDoctorID = ((MasterListItem)cmbReferralDoctor.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtReferralDetail.Text.Trim()))
                BizAction.PatientDetails.GeneralDetails.ReferralDetail = txtReferralDetail.Text.Trim();

            if (cmbPreffix.SelectedItem != null)
                BizAction.PatientDetails.PrefixId = ((MasterListItem)cmbPreffix.SelectedItem).ID;

            //* Added by - Ajit Jadhav
            //* Added Date - 4/8/2016
            //* Comments - New BD Master Combo Add Save Data to T_Registration
            if (cmbBD.SelectedItem != null)
                BizAction.PatientDetails.BDID = ((MasterListItem)cmbBD.SelectedItem).ID;
            //***//...................................

            //======================================================
            //Spouse
            if (!PatientEditMode)
            {
                if (dtpSpouseDOB.SelectedDate == null)
                {
                    BizAction.PatientDetails.SpouseDetails.IsAge = true;
                    if (SpouseDOB != null)
                        BizAction.PatientDetails.SpouseDetails.DateOfBirth = SpouseDOB.Value.Date;
                }



                if (cmbSpouseMaritalStatus.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.MaritalStatusID = ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID;

                if (cmbSpouseGender.SelectedItem != null || cmbGender.SelectedItem != cmbSpouseGender.SelectedItem)
                    BizAction.PatientDetails.SpouseDetails.GenderID = ((MasterListItem)cmbSpouseGender.SelectedItem).ID;

                else
                {
                    cmbSpouseGender.SetValidation("Check Gender!");
                    cmbSpouseGender.RaiseValidationError();
                    cmbSpouseGender.Focus();
                }
                // End

                BizAction.PatientDetails.SpouseDetails.ContactNo1 = txtSpouseContactNo1.Text.Trim();
                BizAction.PatientDetails.SpouseDetails.ContactNo2 = txtSpouseContactNo2.Text.Trim();
                if (!string.IsNullOrEmpty(txtSpouseMobileCountryCode.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.MobileCountryCode = txtSpouseMobileCountryCode.Text.Trim();

                if (!string.IsNullOrEmpty(txtSpouseResiCountryCode.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.ResiNoCountryCode = Convert.ToInt64(txtSpouseResiCountryCode.Text.Trim());

                if (!string.IsNullOrEmpty(txtSpouseResiSTD.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.ResiSTDCode = Convert.ToInt64(txtSpouseResiSTD.Text.Trim());


                if (cmbReferralName.SelectedItem != null)
                    BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;


                //By Anjali.........................

                if (cmbSpousePreffix.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.PrefixId = ((MasterListItem)cmbSpousePreffix.SelectedItem).ID;


                //======================================================

                // Added By CDS
                if (sponsorContent.Content != null && PatientEditMode == false)
                {
                    BizAction.IsSaveSponsor = true;
                    BizAction.BizActionVOSaveSponsor = SaveSponsorWithTransaction();
                    if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)  // AppConfig.  M_PatientCategoryMaster= Couple
                    {
                        BizAction.BizActionVOSaveSponsorForMale = SaveSponsorWithTransactionMale();
                    }
                }

            }
            //======================================================
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    CmdSave.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientID = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                    ((clsPatientVO)this.DataContext).GeneralDetails.MRNo = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                    ((clsPatientVO)this.DataContext).GeneralDetails.UnitId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId;
                    ((clsPatientVO)this.DataContext).SpouseDetails.PatientID = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.SpouseDetails.PatientID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.UnitId = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.SpouseDetails.UnitId;
                    ((IApplicationConfiguration)App.Current).SelectedPatient = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                    ((clsPatientVO)this.DataContext).SpouseDetails.MRNo = ((clsAddPatientBizActionVO)arg.Result).PatientDetails.SpouseDetails.MRNo;

                    if (((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveSponsor != null)
                    {
                        ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID = Convert.ToInt64(((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveSponsor.PatientSponsorDetails.PatientSourceID);
                        ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID = Convert.ToInt64(((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveSponsor.PatientSponsorDetails.PatientCategoryID);
                        ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID = Convert.ToInt64(((clsAddPatientBizActionVO)arg.Result).BizActionVOSaveSponsor.PatientSponsorDetails.CompanyID);
                    }

                    if (mAppointmentID > 0)
                        UpdateAppointment(((clsPatientVO)this.DataContext).GeneralDetails.PatientID, mAppointmentID, ((clsPatientVO)this.DataContext).GeneralDetails.UnitId);

                    Indicatior.Close();

                    //if (sponsorContent.Content != null && PatientEditMode == false)
                    //    SaveSponsor();

                    PatientEditMode = true;
                    //if (sponsorContent.Content == null && PatientEditMode == false)
                    //    GetPatientDetails(((clsPatientVO)this.DataContext).GeneralDetails.PatientID);
                    //((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                        txtMRNumber.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;


                    //Closed By akshays on 18/11/2015


                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.OnMessageBoxClosed += (re) =>
                    {
                        //if (re == MessageBoxResult.OK)
                        //{
                        //}
                        if (formSaveType == SaveType.Registration)
                        {
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        }
                        else if (formSaveType == SaveType.OPD)
                        {
                            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.EncounterWindow") as UIElement;
                            visitwin = new EncounterWindow();
                            visitwin.VAppointmentID = mAppointmentID;
                            ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID = BizAction.BizActionVOSaveSponsor.PatientSponsorDetails.TariffID.Value;

                            visitwin.IsFromRegisterandVisit = true;
                            if ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem != null)
                                visitwin.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;


                            UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                            TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleHeader");
                            mElement1.Text = "Patient Detail";

                            ((IInitiateCIMS)visitwin).Initiate("NEW");
                            ((IApplicationConfiguration)App.Current).OpenMainContent(visitwin as UIElement);


                        }
                        //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                        string msgText = "";
                        msgText = "Are You Sure \n You Want To Print Patient Registration Report?";
                        MessageBoxControl.MessageBoxChildWindow msgW =
                              new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(rptPatientCase);
                        msgW.Show();
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

                else if ((MasterListItem)cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

                if (Sponsortwin.cmbPatientCategory.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientCategoryID = ((MasterListItem)Sponsortwin.cmbPatientCategory.SelectedItem).ID;

                if (Sponsortwin.cmbTariff.SelectedItem != null)
                    BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)Sponsortwin.cmbTariff.SelectedItem).ID;

                if (Sponsortwin.cmbCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;

                if (Sponsortwin.cmbAssCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.AssociatedCompanyID = ((MasterListItem)Sponsortwin.cmbAssCompany.SelectedItem).ID;

                if (Sponsortwin.cmbDesignation.SelectedItem != null)
                    BizAction.PatientSponsorDetails.DesignationID = ((MasterListItem)Sponsortwin.cmbDesignation.SelectedItem).ID;

                if (Sponsortwin.cmbPackageRelations.SelectedItem != null)
                    BizAction.PatientSponsorDetails.MemberRelationID = ((MasterListItem)Sponsortwin.cmbPackageRelations.SelectedItem).ID;
                else
                    BizAction.PatientSponsorDetails.MemberRelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID;

                return BizAction;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }
        private clsAddPatientSponsorBizActionVO SaveSponsorWithTransactionMale()
        {
            try
            {
                //((clsPatientSponsorVO)Sponsortwin.DataContext).PatientId = ((clsPatientVO)this.DataContext).SpouseDetails.PatientID;
                //((clsPatientSponsorVO)Sponsortwin.DataContext).PatientUnitId = ((clsPatientVO)this.DataContext).SpouseDetails.UnitId;

                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails = (clsPatientSponsorVO)Sponsortwin.DataContext;

                if (Sponsortwin.cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)Sponsortwin.cmbPatientSource.SelectedItem).ID;

                else if ((MasterListItem)cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

                if (Sponsortwin.cmbPatientCategory.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientCategoryID = ((MasterListItem)Sponsortwin.cmbPatientCategory.SelectedItem).ID;

                if (Sponsortwin.cmbTariff.SelectedItem != null)
                    BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)Sponsortwin.cmbTariff.SelectedItem).ID;

                if (Sponsortwin.cmbCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;

                if (Sponsortwin.cmbAssCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.AssociatedCompanyID = ((MasterListItem)Sponsortwin.cmbAssCompany.SelectedItem).ID;

                if (Sponsortwin.cmbDesignation.SelectedItem != null)
                    BizAction.PatientSponsorDetails.DesignationID = ((MasterListItem)Sponsortwin.cmbDesignation.SelectedItem).ID;

                if (Sponsortwin.cmbPackageRelations.SelectedItem != null)
                    BizAction.PatientSponsorDetails.MemberRelationID = ((MasterListItem)Sponsortwin.cmbPackageRelations.SelectedItem).ID;
                else
                    BizAction.PatientSponsorDetails.MemberRelationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID;

                return BizAction;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
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
                    // System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
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
                            //if (((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID != null)
                            //{
                            //    ((clsPatientSponsorVO)Sponsortwin.DataContext).ID = ((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID;
                            //    //System.Windows.Browser.HtmlPage.Window.Alert("Sponsor Saved Successfully with ID " + ((clsPatientSponsorVO)this.DataContext).SponsorID);
                            //}
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
                        sponsorContent.Content = null;
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
                // Open Serch Patient
                //AppointmentList win = new AppointmentList();
                // StaffWindow StaffWin = new StaffWindow();
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
            //throw new NotImplementedException();
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
                            cmbMaritalStatus.SelectedValue = myPatient.MaritalStatusID;

                            txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                            txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                            txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                            PatPersonalInfo.SelectedIndex = 0;

                            if (sponsorContent.Content != null)
                            {
                                Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
                                if (Sponsortwin.DataContext != null)
                                {
                                    ((clsPatientSponsorVO)Sponsortwin.DataContext).PatientSourceID = myPatient.GeneralDetails.PatientSourceID;
                                }
                            }
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

        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
            ((clsPatientVO)this.DataContext).FamilyName = txtLastName.Text; //((clsPatientVO)this.DataContext).GeneralDetails.LastName;
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
            //int Yearval = 0;
            //int Monthval = 0;
            //int DayVal = 0;

            //if (txtSpouseYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseYY.Text.Trim()))
            //    Yearval = int.Parse(txtSpouseYY.Text.Trim());

            //if (txtSpouseMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseMM.Text.Trim()))
            //    Monthval = int.Parse(txtSpouseMM.Text.Trim());


            //if (txtSpouseDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtSpouseDD.Text.Trim()))
            //    DayVal = int.Parse(txtSpouseDD.Text.Trim());

            //if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            //{
            //    dtpSpouseDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
            //    dtpSpouseDOB.ClearValidationError();
            //    txtSpouseYY.ClearValidationError();
            //    txtSpouseMM.ClearValidationError();
            //    txtSpouseDD.ClearValidationError();
            //}
            //else
            //{
            //    txtSpouseYY.SetValidation("Age Is Required");
            //    txtSpouseYY.RaiseValidationError();
            //    txtSpouseMM.SetValidation("Age Is Required");
            //    txtSpouseMM.RaiseValidationError();
            //    txtSpouseDD.SetValidation("Age Is Required");
            //    txtSpouseDD.RaiseValidationError();

            //    dtpSpouseDOB.SetValidation("Please Select The Date of Birth");
            //    dtpSpouseDOB.RaiseValidationError();
            //    txtSpouseYY.Text = "0";
            //    txtSpouseMM.Text = "0";
            //    txtSpouseDD.Text = "0";


            //}


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
                SpouseDOB = CalculateDateFromAge(Yearval, Monthval, DayVal);

            }
            else
            {




            }
        }
        DateTime? DOB = null;
        DateTime? SpouseDOB = null;
        void CalculateBirthDate()
        {

            //int Yearval = 0;
            //int Monthval = 0;
            //int DayVal = 0;

            //if (txtYY.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtYY.Text.Trim()))
            //    Yearval = int.Parse(txtYY.Text.Trim());

            //if (txtMM.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtMM.Text.Trim()))
            //    Monthval = int.Parse(txtMM.Text.Trim());


            //if (txtDD.Text.Trim().IsItNumber() && !string.IsNullOrEmpty(txtDD.Text.Trim()))
            //    DayVal = int.Parse(txtDD.Text.Trim());

            //if (Yearval > 0 || Monthval > 0 || DayVal > 0)
            //{
            //    dtpDOB.SelectedDate = CalculateDateFromAge(Yearval, Monthval, DayVal);
            //    dtpDOB.ClearValidationError();
            //    txtYY.ClearValidationError();
            //    txtMM.ClearValidationError();
            //    txtDD.ClearValidationError();
            //}
            //else
            //{

            //    txtYY.SetValidation("Age is Required");
            //    txtYY.RaiseValidationError();
            //    txtMM.SetValidation("Age is Required");
            //    txtMM.RaiseValidationError();
            //    txtDD.SetValidation("Age is Required");
            //    txtDD.RaiseValidationError();

            //    dtpDOB.SetValidation("Please select the Date of birth");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";


            //}

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
                DOB = CalculateDateFromAge(Yearval, Monthval, DayVal);

            }
            else
            {




            }
        }

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
            //if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth > DateTime.Now)
            //{
            //    //txtYY.SetValidation("Age Is Required");
            //    //txtYY.RaiseValidationError();
            //    //txtMM.SetValidation("Age Is Required");
            //    //txtMM.RaiseValidationError();
            //    //txtDD.SetValidation("Age Is Required");
            //    //txtDD.RaiseValidationError();

            //    dtpDOB.SetValidation("Date of Birth Can Not Be Greater Than Today");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null)
            //{
            //    //txtYY.SetValidation("Age Is Required");
            //    //txtYY.RaiseValidationError();
            //    //txtMM.SetValidation("Age Is Required");
            //    //txtMM.RaiseValidationError();
            //    //txtDD.SetValidation("Age Is Required");
            //    //txtDD.RaiseValidationError();

            //    dtpDOB.SetValidation("Please Select The Date of Birth");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else if (((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth.Value == DateTime.Now.Date)
            //{
            //    //txtYY.SetValidation("Age Is Required");
            //    //txtYY.RaiseValidationError();
            //    //txtMM.SetValidation("Age Is Required");
            //    //txtMM.RaiseValidationError();
            //    //txtDD.SetValidation("Age Is Required");
            //    //txtDD.RaiseValidationError();

            //    dtpDOB.SetValidation("Date of Birth Should be less than Todays Date. ");
            //    dtpDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else
            //{
            //    dtpDOB.ClearValidationError();
            //    txtYY.ClearValidationError();
            //    txtMM.ClearValidationError();
            //    txtDD.ClearValidationError();

            //    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
            //    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
            //    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            //}
            if (dtpDOB.SelectedDate != null)
            {
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
            //if (((WaterMarkTextbox)sender).Text.EndsWith(" "))
            //    {
            //        ((WaterMarkTextbox)sender).Text=((WaterMarkTextbox)sender).Text.Remove(((WaterMarkTextbox)sender).Text.Length - 1, 1);
            //    }
            //else
            //{
            if (IsPageLoded)
            {


                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text.Trim()))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsItNumber() && textBefore != null)
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

            // }
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
                if (((AutoCompleteBox)sender).Text.Length > 6)
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
            //if (txtEmail.Text.Length > 0)
            //{
            //    if (txtEmail.Text.IsEmailValid())
            //        txtEmail.ClearValidationError();
            //    else
            //    {
            //        txtEmail.SetValidation("Please Enter Valid Email-ID");
            //        txtEmail.RaiseValidationError();
            //    }
            //}
        }

        private void txtSpouseEmail_LostFocus(object sender, RoutedEventArgs e)
        {

            //if (txtSpouseEmail.Text.Length > 0)
            //{
            //    if (txtSpouseEmail.Text.IsEmailValid())
            //        txtSpouseEmail.ClearValidationError();
            //    else
            //    {
            //        txtSpouseEmail.SetValidation("Please Enter Valid Email-ID");
            //        txtSpouseEmail.RaiseValidationError();
            //    }
            //}
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

                    //by Anjali...........................
                    ((clsPatientVO)this.DataContext).SpecialRegID = mObj.AppointmentDetails.SpecialRegistrationID;

                    //......................................
                    txtResiCountryCode.Text = mObj.AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = mObj.AppointmentDetails.ResiSTDCode.ToString();
                    txtMobileCountryCode.Text = mObj.AppointmentDetails.MobileCountryCode;

                    txtContactNo1.Text = mObj.AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = mObj.AppointmentDetails.ContactNo2;

                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;

                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;

                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                    if (mObj.AppointmentDetails.IsAge == true)
                    {
                        DOB = mObj.AppointmentDetails.DateOfBirthFromAge;
                    }
                    else
                    {
                        ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth = mObj.AppointmentDetails.DOB;
                    }
                    if (mObj.AppointmentDetails.IsAge == true)
                    {
                        txtYY.Text = ConvertDate(DOB, "YY");
                        txtMM.Text = ConvertDate(DOB, "MM");
                        txtDD.Text = ConvertDate(DOB, "DD");
                    }
                    else
                    {
                        txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                        txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                        txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                    }

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

        private void txtArea_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }
        private void cmbPatientType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPatientType.SelectedItem != null)
                SetPatientCategorywiseDefaults(((MasterListItem)cmbPatientType.SelectedItem).ID);
            if (this.DataContext != null && IsfromEdit != true)
                ((clsPatientVO)this.DataContext).ScanDocList = new List<clsPatientScanDocumentVO>();

        }

        private void SetPatientCategorywiseDefaults(long pPatientCategoryID)
        {
            tabPatSpouseInformation.Visibility = System.Windows.Visibility.Collapsed;
            tabPatSponsorInfo.Visibility = System.Windows.Visibility.Visible;
            //tabPatEncounterInfo.Visibility = System.Windows.Visibility.Visible;

            cmbGender.IsEnabled = true;
            cmbSpouseGender.IsEnabled = true;

            switch (pPatientCategoryID)
            {
                case 7: // Couple
                    tabPatSponsorInfo.Visibility = System.Windows.Visibility.Collapsed;
                    if (!PatientEditMode)
                    {
                        //tabPatEncounterInfo.Visibility = System.Windows.Visibility.Collapsed;                       
                        tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;
                        tabPatSponsorInfo.Visibility = System.Windows.Visibility.Visible;

                        ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                        cmbGender.SelectedValue = (long)Genders.Female;
                        cmbSpouseGender.SelectedValue = (long)Genders.Male;
                    }
                    // Addd By CDS 
                    if (PatientEditMode)
                    {
                        //tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;
                        tabPatSponsorInfo.Visibility = System.Windows.Visibility.Visible;
                    }
                    //Added By Yogita
                    cmbGender.IsEnabled = false;
                    cmbSpouseGender.IsEnabled = false;



                    break;
                //End

                case 8://Female Donor
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    cmbGender.SelectedValue = (long)Genders.Female;
                    break;

                case 9://Male Donor
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Male;
                    cmbGender.SelectedValue = (long)Genders.Male;
                    break;

                case 10://Surrogacy
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    cmbGender.SelectedValue = (long)Genders.Female;
                    break;


                // BY ANJALI FOR ANC . . . . 
                case 12: // ANC
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    cmbGender.SelectedValue = (long)Genders.Female;
                    break;
                default:
                    break;
            }

        }

        private void txtSpouseLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseLastName.Text = txtSpouseLastName.Text.ToTitleCase();
            ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName = txtSpouseLastName.Text;
        }

        private void txtSpouseFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSpouseFirstName.Text = txtSpouseFirstName.Text.ToTitleCase();
        }

        private void dtpSpouseDOB_LostFocus(object sender, RoutedEventArgs e)
        {

            if (dtpSpouseDOB.SelectedDate != null)
            {
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

        private void CmdOPD_Click(object sender, RoutedEventArgs e)
        {
            formSaveType = SaveType.OPD;
            SavePatientButton_Click(sender, e);
        }
        private void cmbTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            DoctorMasterChildwindow win = new DoctorMasterChildwindow();
            ((IInitiateCIMS)win).Initiate("PRO");
            win.OnSaveButton_Click += new RoutedEventHandler(DoctorWin_OnSaveButton_Click);
            win.Show();
        }
        void DoctorWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (((DoctorMasterChildwindow)sender).DoctorId > 0)
            {
                FillRefDoctor(((DoctorMasterChildwindow)sender).DoctorId);

            }
            cmbPreffix.Focus();
            cmbPreffix.UpdateLayout();
        }

        void rptPatientCase(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                rptpatientReport();
        }

        private void rptpatientReport()
        {
            long ID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            long UnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
            //long IsDoctorID ;
            //long IsEmployee;
            //long DoctorID ;
            //long EmployeeID;

            if (ID > 0 && UnitID > 0)
            {

                string URL = "../Reports/Patient/rptPatientCaseReportNew.aspx?ID=" + ID + "&UnitId=" + UnitID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }


        }

        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
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

        private void cmbReferralDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            cmbPreffix.Focus();
            cmbPreffix.UpdateLayout();
        }

        private void txtNoOfPeople_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.IsItDecimal())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNoOfPeople_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void ReferralDoc_Click(object sender, RoutedEventArgs e)
        {
            if (ReferralDoc.IsChecked == true)
            {
                FillRefDoctor();
                //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = true;
                cmdSearch.IsEnabled = true;
            }
            else if (InhouseDoc.IsChecked == true)
            {
                fillInhouseDoctor();
                //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = false;
                cmdSearch.IsEnabled = false;
            }
        }

        private void fillInhouseDoctor()
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.IsNonReferralDoctor = true;
            BizAction.ReferralID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorTypeForReferral;
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbReferralDoctor.ItemsSource = null;
                    cmbReferralDoctor.ItemsSource = objList;
                    cmbReferralDoctor.SelectedItem = objList[0];

                    if (this.DataContext != null)
                    {
                        cmbReferralDoctor.SelectedValue = ((clsPatientVO)this.DataContext).ReferralDoctorID;

                    }
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void chkIfYes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbMaritalStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMaritalStatus.SelectedItem != null && ((MasterListItem)cmbMaritalStatus.SelectedItem).ID > 0)
            {
                cmbSpouseMaritalStatus.SelectedValue = cmbMaritalStatus.SelectedValue;
                cmbSpouseMaritalStatus.SelectedItem = cmbMaritalStatus.SelectedItem;
            }
        }

        private void cmbSpouseMaritalStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSpouseMaritalStatus.SelectedItem != null && ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID > 0)
            {
                cmbMaritalStatus.SelectedValue = cmbSpouseMaritalStatus.SelectedValue;
                cmbMaritalStatus.SelectedItem = cmbSpouseMaritalStatus.SelectedItem;
            }
        }

        private void txtResiCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;

        }

        private void txtResiCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {


                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text.Trim()))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsItNumber() && textBefore != null)
                    {
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 3)
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

        private void txtResiSTD_KeyDown(object sender, KeyEventArgs e)
        {

            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtResiSTD_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {


                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text.Trim()))
                {
                    if (!((WaterMarkTextbox)sender).Text.IsItNumber() && textBefore != null)
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


        private void cmbReferralName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbReferralName.SelectedItem).ID == 1)
            {
                ReferralDoc.IsChecked = true;
                ReferralDoc.IsEnabled = true;
                InhouseDoc.IsEnabled = false;
                txtReferralDetail.ClearValidationError();
                FillRefDoctor();
                //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = true;
                cmdSearch.IsEnabled = true;
                cmbReferralDoctor.IsEnabled = true;
            }
            else
                if (((MasterListItem)cmbReferralName.SelectedItem).ID == 2)
                {
                    ReferralDoc.IsEnabled = false;
                    InhouseDoc.IsEnabled = true;
                    InhouseDoc.IsChecked = true;
                    txtReferralDetail.ClearValidationError();
                    fillInhouseDoctor();
                    //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = false;
                    cmdSearch.IsEnabled = false;
                    cmbReferralDoctor.IsEnabled = true;
                }
                else
                {
                    txtReferralDetail.SetValidation("Referral Detail Is Required");
                    txtReferralDetail.RaiseValidationError();
                    ReferralDoc.IsEnabled = false;
                    InhouseDoc.IsEnabled = false;
                    ReferralDoc.IsChecked = true;
                    cmbReferralDoctor.IsEnabled = false;
                    cmdSearch.IsEnabled = false;
                    cmbReferralDoctor.TextBox.ClearValidationError();
                }


        }
    }
}
