using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects;
using CIMS;

namespace EMR
{
    public partial class frmReferralLetter : ChildWindow
    {
        public frmReferralLetter()
        {
            InitializeComponent();
            this.DataContext = new clsEMRReferralLetterVO();
            this.Title = "Referral Letter";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ttlReferralLetter");
        }
        public string Drname;
        public event RoutedEventHandler OnAddButton_Click;
        public ObservableCollection<clsEMRReferralLetterVO> RefLetterList { get; set; }
        List<MasterListItem> ReferralDoctor = new List<MasterListItem>();

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Drname != null)
                txtDrName.Text = Drname;
            if (this.DataContext != null)
            {
                clsEMRReferralLetterVO objReferral = (clsEMRReferralLetterVO)this.DataContext;
                if (objReferral.ReferalType == 1 || objReferral.ReferalType == 0)
                {
                    rdoFirst.IsChecked = true;
                    objReferral.SelectedReferal = Convert.ToString(rdoFirst.Content);
                }
                else if (objReferral.ReferalType == 2)
                {
                    rdoSecomd.IsChecked = true;
                    objReferral.SelectedReferal = Convert.ToString(rdoSecomd.Content);
                }
                else if (objReferral.ReferalType == 3)
                {
                    rdoThird.IsChecked = true;
                    objReferral.SelectedReferal = Convert.ToString(rdoThird.Content);
                }
                if (objReferral.TakeOverDate != null)
                {
                    chkpasien.IsChecked = true;
                    dtppasien.IsEnabled = true;
                }
                if (objReferral.ConsultEndDate != null)
                {
                    chksalesai.IsChecked = true;
                    dtpsalesai.IsEnabled = true;
                }
                if (objReferral.NextConsultDate != null)
                {
                    chkUlang.IsChecked = true;
                    dtpUlang.IsEnabled = true;
                }
                if (objReferral.JointCareDate != null)
                {
                    chkbersama.IsChecked = true;
                    dtpbersama.IsEnabled = true;
                }
                if (objReferral.ID == 0)
                {
                    objReferral.Date = DateTime.Now;
                }
            }
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (ChkValidation())
            {
                this.DialogResult = true;
                if (rdoFirst.IsChecked == true)
                {
                    ((clsEMRReferralLetterVO)this.DataContext).ReferalType = 1;
                    ((clsEMRReferralLetterVO)this.DataContext).SelectedReferal = Convert.ToString(rdoFirst.Content);
                }
                else if (rdoSecomd.IsChecked == true)
                {
                    ((clsEMRReferralLetterVO)this.DataContext).ReferalType = 2;
                    ((clsEMRReferralLetterVO)this.DataContext).SelectedReferal = Convert.ToString(rdoSecomd.Content);
                }
                else if (rdoThird.IsChecked == true)
                {
                    ((clsEMRReferralLetterVO)this.DataContext).ReferalType = 3;
                    ((clsEMRReferralLetterVO)this.DataContext).SelectedReferal = Convert.ToString(rdoThird.Content);
                }
                if (OnAddButton_Click != null)
                    OnAddButton_Click(this, new RoutedEventArgs());
            }
        }
        private bool ChkValidation()
        {
            bool Result = true;

            if (txtReferalSpecialization.Text == null || txtReferalSpecialization.Text == string.Empty)
            {
                txtReferalSpecialization.SetValidation("Please enter di RS");
                txtReferalSpecialization.RaiseValidationError();
                txtReferalSpecialization.Focus();

                Result = false;

            }

            else
                txtReferalSpecialization.ClearValidationError();

            if (String.IsNullOrEmpty(txtDrName.Text))
            {
                txtDrName.SetValidation("Please select dr");
                txtDrName.RaiseValidationError();
                txtDrName.Focus();
                Result = false;
            }
            else
                txtDrName.ClearValidationError();

            return Result;

        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            dtppasien.IsEnabled = Convert.ToBoolean(chkpasien.IsChecked);
            dtppasien.DisplayDate = DateTime.Today;
            dtpsalesai.IsEnabled = Convert.ToBoolean(chksalesai.IsChecked);
            dtpsalesai.DisplayDate = DateTime.Today;
            dtpUlang.IsEnabled = Convert.ToBoolean(chkUlang.IsChecked);
            dtpUlang.DisplayDate = DateTime.Today;
            dtpbersama.IsEnabled = Convert.ToBoolean(chkbersama.IsChecked);
            dtpbersama.DisplayDate = DateTime.Today;
            CheckBox chkDate = ((CheckBox)sender);
            if (chkDate.IsChecked==false)
            {
                clsEMRReferralLetterVO obj=this.DataContext as clsEMRReferralLetterVO;
                switch (chkDate.Name)
                { 
                    case "chksalesai":
                        obj.ConsultEndDate = null;
                        break;
                    case "chkUlang":
                        obj.NextConsultDate = null;
                        break;
                    case "chkbersama":
                        obj.JointCareDate = null;
                        break;
                    case "chkpasien":
                        obj.TakeOverDate = null;
                        break;
                }
            }
        }
    }
}

