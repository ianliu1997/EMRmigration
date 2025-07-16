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
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.IVF.Barcode;
using PalashDynamics.Pharmacy.BarCode;
using System.Windows.Printing;

namespace PalashDynamics
{
    public partial class BarcodeFormNew : ChildWindow
    {
        public BarcodeFormNew()
        {
            InitializeComponent();
        }
        private void LoadPatientHeader()
        {
            clsGetPatientBizActionVO BizAction = new PalashDynamics.ValueObjects.Patient.clsGetPatientBizActionVO();
            BizAction.PatientDetails = new PalashDynamics.ValueObjects.Patient.clsPatientVO();
            BizAction.PatientDetails.GeneralDetails = (clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    BizAction.PatientDetails.GeneralDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.GeneralDetails;
                    BizAction.PatientDetails.SpouseDetails = ((clsGetPatientBizActionVO)args.Result).PatientDetails.SpouseDetails;
                    BarcodeData();
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private clsCoupleVO _CoupleDetails = new clsCoupleVO();
        public clsCoupleVO CoupleDetails
        {
            get
            {
                return _CoupleDetails;
            }
            set
            {
                _CoupleDetails = value;
            }
        }

        private void fillCoupleDetails()
        {
            clsGetCoupleDetailsBizActionVO BizAction = new clsGetCoupleDetailsBizActionVO();
            BizAction.PatientID = ((clsPatientGeneralVO)((IApplicationConfiguration)App.Current).SelectedPatient).PatientID;
            BizAction.PatientUnitID = (((IApplicationConfiguration)App.Current).SelectedPatient).UnitId;
            BizAction.CoupleDetails = new clsCoupleVO();

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails != null && ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.CoupleId > 0)
                    {


                        BizAction.CoupleDetails.MalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.MalePatient;
                        BizAction.CoupleDetails.FemalePatient = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails.FemalePatient;
                        CoupleDetails.MalePatient = new clsPatientGeneralVO();
                        CoupleDetails.FemalePatient = new clsPatientGeneralVO();
                        CoupleDetails = ((clsGetCoupleDetailsBizActionVO)args.Result).CoupleDetails;
                        BarcodeData();
                    }
                    else
                    {
                        LoadPatientHeader();
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void BarcodeData()
        {
            if (RdoTestTube.IsChecked == true)
            {
                int height = (int)this.MyCanvas.ActualHeight;
                //Barcodes barcode = new Barcodes();
                Code128 barcode = new Code128();
                barcode.Height = 30;
                barcode.FontSize = 9;
                barcode.Width = 500;
                List<Rectangle> rList = new List<Rectangle>();
                MyText3.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0);
                if (CoupleDetails != null && CoupleDetails.CoupleId >0)
                {
                    MyText0.Text = CoupleDetails.MalePatient.PatientName;
                    MyText1.Text = CoupleDetails.FemalePatient.PatientName;
                }
                else
                {
                    MyText0.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                   // MyText1.Text = CoupleDetails.MalePatient.PatientName;
                }
                MyText2.Text = "DOB : " + ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth.Value.Date.ToString("dd/M/yyyy");
                rList = barcode.DrawCode128(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0), 1, 3);

                foreach (Rectangle item in rList)
                {
                    MyCanvas.Children.Add(item);
                    
                }

                List<Rectangle> rList2 = new List<Rectangle>();

                MyText32.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0);
                if (CoupleDetails != null && CoupleDetails.CoupleId > 0)
                {
                    MyText02.Text = CoupleDetails.MalePatient.PatientName;
                    MyText12.Text = CoupleDetails.FemalePatient.PatientName;
                }
                else
                {
                    MyText02.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                   // MyText12.Text = CoupleDetails.MalePatient.PatientName;
                }
                MyText22.Text = "DOB : " + ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth.Value.Date.ToString("dd/M/yyyy");
                rList2 = barcode.DrawCode128(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0), 1, 3);

                foreach (Rectangle item2 in rList2)
                {
                    MyCanvas2.Children.Add(item2);
                }
            }
            if (RdoFileSmall.IsChecked == true)
            {
                int height = (int)this.MyCanvas3.ActualHeight;
                Code128 barcode = new Code128();
                barcode.Height = 50;
                barcode.FontSize =10;
                barcode.Width = 500;
                List<Rectangle> rList = new List<Rectangle>();
                MyText33.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0);
                if (CoupleDetails != null && CoupleDetails.CoupleId > 0)
                {
                    MyText03.Text = CoupleDetails.MalePatient.PatientName;
                    MyText13.Text = "W/O " + CoupleDetails.FemalePatient.PatientName;
                }
                else
                {
                    MyText03.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                    //MyText13.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.AddressLine1;
                }
                MyText23.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.City;
                MyText43.Text = "Ph. : " + ((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1;
                rList = barcode.DrawCode128(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0), 1, 3);

                foreach (Rectangle item in rList)
                {
                    MyCanvas3.Children.Add(item);
                }
                
            }
            if (RdoFileLarge.IsChecked == true)
            {
                int height = (int)this.MyCanvas5.ActualHeight;
                Code128 barcode = new Code128();
                barcode.Height = 70;
                barcode.FontSize = 12;
                barcode.Width = 500;
                List<Rectangle> rList = new List<Rectangle>();
                MyText34.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0);
                if (CoupleDetails != null && CoupleDetails.CoupleId > 0)
                {
                    MyText04.Text = CoupleDetails.MalePatient.PatientName;
                    MyText14.Text ="W/O "+ CoupleDetails.FemalePatient.PatientName;
                }
                else
                {
                    MyText04.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                   // MyText14.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.AddressLine1;
                }

                MyText24.Text = ((IApplicationConfiguration)App.Current).SelectedPatient.City;
                MyText44.Text = "Ph. : "+((IApplicationConfiguration)App.Current).SelectedPatient.ContactNO1;
                rList = barcode.DrawCode128(((IApplicationConfiguration)App.Current).SelectedPatient.MRNo.Substring(0), 1, 3);

                foreach (Rectangle item in rList)
                {
                    MyCanvas5.Children.Add(item);
                }
            }
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fillCoupleDetails();
            
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (RdoTestTube.IsChecked == true)
            {
                PrintTestTube.Visibility = Visibility.Visible;
                PrintFileSmall.Visibility = Visibility.Collapsed;
                PrintFileLarge.Visibility = Visibility.Collapsed;
                BarcodeData();
            }
            else if (RdoFileSmall.IsChecked == true)
            {
                PrintTestTube.Visibility = Visibility.Collapsed;
                PrintFileSmall.Visibility = Visibility.Visible;
                PrintFileLarge.Visibility = Visibility.Collapsed;
                BarcodeData();
            }
            else if (RdoFileLarge.IsChecked == true)
            {
                PrintTestTube.Visibility = Visibility.Collapsed;
                PrintFileSmall.Visibility = Visibility.Collapsed;
                PrintFileLarge.Visibility = Visibility.Visible;
                BarcodeData();
            }
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


        PrintDocument pd;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            pd = new PrintDocument();
            pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);
            pd.Print("BarcodeLabel");



        }
        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (RdoTestTube.IsChecked == true)
                e.PageVisual = PrintTestTube;
            else if (RdoFileSmall.IsChecked == true)
                e.PageVisual = PrintFileSmall;
            else if (RdoFileLarge.IsChecked == true)
                e.PageVisual = PrintFileLarge;
            


        }
    }
}
