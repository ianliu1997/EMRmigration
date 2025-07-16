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
using System.Windows.Browser;
using PalashDynamics.MIS.CRM;

namespace PalashDynamics.MIS.IVF
{
    public partial class IVFReport : UserControl
    {
        public IVFReport()
        {
            InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVF/PregnancyRateWithDonor.aspx"), "_blank");
            this.content.Content = new PregnancyRateWithDonor();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void HyperlinkButton2_Click(object sender, RoutedEventArgs e)
        {
            //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVF/PregnancySucessRate.aspx"), "_blank");
            this.content.Content = new PregnancySucessRateGraph();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void HyperlinkButton3_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PregnancySucessRatePieChart();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void HyperlinkButton4_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientETDetails();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void HyperlinkButton5_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientOPU();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void ImplantationRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Implantion_rate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void FertilizationRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Fertilization_rate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void OnGoingPregnancyRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new OnGoingPregancy_rate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void BiochemicalPregnancyRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new BiochemicalPregancy_rate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void ChemicalPregnancyRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ChemicalPregnancyRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void CumulativePregnancyRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Cumulative_PregnancyRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void LiveBirthRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new LiveBirthRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void CleavageRate_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new CleavageRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void occembryocount_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new frmAccEmbryoCount();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void donormather_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DonorMother();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

            this.content.Content = new Patient_IVFANCSummary();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }



        private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientInvestigationReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void IVIPatient_Click(object sender, RoutedEventArgs e)
        {
            //IVIPatientTeropyCountReport
            this.content.Content = new IVIPatientTeropyCountReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void IVIPatientTheropy_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IVF_IUI_PatientTherapyCount();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void OPUANDETPatient_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new OPU_ETCountReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PGD_PGSReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void HyperlinkButton_Click_4(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IVFPatientDemographic();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void HyperlinkButton_Click_5(object sender, RoutedEventArgs e)
        {
            
            this.content.Content = new GroodGradeEmbryos();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void HyperlinkButton_Click_6(object sender, RoutedEventArgs e)
        {
            this.content.Content = new MiscarriageRate();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
