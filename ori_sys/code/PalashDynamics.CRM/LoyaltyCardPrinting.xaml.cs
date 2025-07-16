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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;

namespace PalashDynamics.CRM
{
    public partial class LoyaltyCardPrinting : UserControl
    {
        public LoyaltyCardPrinting()
        {
            InitializeComponent();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintTemplateWindow win = new PrintTemplateWindow();
            win.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
