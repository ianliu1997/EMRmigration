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
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using MessageBoxControl;

namespace PalashDynamics.Pharmacy
{
    public partial class PrintGRNItemBarCode : ChildWindow
    {
        public clsGRNVO GRNItemDetails;
        public event RoutedEventHandler OnCancelButton_Click;
        public PrintGRNItemBarCode()
        {
            InitializeComponent();
        }

        private void PrintGRNItemBarCode_Loaded(object sender, RoutedEventArgs e)
        {
            FillGRNAddDetailslList(GRNItemDetails.ID, GRNItemDetails.Freezed);
            dgGRNItems.UpdateLayout();
            dgGRNItems.Focus();
        }

        private void FillGRNAddDetailslList(long pGRNID, bool Freezed)
        {
            clsGetGRNDetailsListBizActionVO BizAction = new clsGetGRNDetailsListBizActionVO();
            //BizAction.IsActive = true;
            BizAction.GRNID = pGRNID;
            BizAction.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, e) =>
            {
                if (e.Error == null && e.Result != null)
                {
                    List<clsGRNDetailsVO> objList = new List<clsGRNDetailsVO>();

                    objList = ((clsGetGRNDetailsListBizActionVO)e.Result).List;
                    ////objList = ((clsGetMasterListBizActionVO)e.Result).MasterList;


                    //GRNAddedItems.Clear();
                    //foreach (var item in objList)
                    //{
                    //    GRNAddedItems.Add(item);
                    //}

                    dgGRNItems.ItemsSource = null;
                    dgGRNItems.ItemsSource = objList; // GRNAddedItems;

                }


            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void CmdPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            if (dgGRNItems.SelectedItem != null)
            {
               // BarcodeForm win = new BarcodeForm();
                //string date, BarCode;
                //string MRP = "";
                //long ItemID = 0;
                //string BatchID = null;
                //string BatchCode = null;
                //string ItemCode = null;
                //// ItemID = ((clsItemMasterVO)dataGrid2.SelectedItem).ItemID;
                //string ItemName = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemName;

                //if (dgGRNItems.SelectedItem != null)
                //{
                //    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate != null)
                //    {
                //        date = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate.Value.ToShortDateString();
                //    }
                //    else
                //        date = null;
                //    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).MRP != null)
                //    {
                //        //MRP = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).MRP.ToString();
                //        if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).ConversionFactor != null)
                //        {
                //            if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).ConversionFactor != 0)
                //                MRP = String.Format("{0:0.000}", ((clsGRNDetailsVO)dgGRNItems.SelectedItem).MRP / ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ConversionFactor);
                //            else
                //                MRP = String.Format("{0:0.000}", ((clsGRNDetailsVO)dgGRNItems.SelectedItem).MRP);
                //        }
                //    }
                //    else
                //        MRP = null;
                //    if (((clsGRNDetailsVO)dgGRNItems.SelectedItem).BarCode != null)
                //    {
                //        BarCode = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BarCode.ToString();
                //    }
                //    else
                //        BarCode = null;

                //}
                //else
                //{
                //    MRP = null;
                //    date = null;
                //    BarCode = null;
                //}
                //win.PrintData = BarCode.ToUpper();
                //win.PrintItem = ItemName;
                //win.PrintDate = date;
                //win.printMRP = MRP;
                //win.PrintFrom = "GRN";
                //win.GRNItemDetails = (clsGRNDetailsVO)dgGRNItems.SelectedItem;

                //// win.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);
                //win.Show();

                frmNewBarcodeCode128Form win = new frmNewBarcodeCode128Form();
                
                win.PrintData = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BarCode;
                win.PrintItem = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ItemName;
                if(((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate != null)
                win.PrintDate = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).ExpiryDate.Value.ToShortDateString();
                win.PrintBarcode = ((clsGRNDetailsVO)dgGRNItems.SelectedItem).BarCode;
                win.printMRP = String.Format("{0:0.000}", ((clsGRNDetailsVO)dgGRNItems.SelectedItem).MRP);
                win.OnCancelButton_Click += new RoutedEventHandler(BarcodeWin_OnCancelButton_Click);
                if (win.PrintData.Length > 20)
                    win.PrintData = win.PrintData.Substring(0, 20);  // added by hrishikesh 11 Oct 2014 - 15 to 20
                win.Show();


            }
            else
            {
                MessageBoxChildWindow msgbox = new MessageBoxChildWindow("Palash", "Please select Item.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                msgbox.Show();

            }
        }
        void BarcodeWin_OnCancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            if (OnCancelButton_Click != null)
                OnCancelButton_Click(this, new RoutedEventArgs());
        }


    }
}

