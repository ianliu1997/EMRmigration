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
using PalashDynamics.IVF.Barcode;
using System.Windows.Printing;
using PalashDynamics.Pharmacy.BarCode;




namespace PalashDynamics.IVF
{
    public partial class BarcodeForm : ChildWindow
    {
        public string PrintData;
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
            pd.Print("BarcodeLabel");

            

        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

            e.PageVisual = PrintBarcodeArea;


        }
        public event RoutedEventHandler OnCloseButton_Click;
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //OnCloseButton_Click(this, new RoutedEventArgs());
            this.Close();
        }
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                int height = (int)this.MyCanvas.ActualHeight;
                //Barcodes barcode = new Barcodes();
                Code128 barcode = new Code128();
                barcode.Height = 30;
                barcode.FontSize = 9;
                List<Rectangle> rList = new List<Rectangle>();
                MyText.Text = PrintData;
                //MyText2.Text = "DOB : " + ((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth.Value.Date.ToString("dd/M/yyyy");
                rList = barcode.DrawCode128(PrintData, 1, 3);

                foreach (Rectangle item in rList)
                {
                    MyCanvas.Children.Add(item);
                }





                //int height = (int)this.MyCanvas.ActualHeight;
                //Barcodes barcode = new Barcodes();
                //barcode.BarcodeType = Barcodes.BarcodeEnum.Code39;


                //barcode.Data = PrintData;
                //barcode.EncodedData = PrintData;
                //barcode.encode();
                //string encodedData = barcode.EncodedData;
                //MyText.Text = barcode.HumanText;


                //int encodedLength = 0;
                //for (int x = 0; x < encodedData.Length; x++)
                //{
                //    if (encodedData[x] == 't')
                //        encodedLength++;
                //    else if (encodedData[x] == 'w')
                //        encodedLength = encodedLength + 3;
                //}

                //float barWidth = (float)(250 / encodedLength);

                //if (barWidth < 1)
                //    barWidth = 1;
                //float thickWidth = barWidth * 3;
                //double incrementWidth = 0;

                //int swing = 0;
                //for (int x = 0; x < encodedData.Length; x++)
                //{
                //    Brush brush;
                //    if (swing == 0)
                //        brush = new SolidColorBrush(Colors.Black);
                //    else
                //        brush = new SolidColorBrush(Colors.White);

                //    if (encodedData[x] == 't')
                //    {
                //        Rectangle r = new Rectangle();
                //        r.Fill = brush;
                //        r.Width = 3 * barWidth;
                //        r.Height = this.MyCanvas.Height;
                //        r.SetValue(Canvas.LeftProperty, incrementWidth);
                //        r.SetValue(Canvas.TopProperty, 0.0);
                //        MyCanvas.Children.Add(r);
                //        incrementWidth = incrementWidth + ((barWidth));
                //    }
                //    else if (encodedData[x] == 'w')
                //    {
                //        Rectangle r = new Rectangle();
                //        r.Fill = brush;
                //        r.Width = 3 * barWidth;
                //        r.Height = this.MyCanvas.Height;
                //        r.SetValue(Canvas.LeftProperty, incrementWidth);
                //        r.SetValue(Canvas.TopProperty, 0.0);
                //        MyCanvas.Children.Add(r);

                //        incrementWidth = incrementWidth + (3 * (barWidth));
                //    }

                //    if (swing == 0)
                //        swing = 1;
                //    else
                //        swing = 0;
                //}
            }
            catch (Exception ss)
            {
                throw;
            }
        }
    }
}




//        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
//        {
//            int height = (int)this.MyCanvas.ActualHeight;
//            Barcodes barcode = new Barcodes();
//            barcode.BarcodeType = Barcodes.BarcodeEnum.Code39;

            
//            barcode.Data = PrintData;
//            barcode.encode();
//            string encodedData = barcode.EncodedData;
//            MyText.Text = barcode.HumanText;


//            int encodedLength = 0;
//            for (int x = 0; x < encodedData.Length; x++)
//            {
//                if (encodedData[x] == 't')
//                    encodedLength++;
//                else if (encodedData[x] == 'w')
//                    encodedLength = encodedLength + 3;
//            }

//            float barWidth = (float)(250 / encodedLength);

//            if (barWidth < 1)
//                barWidth = 1;
//            float thickWidth = barWidth * 3;
//            double incrementWidth = 0;

//            int swing = 0;
//            for (int x = 0; x < encodedData.Length; x++)
//            {
//                Brush brush;
//                if (swing == 0)
//                    brush = new SolidColorBrush(Colors.Black);
//                else
//                    brush = new SolidColorBrush(Colors.White);

//                if (encodedData[x] == 't')
//                {
//                    Rectangle r = new Rectangle();
//                    r.Fill = brush;
//                    r.Width = 3 * barWidth;
//                    r.Height = this.MyCanvas.Height;
//                    r.SetValue(Canvas.LeftProperty, incrementWidth);
//                    r.SetValue(Canvas.TopProperty, 0.0);
//                    MyCanvas.Children.Add(r);
//                    incrementWidth = incrementWidth + ((barWidth));
//                }
//                else if (encodedData[x] == 'w')
//                {
//                    Rectangle r = new Rectangle();
//                    r.Fill = brush;
//                    r.Width = 3 * barWidth;
//                    r.Height = this.MyCanvas.Height;
//                    r.SetValue(Canvas.LeftProperty, incrementWidth);
//                    r.SetValue(Canvas.TopProperty, 0.0);
//                    MyCanvas.Children.Add(r);

//                    incrementWidth = incrementWidth + (3 * (barWidth));
//                }

//                if (swing == 0)
//                    swing = 1;
//                else
//                    swing = 0;
//            }
//        }
//    }
//}

