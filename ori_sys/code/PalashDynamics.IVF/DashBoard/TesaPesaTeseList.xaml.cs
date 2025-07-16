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
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using CIMS;
using PalashDynamics.ValueObjects.IVFPlanTherapy;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.Patient;
using System.Windows.Media.Imaging;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.DashBoardVO;
using PalashDynamics.Collections;
using System.Collections.ObjectModel;
using PalashDynamics.ValueObjects.Administration.StaffMaster;
using PalashDynamics.Animations;
using System.Windows.Browser;
using System.Text;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class TesaPesaTeseList : ChildWindow
    {
        public clsCoupleVO CoupleDetails;
        public event RoutedEventHandler SelectTesaCode_Click;


        public TesaPesaTeseList TesaPesa;
        public string tesapesacode;
        public long ID;
        public bool IsCancel = true;
        public string DescriptionValue;

        public TesaPesaTeseList()
        {
            InitializeComponent();
        }

        private void SetCommandButtonState(String strFormMode)
        {
            switch (strFormMode)
            {
                case "Load":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "New":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Save":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;
                    IsCancel = true;
                    break;
                case "Modify":
                    cmdNew.IsEnabled = false;
                    CmdSave.IsEnabled = true;
                    CmdClose.IsEnabled = true;
                    IsCancel = false;
                    break;
                case "Cancel":
                    cmdNew.IsEnabled = true;
                    CmdSave.IsEnabled = false;
                    CmdClose.IsEnabled = true;

                    break;
                default:
                    break;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillGrid();

            this.Title = "Semen Thawing  :-(Name- " + CoupleDetails.MalePatient.FirstName +
                   " " + CoupleDetails.MalePatient.LastName + ")";
        }

        private void FillGrid()
        {
            cls_GetSpremFreezingDetilsForThawingBizActionVO bizAction = new cls_GetSpremFreezingDetilsForThawingBizActionVO();

            bizAction.MalePatientID = CoupleDetails.MalePatient.PatientID;
            bizAction.MalePatientUnitID = CoupleDetails.MalePatient.UnitId;
            bizAction.TemplateID = 63;
            bizAction.IsForTemplate = true;
            bizAction.DescriptionValue = DescriptionValue;

            Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy
            PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
            client.ProcessCompleted += (s, arg) =>
            {
                if (arg.Error == null && arg.Result != null && ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails != null)
                {
                    dgTesaExamination.ItemsSource = null;
                    dgTesaExamination.ItemsSource = ((cls_GetSpremFreezingDetilsForThawingBizActionVO)arg.Result).SpremFreezingDetails;
                    dgTesaExamination.UpdateLayout();
                }
            };
            client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client.CloseAsync();
        }

        private void cmdOk_Click(object sender, RoutedEventArgs e)
        {
            if (dgTesaExamination.SelectedItem != null)
            {
                this.DialogResult = true;

                TesaPesa = new TesaPesaTeseList();
                TesaPesa.tesapesacode = Convert.ToString(((clsNew_SpremFreezingVO)dgTesaExamination.SelectedItem).Code);
                TesaPesa.ID = ((clsNew_SpremFreezingVO)dgTesaExamination.SelectedItem).ID;
                if (SelectTesaCode_Click != null)
                    SelectTesaCode_Click(this, new RoutedEventArgs());

            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                                    new MessageBoxControl.MessageBoxChildWindow("Palash", "Please Select Sample !", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);

                msgW1.Show();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        private void CmdClose_Click(object sender, RoutedEventArgs e)
        {
            if (IsCancel == true)
            {
                this.DialogResult = false;
            }
            else
            {
                //objAnimation.Invoke(RotationType.Backward);
                IsCancel = true;
                CmdSave.Content = "Save";
                //  FetchData();
            }
            SetCommandButtonState("Cancel");
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            ((IApplicationConfiguration)App.Current).SelectedPatient = CoupleDetails.MalePatient;
            DateTime OnlyDateTime = Convert.ToDateTime(((clsNew_SpremFreezingVO)dgTesaExamination.SelectedItem).VitrificationDate);
            DateTime OnlyDate = OnlyDateTime;  //OnlyDateTime.Date;   
            HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/PatientCaseRecord.aspx?Type=3&UnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&VisitID=1" + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PatientUnitID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.UnitId + "&TemplateID=" + 67 + "&CurrentDate=" + OnlyDate.ToString("MM/dd/yyyy") + "&EMRId=" + ((clsNew_SpremFreezingVO)dgTesaExamination.SelectedItem).Code), "_blank");


        }
    }
}

