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

namespace PalashDynamics.MIS.Clinical
{
    public partial class ClinicalReport : UserControl
    {
        public ClinicalReport()
        {
            InitializeComponent();
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/WeeklyServicewiseCollection.aspx"), "_blank");
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
