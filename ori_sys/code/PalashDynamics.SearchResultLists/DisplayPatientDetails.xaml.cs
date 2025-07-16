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
using CIMS;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using System.IO;
using PalashDynamics.UserControls;

namespace PalashDynamics.SearchResultLists
{
    public partial class DisplayPatientDetails : UserControl
    {
       // public clsPatientConsoleHeaderVO MasterList { get;private set; }
        WaitIndicator indicator;
        public DisplayPatientDetails()
        {
            InitializeComponent();
            indicator = new WaitIndicator();
            this.Loaded += new RoutedEventHandler(DisplayPatientDetails_Loaded);
            
        }
        void DisplayPatientDetails_Loaded(object sender, RoutedEventArgs e)
        {
            //if (((IApplicationConfiguration)App.Current).SelectedPatient.Photo != null)
            //{
            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);   // Fill WriteableBitmap from byte array (format ARGB)
            //    //bmp.FromByteArray(((IApplicationConfiguration)App.Current).SelectedPatient.Photo);
            //    //imgPhoto.Source = bmp;

            //    byte[] imageBytes = ((IApplicationConfiguration)App.Current).SelectedPatient.Photo;
            //    BitmapImage img = new BitmapImage();
            //    img.SetSource(new MemoryStream(imageBytes, false));
            //    imgPhoto.Source = img;

            //    imgPhoto.Visibility = System.Windows.Visibility.Visible;
            //}

            LoadPatientHeader();
        }

        private void LoadPatientHeader()
        {
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                clsGetPatientConsoleHeaderDeailsBizActionVO BizAction = new clsGetPatientConsoleHeaderDeailsBizActionVO();
                BizAction.PatientID = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID;
                BizAction.UnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;
                BizAction.ISOPDIPD = false;
                //BizAction.PatientUnitID = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        BizAction.PatientDetails = ((clsGetPatientConsoleHeaderDeailsBizActionVO)args.Result).PatientDetails;
                        BizAction.PatientDetails.UnitId = ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId;

                        if (BizAction.PatientDetails.Gender.ToUpper() == "MALE")
                        {
                           /// MasterList = BizAction.PatientDetails;
                            Patient.DataContext = BizAction.PatientDetails; 
                            //BizAction.PatientDetails;
                            //if (BizAction.PatientDetails.Photo != null)
                            //if (BizAction.PatientDetails.ImageName.Length > 0)
                            //{
                            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                            //    //bmp.FromByteArray(BizAction.PatientDetails.Photo);
                            //    //imgPhoto.Source = bmp;

                            //    //commented by neena
                            //    //byte[] imageBytes = BizAction.PatientDetails.Photo;
                            //    //BitmapImage img = new BitmapImage();
                            //    //img.SetSource(new MemoryStream(imageBytes, false));
                            //    //imgPhoto.Source = img;
                            //    //


                            //    //added by neena
                            //    imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            //    //
                            //}
                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/MAle.png", UriKind.Relative));
                        }
                        else
                        {
                            Patient.DataContext = BizAction.PatientDetails;
                            //if (BizAction.PatientDetails.Photo != null)
                            //if (BizAction.PatientDetails.ImageName.Length > 0)
                            //{
                            //    //WriteableBitmap bmp = new WriteableBitmap((int)imgPhoto.Width, (int)imgPhoto.Height);
                            //    //bmp.FromByteArray(BizAction.PatientDetails.Photo);
                            //    //imgPhoto.Source = bmp;

                            //    //commented by neena
                            //    //byte[] imageBytes = BizAction.PatientDetails.Photo;
                            //    //BitmapImage img = new BitmapImage();
                            //    //img.SetSource(new MemoryStream(imageBytes, false));
                            //    //imgPhoto.Source = img;
                            //    //

                            //    //added by neena
                            //    imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            //    //
                            //}
                            if (BizAction.PatientDetails.ImageName != null && BizAction.PatientDetails.ImageName.Length > 0)
                            {
                                imgPhoto.Source = new BitmapImage(new Uri(BizAction.PatientDetails.ImageName, UriKind.Absolute));
                            }
                            else if (BizAction.PatientDetails.Photo != null)
                            {
                                byte[] imageBytes = BizAction.PatientDetails.Photo;
                                BitmapImage img = new BitmapImage();
                                img.SetSource(new MemoryStream(imageBytes, false));
                                imgPhoto.Source = img;
                            }
                            else
                                imgPhoto.Source = new BitmapImage(new Uri("./Icons/images1.jpg", UriKind.Relative));

                        }
                    }
                    indicator.Close();
                };
                client.ProcessAsync(BizAction, new clsUserVO());
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                indicator.Close();
                throw;
            }
        }

       

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion
    }
}
