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

using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using System.IO;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.IPD;

namespace CIMS.Forms
{
    public partial class SponsorWindow : UserControl, IInitiateCIMS
    {
        
     //   public clsPatientGeneralVO MyPatient { get; set; }
        public long sAppointmentID = 0;
        private bool EditMode = false;
        private bool TariffEditFirst = false;
        private bool AssociateCompanyAtEdit = false;
        long PatientID = 0;
        bool IsPatientExist = false;

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
           
            switch (Mode)
            {

                case "NEWR":
                    IsPatientExist = true;
                    PatientID = -1;
                    //  PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //  this.Title = "Sponsor - " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                    //               " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    // App.IsPatientMode = true;
                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                    {
                        UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                        TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                        TextBlock mElement_1 = (TextBlock)rootPage1.FindName("SampleHeader");

                        mElement_1.Text = "Sponsor Details";



                        mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                            " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                            ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                        mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                    }
                    this.DataContext = new clsPatientSponsorVO()
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID,
                        CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID,
                        TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                    };

                    break; 

                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null )
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

                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //this.Title = "Sponsor - " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                    //             " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    // App.IsPatientMode = true;
                     UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");
                  

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsPatientSponsorVO() { PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                                                                   PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                                                                   PatientSourceID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID,
                                                                   CompanyID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID,
                                                                   TariffID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.TariffID
                    };
                    IsPatientExist = true;
                    break; 

                case "EDIT":
                    //tabPatAccountInfo.Visibility = Visibility.Visible;
                    //tabEmergencyPatInfo.Visibility = Visibility.Visible;
                    //tabPatEncounterInfo.Visibility = Visibility.Visible;
                   // this.Title = "Sponsor - " + (clsPatientSponsorVO)this.DataConte
                    EditMode = true;
                    IsPatientExist = true;
                    break;

                //case "NEWR":
                //    //this.Title = null;
                //    //this.HasCloseButton = false;
                   
                //   UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                //   TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                //    mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                //        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                //        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                //    mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo; 


                //    this.DataContext = new clsPatientSponsorVO { PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, CompanyID = 1, DesignationID= 1 };

                //    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                //    {
                //        clsGetPatientSponsorBizActionVO BizAction = new clsGetPatientSponsorBizActionVO();
                //        BizAction.PatientSponsorDetails.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                //        BizAction.GetLatest = true;


                //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //        Client.ProcessCompleted += (s, arg) =>
                //        {
                //            if (arg.Error == null && arg.Result != null)
                //            {
                //                this.DataContext = ((clsGetPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails;
                              
                //                cmbCompany.SelectedValue = ((clsPatientSponsorVO)this.DataContext).CompanyID;
                //                cmbAssCompany.SelectedValue = ((clsPatientSponsorVO)this.DataContext).AssociatedCompanyID;
                //                cmbDesignation.SelectedValue = ((clsPatientSponsorVO)this.DataContext).DesignationID;

                //            }

                //        };

                //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                //        Client.CloseAsync();
                //    }

                //    break;
            }
        }

        #endregion

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;

        //public event RoutedEventHandler OnSaveButton_Click;
        public clsPatientVO myPatient { get; set; }
        public SponsorWindow()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPatientExist == false)
                {
                    ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                }

                if (PatientID == 0)
                {

                    OkButton.IsEnabled = false;

                }
                else
                {
                    if (!IsPageLoded)
                    {
                        //Indicatior = new WaitIndicator();
                        //Indicatior.Show();

                        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                        {
                            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                              && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                            {
                                //do  nothing
                            }
                            else
                                OkButton.IsEnabled = false;
                        }
                   
                        
                        FillPatientSource();
                        FillCompany();
                        FillDesignation();
                        
                        if (((clsPatientSponsorVO)this.DataContext).PatientId > 0)
                            FillSponsorDetails();
                    
                        //Indicatior.Close();
                        //cmbPatientSource.Focus();
                        //cmbPatientSource.UpdateLayout();
                    }

                    
                    IsPageLoded = true;
                }
            }
            catch (Exception)
            {

             
            }
        }

        public void FillSponsorDetails()
        {

            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                
                clsGetPatientSponsorListBizActionVO BizAction = new clsGetPatientSponsorListBizActionVO();
      
                BizAction.SponsorID = 0;
                BizAction.PatientID = ((clsPatientSponsorVO)this.DataContext).PatientId;
                BizAction.PatientUnitID = ((clsPatientSponsorVO)this.DataContext).PatientUnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {

                        dgSponserList.ItemsSource = null;
                        dgSponserList.ItemsSource = ((clsGetPatientSponsorListBizActionVO)arg.Result).PatientSponsorDetails;

                    }
                    Indicatior.Close();
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
               // throw;
            }
        }

        //private void FillPatientSource()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_PatientSourceMaster;
        //    BizAction.Parent = new KeyValue { Key = true, Value = "Status" };
        //    //BizAction.IsActive = true;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            List<MasterListItem> objList = new List<MasterListItem>();

        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
        //            cmbPatientSource.ItemsSource = null;
        //            cmbPatientSource.ItemsSource = objList;
        //        }

        //        if (this.DataContext != null)
        //        {
        //            //if (myPatient == null)
        //                cmbPatientSource.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
        //            //else
        //            //    cmbPatientSource.SelectedValue = myPatient.GeneralDetails.PatientSourceID;

        //            if (((clsPatientSponsorVO)this.DataContext).PatientSourceID == 0)
        //            {
        //                cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");

        //                cmbPatientSource.TextBox.RaiseValidationError();
        //            }
        //            //else
        //            //    FillTariffMaster(((clsPatientSponsorVO)this.DataContext).PatientSourceID,0);
                   
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}
        private void FillPatientSource()
        {
            clsGetPatientSourceListBizActionVO BizAction = new clsGetPatientSourceListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ValidPatientMasterSourceList = true;
            BizAction.ID = 0;
            //BizAction.IsActive = true;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem( 0, "- Select -" ));
                    objList.AddRange(((clsGetPatientSourceListBizActionVO)e.Result).MasterList);
                    cmbPatientSource.ItemsSource = null;
                    cmbPatientSource.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    //if (myPatient == null)
                    cmbPatientSource.SelectedValue = ((clsPatientSponsorVO)this.DataContext).PatientSourceID;
                    //else
                    //    cmbPatientSource.SelectedValue = myPatient.GeneralDetails.PatientSourceID;

                    if (((clsPatientSponsorVO)this.DataContext).PatientSourceID == 0)
                    {
                        cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");

                        cmbPatientSource.TextBox.RaiseValidationError();
                    }
                    //else
                    //    FillTariffMaster(((clsPatientSponsorVO)this.DataContext).PatientSourceID,0);

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }
        private void FillCompany()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CompanyMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                //if (e.Error == null && e.Result != null)
                //{
                //    cmbCompany.ItemsSource = null;
                //    cmbCompany.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                //}

                //if (this.DataContext != null)
                //{

                //    cmbCompany.SelectedValue = ((clsPatientSponsorVO)this.DataContext).CompanyID;
                //    // FillStateList(((clsPatientVO)this.DataContext).CountryId);

                //}

                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbCompany.ItemsSource = null;
                    cmbCompany.ItemsSource = objList;

                    if (this.DataContext != null)
                    {
                        if (myPatient != null && myPatient.GeneralDetails.PatientSourceID == ((clsPatientSponsorVO)this.DataContext).PatientSourceID)
                        {
                            cmbCompany.SelectedValue = objList[0].ID;
                        }
                        else
                            cmbCompany.SelectedValue = ((clsPatientSponsorVO)this.DataContext).CompanyID;


                    }
                }

               


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillAssociateCompany(long pCompanyID)
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CompanyAssociateMaster;

            if (pCompanyID > 0)
                BizAction.Parent = new KeyValue { Key = pCompanyID, Value = "CompanyId" };

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
                    cmbAssCompany.ItemsSource = null;
                    cmbAssCompany.ItemsSource = objList;

                    if (AssociateCompanyAtEdit == true)
                    {
                        cmbAssCompany.SelectedValue = ((clsPatientSponsorVO)this.DataContext).AssociatedCompanyID;
                        AssociateCompanyAtEdit = false;
                    }
                    else if ((MasterListItem)cmbCompany.SelectedItem != null && ((MasterListItem)cmbCompany.SelectedItem).ID !=0)
                    {
                        if (((clsGetMasterListBizActionVO)e.Result).MasterList.Count > 0)
                            cmbAssCompany.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[0];                      
                    }
                    else
                    {
                        cmbAssCompany.SelectedItem = ((MasterListItem)objList[0]);
                        
                                  
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillCompanyCreditDetails(long pCompanyID)
        {
            clsGetCompanyCreditDtlsBizActionVO BizAction = new clsGetCompanyCreditDtlsBizActionVO();
            //BizAction.IsActive = true;
            BizAction.ID = pCompanyID;
            BizAction.Details = new clsCompanyCreditDetailsVO();

            txtTotal.Text = "";
            txtUsed.Text = "";
            txBalance.Text = "";
            txtCreditLimit.Text = "0";

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    clsGetCompanyCreditDtlsBizActionVO obj = (clsGetCompanyCreditDtlsBizActionVO)e.Result;

                    if (obj.Details != null)
                    {
                        txtCreditLimit.Text = obj.Details.CreditLimit.ToString();
                        txtTotal.Text = (obj.Details.TotalAdvance - obj.Details.Refund).ToString();
                        txtUsed.Text = obj.Details.Used.ToString();
                        txBalance.Text = obj.Details.Balance.ToString();

                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void FillDesignation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
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
                    cmbDesignation.ItemsSource = null;
                    cmbDesignation.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbDesignation.SelectedValue = ((clsPatientSponsorVO)this.DataContext).DesignationID;
                    // FillStateList(((clsPatientVO)this.DataContext).CountryId);
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillTariffMaster( long pPatientsourceID, long pCompanyID)
        {
            clsGetTariffMasterVO BizAction = new clsGetTariffMasterVO();
            //BizAction.IsActive = true;
            BizAction.PatientSourceID = pPatientsourceID;
            BizAction.CompanyID = pCompanyID;

            if (myPatient != null && myPatient.GeneralDetails.PatientSourceID == pPatientsourceID)
            {
                BizAction.ParentPatientID = myPatient.ParentPatientID;
                BizAction.PatientSourceType = 1; //For Loyalty
            }

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

                if (EditMode == true && TariffEditFirst == true)
                {
                    cmbTariff.SelectedValue = ((clsPatientSponsorVO)this.DataContext).TariffID;
                    TariffEditFirst = false;
                }
                else if (myPatient != null && ((clsPatientSponsorVO)this.DataContext).PatientSourceID == myPatient.GeneralDetails.PatientSourceID)
                {
                      cmbTariff.SelectedValue = myPatient.TariffID;
                }
                else
                {
                    if (objList.Count == 2)
                        cmbTariff.SelectedItem = objList[1];
                    else
                        cmbTariff.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }


        //private void FillGroup()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.GroupMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    PalashServiceClient Client = null;
        //    Client = new PalashServiceClient();
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            cmbGroup.ItemsSource = null;
        //            cmbGroup.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            if (((clsGetMasterListBizActionVO)e.Result).MasterList.Count > 0)
        //                cmbGroup.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[0];
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}

        //private void FillService()
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.ServiceMaster;
        //    BizAction.MasterList = new List<MasterListItem>();

        //    PalashServiceClient Client = null;
        //    Client = new PalashServiceClient();
        //    Client.ProcessCompleted += (s, e) =>
        //    {
        //        if (e.Error == null && e.Result != null)
        //        {
        //            cmbService.ItemsSource = null;
        //            cmbService.ItemsSource = ((clsGetMasterListBizActionVO)e.Result).MasterList;
        //            if (((clsGetMasterListBizActionVO)e.Result).MasterList.Count > 0)
        //                cmbService.SelectedItem = ((clsGetMasterListBizActionVO)e.Result).MasterList[0];
        //        }
        //    };

        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}
        int ClickedFlag = 0;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (CheckValidations())
                {
                    //OkButton.IsEnabled = false;

                   
                        string msgTitle = "Palash";
                        string msgText = "Are you sure you want to save the Sponsor Details";

                        MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

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
                SaveSponsor();
            else
                ClickedFlag = 0;
          
        }

        private void SaveSponsor()
        {
            try
            {
                clsAddPatientSponsorBizActionVO BizAction = new clsAddPatientSponsorBizActionVO();
                BizAction.PatientSponsorDetails = (clsPatientSponsorVO)this.DataContext;

                if (cmbPatientSource.SelectedItem != null)
                    BizAction.PatientSponsorDetails.PatientSourceID = ((MasterListItem)cmbPatientSource.SelectedItem).ID;

                if (cmbCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.CompanyID = ((MasterListItem)cmbCompany.SelectedItem).ID;

                if (cmbAssCompany.SelectedItem != null)
                    BizAction.PatientSponsorDetails.AssociatedCompanyID = ((MasterListItem)cmbAssCompany.SelectedItem).ID;

                if (cmbDesignation.SelectedItem != null)
                    BizAction.PatientSponsorDetails.DesignationID = ((MasterListItem)cmbDesignation.SelectedItem).ID;

                if (cmbTariff.SelectedItem != null)
                    BizAction.PatientSponsorDetails.TariffID = ((MasterListItem)cmbTariff.SelectedItem).ID;
                
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    ClickedFlag = 0;
                   
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails != null)
                            {
                                ((clsPatientSponsorVO)this.DataContext).ID = ((clsAddPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails.ID;
                                //System.Windows.Browser.HtmlPage.Window.Alert("Sponsor Saved Successfully with ID " + ((clsPatientSponsorVO)this.DataContext).SponsorID);
                                //this.DialogResult = true;
                                //if (OnSaveButton_Click != null)
                                //    OnSaveButton_Click(this, new RoutedEventArgs());
                                //this.DataContext = new clsPatientSourceVO();
                                InitialiseForm();
                                FillSponsorDetails();
                               
                                //OkButton.IsEnabled = true;
                                // System.Windows.Browser.HtmlPage.Window.Alert("Sponsor saved successfully.");
                                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Sponsor saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                msgW1.OnMessageBoxClosed += (re) =>
                                {
                                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                };
                                msgW1.Show();
                             }
                        }
                    }
                    else
                    {
                       // System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding Sponsor.");
                        OkButton.IsEnabled = true;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding Sponsor.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                ClickedFlag = 0;
                //OkButton.IsEnabled = true;
                string err = ex.Message;
                // throw;
            }

        }
        void InitialiseForm()
        {
            cmbCompany.SelectedValue = (long)0;
            cmbAssCompany.SelectedValue = (long)0;
            cmbTariff.SelectedValue = (long)0;
            dtpEffectiveDate.SelectedDate = null;
            ExpiryDate.SelectedDate = null;
            txtRefNo.Text = "";
            cmbDesignation.SelectedValue = (long)0;
            

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO();
            ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
        }

        private void cmdAddSponser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdBackSponser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbCompany.SelectedItem != null)
            {
                FillAssociateCompany(((MasterListItem)cmbCompany.SelectedItem).ID);

                if (((MasterListItem)cmbCompany.SelectedItem).ID > 0)
                {
                    if (((MasterListItem)cmbCompany.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID || ((MasterListItem)cmbCompany.SelectedItem).ID == 0)
                    {

                        txtCreditLimit.IsReadOnly = true;
                        dtpEffectiveDate.IsEnabled = false;
                        ExpiryDate.IsEnabled = false;
                        txtRefNo.IsReadOnly = true;
                        txtRefNo.Text = "";
                       
                        txtCreditLimit.Text = "0";
                        txtTotal.Text = "";
                        txtUsed.Text = "";
                        txBalance.Text = "";

                        if(myPatient !=null && myPatient.GeneralDetails.PatientSourceID == ((clsPatientSponsorVO)this.DataContext).PatientSourceID)
                        {
                            //Do nothing
                            
                        }
                        else
                        {
                           
                            cmbPatientSource.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID;
                        }
                    }
                    else
                    {
                       // FillAssociateCompany(((MasterListItem)cmbCompany.SelectedItem).ID);

                        FillCompanyCreditDetails(((MasterListItem)cmbCompany.SelectedItem).ID);

                        txtCreditLimit.IsReadOnly = false;
                        dtpEffectiveDate.IsEnabled = true;
                        ExpiryDate.IsEnabled = true;
                       // txtRefNo.IsReadOnly = false;

                        if (((MasterListItem)cmbPatientSource.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                        {
                            FillTariffMaster(0, ((MasterListItem)cmbCompany.SelectedItem).ID);
                        }
                         
                    }

                }
            }
          
        }

        private void cmbAssCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //private void hlAddImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog OpenFile = new OpenFileDialog();
        //    OpenFile.Multiselect = false;
        //    OpenFile.Filter = "(*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png;";
        //    OpenFile.FilterIndex = 1;
        //    if (OpenFile.ShowDialog() == true)
        //    {
        //        BitmapImage imageSource = new BitmapImage();
        //        try
        //        {
        //            //imageSource.SetSource(OpenFile.File.OpenRead());
        //            //imgPhoto.Source = imageSource;
        //            if (CardList == null)
        //            {
        //                CardList = new ObservableCollection<clsPatientSponsorCardDetailsVO>();
        //                dgSponserImage.ItemsSource = CardList;
        //            }
        //            byte[] mImage = imageToByteArray(OpenFile.File);
        //            CardList.Add(new clsPatientSponsorCardDetailsVO() { Title = "", Image = mImage });
        //            //((clsPatientVO)this.DataContext).Photo = imageToByteArray(OpenFile.File);
        //        }
        //        catch (Exception)
        //        {
        //            HtmlPage.Window.Alert("Error Loading File");
        //        }
        //    }
        //}

        public byte[] imageToByteArray(FileInfo imageIn)
        {

            Stream stream = imageIn.OpenRead();
            BinaryReader binary = new BinaryReader(stream);
            Byte[] imgB = binary.ReadBytes((int)stream.Length);

            return imgB;
        }
        
        private void hlbEditSponsor_Click(object sender, RoutedEventArgs e)
        {
            EditSponsor();

        }

        void msgWE_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                EditSponsor();
        }

        private void EditSponsor()
        {

            clsGetPatientSponsorBizActionVO BizAction = new clsGetPatientSponsorBizActionVO();
            //BizAction.IsActive = true;
            // BizAction.PatientSponsorDetails.PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            BizAction.PatientSponsorDetails.ID = ((clsPatientSponsorVO)dgSponserList.SelectedItem).ID;
            
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if (((clsGetPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails != null)
                    {
                        clsPatientSponsorVO SponsorObj = ((clsGetPatientSponsorBizActionVO)arg.Result).PatientSponsorDetails;
                        this.DataContext = null;
                        this.DataContext = SponsorObj;
                        AssociateCompanyAtEdit = true;
                        EditMode = true;
                        TariffEditFirst = true;
                        cmbPatientSource.SelectedValue = SponsorObj.PatientSourceID;
                        cmbCompany.SelectedValue = SponsorObj.CompanyID;
                        cmbDesignation.SelectedValue = SponsorObj.DesignationID;
                        cmbTariff.SelectedValue = SponsorObj.TariffID;
                        
                        //dtpEffectiveDate.SelectedDate = SponsorObj.EffectiveDate;
                        //ExpiryDate.SelectedDate = SponsorObj.ExpiryDate;
                    }
                  

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void hlbDeleteSponsor_Click(object sender, RoutedEventArgs e)
        {
            string msgTitle = "Palash";
            string msgText = "Are you sure you want to Delete the Sponsor Details";

            MessageBoxControl.MessageBoxChildWindow msgWD =
                new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

            msgWD.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgWD_OnMessageBoxClosed);

            msgWD.Show();
        }


        void msgWD_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                DeleteSponsor();
        }

        private void DeleteSponsor()
        {

            clsDeletePatientSponsorBizActionVO BizAction = new clsDeletePatientSponsorBizActionVO();

            //BizAction.IsActive = true;
            // BizAction.PatientSponsorDetails.PatientId = ((clsPatientVO)this.DataContext).GeneralDetails.PatientID;
            BizAction.SponsorID = ((clsPatientSponsorVO)dgSponserList.SelectedItem).ID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Sponsor Deleted successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();

                    FillSponsorDetails();


                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        private void txtNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!((TextBox)sender).Text.IsNumberValid() && textBefore !=null)
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

            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        public bool CheckValidations()
        {
            bool result = true;

            if ((MasterListItem)cmbPatientSource.SelectedItem == null)
            {
                cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");

                cmbPatientSource.TextBox.RaiseValidationError();
                cmbPatientSource.Focus();

                result = false;
            }
            else if (((MasterListItem)cmbPatientSource.SelectedItem).ID == 0)
            {
                cmbPatientSource.TextBox.SetValidation("Please select the Patient Source");
                cmbPatientSource.TextBox.RaiseValidationError();
                cmbPatientSource.Focus();

                result = false;
            }
            else if (EditMode == false && ((MasterListItem)cmbPatientSource.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID)
            {
                List<clsPatientSponsorVO> objList = new List<clsPatientSponsorVO>();
              
                objList = (List<clsPatientSponsorVO>)dgSponserList.ItemsSource;

                if (objList != null && objList.Count > 0)
                {
                    var Sponsors = from r in objList
                                where r.PatientSourceID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID
                              
                                select new clsPatientSponsorVO
                                {
                                    Status = r.Status,
                                    ID = r.ID
                                };

                    if( Sponsors.ToList().Count > 0)
                    {
                        result = false;
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                new MessageBoxControl.MessageBoxChildWindow("Palash", "Sponsor is already added.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW1.Show();
                    }
                }
            }
            else
                cmbPatientSource.TextBox.ClearValidationError();


            if (cmbTariff.SelectedItem == null)
            {
                cmbTariff.TextBox.SetValidation("Please select the Tariff");

                cmbTariff.TextBox.RaiseValidationError();
                cmbTariff.Focus();

                result = false;
            }
            else if (((MasterListItem)cmbTariff.SelectedItem).ID == 0)
            {
                cmbTariff.TextBox.SetValidation("Please select the Tariff");
                cmbTariff.TextBox.RaiseValidationError();
                cmbTariff.Focus();

                result = false;
            }
            else
                cmbTariff.TextBox.ClearValidationError();


            if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0 && ((MasterListItem)cmbPatientSource.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
            {

                if (((clsPatientSponsorVO)this.DataContext).ReferenceNo == null)
                {

                    txtRefNo.SetValidation("Reference No. is Required");
                    txtRefNo.RaiseValidationError();
                    result = false;
                    //dtpEffectiveDate.Focus();
                }
                else if (((clsPatientSponsorVO)this.DataContext).ReferenceNo.Length == 0)
                {
                    txtRefNo.SetValidation("Reference No. is Required");
                    txtRefNo.RaiseValidationError();
                    result = false;
                    // dtpEffectiveDate.Focus();
                }
                else
                    txtRefNo.ClearValidationError();

                if (cmbCompany.SelectedItem == null)
                {
                    cmbCompany.TextBox.SetValidation("Please Select the Company");

                    cmbCompany.TextBox.RaiseValidationError();
                    cmbCompany.Focus();

                    result = false;
                }
                else if (((MasterListItem)cmbCompany.SelectedItem).ID == 0)
                {
                    cmbCompany.TextBox.SetValidation("Please select the Company");
                    cmbCompany.TextBox.RaiseValidationError();
                    cmbCompany.Focus();

                    result = false;
                }
                else
                    cmbCompany.TextBox.ClearValidationError();

                


            }
          

            if (dtpEffectiveDate.IsEnabled)
            {
                if (((clsPatientSponsorVO)this.DataContext).EffectiveDate == null)
                {
                    dtpEffectiveDate.SetValidation("Effective Date Required");
                    dtpEffectiveDate.RaiseValidationError();
                    result = false;
                   // dtpEffectiveDate.Focus();
                }
                else
                    dtpEffectiveDate.ClearValidationError();
            }

            if (ExpiryDate.IsEnabled)
            {
                if (((clsPatientSponsorVO)this.DataContext).ExpiryDate == null)
                {
                    ExpiryDate.SetValidation("ExpiryDate Date Required");
                    ExpiryDate.RaiseValidationError();
                    
                  //  ExpiryDate.Focus();
                    result = false;
                }
                else
                    ExpiryDate.ClearValidationError();
            }

            if (dtpEffectiveDate.IsEnabled == true && ExpiryDate.IsEnabled == true && result == true)
            {
                if (((clsPatientSponsorVO)this.DataContext).EffectiveDate != null && ((clsPatientSponsorVO)this.DataContext).ExpiryDate != null && ((clsPatientSponsorVO)this.DataContext).EffectiveDate > ((clsPatientSponsorVO)this.DataContext).ExpiryDate)
                {
                    ExpiryDate.SetValidation("Expiry Date must greater than Effective Date");
                    ExpiryDate.RaiseValidationError();
                    ExpiryDate.Focus();
                    result = false;
                }
                else
                    ExpiryDate.ClearValidationError();
            }
            
            return result;

        }

        private void GetPatientSourceDetails(long pID)
        {
            clsGetPatientSourceDetailsByIDBizActionVO BizAction = new clsGetPatientSourceDetailsByIDBizActionVO();
            BizAction.ID = pID;
            
            BizAction.Details = new clsPatientSourceVO();
            BizAction.Details.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    if (((clsGetPatientSourceDetailsByIDBizActionVO)arg.Result).Details != null)
                    {
                        clsPatientSourceVO obj = ((clsGetPatientSourceDetailsByIDBizActionVO)arg.Result).Details;

                        ((clsPatientSponsorVO)this.DataContext).PatientSourceType = obj.PatientSourceType;
                        ((clsPatientSponsorVO)this.DataContext).PatientSourceTypeID = obj.PatientSourceTypeID;
                        if(obj.PatientSourceType != 1)
                        {
                            
                            ((clsPatientSponsorVO)this.DataContext).EffectiveDate = obj.FromDate;
                            ((clsPatientSponsorVO)this.DataContext).ExpiryDate = obj.ToDate;
                        }
                        else if (obj.PatientSourceType == 1)//Loyalty
                        {
                           

                            if(myPatient != null)
                            {
                                ((clsPatientSponsorVO)this.DataContext).EffectiveDate = myPatient.EffectiveDate;
                                ((clsPatientSponsorVO)this.DataContext).ExpiryDate = myPatient.ExpiryDate;
                                ((clsPatientSponsorVO)this.DataContext).ReferenceNo = myPatient.LoyaltyCardNo;
                                ((clsPatientSponsorVO)this.DataContext).TariffID = myPatient.TariffID;
                            }
                            else if(myPatient == null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0 )
                            {
                                GetPatientDetails(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId);
                            }
                        }
                        //cmbPatientSource.SelectedValue = SponsorObj.PatientSourceID;
                        //cmbDesignation.SelectedValue = SponsorObj.DesignationID;
                        //cmbTariff.SelectedValue = SponsorObj.TariffID;
                        //cmbCompany.SelectedValue = SponsorObj.CompanyID;
                    }

                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }


        private void GetPatientDetails(long lPatientID,long lUnitID)
        {
            clsGetPatientBizActionVO BizAction = new clsGetPatientBizActionVO();
            BizAction.PatientDetails = new clsPatientVO();
            BizAction.PatientDetails.GeneralDetails.PatientID = lPatientID;
            BizAction.PatientDetails.GeneralDetails.UnitId = lUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null && ((clsGetPatientBizActionVO)arg.Result).PatientDetails != null)
                {
                    myPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails;
                    //((IApplicationConfiguration)App.Current).SelectedPatient = ((clsGetPatientBizActionVO)arg.Result).PatientDetails.GeneralDetails;
                    //txtMRNumber.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
                    //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    //mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                    //    " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                    //    ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    ((clsPatientSponsorVO)this.DataContext).EffectiveDate = myPatient.EffectiveDate;
                    ((clsPatientSponsorVO)this.DataContext).ExpiryDate = myPatient.ExpiryDate;
                    ((clsPatientSponsorVO)this.DataContext).ReferenceNo = myPatient.LoyaltyCardNo;

                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void cmbPatientSource_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPatientSource.SelectedItem != null && ((MasterListItem)cmbPatientSource.SelectedItem).ID > 0)
            {
                if (EditMode == false)
                 GetPatientSourceDetails(((MasterListItem)cmbPatientSource.SelectedItem).ID);

                if (((MasterListItem)cmbPatientSource.SelectedItem).ID != ((IApplicationConfiguration)App.Current).ApplicationConfigurations.CompanyPatientSourceID)
                {
                    FillTariffMaster(((MasterListItem)cmbPatientSource.SelectedItem).ID, 0);
                }
                else
                {
                    cmbTariff.ItemsSource = null;
                }

                if (((MasterListItem)cmbPatientSource.SelectedItem).ID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PatientSourceID)
                {
                    cmbCompany.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID;
                    cmbCompany.IsEnabled = false;
                    cmbAssCompany.SelectedValue = (long)0;
                    cmbAssCompany.IsEnabled = false;
                   
                    txtEmployeeNo.IsReadOnly = true;
                    txtEmployeeNo.Text = "";
                    cmbDesignation.IsEnabled = false;
                    txtRefNo.IsReadOnly = true;
                }
                else
                {
                    cmbCompany.IsEnabled = true;
                    cmbCompany.SelectedValue = (long)0;
                   
                    cmbAssCompany.IsEnabled = true;
                    txtEmployeeNo.IsReadOnly = false;
                  
                    cmbDesignation.IsEnabled = true;
                    txtRefNo.Text = "";
                    txtRefNo.IsReadOnly = false;
                   
                   
                }
            }
        }

        //private void ExpiryDate_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (dtpEffectiveDate.SelectedDate != null && ExpiryDate.SelectedDate != null && dtpEffectiveDate.SelectedDate.Value.Date > ExpiryDate.SelectedDate.Value.Date)
        //    {
        //        ExpiryDate.SetValidation("Expiry Date must greater than Effective Date");
        //        ExpiryDate.RaiseValidationError();
              
        //    }        
        //    else
        //    {
        //        ExpiryDate.ClearValidationError();
               
        //    }

        //}
    }
}

