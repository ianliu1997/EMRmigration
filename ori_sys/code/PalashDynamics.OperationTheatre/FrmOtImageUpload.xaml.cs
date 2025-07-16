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

namespace PalashDynamics.OperationTheatre
{
    public partial class FrmOtImageUpload : ChildWindow
    {
        public event RoutedEventHandler OnAddButton_Click;
        public long ScheduleID { get; set; }
        public long UnitID { get; set; }
        public long TemplateID { get; set; }
        public List<ListItems1> GetPreviousPhotos { get; set; }
        public FrmOtImageUpload()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(EmrHistroyImageUpload_Loaded);
        }
        private void EmrHistroyImageUpload_Loaded(object sender, RoutedEventArgs e)
        {
                dgImgListIPD.Visibility = Visibility.Visible;
                cmdOK.Visibility = Visibility.Visible;

                if (GetPreviousPhotos != null)
                {
                    mylistitem = GetPreviousPhotos;
                    dgImgListIPD.ItemsSource = null;
                    dgImgListIPD.ItemsSource = mylistitem;
                }
                else
                {
                    GetPAtientImage();
                }
        }
        private void GetPAtientImage()
        {
            try
            {
                clsGetPatientUploadedImagetHystoLapBizActionVO BizAction = new clsGetPatientUploadedImagetHystoLapBizActionVO();
                BizAction.PatientID = ScheduleID; //PatientID;
                //BizAction.VisitID = VisitID;
                BizAction.TemplateID = TemplateID;
                BizAction.IsFromOtImg = true;
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
                                foreach (ClsImages Obj in ((clsGetPatientUploadedImagetHystoLapBizActionVO)argsBizActionObjPatientHistoryData.Result).Img1)
                                {
                                    
                                    ListItems1 obj = new ListItems1();
                                    obj.ImageName1 = Obj.UserImage;
                                   // obj.ImageName1 = File.ReadAllBytes(Obj.ImageName);
                                    obj.Photo = obj.ImageName1;
                                    obj.ISIPD = "Visible";
                                    mylistitem.Add(obj);
                                }
                                
                                dgImgListIPD.ItemsSource = null;
                                dgImgListIPD.ItemsSource = mylistitem;
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
                        ShowPhoto(MyPhoto);
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
        private void cmdDeleteDrug_Click(object sender, RoutedEventArgs e)
        {
                mylistitem.Remove((ListItems1)dgImgListIPD.SelectedItem);
                dgImgListIPD.ItemsSource = null;
                dgImgListIPD.ItemsSource = mylistitem;
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
        public List<ListItems1> mylistitem = new List<ListItems1>();
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

