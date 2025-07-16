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

namespace PalashDynamics.CRM
{
    public partial class PrintTemplateWindow : ChildWindow
    {
        public PrintTemplateWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {

            this.DialogResult = false;
        }

        private void hlCharges_Click(object sender, RoutedEventArgs e)
        {
            PaymentDetails win = new PaymentDetails();
            win.Show();
        }

        private void hlNewTariff_Click(object sender, RoutedEventArgs e)
        {
            ChildWindow chform = new ChildWindow();
            NewTariff win = new NewTariff();

            chform.Content = win;
            chform.Show();
            
        }
        
    }
}

