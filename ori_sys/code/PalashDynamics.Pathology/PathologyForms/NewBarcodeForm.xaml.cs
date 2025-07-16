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
using System.Windows.Printing;

namespace PalashDynamics.Pathology.PathologyForms
{
    public partial class NewBarcodeForm : ChildWindow
    {
        public string PrintData;
        public string P_Name;
        public string P_Age;
        public string P_Date;
        public string P_Time;
        public string P_Gender;
        public string P_MRNO;
        public string P_SampleNo;
        public string P_TestCode;
        public NewBarcodeForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ChildWindow_Loaded);
        }
        PrintDocument pd;
        public event RoutedEventHandler OnCancelButton_Click;
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
            e.PageVisual = PrintArea;//PrintBarcodeArea;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PrintBarcodeForPatientMRNO();
            if (rbSingle.IsChecked == true)
                rbSingle_Click(null, null);
        }
        public void PrintBarcodeForPatientMRNO()
        {

            int height = (int)this.MyCanvas.ActualHeight;
            Code128 barcode = new Code128();
            List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();
            MyTextName.Text = P_MRNO + " " + P_Name;
            //MyTextGN_AG.Text = "Gender: " + P_Gender + "     Age: " + P_Age + ' ' + "Yrs";
            //MyTextDate.Text = "Date: " + P_Date + "   C.T. :" + P_Time;
            MyText.Text = PrintData +" "+P_TestCode;
            rList = barcode.DrawCode128(PrintData, 1, 3);
            foreach (System.Windows.Shapes.Rectangle item in rList)
            {
                MyCanvas.Children.Add(item);
            }

            List<Rectangle> rList2 = new List<Rectangle>();
            MyTextName1.Text = P_Name + " " + P_Name;
            //MyTextGN_AG1.Text = "Gender: " + P_Gender + "     Age: " + P_Age + ' ' + "Yrs";
            //MyTextDate1.Text = "Date: " + P_Date + "   C.T. :" + P_Time;
            MyText1.Text = PrintData+" "+P_TestCode;
            rList2 = barcode.DrawCode128(PrintData, 1, 3);
            foreach (Rectangle item2 in rList2)
            {
                MyCanvas2.Children.Add(item2);
            }
        }
        #region Radio Button Click Events
        private void rbSingle_Click(object sender, RoutedEventArgs e)
        {
            if (rbSingle.IsChecked == true)
            {
                PrintBarcodeArea.Visibility = Visibility.Visible;
                PrintBarcodeArea2.Visibility = Visibility.Collapsed;
            }
            else
            {
                PrintBarcodeArea.Visibility = Visibility.Visible;
                PrintBarcodeArea2.Visibility = Visibility.Visible;
            }
        }

        private void rbDouble_Click(object sender, RoutedEventArgs e)
        {
            if (rbDouble.IsChecked == true)
            {
                PrintBarcodeArea.Visibility = Visibility.Visible;
                PrintBarcodeArea2.Visibility = Visibility.Visible;
            }
            else
            {
                PrintBarcodeArea.Visibility = Visibility.Visible;
                PrintBarcodeArea2.Visibility = Visibility.Collapsed;
            }
        }
        #endregion
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
    }
}

