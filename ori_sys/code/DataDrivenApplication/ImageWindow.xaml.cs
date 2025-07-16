using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Patient;
using System.IO;
using PalashDynamics.Service.PalashTestServiceReference;
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using DataDrivenApplication;

namespace PalashDynamics.Administration
{
    public partial class ImageWindow : ChildWindow
    {
        public ImageWindow()
        {
            InitializeComponent();
        }

        public long TemplateID = 0;
        ObservableCollection<clsPatientLinkFileBizActionVO> lstFile { get; set; }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        byte[] data;
        FileInfo fi;
        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                        fi = openDialog.File;
                        ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                        ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL = fi.Extension;
                        ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report = data;
                        ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).TemplateID = TemplateID;
                        ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).DocumentName = fi.Name;
                    }
                }
                catch (Exception ex)
                {

                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgWindow.Show();

                }
            }
        }

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            clsPatientLinkFileBizActionVO Obj = new clsPatientLinkFileBizActionVO();
            lstFile.Add(Obj);
            dgReport.ItemsSource = lstFile;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (lstFile != null && lstFile.Count > 0)
            {
                clsAddPatientLinkFileBizActionVO BizAction = new clsAddPatientLinkFileBizActionVO();
                BizAction.PatientDetails = new List<clsPatientLinkFileBizActionVO>();
                BizAction.PatientDetails = lstFile.ToList();
                BizAction.FROMEMR = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                  {
                      if (arg.Error == null)
                      {
                          if (arg.Result != null)
                          {
                              MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Image Added Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                              mgbx.Show();
                              this.DialogResult = true;
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
            }

        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        private void ViewLink_Click(object sender, RoutedEventArgs e)
        {
            byte[] ReportDate = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report;
            string sourURL = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL;
            Uri address1 = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc");
            DataTemplateHttpsServiceClient client1 = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address1.AbsoluteUri);

            client1.GlobalUploadFileCompleted += (s1, args) =>
            {
                if (args.Error == null)
                {
                    HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + sourURL });
                    AttachedFileNameList.Add(sourURL);
                }
            };
            client1.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", sourURL, ReportDate);

        }



        private void GetData()
        {
            clsGetPatientLinkFileViewDetailsBizActionVO BizAction = new clsGetPatientLinkFileViewDetailsBizActionVO();
            BizAction.TemplateID = TemplateID;
            BizAction.FROMEMR = true;
            lstFile = new ObservableCollection<clsPatientLinkFileBizActionVO>();
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if ((((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).PatientDetails) != null)
                    {
                        foreach (var item in (((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).PatientDetails))
                        {
                            lstFile.Add(item);
                        }
                        dgReport.ItemsSource = lstFile;
                    }
                    else
                    {
                        lstFile = new ObservableCollection<clsPatientLinkFileBizActionVO>();
                        clsPatientLinkFileBizActionVO Obj = new clsPatientLinkFileBizActionVO();
                        lstFile.Add(Obj);
                        dgReport.ItemsSource = lstFile;
                    }
                }
                else
                {
                    MessageBoxControl.MessageBoxChildWindow msgWindow =
                        new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while reading file.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    msgWindow.Show();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }
        private void dgReport_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {

        }

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgReport.SelectedItem != null)
            {
                string msgText = "Are you sure you want to Delete the selected Image ?";
                MessageBoxControl.MessageBoxChildWindow msgWD =
                    new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                msgWD.OnMessageBoxClosed += (MessageBoxResult res) =>
                {
                    if (res == MessageBoxResult.Yes)
                    {

                        lstFile.RemoveAt(dgReport.SelectedIndex);
                        dgReport.ItemsSource = null;
                        dgReport.ItemsSource = lstFile;
                        dgReport.UpdateLayout();
                        dgReport.Focus();
                        MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Record Deleted Successfully", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        msgbx.Show();
                    }
                };
                msgWD.Show();
            }
        }
    }
}

