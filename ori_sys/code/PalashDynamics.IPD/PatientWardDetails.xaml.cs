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

namespace PalashDynamics.IPD
{
    public partial class PatientWardDetails : ChildWindow
    {
        public PatientWardDetails()
        {
            InitializeComponent();
        }

    

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
    }
}

