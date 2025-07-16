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
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.IO;
using MessageBoxControl;
using PalashDynamics.UserControls;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.Reflection;
using System.Xml.Linq;
using System.Windows.Resources;

namespace PalashDynamics.Forms.PatientView
{
    public partial class frmPatientScanDocument : ChildWindow,  IInitiateCIMS
    {
        byte[] AttachedFileContents;
        string AttachedFileName;
        public bool IsfromEdit;
        public bool IsSurrogacy;
        public bool IsPatientList;
        public long PatientType;
        int ClickedFlag = 0;
        WaitIndicator Indicatior;
        public event RoutedEventHandler OnSaveButton_Click;
        public List<clsPatientScanDocumentVO> ScanDoc = new List<clsPatientScanDocumentVO>();
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
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
                validation();
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
            if (((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID > 0)
            {
                INV.DoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID;                
            }
            else
            {
                INV.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                INV.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            }
            
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
            if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem) != null)
            {
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).ID != null && ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).ID > 0)
                {
                    INV.ID = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).ID;
                }
            }

            if (IsView == false)
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
            //if (txtIdentityNumber.Text.Trim().Length == 0)
            //{
            //    txtIdentityNumber.SetValidation("Please Enter Identity Number");
            //    txtIdentityNumber.RaiseValidationError();
            //    txtIdentityNumber.Focus();
            //    result = false;
            //}
            //else
            //    txtIdentityNumber.ClearValidationError();

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
            else
                cmbIdentity.TextBox.ClearValidationError();

            //if (txtDescription.Text.Trim().Length == 0)
            //{
            //    //cmbIdentity.TextBox.ClearValidationError();
            //    //txtIdentityNumber.ClearValidationError();
            //    txtDescription.SetValidation("Please Enter Description");
            //    txtDescription.RaiseValidationError();
            //    txtDescription.Focus();
            //    result = false;
            //}
            //else
            //    txtDescription.ClearValidationError();
            if (txtFileName.Text.Length == 0)
            {
                //cmbIdentity.TextBox.ClearValidationError();
                //txtIdentityNumber.ClearValidationError();
                //txtDescription.ClearValidationError();
                txtFileName.SetValidation("Please Browse File");
                txtFileName.RaiseValidationError();
                txtFileName.Focus();
                result = false;
            }
            else
                txtFileName.ClearValidationError();
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
            //else
            //{
            //    cmbIdentity.TextBox.ClearValidationError();
            //    txtIdentityNumber.ClearValidationError();
            //    txtDescription.ClearValidationError();
            //    txtFileName.ClearValidationError();
            //    result = true;

            //}


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
            if (!string.IsNullOrEmpty(((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }

                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).ImageName != null)
                {
                    HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).ImageName });
                }
                
            }
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

                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityID != null)
                {
                    cmbIdentity.SelectedValue = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityID;
                }

                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityNumber != "")
                {
                    txtIdentityNumber.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).IdentityNumber;
                }
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).Description != "")
                {
                    txtDescription.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).Description;
                }              
               
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    AttachedFileContents = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileContent;
                }
                if (((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName != "")
                {
                    txtFileName.Text = ((clsPatientScanDocumentVO)dgDocumentGrid.SelectedItem).AttachedFileName;
                }
               


            }

        }

        public string ModuleName { get; set; }
        public string Action { get; set; }
        UIElement myData = null;

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            Indicatior = new WaitIndicator();
            Indicatior.Show();
             ClickedFlag += 1;

             if (ClickedFlag == 1)
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
                                     if (IsSurrogacy == true && IsPatientList == false)
                                     {
                                         MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", " Surrogacy Document is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                         mgbx.Show();
                                         this.DialogResult = true;
                                         ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                         ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Forms.PatientView.PatientListForSurrogacy");
                                     }
                                     else if (IsSurrogacy == false && IsPatientList == true)
                                     {
                                         MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Document is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                         mgbx.Show();
                                         this.DialogResult = true;
                                         ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                         ((IApplicationConfiguration)App.Current).OpenMainContent("CIMS.Forms.PatientList");
                                     }
                                     else
                                     {
                                         MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Doctor Document is Uploaded Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                         mgbx.Show();
                                         this.DialogResult = true;
                                         ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();                                                                       

                                         ModuleName = "PalashDynamics.Administration";
                                         Action = "PalashDynamics.Administration.DoctorMaster";
                                         UserControl rootPage = Application.Current.RootVisual as UserControl;

                                         WebClient c2 = new WebClient();
                                         c2.OpenReadCompleted += new OpenReadCompletedEventHandler(c2_OpenReadCompleted);
                                         c2.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));                                        

                                     }

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
                         ClickedFlag = 0;
                     }                                  
             }
             
            Indicatior.Close();
        }


        void c2_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IInitiateCIMS)myData).Initiate("PRO");
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void fillDetails()
        {
            clsGetPatientScanDocument BizAction = new clsGetPatientScanDocument();
            BizAction.PatientScanDocList = new List<clsPatientScanDocumentVO>();
            BizAction.PatientScanDoc = new clsPatientScanDocumentVO();
            if (((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID > 0)
            {
                BizAction.PatientScanDoc.DoctorID = ((IApplicationConfiguration)App.Current).SelectedPatient.DoctorID;
            }
            else
            {
                BizAction.PatientScanDoc.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.PatientScanDoc.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            }
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



        private void cmbIdentity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtIdentityNumber.Text = "";
            txtDescription.Text = "";
            txtFileName.Text = "";
        }




        public void Initiate(string Mode)
        {
            throw new NotImplementedException();
        }
    }
}
