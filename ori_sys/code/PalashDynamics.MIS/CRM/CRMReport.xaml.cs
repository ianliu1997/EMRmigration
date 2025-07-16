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
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
//using PalashDynamics.UserControl;
using PalashDynamics.Animations;
using PalashDynamics.MIS.Masters;

namespace PalashDynamics.MIS.CRM
{
    public partial class CRMReport : UserControl
    {
        public CRMReport()
        {
            InitializeComponent();
        }

        private void hlCustomerDetailReport_Click(object sender, RoutedEventArgs e)
        {

            this.content.Content = new CustomerDetailReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlClinicWisePatientDetail_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ClinicWisePatientDetail();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

       
        private void hlClinicwiseInvestigation_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ClinicWiseInvestigationReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlSearchPatient_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new SearchPatient();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

            

        }

        private void hlLoyaltyProgram_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new LoyaltyProgramReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;



        }

      

        private void hlVisitwiseReferralDoctorReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new VisitwiseReferralDoctor();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlAnalysisReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Analysis_Report();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

      

        private void hlReferralPatient_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientReferralListReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlLostPatientList_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new frmLostPatientList();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlAppointmentListReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new AppointmentListReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlPatientRegistartion_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RegistrationReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void ConsaltationandProcedureReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ConsalltionAndProcedureReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void ConsaltationandPartailProcedureDoneReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new Consultation_PartailProcedureDoneReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void FullyProcedureDoneReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new FullyProcedureDone();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void UNITWISEFOOTFALL_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new UnitwiseFootwallReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void UNITWISEFOOTFALLREVENUEMIX_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new unitWiseFootAllRevenueMix();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
