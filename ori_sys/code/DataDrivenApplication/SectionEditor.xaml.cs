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
using System.Threading;

namespace DataDrivenApplication
{
    public partial class SectionEditor : ChildWindow
    {
        public SectionEditor()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SectionEditor_Loaded);
        }

        public RoutedEventHandler OnSaveButtonClick;
        bool Isload = true;
        private bool IsSaved = false;
        void SectionEditor_Loaded(object sender, RoutedEventArgs e)
        {
            cmbContainer.Items.Add("History");
            cmbContainer.Items.Add("Consultation");
            cmbContainer.Items.Add("Follow Up Consultation");
            cmbContainer.Items.Add("Medication");

            if (this.DataContext != null)
            {
                tbTitle.Text = string.IsNullOrEmpty(((SectionDetail)this.DataContext).Title) ? "" : ((SectionDetail)this.DataContext).Title;
                tbDescription.Text = string.IsNullOrEmpty(((SectionDetail)this.DataContext).Description) ? "" : ((SectionDetail)this.DataContext).Description;
                #region Added by Harish
                cmbContainer.SelectedItem = ((SectionDetail)this.DataContext).Tab;
                IsToolTip.IsChecked = ((SectionDetail)this.DataContext).IsToolTip;
                tbToolTip.Text = string.IsNullOrEmpty(((SectionDetail)this.DataContext).ToolTipText) ? "" : ((SectionDetail)this.DataContext).ToolTipText;
                #endregion
                int i = 0;
                while (i < ((SectionDetail)this.DataContext).ReadPermission.Count)
                {
                    switch (((SectionDetail)this.DataContext).ReadPermission[i])
                    {
                        case "Admin":
                            chkAdminRead.IsChecked = true;
                            break;
                        case "Nurse":
                            chkNurseRead.IsChecked = true;
                            break;
                        //case "FrontOffice":
                        //    chkFrontOfficeRead.IsChecked = true;
                        //    break;
                        case "Doctor":
                            chkDoctorRead.IsChecked = true;
                            break;
                    }
                    i++;
                }

                i = 0;
                while (i < ((SectionDetail)this.DataContext).ReadWritePermission.Count)
                {
                    switch (((SectionDetail)this.DataContext).ReadWritePermission[i])
                    {
                        case "Admin":
                            chkAdminWrite.IsChecked = true;
                            break;
                        case "Nurse":
                            chkNurseWrite.IsChecked = true;
                            break;
                        //case "FrontOffice":
                        //    chkFrontOfficeWrite.IsChecked = true;
                        //    break;
                        case "Doctor":
                            chkDoctorWrite.IsChecked = true;
                            break;
                    }
                    i++;
                }
                IsSaved = true;
            }
            else
            {
                this.DataContext = new SectionDetail();
                ((SectionDetail)this.DataContext).ReadPermission = new List<string>();
                ((SectionDetail)this.DataContext).ReadWritePermission = new List<string>();
                cmbContainer.SelectedItem = "Consultation";
                IsSaved = false;
            }
            Isload = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCreateForm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            ((SectionDetail)this.DataContext).Title = tbTitle.Text;
            ((SectionDetail)this.DataContext).Description = tbDescription.Text;
            ((SectionDetail)this.DataContext).IsToolTip = IsToolTip.IsChecked.Value;
            ((SectionDetail)this.DataContext).ToolTipText = tbToolTip.Text;
            ((SectionDetail)this.DataContext).Tab = (string)cmbContainer.SelectedValue;
            if (IsSaved == false)
                ((SectionDetail)this.DataContext).UniqueId = Guid.NewGuid().ToString();
            else
            {
                ((SectionDetail)this.DataContext).ReadPermission.Clear();
                ((SectionDetail)this.DataContext).ReadWritePermission.Clear();
            }

            if (chkAdminRead.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadPermission.Add("Admin");

            if (chkNurseRead.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadPermission.Add("Nurse");

            //if(chkFrontOfficeRead.IsChecked==true)
            //    ((SectionDetail)this.DataContext).ReadPermission.Add("FrontOffice");

            if (chkDoctorRead.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadPermission.Add("Doctor");

            if (chkAdminWrite.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadWritePermission.Add("Admin");

            if (chkNurseWrite.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadWritePermission.Add("Nurse");

            //if (chkFrontOfficeWrite.IsChecked == true)
            //    ((SectionDetail)this.DataContext).ReadWritePermission.Add("FrontOffice");

            if (chkDoctorWrite.IsChecked == true)
                ((SectionDetail)this.DataContext).ReadWritePermission.Add("Doctor");

            if (OnSaveButtonClick != null)
            {
                OnSaveButtonClick(((SectionDetail)this.DataContext), e);
            }
            this.DialogResult = true;
        }


        private void chkReadAll_Checked(object sender, RoutedEventArgs e)
        {
            chkAdminRead.IsChecked = true;
            chkNurseRead.IsChecked = true;
            //chkFrontOfficeRead.IsChecked = true;
            chkDoctorRead.IsChecked = true;
        }

        private void chkReadAll_Unchecked(object sender, RoutedEventArgs e)
        {
            chkAdminRead.IsChecked = false;
            chkNurseRead.IsChecked = false;
            //chkFrontOfficeRead.IsChecked = false;
            chkDoctorRead.IsChecked = false;

            //((SectionDetail)this.DataContext).ReadPermission.Remove("Admin");
            //((SectionDetail)this.DataContext).ReadPermission.Remove("Doctor");
            //((SectionDetail)this.DataContext).ReadPermission.Remove("FrontOffice");
            //((SectionDetail)this.DataContext).ReadPermission.Remove("Nurse");

        }

        private void chkWriteAll_Checked(object sender, RoutedEventArgs e)
        {
            chkAdminWrite.IsChecked = true;
            chkNurseWrite.IsChecked = true;
            //chkFrontOfficeWrite.IsChecked = true;
            chkDoctorWrite.IsChecked = true;
        }

        private void chkWriteAll_Unchecked(object sender, RoutedEventArgs e)
        {
            chkAdminWrite.IsChecked = false;
            chkNurseWrite.IsChecked = false;
            //chkFrontOfficeWrite.IsChecked = false;
            chkDoctorWrite.IsChecked = false;

            //((SectionDetail)this.DataContext).ReadWritePermission.Remove("Admin");
            //((SectionDetail)this.DataContext).ReadWritePermission.Remove("Doctor");
            //((SectionDetail)this.DataContext).ReadWritePermission.Remove("FrontOffice");
            //((SectionDetail)this.DataContext).ReadWritePermission.Remove("Nurse");

        }

        private void chkAdminRead_Unchecked(object sender, RoutedEventArgs e)
        {
            //switch (((CheckBox)sender).Name)
            //{
            //    case "chkAdminRead":
            //        ((SectionDetail)this.DataContext).ReadPermission.Remove("Admin");
            //        break;
            //    case "chkDoctorRead":
            //        ((SectionDetail)this.DataContext).ReadPermission.Remove("Doctor");
            //        break;
            //    case "chkFrontOfficeRead":
            //        ((SectionDetail)this.DataContext).ReadPermission.Remove("FrontOffice");
            //        break;
            //    case "chkNurseRead":
            //        ((SectionDetail)this.DataContext).ReadPermission.Remove("Nurse");
            //        break;
            //}
        }

        private void chkAdminWrite_Unchecked(object sender, RoutedEventArgs e)
        {
            //switch (((CheckBox)sender).Name)
            //{
            //    case "chkAdminWrite":
            //        ((SectionDetail)this.DataContext).ReadWritePermission.Remove("Admin");
            //        break;
            //    case "chkDoctorWrite":
            //        ((SectionDetail)this.DataContext).ReadWritePermission.Remove("Doctor");
            //        break;
            //    case "chkFrontOfficeWrite":
            //        ((SectionDetail)this.DataContext).ReadWritePermission.Remove("FrontOffice");
            //        break;
            //    case "chkNurseWrite":
            //        ((SectionDetail)this.DataContext).ReadWritePermission.Remove("Nurse");
            //        break;
            //}
        }

        private void IsToolTip_Checked(object sender, RoutedEventArgs e)
        {
            checked
            {
                txtBlockToolTip.Visibility = Visibility.Visible;
                tbToolTip.Visibility = Visibility.Visible;
            }
        }

        private void IsToolTip_Unchecked(object sender, RoutedEventArgs e)
        {
            unchecked
            {
                txtBlockToolTip.Visibility = Visibility.Collapsed;
                tbToolTip.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

