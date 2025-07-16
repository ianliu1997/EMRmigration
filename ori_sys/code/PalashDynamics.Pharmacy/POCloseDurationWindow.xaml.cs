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
using CIMS;

namespace PalashDynamics.Pharmacy
{
    public partial class POCloseDurationWindow : ChildWindow
    {
        #region Variable Declaration

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;


        public DateTime ApprovedByLvl2Date;

        #endregion

        public POCloseDurationWindow()
        {
            InitializeComponent();
        }

        private void txtPOAutoCloseDays_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

        }

        private void txtPOAutoCloseDays_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPOAutoCloseDays.Text) && !txtPOAutoCloseDays.Text.IsItNumber())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = String.Empty;
                selectionStart = 0;
                selectionLength = 0;
            }


            if (txtPOAutoCloseDays.Text.Length > 0)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                long timeStamp = (long)(ApprovedByLvl2Date.Subtract(origin).TotalSeconds + Convert.ToDouble(Convert.ToInt32(txtPOAutoCloseDays.Text) * 86400));
                txtPOAutoCloseDate.Text = (origin.AddSeconds(timeStamp)).ToString("dd-MMM-yyyy");
            }

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReasonForChange.Text) && !string.IsNullOrWhiteSpace(txtReasonForChange.Text))
            {
                if (OnSaveButton_Click != null)
                    OnSaveButton_Click(this, new RoutedEventArgs());
                this.DialogResult = true;
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgWD1 =
                 new MessageBoxControl.MessageBoxChildWindow("Palash IVF", "Please enter reason for changing the PO Close Duration.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgWD1.Show();
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }


    }
}

