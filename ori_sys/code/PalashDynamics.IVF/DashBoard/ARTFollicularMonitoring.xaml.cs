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
using System.Windows.Media.Imaging;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.IO;
//using System.Windows.Forms.DataGridViewCell;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class ARTFollicularMonitoring : ChildWindow
    {

        #region Variables
        public event RoutedEventHandler OnSaveButton_Click;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();

        #endregion
        public ARTFollicularMonitoring()
        {
            InitializeComponent();
            FillTakenByComboBox();
           
            cmbTakenBy1.IsEnabled = false;
            cmbPhysician.IsEnabled = false;

        }
        #region Properties

        public byte[] AttachedFileContents { get; set; }

        #endregion
        #region Fill Commbo Box

        public void fillDoctor(long DoctorID)
        {
            clsGetDoctorDepartmentDetailsBizActionVO BizAction = new clsGetDoctorDepartmentDetailsBizActionVO();
            BizAction.MasterList = new List<MasterListItem>();
            BizAction.UnitId = 0;
            BizAction.DepartmentId = 0;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetDoctorDepartmentDetailsBizActionVO)arg.Result).MasterList);

                    cmbPhysician.ItemsSource = null;
                    cmbPhysician.ItemsSource = objList;
                    cmbPhysician.SelectedValue = (long)DoctorID;
                }
               
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        #endregion

        // added by Anumani 

        public bool GenValidation()
        {
            if (dtpFollicularDate.SelectedDate == null)
            {
                dtpFollicularDate.SetValidation("Please Select Planned Treatment");
                dtpFollicularDate.RaiseValidationError();
                dtpFollicularDate.Focus();
                return false;
            }
            else if (txtTime.Value == null)
            {
                txtTime.SetValidation("Please Select Planned Treatment");
                txtTime.RaiseValidationError();
                txtTime.Focus();
                return false;
            }
            if (cmbPhysician.SelectedItem == null)
            {

                
                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if (((MasterListItem)cmbPhysician.SelectedItem).ID == 0)
            {

                cmbPhysician.TextBox.SetValidation("Please Select Physician");
                cmbPhysician.TextBox.RaiseValidationError();
                cmbPhysician.Focus();
                return false;
            }
            else if
               ((string.IsNullOrWhiteSpace(txtEndometriumThickness.Text)))
            {
                txtEndometriumThickness.SetValidation("Please Select Endometrium");
                txtEndometriumThickness.RaiseValidationError();
                txtEndometriumThickness.Focus();
                return false;
            }
            else
            {
                dtpFollicularDate.ClearValidationError();
                txtTime.ClearValidationError();
                cmbPhysician.ClearValidationError();
                txtEndometriumThickness.ClearValidationError();
                txtEndometriumThickness.ClearValidationError();
                return true;
            }
        }

        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                txtFN.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        AttachedFileContents = new byte[stream.Length];
                        stream.Read(AttachedFileContents, 0, (int)stream.Length);
                    }
                }
                catch (Exception ex)
                {
                    string msgText = "Error while reading file.";
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFN.Text))
            {
                if (AttachedFileContents != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + txtFN.Text.Trim() });
                            AttachedFileNameList.Add(txtFN.Text.Trim());

                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", txtFN.Text.Trim(), AttachedFileContents);
                }
            }
        }


        private void dgFollicularSize_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            long LeftSize, FoliculeSize, RightSIze;
            if (dgFollicularSize.SelectedItem != null)
            {
                if (e.Column.DisplayIndex == 1)
                {
                    if (((PalashDynamics.ValueObjects.IVFPlanTherapy.clsFollicularMonitoringSizeDetails)(dgFollicularSize.SelectedItem)).LeftSize != string.Empty)
                    {

                    }
                }
                if (e.Column.DisplayIndex == 0)
                {
                    FoliculeSize = ((PalashDynamics.ValueObjects.IVFPlanTherapy.clsFollicularMonitoringSizeDetails)(dgFollicularSize.SelectedItem)).SizeOf_folicule;
                }
                if (e.Column.DisplayIndex == 2)
                {
                    //   RightSIze = Convert.ToString(((PalashDynamics.ValueObjects.IVFPlanTherapy.clsFollicularMonitoringSizeDetails)(dgFollicularSize.SelectedItem)).RightSIze);
                }
            }

            //int num = 0;
            //if (int.TryParse(LeftSize, out num))
            //{
            //    //Parse was successful so num now contains the integer value 5
            //    //process the parsed number here
            //}
            //else
            //{
            //    //Parse failed, num contains a default value (for int this is zero)
            //    //put error handling here
            //    MessageBox.Show("Please Enter a Valid Number");
            //}      
        }

        private void FillTakenByComboBox()
        {
            try
            {
                clsGetUserMasterListBizActionVO BizAction = new clsGetUserMasterListBizActionVO();
                BizAction.IsDecode = true;
                BizAction.ID = 0;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, ea) =>
                {
                    if (ea.Result != null && ea.Error == null)
                    {
                        List<MasterListItem> uList = new List<MasterListItem>();
                        uList.Add(new MasterListItem(0, "-- Select --"));
                        uList.AddRange(((clsGetUserMasterListBizActionVO)ea.Result).MasterList);
                        cmbTakenBy.ItemsSource = null;
                        cmbTakenBy.ItemsSource = uList;
                        cmbTakenBy.SelectedValue = ((IApplicationConfiguration)App.Current).CurrentUser.ID;
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
                client = null;
            }
            catch (Exception)
            {

            }
        }

        private void txtLeftOvery_KeyDown(object sender, KeyEventArgs e)
        {
            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;

            
                    
        }

        private void txtLeftOvery_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!((TextBox)sender).Text.IsItNumber() && textBefore != null)
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

        private void txtEndometriumThickness_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
            if (!((TextBox)sender).Text.IsValidDigintWithOneDecimalPlaces() && textBefore != null)
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
        private void txtEndometriumThickness_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Subtract || e.Key == Key.Decimal && (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = true;
            //}
            //else
            //{
            //    e.Handled = CIMS.Comman.HandleDecimal(sender, e);
            //}

            textBefore = ((TextBox)sender).Text;
            selectionStart = ((TextBox)sender).SelectionStart;
            selectionLength = ((TextBox)sender).SelectionLength;
        }
        string textBefore = null;
        int selectionStart = 0;
        int selectionLength = 0;

        //private void txtEndometriumThickness_TextChanged(object sender, RoutedEventArgs e)
        //{
        //    if (!((TextBox)sender).Text.IsValidDigintWithTwoDecimalPlaces() && textBefore != null)
        //    //if (!((TextBox)sender).Text.IsValidDigintWithOneDecimalPlaces() && textBefore != null)
        //    {
        //        if (textBefore != null)
        //        {
        //            ((TextBox)sender).Text = textBefore;
        //            ((TextBox)sender).SelectionStart = selectionStart;
        //            ((TextBox)sender).SelectionLength = selectionLength;
        //            textBefore = "";
        //            selectionStart = 0;
        //            selectionLength = 0;
        //        }
        //    }
        //}

        private void txtEndometriumThickness_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtEndometriumThickness.Text != string.Empty)
            {
                decimal value = Convert.ToDecimal(txtEndometriumThickness.Text);
                if (value > Convert.ToDecimal(25.9))
                {
                    txtEndometriumThickness.SetValidation("Please Enter Proper Value");
                    txtEndometriumThickness.RaiseValidationError();
                    txtEndometriumThickness.Focus();
                }
                else
                    txtEndometriumThickness.ClearValidationError();
            }
        }        
    }
}
