using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using PalashDynamics.Collections;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;


namespace PalashDynamics.Radiology
{
    public partial class PatientSearch : ChildWindow
    {
        public event RoutedEventHandler OnSaveButton_Click;
        bool isLoaded = false;

        public PatientSearch()
        {
            InitializeComponent();
            this.DataContext = new PatientSearchViewModel();
            this.Loaded += new RoutedEventHandler(PatientSearch_Loaded);
        }

        public PatientSearch(DateTime? FromDate, DateTime? ToDate)
        {
            InitializeComponent();
            FillLoyaltyProgram();
            this.DataContext = new PatientSearchViewModel(FromDate, ToDate);
        }

        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = (clsPatientGeneralVO)dataGrid2.SelectedItem;
        }

        void PatientSearch_Loaded(object sender, RoutedEventArgs e)
        {
            isLoaded = true;
            FillLoyaltyProgram();
            txtFirstName.Focus();
        }

        private void FillLoyaltyProgram()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_LoyaltyProgramMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
           
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "--Select--"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbLoyaltyProgram.ItemsSource = null;
                    cmbLoyaltyProgram.ItemsSource = objList;
                    cmbLoyaltyProgram.SelectedItem = objList[0];
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void PatientSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ((PatientSearchViewModel)this.DataContext).GetData();
            peopleDataPager.PageIndex = 0;

        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (((IApplicationConfiguration)App.Current).SelectedPatient != null && ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID != 0)
            {
                if (OnSaveButton_Click != null)
                {
                    this.DialogResult = true;
                    OnSaveButton_Click(this, new RoutedEventArgs());

                    this.Close();
                }
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select patient.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                msgW.Show();
            }

        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        #region Validation
        private void txtFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Text = txtFirstName.Text.ToTitleCase();
        }

        private void txtMiddleName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMiddleName.Text = txtMiddleName.Text.ToTitleCase();
        }

        private void txtLastName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Text = txtLastName.Text.ToTitleCase();
        }

        private void txtFamilyName_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFamilyName.Text = txtFamilyName.Text.ToTitleCase();
        }

        string textBefore = null;

        private void txtAutocompleteNumber_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((AutoCompleteBox)sender).Text;
        }

        private void txtAutocompleteNumber_TextChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((AutoCompleteBox)sender).Text))
            {
                if (!((AutoCompleteBox)sender).Text.IsNumberValid())
                {
                    ((AutoCompleteBox)sender).Text = textBefore;


                }
            }
        }

        #endregion
    }
}

