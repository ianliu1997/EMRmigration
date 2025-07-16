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
using PalashDynamics.ValueObjects.Master.DoctorMaster;
//added By Akshays On 17/11/2015
//using PalashDynamics.Pharmacy.BarCode;
using ImageTools.IO.Png;

using ImageTools;
using PalashDynamics.Pharmacy.BarCode;
using PalashDynamics.IVF.Barcode;
using ImageTools;
using ImageTools.IO;
using ImageTools.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using PalashDynamics.ValueObjects.CRM;


//using System;
//using System.IO;
//closed by akshays On 17/11/2015

namespace CIMS
{
    public partial class PatientRegistration : UserControl, IInitiateCIMS
    {
        private bool PatientEditMode = false;
        bool IsPatientExist = false;
        bool Flagref = false;
        //bool PatientSourceFromLoyalty = false;
        public byte[] MyPhoto { get; set; }
        bool IsfromEdit = false;
        bool ChangeIdentityDoc = false;
        bool ValidationsFlag = false; //Added by NileshD on 19April2019
        int InternationalId = 0;

        //added by neena
        MasterListItem RegionObj;
        MasterListItem CityObj;
        MasterListItem StateObj;
        MasterListItem CountryObj;

        MasterListItem SpouseRegionObj;
        MasterListItem SpouseCityObj;
        MasterListItem SpouseStateObj;
        MasterListItem SpouseCountryObj;
        bool chkImgNameFlag = false;
        //

        //***//-----
        private long BankId { get; set; }
        private long BranchId { get; set; }
        private string AccountNumber { get; set; }
        private string AccountHolderName { get; set; }
        private bool AccountTypeId { get; set; }
        private string IFSCCode { get; set; }

        private long BDMID { get; set; }

        

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
                    mElement1.Text = ": New Patient";

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

                        //added by neena
                        Region = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Region,
                        RegionCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionCode,
                        CityN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityN,
                        CityCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityCode,
                        StateN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateN,
                        StateCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateCode,
                        CountryN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryN,
                        CountryCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryCode,
                        //

                        PrefixId = 0,
                        IdentityID = 0,
                        IsInternationalPatient = false,
                        BDID = 0,
                        NationalityID = 0,
                        PreferredLangID = 0,
                        TreatRequiredID = 0,
                        EducationID = 0,
                        SpecialRegID = 0,

                        // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                    };
                    chkIfNordo.IsChecked = true;
                    // chkIfNoJoint.IsChecked = true; //***//
                    ReferralDoc.IsChecked = true;
                    txtClinicName.IsEnabled = false;
                    FillRefDoctor();
                    FillCoConsultantDoctor();
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

                    //added by neena
                    ((clsPatientVO)this.DataContext).SpouseDetails.Region = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Region;
                    ((clsPatientVO)this.DataContext).SpouseDetails.RegionCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryCode;
                    //


                    ((clsPatientVO)this.DataContext).SpouseDetails.NationalityID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.PreferredLangID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.TreatRequiredID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.EducationID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.SpecialRegID = 0;

                    ((clsPatientVO)this.DataContext).BDID = 0;





                    break;

                case "EDIT":
                    #region Patient Edit Mode
                    PatientEditMode = true;
                    IsPatientExist = true;
                    IsfromEdit = true;
                    CmdOPD.Visibility = Visibility.Collapsed;

                    //added by neena
                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSurrogate == true)
                        Flagref = true;
                    //

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }

                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.PatientDetails.GeneralDetails.BringScanDoc = true;
                    BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID; //Added by AJ Date 4/1/2017 C  -- Added on 18042019 for CC merging
                    BizAction.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatient.LinkServer;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                //tabBillingInfo.Visibility = Visibility.Visible;
                                //tabPatEncounterInfo.Visibility = Visibility.Visible;
                                //tabPatSponsorInfo.Visibility = Visibility.Visible;

                                this.DataContext = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;

                                //***//
                                BankId = ((clsGetPatientBizActionVO)arg.Result).BankDetails.BankId;
                                BranchId = ((clsGetPatientBizActionVO)arg.Result).BankDetails.BranchId;
                                AccountNumber = ((clsGetPatientBizActionVO)arg.Result).BankDetails.AccountNumber;
                                AccountHolderName = ((clsGetPatientBizActionVO)arg.Result).BankDetails.AccountHolderName;
                                AccountTypeId = ((clsGetPatientBizActionVO)arg.Result).BankDetails.AccountTypeId;
                                IFSCCode = ((clsGetPatientBizActionVO)arg.Result).BankDetails.IFSCCode;
                                //----------
                                if (this.DataContext != null)
                                {


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



                                    cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                                    cmbPatientType.IsEnabled = false;
                                    cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;  // BY BHUSHAN . . . . .
                                    //Modified by Saily P for SFC to get the patient source on Registration form
                                    cmbPatientSource.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID;
                                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
                                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                                    txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                                    txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                                    txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                                    txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;

                                    //by Anjali........................................
                                    cmbPreffix.SelectedValue = ((clsPatientVO)this.DataContext).PrefixId;
                                    cmbIdentity.SelectedValue = ((clsPatientVO)this.DataContext).IdentityID;
                                    //.................................................

                                    cmbCoConsultantDoctor.SelectedValue = ((clsPatientVO)this.DataContext).CoConsultantDoctorID; //***//19
                                    cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;
                                    //***//
                                    cmbCampDetail.SelectedValue = ((clsPatientVO)this.DataContext).CampID;
                                    cmbAgentType.SelectedValue = ((clsPatientVO)this.DataContext).AgentID;
                                    cmbAgencyName.SelectedValue = ((clsPatientVO)this.DataContext).AgencyID;

                                    txtNoOfYearsOfMarriage.Text = Convert.ToString(((clsPatientVO)this.DataContext).NoOfYearsOfMarriage);
                                    txtNoOfExistingChildren.Text = Convert.ToString(((clsPatientVO)this.DataContext).NoOfExistingChildren);
                                    cmbFamilyType.SelectedValue = ((clsPatientVO)this.DataContext).FamilyTypeID;

                                    //Added by Ashish---
                                    cmbNationality.SelectedValue = ((clsPatientVO)this.DataContext).NationalityID;
                                    cmbPreferredLanguage.SelectedValue = ((clsPatientVO)this.DataContext).PreferredLangID;
                                    cmbTreatmentRequired.SelectedValue = ((clsPatientVO)this.DataContext).TreatRequiredID;
                                    cmbEducation.SelectedValue = ((clsPatientVO)this.DataContext).EducationID;
                                    dtpMarriageAnnDate.SelectedDate = ((clsPatientVO)this.DataContext).MarriageAnnDate;
                                    if (((clsPatientVO)this.DataContext).IsClinicVisited == true)
                                    {
                                        chkIfYesrdo.IsChecked = true;

                                    }
                                    else
                                    {
                                        chkIfNordo.IsChecked = true;
                                        txtClinicName.IsEnabled = false;
                                    }
                                    cmbSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpecialRegID;
                                    if (((clsPatientVO)this.DataContext).ClinicName != null)
                                        txtClinicName.Text = ((clsPatientVO)this.DataContext).ClinicName;
                                    txtNoOfPeople.Text = Convert.ToString(((clsPatientVO)this.DataContext).NoOfPeople);
                                    //
                                    if (((clsPatientVO)this.DataContext).IsReferralDoc == true)
                                    {
                                        ReferralDoc.IsChecked = true;
                                      //  FillRefDoctor();
                                        cmdSearch.IsEnabled = true;
                                    }
                                    else
                                    {
                                        InhouseDoc.IsChecked = true;
                                       // fillInhouseDoctor();
                                        cmdSearch.IsEnabled = false;
                                    }



                                    //txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                    //txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                    //txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");

                                    txtContactNo1.Text = ((clsPatientVO)this.DataContext).ContactNo1;
                                    txtContactNo2.Text = ((clsPatientVO)this.DataContext).ContactNo2;
                                    txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode;
                                    if (((clsPatientVO)this.DataContext).ResiNoCountryCode != null && ((clsPatientVO)this.DataContext).ResiNoCountryCode != 0)
                                        txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    else
                                        txtResiCountryCode.Text = "";
                                    if (((clsPatientVO)this.DataContext).ResiSTDCode != null && ((clsPatientVO)this.DataContext).ResiSTDCode != 0)
                                        txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();
                                    else
                                        txtResiSTD.Text = "";
                                    if (((clsPatientVO)this.DataContext).Pincode != null)
                                    {
                                        txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode.ToString();

                                        txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).Pincode.ToString();
                                    }
                                    if (((clsPatientVO)this.DataContext).CompanyName != null)
                                    {
                                        txtSpouseOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName.ToString();

                                        txtOfficeName.Text = ((clsPatientVO)this.DataContext).CompanyName.ToString();
                                    }


                                    //commented by neena
                                    if (((clsPatientVO)this.DataContext).Photo != null)
                                    {
                                        //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                        //bmp.FromByteArray(((clsPatientVO)this.DataContext).Photo);
                                        //imgPhoto.Source = bmp;
                                        byte[] imageBytes = ((clsPatientVO)this.DataContext).Photo;
                                        BitmapImage img = new BitmapImage();
                                        img.SetSource(new MemoryStream(imageBytes, false));
                                        imgPhoto.Source = img;
                                    }
                                    //

                                    ChangeIdentityDoc = true;

                                    //added by neena
                                    if (((clsPatientVO)this.DataContext).ImageName != null && ((clsPatientVO)this.DataContext).ImageName.Length > 0)
                                    {
                                        chkImgNameFlag = true;
                                        imgPhoto.Source = new BitmapImage(new Uri(((clsPatientVO)this.DataContext).ImageName, UriKind.Absolute));
                                    }
                                    else if (((clsPatientVO)this.DataContext).Photo != null)
                                    {
                                        byte[] imageBytes = ((clsPatientVO)this.DataContext).Photo;
                                        BitmapImage img = new BitmapImage();
                                        img.SetSource(new MemoryStream(imageBytes, false));
                                        imgPhoto.Source = img;
                                    }

                                    if (((clsPatientVO)this.DataContext).OldRegistrationNo != null)
                                    {
                                        txtOldRegistrationNo.Text = ((clsPatientVO)this.DataContext).OldRegistrationNo.ToString();

                                    }
                                    //


                                    //* Commented by - Ajit Jadhav
                                    //* Added Date - 11/8/2016
                                    //* Comments - View Pan NO And BD Name

                                    cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;
                                    if (PatientEditMode == true)
                                    {

                                        if (((clsPatientVO)this.DataContext).BDID == 0)
                                        //if(cmbBD.SelectedItem!=null && ((MasterListItem)cmbBD.SelectedItem).ID==0)
                                        {

                                            cmbBD.IsEnabled = true;
                                        }
                                        else
                                        {
                                            cmbBD.IsEnabled = false;
                                        }
                                    }

                                    if (((clsPatientVO)this.DataContext).PanNumber != null)
                                    {
                                        txtSpousePanNo.Text = ((clsPatientVO)this.DataContext).PanNumber.ToString();
                                        txtPanNo.Text = ((clsPatientVO)this.DataContext).PanNumber.ToString();
                                    }


                                    //***//--------------------


                                    ////=======================================================
                                    ////Spouse 
                                    //if (((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID == 7) // Couple
                                    //{
                                    //    dtpSpouseDOB.SelectedDate = ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth;
                                    //    cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                                    //    cmbSpouseBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.BloodGroupID;
                                    //    cmbSpouseGender.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.GenderID;

                                    //    cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;

                                    //    cmbSpouseOccupation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.OccupationId;
                                    //    txtSpouseYY.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "YY");
                                    //    txtSpouseMM.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "MM");
                                    //    txtSpouseDD.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "DD");

                                    //    txtSpouseContactNo1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo1;
                                    //    txtSpouseContactNo2.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ContactNo2;
                                    //    txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();

                                    //    txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                                    //    txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                                    //}
                                    ////=======================================================

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                    mElement.Text = " : " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName +
                                        " " + ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " +
                                        ((clsPatientVO)this.DataContext).GeneralDetails.LastName;

                                    PatientEditMode = true;

                                }

                                //added by neena
                                //commented by neena
                                //if (this.DataContext != null)
                                //    FillPatientAddress();
                                //
                                //
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

                    //Added By Yogita
                    if (PatientEditMode == true)
                    {
                        cmbGender.IsEnabled = false;
                    }
                    //End

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
                                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
                                    cmbOccupation.SelectedValue = ((clsPatientVO)this.DataContext).OccupationId;
                                    cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;

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

                                    txtContactNo1.Text = ((clsPatientVO)this.DataContext).ContactNo1;
                                    txtContactNo2.Text = ((clsPatientVO)this.DataContext).ContactNo2;
                                    txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode;

                                    if (((clsPatientVO)this.DataContext).ResiNoCountryCode != null && ((clsPatientVO)this.DataContext).ResiNoCountryCode != 0)
                                        txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                    else
                                        txtResiCountryCode.Text = "";
                                    if (((clsPatientVO)this.DataContext).ResiSTDCode != null && ((clsPatientVO)this.DataContext).ResiSTDCode != 0)
                                        txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();
                                    else
                                        txtResiSTD.Text = "";

                                    //if (((clsPatientVO)this.DataContext).Photo != null)
                                    //{
                                    //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                    //    //bmp.FromByteArray(((clsPatientVO)this.DataContext).Photo);
                                    //    //imgPhoto.Source = bmp;

                                    //    //byte[] imageBytes = ((clsPatientVO)this.DataContext).Photo;
                                    //    //BitmapImage img = new BitmapImage();
                                    //    //img.SetSource(new MemoryStream(imageBytes, false));
                                    //    //imgPhoto.Source = img;

                                    //    //added by neena
                                    //    if (((clsPatientVO)this.DataContext).ImageName.Length > 0)
                                    //    {
                                    //        chkImgNameFlag = true;
                                    //        imgPhoto.Source = new BitmapImage(new Uri(((clsPatientVO)this.DataContext).ImageName, UriKind.Absolute));
                                    //    }
                                    //    //
                                    //}

                                    if (((clsPatientVO)this.DataContext).ImageName != null && ((clsPatientVO)this.DataContext).ImageName.Length > 0)
                                    {
                                        chkImgNameFlag = true;
                                        imgPhoto.Source = new BitmapImage(new Uri(((clsPatientVO)this.DataContext).ImageName, UriKind.Absolute));
                                    }
                                    else if (((clsPatientVO)this.DataContext).Photo != null)
                                    {
                                        byte[] imageBytes = ((clsPatientVO)this.DataContext).Photo;
                                        BitmapImage img = new BitmapImage();
                                        img.SetSource(new MemoryStream(imageBytes, false));
                                        imgPhoto.Source = img;
                                    }

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

                                    if (((clsPatientVO)this.DataContext).IsClinicVisited == true)
                                        chkIfYesrdo.IsChecked = true;
                                    else
                                        chkIfNordo.IsChecked = false;
                                    if (((clsPatientVO)this.DataContext).IsReferralDoc == true)
                                    {
                                        ReferralDoc.IsChecked = true;
                                        FillRefDoctor();
                                        cmdSearch.IsEnabled = true;
                                    }
                                    else
                                    {
                                        InhouseDoc.IsChecked = true;
                                        fillInhouseDoctor();
                                        cmdSearch.IsEnabled = false;
                                    }
                                    #region For Pediatric Flow

                                    if (this.DataContext != null && ((clsPatientVO)this.DataContext).GeneralDetails != null)  //if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 13)
                                    {
                                        txtBabyNo.Text = Convert.ToString(((clsPatientVO)this.DataContext).BabyNo);
                                        txtBabyNoOfNo.Text = Convert.ToString(((clsPatientVO)this.DataContext).BabyOfNo);

                                        if (((clsPatientVO)this.DataContext).BabyWeight != null)
                                        {
                                            txtBirthWeight.Text = ((clsPatientVO)this.DataContext).BabyWeight;
                                        }

                                        if (((clsPatientVO)this.DataContext).GeneralDetails.IsAge == false)
                                            txtBirthTime.Value = ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth;
                                        else if (((clsPatientVO)this.DataContext).GeneralDetails.IsAge == true)
                                            txtBirthTime.Value = ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirthFromAge;

                                        LinkPatientID = ((clsPatientVO)this.DataContext).LinkPatientID;
                                        LinkPatientUnitID = ((clsPatientVO)this.DataContext).LinkPatientUnitID;

                                        txtMRNumberParent.Text = ((clsPatientVO)this.DataContext).LinkPatientMrNo;
                                        txtParentName.Text = ((clsPatientVO)this.DataContext).LinkParentName;
                                    }

                                    #endregion

                                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                                    mElement.Text = " : " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName +
                                        " " + ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " +
                                        ((clsPatientVO)this.DataContext).GeneralDetails.LastName;

                                    PatientEditMode = true;
                                    IsPatientExist = true;

                                }

                                //added by neena
                                //commented by neena
                                //if (this.DataContext != null)
                                //    FillPatientAddress();
                                //
                                //
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
                    sponsorContent.IsEnabled = false;
                    visitContent.IsEnabled = false;
                    grpHeader2.IsEnabled = false;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = false;
                    //by Anjali..
                    CmdOPD.IsEnabled = false;
                    //................
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
                        City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City,
                        Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area,
                        CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID,
                        StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID,
                        CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID,
                        RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID,
                        Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                    };

                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    chkIfNordo.IsChecked = true;
                    ReferralDoc.IsChecked = true;
                    FillRefDoctor();

                    // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                    //};
                    //FillTariffMaster();                    
                    FillPatientSponsorDetails();


                    if (mAppointmentID > 0)
                    {
                        // BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                        ShowAppointmentDetails(mAppointmentID, ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);

                    }

                    #endregion
                    break;

                case "Surrogacy":
                    Flagref = true;
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    IsPatientExist = true;
                    UserControl rootPage31 = Application.Current.RootVisual as UserControl;
                    TextBlock mElement31 = (TextBlock)rootPage31.FindName("SampleSubHeader");
                    mElement31.Text = "";

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

                        //added by neena
                        CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID,
                        StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID,
                        CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID,
                        RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID,
                        Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode,
                        Region = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Region,
                        RegionCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionCode,
                        CityN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityN,
                        CityCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityCode,
                        StateN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateN,
                        StateCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateCode,
                        CountryN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryN,
                        CountryCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryCode,
                        PrefixId = 0,
                        IdentityID = 0,
                        IsInternationalPatient = false,
                        BDID = 0,
                        NationalityID = 0,
                        PreferredLangID = 0,
                        TreatRequiredID = 0,
                        EducationID = 0,
                        SpecialRegID = 0,
                        //

                        // AddedOn = ((IApplicationConfiguration)App.Current).UserMachineName
                    };
                    FillRefDoctor();
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Country = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Country;
                    ((clsPatientVO)this.DataContext).SpouseDetails.State = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.State;
                    ((clsPatientVO)this.DataContext).SpouseDetails.District = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.District;
                    ((clsPatientVO)this.DataContext).SpouseDetails.City = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.City;

                    ((clsPatientVO)this.DataContext).SpouseDetails.Area = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Area;
                    ((clsPatientVO)this.DataContext).SpouseDetails.GenderID = (long)Genders.Female;

                    //added by neena
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = 10;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.RegionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionID;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Pincode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Region = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.Region;
                    ((clsPatientVO)this.DataContext).SpouseDetails.RegionCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.RegionCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CityCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.StateCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryN = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryN;
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryCode = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CountryCode;
                    ((clsPatientVO)this.DataContext).SpouseDetails.NationalityID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.PreferredLangID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.TreatRequiredID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.EducationID = 0;
                    ((clsPatientVO)this.DataContext).SpouseDetails.SpecialRegID = 0;

                    ((clsPatientVO)this.DataContext).BDID = 0;
                    //

                    break;
            }

        }

        #endregion
        int ClickedFlag = 0;
        bool IsPageLoded = false;
        public long mAppointmentID = 0;
        public long mAppointmentUnitID = 0;
        bool PatientSponsorNewMode = true;

        bool IsSetWetIndicator = false;    //Flag use to show wait indicator on Country, State, City, Region Fill Combo Methods
        //WaitIndicator IndicatiorFill = new WaitIndicator();    //Variable use to show wait indicator on Country, State, City, Region Fill Combo Methods

        #region  Flag use to show wait indicator on Country, State, City, Region Fill Combo Methods while calling from PatientRegistration_Loaded only 11012017

        bool IndicatiorFromLoad = false;
        WaitIndicator IndicatiorFillFromLoad = new WaitIndicator();

        #endregion

        #region For Pediatric Flow

        long LinkPatientID = 0;
        long LinkPatientUnitID = 0;

        #endregion

        public PatientRegistration()
        {
            InitializeComponent();
            txtYY.IsReadOnly = true;
            txtMM.IsReadOnly = true;
            txtDD.IsReadOnly = true;

            txtSpouseYY.IsReadOnly = true;
            txtSpouseMM.IsReadOnly = true;
            txtSpouseDD.IsReadOnly = true;
            FamilyTypeList();
        }

        private enum SaveType
        {
            Registration = 0,
            IPD = 1,
            OPD = 2
        }

        private List<MasterListItem> _FamilyType = new List<MasterListItem>();
        public List<MasterListItem> FamilyType
        {
            get
            {
                return _FamilyType;
            }
            set
            {
                _FamilyType = value;
            }
        }


        private void FamilyTypeList()
        {
            FamilyType = new List<MasterListItem>();
            FamilyType.Add(new MasterListItem(0, "--Select--"));
            FamilyType.Add(new MasterListItem(1, "Joint"));
            FamilyType.Add(new MasterListItem(2, "Nuclear"));


            cmbFamilyType.ItemsSource = FamilyType;
            cmbFamilyType.SelectedItem = FamilyType.ToList()[0];

        }

        private SaveType formSaveType = SaveType.Registration;

        void PatientRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            //{
                //CaptureDeviceConfiguration.AllowedDeviceAccess 

                //txtSpouseArea1.Text = "abcd"; //commented by neena
                // CmdOPD.IsEnabled = true;

                //added by neena
                if (ckSavePhoto == true)
                    CmdSavePhotoToServer.Visibility = Visibility.Collapsed;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsPhotoMoveToServer == true)
                    CmdSavePhotoToServer.Visibility = Visibility.Collapsed;

                //

                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    //added by neena for surrogacy
                    if (Flagref == true)
                        ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
                    else
                        ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    // ((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                }

                if (!IsPageLoded)
                {
                    WaitIndicator Indicatior = new WaitIndicator();
                    //Indicatior.Show();

                    _captureSource = new CaptureSource();
                    _SpousecaptureSource = new CaptureSource();

                    VideoSources.ItemsSource = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();
                    SpouseVideoSources.ItemsSource = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();



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
                    txtCountry.ItemsSource = objList.DeepCopy();
                    txtCountry.SelectedItem = objM.DeepCopy();
                    txtState.ItemsSource = objList.DeepCopy();
                    txtState.SelectedItem = objM.DeepCopy();
                    txtCity.ItemsSource = objList.DeepCopy();
                    txtCity.SelectedItem = objM.DeepCopy();
                    txtArea.ItemsSource = objList.DeepCopy();
                    txtArea.SelectedItem = objM.DeepCopy();
                    txtSpouseCountry.ItemsSource = objList.DeepCopy();
                    txtSpouseCountry.SelectedItem = objM.DeepCopy();
                    txtSpouseState.ItemsSource = objList.DeepCopy();
                    txtSpouseState.SelectedItem = objM.DeepCopy();
                    txtSpouseCity.ItemsSource = objList.DeepCopy();
                    txtSpouseCity.SelectedItem = objM.DeepCopy();
                    txtSpouseArea.ItemsSource = objList.DeepCopy();
                    txtSpouseArea.SelectedItem = objM.DeepCopy();

                    //* Added by - Ajit Jadhav
                    //* Added Date - 5/8/2016
                    //* Comments - Create New Master Table BDMaster And Get Data
                    //FillBdMaster();
                    //***//-----------
                    GetFamilyList();
                    //FillCountryList();
                    //if (this.DataContext != null)
                    //{
                    //    FillStateList(((clsPatientVO)this.DataContext).Country);
                    //    FillCityList(((clsPatientVO)this.DataContext).State);
                    //    GetAreaList(((clsPatientVO)this.DataContext).City);
                    //    GetPinCodeList(((clsPatientVO)this.DataContext).City);
                    //}

                    //FillPreffix();  //commented by neena

                    FillPatientType();//added by neena

                    //....................................

                    //FillPreffix();
                    FillIdentity();
                    FillReferralName();
                    FillMaritalStatus();
                    FillReligion();
                    FillOccupation();
                    FillBloodGroup();
                    FillGender();
                    FillEducationMaster();
                    FillPrefLanguageMaster();
                    FillTreatRequiredMasterMaster();
                    FillNationalityMaster();
                    FillSpecialRegistrationMaster();

                    //....................................


                    // FillIdentity();
                    // FillReferralName();
                    // FillPatientType();
                    // FillMaritalStatus();
                    // FillReligion();
                    // FillOccupation();
                    // FillBloodGroup();
                    // FillGender();

                    //FillEducationMaster();
                    //FillPrefLanguageMaster();
                    //FillTreatRequiredMasterMaster();
                    // FillNationalityMaster();
                    //FillSpecialRegistrationMaster();

                    // FillTariffMaster();
                    FillPatientSponsorDetails();
                    txtFirstName.Focus();
                    //....................................

                    IndicatiorFromLoad = true;   //  Flag use to show wait indicator on Country, State, City, Region Fill Combo Methods while calling from PatientRegistration_Loaded only 11012017
                    IndicatiorFillFromLoad.Show();

                    IsSetWetIndicator = true;
                    FillCountry();       // commented by neena
                    //.........................................
                    FillCamp(); //***//
                    FillAgentMaster();
                    FillSurrogateAgency();

                    //_SpousecaptureSource.CaptureImageCompleted += ((s, args) =>
                    //{
                    //    _Spouseimages = (args.Result);
                    //    imgSpousePhoto.Source = _Spouseimages;
                    //});

                    //_captureSource.CaptureImageCompleted += ((s, args) =>
                    //{
                    //    _images = (args.Result);
                    //    imgPhoto.Source = _images;
                    //});

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
                    //CheckSpouseValidations();
                    Indicatior.Close();
                }
                //txtFirstName.Focus();
                //txtFirstName.UpdateLayout();
                cmbPatientType.Focus();
                cmbPatientType.UpdateLayout();
                IsPageLoded = true;


                //added by neena
                //commented by neena
                //if (this.DataContext != null)
                //    FillPatientAddress();
                //
                //
                if (PatientEditMode == false)
                    CheckValidations();

            }
        //} //Added by NileshD on 19April2019



        /// <summary>
        /// Checks & assigns validations for the controls.
        /// </summary>
        /// <returns></returns>
        /// 
        private bool CheckSpouseValidations()
        {
            bool result = true;

            if (txtSpouseEmail.Text.Trim().Length > 0)
            {
                if (txtSpouseEmail.Text.IsEmailValid())
                    txtSpouseEmail.ClearValidationError();
                else
                {
                    txtSpouseEmail.SetValidation("Please Enter Valid Email");
                    txtSpouseEmail.RaiseValidationError();
                    txtSpouseEmail.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
            }

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


                //if (txtState1.Text.Trim() == string.Empty)  //added by neena //commented by neena
                //{
                if ((MasterListItem)txtSpouseCountry.SelectedItem == null)
                {
                    txtSpouseCountry.TextBox.SetValidation("Country Is Required");
                    txtSpouseCountry.TextBox.RaiseValidationError();
                    txtSpouseCountry.Focus();
                    //  PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)txtSpouseCountry.SelectedItem).ID == 0)
                {
                    txtSpouseCountry.TextBox.SetValidation("Country Is Required");
                    txtSpouseCountry.TextBox.RaiseValidationError();
                    txtSpouseCountry.Focus();
                    //  PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseCountry.TextBox.ClearValidationError();
                //}
                //else
                //    txtSpouseCountry1.ClearValidationError();

                //if (txtSpouseState1.Text.Trim() == string.Empty)  //added by neena  //commented by neena
                //{
                if ((MasterListItem)txtSpouseState.SelectedItem == null)
                {
                    txtSpouseState.TextBox.SetValidation("State Is Required");
                    txtSpouseState.TextBox.RaiseValidationError();
                    txtSpouseState.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)txtSpouseState.SelectedItem).ID == 0)
                {
                    txtSpouseState.TextBox.SetValidation("State Is Required");
                    txtSpouseState.TextBox.RaiseValidationError();
                    txtSpouseState.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseState.TextBox.ClearValidationError();
                //}
                //else
                //    txtSpouseState1.ClearValidationError();

                //if (txtSpouseCity1.Text.Trim() == string.Empty)  //added by neena //commented by neena
                //{
                if ((MasterListItem)txtSpouseCity.SelectedItem == null)
                {
                    txtSpouseCity.TextBox.SetValidation("City Is Required");
                    txtSpouseCity.TextBox.RaiseValidationError();
                    txtSpouseCity.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else if (((MasterListItem)txtSpouseCity.SelectedItem).ID == 0)
                {
                    txtSpouseCity.TextBox.SetValidation("City Is Required");
                    txtSpouseCity.TextBox.RaiseValidationError();
                    txtSpouseCity.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseCity.TextBox.ClearValidationError();
                //}
                //else
                //    txtSpouseCity1.ClearValidationError();

                //if (txtSpouseArea1.Text.Trim() == string.Empty)  //added by neena  //commented by neena
                //{
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == false)  //Added by NileshD on 19April2019
                {
                    if ((MasterListItem)txtSpouseArea.SelectedItem == null)
                    {
                        txtSpouseArea.TextBox.SetValidation("Area Is Required");
                        txtSpouseArea.TextBox.RaiseValidationError();
                        txtSpouseArea.Focus();
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else if (((MasterListItem)txtSpouseArea.SelectedItem).ID == 0)
                    {
                        txtSpouseArea.TextBox.SetValidation("Area Is Required");
                        txtSpouseArea.TextBox.RaiseValidationError();
                        txtSpouseArea.Focus();
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        txtSpouseArea.TextBox.ClearValidationError();
                } //Added by NileshD on 19April2019
                //}
                //else
                //    txtSpouseArea1.ClearValidationError();

                if (txtSpousePinCode.Text == null)
                {
                    txtSpousePinCode.SetValidation("PIN Code Is Required");
                    txtSpousePinCode.RaiseValidationError();
                    txtSpousePinCode.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpousePinCode.ClearValidationError();

                ////added by neena
                //if (txtSpouseArea1.Text == string.Empty)
                //{
                //    txtSpouseArea1.SetValidation("Area Is Required");
                //    txtSpouseArea1.RaiseValidationError();
                //    txtSpouseArea1.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtSpouseArea1.ClearValidationError();

                //if (txtSpouseCity1.Text == string.Empty)
                //{
                //    txtSpouseCity1.SetValidation("City Is Required");
                //    txtSpouseCity1.RaiseValidationError();
                //    txtSpouseCity1.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtSpouseCity1.ClearValidationError();

                //if (txtSpouseState1.Text == string.Empty)
                //{
                //    txtSpouseState1.SetValidation("State Is Required");
                //    txtSpouseState1.RaiseValidationError();
                //    txtSpouseState1.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtSpouseState1.ClearValidationError();

                //if (txtSpouseCountry1.Text == string.Empty)
                //{
                //    txtSpouseCountry1.SetValidation("Country Is Required");
                //    txtSpouseCountry1.RaiseValidationError();
                //    txtSpouseCountry1.Focus();
                //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtSpouseCountry1.ClearValidationError();
                ////


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

                //if ((MasterListItem)cmbPatientSource.SelectedItem == null)                  
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //{
                //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                //    cmbPatientSource.TextBox.RaiseValidationError();
                //    cmbPatientSource.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else
                //    cmbPatientSource.TextBox.ClearValidationError();

                //if (((clsPatientVO)this.DataContext).SpouseDetails.FamilyName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName.Trim() == "")
                //{
                //    txtSpouseFamilyName.SetValidation("Family Name Is Required");
                //    txtSpouseFamilyName.RaiseValidationError();
                //    txtSpouseFamilyName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else
                //    txtSpouseFamilyName.ClearValidationError();


                //if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == "")
                //{
                //    txtSpouseLastName.SetValidation("Last Name Is Required");
                //    txtSpouseLastName.RaiseValidationError();
                //    txtSpouseLastName.Focus();
                //    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else
                //    txtSpouseLastName.ClearValidationError();

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


                //if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
                //{
                //    dtpSpouseDOB.SetValidation("Birth Date Is Required");
                //    dtpSpouseDOB.RaiseValidationError();
                //    dtpSpouseDOB.Focus();
                //    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                //{
                //    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                //    dtpSpouseDOB.RaiseValidationError();
                //    dtpSpouseDOB.Focus();
                //    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                //{
                //    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                //    dtpSpouseDOB.RaiseValidationError();
                //    dtpSpouseDOB.Focus();
                //    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                //    result = false;
                //}
                //else
                //    dtpSpouseDOB.ClearValidationError();


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



                    else if (((MasterListItem)cmbPatientType.SelectedItem).ID != 13 && ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)    // 1. For Pediatric Flow On 0902017 
                    {
                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                        dtpSpouseDOB.RaiseValidationError();
                        dtpSpouseDOB.Focus();
                        // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        result = false;
                    }
                    else
                        dtpSpouseDOB.ClearValidationError();







                    //else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                    //{
                    //    dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                    //    dtpSpouseDOB.RaiseValidationError();
                    //    dtpSpouseDOB.Focus();
                    //    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    //    result = false;
                    //}
                    //else
                    //    dtpSpouseDOB.ClearValidationError();
                }

                if (txtSpouseYY.Text != "")
                {
                    //if (Convert.ToInt16(txtSpouseYY.Text) < 18)
                    //{
                    //    txtSpouseYY.SetValidation("Age Can Not Be Less Than 18");
                    //    txtSpouseYY.RaiseValidationError();
                    //    txtSpouseYY.Focus();
                    //    result = false;
                    //    //  PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    //    ClickedFlag = 0;
                    //}
                    //else
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
                if (ChkSpouseInternationalPatient.IsChecked == true && cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)(cmbSpouseIdentity.SelectedItem)).ID != (long)(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient))
                {
                    cmbSpouseIdentity.TextBox.SetValidation("For International Patient Passport Details Are Mendatory");
                    cmbSpouseIdentity.TextBox.RaiseValidationError();
                    cmbSpouseIdentity.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    cmbSpouseIdentity.TextBox.ClearValidationError();

                if ((MasterListItem)cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID > 0 && txtSpouseIdentityNumber.Text == string.Empty)
                {
                    txtSpouseIdentityNumber.SetValidation("Identity Number Is Required");
                    txtSpouseIdentityNumber.RaiseValidationError();
                    txtSpouseIdentityNumber.Focus();
                    // PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                    result = false;
                }
                else
                    txtSpouseIdentityNumber.ClearValidationError();
            }



            return result;
        }
        private bool CheckValidations()
        {
            bool result = true;
            try
            {


                if (((clsPatientVO)this.DataContext).GeneralDetails.MarriageAnnDate != null && ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth != null)
                {
                    if (dtpDOB.SelectedDate < dtpMarriageAnnDate.SelectedDate)
                    {
                        //System.DateTime firstDate = (DateTime)dtpDOB.SelectedDate;
                        //System.DateTime secondDate = (DateTime)dtpMarriageAnnDate.SelectedDate;

                        //System.TimeSpan diff = secondDate.Subtract(firstDate);
                        //System.TimeSpan diff1 = secondDate - firstDate;

                        //int totYrs = secondDate.AddYears(-(firstDate.Year)).Year;


                        DateTime zeroTime = new DateTime(1, 1, 1);
                        TimeSpan span = dtpMarriageAnnDate.SelectedDate.Value.Subtract(dtpDOB.SelectedDate.Value);
                        // because we start at year 1 for the Gregorian 
                        // calendar, we must subtract a year here.
                        int years = (zeroTime + span).Year - 1;

                        // 1, where my other algorithm resulted in 0.
                        // Console.WriteLine("Yrs elapsed: " + years);


                        if (years < 18 && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Female)
                        {
                            dtpMarriageAnnDate.SetValidation("Minimun 18 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                            dtpMarriageAnnDate.RaiseValidationError();
                            dtpMarriageAnnDate.Focus();
                            result = false;
                        }
                        else if (years < 21 && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Male)
                        {
                            dtpMarriageAnnDate.SetValidation("Minimun 21 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                            dtpMarriageAnnDate.RaiseValidationError();
                            dtpMarriageAnnDate.Focus();
                            result = false;
                        }
                        else
                            dtpMarriageAnnDate.ClearValidationError();
                    }
                    else
                    {
                        dtpMarriageAnnDate.SetValidation("Marriage Anniversary Date Should be Greater than Date of Birth");
                        dtpMarriageAnnDate.RaiseValidationError();
                        dtpMarriageAnnDate.Focus();
                        result = false;
                    }
                }
                else if (((clsPatientVO)this.DataContext).GeneralDetails.MarriageAnnDate != null && ((clsPatientVO)this.DataContext).GeneralDetails.DateOfBirth == null && DOB != null)
                {
                    if (DOB < dtpMarriageAnnDate.SelectedDate)
                    {
                        //System.DateTime firstDate = (DateTime)dtpDOB.SelectedDate;
                        //System.DateTime secondDate = (DateTime)dtpMarriageAnnDate.SelectedDate;

                        //System.TimeSpan diff = secondDate.Subtract(firstDate);
                        //System.TimeSpan diff1 = secondDate - firstDate;

                        //int totYrs = secondDate.AddYears(-(firstDate.Year)).Year;


                        DateTime zeroTime = new DateTime(1, 1, 1);
                        TimeSpan span = dtpMarriageAnnDate.SelectedDate.Value.Subtract(DOB.Value);
                        // because we start at year 1 for the Gregorian 
                        // calendar, we must subtract a year here.
                        int years = (zeroTime + span).Year - 1;

                        // 1, where my other algorithm resulted in 0.
                        // Console.WriteLine("Yrs elapsed: " + years);


                        if (years < 18 && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Female)
                        {
                            dtpMarriageAnnDate.SetValidation("Minimun 18 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                            dtpMarriageAnnDate.RaiseValidationError();
                            dtpMarriageAnnDate.Focus();
                            result = false;
                        }
                        else if (years < 21 && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Male)
                        {
                            dtpMarriageAnnDate.SetValidation("Minimun 21 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                            dtpMarriageAnnDate.RaiseValidationError();
                            dtpMarriageAnnDate.Focus();
                            result = false;
                        }
                        else
                            dtpMarriageAnnDate.ClearValidationError();
                    }
                    else
                    {
                        dtpMarriageAnnDate.SetValidation("Marriage Anniversary Date Should be Greater than Date of Birth");
                        dtpMarriageAnnDate.RaiseValidationError();
                        dtpMarriageAnnDate.Focus();
                        result = false;
                    }
                }

                if (dtpMarriageAnnDate.SelectedDate > DateTime.Today)
                {
                    dtpMarriageAnnDate.SetValidation("Date Of  Marriage Anniversary should not be Future Date");
                    dtpMarriageAnnDate.RaiseValidationError();
                    dtpMarriageAnnDate.Focus();
                    result = false;
                }



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


                //if (dtpDOB.SelectedDate == null)
                //{
                //    dtpDOB.SetValidation("Birth Date Is Required");
                //    dtpDOB.RaiseValidationError();
                //    dtpDOB.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (dtpDOB.SelectedDate.Value.Date > DateTime.Now.Date)
                //{
                //    dtpDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                //    dtpDOB.RaiseValidationError();
                //    dtpDOB.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (dtpDOB.SelectedDate.Value.Date == DateTime.Now.Date)
                //{
                //    dtpDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                //    dtpDOB.RaiseValidationError();
                //    dtpDOB.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    dtpDOB.ClearValidationError();


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
                    else if (((MasterListItem)cmbPatientType.SelectedItem).ID != 13 && dtpDOB.SelectedDate.Value.Date == DateTime.Now.Date)      // 2. For Pediatric Flow On 090217
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
                //* Added by - Ajit Jadhav
                //* Added Date - 5/80/2016
                //* Comments - Mandatory fields Date Of Birth
                //---------------------------
                else //***//
                {
                    dtpDOB.SetValidation("Date Of Birth Is Required");
                    dtpDOB.RaiseValidationError();
                    dtpDOB.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;

                }
                //***//---------------------------------
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
                    if (((MasterListItem)cmbReferralName.SelectedItem).ID == 1 || ((MasterListItem)cmbReferralName.SelectedItem).ID == 2)
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

                //***//-------------------
                if ((MasterListItem)cmbReferralName.SelectedItem != null)
                {
                    if (((MasterListItem)cmbCampDetail.SelectedItem).ID == 0)
                    {
                        if (((MasterListItem)cmbReferralName.SelectedItem).ID == 9)
                        {
                            cmbCampDetail.TextBox.SetValidation("CAMP Is Required");
                            cmbCampDetail.TextBox.RaiseValidationError();
                            cmbCampDetail.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            result = false;
                        }
                    }
                }
                else
                    cmbReferralDoctor.TextBox.ClearValidationError();

                //------------------------



                //commented by neena
                //if (txtDaughterOf.Text == string.Empty)
                //{
                //    txtDaughterOf.SetValidation(lblDaughterOf.Text + " Is Required");
                //    txtDaughterOf.RaiseValidationError();
                //    txtDaughterOf.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtDaughterOf.ClearValidationError();
                //

                //Commented by ajit jadhav Date 23/9/2016 for User Requirement

                //if ((MasterListItem)cmbEducation.SelectedItem == null)
                //{
                //    cmbEducation.TextBox.SetValidation("Education Is Required");
                //    cmbEducation.TextBox.RaiseValidationError();
                //    cmbEducation.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbEducation.SelectedItem).ID == 0)
                //{
                //    cmbEducation.TextBox.SetValidation("Education Is Required");
                //    cmbEducation.TextBox.RaiseValidationError();
                //    cmbEducation.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbEducation.TextBox.ClearValidationError();

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
                //else if (((MasterListItem)cmbMaritalStatus.SelectedItem).ID == (long)((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaritalStatus)
                //{
                //    if (dtpMarriageAnnDate.SelectedDate == null)
                //    {
                //        dtpMarriageAnnDate.SetValidation("Marriage Anniversary Date Is Required");
                //        dtpMarriageAnnDate.RaiseValidationError();
                //        dtpMarriageAnnDate.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else
                //        dtpMarriageAnnDate.ClearValidationError();
                //}
                else
                    cmbMaritalStatus.TextBox.ClearValidationError();

                //***//Commented by ajit jadhav Date 23/9/2016 for User Requirement
                //if ((MasterListItem)cmbOccupation.SelectedItem == null)
                //{
                //    cmbOccupation.TextBox.SetValidation("Occupation Is Required");
                //    cmbOccupation.TextBox.RaiseValidationError();
                //    cmbOccupation.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbOccupation.SelectedItem).ID == 0)
                //{
                //    cmbOccupation.TextBox.SetValidation("Occupation Is Required");
                //    cmbOccupation.TextBox.RaiseValidationError();
                //    cmbOccupation.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbOccupation.TextBox.ClearValidationError();

                //if ((MasterListItem)cmbSpecialReg.SelectedItem == null)
                //{
                //    cmbSpecialReg.TextBox.SetValidation("Special Registration Is Required");
                //    cmbSpecialReg.TextBox.RaiseValidationError();
                //    cmbSpecialReg.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbSpecialReg.SelectedItem).ID == 0)
                //{
                //    cmbSpecialReg.TextBox.SetValidation("Special Registration Is Required");
                //    cmbSpecialReg.TextBox.RaiseValidationError();
                //    cmbSpecialReg.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbSpecialReg.TextBox.ClearValidationError();

                //if ((MasterListItem)cmbReligion.SelectedItem == null)
                //{
                //    cmbReligion.TextBox.SetValidation("Religion Is Required");
                //    cmbReligion.TextBox.RaiseValidationError();
                //    cmbReligion.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbReligion.SelectedItem).ID == 0)
                //{
                //    cmbReligion.TextBox.SetValidation("Religion Is Required");
                //    cmbReligion.TextBox.RaiseValidationError();
                //    cmbReligion.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbReligion.TextBox.ClearValidationError();

                //if ((MasterListItem)cmbPreferredLanguage.SelectedItem == null)
                //{
                //    cmbPreferredLanguage.TextBox.SetValidation("Preferred Language Is Required");
                //    cmbPreferredLanguage.TextBox.RaiseValidationError();
                //    cmbPreferredLanguage.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPreferredLanguage.SelectedItem).ID == 0)
                //{
                //    cmbPreferredLanguage.TextBox.SetValidation("Preferred Language Is Required");
                //    cmbPreferredLanguage.TextBox.RaiseValidationError();
                //    cmbPreferredLanguage.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbPreferredLanguage.TextBox.ClearValidationError();

                if ((MasterListItem)cmbTreatmentRequired.SelectedItem == null)
                {
                    cmbTreatmentRequired.TextBox.SetValidation("Reason Of Visit Is Required");
                    cmbTreatmentRequired.TextBox.RaiseValidationError();
                    cmbTreatmentRequired.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)cmbTreatmentRequired.SelectedItem).ID == 0)
                {
                    cmbTreatmentRequired.TextBox.SetValidation("Reason Of Visit Is Required");
                    cmbTreatmentRequired.TextBox.RaiseValidationError();
                    cmbTreatmentRequired.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbTreatmentRequired.TextBox.ClearValidationError();


                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == true)  //Added by NileshD on 19April2019
                {
                    if (((MasterListItem)cmbSpecialReg.SelectedItem).ID == (((IApplicationConfiguration)App.Current).ApplicationConfigurations).InternationalId) //Added by NileshD on 19April2019
                    {
                        if ((MasterListItem)cmbNationality.SelectedItem == null)
                        {
                            cmbNationality.TextBox.SetValidation("Nationality Is Required");
                            cmbNationality.TextBox.RaiseValidationError();
                            cmbNationality.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            result = false;
                        }
                        else if (((MasterListItem)cmbNationality.SelectedItem).ID == 0)
                        {
                            cmbNationality.TextBox.SetValidation("Nationality Is Required");
                            cmbNationality.TextBox.RaiseValidationError();
                            cmbNationality.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            result = false;
                        }
                        else
                            cmbNationality.TextBox.ClearValidationError();
                    }
                    else
                        cmbNationality.TextBox.ClearValidationError();
                }

                //* Added by - Ajit Jadhav
                //* Added Date - 5/80/2016
                //* Comments - New Master Combo Box Added cmbBD to check  Mandatory fields
                //---------------------------
                //if ((MasterListItem)cmbBD.SelectedItem == null)
                //{
                //    cmbBD.TextBox.SetValidation("BDName Is Required");
                //    cmbBD.TextBox.RaiseValidationError();
                //    cmbBD.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbBD.SelectedItem).ID == 0)
                //{
                //    cmbBD.TextBox.SetValidation("BDName Is Required");
                //    cmbBD.TextBox.RaiseValidationError();
                //    cmbBD.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbBD.TextBox.ClearValidationError();

                if (txtPanNo.Text != string.Empty && txtPanNo.Text.Trim().Length > 0)
                {
                    string pattern = @"([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$"; //To Check PAN NO Regular Expression Date 21/9/2016
                    Match m = Regex.Match(txtPanNo.Text.Trim(), pattern);
                    if (!m.Success)
                    {
                        txtPanNo.SetValidation("Invalid Pan Number");
                        txtPanNo.RaiseValidationError();
                        txtPanNo.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtPanNo.ClearValidationError();
                }
                //***-----------------

                if (chkIfYesrdo.IsChecked == true)
                {
                    if (txtClinicName.Text == string.Empty)
                    {
                        txtClinicName.SetValidation("Clinic Name Is Required");
                        txtClinicName.RaiseValidationError();
                        txtClinicName.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtClinicName.ClearValidationError();
                }
                else
                    txtClinicName.ClearValidationError();

                if (txtNoOfPeople.Text == string.Empty)
                {
                    txtNoOfPeople.SetValidation("Number Of People At Your Residence Is Required");
                    txtNoOfPeople.RaiseValidationError();
                    txtNoOfPeople.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtNoOfPeople.ClearValidationError();

                if (txtAddress1.Text == string.Empty)
                {
                    txtAddress1.SetValidation("Address Is Required");
                    txtAddress1.RaiseValidationError();
                    txtAddress1.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtAddress1.ClearValidationError();

                if ((MasterListItem)txtCountry.SelectedItem == null)
                {
                    txtCountry.TextBox.SetValidation("Country Is Required");
                    txtCountry.TextBox.RaiseValidationError();
                    txtCountry.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)txtCountry.SelectedItem).ID == 0)
                {
                    txtCountry.TextBox.SetValidation("Country Is Required");
                    txtCountry.TextBox.RaiseValidationError();
                    txtCountry.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtCountry.TextBox.ClearValidationError();

                if ((MasterListItem)txtState.SelectedItem == null)
                {
                    txtState.TextBox.SetValidation("State Is Required");
                    txtState.TextBox.RaiseValidationError();
                    txtState.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)txtState.SelectedItem).ID == 0)
                {
                    txtState.TextBox.SetValidation("State Is Required");
                    txtState.TextBox.RaiseValidationError();
                    txtState.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtState.TextBox.ClearValidationError();

                if ((MasterListItem)txtCity.SelectedItem == null)
                {
                    txtCity.TextBox.SetValidation("City Is Required");
                    txtCity.TextBox.RaiseValidationError();
                    txtCity.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (((MasterListItem)txtCity.SelectedItem).ID == 0)
                {
                    txtCity.TextBox.SetValidation("City Is Required");
                    txtCity.TextBox.RaiseValidationError();
                    txtCity.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtCity.TextBox.ClearValidationError();

                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == false)  //Added by NileshD on 19April2019
                {
                    if ((MasterListItem)txtArea.SelectedItem == null)
                    {
                        txtArea.TextBox.SetValidation("Area Is Required");
                        txtArea.TextBox.RaiseValidationError();
                        txtArea.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (((MasterListItem)txtArea.SelectedItem).ID == 0)
                    {
                        txtArea.TextBox.SetValidation("Area Is Required");
                        txtArea.TextBox.RaiseValidationError();
                        txtArea.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtArea.TextBox.ClearValidationError();
                } //Added by NileshD on 19April2019
            


                if (txtPinCode.Text == null)
                {
                    txtPinCode.SetValidation("PIN Code Is Required");
                    txtPinCode.RaiseValidationError();
                    txtPinCode.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtPinCode.ClearValidationError();



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
                else
                    txtContactNo1.Textbox.ClearValidationError();

                if (txtContactNo1.Text.Trim() != "")
                {
                    if (txtContactNo1.Text.Trim().Length > 14)
                    {
                        txtContactNo1.Textbox.SetValidation("Mobile Number Should Be 14 Digit");
                        txtContactNo1.Textbox.RaiseValidationError();
                        txtContactNo1.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
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


                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == false)  //Added by NileshD on 19April2019
                {
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
                } //Added by NileshD on 19April2019




                //if (txtReferralDetail.Text == string.Empty)
                //{
                //    if (((MasterListItem)cmbReferralName.SelectedItem) == null)
                //    {
                //        txtReferralDetail.SetValidation("Referral Detail Is Required");
                //        txtReferralDetail.RaiseValidationError();
                //        txtReferralDetail.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //    else if (((MasterListItem)cmbReferralName.SelectedItem).ID != 1 && ((MasterListItem)cmbReferralName.SelectedItem).ID != 2)  //added by neena
                //    {
                //        txtReferralDetail.SetValidation("Referral Detail Is Required");
                //        txtReferralDetail.RaiseValidationError();
                //        txtReferralDetail.Focus();
                //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //        result = false;
                //    }
                //}
                //else
                //    txtReferralDetail.ClearValidationError();





                //if ((MasterListItem)cmbReferralDoctor.SelectedItem == null)
                //{
                //    cmbReferralDoctor.TextBox.SetValidation("Referral Doctor Is Required");
                //    cmbReferralDoctor.TextBox.RaiseValidationError();
                //    cmbReferralDoctor.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbReferralDoctor.SelectedItem).ID == 0)
                //{
                //    cmbReferralDoctor.TextBox.SetValidation("Referral Doctor Is Required");
                //    cmbReferralDoctor.TextBox.RaiseValidationError();
                //    cmbReferralDoctor.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbReferralDoctor.TextBox.ClearValidationError();
                //if ((MasterListItem)cmbReferralTo.SelectedItem == null)
                //{
                //    cmbReferralTo.TextBox.SetValidation("Referred To Is Required");
                //    cmbReferralTo.TextBox.RaiseValidationError();
                //    cmbReferralTo.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbReferralTo.SelectedItem).ID == 0)
                //{
                //    cmbReferralTo.TextBox.SetValidation("Referred To Is Required");
                //    cmbReferralTo.TextBox.RaiseValidationError();
                //    cmbReferralTo.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbReferralTo.TextBox.ClearValidationError();


                //if (((clsPatientVO)this.DataContext).FamilyName == null || ((clsPatientVO)this.DataContext).FamilyName.Trim() == "")
                //{
                //    txtFamilyName.SetValidation("Family Name Is Required");
                //    txtFamilyName.RaiseValidationError();
                //    txtFamilyName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtFamilyName.ClearValidationError();
                //if (((clsPatientVO)this.DataContext).GeneralDetails.LastName == null || ((clsPatientVO)this.DataContext).GeneralDetails.LastName.Trim() == "")
                //{
                //    txtLastName.SetValidation("Last Name Is Required");
                //    txtLastName.RaiseValidationError();
                //    txtLastName.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    txtLastName.ClearValidationError();

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

                //if (((clsPatientVO)this.DataContext).Photo == null )
                //{
                //    imgPhoto.SetValidation("Photo Is Required.");
                //    imgPhoto.RaiseValidationError();

                //Commented by ajit jadhav Date 23/9/2016 for User Requirement

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
                #region For Pediatric Flow

                if (cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 13)
                {

                    if (string.IsNullOrEmpty(txtBabyNo.Text))
                    {
                        txtBabyNo.SetValidation("Baby No Is Required");
                        txtBabyNo.RaiseValidationError();
                        txtBabyNo.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (!string.IsNullOrEmpty(txtBabyNo.Text) && ((txtBabyNo.Text.IsPositiveNumber()) && Convert.ToInt16(txtBabyNo.Text) == 0))
                    {
                        txtBabyNo.SetValidation("Baby No should be greater than zero");
                        txtBabyNo.RaiseValidationError();
                        txtBabyNo.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtBabyNo.ClearValidationError();

                    if (string.IsNullOrEmpty(txtBabyNoOfNo.Text))
                    {
                        txtBabyNoOfNo.SetValidation("Baby Of No Is Required");
                        txtBabyNoOfNo.RaiseValidationError();
                        txtBabyNoOfNo.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (!string.IsNullOrEmpty(txtBabyNoOfNo.Text) && ((txtBabyNoOfNo.Text.IsPositiveNumber()) && Convert.ToInt16(txtBabyNoOfNo.Text) == 0))
                    {
                        txtBabyNoOfNo.SetValidation("Baby No should be greater than zero");
                        txtBabyNoOfNo.RaiseValidationError();
                        txtBabyNoOfNo.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtBabyNoOfNo.ClearValidationError();

                    if (string.IsNullOrEmpty(txtBirthWeight.Text))
                    {
                        txtBirthWeight.SetValidation("Birth Weight Is Required");
                        txtBirthWeight.RaiseValidationError();
                        txtBirthWeight.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtBirthWeight.ClearValidationError();

                    if (string.IsNullOrEmpty(txtBirthTime.Value.ToString()))
                    {
                        txtBirthTime.SetValidation("Birth Time Is Required");
                        txtBirthTime.RaiseValidationError();
                        txtBirthTime.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else if (txtBirthTime.Value != null && txtBirthTime.Value.Value != null && txtBirthTime.Value.Value.TimeOfDay > System.DateTime.Now.TimeOfDay)        //.ToShortDateString()
                    {
                        txtBirthTime.SetValidation("Birth Time is greater than current time");
                        txtBirthTime.RaiseValidationError();
                        txtBirthTime.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtBirthTime.ClearValidationError();


                    if (string.IsNullOrEmpty(txtParentName.Text))
                    {
                        txtParentName.SetValidation("Parent Name Is Required");
                        txtParentName.RaiseValidationError();
                        txtParentName.Focus();
                        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        result = false;
                    }
                    else
                        txtParentName.ClearValidationError();


                    if (Convert.ToInt16(txtYY.Text) >= 18 && (Convert.ToInt16(txtMM.Text) > 0 || Convert.ToInt16(txtDD.Text) > 0))
                    {
                        dtpDOB.SetValidation("Age should not be greater than 18 years for Pediatric");
                        dtpDOB.RaiseValidationError();
                        dtpDOB.Focus();
                        result = false;
                    }
                    else
                        dtpDOB.ClearValidationError();

                    //DateTime zeroTimePedia = new DateTime(1, 1, 1);
                    //TimeSpan spanPedia = System.DateTime.Now.Subtract(dtpDOB.SelectedDate.Value); //dtpMarriageAnnDate.SelectedDate.Value.Subtract(dtpDOB.SelectedDate.Value);
                    //// because we start at year 1 for the Gregorian 
                    //// calendar, we must subtract a year here.
                    //int yearsPedia = (zeroTimePedia + spanPedia).Year - 1;



                    //// 1, where my other algorithm resulted in 0.
                    //// Console.WriteLine("Yrs elapsed: " + years);


                    //if (yearsPedia > 18)       //&& ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Female
                    //{
                    //    dtpDOB.SetValidation("Age should not be greater than 18 years for Pediatric");
                    //    dtpDOB.RaiseValidationError();
                    //    dtpDOB.Focus();
                    //    result = false;
                    //}
                    ////else if (years < 21 && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Male)
                    ////{
                    ////    dtpMarriageAnnDate.SetValidation("Minimun 21 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                    ////    dtpMarriageAnnDate.RaiseValidationError();
                    ////    dtpMarriageAnnDate.Focus();
                    ////    result = false;
                    ////}
                    //else
                    //    dtpDOB.ClearValidationError();

                }
                #endregion

                if (IsPageLoded)
                {

                    if (txtEmail.Text.Trim().Length > 0)
                    {
                        if (txtEmail.Text.IsEmailValid())
                            txtEmail.ClearValidationError();
                        else
                        {
                            txtEmail.SetValidation("Please Enter Valid Email");
                            txtEmail.RaiseValidationError();
                            txtEmail.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                            result = false;
                        }
                    }

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
                        //if (Convert.ToInt16(txtSpouseYY.Text) < 18)
                        //{
                        //    txtSpouseYY.SetValidation("Age Can Not Be Less Than 18");
                        //    txtSpouseYY.RaiseValidationError();
                        //    txtSpouseYY.Focus();
                        //    result = false;
                        //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        //    ClickedFlag = 0;
                        //}
                        //else 
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

                }

                if (ChkInternationalPatient.IsChecked == true && cmbIdentity.SelectedItem == null)
                {
                    cmbIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");
                    cmbIdentity.TextBox.RaiseValidationError();
                    cmbIdentity.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else if (ChkInternationalPatient.IsChecked == true && cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID == 0)//&& ((MasterListItem)(cmbIdentity.SelectedItem)).ID != (long)(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient)
                {
                    if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != 0)
                    {
                        if (((MasterListItem)(cmbIdentity.SelectedItem)).ID != (long)(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient))
                            cmbIdentity.TextBox.SetValidation("For International Patient " + ((MasterListItem)cmbIdentity.SelectedItem).Description + " Details Are Mendatory");
                    }
                    else
                        cmbIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");
                    cmbIdentity.TextBox.RaiseValidationError();
                    cmbIdentity.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    cmbIdentity.TextBox.ClearValidationError();

                if ((MasterListItem)cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID > 0 && txtIdentityNumber.Text == string.Empty)
                {
                    txtIdentityNumber.SetValidation("Identity Number Is Required");
                    txtIdentityNumber.RaiseValidationError();
                    txtIdentityNumber.Focus();
                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    result = false;
                }
                else
                    txtIdentityNumber.ClearValidationError();


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


                        if (txtSpouseEmail.Text.Trim().Length > 0)
                        {
                            if (txtSpouseEmail.Text.IsEmailValid())
                                txtSpouseEmail.ClearValidationError();
                            else
                            {
                                txtSpouseEmail.SetValidation("Please Enter Valid Email");
                                txtSpouseEmail.RaiseValidationError();
                                txtSpouseEmail.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                        }

                        //* Commented by - Ajit Jadhav
                        //* Added Date - 3/80/2016
                        //* Comments - New Changes Mandatory fields Birth Date
                        //---------------------------
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
                        //***//------------------------------------------
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

                        //Commented by ajit jadhav Date 23/9/2016 for User Requirement

                        //if ((MasterListItem)cmbSpouseEducation.SelectedItem == null)
                        //{
                        //    cmbSpouseEducation.TextBox.SetValidation("Education Is Required");
                        //    cmbSpouseEducation.TextBox.RaiseValidationError();
                        //    cmbSpouseEducation.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbSpouseEducation.SelectedItem).ID == 0)
                        //{
                        //    cmbSpouseEducation.TextBox.SetValidation("Education Is Required");
                        //    cmbSpouseEducation.TextBox.RaiseValidationError();
                        //    cmbSpouseEducation.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbSpouseEducation.TextBox.ClearValidationError();

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
                        //else if (((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID == (long)((IApplicationConfiguration)App.Current).ApplicationConfigurations.MaritalStatus)
                        //{
                        //    if (dtpSpouseMarriageAnnDate.SelectedDate == null)
                        //    {
                        //        dtpSpouseMarriageAnnDate.SetValidation("Marriage Anniversary Date Is Required");
                        //        dtpSpouseMarriageAnnDate.RaiseValidationError();
                        //        dtpSpouseMarriageAnnDate.Focus();
                        //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //        result = false;
                        //    }
                        //    else
                        //        dtpSpouseMarriageAnnDate.ClearValidationError();
                        //}
                        else
                            cmbSpouseMaritalStatus.TextBox.ClearValidationError();


                        //if ((MasterListItem)cmbSpouseOccupation.SelectedItem == null)
                        //{
                        //    cmbSpouseOccupation.TextBox.SetValidation("Occupation Is Required");
                        //    cmbSpouseOccupation.TextBox.RaiseValidationError();
                        //    cmbSpouseOccupation.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbSpouseOccupation.SelectedItem).ID == 0)
                        //{
                        //    cmbSpouseOccupation.TextBox.SetValidation("Occupation Is Required");
                        //    cmbSpouseOccupation.TextBox.RaiseValidationError();
                        //    cmbSpouseOccupation.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbSpouseOccupation.TextBox.ClearValidationError();

                        //if ((MasterListItem)cmbSpouseSpecialReg.SelectedItem == null)
                        //{
                        //    cmbSpouseSpecialReg.TextBox.SetValidation("Special Registration Is Required");
                        //    cmbSpouseSpecialReg.TextBox.RaiseValidationError();
                        //    cmbSpouseSpecialReg.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbSpouseSpecialReg.SelectedItem).ID == 0)
                        //{
                        //    cmbSpouseSpecialReg.TextBox.SetValidation("Special Registration Is Required");
                        //    cmbSpouseSpecialReg.TextBox.RaiseValidationError();
                        //    cmbSpouseSpecialReg.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbSpouseSpecialReg.TextBox.ClearValidationError();

                        //if ((MasterListItem)cmbSpouseReligion.SelectedItem == null)
                        //{
                        //    cmbSpouseReligion.TextBox.SetValidation("Religion Is Required");
                        //    cmbSpouseReligion.TextBox.RaiseValidationError();
                        //    cmbSpouseReligion.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbSpouseReligion.SelectedItem).ID == 0)
                        //{
                        //    cmbSpouseReligion.TextBox.SetValidation("Religion Is Required");
                        //    cmbSpouseReligion.TextBox.RaiseValidationError();
                        //    cmbSpouseReligion.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbSpouseReligion.TextBox.ClearValidationError();

                        //if ((MasterListItem)cmbSpousePreferredLanguage.SelectedItem == null)
                        //{
                        //    cmbSpousePreferredLanguage.TextBox.SetValidation("Preferred Language Is Required");
                        //    cmbSpousePreferredLanguage.TextBox.RaiseValidationError();
                        //    cmbSpousePreferredLanguage.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbSpousePreferredLanguage.SelectedItem).ID == 0)
                        //{
                        //    cmbSpousePreferredLanguage.TextBox.SetValidation("Preferred Language Is Required");
                        //    cmbSpousePreferredLanguage.TextBox.RaiseValidationError();
                        //    cmbSpousePreferredLanguage.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbSpousePreferredLanguage.TextBox.ClearValidationError();
                        if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == true)  //Added by NileshD on 19April2019
                        {
                            if (((MasterListItem)cmbSpouseSpecialReg.SelectedItem).ID == (((IApplicationConfiguration)App.Current).ApplicationConfigurations).InternationalId) //Added by NileshD on 19April2019
                            {
                                if ((MasterListItem)cmbSpouseNationality.SelectedItem == null)
                                {
                                    cmbSpouseNationality.TextBox.SetValidation("Nationality Is Required");
                                    cmbSpouseNationality.TextBox.RaiseValidationError();
                                    cmbSpouseNationality.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }
                                else if (((MasterListItem)cmbSpouseNationality.SelectedItem).ID == 0)
                                {
                                    cmbSpouseNationality.TextBox.SetValidation("Nationality Is Required");
                                    cmbSpouseNationality.TextBox.RaiseValidationError();
                                    cmbSpouseNationality.Focus();
                                    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                    result = false;
                                }
                                else
                                    cmbSpouseNationality.TextBox.ClearValidationError();
                            }
                            else
                                cmbSpouseNationality.TextBox.ClearValidationError();
                        }//Added by NileshD on 19April2019
                        //if (txtSpouseCountry1.Text.Trim() == string.Empty)  //added by neena //commented by neena
                        //{
                        if ((MasterListItem)txtSpouseCountry.SelectedItem == null)
                        {
                            txtSpouseCountry.TextBox.SetValidation("Country Is Required");
                            txtSpouseCountry.TextBox.RaiseValidationError();
                            txtSpouseCountry.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else if (((MasterListItem)txtSpouseCountry.SelectedItem).ID == 0)
                        {
                            txtSpouseCountry.TextBox.SetValidation("Country Is Required");
                            txtSpouseCountry.TextBox.RaiseValidationError();
                            txtSpouseCountry.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            txtSpouseCountry.TextBox.ClearValidationError();
                        //}
                        //else
                        //    txtSpouseCountry1.ClearValidationError();

                        //if (txtSpouseState1.Text.Trim() == string.Empty)  //added by neena  //commented by neena
                        //{
                        if ((MasterListItem)txtSpouseState.SelectedItem == null)
                        {
                            txtSpouseState.TextBox.SetValidation("State Is Required");
                            txtSpouseState.TextBox.RaiseValidationError();
                            txtSpouseState.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else if (((MasterListItem)txtSpouseState.SelectedItem).ID == 0)
                        {
                            txtSpouseState.TextBox.SetValidation("State Is Required");
                            txtSpouseState.TextBox.RaiseValidationError();
                            txtSpouseState.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            txtSpouseState.TextBox.ClearValidationError();
                        //}
                        //else
                        //    txtSpouseState1.ClearValidationError();

                        //if (txtSpouseCity1.Text.Trim() == string.Empty)  //added by neena  //commented by neena
                        //{
                        if ((MasterListItem)txtSpouseCity.SelectedItem == null)
                        {
                            txtSpouseCity.TextBox.SetValidation("City Is Required");
                            txtSpouseCity.TextBox.RaiseValidationError();
                            txtSpouseCity.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else if (((MasterListItem)txtSpouseCity.SelectedItem).ID == 0)
                        {
                            txtSpouseCity.TextBox.SetValidation("City Is Required");
                            txtSpouseCity.TextBox.RaiseValidationError();
                            txtSpouseCity.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            txtSpouseCity.TextBox.ClearValidationError();
                        //}
                        //else
                        //    txtSpouseCity1.ClearValidationError();

                        //if (txtSpouseArea1.Text.Trim() == string.Empty)  //added by neena //commented by neena
                        //{

                        if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations).ValidationsFlag == false)  //Added by NileshD on 19April2019
                        {
                            if ((MasterListItem)txtSpouseArea.SelectedItem == null)
                            {
                                txtSpouseArea.TextBox.SetValidation("Area Is Required");
                                txtSpouseArea.TextBox.RaiseValidationError();
                                txtSpouseArea.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else if (((MasterListItem)txtSpouseArea.SelectedItem).ID == 0)
                            {
                                txtSpouseArea.TextBox.SetValidation("Area Is Required");
                                txtSpouseArea.TextBox.RaiseValidationError();
                                txtSpouseArea.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }
                            else
                                txtSpouseArea.TextBox.ClearValidationError();
                        } //Added by NileshD on 19April2019
                        //}
                        //else
                        //    txtSpouseArea1.ClearValidationError();


                        ////added by neena
                        //if (txtSpouseArea1.Text == string.Empty)
                        //{
                        //    txtSpouseArea1.SetValidation("Area Is Required");
                        //    txtSpouseArea1.RaiseValidationError();
                        //    txtSpouseArea1.Focus();
                        //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        //    result = false;
                        //}
                        //else
                        //    txtSpouseArea1.ClearValidationError();

                        //if (txtSpouseCity1.Text == string.Empty)
                        //{
                        //    txtSpouseCity1.SetValidation("City Is Required");
                        //    txtSpouseCity1.RaiseValidationError();
                        //    txtSpouseCity1.Focus();
                        //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        //    result = false;
                        //}
                        //else
                        //    txtSpouseCity1.ClearValidationError();

                        //if (txtSpouseState1.Text == string.Empty)
                        //{
                        //    txtSpouseState1.SetValidation("State Is Required");
                        //    txtSpouseState1.RaiseValidationError();
                        //    txtSpouseState1.Focus();
                        //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        //    result = false;
                        //}
                        //else
                        //    txtSpouseState1.ClearValidationError();

                        //if (txtSpouseCountry1.Text == string.Empty)
                        //{
                        //    txtSpouseCountry1.SetValidation("Country Is Required");
                        //    txtSpouseCountry1.RaiseValidationError();
                        //    txtSpouseCountry1.Focus();
                        //    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                        //    result = false;
                        //}
                        //else
                        //    txtSpouseCountry1.ClearValidationError();
                        ////


                        //if ((MasterListItem)txtSpousePinCode.SelectedItem == null)
                        //{
                        //    txtSpousePinCode.SetValidation("PIN Code Is Required");
                        //    txtSpousePinCode.RaiseValidationError();
                        //    txtSpousePinCode.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)txtSpousePinCode.SelectedItem).ID == 0)
                        //{
                        //    txtSpousePinCode.SetValidation("PIN Code Is Required");
                        //    txtSpousePinCode.RaiseValidationError();
                        //    txtSpousePinCode.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    txtSpousePinCode.ClearValidationError();

                        if (txtSpousePinCode.Text == null)
                        {
                            txtSpousePinCode.SetValidation("PIN Code Is Required");
                            txtSpousePinCode.RaiseValidationError();
                            txtSpousePinCode.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            txtSpousePinCode.ClearValidationError();


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
                        else if (txtSpouseContactNo1.Text.Trim().Length > 14)
                        {
                            txtSpouseContactNo1.Textbox.SetValidation("Mobile Number Should Be 14 Digit");
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

                        //if ((MasterListItem)cmbPatientSource.SelectedItem == null)                  
                        //{
                        //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                        //    cmbPatientSource.TextBox.RaiseValidationError();
                        //    cmbPatientSource.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                        //{
                        //    cmbPatientSource.TextBox.SetValidation("Patient Source Is Required");
                        //    cmbPatientSource.TextBox.RaiseValidationError();
                        //    cmbPatientSource.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    cmbPatientSource.TextBox.ClearValidationError();

                        //if (((clsPatientVO)this.DataContext).SpouseDetails.FamilyName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName.Trim() == "")
                        //{
                        //    txtSpouseFamilyName.SetValidation("Family Name Is Required");
                        //    txtSpouseFamilyName.RaiseValidationError();
                        //    txtSpouseFamilyName.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    txtSpouseFamilyName.ClearValidationError();

                        //* Added by - Ajit Jadhav
                        //* Added Date - 3/80/2016
                        //* Comments - Mandatory fields Last Name
                        //---------------------------
                        //if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == "")
                        //{
                        //    txtSpouseLastName.SetValidation("Last Name Is Required");
                        //    txtSpouseLastName.RaiseValidationError();
                        //    txtSpouseLastName.Focus();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    txtLastName.ClearValidationError();
                        //***//---------------------------------------------

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

                        //if (((clsPatientVO)this.DataContext).SpouseDetails.Photo == null)
                        //{
                        //    Spouseborderimage.SetValidation("Photo Is Required.");
                        //    Spouseborderimage.RaiseValidationError();
                        //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //    result = false;
                        //}
                        //else
                        //    Spouseborderimage.ClearValidationError();


                        if (txtSpouseYY.Text != "")
                        {
                            //if (Convert.ToInt16(txtSpouseYY.Text) < 18)
                            //{
                            //    txtSpouseYY.SetValidation("Age Can Not Be Less Than 18");
                            //    txtSpouseYY.RaiseValidationError();
                            //    txtSpouseYY.Focus();
                            //    result = false;
                            //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            //    ClickedFlag = 0;
                            //}
                            //else
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

                        //* Added by - Ajit Jadhav
                        //* Added Date - 3/80/2016
                        //* Comments - Mandatory fields Last Name
                        if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                        {
                            if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
                            {
                                dtpSpouseDOB.SetValidation("Birth Date Is Required");
                                dtpSpouseDOB.RaiseValidationError();
                                dtpSpouseDOB.Focus();
                                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                                result = false;
                            }

                            else if (((MasterListItem)cmbPatientType.SelectedItem).ID != 13 && ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
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
                        //---------------------------

                        //if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                        //{

                        //    if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                        //    {
                        //        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                        //        dtpSpouseDOB.RaiseValidationError();
                        //        dtpSpouseDOB.Focus();
                        //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //        result = false;
                        //    }
                        //    else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)
                        //    {
                        //        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                        //        dtpSpouseDOB.RaiseValidationError();
                        //        dtpSpouseDOB.Focus();
                        //        PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                        //        result = false;
                        //    }

                        //    else
                        //        dtpSpouseDOB.ClearValidationError();
                        //}



                        if (ChkSpouseInternationalPatient.IsChecked == true && cmbSpouseIdentity.SelectedItem == null)
                        {
                            cmbSpouseIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");
                            cmbSpouseIdentity.TextBox.RaiseValidationError();
                            cmbSpouseIdentity.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else if (ChkSpouseInternationalPatient.IsChecked == true && cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 0)//&& ((MasterListItem)(cmbSpouseIdentity.SelectedItem)).ID != (long)(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient)
                        {
                            if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != 0)
                            {
                                if (((MasterListItem)(cmbSpouseIdentity.SelectedItem)).ID != (long)(((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient))
                                    cmbSpouseIdentity.TextBox.SetValidation("For International Patient " + ((MasterListItem)cmbSpouseIdentity.SelectedItem).Description + " Details Are Mendatory");
                            }
                            else
                                cmbSpouseIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");
                            cmbSpouseIdentity.TextBox.RaiseValidationError();
                            cmbSpouseIdentity.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            cmbSpouseIdentity.TextBox.ClearValidationError();

                        if ((MasterListItem)cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID > 0 && txtSpouseIdentityNumber.Text == string.Empty)
                        {
                            txtSpouseIdentityNumber.SetValidation("Identity Number Is Required");
                            txtSpouseIdentityNumber.RaiseValidationError();
                            txtSpouseIdentityNumber.Focus();
                            PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Spouse;
                            result = false;
                        }
                        else
                            txtSpouseIdentityNumber.ClearValidationError();


                        if (((clsPatientVO)this.DataContext).GeneralDetails.MarriageAnnDate != null && ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                        {
                            if (dtpSpouseDOB.SelectedDate < dtpSpouseMarriageAnnDate.SelectedDate)
                            {
                                //System.DateTime firstDate = (DateTime)dtpDOB.SelectedDate;
                                //System.DateTime secondDate = (DateTime)dtpMarriageAnnDate.SelectedDate;

                                //System.TimeSpan diff = secondDate.Subtract(firstDate);
                                //System.TimeSpan diff1 = secondDate - firstDate;

                                //int totYrs = secondDate.AddYears(-(firstDate.Year)).Year;
                                DateTime zeroTime = new DateTime(1, 1, 1);
                                TimeSpan span = dtpSpouseMarriageAnnDate.SelectedDate.Value.Subtract(dtpSpouseDOB.SelectedDate.Value);
                                // because we start at year 1 for the Gregorian 
                                // calendar, we must subtract a year here.
                                int years = (zeroTime + span).Year - 1;

                                if (years < 18 && ((MasterListItem)cmbSpouseGender.SelectedItem).ID == (long)Genders.Female)
                                {
                                    dtpSpouseMarriageAnnDate.SetValidation("Minimun 18 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                                    dtpSpouseMarriageAnnDate.RaiseValidationError();
                                    dtpSpouseMarriageAnnDate.Focus();
                                    result = false;
                                }
                                else if (years < 21 && ((MasterListItem)cmbSpouseGender.SelectedItem).ID == (long)Genders.Male)
                                {
                                    dtpSpouseMarriageAnnDate.SetValidation("Minimun 21 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                                    dtpSpouseMarriageAnnDate.RaiseValidationError();
                                    dtpSpouseMarriageAnnDate.Focus();
                                    result = false;
                                }
                                else
                                    dtpSpouseMarriageAnnDate.ClearValidationError();
                            }
                            else
                            {
                                dtpSpouseMarriageAnnDate.SetValidation("Date of Birth Should be Less than Date Of  Marriage Anniversary");
                                dtpSpouseMarriageAnnDate.RaiseValidationError();
                                dtpSpouseMarriageAnnDate.Focus();
                                result = false;
                            }
                        }
                        else if (((clsPatientVO)this.DataContext).GeneralDetails.MarriageAnnDate != null && ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null && SpouseDOB != null)
                        {
                            if (SpouseDOB < dtpSpouseMarriageAnnDate.SelectedDate)
                            {
                                //System.DateTime firstDate = (DateTime)dtpDOB.SelectedDate;
                                //System.DateTime secondDate = (DateTime)dtpMarriageAnnDate.SelectedDate;

                                //System.TimeSpan diff = secondDate.Subtract(firstDate);
                                //System.TimeSpan diff1 = secondDate - firstDate;

                                //int totYrs = secondDate.AddYears(-(firstDate.Year)).Year;
                                DateTime zeroTime = new DateTime(1, 1, 1);
                                TimeSpan span = dtpSpouseMarriageAnnDate.SelectedDate.Value.Subtract(SpouseDOB.Value);
                                // because we start at year 1 for the Gregorian 
                                // calendar, we must subtract a year here.
                                int years = (zeroTime + span).Year - 1;

                                if (years < 18 && ((MasterListItem)cmbSpouseGender.SelectedItem).ID == (long)Genders.Female)
                                {
                                    dtpSpouseMarriageAnnDate.SetValidation("Minimun 18 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                                    dtpSpouseMarriageAnnDate.RaiseValidationError();
                                    dtpSpouseMarriageAnnDate.Focus();
                                    result = false;
                                }
                                else if (years < 21 && ((MasterListItem)cmbSpouseGender.SelectedItem).ID == (long)Genders.Male)
                                {
                                    dtpSpouseMarriageAnnDate.SetValidation("Minimun 21 Years difference Required Between Date of Birth And Date Of  Marriage Anniversary");
                                    dtpSpouseMarriageAnnDate.RaiseValidationError();
                                    dtpSpouseMarriageAnnDate.Focus();
                                    result = false;
                                }
                                else
                                    dtpSpouseMarriageAnnDate.ClearValidationError();
                            }
                            else
                            {
                                dtpSpouseMarriageAnnDate.SetValidation("Date of Birth Should be Less than Date Of  Marriage Anniversary");
                                dtpSpouseMarriageAnnDate.RaiseValidationError();
                                dtpSpouseMarriageAnnDate.Focus();
                                result = false;
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
        int count = 0;
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

                        //added by neena
                        //commented by neena
                        //if (count == 0)
                        //    count = 1;
                        //if ((txtSpouseCountry1.Visibility == Visibility.Collapsed) || (txtSpouseState1.Visibility == Visibility.Collapsed) || (txtSpouseCity1.Visibility == Visibility.Collapsed) || ((txtSpouseArea1.Visibility == Visibility.Collapsed)))
                        //{
                        //    if (SpouseClick == false)
                        //        FillCountry();
                        //}
                        //
                        //

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
                                //if (((clsPatientVO)this.DataContext).SpouseDetails.FamilyName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FamilyName.Trim() == "")
                                //{
                                //    txtSpouseFamilyName.SetValidation("Family Name Is Required");
                                //    txtSpouseFamilyName.RaiseValidationError();
                                //}
                                //else
                                //    txtSpouseFamilyName.ClearValidationError();


                                //if (((clsPatientVO)this.DataContext).SpouseDetails.LastName == null || ((clsPatientVO)this.DataContext).SpouseDetails.LastName.Trim() == "")
                                //{
                                //    txtSpouseLastName.SetValidation("Last Name Is Required");
                                //    txtSpouseLastName.RaiseValidationError();
                                //}
                                //else
                                //    txtSpouseLastName.ClearValidationError();

                                if (((clsPatientVO)this.DataContext).SpouseDetails.FirstName == null || ((clsPatientVO)this.DataContext).SpouseDetails.FirstName.Trim() == "")
                                {
                                    txtSpouseFirstName.SetValidation("First Name Is Required");
                                    txtSpouseFirstName.RaiseValidationError();
                                }
                                else
                                    txtSpouseFirstName.ClearValidationError();

                                if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
                                {
                                    dtpSpouseDOB.SetValidation("Birth Date Is Required");
                                    dtpSpouseDOB.RaiseValidationError();
                                }
                                else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
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


                                if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth != null)
                                {

                                    if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date > DateTime.Now.Date)
                                    {
                                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Greater Than Todays Date");
                                        dtpSpouseDOB.RaiseValidationError();
                                    }
                                    else if (((MasterListItem)cmbPatientType.SelectedItem).ID != 13 && ((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth.Value.Date == DateTime.Now.Date)      // 5. For Pediatric Flow On 27012017 
                                    {
                                        dtpSpouseDOB.SetValidation("Birth Date Can Not Be Equals To Todays Date");
                                        dtpSpouseDOB.RaiseValidationError();
                                    }
                                    else
                                        dtpSpouseDOB.ClearValidationError();
                                }


                                clsPatientVO myVO = new clsPatientVO();
                                myVO = (clsPatientVO)this.DataContext;

                                //txtSpouseLastName.Text = txtLastName.Text;
                                //txtSpouseFamilyName.Text = txtFamilyName.Text;
                                //txtSpouseAddress1.Text = txtAddress1.Text;
                                //txtSpouseAddress2.Text = txtAddress2.Text;
                                //txtSpouseAddress3.Text = txtAddress3.Text;                              
                                //txtSpouseArea.Text = txtArea.Text;
                                //txtSpouseCountry.Text = txtCountry.Text;
                                //txtSpouseState.Text = txtState.Text;
                                //txtSpouseCity.Text = txtCity.Text;
                                //txtSpouseDistrict.Text = txtDistrict.Text;
                                //txtSpouseTaluka.Text = txtTaluka.Text;

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
                                txtSpouseCountry.SelectedItem = ((MasterListItem)txtCountry.SelectedItem).DeepCopy();
                                txtSpouseState.SelectedItem = ((MasterListItem)txtState.SelectedItem).DeepCopy();
                                txtSpouseCity.SelectedItem = ((MasterListItem)txtCity.SelectedItem).DeepCopy();
                                txtSpouseArea.SelectedItem = ((MasterListItem)txtArea.SelectedItem).DeepCopy();
                                txtSpouseResiCountryCode.Text = txtResiCountryCode.Text;
                                txtSpouseResiSTD.Text = txtResiSTD.Text;
                                txtSpouseMobileCountryCode.Text = txtMobileCountryCode.Text;
                                txtSpousePinCode.Text = myVO.SpouseDetails.Pincode.DeepCopy();
                                //  txtPinCode.Text = myVO.Pincode;
                                cmbSpouseMaritalStatus.SelectedItem = cmbMaritalStatus.SelectedItem;
                                cmbSpouseReligion.SelectedItem = ((MasterListItem)cmbReligion.SelectedItem).DeepCopy();
                                //By Anjali......................................
                                //  ChkSpouseInternationalPatient.IsChecked = ChkInternationalPatient.IsChecked;
                                txtSpouseAddress1.Text = txtAddress1.Text;
                                txtSpouseAddress2.Text = txtAddress2.Text;
                                txtSpouseAddress3.Text = txtAddress3.Text;

                                txtSpousePinCode.Text = txtPinCode.Text;
                                txtSpouseContactNo2.Text = txtContactNo2.Text;
                                //....................................................................



                                //this.UpdateLayout();
                                //((clsPatientVO)this.DataContext).SpouseDetails.Area = ((clsPatientVO)this.DataContext).Area;
                                //((clsPatientVO)this.DataContext).SpouseDetails.Country = ((clsPatientVO)this.DataContext).Country;
                                //((clsPatientVO)this.DataContext).SpouseDetails.State = ((clsPatientVO)this.DataContext).State;
                                //((clsPatientVO)this.DataContext).SpouseDetails.City = ((clsPatientVO)this.DataContext).City;
                                //((clsPatientVO)this.DataContext).SpouseDetails.District = ((clsPatientVO)this.DataContext).District;
                                //((clsPatientVO)this.DataContext).SpouseDetails.Taluka = ((clsPatientVO)this.DataContext).Taluka;
                                //((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID = ((clsPatientVO)this.DataContext).MaritalStatusID;
                                //txtSpouseResiCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiNoCountryCode.ToString();
                                //txtSpouseResiSTD.Text = ((clsPatientVO)this.DataContext).SpouseDetails.ResiSTDCode.ToString();
                                //txtSpouseMobileCountryCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.MobileCountryCode.ToString();
                                //cmbSpouseReligion.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.ReligionID;
                                //cmbSpouseMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.MaritalStatusID;
                                //  CheckSpouseValidations();

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
                    case RegistrationTabs.Visit:
                        //tabPatEncounterInfo

                        if (visitContent.Content == null)
                        {
                            UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.EncounterWindow") as UIElement;
                            visitwin = new EncounterWindow();
                            visitwin = (EncounterWindow)mydata;
                            visitwin.VAppointmentID = mAppointmentID;

                            ((IInitiateCIMS)visitwin).Initiate("NEWRE");
                            visitwin.mainFormBorder.BorderThickness = new Thickness(0);
                            visitwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                            visitContent.Content = visitwin;
                        }
                        break;

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
                            //Sponsortwin.myPatient = (clsPatientVO)this.DataContext;
                            //if (cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID > 0)
                            //{
                            //    Sponsortwin.PatientTypeID = ((MasterListItem)cmbPatientType.SelectedItem).ID;
                            //}
                            Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);
                            Sponsortwin.SaveCommandPanel.Visibility = System.Windows.Visibility.Collapsed;
                            Sponsortwin.mainFormBorder.BorderThickness = new Thickness(0);

                            if (PatientEditMode == true)
                            //Sponsortwin.dgSponserList.IsEnabled = false;
                            {
                                Sponsortwin.pnlAddSponser.Visibility = System.Windows.Visibility.Collapsed;
                                //sponsorContent.IsEnabled = false;   
                                Sponsortwin.dgSponserList.IsEnabled = false; //***//                             
                                Sponsortwin.BankDetails.Visibility = System.Windows.Visibility.Visible;
                                Sponsortwin.objbankDetails = new clsBankDetailsInfoVO();
                                Sponsortwin.objbankDetails.BankId = BankId;
                                Sponsortwin.objbankDetails.BranchId = BranchId;
                                Sponsortwin.objbankDetails.IFSCCode = IFSCCode;
                                Sponsortwin.objbankDetails.AccountNumber = AccountNumber;
                                Sponsortwin.objbankDetails.AccountHolderName = AccountHolderName;
                                Sponsortwin.objbankDetails.AccountTypeId = AccountTypeId;
                            }
                            sponsorContent.Content = Sponsortwin;
                        }
                        break;
                    //case 3:
                    //    //tabBillingInfo
                    //    CmdSave.Visibility = System.Windows.Visibility.Collapsed;
                    //    CmdClose.Visibility = System.Windows.Visibility.Collapsed;
                    //break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 

        // BY BHUSHAN . . . . . 
        //by neena
        private void FillPatientAddress()
        {
            //commented by neena
            //RegionObj = new MasterListItem(((clsPatientVO)this.DataContext).RegionID, ((clsPatientVO)this.DataContext).RegionCode, ((clsPatientVO)this.DataContext).Region);
            //txtArea1.Text = Convert.ToString(RegionObj.TemplateName);

            //CityObj = new MasterListItem(((clsPatientVO)this.DataContext).CityID, ((clsPatientVO)this.DataContext).CityCode, ((clsPatientVO)this.DataContext).CityN);
            //txtCity1.Text = CityObj.TemplateName;

            //StateObj = new MasterListItem(((clsPatientVO)this.DataContext).StateID, ((clsPatientVO)this.DataContext).StateCode, ((clsPatientVO)this.DataContext).StateN);
            //txtState1.Text = StateObj.TemplateName;

            //CountryObj = new MasterListItem(((clsPatientVO)this.DataContext).CountryID, ((clsPatientVO)this.DataContext).CountryCode, ((clsPatientVO)this.DataContext).CountryN);
            //txtCountry1.Text = CountryObj.TemplateName;

            //txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;

            //SpouseRegionObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.RegionID, ((clsPatientVO)this.DataContext).SpouseDetails.RegionCode, ((clsPatientVO)this.DataContext).SpouseDetails.Region);
            //if (SpouseRegionObj.TemplateName != null)
            //    txtSpouseArea1.Text = SpouseRegionObj.TemplateName;

            //SpouseCityObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.CityID, ((clsPatientVO)this.DataContext).SpouseDetails.CityCode, ((clsPatientVO)this.DataContext).SpouseDetails.CityN);
            //if (SpouseCityObj.TemplateName != null)
            //    txtSpouseCity1.Text = SpouseCityObj.TemplateName;

            //SpouseStateObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.StateID, ((clsPatientVO)this.DataContext).SpouseDetails.StateCode, ((clsPatientVO)this.DataContext).SpouseDetails.StateN);
            //if (SpouseStateObj.TemplateName != null)
            //    txtSpouseState1.Text = SpouseStateObj.TemplateName;

            //SpouseCountryObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.CountryID, ((clsPatientVO)this.DataContext).SpouseDetails.CountryCode, ((clsPatientVO)this.DataContext).SpouseDetails.CountryN);
            //if (SpouseCountryObj.TemplateName != null)
            //    txtSpouseCountry1.Text = SpouseCountryObj.TemplateName;

            //if (((clsPatientVO)this.DataContext).SpouseDetails.Pincode != null)
            //    txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
            //
        }
        //

        private void FillReferralName()
        {
            //cmbReferralName
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
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //commented and changed by neena (dated 14/3/17) 
                        //if (Flagref == true)
                        //{
                        //    var results = from r in objList
                        //                  where r.ID == 10
                        //                  select r;
                        //    cmbReferralName.ItemsSource = null;
                        //    cmbReferralName.ItemsSource = results.ToList();
                        //    cmbReferralName.SelectedItem = results.ToList()[0];

                        //}
                        //else
                        //{
                        cmbReferralName.ItemsSource = null;
                        cmbReferralName.ItemsSource = objList;
                        cmbReferralName.SelectedItem = objList[0];
                        //}
                        //cmbPatientType.ItemsSource = null;
                        //cmbPatientType.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbReferralName.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.ReferralTypeID;
                    }
                    FillMaritalStatus();  //added by neena

                    //FillPatientType(); //commented by neena
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
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

                        //if (Flagref == true)
                        //{


                        // changed by neena (for surrogate dated 14/3/17)
                        if (Flagref == false)
                        {
                            var results = from r in objList
                                          //where r.ID != 10
                                          select r;
                            cmbPatientType.ItemsSource = null;
                            cmbPatientType.ItemsSource = results.ToList();
                            cmbPatientType.SelectedItem = results.ToList()[0];
                        }
                        else if (Flagref == true)
                        {
                            //added by neena                          
                            var results = from r in objList
                                          where r.ID == 10
                                          select r;
                            cmbPatientType.ItemsSource = null;
                            cmbPatientType.ItemsSource = results.ToList();
                            cmbPatientType.SelectedItem = results.ToList()[0];
                            // 
                        }


                        //}
                        //else
                        ////{
                        //    cmbPatientType.ItemsSource = null;
                        //    cmbPatientType.ItemsSource = objList;

                        //}
                        //cmbPatientType.ItemsSource = null;
                        //cmbPatientType.ItemsSource = objList;
                    }

                    if (this.DataContext != null)
                    {
                        cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;
                    }
                    FillPreffix(); //added by neena

                    // FillMaritalStatus(); //commented by neena
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
            //BizAction.IsActive = true;
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
                    //FillReligion();
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


        //private void GetBdList()  
        //{
        //    try
        //    {
        //        clsgetBdMasterBizActionVO BizAction = new clsgetBdMasterBizActionVO();

        //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;


        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            if (e.Error == null && e.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "- Select -"));
        //                objList.AddRange(((clsgetBdMasterBizActionVO)e.Result).MasterListItem);

        //                if (Flagref == true)
        //                {
        //                    var results = from r in objList
        //                                  where r.ID == 10
        //                                  select r;
        //                    cmbBD.ItemsSource = null;
        //                    cmbBD.ItemsSource = results.ToList();
        //                    cmbBD.SelectedItem = results.ToList()[0];

        //                }
        //                else
        //                {
        //                    //var resultNew = from r in objList
        //                    //                where r.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID
        //                    //              select r;
        //                    cmbBD.ItemsSource = null;
        //                    cmbBD.ItemsSource = objList;
        //                    cmbBD.SelectedItem = objList[0];
        //                    //cmbBD.ItemsSource = resultNew.ToList();
        //                    //cmbBD.SelectedItem = resultNew.ToList()[0];
        //                }
        //            }

        //            if (this.DataContext != null)
        //            {
        //                cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;
        //            }

        //        };
        //        Client.ProcessAsync(BizAction, null);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        wait.Close();
        //        throw;
        //    } 
        //}
        //clsGetBdMasterBizActionVO
        private void FillBloodGroup()
        {
            try
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
                    //FillGender();
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
                    //FillEducationMaster();

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

        private void FillIdentity()
        {
            try
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
                        cmbIdentity.ItemsSource = objList.DeepCopy();
                        cmbSpouseIdentity.ItemsSource = null;
                        cmbSpouseIdentity.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbIdentity.SelectedValue = ((clsPatientVO)this.DataContext).IdentityID;
                        cmbSpouseIdentity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.IdentityID;
                    }
                    //FillReferralName();
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
        WaitIndicator wait = new WaitIndicator();
        List<MasterListItem> objList = null;
        private void FillPreffix()
        {
            try
            {
                //wait.Show();
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
                        objList = new List<MasterListItem>();
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

                    if (PatientEditMode == false && cmbPatientType.SelectedItem != null && (((MasterListItem)cmbPatientType.SelectedItem).ID == 7 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 8 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 9))   //if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)
                    {
                        //cmbPreffix.SelectedItem = objList[3];
                        //cmbSpousePreffix.SelectedItem = objList[2];
                    }
                    //if (PatientEditMode == false && cmbPatientType.SelectedItem != null && ())   //if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)
                    //{
                    //    cmbPreffix.SelectedItem = objList[2];
                    //    cmbSpousePreffix.SelectedItem = objList[3];
                    //}
                    else if (PatientEditMode == false && cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID == 13)   //if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)
                    {
                        cmbPreffix.SelectedItem = objList[6];
                        //cmbSpousePreffix.SelectedItem = objList[21];
                    }

                    //FillIdentity();
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

        private void FillReligion()
        {
            try
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
                    //FillOccupation();



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

        private void FillOccupation()
        {
            try
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
                    //FillBloodGroup();
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

        //By Anjali........................................
        private void FillRefDoctor()
        {
            try
            {
                clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
                BizAction.ComboList = new List<clsComboMasterBizActionVO>();
                BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

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
            catch (Exception ex)
            {
                wait.Close();
                throw;
            }
        }
        private void FillRefDoctor(long DoctorID)
        {
            clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;

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
                        cmbReferralDoctor.SelectedValue = DoctorID;


                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }
        //.................................................


        //Added By Ashish Z........................................
        private void FillEducationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
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

                        cmbSpouseEducation.ItemsSource = null;
                        cmbSpouseEducation.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbEducation.SelectedValue = ((clsPatientVO)this.DataContext).EducationID;
                        cmbSpouseEducation.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.EducationID;
                    }
                    //FillPrefLanguageMaster();

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

        private void FillPrefLanguageMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_PrefLanguageMaster;
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
                        cmbPreferredLanguage.ItemsSource = null;
                        cmbPreferredLanguage.ItemsSource = objList.DeepCopy();

                        cmbSpousePreferredLanguage.ItemsSource = null;
                        cmbSpousePreferredLanguage.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbPreferredLanguage.SelectedValue = ((clsPatientVO)this.DataContext).PreferredLangID;
                        cmbSpousePreferredLanguage.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.PreferredLangID;
                    }
                    //FillTreatRequiredMasterMaster();
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


        private void FillTreatRequiredMasterMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_TreatRequiredMaster;
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
                        cmbTreatmentRequired.ItemsSource = null;
                        cmbTreatmentRequired.ItemsSource = objList.DeepCopy();

                        cmbSpouseTreatmentRequired.ItemsSource = null;
                        cmbSpouseTreatmentRequired.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbTreatmentRequired.SelectedValue = ((clsPatientVO)this.DataContext).TreatRequiredID;
                        cmbSpouseTreatmentRequired.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.TreatRequiredID;
                    }
                    //FillNationalityMaster();
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


        private void FillNationalityMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_NationalityMaster;
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
                        cmbNationality.ItemsSource = null;
                        cmbNationality.ItemsSource = objList.DeepCopy();

                        cmbSpouseNationality.ItemsSource = null;
                        cmbSpouseNationality.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbNationality.SelectedValue = ((clsPatientVO)this.DataContext).NationalityID;
                        cmbSpouseNationality.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.NationalityID;
                    }
                    //FillSpecialRegistrationMaster();
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

        private void FillSpecialRegistrationMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_SpecialRegistrationMaster;
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
                        cmbSpecialReg.ItemsSource = null;
                        cmbSpecialReg.ItemsSource = objList.DeepCopy();

                        cmbSpouseSpecialReg.ItemsSource = null;
                        cmbSpouseSpecialReg.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpecialRegID;
                        cmbSpouseSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.SpecialRegID;
                    }
                    //wait.Close();
                    //IsSetWetIndicator = true; //Flag set to show wait indicator on Country, State, City, Region Fill Combo Methods

                    if (string.IsNullOrEmpty(txtMobileCountryCode.Text))
                        txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
                    if (string.IsNullOrEmpty(txtSpouseMobileCountryCode.Text))
                        txtSpouseMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;

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



        //---------------------------------------------------------


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
                throw ex;
            }
        }
        private void FillBdMaster()
        {
            try
            {

                clsgetBdMasterBizActionVO BizAction = new clsgetBdMasterBizActionVO();

                BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;


                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "- Select -"));
                        objList.AddRange(((clsgetBdMasterBizActionVO)e.Result).MasterListItem);

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
                            //var resultNew = from r in objList
                            //                where r.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID
                            //              select r;
                            cmbBD.ItemsSource = null;
                            cmbBD.ItemsSource = objList;
                            cmbBD.SelectedItem = objList[0];
                            //cmbBD.ItemsSource = resultNew.ToList();
                            //cmbBD.SelectedItem = resultNew.ToList()[0];
                        }




                    }



                    if (this.DataContext != null)
                    {
                        cmbBD.SelectedValue = ((clsPatientVO)this.DataContext).BDID;
                        if (PatientEditMode == true)
                        {
                            if (((clsPatientVO)this.DataContext).BDID == 0)
                            {

                            }
                            if (cmbBD.SelectedItem != null && ((MasterListItem)cmbBD.SelectedItem).ID == 0)
                            {

                                //  cmbBD.IsEnabled = true;
                            }
                            else
                            {
                                // cmbBD.IsEnabled = false;
                            }
                        }
                        else
                        {

                        }
                    }



                };
                Client.ProcessAsync(BizAction, null);
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
        private void FillBdMaster_OLD()
        {
            //cmbBD
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_BdMaster;

                //  BizAction.UnitID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;


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
                            //var resultNew = from r in objList
                            //                where r.ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID
                            //              select r;
                            cmbBD.ItemsSource = null;
                            cmbBD.ItemsSource = objList;
                            cmbBD.SelectedItem = objList[0];
                            //cmbBD.ItemsSource = resultNew.ToList();
                            //cmbBD.SelectedItem = resultNew.ToList()[0];
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
        //***//-------------------------------------
        private void ClosePatientToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
            //added by neena for surrogacy
            if (Flagref == true)
                ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
            else
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            // ((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }

        private void CheckValidation1()
        {
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

                    if (chkImgNameFlag == true)
                    {
                        //if ((((clsPatientVO)this.DataContext).ImageName == null) || (tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && ((clsPatientVO)this.DataContext).SpouseDetails.Photo == null))
                        ////if ((((clsPatientVO)this.DataContext).Photo == null) || (tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && ((clsPatientVO)this.DataContext).SpouseDetails.Photo == null))
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Photo Is Required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();

                        //    ClickedFlag = 0;
                        //}
                        //chkImgNameFlag = false;
                        // else
                        CheckValidation1();
                    }
                    else if (chkImgNameFlag == false)
                    {
                        //if ((((clsPatientVO)this.DataContext).Photo == null) || (tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible && ((clsPatientVO)this.DataContext).SpouseDetails.Photo == null))
                        //{
                        //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                        //                                         new MessageBoxControl.MessageBoxChildWindow("Palash", "Photo Is Required", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        //    msgW1.Show();

                        //    ClickedFlag = 0;
                        //}
                        //else
                        CheckValidation1();
                    }
                    // }
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
            Indicatior.Show();
            try
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

                //* Added Date - 4/10/2016
                if (!string.IsNullOrEmpty(txtPanNo.Text))
                    BizAction.PatientDetails.GeneralDetails.PanNumber = txtPanNo.Text;
                //***//----

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

                        //* Added Date - 4/10/2016
                        if (!string.IsNullOrEmpty(txtSpousePanNo.Text))
                            BizAction.PatientDetails.SpouseDetails.SpousePanNumber = txtSpousePanNo.Text;
                        //***//----
                    }

                }
                else
                {
                    BizAction.PatientDetails.SpouseDetails.GenderID = 0;
                    BizAction.PatientDetails.SpouseDetails.DateOfBirth = null;
                    BizAction.PatientDetails.SpouseDetails.MobileCountryCode = null;
                    BizAction.PatientDetails.SpouseDetails.ContactNO1 = null;
                    BizAction.PatientDetails.SpouseDetails.SpousePanNumber = null;
                }

                BizAction.PatientEditMode = PatientEditMode;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Error == null && ea.Result != null)
                    {
                        Indicatior.Close();
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
                            if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus == 4)  //Added by ajit date 4/10.2016
                            {
                                strDuplicateMsg = "Pan Number already exists, Are you sure you want to save the Patient Details ?";

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
                            //Indicatior.Close();
                            string strMsg = string.Empty;
                            if (((MasterListItem)(cmbIdentity.SelectedItem)).ID == 0)//((clsPatientVO)this.DataContext).Photo == null ||
                                strMsg = ", without ID Proof ?";
                            else if ((((MasterListItem)(cmbSpouseIdentity.SelectedItem)).ID == 0) && tabPatSpouseInformation.Visibility == System.Windows.Visibility.Visible)//((clsPatientVO)this.DataContext).SpouseDetails.Photo == null ||
                                strMsg = ", without spouse ID Proof ?";

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
            catch (Exception ex)
            {
            }

            //}

            //else
            //{
            //    string msgText = "";
            //    msgText = "Are you sure you want to save the Patient Details ?";
            //    MessageBoxControl.MessageBoxChildWindow msgW =
            //          new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
            //    msgW.Show();
            //}
        }

        class AdultPatient
        {
            public bool IsAdult;
            public string msgAdult;
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.No && formSaveType == SaveType.OPD)  //Added by Ajit date 4/10/2016
            {
                formSaveType = SaveType.Registration;
            }

            if (Sponsortwin != null && Sponsortwin.cmbPatientCategory.SelectedItem == null)
            {
                PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Sponsor;
            }
            if (result == MessageBoxResult.Yes)
                SavePatient();
            else
            {
                ClickedFlag = 0;
                if (Indicatior == null) Indicatior = new WaitIndicator();
                Indicatior.Close();
            }

        }

        WaitIndicator Indicatior = new WaitIndicator();
        private void SavePatient()
        {

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

            if (cmbNationality.SelectedItem != null)
                BizAction.PatientDetails.NationalityID = ((MasterListItem)cmbNationality.SelectedItem).ID;

            if (cmbPreferredLanguage.SelectedItem != null)
                BizAction.PatientDetails.PreferredLangID = ((MasterListItem)cmbPreferredLanguage.SelectedItem).ID;

            if (cmbTreatmentRequired.SelectedItem != null)
                BizAction.PatientDetails.TreatRequiredID = ((MasterListItem)cmbTreatmentRequired.SelectedItem).ID;

            if (cmbEducation.SelectedItem != null)
                BizAction.PatientDetails.EducationID = ((MasterListItem)cmbEducation.SelectedItem).ID;

            if (chkIfYesrdo.IsChecked == true)
                BizAction.PatientDetails.IsClinicVisited = true;
            else
                BizAction.PatientDetails.IsClinicVisited = false;

            if (dtpMarriageAnnDate.SelectedDate != null)
                BizAction.PatientDetails.MarriageAnnDate = dtpMarriageAnnDate.SelectedDate;

            if (cmbSpecialReg.SelectedItem != null)
                BizAction.PatientDetails.SpecialRegID = ((MasterListItem)cmbSpecialReg.SelectedItem).ID;

            //added by neena
            BizAction.PatientDetails.OldRegistrationNo = txtOldRegistrationNo.Text.Trim();
            //

            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;

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

            if (cmbPatientSource.SelectedItem != null) // Changed by Saily P for SFC
                BizAction.PatientDetails.GeneralDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

            //* Added by - Ajit Jadhav
            //* Added Date - 5/8/2016
            //* Comments - Save Data To T_Registration

            if (PatientEditMode == true)
            {
                if (cmbBD.SelectedItem != null)
                    BizAction.PatientDetails.BDID = ((MasterListItem)cmbBD.SelectedItem).ID;
            }
            else
            {
                BizAction.PatientDetails.BDID = BDMID; //***//
            }

            //* Added Date - 11/8/2016
            if (!string.IsNullOrEmpty(txtPanNo.Text.Trim()))
                BizAction.PatientDetails.PanNumber = txtPanNo.Text.Trim().ToUpper();


            if (cmbCampDetail.SelectedItem != null)
                BizAction.PatientDetails.CampID = ((MasterListItem)cmbCampDetail.SelectedItem).ID;

            if (cmbAgentType.SelectedItem != null)
                BizAction.PatientDetails.AgentID = ((MasterListItem)cmbAgentType.SelectedItem).ID;

            if (cmbAgencyName.SelectedItem != null)
                BizAction.PatientDetails.AgencyID = ((MasterListItem)cmbAgencyName.SelectedItem).ID;


            if (!string.IsNullOrEmpty(txtNoOfYearsOfMarriage.Text.Trim()))
                BizAction.PatientDetails.NoOfYearsOfMarriage = Convert.ToInt64(txtNoOfYearsOfMarriage.Text);

            if (!string.IsNullOrEmpty(txtNoOfExistingChildren.Text.Trim()))
                BizAction.PatientDetails.NoOfExistingChildren = Convert.ToInt64(txtNoOfExistingChildren.Text);

            if (cmbFamilyType.SelectedItem != null)
            {
                BizAction.PatientDetails.FamilyTypeID = ((MasterListItem)cmbFamilyType.SelectedItem).ID;
            }

            //if (chkIfYesJoint.IsChecked == true)
            //   BizAction.PatientDetails.IsJointOrNuclear = true;
            //else
            //     BizAction.PatientDetails.IsJointOrNuclear = false;

            //***// ----------------------            

            //BizAction.PatientDetails.GeneralDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;

            BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.ResiNoCountryCode = Convert.ToInt64(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.PatientDetails.ResiSTDCode = Convert.ToInt64(txtResiSTD.Text.Trim());
            // BY BHUSHAN
            if (cmbReferralName.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;
            if (InhouseDoc.IsChecked == true)
                BizAction.PatientDetails.GeneralDetails.IsReferralDoc = false;
            else
                BizAction.PatientDetails.GeneralDetails.IsReferralDoc = true;

            if (cmbReferralDoctor.SelectedItem != null)
                BizAction.PatientDetails.GeneralDetails.ReferralDoctorID = ((MasterListItem)cmbReferralDoctor.SelectedItem).ID;

            if (cmbCoConsultantDoctor.SelectedItem != null) //***//
                BizAction.PatientDetails.GeneralDetails.CoConsultantDoctorID = ((MasterListItem)cmbCoConsultantDoctor.SelectedItem).ID;

            //if (cmbReferralTo.SelectedItem != null)
            //    BizAction.PatientDetails.GeneralDetails.ReferredToDoctorID = ((MasterListItem)cmbReferralTo.SelectedItem).ID;
            if (!string.IsNullOrEmpty(txtReferralDetail.Text.Trim()))
                BizAction.PatientDetails.GeneralDetails.ReferralDetail = txtReferralDetail.Text.Trim();

            //BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
            BizAction.PatientDetails.GeneralDetails.CompanyName = txtOfficeName.Text.Trim();

            //By Anjali.........................

            //commented by neena
            if (txtCountry.SelectedItem != null)
                BizAction.PatientDetails.CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;

            if (txtState.SelectedItem != null)
                BizAction.PatientDetails.StateID = ((MasterListItem)txtState.SelectedItem).ID;

            if (txtCity.SelectedItem != null)
                BizAction.PatientDetails.CityID = ((MasterListItem)txtCity.SelectedItem).ID;

            if (txtArea.SelectedItem != null)
                BizAction.PatientDetails.RegionID = ((MasterListItem)txtArea.SelectedItem).ID;
            //commented by neena

            if (cmbIdentity.SelectedItem != null)
                BizAction.PatientDetails.IdentityID = ((MasterListItem)cmbIdentity.SelectedItem).ID;

            if (!string.IsNullOrEmpty(txtIdentityNumber.Text.Trim()))
                BizAction.PatientDetails.IdentityNumber = txtIdentityNumber.Text.Trim();

            if (cmbPreffix.SelectedItem != null)
                BizAction.PatientDetails.PrefixId = ((MasterListItem)cmbPreffix.SelectedItem).ID;

            //if (!string.IsNullOrEmpty(txtRemark.Text.Trim()))
            //    BizAction.PatientDetails.RemarkForPatientType = txtRemark.Text.Trim();

            //if (!string.IsNullOrEmpty(txtEducation.Text.Trim()))
            //    BizAction.PatientDetails.Education = txtEducation.Text.Trim();

            if (ChkInternationalPatient.IsChecked == true)
                BizAction.PatientDetails.IsInternationalPatient = true;
            else
                BizAction.PatientDetails.IsInternationalPatient = false;


            #region For Pediatric Flow

            if (cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID == 13)  // AppConfig.  M_PatientCategoryMaster= Couple
            {
                if (!string.IsNullOrEmpty(txtBabyNo.Text.Trim()))
                    BizAction.PatientDetails.BabyNo = Convert.ToInt32(txtBabyNo.Text.Trim());

                if (!string.IsNullOrEmpty(txtBabyNoOfNo.Text.Trim()))
                    BizAction.PatientDetails.BabyOfNo = Convert.ToInt32(txtBabyNoOfNo.Text.Trim());

                if (!string.IsNullOrEmpty(txtBirthWeight.Text.Trim()))
                    BizAction.PatientDetails.BabyWeight = txtBirthWeight.Text.Trim();

                TimeSpan time;
                if (TimeSpan.TryParse(txtBirthTime.Value.Value.TimeOfDay.ToString(), out time) && BizAction.PatientDetails != null && BizAction.PatientDetails.GeneralDetails != null && BizAction.PatientDetails.GeneralDetails.DateOfBirth != null)
                    BizAction.PatientDetails.GeneralDetails.DateOfBirth = BizAction.PatientDetails.GeneralDetails.DateOfBirth.Value.Add(time);

                BizAction.PatientDetails.LinkPatientID = LinkPatientID;       // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.ID;
                BizAction.PatientDetails.LinkPatientUnitID = LinkPatientUnitID;  // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.UnitId;
                BizAction.PatientDetails.LinkPatientMrNo = txtMRNumberParent.Text;

            }

            #endregion

            //...................................

            //Added by neena
            //commented by neena
            //if (txtCountry1.Text != string.Empty)
            //    BizAction.PatientDetails.CountryID = ((clsPatientVO)this.DataContext).CountryID;

            //if (txtState1.Text != string.Empty)
            //    BizAction.PatientDetails.StateID = ((clsPatientVO)this.DataContext).StateID;

            //if (txtCity1.Text != string.Empty)
            //    BizAction.PatientDetails.CityID = ((clsPatientVO)this.DataContext).CityID;

            //if (txtArea1.Text != string.Empty)
            //    BizAction.PatientDetails.RegionID = ((clsPatientVO)this.DataContext).RegionID;
            //
            //




            //======================================================
            //Spouse
            if (!PatientEditMode)
            {
                if (((MasterListItem)cmbPatientType.SelectedItem).ID == 8 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 9)
                    dtpSpouseDOB.SelectedDate = dtpSpouseDOB.DisplayDate;
                if (dtpSpouseDOB.SelectedDate == null)
                {
                    BizAction.PatientDetails.SpouseDetails.IsAge = true;
                    if (SpouseDOB != null)
                        BizAction.PatientDetails.SpouseDetails.DateOfBirth = SpouseDOB.Value.Date;
                }

                if (cmbSpouseNationality.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.NationalityID = ((MasterListItem)cmbSpouseNationality.SelectedItem).ID;

                if (cmbSpousePreferredLanguage.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.PreferredLangID = ((MasterListItem)cmbSpousePreferredLanguage.SelectedItem).ID;

                if (cmbSpouseSpecialReg.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.SpecialRegID = ((MasterListItem)cmbSpouseSpecialReg.SelectedItem).ID;

                //if (cmbSpouseTreatmentRequired.SelectedItem != null)
                //    BizAction.PatientDetails.SpouseDetails.TreatRequiredID = ((MasterListItem)cmbSpouseTreatmentRequired.SelectedItem).ID;

                if (cmbSpouseEducation.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.EducationID = ((MasterListItem)cmbSpouseEducation.SelectedItem).ID;

                //if (chkSpouseIfYes.IsChecked == true)
                //    BizAction.PatientDetails.SpouseDetails.IsClinicVisited = true;
                //else
                //    BizAction.PatientDetails.SpouseDetails.IsClinicVisited = false;

                if (dtpSpouseMarriageAnnDate.SelectedDate != null)
                    BizAction.PatientDetails.SpouseDetails.MarriageAnnDate = dtpSpouseMarriageAnnDate.SelectedDate;

                //added by neena
                BizAction.PatientDetails.SpouseDetails.SpouseOldRegistrationNo = txtSpouseOldRegistrationNo.Text.Trim();
                //

                if (cmbSpouseMaritalStatus.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.MaritalStatusID = ((MasterListItem)cmbSpouseMaritalStatus.SelectedItem).ID;
                if (cmbSpouseBloodGroup.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.BloodGroupID = ((MasterListItem)cmbSpouseBloodGroup.SelectedItem).ID;
                // Modified By Yogita
                if (cmbSpouseGender.SelectedItem != null || cmbGender.SelectedItem != cmbSpouseGender.SelectedItem)
                    BizAction.PatientDetails.SpouseDetails.GenderID = ((MasterListItem)cmbSpouseGender.SelectedItem).ID;

                else
                {
                    cmbSpouseGender.SetValidation("Check Gender!");
                    cmbSpouseGender.RaiseValidationError();
                    cmbSpouseGender.Focus();
                }
                // End
                if (cmbSpouseReligion.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.ReligionID = ((MasterListItem)cmbSpouseReligion.SelectedItem).ID;

                if (cmbSpouseOccupation.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.OccupationId = ((MasterListItem)cmbSpouseOccupation.SelectedItem).ID;

                BizAction.PatientDetails.SpouseDetails.ContactNo1 = txtSpouseContactNo1.Text.Trim();
                BizAction.PatientDetails.SpouseDetails.ContactNo2 = txtSpouseContactNo2.Text.Trim();
                if (!string.IsNullOrEmpty(txtSpouseMobileCountryCode.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.MobileCountryCode = txtSpouseMobileCountryCode.Text.Trim();

                if (!string.IsNullOrEmpty(txtSpouseResiCountryCode.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.ResiNoCountryCode = Convert.ToInt64(txtSpouseResiCountryCode.Text.Trim());

                if (!string.IsNullOrEmpty(txtSpouseResiSTD.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.ResiSTDCode = Convert.ToInt64(txtSpouseResiSTD.Text.Trim());

                // By BHUSHAN . . '
                BizAction.PatientDetails.SpouseDetails.Pincode = txtSpousePinCode.Text.Trim();

                if (cmbReferralName.SelectedItem != null)
                    BizAction.PatientDetails.GeneralDetails.ReferralTypeID = ((MasterListItem)cmbReferralName.SelectedItem).ID;

                //BizAction.PatientDetails.CompanyName = txtSpouseOfficeName.Text.Trim();
                BizAction.PatientDetails.SpouseDetails.CompanyName = txtSpouseOfficeName.Text.Trim();
                //By Anjali.........................
                //commented by neena
                if (txtSpouseCountry.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.CountryID = ((MasterListItem)txtSpouseCountry.SelectedItem).ID;

                if (txtSpouseState.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.StateID = ((MasterListItem)txtSpouseState.SelectedItem).ID;

                if (txtSpouseCity.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.CityID = ((MasterListItem)txtSpouseCity.SelectedItem).ID;

                if (txtSpouseArea.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.RegionID = ((MasterListItem)txtSpouseArea.SelectedItem).ID;
                //commented by neena


                if (cmbSpouseIdentity.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.IdentityID = ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID;

                if (!string.IsNullOrEmpty(txtSpouseIdentityNumber.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.IdentityNumber = txtSpouseIdentityNumber.Text.Trim();

                if (cmbSpousePreffix.SelectedItem != null)
                    BizAction.PatientDetails.SpouseDetails.PrefixId = ((MasterListItem)cmbSpousePreffix.SelectedItem).ID;

                //if (!string.IsNullOrEmpty(txtSpouseRemark.Text.Trim()))
                //    BizAction.PatientDetails.SpouseDetails.RemarkForPatientType = txtSpouseRemark.Text.Trim();

                //if (!string.IsNullOrEmpty(txtSpouseEducation.Text.Trim()))
                //    BizAction.PatientDetails.SpouseDetails.Education = txtSpouseEducation.Text.Trim();

                if (ChkSpouseInternationalPatient.IsChecked == true)
                    BizAction.PatientDetails.SpouseDetails.IsInternationalPatient = true;
                else
                    BizAction.PatientDetails.SpouseDetails.IsInternationalPatient = false;

                //* Added by Ajit Jadhav Date - 11/8/2016 
                if (!string.IsNullOrEmpty(txtSpousePanNo.Text.Trim()))
                    BizAction.PatientDetails.SpouseDetails.SpousePanNumber = txtSpousePanNo.Text.Trim().ToUpper();


                //added by neena
                //commented by neena
                //if (txtSpouseCountry1.Text != string.Empty)
                //    BizAction.PatientDetails.SpouseDetails.CountryID = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;

                //if (txtSpouseState1.Text != string.Empty)
                //    BizAction.PatientDetails.SpouseDetails.StateID = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;

                //if (txtSpouseCity1.Text != string.Empty)
                //    BizAction.PatientDetails.SpouseDetails.CityID = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;

                //if (txtSpouseArea1.Text != string.Empty)
                //    BizAction.PatientDetails.SpouseDetails.RegionID = ((clsPatientVO)this.DataContext).RegionID;
                //
                //

                //======================================================

                // Added By CDS
                if (sponsorContent.Content != null && PatientEditMode == false)
                {

                    BizAction.IsSaveSponsor = true;
                    BizAction.BizActionVOSaveSponsor = SaveSponsorWithTransaction();
                    if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 8 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 9)  // AppConfig.  M_PatientCategoryMaster= Couple
                    {
                        BizAction.BizActionVOSaveSponsorForMale = SaveSponsorWithTransactionMale();
                    }
                }

            }

            if (sponsorContent.Content != null && ((MasterListItem)Sponsortwin.cmbBankName.SelectedItem) != null) //***//
            {
                BizAction.BankDetails = SaveBankWithTransaction();
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

                    //added By Akshays on 18/11/2015
                    ////////string a, b;
                    ////////a = ((clsPatientVO)this.DataContext).GeneralDetails.MRNo;
                    ////////b = ((clsPatientVO)this.DataContext).SpouseDetails.MRNo;

                    ////////if (a != null && a != "" && b != null && b != "")
                    ////////{
                    ////////    Barcodesaveboth(a, b);
                    ////////}
                    ////////else
                    ////////{
                    ////////    Barcodesave(a);
                    ////////}

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
                            //added by neena for surrogacy
                            if (Flagref == true)
                                ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy"); //
                            else
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
                            {
                                visitwin.CompanyID = ((MasterListItem)Sponsortwin.cmbCompany.SelectedItem).ID;
                            }
                            else
                            {
                                visitwin.CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                            }


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

                        #region For Pediatric Flow

                        ClearPediatricData();

                        #endregion

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

        private void ClearPediatricData()
        {
            try
            {
                txtBabyNo.Text = "";
                txtBabyNoOfNo.Text = "";
                txtBirthWeight.Text = "";
                txtBirthTime.Value = null;
                txtMRNumberParent.Text = "";
                txtParentName.Text = "";

                LinkPatientID = 0;
                LinkPatientUnitID = 0;
            }
            catch (Exception)
            {
                //throw;
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
                            //if (((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID != null)
                            //{
                            //    ((clsPatientSponsorVO)Sponsortwin.DataContext).ID = ((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID;
                            //    //System.Windows.Browser.HtmlPage.Window.Alert("Sponsor Saved Successfully with ID " + ((clsPatientSponsorVO)this.DataContext).SponsorID);

                            //}
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
                // this.DataContext = Win.ObjPatient;  
                //((IApplicationConfiguration)App.Current).SelectedPatient = Win.ObjPatient.GeneralDetails;
                //Initiate("EDIT"); 
                //clsPatientGeneralVO tempPatient = Win.ObjPatient; 
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
            ((clsPatientVO)this.DataContext).FamilyName = txtLastName.Text; //((clsPatientVO)this.DataContext).GeneralDetails.LastName;
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
                    //  if (!((WaterMarkTextbox)sender).Text.IsMobileNumberValid() && textBefore != null)
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

                    //Added by ajit date 6/10/2016
                    ((clsPatientVO)this.DataContext).NationalityID = mObj.AppointmentDetails.NationalityId;

                    //***//------------------

                    //((clsPatientVO)this.DataContext).ContactNo1 = mObj.AppointmentDetails.ContactNo1;
                    //((clsPatientVO)this.DataContext).ContactNo2 = mObj.AppointmentDetails.ContactNo2;

                    //by Anjali...........................
                    ((clsPatientVO)this.DataContext).SpecialRegID = mObj.AppointmentDetails.SpecialRegistrationID;
                    cmbSpecialReg.SelectedValue = ((clsPatientVO)this.DataContext).SpecialRegID;
                    //......................................
                    txtResiCountryCode.Text = mObj.AppointmentDetails.ResiNoCountryCode.ToString();
                    txtResiSTD.Text = mObj.AppointmentDetails.ResiSTDCode.ToString();
                    txtMobileCountryCode.Text = mObj.AppointmentDetails.MobileCountryCode;

                    txtContactNo1.Text = mObj.AppointmentDetails.ContactNo1;
                    txtContactNo2.Text = mObj.AppointmentDetails.ContactNo2;
                    txtEmail.Text = mObj.AppointmentDetails.Email;
                    cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                    cmbBloodGroup.SelectedValue = ((clsPatientVO)this.DataContext).BloodGroupID;
                    cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                    cmbReligion.SelectedValue = ((clsPatientVO)this.DataContext).ReligionID;
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


            if (cmbPatientType.SelectedItem != null)
                SetPatientCategorywiseDefaults(((MasterListItem)cmbPatientType.SelectedItem).ID);
            if (this.DataContext != null && IsfromEdit != true)
                ((clsPatientVO)this.DataContext).ScanDocList = new List<clsPatientScanDocumentVO>();

            if (objList != null)
            {
                if (((MasterListItem)cmbPatientType.SelectedItem).ID != 7)
                {
                    cmbPreffix.SelectedItem = objList[0];
                    cmbSpousePreffix.SelectedItem = objList[0];
                }
            }

            if (objList != null)
            {
                if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 8 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 9)
                {
                    //cmbPreffix.SelectedItem = objList[22];
                    //cmbSpousePreffix.SelectedItem = objList[21];
                    //cmbPreffix.SelectedItem = objList[3];
                    //cmbSpousePreffix.SelectedItem = objList[2];
                }
                //if (((MasterListItem)cmbPatientType.SelectedItem).ID == 9)   //if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)
                //{
                //    cmbPreffix.SelectedItem = objList[2];
                //    cmbSpousePreffix.SelectedItem = objList[3];
                //}
            }

            if (PatientEditMode == false && cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID == 13)
            {
                //cmbPreffix.SelectedValue = 11;

                if (objList != null)
                {
                    cmbPreffix.SelectedValue = 0;
                    cmbPreffix.SelectedItem = objList[6];
                    //cmbSpousePreffix.SelectedItem = objList[21];
                }
                cmbPreffix.Focus();
            }

            //***//-------
            if (cmbPatientType.SelectedItem != null)
            {
                if (((MasterListItem)cmbPatientType.SelectedItem).ID == 8 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 9 || ((MasterListItem)cmbPatientType.SelectedItem).ID == 10)
                {
                    cmbAgentType.IsEnabled = true;

                }
                else
                {
                    cmbAgentType.IsEnabled = false;
                    FillAgentMaster();
                }
            }
          
            if (cmbPatientType.SelectedItem != null)
            {
                if (((MasterListItem)cmbPatientType.SelectedItem).ID == 10)
                {
                    cmbAgencyName.IsEnabled = true;

                }
                else
                {
                    cmbAgencyName.IsEnabled = false;
                    FillSurrogateAgency();
                }
            }


            //if (cmbPatientType.SelectedItem != null)
            //    SetPatientCategorywiseDefaults(((MasterListItem)cmbPatientType.SelectedItem).ID);
            //if (this.DataContext != null && IsfromEdit != true)
            //    ((clsPatientVO)this.DataContext).ScanDocList = new List<clsPatientScanDocumentVO>();

            //if (objList != null)
            //{
            //    if (((MasterListItem)cmbPatientType.SelectedItem).ID != 7)
            //    {
            //        cmbPreffix.SelectedItem = objList[0];
            //        cmbSpousePreffix.SelectedItem = objList[0];
            //    }
            //}

            //if (objList != null)
            //{
            //    if (((MasterListItem)cmbPatientType.SelectedItem).ID == 7)
            //    {
            //        cmbPreffix.SelectedItem = objList[22];
            //        cmbSpousePreffix.SelectedItem = objList[21];
            //    }
            //} 
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

                    if (this.DataContext != null && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Female)
                    {
                        // lblDaughterOf.Text = "Daughter of";
                    }
                    else if (this.DataContext != null && ((clsPatientVO)this.DataContext).GenderID == (long)Genders.Male)
                    {
                        // lblDaughterOf.Text = "Son of";
                    }

                    SetPediatricControl(false); // For Pediatric flow

                    break;
                //End

                case 8://Female Donor
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



                    //cmbGender.IsEnabled = false;
                    //if (!PatientEditMode)
                    //    tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;
                    //((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    //cmbGender.SelectedValue = (long)Genders.Female;
                    SetPediatricControl(false); // For Pediatric flow
                    break;

                case 9://Male Donor
                    tabPatSponsorInfo.Visibility = System.Windows.Visibility.Collapsed;
                    if (!PatientEditMode)
                    {
                        //tabPatEncounterInfo.Visibility = System.Windows.Visibility.Collapsed;                       
                        tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;
                        tabPatSponsorInfo.Visibility = System.Windows.Visibility.Visible;

                        ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                        cmbGender.SelectedValue = (long)Genders.Male;
                        cmbSpouseGender.SelectedValue = (long)Genders.Female;
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

                    //cmbGender.IsEnabled = false;
                    //if (!PatientEditMode)
                    //    tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;
                    //((clsPatientVO)this.DataContext).GenderID = (long)Genders.Male;
                    //cmbGender.SelectedValue = (long)Genders.Male;
                    SetPediatricControl(false); // For Pediatric flow
                    break;

                case 10://Surrogacy
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    cmbGender.SelectedValue = (long)Genders.Female;
                    SetPediatricControl(false); // For Pediatric flow
                    break;


                // BY ANJALI FOR ANC . . . . 
                case 12: // ANC
                    cmbGender.IsEnabled = false;
                    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                    cmbGender.SelectedValue = (long)Genders.Female;
                    SetPediatricControl(false); // For Pediatric flow
                    break;
                //tabPatSponsorInfo.Visibility = System.Windows.Visibility.Collapsed;
                //if (!PatientEditMode)
                //{
                //    //tabPatEncounterInfo.Visibility = System.Windows.Visibility.Collapsed;                       
                //    tabPatSpouseInformation.Visibility = System.Windows.Visibility.Visible;

                //    ((clsPatientVO)this.DataContext).GenderID = (long)Genders.Female;
                //    cmbGender.SelectedValue = (long)Genders.Female;
                //    cmbSpouseGender.SelectedValue = (long)Genders.Male;
                //}
                //cmbGender.IsEnabled = false;
                //cmbSpouseGender.IsEnabled = false;



                //default:
                //    break;
                case 13:    // For Pediatric flow

                    SetPediatricControl(true);
                    break;

                default:
                    SetPediatricControl(false); // For Pediatric flow
                    break;
            }

        }

        private void SetPediatricControl(bool IsVisibleControl)    // For Pediatric flow
        {
            if (IsVisibleControl == true)
            {
                lblBaby.Visibility = Visibility.Visible;
                grdBaby.Visibility = Visibility.Visible;

                txtBabyNo.Visibility = Visibility.Visible;
                lblOf.Visibility = Visibility.Visible;
                txtBabyNoOfNo.Visibility = Visibility.Visible;

                lblBabyWeight.Visibility = Visibility.Visible;
                txtBirthWeight.Visibility = Visibility.Visible;
                lblBabyWeightKg.Visibility = Visibility.Visible;
                grdBabyWeight.Visibility = Visibility.Visible;

                lblBirthTime.Visibility = Visibility.Visible;
                txtBirthTime.Visibility = Visibility.Visible;

                lblParentName.Visibility = Visibility.Visible;
                txtParentName.Visibility = Visibility.Visible;
                txtMRNumberParent.Visibility = Visibility.Visible;
                btnSearchParent.Visibility = Visibility.Visible;
            }
            else
            {
                lblBaby.Visibility = Visibility.Collapsed;
                grdBaby.Visibility = Visibility.Collapsed;

                txtBabyNo.Visibility = Visibility.Collapsed;
                lblOf.Visibility = Visibility.Collapsed;
                txtBabyNoOfNo.Visibility = Visibility.Collapsed;

                lblBabyWeight.Visibility = Visibility.Collapsed;
                txtBirthWeight.Visibility = Visibility.Collapsed;
                lblBabyWeightKg.Visibility = Visibility.Collapsed;
                grdBabyWeight.Visibility = Visibility.Collapsed;

                lblBirthTime.Visibility = Visibility.Collapsed;
                txtBirthTime.Visibility = Visibility.Collapsed;

                lblParentName.Visibility = Visibility.Collapsed;
                txtParentName.Visibility = Visibility.Collapsed;
                txtMRNumberParent.Visibility = Visibility.Collapsed;
                btnSearchParent.Visibility = Visibility.Collapsed;
            }

            #region For Pediatric Flow

            if (IsVisibleControl == true && PatientEditMode == false) //cmbPatientType.SelectedItem != null && ((MasterListItem)(cmbPatientType.SelectedItem)).ID == 13
            {

                if (string.IsNullOrEmpty(txtBabyNo.Text))
                {
                    txtBabyNo.SetValidation("Baby No Is Required");
                    txtBabyNo.RaiseValidationError();
                    txtBabyNo.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    //result = false;
                }
                else
                    txtBabyNo.ClearValidationError();

                if (string.IsNullOrEmpty(txtBabyNoOfNo.Text))
                {
                    txtBabyNoOfNo.SetValidation("Baby Of No Is Required");
                    txtBabyNoOfNo.RaiseValidationError();
                    txtBabyNoOfNo.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    //result = false;
                }
                else
                    txtBabyNoOfNo.ClearValidationError();

                if (string.IsNullOrEmpty(txtBirthWeight.Text))
                {
                    txtBirthWeight.SetValidation("Birth Weight Is Required");
                    txtBirthWeight.RaiseValidationError();
                    txtBirthWeight.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    //result = false;
                }
                else
                    txtBirthWeight.ClearValidationError();

                if (string.IsNullOrEmpty(txtBirthTime.Value.ToString()))
                {
                    txtBirthTime.SetValidation("Birth Time Is Required");
                    txtBirthTime.RaiseValidationError();
                    txtBirthTime.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    //result = false;
                }
                else
                    txtBirthTime.ClearValidationError();


                if (string.IsNullOrEmpty(txtParentName.Text))
                {
                    txtParentName.SetValidation("Parent Name Is Required");
                    txtParentName.RaiseValidationError();
                    txtParentName.Focus();
                    //PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                    //result = false;
                }
                else
                    txtParentName.ClearValidationError();

                //if (PatientEditMode == false && cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID == 13)
                //{
                //    //cmbPreffix.SelectedValue = 11;

                //    if (objList != null)
                //    {
                //        cmbPreffix.SelectedValue = 0;
                //        cmbPreffix.SelectedItem = objList[6];
                //        //cmbSpousePreffix.SelectedItem = objList[21];
                //    }
                //    cmbPreffix.Focus();
                //}
            }
            else
            {
                txtBabyNo.ClearValidationError();
                txtBabyNoOfNo.ClearValidationError();
                txtBirthWeight.ClearValidationError();
                txtBirthTime.ClearValidationError();
                txtParentName.ClearValidationError();
            }

            #endregion
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
            //if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth > DateTime.Now)
            //{
            //    //txtSpouseYY.SetValidation("Age Is Required");
            //    //txtSpouseYY.RaiseValidationError();
            //    //txtSpouseMM.SetValidation("Age Is Required");
            //    //txtSpouseMM.RaiseValidationError();
            //    //txtSpouseDD.SetValidation("Age Is Required");
            //    //txtSpouseDD.RaiseValidationError();

            //    dtpSpouseDOB.SetValidation("Date of Birth Can Not Be Greater Than Today");
            //    dtpSpouseDOB.RaiseValidationError();
            //    txtSpouseYY.Text = "0";
            //    txtSpouseMM.Text = "0";
            //    txtSpouseDD.Text = "0";
            //}
            //else if (((clsPatientVO)this.DataContext).SpouseDetails.DateOfBirth == null)
            //{
            //    //txtSpouseYY.SetValidation("Age Is Required");
            //    //txtSpouseYY.RaiseValidationError();
            //    //txtSpouseMM.SetValidation("Age Is Required");
            //    //txtSpouseMM.RaiseValidationError();
            //    //txtSpouseDD.SetValidation("Age Is Required");
            //    //txtSpouseDD.RaiseValidationError();

            //    dtpSpouseDOB.SetValidation("Please Select The Date of Birth");
            //    dtpSpouseDOB.RaiseValidationError();
            //    txtYY.Text = "0";
            //    txtMM.Text = "0";
            //    txtDD.Text = "0";
            //}
            //else
            //{
            //    dtpSpouseDOB.ClearValidationError();
            //    txtSpouseYY.ClearValidationError();
            //    txtSpouseMM.ClearValidationError();
            //    txtSpouseDD.ClearValidationError();

            //    txtSpouseYY.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "YY");
            //    txtSpouseMM.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "MM");
            //    txtSpouseDD.Text = ConvertDate(dtpSpouseDOB.SelectedDate, "DD");
            //}


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

        //..................................................

        public void FillCountry()
        {
            WaitIndicator IndicatiorFillCountry = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)         //if (IsSetWetIndicator == true ) 
            {
                IndicatiorFillCountry.Show();
            }
            else if (IndicatiorFromLoad == true)
            {
                IndicatiorFillFromLoad.Show();
            }

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
                        // txtCountry.SelectedItem = objList[0];

                        txtSpouseCountry.ItemsSource = null;
                        txtSpouseCountry.ItemsSource = objList.DeepCopy();
                    }

                    if (this.DataContext != null)
                    {
                        if (((MasterListItem)txtCountry.SelectedItem).ID != ((clsPatientVO)this.DataContext).CountryID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                        if (((MasterListItem)txtSpouseCountry.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.CountryID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;



                        //added by neena
                        //commented by neena
                        //else if ((SpouseClick == true))
                        //    txtSpouseCountry.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CountryID;
                        //
                        //
                    }

                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)      //  if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillCountry.Close();
                    }
                    else if (IndicatiorFromLoad == true)
                    {
                        IndicatiorFillFromLoad.Close();
                    }

                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillCountry.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
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
                        if (((MasterListItem)txtCountry.SelectedItem).ID != ((clsPatientVO)this.DataContext).CountryID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            txtCountry.SelectedValue = ((clsPatientVO)this.DataContext).CountryID;
                        if (((MasterListItem)txtSpouseCountry.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.CountryID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
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
                        if (((MasterListItem)txtState.SelectedItem).ID != ((clsPatientVO)this.DataContext).StateID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;
                        if (((MasterListItem)txtSpouseState.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.StateID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                    }
                    FillCity(StateID, CityID, RegionID);
                    //  GetBdList();//Added BY YK 3 2 17
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
                        if (((MasterListItem)txtCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                        if (((MasterListItem)txtSpouseCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                    }
                    // FillRegion(CityID);
                }
            };

            client.ProcessAsync(BizAction, new clsUserVO());
            client.CloseAsync();
        }
        public void FillState(long CountryID)
        {
            WaitIndicator IndicatiorFillState = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   // if (IsSetWetIndicator == true)
            {
                IndicatiorFillState.Show();
            }

            try
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
                            if (((MasterListItem)txtState.SelectedItem).ID != ((clsPatientVO)this.DataContext).StateID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                                txtState.SelectedValue = ((clsPatientVO)this.DataContext).StateID;

                            if (((MasterListItem)txtSpouseState.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.StateID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                                txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;

                            //added by neena
                            //commented by neena
                            //else if ((SpouseClick == true))
                            //    txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;
                            //
                            //

                        }
                        else
                        {
                            txtState.SelectedItem = objM;
                            txtSpouseState.SelectedItem = objM;
                        }

                    }

                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   // if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillState.Close();
                    }

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillState.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }

        //added by neena
        public void FillStateForSpouse(long CountryID)
        {
            WaitIndicator IndicatiorFillState = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
            {
                IndicatiorFillState.Show();
            }

            try
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

                        txtSpouseState.ItemsSource = null;
                        txtSpouseState.ItemsSource = objList.DeepCopy();

                        if (this.DataContext != null)
                        {

                            if (((MasterListItem)txtSpouseState.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.StateID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                                txtSpouseState.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.StateID;

                        }
                        else
                        {
                            txtSpouseState.SelectedItem = objM;
                        }

                    }

                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillState.Close();
                    }

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillState.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }
        //

        public void FillCity(long StateID)
        {
            WaitIndicator IndicatiorFillCity = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
            {
                IndicatiorFillCity.Show();
            }

            try
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
                            if (((MasterListItem)txtCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                                txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                            if (((MasterListItem)txtSpouseCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                                txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                            //added by neena
                            //commented by neena
                            //else if ((SpouseClick == true))
                            //    txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                            //
                            //
                        }
                        else
                        {
                            txtCity.SelectedItem = objM;
                            txtSpouseCity.SelectedItem = objM;
                        }

                    }

                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillCity.Close();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillCity.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }

        //added by neena
        public void FillCityForSpouse(long StateID)
        {
            WaitIndicator IndicatiorFillCity = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
            {
                IndicatiorFillCity.Show();
            }

            try
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
                        //txtCity.ItemsSource = null;
                        //txtCity.ItemsSource = objList.DeepCopy();

                        //if (!SpouseClick)    //added by neena
                        //{
                        txtSpouseCity.ItemsSource = null;
                        txtSpouseCity.ItemsSource = objList.DeepCopy();
                        //}

                        if (this.DataContext != null)
                        {
                            //if (((MasterListItem)txtCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            //    txtCity.SelectedValue = ((clsPatientVO)this.DataContext).CityID;
                            //if (!SpouseClick)    //added by neena
                            //{
                            if (((MasterListItem)txtSpouseCity.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.CityID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                                txtSpouseCity.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.CityID;
                            //}
                        }
                        else
                        {
                            //txtCity.SelectedItem = objM;
                            txtSpouseCity.SelectedItem = objM;
                        }

                    }

                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillCity.Close();
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillCity.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }
        //

        List<clsRegionForRegVO> RegionList;     //List<clsRegionVO> RegionList;
        public void FillRegion(long CityID)
        {
            WaitIndicator IndicatiorFillRegion = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
            {
                IndicatiorFillRegion.Show();
            }

            try
            {

                clsGetRegionDetailsByCityIDForRegBizActionVO BizAction = new clsGetRegionDetailsByCityIDForRegBizActionVO();    //clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
                BizAction.CityId = CityID;
                BizAction.ListRegionDetails = new List<clsRegionForRegVO>();    //BizAction.ListRegionDetails = new List<clsRegionVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDForRegBizActionVO)args.Result).ListRegionDetails;    //BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                        List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        if (BizAction.ListRegionDetails != null)
                        {
                            if (BizAction.ListRegionDetails.Count > 0)
                            {
                                RegionList = new List<clsRegionForRegVO>();     //RegionList = new List<clsRegionVO>();
                                RegionList = BizAction.ListRegionDetails;   //RegionList = BizAction.ListRegionDetails;
                                foreach (clsRegionForRegVO item in BizAction.ListRegionDetails)     //foreach (clsRegionVO item in BizAction.ListRegionDetails)
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
                            if (((MasterListItem)txtArea.SelectedItem).ID != ((clsPatientVO)this.DataContext).RegionID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            {
                                txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
                            }

                            if (((MasterListItem)txtSpouseArea.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.RegionID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            {
                                txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                            }
                            //added by neena
                            //commented by neena
                            //else if ((SpouseClick == true))
                            //    txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                            //
                            //

                            if (txtPinCode.Text != ((clsPatientVO)this.DataContext).Pincode && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            {
                                txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
                            }

                            if (txtSpousePinCode.Text != ((clsPatientVO)this.DataContext).SpouseDetails.Pincode && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            {
                                txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
                            }
                            //added by neena
                            //    commented by neena
                            //else if ((SpouseClick == true))
                            //    txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
                            //
                            //
                        }
                        else
                        {
                            txtArea.SelectedItem = objM;
                            txtSpouseArea.SelectedItem = objM;

                        }
                    }

                    if (IsSetWetIndicator == true)
                    {
                        if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                        {
                            IndicatiorFillRegion.Close();
                        }
                        if (IndicatiorFromLoad == true)   //if (IsSetWetIndicator == true)
                        {
                            IndicatiorFillFromLoad.Close();
                            IndicatiorFromLoad = false;
                        }
                    }


                    //   GetBdList();//Added By YK  3 2 17

                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();


                if (PatientEditMode == false)
                {
                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillRegion.Close();
                    }
                    if (IndicatiorFromLoad == true)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillFromLoad.Close();
                        IndicatiorFromLoad = false;
                    }
                }

            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillRegion.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }

        //added by neena
        List<clsRegionForRegVO> RegionList1;     //List<clsRegionVO> RegionList1;
        public void FillRegionForSpouse(long CityID)
        {
            WaitIndicator IndicatiorFillRegion = new WaitIndicator();

            if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
            {
                IndicatiorFillRegion.Show();
            }

            try
            {

                clsGetRegionDetailsByCityIDForRegBizActionVO BizAction = new clsGetRegionDetailsByCityIDForRegBizActionVO();    //clsGetRegionDetailsByCityIDBizActionVO BizAction = new clsGetRegionDetailsByCityIDBizActionVO();
                BizAction.CityId = CityID;
                BizAction.ListRegionDetails = new List<clsRegionForRegVO>();    //BizAction.ListRegionDetails = new List<clsRegionVO>();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDForRegBizActionVO)args.Result).ListRegionDetails;    //BizAction.ListRegionDetails = ((clsGetRegionDetailsByCityIDBizActionVO)args.Result).ListRegionDetails;
                        List<MasterListItem> objList = new List<MasterListItem>();      //List<MasterListItem> objList = new List<MasterListItem>();
                        MasterListItem objM = new MasterListItem(0, "-- Select --");
                        objList.Add(objM);
                        if (BizAction.ListRegionDetails != null)
                        {
                            if (BizAction.ListRegionDetails.Count > 0)
                            {
                                RegionList1 = new List<clsRegionForRegVO>();     //RegionList1 = new List<clsRegionVO>();
                                RegionList1 = BizAction.ListRegionDetails;   //RegionList1 = BizAction.ListRegionDetails;
                                foreach (clsRegionForRegVO item in BizAction.ListRegionDetails)     //foreach (clsRegionVO item in BizAction.ListRegionDetails)
                                {
                                    MasterListItem obj = new MasterListItem();
                                    obj.ID = item.Id;
                                    obj.Description = item.Description;
                                    objList.Add(obj);
                                }
                            }
                        }
                        //txtArea.ItemsSource = null;
                        //txtArea.ItemsSource = objList.DeepCopy();


                        //if (!SpouseClick)    //added by neena
                        //{
                        txtSpouseArea.ItemsSource = null;
                        txtSpouseArea.ItemsSource = objList.DeepCopy();
                        //}

                        if (this.DataContext != null)
                        {
                            //if (((MasterListItem)txtArea.SelectedItem).ID != ((clsPatientVO)this.DataContext).RegionID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            //{
                            //    txtArea.SelectedValue = ((clsPatientVO)this.DataContext).RegionID;
                            //}

                            //if (!SpouseClick)    //added by neena
                            //{
                            if (((MasterListItem)txtSpouseArea.SelectedItem).ID != ((clsPatientVO)this.DataContext).SpouseDetails.RegionID && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            {
                                txtSpouseArea.SelectedValue = ((clsPatientVO)this.DataContext).SpouseDetails.RegionID;
                            }
                            //}

                            //if (txtPinCode.Text != ((clsPatientVO)this.DataContext).Pincode && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Patient)
                            //{
                            //    txtPinCode.Text = ((clsPatientVO)this.DataContext).Pincode;
                            //}

                            //if (!SpouseClick)    //added by neena
                            //{
                            if (txtSpousePinCode.Text != ((clsPatientVO)this.DataContext).SpouseDetails.Pincode && PatPersonalInfo.SelectedIndex == (int)RegistrationTabs.Spouse)
                            {
                                txtSpousePinCode.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Pincode;
                            }
                            //}



                        }
                        else
                        {
                            //txtArea.SelectedItem = objM;
                            txtSpouseArea.SelectedItem = objM;

                        }
                    }

                    if (IsSetWetIndicator == true)
                    {
                        if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                        {
                            IndicatiorFillRegion.Close();
                        }

                        if (IndicatiorFromLoad == true)   //if (IsSetWetIndicator == true)
                        {
                            IndicatiorFillFromLoad.Close();
                            IndicatiorFromLoad = false;
                        }
                    }
                };

                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();

                if (PatientEditMode == false)
                {
                    if (IsSetWetIndicator == true && IndicatiorFromLoad == false)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillRegion.Close();
                    }

                    if (IndicatiorFromLoad == true)   //if (IsSetWetIndicator == true)
                    {
                        IndicatiorFillFromLoad.Close();
                        IndicatiorFromLoad = false;
                    }
                }

            }
            catch (Exception ex)
            {
                if (IsSetWetIndicator == true)
                {
                    IndicatiorFillRegion.Close();
                }
                if (IndicatiorFromLoad == true)
                {
                    IndicatiorFillFromLoad.Close();
                }
            }
        }
        //

        private void txtPinCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtCountry.SelectedItem != null && txtCountry.SelectedValue != null)
                if (txtCountry.SelectedItem != null && ((MasterListItem)txtCountry.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).CountryID = ((MasterListItem)txtCountry.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).CountryN = ((MasterListItem)txtCountry.SelectedItem).Description;
                    //txtCountry1.Text = ((clsPatientVO)this.DataContext).CountryN;
                    //txtCountry1.Visibility = Visibility.Visible;
                    //txtCountry.Visibility = Visibility.Collapsed;
                    //StateObj = new MasterListItem(((clsPatientVO)this.DataContext).StateID, ((clsPatientVO)this.DataContext).StateCode, ((clsPatientVO)this.DataContext).StateN);
                    //

                    //added by neena
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList.DeepCopy();
                    txtState.SelectedItem = objM.DeepCopy();
                    txtCity.ItemsSource = objList.DeepCopy();
                    txtCity.SelectedItem = objM.DeepCopy();
                    txtArea.ItemsSource = objList.DeepCopy();
                    txtArea.SelectedItem = objM.DeepCopy();
                    txtPinCode.Text = "";   //added by neena
                    //((clsPatientVO)this.DataContext).Pincode = "";  //added by neena

                    FillState(((MasterListItem)txtCountry.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtState.ItemsSource = objList.DeepCopy();
                    txtState.SelectedItem = objM.DeepCopy();
                    txtCity.ItemsSource = objList.DeepCopy();
                    txtCity.SelectedItem = objM.DeepCopy();
                    txtArea.ItemsSource = objList.DeepCopy();
                    txtArea.SelectedItem = objM.DeepCopy();
                }
        }

        private void txtState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtState.SelectedItem != null && txtState.SelectedValue != null)
                if (txtState.SelectedItem != null && ((MasterListItem)txtState.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).StateID = ((MasterListItem)txtState.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).StateN = ((MasterListItem)txtState.SelectedItem).Description;
                    //txtState1.Text = ((clsPatientVO)this.DataContext).StateN;
                    //txtState1.Visibility = Visibility.Visible;
                    //txtState.Visibility = Visibility.Collapsed;
                    //CountryObj = new MasterListItem(((clsPatientVO)this.DataContext).CountryID, ((clsPatientVO)this.DataContext).CountryCode, ((clsPatientVO)this.DataContext).CountryN);
                    //


                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList.DeepCopy();
                    txtCity.SelectedItem = objList[0].DeepCopy();
                    //txtArea.ItemsSource = objList.DeepCopy();
                    //txtArea.SelectedItem = objList[0].DeepCopy();     

                    //((clsPatientVO)this.DataContext).Pincode = "";  //added by neena
                    txtPinCode.Text = "";   //added by neena
                    FillCity(((MasterListItem)txtState.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtCity.ItemsSource = objList.DeepCopy();
                    txtCity.SelectedItem = objList[0].DeepCopy();
                    //txtArea.ItemsSource = objList.DeepCopy();
                    //txtArea.SelectedItem = objList[0].DeepCopy();
                }
        }

        private void txtDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (txtState.SelectedItem != null && txtState.SelectedValue != null)
            //    if (((MasterListItem)txtState.SelectedItem).ID > 0)
            //    {
            if (txtCity.SelectedItem != null && txtCity.SelectedValue != null)
            {
                if (txtCity.SelectedItem != null && ((MasterListItem)txtCity.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).CityID = ((MasterListItem)txtCity.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).CityN = ((MasterListItem)txtCity.SelectedItem).Description;
                    //txtCity1.Text = ((clsPatientVO)this.DataContext).CityN;
                    //txtCity1.Visibility = Visibility.Visible;
                    //txtCity.Visibility = Visibility.Collapsed;
                    //CityObj = new MasterListItem(((clsPatientVO)this.DataContext).CityID, ((clsPatientVO)this.DataContext).CityCode, ((clsPatientVO)this.DataContext).CityN);
                    //

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();
                    txtArea.SelectedItem = objList[0].DeepCopy();

                    //((clsPatientVO)this.DataContext).Pincode = "";  //added by neena
                    txtPinCode.Text = "";   //added by neena
                    FillRegion(((MasterListItem)txtCity.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtArea.ItemsSource = null;
                    txtArea.ItemsSource = objList.DeepCopy();
                    txtArea.SelectedItem = objList[0].DeepCopy();
                }
            }
        }

        private void txtArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext) != null)
            {
                if (txtArea.SelectedItem != null && ((MasterListItem)txtArea.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).RegionID = ((MasterListItem)txtArea.SelectedItem).ID;

                    //added by neena
                    ((clsPatientVO)this.DataContext).Region = ((MasterListItem)txtArea.SelectedItem).Description;
                    //txtArea1.Text = ((clsPatientVO)this.DataContext).Region;
                    //txtArea1.Visibility = Visibility.Visible;
                    //txtArea.Visibility = Visibility.Collapsed;

                    //RegionObj = new MasterListItem(((clsPatientVO)this.DataContext).RegionID, ((clsPatientVO)this.DataContext).RegionCode, ((clsPatientVO)this.DataContext).Region);
                    //

                    //if (((RegionList.Where(t => t.Id == ((MasterListItem)txtArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault())) == string.Empty)
                    //{
                    //    if (RegionList.Where(t => t.PinCode == txtPinCode.Text).Any())
                    //    {
                    //        txtPinCode.Text = "";
                    //    }
                    //    ((clsPatientVO)this.DataContext).Pincode = txtPinCode.Text;
                    //}
                    //else
                    //{

                    ((clsPatientVO)this.DataContext).Pincode = (RegionList.Where(t => t.Id == ((MasterListItem)txtArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault());
                    //}


                }
            }
            else
            {
                if ((clsPatientVO)this.DataContext != null)
                    ((clsPatientVO)this.DataContext).Pincode = "";
            }

        }

        private void txtSpouseCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSpouseCountry.SelectedItem != null && txtCountry.SelectedValue != null)

                if (txtSpouseCountry.SelectedItem != null && ((MasterListItem)txtSpouseCountry.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryID = ((MasterListItem)txtSpouseCountry.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).SpouseDetails.CountryN = ((MasterListItem)txtSpouseCountry.SelectedItem).Description;
                    //txtSpouseCountry1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.CountryN;
                    //txtSpouseCountry1.Visibility = Visibility.Visible;
                    //txtSpouseCountry.Visibility = Visibility.Collapsed;
                    //SpouseCountryObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.CountryID, ((clsPatientVO)this.DataContext).SpouseDetails.CountryCode, ((clsPatientVO)this.DataContext).SpouseDetails.CountryN);                    
                    //

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseState.ItemsSource = objList.DeepCopy();
                    txtSpouseState.SelectedItem = objM.DeepCopy();
                    txtSpouseCity.ItemsSource = objList.DeepCopy();
                    txtSpouseCity.SelectedItem = objM.DeepCopy();
                    txtSpouseArea.ItemsSource = objList.DeepCopy();
                    txtSpouseArea.SelectedItem = objM.DeepCopy();

                    //commented by neena
                    FillState(((MasterListItem)txtSpouseCountry.SelectedItem).ID);

                    // ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = "";   //added by neena
                    txtSpousePinCode.Text = "";   //added by neena

                    //added by neena
                    //commented by neena
                    //FillStateForSpouse(((MasterListItem)txtSpouseCountry.SelectedItem).ID);  
                }
        }

        private void txtSpouseState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtSpouseState.SelectedItem != null && txtSpouseState.SelectedValue != null)
                if (txtSpouseState.SelectedItem != null && ((MasterListItem)txtSpouseState.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateID = ((MasterListItem)txtSpouseState.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).SpouseDetails.StateN = ((MasterListItem)txtSpouseState.SelectedItem).Description;
                    //txtSpouseState1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.StateN;
                    //txtSpouseState1.Visibility = Visibility.Visible;
                    //txtSpouseState.Visibility = Visibility.Collapsed;
                    //SpouseStateObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.StateID, ((clsPatientVO)this.DataContext).SpouseDetails.StateCode, ((clsPatientVO)this.DataContext).SpouseDetails.StateN);
                    //

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseCity.ItemsSource = objList.DeepCopy();
                    txtSpouseCity.SelectedItem = objList[0].DeepCopy();
                    txtSpouseArea.ItemsSource = objList.DeepCopy();
                    txtSpouseArea.SelectedItem = objList[0].DeepCopy();

                    //commented by neena
                    FillCity(((MasterListItem)txtSpouseState.SelectedItem).ID);

                    //((clsPatientVO)this.DataContext).SpouseDetails.Pincode = "";   //added by neena
                    txtSpousePinCode.Text = "";   //added by neena
                    //added by neena
                    //commented by neena
                    // FillCityForSpouse(((MasterListItem)txtSpouseState.SelectedItem).ID);  
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseArea.ItemsSource = null;
                    txtSpouseArea.ItemsSource = objList.DeepCopy();
                    txtSpouseArea.SelectedItem = objList[0].DeepCopy();
                }
        }

        private void txtSpouseDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtSpouseArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((clsPatientVO)this.DataContext) != null && ((clsPatientVO)this.DataContext).SpouseDetails != null)
            {
                if (txtSpouseArea.SelectedItem != null && ((MasterListItem)txtSpouseArea.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).SpouseDetails.RegionID = ((MasterListItem)txtSpouseArea.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).SpouseDetails.Region = ((MasterListItem)txtSpouseArea.SelectedItem).Description;
                    //txtSpouseArea1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.Region;
                    //txtSpouseArea1.Visibility = Visibility.Visible;
                    //txtSpouseArea.Visibility = Visibility.Collapsed;
                    //SpouseRegionObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.RegionID, ((clsPatientVO)this.DataContext).SpouseDetails.RegionCode, ((clsPatientVO)this.DataContext).SpouseDetails.Region);
                    //

                    //if (((RegionList.Where(t => t.Id == ((MasterListItem)txtSpouseArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault())) == string.Empty)
                    //{
                    //    if (RegionList.Where(t => t.PinCode == txtSpouseArea.Text).Any())
                    //    {
                    //        txtSpousePinCode.Text = "";
                    //    }
                    //    if (txtPinCode.Text != null)
                    //        ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = txtPinCode.Text;
                    //    else
                    //        ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = txtSpousePinCode.Text;
                    //}
                    //else
                    //{

                    ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = (RegionList.Where(t => t.Id == ((MasterListItem)txtSpouseArea.SelectedItem).ID).Select(t => t.PinCode).SingleOrDefault());
                    //}
                }
            }
            else
            {
                if ((clsPatientVO)this.DataContext != null)
                    ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = "";
            }
        }

        private void txtSpouseCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (txtSpouseState.SelectedItem != null && txtSpouseState.SelectedValue != null)
            //    if (((MasterListItem)txtSpouseState.SelectedItem).ID > 0)
            //    {
            if (txtSpouseCity.SelectedItem != null && txtSpouseCity.SelectedValue != null)
            {
                if (txtSpouseCity.SelectedItem != null && ((MasterListItem)txtSpouseCity.SelectedItem).ID > 0)
                {
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityID = ((MasterListItem)txtSpouseCity.SelectedItem).ID;
                    //added by neena
                    ((clsPatientVO)this.DataContext).SpouseDetails.CityN = ((MasterListItem)txtSpouseCity.SelectedItem).Description;
                    //txtSpouseCity1.Text = ((clsPatientVO)this.DataContext).SpouseDetails.CityN;
                    //txtSpouseCity1.Visibility = Visibility.Visible;
                    //txtSpouseCity.Visibility = Visibility.Collapsed;
                    //SpouseCityObj = new MasterListItem(((clsPatientVO)this.DataContext).SpouseDetails.CityID, ((clsPatientVO)this.DataContext).SpouseDetails.CityCode, ((clsPatientVO)this.DataContext).SpouseDetails.CityN);
                    //

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    txtSpouseArea.ItemsSource = null;
                    txtSpouseArea.ItemsSource = objList.DeepCopy();
                    txtSpouseArea.SelectedItem = objList[0].DeepCopy();

                    //commeneted by neena
                    FillRegion(((MasterListItem)txtSpouseCity.SelectedItem).ID);
                    //((clsPatientVO)this.DataContext).SpouseDetails.Pincode = "";   //added by neena
                    txtSpousePinCode.Text = "";   //added by neena
                    //added by neena
                    //commeneted by neena
                    //FillRegionForSpouse(((MasterListItem)txtSpouseCity.SelectedItem).ID); //added by neena

                }
            }
        }

        //private void btnLinkScanDoc_Click(object sender, RoutedEventArgs e)
        //{
        //    frmPatientScanDocument win = new frmPatientScanDocument();
        //    win.IsfromEdit = IsfromEdit;
        //    win.PatientType = ((MasterListItem)cmbPatientType.SelectedItem).ID;
        //    if (this.DataContext != null)
        //        win.ScanDoc = ((clsPatientVO)this.DataContext).ScanDocList;
        //    win.OnSaveButton_Click += new RoutedEventHandler(DocWin_OnSaveButton_Click);
        //    win.Show();
        //}
        //void DocWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (((frmPatientScanDocument)sender).DialogResult == true)
        //        ((clsPatientVO)this.DataContext).ScanDocList = ((frmPatientScanDocument)sender).ScanDoc;
        //}

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
                BDMID = (((DoctorMasterChildwindow)sender).MarketingExecutivesID); //***//
            }
            //  FillRefDoctor();
            cmbPreffix.Focus();
            cmbPreffix.UpdateLayout();
        }

        FileInfo fi;
        private void cmdSpouseBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {

                WriteableBitmap imageSource = new WriteableBitmap((int)imgSpousePhoto.Width, (int)imgSpousePhoto.Height);
                //try
                //{

                WriteableBitmap bmp = new WriteableBitmap((int)imgSpousePhoto.Width, (int)imgSpousePhoto.Height);
                bmp.Render(imgSpousePhoto, new MatrixTransform());
                bmp.Invalidate();

                //    int[] p = bmp.Pixels;
                //    int len = p.Length * 4;
                //    byte[] result = new byte[len]; // ARGB
                //    Buffer.BlockCopy(p, 0, result, 0, len);

                try
                {
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    imgSpousePhoto.Source = imageSource;

                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = OpenFile.File;
                    }

                    //((clsPatientVO)this.DataContext).SpouseDetails.Photo = result;
                    ((clsPatientVO)this.DataContext).SpouseDetails.Photo = data;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }

        private void cmdSpouseStartCapture_Click(object sender, RoutedEventArgs e)
        {
            if (_SpousecaptureSource != null)
            {
                _SpousecaptureSource.Stop(); // stop whatever device may be capturing
                if (_captureSource != null)
                {
                    _captureSource.Stop();
                    borderimage.Visibility = System.Windows.Visibility.Visible;
                    borderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    cmdStartCapture.Visibility = Visibility.Visible;
                    cmdCaptureImage.Visibility = Visibility.Collapsed;
                }

                VideoBrush vidBrush = new VideoBrush();
                vidBrush.SetSource(_SpousecaptureSource);
                SpouseWebcamCapture.Fill = vidBrush; // paint the brush on the rectangle

                // request user permission and display the capture
                if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                {
                    _SpousecaptureSource.Start();
                }
                cmdSpouseStartCapture.Visibility = Visibility.Collapsed;
                cmdSpouseCaptureImage.Visibility = Visibility.Visible;
                Spouseborderimage.Visibility = System.Windows.Visibility.Collapsed;
                Spouseborderwebcap.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void cmdSpouseCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (_SpousecaptureSource != null)
                {
                    // capture the current frame and add it to our observable collection                
                    _SpousecaptureSource.CaptureImageAsync();
                    _SpousecaptureSource.CaptureImageCompleted += ((s, args) =>
                    {
                        _Spouseimages = (args.Result);
                        imgSpousePhoto.Source = _Spouseimages;

                        //WriteableBitmap bmp = new WriteableBitmap((int)imgSpousePhoto.Width, (int)imgSpousePhoto.Height);
                        //bmp.Render(imgSpousePhoto, new MatrixTransform());
                        //bmp.Invalidate();

                        //int[] p = bmp.Pixels;
                        //int len = p.Length * 4;
                        //byte[] result = new byte[len]; // ARGB
                        //Buffer.BlockCopy(p, 0, result, 0, len);
                        // ((clsPatientVO)this.DataContext).SpouseDetails.Photo = result;
                        var image = _Spouseimages.ToImage();

                        using (Stream stream = image.ToStream())
                        {
                            data = new byte[stream.Length];
                            stream.Read(data, 0, (int)stream.Length);

                            stream.Close();
                        }

                        ((clsPatientVO)this.DataContext).SpouseDetails.Photo = data;


                    });
                    cmdSpouseStartCapture.Visibility = Visibility.Visible;
                    cmdSpouseCaptureImage.Visibility = Visibility.Collapsed;
                    Spouseborderimage.Visibility = System.Windows.Visibility.Visible;
                    Spouseborderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    //_SpousecaptureSource.Stop();

                }
            }
            catch (System.Exception ex)
            {


            }
        }

        private void cmdSpouseStopCapture_Click(object sender, RoutedEventArgs e)
        {


        }
        CaptureSource _captureSource = new CaptureSource();
        CaptureSource _SpousecaptureSource = new CaptureSource();
        WriteableBitmap _images = null;
        WriteableBitmap _Spouseimages = null;


        private void cmdStartCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (_captureSource != null)
                {
                    _captureSource.Stop();
                    if (_SpousecaptureSource != null)
                    {
                        _SpousecaptureSource.Stop();
                        cmdSpouseStartCapture.Visibility = Visibility.Visible;
                        cmdSpouseCaptureImage.Visibility = Visibility.Collapsed;
                        Spouseborderimage.Visibility = System.Windows.Visibility.Visible;
                        Spouseborderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    }
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
        byte[] data;
        private void cmdCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (_captureSource != null)
                {
                    _captureSource.CaptureImageAsync();
                    _captureSource.CaptureImageCompleted += ((s, args) =>
                    {

                        _images = (args.Result);
                        imgPhoto.Source = _images;

                        //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                        //bmp.Render(imgPhoto, new MatrixTransform());
                        //bmp.Invalidate();
                        //converterDemo(imgPhoto);
                        //byte[] buffer = null;
                        //int[] p = bmp.Pixels;
                        //int len = p.Length * 4;
                        //byte[] result = new byte[len]; // ARGB
                        //Buffer.BlockCopy(p, 0, result, 0, len);


                        var image = _images.ToImage();

                        using (Stream stream = image.ToStream())
                        {
                            data = new byte[stream.Length];
                            stream.Read(data, 0, (int)stream.Length);

                            stream.Close();
                        }

                        ((clsPatientVO)this.DataContext).Photo = data;





                    });
                    borderimage.Visibility = System.Windows.Visibility.Visible;
                    borderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                    cmdStartCapture.Visibility = Visibility.Visible;
                    cmdCaptureImage.Visibility = Visibility.Collapsed;
                    // _captureSource.Stop();

                }
            }
            catch (System.Exception ex)
            {


            }
        }




        //public static byte[] converterDemo(Image x)
        //{
        //    ImageConverter _imageConverter = new ImageConverter();
        //    byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
        //    return xByte;
        //}
        //public byte[] getJPGFromImageControl(BitmapImage imageC)
        //{
        //    MemoryStream memStream = new MemoryStream();
        //    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        //    encoder.Frames.Add(BitmapFrame.Create(imageC));
        //    encoder.Save(memStream);
        //    return memStream.ToArray();
        //}




        private void cmdStopCapture_Click(object sender, RoutedEventArgs e)
        {

        }


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

                    //int[] p = bmp.Pixels;
                    //int len = p.Length * 4;
                    //byte[] result = new byte[len]; // ARGB
                    //Buffer.BlockCopy(p, 0, result, 0, len);
                    using (Stream stream = OpenFile.File.OpenRead())
                    {
                        long size = stream.Length;
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = OpenFile.File;
                    }

                    Stream str = OpenFile.File.OpenRead();

                    //((clsPatientVO)this.DataContext).Photo = result;
                    ((clsPatientVO)this.DataContext).Photo = data;
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }
        }



        //.....................................................

        //Add By akshays

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

        private void ChkSpouseInternationalPatient_Click(object sender, RoutedEventArgs e)
        {

            if (((CheckBox)sender).IsChecked == true)
            {
                cmbSpouseIdentity.SelectedValue = (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient);
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != null && (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != 0)
                    cmbSpouseIdentity.TextBox.SetValidation("For International Patient " + ((MasterListItem)cmbSpouseIdentity.SelectedItem).Description + " Details Are Mendatory");
                else
                    cmbSpouseIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");
                cmbSpouseIdentity.TextBox.RaiseValidationError();
                cmbSpouseIdentity.Focus();
                txtSpouseIdentityNumber.SetValidation("Identity Number Is Required");
                txtSpouseIdentityNumber.RaiseValidationError();
            }
            else
            {
                cmbSpouseIdentity.SelectedValue = (long)0;
                cmbSpouseIdentity.TextBox.ClearValidationError();
                txtSpouseIdentityNumber.ClearValidationError();
            }
        }

        private void ChkInternationalPatient_Click(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                cmbIdentity.SelectedValue = (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient);
                if ((((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != null && (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IdentityForInternationalPatient) != 0)
                    cmbIdentity.TextBox.SetValidation("For International Patient " + ((MasterListItem)cmbIdentity.SelectedItem).Description + " Details Are Mendatory");
                else
                    cmbIdentity.TextBox.SetValidation("For International Patient Identity Details Are Mendatory");

                cmbIdentity.TextBox.RaiseValidationError();
                cmbIdentity.Focus();
                txtIdentityNumber.SetValidation("Identity Number Is Required");
                txtIdentityNumber.RaiseValidationError();
            }
            else
            {
                cmbIdentity.SelectedValue = (long)0;
                cmbIdentity.TextBox.ClearValidationError();
                txtIdentityNumber.ClearValidationError();

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

        private void txtPinCode_LostFocus(object sender, RoutedEventArgs e)
        {
            ((clsPatientVO)this.DataContext).Pincode = txtPinCode.Text;
            cmbSpousePreffix.Focus();
            cmbSpousePreffix.UpdateLayout();
        }

        private void txtSpousePinCode_LostFocus(object sender, RoutedEventArgs e)
        {
            ((clsPatientVO)this.DataContext).SpouseDetails.Pincode = txtSpousePinCode.Text;
        }

        private void cmbReferralDoctor_LostFocus(object sender, RoutedEventArgs e)
        {
            cmbPreffix.Focus();
            cmbPreffix.UpdateLayout();
        }


        //Added By akshays on 17/11/2015
        public byte[] BarcodeImageGeneralDetailsMR { get; set; }
        public byte[] BarcodeImageSpouseMR { get; set; }
        private void Barcodesaveboth(String GeneralDetailsBarcodeMrno, String SpouseBarcodeMrno)
        {

            #region Barcode Image
            #region GeneralDetailsBarcodeMrno
            if (GeneralDetailsBarcodeMrno != null || GeneralDetailsBarcodeMrno != "")
            {
                int height = (int)this.MyCanvas.ActualHeight;
                Code128 barcode = new Code128();
                List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();

                rList = barcode.DrawCode128(GeneralDetailsBarcodeMrno, 1, 3);
                foreach (System.Windows.Shapes.Rectangle item in rList)
                {
                    MyCanvas.Children.Add(item);
                }
                #region Save Generated Barcode To DataBase
                WriteableBitmap bitmap = new WriteableBitmap(MyCanvas, null);
                var img = bitmap.ToImage();
                var encoder = new PngEncoder();
                MemoryStream stream = new MemoryStream();
                //JpegEncoder encoder = new JpegEncoder(img, 100, stream);
                encoder.Encode(img, stream);
                BarcodeImageGeneralDetailsMR = stream.ToArray();
                stream.Close();
                #endregion
            }
            #endregion

            #region SpouseBarcodeMrno
            if (SpouseBarcodeMrno != null || SpouseBarcodeMrno != "")
            {
                int height1 = (int)this.MyCanvas1.ActualHeight;
                Code128 barcode1 = new Code128();
                List<System.Windows.Shapes.Rectangle> rList1 = new List<System.Windows.Shapes.Rectangle>();

                rList1 = barcode1.DrawCode128(SpouseBarcodeMrno, 1, 3);
                foreach (System.Windows.Shapes.Rectangle item in rList1)
                {
                    MyCanvas1.Children.Add(item);
                }
                #region Save Generated Barcode To DataBase
                WriteableBitmap bitmap1 = new WriteableBitmap(MyCanvas1, null);
                var img1 = bitmap1.ToImage();
                var encoder1 = new PngEncoder();
                MemoryStream stream1 = new MemoryStream();
                encoder1.Encode(img1, stream1);
                BarcodeImageSpouseMR = stream1.ToArray();
                stream1.Close();
                #endregion
            }
            #endregion

            #endregion

            AddBarcodeImageBizActionVO BizAction = new AddBarcodeImageBizActionVO();
            //BizAction.PatientDetails.SpouseDetails.Flag1 = 1;
            //      BizAction.PatientDetails.GeneralDetails.GeneralDetailsBarcodeImage = BarcodeImageGeneralDetailsMR;
            //      BizAction.PatientDetails.SpouseDetails.SpouseBarcodeImage = BarcodeImageSpouseMR;
            //      BizAction.PatientDetails.GeneralDetails.MRNo = GeneralDetailsBarcodeMrno;
            //      BizAction.PatientDetails.SpouseDetails.MRNo = SpouseBarcodeMrno;
            BizAction.ObjBarcodeImage = new AddBarcodeImageVO();
            BizAction.ObjBarcodeImage.GeneralDetailsBarcodeImage = BarcodeImageGeneralDetailsMR;
            BizAction.ObjBarcodeImage.SpouseBarcodeImage = BarcodeImageSpouseMR;
            BizAction.ObjBarcodeImage.GeneralDetailsMRNo = GeneralDetailsBarcodeMrno;
            BizAction.ObjBarcodeImage.SpouseMRNo = SpouseBarcodeMrno;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Barcode Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void Barcodesave(String GeneralDetailsBarcodeMrno)
        {

            #region Barcode Image
            #region GeneralDetailsBarcodeMrno
            if (GeneralDetailsBarcodeMrno != null || GeneralDetailsBarcodeMrno != "")
            {
                int height = (int)this.MyCanvas.ActualHeight;
                Code128 barcode = new Code128();
                List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();

                rList = barcode.DrawCode128(GeneralDetailsBarcodeMrno, 1, 3);
                foreach (System.Windows.Shapes.Rectangle item in rList)
                {
                    MyCanvas.Children.Add(item);
                }
                #region Save Generated Barcode To DataBase
                WriteableBitmap bitmap = new WriteableBitmap(MyCanvas, null);
                var img = bitmap.ToImage();
                var encoder = new PngEncoder();
                MemoryStream stream = new MemoryStream();
                //JpegEncoder encoder = new JpegEncoder(img, 100, stream);
                encoder.Encode(img, stream);
                BarcodeImageGeneralDetailsMR = stream.ToArray();
                stream.Close();
                #endregion
            }
            #endregion



            #endregion

            AddBarcodeImageBizActionVO BizAction = new AddBarcodeImageBizActionVO();
            //BizAction.PatientDetails.SpouseDetails.Flag1 = 1;
            //      BizAction.PatientDetails.GeneralDetails.GeneralDetailsBarcodeImage = BarcodeImageGeneralDetailsMR;
            //      BizAction.PatientDetails.SpouseDetails.SpouseBarcodeImage = BarcodeImageSpouseMR;
            //      BizAction.PatientDetails.GeneralDetails.MRNo = GeneralDetailsBarcodeMrno;
            //      BizAction.PatientDetails.SpouseDetails.MRNo = SpouseBarcodeMrno;
            BizAction.ObjBarcodeImage = new AddBarcodeImageVO();
            BizAction.ObjBarcodeImage.GeneralDetailsBarcodeImage = BarcodeImageGeneralDetailsMR;
            BizAction.ObjBarcodeImage.GeneralDetailsMRNo = GeneralDetailsBarcodeMrno;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW2 =
                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Barcode Saved Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW2.Show();
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void txtDaughterOf_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDaughterOf.Text = txtDaughterOf.Text.ToTitleCase();

            if (cmbPatientType.SelectedItem != null && ((MasterListItem)cmbPatientType.SelectedItem).ID == 13)  // AppConfig.  M_PatientCategoryMaster= Couple
            {
                txtBabyNo.Focus();
            }
            else
            {
                cmbBloodGroup.Focus();
            }
        }



        private void dtpMarriageAnnDate_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpMarriageAnnDate.SelectedDate != null)
            {
                ((clsPatientVO)this.DataContext).MarriageAnnDate = dtpMarriageAnnDate.SelectedDate;
                ((clsPatientVO)this.DataContext).SpouseDetails.MarriageAnnDate = dtpMarriageAnnDate.SelectedDate;
            }
        }

        private void cmbGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbGender.SelectedItem != null && ((MasterListItem)cmbGender.SelectedItem).ID == (long)Genders.Female) //(int)Genders.Female
            {
                //  lblDaughterOf.Text = "Daughter of";  //Commented By YK
                stkVisit.Visibility = Visibility.Visible;
                stkVisityesNo.Visibility = Visibility.Visible;
               // txtClinicName.Visibility = Visibility.Visible;
                //lblClinicName.Visibility = Visibility.Visible;

                //commented by neena
                //txtDaughterOf.SetValidation("Daughter of Is Required");
                //txtDaughterOf.RaiseValidationError();
                //
            }
            else if (cmbGender.SelectedItem != null && ((MasterListItem)cmbGender.SelectedItem).ID == (long)Genders.Male) //(int)Genders.Male
            {
                // lblDaughterOf.Text = "Son of";////Commented By YK
                stkVisit.Visibility = Visibility.Collapsed;
                stkVisityesNo.Visibility = Visibility.Collapsed;
               // txtClinicName.Visibility = Visibility.Collapsed;
                //lblClinicName.Visibility = Visibility.Collapsed;

                //commented by neena
                //txtDaughterOf.SetValidation("Son of Is Required");
                //txtDaughterOf.RaiseValidationError();
                //
            }

        }
        //Closed By akshays on 17/11/2015

        private void cmbIdentity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ChangeIdentityDoc == true)
            {
                if (((clsPatientVO)this.DataContext).GeneralDetails.IdentityNumber != null)
                    txtIdentityNumber.Text = ((clsPatientVO)this.DataContext).GeneralDetails.IdentityNumber;
                ChangeIdentityDoc = false;
            }
            else
            {
                txtIdentityNumber.Text = "";
            }

            //* Added by - Ajit Jadhav
            //* Added Date - 11/8/2016
            //* Comments - Create New TextBox txtPanNo Select pancard textbox is IsReadOnly 
            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID == 4)
            {
                txtPanNo.IsReadOnly = true;
            }
            else  //* Added by - Ajit Jadhav Added Date - 3/10/2016 
            {
                txtPanNo.IsReadOnly = false;
            }
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


        //* Added by - Ajit Jadhav
        //* Added Date - 10/8/2016
        //* Comments - Create New TextBox txtSpousePanNo Select pancard textbox is IsReadOnly 

        private void cmbSpouseIdentity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSpouseIdentityNumber.Text = "";

            if (cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 4)
            {
                txtSpousePanNo.IsReadOnly = true;
            }
            else
            {
                txtSpousePanNo.IsReadOnly = false;
            }
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
            //BizAction.UnitId = 0;//Commented By YK
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;//Added By YK to show doctors list respective centers
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


        ////added by neena
        //commented by neena
        //private void cmdGetArea_Click(object sender, RoutedEventArgs e)
        //{
        //    FillRegion(((clsPatientVO)this.DataContext).CityID);
        //    txtArea1.Visibility = Visibility.Collapsed;
        //    txtArea.Visibility = Visibility.Visible;
        //    txtArea1.Text = string.Empty;

        //    //spouse
        //    txtSpouseArea1.Visibility = Visibility.Collapsed;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetCity_Click(object sender, RoutedEventArgs e)
        //{
        //    FillCity(((clsPatientVO)this.DataContext).StateID);
        //    txtCity1.Visibility = Visibility.Collapsed;
        //    txtCity.Visibility = Visibility.Visible;
        //    txtArea.Visibility = Visibility.Visible;
        //    txtCity1.Text = string.Empty;
        //    txtArea1.Text = string.Empty;

        //    //spouse
        //    txtSpouseCity1.Visibility = Visibility.Collapsed;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetState_Click(object sender, RoutedEventArgs e)
        //{
        //    FillState(((clsPatientVO)this.DataContext).CountryID);
        //    txtState1.Visibility = Visibility.Collapsed;
        //    txtState.Visibility = Visibility.Visible;
        //    txtCity.Visibility = Visibility.Visible;
        //    txtArea.Visibility = Visibility.Visible;
        //    txtState1.Text = string.Empty;
        //    txtCity1.Text = string.Empty;
        //    txtArea1.Text = string.Empty;

        //    //spouse
        //    txtSpouseState1.Visibility = Visibility.Collapsed;
        //    txtSpouseState.Visibility = Visibility.Visible;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseState1.Text = string.Empty;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetCountry_Click(object sender, RoutedEventArgs e)
        //{
        //    FillCountry();
        //    txtCountry1.Visibility = Visibility.Collapsed;
        //    txtCountry.Visibility = Visibility.Visible;
        //    txtState.Visibility = Visibility.Visible;
        //    txtCity.Visibility = Visibility.Visible;
        //    txtArea.Visibility = Visibility.Visible;
        //    txtCountry1.Text = string.Empty;
        //    txtState1.Text = string.Empty;
        //    txtCity1.Text = string.Empty;
        //    txtArea1.Text = string.Empty;


        //    //spouse
        //    txtSpouseCountry1.Visibility = Visibility.Collapsed;
        //    txtSpouseCountry.Visibility = Visibility.Visible;
        //    txtSpouseState.Visibility = Visibility.Visible;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseCountry1.Text = string.Empty;
        //    txtSpouseState1.Text = string.Empty;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;

        //}

        //private void cmdGetSpouseCountry_Click(object sender, RoutedEventArgs e)
        //{
        //    FillCountry();
        //    txtSpouseCountry1.Visibility = Visibility.Collapsed;
        //    txtSpouseCountry.Visibility = Visibility.Visible;
        //    txtSpouseState.Visibility = Visibility.Visible;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseCountry1.Text = string.Empty;
        //    txtSpouseState1.Text = string.Empty;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetSpouseState_Click(object sender, RoutedEventArgs e)
        //{
        //    FillState(((clsPatientVO)this.DataContext).CountryID);
        //    txtSpouseState1.Visibility = Visibility.Collapsed;
        //    txtSpouseState.Visibility = Visibility.Visible;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseState1.Text = string.Empty;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetSpouseCity_Click(object sender, RoutedEventArgs e)
        //{
        //    FillCity(((clsPatientVO)this.DataContext).StateID);
        //    txtSpouseCity1.Visibility = Visibility.Collapsed;
        //    txtSpouseCity.Visibility = Visibility.Visible;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseCity1.Text = string.Empty;
        //    txtSpouseArea1.Text = string.Empty;
        //}

        //private void cmdGetSpouseArea_Click(object sender, RoutedEventArgs e)
        //{
        //    FillRegion(((clsPatientVO)this.DataContext).CityID);
        //    txtSpouseArea1.Visibility = Visibility.Collapsed;
        //    txtSpouseArea.Visibility = Visibility.Visible;
        //    txtSpouseArea1.Text = string.Empty;
        //}
        //

        bool ckSavePhoto = false;
        private void CmdSavePhotoToServer_Click(object sender, RoutedEventArgs e)
        {
            Indicatior.Show();
            clsAddPatientPhotoToServerBizActionVO BizAction = new clsAddPatientPhotoToServerBizActionVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsAddPatientPhotoToServerBizActionVO)arg.Result).PatientDetailsList.Count > 0)
                    {
                        clsMovePatientPhotoToServerBizActionVO BizAction1 = new clsMovePatientPhotoToServerBizActionVO();
                        BizAction1.PatientDetailsList = ((clsAddPatientPhotoToServerBizActionVO)arg.Result).PatientDetailsList;

                        Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                        Client1.ProcessCompleted += (s1, arg1) =>
                        {
                            if (arg1.Error == null && arg1.Result != null)
                            {
                                Indicatior.Close();
                                //if (((clsMovePatientPhotoToServerBizActionVO)arg1.Result).SuccessStatus == 1)
                                //{
                                string msgText = "";
                                ckSavePhoto = true;
                                msgText = "Transaction is complited";
                                MessageBoxControl.MessageBoxChildWindow msgW =
                                      new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Question);
                                msgW.Show();
                                CmdSavePhotoToServer.Visibility = Visibility.Collapsed;
                                //}
                            }
                        };
                        Client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client1.CloseAsync();

                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void cmbReferralName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((MasterListItem)cmbReferralName.SelectedItem).ID == 1)
            {
                ReferralDoc.IsChecked = true;
                ReferralDoc.IsEnabled = true;
                InhouseDoc.IsEnabled = false;
                cmbCampDetail.IsEnabled = false;
                // txtReferralDetail.ClearValidationError();
                cmbCampDetail.TextBox.ClearValidationError();
                FillRefDoctor();
                //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = true;
                cmdSearch.IsEnabled = true;
                cmbReferralDoctor.IsEnabled = true;
                FillCamp();
                FillCoConsultantDoctor();


            }
            else
                if (((MasterListItem)cmbReferralName.SelectedItem).ID == 2)
                {
                    ReferralDoc.IsEnabled = false;
                    InhouseDoc.IsEnabled = true;
                    InhouseDoc.IsChecked = true;
                    cmbCampDetail.IsEnabled = false;
                    //txtReferralDetail.ClearValidationError();
                    cmbCampDetail.TextBox.ClearValidationError();
                    fillInhouseDoctor();
                    //((clsPatientVO)this.DataContext).GeneralDetails.IsReferralDoc = false;
                    cmdSearch.IsEnabled = false;
                    cmbReferralDoctor.IsEnabled = true;
                    FillCamp();
                    FillCoConsultantDoctor();
                }
                else
                    if (((MasterListItem)cmbReferralName.SelectedItem).ID == 9)
                    {
                        cmbCampDetail.IsEnabled = true;
                        ReferralDoc.IsEnabled = true;
                        InhouseDoc.IsEnabled = true;
                        ReferralDoc.IsChecked = true;
                        cmbReferralDoctor.IsEnabled = true;
                        cmdSearch.IsEnabled = false;
                        cmbReferralDoctor.TextBox.ClearValidationError();

                        if (((clsPatientVO)this.DataContext).IsReferralDoc == true)
                        {                           
                            FillRefDoctor();                            
                        }
                        else
                        {                            
                            fillInhouseDoctor();                            
                        }
                        
                    }
                    else
                    {
                        //txtReferralDetail.SetValidation("Referral Detail Is Required");
                        //txtReferralDetail.RaiseValidationError();                        
                        ReferralDoc.IsEnabled = false;
                        InhouseDoc.IsEnabled = false;
                        ReferralDoc.IsChecked = true;
                        cmbReferralDoctor.IsEnabled = false;
                        cmdSearch.IsEnabled = false;
                        cmbCampDetail.IsEnabled = false;
                        cmbReferralDoctor.TextBox.ClearValidationError();
                        cmbCampDetail.TextBox.ClearValidationError();                       
                        FillCamp();
                        FillCoConsultantDoctor();
                        if (((clsPatientVO)this.DataContext).IsReferralDoc == true)
                        {
                            FillRefDoctor();
                        }
                        else
                        {
                            fillInhouseDoctor();
                        }
                    }

        }
        //

        //* Added by - Ajit Jadhav
        //* Added Date - 10/8/2016
        //* Comments - key press pan card select then  display value in txtSpousePanNo And Validation Of Pan Number

        private void txtSpouseIdentityNumber_KeyUp(object sender, RoutedEventArgs e)
        {

            if (cmbSpouseIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 4 && txtSpouseIdentityNumber.Text != string.Empty)
            {
                txtSpousePanNo.Text = txtSpouseIdentityNumber.Text;
            }
            else if (txtSpousePanNo.Text != string.Empty && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 4)
            {
                txtSpousePanNo.Text = "";
            }
        }

        private void txtIdentityNumber_KeyUp(object sender, RoutedEventArgs e)
        {

            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID == 4 && txtIdentityNumber.Text != string.Empty)
            {
                txtPanNo.Text = txtIdentityNumber.Text;
            }
            else if (txtPanNo.Text != string.Empty && ((MasterListItem)cmbIdentity.SelectedItem).ID == 4)
            {
                txtPanNo.Text = "";
            }
        }

        private void txtPanNo_KeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtPanNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length > 10)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }

        private void txtSpousePanNo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtSpousePanNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length > 10)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }

        }
        //***// Date 21/9/2016 check lenth for pan number in select Combo Box
        private void txtIdentityNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID == 4)
            {
                // e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
                textBefore = ((TextBox)sender).Text;
                selectionStart = ((TextBox)sender).SelectionStart;
                selectionLength = ((TextBox)sender).SelectionLength;
            }
        }

        private void txtIdentityNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbIdentity.SelectedItem).ID == 4)
            {
                if (((TextBox)sender).Text.Length > 10)
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

        private void txtSpouseIdentityNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 4)
            {

                //  e.Handled = CIMS.Comman.HandleAlphnumericAndSpecialChar(sender, e, false);
                textBefore = ((TextBox)sender).Text;
                selectionStart = ((TextBox)sender).SelectionStart;
                selectionLength = ((TextBox)sender).SelectionLength;
            }
        }

        private void txtSpouseIdentityNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (cmbIdentity.SelectedItem != null && ((MasterListItem)cmbSpouseIdentity.SelectedItem).ID == 4)
            {
                if (((TextBox)sender).Text.Length > 10)
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

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Trim() != "" && (!((TextBox)sender).Text.IsPositiveDoubleValid()) && textBefore != null)
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void btnSearchParent_Click(object sender, RoutedEventArgs e)
        {
            PatientSearchForPediatric Win = new PatientSearchForPediatric();
            Win.IsPediatric = true; // Use to show only Female Patient on search window
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }
        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatientForPediatric != null && ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.PatientID != 0)
                {

                    clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                    BizAction.PatientDetails = new clsPatientVO();
                    BizAction.PatientDetails.GeneralDetails.BringScanDoc = true;
                    BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.PatientID;
                    BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.UnitId;
                    BizAction.PatientDetails.GeneralDetails.LinkServer = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.LinkServer;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                    Client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                # region Set Controls

                                clsPatientVO objPatient = new clsPatientVO();

                                objPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;

                                ////  MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                                LinkPatientID = objPatient.GeneralDetails.PatientID; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.PatientID;
                                LinkPatientUnitID = objPatient.GeneralDetails.UnitId; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.UnitId;

                                txtFirstName.Text = objPatient.GeneralDetails.FirstName; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.FirstName;
                                txtLastName.Text = objPatient.GeneralDetails.LastName; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.LastName;
                                txtParentName.Text = objPatient.GeneralDetails.FirstName + " " + objPatient.GeneralDetails.LastName; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.LastName;
                                txtMRNumberParent.Text = objPatient.GeneralDetails.MRNo; // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.MRNo;

                                //------------------------------------------------------------------------------

                                List<MasterListItem> educationList = new List<MasterListItem>();

                                if (cmbEducation.ItemsSource != null)
                                    educationList = ((List<MasterListItem>)cmbEducation.ItemsSource).ToList();

                                if (educationList != null)
                                {
                                    var eduItem = from r in educationList
                                                  where r.ID == 12
                                                  select r;

                                    if (eduItem != null && eduItem.ToList().Count > 0)
                                        cmbEducation.SelectedValue = eduItem.ToList()[0].ID;
                                    //cmbEducation.SelectedValue = 12;      // Others
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> maritalStatusList = new List<MasterListItem>();

                                if (cmbMaritalStatus.ItemsSource != null)
                                    maritalStatusList = ((List<MasterListItem>)cmbMaritalStatus.ItemsSource).ToList();

                                if (maritalStatusList != null)
                                {
                                    var marStatusItem = from r in maritalStatusList
                                                        where r.ID == 2
                                                        select r;

                                    if (marStatusItem != null && marStatusItem.ToList().Count > 0)
                                        cmbMaritalStatus.SelectedValue = marStatusItem.ToList()[0].ID;
                                    //cmbMaritalStatus.SelectedValue = 2;  // SINGLE
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> occList = new List<MasterListItem>();

                                if (cmbOccupation.ItemsSource != null)
                                    occList = ((List<MasterListItem>)cmbOccupation.ItemsSource).ToList();

                                if (occList != null)
                                {
                                    var occItem = from r in occList
                                                  where r.ID == 13
                                                  select r;

                                    if (occItem != null && occItem.ToList().Count > 0)
                                        cmbOccupation.SelectedValue = occItem.ToList()[0].ID;
                                    //cmbOccupation.SelectedValue = 13;     // OTHER
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> spcList = new List<MasterListItem>();

                                if (cmbSpecialReg.ItemsSource != null)
                                    spcList = ((List<MasterListItem>)cmbSpecialReg.ItemsSource).ToList();

                                if (spcList != null)
                                {
                                    var spcItem = from r in spcList
                                                  where r.ID == 2
                                                  select r;

                                    if (spcItem != null && spcItem.ToList().Count > 0)
                                        cmbSpecialReg.SelectedValue = spcItem.ToList()[0].ID;
                                    //cmbSpecialReg.SelectedValue = 2;     // NO
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> religionList = new List<MasterListItem>();

                                if (cmbReligion.ItemsSource != null)
                                    religionList = ((List<MasterListItem>)cmbReligion.ItemsSource).ToList();

                                if (religionList != null)
                                {
                                    var religionItem = from r in religionList
                                                       where r.ID == objPatient.ReligionID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.Religion
                                                       select r;

                                    if (religionItem != null && religionItem.ToList().Count > 0)
                                        cmbReligion.SelectedValue = religionItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.Religion;

                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> prefList = new List<MasterListItem>();

                                if (cmbPreferredLanguage.ItemsSource != null)
                                    prefList = ((List<MasterListItem>)cmbPreferredLanguage.ItemsSource).ToList();

                                if (prefList != null)
                                {
                                    var prefItem = from r in prefList
                                                   where r.ID == objPatient.PreferredLangID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.PreferredLangID
                                                   select r;

                                    if (prefItem != null && prefItem.ToList().Count > 0)
                                        cmbPreferredLanguage.SelectedValue = prefItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.Religion;
                                    //cmbPreferredLanguage.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.PreferredLangID;
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> treatList = new List<MasterListItem>();

                                if (cmbTreatmentRequired.ItemsSource != null)
                                    treatList = ((List<MasterListItem>)cmbTreatmentRequired.ItemsSource).ToList();

                                if (treatList != null)
                                {
                                    var treatItem = from r in treatList
                                                    where r.ID == 2
                                                    select r;

                                    if (treatItem != null && treatItem.ToList().Count > 0)
                                        cmbTreatmentRequired.SelectedValue = treatItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.Religion;
                                    //cmbTreatmentRequired.SelectedValue = 2; // OTHERS
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> nationalityList = new List<MasterListItem>();

                                if (cmbNationality.ItemsSource != null)
                                    nationalityList = ((List<MasterListItem>)cmbNationality.ItemsSource).ToList();

                                if (nationalityList != null)
                                {
                                    var nationalityItem = from r in nationalityList
                                                          where r.ID == objPatient.NationalityID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID
                                                          select r;

                                    if (nationalityItem != null && nationalityItem.ToList().Count > 0)
                                        cmbNationality.SelectedValue = nationalityItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                    //cmbNationality.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> countryList = new List<MasterListItem>();

                                if (txtCountry.ItemsSource != null)
                                    countryList = ((List<MasterListItem>)txtCountry.ItemsSource).ToList();

                                if (countryList != null)
                                {
                                    var countryItem = from r in countryList
                                                      where r.ID == objPatient.CountryID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID
                                                      select r;

                                    if (countryItem != null && countryItem.ToList().Count > 0)
                                        txtCountry.SelectedValue = countryItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                    //cmbNationality.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> stateList = new List<MasterListItem>();

                                if (txtState.ItemsSource != null)
                                    stateList = ((List<MasterListItem>)txtState.ItemsSource).ToList();

                                if (stateList != null)
                                {
                                    var stateItem = from r in stateList
                                                    where r.ID == objPatient.StateID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID
                                                    select r;

                                    if (stateItem != null && stateItem.ToList().Count > 0)
                                        txtState.SelectedValue = stateItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                    //cmbNationality.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                }

                                //------------------------------------------------------------------------------

                                List<MasterListItem> cityList = new List<MasterListItem>();

                                if (txtCity.ItemsSource != null)
                                    cityList = ((List<MasterListItem>)txtCity.ItemsSource).ToList();

                                if (cityList != null)
                                {
                                    var cityItem = from r in cityList
                                                   where r.ID == objPatient.CityID    // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID
                                                   select r;

                                    if (cityItem != null && cityItem.ToList().Count > 0)
                                        txtCity.SelectedValue = cityItem.ToList()[0].ID; //((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                    //cmbNationality.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.NationalityID;
                                }

                                //------------------------------------------------------------------------------

                                txtContactNo1.Text = objPatient.ContactNo1;  // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.ContactNO1;
                                txtAddress1.Text = objPatient.AddressLine1;  // ((IApplicationConfiguration)App.Current).SelectedPatientForPediatric.AddressLine1;

                                cmbBloodGroup.Focus();

                                #endregion
                            }
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                        Indicatior.Close();
                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();
                }

            }
            catch (Exception ex)
            {
                Indicatior.Close();
                throw ex;
            }

        }

        private void btnSearchParent_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void txtContactNo1_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (((WaterMarkTextbox)sender).Text.Length == 1)
                    {
                        if (!((WaterMarkTextbox)sender).Text.IsValueNotZero())
                        {
                            ((WaterMarkTextbox)sender).Text = textBefore;
                            ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                            ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                            textBefore = "";
                            selectionStart = 0;
                            selectionLength = 0;
                        }
                    }
                    
                  else if (!((WaterMarkTextbox)sender).Text.IsItNumber()) //if (!((WaterMarkTextbox)sender).Text.IsMobileNumberValid() && textBefore != null)
                    {
                        //if (!((WaterMarkTextbox)sender).Text.IsItNumber())
                        //{
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                        // }
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 14)
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

        private void txtSpouseContactNo1_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;

        }

        private void txtSpouseContactNo1_OnTextChanged(object sender, RoutedEventArgs e)
        {
            if (IsPageLoded)
            {
                if (!string.IsNullOrEmpty(((WaterMarkTextbox)sender).Text))
                {
                    if (((WaterMarkTextbox)sender).Text.Length == 1)
                    {
                        if (!((WaterMarkTextbox)sender).Text.IsValueNotZero())
                        {
                            ((WaterMarkTextbox)sender).Text = textBefore;
                            ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                            ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                            textBefore = "";
                            selectionStart = 0;
                            selectionLength = 0;
                        }
                    }
                    else if (!((WaterMarkTextbox)sender).Text.IsItNumber()) //if (!((WaterMarkTextbox)sender).Text.IsMobileNumberValid() && textBefore != null)
                    {
                        //if (!((WaterMarkTextbox)sender).Text.IsItNumber())
                        //{
                        ((WaterMarkTextbox)sender).Text = textBefore;
                        ((WaterMarkTextbox)sender).SelectionStart = selectionStart;
                        ((WaterMarkTextbox)sender).SelectionLength = selectionLength;
                        textBefore = "";
                        selectionStart = 0;
                        selectionLength = 0;
                        // }
                    }
                    else if (((WaterMarkTextbox)sender).Text.Length > 14)
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


        //private void FillCamp()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    BizAction.MasterTable = MasterTableNameList.M_CampMaster;
        //    BizAction.MasterList = new List<MasterListItem>();
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();
        //            objList.Add(new MasterListItem(0, "-- Select --"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

        //            cmbCampDetail.ItemsSource = null;
        //            cmbCampDetail.ItemsSource = objList;

        //        }
        //        if (this.DataContext != null)
        //        {
        //            cmbCampDetail.SelectedValue = ((clsCampMasterVO)this.DataContext).CampID;


        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}




        private void FillCamp()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                //BizAction.IsActive = true;
                BizAction.MasterTable = MasterTableNameList.M_CampMaster;
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
                        cmbCampDetail.ItemsSource = null;
                        cmbCampDetail.ItemsSource = objList.DeepCopy();
                    }
                    if (this.DataContext != null)
                    {
                        cmbCampDetail.SelectedValue = ((clsPatientVO)this.DataContext).CampID;

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




        private void FillAgentMaster()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_AgentMaster;
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
                        cmbAgentType.ItemsSource = null;
                        cmbAgentType.ItemsSource = objList.DeepCopy();
                        if (cmbAgentType.IsEnabled == false)
                        {
                            cmbAgentType.SelectedItem = objList[0];
                        }
                    }
                    if (cmbAgentType.IsEnabled == true)
                    {
                        if (this.DataContext != null)
                        {
                            cmbAgentType.SelectedValue = ((clsPatientVO)this.DataContext).AgentID;

                        }
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

        private clsBankDetailsInfoVO SaveBankWithTransaction()
        {
            try
            {
                clsBankDetailsInfoVO BizAction = new clsBankDetailsInfoVO();
                BizAction = new clsBankDetailsInfoVO();
                BizAction.BankId = ((MasterListItem)Sponsortwin.cmbBankName.SelectedItem).ID;
                BizAction.BranchId = ((MasterListItem)Sponsortwin.cmbBranch.SelectedItem).ID;
                BizAction.AccountNumber = Convert.ToString(Sponsortwin.txtAccountNo.Text);
                BizAction.AccountHolderName = Convert.ToString(Sponsortwin.txtAccountHolderNm.Text);
                BizAction.IFSCCode = Convert.ToString(Sponsortwin.txtIFSCCode.Text);

                if (Sponsortwin.chkIfYesAccType.IsChecked == true)
                {
                    BizAction.AccountTypeId = true;
                }
                else
                {
                    BizAction.AccountTypeId = false;
                }

                return BizAction;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
        }



        private void txtNoOfExistingChildren_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtNoOfExistingChildren_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


        private void txtNoOfYearsOfMarriage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsValidTwoDigitWithTwoDecimal())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtNoOfYearsOfMarriage_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }


       private void Gynecologist_Click(object sender, RoutedEventArgs e)
        {
            if (chkIfYesrdo.IsChecked == true)
            {
                txtClinicName.IsEnabled = true;
            }
            else if (chkIfNordo.IsChecked == true)
            {
                txtClinicName.IsEnabled = false;
                txtClinicName.Text = string.Empty;
            }
        }



       private void FillSurrogateAgency()
       {
           try
           {
               clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
               BizAction.MasterTable = MasterTableNameList.M_SurrogateAgencyMaster;
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
                       cmbAgencyName.ItemsSource = null;
                       cmbAgencyName.ItemsSource = objList.DeepCopy();
                       if (cmbAgencyName.IsEnabled == false)
                       {
                           cmbAgencyName.SelectedItem = objList[0];
                       }
                   }
                   if (cmbAgencyName.IsEnabled == true)
                   {
                       if (this.DataContext != null)
                       {
                           cmbAgencyName.SelectedValue = ((clsPatientVO)this.DataContext).AgencyID;

                       }
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


       private void FillCoConsultantDoctor()
       {
           clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
           BizAction.MasterList = new List<MasterListItem>();
           BizAction.IsNonReferralDoctor = true;
          // BizAction.ReferralID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorTypeForReferral;
           //BizAction.UnitId = 0;//Commented By YK
           BizAction.UnitId = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID;//Added By YK to show doctors list respective centers
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

                   cmbCoConsultantDoctor.ItemsSource = null;
                   cmbCoConsultantDoctor.ItemsSource = objList;
                   cmbCoConsultantDoctor.SelectedItem = objList[0];

                   if (this.DataContext != null)
                   {
                       cmbCoConsultantDoctor.SelectedValue = ((clsPatientVO)this.DataContext).CoConsultantDoctorID;

                   }
               }

           };
           client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
           client.CloseAsync();
       }


        //***//--------------------------------
    }
}
