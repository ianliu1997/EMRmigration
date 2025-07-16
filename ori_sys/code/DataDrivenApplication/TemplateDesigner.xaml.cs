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
using DataDrivenApplication;

namespace DataDrivenApplication
{
    public partial class TemplateDesigner : UserControl
    {
        public TemplateDesigner()
        {
            InitializeComponent();
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {
            App.MainWindow.MainRegion.Content = new FormDesigner(new FormDetail());
            

        }
    }
}
