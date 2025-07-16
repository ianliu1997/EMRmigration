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
using PalashDynamic.Localization;
using CIMS;
using PalashDynamics.ValueObjects.IPD;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using C1.Silverlight.Data;
using System.Windows.Browser;

namespace EMR
{
    public partial class frmPrintSummery : ChildWindow
    {
      //  public RoutedEventHandler EncounterList_Selectionchanged1;

           public clsVisitVO CurrentVisit { get; set; }
         bool IsFromPrescription = false;
         private long p;
         private void BindGrid()
         {
             //Indicatior = new WaitIndicator();
             try
             {
                 // Indicatior.Show();
                 clsGetPatientPrescriptionandVisitDetailsBizActionVO BizActionCurMed = new clsGetPatientPrescriptionandVisitDetailsBizActionVO();
                 if (CurrentVisit != null)
                 {
                     BizActionCurMed.PatientID = CurrentVisit.PatientId;
                     BizActionCurMed.PatientUnitID = CurrentVisit.PatientUnitId;
                 }
                 if (((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID == ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId && ((IApplicationConfiguration)App.Current).ApplicationConfigurations.IsHO == true)
                 {
                     BizActionCurMed.UnitID = CurrentVisit.PatientUnitId;
                 }
                 else
                 {
                     BizActionCurMed.UnitID = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
                 }

                 if (IsFromPrescription == true)
                     BizActionCurMed.IsFromPresc = true;
                 else
                     BizActionCurMed.IsFromPresc = false;

                 BizActionCurMed.ISForPrint = true;

                 BizActionCurMed.IsOPDIPD = CurrentVisit.OPDIPD;
                 Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                 PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                 client.ProcessCompleted += (s, args) =>
                 {
                     if (args.Error == null && args.Result != null)
                     {
                         if (((clsGetPatientPrescriptionandVisitDetailsBizActionVO)args.Result).PatientVisitEMRDetailsIPD != null)
                         {
                             dgprintsummery.ItemsSource = ((clsGetPatientPrescriptionandVisitDetailsBizActionVO)args.Result).PatientVisitEMRDetailsIPD;
                             //if (dgVisitDetails.ItemsSource != null)
                             //    dgVisitDetails.SelectedIndex = 0;

                             //if (CurrentVisit.OPDIPD)
                             //{
                             //    dgVisitDetails.Columns[2].Header = "IPD";
                             //}
                             //else
                             //{
                             //    dgVisitDetails.Columns[2].Header = "OPD";
                             //}
                         }
                         //Indicatior.Close();
                     }
                     else
                     {
                         // Indicatior.Close();
                     }
                 };
                 client.ProcessAsync(BizActionCurMed, ((IApplicationConfiguration)App.Current).CurrentUser);
                 client.CloseAsync();

             }
             catch (Exception)
             {
               //  Indicatior.Close();
             }

         }

         public frmPrintSummery(clsVisitVO CurrentVisit)
        {
            InitializeComponent();
            this.CurrentVisit = CurrentVisit;
        }

        public frmPrintSummery(long p)
        {
            // TODO: Complete member initialization
            this.p = p;
        }
         
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = true;
          //  GridViewRow row = dgprintsummery.SelectedRow;
            //DataRowView row = (DataRowView)dgprintsummery.SelectedItems[0];
            //// In this example, the first column (index 0) contains
            //object item = dgprintsummery.SelectedItems.i;
            //string ID = (dgprintsummery.SelectedItems[0].Column.GetCellContent(item) as TextBlock).Text;

            //string ID;
            //ID = dgprintsummery.SelectedItems[0].Value.ToString();
            if (dgprintsummery.SelectedItem != null)
            {
              long a =  (dgprintsummery.SelectedItem as clsVisitEMRDetails).PrescriptionID ;

             

            }
            else
            {
                //msgText = "Please select Patient for cancel the Admission";
                //MessageBoxControl.MessageBoxChildWindow msgW =
                //            new MessageBoxControl.MessageBoxChildWindow("", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                //msgW.Show();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Title = LocalizationManager.resourceManager.GetString("ttlAssignDoctor");
           // FillDepartmentList(((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId);
            BindGrid();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void OKButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (dgprintsummery.SelectedItem != null)
            {
                long PrescriptionID = (dgprintsummery.SelectedItem as clsVisitEMRDetails).PrescriptionID;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRPrescriptionReport.aspx?PrescriptionID=" + PrescriptionID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + CurrentVisit.UnitId), "_blank");
                // PatientIPDClinicalSummary.aspx
            }
            else
            {
                string msgText = "Please select Prescription Record..";
                MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }

        private void OKButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (dgprintsummery.SelectedItem != null)
            {
                long PrescriptionID = (dgprintsummery.SelectedItem as clsVisitEMRDetails).PrescriptionID;

                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientIPDClinicalSummary.aspx?PrescriptionID=" + PrescriptionID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + CurrentVisit.UnitId), "_blank");

            }
            else
            {
                string msgText = "Please select Prescription Record..";
                MessageBoxControl.MessageBoxChildWindow msgW =
                            new MessageBoxControl.MessageBoxChildWindow("Palash", msgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW.Show();
            }
        }
    }
}

