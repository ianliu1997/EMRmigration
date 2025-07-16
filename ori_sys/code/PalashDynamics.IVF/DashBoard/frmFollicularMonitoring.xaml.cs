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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.IVF.TherpyExecution;
using CIMS;
using PalashDynamics.Service.DataTemplateHttpsServiceRef;
using System.Windows.Browser;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmFollicularMonitoring : ChildWindow
    {
        System.Collections.ObjectModel.ObservableCollection<string> AttachedFileNameList = new System.Collections.ObjectModel.ObservableCollection<string>();
        public event RoutedEventHandler FollicularMonitoringAttachemntView_ChildClick;
        public List<clsFollicularMonitoring> FollicularMonitoringList = new List<clsFollicularMonitoring>();
      
        public frmFollicularMonitoring()
        {
            InitializeComponent();
        }

        private void FollicularMonitoringView_Click(object sender, RoutedEventArgs e)
        {
            if (FollicularMonitoringAttachemntView_ChildClick != null)
                FollicularMonitoringAttachemntView_ChildClick(this, new RoutedEventArgs());
           
        }

        private void FollicularMonitoringAttachemntView_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath))
            {
                if (((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentFileContent != null)
                {
                    Uri address = new Uri(Application.Current.Host.Source, "../EMR/DataTemplateHttpsService.svc"); // this url will work both in dev and after deploy
                    DataTemplateHttpsServiceClient client = new DataTemplateHttpsServiceClient("BasicHttpBinding_DataTemplateHttpsService", address.AbsoluteUri);
                    client.GlobalUploadFileCompleted += (s, args) =>
                    {
                        if (args.Error == null)
                        {
                            HtmlPage.Window.Invoke("GlobalOpenFile", new string[] { "UserUploadedFilesByTemplateTool/" + ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath });
                            AttachedFileNameList.Add(((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath);
                        }
                    };
                    client.GlobalUploadFileAsync("../UserUploadedFilesByTemplateTool", ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentPath, ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).AttachmentFileContent);
                }
            }
        }
       
        private void cmdOutComeCancel_Click(object sender, RoutedEventArgs e)
        {
           // Close();
            //Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True);
           // Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
            this.DialogResult = false;
            

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        public long TherapyId, TherapyUnitId;
        private void cmdPrintFM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                //if (dgFollicularMonitoring.ItemsSource != null)
                //{
                //    if (dgFollicularMonitoring.SelectedIndex == -1)
                //    {
                //        dgFollicularMonitoring.SelectedIndex = 1;
                //        if (dgFollicularMonitoring.SelectedItem != null)
                //        {
                //            TherapyId = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).TherapyId;
                //            TherapyUnitId = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).TherapyUnitId;
                //        }
                //    }
                //    else
                //    {
                //        TherapyId = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).TherapyId;
                //        TherapyUnitId = ((clsFollicularMonitoring)dgFollicularMonitoring.SelectedItem).TherapyUnitId;
                //    }
                //}
                //if (((IApplicationConfiguration)App.Current).SelectedCoupleDetails != null && (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).FemalePatient.PatientID > 0)
                //{
                    //long CoupleID = (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).CoupleId;
                    //long CoupleUnitID = (((IApplicationConfiguration)App.Current).SelectedCoupleDetails).CoupleUnitId;
                if (FollicularMonitoringList.Count == 0 && dgFollicularMonitoring.ItemsSource == null)
                {
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                               new MessageBoxControl.MessageBoxChildWindow("Palash", "Follicular monitoring details are not present", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
                else
                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/IVFDashboard/IVFDashboard_FollicularMonitoring.aspx?TherapyId=" + TherapyId + "&TherapyUnitId=" + TherapyUnitId ), "_blank");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
