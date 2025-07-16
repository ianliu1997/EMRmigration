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

namespace PalashDynamics
{
    public partial class AddSOPFile : ChildWindow
    {
        public AddSOPFile()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog OpenFile = new OpenFileDialog();

            OpenFile.Multiselect = false;
            OpenFile.Filter = "(*.png)|*.png;";
            OpenFile.FilterIndex = 1;
            if (OpenFile.ShowDialog() == true)
            {

                //WriteableBitmap imageSource = new WriteableBitmap((int)imgSpousePhoto.Width, (int)imgSpousePhoto.Height);
                ////try
                ////{

                //WriteableBitmap bmp = new WriteableBitmap((int)imgSpousePhoto.Width, (int)imgSpousePhoto.Height);
                //bmp.Render(imgSpousePhoto, new MatrixTransform());
                //bmp.Invalidate();

                ////    int[] p = bmp.Pixels;
                ////    int len = p.Length * 4;
                ////    byte[] result = new byte[len]; // ARGB
                ////    Buffer.BlockCopy(p, 0, result, 0, len);

                //try
                //{
                //    imageSource.SetSource(OpenFile.File.OpenRead());
                //    imgSpousePhoto.Source = imageSource;

                //    using (Stream stream = OpenFile.File.OpenRead())
                //    {
                //        data = new byte[stream.Length];
                //        stream.Read(data, 0, (int)stream.Length);
                //        fi = OpenFile.File;
                //    }

                //    //((clsPatientVO)this.DataContext).SpouseDetails.Photo = result;
                //    ((clsPatientVO)this.DataContext).SpouseDetails.Photo = data;
                //}
                //catch (Exception)
                //{
                //    HtmlPage.Window.Alert("Error Loading File");
                //}

            }
        }
    }
}

