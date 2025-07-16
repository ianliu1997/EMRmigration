using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using Silverdraw.Client;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR;
using MessageBoxControl;
using CIMS;
using PalashDynamics.Service.PalashTestServiceReference;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Linq;
using System.Reflection;
using System.Windows.Resources;
using FluxJpeg.Core;
using FluxJpeg.Core.Encoder;
using FluxJpeg.Core.Filtering;
using PalashDynamics.ValueObjects.Administration;
using FluxJpeg;

namespace EMR
{
    public partial class frmEMREducation : UserControl
    {
        #region Data Member
        public clsVisitVO CurrentVisit { get; set; }
        public bool IsEnabledControl = false;
        bool blnDrawing = false;
        DrawingArea DrawArea;
        public bool IsUpdate = false;
        WaitIndicator indicator = null;

        int ClickedFlag = 0; //Added by AJ Date 22/11/2016
        #endregion

        #region Constructor
        public frmEMREducation()
        {
            InitializeComponent();
            DrawArea = new DrawingArea(this.DrawingCanvas);
            DrawArea.Tool = CurrentTool.Pencil;
            DrawArea.StrokeColor = new SolidColorBrush(Colors.Gray);
            DrawArea.FillColor = new SolidColorBrush(Colors.Red);
            DrawArea.StrokeWidth = 5;
            this.DataContext = DrawArea;
            this.Loaded += new RoutedEventHandler(frmEMREducation_Loaded);
        }
        #endregion

        #region Loaded
        void frmEMREducation_Loaded(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit == null)
                CurrentVisit = new clsVisitVO();
            fillComplaintList();
            GetUploadPatientImage();
            IsEnabledControl = false;
            if (CurrentVisit.VisitTypeID == 2 && !CurrentVisit.OPDIPD)
            {
                this.IsEnabledControl = false;
            }
            cmdSave.IsEnabled = IsEnabledControl;
            dgReport.IsReadOnly = !IsEnabledControl;
            btnEditImage.IsEnabled = IsEnabledControl;
            chkRemark.IsEnabled = IsEnabledControl;
            txtRemark.IsEnabled = IsEnabledControl;
            DrawingCanvas.Background = null;

            //DateTime d = CurrentVisit.Date;
            //if (d.ToString("d") != DateTime.Now.ToString("d"))
            //{
            //    cmdSave.IsEnabled = false;
            //}

            // EMR Changes Added by Ashish Z. on dated 02062017
            if (CurrentVisit.EMRModVisitDate <= DateTime.Now)
            {
                cmdSave.IsEnabled = false;
            }
            //End
        }
        #endregion

        #region Method
        private static string GetBase64Jpg(WriteableBitmap bitmap)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bands = 3;
            byte[][,] raster = new byte[bands][,];
            for (int i = 0; i < bands; i++)
            {
                raster[i] = new byte[width, height];
            }
            for (int row = 0; row < height; row++)
            {
                for (int column = 0; column < width; column++)
                {
                    int pixel = bitmap.Pixels[width * row + column];
                    raster[0][column, row] = (byte)(pixel >> 16);
                    raster[1][column, row] = (byte)(pixel >> 8);
                    raster[2][column, row] = (byte)pixel;
                }
            }
            ColorModel model = new ColorModel { colorspace = ColorSpace.RGB };
            FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);
            MemoryStream stream = new MemoryStream();
            ImageResizer resizer = new ImageResizer(img);
            var resizedImage = resizer.Resize(300, 300, ResamplingFilters.NearestNeighbor);
            JpegEncoder encoder = new JpegEncoder(resizedImage, 90, stream);
            encoder.Encode();
            stream.Seek(0, SeekOrigin.Begin);
            byte[] binaryData = new Byte[stream.Length];
            long bytesRead = stream.Read(binaryData, 0, (int)stream.Length);
            string base64String = System.Convert.ToBase64String(binaryData,
                                                  0,
                                                  binaryData.Length);
            return base64String;
        }
        public byte[] GetImages()
        {
            WriteableBitmap bmp = new WriteableBitmap(DrawingCanvas2, null);
            // Display captured snapshot or can save to database
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Margin = new Thickness(10);
            img.Source = bmp;
            //capturedImages.Children.Add(img);

            // Encode to JPEG format
            byte[] screenshot;
            using (Stream pngStream = new MemoryStream())
            {
                EncodeJpeg(bmp, pngStream);
                pngStream.Flush();
                pngStream.Seek(0, SeekOrigin.Begin);
                screenshot = ConvertToByteArray(pngStream);
                pngStream.Close();
            }
            return screenshot;
        }
        public byte[] ConvertToByteArray(Stream fileStream)
        {
            if (fileStream != null)
            {
                int fileStreamLength = (int)fileStream.Length;
                byte[] bytes = new byte[fileStreamLength];
                fileStream.Read(bytes, 0, fileStreamLength);

                return bytes;
            }

            return null;
        }
        public static void EncodeJpeg(WriteableBitmap bitmap, Stream destinationStream)
        {
            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;
            int bands = 3; // RGB
            try
            {
                byte[][,] raster = new byte[bands][,];

                // initialise the bands
                for (int i = 0; i < bands; i++)
                {
                    raster[i] = new byte[width, height];
                }

                // copy over the pixel data from the bitmap
                for (int row = 0; row < height; row++)
                {
                    for (int column = 0; column < width; column++)
                    {
                        int pixel = bitmap.Pixels[width * row + column];
                        raster[0][column, row] = (byte)(pixel >> 16);
                        raster[1][column, row] = (byte)(pixel >> 8);
                        raster[2][column, row] = (byte)pixel;
                    }
                }

                // Use the Flux library to encode the JPG
                ColorModel model = new ColorModel { colorspace = ColorSpace.RGB };
                FluxJpeg.Core.Image img = new FluxJpeg.Core.Image(model, raster);


                // encode it to the destination stream
                JpegEncoder encoder = new JpegEncoder(img, 90, destinationStream);
                encoder.Encode();
            }
            catch (Exception)
            {

                throw;
            }
        }
        void msgW_OnMessageBoxClosed()//MessageBoxResult result
        {
            indicator = new WaitIndicator();
            indicator.Show();
            //if (result == MessageBoxResult.Yes)
            //{
            if (!IsUpdate)
            {
                string msgText = "Palash";
                try
                {
                    clsUploadPatientImageBizActionVO BizActionObjPatientUpload = new clsUploadPatientImageBizActionVO();
                    BizActionObjPatientUpload.UploadMatserDetails = new clsPatientLinkFileBizActionVO();
                    BizActionObjPatientUpload.PatientID = CurrentVisit.PatientId;
                    BizActionObjPatientUpload.VisitID = CurrentVisit.ID;
                    BizActionObjPatientUpload.IsOPDIPD = CurrentVisit.OPDIPD;
                    BizActionObjPatientUpload.UploadMatserDetails.TemplateID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;
                    BizActionObjPatientUpload.Remark = txtRemark.Text;
                    if (((clsPatientLinkFileBizActionVO)dgReport.SelectedItem) != null && DrawingCanvas.Background != null)
                    {
                        BizActionObjPatientUpload.EditImage = GetImages();
                        BizActionObjPatientUpload.OriginalImage = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report;
                        BizActionObjPatientUpload.UploadMatserDetails.DocumentName = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).DocumentName;
                        BizActionObjPatientUpload.UploadMatserDetails.SourceURL = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).SourceURL;
                    }
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, arg) =>
                    {
                        if (arg.Error == null)
                        {
                            if (arg.Result != null)
                            {
                                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Document is Saved Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                mgbx.Show();
                                txtRemark.Text = String.Empty;
                                GetUploadPatientImage();
                                DrawingCanvas.Children.Clear();
                                DrawingCanvas.Background = null;
                                cmdSave.IsEnabled = false;
                                btnEditImage.IsEnabled = false;
                            }
                        }
                        indicator.Close();
                    };
                    client.ProcessAsync(BizActionObjPatientUpload, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    indicator.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgText, "Error occurred while saving record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            else
            {
                string msgText = "Palash";
                try
                {
                    if (((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem) != null && DrawingCanvas.Background != null)
                    {
                        clsUpdateUploadPatientImageBizActionVO BizActionObjPatientUpload = new clsUpdateUploadPatientImageBizActionVO();
                        BizActionObjPatientUpload.PatientID = CurrentVisit.PatientId;
                        BizActionObjPatientUpload.PatientUnitID = CurrentVisit.UnitId;
                        BizActionObjPatientUpload.VisitID = CurrentVisit.ID;
                        BizActionObjPatientUpload.UnitID = CurrentVisit.UnitId;
                        BizActionObjPatientUpload.TemplateID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;
                        //BizActionObjPatientUpload.DocumentName=((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).DocumentName;
                        BizActionObjPatientUpload.SourceURL = ((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem).SourceURL;
                        BizActionObjPatientUpload.EditImage = GetImages();
                        BizActionObjPatientUpload.Remark = txtRemark.Text;
                        Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                        PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                        client.ProcessCompleted += (s, arg) =>
                        {
                            if (arg.Error == null)
                            {
                                if (arg.Result != null)
                                {
                                    MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxChildWindow("Palash", "Patient Document is Updated Successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                    mgbx.Show();
                                    GetUploadPatientImage();
                                    txtRemark.Text = String.Empty;
                                    DrawingCanvas.Children.Clear();
                                    DrawingCanvas.Background = null;
                                }
                            }
                            indicator.Close();
                        };
                        client.ProcessAsync(BizActionObjPatientUpload, ((IApplicationConfiguration)App.Current).CurrentUser);
                        client.CloseAsync();
                    }
                    else
                    {
                        indicator.Close();
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Image.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();

                    }
                }
                catch (Exception)
                {
                    indicator.Close();
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                    new MessageBoxControl.MessageBoxChildWindow(msgText, "Error occurred while saving record", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            }
            //  }
        }
        private void NavigateToNextMenu()
        {
            EMR.frmEMR winEMR = ((this.Parent) as ContentControl).FindAncestor<UserControl>() as frmEMR;
            TreeView tvEMR = winEMR.FindName("tvPatientEMR") as TreeView;
            TreeViewItem SelectedItem = tvEMR.SelectedItem as TreeViewItem;
            clsMenuVO objMenu = SelectedItem.DataContext as clsMenuVO;
            if (SelectedItem.HasItems == true)
            {
                (SelectedItem.Items[0] as TreeViewItem).IsSelected = true;
            }
            else if (objMenu.Parent.Trim() == "Patient EMR")
            {
                int iCount = tvEMR.Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (objMenu.MenuOrder < iCount)
                {
                    if ((tvEMR.Items[iMenuIndex] as TreeViewItem).HasItems == true)
                    {
                        ((tvEMR.Items[iMenuIndex] as TreeViewItem).Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                        (tvEMR.Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
            }
            else
            {
                int iCount = (SelectedItem.Parent as TreeViewItem).Items.Count;
                int iMenuIndex = Convert.ToInt32(objMenu.MenuOrder);
                if (iCount > objMenu.MenuOrder)
                {
                    ((SelectedItem.Parent as TreeViewItem).Items[iMenuIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    objMenu = (SelectedItem.Parent as TreeViewItem).DataContext as clsMenuVO;
                    int iIndex = Convert.ToInt32(objMenu.MenuOrder);
                    (tvEMR.Items[iIndex] as TreeViewItem).IsSelected = true;
                }
            }
        }
        void fillComplaintList()
        {
            WaitIndicator myIndicatior = new WaitIndicator();
            try
            {
                myIndicatior.Show();
                clsGetEMRTemplateListBizActionVO BizAction = new clsGetEMRTemplateListBizActionVO();
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetEMRTemplateListBizActionVO)args.Result).objEMRTemplateList.Count > 0)
                        {
                            List<clsEMRTemplateVO> objEMRTemplateList = new List<clsEMRTemplateVO>();
                            objEMRTemplateList = ((clsGetEMRTemplateListBizActionVO)args.Result).objEMRTemplateList;
                            objEMRTemplateList.Insert(0, new clsEMRTemplateVO { TemplateID = 0, Template = "--Select--", Title = "--Select--" });

                            cmbComplaint.ItemsSource = objEMRTemplateList;
                            cmbComplaint.SelectedItem = objEMRTemplateList[0];

                            myIndicatior.Close();
                        }
                        else
                        {
                            myIndicatior.Close();
                            MessageBoxChildWindow msgbx = new MessageBoxChildWindow("Palash", "Template Not Assigned To The User.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                            msgbx.Show();
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                myIndicatior.Close();
            }
        }
        private void FillAttachment()
        {
            if (cmbComplaint.SelectedItem != null && cmbComplaint.SelectedIndex != 0)
            {
                clsGetPatientLinkFileViewDetailsBizActionVO BizAction = new clsGetPatientLinkFileViewDetailsBizActionVO();
                BizAction.TemplateID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;
                BizAction.IsOPDIPD = CurrentVisit.OPDIPD;
                BizAction.VisitID = CurrentVisit.ID;
                BizAction.FROMEMR = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if ((((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).PatientDetails) != null)
                        {
                            dgReport.ItemsSource = ((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).PatientDetails;
                        }
                        else
                        {
                            dgReport.ItemsSource = null;
                        }
                        if (((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).Remarks != null)
                        {
                            chkRemark.IsChecked = true;
                            txtRemark.Text = Convert.ToString(((clsGetPatientLinkFileViewDetailsBizActionVO)arg.Result).Remarks);
                            txtRemark.IsEnabled = true;
                        }
                        else
                        {
                            chkRemark.IsChecked = false;
                            txtRemark.Text = String.Empty;
                            txtRemark.IsEnabled = false;
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
        }
        private void GetUploadPatientImage()
        {
            clsGetUploadPatientImageBizActionVO BizActionObjPatientUpload = new clsGetUploadPatientImageBizActionVO();
            BizActionObjPatientUpload.PatientID = CurrentVisit.PatientId;
            BizActionObjPatientUpload.PatientUnitID = CurrentVisit.UnitId;
            BizActionObjPatientUpload.VisitID = CurrentVisit.ID;
            BizActionObjPatientUpload.ISOPDIPD = false;
            //    BizActionObjPatientUpload.TemplateID = ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null)
                    {
                        List<clsPatientFollowUpImageVO> ImageDetail = new List<clsPatientFollowUpImageVO>();
                        ImageDetail = ((clsGetUploadPatientImageBizActionVO)arg.Result).ImageDetails;
                        dgReportDetail.ItemsSource = null;
                        dgReportDetail.ItemsSource = ImageDetail;
                    }
                }
            };
            client.ProcessAsync(BizActionObjPatientUpload, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }
        public void SetImage(byte[] bitmap)
        {

            BitmapImage img = new BitmapImage();
            if (bitmap != null)
            {
                img.SetSource(new MemoryStream(bitmap, false));
                DrawingCanvas.Background = new ImageBrush() { ImageSource = img };
            }
            else
            {
                DrawingCanvas.Background = null;
            }

        }
        void c_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();

                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == "Editor.dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);

                myData = asm.CreateInstance("Editor.Controls.EditorControl") as UIElement;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Events
        private void dataGrid2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmdSave.IsEnabled = false;
            btnEditImage.IsEnabled = true;
            txtRemark.IsEnabled = true;
            chkRemark.IsEnabled = true;
            ImageSelection();
        }
        private void ImageSelection()
        {
            DrawingCanvas.Background = null;
            if ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem != null)
                if (((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report != null)
                {
                    byte[] imageBytes = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report;
                    BitmapImage img = new BitmapImage();
                    img.SetSource(new MemoryStream(imageBytes, false));
                    SignatuerImage.Source = img;
                    DrawingCanvas.Children.Clear();
                    txtRemark.Text = String.Empty;
                    //GetUploadPatientImage(false);
                }
        }
        private void cmbComplaint_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbComplaint.SelectedItem != null && ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID != null && ((clsEMRTemplateVO)cmbComplaint.SelectedItem).TemplateID == 0)
            {
                cmdSave.IsEnabled = false;
                btnEditImage.IsEnabled = false;
                chkRemark.IsEnabled = false;
                txtRemark.IsEnabled = false;
                dgReport.ItemsSource = null;
            }
            if (SignatuerImage.Source != null)
            {
                SignatuerImage.Source = null;
            }
            if (dgReportDetail.ItemsSource != null)
            {
                dgReportDetail.ItemsSource = null;
            }
            IsUpdate = false;
            DrawingCanvas.Children.Clear();
            DrawingCanvas.Background = null;
            FillAttachment();
            GetUploadPatientImage();
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag += 1; //Added by AJ Date 22/11/2016
            if (ClickedFlag == 1)
            {
                if (IsUpdate)
                {
                    if (((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem) != null && DrawingCanvas.Background != null)
                    {
                        //string msgText = "Are you sure you want to save the records ?";
                        //MessageBoxControl.MessageBoxChildWindow msgW =
                        //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                        //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                        //msgW.Show();
                        ClickedFlag = 0;
                        msgW_OnMessageBoxClosed();
                    }
                    else
                    {
                        ClickedFlag = 0;
                        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Image.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        mgbx.Show();
                    }
                }
                else
                {
                    //string msgText = DefaultValues.ResourceManager.GetString("SaveConfirmationMsg");
                    //MessageBoxControl.MessageBoxChildWindow msgW =
                    //new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.YesNo, MessageBoxControl.MessageBoxIcon.Question);
                    //msgW.OnMessageBoxClosed += new MessageBoxControl.MessageBoxChildWindow.MessageBoxClosedDelegate(msgW_OnMessageBoxClosed);
                    //msgW.Show();
                    msgW_OnMessageBoxClosed();
                    ClickedFlag = 0;
                }
            }
        }
        private void txtImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtImage.Text.Equals("Add Text on Image"))
                CanvasImageLabel.Content = txtImage.Text;
        }
        private void txtImage_LostFocus(object sender, RoutedEventArgs e)
        {
            txtImage.Text = txtImage.Text.Trim();
            if (txtImage.Text.Equals(""))
            {
                ((SolidColorBrush)txtImage.Foreground).Color = Colors.LightGray;

                txtImage.Text = "Add Text on Image";
            }
        }
        private void txtImage_GotFocus(object sender, RoutedEventArgs e)
        {
            txtImage.Text = txtImage.Text.Trim();
            if (txtImage.Text.Equals("Add Text on Image") || txtImage.Text.Equals(""))
            {
                if (((SolidColorBrush)txtImage.Foreground).Color != Colors.Black)
                {
                    txtImage.Text = "";

                    colorPicker.ColorChanged += (s, d) =>
                    {
                        CanvasImageLabel.Foreground = d.newColor;
                    };
                }
            }
        }
        private void chkRemark_Checked(object sender, RoutedEventArgs e)
        {
            if (chkRemark.IsChecked == true)
            {
                txtRemark.IsEnabled = true;
            }

        }
        private void chkRemark_Unchecked(object sender, RoutedEventArgs e)
        {
            txtRemark.IsEnabled = false;
            txtRemark.Text = string.Empty;
        }
        private void dgReportDetail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmdSave.IsEnabled = false;
            btnEditImage.IsEnabled = false;
            chkRemark.IsEnabled = false;
            txtRemark.IsEnabled = false;
            DrawingCanvas.Background = null;
            try
            {
                if (((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem) != null)
                {
                    if (((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem).EditImage != null)
                    {
                        byte[] imageBytes1 = ((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem).EditImage;
                        BitmapImage img1 = new BitmapImage();

                        SetImage(imageBytes1);
                        if (imageBytes1 != null)
                            IsUpdate = true;
                        else
                            IsUpdate = false;
                        if (((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem).Remark != null)
                        {
                            chkRemark.IsChecked = true; ;
                            txtRemark.Text = ((clsPatientFollowUpImageVO)dgReportDetail.SelectedItem).Remark;
                            txtRemark.IsEnabled = true;
                        }
                        else
                        {
                            chkRemark.IsChecked = false;
                            txtRemark.Text = string.Empty;
                            txtRemark.IsEnabled = false;
                        }
                    }
                }
            }
            catch (Exception es)
            {
                SetImage(null);
                chkRemark.IsChecked = false;
                txtRemark.Text = string.Empty;
                txtRemark.IsEnabled = false;
            }
        }
        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            DrawArea.UndoShape();
        }
        private void RedoButton_Click(object sender, RoutedEventArgs e)
        {
            DrawArea.RedoShape();
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigateToDashBoard();
        }
        private void NavigateToDashBoard()
        {
            this.Content = null;
            ((((((((this.Parent) as ContentControl).Parent as Border).Parent as DockPanel).Parent as DockPanel).FindName("tvPatientEMR") as TreeView)).Items[0] as TreeViewItem).IsSelected = true;
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ClickedFlag = 0; //Added by AJ Date 22/11/2016
            try
            {
                if (((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report != null)
                {
                    byte[] imageBytes = ((clsPatientLinkFileBizActionVO)dgReport.SelectedItem).Report;
                    BitmapImage img = new BitmapImage();

                    if (imageBytes != null)
                    {
                        img.SetSource(new MemoryStream(imageBytes, false));
                        DrawingCanvas.Background = new ImageBrush() { ImageSource = img };
                        IsUpdate = false;
                        cmdSave.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            this.ColorPanel.Visibility = Visibility.Collapsed;
        }
        private void ButtonTool_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn != null && btn.Tag is string)
            {
                DrawArea.Tool = (CurrentTool)Enum.Parse(typeof(CurrentTool), btn.Tag as string, true);
            }
        }
        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawArea.PrevPoint = e.GetPosition(this.DrawingCanvas);
            DrawArea.StartPoint = DrawArea.PrevPoint;
            DrawArea.TempHolder.Clear();

            if (blnDrawing)
                blnDrawing = false;
            else
            {
                blnDrawing = true;
                var cupt = e.GetPosition(this.DrawingCanvas);
                e.Handled = true;
            }

        }
        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            blnDrawing = false;
            var cupt = e.GetPosition(this.DrawingCanvas);
            DrawArea.HideVirtualLine();

            cupt = DrawArea.DrawOnComplete(cupt);

        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnDrawing && DrawArea.PrevPoint != null)
            {
                var cupt = e.GetPosition(this.DrawingCanvas);
                cupt = DrawArea.DrawOnMove(cupt);
            }

        }
        private void ButtonColor_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Canvas)
            {
                this.ColorPanel.Visibility = Visibility.Visible;
                ColorPanel.Tag = sender;
                colorPicker.ColorChanged += (s, c) =>
                {
                    (ColorPanel.Tag as Canvas).Background = c.newColor;
                };
            }
        }
        #endregion
    }
}
