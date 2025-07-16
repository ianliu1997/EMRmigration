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
    public partial class CalculateBillAmount : ChildWindow
    {
        public CalculateBillAmount()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            //txtBillAmt.Text = "";
            txtCashRecieved.Text = "";
            txtAmountToRefund.Text = "";
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBillAmt.Text) && !string.IsNullOrEmpty(txtCashRecieved.Text))
            {
                if (Convert.ToDouble(txtBillAmt.Text) > Convert.ToDouble(txtCashRecieved.Text))
                {

                    string msgText = "Cash Recieved Amount should be greater than or Equal to Bill Amount ";
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWD.Show();
                }
                else
                {
                    txtAmountToRefund.Text = (Convert.ToDouble(txtCashRecieved.Text) - Convert.ToDouble(txtBillAmt.Text)).ToString();
                }
            }
        }

        private void txtCashRecieved_SelectionChanged(object sender, RoutedEventArgs e)
        {
            txtAmountToRefund.Text = "";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

