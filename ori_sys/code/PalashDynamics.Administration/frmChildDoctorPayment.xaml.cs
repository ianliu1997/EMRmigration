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
using PalashDynamics.ValueObjects.Master.DoctorPayment;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects;
using System.Windows.Data;

namespace PalashDynamics.Administration
{
    public partial class frmChildDoctorPayment : ChildWindow
    {
        public frmChildDoctorPayment()
        {
            InitializeComponent();
        }
        PagedCollectionView pcv = null;
        private void GetData()
        {

            PalashDynamics.ValueObjects.Master.DoctorPayment.clsGetPaidDoctorPaymentDetailsBizActionVO BizActionVO = new PalashDynamics.ValueObjects.Master.DoctorPayment.clsGetPaidDoctorPaymentDetailsBizActionVO();
            BizActionVO.DoctorPaymentId = DoctorBillInfo.DoctorPaymentID;
            BizActionVO.DoctorId = DoctorBillInfo.DoctorID;
            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    clsGetPaidDoctorPaymentDetailsBizActionVO result = args.Result as clsGetPaidDoctorPaymentDetailsBizActionVO;
                    if (result.DoctorDetails != null)
                    {
                        pcv = new PagedCollectionView(result.DoctorDetails);
                        pcv.GroupDescriptions.Add(new PropertyGroupDescription("BillNo"));
                        dgPaymentDetailsList.ItemsSource = null;
                        dgPaymentDetailsList.ItemsSource = pcv;
                    }
                }
            };
            client.ProcessAsync(BizActionVO, new clsUserVO());
            client.CloseAsync();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }
        public clsDoctorPaymentVO DoctorBillInfo { get; set; }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChildDocPaymentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
            if (DoctorBillInfo != null)
                txtDoctorName.Text = DoctorBillInfo.DoctorName;
        }
    }
}

