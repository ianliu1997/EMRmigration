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
using PalashDynamic.Localization;

namespace EMR
{
    public partial class frmAddDiagnosis : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        public string NewDiagnosis { get; set; }
        public frmAddDiagnosis()
        {
            InitializeComponent();
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (txtDiagnosisName.Text == "")
            {
                txtDiagnosisName.SetValidation("Please enter Diagnosis");
                txtDiagnosisName.RaiseValidationError();
                txtDiagnosisName.Focus();

            }
            else
            {
                txtDiagnosisName.ClearValidationError();

                NewDiagnosis = txtDiagnosisName.Text.ToTitleCase();
                this.DialogResult = true;
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

