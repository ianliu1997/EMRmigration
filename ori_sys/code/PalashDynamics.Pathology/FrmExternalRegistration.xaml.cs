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
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;
using System.Reflection;
using CIMS.Forms;
using PalashDynamics.UserControls;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration;
using System.Collections.ObjectModel;
using System.Text;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.Pathology;
using System.Windows.Browser;
using PalashDynamics.Pathology.PathologyForms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using CIMS.Forms;
using PalashDynamics.ValueObjects.IPD;

namespace PalashDynamics.Pathology
{
    public partial class FrmExternalRegistration : UserControl,IInitiateCIMS
    {
        private bool PatientEditMode = false;
        bool IsPatientExist = false;
        DateTime? DOB = null;
        long BillID = 0;
        long UnitID = 0;

        public long PatientSourceID1 = 0;
        public long CompanyID1 = 0;
        public long PatientTariffID1 = 0;
        public clsBillVO SelectedBill { get; set; }
        public long PatientCategoryID1 = 0;
        public double TotalConcession = 0;
        ObservableCollection<clsChargeVO> ChargeList { get; set; }
       
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
                    PatientEditMode = false;
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
                    txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
                   
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID;
                    ((clsPatientVO)this.DataContext).GeneralDetails.PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                
                     //txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
                    break;

                case "EDIT":
                           #region Patient Edit Mode

                    PatientEditMode = true;

                            if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                            {
                                // System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                IsPatientExist = false;

                                break;
                            }

                            if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                            {
                                //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();

                                IsPatientExist = false;
                                break;
                            }
                            else
                            {
                                
                                clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
                                BizAction.PatientDetails = new clsPatientVO();
                                BizAction.PatientDetails.GeneralDetails.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                                BizAction.PatientDetails.GeneralDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                                BizAction.PatientDetails.GeneralDetails.VisitID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
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

                                            //     tabServiceInfo.Visibility = Visibility.Visible;
                                            this.DataContext = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;

                                            if (this.DataContext != null)
                                            {
                                                //   cmbPatientType.SelectedValue = ((clsPatientVO)this.DataContext).GeneralDetails.PatientTypeID;

                                                cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
                                              
                                                //  cmbReferenceDoctor.SelectedValue = ((clsPatientVO)this.DataContext).DoctorID;
                                            
                                                cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                                             
                                               

                                                txtContactNo1.Text = ((clsPatientVO)this.DataContext).ContactNo1;
                                                txtContactNo2.Text = ((clsPatientVO)this.DataContext).ContactNo2;
                                                txtMobileCountryCode.Text = ((clsPatientVO)this.DataContext).MobileCountryCode.ToString();
                                               
                                                txtResiCountryCode.Text = ((clsPatientVO)this.DataContext).ResiNoCountryCode.ToString();
                                                txtResiSTD.Text = ((clsPatientVO)this.DataContext).ResiSTDCode.ToString();
                                               
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                        msgW1.Show();
                                        ClickedFlag = 0;
                                    }
                                };
                                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                Client.CloseAsync();
                            }
                            #endregion
                    break;
            }
        }

        #endregion

      
        enum  RegistrationTabs
        {
            Patient = 0,
            Sponsor = 1
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
                        
                    }
                    if (this.DataContext != null)
                    {
                        cmbGender.SelectedValue = ((clsPatientVO)this.DataContext).GenderID;
                        
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

  
        private void FillMaritalStatus()
        {
            try
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
                    }
                    if (this.DataContext != null)
                    {
                        cmbMaritalStatus.SelectedValue = ((clsPatientVO)this.DataContext).MaritalStatusID;
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


        private void FillPatientCategory()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_CategoryL1Master;
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

                    cmbPatientCategory.ItemsSource = null;
                    cmbPatientCategory.ItemsSource = objList;
                    // cmbPatientCategory.SelectedItem = objList[0];

                    if (PatientCategoryID == 0)
                    {
                        cmbPatientCategory.SelectedItem = objList[0];
                    }
                    else if (PatientCategoryID != 0)
                    {
                        foreach (MasterListItem item in objList)
                        {
                            if (item.ID == PatientCategoryID)
                            {
                                cmbPatientCategory.SelectedItem = item;
                            }
                        }
                        // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                    }

                    //if (this.DataContext != null)
                    //{
                    //    //cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                    //    cmbPatientCategory.SelectedValue = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;

                    //    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);

                    //    if (((MasterListItem)cmbPatientCategory.SelectedItem).ID == 0)
                    //    {
                    //        cmbPatientCategory.TextBox.SetValidation("Please select the Patient Category");
                    //        cmbPatientCategory.TextBox.RaiseValidationError();
                    //    }
                    //}


                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }


        private void FillPatientSource(long PatientCategoryID)
        {
            clsGetPatientCategoryMasterVO BizAction = new clsGetPatientCategoryMasterVO();

            BizAction.PatientSourceID = PatientCategoryID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetPatientCategoryMasterVO)e.Result).List);
                }

                cmbPatientSource.ItemsSource = null;
                cmbPatientSource.ItemsSource = objList;
                //cmbPatientSource.SelectedItem = objList[0];

                if (PatientSourceID == 0)
                {
                    cmbPatientSource.SelectedItem = objList[0];
                }
                //else if (PatientSourceID != 0)
                //{
                //   // cmbPatientSource.SelectedValue = PatientSourceID;
                //    ((MasterListItem)cmbPatientSource.SelectedItem).ID = PatientSourceID;
                //}
                else if (PatientSourceID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientSourceID)
                        {
                            cmbPatientSource.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }

                //if (this.DataContext != null)
                //{
                //    cmbPatientSource.SelectedValue = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

                //    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);

                //    if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
                //    {
                //        cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");
                //        cmbPatientSource.TextBox.RaiseValidationError();
                //    }
                //}
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }
        public long CompanyID { get; set; }
        private void FillCompany(long PatientSourceID)
        {
            clsGetCompanyMasterVO BizAction = new clsGetCompanyMasterVO();

            BizAction.PatientCategoryID = PatientSourceID;
            //added by rohini
            BizAction.IsForPathology = true;
            BizAction.PathologyCompanyType=((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyCompanyTypeID;
            //
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetCompanyMasterVO)e.Result).List);
                }

                cmbCompany.ItemsSource = null;
                cmbCompany.ItemsSource = objList;
                //cmbCompany.SelectedItem = objList[0];

                if (CompanyID == 0)
                {
                    cmbCompany.SelectedItem = objList[0];
                }
                //else if (CompanyID != 0)
                //{
                //  // cmbCompany.SelectedValue = CompanyID;
                //    ((MasterListItem)cmbCompany.SelectedItem).ID = CompanyID;
                //}
                else if (CompanyID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == CompanyID)
                        {
                            cmbCompany.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
                //if (this.DataContext != null)
                //{
                //    //cmbPatientCategory.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                //    cmbCompany.SelectedValue = ((MasterListItem)cmbCompany.SelectedItem).ID;

                //    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);

                //    if (((MasterListItem)cmbCompany.SelectedItem).ID == 0)
                //    {
                //        cmbCompany.TextBox.SetValidation("Please select the Company");
                //        cmbCompany.TextBox.RaiseValidationError();
                //    }
                //}


            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

        }
 
        private void FillTariffMaster(long pPatientsourceID, long pCompanyID)
        {
            clsGetTariffMasterVO BizAction = new clsGetTariffMasterVO();

            BizAction.CompanyID = pCompanyID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                List<MasterListItem> objList = new List<MasterListItem>();

                objList.Add(new MasterListItem(0, "- Select -"));
                if (e.Error == null && e.Result != null)
                {
                    objList.AddRange(((clsGetTariffMasterVO)e.Result).List);
                }

                cmbTariff.ItemsSource = null;
                cmbTariff.ItemsSource = objList;
                //cmbTariff.SelectedItem = objList[0];

                if (PatientTariffID == 0)
                {
                    cmbTariff.SelectedValue = objList[0];
                }
                //else if (PatientTariffID != 0)
                //{
                //   // cmbTariff.SelectedValue = PatientTariffID;
                //    ((MasterListItem)cmbTariff.SelectedItem).ID = PatientTariffID;
                //}
                else if (PatientTariffID != 0)
                {
                    foreach (MasterListItem item in objList)
                    {
                        if (item.ID == PatientTariffID)
                        {
                            cmbTariff.SelectedItem = item;
                        }
                    }
                    // ((MasterListItem)cmbPatientCategory.SelectedItem).ID = PatientCategoryID;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);

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
                        
                    }
                    if (this.DataContext != null)
                    {
                        cmbIdentity.SelectedValue = ((clsPatientVO)this.DataContext).IdentityID;
                        
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
        WaitIndicator wait = new WaitIndicator();
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

                    FillIdentity();
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

        public FrmExternalRegistration()
        {
            InitializeComponent();
            dgCharges.CellEditEnded += new EventHandler<DataGridCellEditEndedEventArgs>(dgCharges_CellEditEnded);
           
        }
        void dgCharges_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            //throw new NotImplementedException();
            try
            {
                if (e.Column.Header != null)
                {
                    
                    
                        if (dgCharges.SelectedItem != null)
                        {
                            if (((clsChargeVO)dgCharges.SelectedItem).Concession > ((clsChargeVO)dgCharges.SelectedItem).TotalAmount)
                            {
                                ((clsChargeVO)dgCharges.SelectedItem).Concession = ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;

                                string msgTitle = "";
                                string msgText = "Concession amount should not be greater than Total Amount " + ((clsChargeVO)dgCharges.SelectedItem).TotalAmount;

                                MessageBoxControl.MessageBoxChildWindow msgWD =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgWD.Show();
                            }
                        }
                   
                           CalculateClinicalSummary();
                }
            }
            catch (Exception)
            {
                throw;
            }
            //if (e.Column.DisplayIndex == 3 | e.Column.DisplayIndex == 4 | e.Column.DisplayIndex == 5 | e.Column.DisplayIndex == 6)
            //    CalculateClinicalSummary();

        }

        private void frmPatientRegistration_Loaded(object sender, RoutedEventArgs e)
        {
            ChargeList = new ObservableCollection<clsChargeVO>();
            dgCharges.ItemsSource = ChargeList;
            dgCharges.Focus();
            dgCharges.UpdateLayout();
            chkNewPatient.IsChecked = true;
            btnSearchCriteria.IsEnabled = false;
            FillPreffix();
            FillMaritalStatus();
            FillPatientCategory();
            FillRalations();
            FillGender();
            cmbPreffix.Focus();

            //ADDED BY ROHINI DATED 4.4.16
            CheckValidations();
            //
            if (chkNewPatient.IsChecked == true)
            {
                PatientSourceID = 0;
                PatientCategoryID = 0;
                PatientTariffID = 0;
                CompanyID = 0;               
            }
            else if (chkExPatient.IsChecked == true)
            {
                PatientSourceID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                PatientCategoryID = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                PatientTariffID = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                CompanyID = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
            }
            txtMobileCountryCode.Text = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DefaultCountryCode;
        }

        private void lnkAddServices_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void PatPersonalInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RegistrationTabs SelectedTab = (RegistrationTabs)PatPersonalInfo.SelectedIndex;

            //switch (SelectedTab)
            //{
            //    case RegistrationTabs.Patient:
            //        grpServiceDetails.Visibility = Visibility.Visible;
            //        break;
            //    case RegistrationTabs.Sponsor:
            //        grpServiceDetails.Visibility = Visibility.Visible;
            //        break;
                
            //}
        }


        private void btnSearchCriteria_Click(object sender, RoutedEventArgs e)
        {
            //comented on 1-6-17
            FrmPatientSearch Win = new FrmPatientSearch();
            Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            Win.Show();

            //PatientSearch Win = new PatientSearch();           
            //Win.isfromCouterSale = true;          
            //Win.OnSaveButton_Click += new RoutedEventHandler(PatientSearch_OnSaveButton_Click);
            //Win.Show();
        }
        long ServiceTariffID = 0;
        long VisitId = 0;
        long PatientId = 0;
        long PackageTariffID = 0;

        void PatientSearch_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //add lines for get selected patient by rohini 06/01/2017
            clsPatientGeneralVO Getpatient = new clsPatientGeneralVO();
            Getpatient = ((IApplicationConfiguration)App.Current).SelectedPatient;

            clsGetActiveAdmissionBizActionVO BizObject = new clsGetActiveAdmissionBizActionVO();
            BizObject.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizObject.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizObject.MRNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            Client1.ProcessCompleted += (s1, arg1) =>
            {
                if (arg1.Error == null)
                {
                    if (arg1.Result != null)
                    {
                        if (((clsGetActiveAdmissionBizActionVO)arg1.Result).Details != null && ((clsGetActiveAdmissionBizActionVO)arg1.Result).Details.AdmID > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient is already admitted.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }
                        else
                        {
                            #region
                            try
                            {
                            
                                //if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
                                //{
                                if(Getpatient!=null && Getpatient.PatientID !=0)
                                {
                                    WaitIndicator Indicatior = new WaitIndicator();
                                    Indicatior.Show();
                              

                                    clsGetPatientDetailsForPathologyBizActionVO BizAction = new clsGetPatientDetailsForPathologyBizActionVO();
                                    BizAction.PatientDetails = new clsPatientVO();
                                    BizAction.MRNO = Getpatient.MRNo;//((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                    BizAction.UnitID = Getpatient.UnitId;// ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                                    VisitId = Getpatient.VisitID;// ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;

                                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                                    Client.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                clsGetPatientDetailsForPathologyBizActionVO ObjPatient = new clsGetPatientDetailsForPathologyBizActionVO();
                                                ObjPatient = (clsGetPatientDetailsForPathologyBizActionVO)arg.Result;
                                                PatientId = ObjPatient.PatientDetails.GeneralDetails.PatientID;

                                                txtMRNo.Text = ObjPatient.PatientDetails.GeneralDetails.MRNo;
                                                txtFirstName.Text = ObjPatient.PatientDetails.GeneralDetails.FirstName;
                                                if (ObjPatient.PatientDetails.GeneralDetails.MiddleName != null)
                                                {
                                                    txtMiddleName.Text = ObjPatient.PatientDetails.GeneralDetails.MiddleName;
                                                }
                                                txtLastName.Text = ObjPatient.PatientDetails.GeneralDetails.LastName;

                                                if (ObjPatient.PatientDetails.GeneralDetails.IsAge == false)
                                                {
                                                    dtpDOB.SelectedDate = ObjPatient.PatientDetails.GeneralDetails.DateOfBirth;
                                                    txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                                                    txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                                                    txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
                                                }
                                                else
                                                {
                                                    DOB = ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge;
                                                    txtYY.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "YY");
                                                    txtMM.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "MM");
                                                    txtDD.Text = ConvertDate(ObjPatient.PatientDetails.GeneralDetails.DateOfBirthFromAge, "DD");
                                                }

                                                cmbPreffix.SelectedValue = ObjPatient.PatientDetails.PrefixId;
                                                cmbMaritalStatus.SelectedValue = ObjPatient.PatientDetails.MaritalStatusID;
                                                cmbGender.SelectedValue = ObjPatient.PatientDetails.GenderID;
                                                txtMobileCountryCode.Text = ObjPatient.PatientDetails.MobileCountryCode.ToString();
                                                txtContactNo1.Text = ObjPatient.PatientDetails.ContactNo1.ToString();
                                                // txtReferenceDoctor.Text = ObjPatient.PatientDetails.Doctor.ToString();
                                                cmbPatientCategory.SelectedValue = ObjPatient.PatientDetails.PatientSponsorCategoryID;
                                                txtEmail.Text = ObjPatient.PatientDetails.Email.ToString();

                                                txtFirstName.IsEnabled = false;
                                                txtMiddleName.IsEnabled = false;
                                                txtLastName.IsEnabled = false;
                                                cmbGender.IsEnabled = false;
                                                cmbPreffix.IsEnabled = false;
                                                cmbMaritalStatus.IsEnabled = false;
                                                dtpDOB.IsEnabled = false;
                                                txtMobileCountryCode.IsEnabled = false;
                                                txtContactNo1.IsEnabled = false;
                                                txtYY.IsEnabled = false;
                                                txtMM.IsEnabled = false;
                                                txtDD.IsEnabled = false;
                                                txtEmail.IsEnabled = false;
                                                //added by rohini dated 15.4.16
                                                PatientSourceID = Getpatient.PatientSourceID; //((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;
                                                PatientCategoryID = Getpatient.NewPatientCategoryID; //((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                                                PatientTariffID = Getpatient.TariffID;//((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                                                CompanyID = Getpatient.CompanyID;//((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                                                FillPatientCategory();
                                                //
                                                if (((clsGetPatientDetailsForPathologyBizActionVO)arg.Result).PatientBillDetailList != null)
                                                {
                                                    if (((clsGetPatientDetailsForPathologyBizActionVO)arg.Result).PatientBillDetailList[0].IsFreezed == true)
                                                    {


                                                    }
                                                    else
                                                    {
                                                        dgCharges.ItemsSource = ((clsGetPatientDetailsForPathologyBizActionVO)arg.Result).PatientBillDetailList;
                                                        dgCharges.Focus();
                                                        dgCharges.UpdateLayout();
                                                    }
                                                    CalculateClinicalSummary();
                                                }

                                                ((IApplicationConfiguration)App.Current).SelectedPatient = Getpatient;
                                                CheckVisit();
                                            }
                                        }
                                        else
                                        {
                                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured While Adding Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                                            msgW1.Show();
                                            ClickedFlag = 0;
                                        }
                                        Indicatior.Close();
                                    };

                                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    Client.CloseAsync();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            #endregion
                        }
                    }
                }
            };
            Client1.ProcessAsync(BizObject, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
        

        }

        public bool IsVisit = false;
        public long VisitForExistingID = 0;
        public long VisitForExistingUnitID = 0;
        private void CheckVisit()
        {
            WaitIndicator ind = new WaitIndicator();
            ind.Show();

            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
            BizAction.GetLatestVisit = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    if (((clsGetVisitBizActionVO)arg.Result).Details != null && ((clsGetVisitBizActionVO)arg.Result).Details.ID > 0 && ((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                    {
                        VisitForExistingID = ((clsGetVisitBizActionVO)arg.Result).Details.ID;
                        VisitForExistingUnitID = ((clsGetVisitBizActionVO)arg.Result).Details.UnitId;
                        IsVisit = true;
                        ind.Close();
                    }
                    else
                    {
                        IsVisit = false;
                        ind.Close();
                        return;
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void AddCharges(List<clsServiceMasterVO> mServices, long TariffID)
        {
            
            StringBuilder strError = new StringBuilder();

            for (int i = 0; i < mServices.Count; i++)
            {
                bool Addcharge = true;

                if (ChargeList != null && ChargeList.Count > 0)
                {
                    var item = from r in ChargeList
                               where r.ServiceId == mServices[i].ID
                               select new clsChargeVO
                               {
                                   Status = r.Status,
                                   ID = r.ID,
                                   ServiceName = r.ServiceName
                               };

                    if (item.ToList().Count > 0)
                    {
                        if (strError.ToString().Length > 0)
                            strError.Append(",");
                        strError.Append(item.ToList()[0].ServiceName);
                        Addcharge = false;

                    }
                }

                if (Addcharge)
                {
                    clsChargeVO itemC = new clsChargeVO();
                    itemC.Service = mServices[i].ServiceName;
                    itemC.TariffId = mServices[i].TariffID;
                    itemC.ServiceSpecilizationID = mServices[i].Specialization;
                    itemC.TariffServiceId = mServices[i].TariffServiceMasterID;
                    itemC.ServiceId = mServices[i].ID;
                    itemC.ServiceName = mServices[i].ServiceName;
                    itemC.Quantity = 1;
                    itemC.RateEditable = mServices[i].RateEditable;
                    itemC.MinRate = Convert.ToDouble(mServices[i].MinRate);
                    itemC.MaxRate = Convert.ToDouble(mServices[i].MaxRate);
                    itemC.Rate = Convert.ToDouble(mServices[i].Rate);
                    itemC.PatientSourceID = mServices[i].PatientSourceID;
                    itemC.CompanyID = mServices[i].CompanyID;
                    itemC.TotalAmount = itemC.Rate * itemC.Quantity;
                    itemC.ServiceCode = mServices[i].ServiceCode;

                    //if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 3 || ((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 6)
                    //{
                    //    if (mServices[i].StaffDiscountPercent > 0)
                    //        itemC.StaffDiscountPercent = Convert.ToDouble(mServices[i].StaffDiscountPercent);
                    //    else
                    //        itemC.StaffDiscountAmount = Convert.ToDouble(mServices[i].StaffDiscountAmount);

                    //    if (mServices[i].StaffDependantDiscountPercent > 0)
                    //        itemC.StaffParentDiscountPercent = Convert.ToDouble(mServices[i].StaffDependantDiscountPercent);
                    //    else
                    //        itemC.StaffParentDiscountAmount = Convert.ToDouble(mServices[i].StaffDependantDiscountAmount);

                    //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ApplyConcessionToStaff == true)
                    //    {
                    //        if (mServices[i].ConcessionPercent > 0)
                    //        {
                    //            itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                    //        }
                    //        else
                    //            itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                    //    }
                    //}
                 
                        if (mServices[i].ConcessionPercent > 0)
                        {
                            itemC.ConcessionPercent = Convert.ToDouble(mServices[i].ConcessionPercent);
                        }
                        else
                            itemC.Concession = Convert.ToDouble(mServices[i].ConcessionAmount);
                  


                    if (mServices[i].ServiceTaxPercent > 0)
                        itemC.ServiceTaxPercent = Convert.ToDouble(mServices[i].ServiceTaxPercent);
                    else
                        itemC.ServiceTaxAmount = Convert.ToDouble(mServices[i].ServiceTaxAmount);

                    if (mServices[i].ConditionTypeID > 0)
                    {
                        itemC.ConditionTypeID = mServices[i].ConditionTypeID;
                        itemC.ConditionType = mServices[i].ConditionType;
                    }


                    if (ChargeList == null)
                        ChargeList = new ObservableCollection<clsChargeVO>();

                    ChargeList.Add(itemC);
                  
                }
            }

            CalculateClinicalSummary();

            dgCharges.Focus();
            dgCharges.UpdateLayout();

            dgCharges.SelectedIndex = ChargeList.Count - 1;

            if (!string.IsNullOrEmpty(strError.ToString()))
            {
                string strMsg = "Services already added : " + strError.ToString();

                MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
            }
        }

        private void CalculateClinicalSummary()
        {
            double clinicalTotal, clinicalConcession, clinicalNetAmount;
            clinicalTotal = clinicalConcession = clinicalNetAmount =  0;

            for (int i = 0; i < ChargeList.Count; i++)
            {
                clinicalTotal += (ChargeList[i].TotalAmount);
                clinicalConcession += ChargeList[i].Concession;
                clinicalNetAmount += ChargeList[i].NetAmount;
            }
            txtClinicalTotal.Text = String.Format("{0:0.00}", clinicalTotal);
            txtClinicalConcession.Text = String.Format("{0:0.00}", clinicalConcession);
            txtClinicalNetAmount.Text = String.Format("{0:0.00}", clinicalNetAmount);
            
        }

        private void FillRalations()
        {
            try
            {
                clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
                BizAction.MasterTable = MasterTableNameList.M_RelationMaster;
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
                        List<MasterListItem> objRelList = new List<MasterListItem>();
                        objRelList = new List<MasterListItem>();

                        objRelList.Add(new MasterListItem(0, "-- Select --"));
                        objRelList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);

                    //    cmbPackageRelations.ItemsSource = null;
                       // cmbPackageRelations.ItemsSource = objRelList;
                       // cmbPackageRelations.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfRelationID;

                        //var list = objRelList.FirstOrDefault(a => a.ID == 1);
                        //cmbPackageRelations.SelectedItem = list;
                        //cmbPackageRelations.SelectedItem = objRelList[0];

                        //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientCategoryID == 7)
                        //{

                        //if (PatientTypeID == 7)
                        //{
                        //var ls = objRelList.FirstOrDefault(a => a.ID == 2);
                        //cmbPackageRelations.SelectedItem = ls;         
                        //|| a.ID==2
                        //var ls = objRelList.Where(a => a.Description == "Self");
                        //cmbPackageRelations.SelectedItem = ls.ToList()[0];
                        //}
                    }
                    


                };

                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser); //new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsNewPatient = false;
        public bool IsExPatient = false;
        private void lnkAddServices_Click_1(object sender, RoutedEventArgs e)
        {
            if (((MasterListItem)cmbPatientCategory.SelectedItem).ID != 0 && ((MasterListItem)cmbPatientSource.SelectedItem).ID != 0 && ((MasterListItem)cmbCompany.SelectedItem).ID != 0 && ((MasterListItem)cmbTariff.SelectedItem).ID != 0)
            {
                if (chkNewPatient.IsChecked == true)
                {
                    IsNewPatient = true;
                }
                else if (chkExPatient.IsChecked == true)
                {
                    IsExPatient = true;
                }
                ServiceSearch Searchwin = new ServiceSearch();
                //added by rohini
                Searchwin.PatientCategoryID1 = ((MasterListItem)cmbPatientCategory.SelectedItem).ID;
                Searchwin.PatientSourceID1 = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
                Searchwin.CompanyID1 = ((MasterListItem)cmbCompany.SelectedItem).ID;
                Searchwin.PatientTariffID1 = ((MasterListItem)cmbTariff.SelectedItem).ID;

                Searchwin.IsFromNew = IsNewPatient;
                Searchwin.IsFromEx = IsExPatient;
                Searchwin.OnAddButton_Click += new
                     RoutedEventHandler(ServiceSearch_AddClick);
                Searchwin.Show();

                //comented by rohini 4.3.16
                //ServiceSerachNew serviceSearch = null;
                //serviceSearch = new ServiceSerachNew();
                //serviceSearch.OnAddButton_Click += new
                //     RoutedEventHandler(ServiceSearch_AddClick);
                //serviceSearch.Show();

                //ServiceSerachNew serviceSearch = null;
                //serviceSearch = new ServiceSerachNew();
                //serviceSearch.OnAddButton_Click += new RoutedEventHandler(servicePackageSearch_OnAddButton_Click);
                //serviceSearch.Show();
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The All Sponser Details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
                ClickedFlag = 0;
            }
        }
        public List<clsUnavailableItemStockServiceId> UnavailableItemStockService { get; set; }

        public class clsUnavailableItemStockServiceId
        {
            public long ServiceID { get; set; }
        }
        bool ConcessionFromPlan = false;
        long? PatientCategoryID { get; set; }


        long PatientSourceID { get; set; }
        long PatientTariffID { get; set; }
        void servicePackageSearch_OnAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (((ServiceSerachNew)sender).DialogResult == true)   //if (((PackageServiceSearch)sender).DialogResult == true)
            {
                List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
                lServices = ((ServiceSerachNew)sender).SelectedOtherServices.ToList();   //lServices = ((PackageServiceSearch)sender).SelectedOtherServices.ToList();

                #region clsApplyPackageDiscountRateOnServiceBizActionVO

                clsApplyPackageDiscountRateOnServiceBizActionVO objApplyNewRate = new clsApplyPackageDiscountRateOnServiceBizActionVO();

                objApplyNewRate.objApplyPackageDiscountRate = new List<clsApplyPackageDiscountRateOnServiceVO>();
                objApplyNewRate.ipServiceList = new List<clsServiceMasterVO>();


                objApplyNewRate.ipLoginUnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                objApplyNewRate.ipPatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                objApplyNewRate.ipPatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;


                objApplyNewRate.ipVisitID = VisitID;

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

                if ((ServiceSerachNew)sender != null && ((ServiceSerachNew)sender).cmbPatientSource.SelectedItem != null && ((clsPatientSponsorVO)((ServiceSerachNew)sender).cmbPatientSource.SelectedItem).MemberRelationID > 0)
                    objApplyNewRate.MemberRelationID = ((clsPatientSponsorVO)((ServiceSerachNew)sender).cmbPatientSource.SelectedItem).MemberRelationID;

                objApplyNewRate.SponsorID = ((MasterListItem)((ServiceSerachNew)sender).cmbTariff.SelectedItem).SponsorID;
                objApplyNewRate.SponsorUnitID = ((MasterListItem)((ServiceSerachNew)sender).cmbTariff.SelectedItem).SponsorUnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        objApplyNewRate = (clsApplyPackageDiscountRateOnServiceBizActionVO)arg.Result;


                        UnavailableItemStockService = new List<clsUnavailableItemStockServiceId>();

                        foreach (clsServiceMasterVO item in lServices)
                        {
                            if (item.ConditionID > 0)
                            {
                                if (item.ConditionalQuantity > item.ConditionalUsedQuantity && item.ConditionType == "AND" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)   //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                {
                                    item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                    ConcessionFromPlan = true;
                                }
                                else if (item.ConditionalQuantity > (item.ConditionalUsedQuantity + item.MainServiceUsedQuantity) && item.ConditionType == "OR" && item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0)  //if (item.ConditionalQuantity > item.ConditionalUsedQuantity)
                                {
                                    item.ConcessionPercent = 100;   //Convert.ToDecimal(item1.DiscountedPercentage);
                                    ConcessionFromPlan = true;
                                }
                                else
                                {
                                    if ((item.IsAgeApplicable == true && item.ServiceMemberRelationID > 0))  //&& item.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                                    {
                                        item.ConcessionAmount = 0;
                                        item.ConcessionPercent = Convert.ToDecimal(item.ConditionalDiscount);
                                        ConcessionFromPlan = true;
                                    }

                                    if ((item.IsAgeApplicable == false || item.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                    {
                                        item.ConcessionPercent = 0;
                                        item.ConcessionAmount = 0;
                                    }


                                }
                            }
                            else
                            {
                                foreach (clsApplyPackageDiscountRateOnServiceVO item1 in objApplyNewRate.objApplyPackageDiscountRate)
                                {
                                    if (item.ID == item1.ServiceID && item.TariffID == item1.TariffID && item.ConditionID == 0)
                                    {



                                        if (item1.IsApplyOn_Rate_Percentage == 1 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                        {
                                            item.Rate = Convert.ToDecimal(item1.DiscountedRate);

                                            if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                            {
                                                item.ConcessionAmount = 0;
                                                item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                            }

                                            if (item1.IsDiscountOnQuantity == true || item1.ServiceMemberRelationID == 0 || item1.IsAgeApplicable == false)
                                            {
                                                item.ConcessionPercent = 0;
                                                item.ConcessionAmount = 0;
                                            }
                                        }



                                        if (item1.IsApplyOn_Rate_Percentage == 2 || item1.IsApplyOn_Rate_Percentage == 3 && item.ConditionID == 0)
                                        {
                                            if (item1.IsDiscountOnQuantity == false)
                                            {
                                                if (item.PackageServiceConditionID == 0)
                                                {
                                                    if (item1.ActualQuantity > item1.UsedQuantity && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                    }
                                                    else
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }

                                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 &&
                                                        {
                                                            item.ConcessionPercent = 0;
                                                            item.ConcessionAmount = 0;
                                                        }
                                                    }
                                                }
                                                else if (item.PackageServiceConditionID > 0)
                                                {
                                                    if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                    {
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.DiscountedPercentage);

                                                        ConcessionFromPlan = true; //Added by priyanka-to set validation on concession
                                                    }
                                                    else
                                                    {
                                                        if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                        {
                                                            item.ConcessionAmount = 0;
                                                            item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                        }

                                                        if ((item1.IsAgeApplicable == false || item1.ServiceMemberRelationID == 0))  //item1.ActualQuantity > 0 && 
                                                        {
                                                            item.ConcessionPercent = 0;
                                                            item.ConcessionAmount = 0;
                                                        }
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (item1.ActualQuantity > (item1.UsedQuantity + item.TotalORUsedQuantity) && item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0)
                                                {
                                                    if ((item1.IsAgeApplicable == true && item1.ServiceMemberRelationID > 0) && item1.TariffID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID)
                                                    {
                                                        item.ConcessionAmount = 0;
                                                        item.ConcessionPercent = Convert.ToDecimal(item1.ConcessionPercentage);
                                                    }
                                                }
                                                else
                                                {
                                                    item.ConcessionPercent = 0;
                                                    item.ConcessionAmount = 0;
                                                }
                                            }
                                        }


                                        if (item.IsMarkUp == true && item1.IsServiceItemStockAvailable != true)
                                        {
                                            UnavailableItemStockService.Add(new clsUnavailableItemStockServiceId { ServiceID = item.ServiceID });
                                        }




                                    }
                                }
                            }
                        }

                        StringBuilder sbServiceName = new StringBuilder();

                        foreach (clsUnavailableItemStockServiceId item in UnavailableItemStockService)
                        {
                            clsServiceMasterVO obj = new clsServiceMasterVO();
                            obj = (clsServiceMasterVO)lServices.First(z => z.ServiceID == item.ServiceID && z.IsMarkUp == true);

                            sbServiceName.Append(obj.ServiceName + ",");

                            lServices.Remove(obj);
                        }

                        if (sbServiceName.Length > 0)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgWD =
                                            new MessageBoxControl.MessageBoxChildWindow("", "Items are not available for Service : " + sbServiceName, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgWD.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {

                                    PatientCategoryID = ((ServiceSerachNew)sender).PatientCategoryL1Id_Retail;  //PatientCategoryID = ((PackageServiceSearch)sender).PatientCategoryL1Id_Retail;
                                    PatientSourceID = ((clsPatientSponsorVO)((ServiceSerachNew)sender).cmbPatientSource.SelectedItem).PatientSourceID;  //PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                                    PatientTariffID = ((MasterListItem)((ServiceSerachNew)sender).cmbTariff.SelectedItem).ID;  //PatientTariffID = ((MasterListItem)((PackageServiceSearch)sender).cmbTariff.SelectedItem).ID;

                                    PackageTariffID = ((ServiceSerachNew)sender).PackageTariffID;  //PackageTariffID = ((PackageServiceSearch)sender).PackageTariffID;
                                    ServiceTariffID = ((ServiceSerachNew)sender).ServiceTariffID;    //ServiceTariffID = ((PackageServiceSearch)sender).ServiceTariffID;

                                    //PatientCatID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientCategoryID;

                                    AddCharges(lServices, ServiceTariffID);

                                }
                            };
                            msgWD.Show();
                        }
                        else
                        {

                            PatientCategoryID = ((ServiceSerachNew)sender).PatientCategoryL1Id_Retail;   //PatientCategoryID = ((PackageServiceSearch)sender).PatientCategoryL1Id_Retail;
                            PatientSourceID = ((clsPatientSponsorVO)((ServiceSerachNew)sender).cmbPatientSource.SelectedItem).PatientSourceID;    //PatientSourceID = ((clsPatientSponsorVO)((PackageServiceSearch)sender).cmbPatientSource.SelectedItem).PatientSourceID;
                            PatientTariffID = ((MasterListItem)((ServiceSerachNew)sender).cmbTariff.SelectedItem).ID;   //PatientTariffID = ((MasterListItem)((PackageServiceSearch)sender).cmbTariff.SelectedItem).ID;

                            PackageTariffID = ((ServiceSerachNew)sender).PackageTariffID;   //PackageTariffID = ((PackageServiceSearch)sender).PackageTariffID;
                            ServiceTariffID = ((ServiceSerachNew)sender).ServiceTariffID;     //ServiceTariffID = ((PackageServiceSearch)sender).ServiceTariffID;

                            // PatientCatID = ((clsPatientSponsorVO)((PackageServiceSearchForPackage)sender).cmbPatientSource.SelectedItem).PatientCategoryID;

                            AddCharges(lServices, ServiceTariffID);

                        }




                    }
                };
                client.ProcessAsync(objApplyNewRate, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                #endregion
            }
        }
        void ServiceSearch_AddClick(object sender, RoutedEventArgs e)
        {
            //List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            //lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
            //ServiceTariffID = ((ServiceSearch)sender).ServiceTariffID;
            //PatientSourceID1 = ((ServiceSearch)sender).PatientSourceID;
            //CompanyID1 = ((ServiceSearch)sender).CompanyID;
            //PatientCategoryID1 = ((ServiceSearch)sender).PatientCategoryID;
            //AddCharges(lServices, ServiceTariffID);

            //added by rohini
            List<clsServiceMasterVO> lServices = new List<clsServiceMasterVO>();
            lServices = ((ServiceSearch)sender).SelectedOtherServices.ToList();
            ServiceTariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
            PatientSourceID1 = ((MasterListItem)cmbPatientSource.SelectedItem).ID;
            CompanyID1 = ((MasterListItem)cmbCompany.SelectedItem).ID;
            PatientCategoryID1 = ((MasterListItem)cmbPatientCategory.SelectedItem).ID; 
            AddCharges(lServices, ServiceTariffID);

        }


         int ClickedFlag = 0;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (ChargeList != null && ChargeList.Count == 0 && ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID == 0)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                       new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Service...", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                    msgW1.Show();
                    ClickedFlag = 0;
                }                
                else
                {
                    bool saveDtls = true;
                    saveDtls = CheckValidations();

                    if (saveDtls == true)
                    {
                            string msgTitle = "";
                            string msgText = "";
                            msgText = "Are you sure you want to save the Patient Details";
                            MessageBoxControl.MessageBoxChildWindow msgW =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                            msgW.Show();
                    }
                    else
                        ClickedFlag = 0;

                }
            }
        }



        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                if (IsVisit == false && txtMRNo.Text.Trim() == string.Empty)
                {
                    //comented by rohini
                    //SavePatient();

                    //added by rohini
                    if (txtMRNo.Text == "")
                        checkDuplication(null, true);                    
                        
                }
                else
                {
                    if (chkFreezBill.IsChecked == true)
                    {
                        if (txtMRNo.Text.Trim() == string.Empty)
                            checkDuplication(null, true);
                        else 
                            SaveFreezBills();
                    }
                    else
                    {
                        SaveBill(null,false);
                    }

                }
            }
            else
                ClickedFlag = 0;
        }
        void checkDuplication(clsPaymentVO pPayDetails, bool pFreezBill)
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

            BizAction.PatientDetails.SpouseDetails.GenderID = 0;
            BizAction.PatientDetails.SpouseDetails.DateOfBirth = null;
            BizAction.PatientDetails.SpouseDetails.MobileCountryCode = null;
            BizAction.PatientDetails.SpouseDetails.ContactNO1 = null;


            BizAction.PatientEditMode = false;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus != 0)
                    {
                        ClickedFlag = 0;
                        string strDuplicateMsg = "";
                        if (((clsCheckPatientDuplicacyBizActionVO)ea.Result).SuccessStatus == 3)
                        {
                            //comented for temprory pourpose by rohini

                            strDuplicateMsg = "Mobile Number already exists, Are you sure you want to Continue ?";

                            MessageBoxControl.MessageBoxChildWindow msgW1 = new MessageBoxControl.MessageBoxChildWindow("Palash", strDuplicateMsg, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                            {
                                if (res == MessageBoxResult.Yes)
                                {
                                    SaveFreezBills();
                                    // SaveDetails(pPayDetails, pFreezBill, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);
                                }

                            };
                            msgW1.Show();

                            
                           
                            
                        }
                        else
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient already exists.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                            ClickedFlag = 0;
                        }
                    }
                    else
                    {                        
                      //  SaveDetails(pPayDetails, pFreezBill, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient == null ? 0 : ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID);
                        SaveFreezBills();

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }
       public long VisitID1 = 0;
       public long VisitUnitID = 0;
        private void SavePatient()
        {
             WaitIndicator Indicatior = new WaitIndicator();
             Indicatior.Show();
         
                clsAddPatientForPathologyBizActionVO BizAction = new clsAddPatientForPathologyBizActionVO();
               
                BizAction.PatientDetails = (clsPatientVO)this.DataContext;
                if (cmbMaritalStatus.SelectedItem != null)
                    BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
                  if (cmbGender.SelectedItem != null)
                    BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
                  if (cmbPreffix.SelectedItem != null)
                    BizAction.PatientDetails.PrefixId = ((MasterListItem)cmbPreffix.SelectedItem).ID;

                  BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
                  BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
                  if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                      BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

                  if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                      BizAction.PatientDetails.ResiNoCountryCode = Convert.ToInt64(txtResiCountryCode.Text.Trim());

                  if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                      BizAction.PatientDetails.ResiSTDCode = Convert.ToInt64(txtResiSTD.Text.Trim());


                  BizAction.PatientDetails.GeneralDetails.RegType = (short)PatientRegistrationType.Pathology;

                if (dtpDOB.SelectedDate == null)
                {
                    BizAction.PatientDetails.GeneralDetails.IsAge = true;
                    if (DOB != null)
                        BizAction.PatientDetails.GeneralDetails.DateOfBirth = DOB.Value.Date;
                }

                BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
                BizAction.PatientDetails.PatientCategoryIDForPath = PatientCategoryID1;

                BizAction.BizActionVOSaveSponsor = SaveSponsorWithTransaction();
                BizAction.PatientDetails.IsVisitForPatho = IsVisit;
                BizAction.BizActionVOSaveVisit = SaveVisitWithTransaction();
               
               
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        CmdSave.IsEnabled = false;
                        ((clsPatientVO)this.DataContext).GeneralDetails.PatientID = ((clsAddPatientForPathologyBizActionVO)arg.Result).PatientDetails.GeneralDetails.PatientID;
                        ((clsPatientVO)this.DataContext).GeneralDetails.MRNo = ((clsAddPatientForPathologyBizActionVO)arg.Result).PatientDetails.GeneralDetails.MRNo;
                        ((clsPatientVO)this.DataContext).GeneralDetails.UnitId = ((clsAddPatientForPathologyBizActionVO)arg.Result).PatientDetails.GeneralDetails.UnitId;

                        if (((clsAddPatientForPathologyBizActionVO)arg.Result).BizActionVOSaveVisit.VisitDetails.ID > 0)
                        {
                            VisitID1 = ((clsAddPatientForPathologyBizActionVO)arg.Result).BizActionVOSaveVisit.VisitDetails.ID;
                            VisitUnitID = ((clsAddPatientForPathologyBizActionVO)arg.Result).BizActionVOSaveVisit.VisitDetails.UnitId;
                        }

                        Indicatior.Close();

                        if (chkFreezBill.IsChecked == true)
                        {
                            SaveFreezBills();

                        }
                        else
                        {
                            SaveBill(null, false);
                        }

                       //SaveSponsor(((clsPatientVO)this.DataContext).GeneralDetails.PatientID, ((clsPatientVO)this.DataContext).GeneralDetails.UnitId);
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


        //added by rohini

        private clsAddPatientForPathologyBizActionVO SavePatientWithTransaction()
        {           

            clsAddPatientForPathologyBizActionVO BizAction = new clsAddPatientForPathologyBizActionVO();

            BizAction.PatientDetails = (clsPatientVO)this.DataContext;
            BizAction.PatientDetails.VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyVisitTypeID;
            BizAction.PatientDetails.FirstName = txtFirstName.Text.Trim();
            BizAction.PatientDetails.MiddleName = txtMiddleName.Text.Trim();
            BizAction.PatientDetails.LastName = txtLastName.Text.Trim();

            if (cmbMaritalStatus.SelectedItem != null)
                BizAction.PatientDetails.MaritalStatusID = ((MasterListItem)cmbMaritalStatus.SelectedItem).ID;
            if (cmbGender.SelectedItem != null)
                BizAction.PatientDetails.GenderID = ((MasterListItem)cmbGender.SelectedItem).ID;
            if (cmbPreffix.SelectedItem != null)
                BizAction.PatientDetails.PrefixId = ((MasterListItem)cmbPreffix.SelectedItem).ID;

            BizAction.PatientDetails.ContactNo1 = txtContactNo1.Text.Trim();
            BizAction.PatientDetails.ContactNo2 = txtContactNo2.Text.Trim();
            if (!string.IsNullOrEmpty(txtMobileCountryCode.Text.Trim()))
                BizAction.PatientDetails.MobileCountryCode = txtMobileCountryCode.Text.Trim();

            if (!string.IsNullOrEmpty(txtResiCountryCode.Text.Trim()))
                BizAction.PatientDetails.ResiNoCountryCode = Convert.ToInt64(txtResiCountryCode.Text.Trim());

            if (!string.IsNullOrEmpty(txtResiSTD.Text.Trim()))
                BizAction.PatientDetails.ResiSTDCode = Convert.ToInt64(txtResiSTD.Text.Trim());


            BizAction.PatientDetails.GeneralDetails.RegType = (short)PatientRegistrationType.Pathology;

            if (dtpDOB.SelectedDate == null)
            {
                BizAction.PatientDetails.GeneralDetails.IsAge = true;
                if (DOB != null)
                    BizAction.PatientDetails.GeneralDetails.DateOfBirth = DOB.Value.Date;
            }

            BizAction.PatientDetails.CompanyName = txtOfficeName.Text.Trim();
            BizAction.PatientDetails.PatientCategoryIDForPath = PatientCategoryID1;

            BizAction.BizActionVOSaveSponsor = SaveSponsorWithTransaction();
           BizAction.PatientDetails.IsVisitForPatho = IsVisit;
          //  BizAction.BizActionVOSaveVisit = SaveVisitWithTransaction();

            return BizAction;
        }

        private void SaveFreezBills()
        {
            //string msgText = string.Empty;

            //msgText = "Are you sure you want to Freeze the Bill ?";
            //MessageBoxControl.MessageBoxChildWindow msgWD =
            //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
            //msgWD.OnMessageBoxClosed += (arg) =>
            //{
            //    if (arg == MessageBoxResult.Yes)
            //    {
                    PaymentWindow paymentWin = new PaymentWindow();
                    paymentWin.TotalAmount = double.Parse(txtClinicalNetAmount.Text);
                    //added by rohini dated 20-6-2016 bug fixing discount not display
                    paymentWin.DiscountAmount = double.Parse(txtClinicalConcession.Text);
                   //
                    paymentWin.Initiate("Bill");
                    //paymentWin.ConcessionFromPlan = ConcessionFromPlan;
                    paymentWin.txtPayTotalAmount.Text = this.txtClinicalTotal.Text;
                    //comented by rohini
                   // paymentWin.txtDiscAmt.Text = this.txtTotalConcession.Text;
                   //added by rohini dated 20-6-2016 bug fixing discount not display
                    paymentWin.txtDiscAmt.Text = this.txtClinicalConcession.Text;
                          //
                    paymentWin.txtPayableAmt.Text = this.txtClinicalNetAmount.Text;


                    paymentWin.PatientCategoryID = PatientCategoryID1;
                   
                    if (rdbAgainstBill.IsChecked == true)
                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstBill;
                    else
                        paymentWin.BillPaymentType = PalashDynamics.ValueObjects.BillPaymentTypes.AgainstServices;
                    paymentWin.OnSaveButton_Click += new RoutedEventHandler(paymentWin_OnSaveButton_Click);

                    paymentWin.Show();
                    ClickedFlag = 0;


            ////    }

            ////};
            ////msgWD.Show();
        }

        void paymentWin_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (((PaymentWindow)sender).TxtConAmt.Text != null)
            {
                BillWiseCon = ((PaymentWindow)sender).TotalConAmount;
            }
             SaveBill(((PaymentWindow)sender).Payment, true);
        }

        public long SponserID = 0;
        private clsAddPatientSponsorBizActionVO SaveSponsorWithTransaction()
        {
            try
            {
               
                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails = new clsPatientSponsorVO();

                BizAction.PatientSponsorDetails.PatientCategoryID = PatientCategoryID1;
                BizAction.PatientSponsorDetails.PatientSourceID = PatientSourceID1;
                BizAction.PatientSponsorDetails.CompanyID = CompanyID1;
                BizAction.PatientSponsorDetails.TariffID = ServiceTariffID;
                BizAction.PatientSponsorDetails.MemberRelationID =1;


                return BizAction;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
            //WaitIndicator Indicatior = new WaitIndicator();
            //Indicatior.Show();

            //try
            //{

            //    clsAddPatientSponsorForPathologyBizActionVO BizAction = new clsAddPatientSponsorForPathologyBizActionVO();

            //    BizAction.PatientSponsorDetails.PatientCategoryID = PatientCategoryID1;
            //    BizAction.PatientSponsorDetails.PatientSourceID = PatientSourceID1;
            //    BizAction.PatientSponsorDetails.CompanyID = CompanyID1;
            //    BizAction.PatientSponsorDetails.TariffID = ServiceTariffID;
               

            //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            //    Client.ProcessCompleted += (s, arg) =>
            //    {
            //        if (arg.Error == null)
            //        {
            //            if (arg.Result != null)
            //            {
            //                SponserID = ((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID;
            //                //Savevisit(PatientID, UnitID, SponserID);
            //            }
            //            Indicatior.Close();
            //        }
            //        else
            //        {
            //            // System.Windows.Browser.HtmlPage.Window.Alert("Error occured while saving Sponsor.");
            //            Indicatior.Close();
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //                      new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //            msgW1.Show();
            //        }

            //    };

            //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            //    Client.CloseAsync();

            //}
            //catch (Exception ex)
            //{
            //    // throw;
            //    string err = ex.Message;
            //    Indicatior.Close();
            //}
        }

        private clsAddVisitBizActionVO SaveVisitWithTransaction()
        {
            try
            {
                //clsAddVisitBizActionVO BizAction = new clsAddVisitBizActionVO();
                //  BizAction.VisitDetails = new clsVisitVO();
                  clsAddVisitBizActionVO Visit = new clsAddVisitBizActionVO();
                      Visit.VisitDetails = new clsVisitVO();
                      Visit.VisitDetails.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                      Visit.VisitDetails.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID;
                    Visit.VisitDetails.VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathologyVisitTypeID;
                    //Visit.VisitDetails.ReferredDoctor = txtReferenceDoctor.Text;
                    //if (txtReferenceDoctor.SelectedItem != null)
                    //    Visit.VisitDetails.ReferredDoctorID = ((MasterListItem)txtReferenceDoctor.SelectedItem).ID;
                    Visit.VisitDetails.VisitStatus = false; //As per discussion with girish sir and nilesh sir (25/4/2011) 
                    Visit.VisitDetails.Status = true;
                    return Visit;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return null;
            }
       
        }


        
        public long VisitID = 0;
        private void Savevisit(long PatientID, long UnitID, long SponsorID)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();

            clsAddVisitForPathologyBizActionVO BizAction = new clsAddVisitForPathologyBizActionVO();
            BizAction.VisitDetails.PatientId = PatientID ;
            BizAction.VisitDetails.PatientUnitId = UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsAddVisitForPathologyBizActionVO)arg.Result).VisitDetails != null)
                        {
                            VisitID = ((clsAddVisitForPathologyBizActionVO)arg.Result).VisitDetails.ID;
                            Indicatior.Close();
                            if (chkFreezBill.IsChecked == false)
                            {
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Save Successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                SaveBill(null, false);

                              
                            }
                            else
                            {
 
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
                     System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                     ClickedFlag = 0;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
 
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

        #region Validations
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


                //disscussed with siddant and then added dated 20012017
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

                //comented by rohini dated 14.3.16 as per disscussion with dr priyanka
                //if ((MasterListItem)cmbPreffix.SelectedItem == null)
                //{
                //    cmbPreffix.TextBox.SetValidation("Prefix Is Required");
                //    cmbPreffix.TextBox.RaiseValidationError();
                //    cmbPreffix.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else if (((MasterListItem)cmbPreffix.SelectedItem).ID == 0)
                //{
                //    cmbPreffix.TextBox.SetValidation("Prefix Is Required");
                //    cmbPreffix.TextBox.RaiseValidationError();
                //    cmbPreffix.Focus();
                //    PatPersonalInfo.SelectedIndex = (int)RegistrationTabs.Patient;
                //    result = false;
                //}
                //else
                //    cmbPreffix.TextBox.ClearValidationError();

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
            }
            catch (Exception)
            {
                // throw;
            }
            return result;
        }
        #endregion

        double BillWiseCon = 0;
        WaitIndicator Indicatior = new WaitIndicator();
        private void SaveBill(PalashDynamics.ValueObjects.OutPatientDepartment.Registration.clsPaymentVO pPayDetails, bool pFreezBill)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
        
            if (chkFreezBill.IsChecked == true)
            {

                var results2 = from r in ChargeList
                               orderby r.NetAmount ascending
                               select r;

                ChargeList = new ObservableCollection<clsChargeVO>();
                foreach (var item in results2.ToList())
                {
                    ChargeList.Add(item);
                }
                double Con = BillWiseCon;
                foreach (var item in ChargeList)
                {

                    double Net = item.NetAmount;
                    double TotalConcession = item.Concession;

                    if (item.ConcessionPercent < 100)
                    {
                        if (Con > 0)
                        {
                            if (Con >= Net)
                            {
                                item.Concession = item.Concession + item.NetAmount;
                                item.NetAmount = 0;
                                Con = Con - Net;
                            }
                            else
                            {
                                double usedCon = 0;
                                usedCon = Con;
                                item.Concession = item.Concession + Con;
                                item.NetAmount = Net - item.Concession;
                                Con = Con - usedCon;
                            }
                        }
                    }
                }

                double TotalAmt = 0;
                if (pPayDetails != null)
                    TotalAmt = pPayDetails.PaidAmount;

                double ConsumeAmt = 0;

                var results = from r in ChargeList
                              orderby r.NetAmount descending
                              select r;

                ChargeList = new ObservableCollection<clsChargeVO>();

                foreach (var item in results.ToList())
                {
                    if (item.ChildPackageService == false)
                    {
                        if (TotalAmt > 0)
                        {
                            ConsumeAmt = item.NetAmount;
                            if (TotalAmt >= ConsumeAmt)
                            {
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }
                            else
                            {
                                ConsumeAmt = TotalAmt;
                                TotalAmt = TotalAmt - ConsumeAmt;
                            }

                            item.ServicePaidAmount = ConsumeAmt;
                            item.BalanceAmount = item.NetAmount - item.ServicePaidAmount;
                            item.ChkID = true;
                        }
                        else
                        {
                            item.BalanceAmount = item.NetAmount;
                        }


                        var _List = from obj in results.ToList()
                                    where (obj.PackageID.Equals(item.PackageID) && obj.ChildPackageService == true)
                                    select obj;


                        double PaidAmount = item.ServicePaidAmount;
                        foreach (var Obj in _List)
                        {
                            clsChargeVO clschargeObject = (clsChargeVO)Obj;

                            if (PaidAmount > 0)
                            {
                                ConsumeAmt = Obj.NetAmount;
                                if (PaidAmount >= ConsumeAmt)
                                {
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }
                                else
                                {
                                    ConsumeAmt = PaidAmount;
                                    PaidAmount = PaidAmount - ConsumeAmt;
                                }

                                Obj.ServicePaidAmount = ConsumeAmt;
                                Obj.BalanceAmount = Obj.NetAmount - Obj.ServicePaidAmount;
                                Obj.ChkID = true;
                            }
                            else
                            {
                                Obj.BalanceAmount = Obj.NetAmount;
                            }
                        }
                    }
                    ChargeList.Add(item);
                }
            }

            double TotCon = 0;
            double TotAmt = 0;
            double TotPaid = 0;

            foreach (var item in ChargeList)
            {
                TotCon = TotCon + item.Concession;
                TotAmt = TotAmt + item.NetAmount;
            }
            try
            {
                clsBillVO objBill = new clsBillVO();
                if (txtMRNo.Text != "")
                {
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                else
                {
                    objBill.PatientID = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
                    objBill.PatientUnitID = ((clsPatientVO)this.DataContext).GeneralDetails.UnitId;
                }
               
                

                if (IsVisit == false)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;


                    objBill.Opd_Ipd_External_Id = VisitID1;
                    objBill.Opd_Ipd_External_UnitId = VisitUnitID;

                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                }
                else if (IsExPatient == true && chkExPatient.IsChecked==true)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;


                    objBill.Opd_Ipd_External_Id = VisitForExistingID;
                    objBill.Opd_Ipd_External_UnitId = VisitForExistingUnitID;

                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                    objBill.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    objBill.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                   
                }

                if (pFreezBill)
                {
                    objBill.Date = DateTime.Now;
                    objBill.Time = DateTime.Now;
                    if (pPayDetails != null)
                    {
                        objBill.BalanceAmountSelf = TotAmt - pPayDetails.PaidAmount;
                        objBill.BillPaymentType = pPayDetails.BillPaymentType;
                    }

                    if (!string.IsNullOrEmpty(txtClinicalNetAmount.Text))
                        objBill.SelfAmount = Convert.ToDouble(txtClinicalNetAmount.Text);
                }
                else
                {
                    objBill.SelfAmount = Convert.ToDouble(txtClinicalNetAmount.Text);
                }

                if (PatientSourceID1 > 0)
                {
                    objBill.PatientSourceId = PatientSourceID1;
                }
                else
                {
                    objBill.PatientSourceId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceID;

                }
                if (ServiceTariffID > 0)
                {
                    objBill.TariffId = ServiceTariffID;
                }
                else
                {
                    objBill.TariffId = ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID;
                }
                if (PatientCategoryID1 > 0)
                {
                    objBill.PatientCategoryId = Convert.ToInt64(PatientCategoryID1);
                }
                else
                {
                    objBill.PatientCategoryId = ((IApplicationConfiguration)App.Current).SelectedPatient.NewPatientCategoryID;
                }
                if (CompanyID1 > 0)
                {
                    objBill.CompanyId = Convert.ToInt64(CompanyID1);
                }
                else
                {
                    objBill.CompanyId = ((IApplicationConfiguration)App.Current).SelectedPatient.CompanyID;
                }

                objBill.IsFreezed = pFreezBill;
                objBill.PaymentDetails = pPayDetails;


                //if (!string.IsNullOrEmpty(txtTotalBill.Text))
                //    objBill.TotalBillAmount = Convert.ToDouble(txtTotalBill.Text);

                if (!string.IsNullOrEmpty(txtClinicalTotal.Text))
                    objBill.TotalBillAmount = Convert.ToDouble(txtClinicalTotal.Text);

                if (!string.IsNullOrEmpty(txtTotalConcession.Text))
                    objBill.TotalConcessionAmount = Convert.ToDouble(txtTotalConcession.Text);
                TotalConcession = objBill.TotalConcessionAmount;

                if (!string.IsNullOrEmpty(txtNetAmount.Text))
                    objBill.NetBillAmount = Convert.ToDouble(txtNetAmount.Text);



                objBill.TotalConcessionAmount = TotCon;

                TotalConcession = objBill.TotalConcessionAmount;

                objBill.NetBillAmount = TotAmt;

                if (ChargeList != null & ChargeList.Count > 0)
                {
                    objBill.ChargeDetails = ChargeList.ToList();

                    if (pFreezBill)
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            clsChargeVO objCharge = new clsChargeVO();
                            objCharge = ChargeList[i];

                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                objCharge.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;


                                objBill.PathoWorkOrder = new clsPathOrderBookingVO();
                                objBill.PathoWorkOrder.DoctorID = objCharge.SelectedDoctor.ID;
                                objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;//14;
                                objBill.PathoWorkOrder.IsExternalPatient = true;
                                objBill.PathoWorkOrder.Items = new List<clsPathOrderBookingDetailVO>();
                            
                        }
                    }
                    else
                    {
                        for (int i = 0; i < ChargeList.Count; i++)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                            else
                                ChargeList[i].CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                        }
                        objBill.PathoWorkOrder.PathoSpecilizationID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PathoSpecializationID;//14;
                    }
                }

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsCounterLogin == true)
                {
                    if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0 && objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.ChargeDetails != null && objBill.ChargeDetails.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                    else if (objBill.PharmacyItems != null && objBill.PharmacyItems.Items != null && objBill.PharmacyItems.Items.Count > 0)
                    {
                        // Added By CDS
                        objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.CashCounterID;   //Costing Divisions for Pharmacy Billing  BillTypes.Clinical_Pharmacy;
                    }
                }
                else
                {
                    objBill.CostingDivisionID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.OPDCounterID;
                }

                if (objBill.PaymentDetails != null)
                {
                    //objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                    // Added By CDS
                    objBill.PaymentDetails.CostingDivisionID = objBill.CostingDivisionID;
                }

                clsAddBillBizActionVO BizAction = new clsAddBillBizActionVO();
                BizAction.IsFromPathologyLab = true;
                if(txtMRNo.Text=="")
                {
                    BizAction.obPathoPatientVODetails = SavePatientWithTransaction();
                }
                if (IsVisit == false && chkExPatient.IsChecked == true)
                {

                    BizAction.obPathoPatientVisitVODetails = SaveVisitWithTransaction();
                }
                BizAction.Details = new clsBillVO();
                BizAction.Details = objBill;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsAddBillBizActionVO)arg.Result).Details != null)
                    {

                        if (((clsAddBillBizActionVO)arg.Result).Details != null)
                        {
                            BillID = (((clsAddBillBizActionVO)arg.Result).Details).ID;
                            UnitID = (((clsAddBillBizActionVO)arg.Result).Details).UnitID;
                            Indicatior.Close();
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Bill details saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW1.OnMessageBoxClosed += (re) =>
                            {
                                if (re == MessageBoxResult.OK)
                                {
                                  
                                    if (pFreezBill == true)
                                    {
                                        PrintBill(BillID, UnitID, 0);
                                    }
                                }
                            };

                            msgW1.Show();
                            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
                            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
                            //_flip.Invoke(RotationType.Backward);
                            
                        }
                    }
                    else
                    {
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("NT6", "Error occured while adding Bill details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
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
                throw;
            }
            finally
            {
                Indicatior.Close();
            }

       
        }

        private void PrintBill(long iBillId, long iUnitID, long PrintID)
        {
            if (iBillId > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                string URL = "../Reports/OPD/ClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void dtpDOB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dtpDOB.SelectedDate != null)
            {
                txtYY.Text = ConvertDate(dtpDOB.SelectedDate, "YY");
                txtMM.Text = ConvertDate(dtpDOB.SelectedDate, "MM");
                txtDD.Text = ConvertDate(dtpDOB.SelectedDate, "DD");
            }
            txtYY.SelectAll();
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtYY_TextChanged(object sender, TextChangedEventArgs e)
        {

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
                DOB = CalculateDateFromAge(Yearval, Monthval, DayVal);

            }
            else
            {


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

        private void txtDD_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateBirthDate();
        }

        private void chkNewPatient_Click(object sender, RoutedEventArgs e)
        {
            if (chkNewPatient.IsChecked == true)
            {
                btnSearchCriteria.IsEnabled = false;
                txtFirstName.IsEnabled = true;
                txtMiddleName.IsEnabled = true;
                txtLastName.IsEnabled = true;
                cmbGender.IsEnabled = true;
                cmbPreffix.IsEnabled = true;
                cmbMaritalStatus.IsEnabled = true;
                dtpDOB.IsEnabled = true;
                txtMobileCountryCode.IsEnabled = true;
                txtContactNo1.IsEnabled = true;
                txtYY.IsEnabled = true;
                txtMM.IsEnabled = true;
                txtDD.IsEnabled = true;
                txtEmail.IsEnabled = true;
                dtpDOB.SelectedDate = null;
                PatientCategoryID = 0;
                PatientSourceID = 0;
                PatientTariffID = 0;
                CompanyID = 0;
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
             
            }
            else if (chkExPatient.IsChecked == true)
            {
                btnSearchCriteria.IsEnabled = true;
                txtFirstName.IsEnabled = false;
                txtMiddleName.IsEnabled = false;
                txtLastName.IsEnabled = false;
                cmbGender.IsEnabled = false;
                cmbPreffix.IsEnabled = false;
                cmbMaritalStatus.IsEnabled = false;
                dtpDOB.IsEnabled = false;
                txtMobileCountryCode.IsEnabled = false;
                txtContactNo1.IsEnabled = false;
                txtYY.IsEnabled = false;
                txtMM.IsEnabled = false;
                txtDD.IsEnabled = false;
                txtEmail.IsEnabled = false;
                
            }
            clearAll();
        }
        private void clearAll()
        {
            ChargeList.Clear();
            dgCharges.UpdateLayout();
            txtFirstName.Text = string.Empty;
            txtMiddleName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            cmbGender.SelectedValue = (long)0;
            cmbPreffix.SelectedValue = (long)0;
            cmbMaritalStatus.SelectedValue = (long)0;
            dtpDOB.SelectedDate = null;
            txtMobileCountryCode.Text = string.Empty;
            txtContactNo1.Text = string.Empty;
            txtYY.Text = string.Empty;
            txtMM.Text = string.Empty;
            txtDD.Text = string.Empty;
            txtMRNo.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cmbPreffix.TextBox.ClearValidationError();
            cmbGender.TextBox.ClearValidationError();
            cmbMaritalStatus.TextBox.ClearValidationError();
            txtYY.ClearValidationError();
            txtMM.ClearValidationError();
            txtDD.ClearValidationError();
            txtEmail.ClearValidationError();
            txtFirstName.ClearValidationError();
            dtpDOB.ClearValidationError();
            txtMobileCountryCode.Textbox.ClearValidationError();
            txtContactNo1.Textbox.ClearValidationError();
            cmbPatientCategory.SelectedValue = (long)0;

           // dgCharges.ItemsSource = null;
        }

        private void WaterMarkTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void WaterMarkTextbox_OnTextChanged(object sender, RoutedEventArgs e)
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
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtMiddleName_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, TextChangedEventArgs e)
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

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPathologyWorkOrder = null;
            ((IApplicationConfiguration)App.Current).OpenMainContent(new NewPathologyWorkOrderGeneration());
        }

        private void txtMobileCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtMobileCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
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

        private void txtResiCountryCode_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtResiCountryCode_OnTextChanged(object sender, RoutedEventArgs e)
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

        private void txtResiSTD_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((WaterMarkTextbox)sender).Text;
            selectionStart = ((WaterMarkTextbox)sender).SelectionStart;
            selectionLength = ((WaterMarkTextbox)sender).SelectionLength;
        }

        private void txtResiSTD_OnTextChanged(object sender, RoutedEventArgs e)
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

        private void cmdDeleteCharges_Click(object sender, RoutedEventArgs e)
        {
            //if (IsFromRequestApproval == false)
            //{
                if (dgCharges.SelectedItem != null)
                {
                    string msgText = "Are You Sure \n  You Want To Delete The Selected Service ?";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            ChargeList.RemoveAt(dgCharges.SelectedIndex);
                            CalculateClinicalSummary();
                        }
                    };
                    msgWD.Show();
                }
            //}
        }

        private void cmbPatientCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientCategory.SelectedItem != null && ((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
            //{
            //    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);
            //}

            //added by rohini dated 4.3.16
            if (cmbPatientCategory.SelectedItem != null || cmbPatientCategory.SelectedValue != null)
                if (((MasterListItem)cmbPatientCategory.SelectedItem).ID > 0)
                {

                    //dgCharges.ItemsSource = null;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbPatientSource.ItemsSource = objList;
                    cmbPatientSource.SelectedItem = objM;
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                    //added by rohini dated 15.4.16
                    ChargeList = new ObservableCollection<clsChargeVO>();
                    dgCharges.ItemsSource = ChargeList;
                    dgCharges.UpdateLayout();
                    //
                    FillPatientSource(((MasterListItem)cmbPatientCategory.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbPatientSource.ItemsSource = objList;
                    cmbPatientSource.SelectedItem = objM;
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                }
     
        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            //{
            //    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);
            //}
            //added by rohini dated 4.3.16
            if (cmbPatientSource.SelectedItem != null || cmbPatientSource.SelectedValue != null)
                if (((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    //cmbPatientSource.ItemsSource = objList;
                    //cmbPatientSource.SelectedItem = objM;
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                    //added by rohini dated 15.4.16
                    ChargeList = new ObservableCollection<clsChargeVO>();
                    dgCharges.ItemsSource = ChargeList;
                    dgCharges.UpdateLayout();
                    //
                    FillCompany(((MasterListItem)cmbPatientSource.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbCompany.ItemsSource = objList;
                    cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                }
        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID > 0)
            //{
            //    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
            //}
            //added by rohini dated 4.3.16
            if (cmbCompany.SelectedItem != null || cmbCompany.SelectedValue != null)
                if (((MasterListItem)cmbCompany.SelectedItem).ID > 0)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    //cmbPatientSource.ItemsSource = objList;
                    //cmbPatientSource.SelectedItem = objM;
                    // cmbCompany.ItemsSource = objList;
                    //cmbCompany.SelectedItem = objM;
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                    //added by rohini dated 15.4.16
                    ChargeList = new ObservableCollection<clsChargeVO>();
                    dgCharges.ItemsSource = ChargeList;
                    dgCharges.UpdateLayout();
                    //
                    FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
                }
                else
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    MasterListItem objM = new MasterListItem(0, "-- Select --");
                    objList.Add(objM);
                    cmbTariff.ItemsSource = objList;
                    cmbTariff.SelectedItem = objM;
                }
        }

    }
}
