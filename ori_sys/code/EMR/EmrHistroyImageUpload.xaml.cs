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
using System.IO;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.UserControls;

namespace EMR
{
    public partial class EmrHistroyImageUpload : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        public long PatientID { get; set; }
        public long VisitID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public Boolean ISOPDIPD { get; set; }
        public List<ListItems1> mylistitem = new List<ListItems1>();
        public List<ListItems1> GetPreviousPhotos { get; set; }

        public EmrHistroyImageUpload()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(EmrHistroyImageUpload_Loaded);
        }
        private void EmrHistroyImageUpload_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ISOPDIPD)
            {
                GetPAtientImage();
                cmdOK.Visibility = Visibility.Collapsed;
                dgImgListIPD.Visibility = Visibility.Collapsed;
                dgImgList.Visibility = Visibility.Visible;
            }
            else
            {
                dgImgListIPD.Visibility = Visibility.Visible;
                dgImgList.Visibility = Visibility.Collapsed;
                cmdOK.Visibility = Visibility.Visible;
                if (GetPreviousPhotos != null)
                {
                    mylistitem = GetPreviousPhotos;
                    dgImgListIPD.ItemsSource = null;
                    dgImgListIPD.ItemsSource = mylistitem;
                }
            }
        }
        private void GetPAtientImage()
        {
            try
            {
                clsGetPatientUploadedImagetHystoLapBizActionVO BizAction = new clsGetPatientUploadedImagetHystoLapBizActionVO();
                BizAction.PatientID = PatientID;
                BizAction.VisitID = VisitID;
                BizAction.TemplateID = TemplateID;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient clientBizActionObjPatientHistoryData = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientBizActionObjPatientHistoryData.ProcessCompleted += (sBizActionObjPatientHistoryData, argsBizActionObjPatientHistoryData) =>
                {
                    if (argsBizActionObjPatientHistoryData.Error == null)
                    {
                        if (argsBizActionObjPatientHistoryData.Result != null)
                        {
                            if (((clsGetPatientUploadedImagetHystoLapBizActionVO)argsBizActionObjPatientHistoryData.Result).Img1 != null)
                            {
                                ((clsGetPatientUploadedImagetHystoLapBizActionVO)argsBizActionObjPatientHistoryData.Result).isOPD = "Visible";
                                ((clsGetPatientUploadedImagetHystoLapBizActionVO)argsBizActionObjPatientHistoryData.Result).ISIPD = "Collapsed";
                                dgImgList.ItemsSource = null;
                                dgImgList.ItemsSource = ((clsGetPatientUploadedImagetHystoLapBizActionVO)argsBizActionObjPatientHistoryData.Result).Img1;
                            }
                        }
                    }
                };
                clientBizActionObjPatientHistoryData.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientBizActionObjPatientHistoryData.CloseAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        byte[] MyPhoto { get; set; }

        private void CmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Multiselect = false;
            openDialog.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            openDialog.FilterIndex = 1;
            if (openDialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = openDialog.File.OpenRead())
                    {
                        MyPhoto = new byte[stream.Length];
                        stream.Read(MyPhoto, 0, (int)stream.Length);
                        if (ISOPDIPD)
                        {
                            ShowPhoto(MyPhoto);
                        }
                        else
                        {
                            Save();
                        }
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
        private void Save()
        {
            WaitIndicator IndicatiorDiag = new WaitIndicator();
            IndicatiorDiag.Show();
            try
            {
                clsUploadPatientHystoLapBizActionVO BizAction = new clsUploadPatientHystoLapBizActionVO();
                BizAction.Image = MyPhoto;
                BizAction.PatientID = PatientID;
                BizAction.VisitID = VisitID;
                BizAction.TemplateID = TemplateID;
                BizAction.IsOPDIPD = ISOPDIPD;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        string strErrorMsg = "Image Saved Successfully....";
                        ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        GetPAtientImage();
                        IndicatiorDiag.Close();
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
              //  IndicatiorDiag.Close();
            }
            catch (Exception ex)
            {
                IndicatiorDiag.Close();
            }
        }

        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
            if (ISOPDIPD)
            {
                mylistitem.Remove((ListItems1)dgImgListIPD.SelectedItem);
                dgImgListIPD.ItemsSource = null;
                dgImgListIPD.ItemsSource = mylistitem;
            }
            else
            {
                long GetID = ((ClsImages)dgImgList.SelectedItem).ImageID;
                clsDeleteUploadPatientHystoLapBizActionVO BizAction = new clsDeleteUploadPatientHystoLapBizActionVO();
                BizAction.ImageID = GetID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsDeleteUploadPatientHystoLapBizActionVO)args.Result).SuccessStatus == 1)
                        {
                            string strErrorMsg = "Image Delete Successfully....";
                            ShowMessageBox(strErrorMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            GetPAtientImage();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
        }

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

       
        private void ShowPhoto(byte[] MyPhoto)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource(new MemoryStream(MyPhoto, false));
            ListItems1 obj = new ListItems1();
            obj.ImageName1 = MyPhoto;
            obj.Photo = MyPhoto;
            obj.ISIPD = "Visible";
            mylistitem.Add(obj);
            dgImgListIPD.ItemsSource = null;
            dgImgListIPD.ItemsSource = mylistitem;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

