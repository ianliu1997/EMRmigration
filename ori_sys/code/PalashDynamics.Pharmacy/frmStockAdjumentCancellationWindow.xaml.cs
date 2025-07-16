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
    public partial class frmStockAdjumentCancellationWindow : ChildWindow
    {
        int ClickedFlag = 0;
        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;
        public frmStockAdjumentCancellationWindow()
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

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1;

            if (ClickedFlag == 1)
            {
                if (txtAppReason.Text == "")
                {
                    txtAppReason.SetValidation("Please enter reason");
                    txtAppReason.RaiseValidationError();
                    txtAppReason.Focus();
                    ClickedFlag = 0;
                }
                else
                {
                    txtAppReason.ClearValidationError();
                    ClickedFlag = 0;
                    if (OnSaveButton_Click != null)
                    {
                        this.DialogResult = true;
                        OnSaveButton_Click(this, new RoutedEventArgs());

                        this.Close();
                    }
                }
            }

        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            OnCancelButton_Click(this, new RoutedEventArgs());
            this.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

    }
}

