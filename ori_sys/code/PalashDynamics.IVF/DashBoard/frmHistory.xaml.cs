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
using DataDrivenApplication;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using System.Windows.Browser;


namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmHistory : ChildWindow,IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();
            switch (Mode)
            {
                case "NEW":
                    try
                    {
                        TemplateID = Convert.ToInt64(Mode);
                    }
                    catch (Exception ex)
                    {
                    }
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
                        IsPatientExist = false;
                        break;
                    }                   

                    //IsPatientExist = true;
                    //UserControl rootPage = Application.Current.RootVisual as UserControl;
                    //TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    //mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
                default:
                    try
                    {
                        TemplateID = Convert.ToInt64(Mode);
                    }
                    catch (Exception ex)
                    {
                    }
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
                        
                        IsPatientExist = false;
                        break;
                    }

                    IsPatientExist = true;
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    //try
                    //{
                    //    TemplateID = Convert.ToInt64(Mode);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                    break;
            }
        }

        #endregion

        bool IsPatientExist = false;
        bool IsPageLoded = false;
        bool IsFirst = true;
        WaitIndicator Indicatior = null;
       public long TemplateID ;
        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }
        public clsVisitVO CurrentVisit { get; set; }
        long GlobalTemplateID = 0;
        bool IsCancel = true;
       // public FormDetail SelectedFormStructure { get; set; }
        public bool IsTemplateByDoctor = false;
        public string TemplateByNurse;
        public int ApplicableToCreiteria;
        public string ModuleName { get; set; }
        public string Action { get; set; }
        //Added by Saily P on 10.05.12
        public FormDetail SelectedFormStructure { get; set; }
        public SectionDetail SectionCheck;
        public Settings testSettings;
        public bool IsNewPage;

        public frmHistory()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
            {
                 ModuleName = "PalashDynamics";
                 Action = "CIMS.Forms.PatientList";
                 UserControl rootPage = Application.Current.RootVisual as UserControl;

                    WebClient c2 = new WebClient();
                    c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                    c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else if (IsPageLoded == false)
            {
                UserControl rootPage = Application.Current.RootVisual as UserControl;
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            }
            if (!IsPageLoded && ((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                IsNewPage = true;
                GetTemplateOnSummary();
            }
              IsPageLoded = true;
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
                if(TemplateID == 19 || TemplateID == 22 || TemplateID == 27 || TemplateID == 43)
                    ((IInitiateCIMS)myData).Initiate("VISIT");
                else
                    ((IInitiateCIMS)myData).Initiate("REG");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }   
      
        private void CheckTemplateWithGender(long TemplateID)
        {
            clsGetEMRTemplateBizActionVO bizAction = new clsGetEMRTemplateBizActionVO();
            bizAction.objEMRTemplate = new clsEMRTemplateVO();
            bizAction.objEMRTemplate.TemplateID = TemplateID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    if (((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate != null)
                    {
                        ApplicableToCreiteria = ((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria;
                        if ((((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria == (int)EMR_Template_Applicable_Criteria.Female) && ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Female.ToString())
                        {
                            IVFTemplate.Content = new TemplateAssignment(TemplateID);                            
                        }
                        else if (((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria == (int)EMR_Template_Applicable_Criteria.Male && ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == Genders.Male.ToString())
                        {
                            IVFTemplate.Content = new TemplateAssignment(TemplateID);
                        }
                        else if (((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria == (int)EMR_Template_Applicable_Criteria.Both)
                        {
                            IVFTemplate.Content = new TemplateAssignment(TemplateID);
                        }
                        else if(((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria != (int)EMR_Template_Applicable_Criteria.Male)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Female Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                            msgW5.Show();

                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();

                            ModuleName = "PalashDynamics";
                            Action = "CIMS.Forms.PatientList";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;

                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            //((IInitiateCIMS)App.Current).Initiate("REG");

                        }
                        else if (((clsGetEMRTemplateBizActionVO)e.Result).objEMRTemplate.ApplicableCriteria != (int)EMR_Template_Applicable_Criteria.Female)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select The Male Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW5.Show();
                            ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                            ModuleName = "PalashDynamics";
                            Action = "CIMS.Forms.PatientList";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        }
                    }
                }
            };
            Client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        //Added by Saily P on 25.09.12 to get the patient details in case of Donor
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
                    PatientInfo.Visibility = Visibility.Visible;
                    CoupleInfo.Visibility = Visibility.Collapsed;
                    Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    #region Commented by Saily P on 25.09.12
                    //if (BizAction.PatientDetails.GenderID == 1)
                    //{
                    //    Male.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Female.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    }
                    //}
                    //else
                    //{
                    //    Female.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //    if (BizAction.PatientDetails.SpouseDetails != null && BizAction.PatientDetails.SpouseDetails.ID != 0)
                    //    {
                    //        Male.DataContext = BizAction.PatientDetails.SpouseDetails;
                    //        CoupleInfo.Visibility = Visibility.Visible;
                    //        PatientInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //    else
                    //    {
                    //        Patient.DataContext = BizAction.PatientDetails.GeneralDetails;
                    //        PatientInfo.Visibility = Visibility.Visible;
                    //        CoupleInfo.Visibility = Visibility.Collapsed;
                    //    }
                    //}
                    #endregion
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {                      
                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        if (BizAction.CoupleDetails.MalePatient == null || BizAction.CoupleDetails.FemalePatient == null)
                        {
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                 new MessageBoxControl.MessageBoxChildWindow("Palash", "Only For Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();

                            ModuleName = "PalashDynamics";
                            Action = "CIMS.Forms.PatientList";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;

                            WebClient c2 = new WebClient();
                            c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                            c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                            //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            //((IInitiateCIMS)App.Current).Initiate("REG");
                        }
                        else
                        {
                            PatientInfo.Visibility = Visibility.Collapsed;
                            CoupleInfo.Visibility = Visibility.Visible;
                            GetHeightAndWeight(BizAction.CoupleDetails);

                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto13.Width, (int)imgPhoto13.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient.Photo);
                                imgPhoto13.Source = bmp;
                                imgPhoto13.Visibility = System.Windows.Visibility.Visible;
                            }
                          else
                            {
                                imgP1.Visibility = System.Windows.Visibility.Visible;
                            }
                            if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo != null)
                            {
                                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto12.Width, (int)imgPhoto12.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                                bmp.FromByteArray(((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient.Photo);
                                imgPhoto12.Source = bmp;
                                imgPhoto12.Visibility = System.Windows.Visibility.Visible;
                            }
                            else
                            {
                                imgP2.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                    else if (TemplateID == 41)
                    {
                        if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId>0)
                        {
                            PatientInfo.Visibility = Visibility.Collapsed;
                            CoupleInfo.Visibility = Visibility.Visible;
                            if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Male.ToString())
                            {
                                BizAction.CoupleDetails.MalePatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                                GetHeightAndWeight(BizAction.CoupleDetails);
                            }
                            else if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender == Genders.Female.ToString())
                            {
                                BizAction.CoupleDetails.FemalePatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
                                GetHeightAndWeight(BizAction.CoupleDetails);
                            }
                        }
                        else
                            LoadPatientHeader();
                    }
                    else
                    {
                        #region Commented by Saily P on 25.09.12 Purpose all the forms should work for couples and Donors.
                        ////MessageBoxControl.MessageBoxChildWindow msgW1 =
                        ////         new MessageBoxControl.MessageBoxChildWindow("Palash", "Only For Active Couple", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        ////msgW1.Show();

                        ////ModuleName = "PalashDynamics";
                        ////Action = "CIMS.Forms.PatientList";
                        ////UserControl rootPage = Application.Current.RootVisual as UserControl;

                        ////WebClient c2 = new WebClient();
                        ////c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                        ////c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        //((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                        //((IInitiateCIMS)App.Current).Initiate("REG");
                        #endregion
                        LoadPatientHeader();

                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetHeightAndWeight(clsCoupleVO CoupleDetails)
        {
            clsGetGetCoupleHeightAndWeightBizActionVO BizAction = new clsGetGetCoupleHeightAndWeightBizActionVO();
            BizAction.CoupleDetails = new clsCoupleVO();
            BizAction.FemalePatientID = CoupleDetails.FemalePatient.PatientID;
            BizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails != null)
                        BizAction.CoupleDetails = ((clsGetGetCoupleHeightAndWeightBizActionVO)args.Result).CoupleDetails;
                    if (BizAction.CoupleDetails != null)
                    {
                        clsPatientGeneralVO FemalePatientDetails = new clsPatientGeneralVO();
                        if (BizAction.CoupleDetails.FemalePatient != null)
                        {
                            FemalePatientDetails = CoupleDetails.FemalePatient;
                            FemalePatientDetails.Height = BizAction.CoupleDetails.FemalePatient.Height;
                            FemalePatientDetails.Weight = BizAction.CoupleDetails.FemalePatient.Weight;
                            FemalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.FemalePatient.BMI));
                            FemalePatientDetails.Alerts = BizAction.CoupleDetails.FemalePatient.Alerts;
                            Female.DataContext = FemalePatientDetails;
                        }
                        clsPatientGeneralVO MalePatientDetails = new clsPatientGeneralVO();
                        if (BizAction.CoupleDetails.MalePatient != null)
                        {
                            MalePatientDetails = CoupleDetails.MalePatient;
                            MalePatientDetails.Height = BizAction.CoupleDetails.MalePatient.Height;
                            MalePatientDetails.Weight = BizAction.CoupleDetails.MalePatient.Weight;
                            MalePatientDetails.BMI = Convert.ToDouble(String.Format("{0:0.00}", BizAction.CoupleDetails.MalePatient.BMI));
                            MalePatientDetails.Alerts = BizAction.CoupleDetails.MalePatient.Alerts;
                            Male.DataContext = MalePatientDetails;
                        }
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void GetTemplateOnSummary()
        {
            if (IsNewPage == true)
            {
                if (TemplateID != 0)
                {
                    if (TemplateID == 19)
                    {
                        if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "Male")
                            TemplateID = 22;
                    }
                    if (TemplateID == 27)
                    {
                        if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "Male")
                            TemplateID = 43;
                    }
                    // BY BHUSHAN For Surrogate History. . . . . 
                    if (TemplateID == 63)
                    {
                     //   if (((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).Gender == "Male")
                            TemplateID = 63;
                    }
                    CheckTemplateWithGender(TemplateID);
                    //IVFTemplate.Content = new TemplateAssignment(TemplateID);
                }
                else
                {
                    IVFTemplate.Content = new TemplateAssignment();
                } 
                fillCoupleDetails();               
                SetCommandButtonState("Load");
            }
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
            
            Indicatior = new WaitIndicator();
            Indicatior.Show();
            List<clsPatientEMRDataVO> testList = new List<clsPatientEMRDataVO>();
            clsGetPatientEMRSummaryDataBizActionVO BizActionObjPatientDataConsultation = new clsGetPatientEMRSummaryDataBizActionVO();
            BizActionObjPatientDataConsultation.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizActionObjPatientDataConsultation.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizActionObjPatientDataConsultation.TemplateID = TemplateID;
            BizActionObjPatientDataConsultation.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            BizActionObjPatientDataConsultation.IsIVF = true;
            this.SelectedFormStructure = null;
            PalashServiceClient clientObjPatientDataConsultation = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientObjPatientDataConsultation.ProcessCompleted += (sObjPatientDataConsultation, argsObjPatientDataConsultation) =>
            {
                if (argsObjPatientDataConsultation.Error==null && argsObjPatientDataConsultation.Result != null && ((clsGetPatientEMRSummaryDataBizActionVO)argsObjPatientDataConsultation.Result).SummaryList != null)
                {
                    //Indicatior.Show();
                    if (argsObjPatientDataConsultation.Error == null && argsObjPatientDataConsultation.Result != null)
                    {
                        dgComplaintSummary.ItemsSource = ((clsGetPatientEMRSummaryDataBizActionVO)argsObjPatientDataConsultation.Result).SummaryList;                    
                        testList = ((clsGetPatientEMRSummaryDataBizActionVO)argsObjPatientDataConsultation.Result).SummaryList;
                        testList = AssignToGrid(testList);
                        dgComplaintSummary.ItemsSource = testList;
                        dgComplaintSummary.UpdateLayout();
                        dgComplaintSummary.Focus();
                    }
                }
            };
            clientObjPatientDataConsultation.ProcessAsync(BizActionObjPatientDataConsultation, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientObjPatientDataConsultation.CloseAsync();
            Indicatior.Close();
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
                BizAction.PatientEMR = 1;
                BizAction.TemplateID = TemplateID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    Indicatior.Show();
                    if (arg.Error == null)
                    {
                        if(arg.Result != null)
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
                throw ex;
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Commneted By BHUSHAN . . . . . . .
           // if(TemplateID==23)
           //     HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Templates/Hysteroscopy.aspx?UnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&PatientId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateId=" + TemplateID + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
           // else if(TemplateID==24)
           //     HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Templates/Laproscopy.aspx?UnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&PatientId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateId=" + TemplateID + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
           // else if(TemplateID==37)
           //     HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Templates/SemenWash.aspx?UnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&PatientId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateId=" + TemplateID + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
           // else if (TemplateID == 38)
           //     HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Templates/SpermSurvival.aspx?UnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&PatientId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateId=" + TemplateID + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
           // //else if (TemplateID == 40)
           // //    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Templates/SemenExamination.aspx?UnitId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&PatientId=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitId="
           // //        + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateId=" + TemplateID + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
           //else
           // {
               // dtpT.Value.ToString("MM/dd/yyyy") 
                DateTime OnlyDateTime =Convert.ToDateTime(((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).AddedDateTime);
                DateTime OnlyDate = OnlyDateTime;  //OnlyDateTime.Date;   
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCaseRecord.aspx?Type=3&UnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&VisitID=1" + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateID=" + TemplateID + "&CurrentDate=" + OnlyDate.ToString("MM/dd/yyyy") + "&EMRId=" + ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID), "_blank");
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCaseRecord.aspx?Type=3&UnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&VisitID=1" + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateID=" + TemplateID + "&CurrentDate=" + ((clsVisitVO)dgComplaintSummary.SelectedItem).CurrentDate), "_blank");
           // }
        }

        private void PatPersonalInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0 && PatPersonalInfo != null && ((TabItem)PatPersonalInfo.SelectedItem).Name == "tabPatGeneralInfo" && !IsFirst)
                {
                    Indicatior = new WaitIndicator();
                    clsGetPatientEMRSummaryDataBizActionVO BizAction = new clsGetPatientEMRSummaryDataBizActionVO();
                    BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                    BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                    BizAction.UnitID = ((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserLoginInfo.UnitId;
                    BizAction.PatientEMR = 1;
                    BizAction.TemplateID = TemplateID;
                    tabEMRName.Visibility = Visibility.Collapsed;
                    if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                    {
                        BizAction.UnitID = 0;
                       // dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
                    }
                    List<clsPatientEMRDataVO> testList = new List<clsPatientEMRDataVO>();
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        Indicatior.Show();
                        if (arg.Error == null && arg.Result != null)
                        {                            
                                dgComplaintSummary.ItemsSource = ((clsGetPatientEMRSummaryDataBizActionVO)arg.Result).SummaryList;
                                testList = ((clsGetPatientEMRSummaryDataBizActionVO)arg.Result).SummaryList;
                                testList = AssignToGrid(testList);
                                dgComplaintSummary.ItemsSource = testList;
                                dgComplaintSummary.UpdateLayout();
                                dgComplaintSummary.Focus();                            
                        }
                        Indicatior.Close();
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                IsFirst = false;
            }
            catch (Exception ex)
            {
                throw ex;
             //   Indicatior.Close();
            }            
        }

        private void DragDockPanel_Maximized(object sender, EventArgs e)
        {
        }

        private void DragDockPanel_Minimized(object sender, EventArgs e)
        {
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            tabEMRName.Visibility = Visibility.Visible;
            PatPersonalInfo.SelectedIndex = 1;
            SetCommandButtonState("New");
            tabPatGeneralInfo.IsEnabled = false;
            IsNewPage = false;
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == true)
            {
                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                ModuleName = "PalashDynamics";
                Action = "CIMS.Forms.PatientList";
                UserControl rootPage = Application.Current.RootVisual as UserControl;

                WebClient c2 = new WebClient();
                c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
               
            }
            else
            {
                tabPatGeneralInfo.IsEnabled = true;
                PatPersonalInfo.SelectedIndex = 0;                
                tabEMRName.Visibility = Visibility.Collapsed;
                IsCancel = true;
                //LoadSummary();              
                IsNewPage = false;
                GetTemplateOnSummary();
            }
            SetCommandButtonState("Cancel");
        }

        #region Set Command Button State New/Save/Modify/Print

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;

                case "New":
                    cmdNew.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
               
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    break;

                case "View":
                    cmdNew.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;

                default:
                    break;
            }
        }

        #endregion

        private void hlbViewExamination_Click(object sender, RoutedEventArgs e)
        {
            tabEMRName.Visibility = Visibility.Visible;
            PatPersonalInfo.SelectedIndex = 1;
            SetCommandButtonState("View");
            tabPatGeneralInfo.IsEnabled = false;
            long selectedId = ((clsPatientEMRDataVO)dgComplaintSummary.SelectedItem).ID;
            bool isIVF = true;
            IVFTemplate.Content = new TemplateAssignment(selectedId, isIVF, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, TemplateID);
        }

        #region
        //private List<clsPatientEMRDataVO> AssignToGrid(List<clsPatientEMRDataVO> testList)
        //{
        //    string mTemplate = null;
        //    foreach (var item in testList)
        //    {
        //        if (item.TemplateByDoctor != null)
        //            mTemplate = item.TemplateByDoctor;
        //        else if (item.TemplateByNurse != null)
        //            mTemplate = item.TemplateByNurse;
        //        if (mTemplate != null)
        //        {
        //            SectionCheck = new SectionDetail();
        //            this.SelectedFormStructure = mTemplate.XmlDeserialize<FormDetail>();
        //            foreach (var item1 in SelectedFormStructure.SectionList)
        //            {
        //                SectionCheck.FieldList = null;
        //                SectionCheck.FieldList = item1.FieldList;
        //                foreach (FieldDetail item2 in SectionCheck.FieldList)
        //                {
        //                    if (item2.Name == "M_Medical" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Medical = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
        //                        dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "M_Surgical" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Surgical = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
        //                        dgComplaintSummary.Columns[4].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "FmLMP" && ((DataDrivenApplication.DateFieldSetting)item2.Settings).Date.Value != null)
        //                    {
        //                        item.LMPDate = ((DataDrivenApplication.DateFieldSetting)item2.Settings).Date.Value.ToString(); ;
        //                        dgComplaintSummary.Columns[5].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "FmMarriedSinceYrs" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.MarriedSince = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
        //                        dgComplaintSummary.Columns[6].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "FmPreviousIUI")
        //                    {
        //                        item.PreviousIUI = ((DataDrivenApplication.BooleanFieldSetting)item2.Settings).Value.ToString();
        //                        if (item.PreviousIUI == "False" || item.PreviousIUI == "false")
        //                            item.PreviousIUI = "No";
        //                        if (item.PreviousIUI == "True" || item.PreviousIUI == "true")
        //                            item.PreviousIUI = "Yes";
        //                        dgComplaintSummary.Columns[7].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "Fm_Uterus" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Uterus = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value.ToString();
        //                        dgComplaintSummary.Columns[8].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "Fm_Tubes" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Tubes = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
        //                        dgComplaintSummary.Columns[9].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "Fm_Ovaries" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Ovaries = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
        //                        dgComplaintSummary.Columns[10].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "Volume" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.Volume = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
        //                        dgComplaintSummary.Columns[11].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "TotalSpermCount" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
        //                    {
        //                        item.TotalSpermCount = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
        //                        dgComplaintSummary.Columns[12].Visibility = Visibility.Visible;
        //                    }
        //                    if (item2.Name == "Done By" && ((DataDrivenApplication.AutomatedListFieldSetting)item2.Settings).SelectedItem != null)
        //                    {
        //                        item.DoneBy = ((DataDrivenApplication.AutomatedListFieldSetting)item2.Settings).SelectedItem.ToString();
        //                        dgComplaintSummary.Columns[13].Visibility = Visibility.Visible;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return testList;
        //}

        //commented by neena
        private List<clsPatientEMRDataVO> AssignToGrid(List<clsPatientEMRDataVO> testList)
        {
            //string mTemplate = null;
            //foreach (var item in testList)
            //{
            //    if (item.TemplateByDoctor != null)
            //        mTemplate = item.TemplateByDoctor;
            //    else if (item.TemplateByNurse != null)
            //        mTemplate = item.TemplateByNurse;
            //    if (mTemplate != null)
            //    {
            //        SectionCheck = new SectionDetail();
            //        this.SelectedFormStructure = mTemplate.XmlDeserialize<FormDetail>();
            //        foreach (var item1 in SelectedFormStructure.SectionList)
            //        {
            //            SectionCheck.FieldList = null;
            //            SectionCheck.FieldList = item1.FieldList;
            //            foreach (FieldDetail item2 in SectionCheck.FieldList)
            //            {
            //                if (item2.Name == "M_Medical" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Medical = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
            //                    dgComplaintSummary.Columns[3].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "M_Surgical" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Surgical = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
            //                    dgComplaintSummary.Columns[4].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "FmLMP" && ((DataDrivenApplication.DateFieldSetting)item2.Settings).Date.Value != null)
            //                {
            //                    item.LMPDate = ((DataDrivenApplication.DateFieldSetting)item2.Settings).Date.Value.ToString(); ;
            //                    dgComplaintSummary.Columns[5].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "FmMarriedSinceYrs" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.MarriedSince = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
            //                    dgComplaintSummary.Columns[6].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "FmPreviousIUI")
            //                {
            //                    item.PreviousIUI = ((DataDrivenApplication.BooleanFieldSetting)item2.Settings).Value.ToString();
            //                    if (item.PreviousIUI == "False" || item.PreviousIUI == "false")
            //                        item.PreviousIUI = "No";
            //                    if (item.PreviousIUI == "True" || item.PreviousIUI == "true")
            //                        item.PreviousIUI = "Yes";
            //                    dgComplaintSummary.Columns[7].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "Fm_Uterus" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Uterus = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value.ToString();
            //                    dgComplaintSummary.Columns[8].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "Fm_Tubes" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Tubes = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
            //                    dgComplaintSummary.Columns[9].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "Fm_Ovaries" && ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Ovaries = ((DataDrivenApplication.TextFieldSetting)item2.Settings).Value;
            //                    dgComplaintSummary.Columns[10].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "Volume" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.Volume = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
            //                    dgComplaintSummary.Columns[11].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "TotalSpermCount" && ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value != null)
            //                {
            //                    item.TotalSpermCount = ((DataDrivenApplication.DecimalFieldSetting)item2.Settings).Value.ToString();
            //                    dgComplaintSummary.Columns[12].Visibility = Visibility.Visible;
            //                }
            //                if (item2.Name == "Done By" && ((DataDrivenApplication.AutomatedListFieldSetting)item2.Settings).SelectedItem != null)
            //                {
            //                    item.DoneBy = ((DataDrivenApplication.AutomatedListFieldSetting)item2.Settings).SelectedItem.ToString();
            //                    dgComplaintSummary.Columns[13].Visibility = Visibility.Visible;
            //                }
            //            }
            //        }
            //    }
            //}
            return testList;
        }
        //
        #endregion     

        private void HyperlinkButton_Click_Female(object sender, RoutedEventArgs e)
        {
            //frmAttention PatientAlert = new frmAttention();
            //PatientAlert.AlertsExpanded = CoupleDetails.FemalePatient.Alerts;
            //PatientAlert.Show();
        }

        private void HyperlinkButton_Click_Male(object sender, RoutedEventArgs e)
        {
            //frmAttention PatientAlert = new frmAttention();
            //PatientAlert.AlertsExpanded = CoupleDetails.MalePatient.Alerts;
            //PatientAlert.Show();
        }    

        #region Test
        //private void MapCaseReferral()
        //{
        //    objCaseReferral = null;
        //    objCaseReferral = new clsCaseReferralVO();
        //    if (SelectedFormStructure != null && SelectedFormStructure.SectionList != null && SelectedFormStructure.CaseReferralRelations != null && SelectedFormStructure.CaseReferralRelations.Count > 0)
        //    {
        //        foreach (var item in SelectedFormStructure.CaseReferralRelations)
        //        {
        //            MasterListItem source = null;
        //            FieldDetail target = null;
        //            source = item.SourceField;

        //            foreach (var section in SelectedFormStructure.SectionList)
        //            {
        //                if (section.UniqueId.ToString() == item.TargetSectionId)
        //                {
        //                    foreach (var field in section.FieldList)
        //                    {
        //                        if (field.UniqueId.ToString() == item.TargetFieldId)
        //                        {
        //                            target = field;
        //                            target.Parent = section;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                }
        //            }

        //            if (source != null && target != null)
        //            {
        //                item.TargetField = target;
        //                item.TargetSection = target.Parent.Title;

        //                string tempStr = "";
        //                DateTime? tempDt = null;
        //                long tempId = 0;

                     
        //                switch (source.ID)
        //                {
        //                    case 1:
        //                        objCaseReferral.ReferredToDoctorID = tempId;//((objPCR.ComplaintReported == null || objPCR.ComplaintReported == "") ? "" : (objPCR.ComplaintReported + ", ")) + tempStr;
        //                        break;
        //                    case 2:
        //                        objCaseReferral.ReferredToClinicID = tempId;//((objPCR.ChiefComplaints == null || objPCR.ChiefComplaints == "") ? "" : (objPCR.ChiefComplaints + ", ")) + tempStr;
        //                        break;
        //                    case 3:
        //                        objCaseReferral.ReferredToMobile = ((objCaseReferral.ReferredToMobile == null || objCaseReferral.ReferredToMobile == "") ? "" : (objCaseReferral.ReferredToMobile + ", ")) + tempStr;
        //                        break;
        //                    case 4:
        //                        objCaseReferral.ProvisionalDiagnosis = ((objCaseReferral.ProvisionalDiagnosis == null || objCaseReferral.ProvisionalDiagnosis == "") ? "" : (objCaseReferral.ProvisionalDiagnosis + ", ")) + tempStr;
        //                        break;
        //                    case 5:
        //                        objCaseReferral.ChiefComplaints = ((objCaseReferral.ChiefComplaints == null || objCaseReferral.ChiefComplaints == "") ? "" : (objCaseReferral.ChiefComplaints + ", ")) + tempStr;
        //                        break;
        //                    case 6:
        //                        objCaseReferral.Summary = ((objCaseReferral.Summary == null || objCaseReferral.Summary == "") ? "" : (objCaseReferral.Summary + ", ")) + tempStr;
        //                        break;
        //                    case 7:
        //                        objCaseReferral.Observations = ((objCaseReferral.Observations == null || objCaseReferral.Observations == "") ? "" : (objCaseReferral.Observations + ", ")) + tempStr;
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
    }

}
