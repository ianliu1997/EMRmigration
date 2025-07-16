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
using System.IO;
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Inventory.Quotation;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
//using System.Windows.Media.Imaging.WriteableBitmap;

namespace PalashDynamics.Pharmacy
{
    public partial class QuotationAttachment : ChildWindow
    {
        byte[] AttachedFileContents;
        string AttachedFileName;
        bool IsView = false;
        public long QuotationID = 0;
        public long LeadTime = 0;
        public DateTime QuotationValidity;
        public List<clsQuotationAttachmentsVO> ScanDoc = new List<clsQuotationAttachmentsVO>();
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        WaitIndicator Indicatior;
        public event RoutedEventHandler OnSaveButton_Click;

        public QuotationAttachment()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //FillIdentity();
            fillDetails();
            validation();
            CmdSave.IsEnabled = false;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void cmdAttachedDoc_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0;
            ClickedFlag1 = 0;
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                AttachedFileName = openDialog.File.Name;
                txtFileName.Text = openDialog.File.Name;
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        if ((int)stream.Length <= 1048576)
                        {
                            AttachedFileContents = new byte[stream.Length];
                            stream.Read(AttachedFileContents, 0, (int)stream.Length);
                        }
                        else
                        {
                            string msgText = "Attachment size should be not greater than 1 MB.";

                            MessageBoxControl.MessageBoxChildWindow msgWindow =
                                new MessageBoxControl.MessageBoxChildWindow("Palsh", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgWindow.Show();
                        }
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

        int ClickedFlag = 0;
        private void CmdAddDocument_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag++;
            if (ClickedFlag == 1)
            {
                if (validation())
                {
                    Add();
                    //   cmbIdentity.SelectedValue = (long)0;
                    //   txtIdentityNumber.Text = "";
                    txtDescription.Text = "";
                    txtFileName.Text = "";
                  //  ClickedFlag = 0;
                    CmdSave.IsEnabled = true;
                }
            }
        }

        private void Add()
        {
            //MemoryStream ms = new MemoryStream(AttachedFileContents);
            //WriteableBitmap bit =GetImageSource(ms, 150, 150);
            //byte[] com = ConvertToByteArray(bit);

            //if (validation())
            //{
                clsQuotationAttachmentsVO INV = new clsQuotationAttachmentsVO();
                INV.QuotationID = QuotationID;
                INV.ValidityDate = QuotationValidity;
                INV.Description = txtDescription.Text;
                INV.AttachedFileName = txtFileName.Text;
                INV.AttachedFileContent = AttachedFileContents;//com;
                INV.Status = true;

                if (IsView == false)
                    ScanDoc.Add(INV);
                else
                    ScanDoc[dgDocumentGrid.SelectedIndex] = INV;

                dgDocumentGrid.ItemsSource = null;
                dgDocumentGrid.ItemsSource = ScanDoc;
                dgDocumentGrid.UpdateLayout();
                dgDocumentGrid.Focus();
                IsView = false;
           // }
        }

        private bool validation()
        {
            bool result = true;
            if (txtDescription.Text.Trim().Length == 0)
            {
                //cmbIdentity.TextBox.ClearValidationError();
                //txtIdentityNumber.ClearValidationError();
                txtDescription.SetValidation("Please Enter Description");
                txtDescription.RaiseValidationError();
                txtDescription.Focus();
                result = false;
            }
            else
                txtDescription.ClearValidationError();
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
            if (ScanDoc.Count > 0)
            {
                if (ScanDoc.Where(Items => Items.AttachedFileName == txtFileName.Text).Any() == true)
                {
                    txtFileName.SetValidation("File Name Can Not Be Same");
                    txtFileName.RaiseValidationError();
                    txtFileName.Focus();
                    result = false;
                }
                else if (ScanDoc.Where(Items => Items.Description == txtDescription.Text).Any() == true)
                {
                    txtFileName.ClearValidationError();
                    txtDescription.SetValidation("Description Can Not Be Same");
                    txtDescription.RaiseValidationError();
                    txtDescription.Focus();
                    result = false;
                }
                else if (ScanDoc.Count >= 3)
                {
                    MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("", "You Can Attached Only Three Attachments.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    result = false;
                    msgWD.Show();
                }
            }
            //else
            //{
            //    txtFileName.ClearValidationError();
            //    txtDescription.ClearValidationError();
            //   // result = true;
            //}


            return result;
        }

        private void hpyrlinkFileView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName))
            {
                if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                {
                    //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName });
                            AttachedFileNameList.Add(((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName, ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent);
                }
            }
        }

        int ClickedFlag1 = 0;
        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag1++;
           
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                try
                {
                    //if (validation())
                    //{
                    if (ScanDoc != null && ScanDoc.Count > 0)
                    {
                        if (ClickedFlag1 == 1)
                        {
                            clsAddQuotationAttachmentBizActionVO BizAction = new clsAddQuotationAttachmentBizActionVO();
                            BizAction.AttachmentList = new List<clsQuotationAttachmentsVO>();
                            BizAction.AttachmentList = ScanDoc;

                            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                            client.ProcessCompleted += (s, arg) =>
                            {
                                if (arg.Error == null)
                                {
                                    if (arg.Result != null)
                                    {
                                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Attachment Saved Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                        mgbx.Show();
                                        this.DialogResult = true;
                                        CmdSave.IsEnabled = false;
                                        //  ((IApplicationConfiguration)App.Current).SelectedPatient = new clsPatientGeneralVO();
                                        ((IApplicationConfiguration)App.Current).OpenMainContent("PalashDynamics.Pharmacy.QuotationAttachment");
                                    }
                                }
                            };
                            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                            client.CloseAsync();
                        }
                    }

                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Atleast one file is required.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbx.Show();
                        Indicatior.Close();
                    }
                }
                //  }
                catch (Exception ex)
                {
                    Indicatior.Close();
                }
                Indicatior.Close();
            
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocumentGrid.SelectedItem != null)
            {
                IsView = true;
                //if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).IdentityNumber != null)
                //    txtIdentityNumber.Text = ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).IdentityNumber;
                if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).Description != null)
                    txtDescription.Text = ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).Description;
                if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName != null)
                    txtFileName.Text = ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileName;
                //if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).IdentityID != null)
                //    cmbIdentity.SelectedValue = ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).IdentityID;
                if (((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent != null)
                    AttachedFileContents = ((clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem).AttachedFileContent;
            }

        }

        private void fillDetails()
        {
            clsGetQuotationAttachmentBizActionVO BizAction = new clsGetQuotationAttachmentBizActionVO();
            BizAction.AttachmentList = new List<clsQuotationAttachmentsVO>();
            BizAction.Quotation = new clsQuotaionVO();
            BizAction.QuotationID = QuotationID;
            //     BizAction.Quotation.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);

            Client1.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {

                        foreach (var item in ((clsGetQuotationAttachmentBizActionVO)arg.Result).AttachmentList)
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

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDocumentGrid.SelectedItem != null)
            {
                    string msgTitle = "";
                    string msgText = "Are You Sure You Want To Delete Attachment ?";

                    MessageBoxControl.MessageBoxChildWindow msgWD =
                        new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

                    msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                    {
                        if (res == MessageBoxResult.Yes)
                        {
                            clsQuotationAttachmentsVO objVO = (clsQuotationAttachmentsVO)dgDocumentGrid.SelectedItem;
                            clsQuotationAttachmentsVO obj;
                            if (objVO != null)
                            {
                                if (objVO.ID > 0)
                                {
                                    clsDeleteQuotationAttachmentBizActionVO BizAction = new clsDeleteQuotationAttachmentBizActionVO();
                                    BizAction.QuotationID = QuotationID;
                                    BizAction.ID = objVO.ID;
                                    Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                                    PalashServiceClient Client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                                    Client1.ProcessCompleted += (s, arg) =>
                                    {
                                        if (arg.Error == null)
                                        {
                                            if (arg.Result != null)
                                            {
                                                obj = ScanDoc.Where(z => z.AttachedFileName == objVO.AttachedFileName).FirstOrDefault();
                                                ScanDoc.Remove(obj);
                                            }
                                        }
                                    };
                                    Client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                                    Client1.CloseAsync();
                                }
                                else
                                {
                                    obj = ScanDoc.Where(z => z.AttachedFileName == objVO.AttachedFileName).FirstOrDefault();
                                    ScanDoc.Remove(obj);
                                }
                            }
                            dgDocumentGrid.Focus();
                            dgDocumentGrid.UpdateLayout();
                            dgDocumentGrid.SelectedIndex = ScanDoc.Count - 1;
                        }
                    };
                    msgWD.Show();
            }
        }

        public static WriteableBitmap GetImageSource(Stream stream, double maxWidth, double maxHeight)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.SetSource(stream);

            Image img = new Image();
            img.Effect = new DropShadowEffect() { ShadowDepth = 0, BlurRadius = 0 };
            img.Source = bmp;

            double scaleX = 1;
            double scaleY = 1;

            if (bmp.PixelHeight > maxHeight)
                scaleY = maxHeight / bmp.PixelHeight;
            if (bmp.PixelWidth > maxWidth)
                scaleX = maxWidth / bmp.PixelWidth;

            // maintain aspect ratio by picking the most severe scale
            double scale = Math.Min(scaleY, scaleX);

            return new WriteableBitmap(img, new ScaleTransform() { ScaleX = scale, ScaleY = scale });
        }

        public static byte[] ConvertToByteArray(WriteableBitmap writeableBitmap)
        {
            using (var ms = new MemoryStream())
            {
               return writeableBitmap.ToByteArray();//.SaveJpeg(ms, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
              //  return ms.ToArray();
            }
        }
    }
}

