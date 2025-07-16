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
using PalashDynamics.UserControls;

namespace PalashDynamics.Pharmacy
{
    public partial class ContraIndicationSideEffects : ChildWindow,IInitiateCIMS
    {
        #region IInitiateCIMS Members

        public void Initiate(string Mode)
        {
            //throw new NotImplementedException();

        }

        #endregion
        Boolean IsPageLoded = false;
        WaitIndicator indicator = new WaitIndicator();
        public clsItemMasterOtherDetailsVO objItemOtherDetailVO = null;

        public ContraIndicationSideEffects()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            indicator.Show();
            clsAddUpdateItemMasterOtherDetailsBizActionVO BizAction = new clsAddUpdateItemMasterOtherDetailsBizActionVO();
            BizAction.ItemOtherDetails = new clsItemMasterOtherDetailsVO();
            BizAction.ItemOtherDetails.ID = objItemOtherDetailVO.ID;
            BizAction.ItemOtherDetails.UnitID = objItemOtherDetailVO.UnitID;
            BizAction.ItemOtherDetails.ItemID = objItemOtherDetailVO.ItemID;
            BizAction.ItemOtherDetails.Contradiction = txtContraIndication.Text;
            BizAction.ItemOtherDetails.SideEffect = txtSideEffects.Text;
            BizAction.ItemOtherDetails.URL = txtURL.Text;

            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null)
                {
                    if (arg.Result != null && ((clsAddUpdateItemMasterOtherDetailsBizActionVO)arg.Result).SuccessStatus != -1)
                    {                        
                        MessageBoxChildWindow mgbox = new MessageBoxChildWindow("Palash", "Item other details are saved successfully.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    else
                    {
                        MessageBoxChildWindow mgbox = new MessageBoxChildWindow("Palash", "Item other details are not saved. /n Please try again.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                        mgbox.Show();
                    }
                    indicator.Close();
                }
                this.DialogResult = true;
                
            };
            client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        //    base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsPageLoded)
            {
                lblItemName.Text = "";
                if (objItemOtherDetailVO.ItemName != null)
                {
                    lblItemName.Text = objItemOtherDetailVO.ItemName;
                }
                FillData();
            }
            IsPageLoded = true;
        }

        private void FillData()
        {
            if(objItemOtherDetailVO !=null && objItemOtherDetailVO.ItemID!=null)
            {
                clsGetItemMasterOtherDetailsBizActionVO BizAction = new clsGetItemMasterOtherDetailsBizActionVO();
                BizAction.ItemID = objItemOtherDetailVO.ItemID;
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null)
                    {
                        if (arg.Result != null)
                        {
                            if (((clsGetItemMasterOtherDetailsBizActionVO)arg.Result).ItemOtherDetails != null)
                            {
                                objItemOtherDetailVO = ((clsGetItemMasterOtherDetailsBizActionVO)arg.Result).ItemOtherDetails;
                                txtContraIndication.Text = objItemOtherDetailVO.Contradiction == null ? "" : objItemOtherDetailVO.Contradiction;
                                txtSideEffects.Text = objItemOtherDetailVO.SideEffect == null ? "" : objItemOtherDetailVO.SideEffect;
                                txtURL.Text = objItemOtherDetailVO.URL == null ? "" : objItemOtherDetailVO.URL;
                            }
                            else
                            {
                                //MessageBoxChildWindow mgbox = new MessageBoxChildWindow("Details not Defined", "Contradiction and Side Effects are not Defined.", MessageBoxButtons.Ok, MessageBoxIcon.Information);
                                //mgbox.Show();
                            }
                        }
                    }
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            }
        }
    }
}

