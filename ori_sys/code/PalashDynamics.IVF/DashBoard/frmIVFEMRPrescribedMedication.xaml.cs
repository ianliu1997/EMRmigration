using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CIMS;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.UserControls;
using PalashDynamics.Service.PalashTestServiceReference;
using PalashDynamics.ValueObjects.EMR;

namespace PalashDynamics.IVF.DashBoard
{
    public partial class frmIVFEMRPrescribedMedication : ChildWindow
    {
        bool IsFromPrescription = false;
        WaitIndicator Indicatior;
        public clsVisitVO CurrentVisit { get; set; }
        public List<clsPatientPrescriptionDetailVO> SelectedDrugList;
        List<clsPatientPrescriptionDetailVO> DrugList;
        public event RoutedEventHandler OnAddButton_Click;

        public frmIVFEMRPrescribedMedication()
        {
            InitializeComponent();
        }

        public frmIVFEMRPrescribedMedication(bool IsFromPrescription)
        {
            InitializeComponent();
            this.IsFromPrescription = IsFromPrescription;
            if (IsFromPrescription == true)
                this.Title = "Drug From Prescription";
            else
                this.Title = "Drug From Current Medication";

           

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (SelectedDrugList == null)
                SelectedDrugList = new List<clsPatientPrescriptionDetailVO>();
            DrugList = new List<clsPatientPrescriptionDetailVO>();
            if (CurrentVisit.OPDIPD)
            {
                LBLHeader.Content = "Admission List";
            }
            ShowPrescriptionVisitWise();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            if (OnAddButton_Click != null)
                OnAddButton_Click(this, new RoutedEventArgs());
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void dgVisitDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVisitDetails.SelectedItem != null)
            {
                try
                {
                    clsGetPatientCurrentMedicationDetailsBizActionVO BizActionCurMed = new clsGetPatientCurrentMedicationDetailsBizActionVO();
                    if (CurrentVisit != null)
                    {
                        BizActionCurMed.VisitID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).VisitId;  //CurrentVisit.ID;
                        BizActionCurMed.DoctorID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).DoctorID; //CurrentVisit.DoctorID;
                        BizActionCurMed.PatientID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).PatientId; //CurrentVisit.PatientId;
                        BizActionCurMed.PatientUnitID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).PatientUnitId;  //CurrentVisit.PatientUnitId;CurrentVisit.PatientUnitId;
                        BizActionCurMed.UnitID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).UnitId;  // CurrentVisit.UnitId;
                    }
                    BizActionCurMed.IsPrevious = true;
                    if (IsFromPrescription == true)
                        BizActionCurMed.IsFromPresc = true;
                    else
                        BizActionCurMed.IsFromPresc = false;
                    BizActionCurMed.IsOPDIPD = CurrentVisit.OPDIPD;
                    //BizActionCurMed.PrescriptionID = obj.PrescriptionID;
                    BizActionCurMed.PrescriptionID = ((clsVisitEMRDetails)dgVisitDetails.SelectedItem).PrescriptionID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        DrugList.Clear();
                        if (args.Error == null && args.Result != null)
                        {
                            if (((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList != null)
                            {
                                DrugList = ((clsGetPatientCurrentMedicationDetailsBizActionVO)args.Result).PatientCurrentMedicationDetailList;
                            }
                            Indicatior.Close();
                        }
                        else
                        {
                            Indicatior.Close();
                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DrugList;
                        dgDrugList.UpdateLayout();
                    };
                    client.ProcessAsync(BizActionCurMed, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                catch (Exception)
                {
                    Indicatior.Close();
                }
            }
        }

        private void chkMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugList.SelectedItem != null)
            {
                try
                {
                    CheckBox chk = (CheckBox)sender;
                    System.Text.StringBuilder strError = new System.Text.StringBuilder();
                    if (chk.IsChecked == true)
                    {
                        if (SelectedDrugList.Count > 0)
                        {
                            var item = from r in SelectedDrugList
                                       where r.DrugName == ((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).DrugName
                                       select r;
                            if (item.ToList().Count > 0)
                            {
                                if (strError.ToString().Length > 0)
                                    strError.Append(",");
                                strError.Append(((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem).DrugName);

                                if (!string.IsNullOrEmpty(strError.ToString()))
                                {
                                    string strMsg = "DRUG ALREADY SELECTED : " + strError.ToString();

                                    MessageBoxControl.MessageBoxChildWindow msgW1 =
                                               new MessageBoxControl.MessageBoxChildWindow("Palash", strMsg, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                                    msgW1.Show();
                                }
                            }
                            else
                            {
                                SelectedDrugList.Add((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem);
                            }
                        }
                        else
                        {
                            SelectedDrugList.Add((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem);
                        }
                        foreach (var item in SelectedDrugList)
                        {
                            item.IsSelected = true;
                        }
                    }
                    else
                        SelectedDrugList.Remove((clsPatientPrescriptionDetailVO)dgDrugList.SelectedItem);
                    dgDrugSelectedList.ItemsSource = null;
                    dgDrugSelectedList.ItemsSource = SelectedDrugList;
                    dgDrugSelectedList.UpdateLayout();
                    dgDrugSelectedList.Focus();

                }
                catch (Exception)
                { }

            }
        }

        private void chkSelMultipleDrug_Click(object sender, RoutedEventArgs e)
        {
            if (dgDrugSelectedList.SelectedItem != null)
            {
                try
                {
                    if (((CheckBox)sender).IsChecked == false)
                    {
                        long DrugID = ((clsPatientPrescriptionDetailVO)dgDrugSelectedList.SelectedItem).DrugID;
                        this.SelectedDrugList.Remove((clsPatientPrescriptionDetailVO)dgDrugSelectedList.SelectedItem);
                        foreach (var Drug in DrugList.Where(x => x.DrugID == DrugID))
                        {
                            Drug.IsSelected = false;
                        }
                        dgDrugList.ItemsSource = null;
                        dgDrugList.ItemsSource = DrugList;
                        dgDrugList.UpdateLayout();
                        dgDrugList.Focus();

                        dgDrugSelectedList.ItemsSource = null;
                        dgDrugSelectedList.ItemsSource = SelectedDrugList;
                        dgDrugSelectedList.UpdateLayout();
                    }
                }
                catch (Exception)
                { }
            }
        }
        private void ShowPrescriptionVisitWise()
        {
            Indicatior = new WaitIndicator();
            try
            {
                Indicatior.Show();
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
                BizActionCurMed.IsOPDIPD = CurrentVisit.OPDIPD;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        if (((clsGetPatientPrescriptionandVisitDetailsBizActionVO)args.Result).PatientVisitEMRDetails != null)
                        {
                            dgVisitDetails.ItemsSource = ((clsGetPatientPrescriptionandVisitDetailsBizActionVO)args.Result).PatientVisitEMRDetails;
                            if (dgVisitDetails.ItemsSource != null)
                                dgVisitDetails.SelectedIndex = 0;
                        }
                        Indicatior.Close();
                    }
                    else
                    {
                        Indicatior.Close();
                    }
                };
                client.ProcessAsync(BizActionCurMed, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            catch (Exception)
            {
                Indicatior.Close();
            }
        }
    }
}


