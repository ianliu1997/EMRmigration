using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Text;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;
using System.Windows.Printing;
using C1.Silverlight.RichTextBox;
using CIMS;

namespace PalashDynamics.IPD
{
    public partial class frmPrintDischargeSummary : ChildWindow
    {
        #region Variable Declarations
        clsIPDDischargeSummaryVO PatientEntry = new clsIPDDischargeSummaryVO();
        public long lPrintID = 0;
        public long lUnitID = 0;
        public long lAdmissionUnitID = 0;
        public long lAdmissionID = 0;
        StringBuilder strPatInfo;
        #endregion

        #region Constructor and Loaded
        public frmPrintDischargeSummary()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PrintDischarge_Loaded);

        }

        private void PrintDischarge_Loaded(object sender, RoutedEventArgs e)
        {
            GetPatientsInfoInHTML(lPrintID, lUnitID,lAdmissionID,lAdmissionUnitID);

        }
        #endregion

        #region Button Click Events
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintReport(PatientEntry, strPatInfo.ToString());
            this.DialogResult = true;
        }

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
        #endregion

        #region Private Methods
        private void GetPatientsInfoInHTML(long PrintID, long UnitID, long AdmissionID,long AdmissionUnitID)
        {
            clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO BizAction = new clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO();
        
            BizAction.UnitID = UnitID;
            BizAction.AdmID = AdmissionID;
            BizAction.AdmUnitID = AdmissionUnitID;
            BizAction.PrintID = PrintID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        if (((clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)arg.Result).DischargeSummaryDetails != null)
                        {
                            PatientEntry = new clsIPDDischargeSummaryVO();
                            PatientEntry = ((clsGetPatientsDischargeSummaryInfoInHTMLBizActionVO)arg.Result).DischargeSummaryDetails;

                            strPatInfo = new StringBuilder();
                            strPatInfo.Append(PatientEntry.PatientInfoHTML);
                            strPatInfo = strPatInfo.Replace("[MRNO]", "    :" + PatientEntry.MRNo.ToString());
                            strPatInfo = strPatInfo.Replace("[PatName]", "    :" + PatientEntry.PatientName.ToString());
                            strPatInfo = strPatInfo.Replace("[Age]", "   :" + Convert.ToString(PatientEntry.Age));
                            strPatInfo = strPatInfo.Replace("[Sex]", "    :" + PatientEntry.Gender.ToString());
                        }

                        richTextBox.Html = PatientEntry.TextDocument;
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

            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }

        private void PrintReport(clsIPDDischargeSummaryVO PatientEntry, string strPatInfo)
        {
            richTextBox.Html = null;
            richTextBox.IsReadOnly = true;
            richTextBox.Html = PatientEntry.TextDocument;
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
            string print1 = "DischargeCard_" + System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            print.Print(print1);
        }
        #endregion
    }
}

