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
using PalashDynamics.ValueObjects.Administration.OTConfiguration;
using C1.Silverlight.RichTextBox;
using System.Windows.Printing;
using C1.Silverlight;
using C1.Silverlight.RichTextBox.Documents;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmPrintConsent : ChildWindow
    {
        #region Variable Declaration
        Grid grd { get; set; }
        string Template { get; set; }
        clsConsentDetailsVO PatientDetails { get; set; }
        #endregion

        #region Constructor and Loaded

        public frmPrintConsent()
        {
            InitializeComponent();
        }

        public frmPrintConsent(string Template, clsConsentDetailsVO PatientDetails)
        {
            InitializeComponent();
            this.Template = Template;
            this.PatientDetails = PatientDetails;
            this.PatientDetails.Consent = Template;
            this.Loaded += new RoutedEventHandler(PrintConsent_Loaded);
        }

        private void PrintConsent_Loaded(object sender, RoutedEventArgs e)
        {
            richTextBox.Html = Template;
            richTextBox.IsReadOnly = true;
            var element = (FrameworkElement)HeaderTemplate.LoadContent();
            grd = (Grid)element.FindName("PatientDetails");
            grd.Visibility = Visibility.Visible;
            grd.DataContext = PatientDetails;
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

        private void PrintReport()
        {
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
                grd.Visibility = Visibility.Visible;

                grd = (Grid)element.FindName("PatientDetails");

                if (grd != null)
                {
                    grd.Visibility = Visibility.Visible;
                    grd.DataContext = PatientDetails;
                }


                element.DataContext = viewManager.Presenters[presenter];
                printArgs.PageVisual = element;
                printArgs.HasMorePages = ++presenter < viewManager.Presenters.Count;

            };

            print.Print("A Christmas Carol");
        }

        #endregion

        #region Button Click Events
        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Html = "";
            PrintReport();
            this.DialogResult = true;

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
        #endregion

    }
}

