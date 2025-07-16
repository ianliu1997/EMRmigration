using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;
using System.Text;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.IPD.Forms
{
    public partial class frmPrintConsent : ChildWindow
    {
        #region Variable Declarations
        StringBuilder strPatInfo;
        long newVisitAdmID = 0;
        long newVisitAdmUnitID = 0;
        long PrintID = 0;
        Grid grd { get; set; }
        string Template { get; set; }
        clsConsentDetailsVO PatientDetails { get; set; }
        #endregion

        #region Constructor and Loaded
        public frmPrintConsent()
        {
            InitializeComponent();
            
        }

        public frmPrintConsent(string Template, long PrintID, clsConsentDetailsVO PatientDetails)
        {
            InitializeComponent();
            this.PrintID = PrintID;
            this.Loaded += new RoutedEventHandler(PrintConsent_Loaded);
        }

        private void PrintConsent_Loaded(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedIPDPatient != null)
            {
                this.newVisitAdmID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                this.newVisitAdmUnitID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;

            }
            //richTextBox.Html = Template;
            //richTextBox.IsReadOnly = true;
            //var element = (FrameworkElement)HeaderTemplate.LoadContent();
            //grd = (Grid)element.FindName("PatientDetails");
            //grd.Visibility = Visibility.Visible;
            //grd.DataContext = PatientDetails;
            GetPatientPathoDetailsInHtml(this.newVisitAdmID, this.newVisitAdmUnitID,this.PrintID);

        }
        #endregion

        #region Button Click Events
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintReport(newVisitAdmID, PatientEntry, strPatInfo.ToString());
            this.DialogResult = true;
        }
        #endregion

        #region Private Methods

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var oldItem = e.RemovedItems.OfType<C1TabItem>().FirstOrDefault();
                if (oldItem == null) return; // richTextBoxTab and the others are null the first time around because InitializeComponent is running.

                if (oldItem == richTextBoxTab)
                {
                    htmlBox.Text = richTextBox.Html;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == htmlTab)
                {
                    richTextBox.Html = htmlBox.Text;
                    rtfBox.Text = new RtfFilter().ConvertFromDocument(richTextBox.Document);
                }
                else if (oldItem == rtfTab)
                {
                    richTextBox.Document = new RtfFilter().ConvertToDocument(rtfBox.Text);
                    htmlBox.Text = richTextBox.Html;
                }
            }
            catch { }
        }

        clsConsentDetailsVO PatientEntry = new clsConsentDetailsVO();
        private void GetPatientPathoDetailsInHtml(long VisitAdmID, long VisitAdmUnitID,long printID)
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
                            strPatInfo = strPatInfo.Replace("[Age]", "   :" + Convert.ToString( PatientEntry.Age));
                            strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientEntry.Gender.ToString());
                        }

                        richTextBox.Html = PatientEntry.Consent;
                        richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", strPatInfo.ToString());
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

        private void PrintReport(long VisitAdmID, clsConsentDetailsVO PatientEntry, string strPatInfo)
        {
            richTextBox.Html = null;
            richTextBox.Html = PatientEntry.Consent;
            richTextBox.Html = richTextBox.Html.Replace("[%PATIENTINFO%]", "");
            var viewManager = new C1RichTextViewManager
            {
                Document = richTextBox.Document,
                PresenterInfo = richTextBox.ViewManager.PresenterInfo
            };

            var print = new PrintDocument();
            int presenter = 0;
            int count = 0;

            print.PrintPage += (s, printArgs) =>
            {
                var element = (FrameworkElement)HeaderTemplate.LoadContent();

                if (count == viewManager.Presenters.Count)
                {
                    Grid grd1 = new Grid();
                    grd1 = (Grid)element.FindName("PatientDetails");
                    if (grd1 != null)
                    {
                        grd1.Visibility = Visibility.Collapsed;
                        grd1.DataContext = PatientEntry;
                    }
                }
                else
                {
                    if (viewManager.Presenters.Count == 1)
                    {
                        Grid grd1 = new Grid();
                        grd1 = (Grid)element.FindName("PatientDetails");

                        if (grd1 != null)
                        {
                            grd1.Visibility = Visibility.Visible;
                            grd1.DataContext = PatientEntry;
                        }
                    }
                    else
                    {
                        Grid grd1 = new Grid();
                        grd1 = (Grid)element.FindName("PatientDetails");
                        grd1.Visibility = Visibility.Collapsed;
                    }

                }
                element.DataContext = viewManager.Presenters[presenter];
                printArgs.PageVisual = element;
                printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;
                count = presenter;
                count++;

            };
            string print1 = "Consent_" + System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            print.Print(print1);
        }
        #endregion
    }
}

