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
using PalashDynamics.ValueObjects.Master.DoctorPayment;

namespace PalashDynamics.Administration
{
    public partial class ShareRangeChildWindow : ChildWindow
    {
        public List<clsDoctorShareRangeList> DoctorShareRangeList = new List<clsDoctorShareRangeList>();
        public ShareRangeChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dgDoctorShareRangeList.ItemsSource = null;
            dgDoctorShareRangeList.ItemsSource = DoctorShareRangeList;
        }
    }
}

