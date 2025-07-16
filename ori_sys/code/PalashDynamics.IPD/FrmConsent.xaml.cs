using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Text;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Administration;
using MessageBoxControl;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.IPD.Forms;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using C1.Silverlight.Pdf;
using C1.Silverlight.RichTextBox.PdfFilter;
using System.IO;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using PalashDynamics.UserControls;


namespace PalashDynamics.IPD
{
    public partial class FrmConsent : ChildWindow, IInitiateCIMS
    {
        #region Variable Declaration
        public List<MasterListItem> DataList { get; private set; }
        private SwivelAnimation objAnimation;
        long newVisitAdmID, newVisitAdmUnitID;
        List<clsPatientFieldsConfigVO> objListDesc = null;
        private clsConsentDetailsVO selectedPatient;
        long VisitAdmID, VisitAdmUnitID, PatientID, PatientUnitID;
        int OpdIpd;
        public bool IsCancel = true;
        bool IsBtnClicked = false;
        bool IsPatientExist = true;
        StringBuilder strPatInfo;
        WaitIndicator WaitNew = new WaitIndicator();
        long PrintID = 0;
        #endregion

        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            switch (Mode)
            {
                case "NEW":
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient == null)
                    {
                        //System.Windows.Browser.HtmlPage.Window.Alert("Please select the Patient.");
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }

                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID == 0)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", "Please select the Patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();

                        IsPatientExist = false;
                        break;
                    }
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.IsDischarge == true)
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW5 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Patient Already Discharged.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW5.Show();
                        IsPatientExist = false;
                        break;
                    }
                    IsPatientExist = true;
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    TextBlock mElement = (TextBlock)rootPage.FindName("SampleSubHeader");

                    // mElement.Text = " : " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).FirstName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).MiddleName + " " + ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).LastName;
                    break;
            }
        }

        #endregion

        #region Constructor and Loaded
        public FrmConsent()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                newVisitAdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                newVisitAdmUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                PatientID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID;

            }
            this.Loaded += new RoutedEventHandler(FrmConsent_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 0)));

        }
        public FrmConsent(long VisitAdmID, long VisitAdmUnitID, bool OpdIpd)
        {
            InitializeComponent();
            newVisitAdmID = VisitAdmID;
            newVisitAdmUnitID = VisitAdmUnitID;
            this.Loaded += new RoutedEventHandler(FrmConsent_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 0)));
        }
        public FrmConsent(long PatientID, long PatientUnitID, long VisitID, long UnitID)
        {
            InitializeComponent();
            this.VisitAdmID = VisitID;
            this.VisitAdmUnitID = UnitID;
            this.PatientID = PatientID;
            this.PatientUnitID = PatientUnitID;
            richToolbar.RichTextBox = richTextBox;
            OpdIpd = 0;
            // IsForOPD = true;
            getDetailsOfSelectedPatient();
            getDescription();
            this.Loaded += new RoutedEventHandler(FrmConsent_Loaded);
        }
        public FrmConsent(clsIPDAdmissionVO Obj)
        {
            InitializeComponent();
            newVisitAdmID = Obj.ID;
            newVisitAdmUnitID = Obj.UnitId;
            this.PatientID = Obj.PatientId;
            this.PatientUnitID = Obj.PatientUnitID;

            OpdIpd = 1;
            // IsForIPD = true;
            getDetailsOfSelectedPatient();
            getDescription();
            cmdSave.IsEnabled = false;
            IsCancel = true;
            this.Loaded += new RoutedEventHandler(FrmConsent_Loaded);
            objAnimation = new SwivelAnimation(FrontPanel, BackPanel, RotationDirection.LeftToRight, new Duration(new TimeSpan(0, 0, 0, 0, 0)));
        }
        void FrmConsent_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsPatientExist == false)
                this.DialogResult = false;
            else
            {
                GetPatientConsent(newVisitAdmID, newVisitAdmUnitID, true);
                SetPatientDetails();
                cmdSave.IsEnabled = false;
                if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                    richTextBox.Html = "[%PATIENTINFO%]" + richTextBox.Html;
            }
        }
        #endregion

        #region Fill Combo

        private void FillConsentCombo()
        {
            try
            {
                clsGetConsentByConsentTypeBizActionVO BizAction = new clsGetConsentByConsentTypeBizActionVO();
                BizAction.MasterList = new List<MasterListItem>();
                BizAction.ConsentTypeID = 0;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        List<MasterListItem> objList = new List<MasterListItem>();
                        objList.Add(new MasterListItem(0, "-- Select --"));
                        objList.AddRange(((clsGetConsentByConsentTypeBizActionVO)e.Result).MasterList);

                        CmbOtTheatre.ItemsSource = null;
                        CmbOtTheatre.ItemsSource = objList;
                        CmbOtTheatre.SelectedItem = objList[0];

                        DataList = objList;
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

        //private void FillComboWithConsent()
        //{
        //    try
        //    {
        //        clsGetMasterListConsentBizActionVO BizAction = new clsGetMasterListConsentBizActionVO();
        //        BizAction.MasterTable = MasterTableNameList.M_IPDConsentMaster;
        //        BizAction.MasterList = new List<MasterListItem>();

        //        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
        //        PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //        Client.ProcessCompleted += (s, e) =>
        //        {
        //            if (e.Error == null && e.Result != null)
        //            {
        //                List<MasterListItem> objList = new List<MasterListItem>();
        //                objList.Add(new MasterListItem(0, "-Select-"));
        //                objList.AddRange(((clsGetMasterListConsentBizActionVO)e.Result).MasterList);
        //                CmbOtTheatre.ItemsSource = null;
        //                CmbOtTheatre.ItemsSource = objList;
        //                CmbOtTheatre.SelectedItem = objList[0];

        //                DataList = objList;
        //            }

        //        };
        //        Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //        Client.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        #endregion

        #region Button Click Events
        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Html = string.Empty;
            dtpDate.SelectedDate = System.DateTime.Now;
            IsBtnClicked = false;
            dtpDate.IsEnabled = false;
            FillConsentCombo();
            getDetailsOfSelectedPatient();
            getDescription();
            cmdSave.IsEnabled = true;
            cmdNew.IsEnabled = false;
            IsCancel = false;
            grpPatientDetails.Visibility = Visibility.Collapsed;
            richTextBox.Text = null;
            objAnimation.Invoke(RotationType.Forward);
        }
        private void cmdModify_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            objAnimation.Invoke(RotationType.Backward);
            IsBtnClicked = false;
            cmdNew.IsEnabled = true;
            cmdSave.IsEnabled = false;
            grpPatientDetails.Visibility = Visibility.Visible;
            if (IsCancel == true)
            {
                this.DialogResult = false;
            }
            else
            {
                IsCancel = true;
            }
           
        }
        private void AddConsentTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string template = ((MasterListItem)CmbOtTheatre.SelectedItem).TemplateName;
                if (!string.IsNullOrEmpty(template))
                {
                    IsBtnClicked = true;
                    richTextBox.Html = template;
                    string rtbstring = string.Empty;
                    if (!(richTextBox.Html.Contains("[%PATIENTINFO%]")))
                    {
                        rtbstring = richTextBox.Html;
                        rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "[%PATIENTINFO%]");
                        rtbstring = rtbstring.Insert(rtbstring.IndexOf("<body>") + 6, "");
                        richTextBox.Html = rtbstring;
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgConsentList.SelectedItem != null)
            {
                //string template = ((clsConsentDetailsVO)dgConsentList.SelectedItem).Consent;
                //this.PrintID = ((clsConsentDetailsVO)dgConsentList.SelectedItem).ID;
                string sPath = "PatientConsent" + ((clsConsentDetailsVO)dgConsentList.SelectedItem).UnitID + "_" + ((clsConsentDetailsVO)dgConsentList.SelectedItem).ID + ".pdf";
                ViewPDFReport(sPath);
                //frmPrintConsent win_PrintConsent = new frmPrintConsent(template,PrintID, (clsConsentDetailsVO)dgConsentList.SelectedItem);
                //win_PrintConsent.Show();
            }
        }
       
        string msgText;
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (CmbOtTheatre.SelectedItem != null && ((MasterListItem)CmbOtTheatre.SelectedItem).ID > 0 && IsBtnClicked==true)
            {
                if (!string.IsNullOrEmpty(richTextBox.Text.Trim()))
                {
                    string msgTitle = "Palash";

                    msgText = "Are you sure you want to save ?";

                    MessageBoxChildWindow msgW = null;
                    msgW = new MessageBoxChildWindow(msgTitle, msgText, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    msgW.OnMessageBoxClosed += new MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    msgW.Show();
                }
                else
                {

                    msgText = "Please enter consent";

                    MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                msgText = "Please select a consent Template";
                MessageBoxChildWindow msgW1 = new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgW1.Show();
            }

        }
        void msgW_OnMessageBoxClosed(MessageBoxResult result)
        {
            if (result == MessageBoxResult.Yes)
            {
                Save();

            }
        }
        #endregion

        #region Private Methods
        private void Save()
        {
            try
            {
                clsSaveConsentDetailsBizActionVO BizAction = new clsSaveConsentDetailsBizActionVO();
                BizAction.ConsentDetails = new clsConsentDetailsVO();
                BizAction.ConsentDetails = GetDataToSave();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (((clsSaveConsentDetailsBizActionVO)arg.Result).ConsentDetails != null)
                        {

                            msgText = "Record saved successfully.";
                            MessageBoxChildWindow msgW1 =
                                new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgW1.OnMessageBoxClosed += (MessageBoxResult res) =>
                                {
                                    if (res == MessageBoxResult.OK)
                                    {
                                        WaitNew.Show();
                                        cmdNew.IsEnabled = true;
                                        cmdSave.IsEnabled = false;
                                        GetPatientPathoDetailsInHtml(newVisitAdmID, newVisitAdmUnitID, ((clsSaveConsentDetailsBizActionVO)arg.Result).ConsentDetails.ConsentSummaryID);
                                        GetPatientConsent(newVisitAdmID, newVisitAdmUnitID, true);
                                        grpPatientDetails.Visibility = Visibility.Visible;
                                        objAnimation.Invoke(RotationType.Backward);
                                    }
                                };
                            msgW1.Show();
                            
                        }
                    }
                    else
                    {

                        msgText = "Error occurred while processing.";
                        MessageBoxChildWindow msgW1 =
                               new MessageBoxChildWindow("", msgText, MessageBoxButtons.Ok, MessageBoxIcon.Error);

                        msgW1.Show();
                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        private clsConsentDetailsVO GetDataToSave()
        {
            clsSaveConsentDetailsBizActionVO BizAction = new clsSaveConsentDetailsBizActionVO();
            BizAction.ConsentDetails = new clsConsentDetailsVO();

            BizAction.ConsentDetails.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

            BizAction.ConsentDetails.Date = Convert.ToDateTime(dtpDate.SelectedDate); //DateTime.Now;

            BizAction.ConsentDetails.VisitAdmID = newVisitAdmID;

            BizAction.ConsentDetails.VisitAdmUnitID = newVisitAdmUnitID;

            BizAction.ConsentDetails.Opd_Ipd = 1;

            BizAction.ConsentDetails.ConsentID = ((MasterListItem)CmbOtTheatre.SelectedItem).ID;

            BizAction.ConsentDetails.Consent = richTextBox.Html;

            return BizAction.ConsentDetails;
        }
        private void SetPatientDetails()
        {
            if (IsPatientExist == true)
            {
                lblPatientName1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                lblAdmissionDate1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.AdmissionDate.ToString().Substring(0, 10);
                lblAdmissionNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.IPDNO;
                lblMrNo1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                lblPatientGender1.Text = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName;
            }
        }
        private void getDescription()
        {
            try
            {
                clsGetPatientConfigFieldsBizActionVO bizActionVO = new clsGetPatientConfigFieldsBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        objListDesc = new List<clsPatientFieldsConfigVO>();
                        objListDesc.AddRange(((clsGetPatientConfigFieldsBizActionVO)args.Result).OtPateintConfigFieldsMatserDetails);
                    }
                };
                client.ProcessAsync(bizActionVO, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            { }
        }
        private string getDataForSelectedPatient(string SelectedID, string FiledName)
        {
            switch (SelectedID)
            {
                case "1": return selectedPatient.FirstName;
                case "2": return selectedPatient.MRNo;
                case "3": return selectedPatient.LastName;
                case "4": return selectedPatient.DOB;
                case "5": return selectedPatient.MiddleName;
                case "6": return selectedPatient.VisitorFirstName;
                case "7": return selectedPatient.VisitorMiddleName;
                case "8": return selectedPatient.VisitorLastName;
                case "9": return Convert.ToString(selectedPatient.Date.Date);
                case "12": return selectedPatient.DoctorFirstName;
                case "13": return selectedPatient.DoctorMiddleName;
                case "14": return selectedPatient.DoctoLastName;
                case "15": return selectedPatient.Location;
                case "16": return selectedPatient.Gender;


                default: return "";
            }
        }
        private void getDetailsOfSelectedPatient()
        {
            try
            {
                clsGetConsentDetailsBizActionVO BizAction = new clsGetConsentDetailsBizActionVO();
                BizAction.ConsentDetails = new clsConsentDetailsVO();
                BizAction.ConsentDetails.PatientID = PatientID;
                BizAction.ConsentDetails.PatientUnitID = PatientUnitID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        clsGetConsentDetailsBizActionVO result = arg.Result as clsGetConsentDetailsBizActionVO;
                        selectedPatient = result.ConsentDetails;
                    }
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch
            {
                throw;
            }
        }
        public void GetPatientConsent(long VisitAdmID, long VisitAdmUnitID, bool OpdIpd)
        {
            try
            {
                clsGetPatientConsentsBizActionVO BizAction = new clsGetPatientConsentsBizActionVO();
                BizAction.VisitAdmID = VisitAdmID;
                BizAction.VisitAdmUnitID = VisitAdmUnitID;
                BizAction.OPD_IPD = false;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        if ((clsGetPatientConsentsBizActionVO)e.Result != null && ((clsGetPatientConsentsBizActionVO)e.Result).ConsentList != null)
                        {
                            if (((clsGetPatientConsentsBizActionVO)e.Result).ConsentList.Count > 0)
                            {
                                dgConsentList.ItemsSource = null;
                                dgConsentList.ItemsSource = ((clsGetPatientConsentsBizActionVO)e.Result).ConsentList;

                            }
                        }
                    }

                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch
            {

            }

        }

        clsConsentDetailsVO PatientEntry = new clsConsentDetailsVO();
        private void GetPatientPathoDetailsInHtml(long VisitAdmID, long VisitAdmUnitID, long printID)
        {
            clsGetPatientConsentsDetailsInHTMLBizActionVO BizAction = new clsGetPatientConsentsDetailsInHTMLBizActionVO();
            BizAction.VisitAdmID = VisitAdmID;
            BizAction.VisitAdmUnitID = VisitAdmUnitID;
            BizAction.ConsentTypeID = printID;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetPatientConsentsDetailsInHTMLBizActionVO)arg.Result).ResultDetails != null)
                        {
                            PatientEntry = new clsConsentDetailsVO();
                            PatientEntry = ((clsGetPatientConsentsDetailsInHTMLBizActionVO)arg.Result).ResultDetails;

                            strPatInfo = new StringBuilder();
                            strPatInfo.Append(PatientEntry.PatientInfoHTML);
                            strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientEntry.MRNo.ToString());
                            strPatInfo = strPatInfo.Replace("[Date]", "    :" + PatientEntry.Date.ToString("dd MMM yyyy"));
                            strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientEntry.PatientName.ToString());
                            strPatInfo = strPatInfo.Replace("[AgeSex]", "   :" + Convert.ToString(PatientEntry.Age+" / "+PatientEntry.Gender));
                            strPatInfo = strPatInfo.Replace("[ConsentName]", "    :" + PatientEntry.Description.ToString());
                        }

                        richTextBox.Html = PatientEntry.Consent;
                        richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
                        PrintReport(printID, PatientEntry);
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                      new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();

        }

        public PaperKind PaperKind { get; set; }
        public Thickness PrintMargin { get; set; }
        private void PrintReport(long PrintID, clsConsentDetailsVO PatientResultEntry)
        {
            try
            {
                richTextBox.Document.Margin = new Thickness(5, 5, 5, 5);
                PrintMargin = new Thickness(5, 0, 5, 5);

                //Printing
                var viewManager = new C1RichTextViewManager
                {
                    Document = richTextBox.Document,
                    PresenterInfo = richTextBox.ViewManager.PresenterInfo
                };
                var print = new PrintDocument();
                int presenter = 0;

                print.PrintPage += (s, printArgs) =>
                {
                    var element = (FrameworkElement)HeaderTemplate.LoadContent();
                    Grid grd = (Grid)element.FindName("PatientDetails");

                    if (grd != null)
                    {
                        grd.Visibility = Visibility.Visible;
                        grd.DataContext = PatientResultEntry;

                    }
                    element.DataContext = viewManager.Presenters[presenter];
                    printArgs.PageVisual = element;
                    printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                };

                var pdf = new C1PdfDocument(PaperKind.A4);

                PdfFilter.PrintDocument(richTextBox.Document, pdf, PrintMargin);
                pdf.CurrentPage = pdf.Pages.Count - 1;
                String appPath;

                long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;

                appPath = "PatientConsent" + UnitID + "_" + PrintID + ".pdf";

                Stream FileStream = new MemoryStream();
                MemoryStream MemStrem = new MemoryStream();

                pdf.Save(MemStrem);
                FileStream.CopyTo(MemStrem);

                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                client.UploadReportFileForConsentCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        WaitNew.Close();
                        ViewPDFReport(appPath);
                    }
                };
                client.UploadReportFileForConsentAsync(appPath, MemStrem.ToArray());
                client.CloseAsync();
            }
            catch (Exception)
            {

            }
            finally
            {
                WaitNew.Close();
            }
        }

        private void ViewPDFReport(string FileName)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../PatientConsentReportDocuments");
            string fileName1 = address.ToString();
            fileName1 = fileName1 + "/" + FileName;
            HtmlPage.Window.Invoke("open", new object[] { fileName1, "", "" }); 
        }



        #endregion

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
