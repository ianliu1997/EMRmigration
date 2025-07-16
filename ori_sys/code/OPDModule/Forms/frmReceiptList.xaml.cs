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
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using System.Windows.Browser;

namespace OPDModule
{
    public partial class frmReceiptList : ChildWindow
    {
        public frmReceiptList()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmReceiptList_Loaded);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        public string BillNo { get; set; }
        void frmReceiptList_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();

            FillSettledPaymentList();
            string Title = "Receipts : " + ((IApplicationConfiguration)App.Current).SelectedPatient.FirstName + " " + ((IApplicationConfiguration)App.Current).SelectedPatient.MiddleName + " "
                + ((IApplicationConfiguration)App.Current).SelectedPatient.LastName + " Against Bill No  " + BillNo ;
            this.Title = Title;
        }

        public long BillID { get; set; }
        public long BillUnitID { get; set; }

        public void FillSettledPaymentList()
        {
            try
            {
                clsGetPaymentListBizActionVO BizAction = new clsGetPaymentListBizActionVO();

                BizAction.BillID = BillID;
                BizAction.UnitID = BillUnitID;
         
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetPaymentListBizActionVO)arg.Result).List != null)
                    {
                        dgReceiptList.ItemsSource = null;
                        dgReceiptList.ItemsSource = ((clsGetPaymentListBizActionVO)arg.Result).List;


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
        
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (dgReceiptList.SelectedItem != null)
            {
                PrintSettleBill(BillID, BillUnitID,((clsPaymentVO)dgReceiptList.SelectedItem).ID);
            }
        }

        private void PrintSettleBill(long iBillId, long iUnitID,long iPaymentID)
        {
            if (iBillId > 0 && iPaymentID >0)
            {
                #region added by Prashant Channe to read reports config on 3rdDec2019
                string StrConfigReportsDir = ((IApplicationConfiguration)App.Current).CurrentUser.ReportsFolder;
                string URL = null;
                #endregion

                //long UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                long UnitID = iUnitID;
                //Added by Prashant Channe on 3rdDec2019 for reports configuration
                if (!string.IsNullOrEmpty(StrConfigReportsDir))
                {
                    URL = "../" + StrConfigReportsDir + "/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
                else
                {
                    URL = "../Reports/OPD/SettleClinicalBill.aspx?BillID=" + iBillId + "&UnitID=" + UnitID + "&PaymentID=" + iPaymentID;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, URL), "_blank");
            }

        }
    }
}

