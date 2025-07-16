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
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.CRM;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.EmailServiceReference;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Collections.ObjectModel;

namespace PalashDynamics.CRM
{
    public partial class EmailTemplate : ChildWindow
    {
        #region Public Variables
        public long TemplateId { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();
        byte[] data;
        public string fileName;
        public string msgTitle = "";
        public string msgText = "";
        public string[] PatientEmailId { get; set; }
        public long PatientCount { get; set; }
        public string PatEmailID { get; set; }
        public string EmailSubject { get; set; }
        public string EmailText { get; set; }
        public bool EmailFormat { get; set; }
        public string EmailFilePath { get; set; }
        public long EmailTemplateId { get; set; }
        public event RoutedEventHandler Ok_Click;
        public ObservableCollection<string> AttachmentFiles { get; set; }
        public ObservableCollection<clsEmailAttachmentVO> AddedFiles { get; set; }
        public long AttachmentNo { get; set; }
        public string fileName1;
        byte[] data1;
       
        #endregion

        public EmailTemplate()
        {
            InitializeComponent();

            AttachmentFiles = new ObservableCollection<string>();
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void cmbSelectTemplate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbSelectTemplate.SelectedItem != null)
            {
                ClearFormData();
                EmailTemplateId = ((MasterListItem)cmbSelectTemplate.SelectedItem).ID;
                ShowEmailTemplateDetails(EmailTemplateId);
            }           
        }

        private void ClearFormData()
        {
            txtSubject.Text = string.Empty;
            TextEditor.Html = string.Empty;
            AttachmentFiles = new ObservableCollection<string>();
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
            AttachmentFiles.Clear();
            AddedFiles.Clear();
            dgEmailAttachmentList.ItemsSource = null;
        }

        private void ShowEmailTemplateDetails(long EmailTemplateId)
        {
            waiting.Show();
            clsGetEmailTemplateBizActionVO obj = new clsGetEmailTemplateBizActionVO();
            obj.ID = EmailTemplateId;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    clsEmailTemplateVO objTemp = new clsEmailTemplateVO();
                    objTemp = ((clsGetEmailTemplateBizActionVO)ea.Result).EmailDetails;

                    if (objTemp.AttachmentDetails != null)
                    {
                        txtSubject.Text = objTemp.Subject;
                        TextEditor.Html = objTemp.Text;
                        EmailSubject = objTemp.Subject;
                        EmailFormat = objTemp.EmailFormat;
                        AttachmentNo = objTemp.AttachmentNos;
                        AttachmentFiles = new ObservableCollection<string>();
                        for (int i = 0; i < objTemp.AttachmentDetails.Count;i++ )
                        {
                            AttachmentFiles.Add(objTemp.AttachmentDetails[i].AttachmentFileName);
                            AddedFiles.Add(objTemp.AttachmentDetails[i]);
                        }                

                        if (objTemp.AttachmentNos > 0)
                        {
                            dgEmailAttachmentList.ItemsSource = null;
                            dgEmailAttachmentList.ItemsSource = objTemp.AttachmentDetails;
                        }
                       // txtTemplateText.Text = objTemp.Text;
                        EmailText = objTemp.Text;                                         
                    }
                }
                waiting.Close();
            };
            client.ProcessAsync(obj, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            //get the attachment details
            //  clsEmailTemplateVO objTemplate = CreateFormData();          
            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    try
            //    {
            //        HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
            //    }
            //    catch (Exception ex)
            //    {

            //        throw;
            //    }
            //}
            if (!string.IsNullOrEmpty(fileName))
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
               // DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                client.UploadETemplateFileCompleted += (s, args) =>
                {
                    if (args.Error == null)
                    {
                        HtmlPage.Window.Invoke("openAttachment", new string[] {""});//txtFilePath.Text });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
                        // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                    }
                };
                client.UploadETemplateFileAsync("", data); //txtFilePath.Text, data);
                //client.UploadETemplateFileAsync(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL, ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Report);
            }
            else
            {
                //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                //mgbx.Show();
            }
        }

        private void FillEmail()
        {
            clsGetMasterListBizActionVO BizAction = new clsGetMasterListBizActionVO();
            BizAction.MasterTable = MasterTableNameList.T_EmailTemplate;

            BizAction.MasterList = new List<MasterListItem>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null)
                {
                    List<MasterListItem> objList = new List<MasterListItem>();
                    objList.Add(new MasterListItem(0, "-- Select --"));
                    objList.AddRange(((clsGetMasterListBizActionVO)arg.Result).MasterList);

                    cmbSelectTemplate.ItemsSource = null;
                    cmbSelectTemplate.ItemsSource = objList;
                }
               
                if (this.DataContext != null)
                {
                    cmbSelectTemplate.SelectedValue = ((clsCampMasterVO)this.DataContext).EmailTemplateDetails.Description;
                    EmailTemplateId = ((clsCampMasterVO)this.DataContext).EmailTemplateDetails.ID;
                    dgEmailAttachmentList.ItemsSource = null;
                    dgEmailAttachmentList.ItemsSource = ((clsCampMasterVO)this.DataContext).EmailTemplateDetails.AttachmentDetails;
                }

                if ((clsEmailTemplateVO)cmbSelectTemplate.SelectedItem != null)
                {
                    clsEmailTemplateVO objCampMaster = new clsEmailTemplateVO();
                    cmbSelectTemplate.SelectedValue = objCampMaster.Description;
                }
            };
            client.ProcessAsync(BizAction, new clsUserVO());
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillEmail();
        }

        private void cmdSend_Click(object sender, RoutedEventArgs e)
        {
             msgText = "Are you sure you want to send the Email?";
             MessageBoxControl.MessageBoxChildWindow msgW =
                   new MessageBoxControl.MessageBoxChildWindow(msgTitle, msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);

             msgW.OnMessageBoxClosed += (MessageBoxResult res) =>
             {
                 if (res == MessageBoxResult.Yes)
                 {
                     this.DialogResult = true;

                     if (Ok_Click != null)
                         Ok_Click(this, new RoutedEventArgs());
                 }
             };
             msgW.Show();
            //for (int Emailcount = 0; Emailcount < PatientCount; Emailcount++ )
            //{
            //   // string PatientEmailIdSend = PatientEmailId[Emailcount].Email;
            //waiting.Show();
            //    Uri address1 = new Uri(Application.Current.Host.Source, "../EmailService.svc"); // this url will work both in dev and after deploy
            //    EmailServiceClient EmailClient = new EmailServiceClient("CustomBinding_EmailService", address1.AbsoluteUri);
            //    EmailClient.SendEmailwithAttachmentCompleted += (ea, args) =>
            //    {
            //        if (args.Error == null)
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Email Sent Successfully .", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

            //            msgW1.Show();
            //            waiting.Close();
            //        }
            //        else
            //        {
            //            MessageBoxControl.MessageBoxChildWindow msgW1 =
            //            new MessageBoxControl.MessageBoxChildWindow(msgTitle, "Error Occured while sending the message.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);

            //            msgW1.Show();
            //            waiting.Close();
            //        }
            //    };
            //    EmailClient.SendEmailwithAttachmentAsync("sailyp@seedhealthcare.com", PatientEmailId[Emailcount], txtSubject.Text, txtTemplateText.Text, txtFilePath.Text);
            //   // EmailClient.SendEmailwithAttachmentAsync("sailyp@seedhealthcare.com", PatEmailID, txtSubject.Text, txtTemplateText.Text, txtFilePath.Text);
            //    EmailClient.CloseAsync();
            //    EmailClient.CloseAsync();
            //    EmailClient.CloseAsync();
            //}
            }

        /// <summary>
        /// gets richtext editor
        /// </summary>
        public Liquid.RichTextEditor TextEditor
        {
            get { return richTextEditor; }
        }

        /// <summary>
        /// gets richtext box
        /// </summary>
        public Liquid.RichTextBox rt
        {
            get { return richTextEditor.TextBox; }
        }

       

        private void dgEmailTemplateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dgEmailTemplateList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

        }

        private void dgEmailTemplateList_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            //get the attachment details
            // clsEmailTemplateVO objTemplate = CreateFormData();
            //ShowEmailTemplateDetails()
            fileName1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).AttachmentFileName;
            data1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).Attachment;
            // if (!string.IsNullOrEmpty(txtFilePath.Text.Trim()))
            ////if (IsNew == true)
            ////{
            ////    if (!string.IsNullOrEmpty(fileName1))
            ////    {
            ////        // HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
            ////        HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
            ////    }
            ////}
            ////else
            ////{
                //if (((clsEmailTemplateVO)dgTest.SelectedItem).IsCompleted == true)
                if (!string.IsNullOrEmpty(fileName1))
                {
                   // Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    //DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);

                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.UploadETemplateFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("openAttachment", new string[] { fileName1 });
                            // HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
                            // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
                        }
                    };
                    client.UploadETemplateFileAsync(fileName1, data1);
                    // client.UploadETemplateFileAsync(txtFilePath.Text, data);
                    //client.UploadETemplateFileAsync(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL, ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Report);
                }
                else
                {
                    //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                    //mgbx.Show();
                }
            ////}
        }

        private void richTextEditor_LostFocus(object sender, RoutedEventArgs e)
        {
            EmailText = TextEditor.Html;
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddedFiles.RemoveAt(dgEmailAttachmentList.SelectedIndex);
                //AttachmentList.RemoveAt(dgEmailAttachmentList.SelectedIndex);
                //AttachmentFiles
                dgEmailAttachmentList.ItemsSource = null;
                dgEmailAttachmentList.ItemsSource = AddedFiles;
                dgEmailAttachmentList.UpdateLayout();
                AttachmentFiles = new ObservableCollection<string>();
                for (int i = 0; i < AddedFiles.Count; i++)
                {
                    AttachmentFiles.Add(AddedFiles[i].AttachmentFileName);
                }
                AttachmentNo = AttachmentNo - 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

