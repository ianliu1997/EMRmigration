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

namespace PalashDynamics.MIS.Sales.IPD
{
    public partial class IPDReport : UserControl
    {
        public IPDReport()
        {
            InitializeComponent();
        }

        private void hlkIPDReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IPDReportDetails();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }
    }
}
