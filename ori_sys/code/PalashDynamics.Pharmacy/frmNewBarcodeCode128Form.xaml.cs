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
using PalashDynamics.ValueObjects.Inventory;
using System.Windows.Printing;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace PalashDynamics.Pharmacy
{
    public partial class frmNewBarcodeCode128Form : ChildWindow
    {
        public string PrintData;
        public string PrintItem;
        public string PrintDate;
        public string PrintFrom;
        public string printMRP;
        public string PrintBarcode;
        public clsGRNDetailsVO GRNItemDetails;

        public frmNewBarcodeCode128Form()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(BarcodeForm_Loaded);
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
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }
        private void BarcodeForm_Loaded(object sender, RoutedEventArgs e)
        {
            PrintBarcodeForPatientMRNO();
        }

        public void PrintBarcodeForPatientMRNO()
        {

            int height = (int)this.MyCanvas.ActualHeight;

            Code128 barcode = new Code128();
            List<System.Windows.Shapes.Rectangle> rList = new List<System.Windows.Shapes.Rectangle>();
            MyText.Text = PrintBarcode;
            txtItemName.Text = PrintItem;
            txtItemPrice.Text ="PRICE : KD " + printMRP+" (Ph:24340122)";

            rList = barcode.DrawCode128(PrintData, 1, 3);

            foreach (System.Windows.Shapes.Rectangle item in rList)
            {
                MyCanvas.Children.Add(item);
            }

        }

    }
}

