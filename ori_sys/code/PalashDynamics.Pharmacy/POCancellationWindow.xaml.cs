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

namespace PalashDynamics.Pharmacy
{
    public partial class POCancellationWindow : ChildWindow
    {
        public POCancellationWindow()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        //* Added by - Ajit Jadhav
        //* Added Date - 7/9/2016
        //* Comments - Check WhiteSpace in text box

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
            this.DialogResult = true;
        }
    }
}

