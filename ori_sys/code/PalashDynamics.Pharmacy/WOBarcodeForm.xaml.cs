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
using PalashDynamics.Pharmacy.Barcode;
using System.Windows.Printing;
using CIMS;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy
{
    public partial class WOBarcodeForm : ChildWindow
    {
        public string PrintData;
        public string PrintItem;
        public string PrintDate;
        public string PrintFrom;
        public string printMRP;
        public clsWOGRNDetailsVO WOGRNItemDetails;

        public WOBarcodeForm()
        {
            InitializeComponent();
        }

        PrintDocument pd;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
            pd = new PrintDocument();
            pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);
            pd.EndPrint += new EventHandler<EndPrintEventArgs>(pd_EndPrint);
            pd.Print("BarcodeLabel");



        }

        void pd_EndPrint(object sender, EndPrintEventArgs e)
        {
            UpdateGRNForBarcodePrint();
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            e.PageVisual = PrintBarcodeArea;


        }

        public void UpdateGRNForBarcodePrint()
        {
            WaitIndicator Indicatior = new WaitIndicator();
            Indicatior.Show();

            try
            {

                if (PrintFrom == "GRN")
                {
                    clsAddWOGRNBizActionVO BizAction = new clsAddWOGRNBizActionVO();
                    BizAction.GRNItemDetails = WOGRNItemDetails;
                    BizAction.IsGRNBarcodePrint = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        //ClickedFlag1 = 0;
                        if (arg.Error == null && arg.Result != null && ((clsAddWOGRNBizActionVO)arg.Result).GRNItemDetails != null)
                        {
                            if (((clsAddWOGRNBizActionVO)arg.Result).GRNItemDetails != null)
                            {
                                if (((clsAddWOGRNBizActionVO)arg.Result).SuccessStatus == 1)
                                {
                                    string message = "Barcode for GRN details saved successfully";  // With GRNNo " + ((clsAddGRNBizActionVO)arg.Result).Details.GRNNO;

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", message, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                                    msgW1.Show();
                                }

                            }
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                            //System.Windows.Browser.HtmlPage.Window.Alert("Error occured while adding visit details.");
                            MessageBoxControl.MessageBoxChildWindow msgW1 =
                                        new MessageBoxControl.MessageBoxChildWindow("", "Error occured while adding GRN Barcode details.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            msgW1.Show();
                        }

                    };

                    Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    Client.CloseAsync();

                }
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
            finally
            {
                Indicatior.Close();
            }
        }

        public event RoutedEventHandler OnCancelButton_Click;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //this.Close();
            //if (OnCancelButton_Click != null)
            //    OnCancelButton_Click(this, new RoutedEventArgs());

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int height = (int)this.MyCanvas.ActualHeight;

            PalashDynamics.Pharmacy.Barcode.Barcode barcode = new PalashDynamics.Pharmacy.Barcode.Barcode();
            // barcode.BarcodeType = PalashDynamics.Pharmacy.Barcode.Barcode.BarcodeEnum.Code128;


            barcode.Data = PrintData;
            barcode.encode();
            string encodedData = barcode.EncodedData;
            MyText.Text = barcode.HumanText;
            MyText1.Text = PrintItem;
            MyText2.Text = "WHPL  Exp: " + Convert.ToDateTime(PrintDate).ToString("Y");
            MyText4.Text ="PRICE : KD "+ printMRP;

            int encodedLength = 0;
            for (int x = 0; x < encodedData.Length; x++)
            {
                if (encodedData[x] == 't')
                    encodedLength++;
                else if (encodedData[x] == 'w')
                    encodedLength = encodedLength + 3;
            }

            float barWidth = (float)(250 / encodedLength);

            if (barWidth < 1)
                barWidth = 1;
            float thickWidth = barWidth * 3;
            double incrementWidth = 0;

            int swing = 0;
            for (int x = 0; x < encodedData.Length; x++)
            {
                Brush brush;
                if (swing == 0)
                    brush = new SolidColorBrush(Colors.Black);
                else
                    brush = new SolidColorBrush(Colors.White);

                if (encodedData[x] == 't')
                {
                    Rectangle r = new Rectangle();
                    r.Fill = brush;
                    r.Width = 3 * barWidth;
                    r.Height = this.MyCanvas.Height;
                    r.SetValue(Canvas.LeftProperty, incrementWidth);
                    r.SetValue(Canvas.TopProperty, 0.0);
                    MyCanvas.Children.Add(r);
                    incrementWidth = incrementWidth + ((barWidth));
                }
                else if (encodedData[x] == 'w')
                {
                    Rectangle r = new Rectangle();
                    r.Fill = brush;
                    r.Width = 3 * barWidth;
                    r.Height = this.MyCanvas.Height;
                    r.SetValue(Canvas.LeftProperty, incrementWidth);
                    r.SetValue(Canvas.TopProperty, 0.0);
                    MyCanvas.Children.Add(r);

                    incrementWidth = incrementWidth + (2 * (barWidth));
                }

                if (swing == 0)
                    swing = 1;
                else
                    swing = 0;
            }
        }
    }
}

