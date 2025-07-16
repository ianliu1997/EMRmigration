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

namespace PalashDynamics.MIS.InventoryPharmacy
{
    public partial class InventoryReports : UserControl
    {
        public InventoryReports()
        {
            InitializeComponent();            
        }

        private void hlItemwiseDailySalesReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemWiseDailySales();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }


        private void hlItemwisePackageIssueReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemWisePackageIssue();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDialyPharmacySalesSummary_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyPharmacySales();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlDailyPharmacySalesDetail_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new DailyPharmacyDetailsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlStockStatement_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new StockStatementReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlGRNatClinic_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRNatClinicReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlSalesReturn_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new SalesReturnReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;


        }

        private void hlGRC_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRC();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlItemReturn_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemReturn();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlIndentPendingForApprovalReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IndentPendingForApproval();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPurchaseOrderList_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PurchaseorderList();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlGoodsreceivedNoteReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRNReportWithoutItem();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlIssuestoClinic_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IssuestoClinic();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

       

        private void hlReturnsFromClinic_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ReturnsFromClinic();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        

        }

        private void hlStockLedgerReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new StockLedgerReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        

        }

        private void hlSotckCheckReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new SotckCheckReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        
        }

        private void hlAvailableStock_Click(object sender, RoutedEventArgs e)
        {

            this.content.Content = new AvailableStock();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlIndentReceiveandIssueReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IndentReceiveandIssueReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlMaterialConsumptionReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new MaterialConsumptionReportMIS();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlABCAnalysisReport_Click(object sender, RoutedEventArgs e)
        {
             this.content.Content = new ABCAnalysis();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
       

        private void hlFNSAnalysisReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new FNSAnalysis();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void ItemLedgerReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemLedgerReport();
            ((PalashDynamics.MIS.InventoryPharmacy.ItemLedgerReport)(this.content.Content)).IsOldReport = false;
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void QuotationCMPReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new QuotationComparison();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;

        }

        private void hlGoodsreceivedNoteReportItemWise_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRNReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlPurchaseOrderListItemWise_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PurchaseOrderItemWise();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void PharmacyCollectionReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new PharmacyCollectionReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        //Added by Ashish z. as per Discussion with Dr. Gautham(Milann) and Mangesh on dated 28082016  
        private void ItemLedgerReportOld_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new ItemLedgerReport();
            ((PalashDynamics.MIS.InventoryPharmacy.ItemLedgerReport)(this.content.Content)).IsOldReport = true;
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
        //End

        private void IndentToGRNReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IndentToGRNDetailsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void MaterialConsumptionsReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new MaterialConsumption();

            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlGRNLEDGERREPORT_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRNLedgerReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void hlGRNLEDGERDETAILSREPORT_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new GRNLedgerDetailsReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void StockAdjustmentReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new StockAdjustmentReport();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        //Begin::Added by AniketK on 23Jan2019
        private void QuarantineIssueScrapExpirYReturNReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IssueQSToScrapExpiryReturn();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }

        private void GRNIssueQStoGRNReturnReport_Click(object sender, RoutedEventArgs e)
        {
            this.content.Content = new IssueQSToGRNReturn();
            Links.Visibility = Visibility.Collapsed;
            content.Visibility = Visibility.Visible;
            content_control.Visibility = Visibility.Visible;
            this.content.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.content.VerticalAlignment = System.Windows.VerticalAlignment.Center;
        }
        //End::Added by AniketK on 23Jan2019
      
    }
}
