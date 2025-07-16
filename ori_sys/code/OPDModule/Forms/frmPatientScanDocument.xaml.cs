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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using PalashDynamics.UserControls;
using MessageBoxControl;
namespace OPDModule.Forms
{
    public partial class frmPatientScanDocument : ChildWindow
    {
        byte[] AttachedFileContents;
        string AttachedFileName;
        public bool IsfromEdit;
        public long PatientType;
        WaitIndicator Indicatior;
        public event RoutedEventHandler OnSaveButton_Click;
        public List<clsPatientScanDocumentVO> ScanDoc = new List<clsPatientScanDocumentVO>();
        public frmPatientScanDocument()
        {
            InitializeComponent();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
          
            FillIdentity();
            fillDetails();

        }
        private void FillIdentity()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.M_IdentityMaster;
            BizAction.IsActive = true;
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
                    cmbIdentity.ItemsSource = null;
                    cmbIdentity.ItemsSource = objList;
                    cmbIdentity.SelectedItem = objList[0];
                }
            };
            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
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
                    throw ex;
                }
            }
        }

        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            if (validation())
            {
               Add();
               cmbIdentity.SelectedValue = (long)0;
                 txtIdentityNumber.Text = "";
                txtDescription.Text = "";
                txtFileName.Text = "";
            }
                   
            
        }
        private void Add()
        {
            clsPatientScanDocumentVO INV = new clsPatientScanDocumentVO();
            INV.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            INV.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            if (((MasterListItem)cmbIdentity.SelectedItem).ID != null)
            {
                INV.IdentityID = ((MasterListItem)cmbIdentity.SelectedItem).ID;
                INV.Identity = ((MasterListItem)cmbIdentity.SelectedItem).Description;
            }
            if (!String.IsNullOrEmpty(txtIdentityNumber.Text))
                INV.IdentityNumber = txtIdentityNumber.Text;
            INV.Description = txtDescription.Text;
            INV.AttachedFileName = txtFileName.Text;
            INV.AttachedFileContent = AttachedFileContents;
            INV.Status = true;
            
            if(IsView == false)
                ScanDoc.Add(INV);
            else
                ScanDoc[dgDocumentGrid.SelectedIndex] = INV;

            dgDocumentGrid.ItemsSource = null;
            dgDocumentGrid.ItemsSource = ScanDoc;
            dgDocumentGrid.UpdateLayout();
            dgDocumentGrid.Focus();
            IsView = false;
 
        }
        private bool validation()
        {
            bool result = true;
            if ((MasterListItem)cmbIdentity.SelectedItem == null)
            {
                cmbIdentity.TextBox.SetValidation("Please Select Identity");
                cmbIdentity.TextBox.RaiseValidationError();
                cmbIdentity.Focus();
               
                result = false;
            }
            else if (((MasterListItem)cmbIdentity.SelectedItem).ID == 0)
            {
                cmbIdentity.TextBox.SetValidation(" Please Select Identity");
                cmbIdentity.TextBox.RaiseValidationError();
                cmbIdentity.Focus();
                result = false;
            }
            else  if (txtIdentityNumber.Text.Length == 0)
            {
                cmbIdentity.TextBox.ClearValidationError();
                txtIdentityNumber.SetValidation("Please Enter Identity Number");
                txtIdentityNumber.RaiseValidationError();
                txtIdentityNumber.Focus();
                result = false;
            }
            else if (txtDescription.Text.Length == 0)
            {
                cmbIdentity.TextBox.ClearValidationError();
                txtIdentityNumber.ClearValidationError();
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else if (txtFileName.Text.Length == 0)
            {
                cmbIdentity.TextBox.ClearValidationError();
                txtIdentityNumber.ClearValidationError();
                txtDescription.ClearValidationError();
                txtFileName.SetValidation("Please Browse File");
                txtFileName.RaiseValidationError();
                txtFileName.Focus();
                result = false;
            }
            //else if (DocList.Count > 0)
            //{
            //    if (DocList.Where(Items => Items.Title == txtTitle.Text).Any() == true)
            //    {
            //        txtTitle.SetValidation("Title cannot be same");
            //        txtTitle.RaiseValidationError();
            //        txtTitle.Focus();
            //        result = false;
            //    }
            //    else if (DocList.Where(Items => Items.Description == txtDescription.Text).Any() == true)
            //    {
            //        txtTitle.ClearValidationError();
            //        txtDescription.SetValidation("Description cannot be same");
            //        txtDescription.RaiseValidationError();
            //        txtDescription.Focus();
            //        result = false;
            //    }
            //    else
            //    {
            //        txtTitle.ClearValidationError();
            //        txtDescription.ClearValidationError();
            //        result = true;
            //    }

            //}
            else
            {
                cmbIdentity.TextBox.ClearValidationError();
                txtIdentityNumber.ClearValidationError();
                txtDescription.ClearValidationError();
                txtFileName.ClearValidationError();
                result = true;

            }


            return result;
        }
        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
          
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdDeleteDoc_Click(object sender, RoutedEventArgs e)
        {

        }
        bool IsView = false;
        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocumentGrid.SelectedItem != null)
            {
                IsView = true;
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityNumber != null)
                    txtIdentityNumber.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityNumber;
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).Description != null)
                    txtDescription.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).Description;
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName != null)
                    txtFileName.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName;
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityID != null)
                    cmbIdentity.SelectedValue = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityID;
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                    AttachedFileContents = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent;
              

            }

        }
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {

            Indicatior = new WaitIndicator();
            Indicatior.Show();
            try
            {
                if (ScanDoc != null && ScanDoc.Count > 0)
                {
                    clsAddUpdatePatientScanDocument BizAction = new clsAddUpdatePatientScanDocument();
                    BizAction.PatientScanDocList = new List<clsPatientScanDocumentVO>();
                    BizAction.PatientScanDocList = ScanDoc;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Document is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                                this.DialogResult = true;
                                ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                            }
                        }
                    };
                    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }

                else
                {
                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one file is required.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    mgbx.Show();
                    Indicatior.Close();
                }
            }
            catch (Exception ex)
            {
                Indicatior.Close();
            }
            Indicatior.Close();
        }

        private void fillDetails() 
        {
            clsGetPatientScanDocument BizAction = new clsGetPatientScanDocument();
            BizAction.PatientScanDocList = new List<clsPatientScanDocumentVO>();
            BizAction.PatientScanDoc = new clsPatientScanDocumentVO();
            BizAction.PatientScanDoc.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
            BizAction.PatientScanDoc.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {

                        foreach (var item in ((clsGetPatientScanDocument)arg.Result).PatientScanDocList)
                        {
                            ScanDoc.Add(item);
                        }
                        dgDocumentGrid.ItemsSource = null;
                        dgDocumentGrid.ItemsSource = ScanDoc;
                        dgDocumentGrid.UpdateLayout();
                        dgDocumentGrid.Focus();
                    }
                }
            };
            Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client1.CloseAsync();
        }
      

      
    }
}
