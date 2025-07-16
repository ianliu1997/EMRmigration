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
using PalashDynamics.UserControls;
using System.Windows.Browser;
using System.IO;

namespace PalashDynamics.OperationTheatre
{
    public partial class frmUpdatePhoto : ChildWindow
    {

        public byte[] MyPhoto { get; set; }
        public event RoutedEventHandler OnSaveButton_Click;
        CaptureSource _captureSource;

        WriteableBitmap _images = null;


        public frmUpdatePhoto()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PhotoWindow_Loaded);
            this.Title = null;
            this.HasCloseButton = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        void PhotoWindow_Loaded(object sender, RoutedEventArgs e)
        {

            VideoSources.ItemsSource = CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices();

            _captureSource = new CaptureSource();

            // wire up the async image capture for snapshot
            _captureSource.CaptureImageCompleted += ((s, args) =>
            {
                _images = (args.Result);
                imgPhoto.Source = _images;
            });

            if (MyPhoto != null)
            {
                WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
                //bmp.FromByteArray(MyPhoto);
                Buffer.BlockCopy(MyPhoto, 0, bmp.Pixels, 0, MyPhoto.Length);
                imgPhoto.Source = bmp;
            }
        }        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Win = new WaitIndicator();
            Win.Show();

            this.DialogResult = true;

            WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
            bmp.Render(imgPhoto, new MatrixTransform());
            bmp.Invalidate();

            int[] p = bmp.Pixels;
            int len = p.Length * 4;
            byte[] result = new byte[len]; // ARGB
            Buffer.BlockCopy(p, 0, result, 0, len);

            MyPhoto = result;

            if (OnSaveButton_Click != null)
                OnSaveButton_Click(this, new RoutedEventArgs());

            Win.Close();
            //this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            WaitIndicator Win = new WaitIndicator();
            Win.Show();
            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.jpg, *.png)|*.jpg;*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {
                WriteableBitmap imageSource = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                try
                {
                    imageSource.SetSource(OpenFile.File.OpenRead());
                    imgPhoto.Source = imageSource;
                    // MyPhoto = imageToByteArray(OpenFile.File);
                }
                catch (Exception)
                {
                    HtmlPage.Window.Alert("Error Loading File");
                }

            }

            Win.Close();
        }

        public byte[] imageToByteArray(FileInfo imageIn)
        {

            Stream stream = imageIn.OpenRead();
            BinaryReader binary = new BinaryReader(stream);
            Byte[] imgB = binary.ReadBytes((int)stream.Length);
            return imgB;
        }

        private void cmdCaptureImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_captureSource != null)
                {
                    _captureSource.CaptureImageAsync();
                    borderimage.Visibility = System.Windows.Visibility.Visible;
                    borderwebcap.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void cmdStartCapture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_captureSource != null)
                {
                    _captureSource.Stop(); // stop whatever device may be capturing

                    // set the devices for the capture source
                    _captureSource.VideoCaptureDevice = (VideoCaptureDevice)VideoSources.SelectedItem;
                    // _captureSource.AudioCaptureDevice = (AudioCaptureDevice)AudioSources.SelectedItem;

                    // create the brush
                    VideoBrush vidBrush = new VideoBrush();
                    vidBrush.SetSource(_captureSource);
                    WebcamCapture.Fill = vidBrush; // paint the brush on the rectangle

                    // request user permission and display the capture
                    if (CaptureDeviceConfiguration.AllowedDeviceAccess || CaptureDeviceConfiguration.RequestDeviceAccess())
                    {
                        _captureSource.Start();
                    }

                    borderimage.Visibility = System.Windows.Visibility.Collapsed;
                    borderwebcap.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void cmdStopCapture_Click(object sender, RoutedEventArgs e)
        {
            if (_captureSource != null)
            {
                _captureSource.Stop();
            }
        }

        


    }
}

