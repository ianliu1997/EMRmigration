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
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
//using PalashDynamics.Service.PalashServiceReferance;
using CIMS;
using System.Windows.Browser;
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;

namespace PalashDynamics.IPD
{
    public partial class frmIPDRefundReceiptList : ChildWindow
    {
        //Begin:: Added by AniketK on 25-Dec-2018
        public frmIPDRefundReceiptList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmIPDRefundReceiptList_Loaded);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public string ReceiptNo { get; set; }
        void frmIPDRefundReceiptList_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            FillIPDRefundReceiptList();
            string Title = "Receipts : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " "
                + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName + " Against Receipt No  " + ReceiptNo;
            this.Title = Title;
        }

        public long ID { get; set; }        // as BillID
        public long UnitID { get; set; }    // as BillUnitID
        long PRID = ((IApplicationConfiguration)App.Current).ApplicationConfigurations.PrintFormatID;
        public BillPaymentTypes BillPaymentType { get; set; }

        public void FillIPDRefundReceiptList()
        {
            try
            {
                clsGetRefundReceiptListBizActionVO BizAction = new clsGetRefundReceiptListBizActionVO();

                BizAction.BillID = ID;
                BizAction.BillUnitID = UnitID;

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetRefundReceiptListBizActionVO)arg.Result).Details != null)
                    {
                        dgIPDRefundReceiptList.ItemsSource = null;
                        dgIPDRefundReceiptList.ItemsSource = ((clsGetRefundReceiptListBizActionVO)arg.Result).Details;


                    }
                };
                Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                Client.CloseAsync();
            }
            catch (Exception)
            {
                //Indicatior.Close();
                // throw;
            }
        }
        //End:: Added by AniketK on 25-Dec-2018

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //Begin:: Added by AniketK on 25-Dec-2018
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgIPDRefundReceiptList.SelectedItem != null)
            {
                if (this.BillPaymentType == BillPaymentTypes.AgainstServices)  //None = 0, AgainstBill = 1, AgainstServices = 2
                {
                    PrintBill(((clsRefundVO)dgIPDRefundReceiptList.SelectedItem).ID, ((clsRefundVO)dgIPDRefundReceiptList.SelectedItem).UnitID, PRID);
                }
                else if (this.BillPaymentType == BillPaymentTypes.AgainstBill)  //None = 0, AgainstBill = 1, AgainstServices = 2
                {
                    PrintReceipt(((clsRefundVO)dgIPDRefundReceiptList.SelectedItem).ID, PRID);
                }
            }
        }

        private void PrintBill(long iRefundID, long iUnitID, long PrintID)
        {
            if (iRefundID > 0)
            {
                long UnitID = iUnitID;
                DateTime PrintDate = DateTime.Now;
                string URL = "../Reports/OPD/RefundServicesBill.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintFomatID=" + PrintID + "&PrintDate=" + PrintDate + "&IsFromIPD=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }

        private void PrintReceipt(long iRefundID, long PrintID)
        {
            if (iRefundID > 0)
            {
                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                DateTime PrintDate = DateTime.Now;
                string URL = "../Reports/OPD/RefundServicesBillNew.aspx?RefundID=" + iRefundID + "&UnitID=" + UnitID + "&PrintDate=" + PrintDate.ToString("MM/dd/yyyy") + "&IsFromIPD=" + 1;
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }
        }
        //End:: Added by AniketK on 25-Dec-2018

    }
}

