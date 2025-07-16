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
using PalashDynamics.Pharmacy.BarCode;
using System.Windows.Printing;
using PalashDynamics.Forms.Code128_REG;

namespace PalashDynamics
{
    public partial class BarCodeDoctor : ChildWindow
    {

        public string PrintData;
    
        public BarCodeDoctor()
        {
            InitializeComponent();
        }
        PrintDocument pd;
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            pd = new PrintDocument();
            pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);
            pd.EndPrint += new EventHandler<EndPrintEventArgs>(pd_EndPrint);
            pd.Print("BarcodeLabel");

        }

        void pd_EndPrint(object sender, EndPrintEventArgs e)
        {
            
        }

        void pd_PrintPage(object sender, PrintPageEventArgs e)
        {

           e.PageVisual = PrintBarcodeArea;

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
                Code128_REG barcode = new Code128_REG();
                barcode.Height = 15;
                barcode.Width = 19;

                List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList1 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList2 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList3 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList4 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList5 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList6 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList7 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList8 = new List<System.Windows.Shapes.Rectangle>();

                List<System.Windows.Shapes.Rectangle> rList9 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList10 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList11 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList12 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList13= new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList14 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList15 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList16 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList17 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList18 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList19 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList20 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList21 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList22 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList23 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList24 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList25 = new List<System.Windows.Shapes.Rectangle>();
                List<System.Windows.Shapes.Rectangle> rList26 = new List<System.Windows.Shapes.Rectangle>();
               

                rList = barcode.DrawCode128(PrintData, 1, 3);
                rList1 = barcode.DrawCode128(PrintData, 1, 3);
                rList2 = barcode.DrawCode128(PrintData, 1, 3);
                rList3 = barcode.DrawCode128(PrintData, 1, 3);
                rList4 = barcode.DrawCode128(PrintData, 1, 3);
                rList5 = barcode.DrawCode128(PrintData, 1, 3);
                rList6 = barcode.DrawCode128(PrintData, 1, 3);
                rList7 = barcode.DrawCode128(PrintData, 1, 3);
                rList8 = barcode.DrawCode128(PrintData, 1, 3);
                rList9 = barcode.DrawCode128(PrintData, 1, 3);
                rList10 = barcode.DrawCode128(PrintData, 1, 3);
                rList11 = barcode.DrawCode128(PrintData, 1, 3);
                rList12 = barcode.DrawCode128(PrintData, 1, 3);
                rList13 = barcode.DrawCode128(PrintData, 1, 3);
                rList14= barcode.DrawCode128(PrintData, 1, 3);
                rList15 = barcode.DrawCode128(PrintData, 1, 3);
                rList16 = barcode.DrawCode128(PrintData, 1, 3);
                rList17 = barcode.DrawCode128(PrintData, 1, 3);
                rList18 = barcode.DrawCode128(PrintData, 1, 3);
                rList19 = barcode.DrawCode128(PrintData, 1, 3);
                rList20 = barcode.DrawCode128(PrintData, 1, 3);
                rList21 = barcode.DrawCode128(PrintData, 1, 3);
                rList22 = barcode.DrawCode128(PrintData, 1, 3);
                rList23  = barcode.DrawCode128(PrintData, 1, 3);
                rList24 = barcode.DrawCode128(PrintData, 1, 3);
                rList25 = barcode.DrawCode128(PrintData, 1, 3);
                rList26 = barcode.DrawCode128(PrintData, 1, 3);          



         
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

                foreach (System.Windows.Shapes.Rectangle item in rList4)
                {
                    MyCanvas4.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList5)
                {
                    MyCanvas5.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList6)
                {
                    MyCanvas6.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList7)
                {
                    MyCanvas7.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList8)
                {
                    MyCanvas8.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList9)
                {
                    MyCanvas9.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList10)
                {
                    MyCanvas10.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList11)
                {
                    MyCanvas11.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList12)
                {
                    MyCanvas12.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList13)
                {
                    MyCanvas13.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList14)
                {
                    MyCanvas14.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList15)
                {
                    MyCanvas15.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList16)
                {
                    MyCanvas16.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList17)
                {
                    MyCanvas17.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList18)
                {
                    MyCanvas18.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList19)
                {
                    MyCanvas19.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList20)
                {
                    MyCanvas20.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList21)
                {
                    MyCanvas21.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList22)
                {
                    MyCanvas22.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList23)
                {
                    MyCanvas23.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList24)
                {
                    MyCanvas24.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList25)
                {
                    MyCanvas25.Children.Add(item);
                }
                foreach (System.Windows.Shapes.Rectangle item in rList26)
                {
                    MyCanvas26.Children.Add(item);
                }
                
                
            }
            catch (Exception)
            {

                throw;
            }

        }

        public Rectangle CreateRectangle(Brush brush, double barWidth, int height, double incrementWidth)    
        {
            Rectangle r = new Rectangle();
            r.Fill = brush;
            r.Width = 3 * barWidth;
            r.Height = this.MyCanvas.Height;
            r.SetValue(Canvas.LeftProperty, incrementWidth);
            r.SetValue(Canvas.TopProperty, 0.0);
            return r;
        }
    }
}

