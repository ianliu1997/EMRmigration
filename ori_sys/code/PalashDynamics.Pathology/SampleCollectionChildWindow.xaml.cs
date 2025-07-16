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
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Pathology;
using PalashDynamics.ValueObjects.Master;
using CIMS;

using System.Text.RegularExpressions;
using PalashDynamics.Service.PalashTestServiceReference;
namespace PalashDynamics.Pathology
{
    public partial class SampleCollectionChildWindow : ChildWindow
    {
        //Random r = new Random();

        #region Public Variables
        public string msgText;
        public string msgTitle;
        #endregion

        public SampleCollectionChildWindow()
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
            this.Loaded += new RoutedEventHandler(SampleCollectionChildWindow_Loaded);
        }

        private bool CheckValidations()
        {
            bool result = true;
            try
            {
                if (txtQuantity.Text == null || txtQuantity.Text == "")
                { 
                    txtQuantity.SetValidation("Quantity is required.");
                    txtQuantity.RaiseValidationError();
                    txtQuantity.Focus();
                    result = false;
                }
                else
                    txtQuantity.ClearValidationError();
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        private void FillAgencyMaster()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.MasterTable = MasterTableNameList.M_PathAgencyMaster;
            BizAction.MasterList = new List<MasterListItem>();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {                    
                    List<MasterListItem> objList = new List<MasterListItem>();

                    // objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;
                    objList.Add(new MasterListItem(0, "- Select -"));
                    objList.AddRange(((clsGetMasterListBizActionVO)args.Result).MasterList);
                    txtAgencyName.ItemsSource = null;
                    txtAgencyName.ItemsSource = objList;
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        void SampleCollectionChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            CheckValidations();
            FillAgencyMaster();
            if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoGenerateSampleNo)
            {
                txtSampleNo.IsReadOnly = true;
                this.txtSampleNo.Text = "Auto Generated";
            }            
        }

        public event RoutedEventHandler OnSaveButtonClick;
        public event RoutedEventHandler OnCancelButtonClick;

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (chkval())
            {
                clsPathOrderBookingDetailVO item = (clsPathOrderBookingDetailVO)this.DataContext;

                item.IsOutSourced = chkIsOutSourced.IsChecked.Value;
                if (item.IsOutSourced == true && txtAgencyName.SelectedItem == null)
                {
                    msgText = "Agency Name is not selected.\n Please Select Agency name.";

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
                else
                {
                    if (txtAgencyName.SelectedItem != null)
                        item.AgencyID = ((MasterListItem)txtAgencyName.SelectedItem).ID;

                    if (txtAgencyName.SelectedItem != null && ((MasterListItem)txtAgencyName.SelectedItem).ID != 0)
                        item.AgencyName = ((MasterListItem)txtAgencyName.SelectedItem).Description;

                    if (txtSampleNo.Text != null && txtSampleNo.Text != "Auto Generated")
                        item.SampleNo = Convert.ToString(txtSampleNo.Text);
                    if (txtQuantity.Text != null && txtQuantity.Text != "")
                    {
                        item.Quantity = Double.Parse(txtQuantity.Text);
                        if (OnSaveButtonClick != null)
                        {
                            OnSaveButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
                        }
                        this.DialogResult = true;
                    }
                    else
                    {
                        msgText = "Quantity is Invalid. Please Enter Correct Number.";

                        MessageBoxControl.MessageBoxChildWindow msgWindow =
                            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgWindow.Show();
                        txtQuantity.Text = "";
                    }
                }
            }
        }
                

        private bool chkval()
        {
            bool result = true;
            try
            {
                if(((IApplicationConfiguration)App.Current).ApplicationConfigurations.AutoGenerateSampleNo==false)
                {
                    if (txtSampleNo.Text == null || txtSampleNo.Text == "")
                    {
                        txtSampleNo.SetValidation("Quantity is required.");
                        txtSampleNo.RaiseValidationError();
                        txtSampleNo.Focus();
                        result = false;
                    }
                    else
                        txtSampleNo.ClearValidationError();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick((clsPathOrderBookingDetailVO)(this.DataContext), e);
            }
            this.DialogResult = false;
        }

        private void chkIsOutSourced_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)(((CheckBox)sender).IsChecked))
            {
                tblAgencyName.Visibility = Visibility.Visible;                
                txtAgencyName.Visibility = Visibility.Visible;
            }
            else
            {
                txtAgencyName.Text = "";
                tblAgencyName.Visibility = Visibility.Collapsed;                
                txtAgencyName.Visibility = Visibility.Collapsed;
            }            
        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtQuantity.Text != "")
            //{
            //    bool isNumberValid = true;
            //    double number = -1;
            //    if (!double.TryParse(txtQuantity.Text, out number))
            //    {
            //        isNumberValid = false;

            //        msgText = "Quantity is Invalid. Please Enter Correct Number.";

            //        MessageBoxControl.MessageBoxChildWindow msgWindow =
            //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
            //        msgWindow.Show();
            //        //MessageBox.Show("Quantity is Invalid. Please Enter Correct Number.");
            //        txtQuantity.Text = "";
            //    }
            //}
            if (!((TextBox)sender).Text.IsValueDouble())
            {
                if (textBefore == null)
                    textBefore = "0";
                ((TextBox)sender).Text = textBefore;
                ((TextBox)sender).SelectionStart = selectionStart;
                ((TextBox)sender).SelectionLength = selectionLength;
                textBefore = "";
                selectionStart = 0;
                selectionLength = 0;
            }
        }

        private void txtQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;
        private void txtSampleNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber())
            {
                if (textBefore != null)
                {
                    ((TextBox)sender).Text = textBefore;
                    ((TextBox)sender).SelectionStart = selectionStart;
                    ((TextBox)sender).SelectionLength = selectionLength;
                    textBefore = "";
                    selectionStart = 0;
                    selectionLength = 0;
                }
            }
        }

        private void txtSampleNo_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
    }
}

