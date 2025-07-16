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
using PalashDynamics.ValueObjects;
using CIMS;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Browser;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;

namespace PalashDynamics.CRM
{
    public partial class ViewEmailTemplateDetails : UserControl
    {
        #region Public Variables
        public long TemplateId { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();
        byte[] data;
        public string fileName1;
        byte[] data1;
        public string fileName;

        #endregion

        public ViewEmailTemplateDetails()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           ShowEmailTemplateDetails(TemplateId);
           
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ((ChildWindow)(this.Parent)).Close();
            }
            catch (Exception ex)
            {
            }
        }

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
        ////    //get the attachment details
        ////  //  clsEmailTemplateVO objTemplate = CreateFormData();       
            
        ////    if (!string.IsNullOrEmpty(fileName1))
        ////    {
        ////        Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
        ////        DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
        ////        client.UploadETemplateFileCompleted += (s, args) =>
        ////        {
        ////            if (args.Error == null)
        ////            {
        ////                HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });// { ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL}); //.SelectedItem).SourceURL });
        ////                // listOfReports.Add(((clsPathOrderBookingDetailVO)dgTest.SelectedItem).SourceURL);
        ////            }
        ////        };
        ////        client.UploadETemplateFileAsync(txtFilePath.Text, data);
        ////        //client.UploadETemplateFileAsync(((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).SourceURL, ((clsEmailTemplateVO)dgEmailTemplateList.SelectedItem).Report);
        ////    }
        ////    else
        ////    {
        ////        //MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "This test's report is not uploaded. Please upload the report then click on preview", MessageBoxButtons.Ok, MessageBoxIcon.Information);
        ////        //mgbx.Show();
        ////    }
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
                        txtCode.Text = objTemp.Code;
                        txtName.Text = objTemp.Description;
                        txtSubject.Text = objTemp.Subject;
                        TextEditor.Html = objTemp.Text;
                        if (objTemp.AttachmentNos > 0)
                        {
                            dgEmailAttachmentList.ItemsSource = null;
                            dgEmailAttachmentList.ItemsSource = objTemp.AttachmentDetails;
                        }
                        //txtTemplateText.Text = objTemp.Text;
                        //if (objTemp.AttachmentDetails.AttachmentFileName != null)
                        //{
                        //    txtAttachments.Visibility = Visibility.Visible;
                        //    txtFilePath.Visibility = Visibility.Visible;
                        //    cmdView.Visibility = Visibility.Visible;
                        //    txtFilePath.Text = objTemp.AttachmentDetails.AttachmentFileName;
                        //    data = objTemp.AttachmentDetails.Attachment;
                        //    fileName = objTemp.AttachmentDetails.AttachmentFileName;                       
                        //}                        
                    }
                }
                waiting.Close();
            };
            client.ProcessAsync(obj, new clsUserVO());
            client.CloseAsync();
            client = null;
        }

        private void richTextEditor_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            fileName1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).AttachmentFileName;
            data1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).Attachment;

            if (!string.IsNullOrEmpty(fileName1))
            {
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
        }

        private void cmdDeleteItem_Click(object sender, RoutedEventArgs e)
        {

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

    }
}
