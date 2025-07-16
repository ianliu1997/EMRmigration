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
    public partial class IPDSalesReport : UserControl
    {
        public IPDSalesReport()
        {
            InitializeComponent();
        }

        private void hlBalanceAmountReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new BalanceAmoutReport(1);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDAILYCOLLECTIONREPORT_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyCollectionReportIPD(1);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDAILYCOLLECTIONSUMMARY_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyCollectionSummary(1);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDISCOUNTREGISTERDAILY_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DiscountRegisterDaily(1);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void hlDAILYCOLLECTIONREPORTDUES_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyCollectionReportDues(1);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

    }
}
