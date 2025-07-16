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

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class frmAddEvent : ChildWindow
    {
        public frmAddEvent()
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

        private void dgChangeCouple_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void chkIsSelected_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

