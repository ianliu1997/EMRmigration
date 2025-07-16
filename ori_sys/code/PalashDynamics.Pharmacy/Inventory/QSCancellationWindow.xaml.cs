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

namespace PalashDynamics.Pharmacy.Inventory
{
    public partial class QSCancellationWindow : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        public QSCancellationWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAppReason.Text) && !string.IsNullOrWhiteSpace(txtAppReason.Text))
            {
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = true;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("", "Please enter Remark", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD1.Show();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

