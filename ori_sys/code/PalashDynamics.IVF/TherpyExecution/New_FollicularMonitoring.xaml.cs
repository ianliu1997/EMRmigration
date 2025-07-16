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
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;
using System.IO;

namespace PalashDynamics.IVF.TherpyExecution
{
    public partial class New_FollicularMonitoring : ChildWindow
    {

        #region Variables
        public event RoutedEventHandler OnSaveButton_Click;
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();

        #endregion

        #region Properties

        public byte[] AttachedFileContents { get; set; }

        #endregion

        #region Constructor

        public New_FollicularMonitoring()
        {
            InitializeComponent();
            this.Unloaded += new RoutedEventHandler(FollicularMonitoring_Unloaded);
        }
         #endregion

        #region Unloaded Event
        void FollicularMonitoring_Unloaded(object sender, RoutedEventArgs e)
        {
            Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
            DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
            client.GlobalDeleteFileCompleted += (s1, args1) =>
            {
                if (args1.Error == null)
                {

                }
            };
            client.GlobalDeleteFileAsync("../UserUploadedFilesByTemplateTool", AttachedFileNameList);
        }
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

        #region Save / Cancel Button
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        #endregion

        #region File View Event

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

        #endregion

        #region File Browse
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
        #endregion

       
      
    }
}

