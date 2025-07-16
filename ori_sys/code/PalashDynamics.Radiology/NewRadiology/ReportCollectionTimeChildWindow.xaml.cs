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
using PalashDynamics.ValueObjects.Radiology;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology.PathologyMasters;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Master;

namespace PalashDynamics.Radiology
{
    public partial class ReportCollectionTimeChildWindow : ChildWindow
    {
        public event RoutedEventHandler OnSaveButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;
        public string msgText;
        public string msgTitle;
        public ReportCollectionTimeChildWindow()
        {
            InitializeComponent();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
            {
                if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId
                  && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.AllowClinicalTransaction == true)
                {
                    //do  nothing
                }
                else
                    cmdSave.IsEnabled = false;
            }

            this.Loaded += new RoutedEventHandler(ReportCollectionTimeChildWindow_Loaded);
        }

        void ReportCollectionTimeChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dtpSampColleDate.SelectedDate = DateTime.Now;
            dtpSampCollTime.Value = DateTime.Now;
            chkIsOutSourced.IsChecked = true;
            FillAgency();
        }

        private void FillAgency()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.MasterTable = MasterTableNameList.M_RadAgencyMaster;
            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)e.Result).MasterList);
                    txtAgencyName.ItemsSource = null;
                    txtAgencyName.ItemsSource = objList;
                    txtAgencyName.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private bool chkval()
        {
            bool result = true;
            try
            {
                if (Convert.ToDateTime(dtpSampColleDate.SelectedDate) > DateTime.Now || dtpSampColleDate.SelectedDate == null)
                {
                    msgText = " Please Select Proper Agency Schedule Date.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampColleDate.Focus();
                    result = false;
                }
                else if (Convert.ToDateTime(dtpSampCollTime.Value) > DateTime.Now || dtpSampCollTime.Value == null)
                {
                    msgText = " Please Select Proper Agency Schedule Time.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                    dtpSampCollTime.Focus();
                    result = false;
                }
                if (chkIsOutSourced.IsChecked == true)
                {
                    if (((MasterListItem)txtAgencyName.SelectedItem).ID == 0)
                    {
                        txtAgencyName.TextBox.SetValidation("Agency Name is required");
                        txtAgencyName.TextBox.RaiseValidationError();
                        txtAgencyName.Focus();
                        result = false;
                    }
                    else if (((MasterListItem)txtAgencyName.SelectedItem) == null)
                    {
                        txtAgencyName.TextBox.SetValidation("Agency Name is required");
                        txtAgencyName.TextBox.RaiseValidationError();
                        txtAgencyName.Focus();
                        result = false;
                    }
                    else
                        txtAgencyName.TextBox.ClearValidationError();
                }
                else
                    txtAgencyName.TextBox.ClearValidationError();

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkval())
            {
                DateTime collDate = Convert.ToDateTime(dtpSampColleDate.SelectedDate);
                DateTime collTime = Convert.ToDateTime(dtpSampCollTime.Value);
                DateTime dtCombinedColl = new DateTime(collDate.Year, collDate.Month, collDate.Day, collTime.Hour, collTime.Minute, collTime.Second);
                clsRadOrderBookingDetailsVO item = (clsRadOrderBookingDetailsVO)this.DataContext;
                if (dtpSampColleDate.SelectedDate != null)
                    item.ReportCollectedDateTime = dtCombinedColl;
                if (txtAgencyName.SelectedItem != null && chkIsOutSourced.IsChecked == true)
                {
                    item.IsOutSourced = true;
                    item.AgencyID = ((MasterListItem)txtAgencyName.SelectedItem).ID;
                    item.AgencyName = ((MasterListItem)txtAgencyName.SelectedItem).Description;
                }
                if (OnSaveButtonClick != null)
                {
                    OnSaveButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), e);
                }
                this.DialogResult = true;
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((clsRadOrderBookingDetailsVO)(this.DataContext), e);
            }
            this.DialogResult = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void chkIsOutSourced_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}

