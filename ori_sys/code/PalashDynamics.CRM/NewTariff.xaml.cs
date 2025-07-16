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
    public partial class NewTariff : UserControl
    {
        public NewTariff()
        {
            InitializeComponent();
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
