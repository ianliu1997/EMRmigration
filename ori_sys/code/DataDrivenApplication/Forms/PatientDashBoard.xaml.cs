using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using DataDrivenApplication;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Printing;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.EMR;

namespace CIMS.Forms
{
    public partial class PatientDashBoard : UserControl, IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        //PalashDynamics.MainPage mp = (PalashDynamics.MainPage)Application.Current.RootVisual;

                        //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        //PalashDynamics.App.MainPage = mp;

                        //((HyperlinkButton)mp.FindName("ConfigureSeprator")).Visibility = Visibility.Collapsed;
                        //((TextBlock)mp.FindName("SampleHeader")).Text = "Patients";
                        //((TextBlock)mp.FindName("SampleSubHeader")).Text = "";
                        //((ToggleButton)mp.FindName("cmdShortcutHeader")).Content = "Find Patient";

                        //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
                        //((ContentControl)mp.FindName("MainRegion")).Content = mydata;

                        //PalashDynamics.App.Configuration = new ConfigureDashboard();
                        //mp.RequestXML("Find Patient");
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
                default:
                    if (((IApplicationConfiguration)App.Current).SelectedPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedPatient.PatientID == 0)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        //PalashDynamics.MainPage mp = (PalashDynamics.MainPage)Application.Current.RootVisual;

                        //((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                        //PalashDynamics.App.MainPage = mp;

                        //((HyperlinkButton)mp.FindName("ConfigureSeprator")).Visibility = Visibility.Collapsed;
                        //((TextBlock)mp.FindName("SampleHeader")).Text = "Patients";
                        //((TextBlock)mp.FindName("SampleSubHeader")).Text = "";
                        //((ToggleButton)mp.FindName("cmdShortcutHeader")).Content = "Find Patient";

                        //UIElement mydata = Assembly.GetExecutingAssembly().CreateInstance("CIMS.Forms.PatientList") as UIElement;
                        //((ContentControl)mp.FindName("MainRegion")).Content = mydata;

                        //PalashDynamics.App.Configuration = new ConfigureDashboard();
                        //mp.RequestXML("Find Patient");
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    try
                    {
                        TemplateID = Convert.ToInt64(Mode);
                    }
                    catch (Exception ex)
                    {

                    }
                    break;
            }
        }

        #endregion

        bool IsPatientExist = false;
        bool IsPageLoded = false;
        bool IsFirst = true;
        WaitIndicator Indicatior = null;
        long TemplateID =0;
        
        public PatientDashBoard()
        {
            InitializeComponent();
        }
                
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                //((IApplicationConfiguration)App.Current).FillMenu("Find Patient");
            }
            else if (IsPageLoded == false)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
               // PART_MaximizeToggle.IsChecked = true;
            }

            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                LoadPatientHeader();
                LoadSummary();

                if (TemplateID != 0)
                {
                    PatientEMR.Content = new TemplateAssignment(TemplateID);
                }
                else
                {
                    LoadEMR();
                    PatientEMR.Content = new TemplateAssignment();
                }

                
                #region Commented Code 
                //try
                //{
                    //******** Commented By HArish-- Date-25July2011 -- For seperating functionality in different methods*******
                    //  ************* Code Copied in LoadSummary() Method ***************
                    //Indicatior = new WaitIndicator();

                    //clsGetPatientEMRVisitListBizActionVO BizAction = new clsGetPatientEMRVisitListBizActionVO();
                    //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    //BizAction.UnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId;
                    //dgComplaintSummary.Columns[3].Visibility = Visibility.Collapsed;

                    //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    //{
                    //    BizAction.UnitID = 0;
                    //    dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
                    //}

                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    //client.ProcessCompleted += (s, arg) =>
                    //{
                    //    Indicatior.Show();
                    //    if (arg.Error == null)
                    //    {
                    //        if (arg.Result != null)
                    //        {
                    //            dgComplaintSummary.ItemsSource = ((clsGetPatientEMRVisitListBizActionVO)arg.Result).VisitList;

                    //        }
                    //    }
                    //    Indicatior.Close();
                    //};
                    //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    //client.CloseAsync();

                    //clsGetVisitListBizActionVO BizAction = new clsGetVisitListBizActionVO();
                    //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    //BizAction.CheckPCR = true;

                    //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    //client.ProcessCompleted += (s, arg) =>
                    //{
                    //    Indicatior.Show();
                    //    if (arg.Error == null)
                    //    {
                    //        if (arg.Result != null)
                    //        {
                    //            dgComplaintSummary.ItemsSource = ((clsGetVisitListBizActionVO)arg.Result).VisitList;

                    //        }
                    //    }
                    //    Indicatior.Close();
                    //};
                    //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);


                    //******** Commented By HArish-- Date-25July2011 -- For seperating functionality in different methods*******
                    //  ************* Code Copied in LoadEMR() Method ***************
                    //#region Get Active Visit
                    //clsGetVisitBizActionVO BizAction1 = new clsGetVisitBizActionVO();
                    //BizAction1.Details = new clsVisitVO();
                    //BizAction1.GetLatestVisit = true;
                    //BizAction1.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    //BizAction1.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    //BizAction1.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                    //if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    //{
                    //    BizAction1.ForHO = true;
                    //    BizAction1.Details.UnitId = 0L;
                    //}
                    //Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    //PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                    //client1.ProcessCompleted += (s, arg) =>
                    //{
                    //    if (arg.Error == null)
                    //    {
                    //        if (arg.Result != null)
                    //        {
                    //            BizAction1 = (clsGetVisitBizActionVO)arg.Result;

                    //            if (PatientEMR.Content != null && PatientEMR.Content is TemplateAssignment)
                    //            {
                    //                ((TemplateAssignment)PatientEMR.Content).TxtVisitComplaints.Text = BizAction1.Details.Complaints;
                    //            }
                    //        }
                    //    }
                    //};
                    //client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);

                    //#endregion
                    //PatientEMR.Content = new TemplateAssignment();

                    // Commented to remove form designer from Patient EMR section
                    //TemplateList.Content = new TemplateList();
                //}
                //catch (Exception ex)
                //{
                //    Indicatior.Close();
                //}
                #endregion
            }
            IsPageLoded = true;
        }

        private void LoadEMR()
        {
            try
            {
                Indicatior = new WaitIndicator();

                #region Get Active Visit
                clsGetVisitBizActionVO BizAction1 = new clsGetVisitBizActionVO();
                BizAction1.Details = new clsVisitVO();
                BizAction1.GetLatestVisit = true;
                BizAction1.Details.PatientId = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction1.Details.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction1.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction1.ForHO = true;
                    BizAction1.Details.UnitId = 0L;
                }
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            BizAction1 = (clsGetVisitBizActionVO)arg.Result;

                            if (PatientEMR.Content != null && PatientEMR.Content is TemplateAssignment)
                            {
                                ((TemplateAssignment)PatientEMR.Content).TxtVisitComplaints.Text = BizAction1.Details.Complaints;
                            }
                        }
                    }
                };
                client1.ProcessAsync(BizAction1, ((IApplicationConfiguration)App.Current).CurrentUser);

                #endregion
               

            }
            catch (Exception ex)
            {
                Indicatior.Close();
            }
        }

        private void LoadSummary()
        {
            try
            {
                Indicatior = new WaitIndicator();

                clsGetPatientEMRVisitListBizActionVO BizAction = new clsGetPatientEMRVisitListBizActionVO();
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.UnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId;
                dgComplaintSummary.Columns[3].Visibility = Visibility.Collapsed;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                    dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Show();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            dgComplaintSummary.ItemsSource = ((clsGetPatientEMRVisitListBizActionVO)arg.Result).VisitList;

                        }
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                Indicatior.Close();
            }
        }

        private void LoadPatientHeader()
        {            
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;

                    if (BizAction.PatientDetails.GenderID == 1 )
                    {
                        Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                        if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        {
                            Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                            CoupleInfo.Visibility = Visibility.Visible;
                            PatientInfo.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            PatientInfo.Visibility = Visibility.Visible;
                            CoupleInfo.Visibility = Visibility.Collapsed;
                            Patient.DataContext = BizAction.PatientDetails.GeneralDetails;                            
                        }
                    }
                    else
                    {
                        Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                        if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                        {
                            Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                            CoupleInfo.Visibility = Visibility.Visible;
                            PatientInfo.Visibility = Visibility.Collapsed;
                        }
                        else
                        {                            
                            Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                            PatientInfo.Visibility = Visibility.Visible;
                            CoupleInfo.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void ActiveProblem_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Problem:Active Problem");

        }

        private void PastHistory_Maximized(object sender, EventArgs e)
        {

            //  App.MainPage.RequestXML("History:Past History");

        }

        private void FamilyHistory_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("History:Family History");

        }

        private void SocialHistory_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("History:Social History");

        }

        private void OccupationalHistory_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("History:Occupational History");

        }

        private void BirthHistory_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("History:Birth History");

        }

        private void ImmunizationHistory_Maximized(object sender, EventArgs e)
        {
            //  App.MainPage.RequestXML("History:Immunization History");

        }

        private void Vital_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Clinical Details:Vitals Details");

        }

        private void ExaminationFinding_Maximized(object sender, EventArgs e)
        {
            //  App.MainPage.RequestXML("Clinical Details:Examination/Finding Details");

        }

        private void Diagnosis_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Clinical Details:Diagnosis Details");

        }

        private void Allergies_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Clinical Details:Allergies Details");

        }

        private void ServicesProcedures_Maximized(object sender, EventArgs e)
        {
            //  App.MainPage.RequestXML("Order:Services/Procedures");

        }

        private void LabTest_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Order:Lab Test");

        }

        private void RadiologyTest_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Order:Radiology Test");

        }

        private void ActiveMedication_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Medication:Active Medication");

        }

        private void Documents_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Others:Documents");

        }

        private void ImageManagement_Maximized(object sender, EventArgs e)
        {
            // App.MainPage.RequestXML("Others:Image Management");

        }

        private void PatPersonalInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //App.MainPage.RequestXML("");
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0 && PatPersonalInfo != null && ((TabItem)PatPersonalInfo.SelectedItem).Name == "tabPatGeneralInfo" && !IsFirst)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();

                clsGetPatientEMRVisitListBizActionVO BizAction = new clsGetPatientEMRVisitListBizActionVO();
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.UnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId;

                dgComplaintSummary.Columns[3].Visibility = Visibility.Collapsed;

                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                {
                    BizAction.UnitID = 0;
                    dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Show();
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            dgComplaintSummary.ItemsSource = ((clsGetPatientEMRVisitListBizActionVO)arg.Result).VisitList;

                        }
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

                //clsGetVisitListBizActionVO BizAction = new clsGetVisitListBizActionVO();
                //BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                //BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                //BizAction.CheckPCR = true;

                //Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                //PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                //client.ProcessCompleted += (s, arg) =>
                //{
                //    if (arg.Error == null)
                //    {
                //        if (arg.Result != null)
                //        {
                //            dgComplaintSummary.ItemsSource = ((clsGetVisitListBizActionVO)arg.Result).VisitList;

                //        }
                //    }
                //    Indicatior.Close();
                //};
                //client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
            IsFirst = false;
        }
        
        private void DragDockPanel_Maximized(object sender, EventArgs e)
        {

        }

        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {

        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Old PCR that works with XAML file
            #region old PCR
            //PreviewDrag.Visibility = Visibility.Visible;
            //PreviewName.Text = "Patient Case Record";

            //#region Apply Preview
            //PatientCaseRecordSetting PCRS=((clsVisitVO)dgComplaintSummary.SelectedItem).PatientCaseRecord.XmlDeserialize<PatientCaseRecordSetting>();

            //PatientCaseRecord PCR = new PatientCaseRecord();
            //PCR.SetParticulars(PCRS.Name,PCRS.Age,PCRS.Gender,PCRS.Add,PCRS.Occupation,PCRS.Phone,PCRS.Date,PCRS.ClinicRefNo);
            //PCR.SetIllnessSummary(PCRS.ComplaintReported,PCRS.ChiefComplaint,PCRS.PastHistory,PCRS.DrugHistory,PCRS.Allergies);
            //PCR.SetObservation(PCRS.Investigations,PCRS.ProvisionalDiagnosis,PCRS.FinalDiagnosis);
            //PCR.SetEducation("", "", "Reason :" + PCRS.SpecificInstructions);
            //SetTherapyForPCR(PCRS,PCR);
            //PCR.SetOthers(PCRS.FollowUpDate,PCRS.FollowUpAt,PCRS.ReferralTo);
            //PCRPreview.Content = PCR;
            //temp.Visibility = Visibility.Collapsed;
            //scvPCR.Visibility = Visibility.Visible;
            //#endregion
            #endregion

            // New PCR that works with Crystal Report

            if (((clsVisitVO)dgComplaintSummary.SelectedItem) != null)
            {
                clsVisitVO visit = ((clsVisitVO)dgComplaintSummary.SelectedItem);

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCaseRecord.aspx?Type=1&UnitID=" + visit.UnitId + "&VisitID=" + visit.ID + "&PatientID=" + visit.PatientId + "&PatientUnitID=" + visit.PatientUnitId + "&TemplateID=" + visit.TemplateID), "_blank");
            }

        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            //PreviewDrag.Visibility = Visibility.Collapsed;
            //PreviewName.Text = "Case Details";
            //scvPCR.Visibility = Visibility.Collapsed;
            //temp.Visibility = Visibility.Visible;
        }

        private void SetTherapyForPCR(PatientCaseRecordSetting PCRS, PatientCaseRecord PCR)
        {
            Grid Medication = new Grid();
            Medication.Margin = new Thickness(2, 2, 2, 2);
            Medication.HorizontalAlignment = HorizontalAlignment.Stretch;
            int j = 0;

            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(200, GridUnitType.Auto);
            Medication.ColumnDefinitions.Add(col);

            ColumnDefinition col1 = new ColumnDefinition();
            Medication.ColumnDefinitions.Add(col1);

            while (j < 4)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength();
                Medication.RowDefinitions.Add(row);
                j++;
            }


            TextBlock tb1 = new TextBlock();
            tb1.Text = "Hydration : ";
            Grid.SetRow(tb1, 0);
            Medication.Children.Add(tb1);

            TextBox txtHydration = new TextBox();
            txtHydration.IsReadOnly = true;
            txtHydration.Text = "";
            txtHydration.Text = PCRS.HydrationStatusManagement;

            Grid.SetRow(txtHydration, 0);
            Grid.SetColumn(txtHydration, 1);
            Medication.Children.Add(txtHydration);

            TextBlock tb2 = new TextBlock();
            tb2.Text = "Hydration4 : ";
            Grid.SetRow(tb2, 1);
            Medication.Children.Add(tb2);

            TextBox txtHydration4 = new TextBox();
            txtHydration4.IsReadOnly = true;
            txtHydration4.Text = "";
            txtHydration4.Text = PCRS.Hydration4StatusManagement;

            Grid.SetRow(txtHydration4, 1);
            Grid.SetColumn(txtHydration4, 1);
            Medication.Children.Add(txtHydration4);


            TextBlock tb3 = new TextBlock();
            tb3.Text = "Zinc Supplement : ";
            Grid.SetRow(tb3, 2);
            Medication.Children.Add(tb3);

            TextBox txtZinc = new TextBox();
            txtZinc.IsReadOnly = true;
            txtZinc.Text = "";
            txtZinc.Text = PCRS.ZincSupplementManagement;

            Grid.SetRow(txtZinc, 2);
            Grid.SetColumn(txtZinc, 1);
            Medication.Children.Add(txtZinc);

            TextBlock tb4 = new TextBlock();
            tb4.Text = "Nutritions : ";
            Grid.SetRow(tb4, 3);
            Medication.Children.Add(tb4);

            TextBox txtNutrition = new TextBox();
            txtNutrition.AcceptsReturn = true;
            txtNutrition.TextWrapping = TextWrapping.Wrap;
            txtNutrition.IsReadOnly = true;
            txtNutrition.Text = "";
            txtNutrition.Text = PCRS.NutritionAdvise;

            Grid.SetRow(txtNutrition, 3);
            Grid.SetColumn(txtNutrition, 1);
            Medication.Children.Add(txtNutrition);




            Grid Therapy = new Grid();
            Therapy.HorizontalAlignment = HorizontalAlignment.Stretch;
            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength();
            Therapy.RowDefinitions.Add(row1);
            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength();
            Therapy.RowDefinitions.Add(row2);

            Grid Drugs = new Grid();
            Drugs.Name = "FollowUpMedication";
            Drugs = GetGridSchema(Drugs, PCRS);

            Grid.SetRow(Medication, 0);
            Therapy.Children.Add(Medication);
            Grid.SetRow(Drugs, 1);
            Therapy.Children.Add(Drugs);

            PCR.SetTherapy(Therapy);

        }

        private Grid GetGridSchema(Grid MainGrid, PatientCaseRecordSetting PCRS)
        {
            //Grid MainGrid=new Grid();
            MainGrid.Margin = new Thickness(2, 2, 2, 2);
            MainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            int i = 0;

            ColumnDefinition col = new ColumnDefinition();
            col.Width = new GridLength(200, GridUnitType.Auto);
            MainGrid.ColumnDefinitions.Add(col);

            ColumnDefinition col1 = new ColumnDefinition();
            MainGrid.ColumnDefinitions.Add(col1);

            while (i < 4)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength();
                MainGrid.RowDefinitions.Add(row);
                i++;
            }

            TextBlock tb1 = new TextBlock();
            //tb1.Text = "Antiemetics : ";
            tb1.Text = "Medication : ";
            Grid.SetRow(tb1, 0);
            MainGrid.Children.Add(tb1);

            TextBlock tb2 = new TextBlock();
            //Added at razi
            tb2.Visibility = Visibility.Collapsed;
            tb2.Text = "Antibiotics : ";
            Grid.SetRow(tb2, 1);
            MainGrid.Children.Add(tb2);

            TextBlock tb3 = new TextBlock();
            //Added at razi
            tb3.Visibility = Visibility.Collapsed;
            tb3.Text = "Antipyretic : ";
            Grid.SetRow(tb3, 2);
            MainGrid.Children.Add(tb3);

            TextBlock tb4 = new TextBlock();
            //Added at razi
            tb4.Visibility = Visibility.Collapsed;
            tb4.Text = "Antispasmodic : ";
            Grid.SetRow(tb4, 3);
            MainGrid.Children.Add(tb4);

            int k = 0;
            Grid g1 = GetGridHeading();
            while (k < PCRS.ItemsSource1.Count)
            {
                string s1, s2, s3, s4;
                int s5, s6;

                if (((Medication)PCRS.ItemsSource1[k]).Drug != null)
                    s1 = ((Medication)PCRS.ItemsSource1[k]).Drug.Description;
                else
                    s1 = "";

                s2 = ((Medication)PCRS.ItemsSource1[k]).Dose;

                if (((Medication)PCRS.ItemsSource1[k]).Route != null)
                    s3 = ((Medication)PCRS.ItemsSource1[k]).Route.Description;
                else
                    s3 = "";

                s4 = ((Medication)PCRS.ItemsSource1[k]).Frequency.ToString();
                s5 = ((Medication)PCRS.ItemsSource1[k]).Day;
                s6 = ((Medication)PCRS.ItemsSource1[k]).Quantity;
                g1 = AddGridItem(g1, s1, s2, s3, s4, s5, s6);
                k++;
            }


            Grid g2 = GetGridHeading();
            //Added at razi
            g2.Visibility = Visibility.Collapsed;
            k = 0;
            while (k < PCRS.ItemsSource2.Count)
            {
                string s1, s2, s3, s4;
                int s5, s6;
                if (((Medication)PCRS.ItemsSource2[k]).Drug != null)
                    s1 = ((Medication)PCRS.ItemsSource2[k]).Drug.Description;
                else
                    s1 = "";

                s2 = ((Medication)PCRS.ItemsSource2[k]).Dose;

                if (((Medication)PCRS.ItemsSource2[k]).Route != null)
                    s3 = ((Medication)PCRS.ItemsSource2[k]).Route.Description;
                else
                    s3 = "";

                s4 = ((Medication)PCRS.ItemsSource2[k]).Frequency.ToString();
                s5 = ((Medication)PCRS.ItemsSource2[k]).Day;
                s6 = ((Medication)PCRS.ItemsSource2[k]).Quantity;
                g2 = AddGridItem(g2, s1, s2, s3, s4, s5, s6);
                k++;
            }


            Grid g3 = GetGridHeading();
            //Added at razi
            g3.Visibility = Visibility.Collapsed;
            k = 0;
            while (k < PCRS.ItemsSource3.Count)
            {
                string s1, s2, s3, s4;
                int s5, s6;
                if (((Medication)PCRS.ItemsSource3[k]).Drug != null)
                    s1 = ((Medication)PCRS.ItemsSource3[k]).Drug.Description;
                else
                    s1 = "";

                s2 = ((Medication)PCRS.ItemsSource3[k]).Dose;

                if (((Medication)PCRS.ItemsSource3[k]).Route != null)
                    s3 = ((Medication)PCRS.ItemsSource3[k]).Route.Description;
                else
                    s3 = "";

                s4 = ((Medication)PCRS.ItemsSource3[k]).Frequency.ToString();
                s5 = ((Medication)PCRS.ItemsSource3[k]).Day;
                s6 = ((Medication)PCRS.ItemsSource3[k]).Quantity;
                g3 = AddGridItem(g3, s1, s2, s3, s4, s5, s6);
                k++;
            }


            Grid g4 = GetGridHeading();
            //Added at razi
            g4.Visibility = Visibility.Collapsed;
            k = 0;
            while (k < PCRS.ItemsSource4.Count)
            {
                string s1, s2, s3, s4;
                int s5, s6;
                if (((Medication)PCRS.ItemsSource4[k]).Drug != null)
                    s1 = ((Medication)PCRS.ItemsSource4[k]).Drug.Description;
                else
                    s1 = "";

                s2 = ((Medication)PCRS.ItemsSource4[k]).Dose;

                if (((Medication)PCRS.ItemsSource4[k]).Route != null)
                    s3 = ((Medication)PCRS.ItemsSource4[k]).Route.Description;
                else
                    s3 = "";

                s4 = ((Medication)PCRS.ItemsSource4[k]).Frequency.ToString();
                s5 = ((Medication)PCRS.ItemsSource4[k]).Day;
                s6 = ((Medication)PCRS.ItemsSource4[k]).Quantity;
                g4 = AddGridItem(g4, s1, s2, s3, s4, s5, s6);
                k++;
            }

            Grid.SetRow(g1, 0);
            Grid.SetColumn(g1, 1);
            MainGrid.Children.Add(g1);

            Grid.SetRow(g2, 1);
            Grid.SetColumn(g2, 1);
            MainGrid.Children.Add(g2);

            Grid.SetRow(g3, 2);
            Grid.SetColumn(g3, 1);
            MainGrid.Children.Add(g3);

            Grid.SetRow(g4, 3);
            Grid.SetColumn(g4, 1);
            MainGrid.Children.Add(g4);

            return MainGrid;
        }

        private Grid GetGridHeading()
        {
            Grid grdAnti = new Grid();
            grdAnti.Margin = new Thickness(2, 2, 2, 2);
            RowDefinition r = new RowDefinition();
            r.Height = new GridLength(25, GridUnitType.Auto);
            grdAnti.RowDefinitions.Add(r);

            ColumnDefinition c = new ColumnDefinition();
            //c.Width = new GridLength();
            grdAnti.ColumnDefinitions.Add(c);

            //Used with Dose
            //ColumnDefinition c1 = new ColumnDefinition();
            //grdAnti.ColumnDefinitions.Add(c1);

            int i = 0;
            while (i < 4)
            {
                ColumnDefinition c2 = new ColumnDefinition();
                c2.Width = new GridLength();
                grdAnti.ColumnDefinitions.Add(c2);
                i++;
            }


            LinearGradientBrush a = new LinearGradientBrush();
            GradientStop gs1 = new GradientStop();
            gs1.Offset = 0;
            gs1.Color = Color.FromArgb(20, 0, 0, 155);
            a.GradientStops.Add(gs1);

            TextBox txtName = new TextBox();
            txtName.Background = a;
            txtName.IsReadOnly = true;
            txtName.Text = "Drug Name";
            //txtName.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtName, 0);
            Grid.SetColumn(txtName, 0);
            grdAnti.Children.Add(txtName);

            //used with Dose
            //TextBox txtdose = new TextBox();
            //txtdose.Background = a;
            //txtdose.IsReadOnly = true;
            //txtdose.Text = "Drug dose";
            ////txtdose.HorizontalAlignment = HorizontalAlignment.Stretch;
            //Grid.SetRow(txtdose, 0);
            //Grid.SetColumn(txtdose, 1);
            //grdAnti.Children.Add(txtdose);

            TextBox txtRoute = new TextBox();
            txtRoute.Background = a;
            txtRoute.IsReadOnly = true;
            txtRoute.Text = "Route";
            //txtRoute.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtRoute, 0);
            Grid.SetColumn(txtRoute, 1);
            grdAnti.Children.Add(txtRoute);

            TextBox txtFrequency = new TextBox();
            txtFrequency.Background = a;
            txtFrequency.IsReadOnly = true;
            txtFrequency.Text = "Frequency";
            //txtFrequency.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtFrequency, 0);
            Grid.SetColumn(txtFrequency, 2);
            grdAnti.Children.Add(txtFrequency);

            TextBox txtDays = new TextBox();
            txtDays.Background = a;
            txtDays.IsReadOnly = true;
            txtDays.Text = "Days";
            //txtFrequency.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtDays, 0);
            Grid.SetColumn(txtDays, 3);
            grdAnti.Children.Add(txtDays);

            TextBox txtQty = new TextBox();
            txtQty.Background = a;
            txtQty.IsReadOnly = true;
            txtQty.Text = "Quantity";
            //txtFrequency.HorizontalAlignment = HorizontalAlignment.Stretch;
            Grid.SetRow(txtQty, 0);
            Grid.SetColumn(txtQty, 4);
            grdAnti.Children.Add(txtQty);

            return grdAnti;
        }

        private Grid AddGridItem(Grid g, string s1, string s2, string s3, string s4, int s5, int s6)
        {
            RowDefinition r = new RowDefinition();
            r.Height = new GridLength(25, GridUnitType.Auto);
            g.RowDefinitions.Add(r);

            TextBox txtName = new TextBox();
            txtName.IsReadOnly = true;
            txtName.Text = s1 == null ? "" : s1;
            Grid.SetRow(txtName, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtName, 0);
            g.Children.Add(txtName);

            //used with Dose
            //TextBox txtdose = new TextBox();
            //txtdose.IsReadOnly = true;
            //txtdose.Text = s2 == null ? "" : s2;
            //Grid.SetRow(txtdose, g.RowDefinitions.Count - 1);
            //Grid.SetColumn(txtdose, 1);
            //g.Children.Add(txtdose);

            TextBox txtRoute = new TextBox();
            txtRoute.IsReadOnly = true;
            txtRoute.Text = s3 == null ? "" : s3;
            Grid.SetRow(txtRoute, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtRoute, 1);
            g.Children.Add(txtRoute);

            TextBox txtFrequency = new TextBox();
            txtFrequency.IsReadOnly = true;
            txtFrequency.Text = s4 == null ? "" : s4;
            Grid.SetRow(txtFrequency, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtFrequency, 2);
            g.Children.Add(txtFrequency);

            TextBox txtDays = new TextBox();
            txtDays.IsReadOnly = true;
            txtDays.Text = s5.ToString();
            Grid.SetRow(txtDays, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtDays, 3);
            g.Children.Add(txtDays);

            TextBox txtQty = new TextBox();
            txtQty.IsReadOnly = true;
            txtQty.Text = s6.ToString();
            Grid.SetRow(txtQty, g.RowDefinitions.Count - 1);
            Grid.SetColumn(txtQty, 4);
            g.Children.Add(txtQty);

            return g;
        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDocument document = new PrintDocument();
            //document.BeginPrint += new EventHandler<BeginPrintEventArgs>(document_BeginPrint);
            //document.EndPrint += new EventHandler<EndPrintEventArgs>(document_EndPrint);
            document.PrintPage += new EventHandler<PrintPageEventArgs>(document_PrintPage);
            document.Print("Template");
        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //if (PCRPreview.Content.GetType().ToString() == "DataDrivenApplication.PatientCaseRecord")
            //{
            //    e.PageVisual = (PatientCaseRecord)PCRPreview.Content;
            //}
            //else if (PCRPreview.Content.GetType().ToString() == "DataDrivenApplication.CaseReferralSheet")
            //{
            //    e.PageVisual = (CaseReferralSheet)PCRPreview.Content;
            //}
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            // Old Case Referral that works with XAML file
            #region Old Case Referral
            //PreviewDrag.Visibility = Visibility.Visible;
            //PreviewName.Text = "Case Referral Sheet";

            //#region Apply Preview
            //PatientCaseReferralSetting PCReferral = ((clsVisitVO)dgComplaintSummary.SelectedItem).CaseReferralSheet.XmlDeserialize<PatientCaseReferralSetting>();

            //CaseReferralSheet CRS = new CaseReferralSheet();
            //CRS.SetParticulars(PCReferral.Name, PCReferral.Age, PCReferral.Gender, PCReferral.Add, PCReferral.Occupation, PCReferral.Phone, PCReferral.Date, PCReferral.ClinicRefNo);            
            //CRS.SetReferralDetails(PCReferral.ReferredByDoctor,PCReferral.ReferredToDoctor,PCReferral.ClinicNo1,PCReferral.ClinicNo2,PCReferral.MobileNo1,PCReferral.MobileNo2);
            //CRS.SetProDiagChiefComplaint(PCReferral.ProDiag,PCReferral.ChiefComplaint);
            //CRS.SetPatientDetails(PCReferral.Summary);
            //CRS.SetRefRemarks(PCReferral.Remarks);            

            //PCRPreview.Content = CRS;
            //temp.Visibility = Visibility.Collapsed;
            //scvPCR.Visibility = Visibility.Visible;
            //#endregion
            #endregion

            // New Case Referral that works with Crystal Report

            if (((clsVisitVO)dgComplaintSummary.SelectedItem) != null)
            {
                clsVisitVO visit = ((clsVisitVO)dgComplaintSummary.SelectedItem);

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCaseRecord.aspx?Type=2&UnitID=" + visit.UnitId + "&VisitID=" + visit.ID + "&PatientID=" + visit.PatientId + "&PatientUnitID=" + visit.PatientUnitId + "&TemplateID=" + visit.TemplateID), "_blank");
            }
        }

    }
}
