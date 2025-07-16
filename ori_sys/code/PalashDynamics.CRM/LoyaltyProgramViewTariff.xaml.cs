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
using PalashDynamics.ValueObjects.Billing;
using PalashDynamics.ValueObjects.Administration;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using CIMS;

namespace PalashDynamics.CRM
{
    public partial class LoyaltyProgramViewTariff : ChildWindow
    {
        public long TariffID { get; set; }
        public LoyaltyProgramViewTariff()
        {
            InitializeComponent();
        }

       

        private void LoyaltyProgramViewTariff_Loaded(object sender, RoutedEventArgs e)
        {

            
            if (TariffID > 0)
            {
                GetServiceByTariffID();

            }
        }
       

        private void GetServiceByTariffID()
        {
            clsGetServiceByTariffIDBizActionVO BizAction = new clsGetServiceByTariffIDBizActionVO();
            BizAction.TariffDetails = new clsTariffMasterBizActionVO();
            BizAction.TariffID = TariffID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);

            client.ProcessCompleted += (s, ea) =>
            {
                if (ea.Error == null && ea.Result != null)
                {
                    if (((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails != null)
                    {
                        txtTariffCode.Text = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.Code;
                        txtTariffName.Text = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.Description;
                        cmdAll.IsChecked = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.AllVisit;
                        cmdSpecify.IsChecked = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.Specify;
                        txtSpecify.Text = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.NoOfVisit.ToString();
                        chkDate.IsChecked = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.CheckDate;
                        if (((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.EffectiveDate != null)
                        {
                            dtpEffectiveDate.SelectedDate = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.EffectiveDate;
                        }
                        if (((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.ExpiryDate != null)
                        {
                            dtpExpiryDate.SelectedDate = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.ExpiryDate;
                        }
                        
                        dgServiceList.ItemsSource = ((clsGetServiceByTariffIDBizActionVO)ea.Result).TariffDetails.ServiceMasterList;
                    }
                }
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();

        }


        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

    }
}

