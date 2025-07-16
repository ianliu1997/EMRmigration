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

namespace PalashDynamics.MIS
{
    public partial class SubReportLinks : UserControl
    {
        public SubReportLinks()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SubReportLinks_Loaded);
        }

        void SubReportLinks_Loaded(object sender, RoutedEventArgs e)
        {
            long ActiveLinkId = 133;

            switch (ActiveLinkId)
            {
                case 132:
                    dpMasterList.Visibility = System.Windows.Visibility.Visible;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 133:
                     dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Visible;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 134:
                        dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Visible;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 135:
                        dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Visible;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 148:
                       dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Visible;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 149:
                        dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Visible;
                    dpCustomReport.Visibility = System.Windows.Visibility.Collapsed;
                    break;
                case 150:
                       dpMasterList.Visibility = System.Windows.Visibility.Collapsed;
                    dpCRM.Visibility = System.Windows.Visibility.Collapsed;
                    dpSecurity.Visibility = System.Windows.Visibility.Collapsed;
                    dpInventoryPharmacy.Visibility = System.Windows.Visibility.Collapsed;
                    dpClinical.Visibility = System.Windows.Visibility.Collapsed;
                    dpAccounts.Visibility = System.Windows.Visibility.Collapsed;
                    dpCustomReport.Visibility = System.Windows.Visibility.Visible;
                    break;
                

            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            int a=2;
            int b=3;
            int c = a + b;

        }
    }
}
