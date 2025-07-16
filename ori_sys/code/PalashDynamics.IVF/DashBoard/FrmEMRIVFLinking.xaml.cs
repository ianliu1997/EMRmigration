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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.ValueObjects.Patient;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Reflection;
using PalashDynamics.ValueObjects;
using System.Windows.Media.Imaging;
using System.IO;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class FrmEMRIVFLinking : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public clsMenuVO NewObj = new clsMenuVO();
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;

        WaitIndicator indicator;
        public clsVisitVO CurrentVisit { get; set; }



        public FrmEMRIVFLinking()
        {
            InitializeComponent();         
            //GetPatientCurrentVisit();
        }

        public FrmEMRIVFLinking(clsMenuVO NewObj)
        {
            this.NewObj = NewObj;
            InitializeComponent();
			LoadPatientHeader();
            //GetPatientCurrentVisit();
        }


        //void FrmEMRIVFLinking_Loaded(object sender, RoutedEventArgs e)
        //{
            
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LoadPatientHeader()
        {
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                clsGetPatientConsoleHeaderDeailsBizActionVO BizAction = new clsGetPatientConsoleHeaderDeailsBizActionVO();
                BizAction.PatientID = NewObj.PatientID;
                BizAction.UnitID = NewObj.PatientUnitID;
                BizAction.ISOPDIPD = false;
              
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PatientDetails = ((clsGetPatientConsoleHeaderDeailsBizActionVO)args.Result).PatientDetails;
                        BizAction.PatientDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                        if (BizAction.PatientDetails.Gender.ToUpper() == "MALE")
                        {
                            Patient.DataContext = BizAction.PatientDetails;                           
                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/MAle.png", UriKind.Relative));
                        }
                        else
                        {
                            Patient.DataContext = BizAction.PatientDetails;                           
                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/images1.jpg", UriKind.Relative));

                        }
                        GetPatientCurrentVisit();
                    }                  
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
                throw;
            }
        }

        private void GetPatientCurrentVisit()
        {
            clsGetEMRVisitBizActionVO BizActionVisit = new clsGetEMRVisitBizActionVO();
            BizActionVisit.Details = new clsVisitVO();
            if (SelectedPatient.VisitDate == DateTime.Today)
            {
                BizActionVisit.GetLatestVisit = true;
            }
            //BizActionVisit.Details.PatientId = SelectedPatient.PatientID;
            //BizActionVisit.Details.PatientUnitId = SelectedPatient.UnitId;
            BizActionVisit.Details.PatientId = NewObj.PatientID;
            BizActionVisit.Details.PatientUnitId = NewObj.PatientUnitID;
            BizActionVisit.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionVisit.Details.ID = SelectedPatient.VisitID;
            //if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            //{
            //    indicator = new WaitIndicator();
            //    indicator.Show();
            //}
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient clientVisit = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientVisit.ProcessCompleted += (sVisit, argVisit) =>
                {
                    if (argVisit.Error == null && argVisit.Result != null)
                    {
                        BizActionVisit = (clsGetEMRVisitBizActionVO)argVisit.Result;
                        CurrentVisit = ((clsGetEMRVisitBizActionVO)argVisit.Result).Details;
                        // added by EMR Changes Added by Ashish Z. on dated 31052017
                        CurrentVisit.EMRModVisitDate = CurrentVisit.Date;
                        CurrentVisit.EMRModVisitDate = CurrentVisit.EMRModVisitDate.AddDays(((IApplicationConfiguration)App.Current).ApplicationConfigurations.EMRModVisitDateInDays);
                        //End
                        CurrentVisit.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            CurrentVisit.DoctorCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
                        }
                        //PalashDynamics.SearchResultLists.DisplayPatientDetails winDisplay = new PalashDynamics.SearchResultLists.DisplayPatientDetails();
                        ////winDisplay.MinWidth = this.ActualWidth;
                        //ResultListContent.Content = winDisplay;
                        string str = "    " + CurrentVisit.Allergies;
                        if (str.Length > 150)
                        {
                            str = str.Substring(0, 150) + "...";
                        }
                        allergies.Content = String.Format(str);

                        if (CurrentVisit.CoupleMRNO != null)
                        {
                            if (CurrentVisit.CoupleMRNO.Trim() != "")
                            {
                                CoupleMrNo.Visibility = Visibility.Visible;
                                CoupleMRNO.Content = string.Format(CurrentVisit.CoupleMRNO);
                            }
                        }

                        if (CurrentVisit.DonorMRNO != null)
                        {
                            if (CurrentVisit.DonorMRNO.Trim() != "")
                            {
                                DonorMrNo.Visibility = Visibility.Visible;
                                DonorMRNO.Content = string.Format(CurrentVisit.DonorMRNO);
                            }
                        }

                        if (CurrentVisit.SurrogateMRNO != null)
                        {
                            if (CurrentVisit.SurrogateMRNO.Trim() != "")
                            {
                                SurrogateMrNo.Visibility = Visibility.Visible;
                                SurrogateMRNO.Content = string.Format(CurrentVisit.SurrogateMRNO);
                            }
                        }

                        tabControl1_SelectionChanged(null, null);
                    }
                    indicator.Close();
                };
                clientVisit.ProcessAsync(BizActionVisit, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientVisit.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //GetPatientCurrentVisit();
            //LoadPatientHeader();
        }



        frmIVFEMRProcedure frmProcedure;
        private void tabControl1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TIProcedure != null)
            {
                if (TIProcedure.IsSelected)
                {
                    UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIVFEMRProcedure") as UIElement;
                    frmProcedure = new frmIVFEMRProcedure();
                    frmProcedure = (frmIVFEMRProcedure)mydata5;
                    frmProcedure.IsArt = NewObj.IsArt;
                    frmProcedure.PlanTherapyId = NewObj.PlanTherapyId;
                    frmProcedure.PlanTherapyUnitId = NewObj.PlanTherapyUnitId;
                    frmProcedure.CurrentVisit = CurrentVisit;
                    frmProcedure.IsEnabledControl = CurrentVisit.VisitStatus;
                    //frmProcedure.SelectedPatient = new DataDrivenApplication.Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                    ProcedureContent.Content = frmProcedure;
                }
            }

            if (TIPrescription != null)
            {
                if (TIPrescription.IsSelected)
                {
                    frmIVFEMRMedication frmEMRMed;
                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIVFEMRMedication") as UIElement;
                    frmEMRMed = new frmIVFEMRMedication();
                    frmEMRMed = (frmIVFEMRMedication)mydata;
                    frmEMRMed.IsArt = NewObj.IsArt;
                    frmEMRMed.PlanTherapyId = NewObj.PlanTherapyId;
                    frmEMRMed.PlanTherapyUnitId = NewObj.PlanTherapyUnitId;
                    frmEMRMed.CurrentVisit = CurrentVisit;
                    frmEMRMed.IsEnableControl = CurrentVisit.VisitStatus;
                    //frmProcedure.SelectedPatient = new DataDrivenApplication.Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                    PrescriptionContent.Content = frmEMRMed;
                }
            }

            if (TIInvestigation != null)
            {
                if (TIInvestigation.IsSelected)
                {
                    frmIVFEMRInvestigation frmEMRInv;
                    UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("PalashDynamics.IVF.DashBoard.frmIVFEMRInvestigation") as UIElement;
                    frmEMRInv = new frmIVFEMRInvestigation();
                    frmEMRInv = (frmIVFEMRInvestigation)mydata;
                    //frmEMRInv.IsArt = NewObj.IsArt;
                    //frmEMRInv.PlanTherapyId = NewObj.PlanTherapyId;
                    //frmEMRInv.PlanTherapyUnitId = NewObj.PlanTherapyUnitId;
                    frmEMRInv.CurrentVisit = CurrentVisit;
                    frmEMRInv.IsEnableControl = CurrentVisit.VisitStatus;
                    //frmProcedure.SelectedPatient = new DataDrivenApplication.Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                    InvestigationContent.Content = frmEMRInv;
                }
            }


        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnSaveButton_Click != null)
            {
                OnSaveButton_Click(this, new RoutedEventArgs());
                this.Close();
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
            {
                OnSaveButton_Click(this, new RoutedEventArgs());
                this.Close();
            }
        }

      

        //public string ModuleName { get; set; }
        //public string Action { get; set; }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    ModuleName = "EMR";
        //    Action = "EMR.frmEMRProcedure";
        //    UserControl rootPage = Application.Current.RootVisual as UserControl;
        //    WebClient c = new WebClient();
        //    this.CurrentVisit = CurrentVisit;
        //    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
        //    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

        //    //frmEMRProcedure frmProcedure;
        //    //UIElement mydata5 = Assembly.GetExecutingAssembly().CreateInstance("EMR.frmEMRProcedure") as UIElement;
        //    //frmProcedure = new frmEMRProcedure();
        //    //frmProcedure = (frmEMRProcedure)mydata5;
        //    //frmProcedure.IsArt = NewObj.IsArt;
        //    //frmProcedure.PlanTherapyId = NewObj.PlanTherapyId;
        //    //frmProcedure.PlanTherapyUnitId = NewObj.PlanTherapyUnitId;
        //    //frmProcedure.CurrentVisit = CurrentVisit;
        //    //frmProcedure.IsEnabledControl = CurrentVisit.VisitStatus;
        //    //frmProcedure.SelectedPatient = new EMR.Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
        //    //ProcedureContent.Content = frmProcedure;
        //}

        //void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
        //{
        //    try
        //    {
        //        UIElement myData = null;
        //        string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
        //        XElement deploy = XDocument.Parse(appManifest).Root;
        //        List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
        //                                where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
        //                                select assemblyParts).ToList();
        //        Assembly asm = null;
        //        AssemblyPart asmPart = new AssemblyPart();
        //        StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
        //        asm = asmPart.Load(streamInfo.Stream);
        //        myData = asm.CreateInstance(Action) as UIElement;
        //        //((IApplicationConfiguration)App.Current).OpenMainContent(myData);


        //        //frmProcedure.IsArt = NewObj.IsArt;
        //        //frmProcedure.PlanTherapyId = NewObj.PlanTherapyId;
        //        //frmProcedure.PlanTherapyUnitId = NewObj.PlanTherapyUnitId;
        //        //frmProcedure.CurrentVisit = CurrentVisit;
        //        //frmProcedure.IsEnabledControl = CurrentVisit.VisitStatus;
        //        //frmProcedure.SelectedPatient = new EMR.Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };

        //        NewObj.CurrentVisit = CurrentVisit;
        //        ((IInitiateCIMSIVF)myData).Initiate(NewObj);

        //        ProcedureContent.Content = myData;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}

