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
using PalashDynamics.ValueObjects.EMR;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Inventory;
using PalashDynamics.Service.PalashTestServiceReference;
using CIMS;
using PalashDynamics.Pharmacy.ViewModels;
using OPDModule;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration;
using PalashDynamics.Collections;
using OPDModule.Forms;
using System.Windows.Browser;
using PalashDynamics.UserControls;
using System.Text;

namespace OPDModule.Forms
{
    public partial class GetCounterSalePrescriptionReason : ChildWindow
    {
        long PrescriptionID = 0;
        long PrescriptionUnitID = 0;

        public long VisitID = 0;
        public long UnitID = 0;

        public GetCounterSalePrescriptionReason()
        {
            InitializeComponent();
            //GetPrescriptionID();
            //FillReason();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public void GetPrescriptionID()
        {
            clsGetPrescriptionIDBizActionVO BizAction = new clsGetPrescriptionIDBizActionVO();
            BizAction.PatientPrescriptionReason = new clsPatientPrescriptionReasonOncounterSaleVO();
            BizAction.PatientPrescriptionReason.VisitID = VisitID;
            BizAction.PatientPrescriptionReason.UnitID = UnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && ((clsGetPrescriptionIDBizActionVO)args.Result).PatientPrescriptionReason != null)
                {
                    PrescriptionID = ((clsGetPrescriptionIDBizActionVO)args.Result).PatientPrescriptionReason.PrescriptionID;
                    PrescriptionUnitID = ((clsGetPrescriptionIDBizActionVO)args.Result).PatientPrescriptionReason.PrescriptionUnitID;
                    FillReason();
                }
                else
                {
                    //HtmlPage.Window.Alert("Error occured while processing.");
                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    msgW1.Show();
                }
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }

        private void FillReason()
        {
            clsGetPrescriptionReasonOnCounterSaleBizActionVO BizAction = new clsGetPrescriptionReasonOnCounterSaleBizActionVO();
            BizAction.PatientPrescriptionReason = new clsPatientPrescriptionReasonOncounterSaleVO();
            BizAction.PatientPrescriptionReason.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizAction.PatientPrescriptionReason.PrescriptionID = PrescriptionID;
            BizAction.PatientPrescriptionReason.PrescriptionUnitID = PrescriptionUnitID;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient Client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            Client.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && ((clsGetPrescriptionReasonOnCounterSaleBizActionVO)args.Result).PatientPrescriptionReasonList != null)
                {
                    if (((clsGetPrescriptionReasonOnCounterSaleBizActionVO)args.Result).PatientPrescriptionReasonList.Count > 0)
                    {
                        dgReason.ItemsSource = null;
                        dgReason.ItemsSource = ((clsGetPrescriptionReasonOnCounterSaleBizActionVO)args.Result).PatientPrescriptionReasonList;
                        dgReason.UpdateLayout();
                    }
                }
                //else
                //{
                //    //HtmlPage.Window.Alert("Error occured while processing.");
                //    MessageBoxControl.MessageBoxChildWindow msgW1 =
                //                  new MessageBoxControl.MessageBoxChildWindow("Palash", "Error occured while processing.", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //    msgW1.Show();
                //}
            };

            Client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            Client.CloseAsync();
        }
    }
}

