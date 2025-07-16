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
    public partial class BarcodeForm : ChildWindow
    {
        public string PrintData;
        public string PrintItem;
        public string PrintDate;
        public string PrintFrom;
        public string printMRP;
        public clsGRNDetailsVO GRNItemDetails;
        public clsGRNDetailsFreeVO FreeGRNItemDetails;
        public clsReceivedItemDetailsVO ReceivedItemDetails; //***//


        public BarcodeForm()
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
            //UpdateGRNForBarcodePrint();
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
                    clsAddGRNBizActionVO BizAction = new clsAddGRNBizActionVO();
                    BizAction.GRNItemDetails = GRNItemDetails;
                    BizAction.IsGRNBarcodePrint = true;

                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                    PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    Client.ProcessCompleted += (s, arg) =>
                    {
                        //ClickedFlag1 = 0;
                        if (arg.Error == null && arg.Result != null && ((clsAddGRNBizActionVO)arg.Result).GRNItemDetails != null)
                        {
                            if (((clsAddGRNBizActionVO)arg.Result).GRNItemDetails != null)
                            {
                                if (((clsAddGRNBizActionVO)arg.Result).SuccessStatus == 1)
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

        public Canvas CreateRec(List<Rectangle> rList)
        {
            Canvas objCancas = new Canvas();
            foreach (Rectangle item in rList)
            {
                objCancas.Children.Add(item);
            }

            return objCancas;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Code128 barcode = new Code128();
                barcode.Height = 15;
                barcode.Width = 19;

                List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList1 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList2= new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList3 = new List<System.Windows.Shapes.Rectangle>();

                rList = barcode.DrawCode128(PrintData, 1, 3);
                rList1 = barcode.DrawCode128(PrintData, 1, 3);
                rList2= barcode.DrawCode128(PrintData, 1, 3);
                rList3 = barcode.DrawCode128(PrintData, 1, 3);


                foreach (System.Windows.Shapes.Rectangle item in rList)
                {
                    MyCanvas.Children.Add(item);
                }

                foreach (System.Windows.Shapes.Rectangle item in rList1)
                {
                    MyCanvas1.Children.Add(item);
                }

                foreach (System.Windows.Shapes.Rectangle item in rList2)
                {
                    MyCanvas2.Children.Add(item);
                }

                foreach (System.Windows.Shapes.Rectangle item in rList3)
                {
                    MyCanvas3.Children.Add(item);
                }

                MyText.Text = PrintData;
                MyText1.Text = PrintData;
                MyText2.Text = PrintData;
                MyText3.Text = PrintData;
            }
            catch (Exception)
            {

                throw;
            }


            #region Code39 Barcode
            //try
            //{
            //    int height = (int)this.MyCanvas.ActualHeight;

            //    PalashDynamics.Pharmacy.Barcode.Barcode barcode = new PalashDynamics.Pharmacy.Barcode.Barcode();

            //    barcode.Data = PrintData;
            //    barcode.encode();
            //    string encodedData = barcode.EncodedData;
            //    MyText.Text = barcode.HumanText;
            //    MyText1.Text = barcode.HumanText;
            //    MyText2.Text = barcode.HumanText;
            //    MyText3.Text = barcode.HumanText;

            //    int encodedLength = 0;
            //    for (int x = 0; x < encodedData.Length; x++)
            //    {
            //        if (encodedData[x] == 't')
            //            encodedLength++;
            //        else if (encodedData[x] == 'w')
            //            encodedLength = encodedLength + 3;
            //    }

            //    //float barWidth = (float)(250 / encodedLength);
            //    //float barWidth = (float)(50 / encodedLength);//
            //    double barWidth = (float)(50 / encodedLength);

            //    if (barWidth < 1)
            //        barWidth = 0.75;   //barWidth = 1;
            //    //float thickWidth = barWidth * 3;//
            //    double incrementWidth = 0;

            //    int swing = 0;
            //    for (int x = 0; x < encodedData.Length; x++)
            //    {
            //        Brush brush;
            //        if (swing == 0)
            //            brush = new SolidColorBrush(Colors.Black);
            //        else
            //            brush = new SolidColorBrush(Colors.White);

            //        if (encodedData[x] == 't')
            //        {
            //            //Rectangle r = new Rectangle();
            //            //Rectangle r1 = new Rectangle();
            //            //Rectangle r2 = new Rectangle();
            //            //Rectangle r3 = new Rectangle();
            //            //r.Fill = brush;
            //            //r.Width = 3 * barWidth;
            //            //r.Height = this.MyCanvas.Height;
            //            //r.SetValue(Canvas.LeftProperty, incrementWidth);
            //            //r.SetValue(Canvas.TopProperty, 0.0);
            //            //MyCanvas.Children.Add(r);
            //            MyCanvas.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas1.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas2.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas3.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            incrementWidth = incrementWidth + ((barWidth));
            //        }
            //        else if (encodedData[x] == 'w')
            //        {
            //            //Rectangle r = new Rectangle();
            //            //Rectangle r1 = new Rectangle();
            //            //Rectangle r2 = new Rectangle();
            //            //Rectangle r3 = new Rectangle();
            //            //r.Fill = brush;
            //            //r.Width = 3 * barWidth;
            //            //r.Height = this.MyCanvas.Height;
            //            //r.SetValue(Canvas.LeftProperty, incrementWidth);
            //            //r.SetValue(Canvas.TopProperty, 0.0);
            //            MyCanvas.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas1.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas2.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            MyCanvas3.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //            incrementWidth = incrementWidth + ((2 * (barWidth)));
            //        }

            //        if (swing == 0)
            //            swing = 1;
            //        else
            //            swing = 0;
            //    }

            //    ///////////////////////////////////////////////////////////////
            //    //incrementWidth = MyCanvas.Width + 4;
            //    //swing = 0;
            //    //for (int x = 0; x < encodedData.Length; x++)
            //    //{
            //    //    Brush brush;
            //    //    if (swing == 0)
            //    //        brush = new SolidColorBrush(Colors.Black);
            //    //    else
            //    //        brush = new SolidColorBrush(Colors.White);

            //    //    if (encodedData[x] == 't')
            //    //    {
            //    //        MyCanvas1.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //    //        incrementWidth = incrementWidth + ((barWidth));
            //    //    }
            //    //    else if (encodedData[x] == 'w')
            //    //    {
            //    //        MyCanvas1.Children.Add(CreateRectangle(brush, barWidth, height, incrementWidth));
            //    //        incrementWidth = incrementWidth + ((2 * (barWidth)));
            //    //    }

            //    //    if (swing == 0)
            //    //        swing = 1;
            //    //    else
            //    //        swing = 0;
            //    //}
            //}//
            //catch (Exception Ex)
            //{
            //    string strStackTrace = Ex.StackTrace;
            //}
            #endregion Code39 Barcode
        }

        public Rectangle CreateRectangle(Brush brush, double barWidth, int height, double incrementWidth)    //public Rectangle CreateRectangle(Brush brush, float barWidth, int height, double incrementWidth)
        {
            Rectangle r = new Rectangle();
            r.Fill = brush;
            r.Width = 3 * barWidth;
            r.Height = this.MyCanvas.Height;
            r.SetValue(Canvas.LeftProperty, incrementWidth);
            r.SetValue(Canvas.TopProperty, 0.0);
            return r;
        }

        public void DynamicBarcode()
        {
            //lstCanvas.Items.Clear();
            //Grid grdCanvas = new Grid();
            //grdCanvas.Height = lstCanvas.ActualHeight;
            //grdCanvas.Width = lstCanvas.ActualWidth;

            //int rows = 9;
            //double column = 4; // Math.Sqrt(rows);
            //int row = (int)Math.Sqrt(rows);

            //if ((double)row < column)
            //    column = column + 1;
            //column = (int)column;

            //if ((row * column) < rows)
            //    row++;

            //for (int gridrowDefination = 0; gridrowDefination < row; gridrowDefination++)
            //{
            //    RowDefinition Gridrow = new RowDefinition();
            //    Gridrow.Height = new GridLength((double)(lstCanvas.ActualHeight / row));
            //    grdCanvas.RowDefinitions.Add(Gridrow);
            //}

            //for (int gridDefination = 0; gridDefination < column; gridDefination++)
            //{
            //    ColumnDefinition Gridcolumn = new ColumnDefinition();
            //    Gridcolumn.Width = new GridLength((double)(lstCanvas.ActualWidth / column));
            //    grdCanvas.ColumnDefinitions.Add(Gridcolumn);
            //}
        }
    }
}

