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
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Master;

namespace OPDModule.Forms
{
    public partial class StaffWindow : ChildWindow
    {
        public StaffWindow()
        {
            InitializeComponent();
            //this.
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        bool IsPageLoded = false;
        WaitIndicator Indicatior = null;

        public event RoutedEventHandler OnSaveButton_Click;

        public clsStaffMasterVO StaffDetails = new clsStaffMasterVO();

        private void StaffWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                this.DataContext = new clsStaffMasterVO();
                FetchData();
                FillDesignation();
                SetComboboxValue();
                txtFirstName1.Focus();
                Indicatior.Close();
            }

            txtFirstName1.Focus();
            txtFirstName1.UpdateLayout();
            IsPageLoded = true;
        }

        private void FillDesignation()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_DesignationMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();

                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    cmbDesignation.ItemsSource = null;
                    cmbDesignation.ItemsSource = objList;


                }

                if (this.DataContext != null)
                {
                    cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
                }



            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();

        }

        private void SetComboboxValue()
        {
            cmbDesignation.SelectedValue = ((clsStaffMasterVO)this.DataContext).DesignationID;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButton_Click != null)
            {
                this.DialogResult = true;
                OnSaveButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }
        
        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            FetchData();
        }

        private void FetchData()
        {
            clsGetStaffMasterDetailsBizActionVO BizAction = new clsGetStaffMasterDetailsBizActionVO();
            BizAction.StaffMasterList = new List<clsStaffMasterVO>();
            if (cmbDesignation.SelectedItem != null)
                BizAction.DesignationID = ((MasterListItem)cmbDesignation.SelectedItem).ID;

            if (txtFirstName1.Text != null)
                BizAction.FirstName = txtFirstName1.Text;

            if (txtLastName2.Text != null)
                BizAction.LastName = txtLastName2.Text;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            dgStaffList.ItemsSource = null;
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (((clsGetStaffMasterDetailsBizActionVO)arg.Result).StaffMasterList != null)
                    {
                        dgStaffList.ItemsSource = ((clsGetStaffMasterDetailsBizActionVO)arg.Result).StaffMasterList;

                    }
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", "Error Occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

                    msgW1.Show();
                }

            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        
        #region Validation
        
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        
        private void txtFirstName1_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName1.Text = txtFirstName1.Text.ToTitleCase();
        }

        private void txtLastName2_LostFocus(object sender, RoutedEventArgs e)
        {
            txtLastName2.Text = txtLastName2.Text.ToTitleCase();
        }

        private void TextName_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsPersonNameValid())
            {
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        #endregion

        private void dgStaffList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((clsStaffMasterVO)dgStaffList.SelectedItem != null)
                StaffDetails = ((clsStaffMasterVO)dgStaffList.SelectedItem);
        }

       
    }
}

