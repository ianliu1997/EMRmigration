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
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.MIS.Sales.IPD;

namespace PalashDynamics.MIS.Sales
{
    public partial class SalesReport : UserControl
    {
        public SalesReport()
        {
            InitializeComponent();
        }

        private void hlRevenueStreamClinicwise_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RevenueStreamClinicWiseReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDailySalesReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailySalesReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlClinicWisefortheselectedduration_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ClinicWisefortheselectedduration();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlWeeklySalesReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new WeeklySalesReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //if ((bool)chkExporttoExcel.IsChecked)
            //{
            //    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            //    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            //    client.DeleteMISReportFileCompleted += (s1, args1) =>
            //    {
            //        if (args1.Error == null)
            //        {

            //        }
            //    };
            //    client.DeleteMISReportFileAsync("/Reports/Sales");
            //}
        }

        private void hlDailyCollectionReport_Click(object sender, RoutedEventArgs e)
        {
            //this.content.Content = new DailyCollectionReport();
            //Links.Visibility = Visibility.Collapsed;
            //content.Visibility = Visibility.Visible;
            //content_control.Visibility = Visibility.Visible;
            //this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            //this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

          
            this.content.Content = new DailyCollectionReportIPD(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlCashCollectionReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new CashCollectionReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlServiceWiseCollectonReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ServiceWiseDailyCollection();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlMonthlyRevenueReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new MonthlyRevenueReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPatientwisProcedureReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientWiseProcedureReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPatientadvanceconsumedReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientAdvanceConsumed();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlGyanueRevenueReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GynaueReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPatientDetailsReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientWiseDoctorReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void hlDoctorDetailsReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DoctorWisePatientDetail();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        }

        private void hlDoctorBasedConsultationBillingReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DoctorBasedConsultationBillingReport(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

       //..............................

        private void hlDAILYCOLLECTIONSUMMARY_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyCollectionSummary(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDAILYCOLLECTIONREPORTDues_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyCollectionReportDues(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlBalanceAmountReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new BalanceAmoutReport(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDISCOUNTREGISTERDAILY_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DiscountRegisterDaily(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlRefundReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RefundCollection(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPatientAdvanceReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PatientAdvance(0);
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlServiceWiseBillingReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ServicewiseBillingReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hmbcompanytds_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new COMPANYPAYMENTANDTDS();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDoctorwiseRev_Click(object sender, RoutedEventArgs e)
        {

            this.content.Content = new DoctorWiseRevenue();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
           
        }

        private void collectionReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new CollectionReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void revenuetrandeReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RevenueTrandeReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPackageServiceWiseBillingReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PackageServiceBookingReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPackageServiceSttusReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PackageServiceStatusReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void ledgerDetailReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new LedgerDetailsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void revenueHeaderReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RevenueHeaderReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void revenueDetailsReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new RevenueDetailsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void packageBookingReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PackageBookingReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
    }
}
