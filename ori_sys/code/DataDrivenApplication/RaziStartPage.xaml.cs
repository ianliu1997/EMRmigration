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

namespace DataDrivenApplication
{
    public partial class RaziStartPage : UserControl
    {
        public RaziStartPage()
        {
            InitializeComponent();
            App.MainWindow = this;
            this.Loaded += new RoutedEventHandler(RaziStartPage_Loaded);
        }

        void RaziStartPage_Loaded(object sender, RoutedEventArgs e)
        {
            MainRegion.Content = new TemplateList();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
