using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PalashDynamics.ValueObjects.Administration;
using CIMS;
using PalashDynamics.ValueObjects.Patient;
using PalashDynamics.ValueObjects.OutPatientDepartment.Registration.OPDPatientMaster;
using PalashDynamics.ValueObjects.OutPatientDepartment;
using PalashDynamics.Service.PalashTestServiceReference;
using System;
using PalashDynamics.ValueObjects.Master;
using PalashDynamics.ValueObjects;
using PalashDynamics.ValueObjects.EMR.NewEMR;
using System.Windows.Data;
using System.Windows.Controls.Primitives;
using PalashDynamics.UserControls;
using System.Windows.Browser;
using PalashDynamics.ValueObjects.EMR;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using System.IO;
using System.Net;
using OPDModule;
//using System.Windows.Navigation;
namespace EMR
{
    public partial class frmEMR : UserControl, GetVisitData, IInitiateCIMSIVF
    {
        #region Data Members
        List<clsMenuVO> MenuList = new List<clsMenuVO>();
        clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedPatient;
        public clsVisitVO CurrentVisit { get; set; }
        long RoleID = ((clsUserRoleVO)((clsUserGeneralDetailVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserGeneralDetailVO).RoleDetails).ID;
        long AdminRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).AdminRoleID;
        long NurseRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).NurseRoleID;
        long DoctorRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).DoctorRoleID;
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        WaitIndicator indicator;
        #endregion

        #region Constructor
        public frmEMR()
        {
            InitializeComponent();
            tvPatientEMR.Width = 170;
            this.Loaded += new RoutedEventHandler(frmEMR_Loaded);
            indicator = new WaitIndicator();
            ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            PART_MaximizeToggle.IsChecked = true;
            // ((IApplicationConfiguration)App.Current).IsFromQueueManagMentFlag = true;


        }
        #endregion

        //added by neena
        #region IInitiateCIMS Members

        bool IsArt = false;
        long PlanTherapyId;
        long PlanTherapyUnitId;
        long PatientID;
        long PatientUnitID;
        long GridIndex = 0;
        public void Initiate(clsMenuVO Item)
        {
            IsArt = Item.IsArt;
            PlanTherapyId = Item.PlanTherapyId;
            PlanTherapyUnitId = Item.PlanTherapyUnitId;
            PatientID = Item.PatientID;
            PatientUnitID = Item.PatientUnitID;
            GridIndex = Item.GridIndex;
            if (IsArt == true)
                cmdArt.Visibility = Visibility.Visible;
        }
        #endregion

        //
        Boolean FromDiagnosis = false;
        public void Test(clsVisitVO CurrentVisit1)
        {
            CurrentVisit = CurrentVisit1;
            FromDiagnosis = true;
        }

        void HistoryMenu(int TemplateID)
        {
            frmEMRPastHistory winPastHist1 = new frmEMRPastHistory();
            winPastHist1.CurrentVisit = this.CurrentVisit;
            winPastHist1.IsEnabledControl = this.CurrentVisit.VisitStatus;
            if (RoleID == NurseRoleID)
                winPastHist1.SelectedUser = "Nurse";

            if (RoleID == DoctorRoleID)
                winPastHist1.SelectedUser = "Doctor";

            if (RoleID == AdminRoleID)
                winPastHist1.SelectedUser = "Admin";

            winPastHist1.TemplateID = TemplateID;
            winPastHist1.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
            EMR_RightCorner.Content = winPastHist1;
        }
        void tvi_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvItem = sender as TreeViewItem;
            if (CurrentVisit == null)
            {
                CurrentVisit = new clsVisitVO();
                CurrentVisit.ID = SelectedPatient.VisitID;
                CurrentVisit.PatientId = SelectedPatient.PatientID;
                CurrentVisit.PatientUnitId = SelectedPatient.PatientUnitID;
                CurrentVisit.UnitId = SelectedPatient.VisitUnitId;
                CurrentVisit.OPDNO = SelectedPatient.OPDNO;
                CurrentVisit.MRNO = SelectedPatient.MRNo;
                CurrentVisit.DoctorID = SelectedPatient.DoctorID;
                //CurrentVisit.Date = SelectedPatient.VisitDate;
            }
            CurrentVisit.OPDIPD = false;
            var ccSelected = ((this.FindName("EMR_RightCorner") as ContentControl).Content);
            Boolean blsIsSave = false;
            if (ccSelected != null)
            {
                switch (ccSelected.GetType().Name)
                {
                    //case "frmCurrentMedication":
                    //    frmCurrentMedication objForm = ccSelected as frmCurrentMedication;
                    //    objForm.cmdSave_Click(null, null);

                    //    break;
                    //case "frmEMRChiefComplaint":
                    //    frmEMRChiefComplaint objCCForm = ccSelected as frmEMRChiefComplaint;
                    //    objCCForm.cmdSave_Click(null, null);
                    //    blsIsSave = true;
                    //    break;
                }
            }
            if (!blsIsSave)
            {
                switch (tvItem.Name)
                {
                    case "EMR.frmEMRmaleinfertility":
                        {
                            if (SelectedPatient.GenderID == 1)
                            {
                                HistoryMenu(61);
                            }
                            else
                            {
                                string sMsgText = "Applicable For Male Only";
                                ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            }
                        }
                        break;
                    case "EMR.FrmCostEstimate":
                        FrmCostEstimate winCostEstimate = new FrmCostEstimate();
                        winCostEstimate.CurrentVisit = CurrentVisit;
                        EMR_RightCorner.Content = winCostEstimate;
                        break;
                    case "EMR.PatientEMRDashboard":
                        frmPatientEMRDashboard winDashborad = new frmPatientEMRDashboard();
                        winDashborad.CurrentVisit = CurrentVisit;
                        EMR_RightCorner.Content = winDashborad;
                        break;
                    case "EMR.frmEMRCompoundMedication":
                        frmEMRCompoundMedication winCompdMedication = new frmEMRCompoundMedication();
                        winCompdMedication.CurrentVisit = this.CurrentVisit;
                        winCompdMedication.IsEnableControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winCompdMedication;
                        break;
                    case "EMR.frmFollowpNotes":
                        frmFollowpNotes winFollowUpnote = new frmFollowpNotes();
                        winFollowUpnote.CurrentVisit = this.CurrentVisit;
                        winFollowUpnote.IsEnableControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winFollowUpnote;
                        if (RoleID == NurseRoleID)
                            winFollowUpnote.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winFollowUpnote.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winFollowUpnote.SelectedUser = "Admin";
                        winFollowUpnote.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        break;
                    case "EMR.Procedure":
                        frmEMRProcedure winProcedure = new frmEMRProcedure();
                        winProcedure.IsArt = IsArt;
                        winProcedure.PlanTherapyId = PlanTherapyId;
                        winProcedure.PlanTherapyUnitId = PlanTherapyUnitId;
                        winProcedure.CurrentVisit = CurrentVisit;
                        winProcedure.IsEnabledControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winProcedure;
                        if (RoleID == NurseRoleID)
                            winProcedure.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winProcedure.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winProcedure.SelectedUser = "Admin";
                        winProcedure.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        winProcedure.GenderID = SelectedPatient.GenderID;
                       
                        break;
                    case "EMR.PatientEMRDiagnosis":
                        frmDiagnosis winDiagnosis = new frmDiagnosis();
                        winDiagnosis.CurrentVisit = CurrentVisit;
                        winDiagnosis.ISaDiagnosisValidation = false;
                        winDiagnosis.IsEnabledControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winDiagnosis;
                        if (RoleID == NurseRoleID)
                            winDiagnosis.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winDiagnosis.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winDiagnosis.SelectedUser = "Admin";
                        winDiagnosis.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        break;
                    case "EMR.PatientEMRHistory":
                        //frmEMRPastHistory winPastHist = new frmEMRPastHistory();
                        //winPastHist.CurrentVisit = this.CurrentVisit;
                        //winPastHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        //if (RoleID == NurseRoleID)
                        //    winPastHist.SelectedUser = "Nurse";

                        //if (RoleID == DoctorRoleID)
                        //    winPastHist.SelectedUser = "Doctor";

                        //if (RoleID == AdminRoleID)
                        //    winPastHist.SelectedUser = "Admin";
                        //if (SelectedPatient.GenderID == 2)
                        //{
                        //    winPastHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("HistoryTemplateID"));// 105; // Past History Template ID
                        //}
                        //else
                        //{
                        //    winPastHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("HistoryMaleTemplateID"));// 105; // Past History Template ID
                        //}
                        //winPastHist.TemplateID = 243;

                        //winPastHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        //EMR_RightCorner.Content = winPastHist;
                        break;
                    case "EMR.PatientEMRVitals":
                        frmEMRVitals winVitals = new frmEMRVitals();
                        if (RoleID == NurseRoleID)
                            winVitals.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winVitals.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winVitals.SelectedUser = "Admin";
                        winVitals.IsEnabledControl = CurrentVisit.VisitStatus;
                        winVitals.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        winVitals.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("VitalTemplateID"));// 103;
                        winVitals.CurrentVisit = this.CurrentVisit;
                        EMR_RightCorner.Content = winVitals;
                        break;
                    case "EMR.PatientEMRReferral":
                        frmPatientEMRRefferal winReferral = new frmPatientEMRRefferal();
                        winReferral.CurrentVisit = CurrentVisit;
                        winReferral.IsEnabledControl = CurrentVisit.VisitStatus;
                        winReferral.PatientDetail = this.SelectedPatient;
                        EMR_RightCorner.Content = winReferral;
                        break;
                    case "EMR.PatientEMRRChiefComplaints":
                        frmEMRChiefComplaint winChiefComplaint = new frmEMRChiefComplaint();
                        winChiefComplaint.IsEnabledControl = CurrentVisit.VisitStatus;
                        winChiefComplaint.CurrentVisit = CurrentVisit;
                        EMR_RightCorner.Content = winChiefComplaint;
                        break;
                    case "EMR.PatientEMRAllergies":
                        frmEMRAllergies winAllergies = new frmEMRAllergies();
                        winAllergies.CurrentVisit = this.CurrentVisit;
                        winAllergies.IsControlEnable = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winAllergies;
                        break;
                    case "EMR.frmEMRClinicalHistory":
                        frmEMRClinicalHistory winClinicalHist = new frmEMRClinicalHistory();
                        winClinicalHist.CurrentVisit = this.CurrentVisit;
                        winClinicalHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winClinicalHist.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winClinicalHist.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winClinicalHist.SelectedUser = "Admin";
                        winClinicalHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("ClinicalHistoryTemplateID"));// 105; // Clinical History Template ID
                        winClinicalHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winClinicalHist;
                        break;
                    case "EMR.frmEMRPastHistory":
                        frmEMRPastHistory winPastHistory = new frmEMRPastHistory();
                        winPastHistory.CurrentVisit = this.CurrentVisit;
                        winPastHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winPastHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winPastHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winPastHistory.SelectedUser = "Admin";

                        winPastHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("PastHistoryTemplateID"));// 105; // Past History Template ID
                        winPastHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winPastHistory;
                        break;
                    case "EMR.frmEMRMaleHistory1":
                        if (SelectedPatient.GenderID == 1)
                        {
                            HistoryMenu(22);
                            // HistoryMenu(243);
                        }
                        else
                        {
                            string sMsgText = "Applicable For Male Only";
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                        break;

                    case "EMR.frmEMRFemaleHistory":
                        if (SelectedPatient.GenderID == 2)
                        {
                            HistoryMenu(19);
                        }
                        else
                        {
                            string sMsgText = "Applicable For Female Only";
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                        break;

                    case "EMR.PatientEMRHysteroscopy":
                        if (SelectedPatient.GenderID == 2)
                        {
                            frmEMRCommonHistory winPastHist1 = new frmEMRCommonHistory();
                            winPastHist1.CurrentVisit = this.CurrentVisit;
                            winPastHist1.IsEnabledControl = this.CurrentVisit.VisitStatus;
                            if (RoleID == NurseRoleID)
                                winPastHist1.SelectedUser = "Nurse";

                            if (RoleID == DoctorRoleID)
                                winPastHist1.SelectedUser = "Doctor";

                            if (RoleID == AdminRoleID)
                                winPastHist1.SelectedUser = "Admin";

                            winPastHist1.TemplateID = 23;
                            //winPastHist1.TemplateID = 246;
                            winPastHist1.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                            EMR_RightCorner.Content = winPastHist1;
                        }
                        else
                        {
                            string sMsgText = "Applicable For Female Only";
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                        break;
                    case "EMR.PatientEMRLaparoscopy":
                        if (SelectedPatient.GenderID == 2)
                        {
                            frmEMRCommonHistory winPastHistLap = new frmEMRCommonHistory();
                            winPastHistLap.CurrentVisit = this.CurrentVisit;
                            winPastHistLap.IsEnabledControl = this.CurrentVisit.VisitStatus;
                            if (RoleID == NurseRoleID)
                                winPastHistLap.SelectedUser = "Nurse";

                            if (RoleID == DoctorRoleID)
                                winPastHistLap.SelectedUser = "Doctor";

                            if (RoleID == AdminRoleID)
                                winPastHistLap.SelectedUser = "Admin";

                            winPastHistLap.TemplateID = 24;
                            winPastHistLap.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                            EMR_RightCorner.Content = winPastHistLap;
                        }
                        else
                        {
                            string sMsgText = "Applicable For Female Only.";
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        }
                        break;
                    case "EMR.frmEMRCoupleHistory":
                        HistoryMenu(114);
                        break;
                    case "EMR.frmFemaleFindings":
                        HistoryMenu(27);
                        break;
                    case "EMR.frmEMRUltraSonography":
                        HistoryMenu(28);
                        break;
                    case "EMR.frmEMRHysteroscopy":
                        HistoryMenu(23);
                        break;
                    case "EMR.frmEMRLaproscopy":
                        HistoryMenu(24);
                        break;
                    case "EMR.frmEMRTBPCR":
                        HistoryMenu(30);
                        break;
                    case "EMR.frmEMRPCT":
                        HistoryMenu(44);
                        break;
                    case "EMR.frmEMRHSG":
                        HistoryMenu(62);
                        break;
                    case "EMR.frmEMRMaleHistory":
                        HistoryMenu(22);
                        break;
                    case "EMR.frmEMRMaleFindings":
                        HistoryMenu(43);
                        break;
                    case "EMR.frmEMRSemenCultureAndSensitivity":
                        HistoryMenu(39);
                        break;
                    case "EMR.frmEMRTESE":
                        HistoryMenu(63);
                        break;
                    case "EMR.frmEMRSemenSurvival":
                        HistoryMenu(38);
                        break;
                    case "EMR.frmEMRPersonalHistory":
                        frmEMRPersonalHistory winPesrHist = new frmEMRPersonalHistory();
                        winPesrHist.CurrentVisit = this.CurrentVisit;
                        winPesrHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winPesrHist.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winPesrHist.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winPesrHist.SelectedUser = "Admin";
                        winPesrHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("PersonalHistoryTemplateID"));// 105; // Personal History Template ID
                        winPesrHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winPesrHist;
                        break;
                    case "EMR.frmEMRFamilyHistory":
                        frmEMRFamilyHistory winFamilyHist = new frmEMRFamilyHistory();
                        winFamilyHist.CurrentVisit = this.CurrentVisit;
                        winFamilyHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winFamilyHist.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winFamilyHist.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winFamilyHist.SelectedUser = "Admin";
                        winFamilyHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("FamilyHistoryTemplateID"));// 105; // Family History Template ID
                        winFamilyHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winFamilyHist;
                        break;
                    case "EMR.frmEMRSurgicalHistory":
                        frmEMRSurgicalHistory WinSurgicalHistory = new frmEMRSurgicalHistory();
                        WinSurgicalHistory.CurrentVisit = this.CurrentVisit;
                        WinSurgicalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            WinSurgicalHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            WinSurgicalHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            WinSurgicalHistory.SelectedUser = "Admin";
                        WinSurgicalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("SurgicalHistoryTemplateID"));// 105; // Family History Template ID
                        WinSurgicalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = WinSurgicalHistory;
                        break;
                    case "EMR.frmEMRTreatmentHistory":
                        frmEMRTreatmentHistory WinTreatmentHistory = new frmEMRTreatmentHistory();
                        WinTreatmentHistory.CurrentVisit = this.CurrentVisit;
                        WinTreatmentHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            WinTreatmentHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            WinTreatmentHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            WinTreatmentHistory.SelectedUser = "Admin";
                        WinTreatmentHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("TreatmentHistoryTemplateID"));// 105; // Family History Template ID
                        WinTreatmentHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = WinTreatmentHistory;
                        break;
                    case "EMR.frmEMRObstetricalHistory":
                        frmEMRObstetricalHistory WinObstetricalHistory = new frmEMRObstetricalHistory();
                        WinObstetricalHistory.CurrentVisit = this.CurrentVisit;
                        WinObstetricalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            WinObstetricalHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            WinObstetricalHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            WinObstetricalHistory.SelectedUser = "Admin";
                        WinObstetricalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("ObstetricalHistoryTemplateID"));// 105; // Family History Template ID
                        WinObstetricalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = WinObstetricalHistory;
                        break;
                    case "EMR.frmEMRMaritalHistory":
                        frmEMRMaritalHistory WinMaritalHistory = new frmEMRMaritalHistory();
                        WinMaritalHistory.CurrentVisit = this.CurrentVisit;
                        WinMaritalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            WinMaritalHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            WinMaritalHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            WinMaritalHistory.SelectedUser = "Admin";
                        WinMaritalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("MaritalHistoryTemplateID"));// 105; // Family History Template ID
                        WinMaritalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = WinMaritalHistory;
                        break;
                    case "EMR.frmEMRMenstrualHistory":
                        frmEMRMenstrualHistory WinMenstrualHistory = new frmEMRMenstrualHistory();
                        WinMenstrualHistory.CurrentVisit = this.CurrentVisit;
                        WinMenstrualHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            WinMenstrualHistory.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            WinMenstrualHistory.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            WinMenstrualHistory.SelectedUser = "Admin";
                        WinMenstrualHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("MenstrualHistoryTemplateID"));// 105; // Family History Template ID
                        WinMenstrualHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = WinMenstrualHistory;
                        break;
                    case "EMR.frmEMRERPerformanceHistory":
                        frmEMRERPerformanceHistory winERPHist = new frmEMRERPerformanceHistory();
                        winERPHist.CurrentVisit = this.CurrentVisit;
                        winERPHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winERPHist.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winERPHist.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winERPHist.SelectedUser = "Admin";
                        winERPHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("ERPTemplateID"));// 111; // Family History Template ID
                        winERPHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winERPHist;
                        break;
                    case "EMR.frmEMRInvestigation":
                        frmEMRInvestigation winInvestigation = new frmEMRInvestigation();
                        winInvestigation.CurrentVisit = this.CurrentVisit;
                        winInvestigation.IsEnableControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winInvestigation;
                        break;
                    case "EMR.frmEMRMEdication":
                        frmEMRMedication winMedication = new frmEMRMedication();
                        winMedication.CurrentVisit = this.CurrentVisit;
                        winMedication.IsEnableControl = CurrentVisit.VisitStatus;
                        //added by neena
                        winMedication.IsArt = IsArt;
                        winMedication.PlanTherapyId = PlanTherapyId;
                        winMedication.PlanTherapyUnitId = PlanTherapyUnitId;
                        //
                        EMR_RightCorner.Content = winMedication;
                        break;
                    case "EMR.frmEMROtherServices":
                        frmEMROtherServices winOtherServices = new frmEMROtherServices();
                        winOtherServices.CurrentVisit = this.CurrentVisit;
                        winOtherServices.IsEnabledControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winOtherServices;
                        break;
                    case "EMR.frmEMRFollowup":
                        frmEMRFollowup winFollowUp = new frmEMRFollowup();
                        winFollowUp.CurrentVisit = this.CurrentVisit;
                        winFollowUp.IsEnableControl = CurrentVisit.VisitStatus;
                        this.EMR_RightCorner.Content = winFollowUp;
                        break;
                    case "EMR.PatientEMRPhysicalExam":
                        frmEMRPhysicalExam winPhysicalExam = new frmEMRPhysicalExam();
                        winPhysicalExam.CurrentVisit = this.CurrentVisit;
                        winPhysicalExam.IsEnabledControl = this.CurrentVisit.VisitStatus;
                        if (RoleID == NurseRoleID)
                            winPhysicalExam.SelectedUser = "Nurse";

                        if (RoleID == DoctorRoleID)
                            winPhysicalExam.SelectedUser = "Doctor";

                        if (RoleID == AdminRoleID)
                            winPhysicalExam.SelectedUser = "Admin";
                        winPhysicalExam.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        EMR_RightCorner.Content = winPhysicalExam;
                        break;
                    case "EMR.frmEMRCurrentMedication":
                        frmCurrentMedication winCurrentMedication = new frmCurrentMedication();
                        winCurrentMedication.CurrentVisit = this.CurrentVisit;
                        winCurrentMedication.IsEnabledControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winCurrentMedication;
                        break;
                    case "EMR.frmEMRInvestigationDocs":
                        frmEMRInvestigationDocs winInvest = new frmEMRInvestigationDocs();
                        EMR_RightCorner.Content = winInvest;
                        break;
                    case "EMR.frmEMREducation":
                        frmEMREducation winEducation = new frmEMREducation();
                        winEducation.CurrentVisit = CurrentVisit;
                        winEducation.IsEnabledControl = CurrentVisit.VisitStatus;
                        EMR_RightCorner.Content = winEducation;
                        break;
                    case "EMR.frmGrowthChart":
                        frmViewGrowthChart winGC;
                        string PatientName = ((IApplicationConfiguration)App.Current).SelectedPatient.PatientName;
                        string mrNo = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        long longDrID = 0;
                        longDrID = ((IApplicationConfiguration)App.Current).SelectedPatient.VisitID;
                        int genderid = 0;
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.Gender.ToUpper() == "FEMALE")
                            genderid = 2;
                        else
                            genderid = 1;
                        DateTime Date2 = DateTime.Now;
                        DateTime Date1;
                        if (((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth != null)
                        {
                            Date1 = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirth);
                        }
                        else
                        {
                            Date1 = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedPatient.DateOfBirthFromAge);
                        }
                        int maxAgeInMonth = (Date2.Year - Date1.Year) * 12 + Date2.Month - Date1.Month;
                        if (maxAgeInMonth > 24 && maxAgeInMonth < 241)
                        {
                            winGC = new frmViewGrowthChart(false, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, longDrID, PatientName, mrNo, genderid);
                            EMR_RightCorner.Content = winGC;
                        }
                        else if (maxAgeInMonth <= 24)
                        {
                            winGC = new frmViewGrowthChart(true, ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID, longDrID, PatientName, mrNo, genderid);
                            EMR_RightCorner.Content = winGC;
                        }
                        else
                        {
                            string sMsgText = "Only for 0 - 20 years of age.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("GCAgeLimit_Msg");
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            frmPatientEMRDashboard winDashboard = new frmPatientEMRDashboard();
                            winDashboard.CurrentVisit = CurrentVisit;
                            EMR_RightCorner.Content = winDashboard;
                        }
                        break;
                }
            }
        }

        private void Visibible()
        {
            ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            PART_MaximizeToggle.IsChecked = false;
            PART_MaximizeToggle.Visibility = Visibility.Visible;

            HyperlinkButton DesignBar = (HyperlinkButton)rootPage.FindName("DesignBar");
            DesignBar.Visibility = Visibility.Visible;

            ToggleButton CmdGraphicalAnalysis = (ToggleButton)rootPage.FindName("CmdGraphicalAnalysis");
            CmdGraphicalAnalysis.Visibility = Visibility.Visible;

            ToggleButton CmdcryoBank = (ToggleButton)rootPage.FindName("CmdcryoBank");
            CmdcryoBank.Visibility = Visibility.Visible;

            ToggleButton CmdGotoHome = (ToggleButton)rootPage.FindName("CmdGotoHome");
            CmdGotoHome.Visibility = Visibility.Visible;

            ToggleButton cmdChangePassword = (ToggleButton)rootPage.FindName("cmdChangePassword");
            cmdChangePassword.Visibility = Visibility.Visible;
        }
        #region Loaded
        private void frmEMR_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SelectedPatient.IsFromAppointment)
            {
                if (SelectedPatient != null)
                {
                    if (SelectedPatient.MRNo != null)
                    {
                        if (SelectedPatient != null && SelectedPatient.MRNo.Length > 1)
                        {
                            if (tvPatientEMR != null && tvPatientEMR.Items.Count <= 0)
                            {
                                GetPatientCurrentVisit();
                                GetEMRMenuTree();
                                //if (!IsArt)
                                //Unloaded += frmEMR_Unloaded;
                            }
                        }
                        else
                        {
                            // Visibible();
                            ModuleName = "OPDModule";
                            Action = "CIMS.Forms.QueueManagement";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c = new WebClient();
                            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                            string sMsgText = "Please Select the Patient";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectPatient_Msg");
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Visibible();
                        ModuleName = "OPDModule";
                        Action = "CIMS.Forms.QueueManagement";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c = new WebClient();
                        c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                        c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                        string sMsgText = "Please Select the Patient";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectPatient_Msg");
                        ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Visibible();
                    ModuleName = "OPDModule";
                    Action = "CIMS.Forms.QueueManagement";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                    string sMsgText = "Please Select the Patient";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectPatient_Msg");
                    ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            }
            else
            {
                if (SelectedPatient != null)
                {
                    if (SelectedPatient.MRNo != null)
                    {
                        if (SelectedPatient != null && SelectedPatient.MRNo.Length > 1)
                        {
                            if (tvPatientEMR != null && tvPatientEMR.Items.Count <= 0)
                            {
                                GetPatientCurrentVisit();
                                GetEMRMenuTree();
                            }
                        }
                        else
                        {
                            // Visibible();
                            ModuleName = "OPDModule";
                            Action = "CIMS.Forms.AppointmentList";
                            UserControl rootPage = Application.Current.RootVisual as UserControl;
                            WebClient c = new WebClient();
                            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                            string sMsgText = "Please Register the Patient";
                            ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Visibible();
                        ModuleName = "OPDModule";
                        Action = "CIMS.Forms.AppointmentList";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c = new WebClient();
                        c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                        c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                        string sMsgText = "Please Register the Patient";
                        ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    //Visibible();
                    ModuleName = "OPDModule";
                    Action = "CIMS.Forms.AppointmentList";
                    UserControl rootPage = Application.Current.RootVisual as UserControl;
                    WebClient c = new WebClient();
                    c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                    c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

                    string sMsgText = "Please Select the Patient";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectPatient_Msg");
                    ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        void frmEMR_Unloaded(object sender, RoutedEventArgs e)
        {
            clsGetEMRVisitDignosisiValidationVo BizActionVisit = new clsGetEMRVisitDignosisiValidationVo();
            BizActionVisit.VisitID = SelectedPatient.VisitID;
            BizActionVisit.PatientID = SelectedPatient.PatientID;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
            PalashServiceClient clientVisit = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            clientVisit.ProcessCompleted += (sVisit, argVisit) =>
            {
                if (argVisit.Error == null && argVisit.Result != null)
                {
                    if (((clsGetEMRVisitDignosisiValidationVo)argVisit.Result).SuccessStatus == 0 && CurrentVisit.EMRModVisitDate >= DateTime.Now) //&& CurrentVisit.Date.ToString("d") == DateTime.Now.ToString("d"))
                    {
                        ModuleName = "EMR";
                        Action = "EMR.frmEMR";
                        UserControl rootPage = Application.Current.RootVisual as UserControl;
                        WebClient c = new WebClient();
                        this.CurrentVisit = CurrentVisit;
                        c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival2);
                        c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                    }
                    else
                    {
                        ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                        PART_MaximizeToggle.IsChecked = false;
                        PART_MaximizeToggle.Visibility = Visibility.Visible;

                        HyperlinkButton DesignBar = (HyperlinkButton)rootPage.FindName("DesignBar");
                        DesignBar.Visibility = Visibility.Visible;

                        ToggleButton CmdGraphicalAnalysis = (ToggleButton)rootPage.FindName("CmdGraphicalAnalysis");
                        CmdGraphicalAnalysis.Visibility = Visibility.Visible;

                        ToggleButton CmdcryoBank = (ToggleButton)rootPage.FindName("CmdcryoBank");
                        CmdcryoBank.Visibility = Visibility.Visible;

                        ToggleButton CmdGotoHome = (ToggleButton)rootPage.FindName("CmdGotoHome");
                        CmdGotoHome.Visibility = Visibility.Visible;

                        ToggleButton cmdChangePassword = (ToggleButton)rootPage.FindName("cmdChangePassword");
                        cmdChangePassword.Visibility = Visibility.Visible;

                        ToggleButton cmdIVFDashBoard = (ToggleButton)rootPage.FindName("cmdIVFDashBoard");
                        cmdIVFDashBoard.Visibility = Visibility.Visible;

                        ((IApplicationConfiguration)App.Current).SelectedPatient = null;
                    }
                }

            };
            clientVisit.ProcessAsync(BizActionVisit, ((IApplicationConfiguration)App.Current).CurrentUser);
            clientVisit.CloseAsync();
        }
        void c_OpenReadCompleted_maleSemenSurvival2(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((GetVisitData)myData).Test(CurrentVisit);
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                Header.Text = "Patient Queue List";
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                PART_MaximizeToggle.IsChecked = true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Private MEthods
        private void BuildMenu(List<clsMenuVO> Items, TreeViewItem rootItem, long ID, string Parent)
        {
            string gender = ((IApplicationConfiguration)App.Current).SelectedPatient.Gender;
            List<clsMenuVO> ChildList;
            // find immediate child of parent elemt
            if (gender.ToUpper() == "MALE")
            {
                //,,491,,,495,496,497,498,499,500,501,502,503,504
                //ChildList = Items.Where(z => z.ParentId == ID && z.ID != 489 && z.ID != 490 && z.ID != 491 && z.ID != 493 && z.ID != 494 && z.ID != 495 && z.ID != 496 && z.ID != 497 && z.ID != 498 && z.ID != 499 && z.ID != 500 && z.ID != 501 && z.ID != 502 && z.ID != 503 && z.ID != 504).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = Items.Where(z => z.ParentId == ID && z.ID != 469 && z.ID != 471 && z.ID != 473 && z.ID != 526 && z.ID != 527 && z.ID != 528 && z.ID != 529 && z.ID != 530 && z.ID != 539 && z.ID != 540 && z.ID != 541 && z.ID != 542 && z.ID != 543 && z.ID != 544 && z.ID != 545 ).OrderBy(or => or.MenuOrder).ToList();
                //if (Parent == "History")
                //{
                //  ChildList = Items.Where(z => z.ParentId == ID && z.ISFemale == false).OrderBy(or => or.MenuOrder).ToList();
                //}
                //else
                //{
                ChildList = Items.Where(z => z.ParentId == ID).OrderBy(or => or.MenuOrder).ToList();
                //}
            }
            else
            {
                //if (Parent == "History")
                //{
                //ChildList = Items.Where(z => z.ParentId == ID && z.ID != 505 && z.ID != 506 && z.ID != 507 && z.ID != 508 && z.ID != 509).OrderBy(or => or.MenuOrder).ToList();
                // ChildList = Items.Where(z => z.ParentId == ID && z.ID != 546 && z.ID != 547 && z.ID != 548 && z.ID != 549 && z.ID != 550).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = Items.Where(z => z.ParentId == ID && z.ISFemale == true).OrderBy(or => or.MenuOrder).ToList();
                //}
                //else
                //{
                ChildList = Items.Where(z => z.ParentId == ID).OrderBy(or => or.MenuOrder).ToList();
                //}
            }

            foreach (clsMenuVO menu in ChildList)
            {
                try
                {
                    TreeViewItem _rootItem = new TreeViewItem(); // add root elemt to tree 
                    _rootItem.Header = menu.Header;
                    _rootItem.Name = menu.Action;
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    Image img = new Image();
                    img.SetBinding(Image.SourceProperty, new Binding("ImagePath"));
                    img.Margin = new Thickness(4, 4, 4, 4);
                    img.Height = 16;
                    sp.Children.Add(img);

                    Label lbl = new Label();
                    lbl.Content = menu.Title;
                    ToolTipService.SetToolTip(lbl, menu.Title);
                    // Add into stack
                    sp.Children.Add(lbl);
                    // assign stack to header
                    _rootItem.Header = sp;
                    _rootItem.DataContext = menu;
                    rootItem.Items.Add(_rootItem); // add item to parent item
                    _rootItem.Selected += new RoutedEventHandler(tvi_Selected);
                    BuildMenu(Items, _rootItem, menu.ID, menu.Parent); // recursive call 
                }
                catch (Exception Ex)
                {
                    throw;
                }
            }
        }

        private void GetPatientCurrentVisit()
        {
            clsGetEMRVisitBizActionVO BizActionVisit = new clsGetEMRVisitBizActionVO();
            BizActionVisit.Details = new clsVisitVO();
            if (SelectedPatient.VisitDate == DateTime.Today)
            {
                BizActionVisit.GetLatestVisit = true;
            }
            BizActionVisit.Details.PatientId = SelectedPatient.PatientID;
            BizActionVisit.Details.PatientUnitId = SelectedPatient.UnitId;
            BizActionVisit.Details.UnitId = ((IApplicationConfiguration)App.Current).CurrentUser.UserLoginInfo.UnitId;
            BizActionVisit.Details.ID = SelectedPatient.VisitID;
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy                
                PalashServiceClient clientVisit = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                clientVisit.ProcessCompleted += (sVisit, argVisit) =>
                {
                    if (argVisit.Error == null && argVisit.Result != null)
                    {
                        if (SelectedPatient.IsFromAppointment)
                        {
                            if (((clsGetEMRVisitBizActionVO)argVisit.Result).Details.ID == 0 )
                            {
                                ModuleName = "OPDModule";
                                Action = "CIMS.Forms.AppointmentList";
                                UserControl rootPage = Application.Current.RootVisual as UserControl;
                                WebClient c = new WebClient();
                                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
                                string sMsgText = "visit is not marked for the patient";
                                ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                            }
                        }
                        BizActionVisit = (clsGetEMRVisitBizActionVO)argVisit.Result;
                        CurrentVisit = ((clsGetEMRVisitBizActionVO)argVisit.Result).Details;

                        // added by EMR Changes Added by Ashish Z. on dated 31052017
                        CurrentVisit.EMRModVisitDate = CurrentVisit.Date;
                        CurrentVisit.EMRModVisitDate = CurrentVisit.EMRModVisitDate.AddDays(((IApplicationConfiguration)App.Current).ApplicationConfigurations.EMRModVisitDateInDays);
                        //End


                        CurrentVisit.MRNO = ((IApplicationConfiguration)App.Current).SelectedPatient.MRNo;
                        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                        {
                            CurrentVisit.DoctorCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
                        }
                        PalashDynamics.SearchResultLists.DisplayPatientDetails winDisplay = new PalashDynamics.SearchResultLists.DisplayPatientDetails();
                        winDisplay.MinWidth = this.ActualWidth;
                        ResultListContent.Content = winDisplay;
                        string str = "    " + CurrentVisit.Allergies;
                        if (str.Length > 150)
                        {
                            str = str.Substring(0, 150) + "...";
                        }
                        allergies.Content = String.Format(str);

                        //if (CurrentVisit.IsReferral)
                        //{
                        //    foreach (TreeViewItem tvItem in tvPatientEMR.Items)
                        //    {
                        //        if (((PalashDynamics.ValueObjects.Administration.clsMenuVO)(tvItem.DataContext)).Title == "Referral")
                        //        {
                        //            tvItem.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                        //        }
                        //    }
                        //}

                        //added by neena
                        if (CurrentVisit.CoupleMRNO != null)
                        {
                            if (CurrentVisit.CoupleMRNO.Trim() != "")
                            {
                                CoupleMrNo.Visibility = Visibility.Visible;
                                CoupleMRNO.Content = string.Format(CurrentVisit.CoupleMRNO);
                            }
                        }

                        if (CurrentVisit.DonorMRNO != null)
                        {
                            if (CurrentVisit.DonorMRNO.Trim() != "")
                            {
                                DonorMrNo.Visibility = Visibility.Visible;
                                DonorMRNO.Content = string.Format(CurrentVisit.DonorMRNO);
                            }
                        }

                        if (CurrentVisit.SurrogateMRNO != null)
                        {
                            if (CurrentVisit.SurrogateMRNO.Trim() != "")
                            {
                                SurrogateMrNo.Visibility = Visibility.Visible;
                                SurrogateMRNO.Content = string.Format(CurrentVisit.SurrogateMRNO);
                            }
                        }
                        //
                    }
                    indicator.Close();
                };
                clientVisit.ProcessAsync(BizActionVisit, ((IApplicationConfiguration)App.Current).CurrentUser);
                clientVisit.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }
        private void GetEMRMenuTree()
        {
            clsGetEMRMenuBizActionVO BizAction = new clsGetEMRMenuBizActionVO();
            if (indicator == null || indicator.Visibility == Visibility.Collapsed)
            {
                indicator = new WaitIndicator();
                indicator.Show();
            }
            try
            {
                Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
                client1.ProcessCompleted += (s1, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //int iEMRParent = Convert.ToInt32(DefaultValues.ResourceManager.GetString("EMRMenuParentID"));
                        MenuList = ((clsGetEMRMenuBizActionVO)args.Result).MenuDetails.OrderBy(z => z.MenuOrder).ToList();
                        List<clsMenuVO> lst = MenuList.Where(z => z.Parent == "Patient EMR").ToList();

                        foreach (clsMenuVO menu in lst)
                        {
                            TreeViewItem tvi = new TreeViewItem();
                            tvi.Header = menu.Title;
                            tvi.Name = menu.Action.ToString();
                            // create Image
                            StackPanel sp = new StackPanel();
                            sp.Orientation = Orientation.Horizontal;
                            Image img = new Image();
                            img.SetBinding(Image.SourceProperty, new System.Windows.Data.Binding("ImagePath"));
                            img.Margin = new Thickness(4, 4, 4, 4);
                            img.Height = Convert.ToDouble(DefaultValues.ResourceManager.GetString("EMRIconsHeight"));
                            sp.Children.Add(img);

                            Label lbl = new Label();
                            lbl.Content = menu.Title;
                            ToolTipService.SetToolTip(lbl, menu.Title);
                            //// Add into stack
                            sp.Children.Add(lbl);
                            //// assign stack to header
                            tvi.Header = sp;
                            tvi.DataContext = menu;
                            tvi.Selected += new RoutedEventHandler(tvi_Selected);
                            BuildMenu(MenuList, tvi, menu.ID, menu.Parent);
                            //tvi.Style = (Style)this.Resources["TreeViewContainerStyle"];
                            tvPatientEMR.Items.Add(tvi);
                        }
                        tvPatientEMR.MinHeight = this.ActualHeight - this.acc.ActualHeight - this.accItem.ActualHeight - this.allergies.ActualHeight;
                        //tvPatientEMR.MaxHeight = this.acc.ActualHeight;
                        ////tvPatientEMR.Height = this.acc.ActualHeight - 5;
                        tvPatientEMR.ExpandAll();
                        accItemEMRMenus.IsSelected = true;
                        if (CurrentVisit != null && CurrentVisit.IsReferral)
                        {
                            foreach (TreeViewItem tvItem in tvPatientEMR.Items)
                            {
                                if (((PalashDynamics.ValueObjects.Administration.clsMenuVO)(tvItem.DataContext)).Title == "Referral")
                                {
                                    tvItem.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                                }
                            }
                        }
                        // By default DashBoard should be selected.
                        if (FromDiagnosis == true)
                        {
                            frmDiagnosis winDiagnosis = new frmDiagnosis();
                            winDiagnosis.CurrentVisit = CurrentVisit;
                            winDiagnosis.ISaDiagnosisValidation = true;
                            winDiagnosis.IsEnabledControl = CurrentVisit.VisitStatus;
                            EMR_RightCorner.Content = winDiagnosis;
                            if (RoleID == NurseRoleID)
                                winDiagnosis.SelectedUser = "Nurse";

                            if (RoleID == DoctorRoleID)
                                winDiagnosis.SelectedUser = "Doctor";

                            if (RoleID == AdminRoleID)
                                winDiagnosis.SelectedUser = "Admin";
                            winDiagnosis.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
                        }
                        else
                        {
                            (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                        }
                        indicator.Close();
                    }
                };
                client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client1.CloseAsync();
            }
            catch (Exception)
            {
                indicator.Close();
                throw;
            }
        }
        #endregion

        #region MessageBox
        private void ShowMessageBox(string strMessage, MessageBoxControl.MessageBoxButtons button, MessageBoxControl.MessageBoxIcon icon)
        {
            MessageBoxControl.MessageBoxChildWindow msgWindow = new MessageBoxControl.MessageBoxChildWindow("Palash", strMessage, button, icon);
            msgWindow.Show();
        }
        #endregion

        private void cmdPrintSummary_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
            {
                FrmHistorySelection frmHistory = new FrmHistorySelection();
                frmHistory.CurrentVisit = this.CurrentVisit;
                if (SelectedPatient.GenderID == 1)
                {
                    frmHistory.TemplateID = 22;
                }
                else
                {
                    frmHistory.TemplateID = 19;
                }
                frmHistory.Show();

                //HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientClinicalSummarynew.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD="+ CurrentVisit.OPDIPD + "&UnitID=" + CurrentVisit.UnitId), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "EMR details not enter", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
        private void cmdPrintPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRPatientPrescription.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPDIPD=0&UnitID=" + CurrentVisit.UnitId), "_blank");
        }
        //private void cmdReferral_Click(object sender, RoutedEventArgs e)
        //{
        //    clsGetReferralDetailsBizActionVO BizAction = new clsGetReferralDetailsBizActionVO();
        //    if (CurrentVisit != null)
        //    {
        //        BizAction.VisitID = CurrentVisit.ID;
        //        BizAction.PatientID = CurrentVisit.PatientId;
        //    }

        //    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc"); // this url will work both in dev and after deploy 
        //    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
        //    client.ProcessCompleted += (a, arg) =>
        //    {
        //        if (arg.Error == null && arg.Result != null && ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail != null)
        //        {
        //            if (((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail.Count == 1)
        //            {
        //                clsDoctorSuggestedServiceDetailVO obj = ((clsGetReferralDetailsBizActionVO)arg.Result).DoctorSuggestedServiceDetail[0];
        //                if (obj != null && obj.ID > 0)
        //                {
        //                    HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/AllReferralLetterPrint.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + ((IApplicationConfiguration)App.Current).SelectedPatient.PatientID + "&PrescriptionID=" + obj.PrescriptionID + "&ID=" + obj.ID + "&UnitID="+CurrentVisit.UnitId), "_blank");
        //                }
        //            }
        //            else
        //            {
        //                PrintReferralOrder winSummary = new PrintReferralOrder();
        //                winSummary.Title = ((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ttlVisitList");
        //                winSummary.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
        //                winSummary.IsEnabledControl = CurrentVisit.VisitStatus;
        //                winSummary.CurrentVisit = this.CurrentVisit;
        //                winSummary.Show();
        //            }
        //        }
        //        else
        //        {
        //            string strMessage = string.Empty;
        //           // strMessage = PalashDynamic.Localization.LocalizationManager.resourceManager.GetString("msgNoReferralFound");
        //            MessageBoxControl.MessageBoxChildWindow msgW1 =
        //               new MessageBoxControl.MessageBoxChildWindow("Palash", "NO Refrral Found", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
        //            msgW1.Show();
        //            return;

        //        }
        //    };
        //    client.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
        //    client.CloseAsync();
        //}
        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void cmdPatientList_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            PART_MaximizeToggle.IsChecked = false;
            PART_MaximizeToggle.Visibility = Visibility.Visible;

            HyperlinkButton DesignBar = (HyperlinkButton)rootPage.FindName("DesignBar");
            DesignBar.Visibility = Visibility.Visible;

            ToggleButton CmdGraphicalAnalysis = (ToggleButton)rootPage.FindName("CmdGraphicalAnalysis");
            CmdGraphicalAnalysis.Visibility = Visibility.Visible;

            ToggleButton CmdcryoBank = (ToggleButton)rootPage.FindName("CmdcryoBank");
            CmdcryoBank.Visibility = Visibility.Visible;

            ToggleButton CmdGotoHome = (ToggleButton)rootPage.FindName("CmdGotoHome");
            CmdGotoHome.Visibility = Visibility.Visible;

            ToggleButton cmdChangePassword = (ToggleButton)rootPage.FindName("cmdChangePassword");
            cmdChangePassword.Visibility = Visibility.Visible;

            var IvfDashboardRights = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.RoleDetails.MenuList.Where(z => z.ParentId == 0 && z.Title == "IVFDashboard").SingleOrDefault();
            if (IvfDashboardRights != null)
            {
                ToggleButton cmdIVFDashBoard = (ToggleButton)rootPage.FindName("cmdIVFDashBoard");
                cmdIVFDashBoard.Visibility = Visibility.Visible;
            }

            ToggleButton AddSOPFile = (ToggleButton)rootPage.FindName("AddSOPFile");
            AddSOPFile.Visibility = Visibility.Visible;

            if (!SelectedPatient.IsFromAppointment)
            {
                ModuleName = "OPDModule";
                Action = "CIMS.Forms.QueueManagement";
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
            else
            {
                ModuleName = "OPDModule";
                Action = "CIMS.Forms.AppointmentList";
                UserControl rootPage1 = Application.Current.RootVisual as UserControl;
                WebClient c = new WebClient();
                c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
                c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
            }
        }
        void c_OpenReadCompleted_maleSemenSurvival(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;
                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                if (!SelectedPatient.IsFromAppointment)
                {
                    Header.Text = "Patient Queue List";
                }
                else
                {
                    Header.Text = "Appointment List";
                }
                ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                PART_MaximizeToggle.IsChecked = false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientRadiologydetails.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitId=" + CurrentVisit.UnitId), "_blank");
        }
        private void cmdPathology_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientPathologydetails.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitId=" + CurrentVisit.UnitId), "_blank");
        }
        private void cmdDiagnostics_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientDiagnosticsdetails.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId), "_blank");
        }
        private void cmdVisitList_Click(object sender, RoutedEventArgs e)
        {
            PatientVisitSummary winSummary = new PatientVisitSummary();
            winSummary.Title = "Visit List";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("ttlVisitList");
            winSummary.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientID, patientUnitID = this.SelectedPatient.PatientUnitID };
            winSummary.IsEnabledControl = CurrentVisit.VisitStatus;
            winSummary.CurrentVisit = this.CurrentVisit;
            winSummary.Show();
        }

        private void cmdArt_Click(object sender, RoutedEventArgs e)
        {
            ModuleName = "PalashDynamics.IVF";
            Action = "PalashDynamics.IVF.DashBoard.PatientARTAndLabDayDetails";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_IVFOpenReadCompleted);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));
        }

        void c_IVFOpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                UIElement myData = null;
                string appManifest = new StreamReader(Application.GetResourceStream(new StreamResourceInfo(e.Result, null), new Uri("AppManifest.xaml", UriKind.Relative)).Stream).ReadToEnd();
                XElement deploy = XDocument.Parse(appManifest).Root;
                List<XElement> parts = (from assemblyParts in deploy.Elements().Elements()
                                        where (assemblyParts.Attribute("Source").Value == ModuleName + ".dll")
                                        select assemblyParts).ToList();
                Assembly asm = null;
                AssemblyPart asmPart = new AssemblyPart();
                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(parts[0].Attribute("Source").Value, UriKind.Relative));
                asm = asmPart.Load(streamInfo.Stream);
                myData = asm.CreateInstance(Action) as UIElement;

                ((IApplicationConfiguration)App.Current).OpenMainContent(myData);
                ((IApplicationConfiguration)App.Current).SelectedPatient = SelectedPatient;
                ((IInitiateIVFDashBoard)myData).InitiateDashBoard(PlanTherapyId, PlanTherapyUnitId, PatientID, PatientUnitID, GridIndex);
                //TextBlock Header = (TextBlock)rootPage.FindName("SampleHeader");
                //Header.Text = "Patient Queue List";
                //ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
                //PART_MaximizeToggle.IsChecked = false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}

