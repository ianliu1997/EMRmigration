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

namespace PalashDynamics.MIS.Administration
{
    public partial class AdministrationReports : UserControl
    {
        public AdministrationReports()
        {
            InitializeComponent();
        }

        private void hlUserAuditReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new UserAuditTrail();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlUserListDetail_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new UserList();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlUserAuditUserActivity_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new AuditTrailUser_Activity();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
        
        private void hlDPR_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DPR();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlAdmission_Click(object sender, RoutedEventArgs e)
        {
            //hlAdmission
            this.content.Content = new AdmissionReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDischarge_Click(object sender, RoutedEventArgs e)
        {
            //DischargeReport
            this.content.Content = new DischargeReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hltransfer_Click(object sender, RoutedEventArgs e)
        {
            //TransferReport

            this.content.Content = new TransferReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
               
    }
}
