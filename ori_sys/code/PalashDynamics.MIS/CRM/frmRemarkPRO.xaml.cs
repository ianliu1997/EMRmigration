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

namespace PalashDynamics.MIS.CRM
{
    public partial class frmRemarkPRO : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public string Remarks { get; set; }

        public frmRemarkPRO()
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

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //Remarks = txtAlerts.Text;
              this.DialogResult = true;
              if (OnSaveButton_Click != null)
                  OnSaveButton_Click(this, new RoutedEventArgs());
             
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
      
        }
    }


