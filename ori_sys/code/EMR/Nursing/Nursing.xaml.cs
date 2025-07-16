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
//using EMR.IPD_EMR;
using System.Windows.Media.Imaging;
using PalashDynamics.Collections;
using PalashDynamics.Animations;
using PalashDynamics.ValueObjects.IPD;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Reflection;
using PalashDynamics.IPD;

namespace EMR.Nursing
{
    public partial class Nursing : UserControl
    {
        public int IsClicked = 0;
        public int ISClickedForNurseRound = 0;
        public DataGrid storedata;

        #region Data Members
        List<clsMenuVO> MenuList = new List<clsMenuVO>();
        SwivelAnimation objAnimation;
        string msgText = string.Empty;
        clsIPDAdmissionVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedIPDPatient;
        //clsPatientGeneralVO SelectedPatient = ((IApplicationConfiguration)App.Current).SelectedIPDPatient;
        long RoleID = ((clsUserRoleVO)((clsUserGeneralDetailVO)((clsUserVO)((IApplicationConfiguration)App.Current).CurrentUser).UserGeneralDetailVO).RoleDetails).ID;
        long AdminRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).AdminRoleID;
        long NurseRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).NurseRoleID;
        long DoctorRoleID = ((clsAppConfigVO)((IApplicationConfiguration)App.Current).ApplicationConfigurations).DoctorRoleID;
        UserControl rootPage = Application.Current.RootVisual as UserControl;
        WaitIndicator indicator;
        #endregion

        #region Properties
        public int DataListPageSize
        {
            get
            {
                return MasterList.PageSize;
            }
            set
            {
                if (value == MasterList.PageSize) return;
                MasterList.PageSize = value;
            }
        }
        public clsVisitVO CurrentVisit { get; set; }
        public PagedSortableCollectionView<clsPatientConsoleHeaderVO> MasterList { get; private set; }
        Boolean IsLoaded { get; set; }
        #endregion

        public Nursing()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(frmEMR_Loaded);
            ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            PART_MaximizeToggle.IsChecked = true;
            indicator = new WaitIndicator();
            MasterList = new PagedSortableCollectionView<clsPatientConsoleHeaderVO>();
            MasterList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);
        }

        #region Events
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
            winPastHist1.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
            EMR_RightCorner.Content = winPastHist1;
        }
        void tvi_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvItem = sender as TreeViewItem;
            if (CurrentVisit == null)
            {
                CurrentVisit = new clsVisitVO();
                CurrentVisit.ID = SelectedPatient.ID; //admission id
                CurrentVisit.PatientId = SelectedPatient.PatientId;
                CurrentVisit.PatientUnitId = SelectedPatient.PatientUnitID;
                CurrentVisit.UnitId = SelectedPatient.UnitId; //VisitUnitId;
                CurrentVisit.OPDNO = SelectedPatient.IPDNO;       //OPDNO;
                CurrentVisit.MRNO = SelectedPatient.MRNo;
            }
            switch (tvItem.Name)
            {
                case "EMR.PatientEMRDashboard":
                  //  frmIPDPatientEMRDashboard winDashborad = new frmIPDPatientEMRDashboard();
                    //frmPatientEMRDashboard winDashborad = new frmPatientEMRDashboard();
                    TemplateListNew winDashborad = new TemplateListNew();
                    winDashborad.CurrentVisit = CurrentVisit;
                    EMR_RightCorner.Content = winDashborad;
                    break;
                case "EMR.frmEMRCompoundMedication":
                    frmIPDEMRCompoundMedication winCompdMedication = new frmIPDEMRCompoundMedication();
                    // frmEMRCompoundMedication winCompdMedication = new frmEMRCompoundMedication();
                    winCompdMedication.CurrentVisit = this.CurrentVisit;
                    winCompdMedication.IsEnableControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winCompdMedication;
                    break;
                case "EMR.Procedure":
                    frmIPDEMRProcedure winProcedure = new frmIPDEMRProcedure();
                    //frmEMRProcedure winProcedure = new frmEMRProcedure();
                    winProcedure.CurrentVisit = CurrentVisit;
                    winProcedure.IsEnabledControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winProcedure;
                    if (RoleID == NurseRoleID)
                        winProcedure.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winProcedure.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winProcedure.SelectedUser = "Admin";
                    winProcedure.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    break;

                case "EMR.PatientEMRDiagnosis":
                    frmIPDDiagnosis winDiagnosis = new frmIPDDiagnosis();
                    //frmDiagnosis winDiagnosis = new frmDiagnosis();
                    winDiagnosis.CurrentVisit = CurrentVisit;
                    winDiagnosis.IsEnabledControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winDiagnosis;
                    if (RoleID == NurseRoleID)
                        winDiagnosis.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winDiagnosis.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winDiagnosis.SelectedUser = "Admin";
                    winDiagnosis.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    break;
                case "EMR.PatientEMRVitals":
                    frmIPDEMRVitals winVitals = new frmIPDEMRVitals();
                    //frmEMRVitals winVitals = new frmEMRVitals();
                    if (RoleID == NurseRoleID)
                        winVitals.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winVitals.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winVitals.SelectedUser = "Admin";
                    winVitals.IsEnabledControl = CurrentVisit.VisitStatus;
                    winVitals.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    winVitals.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("VitalTemplateID"));// 103;
                    winVitals.CurrentVisit = this.CurrentVisit;
                    EMR_RightCorner.Content = winVitals;
                    break;
                case "EMR.frmEMRMaleHistory1":
                    if (SelectedPatient.GenderName.ToUpper() == "MALE")
                    {
                        HistoryMenu(22);
                        // HistoryMenu(243);
                    }
                    else
                    {
                        string sMsgText = "Please Select Female Template";
                        ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    break;
                case "EMR.frmEMRFemaleHistory":
                    if (SelectedPatient.GenderName.ToUpper() == "FEMALE")
                    {
                        HistoryMenu(19);
                    }
                    else
                    {
                        string sMsgText = "Please Select Male Template.";
                        ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                    }
                    break;
                case "EMR.frmEMRCoupleHistory":
                    HistoryMenu(114);
                    break;
                case "EMR.PatientEMRReferral":
                    frmIPDPatientEMRRefferal winReferral = new frmIPDPatientEMRRefferal();
                    //frmPatientEMRRefferal winReferral = new frmPatientEMRRefferal();
                    winReferral.CurrentVisit = CurrentVisit;
                    winReferral.IsEnabledControl = CurrentVisit.VisitStatus;
                    // winReferral.PatientDetail = this.SelectedPatient;
                    EMR_RightCorner.Content = winReferral;
                    break;
                case "EMR.PatientEMRRChiefComplaints":
                    frmIPDEMRChiefComplaint winChiefComplaint = new frmIPDEMRChiefComplaint();
                    //frmEMRChiefComplaint winChiefComplaint = new frmEMRChiefComplaint();
                    winChiefComplaint.IsEnabledControl = CurrentVisit.VisitStatus;
                    winChiefComplaint.CurrentVisit = CurrentVisit;
                    EMR_RightCorner.Content = winChiefComplaint;
                    break;
                case "EMR.PatientEMRAllergies":
                    //frmEMRAllergies winAllergies = new frmEMRAllergies();
                    frmIPDEMRAllergies winAllergies = new frmIPDEMRAllergies();
                    winAllergies.CurrentVisit = this.CurrentVisit;
                    //  winAllergies.IsControlEnable = CurrentVisit.VisitStatus;
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
                    winClinicalHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };

                    EMR_RightCorner.Content = winClinicalHist;
                    break;
                case "EMR.PatientEMRHysteroscopy":

                    frmIPDEMRCommonHistory winPastHist1 = new frmIPDEMRCommonHistory();
                    winPastHist1.CurrentVisit = this.CurrentVisit;
                    winPastHist1.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winPastHist1.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winPastHist1.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winPastHist1.SelectedUser = "Admin";

                    winPastHist1.TemplateID = 23;
                    winPastHist1.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winPastHist1;

                    break;
                case "EMR.PatientEMRLaparoscopy":
                    frmIPDEMRCommonHistory winPastHistLap = new frmIPDEMRCommonHistory();
                    winPastHistLap.CurrentVisit = this.CurrentVisit;
                    winPastHistLap.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winPastHistLap.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winPastHistLap.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winPastHistLap.SelectedUser = "Admin";

                    winPastHistLap.TemplateID = 24;
                    winPastHistLap.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winPastHistLap;
                    break;
                case "EMR.frmEMRPastHistory":
                    frmIPDEMRPastHistory winPastHist = new frmIPDEMRPastHistory();
                    winPastHist.CurrentVisit = this.CurrentVisit;
                    winPastHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winPastHist.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winPastHist.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winPastHist.SelectedUser = "Admin";
                    winPastHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("PastHistoryTemplateID"));// 105; // Past History Template ID
                    winPastHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winPastHist;
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
                    frmIPDEMRPersonalHistory winPesrHist = new frmIPDEMRPersonalHistory();
                    winPesrHist.CurrentVisit = this.CurrentVisit;
                    winPesrHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winPesrHist.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winPesrHist.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winPesrHist.SelectedUser = "Admin";
                    winPesrHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("PersonalHistoryTemplateID"));// 105; // Personal History Template ID
                    winPesrHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winPesrHist;
                    break;
                case "EMR.frmEMRFamilyHistory":
                    frmIPDEMRFamilyHistory winFamilyHist = new frmIPDEMRFamilyHistory();
                    winFamilyHist.CurrentVisit = this.CurrentVisit;
                    winFamilyHist.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winFamilyHist.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winFamilyHist.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winFamilyHist.SelectedUser = "Admin";
                    winFamilyHist.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("FamilyHistoryTemplateID"));// 105; // Family History Template ID
                    winFamilyHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winFamilyHist;
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
                    winERPHist.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winERPHist;
                    break;
                case "EMR.frmEMRInvestigation":
                    frmIPDEMRInvestigation winInvestigation = new frmIPDEMRInvestigation();
                    winInvestigation.CurrentVisit = this.CurrentVisit;
                    winInvestigation.IsEnableControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winInvestigation;
                    break;
                case "EMR.frmEMRMEdication":
                    frmIPDEMRMedication winMedication = new frmIPDEMRMedication();
                    //frmEMRMedication winMedication = new frmEMRMedication();
                    winMedication.CurrentVisit = this.CurrentVisit;
                    winMedication.IsEnableControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winMedication;
                    break;
                case "EMR.frmEMROtherServices":
                    frmEMROtherServices winOtherServices = new frmEMROtherServices();
                    winOtherServices.CurrentVisit = this.CurrentVisit;
                    winOtherServices.IsEnabledControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winOtherServices;
                    break;
                case "EMR.frmEMRFollowup":
                    frmIPDEMRFollowup winFollowUp = new frmIPDEMRFollowup();
                    //frmEMRFollowup winFollowUp = new frmEMRFollowup();
                    winFollowUp.CurrentVisit = this.CurrentVisit;
                    winFollowUp.IsEnableControl = CurrentVisit.VisitStatus;
                    this.EMR_RightCorner.Content = winFollowUp;
                    break;
                case "EMR.PatientEMRPhysicalExam":
                    frmIPDEMRPhysicalExam winPhysicalExam = new frmIPDEMRPhysicalExam();
                    //frmEMRPhysicalExam winPhysicalExam = new frmEMRPhysicalExam();
                    winPhysicalExam.CurrentVisit = this.CurrentVisit;
                    winPhysicalExam.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        winPhysicalExam.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        winPhysicalExam.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        winPhysicalExam.SelectedUser = "Admin";
                    winPhysicalExam.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = winPhysicalExam;
                    break;
                case "EMR.frmEMRSurgicalHistory":
                    frmIPDEMRSurgicalHistory WinSurgicalHistory = new frmIPDEMRSurgicalHistory();
                    WinSurgicalHistory.CurrentVisit = this.CurrentVisit;
                    WinSurgicalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        WinSurgicalHistory.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        WinSurgicalHistory.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        WinSurgicalHistory.SelectedUser = "Admin";
                    WinSurgicalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("SurgicalHistoryTemplateID"));// 105; // Family History Template ID
                    WinSurgicalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = WinSurgicalHistory;
                    break;
                case "EMR.frmEMRTreatmentHistory":
                    frmIPDEMRTreatmentHistory WinTreatmentHistory = new frmIPDEMRTreatmentHistory();
                    WinTreatmentHistory.CurrentVisit = this.CurrentVisit;
                    WinTreatmentHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        WinTreatmentHistory.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        WinTreatmentHistory.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        WinTreatmentHistory.SelectedUser = "Admin";
                    WinTreatmentHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("TreatmentHistoryTemplateID"));// 105; // Family History Template ID
                    WinTreatmentHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = WinTreatmentHistory;
                    break;
                case "EMR.frmEMRObstetricalHistory":
                    frmIPDEMRObstetricalHistory WinObstetricalHistory = new frmIPDEMRObstetricalHistory();
                    WinObstetricalHistory.CurrentVisit = this.CurrentVisit;
                    WinObstetricalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        WinObstetricalHistory.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        WinObstetricalHistory.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        WinObstetricalHistory.SelectedUser = "Admin";
                    WinObstetricalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("ObstetricalHistoryTemplateID"));// 105; // Family History Template ID
                    WinObstetricalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = WinObstetricalHistory;
                    break;
                case "EMR.frmEMRMaritalHistory":
                    frmIPDEMRMaritalHistory WinMaritalHistory = new frmIPDEMRMaritalHistory();
                    WinMaritalHistory.CurrentVisit = this.CurrentVisit;
                    WinMaritalHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        WinMaritalHistory.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        WinMaritalHistory.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        WinMaritalHistory.SelectedUser = "Admin";
                    WinMaritalHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("MaritalHistoryTemplateID"));// 105; // Family History Template ID
                    WinMaritalHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = WinMaritalHistory;
                    break;
                case "EMR.frmEMRMenstrualHistory":
                    frmIPDEMRMenstrualHistory WinMenstrualHistory = new frmIPDEMRMenstrualHistory();
                    WinMenstrualHistory.CurrentVisit = this.CurrentVisit;
                    WinMenstrualHistory.IsEnabledControl = this.CurrentVisit.VisitStatus;
                    if (RoleID == NurseRoleID)
                        WinMenstrualHistory.SelectedUser = "Nurse";

                    if (RoleID == DoctorRoleID)
                        WinMenstrualHistory.SelectedUser = "Doctor";

                    if (RoleID == AdminRoleID)
                        WinMenstrualHistory.SelectedUser = "Admin";
                    WinMenstrualHistory.TemplateID = Convert.ToInt64(DefaultValues.ResourceManager.GetString("MenstrualHistoryTemplateID"));// 105; // Family History Template ID
                    WinMenstrualHistory.SelectedPatient = new Patient() { PatientId = this.SelectedPatient.PatientId, patientUnitID = this.SelectedPatient.PatientUnitID };
                    EMR_RightCorner.Content = WinMenstrualHistory;
                    break;
                case "EMR.frmEMRCurrentMedication":
                    frmIPDCurrentMedication winCurrentMedication = new frmIPDCurrentMedication();
                    //frmCurrentMedication winCurrentMedication = new frmCurrentMedication();
                    winCurrentMedication.CurrentVisit = this.CurrentVisit;
                    winCurrentMedication.IsEnabledControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winCurrentMedication;
                    break;
                case "EMR.frmEMRInvestigationDocs":
                    frmIPDEMRInvestigationDocs winInvest = new frmIPDEMRInvestigationDocs();
                    // frmEMRInvestigationDocs winInvest = new frmEMRInvestigationDocs();
                    EMR_RightCorner.Content = winInvest;
                    break;
                case "EMR.frmEMREducation":
                    frmIPDEMREducation winEducation = new frmIPDEMREducation();
                    //frmEMREducation winEducation = new frmEMREducation();
                    winEducation.CurrentVisit = CurrentVisit;
                    winEducation.IsEnabledControl = CurrentVisit.VisitStatus;
                    EMR_RightCorner.Content = winEducation;
                    break;
                case "EMR.frmGrowthChart":
                    // frmViewGrowthChart winGC;
                    frmIPDViewGrowthChart winGC;
                    string PatientName = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientName;
                    string mrNo = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.MRNo;
                    long longDrID = 0;
                    longDrID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    int genderid = 0;
                    if (((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName.ToUpper() == "FEMALE")
                        genderid = 2;
                    else
                        genderid = 1;
                    DateTime Date2 = DateTime.Now;
                    DateTime Date1 = Convert.ToDateTime(((IApplicationConfiguration)App.Current).SelectedIPDPatient.DateOfBirth);
                    int maxAgeInMonth = (Date2.Year - Date1.Year) * 12 + Date2.Month - Date1.Month;
                    if (maxAgeInMonth > 24 && maxAgeInMonth < 241)
                    {
                        winGC = new frmIPDViewGrowthChart(false, ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId, longDrID, PatientName, mrNo, genderid, ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId, CurrentVisit.OPDIPD);
                        EMR_RightCorner.Content = winGC;
                    }
                    else if (maxAgeInMonth <= 24)
                    {
                        winGC = new frmIPDViewGrowthChart(true, ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId, longDrID, PatientName, mrNo, genderid, ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId, CurrentVisit.OPDIPD);
                        EMR_RightCorner.Content = winGC;
                    }
                    else
                    {
                        string sMsgText = "Only for 0 - 20 years of age.";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("GCAgeLimit_Msg");
                        ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                        frmIPDPatientEMRDashboard winDashboard = new frmIPDPatientEMRDashboard();
                        //frmPatientEMRDashboard winDashboard = new frmPatientEMRDashboard();
                        winDashboard.CurrentVisit = CurrentVisit;
                        EMR_RightCorner.Content = winDashboard;
                    }
                    break;
            }
        }
        public string ModuleName { get; set; }
        public string Action { get; set; }
        private void cmdPatientList_Click(object sender, RoutedEventArgs e)
        {
            ModuleName = "PalashDynamics.IPD";
            Action = "PalashDynamics.IPD.Forms.frmAdmissionList";
            UserControl rootPage = Application.Current.RootVisual as UserControl;
            WebClient c = new WebClient();
            c.OpenReadCompleted += new OpenReadCompletedEventHandler(c_OpenReadCompleted_maleSemenSurvival);
            c.OpenReadAsync(new Uri(ModuleName + ".xap", UriKind.Relative));

            //frmAdmissionList ucQueue = new frmAdmissionList();
            //this.Content = null;
            //this.Content = ucQueue;
            //ToggleButton PART_MaximizeToggle = (ToggleButton)rootPage.FindName("PART_MaximizeToggle");
            //PART_MaximizeToggle.IsChecked = false;
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
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            //GetEMRAdmVisitByPatientID();
        }
        private void cmdAddRound_Click(object sender, RoutedEventArgs e)
        {
            //code for select ipd patient when click on ipd round
            //IList<clsPatientConsoleHeaderVO> test = (IList<clsPatientConsoleHeaderVO>)this.storedata.ItemsSource;
            //var aa = from r in test where (r.IPDOPD == "IPD") select new { r };
            try
            {
                if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                {
                    clsIPDRoundDetailsBizactionVO bizAction = new clsIPDRoundDetailsBizactionVO();
                    bizAction.SpecName = CurrentVisit.DoctorSpecialization;
                    bizAction.SpecCode = CurrentVisit.DocSpecCode;
                    bizAction.DoctorName = CurrentVisit.Doctor;
                    bizAction.DoctorId = CurrentVisit.DoctorID;
                    bizAction.AdmisstionId = this.CurrentVisit.ID;
                    Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                    PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                    client.ProcessCompleted += (s, args) =>
                    {
                        if (args.Error == null && args.Result != null)
                        {
                            ShowMessageBox("New Round Add successfully", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                            CurrentVisit.RoundId = ((clsIPDRoundDetailsBizactionVO)args.Result).ID;
                            (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                            accItem.IsSelected = true;
                        }
                    };
                    client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                    client.CloseAsync();
                }
                else
                {
                    frmSelectionDoctorSpecliztion Win = new frmSelectionDoctorSpecliztion();
                    Win.OnAddButton_Click += new RoutedEventHandler(WinDocSpec_OnAddButton_Click);
                    Win.CurrentVisit = this.CurrentVisit;
                    Win.IsWindowCloseForRound = false;
                    Win.Show();
                }
            }
            catch
            {
            }
        }
        private void WinDocSpec_OnAddButton_Click(object sender, EventArgs e)
        {
            clsIPDRoundDetailsBizactionVO objRound = sender as clsIPDRoundDetailsBizactionVO;
            this.CurrentVisit.Doctor = objRound.DoctorName;
            this.CurrentVisit.DoctorCode = objRound.DoctorCode;
            this.CurrentVisit.DoctorID = objRound.DoctorId;
            this.CurrentVisit.DoctorSpecialization = objRound.SpecName;
            this.CurrentVisit.DocSpecCode = objRound.SpecCode;
            this.CurrentVisit.PatientId = objRound.PatientID;
            this.CurrentVisit.ID = objRound.AdmisstionId;
            this.CurrentVisit.RoundId = objRound.ID;
            this.CurrentVisit.OPDIPD = true;
            if (tvPatientEMR.Items.Count > 0)
            {
                // By default DashBoard should be selected.
                (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
            }
            else
            {
                GetEMRMenuTree();
            }
        }
        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            //objAnimation.Invoke(RotationType.Backward);
        }
        private void Encounter_Selectionchanged(object sender, RoutedEventArgs e)
        {
            DataGrid dgEncounterList = sender as DataGrid;
            storedata = dgEncounterList;
            if (dgEncounterList.SelectedIndex != -1)
            {
                clsPatientConsoleHeaderVO objEncounter = ((clsPatientConsoleHeaderVO)dgEncounterList.SelectedItem);
                SelectedPatient.ID = objEncounter.OPD_IPD_ID;
                SelectedPatient.IPDNO = objEncounter.IPDOPDNO;
                CurrentVisit.OPDIPD = objEncounter.OPD_IPD;
                CurrentVisit.ID = objEncounter.OPD_IPD_ID;
                CurrentVisit.DepartmentCode = objEncounter.DepartmentCode;
                CurrentVisit.Department = objEncounter.DepartmentName;
                CurrentVisit.OPDNO = objEncounter.IPDOPDNO;
                CurrentVisit.ISIPDDischarge = objEncounter.ISDischarged;
                CurrentVisit.EncounterListDoctorCode = objEncounter.DoctorCode;

                if (!CurrentVisit.OPDIPD)
                {
                    cmdSave.IsEnabled = false;
                }
                else
                {
                    if (CurrentVisit.ISIPDDischarge == true)
                    {
                        cmdSave.IsEnabled = false;
                    }
                    else
                    {
                        cmdSave.IsEnabled = true;
                    }
                }
                //as this is IPD admission we use the visit Type id as 2.
                CurrentVisit.VisitTypeID = 2;
                //if(dgEncounterList.SelectedIndex == 0 && (CurrentVisit.RoundId == 0 || ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor))
                //if (dgEncounterList.SelectedIndex == 0 && ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor && IsClicked == 0)
                //{
                //    try
                //    {
                //        if (((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                //        {
                //            IsClicked = 1;
                //            frmDoctorRoundSelection winDocRound = new frmDoctorRoundSelection();
                //            winDocRound.OnAddButton_Click += new RoutedEventHandler(winDocRound_OnAddButton_Click);
                //            winDocRound.CurrentVisit = this.CurrentVisit;
                //            winDocRound.Show();
                //        }
                //    }
                //    catch (Exception pp)
                //    {
                //    }
                //}
                //else if (ISClickedForNurseRound == 0 && !((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.IsDoctor)
                //{
                //    ISClickedForNurseRound = 1;
                //    frmSelectionDoctorSpecliztion winDocSpec = new frmSelectionDoctorSpecliztion();
                //    winDocSpec.OnAddButton_Click += new RoutedEventHandler(WinDocSpec_OnAddButton_Click);
                //    winDocSpec.CurrentVisit = this.CurrentVisit;
                //    winDocSpec.IsWindowCloseForRound = true;
                //    winDocSpec.Show();
                //}
                clsMenuVO mnuSelected = new clsMenuVO();
                if (tvPatientEMR.SelectedItem != null)
                {
                    mnuSelected = (tvPatientEMR.SelectedItem as TreeViewItem).DataContext as clsMenuVO;
                    (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = false;
                    (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                    //accItem.IsSelected = true; //1
                }
            }
        }
        #endregion

        private void winDocRound_OnAddButton_Click(object sender, EventArgs e)
        {
            clsIPDRoundDoctorBizactionVO BizAction = new clsIPDRoundDoctorBizactionVO();
            BizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
            Uri address1 = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
            PalashServiceClient client1 = new PalashServiceClient("BasicHttpBinding_IPalashService", address1.AbsoluteUri);
            client1.ProcessCompleted += (s, args) =>
            {
                if (args.Error == null && args.Result != null)
                {
                    if (tvPatientEMR.Items.Count > 0)
                    {
                        // By default DashBoard should be selected.
                        (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                    }
                    else
                    {
                        GetEMRMenuTree();
                    }
                    CurrentVisit.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                    CurrentVisit.DoctorSpecialization = ((clsIPDRoundDoctorBizactionVO)args.Result).SpecName;
                    CurrentVisit.DocSpecCode = ((clsIPDRoundDoctorBizactionVO)args.Result).SpecCode;
                    CurrentVisit.Doctor = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorName;
                }
            };
            client1.ProcessAsync(BizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
            client1.CloseAsync();

            if ((bool)sender)
            {
                clsIPDRoundDetailsBizactionVO bizAction = new clsIPDRoundDetailsBizactionVO();
                bizAction.SpecName = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecialization;
                bizAction.SpecCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorSpecCode;
                bizAction.DoctorId = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                //bizAction.DoctorName = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorName;
                CurrentVisit.Doctor = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.DoctorName;
                bizAction.DoctorCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
                bizAction.AdmisstionId = this.CurrentVisit.ID;
                Uri address = new Uri(Application.Current.Host.Source, "../PalashTestService.svc");
                PalashServiceClient client = new PalashServiceClient("BasicHttpBinding_IPalashService", address.AbsoluteUri);
                client.ProcessCompleted += (s, args) =>
                {
                    if (args.Error == null && args.Result != null)
                    {
                        //CurrentVisit.DoctorCode = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorCode;
                        CurrentVisit.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
                        CurrentVisit.RoundId = ((clsIPDRoundDetailsBizactionVO)args.Result).ID;
                        (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                        //  accItem.IsSelected = true; //1
                    }
                };
                client.ProcessAsync(bizAction, ((IApplicationConfiguration)App.Current).CurrentUser);
                client.CloseAsync();
            }
            else
            {
                CurrentVisit.DoctorID = ((IApplicationConfiguration)App.Current).CurrentUser.UserGeneralDetailVO.DoctorID;
            }
        }

        #region Loaded
        private void frmEMR_Loaded(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded)
            {
                if (SelectedPatient != null && SelectedPatient.MRNo.Length > 1)
                {
                    CurrentVisit = new clsVisitVO();
                    CurrentVisit.PatientId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientId;
                    CurrentVisit.ID = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.ID;
                    CurrentVisit.OPDIPD = true;
                    CurrentVisit.PatientUnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.PatientUnitID;
                    CurrentVisit.UnitId = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.UnitId;
                    accItem.IsSelected = true; //1
                    if (tvPatientEMR != null && tvPatientEMR.Items.Count <= 0)
                    {
                        accItemEMRMenus.IsSelected = false;
                        PalashDynamics.SearchResultLists.DisplayIPDPatientDetails winDisplay = new PalashDynamics.SearchResultLists.DisplayIPDPatientDetails();
                        
                        //winDisplay.EncounterList_Selectionchanged += new RoutedEventHandler(Encounter_Selectionchanged);
                        Grid dgEncounterList = winDisplay.FindName("EncounterGrid") as Grid;
                        dgEncounterList.Visibility = Visibility.Collapsed;
                        ResultListContent.Content = winDisplay;

                        TemplateListNew winDashborad = new TemplateListNew();
                        winDashborad.CurrentVisit = CurrentVisit;
                        EMR_RightCorner.Content = winDashborad;
                        //GetEMRMenuTree();
                        accItem.IsSelected = true;



                    }
                }
                else
                {
                    string sMsgText = "Please Select the Patient";//((IApplicationConfiguration)App.Current).LocalizedManager.GetValue("SelectPatient_Msg");
                    ShowMessageBox(sMsgText, MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Error);
                }
                IsLoaded = true;
            }
        }
        #endregion

        #region Private MEthods

        private void BuildMenu(List<clsMenuVO> Items, TreeViewItem rootItem, long ID, string parent)
        {
            // find immediate child of parent elemt
            List<clsMenuVO> ChildList; //;= Items.Where(z => z.ParentId == ID).OrderBy(or => or.MenuOrder).ToList();
            string gender = ((IApplicationConfiguration)App.Current).SelectedIPDPatient.GenderName;
            // find immediate child of parent elemt
            if (gender == "Male")
            {
                //if (parent == "History")
                //{
                ChildList = Items.Where(z => z.ParentId == ID && z.ID != 529 && z.ID != 530 && z.ID != 531 && z.ID != 533 && z.ID != 534 && z.ID != 535 && z.ID != 536 && z.ID != 537 && z.ID != 538 && z.ID != 539 && z.ID != 540 && z.ID != 541 && z.ID != 542 && z.ID != 543 && z.ID != 544).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = Items.Where(z => z.ParentId == ID && z.ID != 505 && z.ID != 506 && z.ID != 507 && z.ID != 508 && z.ID != 534 && z.ID != 535 && z.ID != 536 && z.ID != 537 && z.ID != 538 && z.ID != 588 && z.ID != 589 && z.ID != 613 && z.ID != 614 && z.ID != 615 && z.ID != 616 && z.ID != 617).OrderBy(or => or.MenuOrder).ToList();
                //    ChildList = Items.Where(z => z.ParentId == ID && z.ISFemale != true).OrderBy(or => or.MenuOrder).ToList();
                //}
                //else
                //{
                //ChildList = Items.Where(z => z.ParentId == ID).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = ChildList.Where(z => z.ISFemale == false && z.Parent == "History").OrderBy(or => or.MenuOrder).ToList();
                //}
            }
            else
            {
                //if (parent == "History")
                //{
                ChildList = Items.Where(z => z.ParentId == ID && z.ID != 545 && z.ID != 546 && z.ID != 547 && z.ID != 548 && z.ID != 549).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = Items.Where(z => z.ParentId == ID && z.ID != 618 && z.ID != 619 && z.ID != 620 && z.ID != 621 && z.ID != 622).OrderBy(or => or.MenuOrder).ToList();
                //    ChildList = Items.Where(z => z.ParentId == ID && z.ISFemale == true).OrderBy(or => or.MenuOrder).ToList();
                //}
                //else
                //{
                //ChildList = Items.Where(z => z.ParentId == ID).OrderBy(or => or.MenuOrder).ToList();
                //ChildList = ChildList.Where(z => z.ISFemale == true && z.Parent == "History").OrderBy(or => or.MenuOrder).ToList();
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
                        List<clsMenuVO> lst = MenuList.Where(z => z.Parent == "Patient IPD EMR").ToList();
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
                        if (tvPatientEMR.Items.Count > 0)
                        {
                            (tvPatientEMR.Items[0] as TreeViewItem).IsSelected = true;
                            accItem.IsSelected = true;
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

        private void cmdPrintPrescription_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/EMRIPDPatientPrescription.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPDIPD=1&UnitID=" + CurrentVisit.UnitId + "&DoctorId=" + CurrentVisit.DoctorID), "_blank");
        }

        private void cmdPathology_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/IPDPatientPathologydetails.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=1&UnitId=" + CurrentVisit.UnitId + "&DoctorId=" + CurrentVisit.DoctorID), "_blank");
        }

        private void cmdRadiology_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/IPDPatientRadiologydetails.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=1&UnitId=" + CurrentVisit.UnitId + "&DoctorId=" + CurrentVisit.DoctorID), "_blank");
        }

        private void cmdReferral_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdPrintSummary_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentVisit != null && CurrentVisit.ID > 0)
            {
                HtmlPage.Window.Navigate(new Uri(Application.Current.Host.Source, "../Reports/EMR/PatientIPDClinicalSummary.aspx?VisitID=" + CurrentVisit.ID + "&PatientID=" + CurrentVisit.PatientId + "&IsOPdIPD=" + CurrentVisit.OPDIPD + "&UnitID=" + CurrentVisit.UnitId), "_blank");
            }
            else
            {
                MessageBoxControl.MessageBoxChildWindow mgbx = new MessageBoxControl.MessageBoxChildWindow("Palash", "EMR details not enter", MessageBoxControl.MessageBoxButtons.Ok, MessageBoxControl.MessageBoxIcon.Information);
                mgbx.Show();
            }
        }
    }
}
