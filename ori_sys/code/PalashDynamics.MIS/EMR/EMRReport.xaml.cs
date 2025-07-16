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

namespace PalashDynamics.MIS.EMR
{
    public partial class EMRReport : UserControl
    {
        public EMRReport()
        {
            InitializeComponent();
        }
        private void hlHistroScopyAndLaproScopy_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new HistroAndLapro();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlCPOEReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new CPOEReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDiagnosisVitalsReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DiagnosisVitalsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlHistory_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new FrmAllHistory();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPrescriptionReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PrescriptionReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlProcedure_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new frmRptProcedure();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDiagnosis_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RptDiagnosis();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlVitals_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DiagnosisVitalsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
