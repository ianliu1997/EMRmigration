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
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.RateContract;
using PalashDynamics.ValueObjects.Inventory.PurchaseOrder;
namespace PalashDynamics.Pharmacy
{
    public partial class PurchaseInfo : ChildWindow
    {
        public PurchaseInfo()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler OnCancelButton_Click;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButton_Click != null)
            {
                this.DialogResult = false;
                OnCancelButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }

        private void dgPO_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public void POLastPurchase(long Itemid, long iUnitID)
        {
            try
            {                             
                clsGetPurchaseOrderDetailsBizActionVO BizAction = new clsGetPurchaseOrderDetailsBizActionVO();
                BizAction.SearchID = Itemid;
                BizAction.UnitID = iUnitID;
                BizAction.ISPOLastPurchasePrice = true;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            clsGetPurchaseOrderDetailsBizActionVO obj = ((clsGetPurchaseOrderDetailsBizActionVO)arg.Result);
                            dgPO.ItemsSource = null;

                            if (obj.PurchaseOrderList != null && obj.PurchaseOrderList.Count > 0)
                            {
                                dgPO.ItemsSource = obj.PurchaseOrderList;
                            }
                        }
                    }
                    else
                    {
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                    }
                };

                Client.ProcessAsync(BizAction, new clsUserVO());
                Client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}

