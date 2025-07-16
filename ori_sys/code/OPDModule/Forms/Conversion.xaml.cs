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
using PalashDynamics.ValueObjects;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;

namespace OPDModule.Forms
{
    public partial class Conversion : ChildWindow
    {
        public Conversion()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler OnSaveButton_Click;
        public event RoutedEventHandler OnCancelButton_Click;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmbConversion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        public List<MasterListItem> UOMConvertLIst = new List<MasterListItem>();
        public List<clsConversionsVO> UOMConversionLIst = new List<clsConversionsVO>();
        public void FillUOMConversions(long Itemid, long Conversion)
        {
            try
            {
                clsGetItemConversionFactorListBizActionVO BizAction = new clsGetItemConversionFactorListBizActionVO();

                BizAction.ItemID = Itemid;
                BizAction.UOMConversionList = new List<clsConversionsVO>();

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
                PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                Client.ProcessCompleted += (s, e) =>
                {
                    if (e.Error == null && e.Result != null)
                    {
                        UOMConvertLIst = new List<MasterListItem>();
                        MasterListItem objConversion = new MasterListItem();
                        objConversion.ID = 0;
                        objConversion.Description = "- Select -";
                        UOMConvertLIst.Add(objConversion);

                        if (((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList != null)
                            UOMConvertLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConvertList);
                        cmbConversion.ItemsSource = UOMConvertLIst;
                        UOMConversionLIst = new List<clsConversionsVO>();
                        UOMConversionLIst.AddRange(((clsGetItemConversionFactorListBizActionVO)e.Result).UOMConversionList);

                        if (Conversion != null)
                        {
                            cmbConversion.SelectedValue = Conversion;
                        }
                        else if (UOMConvertLIst != null)
                            cmbConversion.SelectedItem = UOMConvertLIst[0];
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

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            OnCancelButton_Click(this, new RoutedEventArgs());
            this.Close();

        }


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (OnSaveButton_Click != null)
            {
                this.DialogResult = true;
                OnSaveButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OnCancelButton_Click != null)
            {
                this.DialogResult = false;
                OnCancelButton_Click(this, new RoutedEventArgs());

                this.Close();
            }
        }
    }
}

