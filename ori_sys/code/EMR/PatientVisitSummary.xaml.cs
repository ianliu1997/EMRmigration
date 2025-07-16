using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using System.Windows.Browser;
using CIMS;
using PalashDynamics.UserControls;
using PalashDynamics.ValueObjects.EMR;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamic.Localization;

namespace EMR
{
    public partial class PatientVisitSummary : ChildWindow
    {
        #region Variable Declaration
        public clsVisitVO CurrentVisit;
        public bool IsEnabledControl { get; set; }
        public Patient SelectedPatient { get; set; }
        WaitIndicator Indicatior = new WaitIndicator();
        #endregion

        public PatientVisitSummary()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(PatientVisitSummary_Loaded);
        }

        void PatientVisitSummary_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Visit List";//LocalizationManager.resourceManager.GetString("ttlVisitList");
            LoadSummary();
        }

        private void cmdPrintSummary_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                long TemplateID = 0;
                if (((IApplicationConfiguration)App.Current).SelectedPatient.GenderID == 2)
                {
                    TemplateID = 19;
                }
                else
                {
                    TemplateID = 22;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientClinicalSummarynew.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + CurrentVisit.UnitId + "&HistoryFlag=2" + "&TemplateID="+ TemplateID), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        //private void cmdPrintPrescription_Click(object sender, RoutedEventArgs e)
        //{
        //    if (dgComplaintSummary.SelectedIndex != -1)
        //    {
        //        clsVisitEMRDetails objEMRVisit = dgComplaintSummary.SelectedItem as clsVisitEMRDetails;
        //        HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/Patient/EMRPatientPrescription.aspx?UnitID=" + objEMRVisit.UnitId + "&VisitID=" + objEMRVisit.VisitId + "&PatientID=" + objEMRVisit.PatientId + "&PatientUnitID=" + objEMRVisit.PatientUnitId + "&TemplateID=" + 0 + "&UserID=" + ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID), "_blank");
        //        this.DialogResult = true;
        //    }
        //    else
        //    {
        //        MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "EMR details not enter", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //        mgbx.Show();
        //    }
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientRadiologydetails.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientRadiologydetails.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitId=" + CurrentVisit.UnitId), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        private void cmdPathology_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
               // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientPathologydetails.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientPathologydetails.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitId=" + CurrentVisit.UnitId), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        private void cmdDiagnostics_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientDiagnosticsdetails.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        private void cmdPrintPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRPatientPrescription.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId +"&IsOPDIPD=0&UnitID=" + CurrentVisit.UnitId), "_blank");
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRPatientPrescription.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId + "&IsOPDIPD=0&UnitID=" + CurrentVisit.UnitId), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }

        private void cmdReferral_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
                if (CurrentVisit != null)
                {
                    BizAction.VisitID = ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId;
                    BizAction.PatientID = CurrentVisit.PatientId;
                }

                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (a, arg) =>
                {
                    if (arg.Error == null && arg.Result != null && ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
                    {
                        if (((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail.Count == 1)
                        {
                            clsDoctorSuggestedServiceDetailVO obj = ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail[0];
                            if (obj != null && obj.ID > 0)
                            {
                               // HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllReferralLetterPrint.aspx?VisitID=" + BizAction.VisitID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + obj.PrescriptionID + "&ID=" + obj.ID), "_blank");
                                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllReferralLetterPrint.aspx?VisitID=" + BizAction.VisitID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + obj.PrescriptionID + "&ID=" + obj.ID + "&UnitID="+CurrentVisit.UnitId), "_blank");
                            }
                        }
                        else
                        {
                            PrintReferralOrder winSummary = new PrintReferralOrder();
                            winSummary.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.patientUnitID };
                            winSummary.IsfromVisitWin = true;
                            if(dgComplaintSummary.SelectedItem != null)
                                winSummary.VisitId = ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId;
                            winSummary.IsEnabledControl = CurrentVisit.VisitStatus;
                            winSummary.CurrentVisit = this.CurrentVisit;
                            winSummary.Show();
                        }
                    }
                    else
                    {
                        string strMessage = string.Empty;
                        strMessage = "No Referral Found !";//PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgNoReferralFound");
                        MessageBoxControl.MessageBoxChildWindow msgW1 =
                           new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        msgW1.Show();
                        return;

                    }

                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgSelectPatientVisitFirst");
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }
        private void LoadSummary()
        {
            try
            {
                Indicatior = new WaitIndicator();
                Indicatior.Show();
                clsGetPatientVisitSummaryListForEMRBizActionVO BizAction = new clsGetPatientVisitSummaryListForEMRBizActionVO();
                BizAction.PatientID = CurrentVisit.PatientId;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, arg) =>
                {
                    if (arg.Error == null && arg.Result != null)
                    {
                        List<clsVisitEMRDetails> VisitEMRDetailsList = new List<clsVisitEMRDetails>();
                        VisitEMRDetailsList = ((clsGetPatientVisitSummaryListForEMRBizActionVO)arg.Result).VisitEMRDetailsList;
                        if (VisitEMRDetailsList.Count > 0)
                        {
                            btnPrintPrescription.IsEnabled = true;
                            btnPrintSummary.IsEnabled = true;
                        }
                        dgComplaintSummary.ItemsSource = VisitEMRDetailsList;
                    }
                    Indicatior.Close();
                };
                client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dgComplaintSummary.SelectedItem != null)
            {
                long TemplateID = 0;
                if (((IApplicationConfiguration)App.Current).SelectedPatient.GenderID == 2)
                {
                    TemplateID = 19;
                }
                else
                {
                    TemplateID = 22;
                }
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientVisitWiseHistory.aspx?VisitID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).VisitId + "&PatientID=" + ((clsVisitEMRDetails)dgComplaintSummary.SelectedItem).PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + ((IApplicationConfiguration)App.Current).ApplicationConfigurations.UnitID + "&TemplateID=" + TemplateID + "&EmrID=0"), "_blank");
            }
            else
            {
                string strMessage = string.Empty;
                strMessage = "Select Patient Visit First !";
                MessageBoxControl.MessageBoxChildWindow msgW1 =
                   new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                msgW1.Show();
                return;
            }
        }
    }
}

