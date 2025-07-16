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
using OPDModule;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Billing;
using OPDModule.Forms;
using PalashDynamics.ValueObjects.OutPatientDepartment.Appointment;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master.DoctorMaster;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using PalashDynamics.ValueObjects.Administration.PatientSourceMaster;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.ValueObjects.Administration.UnitMaster;
using PalashDynamics.ValueObjects.Administration.UserRights;
namespace CIMS.Forms
{
    public partial class EncounterWindow : UserControl, IInitiateCIMS, IInitiateCIMSIVF
    {

        long PatientID = 0;
        public long VAppointmentID = 0;
        public long VisitCnt = 0;
        bool IsPatientExist = false;
        bool IsFlag = true;

        long FollowupDays = 0;
        DateTime? FirstFollowupDate = null;
        DateTime? LastFollowupDate = null;
        bool freefollowup = false;

        long visitTypeID = 0;

        #region IInitiateCIMS Members
        public bool EditMode = false;
        private bool UsePrevDoctorID = false;

        private bool IsFreeFollowup = false;
        private bool LoadForm = false;

        // Added By CDS
        public bool IsFromRegisterandVisit = false;
        public long CompanyID = 0;
        //END
        public void Initiate(string Mode)
        {
            switch (Mode)
            {

                case "NEW":
                    //***Added by Ashish Z. for Check Patient Admit in IPD or Not.
                    GetActiveAdmissionOfPatient(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID, ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
                    //***

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.IsSpouse == true)
                    {
                       
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Donor Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        IsPatientExist = false;
                        break;
                    }


                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
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

                    IsPatientExist = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    // Added By CDS
                    if (IsFromRegisterandVisit == true)
                    {
                        //Change By Bhushanp For Genome 18012018
                        cmbSaveandAdvanced.Visibility = Visibility.Collapsed;
                        cmbSaveandBill.Visibility = Visibility.Visible;

                        //mElement = (TextBlock)rootPage.FindName("Patient Detail");
                        //mElement.Text = "Patient Detail";
                    }
                    // END
                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsVisitVO
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID,
                        SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID
                    };
                    // this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {

                        //Indicatior = new WaitIndicator();
                        //Indicatior.Show();
                        clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
                        //BizAction.IsActive = true;
                        BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        BizAction.Details.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;
                        BizAction.GetLatestVisit = true;

                        if (((IApplicationConfiguration)App.Current).SelectedPatient.IsDocAttached != true)
                        {
                            DocLabel.Visibility = Visibility.Visible;
                        }

                        //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                //this.DataContext = ((clsGetVisitBizActionVO)arg.Result).Details;
                                //VisitWindow.Title = "Encounter - " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName + " " +
                                //                    ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " + ((clsPatientVO)this.DataContext).GeneralDetails.LastName;
                                if (((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID > 0)
                                    ((clsVisitVO)this.DataContext).DepartmentID = ((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID;
                                ((clsVisitVO)this.DataContext).CabinID = ((clsGetVisitBizActionVO)arg.Result).Details.CabinID;

                                if (((clsGetVisitBizActionVO)arg.Result).Details.DoctorID > 0)
                                    ((clsVisitVO)this.DataContext).DoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                                ((clsVisitVO)this.DataContext).ReferredDoctor = ((clsGetVisitBizActionVO)arg.Result).Details.ReferredDoctor;
                                ((clsVisitVO)this.DataContext).Complaints = ((clsGetVisitBizActionVO)arg.Result).Details.Complaints;
                                FillRefDoctor();

                                if (((clsGetVisitBizActionVO)arg.Result).Details.ID > 0)
                                {
                                    TimeSpan ts;
                                    ts = ((clsVisitVO)this.DataContext).Date.Date.Subtract(((clsGetVisitBizActionVO)arg.Result).Details.Date.Date);

                                    if (ts.Days <= ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays)
                                        IsFreeFollowup = true;
                                    FillVisitTypeMaster();
                                    UsePrevDoctorID = true;
                                    IsPatientExist = true;
                                }
                            }
                            //Indicatior.Close();
                        };

                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }



                    break;

                case "EDIT":
                    EditMode = true;
                    //IsPatientExist = true;
                    break;

                //case "NEWR" :
                //    //this.Title = null;
                //    //this.HasCloseButton = false;
                //    this.BorderThickness = new Thickness(0);

                //    this.DataContext = new clsVisitVO { PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,  VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID };
                //    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                //           this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                //    UserControl rootPage1 = Application.Current.RootVisual as UserControl;                    
                //    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                //    mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                //        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                //        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                //    break;

                case "NEWRE":
                    IsPatientExist = true;
                    //Indicatior = new WaitIndicator();
                    //Indicatior.Show();
                    LoadForm = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    this.DataContext = new clsVisitVO
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID,
                        SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID

                    };

                    //this.Title = null;
                    //this.HasCloseButton = false;
                    //this.BorderThickness = new Thickness(0);

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                    {
                        clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
                        //BizAction.IsActive = true;
                        BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        BizAction.Details.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;
                        BizAction.GetLatestVisit = true;

                        //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                                {
                                    this.DataContext = ((clsGetVisitBizActionVO)arg.Result).Details;

                                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                                        //this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                                        cmbDepartment.SelectedValue = ((clsVisitVO)this.DataContext).DepartmentID;
                                    cmbCabin.SelectedValue = ((clsVisitVO)this.DataContext).CabinID;
                                    cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;
                                    EditMode = true;
                                }
                                else
                                {
                                    if (((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID > 0)
                                        ((clsVisitVO)this.DataContext).DepartmentID = ((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID;
                                    ((clsVisitVO)this.DataContext).CabinID = ((clsGetVisitBizActionVO)arg.Result).Details.CabinID;
                                    if (((clsGetVisitBizActionVO)arg.Result).Details.DoctorID > 0)
                                        ((clsVisitVO)this.DataContext).DoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                                    ((clsVisitVO)this.DataContext).ReferredDoctor = ((clsGetVisitBizActionVO)arg.Result).Details.ReferredDoctor;
                                    ((clsVisitVO)this.DataContext).Complaints = ((clsGetVisitBizActionVO)arg.Result).Details.Complaints;
                                    UsePrevDoctorID = true;

                                    TimeSpan ts;
                                    ts = ((clsVisitVO)this.DataContext).Date.Date.Subtract(((clsGetVisitBizActionVO)arg.Result).Details.Date.Date);

                                    if (ts.Days <= ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays)
                                        IsFreeFollowup = true;
                                    IsPatientExist = true;
                                }
                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                    break;

            }
        }

        clsMenuVO Menu;
        int flag = 0;
        public void Initiate(clsMenuVO Item)
        {
            Menu = Item;
            switch (Menu.Mode)
            {

                case "NEW":

                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
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

                    IsPatientExist = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                    mElement.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    this.DataContext = new clsVisitVO
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID,
                        SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID
                    };
                    // this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                    {

                        //Indicatior = new WaitIndicator();
                        //Indicatior.Show();
                        clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
                        //BizAction.IsActive = true;
                        BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        BizAction.Details.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;
                        BizAction.GetLatestVisit = true;

                        //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                //this.DataContext = ((clsGetVisitBizActionVO)arg.Result).Details;
                                //VisitWindow.Title = "Encounter - " + ((clsPatientVO)this.DataContext).GeneralDetails.FirstName + " " +
                                //                    ((clsPatientVO)this.DataContext).GeneralDetails.MiddleName + " " + ((clsPatientVO)this.DataContext).GeneralDetails.LastName;
                                if (((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID > 0)
                                    ((clsVisitVO)this.DataContext).DepartmentID = ((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID;
                                ((clsVisitVO)this.DataContext).CabinID = ((clsGetVisitBizActionVO)arg.Result).Details.CabinID;

                                if (((clsGetVisitBizActionVO)arg.Result).Details.DoctorID > 0)
                                    ((clsVisitVO)this.DataContext).DoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                                ((clsVisitVO)this.DataContext).ReferredDoctor = ((clsGetVisitBizActionVO)arg.Result).Details.ReferredDoctor;
                                ((clsVisitVO)this.DataContext).Complaints = ((clsGetVisitBizActionVO)arg.Result).Details.Complaints;
                                FillRefDoctor();

                                if (((clsGetVisitBizActionVO)arg.Result).Details.ID > 0)
                                {
                                    TimeSpan ts;
                                    ts = ((clsVisitVO)this.DataContext).Date.Date.Subtract(((clsGetVisitBizActionVO)arg.Result).Details.Date.Date);

                                    if (ts.Days <= ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays)
                                        IsFreeFollowup = true;

                                    UsePrevDoctorID = true;
                                    IsPatientExist = true;
                                }
                            }
                            //Indicatior.Close();
                        };

                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }

                    break;

                case "EDIT":
                    EditMode = true;
                    //IsPatientExist = true;
                    break;

                //case "NEWR" :
                //    //this.Title = null;
                //    //this.HasCloseButton = false;
                //    this.BorderThickness = new Thickness(0);

                //    this.DataContext = new clsVisitVO { PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,  VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID };
                //    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                //           this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                //    UserControl rootPage1 = Application.Current.RootVisual as UserControl;                    
                //    TextBlock mElement1 = (TextBlock)rootPage1.FindName("SampleSubHeader");

                //    mElement1.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                //        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                //        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                //    break;

                case "NEWRE":
                    IsPatientExist = true;
                    //Indicatior = new WaitIndicator();
                    //Indicatior.Show();
                    LoadForm = true;
                    PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;

                    this.DataContext = new clsVisitVO
                    {
                        PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID,
                        PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId,
                        DepartmentID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DepartmentID,
                        DoctorID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.DoctorID,
                        VisitTypeID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.VisitTypeID,
                        SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID

                    };

                    //this.Title = null;
                    //this.HasCloseButton = false;
                    //this.BorderThickness = new Thickness(0);

                    if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID > 0)
                    {
                        clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();
                        //BizAction.IsActive = true;
                        BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                        BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                        BizAction.Details.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;
                        BizAction.GetLatestVisit = true;

                        //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        Client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null && arg.Result != null)
                            {
                                if (((clsGetVisitBizActionVO)arg.Result).Details.VisitStatus == true)
                                {
                                    this.DataContext = ((clsGetVisitBizActionVO)arg.Result).Details;

                                    if (((IApplicationConfiguration)App.Current).SelectedPatient.MRNo != null)
                                        //this.txtPatient.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;

                                        cmbDepartment.SelectedValue = ((clsVisitVO)this.DataContext).DepartmentID;
                                    cmbCabin.SelectedValue = ((clsVisitVO)this.DataContext).CabinID;
                                    cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;
                                    EditMode = true;
                                }
                                else
                                {
                                    if (((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID > 0)
                                        ((clsVisitVO)this.DataContext).DepartmentID = ((clsGetVisitBizActionVO)arg.Result).Details.DepartmentID;
                                    ((clsVisitVO)this.DataContext).CabinID = ((clsGetVisitBizActionVO)arg.Result).Details.CabinID;
                                    if (((clsGetVisitBizActionVO)arg.Result).Details.DoctorID > 0)
                                        ((clsVisitVO)this.DataContext).DoctorID = ((clsGetVisitBizActionVO)arg.Result).Details.DoctorID;
                                    ((clsVisitVO)this.DataContext).ReferredDoctor = ((clsGetVisitBizActionVO)arg.Result).Details.ReferredDoctor;
                                    ((clsVisitVO)this.DataContext).Complaints = ((clsGetVisitBizActionVO)arg.Result).Details.Complaints;
                                    UsePrevDoctorID = true;

                                    TimeSpan ts;
                                    ts = ((clsVisitVO)this.DataContext).Date.Date.Subtract(((clsGetVisitBizActionVO)arg.Result).Details.Date.Date);

                                    if (ts.Days <= ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays)
                                        IsFreeFollowup = true;
                                    IsPatientExist = true;
                                }
                            }
                        };
                        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                        Client.CloseAsync();
                    }
                    break;

            }

        }

        #endregion
        public string Action { get; set; }
        public string ModuleName { get; set; }
        List<MasterListItem> DoctorList = new List<MasterListItem>();
        List<clsVisitVO> VisitListNew = new List<clsVisitVO>();

        List<MasterListItem> VisitTypeListDetailsNew = new List<MasterListItem>();


        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;
        UIElement myData = null;
        public event RoutedEventHandler OnSaveButton_Click;

//        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;

        public EncounterWindow()
        {
            InitializeComponent();
//            FillUnitList();
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

                if (Menu != null && Menu.Parent == "Surrogacy")
                    ((IInitiateCIMS)myData).Initiate("Surrogacy");
                else
                    ((IInitiateCIMS)myData).Initiate("REG");



                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);



            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //GetActiveAdmissionOfPatient(((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientUnitID, ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);


                if (IsPatientExist == false)
                {
                    //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                    ////((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
                    if (Menu != null && Menu.Parent == "Surrogacy")
                    {
                        ModuleName = "PalashDynamics";
                        Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                    }
                    else
                    {
                        ModuleName = "PalashDynamics";
                        Action = "CIMS.Forms.PatientList";
                    }
                    UserControl rootPage = Application.Current.RootVisual as UserControl;

                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                }

                if (PatientID <= 0 && LoadForm == false)
                {
                    OkButton.IsEnabled = false;
                }
                else
                {
                    if (!IsPageLoded)
                    {
                        Indicatior = new WaitIndicator();
                        Indicatior.Show();

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

                        FillVisitTypeMaster();
                        FillCabin();
                        FillDepartmentMaster(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);

                        if (VAppointmentID > 0)
                        {
                            ShowAppointmentDetails();
                        }
                        else
                        {
                            //FillDoctorList(0, 0); commented on 08082018
                             FillRefDoctor();
                        }

                        txtUnit.Text = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitName;

                        cmbDepartment.SelectedValue = ((clsVisitVO)this.DataContext).DepartmentID;
                        cmbCabin.SelectedValue = ((clsVisitVO)this.DataContext).CabinID;
                        //cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.UnitId != null && ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId > 0)
                        {
                            FillPatientPastVisit(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);
                        }

                        //by Anjali................
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.IsDocAttached != true)
                        {
                            DocLabel.Visibility = Visibility.Visible;
                        }
                        //..........................


                        cmbVisitType.Focus();
                        cmbVisitType.UpdateLayout();



                        Indicatior.Close();
                    }

                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                        " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                        ((IApplicationConfiguration)App.Current).SelectedPatient.LastName;
                    IsPageLoded = true;


                    if (((IApplicationConfiguration)App.Current).SelectedPatient.BillBalanceAmountSelf > 0)
                    {
                        string msgTitle = "";
                        string msgText = "Previous Bill Balance Amount is Pending are you sure you want to continue?";

                        MessageBoxControl.MessageBoxChildWindow msgbox =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        msgbox.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(Msgbox_OnMessageBoxClosed);
                        msgbox.Show();
                    }

                }

            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        void Msgbox_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                //do continue to Save Patient Visit
            }
            else
            {
                if (Menu != null && Menu.Parent == "Surrogacy")
                {
                    ModuleName = "PalashDynamics";
                    Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
                }
                else
                {
                    ModuleName = "PalashDynamics";
                    Action = "CIMS.Forms.PatientList";
                }

                //ModuleName = "PalashDynamics";
                //Action = "CIMS.Forms.PatientList";

                UserControl rootPage = Application.Current.RootVisual as UserControl;

                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;


        //private void TextName_KeyDown(object sender, KeyEventArgs e)
        //{
        //    textBefore = ((TextBox)sender).Text;
        //    selectionStart = ((TextBox)sender).SelectionStart;
        //    selectionLength = ((TextBox)sender).SelectionLength;
        //}

        //private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsPersonNameValid())
        //    {
        //        ((TextBox)sender).Text = textBefore;
        //        ((TextBox)sender).SelectionStart = selectionStart;
        //        ((TextBox)sender).SelectionLength = selectionLength;
        //        textBefore = "";
        //        selectionStart = 0;
        //        selectionLength = 0;
        //    }
        //}
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
        private void cmbDepartment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                //FillDoctor(((MasterListItem)cmbDepartment.SelectedItem).ID);
                FillDoctorList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((MasterListItem)cmbDepartment.SelectedItem).ID);

            }
        }

        //private void FillDoctor(long pID)
        //{
        //    clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
        //    //BizAction.IsActive = true;
        //    BizAction.MasterTable = MasterTableNameList.M_DoctorDepartmentView;
        //    if (pID > 0)
        //        BizAction.Parent = new KeyValue { Key = pID, Value = "DepartmentID" };

        //    BizAction.MasterList = new List<MasterListItem>();

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    Client.ProcessCompleted += (s, e) =>
        //    {

        //            if (e.Error == null && e.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();

        //                objList.Add(new MasterListItem(0, "- Select -"));
        //                objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);

        //                if (pID == 0 )
        //                {
        //                    txtReferenceDoctor.ItemsSource = null;
        //                    txtReferenceDoctor.ItemsSource = objList;
        //                }
        //                else
        //                {
        //                    cmbDoctor.ItemsSource = null;
        //                    cmbDoctor.ItemsSource = objList;

        //                    if (this.DataContext != null)
        //                    {
        //                        if (UsePrevDoctorID == true)
        //                        {
        //                            cmbDoctor.SelectedValue = ((clsVisitVO)this.DataContext).DoctorID;
        //                            UsePrevDoctorID = false;

        //                            if (((clsVisitVO)this.DataContext).DoctorID == 0)
        //                            {
        //                                cmbDoctor.TextBox.SetValidation("Please select the Doctor");

        //                                cmbDoctor.TextBox.RaiseValidationError();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (objList.Count > 0)
        //                            {
        //                                if (objList.Count > 1)
        //                                    cmbDoctor.SelectedValue = objList[0].ID;
        //                                else
        //                                    cmbDoctor.SelectedValue = objList[1].ID;

        //                                if (((clsGetMasterListBizActionVO)e.Result).MasterList[0].ID == 0)
        //                                {
        //                                    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
        //                                    cmbDoctor.TextBox.RaiseValidationError();
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }              
        //    };
        //    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //}


        private void FillDoctorList(long iUnitID, long iDeptID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = iUnitID;
            if ((MasterListItem)cmbDepartment.SelectedItem != null)
            {
                BizAction.DepartmentId = iDeptID;
            }
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)e.Result).MasterList);
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    if (IsFlag)
                    {
                        if (this.DataContext != null)
                        {
                            if (UsePrevDoctorID == true)
                            {
                                cmbDoctor.SelectedValue = ((clsVisitVO)this.DataContext).DoctorID;
                                FillDoctorScheduleWise();
                                if (((clsVisitVO)this.DataContext).DoctorID == 0)
                                {
                                    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                                    cmbDoctor.TextBox.RaiseValidationError();
                                }
                            }
                            else
                            {
                                if (objList.Count > 0)
                                {
                                    cmbDoctor.SelectedValue = objList[0].ID;
                                    FillDoctorScheduleWise();
                                    if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                                    {
                                        cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                                        cmbDoctor.TextBox.RaiseValidationError();
                                    }
                                }
                            }
                            IsFlag = false;
                        }
                    }
                    else
                    {
                        FillDoctorScheduleWise();
                        cmbDoctor.SelectedItem = objList[0];
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void FillDoctorScheduleWise()
        {
            GetDoctorScheduleWiseVO BizAction = new GetDoctorScheduleWiseVO();
            BizAction.DoctorScheduleDetailsList = new List<clsDoctorScheduleDetailsVO>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            if (cmbDepartment.SelectedItem != null)
                BizAction.DepartmentId = ((MasterListItem)cmbDepartment.SelectedItem).ID;
            BizAction.Day = Convert.ToInt64(DateTime.Now.DayOfWeek) + 1;

            if (dtpEncDate.SelectedDate != null)    //For New Doctor Schedule Changes modified on 29052018 
                BizAction.Date = dtpEncDate.SelectedDate.Value.Date;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsDoctorScheduleDetailsVO> ObjScheduleList = new List<clsDoctorScheduleDetailsVO>();
                    ObjScheduleList = ((GetDoctorScheduleWiseVO)e.Result).DoctorScheduleDetailsList;

                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));

                    foreach (var item in ObjScheduleList)
                    {
                        DateTime ST = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.StartTime.Value.Hour, item.StartTime.Value.Minute, 0);
                        DateTime ET = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, item.EndTime.Value.Hour, item.EndTime.Value.Minute, 0);

                        if (DateTime.Now >= ST && DateTime.Now <= ET)
                        {

                            foreach (MasterListItem item1 in ((List<MasterListItem>)(cmbDoctor.ItemsSource)))
                            {
                                if (item1.ID == item.DoctorID)
                                {
                                    objList.Add(item1);
                                }
                            }
                        }
                    }
                    if (objList.Count > 0)
                    {
                        var results = from a in objList
                                      group a by a.ID into grouped
                                      select grouped.First();
                        objList = results.ToList();
                    }
                    cmbDoctor.ItemsSource = null;
                    cmbDoctor.ItemsSource = objList;
                    if (this.DataContext != null)
                    {
                        if (UsePrevDoctorID == true)
                        {
                            cmbDoctor.SelectedValue = ((clsVisitVO)this.DataContext).DoctorID;
                            if (((clsVisitVO)this.DataContext).DoctorID == 0)
                            {
                                cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                                cmbDoctor.TextBox.RaiseValidationError();
                            }
                        }
                        else
                        {
                            if (objList.Count > 0)
                            {
                                cmbDoctor.SelectedValue = objList[0].ID;
                                if (cmbDoctor.SelectedItem != null && ((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
                                {
                                    cmbDoctor.TextBox.SetValidation("Please select the Doctor");
                                    cmbDoctor.TextBox.RaiseValidationError();
                                }
                            }
                        }
                    }
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void GetActiveAdmissionOfPatient(long PatientID, long PatientUnitID, string MRNO)
        {
            try
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
                                                     new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Admitted in IPD, You can not Mark Visit.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                msgW1.Show();
                                OkButton.IsEnabled = false;
                                cmbSaveandBill.IsEnabled = false;
                            }
                            else
                            {
                                OkButton.IsEnabled = true;
                                cmbSaveandBill.IsEnabled = true;
                            }
                        }

                    }
                };
                Client.ProcessAsync(BizObject, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
            }
        }

        private void FillRefDoctor()
        {
            clsGetRefernceDoctorBizActionVO BizAction = new clsGetRefernceDoctorBizActionVO();
            BizAction.ComboList = new List<clsComboMasterBizActionVO>();
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.IsFromVisit = true;

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
                        cmbRefDoctor.ItemsSource = null;
                        cmbRefDoctor.ItemsSource = objList;
                        cmbRefDoctor.SelectedItem = objList[0];

                        if (this.DataContext != null)
                        {
                            cmbRefDoctor.SelectedValue = ((clsVisitVO)this.DataContext).ReferredDoctorID;

                        }

                        // Added by Ashish Z. on dated 230716 (For every visit default reference doctor should be External Doctor (Visit Screen).)
                        if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.ReferralTypeID == 1)
                        {
                            cmbRefDoctor.SelectedValue = ((IApplicationConfiguration)App.Current).SelectedPatient.ReferralDoctorID;
                        }

                    }
                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }


        private void FillVisitTypeMaster()
        {
            clsGetMasterListForVisitBizActionVO BizAction = new clsGetMasterListForVisitBizActionVO();
            //  clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_VisitTypeMaster;
            BizAction.Parent = new KeyValue { Key = true, Value = "Status" };
            BizAction.MasterList = new List<MasterListItem>();



            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListForVisitBizActionVO)e.Result).MasterList);

                    VisitTypeListDetailsNew = objList.DeepCopy();
                    cmbVisitType.ItemsSource = null;
                    cmbVisitType.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    if (IsFreeFollowup == true)
                    {
                        cmbVisitType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupVisitTypeID;
                    }
                    else
                        cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;

                    if (((clsVisitVO)this.DataContext).VisitTypeID == 0)
                    {
                        cmbVisitType.TextBox.SetValidation("Please select the Visit Type");

                        cmbVisitType.TextBox.RaiseValidationError();
                    }

                }

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillCabin()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_CabinMaster;
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
                    cmbCabin.ItemsSource = null;
                    cmbCabin.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbCabin.SelectedValue = ((clsVisitVO)this.DataContext).CabinID;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        }

        private void FillDepartmentMaster(long IUnitID)
        {
            #region Commented to show/not clinical Depatments
            //clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_DepartmentUnitView;
            //BizAction.IsActive = true;

            //if (IUnitID > 0)
            //    BizAction.Parent = new KeyValue { Key = IUnitID, Value = "UnitId" };
            //BizAction.MasterList = new List<MasterListItem>();
            #endregion

            #region To show/not clinical Depatments

            clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO BizActionVo = new clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO();
            BizActionVo.IsUnitWise = true;
            BizActionVo.IsClinical = true;  // flag use to Show/not Clinical Departments  02032017
            BizActionVo.UnitID = IUnitID;   // fill Unitwise Departments  02032017

            #endregion

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {

                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "- Select -"));
                    //objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);  //Commented to show/not clinical Depatments

                    if (((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem != null)      // changes for - to show/not clinical Depatments
                    {
                        objList.AddRange(((clsGetDepartmentListForDoctorMasterByDoctorIDBizActionVO)arg.Result).MasterListItem);
                    }

                    cmbDepartment.ItemsSource = null;
                    cmbDepartment.ItemsSource = objList;
                }

                if (this.DataContext != null)
                {
                    cmbDepartment.SelectedValue = ((clsVisitVO)this.DataContext).DepartmentID;

                    if (((clsVisitVO)this.DataContext).DepartmentID == 0)
                    {
                        cmbDepartment.TextBox.SetValidation("Please select the Department");

                        cmbDepartment.TextBox.RaiseValidationError();
                    }
                }

            };

            Client.ProcessAsync(BizActionVo, ((IApplicationConfiguration)App.Current).CurrentUser);   //Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        public void SAVEclinck()
        {
            if (CheckValidations())
            {
                OkButton.IsEnabled = false;
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.ConftnMsgForAdd && EditMode == false)
                {
                    string msgTitle = "";
                    string msgText = "Are you sure you want to save the Visit Details";

                    MessageBoxControl.MessageBoxChildWindow msgW =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);

                    msgW.Show();
                }
                else
                    SaveVisit();
            }
        }
        int ClickedFlag = 0;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // By Rohini for double click problem
            ClickedFlag += 1;
            if (ClickedFlag == 1)
            {
                SAVEclinck();
            }

            //else
            //{
            //    string msgTitle = "";
            //    string msgText = "Pease enter valid values";

            //    MessageBoxControl.MessageBoxChildWindow msgW1 =
            //        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //    msgW1.Show();
            //}
            //SaveVisit();
        }

        private void cmbSaveandBill_Click(object sender, RoutedEventArgs e)
        {
            // By Rohini for double click problem
            ClickedFlag += 1;
            formSaveType = SaveType.Billing;

            if (ClickedFlag == 1)
            {
                SAVEclinck();
            }
        }

        private void cmbSaveandAdvanced_Click(object sender, RoutedEventArgs e)
        {
            formSaveType = SaveType.Advanced;
            OKButton_Click(sender, e);
        }

        private SaveType formSaveType = SaveType.Visit;
        private enum SaveType
        {
            Visit = 0,
            Billing = 1,
            Advanced = 2
        }

        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
                SaveVisit();
            else
            {
                OkButton.IsEnabled = true;
                ClickedFlag = 0;
            }
        }

        public bool CheckValidations()
        {
            bool result = true;

            if ((MasterListItem)cmbDoctor.SelectedItem == null)
            {
                cmbDoctor.TextBox.SetValidation("Please select the Doctor");

                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbDoctor.SelectedItem).ID == 0)
            {
                cmbDoctor.TextBox.SetValidation("Please select the Doctor");

                cmbDoctor.TextBox.RaiseValidationError();
                cmbDoctor.Focus();
                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else
                cmbDoctor.ClearValidationError();

            if ((MasterListItem)cmbDepartment.SelectedItem == null)
            {
                cmbDepartment.TextBox.SetValidation("Please select the Department");

                cmbDepartment.TextBox.RaiseValidationError();
                cmbDepartment.Focus();
                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbDepartment.SelectedItem).ID == 0)
            {
                cmbDepartment.TextBox.SetValidation("Please select the Department");

                cmbDepartment.TextBox.RaiseValidationError();
                cmbDepartment.Focus();
                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else
                cmbDepartment.TextBox.ClearValidationError();




            if ((MasterListItem)cmbVisitType.SelectedItem == null)
            {
                cmbVisitType.TextBox.SetValidation("Please select the Visit Type");

                cmbVisitType.TextBox.RaiseValidationError();
                cmbVisitType.Focus();
                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else if (((MasterListItem)cmbVisitType.SelectedItem).ID == 0)
            {
                cmbVisitType.TextBox.SetValidation("Please select the Visit Type");

                cmbVisitType.TextBox.RaiseValidationError();
                cmbVisitType.Focus();

                //rohini reset flag
                ClickedFlag = 0;
                result = false;
            }
            else
                cmbVisitType.ClearValidationError();


            return result;

        }

        private void SaveVisit()
        {
            try
            {
                clsAddVisitBizActionVO BizAction = new clsAddVisitBizActionVO();
                BizAction.VisitDetails = (clsVisitVO)this.DataContext;

                if (cmbDepartment.SelectedItem != null)
                    BizAction.VisitDetails.DepartmentID = ((MasterListItem)cmbDepartment.SelectedItem).ID;

                if (cmbVisitType.SelectedItem != null)
                    BizAction.VisitDetails.VisitTypeID = ((MasterListItem)cmbVisitType.SelectedItem).ID;

                if (cmbCabin.SelectedItem != null)
                    BizAction.VisitDetails.CabinID = ((MasterListItem)cmbCabin.SelectedItem).ID;

                if (cmbDoctor.SelectedItem != null)
                    BizAction.VisitDetails.DoctorID = ((MasterListItem)cmbDoctor.SelectedItem).ID;
                if (cmbRefDoctor.SelectedItem != null)
                {

                    BizAction.VisitDetails.ReferredDoctorID = ((MasterListItem)cmbRefDoctor.SelectedItem).ID;
                    BizAction.VisitDetails.ReferredDoctor = ((MasterListItem)cmbRefDoctor.SelectedItem).Description;
                }

                //if (cmbReferenceDoctor.SelectedItem != null)
                //    BizAction.VisitDetails.ReferredDoctorID = ((MasterListItem)cmbReferenceDoctor.SelectedItem).ID;

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
                                ((clsVisitVO)this.DataContext).ID = ((clsAddVisitBizActionVO)arg.Result).VisitDetails.ID;
                                if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                                    ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID = ((clsVisitVO)this.DataContext).ID;

                                if (OnSaveButton_Click != null)
                                    OnSaveButton_Click(this, new RoutedEventArgs());

                                if (EditMode == false)
                                {
                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Visit saved successfully.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    //rohini reset flag
                                    ClickedFlag = 0;

                                    msgW1.OnMessageBoxClosed += (re) =>
                                    {
                                        if (PackageDetails.IsPackage == true)
                                        {
                                            AssignDoctorForPackageService WinDoctor = new AssignDoctorForPackageService();
                                            WinDoctor.ServiceID = PackageDetails.ServiceID;
                                            WinDoctor.VisitID = ((clsAddVisitBizActionVO)arg.Result).VisitDetails.ID;

                                            WinDoctor.Show();
                                        }

                                        if (Menu != null && Menu.Parent == "Surrogacy")
                                        {
                                            if (formSaveType != SaveType.Billing)
                                            {
                                                if (formSaveType == SaveType.Advanced)  // Only In Case From Registre And Visit Button
                                                {

                                                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmAdvance") as UIElement;
                                                    frmAdvance win = new frmAdvance();

                                                    win.IsFromRegisterandVisit = true;

                                                    //if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                                                    //{
                                                    //    win.setpatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                                                    //}                                                    
                                                    if (CompanyID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID)
                                                    {
                                                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                                        mElement.Text = "";
                                                        mElement.Text = "Patient Detail";


                                                        ((IInitiateCIMS)win).Initiate("PA");  // Patient Advance

                                                        ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                    }
                                                    else
                                                    {
                                                        ((IInitiateCIMS)win).Initiate("CA");  // Company Advance

                                                        ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                    }
                                                }
                                                else
                                                {
                                                    // Only Normal Visit Then Patient Search List Form 
                                                    ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy");
                                                }
                                            }
                                            else if (formSaveType == SaveType.Billing)
                                            {
                                                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmBill") as UIElement;
                                                frmBill win = new frmBill();
                                                win.IsFromSaveAndBill = true;
                                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                                //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleSubHeader");

                                                //mElement1.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                                //" " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                                                //((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                                //mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                                mElement.Text = "Bill Details";

                                                ((IInitiateCIMS)win).Initiate("VISIT");
                                                ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                //rohini reset flag
                                                ClickedFlag = 0;

                                            }

                                            //((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy");
                                        }
                                        else
                                        {
                                            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                            if (formSaveType != SaveType.Billing)
                                            {
                                                if (formSaveType == SaveType.Advanced)  // Only In Case From Registre And Visit Button
                                                {

                                                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmAdvance") as UIElement;
                                                    frmAdvance win = new frmAdvance();

                                                    win.IsFromRegisterandVisit = true;

                                                    //if (((IApplicationConfiguration)App.Current).SelectedPatient != null)
                                                    //{
                                                    //    win.setpatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                                                    //}                                                    
                                                    if (CompanyID == ((IApplicationConfiguration)App.Current).ApplicationConfigurations.SelfCompanyID)
                                                    {
                                                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                        TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                                        mElement.Text = "";
                                                        mElement.Text = "Patient Detail";


                                                        ((IInitiateCIMS)win).Initiate("PA");  // Patient Advance

                                                        ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                    }
                                                    else
                                                    {
                                                        ((IInitiateCIMS)win).Initiate("CA");  // Company Advance

                                                        ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                    }
                                                }
                                                else
                                                {
                                                    // Only Normal Visit Then Patient Search List Form 
                                                    ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                                }
                                            }
                                            else if (formSaveType == SaveType.Billing)
                                            {

                                                //bool freefollowup = false;


                                                //FollowupDays=((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays;
                                                ////////////
                                                //cmbVisitType.SelectedValue = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupVisitTypeID;

                                                //   cmbVisitType.SelectedValue = ((clsVisitVO)this.DataContext).VisitTypeID;

                                                ///////////////


                                                //    if(    ((clsVisitVO)cmbVisitType.SelectedValue).ID==10)

                                                //if(FirstFollowupDate!=null)
                                                //LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

                                                //if (LastFollowupDate != null)
                                                //{
                                                //    if (dtpEncDate.SelectedDate.Value.Date.AddDays(1) <= LastFollowupDate)
                                                //    {
                                                //        freefollowup = true;
                                                //    }
                                                //}




                                                UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.frmBill") as UIElement;
                                                frmBill win = new frmBill();
                                                win.IsFromSaveAndBill = true;
                                                //    win.IsfreeFollowupConsultation = freefollowup;//Added by yk to check whether it is free followup or not
                                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                                TextBlock mElement = (TextBlock)rootPage.FindName("SampleHeader");
                                                //TextBlock mElement1 = (TextBlock)rootPage.FindName("SampleSubHeader");

                                                //mElement1.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName +
                                                //" " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " " +
                                                //((IApplicationConfiguration)App.Current).SelectedPatient.LastName;

                                                //mElement1.Text += " - " + ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                                                mElement.Text = "Bill Details";

                                                ((IInitiateCIMS)win).Initiate("VISIT");
                                                ((IApplicationConfiguration)App.Current).OpenMainContent(win as UIElement);
                                                //rohini reset flag
                                                ClickedFlag = 0;

                                            }

                                        }


                                    };
                                    msgW1.Show();
                                    OkButton.IsEnabled = true;
                                    //rohini reset flag
                                    ClickedFlag = 0;
                                    if (VAppointmentID > 0)
                                        AddMarkVisitForAppointment();
                                    SaveVisitTypeCharges(BizAction.VisitDetails.VisitTypeID, BizAction.VisitDetails.ID);

                                    // Check Visit Count
                                    GetVisitCount(BizAction.VisitDetails.ID);
                                }

                                GetVisitDetails();
                            }
                        }
                        //rohini reset flag
                        ClickedFlag = 0;

                    }
                    else
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while adding visit details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        OkButton.IsEnabled = true;
                        //rohini reset flag
                        ClickedFlag = 0;

                    }
                };

                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                // throw;
                OkButton.IsEnabled = true;
                //rohini reset flag
                ClickedFlag = 0;
            }
        }

        private void SaveVisitTypeCharges(long pVisitTypeID, long pVisitID)
        {
            clsGetVisitTypeBizActionVO BizAction = new clsGetVisitTypeBizActionVO();

            BizAction.ID = pVisitTypeID;


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
                            GetServiceDetails(objVO.ServiceID, ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID, pVisitID);
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



            //BizAction.PatientSourceType = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceType;
            //BizAction.PatientSourceTypeID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientSourceTypeID;
            BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;




            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList != null && ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList.Count > 0)
                    {
                        clsServiceMasterVO mService = new clsServiceMasterVO();

                        mService = ((clsGetTariffServiceListBizActionVO)arg.Result).ServiceList[0];

                        // clsChargeVO mCharge = new clsChargeVO() { Opd_Ipd_External = 0, Opd_Ipd_External_Id = pVisitID, ServiceSpecilizationID = mService.Specialization, TariffServiceId = mService.TariffServiceMasterID, ServiceId = mService.ID, ServiceName = mService.Description, Rate = Convert.ToDouble(mService.Rate), Quantity = 1 };

                        // SaveCharge(mCharge);

                        clsChargeVO itemC = new clsChargeVO();
                        itemC.Opd_Ipd_External = 0;
                        itemC.Opd_Ipd_External_Id = pVisitID;
                        itemC.Opd_Ipd_External_UnitId = ((clsVisitVO)this.DataContext).PatientUnitId;

                        itemC.ServiceSpecilizationID = mService.Specialization;
                        itemC.TariffServiceId = mService.TariffServiceMasterID;
                        itemC.TariffId = pTariffID;

                        itemC.ServiceId = mService.ID;
                        itemC.ServiceName = mService.ServiceName;
                        itemC.Quantity = 1;
                        itemC.RateEditable = mService.RateEditable;
                        itemC.MinRate = Convert.ToDouble(mService.MinRate);
                        itemC.MaxRate = Convert.ToDouble(mService.MaxRate);
                        //itemC.Rate = Convert.ToDouble(mService.Rate);

                        if (((MasterListItem)cmbDoctor.SelectedItem).Value == 0)
                        {
                            itemC.Rate = Convert.ToDouble(mService.Rate);
                        }
                        else
                        {
                            itemC.Rate = ((MasterListItem)cmbDoctor.SelectedItem).Value;
                        }

                        itemC.TotalAmount = itemC.Rate * itemC.Quantity;


                        if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 3 || (((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID == 6))
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


                        if (freefollowup)
                        {
                            itemC.ConcessionPercent = 100;
                        }



                        SaveCharge(itemC);
                        //rohini reset flag
                        ClickedFlag = 0;

                    }
                }

                else
                {
                    //HtmlPage.Window.Alert("Error occured while processing.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                    //rohini reset flag
                    ClickedFlag = 0;
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        /// <summary>
        /// Function is for saving the charge for that visit.
        /// </summary>
        /// <param name="pCharge"></param>
        private void SaveCharge(clsChargeVO pCharge)
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {
                clsAddChargeBizActionVO BizAction = new clsAddChargeBizActionVO();
                BizAction.Details = pCharge;
                //Added By CDS for Avoiding Double Entry In T_ChargeDetails
                BizAction.FromVisit = true;
                BizAction.Details.SelectedDoctor.ID = ((MasterListItem)cmbDoctor.SelectedItem).ID;




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
                        //rohini reset flag
                        ClickedFlag = 0;
                    }
                    else
                    {
                        // System.Windows.Browser.HtmlPage.Window.Alert("Error occured while saving Visit Type Charges.");
                        Indicatior.Close();
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while saving Visit Type Charges.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        //rohini reset flag
                        ClickedFlag = 0;
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

        private void AddMarkVisitForAppointment()
        {
            clsAddMarkVisitInAppointmenBizActionVO BizAction = new clsAddMarkVisitInAppointmenBizActionVO();
            BizAction.AppointmentDetails = new clsAppointmentVO();
            BizAction.AppointmentDetails.AppointmentID = VAppointmentID;
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
                    //rohini reset flag
                    ClickedFlag = 0;
                }


            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void GetVisitCount(long pVisitID)
        {
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();

            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.GetVisitCount = true;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    VisitCnt = ((clsGetVisitBizActionVO)arg.Result).Details.VisitCount;
                    if (VisitCnt == 1)
                    {
                        SaveRegistrationTypeCharges(((IApplicationConfiguration)App.Current).SelectedPatient.PatientTypeID, pVisitID);
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }


        private void SaveRegistrationTypeCharges(long PatientTypeID, long pVisitID)
        {
            clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO BizAction = new clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO();

            BizAction.PatientTypeID = PatientTypeID;


            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)e.Result).List != null)
                    {
                        clsRegistrationChargesVO objVO = new clsRegistrationChargesVO();

                        if (((clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)e.Result).List.Count > 0)
                            objVO = ((clsGetRegistrationChargesDetailsByPatientTypeIDBizActionVO)e.Result).List[0];

                        if (objVO.PatientServiceId > 0)
                        {
                            GetServiceDetails(objVO.PatientServiceId, ((IApplicationConfiguration)App.Current).SelectedPatient.TariffID, pVisitID);
                        }
                    }
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }



        private void GetVisitDetails()
        {
            clsGetVisitBizActionVO BizAction = new clsGetVisitBizActionVO();

            BizAction.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.GetLatestVisit = true;

            //BizAction.Details.VisitId = ((clsVisitVO)dgEncounterList.SelectedItem).VisitId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    ((clsVisitVO)this.DataContext).OPDNO = ((clsGetVisitBizActionVO)arg.Result).Details.OPDNO;
                }
                //rohini reset flag
                ClickedFlag = 0;

            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // this.DialogResult = false;


        }

        private void CmdPatientSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChildWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //this.Close();
            }
        }


        private void CancelButton_Checked(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = new PalashDynamics.ValueObjects.Patient.clsPatientGeneralVO();
            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
            //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            if (Menu != null && Menu.Parent == "Surrogacy")
            {
                ModuleName = "PalashDynamics";
                Action = "PalashDynamics.Forms.PatientView.PatientListForSurrogacy";
            }
            else
            {
                ModuleName = "PalashDynamics";
                Action = "CIMS.Forms.PatientList";
            }
            UserControl rootPage = Application.Current.RootVisual as UserControl;

            WebClient c2 = new WebClient();
            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        //private void FillPatientPastVisitNew(string MrNo)
        //{
        //    clsGetPatientPastVisitBizActionVO BizAction = new clsGetPatientPastVisitBizActionVO();
        //    BizAction.MRNO = MrNo;
        //    //if (BizAction.UnitID != null || BizAction.UnitID != 0)
        //    //{
        //    //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    //}

        //    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
        //    {
        //        BizAction.UnitID = 0;
        //    }
        //    else
        //    {
        //        BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
        //    }
        //    BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
        //    BizAction.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //   // dgPastVisitList.ItemsSource = null;

        //    client.ProcessCompleted += (s, arg) =>
        //    {
        //        if (arg.Error == null)
        //        {
        //            if (arg.Result != null && ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList != null)
        //            {

        //                //foreach (var item in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList)
        //                //{                          
        //                //     if (item.VisitTypeID==10)
        //                //     FirstFollowupDate = item.Date;                    
        //                //}  

        //                //       var uniqueGuys = from d in from d in tests.AsEnumerable()
        //                //group d by d.ID into g
        //                //select g.OrderBy(p => p.DateTime).First(); 


        //                //FollowupDays = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays;                      

        //                //if (FirstFollowupDate != null)
        //                //    LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

        //                //if (LastFollowupDate != null)
        //                //{
        //                //    if (dtpEncDate.SelectedDate.Value.Date.AddDays(1) <= LastFollowupDate)
        //                //    {
        //                //        freefollowup = true;
        //                //    }
        //                //}

        //                //var q = from n in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList
        //                //       where n.VisitTypeID==10
        //                //        select n;


        //                visitTypeID = ((clsVisitVO)this.DataContext).VisitTypeID;

        //                //((clsVisitTypeVO)cmbVisitType.SelectedItem).ID;
        //                int count = 0;
        //                if (((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList.Count > 0)
        //                {
        //                    var q = from n in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList
        //                            where n.IsFree == true && n.FreeDaysDuration > 0 && n.ConsultationVisitTypeID == visitTypeID
        //                            select n;

        //                    foreach (var x in q)
        //                    {
        //                        ++count;
        //                        FollowupDays = x.FreeDaysDuration;
        //                    }


        //                    if (count > 0)
        //                    {
        //                        var result = q.OrderBy(p => p.PastVisitDate).First();

        //                        //q.Min(x=>x.PastVisitDate);



        //                        FirstFollowupDate = Convert.ToDateTime(result.PastVisitDate);
        //                    }

        //                    if (FirstFollowupDate != null)
        //                        LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

        //                    //////////////////Check cond///////////////////////////////
        //                    if (LastFollowupDate != null)
        //                    {
        //                        if (dtpEncDate.SelectedDate.Value.Date <= LastFollowupDate)
        //                        {
        //                            freefollowup = true;
        //                        }
        //                    }
        //                    ///////////////////////////////////////////////////
        //                }

        //                //Convert.ToDateTime(result.);

        //              //  dgPastVisitList.ItemsSource = ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList;
        //            }
        //        }
        //        else
        //        {
        //            //HtmlPage.Window.Alert("Error occured while processing.");
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //                          new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgW1.Show();
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}

        private void FillPatientPastVisit(string MrNo)
        {
            clsGetPatientPastVisitBizActionVO BizAction = new clsGetPatientPastVisitBizActionVO();
            BizAction.MRNO = MrNo;
            //if (BizAction.UnitID != null || BizAction.UnitID != 0)
            //{
            //    BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            //}

            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                BizAction.UnitID = 0;
            }
            else
            {
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            }
            BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizAction.SpouseID = ((IApplicationConfiguration)App.Current).SelectedPatient.SpouseID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            dgPastVisitList.ItemsSource = null;

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null && ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList != null)
                    {

                        VisitListNew = ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList.DeepCopy();
                        //foreach (var item in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList)
                        //{                          
                        //     if (item.VisitTypeID==10)
                        //     FirstFollowupDate = item.Date;                    
                        //}  

                        //       var uniqueGuys = from d in from d in tests.AsEnumerable()
                        //group d by d.ID into g
                        //select g.OrderBy(p => p.DateTime).First(); 


                        //FollowupDays = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.FreeFollowupDays;                      

                        //if (FirstFollowupDate != null)
                        //    LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

                        //if (LastFollowupDate != null)
                        //{
                        //    if (dtpEncDate.SelectedDate.Value.Date.AddDays(1) <= LastFollowupDate)
                        //    {
                        //        freefollowup = true;
                        //    }
                        //}

                        //var q = from n in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList
                        //       where n.VisitTypeID==10
                        //        select n;


                        //visitTypeID =  ((clsVisitVO)this.DataContext).VisitTypeID;

                        //      //((clsVisitTypeVO)cmbVisitType.SelectedItem).ID;
                        //int count = 0;
                        //if (((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList.Count > 0 )
                        //{
                        //    var q = from n in ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList
                        //            where n.IsFree == true && n.FreeDaysDuration>0 && n.ConsultationVisitTypeID==visitTypeID
                        //            select n;

                        //    foreach (var x in q)
                        //    {
                        //        ++count;
                        //        FollowupDays = x.FreeDaysDuration;
                        //    }


                        //    if (count>0)
                        //    {
                        //        var result = q.OrderBy(p => p.PastVisitDate).First();                         

                        //        FirstFollowupDate = Convert.ToDateTime(result.PastVisitDate);
                        //    }

                        //    if (FirstFollowupDate != null)
                        //        LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

                        //    //////////////////Check cond///////////////////////////////
                        //    if (LastFollowupDate != null)
                        //    {
                        //        if (dtpEncDate.SelectedDate.Value.Date <= LastFollowupDate)
                        //        {
                        //            freefollowup = true;
                        //        }
                        //    }
                        //    ///////////////////////////////////////////////////
                        //}

                        //Convert.ToDateTime(result.);

                        dgPastVisitList.ItemsSource = ((clsGetPatientPastVisitBizActionVO)arg.Result).VisitList;
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

        private void OkButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void lnkSearch_Click(object sender, RoutedEventArgs e)
        {
            PrimarySymptomsSearchWindow Win = new PrimarySymptomsSearchWindow();
            Win.OnSaveButton_Click += new RoutedEventHandler(Win_OnSaveButton_Click);
            Win.Show();
        }

        void Win_OnSaveButton_Click(object sender, RoutedEventArgs e)
        {
            PrimarySymptomsSearchWindow Itemswin = (PrimarySymptomsSearchWindow)sender;

            if (Itemswin.DialogResult == true)
            {
                if (Itemswin.SelectedItems != null)
                {
                    string strSymptoms = "";

                    foreach (var item in Itemswin.SelectedItems)
                    {
                        strSymptoms = strSymptoms + item.Description;
                        strSymptoms = strSymptoms + ",";
                    }

                    if (strSymptoms.EndsWith(","))
                        strSymptoms = strSymptoms.Remove(strSymptoms.Length - 1, 1);

                    if (((clsVisitVO)this.DataContext).Complaints != null && ((clsVisitVO)this.DataContext).Complaints.Length > 0)
                        ((clsVisitVO)this.DataContext).Complaints = ((clsVisitVO)this.DataContext).Complaints + ",";

                    ((clsVisitVO)this.DataContext).Complaints = ((clsVisitVO)this.DataContext).Complaints + strSymptoms;

                }
            }
        }

        private void ShowAppointmentDetails()
        {
            clsGetAppointmentByIdBizActionVO BizActionObj = new clsGetAppointmentByIdBizActionVO();
            BizActionObj.AppointmentDetails = new clsAppointmentVO();
            BizActionObj.AppointmentDetails.AppointmentID = VAppointmentID;
            BizActionObj.AppointmentDetails.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            Uri address2 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client2 = new PalashServiceClient("BasicHttpBinding_IPalashService", address2.AbsoluteUri);
            Client2.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {

                    clsGetAppointmentByIdBizActionVO mObj = new clsGetAppointmentByIdBizActionVO();
                    mObj = (clsGetAppointmentByIdBizActionVO)arg.Result;
                    ((clsVisitVO)this.DataContext).DepartmentID = mObj.AppointmentDetails.DepartmentId;
                    ((clsVisitVO)this.DataContext).DoctorID = mObj.AppointmentDetails.DoctorId;
                    UsePrevDoctorID = true;
                    // FillDoctorList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId, ((clsVisitVO)this.DataContext).DepartmentID);


                }


            };

            Client2.ProcessAsync(BizActionObj, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client2.CloseAsync();

        }

        private void cmbVisitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MasterListItem VisitTypeSelected = new MasterListItem();
            VisitTypeSelected = ((MasterListItem)cmbVisitType.SelectedItem);



            if (VisitTypeSelected.ConsultationVisitTypeID > 0)
            {
                visitTypeID = VisitTypeSelected.ID;
                //((clsVisitVO)this.DataContext).VisitTypeID;
                long gridvisittypeID = 0;

                //((clsVisitTypeVO)cmbVisitType.SelectedItem).ID;
                int count = 0;
                int countForgrid = 0;
                if (VisitTypeListDetailsNew.Count > 0)
                {
                    var q = from n in VisitTypeListDetailsNew
                            where n.Isfree == true && n.FreeDaysDuration > 0 && n.ID == visitTypeID
                            select n;

                    foreach (var x in q)
                    {
                        ++count;
                        FollowupDays = x.FreeDaysDuration;
                        gridvisittypeID = x.ConsultationVisitTypeID;
                    }


                    //VisitListNew
                    var grid = from n in VisitListNew
                               where n.VisitTypeID == gridvisittypeID
                               select n;
                    foreach (var g in grid)
                    {
                        ++countForgrid;
                    }


                    if (count > 0 && countForgrid > 0)
                    {
                        var result = grid.OrderByDescending(p => p.PastVisitDate).First();

                        //q.Min(x=>x.PastVisitDate);



                        FirstFollowupDate = Convert.ToDateTime(result.PastVisitDate);

                        /////////Block2////////////////
                        if (FirstFollowupDate != null)
                            LastFollowupDate = FirstFollowupDate.Value.AddDays(FollowupDays);

                        //////////////////Check cond///////////////////////////////
                        if (LastFollowupDate != null)
                        {
                            if (dtpEncDate.SelectedDate.Value.Date <= LastFollowupDate)
                            {
                                freefollowup = true;
                            }
                            else
                            {
                                freefollowup = false;
                            }
                        }
                        ///////////////END////////////////////////////

                    }


                }


            }
            else
            {
                freefollowup = false;
            }
            // ((clsVisitVO)this.DataContext).VisitTypeID = ((MasterListItem)cmbVisitType.SelectedItem).ID;
            //  FillPatientPastVisitNew(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo);

            if (cmbVisitType.SelectedItem != null && ((MasterListItem)cmbVisitType.SelectedItem).ID > 0)
                CheckVisitTypeMappedWithPackageService();
        }

        private void CheckVisitTypeMappedWithPackageService()
        {
            long lVisitTypeID = ((MasterListItem)cmbVisitType.SelectedItem).ID;
            GetPackage(lVisitTypeID);


        }
        clsVisitTypeVO PackageDetails { get; set; }
        private void GetPackage(long iID)
        {
            clsCheckVisitTypeMappedWithPackageServiceBizActionVO BizAction = new clsCheckVisitTypeMappedWithPackageServiceBizActionVO();
            BizAction.VisitTypeDetails = new clsVisitTypeVO();
            BizAction.VisitTypeID = iID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    PackageDetails = ((clsCheckVisitTypeMappedWithPackageServiceBizActionVO)arg.Result).VisitTypeDetails;
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void OkButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                SAVEclinck();
            }
        }



        //private void FillUnitList()
        //{

        //    clsGetUnitDetailsByIDBizActionVO BizAction = new clsGetUnitDetailsByIDBizActionVO();
        //    BizAction.UserID = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
        //    BizAction.IsUserWise = true;
        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

        //    client.ProcessCompleted += (s, ea) =>
        //    {
        //        List<MasterListItem> objList = new List<MasterListItem>();
        //        if (ea.Error == null && ea.Result != null)
        //        {
        //            objList.Add(new MasterListItem(0, "- Select -"));
        //            objList.AddRange(((clsGetUnitDetailsByIDBizActionVO)ea.Result).ObjMasterList);
        //            cmbClinic.ItemsSource = null;
        //            cmbClinic.ItemsSource = objList;
        //            cmbClinic.SelectedItem = objList[0];
        //        }
        //        if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO)
        //        {
        //            var res = from r in objList
        //                      where r.ID == User.UserLoginInfo.UnitId
        //                      select r;
        //            cmbClinic.SelectedItem = ((MasterListItem)res.First());
        //            cmbClinic.IsEnabled = true;
        //        }
        //        else
        //        {
        //            GetUserRights(((IApplicationConfiguration)App.Current).CurrentUser.ID);
        //            cmbClinic.SelectedValue = User.UserLoginInfo.UnitId;
        //            cmbClinic.IsEnabled = false;
        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();

        //}

        //public void GetUserRights(long UserId)
        //{
        //    try
        //    {
        //        clsGetUserRightsBizActionVO objBizVO = new clsGetUserRightsBizActionVO();
        //        objBizVO.objUserRight = new clsUserRightsVO();
        //        objBizVO.objUserRight.UserID = UserId;
        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
        //        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        client.ProcessCompleted += (s, ea) =>
        //        {
        //            clsUserRightsVO objUser;
        //            if (ea.Error == null && ea.Result != null)
        //            {
        //                objUser = ((clsGetUserRightsBizActionVO)ea.Result).objUserRight;

        //                if (objUser.IsCrossAppointment)
        //                {
        //                    cmbClinic.IsEnabled = true;
        //                }
        //                else
        //                {
        //                    cmbClinic.IsEnabled = false;

        //                }
        //            }
        //        };
        //        client.ProcessAsync(objBizVO, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        client.CloseAsync();
        //        client = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //private void cmbClinic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{

        //}


    }
}

