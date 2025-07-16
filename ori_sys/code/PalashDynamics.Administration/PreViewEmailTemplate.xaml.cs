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
using System.Windows.Browser;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Collections.ObjectModel;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;


namespace PalashDynamics.Administration
{
    public partial class PreViewEmailTemplate : UserControl
    {
        #region Public Variables
        public long TemplateId { get; set; }
        clsUserVO User = ((IApplicationConfiguration)App.Current).CurrentUser;
        WaitIndicator waiting = new WaitIndicator();
        byte[] data;
        public string fileName;
        long FileAttachmentNo = 0;
        public ObservableCollection<clsEmailAttachmentVO> AddedFiles { get; set; }
        public List<clsEmailAttachmentVO> AttachmentList { get; set; }
        public ObservableCollection<string> AttachmentFiles { get; set; }
        byte[] data1;
        public string fileName1;

        #endregion

        public PreViewEmailTemplate()
        {
            InitializeComponent();
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
            AttachmentList = new List<clsEmailAttachmentVO>();
            AttachmentFiles = new ObservableCollection<string>();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AddedFiles = new ObservableCollection<clsEmailAttachmentVO>();
            AttachmentFiles = new ObservableCollection<string>();
            dgEmailAttachmentList.ItemsSource = AddedFiles;
            AttachmentList = new List<clsEmailAttachmentVO>();

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

        //private void cmdView_Click(object sender, RoutedEventArgs e)
        //{
        //    //get the attachment details
        //    //  clsEmailTemplateVO objTemplate = CreateFormData();          
        //    if (!string.IsNullOrEmpty(fileName))
        //    {
        //        try
        //        {
        //            HtmlPage.Window.Invoke("openAttachment", new string[] { txtFilePath.Text });
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //        }
        //    }
        //}

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
                        FileAttachmentNo = objTemp.AttachmentNos;
                        if (objTemp.AttachmentNos > 0)
                        {
                            dgEmailAttachmentList.ItemsSource = null;
                            dgEmailAttachmentList.ItemsSource = objTemp.AttachmentDetails;
                            AddedFiles.Clear();
                            AttachmentList.Clear();
                            AttachmentFiles = new ObservableCollection<string>();
                            for (int i = 0; i < objTemp.AttachmentNos; i++)
                            {
                                AddedFiles.Add(objTemp.AttachmentDetails[i]);
                                AttachmentList.Add(objTemp.AttachmentDetails[i]);
                                AttachmentFiles.Add(objTemp.AttachmentDetails[i].AttachmentFileName);
                            }
                        }
                        else
                            dgEmailAttachmentList.ItemsSource = null;
                        //txtTemplateText.Text = objTemp.Text;
                        //if (objTemp.AttachmentDetails.AttachmentFileName != null)
                        //{
                        //    txtAttachments.Visibility = Visibility.Visible;
                        //    txtFilePath.Visibility = Visibility.Visible;
                        //    //cmdView.Visibility = Visibility.Visible;
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
      
        private void hlbView_Click(object sender, RoutedEventArgs e)
        {
            fileName1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).AttachmentFileName;
            data1 = ((clsEmailAttachmentVO)dgEmailAttachmentList.SelectedItem).Attachment;
            //if (((clsEmailTemplateVO)dgTest.SelectedItem).IsCompleted == true)
            if (!string.IsNullOrEmpty(fileName1))
            {
                //Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
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
            }
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
